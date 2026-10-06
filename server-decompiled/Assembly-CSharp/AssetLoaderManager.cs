using System;
using System.Collections.Generic;
using AssetManagement;

// Token: 0x020000AF RID: 175
public class AssetLoaderManager
{
	// Token: 0x06000352 RID: 850 RVA: 0x000187AF File Offset: 0x000169AF
	public void Add(AssetManagerLifetimed manager)
	{
		this.managersToUpdate.Add(manager);
		this.managersToCleanup.Add(manager);
	}

	// Token: 0x06000353 RID: 851 RVA: 0x000187C9 File Offset: 0x000169C9
	public void Add(AssetManager manager)
	{
		this.managersToCleanup.Add(manager);
	}

	// Token: 0x06000354 RID: 852 RVA: 0x000187D8 File Offset: 0x000169D8
	public void Update()
	{
		foreach (AssetManagerLifetimed assetManagerLifetimed in this.managersToUpdate)
		{
			assetManagerLifetimed.Update();
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x00018828 File Offset: 0x00016A28
	public void Cleanup()
	{
		foreach (AssetManager assetManager in this.managersToCleanup)
		{
			assetManager.Cleanup();
		}
		this.managersToCleanup.Clear();
		this.managersToUpdate.Clear();
	}

	// Token: 0x040003C9 RID: 969
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AssetManager> managersToCleanup = new List<AssetManager>();

	// Token: 0x040003CA RID: 970
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AssetManagerLifetimed> managersToUpdate = new List<AssetManagerLifetimed>();
}
