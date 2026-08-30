using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001939 RID: 6457
	[Preserve]
	public class ActionAddPlayerToGroup : BaseAction
	{
		// Token: 0x0600C65C RID: 50780 RVA: 0x0048EC38 File Offset: 0x0048CE38
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			List<Entity> list = new List<Entity>();
			EntityPlayer entityPlayer = base.Owner.Target as EntityPlayer;
			if (entityPlayer != null)
			{
				if (entityPlayer.Party != null)
				{
					for (int i = 0; i < entityPlayer.Party.MemberList.Count; i++)
					{
						if (entityPlayer.Party.MemberList[i].EntityName.ToLower() == this.playerName.ToLower())
						{
							list.Add(entityPlayer.Party.MemberList[i]);
						}
					}
				}
				else if (entityPlayer.EntityName.ToLower() == this.playerName.ToLower())
				{
					list.Add(base.Owner.Target);
				}
				base.Owner.AddEntitiesToGroup(this.groupName, list, this.twitchNegative);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C65D RID: 50781 RVA: 0x0048ED12 File Offset: 0x0048CF12
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPlayerToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddPlayerToGroup.PropPlayerName, ref this.playerName);
			properties.ParseBool(ActionAddPlayerToGroup.PropTwitchNegative, ref this.twitchNegative);
		}

		// Token: 0x0600C65E RID: 50782 RVA: 0x0048ED4E File Offset: 0x0048CF4E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddPlayerToGroup
			{
				groupName = this.groupName,
				playerName = this.playerName,
				twitchNegative = this.twitchNegative
			};
		}

		// Token: 0x04009583 RID: 38275
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009584 RID: 38276
		[PublicizedFrom(EAccessModifier.Protected)]
		public string playerName = "";

		// Token: 0x04009585 RID: 38277
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x04009586 RID: 38278
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009587 RID: 38279
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPlayerName = "player_name";

		// Token: 0x04009588 RID: 38280
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";
	}
}
