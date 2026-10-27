using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.XGamingRuntime;
using Unity.XGamingRuntime.Interop;

namespace Platform.XBL
{
	// Token: 0x02001C7D RID: 7293
	public static class XblHelpers
	{
		// Token: 0x1400012E RID: 302
		// (add) Token: 0x0600D843 RID: 55363 RVA: 0x004D7964 File Offset: 0x004D5B64
		// (remove) Token: 0x0600D844 RID: 55364 RVA: 0x004D7998 File Offset: 0x004D5B98
		public static event XblHelpers.ErrorDelegate OnError;

		// Token: 0x0600D845 RID: 55365 RVA: 0x004D79CC File Offset: 0x004D5BCC
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

		// Token: 0x0600D846 RID: 55366 RVA: 0x004D7A48 File Offset: 0x004D5C48
		[PublicizedFrom(EAccessModifier.Private)]
		static XblHelpers()
		{
			XblHelpers.hresultToFriendlyErrorLookup[-2143330041] = "IAP_UNEXPECTED: Does the player you are signed in as have a license for the game? You can get one by downloading your game from the store and purchasing it first. If you can't find your game in the store, have you published it in Partner Center?";
			XblHelpers.hresultToFriendlyErrorLookup[-2015035361] = "Missing Game Config";
		}

		// Token: 0x0600D847 RID: 55367 RVA: 0x004D7B08 File Offset: 0x004D5D08
		public static string GetHRName(int hr)
		{
			string text;
			if (!XblHelpers.s_hrToName.TryGetValue(hr, out text))
			{
				return "UNKNOWN";
			}
			return text;
		}

		// Token: 0x0600D848 RID: 55368 RVA: 0x004D7B2C File Offset: 0x004D5D2C
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

		// Token: 0x0400A441 RID: 42049
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<int, string> hresultToFriendlyErrorLookup = new Dictionary<int, string>();

		// Token: 0x0400A442 RID: 42050
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly IReadOnlyDictionary<int, string> s_hrToName = (from f in new Type[]
			{
				typeof(Unity.XGamingRuntime.Interop.HR),
				typeof(Unity.XGamingRuntime.HR),
				typeof(HREx)
			}.SelectMany<Type, FieldInfo>((Type t) => t.GetFields(BindingFlags.Static | BindingFlags.Public))
			where f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(int)
			select f).ToDictionary<FieldInfo, int, string>((FieldInfo f) => (int)f.GetValue(null), (FieldInfo f) => f.Name);

		// Token: 0x02001C7E RID: 7294
		// (Invoke) Token: 0x0600D84A RID: 55370
		public delegate void ErrorDelegate(int _hresult, string _operationFriendlyName, string _errorMessage);
	}
}
