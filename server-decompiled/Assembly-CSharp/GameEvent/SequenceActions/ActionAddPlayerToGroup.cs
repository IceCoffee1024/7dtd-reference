using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200195F RID: 6495
	[Preserve]
	public class ActionAddPlayerToGroup : BaseAction
	{
		// Token: 0x0600C7E3 RID: 51171 RVA: 0x00494F84 File Offset: 0x00493184
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

		// Token: 0x0600C7E4 RID: 51172 RVA: 0x0049505E File Offset: 0x0049325E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPlayerToGroup.PropGroupName, ref this.groupName);
			properties.ParseString(ActionAddPlayerToGroup.PropPlayerName, ref this.playerName);
			properties.ParseBool(ActionAddPlayerToGroup.PropTwitchNegative, ref this.twitchNegative);
		}

		// Token: 0x0600C7E5 RID: 51173 RVA: 0x0049509A File Offset: 0x0049329A
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

		// Token: 0x040096AC RID: 38572
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x040096AD RID: 38573
		[PublicizedFrom(EAccessModifier.Protected)]
		public string playerName = "";

		// Token: 0x040096AE RID: 38574
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x040096AF RID: 38575
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x040096B0 RID: 38576
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPlayerName = "player_name";

		// Token: 0x040096B1 RID: 38577
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";
	}
}
