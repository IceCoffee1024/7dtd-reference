using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BA RID: 6586
	[Preserve]
	public class ActionBlockGameEvent : ActionBaseBlockAction
	{
		// Token: 0x0600C8B4 RID: 51380 RVA: 0x0049C174 File Offset: 0x0049A374
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				GameEventManager.Current.HandleAction(this.gameEventNames[GameEventManager.Current.Random.RandomRange(0, this.gameEventNames.Count)], base.Owner.Requester, base.Owner.Target, false, currentPos, base.Owner.ExtraData, "", false, true, "", null, null);
			}
			return null;
		}

		// Token: 0x0600C8B5 RID: 51381 RVA: 0x0049C1F2 File Offset: 0x0049A3F2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionBlockGameEvent.PropGameEventNames))
			{
				this.gameEventNames.AddRange(properties.Values[ActionBlockGameEvent.PropGameEventNames].Split(',', StringSplitOptions.None));
			}
		}

		// Token: 0x0600C8B6 RID: 51382 RVA: 0x0049C230 File Offset: 0x0049A430
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockGameEvent
			{
				gameEventNames = this.gameEventNames
			};
		}

		// Token: 0x0400985F RID: 39007
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<string> gameEventNames = new List<string>();

		// Token: 0x04009860 RID: 39008
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameEventNames = "game_events";
	}
}
