using System;

namespace Twitch
{
	// Token: 0x02001876 RID: 6262
	public class TwitchVoteRequirementHasProgression : BaseTwitchVoteOperationRequirement
	{
		// Token: 0x0600C151 RID: 49489 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C152 RID: 49490 RVA: 0x0047917D File Offset: 0x0047737D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float LeftSide(EntityPlayer player)
		{
			return (float)player.Progression.GetProgressionValue(this.SkillName).CalculatedLevel(player);
		}

		// Token: 0x0600C153 RID: 49491 RVA: 0x00479197 File Offset: 0x00477397
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float RightSide(EntityPlayer player)
		{
			return (float)this.Level;
		}

		// Token: 0x0600C154 RID: 49492 RVA: 0x004791A0 File Offset: 0x004773A0
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckPerk(EntityPlayer player, string buffName)
		{
			if (player.Progression.GetProgressionValue(buffName).CalculatedLevel(player) >= this.Level)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C155 RID: 49493 RVA: 0x004791CC File Offset: 0x004773CC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchVoteRequirementHasProgression.PropSkillName, ref this.SkillName);
			properties.ParseInt(TwitchVoteRequirementHasProgression.PropLevel, ref this.Level);
		}

		// Token: 0x0400925E RID: 37470
		[PublicizedFrom(EAccessModifier.Protected)]
		public string SkillName = "";

		// Token: 0x0400925F RID: 37471
		[PublicizedFrom(EAccessModifier.Protected)]
		public int Level = 1;

		// Token: 0x04009260 RID: 37472
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillName = "skill_name";

		// Token: 0x04009261 RID: 37473
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLevel = "level";
	}
}
