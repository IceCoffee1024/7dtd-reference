using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DD RID: 6621
	[Preserve]
	public class ActionModifyVarBool : ActionBaseClientAction
	{
		// Token: 0x0600C951 RID: 51537 RVA: 0x0049EB1E File Offset: 0x0049CD1E
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			base.Owner.EventVariables.SetEventVariable(this.varName, StringParsers.ParseBool(this.valueText, 0, -1, true));
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C952 RID: 51538 RVA: 0x0049EB45 File Offset: 0x0049CD45
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarBool.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarBool.PropVarName, ref this.varName);
		}

		// Token: 0x0600C953 RID: 51539 RVA: 0x0049EB70 File Offset: 0x0049CD70
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionModifyVarBool
			{
				varName = this.varName,
				valueText = this.valueText
			};
		}

		// Token: 0x040098D9 RID: 39129
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x040098DA RID: 39130
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x040098DB RID: 39131
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x040098DC RID: 39132
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";
	}
}
