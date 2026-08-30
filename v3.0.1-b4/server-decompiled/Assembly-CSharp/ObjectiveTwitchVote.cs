using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020009A6 RID: 2470
[Preserve]
public class ObjectiveTwitchVote : BaseObjective
{
	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x060048F3 RID: 18675 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x060048F4 RID: 18676 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060048F5 RID: 18677 RVA: 0x001C10A4 File Offset: 0x001BF2A4
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
	}

	// Token: 0x060048F6 RID: 18678 RVA: 0x001C91B4 File Offset: 0x001C73B4
	public override void SetupDisplay()
	{
		base.Description = "";
		this.StatusText = "";
	}

	// Token: 0x060048F7 RID: 18679 RVA: 0x001C91CC File Offset: 0x001C73CC
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

	// Token: 0x060048F8 RID: 18680 RVA: 0x001C927B File Offset: 0x001C747B
	[PublicizedFrom(EAccessModifier.Private)]
	public void VoteStarted()
	{
		if (TwitchManager.Current.VotingManager.CurrentVoteType.Name == this.voteType)
		{
			this.GameEventState = ObjectiveTwitchVote.TwitchVoteStates.Complete;
		}
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x001C11E8 File Offset: 0x001BF3E8
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

	// Token: 0x060048FA RID: 18682 RVA: 0x001C92A8 File Offset: 0x001C74A8
	public override BaseObjective Clone()
	{
		ObjectiveTwitchVote objectiveTwitchVote = new ObjectiveTwitchVote();
		this.CopyValues(objectiveTwitchVote);
		objectiveTwitchVote.voteType = this.voteType;
		return objectiveTwitchVote;
	}

	// Token: 0x060048FB RID: 18683 RVA: 0x001C92CF File Offset: 0x001C74CF
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveTwitchVote.PropVoteType, ref this.voteType);
	}

	// Token: 0x040039DE RID: 14814
	[PublicizedFrom(EAccessModifier.Protected)]
	public string voteType = "";

	// Token: 0x040039DF RID: 14815
	[PublicizedFrom(EAccessModifier.Protected)]
	public float timeRemaining = 2f;

	// Token: 0x040039E0 RID: 14816
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropVoteType = "vote_type";

	// Token: 0x040039E1 RID: 14817
	[PublicizedFrom(EAccessModifier.Protected)]
	public ObjectiveTwitchVote.TwitchVoteStates GameEventState;

	// Token: 0x020009A7 RID: 2471
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum TwitchVoteStates
	{
		// Token: 0x040039E3 RID: 14819
		Start,
		// Token: 0x040039E4 RID: 14820
		Waiting,
		// Token: 0x040039E5 RID: 14821
		Complete
	}
}
