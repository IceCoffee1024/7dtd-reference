using System;

// Token: 0x02000876 RID: 2166
public class PackageDestinationInRangeOf : IPackageDestinationFilter
{
	// Token: 0x06003F0C RID: 16140 RVA: 0x0018C2DF File Offset: 0x0018A4DF
	public PackageDestinationInRangeOf(int _entityId, int _range)
	{
		this.entityId = _entityId;
		this.range = _range;
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x0018C2F5 File Offset: 0x0018A4F5
	public bool Exclude(ClientInfo _cInfo)
	{
		return !GameManager.Instance.World.IsEntityInRange(this.entityId, _cInfo.entityId, this.range);
	}

	// Token: 0x040032BF RID: 12991
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040032C0 RID: 12992
	[PublicizedFrom(EAccessModifier.Private)]
	public int range;
}
