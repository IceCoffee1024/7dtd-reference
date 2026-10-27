using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D0 RID: 6608
	[Preserve]
	public class ActionClearGroup : BaseAction
	{
		// Token: 0x0600CAAF RID: 51887 RVA: 0x0049F3BD File Offset: 0x0049D5BD
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			base.Owner.ClearEntityGroup(this.groupName);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAB0 RID: 51888 RVA: 0x0049F3D1 File Offset: 0x0049D5D1
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionClearGroup.PropGroupName, ref this.groupName);
		}

		// Token: 0x0600CAB1 RID: 51889 RVA: 0x0049F3EB File Offset: 0x0049D5EB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionClearGroup
			{
				groupName = this.groupName
			};
		}

		// Token: 0x040098DA RID: 39130
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x040098DB RID: 39131
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";
	}
}
