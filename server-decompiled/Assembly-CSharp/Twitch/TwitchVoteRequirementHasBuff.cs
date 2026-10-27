using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018D2 RID: 6354
	[Preserve]
	public class TwitchVoteRequirementHasBuff : BaseTwitchVoteRequirement
	{
		// Token: 0x0600C3B7 RID: 50103 RVA: 0x00480FC2 File Offset: 0x0047F1C2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C3B8 RID: 50104 RVA: 0x00480FD8 File Offset: 0x0047F1D8
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

		// Token: 0x0600C3B9 RID: 50105 RVA: 0x0048100C File Offset: 0x0047F20C
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityPlayer player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C3BA RID: 50106 RVA: 0x0048102C File Offset: 0x0047F22C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchVoteRequirementHasBuff.PropBuffName, ref this.BuffName);
		}

		// Token: 0x0400940A RID: 37898
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x0400940B RID: 37899
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x0400940C RID: 37900
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}
