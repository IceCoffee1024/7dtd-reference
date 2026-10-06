using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001967 RID: 6503
	[Preserve]
	public class RequirementIsIndoors : BaseRequirement
	{
		// Token: 0x0600C88B RID: 51339 RVA: 0x00495C4C File Offset: 0x00493E4C
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

		// Token: 0x0600C88C RID: 51340 RVA: 0x00495C95 File Offset: 0x00493E95
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
