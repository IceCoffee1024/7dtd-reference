using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014B0 RID: 5296
public class ReleaseAssetsOnDestroy : MonoBehaviour
{
	// Token: 0x0600A5E9 RID: 42473 RVA: 0x003DC9A2 File Offset: 0x003DABA2
	public void AddAssetHandle(LoadManager.IAssetHandle handle)
	{
		this.assetHandles.Add(handle);
	}

	// Token: 0x0600A5EA RID: 42474 RVA: 0x003DC9B0 File Offset: 0x003DABB0
	public void CopyTo(ReleaseAssetsOnDestroy other)
	{
		foreach (LoadManager.IAssetHandle assetHandle in other.assetHandles)
		{
			this.assetHandles.Add(assetHandle.Copy());
		}
	}

	// Token: 0x0600A5EB RID: 42475 RVA: 0x003DCA10 File Offset: 0x003DAC10
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		foreach (LoadManager.IAssetHandle assetHandle in this.assetHandles)
		{
			assetHandle.Release();
		}
		this.assetHandles.Clear();
	}

	// Token: 0x04007B96 RID: 31638
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<LoadManager.IAssetHandle> assetHandles = new List<LoadManager.IAssetHandle>();
}
