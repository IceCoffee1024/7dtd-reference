using System;

namespace Twitch
{
	// Token: 0x020018D4 RID: 6356
	public class TwitchVoteRequirementIsNight : BaseTwitchVoteRequirement
	{
		// Token: 0x0600C3C4 RID: 50116 RVA: 0x0048110F File Offset: 0x0047F30F
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
