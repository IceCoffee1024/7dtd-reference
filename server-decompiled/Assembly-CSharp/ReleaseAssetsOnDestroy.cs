using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001475 RID: 5237
public class ReleaseAssetsOnDestroy : MonoBehaviour
{
	// Token: 0x0600A43E RID: 42046 RVA: 0x003D29D2 File Offset: 0x003D0BD2
	public void AddAssetHandle(LoadManager.IAssetHandle handle)
	{
		this.assetHandles.Add(handle);
	}

	// Token: 0x0600A43F RID: 42047 RVA: 0x003D29E0 File Offset: 0x003D0BE0
	public void CopyTo(ReleaseAssetsOnDestroy other)
	{
		foreach (LoadManager.IAssetHandle assetHandle in other.assetHandles)
		{
			this.assetHandles.Add(assetHandle.Copy());
		}
	}

	// Token: 0x0600A440 RID: 42048 RVA: 0x003D2A40 File Offset: 0x003D0C40
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		foreach (LoadManager.IAssetHandle assetHandle in this.assetHandles)
		{
			assetHandle.Release();
		}
		this.assetHandles.Clear();
	}

	// Token: 0x04007A42 RID: 31298
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<LoadManager.IAssetHandle> assetHandles = new List<LoadManager.IAssetHandle>();
}
