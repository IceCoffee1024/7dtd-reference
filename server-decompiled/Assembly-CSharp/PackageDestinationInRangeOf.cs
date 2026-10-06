using System;

// Token: 0x02000885 RID: 2181
public class PackageDestinationInRangeOf : IPackageDestinationFilter
{
	// Token: 0x06003EFF RID: 16127 RVA: 0x0018E1A1 File Offset: 0x0018C3A1
	public PackageDestinationInRangeOf(int _entityId, int _range)
	{
		this.entityId = _entityId;
		this.range = _range;
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x0018E1B7 File Offset: 0x0018C3B7
	public bool Exclude(ClientInfo _cInfo)
	{
		return !GameManager.Instance.World.IsEntityInRange(this.entityId, _cInfo.entityId, this.range);
	}

	// Token: 0x0400332B RID: 13099
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400332C RID: 13100
	[PublicizedFrom(EAccessModifier.Private)]
	public int range;
}
