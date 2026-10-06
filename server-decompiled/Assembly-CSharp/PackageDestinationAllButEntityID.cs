using System;

// Token: 0x02000883 RID: 2179
public class PackageDestinationAllButEntityID : IPackageDestinationFilter
{
	// Token: 0x06003EFB RID: 16123 RVA: 0x0018E16D File Offset: 0x0018C36D
	public PackageDestinationAllButEntityID(int _excludedEntityId)
	{
		this.entityId = _excludedEntityId;
	}

	// Token: 0x06003EFC RID: 16124 RVA: 0x0018E17C File Offset: 0x0018C37C
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity || _cInfo.entityId == this.entityId;
	}

	// Token: 0x0400332A RID: 13098
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
