using System;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A2F RID: 6703
	[Preserve]
	public class ActionTwitchVoteDelay : BaseAction
	{
		// Token: 0x0600CC2D RID: 52269 RVA: 0x004A9328 File Offset: 0x004A7528
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

		// Token: 0x0600CC2E RID: 52270 RVA: 0x004A93A5 File Offset: 0x004A75A5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchVoteDelay.PropTime, ref this.delayTimeText);
		}

		// Token: 0x0600CC2F RID: 52271 RVA: 0x004A93BF File Offset: 0x004A75BF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchVoteDelay
			{
				delayTimeText = this.delayTimeText
			};
		}

		// Token: 0x04009ACE RID: 39630
		[PublicizedFrom(EAccessModifier.Protected)]
		public string delayTimeText;

		// Token: 0x04009ACF RID: 39631
		public static string PropTime = "time";
	}
}
