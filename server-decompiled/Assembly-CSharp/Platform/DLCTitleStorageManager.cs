using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001B78 RID: 7032
	public class DLCTitleStorageManager
	{
		// Token: 0x17001A53 RID: 6739
		// (get) Token: 0x0600D2AA RID: 53930 RVA: 0x004C9FD0 File Offset: 0x004C81D0
		public static DLCTitleStorageManager Instance
		{
			get
			{
				DLCTitleStorageManager dlctitleStorageManager;
				if ((dlctitleStorageManager = DLCTitleStorageManager.instance) == null)
				{
					dlctitleStorageManager = (DLCTitleStorageManager.instance = new DLCTitleStorageManager());
				}
				return dlctitleStorageManager;
			}
		}

		// Token: 0x0600D2AB RID: 53931 RVA: 0x004C9FE6 File Offset: 0x004C81E6
		[PublicizedFrom(EAccessModifier.Private)]
		public DLCTitleStorageManager()
		{
		}

		// Token: 0x0600D2AC RID: 53932 RVA: 0x004C9FFC File Offset: 0x004C81FC
		public bool IsDLCPurchasable(EntitlementSetEnum _dlcSet, DLCEnvironmentFlags _dlcEnvironments)
		{
			DLCTitleStorageManager.CatalogEntry catalogEntry;
			return _dlcSet != EntitlementSetEnum.None && this.dlcPurchasability.TryGetValue(_dlcSet, out catalogEntry) && (catalogEntry.environments & _dlcEnvironments) != DLCEnvironmentFlags.None && (!_dlcEnvironments.HasFlag(DLCEnvironmentFlags.Retail) || catalogEntry.retailDate == null || !(catalogEntry.retailDate.Value > DateTime.Now));
		}

		// Token: 0x0600D2AD RID: 53933 RVA: 0x004CA068 File Offset: 0x004C8268
		public void FetchFromSource()
		{
			if (!this.fetchAttempted)
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				if (crossplatformPlatform != null && crossplatformPlatform.PlatformIdentifier == EPlatformIdentifier.EOS)
				{
					this.fetchAttempted = true;
					PlatformManager.NativePlatform.User.UserLoggedIn += delegate(IPlatform _)
					{
						ThreadManager.StartCoroutine(this.RequestDataCo());
					};
					return;
				}
			}
		}

		// Token: 0x0600D2AE RID: 53934 RVA: 0x004CA0B9 File Offset: 0x004C82B9
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator RequestDataCo()
		{
			DLCTitleStorageManager.<>c__DisplayClass10_0 CS$<>8__locals1 = new DLCTitleStorageManager.<>c__DisplayClass10_0();
			CS$<>8__locals1.<>4__this = this;
			IRemoteFileStorage storage = PlatformManager.MultiPlatform.RemoteFileStorage;
			if (storage == null)
			{
				Log.Warning("[DLCTitleStorageManager] No remote file storage implementation available.");
				yield break;
			}
			bool loggedSlow = false;
			float startTime = Time.time;
			while (!storage.IsReady)
			{
				if (storage.Unavailable)
				{
					Log.Warning("[DLCTitleStorageManager] Remote Storage is unavailable");
					yield break;
				}
				yield return null;
				if (!loggedSlow && Time.time > startTime + 30f)
				{
					loggedSlow = true;
					Log.Warning("[DLCTitleStorageManager] Waiting for DLC configuration from remote storage exceeded 30s");
				}
			}
			CS$<>8__locals1.fileDownloadComplete = false;
			storage.GetFile("DLCConfiguration", new IRemoteFileStorage.FileDownloadCompleteCallback(CS$<>8__locals1.<RequestDataCo>g__fileDownloadedCallback|0));
			while (!CS$<>8__locals1.fileDownloadComplete)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600D2AF RID: 53935 RVA: 0x004CA0C8 File Offset: 0x004C82C8
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetLocalPlatformNetworkString()
		{
			if ((DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX).IsCurrent())
			{
				if (PlatformManager.NativePlatform.PlatformIdentifier == EPlatformIdentifier.Steam)
				{
					return "Standalone_Steam";
				}
				if (PlatformManager.NativePlatform.PlatformIdentifier == EPlatformIdentifier.XBL)
				{
					return "Standalone_XBL";
				}
			}
			else
			{
				if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent())
				{
					return "XboxSeries_XBL";
				}
				if (DeviceFlag.PS5.IsCurrent())
				{
					return "PS5_PSN";
				}
			}
			return null;
		}

		// Token: 0x0400A0FA RID: 41210
		public const string RFSUri = "DLCConfiguration";

		// Token: 0x0400A0FB RID: 41211
		[PublicizedFrom(EAccessModifier.Private)]
		public static DLCTitleStorageManager instance;

		// Token: 0x0400A0FC RID: 41212
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<EntitlementSetEnum, DLCTitleStorageManager.CatalogEntry> dlcPurchasability = new Dictionary<EntitlementSetEnum, DLCTitleStorageManager.CatalogEntry>();

		// Token: 0x0400A0FD RID: 41213
		[PublicizedFrom(EAccessModifier.Private)]
		public bool fetchAttempted;

		// Token: 0x02001B79 RID: 7033
		public struct CatalogEntry
		{
			// Token: 0x0400A0FE RID: 41214
			public DLCEnvironmentFlags environments;

			// Token: 0x0400A0FF RID: 41215
			public DateTime? retailDate;
		}
	}
}
