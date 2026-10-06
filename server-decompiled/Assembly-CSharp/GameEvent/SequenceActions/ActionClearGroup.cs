using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199F RID: 6559
	[Preserve]
	public class ActionClearGroup : BaseAction
	{
		// Token: 0x0600C99B RID: 51611 RVA: 0x0049B8BD File Offset: 0x00499ABD
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			base.Owner.ClearEntityGroup(this.groupName);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C99C RID: 51612 RVA: 0x0049B8D1 File Offset: 0x00499AD1
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionClearGroup.PropGroupName, ref this.groupName);
		}

		// Token: 0x0600C99D RID: 51613 RVA: 0x0049B8EB File Offset: 0x00499AEB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionClearGroup
			{
				groupName = this.groupName
			};
		}

		// Token: 0x040097F1 RID: 38897
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x040097F2 RID: 38898
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";
	}
}
