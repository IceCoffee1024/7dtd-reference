using System;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018B8 RID: 6328
	[Preserve]
	public class TwitchRequirementHasProgression : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C307 RID: 49927 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C308 RID: 49928 RVA: 0x0047C708 File Offset: 0x0047A908
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

		// Token: 0x0600C309 RID: 49929 RVA: 0x0047C742 File Offset: 0x0047A942
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity entity)
		{
			return this.Level;
		}

		// Token: 0x0600C30A RID: 49930 RVA: 0x0047C74F File Offset: 0x0047A94F
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckPerk(EntityPlayer player, string buffName)
		{
			if (player.Progression.GetProgressionValue(buffName).CalculatedLevel(player) >= this.Level)
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C30B RID: 49931 RVA: 0x0047C77B File Offset: 0x0047A97B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(TwitchRequirementHasProgression.PropSkillName, ref this.SkillName);
			properties.ParseInt(TwitchRequirementHasProgression.PropLevel, ref this.Level);
		}

		// Token: 0x040092EF RID: 37615
		[PublicizedFrom(EAccessModifier.Protected)]
		public string SkillName = "";

		// Token: 0x040092F0 RID: 37616
		[PublicizedFrom(EAccessModifier.Protected)]
		public int Level = 1;

		// Token: 0x040092F1 RID: 37617
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSkillName = "skill_name";

		// Token: 0x040092F2 RID: 37618
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropLevel = "level";
	}
}
