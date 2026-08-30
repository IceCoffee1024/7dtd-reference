using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;

namespace Twitch
{
	// Token: 0x02001842 RID: 6210
	public class TwitchLeaderboardStats
	{
		// Token: 0x1400010C RID: 268
		// (add) Token: 0x0600BF76 RID: 49014 RVA: 0x0046A63C File Offset: 0x0046883C
		// (remove) Token: 0x0600BF77 RID: 49015 RVA: 0x0046A674 File Offset: 0x00468874
		public event OnLeaderboardStatsChanged StatsChanged;

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x0600BF78 RID: 49016 RVA: 0x0046A6AC File Offset: 0x004688AC
		// (remove) Token: 0x0600BF79 RID: 49017 RVA: 0x0046A6E4 File Offset: 0x004688E4
		public event OnLeaderboardStatsChanged LeaderboardChanged;

		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x0600BF7A RID: 49018 RVA: 0x0046A719 File Offset: 0x00468919
		// (set) Token: 0x0600BF7B RID: 49019 RVA: 0x0046A721 File Offset: 0x00468921
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

		// Token: 0x0600BF7C RID: 49020 RVA: 0x0046A743 File Offset: 0x00468943
		public void SetupLocalization()
		{
			this.chatOutput_GoodReward = Localization.Get("TwitchChat_GoodReward", false, null);
			this.ingameOutput_GoodReward = Localization.Get("TwitchInGame_GoodReward", false, null);
		}

		// Token: 0x0600BF7D RID: 49021 RVA: 0x0046A769 File Offset: 0x00468969
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleStatsChanged()
		{
			if (this.StatsChanged != null)
			{
				this.StatsChanged();
			}
		}

		// Token: 0x0600BF7E RID: 49022 RVA: 0x0046A77E File Offset: 0x0046897E
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleLeaderboardChanged()
		{
			if (this.LeaderboardChanged != null)
			{
				this.LeaderboardChanged();
			}
		}

		// Token: 0x0600BF7F RID: 49023 RVA: 0x0046A794 File Offset: 0x00468994
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

		// Token: 0x0600BF80 RID: 49024 RVA: 0x0046A9F1 File Offset: 0x00468BF1
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

		// Token: 0x0600BF81 RID: 49025 RVA: 0x0046AA2C File Offset: 0x00468C2C
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

		// Token: 0x0600BF82 RID: 49026 RVA: 0x0046AA80 File Offset: 0x00468C80
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

		// Token: 0x0600BF83 RID: 49027 RVA: 0x0046AAD4 File Offset: 0x00468CD4
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

		// Token: 0x0600BF84 RID: 49028 RVA: 0x0046AB20 File Offset: 0x00468D20
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

		// Token: 0x0600BF85 RID: 49029 RVA: 0x0046AB74 File Offset: 0x00468D74
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

		// Token: 0x0600BF86 RID: 49030 RVA: 0x0046ABEC File Offset: 0x00468DEC
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

		// Token: 0x0600BF87 RID: 49031 RVA: 0x0046AC64 File Offset: 0x00468E64
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

		// Token: 0x0600BF88 RID: 49032 RVA: 0x0046ACB8 File Offset: 0x00468EB8
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

		// Token: 0x04008FF7 RID: 36855
		public int LargestPimpPot;

		// Token: 0x04008FF8 RID: 36856
		public int LargestBitPot;

		// Token: 0x04008FF9 RID: 36857
		public int TotalGood;

		// Token: 0x04008FFA RID: 36858
		public int TotalBad;

		// Token: 0x04008FFB RID: 36859
		public int TotalActions;

		// Token: 0x04008FFC RID: 36860
		public int TotalBits;

		// Token: 0x04008FFD RID: 36861
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GoodReward;

		// Token: 0x04008FFE RID: 36862
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_GoodReward;

		// Token: 0x04009001 RID: 36865
		public TwitchLeaderboardStats.StatEntry TopKillerViewer;

		// Token: 0x04009002 RID: 36866
		public TwitchLeaderboardStats.StatEntry TopGoodViewer;

		// Token: 0x04009003 RID: 36867
		public TwitchLeaderboardStats.StatEntry TopBadViewer;

		// Token: 0x04009004 RID: 36868
		public TwitchLeaderboardStats.StatEntry MostBitsSpentViewer;

		// Token: 0x04009005 RID: 36869
		public TwitchLeaderboardStats.StatEntry CurrentGoodViewer;

		// Token: 0x04009006 RID: 36870
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CurrentGoodDirty;

		// Token: 0x04009007 RID: 36871
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastTime = 1f;

		// Token: 0x04009008 RID: 36872
		public float nextGoodTime = -1f;

		// Token: 0x04009009 RID: 36873
		public int GoodRewardAmount = 1000;

		// Token: 0x0400900A RID: 36874
		[PublicizedFrom(EAccessModifier.Private)]
		public int goodRewardTime = 15;

		// Token: 0x0400900B RID: 36875
		public Dictionary<string, TwitchLeaderboardStats.StatEntry> StatEntries = new Dictionary<string, TwitchLeaderboardStats.StatEntry>();

		// Token: 0x02001843 RID: 6211
		public class StatEntry
		{
			// Token: 0x0400900C RID: 36876
			public string Name;

			// Token: 0x0400900D RID: 36877
			public string UserColor;

			// Token: 0x0400900E RID: 36878
			public int Kills;

			// Token: 0x0400900F RID: 36879
			public int GoodActions;

			// Token: 0x04009010 RID: 36880
			public int BadActions;

			// Token: 0x04009011 RID: 36881
			public int BitsUsed;

			// Token: 0x04009012 RID: 36882
			public int CurrentGoodActions;

			// Token: 0x04009013 RID: 36883
			public int CurrentActions;
		}
	}
}
