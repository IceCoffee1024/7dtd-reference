using System;
using System.Collections.Generic;
using Audio;
using Challenges;
using UniLinq;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001899 RID: 6297
	public class TwitchVotingManager
	{
		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x0600C260 RID: 49760 RVA: 0x0047BA9F File Offset: 0x00479C9F
		// (set) Token: 0x0600C261 RID: 49761 RVA: 0x0047BAA7 File Offset: 0x00479CA7
		public int MaxDailyVotes
		{
			get
			{
				return this.maxDailyVotes;
			}
			set
			{
				if (this.maxDailyVotes != value)
				{
					this.maxDailyVotes = value;
					this.lastGameDay = 1;
				}
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x0600C262 RID: 49762 RVA: 0x0047BAC0 File Offset: 0x00479CC0
		// (set) Token: 0x0600C263 RID: 49763 RVA: 0x0047BAC8 File Offset: 0x00479CC8
		public int CurrentVoteDayTimeRange
		{
			get
			{
				return this.currentVoteDayTimeRange;
			}
			set
			{
				if (this.currentVoteDayTimeRange != value)
				{
					this.currentVoteDayTimeRange = value;
					this.lastGameDay = 1;
				}
			}
		}

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x0600C264 RID: 49764 RVA: 0x0047BAE1 File Offset: 0x00479CE1
		public bool VotingEnabled
		{
			get
			{
				return !this.Owner.CurrentVotePreset.IsEmpty;
			}
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x0600C265 RID: 49765 RVA: 0x0047BAF6 File Offset: 0x00479CF6
		public bool VotingIsActive
		{
			get
			{
				return this.VotingEnabled && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.WaitingForNextVote && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.Init && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.ReadyForVoteStart && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.RequestedVoteStart && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.VoteReady;
			}
		}

		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x0600C266 RID: 49766 RVA: 0x0047BB31 File Offset: 0x00479D31
		public int VoteCount
		{
			get
			{
				if (this.voterlist == null)
				{
					return 0;
				}
				return this.voterlist.Count;
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x0600C267 RID: 49767 RVA: 0x0047BB48 File Offset: 0x00479D48
		public string VoteTypeText
		{
			get
			{
				return this.CurrentVoteType.Title;
			}
		}

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x0600C268 RID: 49768 RVA: 0x0047BB55 File Offset: 0x00479D55
		public string VoteTip
		{
			get
			{
				if (this.CurrentEvent != null)
				{
					return this.CurrentEvent.VoteClass.VoteTip;
				}
				return "";
			}
		}

		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x0600C269 RID: 49769 RVA: 0x0047BB75 File Offset: 0x00479D75
		public string VoteOffset
		{
			get
			{
				if (this.CurrentEvent != null)
				{
					return this.CurrentEvent.VoteClass.VoteHeight;
				}
				return "0";
			}
		}

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x0600C26A RID: 49770 RVA: 0x0047BB95 File Offset: 0x00479D95
		public bool UseMystery
		{
			get
			{
				return this.CurrentVoteType.UseMystery;
			}
		}

		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x0600C26B RID: 49771 RVA: 0x0047BBA2 File Offset: 0x00479DA2
		// (set) Token: 0x0600C26C RID: 49772 RVA: 0x0047BBAA File Offset: 0x00479DAA
		public int NeededLines
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x0600C26D RID: 49773 RVA: 0x0047BBB3 File Offset: 0x00479DB3
		// (set) Token: 0x0600C26E RID: 49774 RVA: 0x0047BBBB File Offset: 0x00479DBB
		public TwitchVoteType CurrentVoteType
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600C26F RID: 49775 RVA: 0x0047BBC4 File Offset: 0x00479DC4
		public TwitchVotingManager(TwitchManager owner)
		{
			this.Owner = owner;
			this.SetupVoteDayTimeRanges();
		}

		// Token: 0x0600C270 RID: 49776 RVA: 0x0047BC92 File Offset: 0x00479E92
		public void CleanupData()
		{
			this.VoteTypes.Clear();
			this.VoteGroups.Clear();
		}

		// Token: 0x0600C271 RID: 49777 RVA: 0x0047BCAC File Offset: 0x00479EAC
		public void SetupLocalization()
		{
			this.chatOutput_VoteStarted = Localization.Get("TwitchChat_VoteStarted", false, null);
			this.chatOutput_VoteFinished = Localization.Get("TwitchChat_VoteFinished", false, null);
			this.dayTimeRangeOutput = Localization.Get("xuiOptionsTwitchVoteDayTimeRangeDisplay", false, null);
			this.VoteOptionA = Localization.Get("TwitchVoteOption_A", false, null);
			this.VoteOptionB = Localization.Get("TwitchVoteOption_B", false, null);
			this.VoteOptionC = Localization.Get("TwitchVoteOption_C", false, null);
			this.VoteOptionD = Localization.Get("TwitchVoteOption_D", false, null);
			this.VoteOptionE = Localization.Get("TwitchVoteOption_E", false, null);
		}

		// Token: 0x0600C272 RID: 49778 RVA: 0x0047BD4C File Offset: 0x00479F4C
		public void AddVoteType(TwitchVoteType voteType)
		{
			this.VoteTypes.Add(voteType.Name, voteType);
			for (int i = 0; i < this.VoteGroups.Count; i++)
			{
				if (this.VoteGroups[i].Name == voteType.Group)
				{
					this.VoteGroups[i].VoteTypes.Add(voteType);
					return;
				}
			}
			TwitchVoteGroup twitchVoteGroup = new TwitchVoteGroup(voteType.Group);
			twitchVoteGroup.VoteTypes.Add(voteType);
			this.VoteGroups.Add(twitchVoteGroup);
		}

		// Token: 0x0600C273 RID: 49779 RVA: 0x0047BDDB File Offset: 0x00479FDB
		public TwitchVoteType GetVoteType(string voteTypeName)
		{
			if (this.VoteTypes.ContainsKey(voteTypeName))
			{
				return this.VoteTypes[voteTypeName];
			}
			return null;
		}

		// Token: 0x0600C274 RID: 49780 RVA: 0x0047BDFC File Offset: 0x00479FFC
		public void AddVote(int index, string userName)
		{
			if (!this.voterlist.Contains(userName) && this.voteList.Count > index)
			{
				this.voteList[index].VoteCount++;
				Manager.PlayInsidePlayerHead("twitch_vote_received", -1, 0f, false, false);
				this.voterlist.Add(userName);
				this.UIDirty = true;
				for (int i = 0; i < this.voteList.Count; i++)
				{
					this.voteList[i].UIDirty = true;
				}
			}
		}

		// Token: 0x0600C275 RID: 49781 RVA: 0x0047BE8C File Offset: 0x0047A08C
		public void ClearVotes()
		{
			for (int i = 0; i < this.voteList.Count; i++)
			{
				this.voteList[i].VoteCount = 0;
				this.voteList[i].VoterNames.Clear();
			}
			this.voterlist.Clear();
		}

		// Token: 0x0600C276 RID: 49782 RVA: 0x0047BEE4 File Offset: 0x0047A0E4
		[PublicizedFrom(EAccessModifier.Private)]
		public void CalculateVoteTimes()
		{
			this.DailyVoteTimes.Clear();
			GameRandom gameRandom = GameManager.Instance.World.GetGameRandom();
			TwitchVotingManager.VoteDayTimeRange voteDayTimeRange = this.VoteDayTimeRanges[this.CurrentVoteDayTimeRange];
			float num = (float)(voteDayTimeRange.EndHour - voteDayTimeRange.StartHour) / (float)this.MaxDailyVotes;
			float num2 = (float)voteDayTimeRange.StartHour;
			for (int i = 0; i < this.MaxDailyVotes; i++)
			{
				float num3 = -1f;
				if (i == 0)
				{
					gameRandom.RandomRange(0f, num);
				}
				else
				{
					gameRandom.RandomRange(num - 1f, num);
				}
				int num4 = (int)(num2 + num3);
				int num5 = gameRandom.RandomRange(0, 59);
				TwitchVotingManager.DailyVoteEntry dailyVoteEntry = new TwitchVotingManager.DailyVoteEntry();
				dailyVoteEntry.VoteStartTime = GameUtils.DayTimeToWorldTime(1, num4, num5);
				dailyVoteEntry.VoteEndTime = GameUtils.DayTimeToWorldTime(1, num4 + 1, num5);
				dailyVoteEntry.Index = i + 1;
				num2 += num;
				this.DailyVoteTimes.Add(dailyVoteEntry);
			}
		}

		// Token: 0x0600C277 RID: 49783 RVA: 0x0047BFDC File Offset: 0x0047A1DC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResetVoteTypeDay()
		{
			foreach (TwitchVoteType twitchVoteType in this.VoteTypes.Values)
			{
				twitchVoteType.CurrentDayCount = 0;
			}
			foreach (TwitchVote twitchVote in TwitchActionManager.TwitchVotes.Values)
			{
				twitchVote.CurrentDayCount = 0;
			}
		}

		// Token: 0x0600C278 RID: 49784 RVA: 0x0047C078 File Offset: 0x0047A278
		public bool SetupVoteList(List<TwitchVoteEntry> voteList)
		{
			World world = GameManager.Instance.World;
			TwitchVoteType currentVoteType = this.CurrentVoteType;
			string name = currentVoteType.Name;
			TwitchVote twitchVote = null;
			int highestGameStage = this.Owner.HighestGameStage;
			this.ClearVotes();
			voteList.Clear();
			this.tempSortList.Clear();
			this.tempVoteGroupList.Clear();
			EntityPlayer localPlayer = this.Owner.LocalPlayer;
			if (currentVoteType.GuaranteedGroup != "")
			{
				foreach (TwitchVote twitchVote2 in TwitchActionManager.TwitchVotes.Values)
				{
					if (twitchVote2.Enabled && twitchVote2.IsInPreset(this.Owner.CurrentVotePreset) && twitchVote2.VoteTypes.Contains(name) && twitchVote2.Group == currentVoteType.GuaranteedGroup && twitchVote2.CanUse(this.hour, highestGameStage, localPlayer))
					{
						this.tempSortList.Add(twitchVote2);
					}
				}
				for (int i = 0; i < this.tempSortList.Count * 3; i++)
				{
					int num = world.GetGameRandom().RandomRange(this.tempSortList.Count);
					int num2 = world.GetGameRandom().RandomRange(this.tempSortList.Count);
					if (num != num2)
					{
						TwitchVote twitchVote3 = this.tempSortList[num];
						this.tempSortList[num] = this.tempSortList[num2];
						this.tempSortList[num2] = twitchVote3;
					}
				}
				twitchVote = this.tempSortList[0];
				this.tempSortList.Clear();
			}
			foreach (TwitchVote twitchVote4 in TwitchActionManager.TwitchVotes.Values)
			{
				if (twitchVote4.Enabled && twitchVote4.IsInPreset(this.Owner.CurrentVotePreset) && twitchVote4.VoteTypes.Contains(name) && twitchVote4.CanUse(this.hour, highestGameStage, localPlayer))
				{
					this.tempSortList.Add(twitchVote4);
				}
			}
			for (int j = 0; j < this.tempSortList.Count * 3; j++)
			{
				int num3 = world.GetGameRandom().RandomRange(this.tempSortList.Count);
				int num4 = world.GetGameRandom().RandomRange(this.tempSortList.Count);
				if (num3 != num4)
				{
					TwitchVote twitchVote5 = this.tempSortList[num3];
					this.tempSortList[num3] = this.tempSortList[num4];
					this.tempSortList[num4] = twitchVote5;
				}
			}
			this.NeededLines = 1;
			int num5 = 0;
			if (twitchVote != null)
			{
				this.tempSortList.Insert(global::UnityEngine.Random.Range(0, 3), twitchVote);
			}
			for (int k = 0; k < this.tempSortList.Count; k++)
			{
				TwitchVote twitchVote6 = this.tempSortList[k];
				if (!(twitchVote6.Group != "") || !this.tempVoteGroupList.Contains(twitchVote6.Group))
				{
					if (twitchVote6.Group != "")
					{
						this.tempVoteGroupList.Add(twitchVote6.Group);
					}
					string text = this.VoteOptionA;
					switch (num5)
					{
					case 1:
						text = this.VoteOptionB;
						break;
					case 2:
						text = this.VoteOptionC;
						break;
					case 3:
						text = this.VoteOptionD;
						break;
					case 4:
						text = this.VoteOptionE;
						break;
					}
					if (twitchVote6.VoteLine1 != "" && this.NeededLines < 2)
					{
						this.NeededLines = 2;
					}
					if (twitchVote6.VoteLine2 != "" && this.NeededLines < 3)
					{
						this.NeededLines = 3;
					}
					voteList.Add(new TwitchVoteEntry(text, twitchVote6)
					{
						Owner = this,
						Index = num5
					});
					num5++;
					if (num5 == currentVoteType.VoteChoiceCount)
					{
						break;
					}
				}
			}
			return voteList.Count != 0;
		}

		// Token: 0x0600C279 RID: 49785 RVA: 0x0047C4BC File Offset: 0x0047A6BC
		public TwitchVoteEntry GetVoteWinner()
		{
			this.tempVoteList.Clear();
			int num = -1;
			for (int i = 0; i < this.voteList.Count; i++)
			{
				if (this.voteList[i].VoteCount > num)
				{
					num = this.voteList[i].VoteCount;
					this.tempVoteList.Clear();
					this.tempVoteList.Add(this.voteList[i]);
				}
				else if (this.voteList[i].VoteCount == num)
				{
					this.tempVoteList.Add(this.voteList[i]);
				}
			}
			return this.tempVoteList[GameManager.Instance.World.GetGameRandom().RandomRange(0, this.tempVoteList.Count)];
		}

		// Token: 0x0600C27A RID: 49786 RVA: 0x0047C590 File Offset: 0x0047A790
		public void ResetVoteOnDeath()
		{
			TwitchVotingManager.VoteStateTypes currentVoteState = this.CurrentVoteState;
			if (currentVoteState - TwitchVotingManager.VoteStateTypes.VoteStarted <= 1)
			{
				this.readyForVote = false;
				this.Owner.LocalPlayer.TwitchVoteLock = TwitchVoteLockTypes.None;
				this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.WaitingForNextVote;
				this.ResetVoteGroupsForVote();
				if (this.VoteEventEnded != null)
				{
					this.VoteEventEnded();
				}
			}
		}

		// Token: 0x0600C27B RID: 49787 RVA: 0x0047C5E4 File Offset: 0x0047A7E4
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResetVoteGroupsForVote()
		{
			for (int i = 0; i < this.VoteGroups.Count; i++)
			{
				this.VoteGroups[i].SkippedThisVote = false;
			}
		}

		// Token: 0x0600C27C RID: 49788 RVA: 0x0047C61C File Offset: 0x0047A81C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckAllVoteGroupsSkipped()
		{
			for (int i = 0; i < this.VoteGroups.Count; i++)
			{
				if (!this.VoteGroups[i].SkippedThisVote)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C27D RID: 49789 RVA: 0x0047C658 File Offset: 0x0047A858
		[PublicizedFrom(EAccessModifier.Private)]
		public bool AllowVoting()
		{
			return this.Owner.CooldownType == TwitchManager.CooldownTypes.None || ((this.Owner.CooldownType == TwitchManager.CooldownTypes.QuestCooldown || this.Owner.CooldownType == TwitchManager.CooldownTypes.QuestDisabled) && this.AllowVotesDuringQuests) || ((this.Owner.CooldownType == TwitchManager.CooldownTypes.BloodMoonCooldown || this.Owner.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled) && this.AllowVotesDuringBloodmoon);
		}

		// Token: 0x0600C27E RID: 49790 RVA: 0x0047C6BC File Offset: 0x0047A8BC
		public void Update(float deltaTime)
		{
			switch (this.CurrentVoteState)
			{
			case TwitchVotingManager.VoteStateTypes.Init:
				GameEventManager.Current.GameEventApproved += this.Current_GameEventApproved;
				GameEventManager.Current.GameEventCompleted += this.Current_GameEventCompleted;
				GameEventManager.Current.GameEntitySpawned += this.Current_GameEntitySpawned;
				GameEventManager.Current.GameEntityKilled += this.Current_GameEntityKilled;
				this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.WaitingForNextVote;
				this.ShuffleVoteGroups();
				this.ShuffleVoteGroupVoteTypes();
				return;
			case TwitchVotingManager.VoteStateTypes.WaitingForNextVote:
			{
				if (this.QueuedVoteType != null)
				{
					this.CurrentVoteType = this.QueuedVoteType;
					if (this.SetupVoteList(this.voteList))
					{
						this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.ReadyForVoteStart;
						this.Owner.RefreshVoteLockedLevel();
					}
					this.QueuedVoteType = null;
				}
				if (this.VoteStartDelayTimeRemaining > 0f)
				{
					this.VoteStartDelayTimeRemaining -= deltaTime;
					return;
				}
				World world = GameManager.Instance.World;
				ulong num = world.worldTime;
				this.day = GameUtils.WorldTimeToDays(num);
				num %= 24000UL;
				this.hour = GameUtils.WorldTimeToHours(num);
				if (this.day == 1)
				{
					return;
				}
				TwitchVotingManager.VoteDayTimeRange voteDayTimeRange = this.VoteDayTimeRanges[this.CurrentVoteDayTimeRange];
				bool flag = !this.AllowVotesDuringBloodmoon && world.IsWorldEvent(World.WorldEvent.BloodMoon);
				if (this.VoteInProgress)
				{
					if (flag)
					{
						this.CancelVote();
					}
					if (this.hour < voteDayTimeRange.StartHour || this.hour > voteDayTimeRange.EndHour)
					{
						this.CancelVote();
					}
					return;
				}
				if (flag || this.hour < voteDayTimeRange.StartHour || this.hour > voteDayTimeRange.EndHour)
				{
					return;
				}
				if (this.day != this.lastGameDay)
				{
					this.CalculateVoteTimes();
					this.ResetVoteTypeDay();
					this.lastGameDay = this.day;
				}
				for (int i = 0; i < this.DailyVoteTimes.Count; i++)
				{
					if (this.DailyVoteTimes[i].LastVoteDay != this.day)
					{
						if (num > this.DailyVoteTimes[i].VoteStartTime && num < this.DailyVoteTimes[i].VoteEndTime)
						{
							if (!this.SetReadyForVote(this.DailyVoteTimes[i].Index))
							{
								if (this.CheckAllVoteGroupsSkipped())
								{
									this.DailyVoteTimes[i].LastVoteDay = this.day;
									this.ResetVoteGroupsForVote();
								}
								return;
							}
							this.ResetVoteGroupsForVote();
							if (this.SetupVoteList(this.voteList))
							{
								this.DailyVoteTimes[i].LastVoteDay = this.day;
								this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.ReadyForVoteStart;
								this.Owner.RefreshVoteLockedLevel();
							}
						}
						else if (num > this.DailyVoteTimes[i].VoteEndTime)
						{
							this.DailyVoteTimes[i].LastVoteDay = this.day;
						}
					}
				}
				return;
			}
			case TwitchVotingManager.VoteStateTypes.ReadyForVoteStart:
				if (this.VoteStartDelayTimeRemaining > 0f)
				{
					this.VoteStartDelayTimeRemaining -= deltaTime;
					return;
				}
				if (this.VotingEnabled && this.Owner.VoteLockedLevel == TwitchVoteLockTypes.None && this.AllowVoting() && (!this.CurrentVoteType.SpawnBlocked || this.Owner.ReadyForVote))
				{
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageTwitchVoteScheduling>().Setup(), false);
					}
					else
					{
						TwitchVoteScheduler.Current.AddParticipant(this.Owner.LocalPlayer.entityId);
					}
					this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.RequestedVoteStart;
					return;
				}
				break;
			case TwitchVotingManager.VoteStateTypes.RequestedVoteStart:
				break;
			case TwitchVotingManager.VoteStateTypes.VoteReady:
				if (this.VotingEnabled && this.Owner.VoteLockedLevel == TwitchVoteLockTypes.None && !this.CheckVoteLock() && this.AllowVoting() && (!this.CurrentVoteType.SpawnBlocked || this.Owner.ReadyForVote))
				{
					this.StartVote();
					return;
				}
				break;
			case TwitchVotingManager.VoteStateTypes.VoteStarted:
				if (this.VotingEnabled && this.AllowVoting())
				{
					this.VoteTimeRemaining -= Time.deltaTime;
					if (this.VoteTimeRemaining <= 0f)
					{
						this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.VoteFinished;
						return;
					}
				}
				break;
			case TwitchVotingManager.VoteStateTypes.VoteFinished:
				this.CurrentEvent = this.GetVoteWinner();
				this.CurrentEvent.ActiveSpawns.Clear();
				this.CurrentEvent.VoteClass.CurrentDayCount++;
				this.Owner.ircClient.SendChannelMessage(string.Format(this.chatOutput_VoteFinished, this.CurrentEvent.VoteClass.VoteDescription), true);
				this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.WaitingForActive;
				this.VoteTimeRemaining = 2f;
				QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.VoteComplete, this.CurrentEvent.VoteClass.Group);
				return;
			case TwitchVotingManager.VoteStateTypes.WaitingForActive:
				if (this.VoteTimeRemaining > 0f)
				{
					this.VoteTimeRemaining -= deltaTime;
					return;
				}
				if (GameEventManager.Current.HandleAction(this.CurrentEvent.VoteClass.GameEvent, this.Owner.LocalPlayer, this.Owner.LocalPlayer, true, " ", "vote", this.Owner.AllowCrateSharing, true, "", null))
				{
					GameEventManager.Current.HandleGameEventApproved(this.CurrentEvent.VoteClass.GameEvent, this.Owner.LocalPlayer.entityId, " ", "vote");
					return;
				}
				this.VoteTimeRemaining = 10f;
				return;
			case TwitchVotingManager.VoteStateTypes.EventActive:
				if (this.VoteEventTimeRemaining < 0f)
				{
					if (this.VoteEventComplete)
					{
						this.HandleGameEventEnded(true);
						return;
					}
				}
				else
				{
					this.VoteEventTimeRemaining -= deltaTime;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600C27F RID: 49791 RVA: 0x0047CC56 File Offset: 0x0047AE56
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckVoteLock()
		{
			return (!this.AllowVotesDuringQuests && QuestEventManager.Current.QuestBounds.width != 0f) || (!this.AllowVotesInSafeZone && this.Owner.IsSafe);
		}

		// Token: 0x0600C280 RID: 49792 RVA: 0x0047CC90 File Offset: 0x0047AE90
		public bool IsHighest(TwitchVoteEntry vote)
		{
			for (int i = 0; i < this.voteList.Count; i++)
			{
				if (i != vote.Index && this.voteList[i].VoteCount > vote.VoteCount)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C281 RID: 49793 RVA: 0x0047CCD8 File Offset: 0x0047AED8
		public bool SetReadyForVote(int index)
		{
			return this.GetNextVoteType();
		}

		// Token: 0x0600C282 RID: 49794 RVA: 0x0047CCE0 File Offset: 0x0047AEE0
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResetCurrentVote()
		{
			this.VoteTimeRemaining = this.VoteTime;
		}

		// Token: 0x0600C283 RID: 49795 RVA: 0x0047CCF0 File Offset: 0x0047AEF0
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetupVoteDayTimeRanges()
		{
			this.VoteDayTimeRanges.Clear();
			this.VoteDayTimeRanges.Add(new TwitchVotingManager.VoteDayTimeRange
			{
				Name = Localization.Get("TwitchVoteDayTimeRange_Short", false, null),
				StartHour = 8,
				EndHour = 16
			});
			this.VoteDayTimeRanges.Add(new TwitchVotingManager.VoteDayTimeRange
			{
				Name = Localization.Get("TwitchVoteDayTimeRange_Average", false, null),
				StartHour = 6,
				EndHour = 18
			});
			this.VoteDayTimeRanges.Add(new TwitchVotingManager.VoteDayTimeRange
			{
				Name = Localization.Get("TwitchVoteDayTimeRange_Extended", false, null),
				StartHour = 4,
				EndHour = 20
			});
			this.VoteDayTimeRanges.Add(new TwitchVotingManager.VoteDayTimeRange
			{
				Name = Localization.Get("TwitchVoteDayTimeRange_All", false, null),
				StartHour = 0,
				EndHour = 23
			});
		}

		// Token: 0x0600C284 RID: 49796 RVA: 0x0047CDCC File Offset: 0x0047AFCC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ShuffleVoteGroups()
		{
			for (int i = 0; i <= this.VoteGroups.Count * this.VoteGroups.Count; i++)
			{
				int num = global::UnityEngine.Random.Range(0, this.VoteGroups.Count);
				int num2 = global::UnityEngine.Random.Range(0, this.VoteGroups.Count);
				if (num != num2)
				{
					TwitchVoteGroup twitchVoteGroup = this.VoteGroups[num];
					this.VoteGroups[num] = this.VoteGroups[num2];
					this.VoteGroups[num2] = twitchVoteGroup;
				}
			}
		}

		// Token: 0x0600C285 RID: 49797 RVA: 0x0047CE58 File Offset: 0x0047B058
		[PublicizedFrom(EAccessModifier.Private)]
		public void ShuffleVoteGroupVoteTypes()
		{
			for (int i = 0; i < this.VoteGroups.Count; i++)
			{
				this.VoteGroups[i].ShuffleVoteTypes();
			}
		}

		// Token: 0x0600C286 RID: 49798 RVA: 0x0047CE8C File Offset: 0x0047B08C
		public void CancelVote()
		{
			if (this.CurrentVoteState == TwitchVotingManager.VoteStateTypes.WaitingForNextVote && this.readyForVote)
			{
				this.readyForVote = false;
			}
		}

		// Token: 0x0600C287 RID: 49799 RVA: 0x0047CEA6 File Offset: 0x0047B0A6
		public void RequestApprovedToStart()
		{
			this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.VoteReady;
		}

		// Token: 0x0600C288 RID: 49800 RVA: 0x0047CEB0 File Offset: 0x0047B0B0
		public void StartVote()
		{
			this.VoteTimeRemaining = this.VoteTime;
			this.voterlist.Clear();
			if (this.VoteStarted != null)
			{
				this.VoteStarted();
			}
			this.Owner.UIDirty = true;
			this.Owner.LocalPlayer.TwitchVoteLock = (this.CurrentVoteType.ActionLockout ? TwitchVoteLockTypes.ActionsLocked : TwitchVoteLockTypes.VoteLocked);
			this.Owner.ircClient.SendChannelMessage(this.chatOutput_VoteStarted, true);
			Manager.BroadcastPlay(this.Owner.LocalPlayer.position, "twitch_vote_started", 0f);
			this.readyForVote = false;
			this.CurrentVoteType.CurrentDayCount++;
			this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.VoteStarted;
		}

		// Token: 0x0600C289 RID: 49801 RVA: 0x0047CF6C File Offset: 0x0047B16C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEntitySpawned(string gameEventID, int entityID, string tag)
		{
			if (this.CurrentEvent == null || tag != "vote")
			{
				return;
			}
			if (gameEventID == this.CurrentEvent.VoteClass.GameEvent)
			{
				this.CurrentEvent.ActiveSpawns.Add(entityID);
			}
		}

		// Token: 0x0600C28A RID: 49802 RVA: 0x0047CFB8 File Offset: 0x0047B1B8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEntityKilled(int entityID)
		{
			if (this.CurrentEvent == null)
			{
				return;
			}
			if (this.CurrentEvent.ActiveSpawns.Contains(entityID))
			{
				this.CurrentEvent.ActiveSpawns.Remove(entityID);
			}
		}

		// Token: 0x0600C28B RID: 49803 RVA: 0x0047CFE8 File Offset: 0x0047B1E8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEventCompleted(string gameEventID, int targetEntityID, string extraData, string tag)
		{
			if (this.CurrentEvent != null && this.CurrentEvent.VoteClass.GameEvent == gameEventID && tag == "vote")
			{
				this.VoteEventComplete = true;
			}
		}

		// Token: 0x0600C28C RID: 49804 RVA: 0x0047D020 File Offset: 0x0047B220
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleGameEventEnded(bool playSound)
		{
			if (this.CurrentVoteType.CooldownOnEnd && this.Owner.AllowActions && this.Owner.CurrentCooldownPreset.CooldownType == CooldownPreset.CooldownTypes.Fill)
			{
				this.Owner.SetCooldown((float)this.Owner.CurrentCooldownPreset.NextCooldownTime, TwitchManager.CooldownTypes.MaxReached, false, true);
			}
			this.CurrentEvent.VoteClass.HandleVoteComplete();
			this.CurrentEvent = null;
			if (playSound)
			{
				Manager.BroadcastPlay(this.Owner.LocalPlayer.position, "twitch_vote_ended", 0f);
			}
			this.Owner.LocalPlayer.TwitchVoteLock = TwitchVoteLockTypes.None;
			this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.WaitingForNextVote;
			this.ResetVoteGroupsForVote();
			if (this.VoteEventEnded != null)
			{
				this.VoteEventEnded();
			}
			this.VoteStartDelayTimeRemaining = 10f;
		}

		// Token: 0x0600C28D RID: 49805 RVA: 0x0047D0F0 File Offset: 0x0047B2F0
		[PublicizedFrom(EAccessModifier.Private)]
		public bool GetNextVoteType()
		{
			if (this.voteGroupIndex == -1)
			{
				this.voteGroupIndex = GameManager.Instance.World.GetGameRandom().RandomRange(this.VoteGroups.Count);
			}
			bool flag = GameManager.Instance.World.IsWorldEvent(World.WorldEvent.BloodMoon);
			TwitchVoteGroup twitchVoteGroup = this.VoteGroups[this.voteGroupIndex];
			if (!twitchVoteGroup.SkippedThisVote)
			{
				for (int i = 0; i < twitchVoteGroup.VoteTypes.Count; i++)
				{
					TwitchVoteType nextVoteType = twitchVoteGroup.GetNextVoteType();
					if (nextVoteType.IsInPreset(this.Owner.CurrentVotePreset.Name) && !nextVoteType.ManualStart && nextVoteType.CanUse() && this.hour >= nextVoteType.AllowedStartHour && this.hour <= nextVoteType.AllowedEndHour && (!nextVoteType.IsBoss || this.Owner.CurrentVotePreset.BossVoteSetting != TwitchVotingManager.BossVoteSettings.Disabled) && (nextVoteType.AllowedWithActions || !this.Owner.AllowActions) && ((nextVoteType.IsBoss && this.Owner.CurrentVotePreset.BossVoteSetting == TwitchVotingManager.BossVoteSettings.Daily) || ((nextVoteType.BloodMoonDay || this.Owner.nextBMDay != this.day) && (nextVoteType.BloodMoonAllowed || !flag))))
					{
						this.CurrentVoteType = nextVoteType;
						this.voteGroupIndex++;
						if (this.voteGroupIndex >= this.VoteGroups.Count)
						{
							this.voteGroupIndex = 0;
						}
						return true;
					}
				}
				twitchVoteGroup.SkippedThisVote = true;
			}
			this.voteGroupIndex++;
			if (this.voteGroupIndex >= this.VoteGroups.Count)
			{
				this.voteGroupIndex = 0;
			}
			return false;
		}

		// Token: 0x0600C28E RID: 49806 RVA: 0x0047D2A4 File Offset: 0x0047B4A4
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEventApproved(string gameEventID, int targetEntityID, string extraData, string tag)
		{
			if (this.CurrentVoteState == TwitchVotingManager.VoteStateTypes.WaitingForActive && this.CurrentEvent.VoteClass.GameEvent == gameEventID)
			{
				this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.EventActive;
				this.VoteEventComplete = false;
				this.VoteEventTimeRemaining = 10f;
				this.CurrentEvent.VoterNames.AddRange(this.voterlist);
				this.Owner.AddVoteHistory(this.CurrentEvent.VoteClass);
				if (this.VoteEventStarted != null)
				{
					this.VoteEventStarted();
				}
			}
		}

		// Token: 0x0600C28F RID: 49807 RVA: 0x0047D32C File Offset: 0x0047B52C
		public void HandleMessage(TwitchIRCClient.TwitchChatMessage message)
		{
			if (this.CurrentVoteState == TwitchVotingManager.VoteStateTypes.VoteStarted)
			{
				if (message.Message.EqualsCaseInsensitive(this.VoteOptionA))
				{
					this.AddVote(0, message.UserName);
					return;
				}
				if (message.Message.EqualsCaseInsensitive(this.VoteOptionB))
				{
					this.AddVote(1, message.UserName);
					return;
				}
				if (message.Message.EqualsCaseInsensitive(this.VoteOptionC))
				{
					this.AddVote(2, message.UserName);
					return;
				}
				if (message.Message.EqualsCaseInsensitive(this.VoteOptionD))
				{
					this.AddVote(3, message.UserName);
					return;
				}
				if (message.Message.EqualsCaseInsensitive(this.VoteOptionE))
				{
					this.AddVote(4, message.UserName);
				}
			}
		}

		// Token: 0x0600C290 RID: 49808 RVA: 0x0047D3EC File Offset: 0x0047B5EC
		public List<string> HandleKiller(TwitchVoteEntry voteEntry)
		{
			if (this.CurrentEvent == null && voteEntry == null)
			{
				return null;
			}
			if (this.CurrentEvent != null)
			{
				List<string> voterNames = this.CurrentEvent.VoterNames;
				this.CurrentEvent.Complete = true;
				this.HandleGameEventEnded(false);
				return this.voterlist;
			}
			if (voteEntry != null)
			{
				return voteEntry.VoterNames;
			}
			return null;
		}

		// Token: 0x0600C291 RID: 49809 RVA: 0x0047D440 File Offset: 0x0047B640
		public string GetDayTimeRange(int tempVoteDayTimeRange)
		{
			TwitchVotingManager.VoteDayTimeRange voteDayTimeRange = this.VoteDayTimeRanges[tempVoteDayTimeRange];
			if (voteDayTimeRange.StartHour == 0 && voteDayTimeRange.EndHour == 23)
			{
				return voteDayTimeRange.Name;
			}
			return string.Format(this.dayTimeRangeOutput, voteDayTimeRange.StartHour, voteDayTimeRange.EndHour);
		}

		// Token: 0x0600C292 RID: 49810 RVA: 0x0047D494 File Offset: 0x0047B694
		public void QueueVote(string voteType)
		{
			if (this.VoteTypes.ContainsKey(voteType))
			{
				this.QueuedVoteType = this.VoteTypes[voteType];
			}
		}

		// Token: 0x0600C293 RID: 49811 RVA: 0x0047D4B6 File Offset: 0x0047B6B6
		public void ForceEndVote()
		{
			if (this.CurrentVoteState == TwitchVotingManager.VoteStateTypes.VoteStarted)
			{
				this.CurrentEvent = null;
				this.Owner.LocalPlayer.TwitchVoteLock = TwitchVoteLockTypes.None;
				this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.WaitingForNextVote;
				if (this.VoteEventEnded != null)
				{
					this.VoteEventEnded();
				}
			}
		}

		// Token: 0x040092C5 RID: 37573
		public TwitchManager Owner;

		// Token: 0x040092C6 RID: 37574
		public TwitchVotingManager.VoteStateTypes CurrentVoteState;

		// Token: 0x040092C7 RID: 37575
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_VoteStarted;

		// Token: 0x040092C8 RID: 37576
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_VoteFinished;

		// Token: 0x040092C9 RID: 37577
		[PublicizedFrom(EAccessModifier.Private)]
		public string dayTimeRangeOutput;

		// Token: 0x040092CA RID: 37578
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionA;

		// Token: 0x040092CB RID: 37579
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionB;

		// Token: 0x040092CC RID: 37580
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionC;

		// Token: 0x040092CD RID: 37581
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionD;

		// Token: 0x040092CE RID: 37582
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionE;

		// Token: 0x040092CF RID: 37583
		[PublicizedFrom(EAccessModifier.Private)]
		public int maxDailyVotes = 4;

		// Token: 0x040092D0 RID: 37584
		public int lastGameDay = 1;

		// Token: 0x040092D1 RID: 37585
		public bool WinnerShowing;

		// Token: 0x040092D2 RID: 37586
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentVoteDayTimeRange = 2;

		// Token: 0x040092D3 RID: 37587
		public List<TwitchVotingManager.VoteDayTimeRange> VoteDayTimeRanges = new List<TwitchVotingManager.VoteDayTimeRange>();

		// Token: 0x040092D4 RID: 37588
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVotingManager.DailyVoteEntry> DailyVoteTimes = new List<TwitchVotingManager.DailyVoteEntry>();

		// Token: 0x040092D5 RID: 37589
		public bool AllowVotesDuringBloodmoon;

		// Token: 0x040092D6 RID: 37590
		public bool AllowVotesDuringQuests;

		// Token: 0x040092D7 RID: 37591
		public bool AllowVotesInSafeZone;

		// Token: 0x040092D8 RID: 37592
		public List<TwitchVoteType> NextVotes = new List<TwitchVoteType>();

		// Token: 0x040092D9 RID: 37593
		public bool VoteInProgress;

		// Token: 0x040092DA RID: 37594
		public float VoteTime = 60f;

		// Token: 0x040092DB RID: 37595
		public int ViewerDefeatReward = 250;

		// Token: 0x040092DD RID: 37597
		public float VoteStartDelayTimeRemaining;

		// Token: 0x040092DE RID: 37598
		public float VoteEventTimeRemaining;

		// Token: 0x040092DF RID: 37599
		public float VoteTimeRemaining;

		// Token: 0x040092E0 RID: 37600
		public bool UIDirty;

		// Token: 0x040092E1 RID: 37601
		public bool VoteEventComplete;

		// Token: 0x040092E2 RID: 37602
		public List<TwitchVoteEntry> voteList = new List<TwitchVoteEntry>();

		// Token: 0x040092E3 RID: 37603
		public List<string> voterlist = new List<string>();

		// Token: 0x040092E4 RID: 37604
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVoteEntry> tempVoteList = new List<TwitchVoteEntry>();

		// Token: 0x040092E5 RID: 37605
		public OnGameEventVoteAction VoteStarted;

		// Token: 0x040092E6 RID: 37606
		public OnGameEventVoteAction VoteEventStarted;

		// Token: 0x040092E7 RID: 37607
		public OnGameEventVoteAction VoteEventEnded;

		// Token: 0x040092E8 RID: 37608
		public Dictionary<string, TwitchVoteType> VoteTypes = new Dictionary<string, TwitchVoteType>();

		// Token: 0x040092E9 RID: 37609
		[PublicizedFrom(EAccessModifier.Private)]
		public int voteGroupIndex = -1;

		// Token: 0x040092EA RID: 37610
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVoteGroup> VoteGroups = new List<TwitchVoteGroup>();

		// Token: 0x040092EB RID: 37611
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchVoteType QueuedVoteType;

		// Token: 0x040092ED RID: 37613
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVote> tempSortList = new List<TwitchVote>();

		// Token: 0x040092EE RID: 37614
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> tempVoteGroupList = new List<string>();

		// Token: 0x040092EF RID: 37615
		[PublicizedFrom(EAccessModifier.Private)]
		public int day = -1;

		// Token: 0x040092F0 RID: 37616
		[PublicizedFrom(EAccessModifier.Private)]
		public int hour = -1;

		// Token: 0x040092F1 RID: 37617
		[PublicizedFrom(EAccessModifier.Private)]
		public bool readyForVote;

		// Token: 0x040092F2 RID: 37618
		public TwitchVoteEntry CurrentEvent;

		// Token: 0x0200189A RID: 6298
		public enum VoteStateTypes
		{
			// Token: 0x040092F4 RID: 37620
			Init,
			// Token: 0x040092F5 RID: 37621
			WaitingForNextVote,
			// Token: 0x040092F6 RID: 37622
			ReadyForVoteStart,
			// Token: 0x040092F7 RID: 37623
			RequestedVoteStart,
			// Token: 0x040092F8 RID: 37624
			VoteReady,
			// Token: 0x040092F9 RID: 37625
			VoteStarted,
			// Token: 0x040092FA RID: 37626
			VoteFinished,
			// Token: 0x040092FB RID: 37627
			WaitingForActive,
			// Token: 0x040092FC RID: 37628
			EventActive
		}

		// Token: 0x0200189B RID: 6299
		public enum BossVoteSettings
		{
			// Token: 0x040092FE RID: 37630
			Disabled,
			// Token: 0x040092FF RID: 37631
			Standard,
			// Token: 0x04009300 RID: 37632
			Daily
		}

		// Token: 0x0200189C RID: 6300
		public class DailyVoteEntry
		{
			// Token: 0x04009301 RID: 37633
			public ulong VoteStartTime;

			// Token: 0x04009302 RID: 37634
			public ulong VoteEndTime;

			// Token: 0x04009303 RID: 37635
			public int LastVoteDay;

			// Token: 0x04009304 RID: 37636
			public int Index = -1;
		}

		// Token: 0x0200189D RID: 6301
		public class VoteDayTimeRange
		{
			// Token: 0x04009305 RID: 37637
			public string Name;

			// Token: 0x04009306 RID: 37638
			public int StartHour;

			// Token: 0x04009307 RID: 37639
			public int EndHour;
		}
	}
}
