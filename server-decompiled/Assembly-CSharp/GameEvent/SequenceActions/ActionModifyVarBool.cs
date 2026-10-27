using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A61 RID: 6753
	[Preserve]
	public class ActionModifyVarBool : ActionBaseClientAction
	{
		// Token: 0x0600CD46 RID: 52550 RVA: 0x004ACEDA File Offset: 0x004AB0DA
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			base.Owner.EventVariables.SetEventVariable(this.varName, StringParsers.ParseBool(this.valueText, 0, -1, true));
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CD47 RID: 52551 RVA: 0x004ACF01 File Offset: 0x004AB101
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyVarBool.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyVarBool.PropVarName, ref this.varName);
		}

		// Token: 0x0600CD48 RID: 52552 RVA: 0x004ACF2C File Offset: 0x004AB12C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionModifyVarBool
			{
				varName = this.varName,
				valueText = this.valueText
			};
		}

		// Token: 0x04009BB9 RID: 39865
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText = "";

		// Token: 0x04009BBA RID: 39866
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName = "";

		// Token: 0x04009BBB RID: 39867
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x04009BBC RID: 39868
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";
	}
}
