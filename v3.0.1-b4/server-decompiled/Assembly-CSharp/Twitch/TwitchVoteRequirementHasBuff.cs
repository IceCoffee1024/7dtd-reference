using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x0200184F RID: 6223
	[Preserve]
	public class TwitchVoteRequirementHasBuff : BaseTwitchVoteRequirement
	{
		// Token: 0x0600BFC6 RID: 49094 RVA: 0x0047333E File Offset: 0x0047153E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600BFC7 RID: 49095 RVA: 0x00473354 File Offset: 0x00471554
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

		// Token: 0x0600BFC8 RID: 49096 RVA: 0x00473388 File Offset: 0x00471588
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityPlayer player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600BFC9 RID: 49097 RVA: 0x004733A8 File Offset: 0x004715A8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchVoteRequirementHasBuff.PropBuffName, ref this.BuffName);
		}

		// Token: 0x0400913F RID: 37183
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x04009140 RID: 37184
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x04009141 RID: 37185
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}
