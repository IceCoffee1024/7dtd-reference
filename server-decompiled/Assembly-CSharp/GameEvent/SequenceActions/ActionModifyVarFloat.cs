using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A05 RID: 6661
	[Preserve]
	public class ActionModifyVarFloat : ActionBaseClientAction
	{
		// Token: 0x0600CADF RID: 51935 RVA: 0x004A4FB8 File Offset: 0x004A31B8
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			float floatValue = GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
			base.Owner.EventVariables.ModifyEventVariable(this.varName, this.operationType, floatValue, float.MinValue, float.MaxValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAE0 RID: 51936 RVA: 0x004A500A File Offset: 0x004A320A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarFloat.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarFloat.PropVarName, ref this.varName);
			properties.ParseEnum<GameEventVariables.OperationTypes>(ActionModifyVarFloat.PropOperation, ref this.operationType);
		}

		// Token: 0x0600CAE1 RID: 51937 RVA: 0x004A5046 File Offset: 0x004A3246
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionModifyVarFloat
			{
				varName = this.varName,
				valueText = this.valueText,
				operationType = this.operationType
			};
		}

		// Token: 0x04009A0E RID: 39438
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x04009A0F RID: 39439
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x04009A10 RID: 39440
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventVariables.OperationTypes operationType;

		// Token: 0x04009A11 RID: 39441
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x04009A12 RID: 39442
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009A13 RID: 39443
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";
	}
}
