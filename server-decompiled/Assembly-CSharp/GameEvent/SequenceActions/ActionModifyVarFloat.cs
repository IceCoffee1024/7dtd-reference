using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A62 RID: 6754
	[Preserve]
	public class ActionModifyVarFloat : ActionBaseClientAction
	{
		// Token: 0x0600CD4B RID: 52555 RVA: 0x004ACF80 File Offset: 0x004AB180
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			float floatValue = GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
			base.Owner.EventVariables.ModifyEventVariable(this.varName, this.operationType, floatValue, float.MinValue, float.MaxValue);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CD4C RID: 52556 RVA: 0x004ACFD2 File Offset: 0x004AB1D2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarFloat.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarFloat.PropVarName, ref this.varName);
			properties.ParseEnum<GameEventVariables.OperationTypes>(ActionModifyVarFloat.PropOperation, ref this.operationType);
		}

		// Token: 0x0600CD4D RID: 52557 RVA: 0x004AD00E File Offset: 0x004AB20E
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

		// Token: 0x04009BBD RID: 39869
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x04009BBE RID: 39870
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x04009BBF RID: 39871
		[PublicizedFrom(EAccessModifier.Protected)]
		public GameEventVariables.OperationTypes operationType;

		// Token: 0x04009BC0 RID: 39872
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x04009BC1 RID: 39873
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009BC2 RID: 39874
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";
	}
}
