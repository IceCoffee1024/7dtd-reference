using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C9 RID: 6601
	[Preserve]
	public class ActionStartHomerun : ActionBaseTargetAction
	{
		// Token: 0x0600C9C1 RID: 51649 RVA: 0x004A04D0 File Offset: 0x0049E6D0
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

		// Token: 0x0600C9C2 RID: 51650 RVA: 0x004A0524 File Offset: 0x0049E724
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

		// Token: 0x0600C9C3 RID: 51651 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void HomeRunComplete()
		{
		}

		// Token: 0x0600C9C4 RID: 51652 RVA: 0x004A05DE File Offset: 0x0049E7DE
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

		// Token: 0x04009903 RID: 39171
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> rewardLevels = new List<int>();

		// Token: 0x04009904 RID: 39172
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> rewardEvents = new List<string>();

		// Token: 0x04009905 RID: 39173
		[PublicizedFrom(EAccessModifier.Protected)]
		public string gameTimeText;

		// Token: 0x04009906 RID: 39174
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDuration = "duration";
	}
}
