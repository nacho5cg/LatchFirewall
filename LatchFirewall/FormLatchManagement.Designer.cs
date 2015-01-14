namespace LatchFirewall
{
    partial class FormLatchManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpPairing = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlUnpair = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUnpair = new System.Windows.Forms.Button();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.txtSecret = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAppId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.udnTimeout = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPairingToken = new System.Windows.Forms.TextBox();
            this.btnPair = new System.Windows.Forms.Button();
            this.pnlPair = new System.Windows.Forms.Panel();
            this.grpPairing.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlUnpair.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.grpConnection.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udnTimeout)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnlPair.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPairing
            // 
            this.grpPairing.Controls.Add(this.flowLayoutPanel1);
            this.grpPairing.Location = new System.Drawing.Point(12, 116);
            this.grpPairing.Name = "grpPairing";
            this.grpPairing.Size = new System.Drawing.Size(367, 77);
            this.grpPairing.TabIndex = 1;
            this.grpPairing.TabStop = false;
            this.grpPairing.Text = "Pairing with Latch service";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.pnlPair);
            this.flowLayoutPanel1.Controls.Add(this.pnlUnpair);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(361, 58);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // pnlUnpair
            // 
            this.pnlUnpair.Controls.Add(this.label2);
            this.pnlUnpair.Controls.Add(this.btnUnpair);
            this.pnlUnpair.Location = new System.Drawing.Point(3, 60);
            this.pnlUnpair.Name = "pnlUnpair";
            this.pnlUnpair.Size = new System.Drawing.Size(285, 51);
            this.pnlUnpair.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Your account is paired with Latch";
            // 
            // btnUnpair
            // 
            this.btnUnpair.Location = new System.Drawing.Point(200, 14);
            this.btnUnpair.Name = "btnUnpair";
            this.btnUnpair.Size = new System.Drawing.Size(75, 23);
            this.btnUnpair.TabIndex = 1;
            this.btnUnpair.Text = "Unpair";
            this.btnUnpair.UseVisualStyleBackColor = true;
            this.btnUnpair.Click += new System.EventHandler(this.btnUnpair_Click);
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.txtSecret);
            this.grpSettings.Controls.Add(this.label4);
            this.grpSettings.Controls.Add(this.txtAppId);
            this.grpSettings.Controls.Add(this.label3);
            this.grpSettings.Location = new System.Drawing.Point(12, 12);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(367, 95);
            this.grpSettings.TabIndex = 0;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Latch settings";
            // 
            // txtSecret
            // 
            this.txtSecret.Location = new System.Drawing.Point(70, 51);
            this.txtSecret.MaxLength = 40;
            this.txtSecret.Name = "txtSecret";
            this.txtSecret.PasswordChar = '*';
            this.txtSecret.Size = new System.Drawing.Size(288, 20);
            this.txtSecret.TabIndex = 3;
            this.txtSecret.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Secret";
            // 
            // txtAppId
            // 
            this.txtAppId.Location = new System.Drawing.Point(70, 25);
            this.txtAppId.MaxLength = 20;
            this.txtAppId.Name = "txtAppId";
            this.txtAppId.Size = new System.Drawing.Size(288, 20);
            this.txtAppId.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "App ID";
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.flowLayoutPanel2);
            this.grpConnection.Location = new System.Drawing.Point(12, 199);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(367, 77);
            this.grpConnection.TabIndex = 2;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "Connection";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.panel1);
            this.flowLayoutPanel2.Controls.Add(this.panel2);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(361, 58);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.udnTimeout);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(285, 51);
            this.panel1.TabIndex = 0;
            // 
            // udnTimeout
            // 
            this.udnTimeout.Location = new System.Drawing.Point(93, 17);
            this.udnTimeout.Maximum = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            this.udnTimeout.Name = "udnTimeout";
            this.udnTimeout.Size = new System.Drawing.Size(100, 20);
            this.udnTimeout.TabIndex = 2;
            this.udnTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Timeout (ms)";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Location = new System.Drawing.Point(3, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(285, 51);
            this.panel2.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Your account is paired with Latch";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(200, 14);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Unpair";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pairing token";
            // 
            // txtPairingToken
            // 
            this.txtPairingToken.Location = new System.Drawing.Point(93, 16);
            this.txtPairingToken.MaxLength = 6;
            this.txtPairingToken.Name = "txtPairingToken";
            this.txtPairingToken.Size = new System.Drawing.Size(100, 20);
            this.txtPairingToken.TabIndex = 1;
            // 
            // btnPair
            // 
            this.btnPair.Location = new System.Drawing.Point(199, 14);
            this.btnPair.Name = "btnPair";
            this.btnPair.Size = new System.Drawing.Size(75, 23);
            this.btnPair.TabIndex = 2;
            this.btnPair.Text = "Pair";
            this.btnPair.UseVisualStyleBackColor = true;
            this.btnPair.Click += new System.EventHandler(this.btnPair_Click);
            // 
            // pnlPair
            // 
            this.pnlPair.Controls.Add(this.btnPair);
            this.pnlPair.Controls.Add(this.txtPairingToken);
            this.pnlPair.Controls.Add(this.label1);
            this.pnlPair.Location = new System.Drawing.Point(3, 3);
            this.pnlPair.Name = "pnlPair";
            this.pnlPair.Size = new System.Drawing.Size(285, 51);
            this.pnlPair.TabIndex = 0;
            // 
            // FormLatchManagement
            // 
            this.AcceptButton = this.btnPair;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 291);
            this.Controls.Add(this.grpConnection);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.grpPairing);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormLatchManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Latch management";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLatchManagement_FormClosing);
            this.Load += new System.EventHandler(this.FormLatchManagement_Load);
            this.grpPairing.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlUnpair.ResumeLayout(false);
            this.pnlUnpair.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpConnection.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udnTimeout)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlPair.ResumeLayout(false);
            this.pnlPair.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPairing;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel pnlUnpair;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUnpair;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.TextBox txtSecret;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAppId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown udnTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel pnlPair;
        private System.Windows.Forms.Button btnPair;
        private System.Windows.Forms.TextBox txtPairingToken;
        private System.Windows.Forms.Label label1;
    }
}