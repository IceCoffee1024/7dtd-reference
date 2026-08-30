using System;

// Token: 0x0200053A RID: 1338
public static class EnumDamageSourceExtensions
{
	// Token: 0x06002C02 RID: 11266 RVA: 0x000CD933 File Offset: 0x000CBB33
	public static bool AffectedByArmor(this EnumDamageSource s)
	{
		return s == EnumDamageSource.External;
	}
}
