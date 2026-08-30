using System;
using UnityEngine.Scripting;

// Token: 0x020006C6 RID: 1734
[Preserve]
public class MinEventActionRemoveAllNegativeBuffs : MinEventActionTargetedBase
{
	// Token: 0x060035C7 RID: 13767 RVA: 0x0016383C File Offset: 0x00161A3C
	public override void Execute(MinEventParams _params)
	{
		for (int i = 0; i < this.targets.Count; i++)
		{
			for (int j = 0; j < this.targets[i].Buffs.ActiveBuffs.Count; j++)
			{
				if (this.targets[i].Buffs.ActiveBuffs[j].BuffClass.DamageType != EnumDamageTypes.None)
				{
					this.targets[i].Buffs.ActiveBuffs[j].Remove = true;
				}
			}
		}
	}
}
