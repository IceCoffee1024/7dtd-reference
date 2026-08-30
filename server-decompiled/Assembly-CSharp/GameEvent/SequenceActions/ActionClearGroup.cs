using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001973 RID: 6515
	[Preserve]
	public class ActionClearGroup : BaseAction
	{
		// Token: 0x0600C843 RID: 51267 RVA: 0x004973E1 File Offset: 0x004955E1
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			base.Owner.ClearEntityGroup(this.groupName);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C844 RID: 51268 RVA: 0x004973F5 File Offset: 0x004955F5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionClearGroup.PropGroupName, ref this.groupName);
		}

		// Token: 0x0600C845 RID: 51269 RVA: 0x0049740F File Offset: 0x0049560F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionClearGroup
			{
				groupName = this.groupName
			};
		}

		// Token: 0x0400972B RID: 38699
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x0400972C RID: 38700
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";
	}
}
