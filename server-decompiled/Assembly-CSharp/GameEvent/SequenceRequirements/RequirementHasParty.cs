using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200198D RID: 6541
	[Preserve]
	public class RequirementHasParty : BaseRequirement
	{
		// Token: 0x0600C971 RID: 51569 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C972 RID: 51570 RVA: 0x00498FE0 File Offset: 0x004971E0
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null && entityPlayer.Party != null)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C973 RID: 51571 RVA: 0x0049900F File Offset: 0x0049720F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasParty();
		}
	}
}
