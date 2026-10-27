using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BC RID: 6588
	[Preserve]
	public class ActionAddPlayerToGroup : BaseAction
	{
		// Token: 0x0600CA4F RID: 51791 RVA: 0x0049CE64 File Offset: 0x0049B064
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

		// Token: 0x0600CA50 RID: 51792 RVA: 0x0049CF3E File Offset: 0x0049B13E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPlayerToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddPlayerToGroup.PropPlayerName, ref this.playerName);
			properties.ParseBool(ActionAddPlayerToGroup.PropTwitchNegative, ref this.twitchNegative);
		}

		// Token: 0x0600CA51 RID: 51793 RVA: 0x0049CF7A File Offset: 0x0049B17A
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

		// Token: 0x0400985B RID: 39003
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x0400985C RID: 39004
		[PublicizedFrom(EAccessModifier.Protected)]
		public string playerName = "";

		// Token: 0x0400985D RID: 39005
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x0400985E RID: 39006
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x0400985F RID: 39007
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPlayerName = "player_name";

		// Token: 0x04009860 RID: 39008
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";
	}
}
