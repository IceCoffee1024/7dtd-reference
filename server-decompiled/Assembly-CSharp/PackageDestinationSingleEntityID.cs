using System;

// Token: 0x02000882 RID: 2178
public class PackageDestinationSingleEntityID : IPackageDestinationFilter
{
	// Token: 0x06003EF9 RID: 16121 RVA: 0x0018E141 File Offset: 0x0018C341
	public PackageDestinationSingleEntityID(int _entityId)
	{
		this.entityId = _entityId;
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x0018E150 File Offset: 0x0018C350
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity || _cInfo.entityId != this.entityId;
	}

	// Token: 0x04003329 RID: 13097
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
