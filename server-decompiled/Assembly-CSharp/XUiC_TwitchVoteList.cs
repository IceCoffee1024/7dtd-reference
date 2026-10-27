using System;
using System.Collections.Generic;
using System.Globalization;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010CC RID: 4300
[Preserve]
public class XUiC_TwitchVoteList : XUiController
{
	// Token: 0x060088A2 RID: 34978 RVA: 0x00344764 File Offset: 0x00342964
	public float GetHeight()
	{
		return 90f;
	}

	// Token: 0x060088A3 RID: 34979 RVA: 0x0034476C File Offset: 0x0034296C
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

	// Token: 0x060088A4 RID: 34980 RVA: 0x003447A8 File Offset: 0x003429A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void VoteStateChanged()
	{
		if (this.lineCount == this.votingManager.NeededLines)
		{
			this.IsDirty = true;
			this.voteList.Clear();
		}
	}

	// Token: 0x060088A5 RID: 34981 RVA: 0x003447A8 File Offset: 0x003429A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void VoteEndedChanged()
	{
		if (this.lineCount == this.votingManager.NeededLines)
		{
			this.IsDirty = true;
			this.voteList.Clear();
		}
	}

	// Token: 0x060088A6 RID: 34982 RVA: 0x003447D0 File Offset: 0x003429D0
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

	// Token: 0x060088A7 RID: 34983 RVA: 0x00344988 File Offset: 0x00342B88
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

	// Token: 0x060088A8 RID: 34984 RVA: 0x00344A4C File Offset: 0x00342C4C
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

	// Token: 0x060088A9 RID: 34985 RVA: 0x00344B1B File Offset: 0x00342D1B
	public void SetupCommandList()
	{
		this.voteList.Clear();
		this.voteList.AddRange(this.votingManager.voteList);
	}

	// Token: 0x060088AA RID: 34986 RVA: 0x00344B40 File Offset: 0x00342D40
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

	// Token: 0x060088AB RID: 34987 RVA: 0x00344C54 File Offset: 0x00342E54
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "line_count")
		{
			this.lineCount = (int)StringParsers.ParseSInt16(value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(name, value);
	}

	// Token: 0x040065D1 RID: 26065
	public List<TwitchVoteEntry> voteList = new List<TwitchVoteEntry>();

	// Token: 0x040065D2 RID: 26066
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchVoteEntry> voteEntries = new List<XUiC_TwitchVoteEntry>();

	// Token: 0x040065D3 RID: 26067
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVotingManager votingManager;

	// Token: 0x040065D4 RID: 26068
	public XUiC_TwitchWindow Owner;

	// Token: 0x040065D5 RID: 26069
	[PublicizedFrom(EAccessModifier.Private)]
	public int lineCount = 1;
}
