using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014C2 RID: 5314
public class ReleaseAssetsOnDestroy : MonoBehaviour
{
	// Token: 0x0600A641 RID: 42561 RVA: 0x003DEDA6 File Offset: 0x003DCFA6
	public void AddAssetHandle(LoadManager.IAssetHandle handle)
	{
		this.assetHandles.Add(handle);
	}

	// Token: 0x0600A642 RID: 42562 RVA: 0x003DEDB4 File Offset: 0x003DCFB4
	public void CopyTo(ReleaseAssetsOnDestroy other)
	{
		foreach (LoadManager.IAssetHandle assetHandle in other.assetHandles)
		{
			this.assetHandles.Add(assetHandle.Copy());
		}
	}

	// Token: 0x0600A643 RID: 42563 RVA: 0x003DEE14 File Offset: 0x003DD014
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		foreach (LoadManager.IAssetHandle assetHandle in this.assetHandles)
		{
			assetHandle.Release();
		}
		this.assetHandles.Clear();
	}

	// Token: 0x04007BD7 RID: 31703
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<LoadManager.IAssetHandle> assetHandles = new List<LoadManager.IAssetHandle>();
}
