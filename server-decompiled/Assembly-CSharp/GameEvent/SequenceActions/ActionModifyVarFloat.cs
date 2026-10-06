using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A31 RID: 6705
	[Preserve]
	public class ActionModifyVarFloat : ActionBaseClientAction
	{
		// Token: 0x0600CC37 RID: 52279 RVA: 0x004A9484 File Offset: 0x004A7684
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			float floatValue = GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
			base.Owner.EventVariables.ModifyEventVariable(this.varName, this.operationType, floatValue, float.MinValue, float.MaxValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CC38 RID: 52280 RVA: 0x004A94D6 File Offset: 0x004A76D6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarFloat.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarFloat.PropVarName, ref this.varName);
			properties.ParseEnum<GameEventVariables.OperationTypes>(ActionModifyVarFloat.PropOperation, ref this.operationType);
		}

		// Token: 0x0600CC39 RID: 52281 RVA: 0x004A9512 File Offset: 0x004A7712
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

		// Token: 0x04009AD4 RID: 39636
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x04009AD5 RID: 39637
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x04009AD6 RID: 39638
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventVariables.OperationTypes operationType;

		// Token: 0x04009AD7 RID: 39639
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x04009AD8 RID: 39640
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009AD9 RID: 39641
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";
	}
}
