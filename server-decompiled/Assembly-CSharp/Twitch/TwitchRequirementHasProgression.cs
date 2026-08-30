using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x0200185B RID: 6235
	[Preserve]
	public class TwitchRequirementHasProgression : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C09B RID: 49307 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C09C RID: 49308 RVA: 0x00474820 File Offset: 0x00472A20
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity entity)
		{
			EntityPlayer entityPlayer = entity as EntityPlayer;
			if (entityPlayer != null)
			{
				return entityPlayer.Progression.GetProgressionValue(this.SkillName).CalculatedLevel(entityPlayer);
			}
			return 0;
		}

		// Token: 0x0600C09D RID: 49309 RVA: 0x0047485A File Offset: 0x00472A5A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity entity)
		{
			return this.Level;
		}

		// Token: 0x0600C09E RID: 49310 RVA: 0x00474867 File Offset: 0x00472A67
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckPerk(EntityPlayer player, string buffName)
		{
			if (player.Progression.GetProgressionValue(buffName).CalculatedLevel(player) >= this.Level)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C09F RID: 49311 RVA: 0x00474893 File Offset: 0x00472A93
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchRequirementHasProgression.PropSkillName, ref this.SkillName);
			properties.ParseInt(TwitchRequirementHasProgression.PropLevel, ref this.Level);
		}

		// Token: 0x04009140 RID: 37184
		[PublicizedFrom(EAccessModifier.Protected)]
		public string SkillName = "";

		// Token: 0x04009141 RID: 37185
		[PublicizedFrom(EAccessModifier.Protected)]
		public int Level = 1;

		// Token: 0x04009142 RID: 37186
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillName = "skill_name";

		// Token: 0x04009143 RID: 37187
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLevel = "level";
	}
}
