using System;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A60 RID: 6752
	[Preserve]
	public class ActionTwitchVoteDelay : BaseAction
	{
		// Token: 0x0600CD41 RID: 52545 RVA: 0x004ACE24 File Offset: 0x004AB024
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

		// Token: 0x0600CD42 RID: 52546 RVA: 0x004ACEA1 File Offset: 0x004AB0A1
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchVoteDelay.PropTime, ref this.delayTimeText);
		}

		// Token: 0x0600CD43 RID: 52547 RVA: 0x004ACEBB File Offset: 0x004AB0BB
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchVoteDelay
			{
				delayTimeText = this.delayTimeText
			};
		}

		// Token: 0x04009BB7 RID: 39863
		[PublicizedFrom(EAccessModifier.Protected)]
		public string delayTimeText;

		// Token: 0x04009BB8 RID: 39864
		public static string PropTime = "time";
	}
}
