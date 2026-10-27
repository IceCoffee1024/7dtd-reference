using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001987 RID: 6535
	[Preserve]
	public class RequirementGameStatInt : BaseOperationRequirement
	{
		// Token: 0x0600C94D RID: 51533 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C94E RID: 51534 RVA: 0x00498BE2 File Offset: 0x00496DE2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return GameStats.GetInt(this.GameStat);
		}

		// Token: 0x0600C94F RID: 51535 RVA: 0x00498BF4 File Offset: 0x00496DF4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C950 RID: 51536 RVA: 0x00498C0D File Offset: 0x00496E0D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatInt.PropGameStat, ref this.GameStat);
			properties.ParseString(RequirementGameStatInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C951 RID: 51537 RVA: 0x00498C38 File Offset: 0x00496E38
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementGameStatInt
			{
				Invert = this.Invert,
				operation = this.operation,
				GameStat = this.GameStat,
				valueText = this.valueText
			};
		}

		// Token: 0x0400978A RID: 38794
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x0400978B RID: 38795
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x0400978C RID: 38796
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";

		// Token: 0x0400978D RID: 38797
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
