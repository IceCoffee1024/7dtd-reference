using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001911 RID: 6417
	[Preserve]
	public class RequirementInTraderArea : BaseRequirement
	{
		// Token: 0x0600C59B RID: 50587 RVA: 0x0048B210 File Offset: 0x00489410
		public override bool CanPerform(Entity target)
		{
			if (!GameManager.Instance.World.IsWithinTraderArea(new Vector3i((target == null) ? this.Owner.TargetPosition : target.position)))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C59C RID: 50588 RVA: 0x0048B25F File Offset: 0x0048945F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInTraderArea();
		}
	}
}
