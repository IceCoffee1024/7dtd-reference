using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x0200185C RID: 6236
	[Preserve]
	public class TwitchRequirementIsNight : BaseTwitchRequirement
	{
		// Token: 0x0600C0A2 RID: 49314 RVA: 0x004748EE File Offset: 0x00472AEE
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
