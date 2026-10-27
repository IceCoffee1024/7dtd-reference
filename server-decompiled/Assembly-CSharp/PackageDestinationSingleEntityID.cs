using System;

// Token: 0x0200088C RID: 2188
public class PackageDestinationSingleEntityID : IPackageDestinationFilter
{
	// Token: 0x06003F1C RID: 16156 RVA: 0x0018F1E9 File Offset: 0x0018D3E9
	public PackageDestinationSingleEntityID(int _entityId)
	{
		this.entityId = _entityId;
	}

	// Token: 0x06003F1D RID: 16157 RVA: 0x0018F1F8 File Offset: 0x0018D3F8
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity || _cInfo.entityId != this.entityId;
	}

	// Token: 0x04003349 RID: 13129
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
