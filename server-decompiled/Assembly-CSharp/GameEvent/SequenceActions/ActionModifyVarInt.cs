using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A06 RID: 6662
	[Preserve]
	public class ActionModifyVarInt : ActionBaseClientAction
	{
		// Token: 0x0600CAE4 RID: 51940 RVA: 0x004A50B0 File Offset: 0x004A32B0
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			int intValue = GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
			base.Owner.EventVariables.ModifyEventVariable(this.varName, this.operationType, intValue, this.minValue, this.maxValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAE5 RID: 51941 RVA: 0x004A50FC File Offset: 0x004A32FC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarInt.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarInt.PropVarName, ref this.varName);
			properties.ParseEnum<GameEventVariables.OperationTypes>(ActionModifyVarInt.PropOperation, ref this.operationType);
			properties.ParseInt(ActionModifyVarInt.PropMinValue, ref this.minValue);
			properties.ParseInt(ActionModifyVarInt.PropMaxValue, ref this.maxValue);
		}

		// Token: 0x0600CAE6 RID: 51942 RVA: 0x004A5168 File Offset: 0x004A3368
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

		// Token: 0x04009A14 RID: 39444
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x04009A15 RID: 39445
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x04009A16 RID: 39446
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventVariables.OperationTypes operationType;

		// Token: 0x04009A17 RID: 39447
		[PublicizedFrom(EAccessModifier.Protected)]
		public int minValue = int.MinValue;

		// Token: 0x04009A18 RID: 39448
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxValue = int.MaxValue;

		// Token: 0x04009A19 RID: 39449
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x04009A1A RID: 39450
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009A1B RID: 39451
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x04009A1C RID: 39452
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinValue = "min_value";

		// Token: 0x04009A1D RID: 39453
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxValue = "min_value";
	}
}
