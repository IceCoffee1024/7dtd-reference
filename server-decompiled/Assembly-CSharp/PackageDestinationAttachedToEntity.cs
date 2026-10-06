using System;

// Token: 0x02000884 RID: 2180
public class PackageDestinationAttachedToEntity : IPackageDestinationFilter
{
	// Token: 0x06003EFE RID: 16126 RVA: 0x0018E196 File Offset: 0x0018C396
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity;
	}
}
