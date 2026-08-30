using System;

// Token: 0x02000874 RID: 2164
public class PackageDestinationAllButEntityID : IPackageDestinationFilter
{
	// Token: 0x06003F08 RID: 16136 RVA: 0x0018C2AB File Offset: 0x0018A4AB
	public PackageDestinationAllButEntityID(int _excludedEntityId)
	{
		this.entityId = _excludedEntityId;
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x0018C2BA File Offset: 0x0018A4BA
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity || _cInfo.entityId == this.entityId;
	}

	// Token: 0x040032BE RID: 12990
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
