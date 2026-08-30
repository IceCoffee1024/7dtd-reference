using System;

// Token: 0x0200087F RID: 2175
public class PackageDestinationSingleEntityID : IPackageDestinationFilter
{
	// Token: 0x06003F6B RID: 16235 RVA: 0x0018E34F File Offset: 0x0018C54F
	public PackageDestinationSingleEntityID(int _entityId)
	{
		this.entityId = _entityId;
	}

	// Token: 0x06003F6C RID: 16236 RVA: 0x0018E35E File Offset: 0x0018C55E
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity || _cInfo.entityId != this.entityId;
	}

	// Token: 0x0400333E RID: 13118
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
