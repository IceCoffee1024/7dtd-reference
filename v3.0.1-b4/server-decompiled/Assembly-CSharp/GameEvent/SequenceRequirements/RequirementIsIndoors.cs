using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001915 RID: 6421
	[Preserve]
	public class RequirementIsIndoors : BaseRequirement
	{
		// Token: 0x0600C5AC RID: 50604 RVA: 0x0048B414 File Offset: 0x00489614
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

		// Token: 0x0600C5AD RID: 50605 RVA: 0x0048B45D File Offset: 0x0048965D
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
