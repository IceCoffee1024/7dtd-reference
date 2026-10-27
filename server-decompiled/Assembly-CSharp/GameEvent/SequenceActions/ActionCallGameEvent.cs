using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CF RID: 6607
	[Preserve]
	public class ActionCallGameEvent : BaseAction
	{
		// Token: 0x0600CAAA RID: 51882 RVA: 0x0049F1D0 File Offset: 0x0049D3D0
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

		// Token: 0x0600CAAB RID: 51883 RVA: 0x0049F310 File Offset: 0x0049D510
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionCallGameEvent.PropGameEventNames))
			{
				this.gameEventNames.AddRange(properties.Values[ActionCallGameEvent.PropGameEventNames].Split(',', StringSplitOptions.None));
			}
			properties.ParseString(ActionCallGameEvent.PropTargetGroup, ref this.targetGroup);
		}

		// Token: 0x0600CAAC RID: 51884 RVA: 0x0049F36A File Offset: 0x0049D56A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionCallGameEvent
			{
				gameEventNames = this.gameEventNames,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x040098D6 RID: 39126
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<string> gameEventNames = new List<string>();

		// Token: 0x040098D7 RID: 39127
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040098D8 RID: 39128
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameEventNames = "game_events";

		// Token: 0x040098D9 RID: 39129
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";
	}
}
