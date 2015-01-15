using namespace LatchSDK;
using namespace LatchFirewallLibrary;
using namespace System::IO;
using namespace System::Collections::Generic;
using namespace std;

#include <winsock2.h>
#include <windows.h>
#include <tchar.h>
#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <vector>

#include "windivert.h"
#include <msclr\marshal_cppstd.h>

#define MAXBUF  0xFFFF
#define CONFIG_FILENAME _T("Config.txt")
#define LATCH_CONFIG_FILENAME _T("LatchConfig.txt")

static DWORD passthru(LPVOID arg);
static DWORD Filter(string arg[2]);

static string AppId;
static string Secret;
static string AccId;
static int Timeout;


/*
* Pre-fabricated packets.
*/
typedef struct
{
	WINDIVERT_IPHDR ip;
	WINDIVERT_TCPHDR tcp;
} TCPPACKET, *PTCPPACKET;

typedef struct
{
	WINDIVERT_IPV6HDR ipv6;
	WINDIVERT_TCPHDR tcp;
} TCPV6PACKET, *PTCPV6PACKET;

typedef struct
{
	WINDIVERT_IPHDR ip;
	WINDIVERT_ICMPHDR icmp;
	UINT8 data[];
} ICMPPACKET, *PICMPPACKET;

typedef struct
{
	WINDIVERT_IPV6HDR ipv6;
	WINDIVERT_ICMPV6HDR icmpv6;
	UINT8 data[];
} ICMPV6PACKET, *PICMPV6PACKET;

/*
* Prototypes.
*/
static void PacketIpInit(PWINDIVERT_IPHDR packet);
static void PacketIpTcpInit(PTCPPACKET packet);
static void PacketIpIcmpInit(PICMPPACKET packet);
static void PacketIpv6Init(PWINDIVERT_IPV6HDR packet);
static void PacketIpv6TcpInit(PTCPV6PACKET packet);
static void PacketIpv6Icmpv6Init(PICMPV6PACKET packet);
bool InitializeLatchConfig();

/*
* Entry.
*/
int __cdecl main(int argc, char **argv)
{
	HANDLE handle, thread;

	System::String^ path = Path::Combine(System::Environment::CurrentDirectory, CONFIG_FILENAME);
	System::Console::WriteLine(path);

//#if (_DEBUG)
//	while (!System::Diagnostics::Debugger::IsAttached) Sleep(1000);
//#endif

	Configuration^ conf = Configuration::GetConfig();

	for each (FirewallRule^ rule in conf->Rules)
	{
		std::string sLine = msclr::interop::marshal_as<std::string>(rule->FilterExpression);

		string* args = new string[2];	//just to match LPVOID
		args[0] = sLine;

		thread = CreateThread(NULL, 1, (LPTHREAD_START_ROUTINE)Filter, args, 0, NULL);
		if (thread == NULL)
		{
			UserLog::LogMessage(System::String::Format("Error creating thread for: {0} - {1}",
				rule->FilterExpression, GetLastError()));
		}
		else
		{
			UserLog::LogMessage(System::String::Format("Created thread for: {0}",
				rule->FilterExpression));
		}
	}

	while (true)
	{
		Sleep(10000);
	}

	return 0;
}

static DWORD Filter(string args[])
{
	unsigned char packet[MAXBUF];
	UINT packet_len;
	WINDIVERT_ADDRESS addr;
	
	WINDIVERT_ADDRESS recv_addr, send_addr;
	PWINDIVERT_IPHDR ip_header;
	PWINDIVERT_IPV6HDR ipv6_header;
	PWINDIVERT_ICMPHDR icmp_header;
	PWINDIVERT_ICMPV6HDR icmpv6_header;
	PWINDIVERT_TCPHDR tcp_header;
	PWINDIVERT_UDPHDR udp_header;
	UINT payload_len;

	TCPPACKET reset0;
	PTCPPACKET reset = &reset0;
	UINT8 dnr0[sizeof(ICMPPACKET) + 0x0F * sizeof(UINT32) + 8 + 1];
	PICMPPACKET dnr = (PICMPPACKET)dnr0;

	TCPV6PACKET resetv6_0;
	PTCPV6PACKET resetv6 = &resetv6_0;
	UINT8 dnrv6_0[sizeof(ICMPV6PACKET) + sizeof(WINDIVERT_IPV6HDR) +
		sizeof(WINDIVERT_TCPHDR)];
	PICMPV6PACKET dnrv6 = (PICMPV6PACKET)dnrv6_0;


	// Initialize all packets.
	PacketIpTcpInit(reset);
	reset->tcp.Rst = 1;
	reset->tcp.Ack = 1;
	PacketIpIcmpInit(dnr);
	dnr->icmp.Type = 3;         // Destination not reachable.
	dnr->icmp.Code = 3;         // Port not reachable.
	PacketIpv6TcpInit(resetv6);
	resetv6->tcp.Rst = 1;
	resetv6->tcp.Ack = 1;
	PacketIpv6Icmpv6Init(dnrv6);
	dnrv6->ipv6.Length = htons(sizeof(WINDIVERT_ICMPV6HDR) + 4 +
		sizeof(WINDIVERT_IPV6HDR) + sizeof(WINDIVERT_TCPHDR));
	dnrv6->icmpv6.Type = 1;     // Destination not reachable.
	dnrv6->icmpv6.Code = 4;     // Port not reachable.


	System::String^ argString = gcnew System::String((args[0]).c_str());

	System::String^ filterExp;
	System::String^ opId;

	System::String^ delimStr = "opId=";
	cli::array<System::String^, 1>^ arr = { delimStr };

	array<System::String^>^ strarray = argString->Split(arr, System::StringSplitOptions::RemoveEmptyEntries);

	if (strarray->Length > 0) filterExp = strarray[0]->Trim();
	if (strarray->Length > 1) opId = strarray[1]->Trim();

	if (opId->Contains("#")) opId = opId->Split('#')[0]->Trim();

	HANDLE handle = INVALID_HANDLE_VALUE;
	if (!System::String::IsNullOrEmpty(filterExp))
	{
		handle = WinDivertOpen(msclr::interop::marshal_as<std::string>(filterExp).c_str(), WINDIVERT_LAYER_NETWORK, 0, 0);
	}

	if (handle == INVALID_HANDLE_VALUE)
	{
		DWORD lastError = GetLastError();
		if (lastError == ERROR_INVALID_PARAMETER)
		{
			UserLog::LogMessage(System::String::Format("Error: filter syntax error {0}", filterExp));
		}
		else
		{
			UserLog::LogMessage(System::String::Format("Error: failed to open WinDivert handle: {0} - {1}", lastError, filterExp));
		}
	}

	else
	{
		while (TRUE)
		{
			// Read a matching packet.
			if (!WinDivertRecv(handle, packet, sizeof(packet), &addr, &packet_len))
			{
				UserLog::LogMessage(System::String::Format("Warning: failed to read packet: {0}", GetLastError()));
				continue;
			}

			System::Nullable<bool>^ status;
			if (!System::String::IsNullOrEmpty(opId)) status = LatchHandler::CheckOperationStatus(opId, Configuration::GetConfig()->Timeout);

			if (!status->HasValue || (status->HasValue && status->Value) || System::String::IsNullOrEmpty(opId))
			{
				// Re-inject the matching packet.
				if (!WinDivertSend(handle, packet, packet_len, &addr, NULL))
				{
					UserLog::LogMessage(System::String::Format("Warning: failed to reinject packet: {0}", GetLastError()));
				}
			}
			else
			{
				UserLog::LogMessage(System::String::Format("Blocked packet matching: {0}", argString));

				// Print info about the matching packet.
				if (WinDivertHelperParsePacket(packet, packet_len, &ip_header,
					&ipv6_header, &icmp_header, &icmpv6_header, &tcp_header,
					&udp_header, NULL, &payload_len) && !(ip_header == NULL && ipv6_header == NULL))
				{
					System::String^ logMessage = System::String::Empty;
					// Dump packet info: 
					logMessage += "BLOCK ";

					if (ip_header != NULL)
					{
						UINT8 *src_addr = (UINT8 *)&ip_header->SrcAddr;
						UINT8 *dst_addr = (UINT8 *)&ip_header->DstAddr;
						logMessage += System::String::Format("ip.SrcAddr={0}.{1}.{2}.{3} ip.DstAddr={4}.{5}.{6}.{7} ",
							src_addr[0], src_addr[1], src_addr[2], src_addr[3],
							dst_addr[0], dst_addr[1], dst_addr[2], dst_addr[3]);
					}
					if (ipv6_header != NULL)
					{
						UINT16 *src_addr = (UINT16 *)&ipv6_header->SrcAddr;
						UINT16 *dst_addr = (UINT16 *)&ipv6_header->DstAddr;
						logMessage += "ipv6.SrcAddr=";
						for (int i = 0; i < 8; i++)
						{
							logMessage += System::String::Format("{0}{1}", ntohs(src_addr[i]), (i == 7 ? ' ' : ':'));
						}
						logMessage += " ipv6.DstAddr=";
						for (int i = 0; i < 8; i++)
						{
							logMessage += System::String::Format("{0}{1}", ntohs(dst_addr[i]), (i == 7 ? ' ' : ':'));
						}
						logMessage += " ";
					}
					if (icmp_header != NULL)
					{
						logMessage += System::String::Format("icmp.Type={0} icmp.Code={1} ",
							icmp_header->Type, icmp_header->Code);
						// Simply drop ICMP
					}
					if (icmpv6_header != NULL)
					{
						logMessage += System::String::Format("icmpv6.Type={0} icmpv6.Code={1} ",
							icmpv6_header->Type, icmpv6_header->Code);
						// Simply drop ICMPv6
					}
					if (tcp_header != NULL)
					{
						logMessage += System::String::Format("tcp.SrcPort={0} tcp.DstPort={1} tcp.Flags=",
							ntohs(tcp_header->SrcPort), ntohs(tcp_header->DstPort));
						if (tcp_header->Fin)
						{
							logMessage += "[FIN]";
						}
						if (tcp_header->Rst)
						{
							logMessage += "[RST]";
						}
						if (tcp_header->Urg)
						{
							logMessage += "[URG]";
						}
						if (tcp_header->Syn)
						{
							logMessage += "[SYN]";
						}
						if (tcp_header->Psh)
						{
							logMessage += "[PSH]";
						}
						if (tcp_header->Ack)
						{
							logMessage += "[ACK]";
						}
						logMessage += " ";

						if (ip_header != NULL)
						{
							reset->ip.SrcAddr = ip_header->DstAddr;
							reset->ip.DstAddr = ip_header->SrcAddr;
							reset->tcp.SrcPort = tcp_header->DstPort;
							reset->tcp.DstPort = tcp_header->SrcPort;
							reset->tcp.SeqNum =
								(tcp_header->Ack ? tcp_header->AckNum : 0);
							reset->tcp.AckNum =
								(tcp_header->Syn ?
								htonl(ntohl(tcp_header->SeqNum) + 1) :
								htonl(ntohl(tcp_header->SeqNum) + payload_len));

							WinDivertHelperCalcChecksums((PVOID)reset, sizeof(TCPPACKET),
								0);

							memcpy(&send_addr, &recv_addr, sizeof(send_addr));
							send_addr.Direction = !recv_addr.Direction;
							if (!WinDivertSend(handle, (PVOID)reset, sizeof(TCPPACKET),
								&send_addr, NULL))
							{
								UserLog::LogMessage(System::String::Format(
									"Warning: failed to send TCP reset ({0}), dropping packet instead...", GetLastError()));
							}
						}

						if (ipv6_header != NULL)
						{
							memcpy(resetv6->ipv6.SrcAddr, ipv6_header->DstAddr,
								sizeof(resetv6->ipv6.SrcAddr));
							memcpy(resetv6->ipv6.DstAddr, ipv6_header->SrcAddr,
								sizeof(resetv6->ipv6.DstAddr));
							resetv6->tcp.SrcPort = tcp_header->DstPort;
							resetv6->tcp.DstPort = tcp_header->SrcPort;
							resetv6->tcp.SeqNum =
								(tcp_header->Ack ? tcp_header->AckNum : 0);
							resetv6->tcp.AckNum =
								(tcp_header->Syn ?
								htonl(ntohl(tcp_header->SeqNum) + 1) :
								htonl(ntohl(tcp_header->SeqNum) + payload_len));

							WinDivertHelperCalcChecksums((PVOID)resetv6,
								sizeof(TCPV6PACKET), 0);

							memcpy(&send_addr, &recv_addr, sizeof(send_addr));
							send_addr.Direction = !recv_addr.Direction;
							if (!WinDivertSend(handle, (PVOID)resetv6, sizeof(TCPV6PACKET),
								&send_addr, NULL))
							{
								UserLog::LogMessage(System::String::Format(
									"Warning: failed to send TCP (IPV6) reset ({0}), dropping packet instead...", GetLastError()));
							}
						}
					}
					if (udp_header != NULL)
					{
						logMessage += System::String::Format("udp.SrcPort={0} udp.DstPort={1} ",
							ntohs(udp_header->SrcPort), ntohs(udp_header->DstPort));

						if (ip_header != NULL)
						{
							// NOTE: For some ICMP error messages, WFP does not seem to
							//       support INBOUND injection.  As a work-around, we
							//       always inject OUTBOUND.
							UINT icmp_length = ip_header->HdrLength*sizeof(UINT32) + 8;
							memcpy(dnr->data, ip_header, icmp_length);
							icmp_length += sizeof(ICMPPACKET);
							dnr->ip.Length = htons((UINT16)icmp_length);
							dnr->ip.SrcAddr = ip_header->DstAddr;
							dnr->ip.DstAddr = ip_header->SrcAddr;

							WinDivertHelperCalcChecksums((PVOID)dnr, icmp_length, 0);

							memcpy(&send_addr, &recv_addr, sizeof(send_addr));
							send_addr.Direction = WINDIVERT_DIRECTION_OUTBOUND;
							if (!WinDivertSend(handle, (PVOID)dnr, icmp_length, &send_addr,
								NULL))
							{
								UserLog::LogMessage(System::String::Format(
									"Warning: failed to send ICMP message({0}), dropping packet instead...", GetLastError()));
							}
						}

						if (ipv6_header != NULL)
						{
							UINT icmpv6_length = sizeof(WINDIVERT_IPV6HDR) +
								sizeof(WINDIVERT_TCPHDR);
							memcpy(dnrv6->data, ipv6_header, icmpv6_length);
							icmpv6_length += sizeof(ICMPV6PACKET);
							memcpy(dnrv6->ipv6.SrcAddr, ipv6_header->DstAddr,
								sizeof(dnrv6->ipv6.SrcAddr));
							memcpy(dnrv6->ipv6.DstAddr, ipv6_header->SrcAddr,
								sizeof(dnrv6->ipv6.DstAddr));

							WinDivertHelperCalcChecksums((PVOID)dnrv6, icmpv6_length, 0);

							memcpy(&send_addr, &recv_addr, sizeof(send_addr));
							send_addr.Direction = WINDIVERT_DIRECTION_OUTBOUND;
							if (!WinDivertSend(handle, (PVOID)dnrv6, icmpv6_length,
								&send_addr, NULL))
							{
								UserLog::LogMessage(System::String::Format(
									"Warning: failed to send ICMPv6 message({0}), dropping packet instead...", GetLastError()));
							}
						}
					}
					
					if (!System::String::IsNullOrEmpty(logMessage)) UserLog::LogMessage(logMessage);
				}
				else
				{
					// DROP PACKET
					UserLog::LogMessage("Dropping packet");
				}

			}
		}
	}

	return 0;
}

/*
* Initialize a PACKET.
*/
static void PacketIpInit(PWINDIVERT_IPHDR packet)
{
	memset(packet, 0, sizeof(WINDIVERT_IPHDR));
	packet->Version = 4;
	packet->HdrLength = sizeof(WINDIVERT_IPHDR) / sizeof(UINT32);
	packet->Id = ntohs(0xDEAD);
	packet->TTL = 64;
}

/*
* Initialize a TCPPACKET.
*/
static void PacketIpTcpInit(PTCPPACKET packet)
{
	memset(packet, 0, sizeof(TCPPACKET));
	PacketIpInit(&packet->ip);
	packet->ip.Length = htons(sizeof(TCPPACKET));
	packet->ip.Protocol = IPPROTO_TCP;
	packet->tcp.HdrLength = sizeof(WINDIVERT_TCPHDR) / sizeof(UINT32);
}

/*
* Initialize an ICMPPACKET.
*/
static void PacketIpIcmpInit(PICMPPACKET packet)
{
	memset(packet, 0, sizeof(ICMPPACKET));
	PacketIpInit(&packet->ip);
	packet->ip.Protocol = IPPROTO_ICMP;
}

/*
* Initialize a PACKETV6.
*/
static void PacketIpv6Init(PWINDIVERT_IPV6HDR packet)
{
	memset(packet, 0, sizeof(WINDIVERT_IPV6HDR));
	packet->Version = 6;
	packet->HopLimit = 64;
}

/*
* Initialize a TCPV6PACKET.
*/
static void PacketIpv6TcpInit(PTCPV6PACKET packet)
{
	memset(packet, 0, sizeof(TCPV6PACKET));
	PacketIpv6Init(&packet->ipv6);
	packet->ipv6.Length = htons(sizeof(WINDIVERT_TCPHDR));
	packet->ipv6.NextHdr = IPPROTO_TCP;
	packet->tcp.HdrLength = sizeof(WINDIVERT_TCPHDR) / sizeof(UINT32);
}

/*
* Initialize an ICMP PACKET.
*/
static void PacketIpv6Icmpv6Init(PICMPV6PACKET packet)
{
	memset(packet, 0, sizeof(ICMPV6PACKET));
	PacketIpv6Init(&packet->ipv6);
	packet->ipv6.NextHdr = IPPROTO_ICMPV6;
}