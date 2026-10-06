using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001963 RID: 6499
	[Preserve]
	public class RequirementInTraderArea : BaseRequirement
	{
		// Token: 0x0600C87A RID: 51322 RVA: 0x00495A48 File Offset: 0x00493C48
		public override bool CanPerform(Entity target)
		{
			if (!GameManager.Instance.World.IsWithinTraderArea(new Vector3i((target == null) ? this.Owner.TargetPosition : target.position)))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C87B RID: 51323 RVA: 0x00495A97 File Offset: 0x00493C97
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInTraderArea();
		}
	}
}
