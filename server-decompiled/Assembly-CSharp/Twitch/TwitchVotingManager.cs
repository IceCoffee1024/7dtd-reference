using System;
using System.Collections.Generic;
using Audio;
using Challenges;
using UniLinq;
using UnityEngine;

namespace Twitch
{
	// Token: 0x0200186D RID: 6253
	public class TwitchVotingManager
	{
		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x0600C108 RID: 49416 RVA: 0x00477563 File Offset: 0x00475763
		// (set) Token: 0x0600C109 RID: 49417 RVA: 0x0047756B File Offset: 0x0047576B
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

		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x0600C10A RID: 49418 RVA: 0x00477584 File Offset: 0x00475784
		// (set) Token: 0x0600C10B RID: 49419 RVA: 0x0047758C File Offset: 0x0047578C
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

		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x0600C10C RID: 49420 RVA: 0x004775A5 File Offset: 0x004757A5
		public bool VotingEnabled
		{
			get
			{
				return !this.Owner.CurrentVotePreset.IsEmpty;
			}
		}

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x0600C10D RID: 49421 RVA: 0x004775BA File Offset: 0x004757BA
		public bool VotingIsActive
		{
			get
			{
				return this.VotingEnabled && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.WaitingForNextVote && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.Init && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.ReadyForVoteStart && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.RequestedVoteStart && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.VoteReady;
			}
		}

		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x0600C10E RID: 49422 RVA: 0x004775F5 File Offset: 0x004757F5
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

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x0600C10F RID: 49423 RVA: 0x0047760C File Offset: 0x0047580C
		public string VoteTypeText
		{
			get
			{
				return this.CurrentVoteType.Title;
			}
		}

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x0600C110 RID: 49424 RVA: 0x00477619 File Offset: 0x00475819
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

		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x0600C111 RID: 49425 RVA: 0x00477639 File Offset: 0x00475839
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

		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x0600C112 RID: 49426 RVA: 0x00477659 File Offset: 0x00475859
		public bool UseMystery
		{
			get
			{
				return this.CurrentVoteType.UseMystery;
			}
		}

		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x0600C113 RID: 49427 RVA: 0x00477666 File Offset: 0x00475866
		// (set) Token: 0x0600C114 RID: 49428 RVA: 0x0047766E File Offset: 0x0047586E
		public int NeededLines
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170017AD RID: 6061
		// (get) Token: 0x0600C115 RID: 49429 RVA: 0x00477677 File Offset: 0x00475877
		// (set) Token: 0x0600C116 RID: 49430 RVA: 0x0047767F File Offset: 0x0047587F
		public TwitchVoteType CurrentVoteType
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600C117 RID: 49431 RVA: 0x00477688 File Offset: 0x00475888
		public TwitchVotingManager(TwitchManager owner)
		{
			this.Owner = owner;
			this.SetupVoteDayTimeRanges();
		}

		// Token: 0x0600C118 RID: 49432 RVA: 0x00477756 File Offset: 0x00475956
		public void CleanupData()
		{
			this.VoteTypes.Clear();
			this.VoteGroups.Clear();
		}

		// Token: 0x0600C119 RID: 49433 RVA: 0x00477770 File Offset: 0x00475970
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

		// Token: 0x0600C11A RID: 49434 RVA: 0x00477810 File Offset: 0x00475A10
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

		// Token: 0x0600C11B RID: 49435 RVA: 0x0047789F File Offset: 0x00475A9F
		public TwitchVoteType GetVoteType(string voteTypeName)
		{
			if (this.VoteTypes.ContainsKey(voteTypeName))
			{
				return this.VoteTypes[voteTypeName];
			}
			return null;
		}

		// Token: 0x0600C11C RID: 49436 RVA: 0x004778C0 File Offset: 0x00475AC0
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

		// Token: 0x0600C11D RID: 49437 RVA: 0x00477950 File Offset: 0x00475B50
		public void ClearVotes()
		{
			for (int i = 0; i < this.voteList.Count; i++)
			{
				this.voteList[i].VoteCount = 0;
				this.voteList[i].VoterNames.Clear();
			}
			this.voterlist.Clear();
		}

		// Token: 0x0600C11E RID: 49438 RVA: 0x004779A8 File Offset: 0x00475BA8
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

		// Token: 0x0600C11F RID: 49439 RVA: 0x00477AA0 File Offset: 0x00475CA0
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

		// Token: 0x0600C120 RID: 49440 RVA: 0x00477B3C File Offset: 0x00475D3C
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

		// Token: 0x0600C121 RID: 49441 RVA: 0x00477F80 File Offset: 0x00476180
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

		// Token: 0x0600C122 RID: 49442 RVA: 0x00478054 File Offset: 0x00476254
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

		// Token: 0x0600C123 RID: 49443 RVA: 0x004780A8 File Offset: 0x004762A8
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResetVoteGroupsForVote()
		{
			for (int i = 0; i < this.VoteGroups.Count; i++)
			{
				this.VoteGroups[i].SkippedThisVote = false;
			}
		}

		// Token: 0x0600C124 RID: 49444 RVA: 0x004780E0 File Offset: 0x004762E0
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

		// Token: 0x0600C125 RID: 49445 RVA: 0x0047811C File Offset: 0x0047631C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool AllowVoting()
		{
			return this.Owner.CooldownType == TwitchManager.CooldownTypes.None || ((this.Owner.CooldownType == TwitchManager.CooldownTypes.QuestCooldown || this.Owner.CooldownType == TwitchManager.CooldownTypes.QuestDisabled) && this.AllowVotesDuringQuests) || ((this.Owner.CooldownType == TwitchManager.CooldownTypes.BloodMoonCooldown || this.Owner.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled) && this.AllowVotesDuringBloodmoon);
		}

		// Token: 0x0600C126 RID: 49446 RVA: 0x00478180 File Offset: 0x00476380
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

		// Token: 0x0600C127 RID: 49447 RVA: 0x0047871A File Offset: 0x0047691A
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckVoteLock()
		{
			return (!this.AllowVotesDuringQuests && QuestEventManager.Current.QuestBounds.width != 0f) || (!this.AllowVotesInSafeZone && this.Owner.IsSafe);
		}

		// Token: 0x0600C128 RID: 49448 RVA: 0x00478754 File Offset: 0x00476954
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

		// Token: 0x0600C129 RID: 49449 RVA: 0x0047879C File Offset: 0x0047699C
		public bool SetReadyForVote(int index)
		{
			return this.GetNextVoteType();
		}

		// Token: 0x0600C12A RID: 49450 RVA: 0x004787A4 File Offset: 0x004769A4
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResetCurrentVote()
		{
			this.VoteTimeRemaining = this.VoteTime;
		}

		// Token: 0x0600C12B RID: 49451 RVA: 0x004787B4 File Offset: 0x004769B4
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

		// Token: 0x0600C12C RID: 49452 RVA: 0x00478890 File Offset: 0x00476A90
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

		// Token: 0x0600C12D RID: 49453 RVA: 0x0047891C File Offset: 0x00476B1C
		[PublicizedFrom(EAccessModifier.Private)]
		public void ShuffleVoteGroupVoteTypes()
		{
			for (int i = 0; i < this.VoteGroups.Count; i++)
			{
				this.VoteGroups[i].ShuffleVoteTypes();
			}
		}

		// Token: 0x0600C12E RID: 49454 RVA: 0x00478950 File Offset: 0x00476B50
		public void CancelVote()
		{
			if (this.CurrentVoteState == TwitchVotingManager.VoteStateTypes.WaitingForNextVote && this.readyForVote)
			{
				this.readyForVote = false;
			}
		}

		// Token: 0x0600C12F RID: 49455 RVA: 0x0047896A File Offset: 0x00476B6A
		public void RequestApprovedToStart()
		{
			this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.VoteReady;
		}

		// Token: 0x0600C130 RID: 49456 RVA: 0x00478974 File Offset: 0x00476B74
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

		// Token: 0x0600C131 RID: 49457 RVA: 0x00478A30 File Offset: 0x00476C30
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

		// Token: 0x0600C132 RID: 49458 RVA: 0x00478A7C File Offset: 0x00476C7C
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

		// Token: 0x0600C133 RID: 49459 RVA: 0x00478AAC File Offset: 0x00476CAC
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEventCompleted(string gameEventID, int targetEntityID, string extraData, string tag)
		{
			if (this.CurrentEvent != null && this.CurrentEvent.VoteClass.GameEvent == gameEventID && tag == "vote")
			{
				this.VoteEventComplete = true;
			}
		}

		// Token: 0x0600C134 RID: 49460 RVA: 0x00478AE4 File Offset: 0x00476CE4
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

		// Token: 0x0600C135 RID: 49461 RVA: 0x00478BB4 File Offset: 0x00476DB4
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

		// Token: 0x0600C136 RID: 49462 RVA: 0x00478D68 File Offset: 0x00476F68
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

		// Token: 0x0600C137 RID: 49463 RVA: 0x00478DF0 File Offset: 0x00476FF0
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

		// Token: 0x0600C138 RID: 49464 RVA: 0x00478EB0 File Offset: 0x004770B0
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

		// Token: 0x0600C139 RID: 49465 RVA: 0x00478F04 File Offset: 0x00477104
		public string GetDayTimeRange(int tempVoteDayTimeRange)
		{
			TwitchVotingManager.VoteDayTimeRange voteDayTimeRange = this.VoteDayTimeRanges[tempVoteDayTimeRange];
			if (voteDayTimeRange.StartHour == 0 && voteDayTimeRange.EndHour == 23)
			{
				return voteDayTimeRange.Name;
			}
			return string.Format(this.dayTimeRangeOutput, voteDayTimeRange.StartHour, voteDayTimeRange.EndHour);
		}

		// Token: 0x0600C13A RID: 49466 RVA: 0x00478F58 File Offset: 0x00477158
		public void QueueVote(string voteType)
		{
			if (this.VoteTypes.ContainsKey(voteType))
			{
				this.QueuedVoteType = this.VoteTypes[voteType];
			}
		}

		// Token: 0x0600C13B RID: 49467 RVA: 0x00478F7A File Offset: 0x0047717A
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

		// Token: 0x040091FF RID: 37375
		public TwitchManager Owner;

		// Token: 0x04009200 RID: 37376
		public TwitchVotingManager.VoteStateTypes CurrentVoteState;

		// Token: 0x04009201 RID: 37377
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_VoteStarted;

		// Token: 0x04009202 RID: 37378
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_VoteFinished;

		// Token: 0x04009203 RID: 37379
		[PublicizedFrom(EAccessModifier.Private)]
		public string dayTimeRangeOutput;

		// Token: 0x04009204 RID: 37380
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionA;

		// Token: 0x04009205 RID: 37381
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionB;

		// Token: 0x04009206 RID: 37382
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionC;

		// Token: 0x04009207 RID: 37383
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionD;

		// Token: 0x04009208 RID: 37384
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionE;

		// Token: 0x04009209 RID: 37385
		[PublicizedFrom(EAccessModifier.Private)]
		public int maxDailyVotes = 4;

		// Token: 0x0400920A RID: 37386
		public int lastGameDay = 1;

		// Token: 0x0400920B RID: 37387
		public bool WinnerShowing;

		// Token: 0x0400920C RID: 37388
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentVoteDayTimeRange = 2;

		// Token: 0x0400920D RID: 37389
		public List<TwitchVotingManager.VoteDayTimeRange> VoteDayTimeRanges = new List<TwitchVotingManager.VoteDayTimeRange>();

		// Token: 0x0400920E RID: 37390
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVotingManager.DailyVoteEntry> DailyVoteTimes = new List<TwitchVotingManager.DailyVoteEntry>();

		// Token: 0x0400920F RID: 37391
		public bool AllowVotesDuringBloodmoon;

		// Token: 0x04009210 RID: 37392
		public bool AllowVotesDuringQuests;

		// Token: 0x04009211 RID: 37393
		public bool AllowVotesInSafeZone;

		// Token: 0x04009212 RID: 37394
		public List<TwitchVoteType> NextVotes = new List<TwitchVoteType>();

		// Token: 0x04009213 RID: 37395
		public bool VoteInProgress;

		// Token: 0x04009214 RID: 37396
		public float VoteTime = 60f;

		// Token: 0x04009215 RID: 37397
		public int ViewerDefeatReward = 250;

		// Token: 0x04009217 RID: 37399
		public float VoteStartDelayTimeRemaining;

		// Token: 0x04009218 RID: 37400
		public float VoteEventTimeRemaining;

		// Token: 0x04009219 RID: 37401
		public float VoteTimeRemaining;

		// Token: 0x0400921A RID: 37402
		public bool UIDirty;

		// Token: 0x0400921B RID: 37403
		public bool VoteEventComplete;

		// Token: 0x0400921C RID: 37404
		public List<TwitchVoteEntry> voteList = new List<TwitchVoteEntry>();

		// Token: 0x0400921D RID: 37405
		public List<string> voterlist = new List<string>();

		// Token: 0x0400921E RID: 37406
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVoteEntry> tempVoteList = new List<TwitchVoteEntry>();

		// Token: 0x0400921F RID: 37407
		public OnGameEventVoteAction VoteStarted;

		// Token: 0x04009220 RID: 37408
		public OnGameEventVoteAction VoteEventStarted;

		// Token: 0x04009221 RID: 37409
		public OnGameEventVoteAction VoteEventEnded;

		// Token: 0x04009222 RID: 37410
		public Dictionary<string, TwitchVoteType> VoteTypes = new Dictionary<string, TwitchVoteType>();

		// Token: 0x04009223 RID: 37411
		[PublicizedFrom(EAccessModifier.Private)]
		public int voteGroupIndex = -1;

		// Token: 0x04009224 RID: 37412
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVoteGroup> VoteGroups = new List<TwitchVoteGroup>();

		// Token: 0x04009225 RID: 37413
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchVoteType QueuedVoteType;

		// Token: 0x04009227 RID: 37415
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVote> tempSortList = new List<TwitchVote>();

		// Token: 0x04009228 RID: 37416
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> tempVoteGroupList = new List<string>();

		// Token: 0x04009229 RID: 37417
		[PublicizedFrom(EAccessModifier.Private)]
		public int day = -1;

		// Token: 0x0400922A RID: 37418
		[PublicizedFrom(EAccessModifier.Private)]
		public int hour = -1;

		// Token: 0x0400922B RID: 37419
		[PublicizedFrom(EAccessModifier.Private)]
		public bool readyForVote;

		// Token: 0x0400922C RID: 37420
		public TwitchVoteEntry CurrentEvent;

		// Token: 0x0200186E RID: 6254
		public enum VoteStateTypes
		{
			// Token: 0x0400922E RID: 37422
			Init,
			// Token: 0x0400922F RID: 37423
			WaitingForNextVote,
			// Token: 0x04009230 RID: 37424
			ReadyForVoteStart,
			// Token: 0x04009231 RID: 37425
			RequestedVoteStart,
			// Token: 0x04009232 RID: 37426
			VoteReady,
			// Token: 0x04009233 RID: 37427
			VoteStarted,
			// Token: 0x04009234 RID: 37428
			VoteFinished,
			// Token: 0x04009235 RID: 37429
			WaitingForActive,
			// Token: 0x04009236 RID: 37430
			EventActive
		}

		// Token: 0x0200186F RID: 6255
		public enum BossVoteSettings
		{
			// Token: 0x04009238 RID: 37432
			Disabled,
			// Token: 0x04009239 RID: 37433
			Standard,
			// Token: 0x0400923A RID: 37434
			Daily
		}

		// Token: 0x02001870 RID: 6256
		public class DailyVoteEntry
		{
			// Token: 0x0400923B RID: 37435
			public ulong VoteStartTime;

			// Token: 0x0400923C RID: 37436
			public ulong VoteEndTime;

			// Token: 0x0400923D RID: 37437
			public int LastVoteDay;

			// Token: 0x0400923E RID: 37438
			public int Index = -1;
		}

		// Token: 0x02001871 RID: 6257
		public class VoteDayTimeRange
		{
			// Token: 0x0400923F RID: 37439
			public string Name;

			// Token: 0x04009240 RID: 37440
			public int StartHour;

			// Token: 0x04009241 RID: 37441
			public int EndHour;
		}
	}
}
