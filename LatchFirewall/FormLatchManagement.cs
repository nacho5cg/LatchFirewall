using LatchFirewallLibrary;
using System;
using System.Windows.Forms;

namespace LatchFirewall
{
    public partial class FormLatchManagement : Form
    {
        public FormLatchManagement()
        {
            InitializeComponent();
        }

        private void FormLatchManagement_Load(object sender, EventArgs e)
        {
            UpdatePairingStatus();

            Configuration config = Configuration.GetConfig();
            this.txtAppId.Text = config.AppId;
            this.txtSecret.Text = config.Secret;
            this.udnTimeout.Value = config.Timeout;
        }

        private void FormLatchManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateAndSaveConfig();
        }

        private void UpdateAndSaveConfig()
        {
            Configuration config = Configuration.GetConfig();
            config.AppId = this.txtAppId.Text.Trim();
            config.Secret = this.txtSecret.Text.Trim();
            try
            {
                config.Timeout = Convert.ToInt32(this.udnTimeout.Value);
            }
            catch (OverflowException)
            {
                MessageBox.Show(String.Format("Value must be between 0 and {0}", udnTimeout.Maximum));
                config.Timeout = 0;
            }
            Configuration.Save();
        }

        private void btnPair_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateAndSaveConfig();
                LatchHandler.Pair(this.txtPairingToken.Text);
                this.txtPairingToken.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Account pairing error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            UpdatePairingStatus();
        }

        private void btnUnpair_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateAndSaveConfig();
                LatchHandler.Unpair();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Account unpairing error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            UpdatePairingStatus();
        }

        private void UpdatePairingStatus()
        {
            this.pnlPair.Visible = !LatchHandler.IsPaired;
            this.pnlUnpair.Visible = LatchHandler.IsPaired;
        }

    }
}
