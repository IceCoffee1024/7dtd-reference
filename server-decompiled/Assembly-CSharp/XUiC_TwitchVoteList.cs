using System;
using System.Collections.Generic;
using System.Globalization;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010AB RID: 4267
[Preserve]
public class XUiC_TwitchVoteList : XUiController
{
	// Token: 0x06008800 RID: 34816 RVA: 0x0033E8C8 File Offset: 0x0033CAC8
	public float GetHeight()
	{
		return 90f;
	}

	// Token: 0x06008801 RID: 34817 RVA: 0x0033E8D0 File Offset: 0x0033CAD0
	public override void Init()
	{
		base.Init();
		XUiC_TwitchVoteEntry[] childrenByType = base.GetChildrenByType<XUiC_TwitchVoteEntry>(null);
		for (int i = 0; i < childrenByType.Length; i++)
		{
			if (childrenByType[i] != null)
			{
				this.voteEntries.Add(childrenByType[i]);
			}
		}
	}

	// Token: 0x06008802 RID: 34818 RVA: 0x0033E90C File Offset: 0x0033CB0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void VoteStateChanged()
	{
		if (this.lineCount == this.votingManager.NeededLines)
		{
			this.IsDirty = true;
			this.voteList.Clear();
		}
	}

	// Token: 0x06008803 RID: 34819 RVA: 0x0033E90C File Offset: 0x0033CB0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void VoteEndedChanged()
	{
		if (this.lineCount == this.votingManager.NeededLines)
		{
			this.IsDirty = true;
			this.voteList.Clear();
		}
	}

	// Token: 0x06008804 RID: 34820 RVA: 0x0033E934 File Offset: 0x0033CB34
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.Owner.IsDirty = true;
			if (this.lineCount != this.votingManager.NeededLines)
			{
				for (int i = 0; i < this.voteEntries.Count; i++)
				{
					this.voteEntries[i].Vote = null;
					this.voteEntries[i].isWinner = false;
				}
			}
			else if (this.votingManager.CurrentVoteState == TwitchVotingManager.VoteStateTypes.EventActive || this.votingManager.CurrentVoteState == TwitchVotingManager.VoteStateTypes.WaitingForActive)
			{
				this.SetupWinner();
				this.votingManager.WinnerShowing = true;
				this.Owner.IsDirty = true;
			}
			else if (this.votingManager.CurrentVoteState == TwitchVotingManager.VoteStateTypes.WaitingForNextVote)
			{
				this.votingManager.WinnerShowing = false;
				for (int j = 0; j < this.voteEntries.Count; j++)
				{
					this.voteEntries[j].Vote = null;
					this.voteEntries[j].isWinner = false;
				}
			}
			else if (this.votingManager.CurrentVoteState == TwitchVotingManager.VoteStateTypes.VoteStarted)
			{
				this.SetupForVote();
				this.votingManager.WinnerShowing = false;
			}
			else
			{
				for (int k = 0; k < this.voteEntries.Count; k++)
				{
					this.voteEntries[k].Vote = null;
					this.voteEntries[k].isWinner = false;
				}
			}
			this.IsDirty = false;
		}
		if (this.votingManager.UIDirty)
		{
			for (int l = 0; l < this.voteEntries.Count; l++)
			{
				this.voteEntries[l].IsDirty = true;
			}
			this.votingManager.UIDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x06008805 RID: 34821 RVA: 0x0033EAEC File Offset: 0x0033CCEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupForVote()
	{
		if (this.voteList.Count == 0)
		{
			this.SetupCommandList();
		}
		int num = 0;
		int num2 = 0;
		while (num2 < this.voteList.Count && num < this.voteEntries.Count)
		{
			if (this.voteEntries[num] != null)
			{
				this.voteEntries[num].Vote = this.voteList[num2];
				this.voteEntries[num].isWinner = false;
				num++;
			}
			num2++;
		}
		for (int i = num; i < this.voteEntries.Count; i++)
		{
			this.voteEntries[i].Vote = null;
			this.voteEntries[i].isWinner = false;
		}
	}

	// Token: 0x06008806 RID: 34822 RVA: 0x0033EBB0 File Offset: 0x0033CDB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupWinner()
	{
		this.voteList.Clear();
		this.voteList.Add(this.votingManager.CurrentEvent);
		int num = 0;
		int num2 = 0;
		while (num2 < this.voteList.Count && num < this.voteEntries.Count)
		{
			if (this.voteEntries[num] != null)
			{
				this.voteEntries[num].Vote = this.voteList[num2];
				this.voteEntries[num].isWinner = true;
				num++;
			}
			num2++;
		}
		for (int i = num; i < this.voteEntries.Count; i++)
		{
			this.voteEntries[i].Vote = null;
			this.voteEntries[i].isWinner = false;
		}
	}

	// Token: 0x06008807 RID: 34823 RVA: 0x0033EC7F File Offset: 0x0033CE7F
	public void SetupCommandList()
	{
		this.voteList.Clear();
		this.voteList.AddRange(this.votingManager.voteList);
	}

	// Token: 0x06008808 RID: 34824 RVA: 0x0033ECA4 File Offset: 0x0033CEA4
	public override void OnOpen()
	{
		base.OnOpen();
		this.votingManager = TwitchManager.Current.VotingManager;
		this.IsDirty = true;
		TwitchVotingManager twitchVotingManager = this.votingManager;
		twitchVotingManager.VoteStarted = (OnGameEventVoteAction)Delegate.Remove(twitchVotingManager.VoteStarted, new OnGameEventVoteAction(this.VoteStateChanged));
		TwitchVotingManager twitchVotingManager2 = this.votingManager;
		twitchVotingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Combine(twitchVotingManager2.VoteStarted, new OnGameEventVoteAction(this.VoteStateChanged));
		TwitchVotingManager twitchVotingManager3 = this.votingManager;
		twitchVotingManager3.VoteEventStarted = (OnGameEventVoteAction)Delegate.Remove(twitchVotingManager3.VoteEventStarted, new OnGameEventVoteAction(this.VoteStateChanged));
		TwitchVotingManager twitchVotingManager4 = this.votingManager;
		twitchVotingManager4.VoteEventStarted = (OnGameEventVoteAction)Delegate.Combine(twitchVotingManager4.VoteEventStarted, new OnGameEventVoteAction(this.VoteStateChanged));
		TwitchVotingManager twitchVotingManager5 = this.votingManager;
		twitchVotingManager5.VoteEventEnded = (OnGameEventVoteAction)Delegate.Remove(twitchVotingManager5.VoteEventEnded, new OnGameEventVoteAction(this.VoteEndedChanged));
		TwitchVotingManager twitchVotingManager6 = this.votingManager;
		twitchVotingManager6.VoteEventEnded = (OnGameEventVoteAction)Delegate.Combine(twitchVotingManager6.VoteEventEnded, new OnGameEventVoteAction(this.VoteEndedChanged));
	}

	// Token: 0x06008809 RID: 34825 RVA: 0x0033EDB8 File Offset: 0x0033CFB8
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "line_count")
		{
			this.lineCount = (int)StringParsers.ParseSInt16(value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(name, value);
	}

	// Token: 0x04006550 RID: 25936
	public List<TwitchVoteEntry> voteList = new List<TwitchVoteEntry>();

	// Token: 0x04006551 RID: 25937
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchVoteEntry> voteEntries = new List<XUiC_TwitchVoteEntry>();

	// Token: 0x04006552 RID: 25938
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVotingManager votingManager;

	// Token: 0x04006553 RID: 25939
	public XUiC_TwitchWindow Owner;

	// Token: 0x04006554 RID: 25940
	[PublicizedFrom(EAccessModifier.Private)]
	public int lineCount = 1;
}
