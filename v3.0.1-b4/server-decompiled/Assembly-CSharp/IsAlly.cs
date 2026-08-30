using System;
using UnityEngine.Scripting;

// Token: 0x02000671 RID: 1649
[Preserve]
public class IsAlly : TargetedCompareRequirementBase
{
	// Token: 0x060034A8 RID: 13480 RVA: 0x0015D664 File Offset: 0x0015B864
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
