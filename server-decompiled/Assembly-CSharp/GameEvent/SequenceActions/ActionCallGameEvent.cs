using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199E RID: 6558
	[Preserve]
	public class ActionCallGameEvent : BaseAction
	{
		// Token: 0x0600C996 RID: 51606 RVA: 0x0049B6D0 File Offset: 0x004998D0
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

		// Token: 0x0600C997 RID: 51607 RVA: 0x0049B810 File Offset: 0x00499A10
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionCallGameEvent.PropGameEventNames))
			{
				this.gameEventNames.AddRange(properties.Values[ActionCallGameEvent.PropGameEventNames].Split(',', StringSplitOptions.None));
			}
			properties.ParseString(ActionCallGameEvent.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600C998 RID: 51608 RVA: 0x0049B86A File Offset: 0x00499A6A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionCallGameEvent
			{
				gameEventNames = this.gameEventNames,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x040097ED RID: 38893
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<string> gameEventNames = new List<string>();

		// Token: 0x040097EE RID: 38894
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040097EF RID: 38895
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameEventNames = "game_events";

		// Token: 0x040097F0 RID: 38896
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";
	}
}
