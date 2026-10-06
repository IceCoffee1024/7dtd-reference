using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019F5 RID: 6645
	[Preserve]
	public class ActionStartHomerun : ActionBaseTargetAction
	{
		// Token: 0x0600CB19 RID: 51993 RVA: 0x004A4974 File Offset: 0x004A2B74
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

		// Token: 0x0600CB1A RID: 51994 RVA: 0x004A49C8 File Offset: 0x004A2BC8
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

		// Token: 0x0600CB1B RID: 51995 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void HomeRunComplete()
		{
		}

		// Token: 0x0600CB1C RID: 51996 RVA: 0x004A4A82 File Offset: 0x004A2C82
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

		// Token: 0x040099C9 RID: 39369
		[PublicizedFrom(EAccessModifier.Private)]
		public List<int> rewardLevels = new List<int>();

		// Token: 0x040099CA RID: 39370
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> rewardEvents = new List<string>();

		// Token: 0x040099CB RID: 39371
		[PublicizedFrom(EAccessModifier.Protected)]
		public string gameTimeText;

		// Token: 0x040099CC RID: 39372
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDuration = "duration";
	}
}
