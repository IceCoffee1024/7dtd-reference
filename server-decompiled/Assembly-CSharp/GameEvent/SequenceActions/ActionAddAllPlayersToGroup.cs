using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B3 RID: 6579
	[Preserve]
	public class ActionAddAllPlayersToGroup : BaseAction
	{
		// Token: 0x0600CA24 RID: 51748 RVA: 0x0049BA58 File Offset: 0x00499C58
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

		// Token: 0x0600CA25 RID: 51749 RVA: 0x0049BAD8 File Offset: 0x00499CD8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddAllPlayersToGroup.PropGroupName, ref this.groupName);
			properties.ParseBool(ActionAddAllPlayersToGroup.PropTwitchNegative, ref this.twitchNegative);
		}

		// Token: 0x0600CA26 RID: 51750 RVA: 0x0049BB03 File Offset: 0x00499D03
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddAllPlayersToGroup
			{
				groupName = this.groupName,
				twitchNegative = this.twitchNegative
			};
		}

		// Token: 0x04009812 RID: 38930
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009813 RID: 38931
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x04009814 RID: 38932
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009815 RID: 38933
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";
	}
}
