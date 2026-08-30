using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001490 RID: 5264
public class ReleaseAssetsOnDestroy : MonoBehaviour
{
	// Token: 0x0600A53B RID: 42299 RVA: 0x003D7D16 File Offset: 0x003D5F16
	public void AddAssetHandle(LoadManager.IAssetHandle handle)
	{
		this.assetHandles.Add(handle);
	}

	// Token: 0x0600A53C RID: 42300 RVA: 0x003D7D24 File Offset: 0x003D5F24
	public void CopyTo(ReleaseAssetsOnDestroy other)
	{
		foreach (LoadManager.IAssetHandle assetHandle in other.assetHandles)
		{
			this.assetHandles.Add(assetHandle.Copy());
		}
	}

	// Token: 0x0600A53D RID: 42301 RVA: 0x003D7D84 File Offset: 0x003D5F84
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		foreach (LoadManager.IAssetHandle assetHandle in this.assetHandles)
		{
			assetHandle.Release();
		}
		this.assetHandles.Clear();
	}

	// Token: 0x04007B1B RID: 31515
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<LoadManager.IAssetHandle> assetHandles = new List<LoadManager.IAssetHandle>();
}
