using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Audio;
using Challenges;
using Newtonsoft.Json.Linq;
using Platform;
using Twitch.PubSub;
using UniLinq;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001874 RID: 6260
	public class TwitchManager
	{
		// Token: 0x17001822 RID: 6178
		// (get) Token: 0x0600C0F3 RID: 49395 RVA: 0x0046F2B7 File Offset: 0x0046D4B7
		public static TwitchManager Current
		{
			get
			{
				if (TwitchManager.instance == null)
				{
					TwitchManager.instance = new TwitchManager();
				}
				return TwitchManager.instance;
			}
		}

		// Token: 0x17001823 RID: 6179
		// (get) Token: 0x0600C0F4 RID: 49396 RVA: 0x0046F2CF File Offset: 0x0046D4CF
		public static bool HasInstance
		{
			get
			{
				return TwitchManager.instance != null;
			}
		}

		// Token: 0x0600C0F5 RID: 49397 RVA: 0x0046F2DC File Offset: 0x0046D4DC
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchManager()
		{
			this.ViewerData = new TwitchViewerData(this);
			this.VotingManager = new TwitchVotingManager(this);
			this.HighestGameStage = -1;
			this.UseProgression = true;
		}

		// Token: 0x0600C0F6 RID: 49398 RVA: 0x0046F5B5 File Offset: 0x0046D7B5
		public void Cleanup()
		{
			this.Disconnect();
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.InitState == TwitchManager.InitStates.Ready)
			{
				this.SaveViewerData();
			}
			TwitchManager.instance = null;
		}

		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x0600C0F7 RID: 49399 RVA: 0x0046F5DE File Offset: 0x0046D7DE
		// (set) Token: 0x0600C0F8 RID: 49400 RVA: 0x0046F5E6 File Offset: 0x0046D7E6
		public byte CurrentFileVersion { get; set; }

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x0600C0F9 RID: 49401 RVA: 0x0046F5EF File Offset: 0x0046D7EF
		// (set) Token: 0x0600C0FA RID: 49402 RVA: 0x0046F5F7 File Offset: 0x0046D7F7
		public byte CurrentMainFileVersion { get; set; }

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x0600C0FB RID: 49403 RVA: 0x0046F600 File Offset: 0x0046D800
		// (set) Token: 0x0600C0FC RID: 49404 RVA: 0x0046F608 File Offset: 0x0046D808
		public bool OverrideProgession
		{
			get
			{
				return this.overrideProgression;
			}
			set
			{
				if (this.overrideProgression != value)
				{
					this.overrideProgression = value;
					if (this.InitState == TwitchManager.InitStates.Ready)
					{
						this.resetCommandsNeeded = true;
					}
				}
			}
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x0600C0FD RID: 49405 RVA: 0x0046F62A File Offset: 0x0046D82A
		// (set) Token: 0x0600C0FE RID: 49406 RVA: 0x0046F632 File Offset: 0x0046D832
		public bool UseProgression
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x0600C0FF RID: 49407 RVA: 0x0046F63B File Offset: 0x0046D83B
		// (set) Token: 0x0600C100 RID: 49408 RVA: 0x0046F643 File Offset: 0x0046D843
		public int HighestGameStage
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x0600C101 RID: 49409 RVA: 0x0046F64C File Offset: 0x0046D84C
		public static bool BossHordeActive
		{
			get
			{
				return TwitchManager.HasInstance && TwitchManager.Current.IsBossHordeActive;
			}
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x0600C102 RID: 49410 RVA: 0x0046F661 File Offset: 0x0046D861
		// (set) Token: 0x0600C103 RID: 49411 RVA: 0x0046F669 File Offset: 0x0046D869
		public TwitchManager.IntegrationSettings IntegrationSetting
		{
			get
			{
				return this.integrationSetting;
			}
			set
			{
				if (this.integrationSetting != value)
				{
					this.integrationSetting = value;
					this.IntegrationTypeChanged();
				}
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x0600C104 RID: 49412 RVA: 0x0046F681 File Offset: 0x0046D881
		// (set) Token: 0x0600C105 RID: 49413 RVA: 0x0046F689 File Offset: 0x0046D889
		public float ActionCooldownModifier
		{
			get
			{
				return this.actionCooldownModifier;
			}
			set
			{
				if (value != this.actionCooldownModifier)
				{
					this.actionCooldownModifier = value;
					this.UpdateActionCooldowns(value);
				}
			}
		}

		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x0600C106 RID: 49414 RVA: 0x0046F6A2 File Offset: 0x0046D8A2
		public bool AllowActions
		{
			get
			{
				return !this.CurrentActionPreset.IsEmpty;
			}
		}

		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x0600C107 RID: 49415 RVA: 0x0046F6B2 File Offset: 0x0046D8B2
		public bool AllowEvents
		{
			get
			{
				return !this.CurrentEventPreset.IsEmpty;
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x0600C108 RID: 49416 RVA: 0x0046F6C2 File Offset: 0x0046D8C2
		public bool OnCooldown
		{
			get
			{
				return this.CooldownTime > 0f || this.CurrentCooldownPreset.CooldownType == CooldownPreset.CooldownTypes.Always;
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x0600C109 RID: 49417 RVA: 0x0046F6E1 File Offset: 0x0046D8E1
		// (set) Token: 0x0600C10A RID: 49418 RVA: 0x0046F6E9 File Offset: 0x0046D8E9
		public float BitPriceMultiplier
		{
			get
			{
				return this.bitPriceMultiplier;
			}
			set
			{
				if (this.bitPriceMultiplier != value)
				{
					this.bitPriceMultiplier = value;
					this.ResetPrices();
				}
			}
		}

		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x0600C10B RID: 49419 RVA: 0x0046F701 File Offset: 0x0046D901
		// (set) Token: 0x0600C10C RID: 49420 RVA: 0x0046F709 File Offset: 0x0046D909
		public TwitchVotingManager VotingManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x0600C10D RID: 49421 RVA: 0x0046F712 File Offset: 0x0046D912
		// (set) Token: 0x0600C10E RID: 49422 RVA: 0x0046F71A File Offset: 0x0046D91A
		public TwitchViewerData ViewerData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x0600C10F RID: 49423 RVA: 0x0046F723 File Offset: 0x0046D923
		// (set) Token: 0x0600C110 RID: 49424 RVA: 0x0046F72B File Offset: 0x0046D92B
		public string BroadcasterType
		{
			get
			{
				return this.broadcasterType;
			}
			set
			{
				this.broadcasterType = value;
				this.UIDirty = true;
				if (this.CommandsChanged != null)
				{
					this.CommandsChanged();
				}
			}
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x0600C111 RID: 49425 RVA: 0x0046F74E File Offset: 0x0046D94E
		// (set) Token: 0x0600C112 RID: 49426 RVA: 0x0046F758 File Offset: 0x0046D958
		public TwitchManager.InitStates InitState
		{
			get
			{
				return this.initState;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				if (this.initState != value)
				{
					TwitchManager.InitStates initStates = this.initState;
					this.initState = value;
					if (this.ConnectionStateChanged != null)
					{
						this.ConnectionStateChanged(initStates, this.initState);
					}
				}
			}
		}

		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x0600C113 RID: 49427 RVA: 0x0046F798 File Offset: 0x0046D998
		public string StateText
		{
			get
			{
				switch (this.initState)
				{
				case TwitchManager.InitStates.Setup:
				case TwitchManager.InitStates.WaitingForOAuth:
				case TwitchManager.InitStates.Authenticating:
				case TwitchManager.InitStates.Authenticated:
					return Localization.Get("xuiTwitchStatus_Connecting", false, null);
				case TwitchManager.InitStates.WaitingForPermission:
					return Localization.Get("xuiTwitchStatus_RequestPermission", false, null);
				case TwitchManager.InitStates.PermissionDenied:
					return Localization.Get("xuiTwitchStatus_PermissionDenied", false, null);
				case TwitchManager.InitStates.Ready:
					return string.Format(Localization.Get("xuiTwitchStatus_Connected", false, null), this.Authentication.userName);
				case TwitchManager.InitStates.ExtensionNotInstalled:
					return Localization.Get("xuiTwitchStatus_ExtensionDenied", false, null);
				case TwitchManager.InitStates.Failed:
					return Localization.Get("xuiTwitchStatus_ConnectionFailed", false, null);
				}
				return "";
			}
		}

		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x0600C114 RID: 49428 RVA: 0x0046F843 File Offset: 0x0046DA43
		public bool IsReady
		{
			get
			{
				return this.initState == TwitchManager.InitStates.Ready;
			}
		}

		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x0600C115 RID: 49429 RVA: 0x0046F84E File Offset: 0x0046DA4E
		public bool IsVoting
		{
			get
			{
				return this.initState == TwitchManager.InitStates.Ready && this.VotingManager.VotingIsActive;
			}
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x0600C116 RID: 49430 RVA: 0x0046F866 File Offset: 0x0046DA66
		public bool IsBossHordeActive
		{
			get
			{
				return this.initState == TwitchManager.InitStates.Ready && (this.VotingManager.CurrentVoteState == TwitchVotingManager.VoteStateTypes.EventActive || this.VotingManager.CurrentVoteState == TwitchVotingManager.VoteStateTypes.WaitingForActive);
			}
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x0600C117 RID: 49431 RVA: 0x0046F891 File Offset: 0x0046DA91
		public bool ReadyForVote
		{
			get
			{
				return this.actionSpawnLiveList.Count == 0;
			}
		}

		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x0600C118 RID: 49432 RVA: 0x0046F8A1 File Offset: 0x0046DAA1
		// (set) Token: 0x0600C119 RID: 49433 RVA: 0x0046F8A9 File Offset: 0x0046DAA9
		public bool IsSafe
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x0600C11A RID: 49434 RVA: 0x0046F8B4 File Offset: 0x0046DAB4
		// (remove) Token: 0x0600C11B RID: 49435 RVA: 0x0046F8EC File Offset: 0x0046DAEC
		public event OnTwitchConnectionStateChange ConnectionStateChanged;

		// Token: 0x1400010E RID: 270
		// (add) Token: 0x0600C11C RID: 49436 RVA: 0x0046F924 File Offset: 0x0046DB24
		// (remove) Token: 0x0600C11D RID: 49437 RVA: 0x0046F95C File Offset: 0x0046DB5C
		public event OnCommandsChanged CommandsChanged;

		// Token: 0x1400010F RID: 271
		// (add) Token: 0x0600C11E RID: 49438 RVA: 0x0046F994 File Offset: 0x0046DB94
		// (remove) Token: 0x0600C11F RID: 49439 RVA: 0x0046F9CC File Offset: 0x0046DBCC
		public event OnHistoryAdded ActionHistoryAdded;

		// Token: 0x14000110 RID: 272
		// (add) Token: 0x0600C120 RID: 49440 RVA: 0x0046FA04 File Offset: 0x0046DC04
		// (remove) Token: 0x0600C121 RID: 49441 RVA: 0x0046FA3C File Offset: 0x0046DC3C
		public event OnHistoryAdded VoteHistoryAdded;

		// Token: 0x14000111 RID: 273
		// (add) Token: 0x0600C122 RID: 49442 RVA: 0x0046FA74 File Offset: 0x0046DC74
		// (remove) Token: 0x0600C123 RID: 49443 RVA: 0x0046FAAC File Offset: 0x0046DCAC
		public event OnHistoryAdded EventHistoryAdded;

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x0600C124 RID: 49444 RVA: 0x0046FAE1 File Offset: 0x0046DCE1
		public bool HasCustomEvents
		{
			get
			{
				return this.EventPresets.Count > 0;
			}
		}

		// Token: 0x0600C125 RID: 49445 RVA: 0x0046FAF4 File Offset: 0x0046DCF4
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetupLocalization()
		{
			this.chatOutput_ActivatedAction = Localization.Get("TwitchChat_ActivatedAction", false, null);
			this.chatOutput_ActivatedBitAction = Localization.Get("TwitchChat_ActivatedBitAction", false, null);
			this.chatOutput_BitCredits = Localization.Get("TwitchChat_BitCredits", false, null);
			this.chatOutput_BitEvent = Localization.Get("TwitchChat_BitEvent", false, null);
			this.chatOutput_BitPotBalance = Localization.Get("TwitchChat_BitPotBalance", false, null);
			this.chatOutput_ChannelPointEvent = Localization.Get("TwitchChat_ChannelPointEvent", false, null);
			this.chatOutput_CharityEvent = Localization.Get("TwitchChat_CharityEvent", false, null);
			this.chatOutput_Commands = Localization.Get("TwitchChat_Commands", false, null);
			this.chatOutput_CooldownComplete = Localization.Get("TwitchChat_CooldownComplete", false, null);
			this.chatOutput_CooldownStarted = Localization.Get("TwitchChat_CooldownStarted", false, null);
			this.chatOutput_CooldownTime = Localization.Get("TwitchChat_CooldownTime", false, null);
			this.chatOutput_CreatorGoalEvent = Localization.Get("TwitchChat_CreatorGoalEvent", false, null);
			this.chatOutput_DonateBits = Localization.Get("TwitchChat_DonateBits", false, null);
			this.chatOutput_DonateCharity = Localization.Get("TwitchChat_DonateCharity", false, null);
			this.chatOutput_Gamestage = Localization.Get("TwitchChat_Gamestage", false, null);
			this.chatOutput_GiftSubEvent = Localization.Get("TwitchChat_GiftedSubEvent", false, null);
			this.chatOutput_GiftSubs = Localization.Get("TwitchChat_GiftedSubs", false, null);
			this.chatOutput_HypeTrainEvent = Localization.Get("TwitchChat_HypeTrainEvent", false, null);
			this.chatOutput_KilledParty = Localization.Get("TwitchChat_KilledParty", false, null);
			this.chatOutput_KilledStreamer = Localization.Get("TwitchChat_KilledStreamer", false, null);
			this.chatOutput_KilledByBits = Localization.Get("TwitchChat_KilledByBits", false, null);
			this.chatOutput_KilledByHypeTrain = Localization.Get("TwitchChat_KilledByHypeTrain", false, null);
			this.chatOutput_KilledByVote = Localization.Get("TwitchChat_KilledByVote", false, null);
			this.chatOutput_NewActions = Localization.Get("TwitchChat_NewActions", false, null);
			this.chatOutput_PimpPotBalance = Localization.Get("TwitchChat_PimpPotBalance", false, null);
			this.chatOutput_PointsWithSpecial = Localization.Get("TwitchChat_PointsWithSpecial", false, null);
			this.chatOutput_PointsWithoutSpecial = Localization.Get("TwitchChat_PointsWithoutSpecial", false, null);
			this.chatOutput_QueuedBitAction = Localization.Get("TwitchChat_QueuedBitAction", false, null);
			this.chatOutput_RaidEvent = Localization.Get("TwitchChat_RaidEvent", false, null);
			this.chatOutput_RaidPoints = Localization.Get("TwitchChat_RaidPoints", false, null);
			this.chatOutput_SubEvent = Localization.Get("TwitchChat_SubEvent", false, null);
			this.chatOutput_Subscribed = Localization.Get("TwitchChat_Subscribed", false, null);
			this.ingameOutput_ActivatedAction = Localization.Get("TwitchInGame_ActivatedAction", false, null);
			this.ingameOutput_BitRespawns = Localization.Get("TwitchInGame_BitRespawns", false, null);
			this.ingameOutput_DonateBits = Localization.Get("TwitchInGame_DonateBits", false, null);
			this.ingameOutput_DonateCharity = Localization.Get("TwitchInGame_DonateCharity", false, null);
			this.ingameOutput_GiftSubs = Localization.Get("TwitchInGame_GiftedSubs", false, null);
			this.ingameOutput_KilledParty = Localization.Get("TwitchInGame_KilledParty", false, null);
			this.ingameOutput_KilledStreamer = Localization.Get("TwitchInGame_KilledStreamer", false, null);
			this.ingameOutput_KilledByBits = Localization.Get("TwitchInGame_KilledByBits", false, null);
			this.ingameOutput_KilledByHypeTrain = Localization.Get("TwitchInGame_KilledByHypeTrain", false, null);
			this.ingameOutput_KilledByVote = Localization.Get("TwitchInGame_KilledByVote", false, null);
			this.ingameOutput_RaidPoints = Localization.Get("TwitchInGame_RaidPoints", false, null);
			this.ingameOutput_RefundedAction = Localization.Get("TwitchInGame_RefundedAction", false, null);
			this.ingameOutput_Subscribed = Localization.Get("TwitchInGame_Subscribed", false, null);
			this.ingameDeathScreen_Message = Localization.Get("TwitchDeathMessage", false, null);
			this.ingameBitsDeathScreen_Message = Localization.Get("TwitchBitsDeathMessage", false, null);
			this.ingameHypeTrainDeathScreen_Message = Localization.Get("TwitchHypeTrainDeathMessage", false, null);
			this.ingameVoteDeathScreen_Message = Localization.Get("TwitchVoteDeathMessage", false, null);
			this.subPointDisplay = Localization.Get("xuiOptionsTwitchSubPointDisplay", false, null);
			this.ViewerData.SetupLocalization();
			this.VotingManager.SetupLocalization();
			TwitchManager.LeaderboardStats.SetupLocalization();
		}

		// Token: 0x0600C126 RID: 49446 RVA: 0x0046FEA8 File Offset: 0x0046E0A8
		public void SetupClient(string twitchChannel, string password)
		{
			this.ircClient = new TwitchIRCClient("irc.twitch.tv", 6667, twitchChannel, password);
			if (this.EventSub == null)
			{
				this.EventSub = new EventSubClient(this.Authentication.userID, this.Authentication.oauth.Substring(6), TwitchAuthentication.client_id);
			}
		}

		// Token: 0x0600C127 RID: 49447 RVA: 0x0046FF00 File Offset: 0x0046E100
		public void IntegrationTypeChanged()
		{
			if (this.IsReady && this.extensionManager == null)
			{
				this.extensionManager = new ExtensionManager();
				this.extensionManager.Init();
			}
		}

		// Token: 0x0600C128 RID: 49448 RVA: 0x0046FF28 File Offset: 0x0046E128
		public void CleanupData()
		{
			BaseTwitchCommand.ClearCommandPermissionOverrides();
			this.VotingManager.CleanupData();
			this.CooldownPresets.Clear();
			this.tipTitleList.Clear();
			this.tipDescriptionList.Clear();
			this.ActionPresets.Clear();
			this.VotePresets.Clear();
			this.CurrentActionPreset = null;
			this.CurrentVotePreset = null;
			this.CleanupEventData();
		}

		// Token: 0x0600C129 RID: 49449 RVA: 0x0046FF90 File Offset: 0x0046E190
		[PublicizedFrom(EAccessModifier.Private)]
		public void RemoveChannelPointRedeems()
		{
			if (this.CurrentEventPreset != null)
			{
				this.CurrentEventPreset.RemoveChannelPointRedemptions(null);
			}
		}

		// Token: 0x0600C12A RID: 49450 RVA: 0x0046FFA6 File Offset: 0x0046E1A6
		public void CleanupEventData()
		{
			this.RemoveChannelPointRedeems();
			this.CurrentEventPreset = null;
			this.EventPresets.Clear();
		}

		// Token: 0x0600C12B RID: 49451 RVA: 0x0046FFC0 File Offset: 0x0046E1C0
		[PublicizedFrom(EAccessModifier.Private)]
		public void Disconnect()
		{
			this.RemoveChannelPointRedeems();
			if (this.EventSub != null)
			{
				this.EventSub.Disconnect();
				this.EventSub.Cleanup();
				this.EventSub = null;
			}
			if (this.ircClient != null)
			{
				this.ircClient.Disconnect();
				this.ircClient = null;
			}
			if (this.extensionManager != null)
			{
				this.extensionManager.Cleanup();
				this.extensionManager = null;
			}
			this.Authentication = null;
			if (this.LocalPlayer != null && this.LocalPlayer.PlayerUI != null && this.LocalPlayer.PlayerUI.windowManager.IsWindowOpen("twitch"))
			{
				this.LocalPlayer.PlayerUI.windowManager.Close("twitch");
			}
		}

		// Token: 0x0600C12C RID: 49452 RVA: 0x0047008A File Offset: 0x0046E28A
		public void AddRandomGroup(string name, int randomCount)
		{
			if (!this.randomGroups.ContainsKey(name))
			{
				this.randomGroups.Add(name, new TwitchRandomActionGroup
				{
					Name = name,
					RandomCount = randomCount
				});
			}
		}

		// Token: 0x0600C12D RID: 49453 RVA: 0x004700BC File Offset: 0x0046E2BC
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResetDailyCommands(int currentDay, int lastGameStage = -1)
		{
			this.randomKeys.Clear();
			foreach (string text in this.AvailableCommands.Keys)
			{
				TwitchAction twitchAction = this.AvailableCommands[text];
				if (twitchAction.IsInPreset(this.CurrentActionPreset))
				{
					if (twitchAction.RandomDaily)
					{
						if (!this.randomKeys.ContainsKey(twitchAction.RandomGroup))
						{
							this.randomKeys.Add(twitchAction.RandomGroup, new List<TwitchAction>());
						}
						this.randomKeys[twitchAction.RandomGroup].Add(twitchAction);
					}
					else if (twitchAction.SingleDayUse)
					{
						twitchAction.AllowedDay = currentDay;
					}
				}
			}
			foreach (string text2 in this.randomKeys.Keys)
			{
				int num = 1;
				if (this.randomGroups.ContainsKey(text2))
				{
					num = this.randomGroups[text2].RandomCount;
				}
				List<TwitchAction> list = this.randomKeys[text2];
				if (lastGameStage != -1)
				{
					bool flag = false;
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].StartGameStage > lastGameStage)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					int num2 = global::UnityEngine.Random.Range(0, list.Count);
					int num3 = global::UnityEngine.Random.Range(0, list.Count);
					TwitchAction twitchAction2 = list[num2];
					list[num2] = list[num3];
					list[num3] = twitchAction2;
				}
				for (int k = 0; k < list.Count; k++)
				{
					list[k].AllowedDay = ((k < num) ? currentDay : (-1));
				}
			}
			if (this.CommandsChanged != null)
			{
				this.CommandsChanged();
			}
		}

		// Token: 0x0600C12E RID: 49454 RVA: 0x004702FC File Offset: 0x0046E4FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetupTwitchCommands()
		{
			this.TwitchCommandList.Clear();
			this.TwitchCommandList.Add(new TwitchCommandAddBitCredit());
			this.TwitchCommandList.Add(new TwitchCommandAddPoints());
			this.TwitchCommandList.Add(new TwitchCommandAddSpecialPoints());
			this.TwitchCommandList.Add(new TwitchCommandCheckCredit());
			this.TwitchCommandList.Add(new TwitchCommandCheckPoints());
			this.TwitchCommandList.Add(new TwitchCommandCommands());
			this.TwitchCommandList.Add(new TwitchCommandDebug());
			this.TwitchCommandList.Add(new TwitchCommandDisableCommand());
			this.TwitchCommandList.Add(new TwitchCommandGamestage());
			this.TwitchCommandList.Add(new TwitchCommandPauseCommand());
			this.TwitchCommandList.Add(new TwitchCommandUnpauseCommand());
			this.TwitchCommandList.Add(new TwitchCommandRemoveViewer());
			this.TwitchCommandList.Add(new TwitchCommandResetCooldowns());
			this.TwitchCommandList.Add(new TwitchCommandSetBitPot());
			this.TwitchCommandList.Add(new TwitchCommandSetCooldown());
			this.TwitchCommandList.Add(new TwitchCommandSetPot());
			this.TwitchCommandList.Add(new TwitchCommandTeleportBackpack());
			if (this.CurrentEventPreset.HasBitEvents && this.AllowBitEvents)
			{
				this.TwitchCommandList.Add(new TwitchCommandRedeemBits());
			}
			if (this.CurrentEventPreset.HasSubEvents && this.AllowSubEvents)
			{
				this.TwitchCommandList.Add(new TwitchCommandRedeemSub());
			}
			if (this.CurrentEventPreset.HasGiftSubEvents && this.AllowGiftSubEvents)
			{
				this.TwitchCommandList.Add(new TwitchCommandRedeemGiftSub());
			}
			if (this.CurrentEventPreset.HasRaidEvents && this.AllowRaidEvents)
			{
				this.TwitchCommandList.Add(new TwitchCommandRedeemRaid());
			}
			if (this.CurrentEventPreset.HasCharityEvents && this.AllowCharityEvents)
			{
				this.TwitchCommandList.Add(new TwitchCommandRedeemCharity());
			}
			if (this.CurrentEventPreset.HasHypeTrainEvents && this.AllowHypeTrainEvents)
			{
				this.TwitchCommandList.Add(new TwitchCommandRedeemHypeTrain());
			}
			if (this.CurrentEventPreset.HasCreatorGoalEvents && this.AllowCreatorGoalEvents)
			{
				this.TwitchCommandList.Add(new TwitchCommandRedeemCreatorGoal());
			}
			this.TwitchCommandList.Add(new TwitchCommandUseProgression());
		}

		// Token: 0x0600C12F RID: 49455 RVA: 0x00470538 File Offset: 0x0046E738
		public void StartTwitchIntegration()
		{
			this.updateTime = 60f;
			try
			{
				if (this.Authentication == null)
				{
					this.Authentication = new TwitchAuthentication();
				}
				this.Authentication.StopListener();
				this.Authentication.GetToken();
			}
			catch (Exception ex)
			{
				Log.Out("Twitch integration failed to start with message " + ex.Message);
				this.updateTime = 5f;
			}
			this.InitialCooldownSet = false;
			this.InitState = TwitchManager.InitStates.WaitingForOAuth;
		}

		// Token: 0x0600C130 RID: 49456 RVA: 0x004705BC File Offset: 0x0046E7BC
		public void StopTwitchIntegration(TwitchManager.InitStates initState = TwitchManager.InitStates.None)
		{
			this.resetClientAttempts = 0;
			this.Disconnect();
			this.TwitchDisconnectPartyUpdate();
			this.ClearEventHandlers();
			if (this.LocalPlayer != null)
			{
				this.LocalPlayer.TwitchEnabled = false;
				this.LocalPlayer.TwitchActionsEnabled = EntityPlayer.TwitchActionsStates.Enabled;
			}
			if (this.Authentication != null)
			{
				this.Authentication.StopListener();
			}
			this.InitState = initState;
		}

		// Token: 0x0600C131 RID: 49457 RVA: 0x00470622 File Offset: 0x0046E822
		public void WaitForOAuth()
		{
			this.updateTime = 10f;
			this.InitState = TwitchManager.InitStates.WaitingForOAuth;
		}

		// Token: 0x0600C132 RID: 49458 RVA: 0x00470636 File Offset: 0x0046E836
		public void WaitForPermission()
		{
			this.updateTime = 10f;
			this.InitState = TwitchManager.InitStates.WaitingForPermission;
		}

		// Token: 0x0600C133 RID: 49459 RVA: 0x0047064A File Offset: 0x0046E84A
		public void DeniedPermission()
		{
			this.InitState = TwitchManager.InitStates.PermissionDenied;
		}

		// Token: 0x0600C134 RID: 49460 RVA: 0x00470654 File Offset: 0x0046E854
		[PublicizedFrom(EAccessModifier.Private)]
		public void ClearEventHandlers()
		{
			GameEventManager gameEventManager = GameEventManager.Current;
			gameEventManager.GameEntitySpawned -= this.Current_GameEntitySpawned;
			gameEventManager.GameEntityDespawned -= this.Current_GameEntityDespawned;
			gameEventManager.GameEntityKilled -= this.Current_GameEntityKilled;
			gameEventManager.GameBlocksAdded -= this.Current_GameBlocksAdded;
			gameEventManager.GameBlocksRemoved -= this.Current_GameBlocksRemoved;
			gameEventManager.GameBlockRemoved -= this.Current_GameBlockRemoved;
			gameEventManager.GameEventApproved -= this.Current_GameEventApproved;
			gameEventManager.TwitchPartyGameEventApproved -= this.Current_TwitchPartyGameEventApproved;
			gameEventManager.TwitchRefundNeeded -= this.Current_TwitchRefundNeeded;
			gameEventManager.GameEventDenied -= this.Current_GameEventDenied;
			gameEventManager.GameEventCompleted -= this.Current_GameEventCompleted;
			if (this.LocalPlayer != null)
			{
				this.LocalPlayer.PartyLeave -= this.LocalPlayer_PartyLeave;
				this.LocalPlayer.PartyJoined -= this.LocalPlayer_PartyJoined;
				this.LocalPlayer.PartyChanged -= this.LocalPlayer_PartyChanged;
				if (this.LocalPlayer.Party != null)
				{
					this.LocalPlayer.Party.PartyMemberAdded -= this.Party_PartyMemberAdded;
					this.LocalPlayer.Party.PartyMemberRemoved -= this.Party_PartyMemberRemoved;
				}
			}
		}

		// Token: 0x0600C135 RID: 49461 RVA: 0x004707C8 File Offset: 0x0046E9C8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_TwitchPartyGameEventApproved(string gameEventID, int targetEntityID, string extraData, string tag)
		{
			foreach (TwitchAction twitchAction in TwitchActionManager.TwitchActions.Values)
			{
				if (twitchAction.IsInPreset(this.CurrentActionPreset) && twitchAction.EventName == gameEventID)
				{
					this.AddCooldownForAction(twitchAction);
					break;
				}
			}
		}

		// Token: 0x0600C136 RID: 49462 RVA: 0x00470840 File Offset: 0x0046EA40
		public void Update(float deltaTime)
		{
			GameManager gameManager = GameManager.Instance;
			if (gameManager.World == null || gameManager.World.Players == null || gameManager.World.Players.Count == 0)
			{
				return;
			}
			this.CurrentUnityTime = Time.time;
			switch (this.InitState)
			{
			case TwitchManager.InitStates.Setup:
			{
				GameEventManager gameEventManager = GameEventManager.Current;
				gameEventManager.GameEventAccessApproved -= this.Current_GameEventAccessApproved;
				gameEventManager.GameEventAccessApproved += this.Current_GameEventAccessApproved;
				this.SetupLocalization();
				if (!this.isLoaded)
				{
					this.isLoaded = true;
					this.LoadViewerData();
				}
				this.LoadMainViewerData();
				this.InitState = TwitchManager.InitStates.None;
				break;
			}
			case TwitchManager.InitStates.WaitingForPermission:
				this.updateTime -= deltaTime;
				if (this.updateTime <= 0f)
				{
					this.StopTwitchIntegration(TwitchManager.InitStates.None);
					Log.Warning("Twitch: login failed in " + this.InitState.ToString() + " state");
					this.InitState = TwitchManager.InitStates.Failed;
					return;
				}
				break;
			case TwitchManager.InitStates.WaitingForOAuth:
				this.updateTime -= deltaTime;
				if (this.updateTime <= 0f)
				{
					this.StopTwitchIntegration(TwitchManager.InitStates.None);
					Log.Warning("Twitch: Login failed in " + this.InitState.ToString() + " state");
					this.InitState = TwitchManager.InitStates.Failed;
					return;
				}
				if (this.Authentication.oauth != "" && this.Authentication.userName != "" && this.Authentication.userID != "")
				{
					this.SetupClient(this.Authentication.userName, this.Authentication.oauth);
					this.EventSub.OnEventReceived -= this.EventSubMessageReceived;
					this.EventSub.OnEventReceived += this.EventSubMessageReceived;
					this.EventSub.Connect();
					this.updateTime = 3f;
					Log.Out("retrieved oauth. Waiting for IRC to post auth...");
					this.InitState = TwitchManager.InitStates.Authenticating;
				}
				break;
			case TwitchManager.InitStates.Authenticating:
				this.updateTime -= deltaTime;
				if (this.updateTime <= 0f)
				{
					if (this.Authentication.oauth != "" && this.Authentication.userName != "" && this.Authentication.userID != "")
					{
						int num = this.resetClientAttempts;
						this.resetClientAttempts = num + 1;
						if (num < 5)
						{
							this.SetupClient(this.Authentication.userName, this.Authentication.oauth);
							this.updateTime = 2.5f;
							Log.Out("attempting to reset client...");
							break;
						}
					}
					Log.Warning("Twitch: Login failed in " + this.InitState.ToString() + " state");
					this.StopTwitchIntegration(TwitchManager.InitStates.Failed);
					this.InitState = TwitchManager.InitStates.Failed;
					return;
				}
				break;
			case TwitchManager.InitStates.Authenticated:
			{
				this.ClearEventHandlers();
				GameEventManager gameEventManager2 = GameEventManager.Current;
				gameEventManager2.GameEntitySpawned += this.Current_GameEntitySpawned;
				gameEventManager2.GameEntityDespawned += this.Current_GameEntityDespawned;
				gameEventManager2.GameEntityKilled += this.Current_GameEntityKilled;
				gameEventManager2.GameBlocksAdded += this.Current_GameBlocksAdded;
				gameEventManager2.GameBlockRemoved += this.Current_GameBlockRemoved;
				gameEventManager2.GameBlocksRemoved += this.Current_GameBlocksRemoved;
				gameEventManager2.GameEventApproved += this.Current_GameEventApproved;
				gameEventManager2.TwitchPartyGameEventApproved += this.Current_TwitchPartyGameEventApproved;
				gameEventManager2.TwitchRefundNeeded += this.Current_TwitchRefundNeeded;
				gameEventManager2.GameEventDenied += this.Current_GameEventDenied;
				gameEventManager2.GameEventCompleted += this.Current_GameEventCompleted;
				this.world = gameManager.World;
				if (this.extensionManager == null)
				{
					this.extensionManager = new ExtensionManager();
					this.extensionManager.Init();
				}
				this.InitState = TwitchManager.InitStates.Ready;
				QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.Enabled, "");
				break;
			}
			case TwitchManager.InitStates.CheckingForExtension:
				if (!this.AllowActions)
				{
					this.InitState = TwitchManager.InitStates.Authenticated;
				}
				else if (!this.checkingExtensionInstalled)
				{
					this.checkingExtensionInstalled = true;
					ExtensionManager.CheckExtensionInstalled(delegate(bool IsInstalled)
					{
						this.checkingExtensionInstalled = false;
						if (!IsInstalled)
						{
							XUiC_MessageBoxWindowGroup.ShowOk(this.LocalPlayerXUi, Localization.Get("xuiTwitchPopup_ExtensionNeededHeader", false, null), Localization.Get("xuiTwitchPopup_ExtensionNeeded", false, null), "", null, true, true, false);
							Application.OpenURL("https://dashboard.twitch.tv/extensions/k6ji189bf7i4ge8il4iczzw7kpgmjt");
						}
						this.InitState = TwitchManager.InitStates.Authenticated;
					});
				}
				break;
			case TwitchManager.InitStates.Ready:
				if (this.LocalPlayer == null)
				{
					this.SetupTwitchCommands();
					this.LocalPlayer = XUiM_Player.GetPlayer() as EntityPlayerLocal;
					this.RefreshPartyInfo();
					this.HighestGameStage = this.LocalPlayer.unModifiedGameStage;
					this.ActionMessages.Clear();
					this.GetCooldownMax();
					this.SetCooldown((float)this.CurrentCooldownPreset.NextCooldownTime, TwitchManager.CooldownTypes.Startup, false, false);
					this.LocalPlayer.PartyLeave += this.LocalPlayer_PartyLeave;
					this.LocalPlayer.PartyJoined += this.LocalPlayer_PartyJoined;
					this.LocalPlayer.PartyChanged += this.LocalPlayer_PartyChanged;
					if (this.LocalPlayer.Party != null)
					{
						this.LocalPlayer.Party.PartyMemberAdded += this.Party_PartyMemberAdded;
						this.LocalPlayer.Party.PartyMemberRemoved += this.Party_PartyMemberRemoved;
					}
					if (!this.InitialCooldownSet)
					{
						if (this.CurrentCooldownPreset.StartCooldownTime > 0)
						{
							this.SetCooldown(100000f, TwitchManager.CooldownTypes.Startup, false, true);
						}
						if (this.CurrentCooldownPreset.CooldownType != CooldownPreset.CooldownTypes.Fill)
						{
							this.SetCooldown(0f, TwitchManager.CooldownTypes.None, false, false);
						}
						this.CurrentActionPreset.HandleCooldowns();
						this.InitialCooldownSet = true;
					}
				}
				this.LocalPlayer.TwitchEnabled = true;
				this.LocalPlayerInLandClaim = GameManager.Instance.World.GetLandClaimOwnerInParty(this.LocalPlayer, this.LocalPlayer.persistentPlayerData);
				if (!this.ircClient.IsConnected)
				{
					Log.Out("Reached 'Ready' but waiting for IRC to post auth message...");
					this.ircClient.Reconnect();
					this.InitState = TwitchManager.InitStates.Authenticating;
					this.updateTime = 30f;
				}
				TwitchManager.LeaderboardStats.UpdateStats(deltaTime);
				if (this.resetCommandsNeeded)
				{
					this.ResetCommands();
				}
				if (!XUi.InGameMenuOpen && this.AllowActions && this.ExtensionCheckTime < 0f)
				{
					this.ExtensionCheckTime = 30f;
					ExtensionManager.CheckExtensionInstalled(delegate(bool IsInstalled)
					{
						if (IsInstalled)
						{
							this.extensionActiveCheckFailures = 0;
							return;
						}
						if (this.extensionActiveCheckFailures < 3)
						{
							this.extensionActiveCheckFailures++;
							return;
						}
						this.extensionActiveCheckFailures = 0;
						LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.LocalPlayer);
						this.ircClient.SendChannelMessage(Localization.Get("TwitchChat_ExtensionNotInstalled", false, null), false);
						XUiC_ChatOutput.AddMessage(uiforPlayer.xui, EnumGameMessages.PlainTextLocal, Localization.Get("TwitchChat_ExtensionNotInstalled", false, null), EChatType.Global, EChatDirection.Inbound, -1, null, null, EMessageSender.None, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
						this.StopTwitchIntegration(TwitchManager.InitStates.None);
						this.InitState = TwitchManager.InitStates.ExtensionNotInstalled;
						Application.OpenURL("https://dashboard.twitch.tv/extensions/k6ji189bf7i4ge8il4iczzw7kpgmjt");
					});
				}
				this.ExtensionCheckTime -= deltaTime;
				if (this.LocalPlayer.Buffs.HasBuff("twitch_extensionneeded"))
				{
					this.updateTime -= deltaTime;
					if (this.updateTime <= 0f)
					{
						ExtensionManager.CheckExtensionInstalled(delegate(bool IsInstalled)
						{
							if (IsInstalled)
							{
								this.LocalPlayer.Buffs.RemoveBuff("twitch_extensionneeded", -1, true);
							}
						});
						this.updateTime = 5f;
					}
				}
				break;
			}
			if (this.extensionManager != null)
			{
				if (this.extensionManager.HasCommand())
				{
					ExtensionAction command = this.extensionManager.GetCommand();
					int num2 = int.Parse(command.username);
					string command2 = command.command;
					bool flag = false;
					int creditUsed = command.creditUsed;
					ExtensionBitAction extensionBitAction = command as ExtensionBitAction;
					this.HandleExtensionMessage(num2, command2, flag, creditUsed, (extensionBitAction != null) ? extensionBitAction.cost : 0);
				}
				this.extensionManager.Update();
			}
			if (this.ircClient != null)
			{
				this.ircClient.Update(deltaTime);
				if (this.ircClient.AvailableMessage())
				{
					this.HandleMessage(this.ircClient.ReadMessage());
				}
				this.ViewerData.Update(deltaTime);
				if (this.LocalPlayer == null)
				{
					return;
				}
				bool flag2 = false;
				for (int i = this.LiveActionEntries.Count - 1; i >= 0; i--)
				{
					if (this.LiveActionEntries[i].ReadyForRemove)
					{
						this.LiveActionEntries.RemoveAt(i);
					}
					else if (this.LiveActionEntries[i].Action.CooldownBlocked)
					{
						flag2 = true;
					}
				}
				for (int j = this.actionSpawnLiveList.Count - 1; j >= 0; j--)
				{
					if (this.actionSpawnLiveList[j].SpawnedEntity == null)
					{
						this.actionSpawnLiveList.RemoveAt(j);
					}
				}
				for (int k = this.LiveEvents.Count - 1; k >= 0; k--)
				{
					if (this.LiveEvents[k].ReadyForRemove)
					{
						this.LiveEvents.RemoveAt(k);
					}
				}
				if (this.LocalPlayer.IsAlive() && this.CooldownTime > 0f && this.TwitchActive)
				{
					if (this.CooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting && this.actionSpawnLiveList.Count == 0 && !flag2)
					{
						this.SetCooldown(this.CooldownTime, TwitchManager.CooldownTypes.MaxReached, false, true);
					}
					if (this.CooldownType == TwitchManager.CooldownTypes.MaxReached || this.CooldownType == TwitchManager.CooldownTypes.Time || this.CooldownType == TwitchManager.CooldownTypes.Startup || this.CooldownType == TwitchManager.CooldownTypes.SafeCooldownExit)
					{
						float cooldownTime = this.CooldownTime;
						this.CooldownTime -= Time.deltaTime;
						if (cooldownTime >= 15f && this.CooldownTime < 15f && this.CooldownTime > 0f && this.CooldownType != TwitchManager.CooldownTypes.SafeCooldownExit)
						{
							this.LocalPlayer.TwitchActionsEnabled = EntityPlayer.TwitchActionsStates.TempDisabledEnding;
						}
					}
					if (this.CooldownTime <= 0f)
					{
						if (this.CooldownType == TwitchManager.CooldownTypes.SafeCooldownExit)
						{
							this.HandleEndCooldownStateChanging();
						}
						else
						{
							this.HandleEndCooldown();
						}
						this.VotingManager.VoteStartDelayTimeRemaining = 10f;
					}
				}
				for (int l = this.liveList.Count - 1; l >= 0; l--)
				{
					if (this.liveList[l].SpawnedEntity == null)
					{
						this.liveList[l].SpawnedEntity = this.world.GetEntity(this.liveList[l].SpawnedEntityID);
						this.liveList[l].SpawnedEntity == null;
					}
				}
				for (int m = this.recentlyDeadList.Count - 1; m >= 0; m--)
				{
					this.recentlyDeadList[m].TimeRemaining -= deltaTime;
					if (this.recentlyDeadList[m].TimeRemaining <= 0f)
					{
						this.recentlyDeadList.RemoveAt(m);
					}
				}
				for (int n = this.liveBlockList.Count - 1; n >= 0; n--)
				{
					TwitchSpawnedBlocksEntry twitchSpawnedBlocksEntry = this.liveBlockList[n];
					if (twitchSpawnedBlocksEntry.TimeRemaining > 0f)
					{
						twitchSpawnedBlocksEntry.TimeRemaining -= deltaTime;
						if (twitchSpawnedBlocksEntry.TimeRemaining <= 0f)
						{
							this.liveBlockList.RemoveAt(n);
						}
					}
				}
				int num3 = GameUtils.WorldTimeToDays(this.world.worldTime);
				if (num3 != this.lastGameDay)
				{
					this.SetupAvailableCommands();
					this.ResetDailyCommands(num3, -1);
					this.HandleCooldownActionLocking();
					this.lastGameDay = num3;
				}
				if (this.CooldownType != TwitchManager.CooldownTypes.Startup && this.TwitchActive && !gameManager.IsPaused() && this.InitState == TwitchManager.InitStates.Ready)
				{
					this.VotingManager.Update(deltaTime);
				}
				this.HandleEventQueue();
				if (this.LocalPlayer.IsAlive() && this.CooldownType != TwitchManager.CooldownTypes.Time)
				{
					int num4 = 0;
					while (num4 < this.QueuedActionEntries.Count)
					{
						if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
						{
							if (!this.QueuedActionEntries[num4].IsSent)
							{
								SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageGameEventRequest>().Setup(this.QueuedActionEntries[num4].Action.EventName, this.QueuedActionEntries[num4].Target.entityId, true, Vector3.zero, null, this.QueuedActionEntries[num4].UserName, "action", this.AllowCrateSharing, true, null), false);
								this.QueuedActionEntries[num4].IsSent = true;
								break;
							}
							num4++;
						}
						else
						{
							if (GameEventManager.Current.HandleAction(this.QueuedActionEntries[num4].Action.EventName, this.LocalPlayer, this.QueuedActionEntries[num4].Target, true, this.QueuedActionEntries[num4].UserName, "action", this.AllowCrateSharing, true, "", null))
							{
								if (this.LocalPlayer.Party != null)
								{
									for (int num5 = 0; num5 < this.LocalPlayer.Party.MemberList.Count; num5++)
									{
										EntityPlayer entityPlayer = this.LocalPlayer.Party.MemberList[num5];
										if (entityPlayer != this.LocalPlayer && entityPlayer.TwitchEnabled)
										{
											SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(this.QueuedActionEntries[num4].Action.EventName, this.QueuedActionEntries[num4].Target.entityId, this.QueuedActionEntries[num4].UserName, "action", NetPackageGameEventResponse.ResponseTypes.TwitchPartyActionApproved, -1, -1, false, ""), false, entityPlayer.entityId, -1, -1, null, 192, false);
										}
									}
								}
								GameEventManager.Current.HandleGameEventApproved(this.QueuedActionEntries[num4].Action.EventName, this.QueuedActionEntries[num4].Target.entityId, this.QueuedActionEntries[num4].UserName, "action");
								break;
							}
							TwitchActionEntry twitchActionEntry = this.QueuedActionEntries[num4];
							ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(twitchActionEntry.UserName);
							this.AddActionHistory(twitchActionEntry, viewerEntry, TwitchActionHistoryEntry.EntryStates.Reimbursed);
							this.ShowReimburseMessage(twitchActionEntry, viewerEntry);
							this.ViewerData.ReimburseAction(twitchActionEntry);
							this.QueuedActionEntries.RemoveAt(num4);
							break;
						}
					}
				}
				this.saveTime -= Time.deltaTime;
				if (this.saveTime <= 0f && (this.dataSaveThreadInfo == null || this.dataSaveThreadInfo.HasTerminated()))
				{
					this.saveTime = 30f;
					if (this.HasDataChanges)
					{
						this.SaveViewerData();
						this.HasDataChanges = false;
					}
				}
				this.updateTime -= deltaTime;
				if (this.updateTime <= 0f)
				{
					this.updateTime = 2f;
					if (this.UseProgression && !this.OverrideProgession && this.commandsAvailable == -1)
					{
						this.commandsAvailable = this.GetCommandCount();
					}
					this.RefreshCommands(true);
					int @int = GameStats.GetInt(EnumGameStats.BloodMoonDay);
					if (this.nextBMDay != @int)
					{
						this.nextBMDay = @int;
						if (num3 != this.currentBMDayEnd)
						{
							this.currentBMDayEnd = this.nextBMDay + 1;
						}
						this.SetupBloodMoonData();
					}
					this.RefreshVoteLockedLevel();
					this.IsSafe = this.LocalPlayer.TwitchSafe;
					this.isBMActive = false;
					if (this.CooldownType != TwitchManager.CooldownTypes.Time && !this.IsVoting)
					{
						if (this.UseActionsDuringBloodmoon != 1)
						{
							if (this.WithinBloodMoonPeriod())
							{
								this.isBMActive = true;
								if (this.CooldownType != TwitchManager.CooldownTypes.BloodMoonDisabled && this.CooldownType != TwitchManager.CooldownTypes.BloodMoonCooldown)
								{
									this.SetCooldown(5f, (this.UseActionsDuringBloodmoon == 0) ? TwitchManager.CooldownTypes.BloodMoonDisabled : TwitchManager.CooldownTypes.BloodMoonCooldown, false, true);
								}
							}
							else if (this.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.CooldownType == TwitchManager.CooldownTypes.BloodMoonCooldown)
							{
								this.SetCooldown(5f, TwitchManager.CooldownTypes.Time, false, true);
								this.currentBMDayEnd = this.nextBMDay + 1;
								this.VotingManager.VoteStartDelayTimeRemaining += 35f;
							}
						}
						else if (this.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.CooldownType == TwitchManager.CooldownTypes.BloodMoonCooldown)
						{
							this.SetCooldown(5f, TwitchManager.CooldownTypes.Time, false, true);
						}
						if (this.AllowActions && this.CooldownType != TwitchManager.CooldownTypes.BloodMoonDisabled && this.CooldownType != TwitchManager.CooldownTypes.BloodMoonCooldown)
						{
							if (this.UseActionsDuringQuests != 1 && this.CooldownType != TwitchManager.CooldownTypes.Time)
							{
								if (QuestEventManager.Current.QuestBounds.width != 0f)
								{
									if (this.UseActionsDuringQuests == 0 && this.CooldownType != TwitchManager.CooldownTypes.QuestDisabled)
									{
										this.SetCooldown(5f, TwitchManager.CooldownTypes.QuestDisabled, false, false);
									}
									else if (this.UseActionsDuringQuests == 2 && this.CooldownType != TwitchManager.CooldownTypes.QuestCooldown)
									{
										this.SetCooldown(5f, TwitchManager.CooldownTypes.QuestCooldown, false, false);
									}
								}
								else if (this.CooldownType == TwitchManager.CooldownTypes.QuestCooldown || this.CooldownType == TwitchManager.CooldownTypes.QuestDisabled)
								{
									this.SetCooldown(60f, TwitchManager.CooldownTypes.Time, false, true);
									this.CurrentCooldownFill = 0f;
								}
							}
							else if (this.UseActionsDuringQuests == 1 && (this.CooldownType == TwitchManager.CooldownTypes.QuestCooldown || this.CooldownType == TwitchManager.CooldownTypes.QuestDisabled))
							{
								this.SetCooldown(60f, TwitchManager.CooldownTypes.Time, false, true);
							}
							if (this.CooldownType != TwitchManager.CooldownTypes.Time && this.CooldownType != TwitchManager.CooldownTypes.Startup && this.CooldownType != TwitchManager.CooldownTypes.MaxReached && this.CooldownType != TwitchManager.CooldownTypes.MaxReachedWaiting && this.CooldownType != TwitchManager.CooldownTypes.QuestCooldown && this.CooldownType != TwitchManager.CooldownTypes.QuestDisabled)
							{
								if (this.CooldownType != TwitchManager.CooldownTypes.SafeCooldown && this.LocalPlayer.TwitchSafe)
								{
									this.SetCooldown(5f, TwitchManager.CooldownTypes.SafeCooldown, false, false);
								}
								else if (this.CooldownType == TwitchManager.CooldownTypes.SafeCooldown && !this.LocalPlayer.TwitchSafe)
								{
									this.SetCooldown(5f, TwitchManager.CooldownTypes.SafeCooldownExit, false, true);
								}
							}
						}
						else if (this.CooldownType == TwitchManager.CooldownTypes.QuestCooldown || this.CooldownType == TwitchManager.CooldownTypes.QuestDisabled)
						{
							this.SetCooldown(60f, TwitchManager.CooldownTypes.Time, false, true);
						}
					}
					for (int num6 = this.RespawnEntries.Count - 1; num6 >= 0; num6--)
					{
						TwitchRespawnEntry twitchRespawnEntry = this.RespawnEntries[num6];
						if (twitchRespawnEntry.CanRespawn(this))
						{
							EntityPlayer target = twitchRespawnEntry.Target;
							if (!this.PartyInfo.ContainsKey(target) || this.PartyInfo[target].Cooldown <= 0f)
							{
								if (target.Buffs.HasBuff("twitch_pausedspawns"))
								{
									target.Buffs.RemoveBuff("twitch_pausedspawns", -1, true);
								}
								target.PlayOneShot("twitch_unpause", false, false, false, null, 1f);
								this.QueuedActionEntries.Add(twitchRespawnEntry.RespawnAction());
							}
						}
					}
				}
				if (this.lastAlive && this.LocalPlayer.IsDead())
				{
					this.CurrentCooldownFill = 0f;
					if (this.CurrentCooldownPreset.CooldownType == CooldownPreset.CooldownTypes.Fill)
					{
						this.SetCooldown((float)this.CurrentCooldownPreset.AfterDeathCooldownTime, TwitchManager.CooldownTypes.Time, false, true);
					}
					this.KillAllSpawnsForPlayer(this.LocalPlayer);
				}
				if (this.lastAlive && this.LocalPlayer.IsAlive())
				{
					TwitchManager.DeathText = "";
				}
				if (!this.lastAlive && this.LocalPlayer.IsAlive())
				{
					this.respawnEventNeeded = true;
				}
				if (this.respawnEventNeeded && this.CheckCanRespawnEvent(this.LocalPlayer))
				{
					if (this.OnPlayerRespawnEvent != "")
					{
						GameEventManager.Current.HandleAction(this.OnPlayerRespawnEvent, this.LocalPlayer, this.LocalPlayer, false, "", "", false, true, "", null);
					}
					this.respawnEventNeeded = false;
				}
				this.lastAlive = this.LocalPlayer.IsAlive();
				this.twitchPlayerDeathsThisFrame.Clear();
				this.UpdatePartyInfo(deltaTime);
			}
			this.HandleInGameChatQueue();
		}

		// Token: 0x0600C137 RID: 49463 RVA: 0x00471B52 File Offset: 0x0046FD52
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleEndCooldown()
		{
			this.CurrentCooldownFill = 0f;
			Manager.BroadcastPlayByLocalPlayer(this.LocalPlayer.position, "twitch_cooldown_ended");
			this.ircClient.SendChannelMessage(this.chatOutput_CooldownComplete, true);
			this.HandleEndCooldownStateChanging();
		}

		// Token: 0x0600C138 RID: 49464 RVA: 0x00471B8C File Offset: 0x0046FD8C
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleEndCooldownStateChanging()
		{
			this.CooldownType = TwitchManager.CooldownTypes.None;
			if (this.LocalPlayer.TwitchActionsEnabled == EntityPlayer.TwitchActionsStates.TempDisabled || this.LocalPlayer.TwitchActionsEnabled == EntityPlayer.TwitchActionsStates.TempDisabledEnding)
			{
				this.LocalPlayer.TwitchActionsEnabled = EntityPlayer.TwitchActionsStates.Enabled;
			}
			if (this.ConnectionStateChanged != null)
			{
				this.ConnectionStateChanged(this.initState, this.initState);
			}
			this.HandleCooldownActionLocking();
		}

		// Token: 0x0600C139 RID: 49465 RVA: 0x00471BED File Offset: 0x0046FDED
		public void AddToInGameChatQueue(string msg, string sound = null)
		{
			this.inGameChatQueue.Add(new TwitchMessageEntry(msg, sound));
		}

		// Token: 0x0600C13A RID: 49466 RVA: 0x00471C04 File Offset: 0x0046FE04
		public void HandleInGameChatQueue()
		{
			if (this.inGameChatQueue.Count > 0 && this.LocalPlayer != null && this.LocalPlayer.IsAlive())
			{
				TwitchMessageEntry twitchMessageEntry = this.inGameChatQueue[0];
				XUiC_ChatOutput.AddMessage(this.LocalPlayerXUi, EnumGameMessages.PlainTextLocal, twitchMessageEntry.Message, EChatType.Global, EChatDirection.Inbound, -1, null, null, EMessageSender.Server, GeneratedTextManager.TextFilteringMode.None, GeneratedTextManager.BbCodeSupportMode.Supported);
				if (twitchMessageEntry.Sound != null)
				{
					this.LocalPlayer.PlayOneShot(twitchMessageEntry.Sound, false, false, false, null, 1f);
				}
				this.inGameChatQueue.RemoveAt(0);
			}
		}

		// Token: 0x0600C13B RID: 49467 RVA: 0x00471C8F File Offset: 0x0046FE8F
		public void RefreshVoteLockedLevel()
		{
			this.VoteLockedLevel = this.LocalPlayer.HasTwitchVoteLockMember();
		}

		// Token: 0x0600C13C RID: 49468 RVA: 0x00471CA4 File Offset: 0x0046FEA4
		public void SetupBloodMoonData()
		{
			ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			this.BMCooldownStart = item - this.CurrentCooldownPreset.BMStartOffset;
			this.BMCooldownEnd = item2 + this.CurrentCooldownPreset.BMEndOffset;
		}

		// Token: 0x0600C13D RID: 49469 RVA: 0x00471CF0 File Offset: 0x0046FEF0
		public bool WithinBloodMoonPeriod()
		{
			ulong worldTime = this.world.worldTime;
			int num = GameUtils.WorldTimeToDays(worldTime);
			int num2 = GameUtils.WorldTimeToHours(worldTime);
			if (num == this.nextBMDay)
			{
				if (num2 >= this.BMCooldownStart)
				{
					return true;
				}
			}
			else if (num > 1 && num == this.currentBMDayEnd && num2 < this.BMCooldownEnd)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600C13E RID: 49470 RVA: 0x00471D42 File Offset: 0x0046FF42
		[PublicizedFrom(EAccessModifier.Private)]
		public void Party_PartyMemberAdded(EntityPlayer player)
		{
			this.GetCooldownMax();
			if (!this.PartyInfo.ContainsKey(player))
			{
				this.PartyInfo.Add(player, new TwitchManager.TwitchPartyMemberInfo());
				ExtensionManager extensionManager = this.extensionManager;
				if (extensionManager == null)
				{
					return;
				}
				extensionManager.OnPartyChanged();
			}
		}

		// Token: 0x0600C13F RID: 49471 RVA: 0x00471D79 File Offset: 0x0046FF79
		[PublicizedFrom(EAccessModifier.Private)]
		public void Party_PartyMemberRemoved(EntityPlayer player)
		{
			this.GetCooldownMax();
			if (this.PartyInfo.ContainsKey(player))
			{
				this.PartyInfo.Remove(player);
				ExtensionManager extensionManager = this.extensionManager;
				if (extensionManager == null)
				{
					return;
				}
				extensionManager.OnPartyChanged();
			}
		}

		// Token: 0x0600C140 RID: 49472 RVA: 0x00471DAC File Offset: 0x0046FFAC
		[PublicizedFrom(EAccessModifier.Protected)]
		public void RefreshPartyInfo()
		{
			if (this.LocalPlayer == null)
			{
				return;
			}
			if (this.LocalPlayer.Party == null)
			{
				this.PartyInfo.Clear();
				return;
			}
			this.PartyInfo.Clear();
			for (int i = 0; i < this.LocalPlayer.Party.MemberList.Count; i++)
			{
				EntityPlayer entityPlayer = this.LocalPlayer.Party.MemberList[i];
				if (!(entityPlayer == this.LocalPlayer) && !this.PartyInfo.ContainsKey(entityPlayer))
				{
					this.PartyInfo.Add(entityPlayer, new TwitchManager.TwitchPartyMemberInfo());
				}
			}
		}

		// Token: 0x0600C141 RID: 49473 RVA: 0x00471E50 File Offset: 0x00470050
		[PublicizedFrom(EAccessModifier.Protected)]
		public void UpdatePartyInfo(float deltaTime)
		{
			bool flag = false;
			foreach (EntityPlayer entityPlayer in this.PartyInfo.Keys)
			{
				TwitchManager.TwitchPartyMemberInfo twitchPartyMemberInfo = this.PartyInfo[entityPlayer];
				if (!twitchPartyMemberInfo.LastAlive && entityPlayer.IsAlive() && this.PartyRespawnEvent != "")
				{
					GameEventManager.Current.HandleAction(this.PartyRespawnEvent, this.LocalPlayer, entityPlayer, false, "", "", false, true, "", null);
				}
				if (twitchPartyMemberInfo.LastOptedOut != (entityPlayer.TwitchActionsEnabled == EntityPlayer.TwitchActionsStates.Disabled))
				{
					twitchPartyMemberInfo.LastOptedOut = entityPlayer.TwitchActionsEnabled == EntityPlayer.TwitchActionsStates.Disabled;
					flag = true;
				}
				if (twitchPartyMemberInfo.Cooldown > 0f)
				{
					twitchPartyMemberInfo.Cooldown -= deltaTime;
				}
				if (twitchPartyMemberInfo.LastAlive && !entityPlayer.IsAlive())
				{
					this.KillAllSpawnsForPlayer(entityPlayer);
					twitchPartyMemberInfo.Cooldown = 60f;
				}
				if (!twitchPartyMemberInfo.LastAlive && entityPlayer.IsAlive())
				{
					twitchPartyMemberInfo.NeedsRespawnEvent = true;
				}
				if (twitchPartyMemberInfo.NeedsRespawnEvent && this.CheckCanRespawnEvent(entityPlayer) && twitchPartyMemberInfo.Cooldown <= 0f)
				{
					if (this.OnPlayerRespawnEvent != "")
					{
						GameEventManager.Current.HandleAction(this.OnPlayerRespawnEvent, this.LocalPlayer, entityPlayer, false, "", "", false, true, "", null);
					}
					twitchPartyMemberInfo.NeedsRespawnEvent = false;
				}
				twitchPartyMemberInfo.LastAlive = entityPlayer.IsAlive();
			}
			if (flag)
			{
				this.GetCooldownMax();
			}
		}

		// Token: 0x0600C142 RID: 49474 RVA: 0x00471FFC File Offset: 0x004701FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public void TwitchDisconnectPartyUpdate()
		{
			this.RespawnEntries.Clear();
			if (this.OnPlayerRespawnEvent != "")
			{
				GameEventManager.Current.HandleAction(this.OnPlayerRespawnEvent, this.LocalPlayer, this.LocalPlayer, false, "", "", false, true, "", null);
			}
			if (this.LocalPlayer != null && this.LocalPlayer.Buffs.HasBuff("twitch_pausedspawns"))
			{
				this.LocalPlayer.Buffs.RemoveBuff("twitch_pausedspawns", -1, true);
			}
			foreach (EntityPlayer entityPlayer in this.PartyInfo.Keys)
			{
				TwitchManager.TwitchPartyMemberInfo twitchPartyMemberInfo = this.PartyInfo[entityPlayer];
				if (twitchPartyMemberInfo.NeedsRespawnEvent)
				{
					if (this.OnPlayerRespawnEvent != "")
					{
						GameEventManager.Current.HandleAction(this.OnPlayerRespawnEvent, this.LocalPlayer, entityPlayer, false, "", "", false, true, "", null);
					}
					twitchPartyMemberInfo.NeedsRespawnEvent = false;
				}
				if (entityPlayer.Buffs.HasBuff("twitch_pausedspawns"))
				{
					entityPlayer.Buffs.RemoveBuff("twitch_pausedspawns", -1, true);
				}
			}
		}

		// Token: 0x0600C143 RID: 49475 RVA: 0x0047215C File Offset: 0x0047035C
		public bool CheckCanRespawnEvent(EntityPlayer player)
		{
			return player != null && player.IsAlive() && player.TwitchActionsEnabled == EntityPlayer.TwitchActionsStates.Enabled && !player.TwitchSafe;
		}

		// Token: 0x0600C144 RID: 49476 RVA: 0x00472184 File Offset: 0x00470384
		[PublicizedFrom(EAccessModifier.Protected)]
		public void KillAllSpawnsForPlayer(EntityPlayer player)
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = this.RespawnEntries.Count - 1; i >= 0; i--)
			{
				TwitchRespawnEntry twitchRespawnEntry = this.RespawnEntries[i];
				if (twitchRespawnEntry != null && twitchRespawnEntry.Target == player)
				{
					twitchRespawnEntry.NeedsRespawn = twitchRespawnEntry.SpawnedEntities.Count > twitchRespawnEntry.Action.RespawnThreshold;
					if (twitchRespawnEntry.NeedsRespawn)
					{
						if (flag)
						{
							stringBuilder.Append(", ");
						}
						flag = true;
						stringBuilder.Append(twitchRespawnEntry.Action.Command);
					}
					else
					{
						Debug.LogWarning(string.Format("Respawn Entry removed '{0}' because count {1} was less than {2}", twitchRespawnEntry.Action.Command, twitchRespawnEntry.SpawnedEntities.Count, twitchRespawnEntry.Action.RespawnThreshold));
						this.RespawnEntries.RemoveAt(i);
					}
				}
			}
			if (flag)
			{
				if (!player.Buffs.HasBuff("twitch_pausedspawns"))
				{
					player.Buffs.AddBuff("twitch_pausedspawns", -1, true, false, -1f);
				}
				string text = stringBuilder.ToString();
				string text2 = string.Format(this.ingameOutput_BitRespawns, text);
				this.AddToInGameChatQueue(text2, "twitch_pause");
				Debug.LogWarning(string.Format("Respawns Found for {0}: {1}", player.EntityName, text));
			}
			else
			{
				Debug.LogWarning("No Respawns Found!");
				if (player.Buffs.HasBuff("twitch_pausedspawns"))
				{
					player.Buffs.RemoveBuff("twitch_pausedspawns", -1, true);
				}
			}
			if (this.OnPlayerDeathEvent != "")
			{
				GameEventManager.Current.HandleAction(this.OnPlayerDeathEvent, this.LocalPlayer, player, false, "", "", false, true, "", null);
			}
			for (int j = this.LiveActionEntries.Count - 1; j >= 0; j--)
			{
				if (this.LiveActionEntries[j] != null && this.LiveActionEntries[j].Target == player)
				{
					TwitchActionEntry twitchActionEntry = this.LiveActionEntries[j];
					twitchActionEntry.ReadyForRemove = true;
					if (twitchActionEntry.HistoryEntry != null)
					{
						twitchActionEntry.HistoryEntry.EntryState = TwitchActionHistoryEntry.EntryStates.Despawned;
					}
				}
			}
		}

		// Token: 0x0600C145 RID: 49477 RVA: 0x004723B4 File Offset: 0x004705B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void LocalPlayer_PartyChanged(Party _affectedParty, EntityPlayer _player)
		{
			if (this.extensionManager != null)
			{
				this.extensionManager.OnPartyChanged();
			}
			this.GetCooldownMax();
		}

		// Token: 0x0600C146 RID: 49478 RVA: 0x004723B4 File Offset: 0x004705B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void LocalPlayer_PartyLeave(Party _affectedParty, EntityPlayer _player)
		{
			if (this.extensionManager != null)
			{
				this.extensionManager.OnPartyChanged();
			}
			this.GetCooldownMax();
		}

		// Token: 0x0600C147 RID: 49479 RVA: 0x004723D0 File Offset: 0x004705D0
		[PublicizedFrom(EAccessModifier.Private)]
		public void LocalPlayer_PartyJoined(Party _affectedParty, EntityPlayer _player)
		{
			if (this.LocalPlayer.Party != null)
			{
				this.LocalPlayer.Party.PartyMemberAdded += this.Party_PartyMemberAdded;
				this.LocalPlayer.Party.PartyMemberRemoved += this.Party_PartyMemberRemoved;
			}
			if (this.extensionManager != null)
			{
				this.extensionManager.OnPartyChanged();
			}
			this.GetCooldownMax();
			this.RefreshPartyInfo();
		}

		// Token: 0x0600C148 RID: 49480 RVA: 0x00472444 File Offset: 0x00470644
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEntityKilled(int entityID)
		{
			for (int i = this.liveList.Count - 1; i >= 0; i--)
			{
				TwitchSpawnedEntityEntry twitchSpawnedEntityEntry = this.liveList[i];
				if (twitchSpawnedEntityEntry.SpawnedEntityID == entityID)
				{
					if (twitchSpawnedEntityEntry.RespawnEntry != null)
					{
						TwitchRespawnEntry respawnEntry = twitchSpawnedEntityEntry.RespawnEntry;
						if (respawnEntry.RemoveSpawnedEntry(entityID, true) && respawnEntry.ReadyForRemove)
						{
							this.RespawnEntries.Remove(respawnEntry);
						}
					}
					this.actionSpawnLiveList.Remove(twitchSpawnedEntityEntry);
					this.recentlyDeadList.Add(new TwitchRecentlyRemovedEntityEntry(twitchSpawnedEntityEntry));
					this.liveList.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600C149 RID: 49481 RVA: 0x004724D8 File Offset: 0x004706D8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEntityDespawned(int entityID)
		{
			for (int i = this.liveList.Count - 1; i >= 0; i--)
			{
				if (this.liveList[i].SpawnedEntityID == entityID)
				{
					TwitchSpawnedEntityEntry twitchSpawnedEntityEntry = this.liveList[i];
					if (twitchSpawnedEntityEntry.Action != null && twitchSpawnedEntityEntry.Action.UserName != null)
					{
						this.ViewerData.AddPoints(twitchSpawnedEntityEntry.Action.UserName, (int)((float)twitchSpawnedEntityEntry.Action.ActionCost * 0.25f), false, false);
						if (twitchSpawnedEntityEntry.Action.HistoryEntry != null)
						{
							twitchSpawnedEntityEntry.Action.HistoryEntry.EntryState = TwitchActionHistoryEntry.EntryStates.Despawned;
						}
						if (twitchSpawnedEntityEntry.RespawnEntry != null)
						{
							TwitchRespawnEntry respawnEntry = twitchSpawnedEntityEntry.RespawnEntry;
							if (respawnEntry.RemoveSpawnedEntry(entityID, false) && respawnEntry.RespawnsLeft == 0)
							{
								this.RespawnEntries.Remove(respawnEntry);
							}
						}
					}
					else if (twitchSpawnedEntityEntry.Event != null && twitchSpawnedEntityEntry.Event.HistoryEntry != null)
					{
						twitchSpawnedEntityEntry.Event.HistoryEntry.EntryState = TwitchActionHistoryEntry.EntryStates.Despawned;
					}
					this.actionSpawnLiveList.Remove(twitchSpawnedEntityEntry);
					this.recentlyDeadList.Add(new TwitchRecentlyRemovedEntityEntry(twitchSpawnedEntityEntry));
					this.liveList.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600C14A RID: 49482 RVA: 0x00472609 File Offset: 0x00470809
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEventAccessApproved()
		{
			if (this.InitState == TwitchManager.InitStates.None || this.InitState == TwitchManager.InitStates.WaitingForPermission)
			{
				this.StartTwitchIntegration();
			}
		}

		// Token: 0x0600C14B RID: 49483 RVA: 0x00472624 File Offset: 0x00470824
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEventApproved(string gameEventID, int targetEntityID, string viewerName, string tag)
		{
			if (tag == "action")
			{
				for (int i = 0; i < this.QueuedActionEntries.Count; i++)
				{
					if (this.QueuedActionEntries[i].Action.EventName == gameEventID && this.QueuedActionEntries[i].Target.entityId == targetEntityID && this.QueuedActionEntries[i].UserName == viewerName)
					{
						this.ConfirmAction(this.QueuedActionEntries[i]);
						this.LiveActionEntries.Add(this.QueuedActionEntries[i]);
						this.QueuedActionEntries.RemoveAt(i);
						return;
					}
				}
				return;
			}
			if (tag == "event")
			{
				for (int j = 0; j < this.EventQueue.Count; j++)
				{
					if (this.EventQueue[j].UserName == viewerName && this.EventQueue[j].Event.EventName == gameEventID)
					{
						this.LiveEvents.Add(this.EventQueue[j]);
						this.EventQueue.RemoveAt(j);
						return;
					}
				}
			}
		}

		// Token: 0x0600C14C RID: 49484 RVA: 0x00472764 File Offset: 0x00470964
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEventDenied(string gameEventID, int targetEntityID, string viewerName, string tag)
		{
			if (tag == "action")
			{
				for (int i = 0; i < this.QueuedActionEntries.Count; i++)
				{
					if (this.QueuedActionEntries[i].Action.EventName == gameEventID && this.QueuedActionEntries[i].Target.entityId == targetEntityID && this.QueuedActionEntries[i].UserName == viewerName)
					{
						this.ViewerData.ReimburseAction(this.QueuedActionEntries[i]);
						this.QueuedActionEntries.RemoveAt(i);
						return;
					}
				}
			}
		}

		// Token: 0x0600C14D RID: 49485 RVA: 0x00472810 File Offset: 0x00470A10
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_TwitchRefundNeeded(string gameEventID, int targetEntityID, string viewerName, string tag)
		{
			if (tag == "action")
			{
				for (int i = 0; i < this.LiveActionEntries.Count; i++)
				{
					TwitchActionEntry twitchActionEntry = this.LiveActionEntries[i];
					if (twitchActionEntry.Action.EventName == gameEventID && twitchActionEntry.Target.entityId == targetEntityID && twitchActionEntry.UserName == viewerName)
					{
						this.ViewerData.ReimburseAction(this.LiveActionEntries[i]);
						this.ShowReimburseMessage(twitchActionEntry, null);
						Debug.LogWarning(string.Format("TwitchAction {0} refunded for {1}.", twitchActionEntry.Action.Name, viewerName));
						if (twitchActionEntry.HistoryEntry != null)
						{
							twitchActionEntry.HistoryEntry.EntryState = TwitchActionHistoryEntry.EntryStates.Reimbursed;
						}
						if (twitchActionEntry.Action.tempCooldown > 30f)
						{
							twitchActionEntry.Action.SetCooldown(this.CurrentUnityTime, 30f);
						}
						this.LiveActionEntries.RemoveAt(i);
						return;
					}
				}
				return;
			}
			if (tag == "event")
			{
				int j = 0;
				while (j < this.LiveEvents.Count)
				{
					TwitchEventActionEntry twitchEventActionEntry = this.LiveEvents[j];
					if (twitchEventActionEntry.Event.EventName == gameEventID && twitchEventActionEntry.UserName == viewerName)
					{
						Debug.LogWarning(string.Format("Twitch Debug: Live Event: Refunded {0} for {1}.", twitchEventActionEntry.Event.EventTitle, viewerName));
						if (twitchEventActionEntry.HistoryEntry != null)
						{
							twitchEventActionEntry.HistoryEntry.EntryState = TwitchActionHistoryEntry.EntryStates.Reimbursed;
							return;
						}
						break;
					}
					else
					{
						j++;
					}
				}
			}
		}

		// Token: 0x0600C14E RID: 49486 RVA: 0x00472998 File Offset: 0x00470B98
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEventCompleted(string gameEventID, int targetEntityID, string viewerName, string tag)
		{
			if (tag == "action")
			{
				for (int i = 0; i < this.LiveActionEntries.Count; i++)
				{
					TwitchActionEntry twitchActionEntry = this.LiveActionEntries[i];
					if (!twitchActionEntry.ReadyForRemove && twitchActionEntry.Action.EventName == gameEventID && twitchActionEntry.Target.entityId == targetEntityID && twitchActionEntry.UserName == viewerName)
					{
						twitchActionEntry.ReadyForRemove = true;
						if (twitchActionEntry.HistoryEntry != null)
						{
							twitchActionEntry.HistoryEntry.EntryState = TwitchActionHistoryEntry.EntryStates.Completed;
						}
						return;
					}
				}
				return;
			}
			if (tag == "event")
			{
				for (int j = 0; j < this.LiveEvents.Count; j++)
				{
					TwitchEventActionEntry twitchEventActionEntry = this.LiveEvents[j];
					if (!twitchEventActionEntry.ReadyForRemove && twitchEventActionEntry.UserName == viewerName && twitchEventActionEntry.Event.EventName == gameEventID)
					{
						if (twitchEventActionEntry.HistoryEntry != null)
						{
							twitchEventActionEntry.HistoryEntry.EntryState = TwitchActionHistoryEntry.EntryStates.Completed;
						}
						twitchEventActionEntry.ReadyForRemove = true;
						return;
					}
				}
			}
		}

		// Token: 0x0600C14F RID: 49487 RVA: 0x00472AA0 File Offset: 0x00470CA0
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEntitySpawned(string gameEventID, int entityID, string tag)
		{
			if (tag == "action")
			{
				int i = 0;
				while (i < this.LiveActionEntries.Count)
				{
					if (!this.LiveActionEntries[i].ReadyForRemove && this.LiveActionEntries[i].Action.EventName == gameEventID)
					{
						Entity entity = null;
						if (!this.LiveActionEntries[i].Action.AddsToCooldown)
						{
							return;
						}
						TwitchSpawnedEntityEntry twitchSpawnedEntityEntry = new TwitchSpawnedEntityEntry
						{
							Action = this.LiveActionEntries[i],
							SpawnedEntityID = entityID
						};
						this.liveList.Add(twitchSpawnedEntityEntry);
						if (entity == null)
						{
							entity = this.world.GetEntity(entityID);
						}
						twitchSpawnedEntityEntry.SpawnedEntity = entity;
						if (twitchSpawnedEntityEntry.Action.Action.RespawnCountType != TwitchAction.RespawnCountTypes.None)
						{
							TwitchRespawnEntry respawnEntry = this.GetRespawnEntry(twitchSpawnedEntityEntry.Action.UserName, twitchSpawnedEntityEntry.Action.Target, twitchSpawnedEntityEntry.Action.Action);
							respawnEntry.SpawnedEntities.Add(entityID);
							twitchSpawnedEntityEntry.RespawnEntry = respawnEntry;
						}
						this.actionSpawnLiveList.Add(twitchSpawnedEntityEntry);
						return;
					}
					else
					{
						i++;
					}
				}
				return;
			}
			if (tag == "event")
			{
				int j = 0;
				while (j < this.LiveEvents.Count)
				{
					if (!this.LiveEvents[j].ReadyForRemove && this.LiveEvents[j].Event.EventName == gameEventID)
					{
						Entity entity2 = null;
						if (entity2 == null)
						{
							entity2 = this.world.GetEntity(entityID);
						}
						if (!(entity2 is EntityAlive))
						{
							return;
						}
						TwitchSpawnedEntityEntry twitchSpawnedEntityEntry2 = new TwitchSpawnedEntityEntry
						{
							Event = this.LiveEvents[j],
							SpawnedEntityID = entityID
						};
						this.liveList.Add(twitchSpawnedEntityEntry2);
						twitchSpawnedEntityEntry2.SpawnedEntity = entity2;
						this.actionSpawnLiveList.Add(twitchSpawnedEntityEntry2);
						return;
					}
					else
					{
						j++;
					}
				}
				return;
			}
			if (!(tag == "vote") || this.VotingManager.CurrentEvent == null || !(this.VotingManager.CurrentEvent.VoteClass.GameEvent == gameEventID))
			{
				return;
			}
			Entity entity3 = null;
			if (entity3 == null)
			{
				entity3 = this.world.GetEntity(entityID);
			}
			if (!(entity3 is EntityAlive))
			{
				return;
			}
			TwitchSpawnedEntityEntry twitchSpawnedEntityEntry3 = new TwitchSpawnedEntityEntry
			{
				Vote = this.VotingManager.CurrentEvent,
				SpawnedEntityID = entityID
			};
			this.liveList.Add(twitchSpawnedEntityEntry3);
			twitchSpawnedEntityEntry3.SpawnedEntity = entity3;
			this.VotingManager.CurrentEvent.ActiveSpawns.Add(entityID);
			this.actionSpawnLiveList.Add(twitchSpawnedEntityEntry3);
		}

		// Token: 0x0600C150 RID: 49488 RVA: 0x00472D5C File Offset: 0x00470F5C
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameBlocksAdded(string gameEventID, int blockGroupID, List<Vector3i> blockList, string tag)
		{
			if (tag == "action")
			{
				int i = 0;
				while (i < this.LiveActionEntries.Count)
				{
					if (!this.LiveActionEntries[i].ReadyForRemove && this.LiveActionEntries[i].Action.EventName == gameEventID)
					{
						if (!this.LiveActionEntries[i].Action.AddsToCooldown)
						{
							return;
						}
						TwitchSpawnedBlocksEntry twitchSpawnedBlocksEntry = new TwitchSpawnedBlocksEntry
						{
							BlockGroupID = blockGroupID,
							Action = this.LiveActionEntries[i],
							blocks = blockList.ToList<Vector3i>()
						};
						this.liveBlockList.Add(twitchSpawnedBlocksEntry);
						if (twitchSpawnedBlocksEntry.Action.Action.RespawnCountType != TwitchAction.RespawnCountTypes.None)
						{
							TwitchRespawnEntry respawnEntry = this.GetRespawnEntry(twitchSpawnedBlocksEntry.Action.UserName, twitchSpawnedBlocksEntry.Action.Target, twitchSpawnedBlocksEntry.Action.Action);
							respawnEntry.SpawnedBlocks.AddRange(blockList);
							twitchSpawnedBlocksEntry.RespawnEntry = respawnEntry;
						}
						return;
					}
					else
					{
						i++;
					}
				}
				return;
			}
			if (tag == "event")
			{
				for (int j = 0; j < this.LiveEvents.Count; j++)
				{
					if (!this.LiveEvents[j].ReadyForRemove && this.LiveEvents[j].Event.EventName == gameEventID)
					{
						TwitchSpawnedBlocksEntry twitchSpawnedBlocksEntry2 = new TwitchSpawnedBlocksEntry
						{
							BlockGroupID = blockGroupID,
							Event = this.LiveEvents[j],
							blocks = blockList.ToList<Vector3i>()
						};
						this.liveBlockList.Add(twitchSpawnedBlocksEntry2);
						return;
					}
				}
				return;
			}
			if (tag == "vote" && this.VotingManager.CurrentEvent != null && this.VotingManager.CurrentEvent.VoteClass.GameEvent == gameEventID)
			{
				TwitchSpawnedBlocksEntry twitchSpawnedBlocksEntry3 = new TwitchSpawnedBlocksEntry
				{
					BlockGroupID = blockGroupID,
					Vote = this.VotingManager.CurrentEvent,
					blocks = blockList.ToList<Vector3i>()
				};
				this.liveBlockList.Add(twitchSpawnedBlocksEntry3);
				return;
			}
		}

		// Token: 0x0600C151 RID: 49489 RVA: 0x00472F70 File Offset: 0x00471170
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameBlocksRemoved(int blockGroupID, bool isDespawn)
		{
			for (int i = 0; i < this.liveBlockList.Count; i++)
			{
				if (this.liveBlockList[i].BlockGroupID == blockGroupID)
				{
					if (this.liveBlockList[i].RespawnEntry != null)
					{
						TwitchRespawnEntry respawnEntry = this.liveBlockList[i].RespawnEntry;
						if (respawnEntry.RemoveAllSpawnedBlock(!isDespawn) && respawnEntry.ReadyForRemove)
						{
							this.RespawnEntries.Remove(respawnEntry);
						}
					}
					this.liveBlockList.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600C152 RID: 49490 RVA: 0x00472FFC File Offset: 0x004711FC
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameBlockRemoved(Vector3i blockRemoved)
		{
			for (int i = 0; i < this.liveBlockList.Count; i++)
			{
				if (this.liveBlockList[i].RemoveBlock(blockRemoved))
				{
					this.liveBlockList[i].TimeRemaining = 5f;
					return;
				}
			}
		}

		// Token: 0x0600C153 RID: 49491 RVA: 0x0047304C File Offset: 0x0047124C
		public void HandleConsoleAction(List<string> consoleParams)
		{
			for (int i = 0; i < this.TwitchCommandList.Count; i++)
			{
				for (int j = 0; j < this.TwitchCommandList[i].CommandText.Length; j++)
				{
					if (consoleParams[0].StartsWith(this.TwitchCommandList[i].CommandText[j]))
					{
						this.TwitchCommandList[i].ExecuteConsole(consoleParams);
						return;
					}
				}
			}
		}

		// Token: 0x0600C154 RID: 49492 RVA: 0x004730C4 File Offset: 0x004712C4
		public bool IsActionAvailable(string actionName)
		{
			if (!this.VotingManager.VotingIsActive)
			{
				if (actionName[0] != '#')
				{
					actionName = "#" + actionName.ToLower();
				}
				if (this.twitchActive && this.VoteLockedLevel != TwitchVoteLockTypes.ActionsLocked && this.AllowActions && this.AvailableCommands.ContainsKey(actionName))
				{
					TwitchAction twitchAction = this.AvailableCommands[actionName];
					if (!twitchAction.IgnoreCooldown)
					{
						if (this.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.CooldownType == TwitchManager.CooldownTypes.Time || this.CooldownType == TwitchManager.CooldownTypes.QuestDisabled)
						{
							return false;
						}
						if ((this.CooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting || this.CooldownType == TwitchManager.CooldownTypes.SafeCooldown) && twitchAction.WaitingBlocked)
						{
							return false;
						}
					}
					if (this.UseProgression && !this.OverrideProgession && twitchAction.StartGameStage != -1 && twitchAction.StartGameStage > this.HighestGameStage)
					{
						return false;
					}
					if (!twitchAction.CanUse)
					{
						return false;
					}
					if ((twitchAction.IgnoreCooldown || !twitchAction.CooldownBlocked || !this.OnCooldown) && twitchAction.IsReady(this))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600C155 RID: 49493 RVA: 0x004731D8 File Offset: 0x004713D8
		public void HandleExtensionMessage(int userId, string message, bool isRerun, int creditUsed, int bitsUsed)
		{
			string text;
			if (!this.ViewerData.IdToUsername.TryGetValue(userId, out text))
			{
				return;
			}
			bool flag = false;
			if (creditUsed < 0)
			{
				creditUsed = 0;
			}
			string[] array = message.Split(' ', StringSplitOptions.None);
			string text2 = array[0];
			TwitchAction twitchAction = null;
			if (this.AvailableCommands.ContainsKey(text2))
			{
				twitchAction = this.AvailableCommands[text2];
			}
			else
			{
				foreach (TwitchAction twitchAction2 in TwitchActionManager.TwitchActions.Values)
				{
					if (twitchAction2.IsInPreset(this.CurrentActionPreset) && twitchAction2.Command == text2)
					{
						twitchAction = twitchAction2;
						break;
					}
				}
			}
			if (twitchAction == null)
			{
				return;
			}
			bool flag2 = twitchAction.PointType == TwitchAction.PointTypes.Bits;
			if (!this.VotingManager.VotingIsActive && this.twitchActive && (flag2 || (this.VoteLockedLevel != TwitchVoteLockTypes.ActionsLocked && this.AllowActions)))
			{
				if (!isRerun && !flag2)
				{
					if (!twitchAction.IgnoreCooldown)
					{
						if (this.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.CooldownType == TwitchManager.CooldownTypes.Time || this.CooldownType == TwitchManager.CooldownTypes.QuestDisabled)
						{
							return;
						}
						if ((this.CooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting || this.CooldownType == TwitchManager.CooldownTypes.SafeCooldown) && twitchAction.WaitingBlocked)
						{
							return;
						}
					}
					if (this.UseProgression && !this.OverrideProgession && twitchAction.StartGameStage != -1 && twitchAction.StartGameStage > this.HighestGameStage)
					{
						return;
					}
					if (!twitchAction.CanUse)
					{
						return;
					}
				}
				if (flag2 || isRerun || twitchAction.IgnoreCooldown || !twitchAction.CooldownBlocked || !this.OnCooldown || ((this.CooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting || this.CooldownType == TwitchManager.CooldownTypes.SafeCooldown) && !twitchAction.WaitingBlocked))
				{
					TwitchActionEntry twitchActionEntry = null;
					if ((isRerun || twitchAction.IsReady(this)) && this.ViewerData.HandleInitialActionEntrySetup(text, twitchAction, isRerun, flag2, bitsUsed, out twitchActionEntry))
					{
						twitchActionEntry.UserName = text;
						twitchActionEntry.ChannelNotify = twitchAction.PointType == TwitchAction.PointTypes.Bits;
						twitchActionEntry.IsBitAction = flag2;
						twitchActionEntry.IsReRun = isRerun;
						twitchActionEntry.Action = twitchAction;
						EntityPlayer entityPlayer = this.LocalPlayer;
						if (array.Length > 1 && this.LocalPlayer.Party != null)
						{
							string text3 = message.Substring(array[0].Length + 1).ToLower();
							for (int i = 0; i < this.LocalPlayer.Party.MemberList.Count; i++)
							{
								if (this.LocalPlayer.Party.MemberList[i].EntityName.ToLower() == text3)
								{
									entityPlayer = this.LocalPlayer.Party.MemberList[i];
									break;
								}
							}
							if (entityPlayer != this.LocalPlayer && entityPlayer.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Enabled)
							{
								if (flag2)
								{
									ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(twitchActionEntry.UserName);
									twitchActionEntry.Target = entityPlayer;
									this.AddActionHistory(twitchActionEntry, viewerEntry, TwitchActionHistoryEntry.EntryStates.Reimbursed);
									this.ShowReimburseMessage(twitchActionEntry, viewerEntry);
								}
								this.ViewerData.ReimburseAction(twitchActionEntry);
								return;
							}
							if (this.PartyInfo.ContainsKey(entityPlayer) && this.PartyInfo[entityPlayer].Cooldown > 0f)
							{
								if (flag2)
								{
									ViewerEntry viewerEntry2 = this.ViewerData.GetViewerEntry(twitchActionEntry.UserName);
									twitchActionEntry.Target = entityPlayer;
									this.AddActionHistory(twitchActionEntry, viewerEntry2, TwitchActionHistoryEntry.EntryStates.Reimbursed);
									this.ShowReimburseMessage(twitchActionEntry, viewerEntry2);
								}
								this.ViewerData.ReimburseAction(twitchActionEntry);
								return;
							}
						}
						twitchActionEntry.Target = entityPlayer;
						if (flag2)
						{
							int currentCost = twitchAction.CurrentCost;
							int creditsUsed = twitchActionEntry.CreditsUsed;
							int num = Utils.FastMax(0, currentCost - creditsUsed);
							if (bitsUsed < num)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"[Bits] Insufficient bits for '",
									text2,
									"' by ",
									text,
									". ",
									string.Format("Cost={0} CreditsSpent={1} BitsRequired={2} BitsUsed={3}. ", new object[] { currentCost, creditsUsed, num, bitsUsed }),
									"Rejecting + reimbursing credits + resyncing."
								}));
								if (creditsUsed > 0)
								{
									this.ViewerData.ReimburseAction(text, creditsUsed, twitchAction);
								}
								ViewerEntry viewerEntry3 = this.ViewerData.GetViewerEntry(text);
								if (viewerEntry3 != null)
								{
									this.PushBalanceToExtensionQueue(viewerEntry3.UserID.ToString(), viewerEntry3.BitCredits);
								}
								return;
							}
							if (creditsUsed != creditUsed)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"[Bits] Credit split mismatch for '",
									text2,
									"' by ",
									text,
									". ",
									string.Format("ClientCreditsSpent={0} ExtCreditUsed={1}. Proceeding (client-authoritative).", creditsUsed, creditUsed)
								}));
							}
							int num2 = bitsUsed - num;
							if (num2 > 0)
							{
								this.ViewerData.AddCredit(text, num2, false);
							}
						}
						if (twitchAction.ModifiedCooldown > 0f)
						{
							twitchAction.tempCooldownSet = this.CurrentUnityTime;
							twitchAction.tempCooldown = 1f;
						}
						if (twitchActionEntry.CreditsUsed > 0)
						{
							ViewerEntry viewerEntry4 = this.ViewerData.GetViewerEntry(twitchActionEntry.UserName);
							this.PushBalanceToExtensionQueue(viewerEntry4.UserID.ToString(), viewerEntry4.BitCredits);
						}
						this.QueuedActionEntries.Add(twitchActionEntry);
						flag = true;
					}
				}
			}
			if (!flag && flag2)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[Bits] Bit action '",
					text2,
					"' did not start for ",
					text,
					". Rejecting + resyncing balance (no crediting). ",
					string.Format("ExtCreditUsed={0} BitsUsed={1} Cost={2}", creditUsed, bitsUsed, twitchAction.CurrentCost)
				}));
				ViewerEntry viewerEntry5 = this.ViewerData.GetViewerEntry(text);
				if (viewerEntry5 != null)
				{
					this.PushBalanceToExtensionQueue(viewerEntry5.UserID.ToString(), viewerEntry5.BitCredits);
				}
				return;
			}
		}

		// Token: 0x0600C156 RID: 49494 RVA: 0x004737C4 File Offset: 0x004719C4
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleMessage(TwitchIRCClient.TwitchChatMessage message)
		{
			switch (message.MessageType)
			{
			case TwitchIRCClient.TwitchChatMessage.MessageTypes.Invalid:
				return;
			case TwitchIRCClient.TwitchChatMessage.MessageTypes.Message:
			{
				if (this.InitState != TwitchManager.InitStates.Ready)
				{
					return;
				}
				string text = message.Message.ToLower();
				ViewerEntry viewerEntry = this.ViewerData.UpdateViewerEntry(message.UserID, message.UserName, message.UserNameColor, message.isSub);
				if (message.isBroadcaster)
				{
					if (text.StartsWith("#cooldowninfo"))
					{
						this.ircClient.SendChannelMessage(string.Format("[7DTD]: Cooldown is at {0}/{1}.", this.CurrentCooldownFill, this.CurrentCooldownPreset.CooldownFillMax), true);
					}
					else if (text.StartsWith("#reset "))
					{
						this.actionSpawnLiveList.Clear();
						this.LiveActionEntries.Clear();
						this.ircClient.SendChannelMessage("[7DTD]: Action Live list Cleared!", true);
					}
				}
				for (int i = 0; i < this.TwitchCommandList.Count; i++)
				{
					for (int j = 0; j < this.TwitchCommandList[i].CommandTextList.Count; j++)
					{
						if (text.StartsWith(this.TwitchCommandList[i].CommandTextList[j]) && this.TwitchCommandList[i].CheckAllowed(message))
						{
							this.TwitchCommandList[i].Execute(viewerEntry, message);
							break;
						}
					}
				}
				this.VotingManager.HandleMessage(message);
				if (!this.VotingManager.VotingIsActive)
				{
					if (this.IntegrationSetting == TwitchManager.IntegrationSettings.ExtensionOnly)
					{
						return;
					}
					string[] array = text.Split(' ', StringSplitOptions.None);
					string text2 = array[0];
					if (this.AlternateCommands.ContainsKey(text2))
					{
						text2 = this.AlternateCommands[text2];
					}
					if (this.twitchActive && this.VoteLockedLevel != TwitchVoteLockTypes.ActionsLocked && this.AllowActions && this.AvailableCommands.ContainsKey(text2))
					{
						TwitchAction twitchAction = this.AvailableCommands[text2];
						if (twitchAction.PointType == TwitchAction.PointTypes.Bits)
						{
							return;
						}
						if (!twitchAction.IgnoreCooldown)
						{
							if (this.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.CooldownType == TwitchManager.CooldownTypes.Time || this.CooldownType == TwitchManager.CooldownTypes.Startup || this.CooldownType == TwitchManager.CooldownTypes.QuestDisabled)
							{
								return;
							}
							if ((this.CooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting || this.CooldownType == TwitchManager.CooldownTypes.SafeCooldown) && twitchAction.WaitingBlocked)
							{
								return;
							}
						}
						if (this.UseProgression && !this.OverrideProgession && twitchAction.StartGameStage != -1 && twitchAction.StartGameStage > this.HighestGameStage)
						{
							return;
						}
						if (!twitchAction.CanUse || !twitchAction.CheckUsable(message))
						{
							return;
						}
						if (twitchAction.IgnoreCooldown || !twitchAction.CooldownBlocked || !this.OnCooldown || ((this.CooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting || this.CooldownType == TwitchManager.CooldownTypes.SafeCooldown) && !twitchAction.WaitingBlocked))
						{
							TwitchActionEntry twitchActionEntry = null;
							if (twitchAction.IsReady(this) && this.ViewerData.HandleInitialActionEntrySetup(message.UserName, twitchAction, false, false, out twitchActionEntry))
							{
								twitchActionEntry.UserName = message.UserName;
								EntityPlayer entityPlayer = this.LocalPlayer;
								if (array.Length > 1 && this.LocalPlayer.Party != null)
								{
									int num = -1;
									if (StringParsers.TryParseSInt32(array[1], out num))
									{
										entityPlayer = this.LocalPlayer.Party.GetMemberAtIndex(num, this.LocalPlayer);
										if (entityPlayer == null)
										{
											this.ViewerData.ReimburseAction(twitchActionEntry);
											return;
										}
										if (entityPlayer.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Enabled)
										{
											this.ViewerData.ReimburseAction(twitchActionEntry);
											return;
										}
									}
									else
									{
										string text3 = text.Substring(array[0].Length + 1);
										bool flag = false;
										for (int k = 0; k < this.LocalPlayer.Party.MemberList.Count; k++)
										{
											if (this.LocalPlayer.Party.MemberList[k].EntityName.ToLower() == text3)
											{
												entityPlayer = this.LocalPlayer.Party.MemberList[k];
												flag = true;
												break;
											}
										}
										if (!flag)
										{
											this.ViewerData.ReimburseAction(twitchActionEntry);
											return;
										}
										if (entityPlayer != this.LocalPlayer && entityPlayer.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Enabled)
										{
											this.ViewerData.ReimburseAction(twitchActionEntry);
											return;
										}
										if (this.PartyInfo.ContainsKey(entityPlayer) && this.PartyInfo[entityPlayer].Cooldown > 0f)
										{
											this.ViewerData.ReimburseAction(twitchActionEntry);
											return;
										}
									}
								}
								if (twitchAction.StreamerOnly && entityPlayer != this.LocalPlayer)
								{
									this.ViewerData.ReimburseAction(twitchActionEntry);
									return;
								}
								twitchActionEntry.Target = entityPlayer;
								twitchActionEntry.Action = twitchAction;
								this.QueuedActionEntries.Add(twitchActionEntry);
								return;
							}
						}
					}
				}
				break;
			}
			case TwitchIRCClient.TwitchChatMessage.MessageTypes.Output:
				break;
			case TwitchIRCClient.TwitchChatMessage.MessageTypes.Authenticated:
				if (this.InitState != TwitchManager.InitStates.Ready)
				{
					List<string> list = new List<string>();
					list.Add("CAP REQ :twitch.tv/membership");
					list.Add("CAP REQ :twitch.tv/tags");
					list.Add("CAP REQ :twitch.tv/commands");
					this.ircClient.SendIrcMessages(list, false);
					this.InitState = TwitchManager.InitStates.CheckingForExtension;
					TwitchAuthentication.bFirstLogin = false;
					return;
				}
				break;
			case TwitchIRCClient.TwitchChatMessage.MessageTypes.Raid:
			{
				int num2 = StringParsers.ParseSInt32(message.Message, 0, -1, NumberStyles.Integer);
				this.HandleRaid(message.UserName.ToLower(), message.UserID, num2);
				return;
			}
			case TwitchIRCClient.TwitchChatMessage.MessageTypes.Charity:
			{
				int num3 = StringParsers.ParseSInt32(message.Message, 0, -1, NumberStyles.Integer);
				this.HandleCharity(message.UserName.ToLower(), message.UserID, num3);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600C157 RID: 49495 RVA: 0x00473D3C File Offset: 0x00471F3C
		public void DisplayDebug(string message)
		{
			Debug.LogWarning("Called: " + message);
			Debug.LogWarning(string.Format("[7DTD]: Spawns Alive: {0}  Blocks Alive: {1}  ActionLiveList: {2}.", this.actionSpawnLiveList.Count, this.liveBlockList.Count, this.LiveActionEntries.Count));
			for (int i = 0; i < this.actionSpawnLiveList.Count; i++)
			{
				if (this.actionSpawnLiveList[i].SpawnedEntity != null)
				{
					Debug.LogWarning(string.Format("Spawn Alive: {0}", this.actionSpawnLiveList[i].SpawnedEntity.name));
				}
			}
			for (int j = 0; j < this.LiveActionEntries.Count; j++)
			{
				Debug.LogWarning(string.Format("Action: {0} Target: {1} Viewer: {2}", this.LiveActionEntries[j].Action.Name, this.LiveActionEntries[j].Target.EntityName, this.LiveActionEntries[j].UserName));
			}
			for (int k = 0; k < this.EventQueue.Count; k++)
			{
				Debug.LogWarning(string.Format("Event: {0} User: {1} Sent: {2}", this.EventQueue[k].Event.EventTitle, this.EventQueue[k].UserName, this.EventQueue[k].IsSent));
			}
			this.ircClient.SendChannelMessage("[7DTD]: Debug Complete!", true);
		}

		// Token: 0x0600C158 RID: 49496 RVA: 0x00473EC4 File Offset: 0x004720C4
		public void AddToPot(int amount)
		{
			this.RewardPot += amount;
			if (this.RewardPot < 0)
			{
				this.RewardPot = 0;
			}
			if (this.RewardPot > TwitchManager.LeaderboardStats.LargestPimpPot)
			{
				TwitchManager.LeaderboardStats.LargestPimpPot = this.RewardPot;
			}
		}

		// Token: 0x0600C159 RID: 49497 RVA: 0x00473F14 File Offset: 0x00472114
		public void AddToBitPot(int amount)
		{
			this.BitPot += amount;
			if (this.BitPot < 0)
			{
				this.BitPot = 0;
			}
			if (this.BitPot > TwitchManager.LeaderboardStats.LargestBitPot)
			{
				TwitchManager.LeaderboardStats.LargestBitPot = this.BitPot;
			}
		}

		// Token: 0x0600C15A RID: 49498 RVA: 0x00473F64 File Offset: 0x00472164
		public void SetPot(int newPot)
		{
			if (newPot < 0)
			{
				newPot = 0;
			}
			this.RewardPot = newPot;
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_PimpPotBalance, this.RewardPot), true);
			if (this.RewardPot > TwitchManager.LeaderboardStats.LargestPimpPot)
			{
				TwitchManager.LeaderboardStats.LargestPimpPot = this.RewardPot;
			}
		}

		// Token: 0x0600C15B RID: 49499 RVA: 0x00473FC4 File Offset: 0x004721C4
		public void SetBitPot(int newPot)
		{
			if (newPot < 0)
			{
				newPot = 0;
			}
			this.BitPot = newPot;
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_BitPotBalance, this.BitPot), true);
			if (this.BitPot > TwitchManager.LeaderboardStats.LargestBitPot)
			{
				TwitchManager.LeaderboardStats.LargestBitPot = this.BitPot;
			}
		}

		// Token: 0x0600C15C RID: 49500 RVA: 0x00474024 File Offset: 0x00472224
		public void SetCooldown(float newCooldownTime, TwitchManager.CooldownTypes newCooldownType, bool displayToChannel = false, bool playCooldownSound = true)
		{
			if (this.LocalPlayer == null)
			{
				return;
			}
			if (this.CooldownType == newCooldownType && this.CooldownTime == newCooldownTime)
			{
				return;
			}
			if (newCooldownType != TwitchManager.CooldownTypes.MaxReachedWaiting && newCooldownType != TwitchManager.CooldownTypes.SafeCooldown && newCooldownType != TwitchManager.CooldownTypes.SafeCooldownExit && newCooldownType != TwitchManager.CooldownTypes.None)
			{
				this.LocalPlayer.HandleTwitchActionsTempEnabled((newCooldownTime > 15f) ? EntityPlayer.TwitchActionsStates.TempDisabled : EntityPlayer.TwitchActionsStates.TempDisabledEnding);
			}
			else if (newCooldownType == TwitchManager.CooldownTypes.None)
			{
				this.LocalPlayer.HandleTwitchActionsTempEnabled(EntityPlayer.TwitchActionsStates.Enabled);
			}
			this.CooldownType = newCooldownType;
			this.CooldownTime = newCooldownTime;
			if (displayToChannel)
			{
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_CooldownTime, newCooldownTime), true);
			}
			this.HandleCooldownActionLocking();
			if (playCooldownSound && this.LocalPlayer != null && newCooldownType != TwitchManager.CooldownTypes.None)
			{
				Manager.BroadcastPlayByLocalPlayer(this.LocalPlayer.position, "twitch_cooldown_started");
			}
		}

		// Token: 0x0600C15D RID: 49501 RVA: 0x004740EC File Offset: 0x004722EC
		public bool ForceEndCooldown(bool playEndSound = true)
		{
			if (this.IsReady && (this.CooldownType == TwitchManager.CooldownTypes.MaxReached || this.CooldownType == TwitchManager.CooldownTypes.Time))
			{
				this.SetCooldown(0f, TwitchManager.CooldownTypes.None, false, true);
				this.CurrentCooldownFill = 0f;
				if (playEndSound)
				{
					Manager.BroadcastPlayByLocalPlayer(this.LocalPlayer.position, "twitch_end_cooldown");
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600C15E RID: 49502 RVA: 0x00474148 File Offset: 0x00472348
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConfirmAction(TwitchActionEntry entry)
		{
			TwitchAction action = entry.Action;
			if (!entry.IsRespawn)
			{
				action.SetQueued();
			}
			if (!entry.IsRespawn)
			{
				if (!entry.IsBitAction && this.PimpPotType != TwitchManager.PimpPotSettings.Disabled)
				{
					int num = (int)EffectManager.GetValue(PassiveEffects.TwitchAddPimpPot, null, (float)action.ModifiedCost * this.ActionPotPercentage, this.LocalPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
					if (num > 0)
					{
						this.RewardPot += num;
						if (this.RewardPot > TwitchManager.LeaderboardStats.LargestPimpPot)
						{
							TwitchManager.LeaderboardStats.LargestPimpPot = this.RewardPot;
						}
					}
				}
				if (entry.IsBitAction && entry.BitsUsed > 0)
				{
					int num2 = (int)((float)(entry.BitsUsed - entry.CreditsUsed) * this.BitPotPercentage);
					if (num2 > 0)
					{
						this.BitPot += num2;
						if (this.BitPot > TwitchManager.LeaderboardStats.LargestBitPot)
						{
							TwitchManager.LeaderboardStats.LargestBitPot = this.BitPot;
						}
					}
				}
			}
			this.AddCooldownForAction(action);
			if (!entry.IsRespawn)
			{
				ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(entry.UserName);
				if (EffectManager.GetValue(PassiveEffects.DisableGameEventNotify, null, 0f, this.LocalPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) == 0f)
				{
					if (action.DelayNotify)
					{
						GameManager.Instance.StartCoroutine(this.onDelayedActionNotify(entry, viewerEntry));
					}
					else
					{
						this.DisplayActionNotification(entry, viewerEntry);
					}
				}
				this.AddActionHistory(entry, viewerEntry, TwitchActionHistoryEntry.EntryStates.Waiting);
			}
		}

		// Token: 0x0600C15F RID: 49503 RVA: 0x004742CC File Offset: 0x004724CC
		public void ShowReimburseMessage(string userName, int bitsUsed, TwitchAction action, ViewerEntry viewerEntry = null)
		{
			if (bitsUsed > 0)
			{
				if (viewerEntry == null)
				{
					viewerEntry = this.ViewerData.GetViewerEntry(userName);
				}
				if (viewerEntry == null)
				{
					return;
				}
				string text = string.Format(this.ingameOutput_RefundedAction, new object[] { viewerEntry.UserColor, userName, bitsUsed, action.Command });
				Debug.LogWarning(string.Format("{0} has been refunded {1} bits for {2}", userName, bitsUsed, action.Command));
				this.AddToInGameChatQueue(text, "twitch_refund");
			}
		}

		// Token: 0x0600C160 RID: 49504 RVA: 0x0047434E File Offset: 0x0047254E
		public void ShowReimburseMessage(TwitchActionEntry entry, ViewerEntry viewerEntry = null)
		{
			this.ShowReimburseMessage(entry.UserName, entry.BitsUsed, entry.Action, viewerEntry);
		}

		// Token: 0x0600C161 RID: 49505 RVA: 0x0047436C File Offset: 0x0047256C
		public TwitchActionHistoryEntry AddActionHistory(TwitchActionEntry entry, ViewerEntry viewerEntry, TwitchActionHistoryEntry.EntryStates startState = TwitchActionHistoryEntry.EntryStates.Waiting)
		{
			if (entry.IsReRun)
			{
				return null;
			}
			if (entry.HistoryEntry == null)
			{
				TwitchActionHistoryEntry twitchActionHistoryEntry = entry.SetupHistoryEntry(viewerEntry);
				twitchActionHistoryEntry.EntryState = startState;
				entry.HistoryEntry = twitchActionHistoryEntry;
				this.ActionHistory.Insert(0, twitchActionHistoryEntry);
				if (this.ActionHistory.Count > 500)
				{
					this.ActionHistory.RemoveAt(this.ActionHistory.Count - 1);
				}
				if (this.ActionHistoryAdded != null)
				{
					this.ActionHistoryAdded();
				}
				return twitchActionHistoryEntry;
			}
			entry.HistoryEntry.EntryState = startState;
			return entry.HistoryEntry;
		}

		// Token: 0x0600C162 RID: 49506 RVA: 0x00474400 File Offset: 0x00472600
		public void AddVoteHistory(TwitchVote vote)
		{
			TwitchActionHistoryEntry twitchActionHistoryEntry = new TwitchActionHistoryEntry("Vote", "FFFFFF", null, vote, null);
			this.VoteHistory.Insert(0, twitchActionHistoryEntry);
			twitchActionHistoryEntry.EntryState = TwitchActionHistoryEntry.EntryStates.Completed;
			if (this.VoteHistory.Count > 500)
			{
				this.VoteHistory.RemoveAt(this.VoteHistory.Count - 1);
			}
			if (this.VoteHistoryAdded != null)
			{
				this.VoteHistoryAdded();
			}
		}

		// Token: 0x0600C163 RID: 49507 RVA: 0x00474474 File Offset: 0x00472674
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddCooldownForAction(TwitchAction action)
		{
			if (action.AddsToCooldown)
			{
				int num = (int)EffectManager.GetValue(PassiveEffects.TwitchAddCooldown, null, (float)action.CooldownAddAmount, this.LocalPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
				if (num > 0)
				{
					this.AddCooldownAmount(num);
				}
			}
		}

		// Token: 0x0600C164 RID: 49508 RVA: 0x004744C0 File Offset: 0x004726C0
		public void AddCooldownAmount(int amount)
		{
			if (this.CurrentCooldownPreset == null)
			{
				this.GetCooldownMax();
			}
			if (this.CooldownType == TwitchManager.CooldownTypes.QuestCooldown || this.CooldownType == TwitchManager.CooldownTypes.QuestDisabled || this.CooldownType == TwitchManager.CooldownTypes.BloodMoonCooldown || this.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled)
			{
				return;
			}
			if (this.IsVoting)
			{
				return;
			}
			if (this.CurrentCooldownPreset != null && this.CurrentCooldownPreset.CooldownType == CooldownPreset.CooldownTypes.Fill)
			{
				if (this.CurrentCooldownFill < this.CurrentCooldownPreset.CooldownFillMax)
				{
					this.CurrentCooldownFill += (float)amount;
					if (this.CurrentCooldownFill >= this.CurrentCooldownPreset.CooldownFillMax)
					{
						this.SetCooldown((float)this.CurrentCooldownPreset.NextCooldownTime, TwitchManager.CooldownTypes.MaxReachedWaiting, false, true);
						if (this.ircClient != null)
						{
							this.ircClient.SendChannelMessage(this.chatOutput_CooldownStarted, true);
						}
					}
				}
				else if (this.CooldownType != TwitchManager.CooldownTypes.MaxReachedWaiting && this.CooldownType != TwitchManager.CooldownTypes.MaxReached)
				{
					this.SetCooldown((float)this.CurrentCooldownPreset.NextCooldownTime, TwitchManager.CooldownTypes.MaxReachedWaiting, false, true);
					if (this.ircClient != null)
					{
						this.ircClient.SendChannelMessage(this.chatOutput_CooldownStarted, true);
					}
				}
			}
			this.UIDirty = true;
		}

		// Token: 0x0600C165 RID: 49509 RVA: 0x004745D3 File Offset: 0x004727D3
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator onDelayedActionNotify(TwitchActionEntry entry, ViewerEntry viewerEntry)
		{
			yield return new WaitForSeconds(5f);
			if (this.ircClient != null)
			{
				this.DisplayActionNotification(entry, viewerEntry);
			}
			yield break;
		}

		// Token: 0x0600C166 RID: 49510 RVA: 0x004745F0 File Offset: 0x004727F0
		[PublicizedFrom(EAccessModifier.Private)]
		public void DisplayActionNotification(TwitchActionEntry entry, ViewerEntry viewerEntry)
		{
			if (entry.HistoryEntry != null && entry.HistoryEntry.EntryState == TwitchActionHistoryEntry.EntryStates.Reimbursed)
			{
				return;
			}
			TwitchAction action = entry.Action;
			if (entry.ChannelNotify && action.TwitchNotify)
			{
				string text;
				if (action.PointType == TwitchAction.PointTypes.Bits)
				{
					text = string.Format(this.chatOutput_ActivatedBitAction, new object[]
					{
						entry.UserName,
						action.Command,
						viewerEntry.CombinedPoints,
						entry.Target.EntityName,
						entry.Action.CurrentCost
					});
					if (action.PlayBitSound)
					{
						Manager.PlayInsidePlayerHead(action.IsPositive ? "twitch_donation" : "twitch_donation_bad", this.LocalPlayer.entityId, 0f, false, false);
					}
				}
				else
				{
					text = string.Format(this.chatOutput_ActivatedAction, new object[]
					{
						entry.UserName,
						action.Command,
						viewerEntry.CombinedPoints,
						entry.Target.EntityName
					});
				}
				this.ircClient.SendChannelMessage(text, true);
			}
			string text2 = string.Format(this.ingameOutput_ActivatedAction, new object[]
			{
				viewerEntry.UserColor,
				entry.UserName,
				action.Command,
				entry.Target.EntityName
			});
			this.SendServerChatMessage(text2);
			this.AddToInGameChatQueue(text2, null);
		}

		// Token: 0x0600C167 RID: 49511 RVA: 0x00474764 File Offset: 0x00472964
		[PublicizedFrom(EAccessModifier.Private)]
		public void SendServerChatMessage(string serverMsg)
		{
			if (this.LocalPlayer.IsInParty())
			{
				List<int> memberIdList = this.LocalPlayer.Party.GetMemberIdList(this.LocalPlayer);
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					if (memberIdList == null)
					{
						return;
					}
					using (List<int>.Enumerator enumerator = memberIdList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							int num = enumerator.Current;
							ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(num);
							if (clientInfo != null)
							{
								clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageSimpleChat>().Setup(serverMsg));
							}
						}
						return;
					}
				}
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageSimpleChat>().Setup(serverMsg, memberIdList), false);
			}
		}

		// Token: 0x0600C168 RID: 49512 RVA: 0x00474820 File Offset: 0x00472A20
		[PublicizedFrom(EAccessModifier.Private)]
		public void RefreshCommands(bool displayMessage)
		{
			if (this.LocalPlayer.unModifiedGameStage != this.HighestGameStage)
			{
				int highestGameStage = this.HighestGameStage;
				this.HighestGameStage = this.LocalPlayer.unModifiedGameStage;
				this.GetCooldownMax();
				if (this.UseProgression && !this.OverrideProgession)
				{
					this.ActionMessages.Clear();
					this.SetupAvailableCommandsWithOutput(highestGameStage, displayMessage);
					this.ResetDailyCommands(this.lastGameDay, highestGameStage);
					this.HandleCooldownActionLocking();
					this.commandsAvailable = this.AvailableCommands.Count;
				}
			}
		}

		// Token: 0x0600C169 RID: 49513 RVA: 0x004748A5 File Offset: 0x00472AA5
		public void ToggleTwitchActive()
		{
			this.twitchActive = !this.twitchActive;
			this.ActionMessages.Clear();
			this.HandleCooldownActionLocking();
		}

		// Token: 0x0600C16A RID: 49514 RVA: 0x004748C7 File Offset: 0x00472AC7
		public void SetTwitchActive(bool newActive)
		{
			if (this.twitchActive != newActive)
			{
				this.twitchActive = newActive;
				this.ActionMessages.Clear();
				this.HandleCooldownActionLocking();
			}
		}

		// Token: 0x0600C16B RID: 49515 RVA: 0x004748EC File Offset: 0x00472AEC
		public void ResetPrices()
		{
			bool flag = false;
			using (Dictionary<string, TwitchAction>.ValueCollection.Enumerator enumerator = TwitchActionManager.TwitchActions.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.UpdateCost(this.BitPriceMultiplier))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.resetCommandsNeeded = true;
			}
		}

		// Token: 0x0600C16C RID: 49516 RVA: 0x00474958 File Offset: 0x00472B58
		public void ResetPricesToDefault()
		{
			bool flag = false;
			foreach (TwitchAction twitchAction in TwitchActionManager.TwitchActions.Values)
			{
				twitchAction.ResetToDefaultCost();
				if (twitchAction.UpdateCost(this.BitPriceMultiplier))
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.resetCommandsNeeded = true;
			}
		}

		// Token: 0x0600C16D RID: 49517 RVA: 0x004749C8 File Offset: 0x00472BC8
		[PublicizedFrom(EAccessModifier.Private)]
		public void ResetCommands()
		{
			this.ActionMessages.Clear();
			this.SetupAvailableCommands();
			if (this.UseProgression && !this.OverrideProgession)
			{
				this.ResetDailyCommands(this.lastGameDay, -1);
			}
			this.HandleCooldownActionLocking();
			this.resetCommandsNeeded = false;
		}

		// Token: 0x0600C16E RID: 49518 RVA: 0x00474A05 File Offset: 0x00472C05
		public void SetUseProgression(bool useProgression)
		{
			if (this.UseProgression != useProgression)
			{
				this.UseProgression = useProgression;
				if (this.InitState == TwitchManager.InitStates.Ready)
				{
					this.resetCommandsNeeded = true;
				}
			}
		}

		// Token: 0x0600C16F RID: 49519 RVA: 0x00474A28 File Offset: 0x00472C28
		[PublicizedFrom(EAccessModifier.Private)]
		public int GetCommandCount()
		{
			int num = 0;
			if (this.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.CooldownType == TwitchManager.CooldownTypes.Time || this.CooldownType == TwitchManager.CooldownTypes.QuestDisabled)
			{
				return 0;
			}
			foreach (string text in TwitchActionManager.TwitchActions.Keys)
			{
				TwitchAction twitchAction = TwitchActionManager.TwitchActions[text];
				if (twitchAction.IsInPreset(this.CurrentActionPreset))
				{
					if (this.CooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting)
					{
						if (twitchAction.WaitingBlocked)
						{
							continue;
						}
					}
					else if (twitchAction.CooldownBlocked && this.CooldownTime > 0f)
					{
						continue;
					}
					int startGameStage = twitchAction.StartGameStage;
					if (startGameStage == -1 || startGameStage <= this.HighestGameStage)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600C170 RID: 49520 RVA: 0x00474AF8 File Offset: 0x00472CF8
		public TwitchRespawnEntry GetRespawnEntry(string username, EntityPlayer target, TwitchAction action)
		{
			for (int i = 0; i < this.RespawnEntries.Count; i++)
			{
				if (this.RespawnEntries[i].CheckRespawn(username, target, action))
				{
					return this.RespawnEntries[i];
				}
			}
			TwitchRespawnEntry twitchRespawnEntry = new TwitchRespawnEntry(username, GameEventManager.Current.Random.RandomRange(action.MinRespawnCount, action.MaxRespawnCount), target, action);
			this.RespawnEntries.Add(twitchRespawnEntry);
			return twitchRespawnEntry;
		}

		// Token: 0x0600C171 RID: 49521 RVA: 0x00474B70 File Offset: 0x00472D70
		public void CheckKiller(EntityPlayer player, EntityAlive killer, Vector3i pos)
		{
			if (this.LocalPlayer == null)
			{
				return;
			}
			if (this.twitchPlayerDeathsThisFrame.Contains(player))
			{
				return;
			}
			if (player == this.LocalPlayer && this.VotingManager != null && this.VotingManager.VotingEnabled && this.VotingManager.CurrentEvent != null)
			{
				this.HandleVoteKill(null);
				this.VotingManager.ResetVoteOnDeath();
				this.twitchPlayerDeathsThisFrame.Add(player);
				return;
			}
			bool flag;
			if (player == this.LocalPlayer)
			{
				flag = true;
			}
			else
			{
				if (this.LocalPlayer.Party == null || !this.LocalPlayer.Party.ContainsMember(player))
				{
					return;
				}
				flag = false;
			}
			TwitchActionEntry twitchActionEntry = null;
			TwitchEventActionEntry twitchEventActionEntry = null;
			TwitchVoteEntry twitchVoteEntry = null;
			if (killer == null)
			{
				int i = this.liveBlockList.Count - 1;
				while (i >= 0)
				{
					if (this.liveBlockList[i].CheckPos(pos))
					{
						TwitchSpawnedBlocksEntry twitchSpawnedBlocksEntry = this.liveBlockList[i];
						twitchActionEntry = twitchSpawnedBlocksEntry.Action;
						twitchEventActionEntry = twitchSpawnedBlocksEntry.Event;
						twitchVoteEntry = twitchSpawnedBlocksEntry.Vote;
						if (twitchSpawnedBlocksEntry.RespawnEntry != null && (flag || twitchActionEntry.Target == player))
						{
							this.RespawnEntries.Remove(twitchSpawnedBlocksEntry.RespawnEntry);
							break;
						}
						break;
					}
					else
					{
						i--;
					}
				}
			}
			else
			{
				int j = this.liveList.Count - 1;
				while (j >= 0)
				{
					if (this.liveList[j].SpawnedEntity == killer)
					{
						TwitchSpawnedEntityEntry twitchSpawnedEntityEntry = this.liveList[j];
						twitchActionEntry = twitchSpawnedEntityEntry.Action;
						twitchEventActionEntry = twitchSpawnedEntityEntry.Event;
						twitchVoteEntry = twitchSpawnedEntityEntry.Vote;
						if (twitchSpawnedEntityEntry.RespawnEntry != null && (flag || twitchSpawnedEntityEntry.Action.Target == player))
						{
							this.RespawnEntries.Remove(twitchSpawnedEntityEntry.RespawnEntry);
							break;
						}
						break;
					}
					else
					{
						j--;
					}
				}
			}
			if (twitchActionEntry == null && twitchEventActionEntry == null && twitchVoteEntry == null)
			{
				for (int k = this.recentlyDeadList.Count - 1; k >= 0; k--)
				{
					if (this.recentlyDeadList[k].SpawnedEntity == killer)
					{
						twitchActionEntry = this.recentlyDeadList[k].Action;
						twitchEventActionEntry = this.recentlyDeadList[k].Event;
						twitchVoteEntry = this.recentlyDeadList[k].Vote;
						break;
					}
				}
			}
			if (twitchActionEntry != null || twitchEventActionEntry != null)
			{
				string text = ((twitchActionEntry != null) ? twitchActionEntry.UserName : twitchEventActionEntry.UserName);
				string text2 = ((twitchActionEntry != null) ? twitchActionEntry.Action.Command : twitchEventActionEntry.Event.EventTitle);
				if (twitchEventActionEntry != null && (twitchEventActionEntry.Event.EventType == BaseTwitchEventEntry.EventTypes.HypeTrain || twitchEventActionEntry.Event.EventType == BaseTwitchEventEntry.EventTypes.CreatorGoal))
				{
					if (flag)
					{
						int num;
						bool flag2;
						if (twitchEventActionEntry.Event.EventType == BaseTwitchEventEntry.EventTypes.HypeTrain)
						{
							TwitchHypeTrainEventEntry twitchHypeTrainEventEntry = (TwitchHypeTrainEventEntry)twitchEventActionEntry.Event;
							num = twitchHypeTrainEventEntry.RewardAmount;
							flag2 = twitchHypeTrainEventEntry.RewardType > TwitchAction.PointTypes.PP;
						}
						else
						{
							TwitchCreatorGoalEventEntry twitchCreatorGoalEventEntry = (TwitchCreatorGoalEventEntry)twitchEventActionEntry.Event;
							num = twitchCreatorGoalEventEntry.RewardAmount;
							flag2 = twitchCreatorGoalEventEntry.RewardType > TwitchAction.PointTypes.PP;
						}
						this.ViewerData.AddPointsAll((!flag2) ? num : 0, flag2 ? num : 0, false);
						string text3 = (flag2 ? Localization.Get("TwitchPoints_SP", false, null) : Localization.Get("TwitchPoints_PP", false, null));
						string text4 = string.Format(this.ingameOutput_KilledByHypeTrain, num, text3, this.LocalPlayer.EntityName);
						GameManager.ShowTooltip(this.LocalPlayer, text4, false, false, 0f);
						GameManager.Instance.ChatMessageServer(null, EChatType.Global, -1, Utils.CreateGameMessage(this.Authentication.userName, text4), null, EMessageSender.None, GeneratedTextManager.BbCodeSupportMode.Supported);
						this.ircClient.SendChannelMessage(string.Format(this.chatOutput_KilledByHypeTrain, num, text3, this.LocalPlayer.EntityName), true);
						TwitchManager.DeathText = string.Format(this.ingameHypeTrainDeathScreen_Message, num, text3);
					}
					this.twitchPlayerDeathsThisFrame.Add(player);
					return;
				}
				if (flag)
				{
					ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(text);
					if ((twitchActionEntry != null && twitchActionEntry.IsBitAction) || (twitchEventActionEntry != null && twitchEventActionEntry.Event.RewardsBitPot))
					{
						if (this.BitPot > 0)
						{
							viewerEntry.BitCredits += this.BitPot;
						}
						string text5 = ((this.PimpPotType == TwitchManager.PimpPotSettings.EnabledSP) ? Localization.Get("TwitchPoints_SP", false, null) : Localization.Get("TwitchPoints_PP", false, null));
						if (this.PimpPotType != TwitchManager.PimpPotSettings.Disabled)
						{
							if (this.PimpPotType == TwitchManager.PimpPotSettings.EnabledSP)
							{
								viewerEntry.SpecialPoints += (float)this.RewardPot;
							}
							else
							{
								viewerEntry.StandardPoints += (float)this.RewardPot;
							}
						}
						this.ircClient.SendChannelMessage(string.Format(this.chatOutput_KilledByBits, new object[] { text, viewerEntry.BitCredits, this.BitPot, player.EntityName, this.RewardPot, text5 }), true);
						string text6 = string.Format(this.ingameOutput_KilledByBits, new object[] { viewerEntry.UserColor, text, this.BitPot, player.EntityName, this.RewardPot, text5 });
						GameManager.ShowTooltip(this.LocalPlayer, text6, false, false, 0f);
						GameManager.Instance.ChatMessageServer(null, EChatType.Global, -1, Utils.CreateGameMessage(this.Authentication.userName, text6), null, EMessageSender.None, GeneratedTextManager.BbCodeSupportMode.Supported);
						TwitchManager.DeathText = string.Format(this.ingameBitsDeathScreen_Message, new object[] { viewerEntry.UserColor, text, text2, this.BitPot, this.RewardPot, text5 });
						QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.BitPot, "");
						this.BitPot = 0;
						this.RewardPot = TwitchManager.PimpPotDefault;
					}
					else
					{
						string text7 = ((this.PimpPotType == TwitchManager.PimpPotSettings.EnabledSP) ? Localization.Get("TwitchPoints_SP", false, null) : Localization.Get("TwitchPoints_PP", false, null));
						if (this.PimpPotType != TwitchManager.PimpPotSettings.Disabled)
						{
							if (this.PimpPotType == TwitchManager.PimpPotSettings.EnabledSP)
							{
								viewerEntry.SpecialPoints += (float)this.RewardPot;
							}
							else
							{
								viewerEntry.StandardPoints += (float)this.RewardPot;
							}
							this.ircClient.SendChannelMessage(string.Format(this.chatOutput_KilledStreamer, new object[] { text, viewerEntry.CombinedPoints, this.RewardPot, text7, player.EntityName }), true);
							string text8 = string.Format(this.ingameOutput_KilledStreamer, new object[] { viewerEntry.UserColor, text, this.RewardPot, text7, player.EntityName });
							GameManager.ShowTooltip(TwitchManager.Current.LocalPlayer, text8, false, false, 0f);
							GameManager.Instance.ChatMessageServer(null, EChatType.Global, -1, Utils.CreateGameMessage(this.Authentication.userName, text8), null, EMessageSender.None, GeneratedTextManager.BbCodeSupportMode.Supported);
							QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.PimpPot, "");
						}
						TwitchManager.DeathText = string.Format(this.ingameDeathScreen_Message, new object[] { viewerEntry.UserColor, text, text2, this.RewardPot, text7 });
						this.RewardPot = TwitchManager.PimpPotDefault;
					}
					TwitchManager.LeaderboardStats.CheckTopKiller(TwitchManager.LeaderboardStats.AddKill(text, viewerEntry.UserColor));
					this.AddKillToLeaderboard(text, viewerEntry.UserColor);
					this.twitchPlayerDeathsThisFrame.Add(player);
				}
				else
				{
					if (this.PimpPotType != TwitchManager.PimpPotSettings.Disabled)
					{
						ViewerEntry viewerEntry2 = this.ViewerData.GetViewerEntry(text);
						int num2 = Mathf.Min(this.PartyKillRewardMax, this.RewardPot);
						viewerEntry2.StandardPoints += (float)num2;
						this.ircClient.SendChannelMessage(string.Format(this.chatOutput_KilledParty, new object[] { text, viewerEntry2.CombinedPoints, num2, player.EntityName }), true);
						string text9 = string.Format(this.ingameOutput_KilledParty, new object[] { viewerEntry2.UserColor, text, num2, player.EntityName });
						GameManager.ShowTooltip(TwitchManager.Current.LocalPlayer, text9, false, false, 0f);
						GameManager.Instance.ChatMessageServer(null, EChatType.Global, -1, Utils.CreateGameMessage(this.Authentication.userName, text9), null, EMessageSender.None, GeneratedTextManager.BbCodeSupportMode.Supported);
					}
					this.twitchPlayerDeathsThisFrame.Add(player);
				}
			}
			else if (twitchVoteEntry != null && flag && this.VotingManager != null && !twitchVoteEntry.Complete)
			{
				this.HandleVoteKill(twitchVoteEntry);
			}
			if (flag)
			{
				this.VotingManager.ResetVoteOnDeath();
			}
		}

		// Token: 0x0600C172 RID: 49522 RVA: 0x00475464 File Offset: 0x00473664
		[PublicizedFrom(EAccessModifier.Protected)]
		public void HandleVoteKill(TwitchVoteEntry voteEntry)
		{
			List<string> list = this.VotingManager.HandleKiller(voteEntry);
			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.ViewerData.GetViewerEntry(list[i]).StandardPoints += (float)this.VotingManager.ViewerDefeatReward;
				}
				string text = string.Format(this.ingameOutput_KilledByVote, this.VotingManager.ViewerDefeatReward, this.LocalPlayer.EntityName);
				GameManager.ShowTooltip(TwitchManager.Current.LocalPlayer, text, false, false, 0f);
				GameManager.Instance.ChatMessageServer(null, EChatType.Global, -1, Utils.CreateGameMessage(this.Authentication.userName, text), null, EMessageSender.None, GeneratedTextManager.BbCodeSupportMode.Supported);
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_KilledByVote, this.VotingManager.ViewerDefeatReward, this.LocalPlayer.EntityName), true);
				TwitchManager.DeathText = string.Format(this.ingameVoteDeathScreen_Message, this.VotingManager.ViewerDefeatReward);
				list.Clear();
			}
			if (voteEntry != null)
			{
				voteEntry.Complete = true;
			}
		}

		// Token: 0x0600C173 RID: 49523 RVA: 0x0047558C File Offset: 0x0047378C
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddKillToLeaderboard(string username, string usercolor)
		{
			bool flag = false;
			for (int i = 0; i < this.Leaderboard.Count; i++)
			{
				if (this.Leaderboard[i].UserName == username)
				{
					this.Leaderboard[i].Kills++;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.Leaderboard.Add(new TwitchLeaderboardEntry(username, usercolor, 1));
			}
		}

		// Token: 0x0600C174 RID: 49524 RVA: 0x004755FD File Offset: 0x004737FD
		public void ClearLeaderboard()
		{
			this.Leaderboard.Clear();
		}

		// Token: 0x0600C175 RID: 49525 RVA: 0x0047560A File Offset: 0x0047380A
		public void AddCooldownPreset(CooldownPreset preset)
		{
			if (this.CooldownPresets == null)
			{
				this.CooldownPresets = new List<CooldownPreset>();
			}
			if (preset.IsDefault)
			{
				this.CooldownPresetIndex = this.CooldownPresets.Count;
			}
			this.CooldownPresets.Add(preset);
		}

		// Token: 0x0600C176 RID: 49526 RVA: 0x00475644 File Offset: 0x00473844
		public void SetCooldownPreset(int index)
		{
			if (this.InitState == TwitchManager.InitStates.Ready)
			{
				bool flag = this.CurrentCooldownPreset.CooldownType != this.CooldownPresets[index].CooldownType;
				this.CooldownPresetIndex = index;
				this.GetCooldownMax();
				if (flag)
				{
					this.resetCommandsNeeded = true;
					return;
				}
			}
			else
			{
				this.CooldownPresetIndex = index;
			}
		}

		// Token: 0x0600C177 RID: 49527 RVA: 0x0047569C File Offset: 0x0047389C
		public void SetToDefaultCooldown()
		{
			for (int i = 0; i < this.CooldownPresets.Count; i++)
			{
				if (this.CooldownPresets[i].IsDefault)
				{
					this.SetCooldownPreset(i);
					return;
				}
			}
		}

		// Token: 0x0600C178 RID: 49528 RVA: 0x004756DA File Offset: 0x004738DA
		public void GetCooldownMax()
		{
			this.CurrentCooldownPreset = this.CooldownPresets[this.CooldownPresetIndex];
			this.CurrentCooldownPreset.SetupCooldownInfo(this.HighestGameStage, this.LocalPlayer);
			this.SetupBloodMoonData();
		}

		// Token: 0x0600C179 RID: 49529 RVA: 0x00475710 File Offset: 0x00473910
		public void AddTwitchActionPreset(TwitchActionPreset preset)
		{
			if (this.ActionPresets == null)
			{
				this.ActionPresets = new List<TwitchActionPreset>();
			}
			this.ActionPresets.Add(preset);
			if (preset.IsDefault)
			{
				this.ActionPresetIndex = this.ActionPresets.Count - 1;
				this.CurrentActionPreset = this.ActionPresets[this.ActionPresetIndex];
			}
		}

		// Token: 0x0600C17A RID: 49530 RVA: 0x00475770 File Offset: 0x00473970
		public void SetTwitchActionPreset(int index)
		{
			if (this.ActionPresetIndex == index)
			{
				return;
			}
			this.ActionPresets[this.ActionPresetIndex].AddedActions.Clear();
			this.ActionPresets[this.ActionPresetIndex].RemovedActions.Clear();
			this.ActionPresetIndex = index;
			this.CurrentActionPreset = this.ActionPresets[this.ActionPresetIndex];
			this.CurrentActionPreset.HandleCooldowns();
			if (this.InitState == TwitchManager.InitStates.Ready)
			{
				this.resetCommandsNeeded = true;
			}
		}

		// Token: 0x0600C17B RID: 49531 RVA: 0x004757F8 File Offset: 0x004739F8
		public void SetToDefaultActionPreset()
		{
			for (int i = 0; i < this.ActionPresets.Count; i++)
			{
				if (this.ActionPresets[i].IsDefault)
				{
					this.SetTwitchActionPreset(i);
					return;
				}
			}
		}

		// Token: 0x0600C17C RID: 49532 RVA: 0x00475838 File Offset: 0x00473A38
		public void AddTwitchVotePreset(TwitchVotePreset preset)
		{
			if (this.VotePresets == null)
			{
				this.VotePresets = new List<TwitchVotePreset>();
			}
			this.VotePresets.Add(preset);
			if (preset.IsDefault)
			{
				this.VotePresetIndex = this.VotePresets.Count - 1;
				this.CurrentVotePreset = this.VotePresets[this.VotePresetIndex];
			}
		}

		// Token: 0x0600C17D RID: 49533 RVA: 0x00475898 File Offset: 0x00473A98
		public void SetTwitchVotePreset(int index)
		{
			if (this.VotePresetIndex == index)
			{
				return;
			}
			this.VotePresetIndex = index;
			this.CurrentVotePreset = this.VotePresets[this.VotePresetIndex];
			if (this.CurrentVotePreset.IsEmpty)
			{
				this.VotingManager.ForceEndVote();
			}
			this.SetupAvailableCommands();
		}

		// Token: 0x0600C17E RID: 49534 RVA: 0x004758EC File Offset: 0x00473AEC
		public void SetToDefaultVotePreset()
		{
			for (int i = 0; i < this.VotePresets.Count; i++)
			{
				if (this.VotePresets[i].IsDefault)
				{
					this.SetTwitchVotePreset(i);
					return;
				}
			}
		}

		// Token: 0x0600C17F RID: 49535 RVA: 0x0047592C File Offset: 0x00473B2C
		public void AddTwitchEventPreset(TwitchEventPreset preset)
		{
			if (this.EventPresets == null)
			{
				this.EventPresets = new List<TwitchEventPreset>();
			}
			this.EventPresets.Add(preset);
			if (preset.IsDefault)
			{
				this.EventPresetIndex = this.EventPresets.Count - 1;
				this.CurrentEventPreset = this.EventPresets[this.EventPresetIndex];
			}
		}

		// Token: 0x0600C180 RID: 49536 RVA: 0x0047598C File Offset: 0x00473B8C
		public void SetTwitchEventPreset(int index, bool oldAllowChannelPointRedeems)
		{
			TwitchEventPreset currentEventPreset = this.CurrentEventPreset;
			this.EventPresetIndex = index;
			this.CurrentEventPreset = this.EventPresets[this.EventPresetIndex];
			if (currentEventPreset != null)
			{
				currentEventPreset.RemoveChannelPointRedemptions(this.AllowChannelPointRedemptions ? this.CurrentEventPreset : null);
			}
			this.SetupTwitchCommands();
			if (this.AllowChannelPointRedemptions && this.CurrentEventPreset != null)
			{
				this.CurrentEventPreset.AddChannelPointRedemptions();
			}
		}

		// Token: 0x0600C181 RID: 49537 RVA: 0x004759FC File Offset: 0x00473BFC
		public TwitchEventPreset GetEventPreset(string name)
		{
			for (int i = 0; i < this.EventPresets.Count; i++)
			{
				if (this.EventPresets[i].Name.EqualsCaseInsensitive(name))
				{
					return this.EventPresets[i];
				}
			}
			return null;
		}

		// Token: 0x0600C182 RID: 49538 RVA: 0x00475A48 File Offset: 0x00473C48
		public void SetToDefaultEventPreset()
		{
			for (int i = 0; i < this.EventPresets.Count; i++)
			{
				if (this.EventPresets[i].IsDefault)
				{
					this.SetTwitchEventPreset(i, this.AllowChannelPointRedemptions);
					return;
				}
			}
		}

		// Token: 0x0600C183 RID: 49539 RVA: 0x00475A8C File Offset: 0x00473C8C
		[PublicizedFrom(EAccessModifier.Private)]
		public void EventSub_OnSubscriptionRedeemed(SubscriptionEventBase e)
		{
			if (e.UserName == null)
			{
				return;
			}
			string text = e.UserName.ToLower();
			TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.GetSubTier(e.Tier);
			ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(text);
			viewerEntry.UserID = StringParsers.ParseSInt32(e.UserId, 0, -1, NumberStyles.Integer);
			int num = this.ViewerData.GetSubTierPoints(subTier) * this.SubPointModifier;
			if (num > 0)
			{
				viewerEntry.SpecialPoints += (float)num;
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_Subscribed, new object[]
				{
					text,
					viewerEntry.CombinedPoints,
					this.GetTierName(subTier),
					num
				}), true);
				string text2 = string.Format(this.ingameOutput_Subscribed, text, this.GetTierName(subTier), num);
				this.AddToInGameChatQueue(text2, null);
			}
			SubscriptionMessageEvent subscriptionMessageEvent = e as SubscriptionMessageEvent;
			if (subscriptionMessageEvent != null)
			{
				this.HandleSubEvent(text, subscriptionMessageEvent.CumulativeMonths, subTier);
				return;
			}
			this.HandleSubEvent(text, 1, subTier);
		}

		// Token: 0x0600C184 RID: 49540 RVA: 0x00475B90 File Offset: 0x00473D90
		public void EventSub_OnSubGifted(SubscriptionGiftEvent e)
		{
			if (e.UserName != null && !e.IsAnonymous)
			{
				this.ViewerData.AddGiftSubEntry(e.UserName.ToLower(), StringParsers.ParseSInt32(e.UserId, 0, -1, NumberStyles.Integer), TwitchSubEventEntry.GetSubTier(e.Tier), e.Total);
			}
		}

		// Token: 0x0600C185 RID: 49541 RVA: 0x00475BE2 File Offset: 0x00473DE2
		public string GetTierName(TwitchSubEventEntry.SubTierTypes tier)
		{
			switch (tier)
			{
			case TwitchSubEventEntry.SubTierTypes.Prime:
				return "Prime";
			case TwitchSubEventEntry.SubTierTypes.Tier1:
				return "1";
			case TwitchSubEventEntry.SubTierTypes.Tier2:
				return "2";
			case TwitchSubEventEntry.SubTierTypes.Tier3:
				return "3";
			default:
				return "1";
			}
		}

		// Token: 0x0600C186 RID: 49542 RVA: 0x00475C1C File Offset: 0x00473E1C
		public string GetSubTierRewards(int subModifier)
		{
			if (subModifier == 0)
			{
				return Localization.Get("xuiLightPropShadowsNone", false, null);
			}
			return string.Format(this.subPointDisplay, this.ViewerData.GetSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier1) * subModifier, this.ViewerData.GetSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier2) * subModifier, this.ViewerData.GetSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier3) * subModifier);
		}

		// Token: 0x0600C187 RID: 49543 RVA: 0x00475C80 File Offset: 0x00473E80
		public string GetGiftSubTierRewards(int subModifier)
		{
			if (subModifier == 0)
			{
				return Localization.Get("xuiLightPropShadowsNone", false, null);
			}
			return string.Format(this.subPointDisplay, this.ViewerData.GetGiftSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier1) * subModifier, this.ViewerData.GetGiftSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier2) * subModifier, this.ViewerData.GetGiftSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier3) * subModifier);
		}

		// Token: 0x0600C188 RID: 49544 RVA: 0x00475CE4 File Offset: 0x00473EE4
		public void HandleSubEvent(string username, int months, TwitchSubEventEntry.SubTierTypes tier)
		{
			if (!this.AllowEvents)
			{
				return;
			}
			TwitchSubEventEntry twitchSubEventEntry = this.CurrentEventPreset.HandleSubEvent(months, tier);
			if (twitchSubEventEntry != null)
			{
				ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(username);
				TwitchEventActionEntry twitchEventActionEntry = new TwitchEventActionEntry();
				twitchEventActionEntry.UserName = username;
				twitchEventActionEntry.Event = twitchSubEventEntry;
				this.EventQueue.Add(twitchEventActionEntry);
				twitchEventActionEntry.Event.HandleInstant(username, this);
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_SubEvent, twitchSubEventEntry.EventTitle, username, viewerEntry.CombinedPoints), true);
			}
		}

		// Token: 0x0600C189 RID: 49545 RVA: 0x00475D70 File Offset: 0x00473F70
		public void HandleGiftSubEvent(string username, int giftCounts, TwitchSubEventEntry.SubTierTypes tier)
		{
			if (!this.AllowEvents)
			{
				return;
			}
			TwitchSubEventEntry twitchSubEventEntry = this.CurrentEventPreset.HandleGiftSubEvent(giftCounts, tier);
			if (twitchSubEventEntry != null)
			{
				ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(username);
				TwitchEventActionEntry twitchEventActionEntry = new TwitchEventActionEntry();
				twitchEventActionEntry.UserName = username;
				twitchEventActionEntry.Event = twitchSubEventEntry;
				this.EventQueue.Add(twitchEventActionEntry);
				twitchEventActionEntry.Event.HandleInstant(username, this);
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_GiftSubEvent, twitchSubEventEntry.EventTitle, username, viewerEntry.CombinedPoints), true);
			}
		}

		// Token: 0x0600C18A RID: 49546 RVA: 0x00475DFC File Offset: 0x00473FFC
		[PublicizedFrom(EAccessModifier.Private)]
		public void EventSubMessageReceived(JObject payload)
		{
			JToken jtoken = payload["subscription"];
			string text;
			if (jtoken == null)
			{
				text = null;
			}
			else
			{
				JToken jtoken2 = jtoken["type"];
				text = ((jtoken2 != null) ? jtoken2.ToString() : null);
			}
			string text2 = text ?? "unknown";
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text2);
			if (num <= 765010691U)
			{
				if (num <= 303371570U)
				{
					if (num != 62361312U)
					{
						if (num == 303371570U)
						{
							if (text2 == "channel.subscribe")
							{
								SubscriptionEvent subscriptionEvent = payload["event"].ToObject<SubscriptionEvent>();
								if (subscriptionEvent != null && !subscriptionEvent.IsGift)
								{
									this.EventSub_OnSubscriptionRedeemed(subscriptionEvent);
									return;
								}
								return;
							}
						}
					}
					else if (text2 == "channel.hype_train.end")
					{
						this.EndHypeTrain();
						return;
					}
				}
				else if (num != 336644660U)
				{
					if (num == 765010691U)
					{
						if (text2 == "channel.bits.use")
						{
							BitsUsedEvent bitsUsedEvent = payload["event"].ToObject<BitsUsedEvent>();
							if (bitsUsedEvent != null)
							{
								this.EventSub_OnBitsRedeemed(bitsUsedEvent);
								return;
							}
							return;
						}
					}
				}
				else if (text2 == "channel.hype_train.begin")
				{
					if (this.HypeTrainLevel == 0)
					{
						this.StartHypeTrain();
						return;
					}
					return;
				}
			}
			else if (num <= 2096046674U)
			{
				if (num != 1906819046U)
				{
					if (num == 2096046674U)
					{
						if (text2 == "channel.raid")
						{
							RaidEvent raidEvent = payload["event"].ToObject<RaidEvent>();
							int num2;
							if (int.TryParse(raidEvent.RaiderID, out num2))
							{
								this.HandleRaid(raidEvent.RaiderUserName.ToLower(), num2, raidEvent.viewerCount);
								return;
							}
							Log.Warning("Failed to run Raid Event because RaiderID could not be parsed as an int");
							return;
						}
					}
				}
				else if (text2 == "channel.subscription.message")
				{
					SubscriptionMessageEvent subscriptionMessageEvent = payload["event"].ToObject<SubscriptionMessageEvent>();
					if (subscriptionMessageEvent != null)
					{
						this.EventSub_OnSubscriptionRedeemed(subscriptionMessageEvent);
						return;
					}
					return;
				}
			}
			else if (num != 2529875513U)
			{
				if (num != 3677527194U)
				{
					if (num == 3760536684U)
					{
						if (text2 == "channel.hype_train.progress")
						{
							HypeTrainProgressEvent hypeTrainProgressEvent = payload["event"].ToObject<HypeTrainProgressEvent>();
							if (this.HypeTrainLevel < hypeTrainProgressEvent.Level)
							{
								this.IncrementHypeTrainLevel();
								return;
							}
							return;
						}
					}
				}
				else if (text2 == "channel.channel_points_custom_reward_redemption.add")
				{
					ChannelPointsRedemptionEvent channelPointsRedemptionEvent = payload["event"].ToObject<ChannelPointsRedemptionEvent>();
					if (channelPointsRedemptionEvent != null)
					{
						this.EventSub_OnChannelPointsRedeemed(channelPointsRedemptionEvent);
						return;
					}
					return;
				}
			}
			else if (text2 == "channel.subscription.gift")
			{
				SubscriptionGiftEvent subscriptionGiftEvent = payload["event"].ToObject<SubscriptionGiftEvent>();
				this.EventSub_OnSubGifted(subscriptionGiftEvent);
				return;
			}
			Log.Warning("Unhandled event: " + text2);
		}

		// Token: 0x0600C18B RID: 49547 RVA: 0x004760D0 File Offset: 0x004742D0
		[PublicizedFrom(EAccessModifier.Private)]
		public void PubSub_OnBitsRedeemed(object sender, PubSubBitRedemptionMessage.BitRedemptionData e)
		{
			if (e.user_name == null)
			{
				return;
			}
			ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(e.user_name);
			int num = e.bits_used * this.BitPointModifier;
			viewerEntry.UserID = StringParsers.ParseSInt32(e.user_id, 0, -1, NumberStyles.Integer);
			viewerEntry.SpecialPoints += (float)num;
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_DonateBits, new object[] { e.user_name, viewerEntry.CombinedPoints, e.bits_used, num }), true);
			string text = string.Format(this.ingameOutput_DonateBits, e.user_name, e.bits_used, num);
			this.AddToInGameChatQueue(text, null);
			this.HandleBitRedeem(e.user_name, e.bits_used, viewerEntry);
		}

		// Token: 0x0600C18C RID: 49548 RVA: 0x004761B4 File Offset: 0x004743B4
		[PublicizedFrom(EAccessModifier.Private)]
		public void EventSub_OnBitsRedeemed(BitsUsedEvent e)
		{
			if (e.UserName == null)
			{
				return;
			}
			string text = e.UserName.ToLower();
			ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(text);
			int num = e.Bits * this.BitPointModifier;
			viewerEntry.UserID = StringParsers.ParseSInt32(e.UserId, 0, -1, NumberStyles.Integer);
			viewerEntry.SpecialPoints += (float)num;
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_DonateBits, new object[] { text, viewerEntry.CombinedPoints, e.Bits, num }), true);
			string text2 = string.Format(this.ingameOutput_DonateBits, text, e.Bits, num);
			this.AddToInGameChatQueue(text2, null);
			this.HandleBitRedeem(text, e.Bits, viewerEntry);
		}

		// Token: 0x0600C18D RID: 49549 RVA: 0x00476290 File Offset: 0x00474490
		public void HandleBitRedeem(string userName, int bitAmount, ViewerEntry viewerEntry = null)
		{
			if (!this.AllowEvents)
			{
				return;
			}
			TwitchEventEntry twitchEventEntry = this.CurrentEventPreset.HandleBitRedeem(bitAmount);
			if (twitchEventEntry != null)
			{
				if (viewerEntry == null)
				{
					viewerEntry = this.ViewerData.GetViewerEntry(userName);
				}
				TwitchEventActionEntry twitchEventActionEntry = new TwitchEventActionEntry();
				twitchEventActionEntry.UserName = userName;
				twitchEventActionEntry.Event = twitchEventEntry;
				this.EventQueue.Add(twitchEventActionEntry);
				twitchEventActionEntry.Event.HandleInstant(userName, this);
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_BitEvent, twitchEventEntry.EventTitle, userName, viewerEntry.CombinedPoints), true);
			}
		}

		// Token: 0x0600C18E RID: 49550 RVA: 0x0047631D File Offset: 0x0047451D
		[PublicizedFrom(EAccessModifier.Private)]
		public void PubSub_OnChannelPointsRedeemed(object sender, PubSubChannelPointMessage.ChannelRedemptionData e)
		{
			this.HandleChannelPointsRedeem(e.redemption.reward.title, e.redemption.user.display_name.ToLower());
		}

		// Token: 0x0600C18F RID: 49551 RVA: 0x0047634A File Offset: 0x0047454A
		[PublicizedFrom(EAccessModifier.Private)]
		public void EventSub_OnChannelPointsRedeemed(ChannelPointsRedemptionEvent e)
		{
			this.HandleChannelPointsRedeem(e.Reward.Title, e.UserLogin.ToLower());
		}

		// Token: 0x0600C190 RID: 49552 RVA: 0x00476368 File Offset: 0x00474568
		public void HandleChannelPointsRedeem(string title, string userName)
		{
			if (!this.AllowEvents)
			{
				return;
			}
			TwitchChannelPointEventEntry twitchChannelPointEventEntry = this.CurrentEventPreset.HandleChannelPointsRedeem(title);
			if (twitchChannelPointEventEntry != null)
			{
				ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(userName);
				TwitchEventActionEntry twitchEventActionEntry = new TwitchEventActionEntry();
				twitchEventActionEntry.UserName = userName;
				twitchEventActionEntry.Event = twitchChannelPointEventEntry;
				this.EventQueue.Add(twitchEventActionEntry);
				twitchEventActionEntry.Event.HandleInstant(userName, this);
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_ChannelPointEvent, twitchChannelPointEventEntry.EventTitle, userName, viewerEntry.CombinedPoints), true);
				QuestEventManager.Current.TwitchEventReceived(TwitchObjectiveTypes.ChannelPointRedeems, twitchChannelPointEventEntry.EventName);
			}
		}

		// Token: 0x0600C191 RID: 49553 RVA: 0x00476404 File Offset: 0x00474604
		public void HandleRaid(string userName, int userID, int viewerAmount)
		{
			ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(userName);
			if (viewerAmount >= this.RaidViewerMinimum && this.RaidPointAdd > 0)
			{
				viewerEntry.UserID = userID;
				viewerEntry.SpecialPoints += (float)this.RaidPointAdd;
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_RaidPoints, new object[] { userName, viewerEntry.CombinedPoints, viewerAmount, this.RaidPointAdd }), true);
				string text = string.Format(this.ingameOutput_RaidPoints, userName, viewerAmount, this.RaidPointAdd);
				this.AddToInGameChatQueue(text, null);
			}
			this.HandleRaidRedeem(userName, viewerAmount, viewerEntry);
		}

		// Token: 0x0600C192 RID: 49554 RVA: 0x004764C8 File Offset: 0x004746C8
		public void HandleRaidRedeem(string userName, int viewerAmount, ViewerEntry viewerEntry = null)
		{
			if (!this.AllowEvents)
			{
				return;
			}
			TwitchEventEntry twitchEventEntry = this.CurrentEventPreset.HandleRaid(viewerAmount);
			if (twitchEventEntry != null)
			{
				if (viewerEntry == null)
				{
					viewerEntry = this.ViewerData.GetViewerEntry(userName);
				}
				TwitchEventActionEntry twitchEventActionEntry = new TwitchEventActionEntry();
				twitchEventActionEntry.UserName = userName;
				twitchEventActionEntry.Event = twitchEventEntry;
				this.EventQueue.Add(twitchEventActionEntry);
				twitchEventActionEntry.Event.HandleInstant(userName, this);
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_RaidEvent, new object[] { twitchEventEntry.EventTitle, userName, viewerEntry.CombinedPoints, viewerAmount }), true);
			}
		}

		// Token: 0x0600C193 RID: 49555 RVA: 0x00476570 File Offset: 0x00474770
		public void HandleCharity(string userName, int userID, int charityAmount)
		{
			ViewerEntry viewerEntry = this.ViewerData.GetViewerEntry(userName);
			int num = charityAmount * this.BitPointModifier;
			viewerEntry.UserID = userID;
			viewerEntry.SpecialPoints += (float)num;
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_DonateCharity, new object[] { userName, viewerEntry.CombinedPoints, charityAmount, num }), true);
			string text = string.Format(this.ingameOutput_DonateCharity, userName, charityAmount, num);
			this.AddToInGameChatQueue(text, null);
			this.HandleCharityRedeem(userName, charityAmount, viewerEntry);
		}

		// Token: 0x0600C194 RID: 49556 RVA: 0x00476614 File Offset: 0x00474814
		public void HandleCharityRedeem(string userName, int charityAmount, ViewerEntry viewerEntry = null)
		{
			if (!this.AllowEvents)
			{
				return;
			}
			TwitchEventEntry twitchEventEntry = this.CurrentEventPreset.HandleCharityRedeem(charityAmount);
			if (twitchEventEntry != null)
			{
				if (viewerEntry == null)
				{
					viewerEntry = this.ViewerData.GetViewerEntry(userName);
				}
				TwitchEventActionEntry twitchEventActionEntry = new TwitchEventActionEntry();
				twitchEventActionEntry.UserName = userName;
				twitchEventActionEntry.Event = twitchEventEntry;
				this.EventQueue.Add(twitchEventActionEntry);
				twitchEventActionEntry.Event.HandleInstant(userName, this);
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_CharityEvent, twitchEventEntry.EventTitle, userName, viewerEntry.CombinedPoints), true);
			}
		}

		// Token: 0x0600C195 RID: 49557 RVA: 0x004766A1 File Offset: 0x004748A1
		public void StartHypeTrain()
		{
			this.HypeTrainLevel = 1;
			this.HandleHypeTrainRedeem(this.HypeTrainLevel);
		}

		// Token: 0x0600C196 RID: 49558 RVA: 0x004766B6 File Offset: 0x004748B6
		public void IncrementHypeTrainLevel()
		{
			this.HypeTrainLevel++;
			this.HandleHypeTrainRedeem(this.HypeTrainLevel);
		}

		// Token: 0x0600C197 RID: 49559 RVA: 0x004766D2 File Offset: 0x004748D2
		public void EndHypeTrain()
		{
			this.HypeTrainLevel = 0;
		}

		// Token: 0x0600C198 RID: 49560 RVA: 0x004766DC File Offset: 0x004748DC
		public void HandleHypeTrainRedeem(int hypeTrainLevel)
		{
			if (!this.AllowEvents)
			{
				return;
			}
			TwitchEventEntry twitchEventEntry = this.CurrentEventPreset.HandleHypeTrainRedeem(hypeTrainLevel);
			if (twitchEventEntry != null)
			{
				TwitchEventActionEntry twitchEventActionEntry = new TwitchEventActionEntry();
				twitchEventActionEntry.UserName = " ";
				twitchEventActionEntry.Event = twitchEventEntry;
				this.EventQueue.Add(twitchEventActionEntry);
				twitchEventActionEntry.Event.HandleInstant(twitchEventActionEntry.UserName, this);
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_HypeTrainEvent, twitchEventEntry.EventTitle, hypeTrainLevel), true);
			}
		}

		// Token: 0x0600C199 RID: 49561 RVA: 0x0047675B File Offset: 0x0047495B
		[PublicizedFrom(EAccessModifier.Private)]
		public void PubSub_OnGoalAchieved(object sender, PubSubGoalMessage.Goal e)
		{
			this.HandleCreatorGoalRedeem(e.contributionType.ToLower());
		}

		// Token: 0x0600C19A RID: 49562 RVA: 0x00476770 File Offset: 0x00474970
		public void HandleCreatorGoalRedeem(string goalType)
		{
			if (!this.AllowEvents)
			{
				return;
			}
			TwitchCreatorGoalEventEntry twitchCreatorGoalEventEntry = this.CurrentEventPreset.HandleCreatorGoalEvent(goalType);
			if (twitchCreatorGoalEventEntry != null)
			{
				TwitchEventActionEntry twitchEventActionEntry = new TwitchEventActionEntry();
				twitchEventActionEntry.UserName = " ";
				twitchEventActionEntry.Event = twitchCreatorGoalEventEntry;
				this.EventQueue.Add(twitchEventActionEntry);
				twitchEventActionEntry.Event.HandleInstant(twitchEventActionEntry.UserName, this);
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_CreatorGoalEvent, twitchCreatorGoalEventEntry.EventTitle), true);
			}
		}

		// Token: 0x0600C19B RID: 49563 RVA: 0x004767EC File Offset: 0x004749EC
		public void HandleEventQueue()
		{
			if (!this.twitchActive)
			{
				return;
			}
			for (int i = 0; i < this.EventQueue.Count; i++)
			{
				TwitchEventActionEntry twitchEventActionEntry = this.EventQueue[i];
				if (!twitchEventActionEntry.IsSent && twitchEventActionEntry.HandleEvent(this))
				{
					Manager.BroadcastPlayByLocalPlayer(this.LocalPlayer.position, "twitch_custom_event");
					if (!twitchEventActionEntry.IsRetry)
					{
						TwitchActionHistoryEntry twitchActionHistoryEntry = new TwitchActionHistoryEntry(twitchEventActionEntry.UserName, "FFFFFF", null, null, twitchEventActionEntry);
						twitchActionHistoryEntry.EventEntry = twitchEventActionEntry;
						twitchEventActionEntry.HistoryEntry = twitchActionHistoryEntry;
						this.EventHistory.Insert(0, twitchActionHistoryEntry);
						if (this.EventHistory.Count > 500)
						{
							this.EventHistory.RemoveAt(this.EventHistory.Count - 1);
						}
						if (this.EventHistoryAdded != null)
						{
							this.EventHistoryAdded();
						}
					}
					return;
				}
			}
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x0600C19C RID: 49564 RVA: 0x004768CC File Offset: 0x00474ACC
		public bool TwitchActive
		{
			get
			{
				return this.twitchActive;
			}
		}

		// Token: 0x0600C19D RID: 49565 RVA: 0x004768D4 File Offset: 0x00474AD4
		public void SaveExportViewerData(string _filePath)
		{
			this.ViewerData.WriteExport(_filePath);
		}

		// Token: 0x0600C19E RID: 49566 RVA: 0x004768E4 File Offset: 0x00474AE4
		public void LoadExportViewerData(string _filePath)
		{
			using (StreamReader streamReader = File.OpenText(_filePath))
			{
				this.ViewerData.LoadExport(streamReader);
			}
		}

		// Token: 0x0600C19F RID: 49567 RVA: 0x00476920 File Offset: 0x00474B20
		[PublicizedFrom(EAccessModifier.Private)]
		public int saveViewerDataThreaded(ThreadManager.ThreadInfo _threadInfo)
		{
			PooledExpandableMemoryStream[] array = (PooledExpandableMemoryStream[])_threadInfo.parameter;
			string text = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient ? GameIO.GetSaveGameLocalDir() : GameIO.GetSaveGameDir());
			string text2 = string.Format("{0}/{1}", text, "twitch.dat");
			if (SdFile.Exists(text2))
			{
				SdFile.Copy(text2, string.Format("{0}/{1}", text, "twitch.dat.bak"), true);
			}
			array[0].Position = 0L;
			StreamUtils.WriteStreamToFile(array[0], text2);
			MemoryPools.poolMemoryStream.FreeSync(array[0]);
			string text3 = GameIO.GetUserGameDataDir() + "/Twitch/" + TwitchManager.MainFileVersion.ToString();
			text2 = string.Format("{0}/{1}", text3, "twitch_main.dat");
			if (SdFile.Exists(text2))
			{
				SdFile.Copy(text2, string.Format("{0}/{1}", text3, "twitch_main.dat.bak"), true);
			}
			array[1].Position = 0L;
			StreamUtils.WriteStreamToFile(array[1], text2);
			MemoryPools.poolMemoryStream.FreeSync(array[1]);
			return -1;
		}

		// Token: 0x0600C1A0 RID: 49568 RVA: 0x00476A10 File Offset: 0x00474C10
		[PublicizedFrom(EAccessModifier.Private)]
		public void LoadViewerData()
		{
			string text = (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient ? GameIO.GetSaveGameLocalDir() : GameIO.GetSaveGameDir());
			string text2 = string.Format("{0}/{1}", text, "twitch.dat");
			if (SdFile.Exists(text2))
			{
				try
				{
					using (Stream stream = SdFile.OpenRead(text2))
					{
						using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader.SetBaseStream(stream);
							this.Read(pooledBinaryReader);
						}
					}
				}
				catch (Exception)
				{
					text2 = string.Format("{0}/{1}", text, "twitch.dat.bak");
					if (SdFile.Exists(text2))
					{
						using (Stream stream2 = SdFile.OpenRead(text2))
						{
							using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
							{
								pooledBinaryReader2.SetBaseStream(stream2);
								this.Read(pooledBinaryReader2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600C1A1 RID: 49569 RVA: 0x00476B2C File Offset: 0x00474D2C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool LoadSpecialViewerData(UserDataStorageType _userDataStorage)
		{
			string text = string.Format("{0}/{1}", GameIO.GetSaveGameRootDir(_userDataStorage), "twitch_special.dat");
			if (SdFile.Exists(text))
			{
				try
				{
					using (Stream stream = SdFile.OpenRead(text))
					{
						using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader.SetBaseStream(stream);
							this.ReadSpecial(pooledBinaryReader);
						}
					}
				}
				catch (Exception)
				{
					text = string.Format("{0}/{1}", GameIO.GetSaveGameRootDir(_userDataStorage), "twitch_special.dat.bak");
					if (SdFile.Exists(text))
					{
						using (Stream stream2 = SdFile.OpenRead(text))
						{
							using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
							{
								pooledBinaryReader2.SetBaseStream(stream2);
								this.ReadSpecial(pooledBinaryReader2);
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600C1A2 RID: 49570 RVA: 0x00476C38 File Offset: 0x00474E38
		[PublicizedFrom(EAccessModifier.Private)]
		public bool LoadMainViewerData(UserDataStorageType _userDataStorage)
		{
			string text = string.Format("{0}/{1}", GameIO.GetSaveGameRootDir(_userDataStorage), "twitch_main.dat");
			if (SdFile.Exists(text))
			{
				try
				{
					using (Stream stream = SdFile.OpenRead(text))
					{
						using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader.SetBaseStream(stream);
							this.ReadMain(pooledBinaryReader);
						}
					}
				}
				catch (Exception)
				{
					text = string.Format("{0}/{1}", GameIO.GetSaveGameRootDir(_userDataStorage), "twitch_main.dat.bak");
					if (SdFile.Exists(text))
					{
						using (Stream stream2 = SdFile.OpenRead(text))
						{
							using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
							{
								pooledBinaryReader2.SetBaseStream(stream2);
								this.ReadMain(pooledBinaryReader2);
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600C1A3 RID: 49571 RVA: 0x00476D44 File Offset: 0x00474F44
		[PublicizedFrom(EAccessModifier.Private)]
		public bool LoadLatestMainViewerData(UserDataStorageType _userDataStorage)
		{
			for (int i = (int)TwitchManager.MainFileVersion; i >= 2; i--)
			{
				if (this.LoadLatestMainViewerData(_userDataStorage, i))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C1A4 RID: 49572 RVA: 0x00476D70 File Offset: 0x00474F70
		[PublicizedFrom(EAccessModifier.Private)]
		public bool LoadLatestMainViewerData(UserDataStorageType _userDataStorage, int version)
		{
			string text = GameIO.GetUserGameDataDir(_userDataStorage) + "/Twitch/" + version.ToString();
			if (!SdDirectory.Exists(text))
			{
				SdDirectory.CreateDirectory(text);
			}
			string text2 = string.Format("{0}/{1}", text, "twitch_main.dat");
			if (SdFile.Exists(text2))
			{
				try
				{
					using (Stream stream = SdFile.OpenRead(text2))
					{
						using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader.SetBaseStream(stream);
							this.ReadMain(pooledBinaryReader);
						}
					}
				}
				catch (Exception)
				{
					text2 = string.Format("{0}/{1}", text, "twitch_main.dat.bak");
					if (SdFile.Exists(text2))
					{
						using (Stream stream2 = SdFile.OpenRead(text2))
						{
							using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
							{
								pooledBinaryReader2.SetBaseStream(stream2);
								this.ReadMain(pooledBinaryReader2);
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600C1A5 RID: 49573 RVA: 0x00476E9C File Offset: 0x0047509C
		[PublicizedFrom(EAccessModifier.Private)]
		public void LoadMainViewerData()
		{
			UserDataStorageType defaultSaveStorage = PlatformManager.MultiPlatform.UserDataRoaming.DefaultSaveStorage;
			if (!this.<LoadMainViewerData>g__LoadViewerDataFromStorage|386_0(defaultSaveStorage))
			{
				UserDataStorageType userDataStorageType = UserDataStorageType.DeviceLocal;
				if (defaultSaveStorage != userDataStorageType && this.<LoadMainViewerData>g__LoadViewerDataFromStorage|386_0(userDataStorageType))
				{
					Log.Out(string.Format("Twitch main viewer data found in {0}", userDataStorageType));
				}
			}
		}

		// Token: 0x0600C1A6 RID: 49574 RVA: 0x00476EE8 File Offset: 0x004750E8
		public void SaveViewerData()
		{
			if (this.dataSaveThreadInfo == null || !ThreadManager.ActiveThreads.ContainsKey("viewerDataSave"))
			{
				PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
					this.Write(pooledBinaryWriter);
				}
				PooledExpandableMemoryStream pooledExpandableMemoryStream2 = MemoryPools.poolMemoryStream.AllocSync(true);
				using (PooledBinaryWriter pooledBinaryWriter2 = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter2.SetBaseStream(pooledExpandableMemoryStream2);
					this.WriteMain(pooledBinaryWriter2);
				}
				this.dataSaveThreadInfo = ThreadManager.StartThread("viewerDataSave", null, new ThreadManager.ThreadFunctionLoopDelegate(this.saveViewerDataThreaded), null, new PooledExpandableMemoryStream[] { pooledExpandableMemoryStream, pooledExpandableMemoryStream2 }, null, false, true);
			}
		}

		// Token: 0x0600C1A7 RID: 49575 RVA: 0x00476FC0 File Offset: 0x004751C0
		public void Write(PooledBinaryWriter bw)
		{
			bw.Write(TwitchManager.FileVersion);
			bw.Write(this.UseProgression);
			this.ViewerData.Write(bw);
			bw.Write(this.Leaderboard.Count);
			for (int i = 0; i < this.Leaderboard.Count; i++)
			{
				TwitchLeaderboardEntry twitchLeaderboardEntry = this.Leaderboard[i];
				bw.Write(twitchLeaderboardEntry.UserName);
				bw.Write(twitchLeaderboardEntry.Kills);
				bw.Write(twitchLeaderboardEntry.UserColor);
			}
			bw.Write(this.UseActionsDuringBloodmoon);
			bw.Write(this.RewardPot);
			bw.Write(this.ViewerData.PointRate);
			bw.Write(this.CooldownPresetIndex);
			bw.Write((byte)this.PimpPotType);
			bw.Write(this.AllowCrateSharing);
			bw.Write(this.BitPointModifier);
			bw.Write(this.RaidPointAdd);
			bw.Write(this.RaidViewerMinimum);
			bw.Write(this.SubPointModifier);
			bw.Write(this.GiftSubPointModifier);
			bw.Write((byte)this.VotingManager.MaxDailyVotes);
			bw.Write(this.VotingManager.VoteTime);
			bw.Write((byte)this.VotingManager.CurrentVoteDayTimeRange);
			bw.Write(this.VotingManager.ViewerDefeatReward);
			bw.Write(this.VotingManager.AllowVotesDuringBloodmoon);
			bw.Write(this.ViewerData.ActionSpamDelay);
			bw.Write(this.ViewerData.StartingPoints);
			bw.Write(this.UseActionsDuringQuests);
			bw.Write(this.VotingManager.AllowVotesDuringQuests);
			bw.Write(this.VotingManager.AllowVotesInSafeZone);
			bw.Write(this.changedEnabledVoteList.Count);
			for (int j = 0; j < this.changedEnabledVoteList.Count; j++)
			{
				bw.Write(this.changedEnabledVoteList[j]);
				bw.Write(TwitchActionManager.TwitchVotes[this.changedEnabledVoteList[j]].Enabled);
			}
			bw.Write((byte)this.integrationSetting);
			bw.Write(this.EventPresetIndex);
			bw.Write(this.ActionPresetIndex);
			bw.Write(this.VotePresetIndex);
			bw.Write(this.AllowBitEvents);
			bw.Write(this.AllowSubEvents);
			bw.Write(this.AllowGiftSubEvents);
			bw.Write(this.AllowCharityEvents);
			bw.Write(this.AllowRaidEvents);
			bw.Write(this.AllowHypeTrainEvents);
			bw.Write(this.AllowChannelPointRedemptions);
			bw.Write(TwitchManager.LeaderboardStats.GoodRewardTime);
			bw.Write(TwitchManager.LeaderboardStats.GoodRewardAmount);
			int num = 0;
			for (int k = 0; k < this.ActionPresets.Count; k++)
			{
				TwitchActionPreset twitchActionPreset = this.ActionPresets[k];
				if (twitchActionPreset.AddedActions.Count > 0 || twitchActionPreset.RemovedActions.Count > 0)
				{
					num++;
				}
			}
			bw.Write(num);
			for (int l = 0; l < this.ActionPresets.Count; l++)
			{
				TwitchActionPreset twitchActionPreset2 = this.ActionPresets[l];
				if (twitchActionPreset2.AddedActions.Count > 0 || twitchActionPreset2.RemovedActions.Count > 0)
				{
					bw.Write(twitchActionPreset2.Name);
					bw.Write(twitchActionPreset2.AddedActions.Count);
					for (int m = 0; m < twitchActionPreset2.AddedActions.Count; m++)
					{
						bw.Write(twitchActionPreset2.AddedActions[m]);
					}
					bw.Write(twitchActionPreset2.RemovedActions.Count);
					for (int n = 0; n < twitchActionPreset2.RemovedActions.Count; n++)
					{
						bw.Write(twitchActionPreset2.RemovedActions[n]);
					}
				}
			}
			bw.Write(this.bitPriceMultiplier);
			bw.Write(this.AllowCreatorGoalEvents);
			bw.Write(this.changedActionList.Count);
			for (int num2 = 0; num2 < this.changedActionList.Count; num2++)
			{
				bw.Write(this.changedActionList[num2]);
				bw.Write(TwitchActionManager.TwitchActions[this.changedActionList[num2]].ModifiedCost);
			}
			bw.Write(this.BitPot);
			bw.Write(this.BitPotPercentage);
		}

		// Token: 0x0600C1A8 RID: 49576 RVA: 0x0047743C File Offset: 0x0047563C
		public void HandleChangedPropertyList()
		{
			this.changedActionList.Clear();
			this.changedEnabledVoteList.Clear();
			foreach (string text in TwitchActionManager.TwitchActions.Keys)
			{
				TwitchAction twitchAction = TwitchActionManager.TwitchActions[text];
				if (twitchAction.DefaultCost != twitchAction.ModifiedCost)
				{
					this.changedActionList.Add(text);
				}
			}
			foreach (string text2 in TwitchActionManager.TwitchVotes.Keys)
			{
				TwitchVote twitchVote = TwitchActionManager.TwitchVotes[text2];
				if (twitchVote.Enabled != twitchVote.OriginalEnabled)
				{
					this.changedEnabledVoteList.Add(text2);
				}
			}
		}

		// Token: 0x0600C1A9 RID: 49577 RVA: 0x00477534 File Offset: 0x00475734
		public void WriteSpecial(PooledBinaryWriter bw)
		{
			this.ViewerData.WriteSpecial(bw);
		}

		// Token: 0x0600C1AA RID: 49578 RVA: 0x00477542 File Offset: 0x00475742
		public void WriteMain(PooledBinaryWriter bw)
		{
			bw.Write(TwitchManager.MainFileVersion);
			bw.Write(this.HasViewedSettings);
			this.ViewerData.WriteSpecial(bw);
		}

		// Token: 0x0600C1AB RID: 49579 RVA: 0x00477568 File Offset: 0x00475768
		public void Read(PooledBinaryReader br)
		{
			this.CurrentFileVersion = br.ReadByte();
			if (this.CurrentFileVersion > 1)
			{
				this.UseProgression = br.ReadBoolean();
			}
			this.ViewerData.Read(br, this.CurrentFileVersion);
			if (this.CurrentFileVersion > 3)
			{
				int num = br.ReadInt32();
				this.Leaderboard.Clear();
				for (int i = 0; i < num; i++)
				{
					string text;
					int num2;
					string text2;
					if (this.CurrentFileVersion > 10)
					{
						text = br.ReadString();
						num2 = br.ReadInt32();
						text2 = br.ReadString();
					}
					else
					{
						text = br.ReadString();
						text2 = br.ReadString();
						num2 = br.ReadInt32();
					}
					this.Leaderboard.Add(new TwitchLeaderboardEntry(text, text2, num2));
				}
			}
			if (this.CurrentFileVersion > 4)
			{
				this.UseActionsDuringBloodmoon = br.ReadInt32();
			}
			if (this.CurrentFileVersion > 5)
			{
				this.RewardPot = br.ReadInt32();
				if (this.RewardPot <= 0)
				{
					this.RewardPot = 0;
				}
				if (this.RewardPot > TwitchManager.LeaderboardStats.LargestPimpPot)
				{
					TwitchManager.LeaderboardStats.LargestPimpPot = this.RewardPot;
				}
			}
			if (this.CurrentFileVersion > 6)
			{
				this.ViewerData.PointRate = br.ReadSingle();
				this.CooldownPresetIndex = br.ReadInt32();
				if (this.CurrentFileVersion <= 18)
				{
					this.ActionCooldownModifier = br.ReadSingle();
				}
				this.PimpPotType = (TwitchManager.PimpPotSettings)br.ReadByte();
				this.AllowCrateSharing = br.ReadBoolean();
			}
			if (this.CurrentFileVersion > 7)
			{
				if (this.CurrentFileVersion <= 17)
				{
					br.ReadBoolean();
					br.ReadBoolean();
					br.ReadBoolean();
				}
				this.BitPointModifier = br.ReadInt32();
				this.RaidPointAdd = br.ReadInt32();
				this.RaidViewerMinimum = br.ReadInt32();
				this.SubPointModifier = br.ReadInt32();
				this.GiftSubPointModifier = br.ReadInt32();
				if (this.CurrentFileVersion <= 20)
				{
					int num3 = br.ReadInt32();
					this.changedActionList.Clear();
					for (int j = 0; j < num3; j++)
					{
						string text3 = br.ReadString();
						bool flag = br.ReadBoolean();
						if (TwitchActionManager.TwitchActions.ContainsKey(text3))
						{
							this.changedActionList.Add(text3);
							TwitchActionManager.TwitchActions[text3].Enabled = flag;
						}
					}
				}
			}
			if (this.CurrentFileVersion > 8)
			{
				this.VotingManager.MaxDailyVotes = (int)br.ReadByte();
				this.VotingManager.VoteTime = br.ReadSingle();
				this.VotingManager.CurrentVoteDayTimeRange = (int)br.ReadByte();
				this.VotingManager.ViewerDefeatReward = br.ReadInt32();
				this.VotingManager.AllowVotesDuringBloodmoon = br.ReadBoolean();
			}
			if (this.CurrentFileVersion > 9)
			{
				this.ViewerData.ActionSpamDelay = br.ReadSingle();
			}
			if (this.CurrentFileVersion > 10)
			{
				this.ViewerData.StartingPoints = br.ReadInt32();
			}
			if (this.CurrentFileVersion > 11)
			{
				this.UseActionsDuringQuests = br.ReadInt32();
				this.VotingManager.AllowVotesDuringQuests = br.ReadBoolean();
			}
			if (this.CurrentFileVersion > 12)
			{
				this.VotingManager.AllowVotesInSafeZone = br.ReadBoolean();
				if (this.CurrentFileVersion <= 17)
				{
					br.ReadByte();
				}
			}
			if (this.CurrentFileVersion > 13)
			{
				int num4 = br.ReadInt32();
				this.changedEnabledVoteList.Clear();
				for (int k = 0; k < num4; k++)
				{
					string text4 = br.ReadString();
					this.changedEnabledVoteList.Add(text4);
					TwitchActionManager.TwitchVotes[text4].Enabled = br.ReadBoolean();
				}
			}
			if (this.CurrentFileVersion >= 16)
			{
				byte b = br.ReadByte();
				if (b > 1)
				{
					b = 1;
				}
				this.IntegrationSetting = (TwitchManager.IntegrationSettings)b;
			}
			if (this.CurrentFileVersion >= 17)
			{
				this.EventPresetIndex = br.ReadInt32();
				this.CurrentEventPreset = this.EventPresets[this.EventPresetIndex];
			}
			if (this.CurrentFileVersion >= 18)
			{
				this.ActionPresetIndex = br.ReadInt32();
				this.CurrentActionPreset = this.ActionPresets[this.ActionPresetIndex];
				this.VotePresetIndex = br.ReadInt32();
				this.CurrentVotePreset = this.VotePresets[this.VotePresetIndex];
				this.AllowBitEvents = br.ReadBoolean();
				this.AllowSubEvents = br.ReadBoolean();
				this.AllowGiftSubEvents = br.ReadBoolean();
				this.AllowCharityEvents = br.ReadBoolean();
				this.AllowRaidEvents = br.ReadBoolean();
				this.AllowHypeTrainEvents = br.ReadBoolean();
				this.AllowChannelPointRedemptions = br.ReadBoolean();
			}
			if (this.CurrentFileVersion >= 20)
			{
				TwitchManager.LeaderboardStats.GoodRewardTime = br.ReadInt32();
				TwitchManager.LeaderboardStats.GoodRewardAmount = br.ReadInt32();
			}
			if (this.CurrentFileVersion >= 21)
			{
				int num5 = br.ReadInt32();
				for (int l = 0; l < num5; l++)
				{
					string text5 = br.ReadString();
					TwitchActionPreset twitchActionPreset = null;
					for (int m = 0; m < this.ActionPresets.Count; m++)
					{
						if (this.ActionPresets[m].Name == text5)
						{
							twitchActionPreset = this.ActionPresets[m];
							break;
						}
					}
					int num6 = br.ReadInt32();
					if (twitchActionPreset != null)
					{
						twitchActionPreset.AddedActions.Clear();
						twitchActionPreset.RemovedActions.Clear();
					}
					for (int n = 0; n < num6; n++)
					{
						if (twitchActionPreset != null)
						{
							twitchActionPreset.AddedActions.Add(br.ReadString());
						}
					}
					num6 = br.ReadInt32();
					for (int num7 = 0; num7 < num6; num7++)
					{
						if (twitchActionPreset != null)
						{
							twitchActionPreset.RemovedActions.Add(br.ReadString());
						}
					}
				}
			}
			if (this.CurrentFileVersion >= 22)
			{
				this.BitPriceMultiplier = br.ReadSingle();
			}
			if (this.CurrentFileVersion >= 23)
			{
				this.AllowCreatorGoalEvents = br.ReadBoolean();
			}
			if (this.CurrentFileVersion >= 24)
			{
				int num8 = br.ReadInt32();
				this.changedActionList.Clear();
				for (int num9 = 0; num9 < num8; num9++)
				{
					string text6 = br.ReadString();
					int num10 = br.ReadInt32();
					if (TwitchActionManager.TwitchActions.ContainsKey(text6))
					{
						this.changedActionList.Add(text6);
						TwitchActionManager.TwitchActions[text6].ModifiedCost = num10;
					}
				}
			}
			if (this.CurrentFileVersion >= 25)
			{
				this.BitPot = br.ReadInt32();
				if (this.BitPot <= 0)
				{
					this.BitPot = 0;
				}
				if (this.BitPot > TwitchManager.LeaderboardStats.LargestBitPot)
				{
					TwitchManager.LeaderboardStats.LargestBitPot = this.BitPot;
				}
			}
			if (this.CurrentFileVersion >= 26)
			{
				this.BitPotPercentage = br.ReadSingle();
			}
		}

		// Token: 0x0600C1AC RID: 49580 RVA: 0x00477BFB File Offset: 0x00475DFB
		public void ReadSpecial(PooledBinaryReader br)
		{
			this.ViewerData.ReadSpecial(br, 1);
		}

		// Token: 0x0600C1AD RID: 49581 RVA: 0x00477C0A File Offset: 0x00475E0A
		public void ReadMain(PooledBinaryReader br)
		{
			this.CurrentMainFileVersion = br.ReadByte();
			this.HasViewedSettings = br.ReadBoolean();
			this.ViewerData.ReadSpecial(br, this.CurrentMainFileVersion);
		}

		// Token: 0x0600C1AE RID: 49582 RVA: 0x00477C38 File Offset: 0x00475E38
		public void SendChannelPointOutputMessage(string name, ViewerEntry entry)
		{
			if (entry.SpecialPoints == 0f)
			{
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_PointsWithoutSpecial, name, entry.CombinedPoints), true);
				return;
			}
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_PointsWithSpecial, name, entry.CombinedPoints, entry.SpecialPoints), true);
		}

		// Token: 0x0600C1AF RID: 49583 RVA: 0x00477CA4 File Offset: 0x00475EA4
		public void SendChannelCreditOutputMessage(string name, ViewerEntry entry)
		{
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_BitCredits, name, entry.BitCredits), true);
		}

		// Token: 0x0600C1B0 RID: 49584 RVA: 0x00477CC9 File Offset: 0x00475EC9
		public void SendChannelPointOutputMessage(string name)
		{
			this.SendChannelPointOutputMessage(name, this.ViewerData.GetViewerEntry(name));
		}

		// Token: 0x0600C1B1 RID: 49585 RVA: 0x00477CDE File Offset: 0x00475EDE
		public void SendChannelCreditOutputMessage(string name)
		{
			this.SendChannelCreditOutputMessage(name, this.ViewerData.GetViewerEntry(name));
		}

		// Token: 0x0600C1B2 RID: 49586 RVA: 0x00477CF3 File Offset: 0x00475EF3
		public void SendChannelMessage(string message, bool useQueue = true)
		{
			this.ircClient.SendChannelMessage(message, useQueue);
		}

		// Token: 0x0600C1B3 RID: 49587 RVA: 0x00477D04 File Offset: 0x00475F04
		[PublicizedFrom(EAccessModifier.Private)]
		public void UpdateActionCooldowns(float modifier)
		{
			foreach (TwitchAction twitchAction in TwitchActionManager.TwitchActions.Values)
			{
				if (twitchAction.IsInPreset(this.CurrentActionPreset))
				{
					twitchAction.UpdateModifiedCooldown(modifier);
				}
			}
		}

		// Token: 0x0600C1B4 RID: 49588 RVA: 0x00477D6C File Offset: 0x00475F6C
		public void SetupAvailableCommands()
		{
			this.AvailableCommands.Clear();
			this.AlternateCommands.Clear();
			TwitchAction[] array = (from a in TwitchActionManager.TwitchActions.Values
				where a.CanUse && a.IsInPreset(this.CurrentActionPreset)
				orderby a.Command
				orderby a.PointType
				select a).ToArray<TwitchAction>();
			List<string> list = null;
			for (int i = 0; i < array.Count<TwitchAction>(); i++)
			{
				TwitchAction twitchAction = array[i];
				if (this.UseProgression && !this.OverrideProgession)
				{
					int startGameStage = twitchAction.StartGameStage;
					if (startGameStage == -1 || startGameStage <= this.HighestGameStage)
					{
						if (twitchAction.Replaces != "")
						{
							string text = twitchAction.Replaces;
							if (this.AlternateCommands.ContainsKey(twitchAction.Replaces))
							{
								text = this.AlternateCommands[twitchAction.Replaces];
							}
							if (list == null)
							{
								list = new List<string>();
							}
							list.Add(text);
						}
						if (this.AvailableCommands.ContainsKey(twitchAction.Command))
						{
							this.AvailableCommands[twitchAction.Command] = twitchAction;
						}
						else
						{
							this.AvailableCommands.Add(twitchAction.Command, twitchAction);
						}
						if (!this.AlternateCommands.ContainsKey(twitchAction.BaseCommand))
						{
							this.AlternateCommands.Add(twitchAction.BaseCommand, twitchAction.Command);
						}
					}
				}
				else
				{
					if (twitchAction.RandomDaily)
					{
						twitchAction.AllowedDay = this.lastGameDay;
					}
					if (twitchAction.Replaces != "")
					{
						string text2 = twitchAction.Replaces;
						if (this.AlternateCommands.ContainsKey(twitchAction.Replaces))
						{
							text2 = this.AlternateCommands[twitchAction.Replaces];
						}
						if (list == null)
						{
							list = new List<string>();
						}
						list.Add(text2);
					}
					if (this.AvailableCommands.ContainsKey(twitchAction.Command))
					{
						this.AvailableCommands[twitchAction.Command] = twitchAction;
					}
					else
					{
						this.AvailableCommands.Add(twitchAction.Command, twitchAction);
					}
					if (!this.AlternateCommands.ContainsKey(twitchAction.BaseCommand))
					{
						this.AlternateCommands.Add(twitchAction.BaseCommand, twitchAction.Command);
					}
				}
			}
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					string text3 = list[j];
					if (this.AvailableCommands.ContainsKey(text3))
					{
						this.AvailableCommands.Remove(text3);
					}
				}
			}
		}

		// Token: 0x0600C1B5 RID: 49589 RVA: 0x00478008 File Offset: 0x00476208
		public void SetupAvailableCommandsWithOutput(int lastGameStage, bool displayMessage)
		{
			this.AvailableCommands.Clear();
			this.AlternateCommands.Clear();
			StringBuilder stringBuilder = null;
			TwitchAction[] array = (from a in TwitchActionManager.TwitchActions.Values
				where a.CanUse && a.IsInPreset(this.CurrentActionPreset)
				orderby a.Command
				orderby a.PointType
				select a).ToArray<TwitchAction>();
			List<string> list = null;
			for (int i = 0; i < array.Count<TwitchAction>(); i++)
			{
				TwitchAction twitchAction = array[i];
				if (this.UseProgression && !this.OverrideProgession)
				{
					int startGameStage = twitchAction.StartGameStage;
					if (startGameStage == -1 || startGameStage <= this.HighestGameStage)
					{
						if (twitchAction.Replaces != "")
						{
							string text = twitchAction.Replaces;
							if (this.AlternateCommands.ContainsKey(twitchAction.Replaces))
							{
								text = this.AlternateCommands[twitchAction.Replaces];
							}
							if (list == null)
							{
								list = new List<string>();
							}
							list.Add(text);
						}
						if (this.AvailableCommands.ContainsKey(twitchAction.Command))
						{
							this.AvailableCommands[twitchAction.Command] = twitchAction;
							if (startGameStage > lastGameStage)
							{
								if (stringBuilder == null)
								{
									stringBuilder = new StringBuilder();
									stringBuilder.Append("*" + twitchAction.Command);
								}
								else
								{
									stringBuilder.Append(", " + twitchAction.Command);
								}
							}
						}
						else
						{
							this.AvailableCommands.Add(twitchAction.Command, twitchAction);
							if (startGameStage > lastGameStage)
							{
								if (stringBuilder == null)
								{
									stringBuilder = new StringBuilder();
									stringBuilder.Append(twitchAction.Command);
								}
								else
								{
									stringBuilder.Append(", " + twitchAction.Command);
								}
							}
						}
						if (!this.AlternateCommands.ContainsKey(twitchAction.BaseCommand))
						{
							this.AlternateCommands.Add(twitchAction.BaseCommand, twitchAction.Command);
						}
					}
				}
				else
				{
					if (twitchAction.RandomDaily)
					{
						twitchAction.AllowedDay = this.lastGameDay;
					}
					if (this.AvailableCommands.ContainsKey(twitchAction.Command))
					{
						this.AvailableCommands[twitchAction.Command] = twitchAction;
					}
					else
					{
						this.AvailableCommands.Add(twitchAction.Command, twitchAction);
					}
					if (!this.AlternateCommands.ContainsKey(twitchAction.BaseCommand))
					{
						this.AlternateCommands.Add(twitchAction.BaseCommand, twitchAction.Command);
					}
				}
			}
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					string text2 = list[j];
					if (this.AvailableCommands.ContainsKey(text2))
					{
						this.AvailableCommands.Remove(text2);
					}
				}
			}
			if (displayMessage && stringBuilder != null && this.AllowActions && this.CurrentActionPreset.ShowNewCommands)
			{
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_NewActions, stringBuilder), true);
				Manager.BroadcastPlayByLocalPlayer(this.LocalPlayer.position, "twitch_new_commands");
			}
		}

		// Token: 0x0600C1B6 RID: 49590 RVA: 0x00478330 File Offset: 0x00476530
		public void HandleCooldownActionLocking()
		{
			foreach (string text in this.AvailableCommands.Keys)
			{
				TwitchAction twitchAction = this.AvailableCommands[text];
				if (twitchAction.IsInPreset(this.CurrentActionPreset))
				{
					if (this.OnCooldown)
					{
						if (this.CooldownType == TwitchManager.CooldownTypes.BloodMoonDisabled || this.CooldownType == TwitchManager.CooldownTypes.Time)
						{
							twitchAction.OnCooldown = true;
						}
						else if (this.CooldownType == TwitchManager.CooldownTypes.MaxReachedWaiting || this.CooldownType == TwitchManager.CooldownTypes.SafeCooldown)
						{
							twitchAction.OnCooldown = twitchAction.WaitingBlocked;
						}
						else
						{
							twitchAction.OnCooldown = twitchAction.CooldownBlocked;
						}
					}
					else
					{
						twitchAction.OnCooldown = false;
					}
				}
			}
			if (this.CommandsChanged != null)
			{
				this.CommandsChanged();
			}
		}

		// Token: 0x0600C1B7 RID: 49591 RVA: 0x0047840C File Offset: 0x0047660C
		public void PushBalanceToExtensionQueue(string userID, int creditBalance)
		{
			if (this.extensionManager != null)
			{
				this.extensionManager.PushUserBalance(new ValueTuple<string, int>(userID, creditBalance));
			}
		}

		// Token: 0x0600C1B8 RID: 49592 RVA: 0x00478428 File Offset: 0x00476628
		public void DisplayActions()
		{
			int count = this.ActionMessages.Count;
			if (this.CurrentUnityTime > this.nextDisplayCommandsTime)
			{
				this.nextDisplayCommandsTime = this.CurrentUnityTime + 15f;
				this.ircClient.SendChannelMessages(this.ActionMessages, true);
			}
		}

		// Token: 0x0600C1B9 RID: 49593 RVA: 0x00478468 File Offset: 0x00476668
		public void DisplayCommands(bool isBroadcaster, bool isMod, bool isVIP, bool isSub)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.TwitchCommandList.Count; i++)
			{
				if (!(this.TwitchCommandList[i] is TwitchCommandCommands))
				{
					bool flag = false;
					switch (BaseTwitchCommand.GetPermission(this.TwitchCommandList[i]))
					{
					case BaseTwitchCommand.PermissionLevels.Everyone:
						flag = true;
						break;
					case BaseTwitchCommand.PermissionLevels.VIP:
						flag = isVIP;
						break;
					case BaseTwitchCommand.PermissionLevels.Sub:
						flag = isSub;
						break;
					case BaseTwitchCommand.PermissionLevels.Mod:
						flag = isMod;
						break;
					case BaseTwitchCommand.PermissionLevels.Broadcaster:
						flag = isBroadcaster;
						break;
					}
					if (flag)
					{
						for (int j = 0; j < this.TwitchCommandList[i].LocalizedCommandNames.Length; j++)
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append(this.TwitchCommandList[i].LocalizedCommandNames[j]);
						}
					}
				}
			}
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_Commands, stringBuilder.ToString()), true);
		}

		// Token: 0x0600C1BA RID: 49594 RVA: 0x00478564 File Offset: 0x00476764
		public void AddTip(string tipname)
		{
			string text = Localization.Get(tipname, false, null);
			string text2 = Localization.Get(tipname + "Desc", false, null);
			if (text != "")
			{
				this.tipTitleList.Add(text);
				this.tipDescriptionList.Add(text2);
			}
		}

		// Token: 0x0600C1BB RID: 49595 RVA: 0x004785B2 File Offset: 0x004767B2
		public void DisplayGameStage()
		{
			if (this.LocalPlayer != null)
			{
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_Gamestage, this.LocalPlayer.unModifiedGameStage), true);
			}
		}

		// Token: 0x0600C1BC RID: 49596 RVA: 0x004785E9 File Offset: 0x004767E9
		public bool CheckIfTwitchKill(EntityPlayer player)
		{
			return this.twitchPlayerDeathsThisFrame.Contains(player);
		}

		// Token: 0x0600C1BD RID: 49597 RVA: 0x004785F8 File Offset: 0x004767F8
		public bool LiveListContains(int entityID)
		{
			for (int i = 0; i < this.liveList.Count; i++)
			{
				if (this.liveList[i].SpawnedEntityID == entityID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C1C2 RID: 49602 RVA: 0x0047876A File Offset: 0x0047696A
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool <LoadMainViewerData>g__LoadViewerDataFromStorage|386_0(UserDataStorageType storageType)
		{
			return this.LoadLatestMainViewerData(storageType) || this.LoadMainViewerData(storageType) || this.LoadSpecialViewerData(storageType);
		}

		// Token: 0x040090DD RID: 37085
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchManager instance = null;

		// Token: 0x040090DE RID: 37086
		[PublicizedFrom(EAccessModifier.Private)]
		public const float SAVE_TIME_SEC = 30f;

		// Token: 0x040090DF RID: 37087
		public float saveTime;

		// Token: 0x040090E0 RID: 37088
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo dataSaveThreadInfo;

		// Token: 0x040090E1 RID: 37089
		public static byte FileVersion = 26;

		// Token: 0x040090E3 RID: 37091
		public static byte MainFileVersion = 3;

		// Token: 0x040090E5 RID: 37093
		public TwitchIRCClient ircClient;

		// Token: 0x040090E6 RID: 37094
		public ExtensionManager extensionManager;

		// Token: 0x040090E7 RID: 37095
		[PublicizedFrom(EAccessModifier.Private)]
		public int resetClientAttempts;

		// Token: 0x040090E8 RID: 37096
		[PublicizedFrom(EAccessModifier.Private)]
		public bool overrideProgression;

		// Token: 0x040090EB RID: 37099
		[PublicizedFrom(EAccessModifier.Private)]
		public int commandsAvailable = -1;

		// Token: 0x040090EC RID: 37100
		public Dictionary<string, TwitchAction> AvailableCommands = new Dictionary<string, TwitchAction>();

		// Token: 0x040090ED RID: 37101
		public Dictionary<string, string> AlternateCommands = new Dictionary<string, string>();

		// Token: 0x040090EE RID: 37102
		public TwitchManager.PimpPotSettings PimpPotType = TwitchManager.PimpPotSettings.EnabledSP;

		// Token: 0x040090EF RID: 37103
		public static int PimpPotDefault = 500;

		// Token: 0x040090F0 RID: 37104
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchManager.IntegrationSettings integrationSetting = TwitchManager.IntegrationSettings.Both;

		// Token: 0x040090F1 RID: 37105
		[PublicizedFrom(EAccessModifier.Private)]
		public float actionCooldownModifier = 1f;

		// Token: 0x040090F2 RID: 37106
		[PublicizedFrom(EAccessModifier.Private)]
		public const int HistoryItemMax = 500;

		// Token: 0x040090F3 RID: 37107
		public float ActionPotPercentage = 0.15f;

		// Token: 0x040090F4 RID: 37108
		public float BitPotPercentage = 0.25f;

		// Token: 0x040090F5 RID: 37109
		public int RewardPot = TwitchManager.PimpPotDefault;

		// Token: 0x040090F6 RID: 37110
		public int BitPot;

		// Token: 0x040090F7 RID: 37111
		public int PartyKillRewardMax = 250;

		// Token: 0x040090F8 RID: 37112
		public EntityPlayerLocal LocalPlayer;

		// Token: 0x040090F9 RID: 37113
		public bool LocalPlayerInLandClaim;

		// Token: 0x040090FA RID: 37114
		public TwitchManager.CooldownTypes CooldownType = TwitchManager.CooldownTypes.Startup;

		// Token: 0x040090FB RID: 37115
		public float CooldownTime = 300f;

		// Token: 0x040090FC RID: 37116
		public float CurrentCooldownFill;

		// Token: 0x040090FD RID: 37117
		public float CooldownFillMax = 50f;

		// Token: 0x040090FE RID: 37118
		public int NextCooldownTime = 180;

		// Token: 0x040090FF RID: 37119
		public bool AllowCrateSharing;

		// Token: 0x04009100 RID: 37120
		public bool AllowBitEvents = true;

		// Token: 0x04009101 RID: 37121
		public bool AllowSubEvents = true;

		// Token: 0x04009102 RID: 37122
		public bool AllowGiftSubEvents = true;

		// Token: 0x04009103 RID: 37123
		public bool AllowCharityEvents = true;

		// Token: 0x04009104 RID: 37124
		public bool AllowRaidEvents = true;

		// Token: 0x04009105 RID: 37125
		public bool AllowHypeTrainEvents = true;

		// Token: 0x04009106 RID: 37126
		public bool AllowCreatorGoalEvents = true;

		// Token: 0x04009107 RID: 37127
		public bool AllowChannelPointRedemptions = true;

		// Token: 0x04009108 RID: 37128
		public List<CooldownPreset> CooldownPresets = new List<CooldownPreset>();

		// Token: 0x04009109 RID: 37129
		public int CooldownPresetIndex;

		// Token: 0x0400910A RID: 37130
		public CooldownPreset CurrentCooldownPreset;

		// Token: 0x0400910B RID: 37131
		public List<TwitchActionPreset> ActionPresets = new List<TwitchActionPreset>();

		// Token: 0x0400910C RID: 37132
		public List<TwitchVotePreset> VotePresets = new List<TwitchVotePreset>();

		// Token: 0x0400910D RID: 37133
		public List<TwitchEventPreset> EventPresets = new List<TwitchEventPreset>();

		// Token: 0x0400910E RID: 37134
		public int ActionPresetIndex;

		// Token: 0x0400910F RID: 37135
		public int VotePresetIndex;

		// Token: 0x04009110 RID: 37136
		public int EventPresetIndex;

		// Token: 0x04009111 RID: 37137
		public TwitchActionPreset CurrentActionPreset;

		// Token: 0x04009112 RID: 37138
		public TwitchVotePreset CurrentVotePreset;

		// Token: 0x04009113 RID: 37139
		public TwitchEventPreset CurrentEventPreset;

		// Token: 0x04009114 RID: 37140
		public bool UIDirty;

		// Token: 0x04009115 RID: 37141
		[PublicizedFrom(EAccessModifier.Private)]
		public float updateTime = 1f;

		// Token: 0x04009116 RID: 37142
		public float ExtensionCheckTime;

		// Token: 0x04009117 RID: 37143
		[PublicizedFrom(EAccessModifier.Private)]
		public World world;

		// Token: 0x04009118 RID: 37144
		public int lastGameDay = -1;

		// Token: 0x04009119 RID: 37145
		public int currentBMDayEnd = -1;

		// Token: 0x0400911A RID: 37146
		public int nextBMDay = -1;

		// Token: 0x0400911B RID: 37147
		public int BMCooldownStart;

		// Token: 0x0400911C RID: 37148
		public int BMCooldownEnd;

		// Token: 0x0400911D RID: 37149
		public int BitPointModifier = 1;

		// Token: 0x0400911E RID: 37150
		public int SubPointModifier = 1;

		// Token: 0x0400911F RID: 37151
		public int GiftSubPointModifier = 2;

		// Token: 0x04009120 RID: 37152
		public int RaidPointAdd = 1000;

		// Token: 0x04009121 RID: 37153
		public int RaidViewerMinimum = 10;

		// Token: 0x04009122 RID: 37154
		public int HypeTrainLevel;

		// Token: 0x04009123 RID: 37155
		[PublicizedFrom(EAccessModifier.Private)]
		public float bitPriceMultiplier = 1f;

		// Token: 0x04009124 RID: 37156
		public static TwitchLeaderboardStats LeaderboardStats = new TwitchLeaderboardStats();

		// Token: 0x04009125 RID: 37157
		public bool isBMActive;

		// Token: 0x04009126 RID: 37158
		public TwitchVoteLockTypes VoteLockedLevel;

		// Token: 0x04009127 RID: 37159
		public List<TwitchActionHistoryEntry> ActionHistory = new List<TwitchActionHistoryEntry>();

		// Token: 0x04009128 RID: 37160
		public List<TwitchActionHistoryEntry> VoteHistory = new List<TwitchActionHistoryEntry>();

		// Token: 0x04009129 RID: 37161
		public List<TwitchActionHistoryEntry> EventHistory = new List<TwitchActionHistoryEntry>();

		// Token: 0x0400912A RID: 37162
		public List<TwitchLeaderboardEntry> Leaderboard = new List<TwitchLeaderboardEntry>();

		// Token: 0x0400912B RID: 37163
		public List<TwitchRespawnEntry> RespawnEntries = new List<TwitchRespawnEntry>();

		// Token: 0x0400912C RID: 37164
		public int UseActionsDuringBloodmoon = 2;

		// Token: 0x0400912D RID: 37165
		public int UseActionsDuringQuests = 2;

		// Token: 0x0400912E RID: 37166
		public bool InitialCooldownSet;

		// Token: 0x04009131 RID: 37169
		public List<EntityPlayer> twitchPlayerDeathsThisFrame = new List<EntityPlayer>();

		// Token: 0x04009132 RID: 37170
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchManager.InitStates initState;

		// Token: 0x04009133 RID: 37171
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isLoaded;

		// Token: 0x04009134 RID: 37172
		public XUi LocalPlayerXUi;

		// Token: 0x04009135 RID: 37173
		public float CurrentUnityTime;

		// Token: 0x04009136 RID: 37174
		[PublicizedFrom(EAccessModifier.Private)]
		public bool resetCommandsNeeded;

		// Token: 0x04009137 RID: 37175
		[PublicizedFrom(EAccessModifier.Private)]
		public bool respawnEventNeeded;

		// Token: 0x04009138 RID: 37176
		[PublicizedFrom(EAccessModifier.Private)]
		public bool checkingExtensionInstalled;

		// Token: 0x04009139 RID: 37177
		[PublicizedFrom(EAccessModifier.Private)]
		public string broadcasterType = "";

		// Token: 0x0400913A RID: 37178
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchMessageEntry> inGameChatQueue = new List<TwitchMessageEntry>();

		// Token: 0x04009141 RID: 37185
		public TwitchAuthentication Authentication;

		// Token: 0x04009142 RID: 37186
		public EventSubClient EventSub;

		// Token: 0x04009143 RID: 37187
		public bool HasViewedSettings;

		// Token: 0x04009144 RID: 37188
		public string DeniedCrateEvent = "";

		// Token: 0x04009145 RID: 37189
		public string StealingCrateEvent = "";

		// Token: 0x04009146 RID: 37190
		public string PartyRespawnEvent = "";

		// Token: 0x04009147 RID: 37191
		public string OnPlayerDeathEvent = "";

		// Token: 0x04009148 RID: 37192
		public string OnPlayerRespawnEvent = "";

		// Token: 0x04009149 RID: 37193
		public List<string> tipTitleList = new List<string>();

		// Token: 0x0400914A RID: 37194
		public List<string> tipDescriptionList = new List<string>();

		// Token: 0x0400914B RID: 37195
		[PublicizedFrom(EAccessModifier.Private)]
		public int extensionActiveCheckFailures;

		// Token: 0x0400914C RID: 37196
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ActivatedAction;

		// Token: 0x0400914D RID: 37197
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ActivatedBitAction;

		// Token: 0x0400914E RID: 37198
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_BitCredits;

		// Token: 0x0400914F RID: 37199
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_BitEvent;

		// Token: 0x04009150 RID: 37200
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_BitPotBalance;

		// Token: 0x04009151 RID: 37201
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ChannelPointEvent;

		// Token: 0x04009152 RID: 37202
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CharityEvent;

		// Token: 0x04009153 RID: 37203
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CooldownComplete;

		// Token: 0x04009154 RID: 37204
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CooldownStarted;

		// Token: 0x04009155 RID: 37205
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CooldownTime;

		// Token: 0x04009156 RID: 37206
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_Commands;

		// Token: 0x04009157 RID: 37207
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CreatorGoalEvent;

		// Token: 0x04009158 RID: 37208
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_DonateBits;

		// Token: 0x04009159 RID: 37209
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_DonateCharity;

		// Token: 0x0400915A RID: 37210
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_Gamestage;

		// Token: 0x0400915B RID: 37211
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GiftSubEvent;

		// Token: 0x0400915C RID: 37212
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GiftSubs;

		// Token: 0x0400915D RID: 37213
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_HypeTrainEvent;

		// Token: 0x0400915E RID: 37214
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledParty;

		// Token: 0x0400915F RID: 37215
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledStreamer;

		// Token: 0x04009160 RID: 37216
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledByBits;

		// Token: 0x04009161 RID: 37217
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledByHypeTrain;

		// Token: 0x04009162 RID: 37218
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledByVote;

		// Token: 0x04009163 RID: 37219
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_NewActions;

		// Token: 0x04009164 RID: 37220
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_PimpPotBalance;

		// Token: 0x04009165 RID: 37221
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_PointsWithSpecial;

		// Token: 0x04009166 RID: 37222
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_PointsWithoutSpecial;

		// Token: 0x04009167 RID: 37223
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_QueuedBitAction;

		// Token: 0x04009168 RID: 37224
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_RaidEvent;

		// Token: 0x04009169 RID: 37225
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_RaidPoints;

		// Token: 0x0400916A RID: 37226
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_SubEvent;

		// Token: 0x0400916B RID: 37227
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_Subscribed;

		// Token: 0x0400916C RID: 37228
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_ActivatedAction;

		// Token: 0x0400916D RID: 37229
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_BitRespawns;

		// Token: 0x0400916E RID: 37230
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_DonateBits;

		// Token: 0x0400916F RID: 37231
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_DonateCharity;

		// Token: 0x04009170 RID: 37232
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_GiftSubs;

		// Token: 0x04009171 RID: 37233
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledParty;

		// Token: 0x04009172 RID: 37234
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledStreamer;

		// Token: 0x04009173 RID: 37235
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledByBits;

		// Token: 0x04009174 RID: 37236
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledByHypeTrain;

		// Token: 0x04009175 RID: 37237
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledByVote;

		// Token: 0x04009176 RID: 37238
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_RaidPoints;

		// Token: 0x04009177 RID: 37239
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_RefundedAction;

		// Token: 0x04009178 RID: 37240
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_Subscribed;

		// Token: 0x04009179 RID: 37241
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameDeathScreen_Message;

		// Token: 0x0400917A RID: 37242
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameBitsDeathScreen_Message;

		// Token: 0x0400917B RID: 37243
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameHypeTrainDeathScreen_Message;

		// Token: 0x0400917C RID: 37244
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameVoteDeathScreen_Message;

		// Token: 0x0400917D RID: 37245
		[PublicizedFrom(EAccessModifier.Private)]
		public string subPointDisplay;

		// Token: 0x0400917E RID: 37246
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, TwitchRandomActionGroup> randomGroups = new Dictionary<string, TwitchRandomActionGroup>();

		// Token: 0x0400917F RID: 37247
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, List<TwitchAction>> randomKeys = new Dictionary<string, List<TwitchAction>>();

		// Token: 0x04009180 RID: 37248
		public List<BaseTwitchCommand> TwitchCommandList = new List<BaseTwitchCommand>();

		// Token: 0x04009181 RID: 37249
		[PublicizedFrom(EAccessModifier.Protected)]
		public Dictionary<EntityPlayer, TwitchManager.TwitchPartyMemberInfo> PartyInfo = new Dictionary<EntityPlayer, TwitchManager.TwitchPartyMemberInfo>();

		// Token: 0x04009182 RID: 37250
		[PublicizedFrom(EAccessModifier.Private)]
		public bool lastAlive;

		// Token: 0x04009183 RID: 37251
		public static string DeathText = "";

		// Token: 0x04009184 RID: 37252
		[PublicizedFrom(EAccessModifier.Private)]
		public bool twitchActive = true;

		// Token: 0x04009185 RID: 37253
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchActionEntry> QueuedActionEntries = new List<TwitchActionEntry>();

		// Token: 0x04009186 RID: 37254
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchActionEntry> LiveActionEntries = new List<TwitchActionEntry>();

		// Token: 0x04009187 RID: 37255
		public List<TwitchEventActionEntry> EventQueue = new List<TwitchEventActionEntry>();

		// Token: 0x04009188 RID: 37256
		public List<TwitchEventActionEntry> LiveEvents = new List<TwitchEventActionEntry>();

		// Token: 0x04009189 RID: 37257
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSpawnedEntityEntry> liveList = new List<TwitchSpawnedEntityEntry>();

		// Token: 0x0400918A RID: 37258
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSpawnedBlocksEntry> liveBlockList = new List<TwitchSpawnedBlocksEntry>();

		// Token: 0x0400918B RID: 37259
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchRecentlyRemovedEntityEntry> recentlyDeadList = new List<TwitchRecentlyRemovedEntityEntry>();

		// Token: 0x0400918C RID: 37260
		public List<TwitchSpawnedEntityEntry> actionSpawnLiveList = new List<TwitchSpawnedEntityEntry>();

		// Token: 0x0400918D RID: 37261
		public bool HasDataChanges;

		// Token: 0x0400918E RID: 37262
		public List<string> changedActionList = new List<string>();

		// Token: 0x0400918F RID: 37263
		public List<string> changedEnabledVoteList = new List<string>();

		// Token: 0x04009190 RID: 37264
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> ActionMessages = new List<string>();

		// Token: 0x04009191 RID: 37265
		[PublicizedFrom(EAccessModifier.Private)]
		public float nextDisplayCommandsTime;

		// Token: 0x02001875 RID: 6261
		public enum PimpPotSettings
		{
			// Token: 0x04009193 RID: 37267
			Disabled,
			// Token: 0x04009194 RID: 37268
			EnabledSP,
			// Token: 0x04009195 RID: 37269
			EnabledPP
		}

		// Token: 0x02001876 RID: 6262
		public enum IntegrationSettings
		{
			// Token: 0x04009197 RID: 37271
			ExtensionOnly,
			// Token: 0x04009198 RID: 37272
			Both
		}

		// Token: 0x02001877 RID: 6263
		public enum CooldownTypes
		{
			// Token: 0x0400919A RID: 37274
			None,
			// Token: 0x0400919B RID: 37275
			Startup,
			// Token: 0x0400919C RID: 37276
			Time,
			// Token: 0x0400919D RID: 37277
			MaxReached,
			// Token: 0x0400919E RID: 37278
			MaxReachedWaiting,
			// Token: 0x0400919F RID: 37279
			BloodMoonDisabled,
			// Token: 0x040091A0 RID: 37280
			BloodMoonCooldown,
			// Token: 0x040091A1 RID: 37281
			QuestDisabled,
			// Token: 0x040091A2 RID: 37282
			QuestCooldown,
			// Token: 0x040091A3 RID: 37283
			SafeCooldown,
			// Token: 0x040091A4 RID: 37284
			SafeCooldownExit
		}

		// Token: 0x02001878 RID: 6264
		public enum InitStates
		{
			// Token: 0x040091A6 RID: 37286
			Setup,
			// Token: 0x040091A7 RID: 37287
			None,
			// Token: 0x040091A8 RID: 37288
			WaitingForPermission,
			// Token: 0x040091A9 RID: 37289
			PermissionDenied,
			// Token: 0x040091AA RID: 37290
			WaitingForOAuth,
			// Token: 0x040091AB RID: 37291
			Authenticating,
			// Token: 0x040091AC RID: 37292
			Authenticated,
			// Token: 0x040091AD RID: 37293
			CheckingForExtension,
			// Token: 0x040091AE RID: 37294
			Ready,
			// Token: 0x040091AF RID: 37295
			ExtensionNotInstalled,
			// Token: 0x040091B0 RID: 37296
			Failed
		}

		// Token: 0x02001879 RID: 6265
		public class TwitchPartyMemberInfo
		{
			// Token: 0x040091B1 RID: 37297
			public bool LastOptedOut;

			// Token: 0x040091B2 RID: 37298
			public bool LastAlive = true;

			// Token: 0x040091B3 RID: 37299
			public float Cooldown;

			// Token: 0x040091B4 RID: 37300
			public bool NeedsRespawnEvent;
		}
	}
}
