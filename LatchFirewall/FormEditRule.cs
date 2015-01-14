using LatchFirewallLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LatchFirewall
{
    public partial class FormEditRule : Form
    {
        public FormEditRule()
        {
            InitializeComponent();

            this.cmbType.DataSource = FirewallRule.GetRuleTypesWithNames().ToList();
        }        

        public FormEditRule(FirewallRule rule) : this()
		{			
			this.Rule = rule;
            if (this.Rule == null)
			{
				this.Rule = new FirewallRule();				
				return;
			}

            FillControls();
		}

        public FirewallRule Rule
        {
            get;
            private set;
        }

        private void FillControls()
		{
            this.cmbType.SelectedValue = this.Rule.RuleTypeInUse;
            this.txtSrcIp.Text = this.Rule.SourceIp;
            this.txtDstIp.Text = this.Rule.DestinationIp;
            this.numSrcPort.Value = this.Rule.SourcePort;
            this.numDstPort.Value = this.Rule.DestinationPort;
            this.radioButton1.Checked = (this.Rule.ProtocolInUse == FirewallRule.Protocol.tcp);
            this.radioButton2.Checked = (this.Rule.ProtocolInUse == FirewallRule.Protocol.udp);
            this.txtComment.Text = this.Rule.Comment;
            this.txtOpId.Text = this.Rule.OpId;

            if (!String.IsNullOrEmpty(this.Rule.CustomFilterExpression))
            {
                this.txtCustomExp.Text = this.Rule.CustomFilterExpression;
                this.chkCustom.Checked = true;
            }
		}

        private void UpdateMonitoredUsbFromControls()
        {
            if (!this.chkCustom.Checked)
            {
                this.Rule.RuleTypeInUse = (FirewallRule.RuleType)this.cmbType.SelectedValue;
                this.Rule.SourceIp = this.txtSrcIp.Text;
                this.Rule.DestinationIp = this.txtDstIp.Text;
                this.Rule.SourcePort = Convert.ToUInt32(this.numSrcPort.Value);
                this.Rule.DestinationPort = Convert.ToUInt32(this.numDstPort.Value);
                this.Rule.Comment = this.txtComment.Text;
                this.Rule.ProtocolInUse = (this.radioButton1.Checked) ? FirewallRule.Protocol.tcp : FirewallRule.Protocol.udp;
                this.Rule.CustomFilterExpression = string.Empty;
            }
            else
            {
                this.Rule.CustomFilterExpression = this.txtCustomExp.Text.Trim();
            }
                        
            this.Rule.OpId = this.txtOpId.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string errorMsg = string.Empty;
            Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            Regex opId = new Regex("[a-zA-Z0-9]{20}");

            if (opId.IsMatch(this.txtOpId.Text))
            {
                if (ip.IsMatch(this.txtSrcIp.Text) || String.IsNullOrEmpty(this.txtSrcIp.Text))
                {
                    if (ip.IsMatch(this.txtDstIp.Text) || String.IsNullOrEmpty(this.txtDstIp.Text))
                    {
                        this.UpdateMonitoredUsbFromControls();
                    }
                    else
                    {
                        errorMsg = "Invalid Destination IP";
                    }
                }
                else
                {
                    errorMsg = "Invalid Source IP";
                }
            }
            else
            {
                errorMsg = "Invalid Operation ID";                
            }

            if (this.chkCustom.Checked && !this.txtCustomExp.Text.Contains("opId="))
            {
                if (MessageBox.Show("No operation ID detected in custom expression. All matching packets will be blocked without Latch being consulted, continue anyhow?",
                    "Continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    base.DialogResult = DialogResult.None;
            }

            if (errorMsg != string.Empty)
            {
                MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                base.DialogResult = DialogResult.None;
            }
            base.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {            
            if (((CheckBox)sender).Checked)
            {
                SetBasicControlsEnabledState(false);
            }
            else
            {
                SetBasicControlsEnabledState(true);
            }
        }

        private void SetBasicControlsEnabledState(bool state)
        {
            this.txtSrcIp.Enabled = state;
            this.txtDstIp.Enabled = state;
            this.numSrcPort.Enabled = state;
            this.numDstPort.Enabled = state;
            this.radioButton1.Enabled = state;
            this.radioButton2.Enabled = state;
            this.cmbType.Enabled = state;
            this.txtOpId.Enabled = state;
            this.txtCustomExp.Enabled = !state;            
        }
    }
}
