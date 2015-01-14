using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using LatchFirewallLibrary;
using System.IO;

namespace LatchFirewallService
{
    public partial class FirewallService : ServiceBase
    {
        private static Thread workerThread = null;
        private static string logFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Log.txt");
        private static FileSystemWatcher configWatcher;
        private static Process CoreProcessHandle;

        public FirewallService()
        {
            InitializeComponent();

            try
            {
                configWatcher = new FileSystemWatcher(Path.GetDirectoryName(Configuration.ConfigFile), Path.GetFileName(Configuration.ConfigFile));
                configWatcher.Changed += configWatcher_Changed;
                configWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                UserLog.LogMessage(ex);
            }

            workerThread = new Thread(new ThreadStart(ThreadEntryPoint));
        }

        static void configWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Configuration.InvalidateCachedData();            

            RestartCoreProcess();
        }

        protected override void OnStart(string[] args)
        {
            workerThread.Start();
        }

        protected override void OnStop()
        {
            UserLog.LogMessage("Stopping service...");
            try
            {
                if (CoreProcessHandle != null) CoreProcessHandle.Kill();
                workerThread.Abort();
            }
            catch (Exception ex)
            {
                UserLog.LogMessage(ex);
            }
        }

        static private void RestartCoreProcess()
        {
            try
            {
                if (CoreProcessHandle != null)
                {
                    CoreProcessHandle.Kill();
                    Thread.Sleep(150);
                }

                var pi = new ProcessStartInfo(Path.Combine(UserLog.CurrentDirectory, "LatchFirewallCore.exe"))
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    Verb = "runas"
                };
                CoreProcessHandle = Process.Start(pi);
            }
            catch (Exception ex)
            {
                UserLog.LogMessage(ex);
            }
        }

        private void ThreadEntryPoint()
        {
//#if (DEBUG)
//            while (!Debugger.IsAttached) Thread.Sleep(1000);
//#endif

            UserLog.LogMessage("Starting service...");
            RestartCoreProcess();            
        }
    }
}       