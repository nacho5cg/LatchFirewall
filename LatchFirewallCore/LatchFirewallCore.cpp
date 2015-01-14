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

#if (_DEBUG)
	while (!System::Diagnostics::Debugger::IsAttached) Sleep(1000);
#endif

	Configuration^ conf = Configuration::GetConfig();

	for each (FirewallRule^ rule in Configuration::GetConfig()->Rules)
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

			if (!status->HasValue || status->HasValue && status->Value)
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