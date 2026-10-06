using System;
using UnityEngine.Scripting;

// Token: 0x0200046B RID: 1131
[Preserve]
public static class EnumEntityStunTypeExtensions
{
	// Token: 0x0600220B RID: 8715 RVA: 0x000CE183 File Offset: 0x000CC383
	public static bool CanMove(this EnumEntityStunType type)
	{
		return type == EnumEntityStunType.None;
	}
}
