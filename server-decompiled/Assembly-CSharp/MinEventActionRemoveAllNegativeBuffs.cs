using System;
using UnityEngine.Scripting;

// Token: 0x020006D9 RID: 1753
[Preserve]
public class MinEventActionRemoveAllNegativeBuffs : MinEventActionTargetedBase
{
	// Token: 0x0600366F RID: 13935 RVA: 0x00166F80 File Offset: 0x00165180
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
