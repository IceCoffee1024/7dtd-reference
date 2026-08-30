using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001835 RID: 6197
	[Preserve]
	public class TwitchRequirementHasProgression : BaseTwitchOperationRequirement
	{
		// Token: 0x0600BF16 RID: 48918 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600BF17 RID: 48919 RVA: 0x0046EA84 File Offset: 0x0046CC84
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

		// Token: 0x0600BF18 RID: 48920 RVA: 0x0046EABE File Offset: 0x0046CCBE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity entity)
		{
			return this.Level;
		}

		// Token: 0x0600BF19 RID: 48921 RVA: 0x0046EACB File Offset: 0x0046CCCB
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckPerk(EntityPlayer player, string buffName)
		{
			if (player.Progression.GetProgressionValue(buffName).CalculatedLevel(player) >= this.Level)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600BF1A RID: 48922 RVA: 0x0046EAF7 File Offset: 0x0046CCF7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchRequirementHasProgression.PropSkillName, ref this.SkillName);
			properties.ParseInt(TwitchRequirementHasProgression.PropLevel, ref this.Level);
		}

		// Token: 0x04009024 RID: 36900
		[PublicizedFrom(EAccessModifier.Protected)]
		public string SkillName = "";

		// Token: 0x04009025 RID: 36901
		[PublicizedFrom(EAccessModifier.Protected)]
		public int Level = 1;

		// Token: 0x04009026 RID: 36902
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillName = "skill_name";

		// Token: 0x04009027 RID: 36903
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLevel = "level";
	}
}
