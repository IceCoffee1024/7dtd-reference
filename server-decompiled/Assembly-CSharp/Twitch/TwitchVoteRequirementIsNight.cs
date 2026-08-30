using System;

namespace Twitch
{
	// Token: 0x02001851 RID: 6225
	public class TwitchVoteRequirementIsNight : BaseTwitchVoteRequirement
	{
		// Token: 0x0600BFD3 RID: 49107 RVA: 0x0047348B File Offset: 0x0047168B
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
