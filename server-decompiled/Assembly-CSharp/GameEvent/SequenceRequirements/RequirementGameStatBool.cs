using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001954 RID: 6484
	[Preserve]
	public class RequirementGameStatBool : BaseRequirement
	{
		// Token: 0x0600C82D RID: 51245 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C82E RID: 51246 RVA: 0x004950C3 File Offset: 0x004932C3
		public override bool CanPerform(Entity target)
		{
			if (GameStats.GetBool(this.GameStat))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C82F RID: 51247 RVA: 0x004950E2 File Offset: 0x004932E2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatBool.PropGameStat, ref this.GameStat);
		}

		// Token: 0x0600C830 RID: 51248 RVA: 0x004950FC File Offset: 0x004932FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementGameStatBool
			{
				Invert = this.Invert,
				GameStat = this.GameStat
			};
		}

		// Token: 0x0400969B RID: 38555
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x0400969C RID: 38556
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";
	}
}
