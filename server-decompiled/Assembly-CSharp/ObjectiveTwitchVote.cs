using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020009C7 RID: 2503
[Preserve]
public class ObjectiveTwitchVote : BaseObjective
{
	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x0600495C RID: 18780 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x0600495D RID: 18781 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600495E RID: 18782 RVA: 0x001C5137 File Offset: 0x001C3337
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x001CD56C File Offset: 0x001CB76C
	public override void SetupDisplay()
	{
		base.Description = "";
		this.StatusText = "";
	}

	// Token: 0x06004960 RID: 18784 RVA: 0x001CD584 File Offset: 0x001CB784
	public override void Update(float updateTime)
	{
		TwitchManager twitchManager = TwitchManager.Current;
		switch (this.GameEventState)
		{
		case ObjectiveTwitchVote.TwitchVoteStates.Start:
		{
			if (this.voteType == "" || !twitchManager.IsReady || !twitchManager.VotingManager.VotingEnabled)
			{
				base.CurrentValue = 1;
				this.Refresh();
				return;
			}
			TwitchVotingManager votingManager = twitchManager.VotingManager;
			votingManager.VoteStarted = (OnGameEventVoteAction)Delegate.Combine(votingManager.VoteStarted, new OnGameEventVoteAction(this.VoteStarted));
			twitchManager.VotingManager.QueueVote(this.voteType);
			this.GameEventState = ObjectiveTwitchVote.TwitchVoteStates.Waiting;
			return;
		}
		case ObjectiveTwitchVote.TwitchVoteStates.Waiting:
			break;
		case ObjectiveTwitchVote.TwitchVoteStates.Complete:
			base.CurrentValue = 1;
			this.Refresh();
			break;
		default:
			return;
		}
	}

	// Token: 0x06004961 RID: 18785 RVA: 0x001CD633 File Offset: 0x001CB833
	[PublicizedFrom(EAccessModifier.Private)]
	public void VoteStarted()
	{
		if (TwitchManager.Current.VotingManager.CurrentVoteType.Name == this.voteType)
		{
			this.GameEventState = ObjectiveTwitchVote.TwitchVoteStates.Complete;
		}
	}

	// Token: 0x06004962 RID: 18786 RVA: 0x001C527C File Offset: 0x001C347C
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		base.Complete = base.CurrentValue == 1;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x06004963 RID: 18787 RVA: 0x001CD660 File Offset: 0x001CB860
	public override BaseObjective Clone()
	{
		ObjectiveTwitchVote objectiveTwitchVote = new ObjectiveTwitchVote();
		this.CopyValues(objectiveTwitchVote);
		objectiveTwitchVote.voteType = this.voteType;
		return objectiveTwitchVote;
	}

	// Token: 0x06004964 RID: 18788 RVA: 0x001CD687 File Offset: 0x001CB887
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveTwitchVote.PropVoteType, ref this.voteType);
	}

	// Token: 0x06004965 RID: 18789 RVA: 0x001CD6A1 File Offset: 0x001CB8A1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("vote_type", this.voteType);
	}

	// Token: 0x04003A8C RID: 14988
	[PublicizedFrom(EAccessModifier.Protected)]
	public string voteType = "";

	// Token: 0x04003A8D RID: 14989
	[PublicizedFrom(EAccessModifier.Protected)]
	public float timeRemaining = 2f;

	// Token: 0x04003A8E RID: 14990
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropVoteType = "vote_type";

	// Token: 0x04003A8F RID: 14991
	[PublicizedFrom(EAccessModifier.Protected)]
	public ObjectiveTwitchVote.TwitchVoteStates GameEventState;

	// Token: 0x020009C8 RID: 2504
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum TwitchVoteStates
	{
		// Token: 0x04003A91 RID: 14993
		Start,
		// Token: 0x04003A92 RID: 14994
		Waiting,
		// Token: 0x04003A93 RID: 14995
		Complete
	}
}
