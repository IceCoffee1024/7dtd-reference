using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001956 RID: 6486
	[Preserve]
	public class ActionAddAllPlayersToGroup : BaseAction
	{
		// Token: 0x0600C7B8 RID: 51128 RVA: 0x00493B78 File Offset: 0x00491D78
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			List<Entity> list = new List<Entity>();
			foreach (EntityPlayer entityPlayer in GameManager.Instance.World.Players.list)
			{
				list.Add(entityPlayer);
			}
			base.Owner.AddEntitiesToGroup(this.groupName, list, this.twitchNegative);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C7B9 RID: 51129 RVA: 0x00493BF8 File Offset: 0x00491DF8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddAllPlayersToGroup.PropGroupName, ref this.groupName);
			properties.ParseBool(ActionAddAllPlayersToGroup.PropTwitchNegative, ref this.twitchNegative);
		}

		// Token: 0x0600C7BA RID: 51130 RVA: 0x00493C23 File Offset: 0x00491E23
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddAllPlayersToGroup
			{
				groupName = this.groupName,
				twitchNegative = this.twitchNegative
			};
		}

		// Token: 0x04009663 RID: 38499
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009664 RID: 38500
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x04009665 RID: 38501
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009666 RID: 38502
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";
	}
}
