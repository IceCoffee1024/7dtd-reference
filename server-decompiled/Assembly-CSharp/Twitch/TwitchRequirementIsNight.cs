using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001888 RID: 6280
	[Preserve]
	public class TwitchRequirementIsNight : BaseTwitchRequirement
	{
		// Token: 0x0600C1FA RID: 49658 RVA: 0x00478E2A File Offset: 0x0047702A
		public override bool CanPerform(Entity entity)
		{
			if (!this.Invert)
			{
				return !GameManager.Instance.World.IsDaytime();
			}
			return GameManager.Instance.World.IsDaytime();
		}
	}
}
