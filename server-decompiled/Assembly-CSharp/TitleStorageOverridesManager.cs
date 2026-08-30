using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x020012D8 RID: 4824
public class TitleStorageOverridesManager
{
	// Token: 0x170011F7 RID: 4599
	// (get) Token: 0x060098AC RID: 39084 RVA: 0x0039DBA9 File Offset: 0x0039BDA9
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
	// (add) Token: 0x060098AD RID: 39085 RVA: 0x0039DBC0 File Offset: 0x0039BDC0
	// (remove) Token: 0x060098AE RID: 39086 RVA: 0x0039DBF8 File Offset: 0x0039BDF8
	[method: PublicizedFrom(EAccessModifier.Private)]
	public event Action<TitleStorageOverridesManager.TSOverrides> fetchFinished;

	// Token: 0x060098AF RID: 39087 RVA: 0x0039DC30 File Offset: 0x0039BE30
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

	// Token: 0x060098B0 RID: 39088 RVA: 0x0039DCC8 File Offset: 0x0039BEC8
	public void ClearOverrides()
	{
		this.overrides = default(TitleStorageOverridesManager.TSOverrides);
	}

	// Token: 0x060098B1 RID: 39089 RVA: 0x0039DCD8 File Offset: 0x0039BED8
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

	// Token: 0x060098B2 RID: 39090 RVA: 0x0039DD40 File Offset: 0x0039BF40
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
			storage.GetFile("PlatformOverrides", new IRemoteFileStorage.FileDownloadCompleteCallback(CS$<>8__locals1.<RequestDataCo>g__fileDownloadedCallback|0));
			while (!CS$<>8__locals1.fileDownloadComplete)
			{
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

	// Token: 0x040072EE RID: 29422
	public const string RFSUri = "PlatformOverrides";

	// Token: 0x040072EF RID: 29423
	[PublicizedFrom(EAccessModifier.Private)]
	public static TitleStorageOverridesManager instance;

	// Token: 0x040072F0 RID: 29424
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime lastSuccess = DateTime.MinValue;

	// Token: 0x040072F1 RID: 29425
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object fetchLock = new object();

	// Token: 0x040072F2 RID: 29426
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fetching;

	// Token: 0x040072F4 RID: 29428
	[PublicizedFrom(EAccessModifier.Private)]
	public TitleStorageOverridesManager.TSOverrides overrides;

	// Token: 0x020012D9 RID: 4825
	public struct TSOverrides
	{
		// Token: 0x040072F5 RID: 29429
		public bool Crossplay;
	}
}
