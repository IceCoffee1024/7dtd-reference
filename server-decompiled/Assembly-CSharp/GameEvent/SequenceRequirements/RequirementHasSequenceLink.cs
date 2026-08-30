using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200190B RID: 6411
	[Preserve]
	public class RequirementHasSequenceLink : BaseRequirement
	{
		// Token: 0x0600C582 RID: 50562 RVA: 0x0048ADEA File Offset: 0x00488FEA
		public override bool CanPerform(Entity target)
		{
			if (GameEventManager.Current.HasSequenceLink(this.Owner))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C583 RID: 50563 RVA: 0x0048AE0E File Offset: 0x0048900E
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
