using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001955 RID: 6485
	[Preserve]
	public class RequirementGameStatFloat : BaseOperationRequirement
	{
		// Token: 0x0600C833 RID: 51251 RVA: 0x00495137 File Offset: 0x00493337
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return GameStats.GetFloat(this.GameStat);
		}

		// Token: 0x0600C834 RID: 51252 RVA: 0x00495149 File Offset: 0x00493349
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C835 RID: 51253 RVA: 0x00495166 File Offset: 0x00493366
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatFloat.PropGameStat, ref this.GameStat);
			properties.ParseString(RequirementGameStatFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C836 RID: 51254 RVA: 0x00495191 File Offset: 0x00493391
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementGameStatFloat
			{
				Invert = this.Invert,
				operation = this.operation,
				GameStat = this.GameStat,
				valueText = this.valueText
			};
		}

		// Token: 0x0400969D RID: 38557
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x0400969E RID: 38558
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x0400969F RID: 38559
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";

		// Token: 0x040096A0 RID: 38560
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
