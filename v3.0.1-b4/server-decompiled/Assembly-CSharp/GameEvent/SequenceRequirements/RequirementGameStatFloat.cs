using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001903 RID: 6403
	[Preserve]
	public class RequirementGameStatFloat : BaseOperationRequirement
	{
		// Token: 0x0600C554 RID: 50516 RVA: 0x0048A8FF File Offset: 0x00488AFF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return GameStats.GetFloat(this.GameStat);
		}

		// Token: 0x0600C555 RID: 50517 RVA: 0x0048A911 File Offset: 0x00488B11
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C556 RID: 50518 RVA: 0x0048A92E File Offset: 0x00488B2E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatFloat.PropGameStat, ref this.GameStat);
			properties.ParseString(RequirementGameStatFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C557 RID: 50519 RVA: 0x0048A959 File Offset: 0x00488B59
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

		// Token: 0x040094AE RID: 38062
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x040094AF RID: 38063
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094B0 RID: 38064
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";

		// Token: 0x040094B1 RID: 38065
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
