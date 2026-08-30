using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001930 RID: 6448
	[Preserve]
	public class RequirementHasParty : BaseRequirement
	{
		// Token: 0x0600C705 RID: 50949 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C706 RID: 50950 RVA: 0x00491100 File Offset: 0x0048F300
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null && entityPlayer.Party != null)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C707 RID: 50951 RVA: 0x0049112F File Offset: 0x0048F32F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasParty();
		}
	}
}
