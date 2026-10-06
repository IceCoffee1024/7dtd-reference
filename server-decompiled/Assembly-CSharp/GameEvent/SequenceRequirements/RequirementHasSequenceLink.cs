using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200195D RID: 6493
	[Preserve]
	public class RequirementHasSequenceLink : BaseRequirement
	{
		// Token: 0x0600C861 RID: 51297 RVA: 0x00495622 File Offset: 0x00493822
		public override bool CanPerform(Entity target)
		{
			if (GameEventManager.Current.HasSequenceLink(this.Owner))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C862 RID: 51298 RVA: 0x00495646 File Offset: 0x00493846
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
