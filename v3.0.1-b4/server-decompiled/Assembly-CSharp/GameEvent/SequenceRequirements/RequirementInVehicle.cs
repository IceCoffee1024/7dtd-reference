using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001912 RID: 6418
	[Preserve]
	public class RequirementInVehicle : BaseRequirement
	{
		// Token: 0x0600C59E RID: 50590 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C59F RID: 50591 RVA: 0x0048B266 File Offset: 0x00489466
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

		// Token: 0x0600C5A0 RID: 50592 RVA: 0x0048B290 File Offset: 0x00489490
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
