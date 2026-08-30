using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001875 RID: 6261
	[Preserve]
	public class TwitchVoteRequirementHasBuff : BaseTwitchVoteRequirement
	{
		// Token: 0x0600C14B RID: 49483 RVA: 0x004790DA File Offset: 0x004772DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C14C RID: 49484 RVA: 0x004790F0 File Offset: 0x004772F0
		public override bool CanPerform(EntityPlayer player)
		{
			for (int i = 0; i < this.BuffList.Length; i++)
			{
				if (!this.CheckBuff(player, this.BuffList[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C14D RID: 49485 RVA: 0x00479124 File Offset: 0x00477324
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityPlayer player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C14E RID: 49486 RVA: 0x00479144 File Offset: 0x00477344
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchVoteRequirementHasBuff.PropBuffName, ref this.BuffName);
		}

		// Token: 0x0400925B RID: 37467
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x0400925C RID: 37468
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x0400925D RID: 37469
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}
