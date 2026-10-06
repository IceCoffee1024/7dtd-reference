using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198B RID: 6539
	[Preserve]
	public class ActionAddPlayerToGroup : BaseAction
	{
		// Token: 0x0600C93B RID: 51515 RVA: 0x00499470 File Offset: 0x00497670
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

		// Token: 0x0600C93C RID: 51516 RVA: 0x0049954A File Offset: 0x0049774A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPlayerToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddPlayerToGroup.PropPlayerName, ref this.playerName);
			properties.ParseBool(ActionAddPlayerToGroup.PropTwitchNegative, ref this.twitchNegative);
		}

		// Token: 0x0600C93D RID: 51517 RVA: 0x00499586 File Offset: 0x00497786
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

		// Token: 0x04009772 RID: 38770
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009773 RID: 38771
		[PublicizedFrom(EAccessModifier.Protected)]
		public string playerName = "";

		// Token: 0x04009774 RID: 38772
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x04009775 RID: 38773
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009776 RID: 38774
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPlayerName = "player_name";

		// Token: 0x04009777 RID: 38775
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";
	}
}
