using System;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A03 RID: 6659
	[Preserve]
	public class ActionTwitchVoteDelay : BaseAction
	{
		// Token: 0x0600CAD5 RID: 51925 RVA: 0x004A4E5C File Offset: 0x004A305C
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

		// Token: 0x0600CAD6 RID: 51926 RVA: 0x004A4ED9 File Offset: 0x004A30D9
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchVoteDelay.PropTime, ref this.delayTimeText);
		}

		// Token: 0x0600CAD7 RID: 51927 RVA: 0x004A4EF3 File Offset: 0x004A30F3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchVoteDelay
			{
				delayTimeText = this.delayTimeText
			};
		}

		// Token: 0x04009A08 RID: 39432
		[PublicizedFrom(EAccessModifier.Protected)]
		public string delayTimeText;

		// Token: 0x04009A09 RID: 39433
		public static string PropTime = "time";
	}
}
