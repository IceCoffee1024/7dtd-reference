using System;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B84 RID: 7044
	public abstract class AssetManager : IAssetManager
	{
		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x0600D307 RID: 54023
		public abstract int ActiveAssetCount { get; }

		// Token: 0x0600D308 RID: 54024 RVA: 0x004C8E44 File Offset: 0x004C7044
		public AssetHandle<T> AcquireAsset<T>(string uri, bool ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
		{
			LoadManager.AssetRequestTask<T> assetRequestTask = LoadManager.LoadAsset<T>(uri, null, null, false, true, ignoreDlcEntitlements);
			if (assetRequestTask.Asset == null)
			{
				return new AssetHandle<T>(this, uri, null);
			}
			return this.TrackAsset<T>(uri, assetRequestTask);
		}

		// Token: 0x0600D309 RID: 54025 RVA: 0x004C8E84 File Offset: 0x004C7084
		public AssetHandle<T> AcquireAssetAsync<T>(string uri, bool ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
		{
			LoadManager.AssetRequestTask<T> assetRequestTask = LoadManager.LoadAsset<T>(uri, null, null, false, false, ignoreDlcEntitlements);
			if (assetRequestTask.IsDone && assetRequestTask.Asset == null)
			{
				return new AssetHandle<T>(this, uri, null);
			}
			return this.TrackAsset<T>(uri, assetRequestTask);
		}

		// Token: 0x0600D30A RID: 54026
		[PublicizedFrom(EAccessModifier.Internal)]
		public abstract AssetHandle<T> TrackAsset<T>(string uri, LoadManager.AssetRequestTask<T> loadTask) where T : global::UnityEngine.Object;

		// Token: 0x0600D30B RID: 54027
		[PublicizedFrom(EAccessModifier.Internal)]
		public abstract InstanceHandle<T> TrackInstance<T>(string uri, T instance) where T : global::UnityEngine.Object;

		// Token: 0x0600D30C RID: 54028
		[PublicizedFrom(EAccessModifier.Internal)]
		public abstract void Release(string uri);

		// Token: 0x0600D30D RID: 54029
		public abstract void Cleanup();

		// Token: 0x0600D30E RID: 54030 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetManager()
		{
		}
	}
}
