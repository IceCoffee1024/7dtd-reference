using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001931 RID: 6449
	[Preserve]
	public class RequirementHasSequenceLink : BaseRequirement
	{
		// Token: 0x0600C709 RID: 50953 RVA: 0x00491136 File Offset: 0x0048F336
		public override bool CanPerform(Entity target)
		{
			if (GameEventManager.Current.HasSequenceLink(this.Owner))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C70A RID: 50954 RVA: 0x0049115A File Offset: 0x0048F35A
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
