using System;

namespace Twitch
{
	// Token: 0x02001877 RID: 6263
	public class TwitchVoteRequirementIsNight : BaseTwitchVoteRequirement
	{
		// Token: 0x0600C158 RID: 49496 RVA: 0x00479227 File Offset: 0x00477427
		public override bool CanPerform(EntityPlayer player)
		{
			if (!this.Invert)
			{
				return !GameManager.Instance.World.IsDaytime();
			}
			return GameManager.Instance.World.IsDaytime();
		}
	}
}
