using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A32 RID: 6706
	[Preserve]
	public class ActionModifyVarInt : ActionBaseClientAction
	{
		// Token: 0x0600CC3C RID: 52284 RVA: 0x004A957C File Offset: 0x004A777C
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			int intValue = GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
			base.Owner.EventVariables.ModifyEventVariable(this.varName, this.operationType, intValue, this.minValue, this.maxValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CC3D RID: 52285 RVA: 0x004A95C8 File Offset: 0x004A77C8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarInt.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarInt.PropVarName, ref this.varName);
			properties.ParseEnum<GameEventVariables.OperationTypes>(ActionModifyVarInt.PropOperation, ref this.operationType);
			properties.ParseInt(ActionModifyVarInt.PropMinValue, ref this.minValue);
			properties.ParseInt(ActionModifyVarInt.PropMaxValue, ref this.maxValue);
		}

		// Token: 0x0600CC3E RID: 52286 RVA: 0x004A9634 File Offset: 0x004A7834
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

		// Token: 0x04009ADA RID: 39642
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x04009ADB RID: 39643
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x04009ADC RID: 39644
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventVariables.OperationTypes operationType;

		// Token: 0x04009ADD RID: 39645
		[PublicizedFrom(EAccessModifier.Protected)]
		public int minValue = int.MinValue;

		// Token: 0x04009ADE RID: 39646
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxValue = int.MaxValue;

		// Token: 0x04009ADF RID: 39647
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x04009AE0 RID: 39648
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009AE1 RID: 39649
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x04009AE2 RID: 39650
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinValue = "min_value";

		// Token: 0x04009AE3 RID: 39651
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxValue = "min_value";
	}
}
