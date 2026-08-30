using System;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DC RID: 6620
	[Preserve]
	public class ActionTwitchVoteDelay : BaseAction
	{
		// Token: 0x0600C94C RID: 51532 RVA: 0x0049EA68 File Offset: 0x0049CC68
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			TwitchManager twitchManager = TwitchManager.Current;
			if (twitchManager.VotingManager.CurrentVoteState == TwitchVotingManager.VoteStateTypes.WaitingForNextVote)
			{
				float floatValue = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.delayTimeText, 5f);
				twitchManager.VotingManager.VoteStartDelayTimeRemaining += floatValue;
			}
			else
			{
				Debug.LogWarning("Error: VoteDelay set in wrong state. " + twitchManager.VotingManager.CurrentVoteState.ToString());
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C94D RID: 51533 RVA: 0x0049EAE5 File Offset: 0x0049CCE5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchVoteDelay.PropTime, ref this.delayTimeText);
		}

		// Token: 0x0600C94E RID: 51534 RVA: 0x0049EAFF File Offset: 0x0049CCFF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchVoteDelay
			{
				delayTimeText = this.delayTimeText
			};
		}

		// Token: 0x040098D7 RID: 39127
		[PublicizedFrom(EAccessModifier.Protected)]
		public string delayTimeText;

		// Token: 0x040098D8 RID: 39128
		public static string PropTime = "time";
	}
}
