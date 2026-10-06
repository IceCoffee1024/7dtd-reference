using System;

namespace Twitch
{
	// Token: 0x020018A2 RID: 6306
	public class TwitchVoteRequirementHasProgression : BaseTwitchVoteOperationRequirement
	{
		// Token: 0x0600C2A9 RID: 49833 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C2AA RID: 49834 RVA: 0x0047D6B9 File Offset: 0x0047B8B9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float LeftSide(EntityPlayer player)
		{
			return (float)player.Progression.GetProgressionValue(this.SkillName).CalculatedLevel(player);
		}

		// Token: 0x0600C2AB RID: 49835 RVA: 0x0047D6D3 File Offset: 0x0047B8D3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override float RightSide(EntityPlayer player)
		{
			return (float)this.Level;
		}

		// Token: 0x0600C2AC RID: 49836 RVA: 0x0047D6DC File Offset: 0x0047B8DC
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckPerk(EntityPlayer player, string buffName)
		{
			if (player.Progression.GetProgressionValue(buffName).CalculatedLevel(player) >= this.Level)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C2AD RID: 49837 RVA: 0x0047D708 File Offset: 0x0047B908
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchVoteRequirementHasProgression.PropSkillName, ref this.SkillName);
			properties.ParseInt(TwitchVoteRequirementHasProgression.PropLevel, ref this.Level);
		}

		// Token: 0x04009324 RID: 37668
		[PublicizedFrom(EAccessModifier.Protected)]
		public string SkillName = "";

		// Token: 0x04009325 RID: 37669
		[PublicizedFrom(EAccessModifier.Protected)]
		public int Level = 1;

		// Token: 0x04009326 RID: 37670
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillName = "skill_name";

		// Token: 0x04009327 RID: 37671
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLevel = "level";
	}
}
