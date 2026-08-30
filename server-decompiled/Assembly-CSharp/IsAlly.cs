using System;
using UnityEngine.Scripting;

// Token: 0x0200067A RID: 1658
[Preserve]
public class IsAlly : TargetedCompareRequirementBase
{
	// Token: 0x060034FB RID: 13563 RVA: 0x0015F7D4 File Offset: 0x0015D9D4
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
