using System;

// Token: 0x02000545 RID: 1349
public static class EnumDamageSourceExtensions
{
	// Token: 0x06002C22 RID: 11298 RVA: 0x000CE183 File Offset: 0x000CC383
	public static bool AffectedByArmor(this EnumDamageSource s)
	{
		return s == EnumDamageSource.External;
	}
}
