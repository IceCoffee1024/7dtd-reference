using System;

namespace Twitch
{
	// Token: 0x020018D3 RID: 6355
	public class TwitchVoteRequirementHasProgression : BaseTwitchVoteOperationRequirement
	{
		// Token: 0x0600C3BD RID: 50109 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C3BE RID: 50110 RVA: 0x00481065 File Offset: 0x0047F265
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float LeftSide(EntityPlayer player)
		{
			return (float)player.Progression.GetProgressionValue(this.SkillName).CalculatedLevel(player);
		}

		// Token: 0x0600C3BF RID: 50111 RVA: 0x0048107F File Offset: 0x0047F27F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float RightSide(EntityPlayer player)
		{
			return (float)this.Level;
		}

		// Token: 0x0600C3C0 RID: 50112 RVA: 0x00481088 File Offset: 0x0047F288
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckPerk(EntityPlayer player, string buffName)
		{
			if (player.Progression.GetProgressionValue(buffName).CalculatedLevel(player) >= this.Level)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C3C1 RID: 50113 RVA: 0x004810B4 File Offset: 0x0047F2B4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchVoteRequirementHasProgression.PropSkillName, ref this.SkillName);
			properties.ParseInt(TwitchVoteRequirementHasProgression.PropLevel, ref this.Level);
		}

		// Token: 0x0400940D RID: 37901
		[PublicizedFrom(EAccessModifier.Protected)]
		public string SkillName = "";

		// Token: 0x0400940E RID: 37902
		[PublicizedFrom(EAccessModifier.Protected)]
		public int Level = 1;

		// Token: 0x0400940F RID: 37903
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillName = "skill_name";

		// Token: 0x04009410 RID: 37904
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLevel = "level";
	}
}
