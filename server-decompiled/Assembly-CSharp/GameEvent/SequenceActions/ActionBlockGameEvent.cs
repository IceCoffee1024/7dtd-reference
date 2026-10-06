using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A0D RID: 6669
	[Preserve]
	public class ActionBlockGameEvent : ActionBaseBlockAction
	{
		// Token: 0x0600CB95 RID: 52117 RVA: 0x004A6A0C File Offset: 0x004A4C0C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				GameEventManager.Current.HandleAction(this.gameEventNames[GameEventManager.Current.Random.RandomRange(0, this.gameEventNames.Count)], base.Owner.Requester, base.Owner.Target, false, currentPos, base.Owner.ExtraData, "", false, true, "", null, null);
			}
			return null;
		}

		// Token: 0x0600CB96 RID: 52118 RVA: 0x004A6A8A File Offset: 0x004A4C8A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionBlockGameEvent.PropGameEventNames))
			{
				this.gameEventNames.AddRange(properties.Values[ActionBlockGameEvent.PropGameEventNames].Split(',', StringSplitOptions.None));
			}
		}

		// Token: 0x0600CB97 RID: 52119 RVA: 0x004A6AC8 File Offset: 0x004A4CC8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockGameEvent
			{
				gameEventNames = this.gameEventNames
			};
		}

		// Token: 0x04009A56 RID: 39510
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<string> gameEventNames = new List<string>();

		// Token: 0x04009A57 RID: 39511
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameEventNames = "game_events";
	}
}
