using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001929 RID: 6441
	[Preserve]
	public class RequirementGameStatFloat : BaseOperationRequirement
	{
		// Token: 0x0600C6DB RID: 50907 RVA: 0x00490C4B File Offset: 0x0048EE4B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return GameStats.GetFloat(this.GameStat);
		}

		// Token: 0x0600C6DC RID: 50908 RVA: 0x00490C5D File Offset: 0x0048EE5D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C6DD RID: 50909 RVA: 0x00490C7A File Offset: 0x0048EE7A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatFloat.PropGameStat, ref this.GameStat);
			properties.ParseString(RequirementGameStatFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C6DE RID: 50910 RVA: 0x00490CA5 File Offset: 0x0048EEA5
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

		// Token: 0x040095D7 RID: 38359
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x040095D8 RID: 38360
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040095D9 RID: 38361
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";

		// Token: 0x040095DA RID: 38362
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
