using System;

// Token: 0x02000882 RID: 2178
public class PackageDestinationInRangeOf : IPackageDestinationFilter
{
	// Token: 0x06003F71 RID: 16241 RVA: 0x0018E3AF File Offset: 0x0018C5AF
	public PackageDestinationInRangeOf(int _entityId, int _range)
	{
		this.entityId = _entityId;
		this.range = _range;
	}

	// Token: 0x06003F72 RID: 16242 RVA: 0x0018E3C5 File Offset: 0x0018C5C5
	public bool Exclude(ClientInfo _cInfo)
	{
		return !GameManager.Instance.World.IsEntityInRange(this.entityId, _cInfo.entityId, this.range);
	}

	// Token: 0x04003340 RID: 13120
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003341 RID: 13121
	[PublicizedFrom(EAccessModifier.Private)]
	public int range;
}
