using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200190A RID: 6410
	[Preserve]
	public class RequirementHasParty : BaseRequirement
	{
		// Token: 0x0600C57E RID: 50558 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C57F RID: 50559 RVA: 0x0048ADB4 File Offset: 0x00488FB4
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null && entityPlayer.Party != null)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C580 RID: 50560 RVA: 0x0048ADE3 File Offset: 0x00488FE3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasParty();
		}
	}
}
