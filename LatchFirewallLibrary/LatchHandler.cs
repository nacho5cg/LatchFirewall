using LatchSDK;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LatchFirewallLibrary
{
	public class LatchHandler
	{
		private delegate bool? CheckOperationStatusDelegate(string operationId);
		public static bool IsPaired
		{
			get
			{
				return !string.IsNullOrEmpty(Configuration.GetConfig().AccountId);
			}
		}
		private static Latch LatchSDK
		{
			get
			{
				return new Latch(Configuration.GetConfig().AppId, Configuration.GetConfig().Secret);
			}
		}
		public static void Pair(string pairingToken)
		{
			if (!LatchHandler.IsPaired && !string.IsNullOrWhiteSpace(pairingToken))
			{
				LatchResponse latchResponse = LatchHandler.LatchSDK.Pair(pairingToken);
				if (latchResponse.Data != null && latchResponse.Data.ContainsKey("accountId"))
				{
					Configuration.GetConfig().AccountId = latchResponse.Data["accountId"].ToString();
					Configuration.Save();
					return;
				}
				if (latchResponse.Error != null)
				{
					throw new ApplicationException(latchResponse.Error.Message);
				}
			}
		}
		public static void Unpair()
		{
			if (LatchHandler.IsPaired)
			{
				Configuration config = Configuration.GetConfig();
				LatchHandler.LatchSDK.Unpair(config.AccountId);
				config.AccountId = null;
				Configuration.Save();
			}
		}
		public static bool? CheckOperationStatus(string operationId)
		{
			if (LatchHandler.IsPaired && !string.IsNullOrEmpty(operationId))
			{
				try
				{
					LatchResponse latchResponse = LatchHandler.LatchSDK.OperationStatus(Configuration.GetConfig().AccountId, operationId);
					if (latchResponse.Data != null && latchResponse.Data.ContainsKey("operations"))
					{
						Dictionary<string, object> dictionary = (Dictionary<string, object>)latchResponse.Data["operations"];
						if (dictionary.ContainsKey(operationId))
						{
							Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary[operationId];
							if (dictionary2.ContainsKey("status"))
							{
								string text = dictionary2["status"].ToString();
								return new bool?(text.Equals("on", StringComparison.InvariantCultureIgnoreCase));
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return null;
		}
		public static bool? CheckOperationStatus(string operationId, int timeout)
		{
			if (timeout <= 0)
			{
				return LatchHandler.CheckOperationStatus(operationId);
			}
			bool? result;
			try
			{
				LatchHandler.CheckOperationStatusDelegate checkOperationStatusDelegate = new LatchHandler.CheckOperationStatusDelegate(LatchHandler.CheckOperationStatus);
				IAsyncResult asyncResult = checkOperationStatusDelegate.BeginInvoke(operationId, null, null);
				asyncResult.AsyncWaitHandle.WaitOne(timeout, false);
				if (asyncResult.IsCompleted)
				{
					result = checkOperationStatusDelegate.EndInvoke(asyncResult);
				}
				else
				{
					ThreadPool.QueueUserWorkItem(delegate(object state)
					{
						try
						{
							object[] array = state as object[];
							LatchHandler.CheckOperationStatusDelegate checkOperationStatusDelegate2 = (LatchHandler.CheckOperationStatusDelegate)array[0];
							IAsyncResult result2 = (IAsyncResult)array[1];
							checkOperationStatusDelegate2.EndInvoke(result2);
						}
						catch (Exception)
						{
						}
					}, new object[]
					{
						checkOperationStatusDelegate,
						asyncResult
					});
					result = null;
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}
	}
}
