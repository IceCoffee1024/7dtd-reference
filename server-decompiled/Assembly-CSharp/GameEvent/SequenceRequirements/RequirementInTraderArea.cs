using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001937 RID: 6455
	[Preserve]
	public class RequirementInTraderArea : BaseRequirement
	{
		// Token: 0x0600C722 RID: 50978 RVA: 0x0049155C File Offset: 0x0048F75C
		public override bool CanPerform(Entity target)
		{
			if (!GameManager.Instance.World.IsWithinTraderArea(new Vector3i((target == null) ? this.Owner.TargetPosition : target.position)))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C723 RID: 50979 RVA: 0x004915AB File Offset: 0x0048F7AB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInTraderArea();
		}
	}
}
