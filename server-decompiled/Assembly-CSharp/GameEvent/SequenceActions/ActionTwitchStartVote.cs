using System;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A2E RID: 6702
	[Preserve]
	public class ActionTwitchStartVote : BaseAction
	{
		// Token: 0x0600CC28 RID: 52264 RVA: 0x004A928C File Offset: 0x004A748C
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

		// Token: 0x0600CC29 RID: 52265 RVA: 0x004A92DC File Offset: 0x004A74DC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTwitchStartVote.PropVoteType, ref this.voteType);
		}

		// Token: 0x0600CC2A RID: 52266 RVA: 0x004A92F6 File Offset: 0x004A74F6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchStartVote
			{
				voteType = this.voteType
			};
		}

		// Token: 0x04009ACC RID: 39628
		[PublicizedFrom(EAccessModifier.Protected)]
		public string voteType = "";

		// Token: 0x04009ACD RID: 39629
		public static string PropVoteType = "vote_type";
	}
}
