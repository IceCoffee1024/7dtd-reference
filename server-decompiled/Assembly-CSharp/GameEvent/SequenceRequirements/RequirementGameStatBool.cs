using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001985 RID: 6533
	[Preserve]
	public class RequirementGameStatBool : BaseRequirement
	{
		// Token: 0x0600C941 RID: 51521 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C942 RID: 51522 RVA: 0x00498AB7 File Offset: 0x00496CB7
		public override bool CanPerform(Entity target)
		{
			if (GameStats.GetBool(this.GameStat))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C943 RID: 51523 RVA: 0x00498AD6 File Offset: 0x00496CD6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatBool.PropGameStat, ref this.GameStat);
		}

		// Token: 0x0600C944 RID: 51524 RVA: 0x00498AF0 File Offset: 0x00496CF0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementGameStatBool
			{
				Invert = this.Invert,
				GameStat = this.GameStat
			};
		}

		// Token: 0x04009784 RID: 38788
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x04009785 RID: 38789
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";
	}
}
