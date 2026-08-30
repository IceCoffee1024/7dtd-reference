using System;
using UnityEngine.Scripting;

// Token: 0x02000469 RID: 1129
[Preserve]
public static class EnumEntityStunTypeExtensions
{
	// Token: 0x0600221C RID: 8732 RVA: 0x000CE0CF File Offset: 0x000CC2CF
	public static bool CanMove(this EnumEntityStunType type)
	{
		return type == EnumEntityStunType.None;
	}
}
