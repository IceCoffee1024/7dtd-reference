using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001994 RID: 6548
	[Preserve]
	public class RequirementInTraderArea : BaseRequirement
	{
		// Token: 0x0600C98E RID: 51598 RVA: 0x0049943C File Offset: 0x0049763C
		public override bool CanPerform(Entity target)
		{
			if (!GameManager.Instance.World.IsWithinTraderArea(new Vector3i((target == null) ? this.Owner.TargetPosition : target.position)))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C98F RID: 51599 RVA: 0x0049948B File Offset: 0x0049768B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInTraderArea();
		}
	}
}
