using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001938 RID: 6456
	[Preserve]
	public class RequirementInVehicle : BaseRequirement
	{
		// Token: 0x0600C725 RID: 50981 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C726 RID: 50982 RVA: 0x004915B2 File Offset: 0x0048F7B2
		public override bool CanPerform(Entity target)
		{
			if (target == null)
			{
				return false;
			}
			if (target.AttachedToEntity)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C727 RID: 50983 RVA: 0x004915DC File Offset: 0x0048F7DC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInVehicle
			{
				Invert = this.Invert
			};
		}
	}
}
