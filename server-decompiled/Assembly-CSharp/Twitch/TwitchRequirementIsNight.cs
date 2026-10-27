using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018B9 RID: 6329
	[Preserve]
	public class TwitchRequirementIsNight : BaseTwitchRequirement
	{
		// Token: 0x0600C30E RID: 49934 RVA: 0x0047C7D6 File Offset: 0x0047A9D6
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
