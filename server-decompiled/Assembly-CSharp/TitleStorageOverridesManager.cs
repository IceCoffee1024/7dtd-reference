using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x02001324 RID: 4900
public class TitleStorageOverridesManager
{
	// Token: 0x17001281 RID: 4737
	// (get) Token: 0x06009AAB RID: 39595 RVA: 0x003A9DAD File Offset: 0x003A7FAD
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

	// Token: 0x14000100 RID: 256
	// (add) Token: 0x06009AAC RID: 39596 RVA: 0x003A9DC4 File Offset: 0x003A7FC4
	// (remove) Token: 0x06009AAD RID: 39597 RVA: 0x003A9DFC File Offset: 0x003A7FFC
	[method: PublicizedFrom(EAccessModifier.Private)]
	public event Action<TitleStorageOverridesManager.TSOverrides> fetchFinished;

	// Token: 0x06009AAE RID: 39598 RVA: 0x003A9E34 File Offset: 0x003A8034
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

	// Token: 0x06009AAF RID: 39599 RVA: 0x003A9ECC File Offset: 0x003A80CC
	public void ClearOverrides()
	{
		this.overrides = new TitleStorageOverridesManager.TSOverrides
		{
			Crossplay = true
		};
	}

	// Token: 0x06009AB0 RID: 39600 RVA: 0x003A9EF0 File Offset: 0x003A80F0
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

	// Token: 0x06009AB1 RID: 39601 RVA: 0x003A9F58 File Offset: 0x003A8158
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

	// Token: 0x04007481 RID: 29825
	public const string RFSUri = "PlatformOverrides";

	// Token: 0x04007482 RID: 29826
	[PublicizedFrom(EAccessModifier.Private)]
	public static TitleStorageOverridesManager instance;

	// Token: 0x04007483 RID: 29827
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime lastSuccess = DateTime.MinValue;

	// Token: 0x04007484 RID: 29828
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object fetchLock = new object();

	// Token: 0x04007485 RID: 29829
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fetching;

	// Token: 0x04007487 RID: 29831
	[PublicizedFrom(EAccessModifier.Private)]
	public TitleStorageOverridesManager.TSOverrides overrides;

	// Token: 0x02001325 RID: 4901
	public struct TSOverrides
	{
		// Token: 0x04007488 RID: 29832
		public bool Crossplay;
	}
}
