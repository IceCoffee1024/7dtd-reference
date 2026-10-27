using System;

// Token: 0x02000549 RID: 1353
public static class EnumDamageSourceExtensions
{
	// Token: 0x06002C2A RID: 11306 RVA: 0x000CE52F File Offset: 0x000CC72F
	public static bool AffectedByArmor(this EnumDamageSource s)
	{
		return s == EnumDamageSource.External;
	}
}
