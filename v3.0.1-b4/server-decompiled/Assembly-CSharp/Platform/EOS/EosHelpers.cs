using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Platform.EOS
{
	// Token: 0x02001CC5 RID: 7365
	public static class EosHelpers
	{
		// Token: 0x0600DA1E RID: 55838 RVA: 0x004E20B0 File Offset: 0x004E02B0
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

		// Token: 0x0600DA1F RID: 55839 RVA: 0x004E21A8 File Offset: 0x004E03A8
		public static void TestEosConnection(Action<bool> _callback)
		{
			ThreadManager.StartThread("TestEosConnection", new ThreadManager.ThreadFunctionDelegate(EosHelpers.<TestEosConnection>g__workerFunc|10_0), new EosHelpers.EosConnectionTestInfo
			{
				Callback = _callback
			}, null, false, true);
		}

		// Token: 0x0600DA20 RID: 55840 RVA: 0x004E21D0 File Offset: 0x004E03D0
		public static void AssertMainThread(string _id)
		{
			if (!ThreadManager.IsMainThread())
			{
				Log.Warning("[EOSH] Called EOS code from secondary thread: " + _id);
			}
		}

		// Token: 0x0600DA21 RID: 55841 RVA: 0x004E21EC File Offset: 0x004E03EC
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

		// Token: 0x0600DA22 RID: 55842 RVA: 0x004E2246 File Offset: 0x004E0446
		public static bool RequiresAntiCheat(this ClientInfo.EDeviceType deviceType)
		{
			return deviceType != ClientInfo.EDeviceType.PlayStation && deviceType != ClientInfo.EDeviceType.Xbox;
		}

		// Token: 0x0600DA23 RID: 55843 RVA: 0x004E2258 File Offset: 0x004E0458
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

		// Token: 0x0600DA24 RID: 55844 RVA: 0x004E2314 File Offset: 0x004E0514
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <TestEosConnection>g__mainThreadSyncFunc|10_1(object _parameter)
		{
			EosHelpers.EosConnectionTestInfo eosConnectionTestInfo = (EosHelpers.EosConnectionTestInfo)_parameter;
			eosConnectionTestInfo.Callback(eosConnectionTestInfo.Result);
		}

		// Token: 0x0400A4DF RID: 42207
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

		// Token: 0x0400A4E0 RID: 42208
		public static readonly ReadOnlyDictionary<ExternalAccountType, EPlatformIdentifier> AccountTypeMappings = new ReadOnlyDictionary<ExternalAccountType, EPlatformIdentifier>(EosHelpers.accountTypeMappings);

		// Token: 0x0400A4E1 RID: 42209
		public static readonly ReadOnlyDictionary<EPlatformIdentifier, ExternalAccountType> PlatformIdentifierMappings;

		// Token: 0x0400A4E2 RID: 42210
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

		// Token: 0x0400A4E3 RID: 42211
		public static readonly ReadOnlyDictionary<ClientInfo.EDeviceType, AntiCheatCommonClientPlatform> DeviceTypeToAntiCheatPlatformMappings = new ReadOnlyDictionary<ClientInfo.EDeviceType, AntiCheatCommonClientPlatform>(EosHelpers.deviceTypeToAntiCheatPlatformMappings);

		// Token: 0x0400A4E4 RID: 42212
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosApiUrl = "https://api.epicgames.dev/sdk/v1/default";

		// Token: 0x0400A4E5 RID: 42213
		[PublicizedFrom(EAccessModifier.Private)]
		public const int eosApiTestTimeout = 5000;

		// Token: 0x0400A4E6 RID: 42214
		public static EUserAccountState UserAccountState = EUserAccountState.Unknown;

		// Token: 0x02001CC6 RID: 7366
		[PublicizedFrom(EAccessModifier.Private)]
		public class EosConnectionTestInfo
		{
			// Token: 0x0400A4E7 RID: 42215
			public Action<bool> Callback;

			// Token: 0x0400A4E8 RID: 42216
			public bool Result;
		}
	}
}
