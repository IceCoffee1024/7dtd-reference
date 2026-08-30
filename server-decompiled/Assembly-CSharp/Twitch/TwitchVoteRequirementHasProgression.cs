using System;

namespace Twitch
{
	// Token: 0x02001850 RID: 6224
	public class TwitchVoteRequirementHasProgression : BaseTwitchVoteOperationRequirement
	{
		// Token: 0x0600BFCC RID: 49100 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600BFCD RID: 49101 RVA: 0x004733E1 File Offset: 0x004715E1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float LeftSide(EntityPlayer player)
		{
			return (float)player.Progression.GetProgressionValue(this.SkillName).CalculatedLevel(player);
		}

		// Token: 0x0600BFCE RID: 49102 RVA: 0x004733FB File Offset: 0x004715FB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float RightSide(EntityPlayer player)
		{
			return (float)this.Level;
		}

		// Token: 0x0600BFCF RID: 49103 RVA: 0x00473404 File Offset: 0x00471604
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckPerk(EntityPlayer player, string buffName)
		{
			if (player.Progression.GetProgressionValue(buffName).CalculatedLevel(player) >= this.Level)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600BFD0 RID: 49104 RVA: 0x00473430 File Offset: 0x00471630
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchVoteRequirementHasProgression.PropSkillName, ref this.SkillName);
			properties.ParseInt(TwitchVoteRequirementHasProgression.PropLevel, ref this.Level);
		}

		// Token: 0x04009142 RID: 37186
		[PublicizedFrom(EAccessModifier.Protected)]
		public string SkillName = "";

		// Token: 0x04009143 RID: 37187
		[PublicizedFrom(EAccessModifier.Protected)]
		public int Level = 1;

		// Token: 0x04009144 RID: 37188
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillName = "skill_name";

		// Token: 0x04009145 RID: 37189
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLevel = "level";
	}
}
