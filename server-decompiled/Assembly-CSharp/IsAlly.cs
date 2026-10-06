using System;
using UnityEngine.Scripting;

// Token: 0x0200067C RID: 1660
[Preserve]
public class IsAlly : TargetedCompareRequirementBase
{
	// Token: 0x0600353E RID: 13630 RVA: 0x00160138 File Offset: 0x0015E338
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
		bool flag = entityPlayer.IsFriendOfLocalPlayer && !(this.target is EntityPlayerLocal);
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}
}
