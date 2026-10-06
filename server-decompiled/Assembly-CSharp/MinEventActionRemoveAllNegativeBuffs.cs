using System;
using UnityEngine.Scripting;

// Token: 0x020006D2 RID: 1746
[Preserve]
public class MinEventActionRemoveAllNegativeBuffs : MinEventActionTargetedBase
{
	// Token: 0x06003660 RID: 13920 RVA: 0x00166384 File Offset: 0x00164584
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
