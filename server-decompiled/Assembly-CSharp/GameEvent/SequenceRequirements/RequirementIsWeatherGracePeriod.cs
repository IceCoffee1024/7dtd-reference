using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200193D RID: 6461
	[Preserve]
	public class RequirementIsWeatherGracePeriod : BaseRequirement
	{
		// Token: 0x0600C739 RID: 51001 RVA: 0x00491800 File Offset: 0x0048FA00
		public override bool CanPerform(Entity target)
		{
			bool flag = GameManager.Instance.World.GetWorldTime() <= 30000UL;
			if (!this.Invert)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x0600C73A RID: 51002 RVA: 0x00491836 File Offset: 0x0048FA36
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
