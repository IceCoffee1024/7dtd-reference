using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001BF3 RID: 7155
	public static class XblHelpers
	{
		// Token: 0x1400012E RID: 302
		// (add) Token: 0x0600D429 RID: 54313 RVA: 0x004C8F60 File Offset: 0x004C7160
		// (remove) Token: 0x0600D42A RID: 54314 RVA: 0x004C8F94 File Offset: 0x004C7194
		public static event XblHelpers.ErrorDelegate OnError;

		// Token: 0x0600D42B RID: 54315 RVA: 0x004C8FC8 File Offset: 0x004C71C8
		public static bool Succeeded(int _hresult, string _operationFriendlyName, bool _logToConsole = true, bool _printSuccess = false)
		{
			if (Unity.XGamingRuntime.Interop.HR.SUCCEEDED(_hresult))
			{
				if (_printSuccess && _logToConsole)
				{
					Log.Out("[XBL] Success: " + _operationFriendlyName);
				}
				return true;
			}
			string text;
			if (!XblHelpers.hresultToFriendlyErrorLookup.TryGetValue(_hresult, out text))
			{
				text = _operationFriendlyName + " failed. Error code: " + XblHelpers.GetHRName(_hresult);
			}
			if (_logToConsole)
			{
				Log.Error(string.Format("[XBL] Error: 0x{0:X8} - {1}", _hresult, text));
			}
			XblHelpers.ErrorDelegate onError = XblHelpers.OnError;
			if (onError != null)
			{
				onError(_hresult, _operationFriendlyName, text);
			}
			return false;
		}

		// Token: 0x0600D42C RID: 54316 RVA: 0x004C9044 File Offset: 0x004C7244
		[PublicizedFrom(EAccessModifier.Private)]
		static XblHelpers()
		{
			XblHelpers.hresultToFriendlyErrorLookup[-2143330041] = "IAP_UNEXPECTED: Does the player you are signed in as have a license for the game? You can get one by downloading your game from the store and purchasing it first. If you can't find your game in the store, have you published it in Partner Center?";
			XblHelpers.hresultToFriendlyErrorLookup[-2015035361] = "Missing Game Config";
		}

		// Token: 0x0600D42D RID: 54317 RVA: 0x004C9104 File Offset: 0x004C7304
		public static string GetHRName(int hr)
		{
			string text;
			if (!XblHelpers.s_hrToName.TryGetValue(hr, out text))
			{
				return "UNKNOWN";
			}
			return text;
		}

		// Token: 0x0600D42E RID: 54318 RVA: 0x004C9128 File Offset: 0x004C7328
		public static void LogHR(int hr, string identifier, bool failWarn = false)
		{
			bool flag = Unity.XGamingRuntime.Interop.HR.SUCCEEDED(hr);
			string text = (flag ? "SUCCEEDED" : "FAILED");
			string text2 = string.Format("[HResult] {0} (0x{1:X8} = {2}) {3}", new object[]
			{
				text,
				hr,
				XblHelpers.GetHRName(hr),
				identifier
			});
			if (flag)
			{
				Log.Out(text2);
				return;
			}
			if (failWarn)
			{
				Log.Warning(text2);
				return;
			}
			Log.Error(text2);
		}

		// Token: 0x0400A152 RID: 41298
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<int, string> hresultToFriendlyErrorLookup = new Dictionary<int, string>();

		// Token: 0x0400A153 RID: 41299
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly IReadOnlyDictionary<int, string> s_hrToName = (from f in new Type[]
			{
				typeof(Unity.XGamingRuntime.Interop.HR),
				typeof(Unity.XGamingRuntime.HR),
				typeof(HREx)
			}.SelectMany<Type, FieldInfo>((Type t) => t.GetFields(BindingFlags.Static | BindingFlags.Public))
			where f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(int)
			select f).ToDictionary<FieldInfo, int, string>((FieldInfo f) => (int)f.GetValue(null), (FieldInfo f) => f.Name);

		// Token: 0x02001BF4 RID: 7156
		// (Invoke) Token: 0x0600D430 RID: 54320
		public delegate void ErrorDelegate(int _hresult, string _operationFriendlyName, string _errorMessage);
	}
}
