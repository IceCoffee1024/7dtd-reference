using System;

// Token: 0x02000873 RID: 2163
public class PackageDestinationSingleEntityID : IPackageDestinationFilter
{
	// Token: 0x06003F06 RID: 16134 RVA: 0x0018C27F File Offset: 0x0018A47F
	public PackageDestinationSingleEntityID(int _entityId)
	{
		this.entityId = _entityId;
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x0018C28E File Offset: 0x0018A48E
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity || _cInfo.entityId != this.entityId;
	}

	// Token: 0x040032BD RID: 12989
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
