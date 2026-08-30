using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006A9 RID: 1705
[Preserve]
public class InSafeZone : TargetedCompareRequirementBase
{
	// Token: 0x0600355A RID: 13658 RVA: 0x0015FE84 File Offset: 0x0015E084
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

	// Token: 0x0600355B RID: 13659 RVA: 0x0015FEC5 File Offset: 0x0015E0C5
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} In Safe Zone", this.invert ? "NOT " : ""));
	}
}
