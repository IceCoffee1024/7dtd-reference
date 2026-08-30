using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DE RID: 6622
	[Preserve]
	public class ActionModifyVarFloat : ActionBaseClientAction
	{
		// Token: 0x0600C956 RID: 51542 RVA: 0x0049EBC4 File Offset: 0x0049CDC4
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			float floatValue = GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
			base.Owner.EventVariables.ModifyEventVariable(this.varName, this.operationType, floatValue, float.MinValue, float.MaxValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C957 RID: 51543 RVA: 0x0049EC16 File Offset: 0x0049CE16
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarFloat.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarFloat.PropVarName, ref this.varName);
			properties.ParseEnum<GameEventVariables.OperationTypes>(ActionModifyVarFloat.PropOperation, ref this.operationType);
		}

		// Token: 0x0600C958 RID: 51544 RVA: 0x0049EC52 File Offset: 0x0049CE52
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

		// Token: 0x040098DD RID: 39133
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x040098DE RID: 39134
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x040098DF RID: 39135
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventVariables.OperationTypes operationType;

		// Token: 0x040098E0 RID: 39136
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x040098E1 RID: 39137
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040098E2 RID: 39138
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";
	}
}
