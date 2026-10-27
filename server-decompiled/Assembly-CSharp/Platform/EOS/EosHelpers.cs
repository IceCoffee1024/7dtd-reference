using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Platform.EOS
{
	// Token: 0x02001D4F RID: 7503
	public static class EosHelpers
	{
		// Token: 0x0600DE48 RID: 56904 RVA: 0x004F0DAC File Offset: 0x004EEFAC
		[PublicizedFrom(EAccessModifier.Private)]
		static EosHelpers()
		{
			EnumDictionary<EPlatformIdentifier, ExternalAccountType> enumDictionary = new EnumDictionary<EPlatformIdentifier, ExternalAccountType>();
			foreach (KeyValuePair<ExternalAccountType, EPlatformIdentifier> keyValuePair in EosHelpers.accountTypeMappings)
			{
				enumDictionary.Add(keyValuePair.Value, keyValuePair.Key);
			}
			EosHelpers.PlatformIdentifierMappings = new ReadOnlyDictionary<EPlatformIdentifier, ExternalAccountType>(enumDictionary);
		}

		// Token: 0x0600DE49 RID: 56905 RVA: 0x004F0EA4 File Offset: 0x004EF0A4
		public static void TestEosConnection(Action<bool> _callback)
		{
			ThreadManager.StartThread("TestEosConnection", new ThreadManager.ThreadFunctionDelegate(EosHelpers.<TestEosConnection>g__workerFunc|10_0), new EosHelpers.EosConnectionTestInfo
			{
				Callback = _callback
			}, null, false, true);
		}

		// Token: 0x0600DE4A RID: 56906 RVA: 0x004F0ECC File Offset: 0x004EF0CC
		public static void AssertMainThread(string _id)
		{
			if (!ThreadManager.IsMainThread())
			{
				Log.Warning("[EOSH] Called EOS code from secondary thread: " + _id);
			}
		}

		// Token: 0x0600DE4B RID: 56907 RVA: 0x004F0EE8 File Offset: 0x004EF0E8
		public static ClientInfo.EDeviceType GetDeviceTypeFromPlatform(string platform)
		{
			if (platform == "other" || platform == "steam")
			{
				return ClientInfo.EDeviceType.Unknown;
			}
			if (platform == "playstation")
			{
				return ClientInfo.EDeviceType.PlayStation;
			}
			if (!(platform == "xbox"))
			{
				Log.Error("[EOS] [Auth] GetDeviceTypeFromPlatform: Unknown platform: " + platform);
				return ClientInfo.EDeviceType.Unknown;
			}
			return ClientInfo.EDeviceType.Xbox;
		}

		// Token: 0x0600DE4C RID: 56908 RVA: 0x004F0F42 File Offset: 0x004EF142
		public static bool RequiresAntiCheat(this ClientInfo.EDeviceType deviceType)
		{
			return deviceType != ClientInfo.EDeviceType.PlayStation && deviceType != ClientInfo.EDeviceType.Xbox;
		}

		// Token: 0x0600DE4D RID: 56909 RVA: 0x004F0F54 File Offset: 0x004EF154
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <TestEosConnection>g__workerFunc|10_0(ThreadManager.ThreadInfo _info)
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.epicgames.dev/sdk/v1/default");
				httpWebRequest.Timeout = 5000;
				httpWebRequest.KeepAlive = false;
				using ((HttpWebResponse)httpWebRequest.GetResponse())
				{
					((EosHelpers.EosConnectionTestInfo)_info.parameter).Result = true;
				}
			}
			catch (Exception ex)
			{
				Log.Out("[EOS] Connection test failed: " + ex.Message);
				((EosHelpers.EosConnectionTestInfo)_info.parameter).Result = false;
			}
			ThreadManager.AddSingleTaskMainThread("TestEosConnectionResult", new ThreadManager.MainThreadTaskFunctionDelegate(EosHelpers.<TestEosConnection>g__mainThreadSyncFunc|10_1), _info.parameter);
		}

		// Token: 0x0600DE4E RID: 56910 RVA: 0x004F1010 File Offset: 0x004EF210
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <TestEosConnection>g__mainThreadSyncFunc|10_1(object _parameter)
		{
			EosHelpers.EosConnectionTestInfo eosConnectionTestInfo = (EosHelpers.EosConnectionTestInfo)_parameter;
			eosConnectionTestInfo.Callback(eosConnectionTestInfo.Result);
		}

		// Token: 0x0400A7D0 RID: 42960
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<ExternalAccountType, EPlatformIdentifier> accountTypeMappings = new EnumDictionary<ExternalAccountType, EPlatformIdentifier>
		{
			{
				ExternalAccountType.Epic,
				EPlatformIdentifier.EGS
			},
			{
				ExternalAccountType.Psn,
				EPlatformIdentifier.PSN
			},
			{
				ExternalAccountType.Steam,
				EPlatformIdentifier.Steam
			},
			{
				ExternalAccountType.Xbl,
				EPlatformIdentifier.XBL
			}
		};

		// Token: 0x0400A7D1 RID: 42961
		public static readonly ReadOnlyDictionary<ExternalAccountType, EPlatformIdentifier> AccountTypeMappings = new ReadOnlyDictionary<ExternalAccountType, EPlatformIdentifier>(EosHelpers.accountTypeMappings);

		// Token: 0x0400A7D2 RID: 42962
		public static readonly ReadOnlyDictionary<EPlatformIdentifier, ExternalAccountType> PlatformIdentifierMappings;

		// Token: 0x0400A7D3 RID: 42963
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Dictionary<ClientInfo.EDeviceType, AntiCheatCommonClientPlatform> deviceTypeToAntiCheatPlatformMappings = new EnumDictionary<ClientInfo.EDeviceType, AntiCheatCommonClientPlatform>
		{
			{
				ClientInfo.EDeviceType.Unknown,
				AntiCheatCommonClientPlatform.Unknown
			},
			{
				ClientInfo.EDeviceType.Linux,
				AntiCheatCommonClientPlatform.Linux
			},
			{
				ClientInfo.EDeviceType.Mac,
				AntiCheatCommonClientPlatform.Mac
			},
			{
				ClientInfo.EDeviceType.Windows,
				AntiCheatCommonClientPlatform.Windows
			},
			{
				ClientInfo.EDeviceType.PlayStation,
				AntiCheatCommonClientPlatform.PlayStation
			},
			{
				ClientInfo.EDeviceType.Xbox,
				AntiCheatCommonClientPlatform.Xbox
			}
		};

		// Token: 0x0400A7D4 RID: 42964
		public static readonly ReadOnlyDictionary<ClientInfo.EDeviceType, AntiCheatCommonClientPlatform> DeviceTypeToAntiCheatPlatformMappings = new ReadOnlyDictionary<ClientInfo.EDeviceType, AntiCheatCommonClientPlatform>(EosHelpers.deviceTypeToAntiCheatPlatformMappings);

		// Token: 0x0400A7D5 RID: 42965
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosApiUrl = "https://api.epicgames.dev/sdk/v1/default";

		// Token: 0x0400A7D6 RID: 42966
		[PublicizedFrom(EAccessModifier.Private)]
		public const int eosApiTestTimeout = 5000;

		// Token: 0x0400A7D7 RID: 42967
		public static EUserAccountState UserAccountState = EUserAccountState.Unknown;

		// Token: 0x02001D50 RID: 7504
		[PublicizedFrom(EAccessModifier.Private)]
		public class EosConnectionTestInfo
		{
			// Token: 0x0400A7D8 RID: 42968
			public Action<bool> Callback;

			// Token: 0x0400A7D9 RID: 42969
			public bool Result;
		}
	}
}
