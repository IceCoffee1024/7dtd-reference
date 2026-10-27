using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200198E RID: 6542
	[Preserve]
	public class RequirementHasSequenceLink : BaseRequirement
	{
		// Token: 0x0600C975 RID: 51573 RVA: 0x00499016 File Offset: 0x00497216
		public override bool CanPerform(Entity target)
		{
			if (GameEventManager.Current.HasSequenceLink(this.Owner))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C976 RID: 51574 RVA: 0x0049903A File Offset: 0x0049723A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasSequenceLink
			{
				Invert = this.Invert
			};
		}
	}
}
