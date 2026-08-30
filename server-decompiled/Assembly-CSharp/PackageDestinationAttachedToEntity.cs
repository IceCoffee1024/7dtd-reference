using System;

// Token: 0x02000875 RID: 2165
public class PackageDestinationAttachedToEntity : IPackageDestinationFilter
{
	// Token: 0x06003F0B RID: 16139 RVA: 0x0018C2D4 File Offset: 0x0018A4D4
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity;
	}
}
