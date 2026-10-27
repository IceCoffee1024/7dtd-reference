using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200199A RID: 6554
	[Preserve]
	public class RequirementIsWeatherGracePeriod : BaseRequirement
	{
		// Token: 0x0600C9A5 RID: 51621 RVA: 0x004996E0 File Offset: 0x004978E0
		public override bool CanPerform(Entity target)
		{
			bool flag = GameManager.Instance.World.GetWorldTime() <= 30000UL;
			if (!this.Invert)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x0600C9A6 RID: 51622 RVA: 0x00499716 File Offset: 0x00497916
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementIsWeatherGracePeriod
			{
				Invert = this.Invert
			};
		}
	}
}
