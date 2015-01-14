using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LatchFirewallLibrary
{
	public static class UserLog
	{
		public static readonly string LogFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Log.txt");
        public static readonly string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		public static void LogMessage(string message)
		{
			try
			{
				File.AppendAllText(UserLog.LogFile, DateTime.Now.ToString() + "\t" + message + Environment.NewLine);
			}
			catch (Exception)
			{
			}
		}

		public static void LogMessage(Exception ex)
		{
			string message = ex.Message;
			UserLog.LogMessage(message);
		}

		public static string GetLogTail()
		{
			string result;
			try
			{
				using (StreamReader streamReader = new StreamReader(UserLog.LogFile))
				{
					if (new FileInfo(UserLog.LogFile).Length >= 2500L)
					{
						streamReader.BaseStream.Position = 2000L;
						streamReader.ReadLine();
					}
					result = streamReader.ReadToEnd();
				}
			}
			catch (Exception)
			{
				result = string.Empty;
			}
			return result;
		}

		public static string GetLastLine()
		{
			string result;
			try
			{
				result = File.ReadLines(UserLog.LogFile).Last<string>();
			}
			catch (Exception)
			{
				result = string.Empty;
			}
			return result;
		}
	}
}