using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;

namespace Twitch
{
	// Token: 0x0200186E RID: 6254
	public class TwitchLeaderboardStats
	{
		// Token: 0x1400010B RID: 267
		// (add) Token: 0x0600C0CE RID: 49358 RVA: 0x0046EB78 File Offset: 0x0046CD78
		// (remove) Token: 0x0600C0CF RID: 49359 RVA: 0x0046EBB0 File Offset: 0x0046CDB0
		public event OnLeaderboardStatsChanged StatsChanged;

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x0600C0D0 RID: 49360 RVA: 0x0046EBE8 File Offset: 0x0046CDE8
		// (remove) Token: 0x0600C0D1 RID: 49361 RVA: 0x0046EC20 File Offset: 0x0046CE20
		public event OnLeaderboardStatsChanged LeaderboardChanged;

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x0600C0D2 RID: 49362 RVA: 0x0046EC55 File Offset: 0x0046CE55
		// (set) Token: 0x0600C0D3 RID: 49363 RVA: 0x0046EC5D File Offset: 0x0046CE5D
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

		// Token: 0x0600C0D4 RID: 49364 RVA: 0x0046EC7F File Offset: 0x0046CE7F
		public void SetupLocalization()
		{
			this.chatOutput_GoodReward = Localization.Get("TwitchChat_GoodReward", false, null);
			this.ingameOutput_GoodReward = Localization.Get("TwitchInGame_GoodReward", false, null);
		}

		// Token: 0x0600C0D5 RID: 49365 RVA: 0x0046ECA5 File Offset: 0x0046CEA5
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleStatsChanged()
		{
			if (this.StatsChanged != null)
			{
				this.StatsChanged();
			}
		}

		// Token: 0x0600C0D6 RID: 49366 RVA: 0x0046ECBA File Offset: 0x0046CEBA
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleLeaderboardChanged()
		{
			if (this.LeaderboardChanged != null)
			{
				this.LeaderboardChanged();
			}
		}

		// Token: 0x0600C0D7 RID: 49367 RVA: 0x0046ECD0 File Offset: 0x0046CED0
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

		// Token: 0x0600C0D8 RID: 49368 RVA: 0x0046EF2D File Offset: 0x0046D12D
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

		// Token: 0x0600C0D9 RID: 49369 RVA: 0x0046EF68 File Offset: 0x0046D168
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

		// Token: 0x0600C0DA RID: 49370 RVA: 0x0046EFBC File Offset: 0x0046D1BC
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

		// Token: 0x0600C0DB RID: 49371 RVA: 0x0046F010 File Offset: 0x0046D210
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

		// Token: 0x0600C0DC RID: 49372 RVA: 0x0046F05C File Offset: 0x0046D25C
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

		// Token: 0x0600C0DD RID: 49373 RVA: 0x0046F0B0 File Offset: 0x0046D2B0
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

		// Token: 0x0600C0DE RID: 49374 RVA: 0x0046F128 File Offset: 0x0046D328
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

		// Token: 0x0600C0DF RID: 49375 RVA: 0x0046F1A0 File Offset: 0x0046D3A0
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

		// Token: 0x0600C0E0 RID: 49376 RVA: 0x0046F1F4 File Offset: 0x0046D3F4
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

		// Token: 0x040090BD RID: 37053
		public int LargestPimpPot;

		// Token: 0x040090BE RID: 37054
		public int LargestBitPot;

		// Token: 0x040090BF RID: 37055
		public int TotalGood;

		// Token: 0x040090C0 RID: 37056
		public int TotalBad;

		// Token: 0x040090C1 RID: 37057
		public int TotalActions;

		// Token: 0x040090C2 RID: 37058
		public int TotalBits;

		// Token: 0x040090C3 RID: 37059
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GoodReward;

		// Token: 0x040090C4 RID: 37060
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_GoodReward;

		// Token: 0x040090C7 RID: 37063
		public TwitchLeaderboardStats.StatEntry TopKillerViewer;

		// Token: 0x040090C8 RID: 37064
		public TwitchLeaderboardStats.StatEntry TopGoodViewer;

		// Token: 0x040090C9 RID: 37065
		public TwitchLeaderboardStats.StatEntry TopBadViewer;

		// Token: 0x040090CA RID: 37066
		public TwitchLeaderboardStats.StatEntry MostBitsSpentViewer;

		// Token: 0x040090CB RID: 37067
		public TwitchLeaderboardStats.StatEntry CurrentGoodViewer;

		// Token: 0x040090CC RID: 37068
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CurrentGoodDirty;

		// Token: 0x040090CD RID: 37069
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastTime = 1f;

		// Token: 0x040090CE RID: 37070
		public float nextGoodTime = -1f;

		// Token: 0x040090CF RID: 37071
		public int GoodRewardAmount = 1000;

		// Token: 0x040090D0 RID: 37072
		[PublicizedFrom(EAccessModifier.Private)]
		public int goodRewardTime = 15;

		// Token: 0x040090D1 RID: 37073
		public Dictionary<string, TwitchLeaderboardStats.StatEntry> StatEntries = new Dictionary<string, TwitchLeaderboardStats.StatEntry>();

		// Token: 0x0200186F RID: 6255
		public class StatEntry
		{
			// Token: 0x040090D2 RID: 37074
			public string Name;

			// Token: 0x040090D3 RID: 37075
			public string UserColor;

			// Token: 0x040090D4 RID: 37076
			public int Kills;

			// Token: 0x040090D5 RID: 37077
			public int GoodActions;

			// Token: 0x040090D6 RID: 37078
			public int BadActions;

			// Token: 0x040090D7 RID: 37079
			public int BitsUsed;

			// Token: 0x040090D8 RID: 37080
			public int CurrentGoodActions;

			// Token: 0x040090D9 RID: 37081
			public int CurrentActions;
		}
	}
}
