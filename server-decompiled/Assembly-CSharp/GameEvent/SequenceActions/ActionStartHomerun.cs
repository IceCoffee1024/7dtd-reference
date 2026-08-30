using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A3 RID: 6563
	[Preserve]
	public class ActionStartHomerun : ActionBaseTargetAction
	{
		// Token: 0x0600C83A RID: 51258 RVA: 0x0049A178 File Offset: 0x00498378
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

		// Token: 0x0600C83B RID: 51259 RVA: 0x0049A1CC File Offset: 0x004983CC
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

		// Token: 0x0600C83C RID: 51260 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void HomeRunComplete()
		{
		}

		// Token: 0x0600C83D RID: 51261 RVA: 0x0049A286 File Offset: 0x00498486
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

		// Token: 0x040097DA RID: 38874
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> rewardLevels = new List<int>();

		// Token: 0x040097DB RID: 38875
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> rewardEvents = new List<string>();

		// Token: 0x040097DC RID: 38876
		[PublicizedFrom(EAccessModifier.Protected)]
		public string gameTimeText;

		// Token: 0x040097DD RID: 38877
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDuration = "duration";
	}
}
