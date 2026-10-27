using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BB RID: 6587
	[Preserve]
	public class ActionAddPartyToGroup : BaseAction
	{
		// Token: 0x0600CA4A RID: 51786 RVA: 0x0049CCB8 File Offset: 0x0049AEB8
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			EntityPlayer entityPlayer = base.Owner.Target as EntityPlayer;
			if (entityPlayer != null)
			{
				List<Entity> list = new List<Entity>();
				if (entityPlayer.Party != null)
				{
					list.AddRange(entityPlayer.Party.MemberList);
				}
				else
				{
					list.Add(base.Owner.Target);
				}
				if (this.excludeTarget)
				{
					list.Remove(base.Owner.Target);
				}
				if (this.excludeTwitchActive)
				{
					for (int i = list.Count - 1; i >= 0; i--)
					{
						EntityPlayer entityPlayer2 = list[i] as EntityPlayer;
						if (entityPlayer2 != null && entityPlayer2.TwitchEnabled && entityPlayer2 != base.Owner.Target)
						{
							list.RemoveAt(i);
						}
					}
				}
				base.Owner.AddEntitiesToGroup(this.groupName, list, this.twitchNegative);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CA4B RID: 51787 RVA: 0x0049CD90 File Offset: 0x0049AF90
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPartyToGroup.PropGroupName, ref this.groupName);
			properties.ParseBool(ActionAddPartyToGroup.PropTwitchNegative, ref this.twitchNegative);
			properties.ParseBool(ActionAddPartyToGroup.PropExcludeTarget, ref this.excludeTarget);
			properties.ParseBool(ActionAddPartyToGroup.PropExcludeTwitchActive, ref this.excludeTwitchActive);
		}

		// Token: 0x0600CA4C RID: 51788 RVA: 0x0049CDE8 File Offset: 0x0049AFE8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddPartyToGroup
			{
				groupName = this.groupName,
				twitchNegative = this.twitchNegative,
				excludeTarget = this.excludeTarget,
				excludeTwitchActive = this.excludeTwitchActive
			};
		}

		// Token: 0x04009853 RID: 38995
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x04009854 RID: 38996
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x04009855 RID: 38997
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTarget;

		// Token: 0x04009856 RID: 38998
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTwitchActive;

		// Token: 0x04009857 RID: 38999
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x04009858 RID: 39000
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";

		// Token: 0x04009859 RID: 39001
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTarget = "exclude_target";

		// Token: 0x0400985A RID: 39002
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTwitchActive = "exclude_twitch_active";
	}
}
