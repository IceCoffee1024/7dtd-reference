using System;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B85 RID: 7045
	public class AssetHandle<T> where T : global::UnityEngine.Object
	{
		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x0600D30F RID: 54031 RVA: 0x004C8ECC File Offset: 0x004C70CC
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

		// Token: 0x0600D310 RID: 54032 RVA: 0x004C8EF2 File Offset: 0x004C70F2
		public bool IsValid()
		{
			return this.loadHandle != null;
		}

		// Token: 0x0600D311 RID: 54033 RVA: 0x004C8EFD File Offset: 0x004C70FD
		public bool IsComplete()
		{
			LoadManager.AssetRequestTask<T> assetRequestTask = this.loadHandle;
			return assetRequestTask == null || assetRequestTask.IsDone;
		}

		// Token: 0x0600D312 RID: 54034 RVA: 0x004C8F10 File Offset: 0x004C7110
		public AssetHandle(AssetManager manager, string uri, LoadManager.AssetRequestTask<T> loadHandle)
		{
			this.manager = manager;
			this.Uri = uri;
			this.loadHandle = loadHandle;
		}

		// Token: 0x0600D313 RID: 54035 RVA: 0x004C8F2D File Offset: 0x004C712D
		public void WaitForComplete()
		{
			if (!this.IsValid())
			{
				return;
			}
			this.loadHandle.WaitForComplete();
		}

		// Token: 0x0600D314 RID: 54036 RVA: 0x004C8F44 File Offset: 0x004C7144
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

		// Token: 0x0600D315 RID: 54037 RVA: 0x004C8FB8 File Offset: 0x004C71B8
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

		// Token: 0x0400A099 RID: 41113
		public readonly string Uri;

		// Token: 0x0400A09A RID: 41114
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly AssetManager manager;

		// Token: 0x0400A09B RID: 41115
		[PublicizedFrom(EAccessModifier.Private)]
		public LoadManager.AssetRequestTask<T> loadHandle;
	}
}
