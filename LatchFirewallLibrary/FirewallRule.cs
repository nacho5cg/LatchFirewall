using System;
using System.Collections.Generic;

namespace LatchFirewallLibrary
{
    [Serializable]
    public class FirewallRule
    {
        public enum RuleType { inbound, outbound, both };
        public enum Protocol { tcp, udp };

        public RuleType RuleTypeInUse { get; set; }
        public Protocol ProtocolInUse { get; set; }
        public string SourceIp { get; set; }
        public string DestinationIp { get; set; }
        public UInt32 SourcePort { get; set; }
        public UInt32 DestinationPort { get; set; }
        public string OpId { get; set; }
        public string Comment { get; set; }

        public string CustomFilterExpression { get; set; }
        public string FilterExpression
        {
            get
            {
                if (string.IsNullOrEmpty(CustomFilterExpression))
                {
                    string expression = string.Empty;

                    if (this.RuleTypeInUse != RuleType.both) expression += String.Format("{0}", RuleTypeInUse.ToString());
                    if (!String.IsNullOrEmpty(this.SourceIp)) expression += String.Format("{0}ip.SrcAddr == {1}", (this.RuleTypeInUse != RuleType.both) ? " and " : "", SourceIp);
                    if (!String.IsNullOrEmpty(this.DestinationIp)) expression += String.Format("{0}ip.DstAddr == {1}", expression.Length > 0 ? " and " : "", DestinationIp);
                    if (this.SourcePort > 0) expression += String.Format(" and {0}.SrcPort == {1}", ProtocolInUse.ToString(), SourcePort.ToString());
                    if (this.DestinationPort > 0) expression += String.Format(" and {0}.DstPort == {1}", ProtocolInUse.ToString(), DestinationPort.ToString());
                    if (!String.IsNullOrEmpty(this.OpId)) expression += String.Format(" opId={0}", this.OpId);

                    return expression;
                }
                else
                {
                    return CustomFilterExpression;
                }
            }
        }             

        public FirewallRule()
        {
        }

        public static Dictionary<RuleType, string> GetRuleTypesWithNames()
        {
            return new Dictionary<RuleType, string>() { { RuleType.inbound, "Inbound" }, { RuleType.outbound, "Outbound" }, { RuleType.both, "Both" } };
        }

        public override string ToString()
        {
            return string.Format("{0} opId={1} #{2}", this.FilterExpression, this.OpId, this.Comment);
        }

    }
}
