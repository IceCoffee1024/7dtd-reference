using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;

namespace Twitch
{
	// Token: 0x0200189F RID: 6303
	public class TwitchLeaderboardStats
	{
		// Token: 0x1400010B RID: 267
		// (add) Token: 0x0600C1E2 RID: 49634 RVA: 0x00472524 File Offset: 0x00470724
		// (remove) Token: 0x0600C1E3 RID: 49635 RVA: 0x0047255C File Offset: 0x0047075C
		public event OnLeaderboardStatsChanged StatsChanged;

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x0600C1E4 RID: 49636 RVA: 0x00472594 File Offset: 0x00470794
		// (remove) Token: 0x0600C1E5 RID: 49637 RVA: 0x004725CC File Offset: 0x004707CC
		public event OnLeaderboardStatsChanged LeaderboardChanged;

		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x0600C1E6 RID: 49638 RVA: 0x00472601 File Offset: 0x00470801
		// (set) Token: 0x0600C1E7 RID: 49639 RVA: 0x00472609 File Offset: 0x00470809
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

		// Token: 0x0600C1E8 RID: 49640 RVA: 0x0047262B File Offset: 0x0047082B
		public void SetupLocalization()
		{
			this.chatOutput_GoodReward = Localization.Get("TwitchChat_GoodReward", false, null);
			this.ingameOutput_GoodReward = Localization.Get("TwitchInGame_GoodReward", false, null);
		}

		// Token: 0x0600C1E9 RID: 49641 RVA: 0x00472651 File Offset: 0x00470851
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleStatsChanged()
		{
			if (this.StatsChanged != null)
			{
				this.StatsChanged();
			}
		}

		// Token: 0x0600C1EA RID: 49642 RVA: 0x00472666 File Offset: 0x00470866
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleLeaderboardChanged()
		{
			if (this.LeaderboardChanged != null)
			{
				this.LeaderboardChanged();
			}
		}

		// Token: 0x0600C1EB RID: 49643 RVA: 0x0047267C File Offset: 0x0047087C
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

		// Token: 0x0600C1EC RID: 49644 RVA: 0x004728D9 File Offset: 0x00470AD9
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

		// Token: 0x0600C1ED RID: 49645 RVA: 0x00472914 File Offset: 0x00470B14
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

		// Token: 0x0600C1EE RID: 49646 RVA: 0x00472968 File Offset: 0x00470B68
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

		// Token: 0x0600C1EF RID: 49647 RVA: 0x004729BC File Offset: 0x00470BBC
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

		// Token: 0x0600C1F0 RID: 49648 RVA: 0x00472A08 File Offset: 0x00470C08
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

		// Token: 0x0600C1F1 RID: 49649 RVA: 0x00472A5C File Offset: 0x00470C5C
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

		// Token: 0x0600C1F2 RID: 49650 RVA: 0x00472AD4 File Offset: 0x00470CD4
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

		// Token: 0x0600C1F3 RID: 49651 RVA: 0x00472B4C File Offset: 0x00470D4C
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

		// Token: 0x0600C1F4 RID: 49652 RVA: 0x00472BA0 File Offset: 0x00470DA0
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

		// Token: 0x040091A6 RID: 37286
		public int LargestPimpPot;

		// Token: 0x040091A7 RID: 37287
		public int LargestBitPot;

		// Token: 0x040091A8 RID: 37288
		public int TotalGood;

		// Token: 0x040091A9 RID: 37289
		public int TotalBad;

		// Token: 0x040091AA RID: 37290
		public int TotalActions;

		// Token: 0x040091AB RID: 37291
		public int TotalBits;

		// Token: 0x040091AC RID: 37292
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GoodReward;

		// Token: 0x040091AD RID: 37293
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_GoodReward;

		// Token: 0x040091B0 RID: 37296
		public TwitchLeaderboardStats.StatEntry TopKillerViewer;

		// Token: 0x040091B1 RID: 37297
		public TwitchLeaderboardStats.StatEntry TopGoodViewer;

		// Token: 0x040091B2 RID: 37298
		public TwitchLeaderboardStats.StatEntry TopBadViewer;

		// Token: 0x040091B3 RID: 37299
		public TwitchLeaderboardStats.StatEntry MostBitsSpentViewer;

		// Token: 0x040091B4 RID: 37300
		public TwitchLeaderboardStats.StatEntry CurrentGoodViewer;

		// Token: 0x040091B5 RID: 37301
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CurrentGoodDirty;

		// Token: 0x040091B6 RID: 37302
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastTime = 1f;

		// Token: 0x040091B7 RID: 37303
		public float nextGoodTime = -1f;

		// Token: 0x040091B8 RID: 37304
		public int GoodRewardAmount = 1000;

		// Token: 0x040091B9 RID: 37305
		[PublicizedFrom(EAccessModifier.Private)]
		public int goodRewardTime = 15;

		// Token: 0x040091BA RID: 37306
		public Dictionary<string, TwitchLeaderboardStats.StatEntry> StatEntries = new Dictionary<string, TwitchLeaderboardStats.StatEntry>();

		// Token: 0x020018A0 RID: 6304
		public class StatEntry
		{
			// Token: 0x040091BB RID: 37307
			public string Name;

			// Token: 0x040091BC RID: 37308
			public string UserColor;

			// Token: 0x040091BD RID: 37309
			public int Kills;

			// Token: 0x040091BE RID: 37310
			public int GoodActions;

			// Token: 0x040091BF RID: 37311
			public int BadActions;

			// Token: 0x040091C0 RID: 37312
			public int BitsUsed;

			// Token: 0x040091C1 RID: 37313
			public int CurrentGoodActions;

			// Token: 0x040091C2 RID: 37314
			public int CurrentActions;
		}
	}
}
