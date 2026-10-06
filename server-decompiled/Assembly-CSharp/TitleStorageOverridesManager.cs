using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x02001312 RID: 4882
public class TitleStorageOverridesManager
{
	// Token: 0x17001275 RID: 4725
	// (get) Token: 0x06009A53 RID: 39507 RVA: 0x003A79A9 File Offset: 0x003A5BA9
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
	// (add) Token: 0x06009A54 RID: 39508 RVA: 0x003A79C0 File Offset: 0x003A5BC0
	// (remove) Token: 0x06009A55 RID: 39509 RVA: 0x003A79F8 File Offset: 0x003A5BF8
	[method: PublicizedFrom(EAccessModifier.Private)]
	public event Action<TitleStorageOverridesManager.TSOverrides> fetchFinished;

	// Token: 0x06009A56 RID: 39510 RVA: 0x003A7A30 File Offset: 0x003A5C30
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

	// Token: 0x06009A57 RID: 39511 RVA: 0x003A7AC8 File Offset: 0x003A5CC8
	public void ClearOverrides()
	{
		this.overrides = new TitleStorageOverridesManager.TSOverrides
		{
			Crossplay = true
		};
	}

	// Token: 0x06009A58 RID: 39512 RVA: 0x003A7AEC File Offset: 0x003A5CEC
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

	// Token: 0x06009A59 RID: 39513 RVA: 0x003A7B54 File Offset: 0x003A5D54
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

	// Token: 0x04007440 RID: 29760
	public const string RFSUri = "PlatformOverrides";

	// Token: 0x04007441 RID: 29761
	[PublicizedFrom(EAccessModifier.Private)]
	public static TitleStorageOverridesManager instance;

	// Token: 0x04007442 RID: 29762
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime lastSuccess = DateTime.MinValue;

	// Token: 0x04007443 RID: 29763
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object fetchLock = new object();

	// Token: 0x04007444 RID: 29764
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fetching;

	// Token: 0x04007446 RID: 29766
	[PublicizedFrom(EAccessModifier.Private)]
	public TitleStorageOverridesManager.TSOverrides overrides;

	// Token: 0x02001313 RID: 4883
	public struct TSOverrides
	{
		// Token: 0x04007447 RID: 29767
		public bool Crossplay;
	}
}
