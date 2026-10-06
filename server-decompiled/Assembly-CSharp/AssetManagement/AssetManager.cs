using System;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B53 RID: 6995
	public abstract class AssetManager : IAssetManager
	{
		// Token: 0x17001A3B RID: 6715
		// (get) Token: 0x0600D1F3 RID: 53747
		public abstract int ActiveAssetCount { get; }

		// Token: 0x0600D1F4 RID: 53748 RVA: 0x004C531C File Offset: 0x004C351C
		public AssetHandle<T> AcquireAsset<T>(string uri, bool ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
		{
			LoadManager.AssetRequestTask<T> assetRequestTask = LoadManager.LoadAsset<T>(uri, null, null, false, true, ignoreDlcEntitlements);
			if (assetRequestTask.Asset == null)
			{
				return new AssetHandle<T>(this, uri, null);
			}
			return this.TrackAsset<T>(uri, assetRequestTask);
		}

		// Token: 0x0600D1F5 RID: 53749 RVA: 0x004C535C File Offset: 0x004C355C
		public AssetHandle<T> AcquireAssetAsync<T>(string uri, bool ignoreDlcEntitlements = false) where T : global::UnityEngine.Object
		{
			LoadManager.AssetRequestTask<T> assetRequestTask = LoadManager.LoadAsset<T>(uri, null, null, false, false, ignoreDlcEntitlements);
			if (assetRequestTask.IsDone && assetRequestTask.Asset == null)
			{
				return new AssetHandle<T>(this, uri, null);
			}
			return this.TrackAsset<T>(uri, assetRequestTask);
		}

		// Token: 0x0600D1F6 RID: 53750
		[PublicizedFrom(EAccessModifier.Internal)]
		public abstract AssetHandle<T> TrackAsset<T>(string uri, LoadManager.AssetRequestTask<T> loadTask) where T : global::UnityEngine.Object;

		// Token: 0x0600D1F7 RID: 53751
		[PublicizedFrom(EAccessModifier.Internal)]
		public abstract InstanceHandle<T> TrackInstance<T>(string uri, T instance) where T : global::UnityEngine.Object;

		// Token: 0x0600D1F8 RID: 53752
		[PublicizedFrom(EAccessModifier.Internal)]
		public abstract void Release(string uri);

		// Token: 0x0600D1F9 RID: 53753
		public abstract void Cleanup();

		// Token: 0x0600D1FA RID: 53754 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public AssetManager()
		{
		}
	}
}
