using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200195E RID: 6494
	[Preserve]
	public class ActionAddPartyToGroup : BaseAction
	{
		// Token: 0x0600C7DE RID: 51166 RVA: 0x00494DD8 File Offset: 0x00492FD8
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

		// Token: 0x0600C7DF RID: 51167 RVA: 0x00494EB0 File Offset: 0x004930B0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddPartyToGroup.PropGroupName, ref this.groupName);
			properties.ParseBool(ActionAddPartyToGroup.PropTwitchNegative, ref this.twitchNegative);
			properties.ParseBool(ActionAddPartyToGroup.PropExcludeTarget, ref this.excludeTarget);
			properties.ParseBool(ActionAddPartyToGroup.PropExcludeTwitchActive, ref this.excludeTwitchActive);
		}

		// Token: 0x0600C7E0 RID: 51168 RVA: 0x00494F08 File Offset: 0x00493108
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

		// Token: 0x040096A4 RID: 38564
		[PublicizedFrom(EAccessModifier.Protected)]
		public string groupName = "";

		// Token: 0x040096A5 RID: 38565
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool twitchNegative = true;

		// Token: 0x040096A6 RID: 38566
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTarget;

		// Token: 0x040096A7 RID: 38567
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool excludeTwitchActive;

		// Token: 0x040096A8 RID: 38568
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupName = "group_name";

		// Token: 0x040096A9 RID: 38569
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTwitchNegative = "twitch_negative";

		// Token: 0x040096AA RID: 38570
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTarget = "exclude_target";

		// Token: 0x040096AB RID: 38571
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTwitchActive = "exclude_twitch_active";
	}
}
