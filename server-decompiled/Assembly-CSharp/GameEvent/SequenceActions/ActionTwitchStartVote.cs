using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A5F RID: 6751
	[Preserve]
	public class ActionTwitchStartVote : BaseAction
	{
		// Token: 0x0600CD3C RID: 52540 RVA: 0x004ACD88 File Offset: 0x004AAF88
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

		// Token: 0x0600CD3D RID: 52541 RVA: 0x004ACDD8 File Offset: 0x004AAFD8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchStartVote.PropVoteType, ref this.voteType);
		}

		// Token: 0x0600CD3E RID: 52542 RVA: 0x004ACDF2 File Offset: 0x004AAFF2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchStartVote
			{
				voteType = this.voteType
			};
		}

		// Token: 0x04009BB5 RID: 39861
		[PublicizedFrom(EAccessModifier.Protected)]
		public string voteType = "";

		// Token: 0x04009BB6 RID: 39862
		public static string PropVoteType = "vote_type";
	}
}
