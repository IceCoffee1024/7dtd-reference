using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A26 RID: 6694
	[Preserve]
	public class ActionStartHomerun : ActionBaseTargetAction
	{
		// Token: 0x0600CC2D RID: 52269 RVA: 0x004A8470 File Offset: 0x004A6670
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				float floatValue = GameEventManager.GetFloatValue(entityPlayer, this.gameTimeText, 120f);
				GameEventManager.Current.HomerunManager.AddPlayerToHomerun(entityPlayer, this.rewardLevels, this.rewardEvents, floatValue, new Action(this.HomeRunComplete));
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CC2E RID: 52270 RVA: 0x004A84C4 File Offset: 0x004A66C4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			for (int i = 1; i <= 5; i++)
			{
				string text = string.Format("reward_level_{0}", i);
				string text2 = string.Format("reward_event_{0}", i);
				if (this.Properties.Contains(text) && this.Properties.Contains(text2))
				{
					this.rewardLevels.Add(StringParsers.ParseSInt32(this.Properties.Values[text], 0, -1, NumberStyles.Integer));
					this.rewardEvents.Add(this.Properties.Values[text2]);
				}
			}
			this.Properties.ParseString(ActionStartHomerun.PropDuration, ref this.gameTimeText);
		}

		// Token: 0x0600CC2F RID: 52271 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void HomeRunComplete()
		{
		}

		// Token: 0x0600CC30 RID: 52272 RVA: 0x004A857E File Offset: 0x004A677E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionStartHomerun
			{
				targetGroup = this.targetGroup,
				rewardEvents = this.rewardEvents,
				rewardLevels = this.rewardLevels,
				gameTimeText = this.gameTimeText
			};
		}

		// Token: 0x04009AB2 RID: 39602
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> rewardLevels = new List<int>();

		// Token: 0x04009AB3 RID: 39603
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> rewardEvents = new List<string>();

		// Token: 0x04009AB4 RID: 39604
		[PublicizedFrom(EAccessModifier.Protected)]
		public string gameTimeText;

		// Token: 0x04009AB5 RID: 39605
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDuration = "duration";
	}
}
