using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A04 RID: 6660
	[Preserve]
	public class ActionModifyVarBool : ActionBaseClientAction
	{
		// Token: 0x0600CADA RID: 51930 RVA: 0x004A4F12 File Offset: 0x004A3112
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			base.Owner.EventVariables.SetEventVariable(this.varName, StringParsers.ParseBool(this.valueText, 0, -1, true));
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CADB RID: 51931 RVA: 0x004A4F39 File Offset: 0x004A3139
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarBool.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarBool.PropVarName, ref this.varName);
		}

		// Token: 0x0600CADC RID: 51932 RVA: 0x004A4F64 File Offset: 0x004A3164
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionModifyVarBool
			{
				varName = this.varName,
				valueText = this.valueText
			};
		}

		// Token: 0x04009A0A RID: 39434
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x04009A0B RID: 39435
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x04009A0C RID: 39436
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x04009A0D RID: 39437
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";
	}
}
