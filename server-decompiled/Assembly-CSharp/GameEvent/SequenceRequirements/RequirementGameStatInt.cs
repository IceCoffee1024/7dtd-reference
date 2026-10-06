using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001956 RID: 6486
	[Preserve]
	public class RequirementGameStatInt : BaseOperationRequirement
	{
		// Token: 0x0600C839 RID: 51257 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C83A RID: 51258 RVA: 0x004951EE File Offset: 0x004933EE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return GameStats.GetInt(this.GameStat);
		}

		// Token: 0x0600C83B RID: 51259 RVA: 0x00495200 File Offset: 0x00493400
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C83C RID: 51260 RVA: 0x00495219 File Offset: 0x00493419
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatInt.PropGameStat, ref this.GameStat);
			properties.ParseString(RequirementGameStatInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C83D RID: 51261 RVA: 0x00495244 File Offset: 0x00493444
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

		// Token: 0x040096A1 RID: 38561
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x040096A2 RID: 38562
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096A3 RID: 38563
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";

		// Token: 0x040096A4 RID: 38564
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
