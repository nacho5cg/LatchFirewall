namespace LatchFirewall
{
    partial class FormEditRule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditRule));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.numDstPort = new System.Windows.Forms.NumericUpDown();
            this.numSrcPort = new System.Windows.Forms.NumericUpDown();
            this.txtDstIp = new System.Windows.Forms.TextBox();
            this.txtSrcIp = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtOpId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtCustomExp = new System.Windows.Forms.TextBox();
            this.chkCustom = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDstPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtComment);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbType);
            this.groupBox1.Controls.Add(this.numDstPort);
            this.groupBox1.Controls.Add(this.numSrcPort);
            this.groupBox1.Controls.Add(this.txtDstIp);
            this.groupBox1.Controls.Add(this.txtSrcIp);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 205);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rule";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(103, 173);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(331, 20);
            this.txtComment.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Comment";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(155, 149);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(48, 17);
            this.radioButton2.TabIndex = 12;
            this.radioButton2.Text = "UDP";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Protocol";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(103, 149);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(46, 17);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "TCP";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Type";
            // 
            // cmbType
            // 
            this.cmbType.DisplayMember = "Value";
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(103, 18);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(331, 21);
            this.cmbType.TabIndex = 8;
            this.cmbType.ValueMember = "Key";
            // 
            // numDstPort
            // 
            this.numDstPort.Location = new System.Drawing.Point(103, 123);
            this.numDstPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numDstPort.Name = "numDstPort";
            this.numDstPort.Size = new System.Drawing.Size(331, 20);
            this.numDstPort.TabIndex = 7;
            // 
            // numSrcPort
            // 
            this.numSrcPort.Location = new System.Drawing.Point(103, 97);
            this.numSrcPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numSrcPort.Name = "numSrcPort";
            this.numSrcPort.Size = new System.Drawing.Size(331, 20);
            this.numSrcPort.TabIndex = 6;
            // 
            // txtDstIp
            // 
            this.txtDstIp.Location = new System.Drawing.Point(103, 71);
            this.txtDstIp.Name = "txtDstIp";
            this.txtDstIp.Size = new System.Drawing.Size(331, 20);
            this.txtDstIp.TabIndex = 5;
            // 
            // txtSrcIp
            // 
            this.txtSrcIp.Location = new System.Drawing.Point(103, 45);
            this.txtSrcIp.Name = "txtSrcIp";
            this.txtSrcIp.Size = new System.Drawing.Size(331, 20);
            this.txtSrcIp.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Destination port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Source port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Destination IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Source IP";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtOpId);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 220);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(440, 52);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Latch";
            // 
            // txtOpId
            // 
            this.txtOpId.Location = new System.Drawing.Point(103, 19);
            this.txtOpId.MaxLength = 20;
            this.txtOpId.Name = "txtOpId";
            this.txtOpId.Size = new System.Drawing.Size(331, 20);
            this.txtOpId.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Operation ID";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(296, 338);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(377, 338);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtCustomExp);
            this.groupBox3.Controls.Add(this.chkCustom);
            this.groupBox3.Location = new System.Drawing.Point(12, 278);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(440, 46);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Advanced";
            // 
            // txtCustomExp
            // 
            this.txtCustomExp.Enabled = false;
            this.txtCustomExp.Location = new System.Drawing.Point(141, 17);
            this.txtCustomExp.Name = "txtCustomExp";
            this.txtCustomExp.Size = new System.Drawing.Size(293, 20);
            this.txtCustomExp.TabIndex = 2;
            // 
            // chkCustom
            // 
            this.chkCustom.AutoSize = true;
            this.chkCustom.Location = new System.Drawing.Point(21, 19);
            this.chkCustom.Name = "chkCustom";
            this.chkCustom.Size = new System.Drawing.Size(114, 17);
            this.chkCustom.TabIndex = 1;
            this.chkCustom.Text = "Custom expression";
            this.chkCustom.UseVisualStyleBackColor = true;
            this.chkCustom.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // FormEditRule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 371);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(480, 410);
            this.MinimumSize = new System.Drawing.Size(480, 410);
            this.Name = "FormEditRule";
            this.Text = "FormEditRule";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDstPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSrcPort)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numDstPort;
        private System.Windows.Forms.NumericUpDown numSrcPort;
        private System.Windows.Forms.TextBox txtDstIp;
        private System.Windows.Forms.TextBox txtSrcIp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtOpId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtCustomExp;
        private System.Windows.Forms.CheckBox chkCustom;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label label8;
    }
}