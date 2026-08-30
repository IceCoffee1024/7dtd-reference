using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;

namespace Twitch
{
	// Token: 0x0200181C RID: 6172
	public class TwitchLeaderboardStats
	{
		// Token: 0x1400010B RID: 267
		// (add) Token: 0x0600BDF1 RID: 48625 RVA: 0x004648A0 File Offset: 0x00462AA0
		// (remove) Token: 0x0600BDF2 RID: 48626 RVA: 0x004648D8 File Offset: 0x00462AD8
		public event OnLeaderboardStatsChanged StatsChanged;

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x0600BDF3 RID: 48627 RVA: 0x00464910 File Offset: 0x00462B10
		// (remove) Token: 0x0600BDF4 RID: 48628 RVA: 0x00464948 File Offset: 0x00462B48
		public event OnLeaderboardStatsChanged LeaderboardChanged;

		// Token: 0x1700170D RID: 5901
		// (get) Token: 0x0600BDF5 RID: 48629 RVA: 0x0046497D File Offset: 0x00462B7D
		// (set) Token: 0x0600BDF6 RID: 48630 RVA: 0x00464985 File Offset: 0x00462B85
		public int GoodRewardTime
		{
			get
			{
				return this.goodRewardTime;
			}
			set
			{
				if (this.goodRewardTime != value)
				{
					this.goodRewardTime = value;
					this.nextGoodTime = (float)(this.goodRewardTime * 60);
				}
			}
		}

		// Token: 0x0600BDF7 RID: 48631 RVA: 0x004649A7 File Offset: 0x00462BA7
		public void SetupLocalization()
		{
			this.chatOutput_GoodReward = Localization.Get("TwitchChat_GoodReward", false, null);
			this.ingameOutput_GoodReward = Localization.Get("TwitchInGame_GoodReward", false, null);
		}

		// Token: 0x0600BDF8 RID: 48632 RVA: 0x004649CD File Offset: 0x00462BCD
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleStatsChanged()
		{
			if (this.StatsChanged != null)
			{
				this.StatsChanged();
			}
		}

		// Token: 0x0600BDF9 RID: 48633 RVA: 0x004649E2 File Offset: 0x00462BE2
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleLeaderboardChanged()
		{
			if (this.LeaderboardChanged != null)
			{
				this.LeaderboardChanged();
			}
		}

		// Token: 0x0600BDFA RID: 48634 RVA: 0x004649F8 File Offset: 0x00462BF8
		public void UpdateStats(float deltaTime)
		{
			if (this.CurrentGoodDirty)
			{
				this.lastTime -= deltaTime;
				if (this.lastTime <= 0f)
				{
					List<TwitchLeaderboardStats.StatEntry> list = (from entry in this.StatEntries.Values
						where entry.CurrentGoodActions > 0
						orderby entry.CurrentGoodActions descending
						select entry).ToList<TwitchLeaderboardStats.StatEntry>();
					if (list.Count > 0)
					{
						if (this.CurrentGoodViewer != list[0])
						{
							this.CurrentGoodViewer = list[0];
							this.HandleStatsChanged();
						}
					}
					else if (this.CurrentGoodViewer != null)
					{
						this.CurrentGoodViewer = null;
						this.HandleStatsChanged();
					}
					this.CurrentGoodDirty = false;
					this.lastTime = 1f;
				}
			}
			if (this.nextGoodTime == -1f)
			{
				this.nextGoodTime = (float)(this.GoodRewardTime * 60);
			}
			if (this.CurrentGoodViewer == null)
			{
				return;
			}
			this.nextGoodTime -= deltaTime;
			if (this.nextGoodTime <= 0f)
			{
				TwitchManager twitchManager = TwitchManager.Current;
				if (twitchManager.CurrentActionPreset.UseHelperReward && this.CurrentGoodViewer != null)
				{
					ViewerEntry viewerEntry = TwitchManager.Current.ViewerData.GetViewerEntry(this.CurrentGoodViewer.Name.ToLower());
					viewerEntry.StandardPoints += (float)this.GoodRewardAmount;
					twitchManager.ircClient.SendChannelMessage(string.Format(this.chatOutput_GoodReward, new object[]
					{
						this.CurrentGoodViewer.Name,
						viewerEntry.CombinedPoints,
						this.GoodRewardAmount,
						Localization.Get("TwitchPoints_PP", false, null)
					}), true);
					twitchManager.AddToInGameChatQueue(string.Format(this.ingameOutput_GoodReward, new object[]
					{
						viewerEntry.UserColor,
						this.CurrentGoodViewer.Name,
						this.GoodRewardAmount,
						Localization.Get("TwitchPoints_PP", false, null)
					}), null);
					twitchManager.LocalPlayer.PlayOneShot("twitch_top_helper", false, false, false, null, 1f);
					this.ClearAllCurrentGood();
					QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.HelperReward, "");
				}
				this.nextGoodTime = (float)(this.GoodRewardTime * 60);
			}
		}

		// Token: 0x0600BDFB RID: 48635 RVA: 0x00464C55 File Offset: 0x00462E55
		public void CheckTopKiller(TwitchLeaderboardStats.StatEntry newData)
		{
			if (this.TopKillerViewer == null || this.TopKillerViewer.Kills < newData.Kills)
			{
				this.TopKillerViewer = newData;
				this.HandleStatsChanged();
				return;
			}
			if (this.TopKillerViewer == newData)
			{
				this.HandleStatsChanged();
			}
		}

		// Token: 0x0600BDFC RID: 48636 RVA: 0x00464C90 File Offset: 0x00462E90
		public void CheckTopGood(TwitchLeaderboardStats.StatEntry newData)
		{
			if (this.TopGoodViewer == null || this.TopGoodViewer.GoodActions < newData.GoodActions)
			{
				this.TopGoodViewer = newData;
				this.HandleStatsChanged();
			}
			else if (this.TopGoodViewer == newData)
			{
				this.HandleStatsChanged();
			}
			this.HandleLeaderboardChanged();
			this.CurrentGoodDirty = true;
		}

		// Token: 0x0600BDFD RID: 48637 RVA: 0x00464CE4 File Offset: 0x00462EE4
		public void CheckTopBad(TwitchLeaderboardStats.StatEntry newData)
		{
			if (this.TopBadViewer == null || this.TopBadViewer.BadActions < newData.BadActions)
			{
				this.TopBadViewer = newData;
				this.HandleStatsChanged();
			}
			else if (this.TopBadViewer == newData)
			{
				this.HandleStatsChanged();
			}
			this.HandleLeaderboardChanged();
			this.CurrentGoodDirty = true;
		}

		// Token: 0x0600BDFE RID: 48638 RVA: 0x00464D38 File Offset: 0x00462F38
		public void CheckMostBitsSpent(TwitchLeaderboardStats.StatEntry newData)
		{
			if (this.MostBitsSpentViewer == null || this.MostBitsSpentViewer.BitsUsed < newData.BitsUsed)
			{
				this.MostBitsSpentViewer = newData;
				this.HandleStatsChanged();
			}
			else if (this.MostBitsSpentViewer == newData)
			{
				this.HandleStatsChanged();
			}
			this.HandleLeaderboardChanged();
		}

		// Token: 0x0600BDFF RID: 48639 RVA: 0x00464D84 File Offset: 0x00462F84
		public TwitchLeaderboardStats.StatEntry AddKill(string name, string userColor)
		{
			if (!this.StatEntries.ContainsKey(name))
			{
				this.StatEntries.Add(name, new TwitchLeaderboardStats.StatEntry());
			}
			TwitchLeaderboardStats.StatEntry statEntry = this.StatEntries[name];
			statEntry.Name = name;
			statEntry.UserColor = userColor;
			statEntry.Kills++;
			return statEntry;
		}

		// Token: 0x0600BE00 RID: 48640 RVA: 0x00464DD8 File Offset: 0x00462FD8
		public TwitchLeaderboardStats.StatEntry AddGoodActionUsed(string name, string userColor, bool isBits)
		{
			if (!this.StatEntries.ContainsKey(name))
			{
				this.StatEntries.Add(name, new TwitchLeaderboardStats.StatEntry());
			}
			int num = (isBits ? 2 : 1);
			TwitchLeaderboardStats.StatEntry statEntry = this.StatEntries[name];
			statEntry.Name = name;
			statEntry.UserColor = userColor;
			statEntry.GoodActions += num;
			statEntry.CurrentGoodActions += num;
			statEntry.CurrentActions++;
			return statEntry;
		}

		// Token: 0x0600BE01 RID: 48641 RVA: 0x00464E50 File Offset: 0x00463050
		public TwitchLeaderboardStats.StatEntry AddBadActionUsed(string name, string userColor, bool isBits)
		{
			if (!this.StatEntries.ContainsKey(name))
			{
				this.StatEntries.Add(name, new TwitchLeaderboardStats.StatEntry());
			}
			int num = (isBits ? 2 : 1);
			TwitchLeaderboardStats.StatEntry statEntry = this.StatEntries[name];
			statEntry.Name = name;
			statEntry.UserColor = userColor;
			statEntry.BadActions += num;
			statEntry.CurrentGoodActions -= num;
			statEntry.CurrentActions++;
			return statEntry;
		}

		// Token: 0x0600BE02 RID: 48642 RVA: 0x00464EC8 File Offset: 0x004630C8
		public TwitchLeaderboardStats.StatEntry AddBitsUsed(string name, string userColor, int amount)
		{
			if (!this.StatEntries.ContainsKey(name))
			{
				this.StatEntries.Add(name, new TwitchLeaderboardStats.StatEntry());
			}
			TwitchLeaderboardStats.StatEntry statEntry = this.StatEntries[name];
			statEntry.Name = name;
			statEntry.UserColor = userColor;
			statEntry.BitsUsed += amount;
			return statEntry;
		}

		// Token: 0x0600BE03 RID: 48643 RVA: 0x00464F1C File Offset: 0x0046311C
		public void ClearAllCurrentGood()
		{
			foreach (TwitchLeaderboardStats.StatEntry statEntry in this.StatEntries.Values)
			{
				statEntry.CurrentGoodActions = 0;
				statEntry.CurrentActions = 0;
			}
			this.CurrentGoodViewer = null;
			this.HandleStatsChanged();
			this.HandleLeaderboardChanged();
		}

		// Token: 0x04008EDB RID: 36571
		public int LargestPimpPot;

		// Token: 0x04008EDC RID: 36572
		public int LargestBitPot;

		// Token: 0x04008EDD RID: 36573
		public int TotalGood;

		// Token: 0x04008EDE RID: 36574
		public int TotalBad;

		// Token: 0x04008EDF RID: 36575
		public int TotalActions;

		// Token: 0x04008EE0 RID: 36576
		public int TotalBits;

		// Token: 0x04008EE1 RID: 36577
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GoodReward;

		// Token: 0x04008EE2 RID: 36578
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_GoodReward;

		// Token: 0x04008EE5 RID: 36581
		public TwitchLeaderboardStats.StatEntry TopKillerViewer;

		// Token: 0x04008EE6 RID: 36582
		public TwitchLeaderboardStats.StatEntry TopGoodViewer;

		// Token: 0x04008EE7 RID: 36583
		public TwitchLeaderboardStats.StatEntry TopBadViewer;

		// Token: 0x04008EE8 RID: 36584
		public TwitchLeaderboardStats.StatEntry MostBitsSpentViewer;

		// Token: 0x04008EE9 RID: 36585
		public TwitchLeaderboardStats.StatEntry CurrentGoodViewer;

		// Token: 0x04008EEA RID: 36586
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CurrentGoodDirty;

		// Token: 0x04008EEB RID: 36587
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastTime = 1f;

		// Token: 0x04008EEC RID: 36588
		public float nextGoodTime = -1f;

		// Token: 0x04008EED RID: 36589
		public int GoodRewardAmount = 1000;

		// Token: 0x04008EEE RID: 36590
		[PublicizedFrom(EAccessModifier.Private)]
		public int goodRewardTime = 15;

		// Token: 0x04008EEF RID: 36591
		public Dictionary<string, TwitchLeaderboardStats.StatEntry> StatEntries = new Dictionary<string, TwitchLeaderboardStats.StatEntry>();

		// Token: 0x0200181D RID: 6173
		public class StatEntry
		{
			// Token: 0x04008EF0 RID: 36592
			public string Name;

			// Token: 0x04008EF1 RID: 36593
			public string UserColor;

			// Token: 0x04008EF2 RID: 36594
			public int Kills;

			// Token: 0x04008EF3 RID: 36595
			public int GoodActions;

			// Token: 0x04008EF4 RID: 36596
			public int BadActions;

			// Token: 0x04008EF5 RID: 36597
			public int BitsUsed;

			// Token: 0x04008EF6 RID: 36598
			public int CurrentGoodActions;

			// Token: 0x04008EF7 RID: 36599
			public int CurrentActions;
		}
	}
}
