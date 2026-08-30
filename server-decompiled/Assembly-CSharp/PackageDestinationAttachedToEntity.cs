using System;

// Token: 0x02000881 RID: 2177
public class PackageDestinationAttachedToEntity : IPackageDestinationFilter
{
	// Token: 0x06003F70 RID: 16240 RVA: 0x0018E3A4 File Offset: 0x0018C5A4
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity;
	}
}
