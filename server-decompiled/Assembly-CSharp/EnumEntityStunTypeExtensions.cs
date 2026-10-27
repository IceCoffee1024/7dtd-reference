using System;
using UnityEngine.Scripting;

// Token: 0x0200046D RID: 1133
[Preserve]
public static class EnumEntityStunTypeExtensions
{
	// Token: 0x06002210 RID: 8720 RVA: 0x000CE52F File Offset: 0x000CC72F
	public static bool CanMove(this EnumEntityStunType type)
	{
		return type == EnumEntityStunType.None;
	}
}
