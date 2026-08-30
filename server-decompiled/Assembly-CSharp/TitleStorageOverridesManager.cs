using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x020012F2 RID: 4850
public class TitleStorageOverridesManager
{
	// Token: 0x17001230 RID: 4656
	// (get) Token: 0x060099A7 RID: 39335 RVA: 0x003A2DF9 File Offset: 0x003A0FF9
	public static TitleStorageOverridesManager Instance
	{
		get
		{
			TitleStorageOverridesManager titleStorageOverridesManager;
			if ((titleStorageOverridesManager = TitleStorageOverridesManager.instance) == null)
			{
				titleStorageOverridesManager = (TitleStorageOverridesManager.instance = new TitleStorageOverridesManager());
			}
			return titleStorageOverridesManager;
		}
	}

	// Token: 0x14000101 RID: 257
	// (add) Token: 0x060099A8 RID: 39336 RVA: 0x003A2E10 File Offset: 0x003A1010
	// (remove) Token: 0x060099A9 RID: 39337 RVA: 0x003A2E48 File Offset: 0x003A1048
	[method: PublicizedFrom(EAccessModifier.Private)]
	public event Action<TitleStorageOverridesManager.TSOverrides> fetchFinished;

	// Token: 0x060099AA RID: 39338 RVA: 0x003A2E80 File Offset: 0x003A1080
	public void FetchFromSource(Action<TitleStorageOverridesManager.TSOverrides> _callback)
	{
		IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
		if (crossplatformPlatform == null || crossplatformPlatform.PlatformIdentifier != EPlatformIdentifier.EOS)
		{
			if (_callback != null)
			{
				_callback(new TitleStorageOverridesManager.TSOverrides
				{
					Crossplay = true
				});
			}
			return;
		}
		bool flag = false;
		object obj = this.fetchLock;
		lock (obj)
		{
			if (!this.fetching)
			{
				flag = true;
				this.fetching = true;
			}
			this.fetchFinished += _callback;
		}
		if (flag)
		{
			ThreadManager.StartCoroutine(this.RequestDataCo());
		}
	}

	// Token: 0x060099AB RID: 39339 RVA: 0x003A2F18 File Offset: 0x003A1118
	public void ClearOverrides()
	{
		this.overrides = new TitleStorageOverridesManager.TSOverrides
		{
			Crossplay = true
		};
	}

	// Token: 0x060099AC RID: 39340 RVA: 0x003A2F3C File Offset: 0x003A113C
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
			if (DeviceFlag.XBoxSeriesX.IsCurrent())
			{
				return "XboxSeriesX_XBL";
			}
			if (DeviceFlag.XBoxSeriesS.IsCurrent())
			{
				return "XboxSeriesS_XBL";
			}
			if (DeviceFlag.PS5.IsCurrent())
			{
				return "PS5_PSN";
			}
		}
		return null;
	}

	// Token: 0x060099AD RID: 39341 RVA: 0x003A2FA4 File Offset: 0x003A11A4
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator RequestDataCo()
	{
		TitleStorageOverridesManager.<>c__DisplayClass15_0 CS$<>8__locals1 = new TitleStorageOverridesManager.<>c__DisplayClass15_0();
		CS$<>8__locals1.<>4__this = this;
		try
		{
			if ((DateTime.Now - this.lastSuccess).TotalMinutes < 5.0)
			{
				Log.Out("[TitleStorageOverridesManager] Using cached last success.");
				yield break;
			}
			IRemoteFileStorage storage = PlatformManager.MultiPlatform.RemoteFileStorage;
			if (storage == null)
			{
				Log.Warning("[TitleStorageOverridesManager] No remote file storage implementation available.");
				yield break;
			}
			bool loggedSlow = false;
			float startTime = Time.time;
			while (!storage.IsReady)
			{
				if (storage.Unavailable)
				{
					Log.Warning("[TitleStorageOverridesManager] Remote Storage is unavailable");
					this.ClearOverrides();
					yield break;
				}
				yield return null;
				if (!loggedSlow && Time.time > startTime + 30f)
				{
					loggedSlow = true;
					Log.Warning("[TitleStorageOverridesManager] Waiting for title storage overrides from remote storage exceeded 30s");
				}
			}
			CS$<>8__locals1.fileDownloadComplete = false;
			float fileDownloadStart = Time.time;
			storage.GetFile("PlatformOverrides", new IRemoteFileStorage.FileDownloadCompleteCallback(CS$<>8__locals1.<RequestDataCo>g__fileDownloadedCallback|0));
			while (!CS$<>8__locals1.fileDownloadComplete)
			{
				if (Time.time - fileDownloadStart > 5f)
				{
					storage.CancelGetFile("PlatformOverrides", "Timed Out");
				}
				yield return null;
			}
			storage = null;
		}
		finally
		{
			object obj = this.fetchLock;
			lock (obj)
			{
				Action<TitleStorageOverridesManager.TSOverrides> action = this.fetchFinished;
				if (action != null)
				{
					action(this.overrides);
				}
				this.fetchFinished = null;
				this.fetching = false;
			}
		}
		yield break;
		yield break;
	}

	// Token: 0x040073C5 RID: 29637
	public const string RFSUri = "PlatformOverrides";

	// Token: 0x040073C6 RID: 29638
	[PublicizedFrom(EAccessModifier.Private)]
	public static TitleStorageOverridesManager instance;

	// Token: 0x040073C7 RID: 29639
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime lastSuccess = DateTime.MinValue;

	// Token: 0x040073C8 RID: 29640
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object fetchLock = new object();

	// Token: 0x040073C9 RID: 29641
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fetching;

	// Token: 0x040073CB RID: 29643
	[PublicizedFrom(EAccessModifier.Private)]
	public TitleStorageOverridesManager.TSOverrides overrides;

	// Token: 0x020012F3 RID: 4851
	public struct TSOverrides
	{
		// Token: 0x040073CC RID: 29644
		public bool Crossplay;
	}
}
