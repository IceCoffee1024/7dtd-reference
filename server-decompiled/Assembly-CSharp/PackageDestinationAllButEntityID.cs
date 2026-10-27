using System;

// Token: 0x0200088D RID: 2189
public class PackageDestinationAllButEntityID : IPackageDestinationFilter
{
	// Token: 0x06003F1E RID: 16158 RVA: 0x0018F215 File Offset: 0x0018D415
	public PackageDestinationAllButEntityID(int _excludedEntityId)
	{
		this.entityId = _excludedEntityId;
	}

	// Token: 0x06003F1F RID: 16159 RVA: 0x0018F224 File Offset: 0x0018D424
	public bool Exclude(ClientInfo _cInfo)
	{
		return !_cInfo.bAttachedToEntity || _cInfo.entityId == this.entityId;
	}

	// Token: 0x0400334A RID: 13130
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;
}
