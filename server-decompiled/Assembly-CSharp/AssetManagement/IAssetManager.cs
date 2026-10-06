using System;
using UnityEngine;

namespace AssetManagement
{
	// Token: 0x02001B52 RID: 6994
	public interface IAssetManager
	{
		// Token: 0x17001A3A RID: 6714
		// (get) Token: 0x0600D1EF RID: 53743
		int ActiveAssetCount { get; }

		// Token: 0x0600D1F0 RID: 53744
		AssetHandle<T> AcquireAsset<T>(string uri, bool ignoreDlcEntitlements = false) where T : global::UnityEngine.Object;

		// Token: 0x0600D1F1 RID: 53745
		AssetHandle<T> AcquireAssetAsync<T>(string uri, bool ignoreDlcEntitlements = false) where T : global::UnityEngine.Object;

		// Token: 0x0600D1F2 RID: 53746
		void Cleanup();
	}
}
