using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001928 RID: 6440
	[Preserve]
	public class RequirementGameStatBool : BaseRequirement
	{
		// Token: 0x0600C6D5 RID: 50901 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C6D6 RID: 50902 RVA: 0x00490BD7 File Offset: 0x0048EDD7
		public override bool CanPerform(Entity target)
		{
			if (GameStats.GetBool(this.GameStat))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C6D7 RID: 50903 RVA: 0x00490BF6 File Offset: 0x0048EDF6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatBool.PropGameStat, ref this.GameStat);
		}

		// Token: 0x0600C6D8 RID: 50904 RVA: 0x00490C10 File Offset: 0x0048EE10
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementGameStatBool
			{
				Invert = this.Invert,
				GameStat = this.GameStat
			};
		}

		// Token: 0x040095D5 RID: 38357
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x040095D6 RID: 38358
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";
	}
}
