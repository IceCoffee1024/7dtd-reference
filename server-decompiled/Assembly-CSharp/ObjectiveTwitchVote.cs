using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020009B7 RID: 2487
[Preserve]
public class ObjectiveTwitchVote : BaseObjective
{
	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x06004979 RID: 18809 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x0600497A RID: 18810 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600497B RID: 18811 RVA: 0x001C3898 File Offset: 0x001C1A98
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
	}

	// Token: 0x0600497C RID: 18812 RVA: 0x001CB9A8 File Offset: 0x001C9BA8
	public override void SetupDisplay()
	{
		base.Description = "";
		this.StatusText = "";
	}

	// Token: 0x0600497D RID: 18813 RVA: 0x001CB9C0 File Offset: 0x001C9BC0
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

	// Token: 0x0600497E RID: 18814 RVA: 0x001CBA6F File Offset: 0x001C9C6F
	[PublicizedFrom(EAccessModifier.Private)]
	public void VoteStarted()
	{
		if (TwitchManager.Current.VotingManager.CurrentVoteType.Name == this.voteType)
		{
			this.GameEventState = ObjectiveTwitchVote.TwitchVoteStates.Complete;
		}
	}

	// Token: 0x0600497F RID: 18815 RVA: 0x001C39DC File Offset: 0x001C1BDC
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

	// Token: 0x06004980 RID: 18816 RVA: 0x001CBA9C File Offset: 0x001C9C9C
	public override BaseObjective Clone()
	{
		ObjectiveTwitchVote objectiveTwitchVote = new ObjectiveTwitchVote();
		this.CopyValues(objectiveTwitchVote);
		objectiveTwitchVote.voteType = this.voteType;
		return objectiveTwitchVote;
	}

	// Token: 0x06004981 RID: 18817 RVA: 0x001CBAC3 File Offset: 0x001C9CC3
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveTwitchVote.PropVoteType, ref this.voteType);
	}

	// Token: 0x04003A6F RID: 14959
	[PublicizedFrom(EAccessModifier.Protected)]
	public string voteType = "";

	// Token: 0x04003A70 RID: 14960
	[PublicizedFrom(EAccessModifier.Protected)]
	public float timeRemaining = 2f;

	// Token: 0x04003A71 RID: 14961
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropVoteType = "vote_type";

	// Token: 0x04003A72 RID: 14962
	[PublicizedFrom(EAccessModifier.Protected)]
	public ObjectiveTwitchVote.TwitchVoteStates GameEventState;

	// Token: 0x020009B8 RID: 2488
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum TwitchVoteStates
	{
		// Token: 0x04003A74 RID: 14964
		Start,
		// Token: 0x04003A75 RID: 14965
		Waiting,
		// Token: 0x04003A76 RID: 14966
		Complete
	}
}
