using System;
using AssetManagement;

// Token: 0x020000AE RID: 174
public static class AssetManagers
{
	// Token: 0x06000351 RID: 849 RVA: 0x00018780 File Offset: 0x00016980
	public static void Initialize(AssetLoaderManager managerOfManagers)
	{
		AssetManagerLifetimed assetManagerLifetimed = new AssetManagerLifetimed(0, 5.0, int.MaxValue);
		managerOfManagers.Add(assetManagerLifetimed);
		AssetManagers.Items = assetManagerLifetimed;
	}

	// Token: 0x040003C8 RID: 968
	public static IAssetManager Items;
}
