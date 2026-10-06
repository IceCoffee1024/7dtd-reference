using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A30 RID: 6704
	[Preserve]
	public class ActionModifyVarBool : ActionBaseClientAction
	{
		// Token: 0x0600CC32 RID: 52274 RVA: 0x004A93DE File Offset: 0x004A75DE
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			base.Owner.EventVariables.SetEventVariable(this.varName, StringParsers.ParseBool(this.valueText, 0, -1, true));
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CC33 RID: 52275 RVA: 0x004A9405 File Offset: 0x004A7605
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarBool.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarBool.PropVarName, ref this.varName);
		}

		// Token: 0x0600CC34 RID: 52276 RVA: 0x004A9430 File Offset: 0x004A7630
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionModifyVarBool
			{
				varName = this.varName,
				valueText = this.valueText
			};
		}

		// Token: 0x04009AD0 RID: 39632
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x04009AD1 RID: 39633
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x04009AD2 RID: 39634
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x04009AD3 RID: 39635
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";
	}
}
