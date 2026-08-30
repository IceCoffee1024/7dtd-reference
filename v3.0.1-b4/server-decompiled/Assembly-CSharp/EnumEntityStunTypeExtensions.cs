using System;
using UnityEngine.Scripting;

// Token: 0x02000465 RID: 1125
[Preserve]
public static class EnumEntityStunTypeExtensions
{
	// Token: 0x060021FF RID: 8703 RVA: 0x000CD933 File Offset: 0x000CBB33
	public static bool CanMove(this EnumEntityStunType type)
	{
		return type == EnumEntityStunType.None;
	}
}
