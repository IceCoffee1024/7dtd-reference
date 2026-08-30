using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A02 RID: 6658
	[Preserve]
	public class ActionTwitchStartVote : BaseAction
	{
		// Token: 0x0600CAD0 RID: 51920 RVA: 0x004A4DC0 File Offset: 0x004A2FC0
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.voteType == "")
			{
				return BaseAction.ActionCompleteStates.InCompleteRefund;
			}
			TwitchManager twitchManager = TwitchManager.Current;
			if (!twitchManager.TwitchActive || !twitchManager.VotingManager.VotingEnabled)
			{
				return BaseAction.ActionCompleteStates.InCompleteRefund;
			}
			twitchManager.VotingManager.QueueVote(this.voteType);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAD1 RID: 51921 RVA: 0x004A4E10 File Offset: 0x004A3010
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchStartVote.PropVoteType, ref this.voteType);
		}

		// Token: 0x0600CAD2 RID: 51922 RVA: 0x004A4E2A File Offset: 0x004A302A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchStartVote
			{
				voteType = this.voteType
			};
		}

		// Token: 0x04009A06 RID: 39430
		[PublicizedFrom(EAccessModifier.Protected)]
		public string voteType = "";

		// Token: 0x04009A07 RID: 39431
		public static string PropVoteType = "vote_type";
	}
}
