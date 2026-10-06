using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006B5 RID: 1717
[Preserve]
public class InSafeZone : TargetedCompareRequirementBase
{
	// Token: 0x060035F3 RID: 13811 RVA: 0x001629CC File Offset: 0x00160BCC
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

	// Token: 0x060035F4 RID: 13812 RVA: 0x00162A0D File Offset: 0x00160C0D
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} In Safe Zone", this.invert ? "NOT " : ""));
	}
}
