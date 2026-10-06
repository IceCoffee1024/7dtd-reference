using System;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B54 RID: 6996
	public class AssetHandle<T> where T : global::UnityEngine.Object
	{
		// Token: 0x17001A3C RID: 6716
		// (get) Token: 0x0600D1FB RID: 53755 RVA: 0x004C53A4 File Offset: 0x004C35A4
		public T Asset
		{
			get
			{
				LoadManager.AssetRequestTask<T> assetRequestTask = this.loadHandle;
				if (assetRequestTask == null)
				{
					return default(T);
				}
				return assetRequestTask.Asset;
			}
		}

		// Token: 0x0600D1FC RID: 53756 RVA: 0x004C53CA File Offset: 0x004C35CA
		public bool IsValid()
		{
			return this.loadHandle != null;
		}

		// Token: 0x0600D1FD RID: 53757 RVA: 0x004C53D5 File Offset: 0x004C35D5
		public bool IsComplete()
		{
			LoadManager.AssetRequestTask<T> assetRequestTask = this.loadHandle;
			return assetRequestTask == null || assetRequestTask.IsDone;
		}

		// Token: 0x0600D1FE RID: 53758 RVA: 0x004C53E8 File Offset: 0x004C35E8
		public AssetHandle(AssetManager manager, string uri, LoadManager.AssetRequestTask<T> loadHandle)
		{
			this.manager = manager;
			this.Uri = uri;
			this.loadHandle = loadHandle;
		}

		// Token: 0x0600D1FF RID: 53759 RVA: 0x004C5405 File Offset: 0x004C3605
		public void WaitForComplete()
		{
			if (!this.IsValid())
			{
				return;
			}
			this.loadHandle.WaitForComplete();
		}

		// Token: 0x0600D200 RID: 53760 RVA: 0x004C541C File Offset: 0x004C361C
		public InstanceHandle<T> CreateInstance()
		{
			if (!this.IsValid())
			{
				throw new Exception("Cannot create instance for " + this.Uri + ", load operation is not valid");
			}
			if (!this.loadHandle.IsDone)
			{
				throw new Exception("Cannot create instance for " + this.Uri + ", load operation still in progress");
			}
			return this.manager.TrackInstance<T>(this.Uri, global::UnityEngine.Object.Instantiate<T>(this.Asset));
		}

		// Token: 0x0600D201 RID: 53761 RVA: 0x004C5490 File Offset: 0x004C3690
		public void Release()
		{
			if (this.loadHandle == null)
			{
				Log.Error("Trying to release an invalid AssetHandle for " + this.Uri);
				return;
			}
			this.manager.Release(this.Uri);
			this.loadHandle.Release();
			this.loadHandle = null;
		}

		// Token: 0x04009FB0 RID: 40880
		public readonly string Uri;

		// Token: 0x04009FB1 RID: 40881
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetManager manager;

		// Token: 0x04009FB2 RID: 40882
		[PublicizedFrom(EAccessModifier.Private)]
		public LoadManager.AssetRequestTask<T> loadHandle;
	}
}
