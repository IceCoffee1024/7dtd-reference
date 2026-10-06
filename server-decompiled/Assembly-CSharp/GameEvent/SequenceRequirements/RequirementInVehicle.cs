using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001964 RID: 6500
	[Preserve]
	public class RequirementInVehicle : BaseRequirement
	{
		// Token: 0x0600C87D RID: 51325 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C87E RID: 51326 RVA: 0x00495A9E File Offset: 0x00493C9E
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

		// Token: 0x0600C87F RID: 51327 RVA: 0x00495AC8 File Offset: 0x00493CC8
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
