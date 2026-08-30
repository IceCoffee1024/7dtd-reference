using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001836 RID: 6198
	[Preserve]
	public class TwitchRequirementIsNight : BaseTwitchRequirement
	{
		// Token: 0x0600BF1D RID: 48925 RVA: 0x0046EB52 File Offset: 0x0046CD52
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
