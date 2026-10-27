using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A63 RID: 6755
	[Preserve]
	public class ActionModifyVarInt : ActionBaseClientAction
	{
		// Token: 0x0600CD50 RID: 52560 RVA: 0x004AD078 File Offset: 0x004AB278
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			int intValue = GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
			base.Owner.EventVariables.ModifyEventVariable(this.varName, this.operationType, intValue, this.minValue, this.maxValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CD51 RID: 52561 RVA: 0x004AD0C4 File Offset: 0x004AB2C4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarInt.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarInt.PropVarName, ref this.varName);
			properties.ParseEnum<GameEventVariables.OperationTypes>(ActionModifyVarInt.PropOperation, ref this.operationType);
			properties.ParseInt(ActionModifyVarInt.PropMinValue, ref this.minValue);
			properties.ParseInt(ActionModifyVarInt.PropMaxValue, ref this.maxValue);
		}

		// Token: 0x0600CD52 RID: 52562 RVA: 0x004AD130 File Offset: 0x004AB330
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

		// Token: 0x04009BC3 RID: 39875
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x04009BC4 RID: 39876
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x04009BC5 RID: 39877
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventVariables.OperationTypes operationType;

		// Token: 0x04009BC6 RID: 39878
		[PublicizedFrom(EAccessModifier.Protected)]
		public int minValue = int.MinValue;

		// Token: 0x04009BC7 RID: 39879
		[PublicizedFrom(EAccessModifier.Protected)]
		public int maxValue = int.MaxValue;

		// Token: 0x04009BC8 RID: 39880
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x04009BC9 RID: 39881
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009BCA RID: 39882
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x04009BCB RID: 39883
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinValue = "min_value";

		// Token: 0x04009BCC RID: 39884
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxValue = "min_value";
	}
}
