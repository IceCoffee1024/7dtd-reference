using System;

namespace Twitch
{
	// Token: 0x020018A3 RID: 6307
	public class TwitchVoteRequirementIsNight : BaseTwitchVoteRequirement
	{
		// Token: 0x0600C2B0 RID: 49840 RVA: 0x0047D763 File Offset: 0x0047B963
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
