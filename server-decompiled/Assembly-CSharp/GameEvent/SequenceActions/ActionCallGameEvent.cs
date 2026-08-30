using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001972 RID: 6514
	[Preserve]
	public class ActionCallGameEvent : BaseAction
	{
		// Token: 0x0600C83E RID: 51262 RVA: 0x004971F4 File Offset: 0x004953F4
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameEventActionSequence gameEventActionSequence = ((base.Owner.OwnerSequence != null) ? base.Owner.OwnerSequence : base.Owner);
			if (this.targetGroup != "")
			{
				List<Entity> entityGroup = base.Owner.GetEntityGroup(this.targetGroup);
				if (entityGroup != null)
				{
					for (int i = 0; i < entityGroup.Count; i++)
					{
						GameEventManager.Current.HandleAction(this.gameEventNames[GameEventManager.Current.Random.RandomRange(0, this.gameEventNames.Count)], base.Owner.Requester, entityGroup[i], base.Owner.TwitchActivated, base.Owner.ExtraData, "", false, true, "", gameEventActionSequence);
					}
				}
			}
			else
			{
				GameEventManager.Current.HandleAction(this.gameEventNames[GameEventManager.Current.Random.RandomRange(0, this.gameEventNames.Count)], base.Owner.Requester, base.Owner.Target, base.Owner.TwitchActivated, base.Owner.ExtraData, "", false, true, "", gameEventActionSequence);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C83F RID: 51263 RVA: 0x00497334 File Offset: 0x00495534
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionCallGameEvent.PropGameEventNames))
			{
				this.gameEventNames.AddRange(properties.Values[ActionCallGameEvent.PropGameEventNames].Split(',', StringSplitOptions.None));
			}
			properties.ParseString(ActionCallGameEvent.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600C840 RID: 51264 RVA: 0x0049738E File Offset: 0x0049558E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionCallGameEvent
			{
				gameEventNames = this.gameEventNames,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x04009727 RID: 38695
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<string> gameEventNames = new List<string>();

		// Token: 0x04009728 RID: 38696
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x04009729 RID: 38697
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameEventNames = "game_events";

		// Token: 0x0400972A RID: 38698
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";
	}
}
