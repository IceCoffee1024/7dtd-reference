using System;
using System.Collections.Generic;
using Audio;
using Challenges;
using UniLinq;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001847 RID: 6215
	public class TwitchVotingManager
	{
		// Token: 0x17001731 RID: 5937
		// (get) Token: 0x0600BF83 RID: 49027 RVA: 0x004717C7 File Offset: 0x0046F9C7
		// (set) Token: 0x0600BF84 RID: 49028 RVA: 0x004717CF File Offset: 0x0046F9CF
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

		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x0600BF85 RID: 49029 RVA: 0x004717E8 File Offset: 0x0046F9E8
		// (set) Token: 0x0600BF86 RID: 49030 RVA: 0x004717F0 File Offset: 0x0046F9F0
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

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x0600BF87 RID: 49031 RVA: 0x00471809 File Offset: 0x0046FA09
		public bool VotingEnabled
		{
			get
			{
				return !this.Owner.CurrentVotePreset.IsEmpty;
			}
		}

		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x0600BF88 RID: 49032 RVA: 0x0047181E File Offset: 0x0046FA1E
		public bool VotingIsActive
		{
			get
			{
				return this.VotingEnabled && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.WaitingForNextVote && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.Init && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.ReadyForVoteStart && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.RequestedVoteStart && this.CurrentVoteState != TwitchVotingManager.VoteStateTypes.VoteReady;
			}
		}

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x0600BF89 RID: 49033 RVA: 0x00471859 File Offset: 0x0046FA59
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

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x0600BF8A RID: 49034 RVA: 0x00471870 File Offset: 0x0046FA70
		public string VoteTypeText
		{
			get
			{
				return this.CurrentVoteType.Title;
			}
		}

		// Token: 0x17001737 RID: 5943
		// (get) Token: 0x0600BF8B RID: 49035 RVA: 0x0047187D File Offset: 0x0046FA7D
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

		// Token: 0x17001738 RID: 5944
		// (get) Token: 0x0600BF8C RID: 49036 RVA: 0x0047189D File Offset: 0x0046FA9D
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

		// Token: 0x17001739 RID: 5945
		// (get) Token: 0x0600BF8D RID: 49037 RVA: 0x004718BD File Offset: 0x0046FABD
		public bool UseMystery
		{
			get
			{
				return this.CurrentVoteType.UseMystery;
			}
		}

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x0600BF8E RID: 49038 RVA: 0x004718CA File Offset: 0x0046FACA
		// (set) Token: 0x0600BF8F RID: 49039 RVA: 0x004718D2 File Offset: 0x0046FAD2
		public int NeededLines
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x0600BF90 RID: 49040 RVA: 0x004718DB File Offset: 0x0046FADB
		// (set) Token: 0x0600BF91 RID: 49041 RVA: 0x004718E3 File Offset: 0x0046FAE3
		public TwitchVoteType CurrentVoteType
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600BF92 RID: 49042 RVA: 0x004718EC File Offset: 0x0046FAEC
		public TwitchVotingManager(TwitchManager owner)
		{
			this.Owner = owner;
			this.SetupVoteDayTimeRanges();
		}

		// Token: 0x0600BF93 RID: 49043 RVA: 0x004719BA File Offset: 0x0046FBBA
		public void CleanupData()
		{
			this.VoteTypes.Clear();
			this.VoteGroups.Clear();
		}

		// Token: 0x0600BF94 RID: 49044 RVA: 0x004719D4 File Offset: 0x0046FBD4
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

		// Token: 0x0600BF95 RID: 49045 RVA: 0x00471A74 File Offset: 0x0046FC74
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

		// Token: 0x0600BF96 RID: 49046 RVA: 0x00471B03 File Offset: 0x0046FD03
		public TwitchVoteType GetVoteType(string voteTypeName)
		{
			if (this.VoteTypes.ContainsKey(voteTypeName))
			{
				return this.VoteTypes[voteTypeName];
			}
			return null;
		}

		// Token: 0x0600BF97 RID: 49047 RVA: 0x00471B24 File Offset: 0x0046FD24
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

		// Token: 0x0600BF98 RID: 49048 RVA: 0x00471BB4 File Offset: 0x0046FDB4
		public void ClearVotes()
		{
			for (int i = 0; i < this.voteList.Count; i++)
			{
				this.voteList[i].VoteCount = 0;
				this.voteList[i].VoterNames.Clear();
			}
			this.voterlist.Clear();
		}

		// Token: 0x0600BF99 RID: 49049 RVA: 0x00471C0C File Offset: 0x0046FE0C
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

		// Token: 0x0600BF9A RID: 49050 RVA: 0x00471D04 File Offset: 0x0046FF04
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

		// Token: 0x0600BF9B RID: 49051 RVA: 0x00471DA0 File Offset: 0x0046FFA0
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

		// Token: 0x0600BF9C RID: 49052 RVA: 0x004721E4 File Offset: 0x004703E4
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

		// Token: 0x0600BF9D RID: 49053 RVA: 0x004722B8 File Offset: 0x004704B8
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

		// Token: 0x0600BF9E RID: 49054 RVA: 0x0047230C File Offset: 0x0047050C
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResetVoteGroupsForVote()
		{
			for (int i = 0; i < this.VoteGroups.Count; i++)
			{
				this.VoteGroups[i].SkippedThisVote = false;
			}
		}

		// Token: 0x0600BF9F RID: 49055 RVA: 0x00472344 File Offset: 0x00470544
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

		// Token: 0x0600BFA0 RID: 49056 RVA: 0x00472380 File Offset: 0x00470580
		[PublicizedFrom(EAccessModifier.Private)]
		public bool AllowVoting()
		{
			return this.Owner.CooldownType == TwitchManager.CooldownTypes.None || ((this.Owner.CooldownType == TwitchManager.CooldownTypes.QuestCooldown || this.Owner.CooldownType == TwitchManager.CooldownTypes.QuestDisabled) && this.AllowVotesDuringQuests) || ((this.Owner.CooldownType == TwitchManager.CooldownTypes.BloodMoonCooldown || this.Owner.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled) && this.AllowVotesDuringBloodmoon);
		}

		// Token: 0x0600BFA1 RID: 49057 RVA: 0x004723E4 File Offset: 0x004705E4
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

		// Token: 0x0600BFA2 RID: 49058 RVA: 0x0047297E File Offset: 0x00470B7E
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckVoteLock()
		{
			return (!this.AllowVotesDuringQuests && QuestEventManager.Current.QuestBounds.width != 0f) || (!this.AllowVotesInSafeZone && this.Owner.IsSafe);
		}

		// Token: 0x0600BFA3 RID: 49059 RVA: 0x004729B8 File Offset: 0x00470BB8
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

		// Token: 0x0600BFA4 RID: 49060 RVA: 0x00472A00 File Offset: 0x00470C00
		public bool SetReadyForVote(int index)
		{
			return this.GetNextVoteType();
		}

		// Token: 0x0600BFA5 RID: 49061 RVA: 0x00472A08 File Offset: 0x00470C08
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResetCurrentVote()
		{
			this.VoteTimeRemaining = this.VoteTime;
		}

		// Token: 0x0600BFA6 RID: 49062 RVA: 0x00472A18 File Offset: 0x00470C18
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

		// Token: 0x0600BFA7 RID: 49063 RVA: 0x00472AF4 File Offset: 0x00470CF4
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

		// Token: 0x0600BFA8 RID: 49064 RVA: 0x00472B80 File Offset: 0x00470D80
		[PublicizedFrom(EAccessModifier.Private)]
		public void ShuffleVoteGroupVoteTypes()
		{
			for (int i = 0; i < this.VoteGroups.Count; i++)
			{
				this.VoteGroups[i].ShuffleVoteTypes();
			}
		}

		// Token: 0x0600BFA9 RID: 49065 RVA: 0x00472BB4 File Offset: 0x00470DB4
		public void CancelVote()
		{
			if (this.CurrentVoteState == TwitchVotingManager.VoteStateTypes.WaitingForNextVote && this.readyForVote)
			{
				this.readyForVote = false;
			}
		}

		// Token: 0x0600BFAA RID: 49066 RVA: 0x00472BCE File Offset: 0x00470DCE
		public void RequestApprovedToStart()
		{
			this.CurrentVoteState = TwitchVotingManager.VoteStateTypes.VoteReady;
		}

		// Token: 0x0600BFAB RID: 49067 RVA: 0x00472BD8 File Offset: 0x00470DD8
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

		// Token: 0x0600BFAC RID: 49068 RVA: 0x00472C94 File Offset: 0x00470E94
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

		// Token: 0x0600BFAD RID: 49069 RVA: 0x00472CE0 File Offset: 0x00470EE0
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

		// Token: 0x0600BFAE RID: 49070 RVA: 0x00472D10 File Offset: 0x00470F10
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEventCompleted(string gameEventID, int targetEntityID, string extraData, string tag)
		{
			if (this.CurrentEvent != null && this.CurrentEvent.VoteClass.GameEvent == gameEventID && tag == "vote")
			{
				this.VoteEventComplete = true;
			}
		}

		// Token: 0x0600BFAF RID: 49071 RVA: 0x00472D48 File Offset: 0x00470F48
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

		// Token: 0x0600BFB0 RID: 49072 RVA: 0x00472E18 File Offset: 0x00471018
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

		// Token: 0x0600BFB1 RID: 49073 RVA: 0x00472FCC File Offset: 0x004711CC
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

		// Token: 0x0600BFB2 RID: 49074 RVA: 0x00473054 File Offset: 0x00471254
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

		// Token: 0x0600BFB3 RID: 49075 RVA: 0x00473114 File Offset: 0x00471314
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

		// Token: 0x0600BFB4 RID: 49076 RVA: 0x00473168 File Offset: 0x00471368
		public string GetDayTimeRange(int tempVoteDayTimeRange)
		{
			TwitchVotingManager.VoteDayTimeRange voteDayTimeRange = this.VoteDayTimeRanges[tempVoteDayTimeRange];
			if (voteDayTimeRange.StartHour == 0 && voteDayTimeRange.EndHour == 23)
			{
				return voteDayTimeRange.Name;
			}
			return string.Format(this.dayTimeRangeOutput, voteDayTimeRange.StartHour, voteDayTimeRange.EndHour);
		}

		// Token: 0x0600BFB5 RID: 49077 RVA: 0x004731BC File Offset: 0x004713BC
		public void QueueVote(string voteType)
		{
			if (this.VoteTypes.ContainsKey(voteType))
			{
				this.QueuedVoteType = this.VoteTypes[voteType];
			}
		}

		// Token: 0x0600BFB6 RID: 49078 RVA: 0x004731DE File Offset: 0x004713DE
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

		// Token: 0x040090E3 RID: 37091
		public TwitchManager Owner;

		// Token: 0x040090E4 RID: 37092
		public TwitchVotingManager.VoteStateTypes CurrentVoteState;

		// Token: 0x040090E5 RID: 37093
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_VoteStarted;

		// Token: 0x040090E6 RID: 37094
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_VoteFinished;

		// Token: 0x040090E7 RID: 37095
		[PublicizedFrom(EAccessModifier.Private)]
		public string dayTimeRangeOutput;

		// Token: 0x040090E8 RID: 37096
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionA;

		// Token: 0x040090E9 RID: 37097
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionB;

		// Token: 0x040090EA RID: 37098
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionC;

		// Token: 0x040090EB RID: 37099
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionD;

		// Token: 0x040090EC RID: 37100
		[PublicizedFrom(EAccessModifier.Private)]
		public string VoteOptionE;

		// Token: 0x040090ED RID: 37101
		[PublicizedFrom(EAccessModifier.Private)]
		public int maxDailyVotes = 4;

		// Token: 0x040090EE RID: 37102
		public int lastGameDay = 1;

		// Token: 0x040090EF RID: 37103
		public bool WinnerShowing;

		// Token: 0x040090F0 RID: 37104
		[PublicizedFrom(EAccessModifier.Private)]
		public int currentVoteDayTimeRange = 2;

		// Token: 0x040090F1 RID: 37105
		public List<TwitchVotingManager.VoteDayTimeRange> VoteDayTimeRanges = new List<TwitchVotingManager.VoteDayTimeRange>();

		// Token: 0x040090F2 RID: 37106
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVotingManager.DailyVoteEntry> DailyVoteTimes = new List<TwitchVotingManager.DailyVoteEntry>();

		// Token: 0x040090F3 RID: 37107
		public bool AllowVotesDuringBloodmoon;

		// Token: 0x040090F4 RID: 37108
		public bool AllowVotesDuringQuests;

		// Token: 0x040090F5 RID: 37109
		public bool AllowVotesInSafeZone;

		// Token: 0x040090F6 RID: 37110
		public List<TwitchVoteType> NextVotes = new List<TwitchVoteType>();

		// Token: 0x040090F7 RID: 37111
		public bool VoteInProgress;

		// Token: 0x040090F8 RID: 37112
		public float VoteTime = 60f;

		// Token: 0x040090F9 RID: 37113
		public int ViewerDefeatReward = 250;

		// Token: 0x040090FB RID: 37115
		public float VoteStartDelayTimeRemaining;

		// Token: 0x040090FC RID: 37116
		public float VoteEventTimeRemaining;

		// Token: 0x040090FD RID: 37117
		public float VoteTimeRemaining;

		// Token: 0x040090FE RID: 37118
		public bool UIDirty;

		// Token: 0x040090FF RID: 37119
		public bool VoteEventComplete;

		// Token: 0x04009100 RID: 37120
		public List<TwitchVoteEntry> voteList = new List<TwitchVoteEntry>();

		// Token: 0x04009101 RID: 37121
		public List<string> voterlist = new List<string>();

		// Token: 0x04009102 RID: 37122
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVoteEntry> tempVoteList = new List<TwitchVoteEntry>();

		// Token: 0x04009103 RID: 37123
		public OnGameEventVoteAction VoteStarted;

		// Token: 0x04009104 RID: 37124
		public OnGameEventVoteAction VoteEventStarted;

		// Token: 0x04009105 RID: 37125
		public OnGameEventVoteAction VoteEventEnded;

		// Token: 0x04009106 RID: 37126
		public Dictionary<string, TwitchVoteType> VoteTypes = new Dictionary<string, TwitchVoteType>();

		// Token: 0x04009107 RID: 37127
		[PublicizedFrom(EAccessModifier.Private)]
		public int voteGroupIndex = -1;

		// Token: 0x04009108 RID: 37128
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVoteGroup> VoteGroups = new List<TwitchVoteGroup>();

		// Token: 0x04009109 RID: 37129
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchVoteType QueuedVoteType;

		// Token: 0x0400910B RID: 37131
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchVote> tempSortList = new List<TwitchVote>();

		// Token: 0x0400910C RID: 37132
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> tempVoteGroupList = new List<string>();

		// Token: 0x0400910D RID: 37133
		[PublicizedFrom(EAccessModifier.Private)]
		public int day = -1;

		// Token: 0x0400910E RID: 37134
		[PublicizedFrom(EAccessModifier.Private)]
		public int hour = -1;

		// Token: 0x0400910F RID: 37135
		[PublicizedFrom(EAccessModifier.Private)]
		public bool readyForVote;

		// Token: 0x04009110 RID: 37136
		public TwitchVoteEntry CurrentEvent;

		// Token: 0x02001848 RID: 6216
		public enum VoteStateTypes
		{
			// Token: 0x04009112 RID: 37138
			Init,
			// Token: 0x04009113 RID: 37139
			WaitingForNextVote,
			// Token: 0x04009114 RID: 37140
			ReadyForVoteStart,
			// Token: 0x04009115 RID: 37141
			RequestedVoteStart,
			// Token: 0x04009116 RID: 37142
			VoteReady,
			// Token: 0x04009117 RID: 37143
			VoteStarted,
			// Token: 0x04009118 RID: 37144
			VoteFinished,
			// Token: 0x04009119 RID: 37145
			WaitingForActive,
			// Token: 0x0400911A RID: 37146
			EventActive
		}

		// Token: 0x02001849 RID: 6217
		public enum BossVoteSettings
		{
			// Token: 0x0400911C RID: 37148
			Disabled,
			// Token: 0x0400911D RID: 37149
			Standard,
			// Token: 0x0400911E RID: 37150
			Daily
		}

		// Token: 0x0200184A RID: 6218
		public class DailyVoteEntry
		{
			// Token: 0x0400911F RID: 37151
			public ulong VoteStartTime;

			// Token: 0x04009120 RID: 37152
			public ulong VoteEndTime;

			// Token: 0x04009121 RID: 37153
			public int LastVoteDay;

			// Token: 0x04009122 RID: 37154
			public int Index = -1;
		}

		// Token: 0x0200184B RID: 6219
		public class VoteDayTimeRange
		{
			// Token: 0x04009123 RID: 37155
			public string Name;

			// Token: 0x04009124 RID: 37156
			public int StartHour;

			// Token: 0x04009125 RID: 37157
			public int EndHour;
		}
	}
}
