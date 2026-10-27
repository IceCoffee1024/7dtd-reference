using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001995 RID: 6549
	[Preserve]
	public class RequirementInVehicle : BaseRequirement
	{
		// Token: 0x0600C991 RID: 51601 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C992 RID: 51602 RVA: 0x00499492 File Offset: 0x00497692
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

		// Token: 0x0600C993 RID: 51603 RVA: 0x004994BC File Offset: 0x004976BC
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
