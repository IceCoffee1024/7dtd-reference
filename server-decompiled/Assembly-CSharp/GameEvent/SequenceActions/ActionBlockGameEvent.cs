using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A3E RID: 6718
	[Preserve]
	public class ActionBlockGameEvent : ActionBaseBlockAction
	{
		// Token: 0x0600CCA9 RID: 52393 RVA: 0x004AA508 File Offset: 0x004A8708
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				GameEventManager.Current.HandleAction(this.gameEventNames[GameEventManager.Current.Random.RandomRange(0, this.gameEventNames.Count)], base.Owner.Requester, base.Owner.Target, false, currentPos, base.Owner.ExtraData, "", false, true, "", null, null);
			}
			return null;
		}

		// Token: 0x0600CCAA RID: 52394 RVA: 0x004AA586 File Offset: 0x004A8786
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionBlockGameEvent.PropGameEventNames))
			{
				this.gameEventNames.AddRange(properties.Values[ActionBlockGameEvent.PropGameEventNames].Split(',', StringSplitOptions.None));
			}
		}

		// Token: 0x0600CCAB RID: 52395 RVA: 0x004AA5C4 File Offset: 0x004A87C4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockGameEvent
			{
				gameEventNames = this.gameEventNames
			};
		}

		// Token: 0x04009B3F RID: 39743
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<string> gameEventNames = new List<string>();

		// Token: 0x04009B40 RID: 39744
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameEventNames = "game_events";
	}
}
