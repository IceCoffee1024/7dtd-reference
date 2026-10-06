using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018A1 RID: 6305
	[Preserve]
	public class TwitchVoteRequirementHasBuff : BaseTwitchVoteRequirement
	{
		// Token: 0x0600C2A3 RID: 49827 RVA: 0x0047D616 File Offset: 0x0047B816
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.BuffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C2A4 RID: 49828 RVA: 0x0047D62C File Offset: 0x0047B82C
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

		// Token: 0x0600C2A5 RID: 49829 RVA: 0x0047D660 File Offset: 0x0047B860
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBuff(EntityPlayer player, string buffName)
		{
			if (player.Buffs.HasBuff(buffName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C2A6 RID: 49830 RVA: 0x0047D680 File Offset: 0x0047B880
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchVoteRequirementHasBuff.PropBuffName, ref this.BuffName);
		}

		// Token: 0x04009321 RID: 37665
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BuffName = "";

		// Token: 0x04009322 RID: 37666
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x04009323 RID: 37667
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}
