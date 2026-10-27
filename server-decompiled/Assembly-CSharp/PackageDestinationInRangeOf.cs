using System;

// Token: 0x0200088F RID: 2191
public class PackageDestinationInRangeOf : IPackageDestinationFilter
{
	// Token: 0x06003F22 RID: 16162 RVA: 0x0018F249 File Offset: 0x0018D449
	public PackageDestinationInRangeOf(int _entityId, int _range)
	{
		this.entityId = _entityId;
		this.range = _range;
	}

	// Token: 0x06003F23 RID: 16163 RVA: 0x0018F25F File Offset: 0x0018D45F
	public bool Exclude(ClientInfo _cInfo)
	{
		return !GameManager.Instance.World.IsEntityInRange(this.entityId, _cInfo.entityId, this.range);
	}

	// Token: 0x0400334B RID: 13131
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400334C RID: 13132
	[PublicizedFrom(EAccessModifier.Private)]
	public int range;
}
