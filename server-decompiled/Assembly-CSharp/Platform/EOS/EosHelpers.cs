using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.CompilerServices;
using Epic.OnlineServices;
using Epic.OnlineServices.AntiCheatCommon;

namespace Platform.EOS
{
	// Token: 0x02001D1E RID: 7454
	public static class EosHelpers
	{
		// Token: 0x0600DD34 RID: 56628 RVA: 0x004ED20C File Offset: 0x004EB40C
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

		// Token: 0x0600DD35 RID: 56629 RVA: 0x004ED304 File Offset: 0x004EB504
		public static void TestEosConnection(Action<bool> _callback)
		{
			ThreadManager.StartThread("TestEosConnection", new ThreadManager.ThreadFunctionDelegate(EosHelpers.<TestEosConnection>g__workerFunc|10_0), new EosHelpers.EosConnectionTestInfo
			{
				Callback = _callback
			}, null, false, true);
		}

		// Token: 0x0600DD36 RID: 56630 RVA: 0x004ED32C File Offset: 0x004EB52C
		public static void AssertMainThread(string _id)
		{
			if (!ThreadManager.IsMainThread())
			{
				Log.Warning("[EOSH] Called EOS code from secondary thread: " + _id);
			}
		}

		// Token: 0x0600DD37 RID: 56631 RVA: 0x004ED348 File Offset: 0x004EB548
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

		// Token: 0x0600DD38 RID: 56632 RVA: 0x004ED3A2 File Offset: 0x004EB5A2
		public static bool RequiresAntiCheat(this ClientInfo.EDeviceType deviceType)
		{
			return deviceType != ClientInfo.EDeviceType.PlayStation && deviceType != ClientInfo.EDeviceType.Xbox;
		}

		// Token: 0x0600DD39 RID: 56633 RVA: 0x004ED3B4 File Offset: 0x004EB5B4
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

		// Token: 0x0600DD3A RID: 56634 RVA: 0x004ED470 File Offset: 0x004EB670
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <TestEosConnection>g__mainThreadSyncFunc|10_1(object _parameter)
		{
			EosHelpers.EosConnectionTestInfo eosConnectionTestInfo = (EosHelpers.EosConnectionTestInfo)_parameter;
			eosConnectionTestInfo.Callback(eosConnectionTestInfo.Result);
		}

		// Token: 0x0400A6E7 RID: 42727
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

		// Token: 0x0400A6E8 RID: 42728
		public static readonly ReadOnlyDictionary<ExternalAccountType, EPlatformIdentifier> AccountTypeMappings = new ReadOnlyDictionary<ExternalAccountType, EPlatformIdentifier>(EosHelpers.accountTypeMappings);

		// Token: 0x0400A6E9 RID: 42729
		public static readonly ReadOnlyDictionary<EPlatformIdentifier, ExternalAccountType> PlatformIdentifierMappings;

		// Token: 0x0400A6EA RID: 42730
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

		// Token: 0x0400A6EB RID: 42731
		public static readonly ReadOnlyDictionary<ClientInfo.EDeviceType, AntiCheatCommonClientPlatform> DeviceTypeToAntiCheatPlatformMappings = new ReadOnlyDictionary<ClientInfo.EDeviceType, AntiCheatCommonClientPlatform>(EosHelpers.deviceTypeToAntiCheatPlatformMappings);

		// Token: 0x0400A6EC RID: 42732
		[PublicizedFrom(EAccessModifier.Private)]
		public const string eosApiUrl = "https://api.epicgames.dev/sdk/v1/default";

		// Token: 0x0400A6ED RID: 42733
		[PublicizedFrom(EAccessModifier.Private)]
		public const int eosApiTestTimeout = 5000;

		// Token: 0x0400A6EE RID: 42734
		public static EUserAccountState UserAccountState = EUserAccountState.Unknown;

		// Token: 0x02001D1F RID: 7455
		[PublicizedFrom(EAccessModifier.Private)]
		public class EosConnectionTestInfo
		{
			// Token: 0x0400A6EF RID: 42735
			public Action<bool> Callback;

			// Token: 0x0400A6F0 RID: 42736
			public bool Result;
		}
	}
}
