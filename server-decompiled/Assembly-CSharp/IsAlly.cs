using System;
using UnityEngine.Scripting;

// Token: 0x02000683 RID: 1667
[Preserve]
public class IsAlly : TargetedCompareRequirementBase
{
	// Token: 0x0600354C RID: 13644 RVA: 0x00160D2C File Offset: 0x0015EF2C
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
