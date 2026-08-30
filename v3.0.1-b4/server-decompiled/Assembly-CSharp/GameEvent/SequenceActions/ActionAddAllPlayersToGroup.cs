using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001930 RID: 6448
	[Preserve]
	public class ActionAddAllPlayersToGroup : BaseAction
	{
		// Token: 0x0600C631 RID: 50737 RVA: 0x0048D82C File Offset: 0x0048BA2C
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

		// Token: 0x0600C632 RID: 50738 RVA: 0x0048D8AC File Offset: 0x0048BAAC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddAllPlayersToGroup.PropGroupName, ref this.groupName);
			properties.ParseBool(ActionAddAllPlayersToGroup.PropTwitchNegative, ref this.twitchNegative);
		}

		// Token: 0x0600C633 RID: 50739 RVA: 0x0048D8D7 File Offset: 0x0048BAD7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddAllPlayersToGroup
			{
				groupName = this.groupName,
				twitchNegative = this.twitchNegative
			};
		}

		// Token: 0x0400953A RID: 38202
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x0400953B RID: 38203
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x0400953C RID: 38204
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x0400953D RID: 38205
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";
	}
}
