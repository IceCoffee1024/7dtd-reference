using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001BA9 RID: 7081
	public class DLCTitleStorageManager
	{
		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x0600D3BE RID: 54206 RVA: 0x004CDAF8 File Offset: 0x004CBCF8
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

		// Token: 0x0600D3BF RID: 54207 RVA: 0x004CDB0E File Offset: 0x004CBD0E
		[PublicizedFrom(EAccessModifier.Private)]
		public DLCTitleStorageManager()
		{
		}

		// Token: 0x0600D3C0 RID: 54208 RVA: 0x004CDB24 File Offset: 0x004CBD24
		public bool IsDLCPurchasable(EntitlementSetEnum _dlcSet, DLCEnvironmentFlags _dlcEnvironments)
		{
			DLCTitleStorageManager.CatalogEntry catalogEntry;
			return _dlcSet != EntitlementSetEnum.None && this.dlcPurchasability.TryGetValue(_dlcSet, out catalogEntry) && (catalogEntry.environments & _dlcEnvironments) != DLCEnvironmentFlags.None && (!_dlcEnvironments.HasFlag(DLCEnvironmentFlags.Retail) || catalogEntry.retailDate == null || !(catalogEntry.retailDate.Value > DateTime.Now));
		}

		// Token: 0x0600D3C1 RID: 54209 RVA: 0x004CDB90 File Offset: 0x004CBD90
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

		// Token: 0x0600D3C2 RID: 54210 RVA: 0x004CDBE1 File Offset: 0x004CBDE1
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

		// Token: 0x0600D3C3 RID: 54211 RVA: 0x004CDBF0 File Offset: 0x004CBDF0
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

		// Token: 0x0400A1E3 RID: 41443
		public const string RFSUri = "DLCConfiguration";

		// Token: 0x0400A1E4 RID: 41444
		[PublicizedFrom(EAccessModifier.Private)]
		public static DLCTitleStorageManager instance;

		// Token: 0x0400A1E5 RID: 41445
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<EntitlementSetEnum, DLCTitleStorageManager.CatalogEntry> dlcPurchasability = new Dictionary<EntitlementSetEnum, DLCTitleStorageManager.CatalogEntry>();

		// Token: 0x0400A1E6 RID: 41446
		[PublicizedFrom(EAccessModifier.Private)]
		public bool fetchAttempted;

		// Token: 0x02001BAA RID: 7082
		public struct CatalogEntry
		{
			// Token: 0x0400A1E7 RID: 41447
			public DLCEnvironmentFlags environments;

			// Token: 0x0400A1E8 RID: 41448
			public DateTime? retailDate;
		}
	}
}
