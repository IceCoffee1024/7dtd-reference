using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200195C RID: 6492
	[Preserve]
	public class RequirementHasParty : BaseRequirement
	{
		// Token: 0x0600C85D RID: 51293 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C85E RID: 51294 RVA: 0x004955EC File Offset: 0x004937EC
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null && entityPlayer.Party != null)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C85F RID: 51295 RVA: 0x0049561B File Offset: 0x0049381B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasParty();
		}
	}
}
