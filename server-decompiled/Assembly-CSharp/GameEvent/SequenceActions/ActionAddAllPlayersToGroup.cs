using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001982 RID: 6530
	[Preserve]
	public class ActionAddAllPlayersToGroup : BaseAction
	{
		// Token: 0x0600C910 RID: 51472 RVA: 0x00498064 File Offset: 0x00496264
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

		// Token: 0x0600C911 RID: 51473 RVA: 0x004980E4 File Offset: 0x004962E4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddAllPlayersToGroup.PropGroupName, ref this.groupName);
			properties.ParseBool(ActionAddAllPlayersToGroup.PropTwitchNegative, ref this.twitchNegative);
		}

		// Token: 0x0600C912 RID: 51474 RVA: 0x0049810F File Offset: 0x0049630F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddAllPlayersToGroup
			{
				groupName = this.groupName,
				twitchNegative = this.twitchNegative
			};
		}

		// Token: 0x04009729 RID: 38697
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x0400972A RID: 38698
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x0400972B RID: 38699
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x0400972C RID: 38700
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";
	}
}
