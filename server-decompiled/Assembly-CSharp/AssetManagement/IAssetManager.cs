using System;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B83 RID: 7043
	public interface IAssetManager
	{
		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x0600D303 RID: 54019
		int ActiveAssetCount { get; }

		// Token: 0x0600D304 RID: 54020
		AssetHandle<T> AcquireAsset<T>(string uri, bool ignoreDlcEntitlements = false) where T : global::UnityEngine.Object;

		// Token: 0x0600D305 RID: 54021
		AssetHandle<T> AcquireAssetAsync<T>(string uri, bool ignoreDlcEntitlements = false) where T : global::UnityEngine.Object;

		// Token: 0x0600D306 RID: 54022
		void Cleanup();
	}
}
