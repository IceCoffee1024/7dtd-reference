using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DB RID: 6619
	[Preserve]
	public class ActionTwitchStartVote : BaseAction
	{
		// Token: 0x0600C947 RID: 51527 RVA: 0x0049E9CC File Offset: 0x0049CBCC
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

		// Token: 0x0600C948 RID: 51528 RVA: 0x0049EA1C File Offset: 0x0049CC1C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchStartVote.PropVoteType, ref this.voteType);
		}

		// Token: 0x0600C949 RID: 51529 RVA: 0x0049EA36 File Offset: 0x0049CC36
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchStartVote
			{
				voteType = this.voteType
			};
		}

		// Token: 0x040098D5 RID: 39125
		[PublicizedFrom(EAccessModifier.Protected)]
		public string voteType = "";

		// Token: 0x040098D6 RID: 39126
		public static string PropVoteType = "vote_type";
	}
}
