using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E1 RID: 6625
	[Preserve]
	public class ActionBlockGameEvent : ActionBaseBlockAction
	{
		// Token: 0x0600CA3D RID: 51773 RVA: 0x004A2568 File Offset: 0x004A0768
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				GameEventManager.Current.HandleAction(this.gameEventNames[GameEventManager.Current.Random.RandomRange(0, this.gameEventNames.Count)], base.Owner.Requester, base.Owner.Target, false, currentPos, base.Owner.ExtraData, "", false, true, "", null, null);
			}
			return null;
		}

		// Token: 0x0600CA3E RID: 51774 RVA: 0x004A25E6 File Offset: 0x004A07E6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionBlockGameEvent.PropGameEventNames))
			{
				this.gameEventNames.AddRange(properties.Values[ActionBlockGameEvent.PropGameEventNames].Split(',', StringSplitOptions.None));
			}
		}

		// Token: 0x0600CA3F RID: 51775 RVA: 0x004A2624 File Offset: 0x004A0824
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockGameEvent
			{
				gameEventNames = this.gameEventNames
			};
		}

		// Token: 0x04009990 RID: 39312
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<string> gameEventNames = new List<string>();

		// Token: 0x04009991 RID: 39313
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGameEventNames = "game_events";
	}
}
