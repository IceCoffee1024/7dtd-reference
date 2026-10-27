using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001998 RID: 6552
	[Preserve]
	public class RequirementIsIndoors : BaseRequirement
	{
		// Token: 0x0600C99F RID: 51615 RVA: 0x00499640 File Offset: 0x00497840
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

		// Token: 0x0600C9A0 RID: 51616 RVA: 0x00499689 File Offset: 0x00497889
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
