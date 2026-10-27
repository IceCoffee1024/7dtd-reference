using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020006BC RID: 1724
[Preserve]
public class InSafeZone : TargetedCompareRequirementBase
{
	// Token: 0x06003601 RID: 13825 RVA: 0x001635C0 File Offset: 0x001617C0
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

	// Token: 0x06003602 RID: 13826 RVA: 0x00163601 File Offset: 0x00161801
	public override void GetInfoStrings(ref List<string> list)
	{
		list.Add(string.Format("Is {0} In Safe Zone", this.invert ? "NOT " : ""));
	}
}
