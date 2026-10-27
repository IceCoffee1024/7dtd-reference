using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001986 RID: 6534
	[Preserve]
	public class RequirementGameStatFloat : BaseOperationRequirement
	{
		// Token: 0x0600C947 RID: 51527 RVA: 0x00498B2B File Offset: 0x00496D2B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return GameStats.GetFloat(this.GameStat);
		}

		// Token: 0x0600C948 RID: 51528 RVA: 0x00498B3D File Offset: 0x00496D3D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C949 RID: 51529 RVA: 0x00498B5A File Offset: 0x00496D5A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<EnumGameStats>(RequirementGameStatFloat.PropGameStat, ref this.GameStat);
			properties.ParseString(RequirementGameStatFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C94A RID: 51530 RVA: 0x00498B85 File Offset: 0x00496D85
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

		// Token: 0x04009786 RID: 38790
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGameStats GameStat = EnumGameStats.AnimalCount;

		// Token: 0x04009787 RID: 38791
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009788 RID: 38792
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameStat = "gamestat";

		// Token: 0x04009789 RID: 38793
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
