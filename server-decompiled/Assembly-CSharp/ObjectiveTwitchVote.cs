using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020009BD RID: 2493
[Preserve]
public class ObjectiveTwitchVote : BaseObjective
{
	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x06004939 RID: 18745 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x0600493A RID: 18746 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600493B RID: 18747 RVA: 0x001C40B3 File Offset: 0x001C22B3
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveAssemble_keyword", false, null);
	}

	// Token: 0x0600493C RID: 18748 RVA: 0x001CC498 File Offset: 0x001CA698
	public override void SetupDisplay()
	{
		base.Description = "";
		this.StatusText = "";
	}

	// Token: 0x0600493D RID: 18749 RVA: 0x001CC4B0 File Offset: 0x001CA6B0
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

	// Token: 0x0600493E RID: 18750 RVA: 0x001CC55F File Offset: 0x001CA75F
	[PublicizedFrom(EAccessModifier.Private)]
	public void VoteStarted()
	{
		if (TwitchManager.Current.VotingManager.CurrentVoteType.Name == this.voteType)
		{
			this.GameEventState = ObjectiveTwitchVote.TwitchVoteStates.Complete;
		}
	}

	// Token: 0x0600493F RID: 18751 RVA: 0x001C41F8 File Offset: 0x001C23F8
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

	// Token: 0x06004940 RID: 18752 RVA: 0x001CC58C File Offset: 0x001CA78C
	public override BaseObjective Clone()
	{
		ObjectiveTwitchVote objectiveTwitchVote = new ObjectiveTwitchVote();
		this.CopyValues(objectiveTwitchVote);
		objectiveTwitchVote.voteType = this.voteType;
		return objectiveTwitchVote;
	}

	// Token: 0x06004941 RID: 18753 RVA: 0x001CC5B3 File Offset: 0x001CA7B3
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveTwitchVote.PropVoteType, ref this.voteType);
	}

	// Token: 0x06004942 RID: 18754 RVA: 0x001CC5CD File Offset: 0x001CA7CD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("vote_type", this.voteType);
	}

	// Token: 0x04003A6C RID: 14956
	[PublicizedFrom(EAccessModifier.Protected)]
	public string voteType = "";

	// Token: 0x04003A6D RID: 14957
	[PublicizedFrom(EAccessModifier.Protected)]
	public float timeRemaining = 2f;

	// Token: 0x04003A6E RID: 14958
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropVoteType = "vote_type";

	// Token: 0x04003A6F RID: 14959
	[PublicizedFrom(EAccessModifier.Protected)]
	public ObjectiveTwitchVote.TwitchVoteStates GameEventState;

	// Token: 0x020009BE RID: 2494
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum TwitchVoteStates
	{
		// Token: 0x04003A71 RID: 14961
		Start,
		// Token: 0x04003A72 RID: 14962
		Waiting,
		// Token: 0x04003A73 RID: 14963
		Complete
	}
}
