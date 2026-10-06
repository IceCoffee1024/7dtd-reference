using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198A RID: 6538
	[Preserve]
	public class ActionAddPartyToGroup : BaseAction
	{
		// Token: 0x0600C936 RID: 51510 RVA: 0x004992C4 File Offset: 0x004974C4
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

		// Token: 0x0600C937 RID: 51511 RVA: 0x0049939C File Offset: 0x0049759C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPartyToGroup.PropGroupName, ref this.groupName);
			properties.ParseBool(ActionAddPartyToGroup.PropTwitchNegative, ref this.twitchNegative);
			properties.ParseBool(ActionAddPartyToGroup.PropExcludeTarget, ref this.excludeTarget);
			properties.ParseBool(ActionAddPartyToGroup.PropExcludeTwitchActive, ref this.excludeTwitchActive);
		}

		// Token: 0x0600C938 RID: 51512 RVA: 0x004993F4 File Offset: 0x004975F4
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

		// Token: 0x0400976A RID: 38762
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x0400976B RID: 38763
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x0400976C RID: 38764
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTarget;

		// Token: 0x0400976D RID: 38765
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTwitchActive;

		// Token: 0x0400976E RID: 38766
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x0400976F RID: 38767
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";

		// Token: 0x04009770 RID: 38768
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTarget = "exclude_target";

		// Token: 0x04009771 RID: 38769
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTwitchActive = "exclude_twitch_active";
	}
}
