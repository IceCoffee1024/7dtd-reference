using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200193B RID: 6459
	[Preserve]
	public class RequirementIsIndoors : BaseRequirement
	{
		// Token: 0x0600C733 RID: 50995 RVA: 0x00491760 File Offset: 0x0048F960
		public override bool CanPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive == null)
			{
				return false;
			}
			if (!this.Invert)
			{
				return entityAlive.Stats.AmountEnclosed > 0f;
			}
			return entityAlive.Stats.AmountEnclosed <= 0f;
		}

		// Token: 0x0600C734 RID: 50996 RVA: 0x004917A9 File Offset: 0x0048F9A9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementIsIndoors
			{
				Invert = this.Invert
			};
		}
	}
}
