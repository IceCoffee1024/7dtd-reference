using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001B46 RID: 6982
	public class DLCTitleStorageManager
	{
		// Token: 0x170019AD RID: 6573
		// (get) Token: 0x0600D133 RID: 53555 RVA: 0x004C54E8 File Offset: 0x004C36E8
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

		// Token: 0x0600D134 RID: 53556 RVA: 0x004C54FE File Offset: 0x004C36FE
		[PublicizedFrom(EAccessModifier.Private)]
		public DLCTitleStorageManager()
		{
		}

		// Token: 0x0600D135 RID: 53557 RVA: 0x004C5514 File Offset: 0x004C3714
		public bool IsDLCPurchasable(EntitlementSetEnum _dlcSet, DLCEnvironmentFlags _dlcEnvironments)
		{
			DLCTitleStorageManager.CatalogEntry catalogEntry;
			return _dlcSet != EntitlementSetEnum.None && this.dlcPurchasability.TryGetValue(_dlcSet, out catalogEntry) && (catalogEntry.environments & _dlcEnvironments) != DLCEnvironmentFlags.None && (!_dlcEnvironments.HasFlag(DLCEnvironmentFlags.Retail) || catalogEntry.retailDate == null || !(catalogEntry.retailDate.Value > DateTime.Now));
		}

		// Token: 0x0600D136 RID: 53558 RVA: 0x004C5580 File Offset: 0x004C3780
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

		// Token: 0x0600D137 RID: 53559 RVA: 0x004C55D1 File Offset: 0x004C37D1
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

		// Token: 0x0600D138 RID: 53560 RVA: 0x004C55E0 File Offset: 0x004C37E0
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

		// Token: 0x0400A025 RID: 40997
		public const string RFSUri = "DLCConfiguration";

		// Token: 0x0400A026 RID: 40998
		[PublicizedFrom(EAccessModifier.Private)]
		public static DLCTitleStorageManager instance;

		// Token: 0x0400A027 RID: 40999
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<EntitlementSetEnum, DLCTitleStorageManager.CatalogEntry> dlcPurchasability = new Dictionary<EntitlementSetEnum, DLCTitleStorageManager.CatalogEntry>();

		// Token: 0x0400A028 RID: 41000
		[PublicizedFrom(EAccessModifier.Private)]
		public bool fetchAttempted;

		// Token: 0x02001B47 RID: 6983
		public struct CatalogEntry
		{
			// Token: 0x0400A029 RID: 41001
			public DLCEnvironmentFlags environments;

			// Token: 0x0400A02A RID: 41002
			public DateTime? retailDate;
		}
	}
}
