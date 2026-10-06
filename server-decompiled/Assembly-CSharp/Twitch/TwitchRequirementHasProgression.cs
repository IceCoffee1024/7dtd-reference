using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001887 RID: 6279
	[Preserve]
	public class TwitchRequirementHasProgression : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C1F3 RID: 49651 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C1F4 RID: 49652 RVA: 0x00478D5C File Offset: 0x00476F5C
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

		// Token: 0x0600C1F5 RID: 49653 RVA: 0x00478D96 File Offset: 0x00476F96
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity entity)
		{
			return this.Level;
		}

		// Token: 0x0600C1F6 RID: 49654 RVA: 0x00478DA3 File Offset: 0x00476FA3
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckPerk(EntityPlayer player, string buffName)
		{
			if (player.Progression.GetProgressionValue(buffName).CalculatedLevel(player) >= this.Level)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C1F7 RID: 49655 RVA: 0x00478DCF File Offset: 0x00476FCF
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchRequirementHasProgression.PropSkillName, ref this.SkillName);
			properties.ParseInt(TwitchRequirementHasProgression.PropLevel, ref this.Level);
		}

		// Token: 0x04009206 RID: 37382
		[PublicizedFrom(EAccessModifier.Protected)]
		public string SkillName = "";

		// Token: 0x04009207 RID: 37383
		[PublicizedFrom(EAccessModifier.Protected)]
		public int Level = 1;

		// Token: 0x04009208 RID: 37384
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillName = "skill_name";

		// Token: 0x04009209 RID: 37385
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLevel = "level";
	}
}
