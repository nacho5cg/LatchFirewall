using LatchFirewallLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LatchFirewall
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            this.Load += FormMain_Load;            
        }

        private FileSystemWatcher logWatcher;
        private static ServiceController serviceController = new ServiceController("Latch Firewall Service");
        private delegate void ShowLogCallback(string message);

        void FormMain_Load(object sender, EventArgs e)
        {
            //this.linkService.Text = (ServiceRunning.HasValue && ServiceRunning.Value) ? "Stop service" : "Start service";

            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.CellDoubleClick += new DataGridViewCellEventHandler(this.editButton_Click);

            if (File.Exists(UserLog.LogFile))
            {
                try
                {
                    base.Show();
                    this.ShowLog(UserLog.GetLogTail());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Couldn't read from {0}: {1}", UserLog.LogFile, ex.Message), "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            this.logWatcher = new FileSystemWatcher(Path.GetDirectoryName(UserLog.LogFile), Path.GetFileName(UserLog.LogFile));
            this.logWatcher.Changed += new FileSystemEventHandler(this.logWatcher_Changed);
            this.logWatcher.EnableRaisingEvents = true;

            this.UpdateRules();
        }          

        private Nullable<bool> ServiceRunning
        {
            get
            {
                try
                {
                    return (serviceController.Status == ServiceControllerStatus.Running);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Service state error: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        //private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    try
        //    {
        //        this.linkService.Enabled = false;

        //        if (ServiceRunning.HasValue)
        //        {
        //            if (ServiceRunning.Value)
        //            {
        //                serviceController.Stop();
        //                this.linkService.Text = "Start service";
        //            }
        //            else
        //            {
        //                serviceController.Start();
        //                this.linkService.Text = "Stop service";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(String.Format("Error: {0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        this.linkService.Enabled = true;
        //    }
        //}

        private void ShowLog(string message)
        {
            if (this.txtActionLog.InvokeRequired)
            {
                FormMain.ShowLogCallback method = new FormMain.ShowLogCallback(this.ShowLog);
                base.Invoke(method, new object[]
				{
					message
				});
                return;
            }
            this.txtActionLog.Text = message;
            this.txtActionLog.SelectionStart = this.txtActionLog.Text.Length;
            this.txtActionLog.ScrollToCaret();
        }

        private void logWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            this.ShowLog(UserLog.GetLogTail());
        }

        private void SaveConfigAndRefresh()
        {
            Configuration.Save();
            this.UpdateRules();
        }

        private void UpdateRules()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Configuration.GetConfig().Rules;
            this.dataGridView1.Refresh();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            FormEditRule FormEditRule = new FormEditRule(null);
            if (FormEditRule.ShowDialog() == DialogResult.OK)
            {
                Configuration.GetConfig().Rules.Add(FormEditRule.Rule);
                this.SaveConfigAndRefresh();
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                FirewallRule usb = (FirewallRule)this.dataGridView1.SelectedRows[0].DataBoundItem;
                FormEditRule FormEditRule = new FormEditRule(usb);
                if (FormEditRule.ShowDialog() == DialogResult.OK)
                {
                    this.SaveConfigAndRefresh();
                }
            }
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0 && MessageBox.Show("Do you want to remove the selected rule?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                FirewallRule item = (FirewallRule)this.dataGridView1.SelectedRows[0].DataBoundItem;
                Configuration.GetConfig().Rules.Remove(item);
                this.SaveConfigAndRefresh();
            }
        }

        private void btnLatch_Click(object sender, EventArgs e)
        {
            var frm = new FormLatchManagement();
            frm.ShowDialog();

            Configuration.Save();
        }
    }
}
