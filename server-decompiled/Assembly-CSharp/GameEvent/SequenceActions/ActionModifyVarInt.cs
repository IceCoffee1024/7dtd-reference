using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DF RID: 6623
	[Preserve]
	public class ActionModifyVarInt : ActionBaseClientAction
	{
		// Token: 0x0600C95B RID: 51547 RVA: 0x0049ECBC File Offset: 0x0049CEBC
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			int intValue = GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
			base.Owner.EventVariables.ModifyEventVariable(this.varName, this.operationType, intValue, this.minValue, this.maxValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C95C RID: 51548 RVA: 0x0049ED08 File Offset: 0x0049CF08
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarInt.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarInt.PropVarName, ref this.varName);
			properties.ParseEnum<GameEventVariables.OperationTypes>(ActionModifyVarInt.PropOperation, ref this.operationType);
			properties.ParseInt(ActionModifyVarInt.PropMinValue, ref this.minValue);
			properties.ParseInt(ActionModifyVarInt.PropMaxValue, ref this.maxValue);
		}

		// Token: 0x0600C95D RID: 51549 RVA: 0x0049ED74 File Offset: 0x0049CF74
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionModifyVarInt
			{
				varName = this.varName,
				valueText = this.valueText,
				operationType = this.operationType,
				minValue = this.minValue,
				maxValue = this.maxValue
			};
		}

		// Token: 0x040098E3 RID: 39139
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x040098E4 RID: 39140
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x040098E5 RID: 39141
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventVariables.OperationTypes operationType;

		// Token: 0x040098E6 RID: 39142
		[PublicizedFrom(EAccessModifier.Protected)]
		public int minValue = int.MinValue;

		// Token: 0x040098E7 RID: 39143
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxValue = int.MaxValue;

		// Token: 0x040098E8 RID: 39144
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x040098E9 RID: 39145
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040098EA RID: 39146
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x040098EB RID: 39147
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinValue = "min_value";

		// Token: 0x040098EC RID: 39148
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxValue = "min_value";
	}
}
