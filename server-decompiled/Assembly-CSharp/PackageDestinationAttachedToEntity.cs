using System;

// Token: 0x0200088E RID: 2190
public class PackageDestinationAttachedToEntity : IPackageDestinationFilter
{
	// Token: 0x06003F21 RID: 16161 RVA: 0x0018F23E File Offset: 0x0018D43E
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity;
	}
}
