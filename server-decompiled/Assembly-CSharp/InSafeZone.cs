using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006B3 RID: 1715
[Preserve]
public class InSafeZone : TargetedCompareRequirementBase
{
	// Token: 0x060035B0 RID: 13744 RVA: 0x0016205C File Offset: 0x0016025C
	public override bool IsValid(MinEventParams _params)
	{
		if (!base.IsValid(_params))
		{
			return false;
		}
		EntityPlayer entityPlayer = this.target as EntityPlayer;
		if (entityPlayer == null)
		{
			return false;
		}
		if (!this.invert)
		{
			return entityPlayer.TwitchSafe;
		}
		return !entityPlayer.TwitchSafe;
	}

	// Token: 0x060035B1 RID: 13745 RVA: 0x0016209D File Offset: 0x0016029D
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} In Safe Zone", this.invert ? "NOT " : ""));
	}
}
