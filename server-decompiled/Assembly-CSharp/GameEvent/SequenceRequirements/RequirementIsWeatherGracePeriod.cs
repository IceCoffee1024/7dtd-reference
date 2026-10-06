using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001969 RID: 6505
	[Preserve]
	public class RequirementIsWeatherGracePeriod : BaseRequirement
	{
		// Token: 0x0600C891 RID: 51345 RVA: 0x00495CEC File Offset: 0x00493EEC
		public override bool CanPerform(Entity target)
		{
			bool flag = GameManager.Instance.World.GetWorldTime() <= 30000UL;
			if (!this.Invert)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x0600C892 RID: 51346 RVA: 0x00495D22 File Offset: 0x00493F22
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
