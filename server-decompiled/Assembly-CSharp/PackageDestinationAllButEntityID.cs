using System;

// Token: 0x02000880 RID: 2176
public class PackageDestinationAllButEntityID : IPackageDestinationFilter
{
	// Token: 0x06003F6D RID: 16237 RVA: 0x0018E37B File Offset: 0x0018C57B
	public PackageDestinationAllButEntityID(int _excludedEntityId)
	{
		this.entityId = _excludedEntityId;
	}

	// Token: 0x06003F6E RID: 16238 RVA: 0x0018E38A File Offset: 0x0018C58A
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity || _cInfo.entityId == this.entityId;
	}

	// Token: 0x0400333F RID: 13119
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
