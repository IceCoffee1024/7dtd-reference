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
	// Token: 0x020018A5 RID: 6309
	public class TwitchManager
	{
		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x0600C207 RID: 49671 RVA: 0x00472C63 File Offset: 0x00470E63
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

		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x0600C208 RID: 49672 RVA: 0x00472C7B File Offset: 0x00470E7B
		public static bool HasInstance
		{
			get
			{
				return TwitchManager.instance != null;
			}
		}

		// Token: 0x0600C209 RID: 49673 RVA: 0x00472C88 File Offset: 0x00470E88
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchManager()
		{
			this.ViewerData = new TwitchViewerData(this);
			this.VotingManager = new TwitchVotingManager(this);
			this.HighestGameStage = -1;
			this.UseProgression = true;
		}

		// Token: 0x0600C20A RID: 49674 RVA: 0x00472F61 File Offset: 0x00471161
		public void Cleanup()
		{
			this.Disconnect();
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.InitState == TwitchManager.InitStates.Ready)
			{
				this.SaveViewerData();
			}
			TwitchManager.instance = null;
		}

		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x0600C20B RID: 49675 RVA: 0x00472F8A File Offset: 0x0047118A
		// (set) Token: 0x0600C20C RID: 49676 RVA: 0x00472F92 File Offset: 0x00471192
		public byte CurrentFileVersion { get; set; }

		// Token: 0x1700186D RID: 6253
		// (get) Token: 0x0600C20D RID: 49677 RVA: 0x00472F9B File Offset: 0x0047119B
		// (set) Token: 0x0600C20E RID: 49678 RVA: 0x00472FA3 File Offset: 0x004711A3
		public byte CurrentMainFileVersion { get; set; }

		// Token: 0x1700186E RID: 6254
		// (get) Token: 0x0600C20F RID: 49679 RVA: 0x00472FAC File Offset: 0x004711AC
		// (set) Token: 0x0600C210 RID: 49680 RVA: 0x00472FB4 File Offset: 0x004711B4
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

		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x0600C211 RID: 49681 RVA: 0x00472FD6 File Offset: 0x004711D6
		// (set) Token: 0x0600C212 RID: 49682 RVA: 0x00472FDE File Offset: 0x004711DE
		public bool UseProgression
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001870 RID: 6256
		// (get) Token: 0x0600C213 RID: 49683 RVA: 0x00472FE7 File Offset: 0x004711E7
		// (set) Token: 0x0600C214 RID: 49684 RVA: 0x00472FEF File Offset: 0x004711EF
		public int HighestGameStage
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x0600C215 RID: 49685 RVA: 0x00472FF8 File Offset: 0x004711F8
		public static bool BossHordeActive
		{
			get
			{
				return TwitchManager.HasInstance && TwitchManager.Current.IsBossHordeActive;
			}
		}

		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x0600C216 RID: 49686 RVA: 0x0047300D File Offset: 0x0047120D
		// (set) Token: 0x0600C217 RID: 49687 RVA: 0x00473015 File Offset: 0x00471215
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

		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x0600C218 RID: 49688 RVA: 0x0047302D File Offset: 0x0047122D
		// (set) Token: 0x0600C219 RID: 49689 RVA: 0x00473035 File Offset: 0x00471235
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

		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x0600C21A RID: 49690 RVA: 0x0047304E File Offset: 0x0047124E
		public bool AllowActions
		{
			get
			{
				return !this.CurrentActionPreset.IsEmpty;
			}
		}

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x0600C21B RID: 49691 RVA: 0x0047305E File Offset: 0x0047125E
		public bool AllowEvents
		{
			get
			{
				return !this.CurrentEventPreset.IsEmpty;
			}
		}

		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x0600C21C RID: 49692 RVA: 0x0047306E File Offset: 0x0047126E
		public bool OnCooldown
		{
			get
			{
				return this.CooldownTime > 0f || this.CurrentCooldownPreset.CooldownType == CooldownPreset.CooldownTypes.Always;
			}
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x0600C21D RID: 49693 RVA: 0x0047308D File Offset: 0x0047128D
		// (set) Token: 0x0600C21E RID: 49694 RVA: 0x00473095 File Offset: 0x00471295
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

		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x0600C21F RID: 49695 RVA: 0x004730AD File Offset: 0x004712AD
		// (set) Token: 0x0600C220 RID: 49696 RVA: 0x004730B5 File Offset: 0x004712B5
		public TwitchVotingManager VotingManager
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001879 RID: 6265
		// (get) Token: 0x0600C221 RID: 49697 RVA: 0x004730BE File Offset: 0x004712BE
		// (set) Token: 0x0600C222 RID: 49698 RVA: 0x004730C6 File Offset: 0x004712C6
		public TwitchViewerData ViewerData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700187A RID: 6266
		// (get) Token: 0x0600C223 RID: 49699 RVA: 0x004730CF File Offset: 0x004712CF
		// (set) Token: 0x0600C224 RID: 49700 RVA: 0x004730D7 File Offset: 0x004712D7
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

		// Token: 0x1700187B RID: 6267
		// (get) Token: 0x0600C225 RID: 49701 RVA: 0x004730FA File Offset: 0x004712FA
		// (set) Token: 0x0600C226 RID: 49702 RVA: 0x00473104 File Offset: 0x00471304
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

		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x0600C227 RID: 49703 RVA: 0x00473144 File Offset: 0x00471344
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

		// Token: 0x1700187D RID: 6269
		// (get) Token: 0x0600C228 RID: 49704 RVA: 0x004731EF File Offset: 0x004713EF
		public bool IsReady
		{
			get
			{
				return this.initState == TwitchManager.InitStates.Ready;
			}
		}

		// Token: 0x1700187E RID: 6270
		// (get) Token: 0x0600C229 RID: 49705 RVA: 0x004731FA File Offset: 0x004713FA
		public bool IsVoting
		{
			get
			{
				return this.initState == TwitchManager.InitStates.Ready && this.VotingManager.VotingIsActive;
			}
		}

		// Token: 0x1700187F RID: 6271
		// (get) Token: 0x0600C22A RID: 49706 RVA: 0x00473212 File Offset: 0x00471412
		public bool IsBossHordeActive
		{
			get
			{
				return this.initState == TwitchManager.InitStates.Ready && (this.VotingManager.CurrentVoteState == TwitchVotingManager.VoteStateTypes.EventActive || this.VotingManager.CurrentVoteState == TwitchVotingManager.VoteStateTypes.WaitingForActive);
			}
		}

		// Token: 0x17001880 RID: 6272
		// (get) Token: 0x0600C22B RID: 49707 RVA: 0x0047323D File Offset: 0x0047143D
		public bool ReadyForVote
		{
			get
			{
				return this.actionSpawnLiveList.Count == 0;
			}
		}

		// Token: 0x17001881 RID: 6273
		// (get) Token: 0x0600C22C RID: 49708 RVA: 0x0047324D File Offset: 0x0047144D
		// (set) Token: 0x0600C22D RID: 49709 RVA: 0x00473255 File Offset: 0x00471455
		public bool IsSafe
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x0600C22E RID: 49710 RVA: 0x00473260 File Offset: 0x00471460
		// (remove) Token: 0x0600C22F RID: 49711 RVA: 0x00473298 File Offset: 0x00471498
		public event OnTwitchConnectionStateChange ConnectionStateChanged;

		// Token: 0x1400010E RID: 270
		// (add) Token: 0x0600C230 RID: 49712 RVA: 0x004732D0 File Offset: 0x004714D0
		// (remove) Token: 0x0600C231 RID: 49713 RVA: 0x00473308 File Offset: 0x00471508
		public event OnCommandsChanged CommandsChanged;

		// Token: 0x1400010F RID: 271
		// (add) Token: 0x0600C232 RID: 49714 RVA: 0x00473340 File Offset: 0x00471540
		// (remove) Token: 0x0600C233 RID: 49715 RVA: 0x00473378 File Offset: 0x00471578
		public event OnHistoryAdded ActionHistoryAdded;

		// Token: 0x14000110 RID: 272
		// (add) Token: 0x0600C234 RID: 49716 RVA: 0x004733B0 File Offset: 0x004715B0
		// (remove) Token: 0x0600C235 RID: 49717 RVA: 0x004733E8 File Offset: 0x004715E8
		public event OnHistoryAdded VoteHistoryAdded;

		// Token: 0x14000111 RID: 273
		// (add) Token: 0x0600C236 RID: 49718 RVA: 0x00473420 File Offset: 0x00471620
		// (remove) Token: 0x0600C237 RID: 49719 RVA: 0x00473458 File Offset: 0x00471658
		public event OnHistoryAdded EventHistoryAdded;

		// Token: 0x17001882 RID: 6274
		// (get) Token: 0x0600C238 RID: 49720 RVA: 0x0047348D File Offset: 0x0047168D
		public bool HasCustomEvents
		{
			get
			{
				return this.EventPresets.Count > 0;
			}
		}

		// Token: 0x0600C239 RID: 49721 RVA: 0x004734A0 File Offset: 0x004716A0
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

		// Token: 0x0600C23A RID: 49722 RVA: 0x00473854 File Offset: 0x00471A54
		public void SetupClient(string twitchChannel, string password)
		{
			this.ircClient = new TwitchIRCClient("irc.twitch.tv", 6667, twitchChannel, password);
			if (this.EventSub == null)
			{
				this.EventSub = new EventSubClient(this.Authentication.userID, this.Authentication.oauth.Substring(6), TwitchAuthentication.client_id);
			}
		}

		// Token: 0x0600C23B RID: 49723 RVA: 0x004738AC File Offset: 0x00471AAC
		public void IntegrationTypeChanged()
		{
			if (this.IsReady && this.extensionManager == null)
			{
				this.extensionManager = new ExtensionManager();
				this.extensionManager.Init();
			}
		}

		// Token: 0x0600C23C RID: 49724 RVA: 0x004738D4 File Offset: 0x00471AD4
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

		// Token: 0x0600C23D RID: 49725 RVA: 0x0047393C File Offset: 0x00471B3C
		[PublicizedFrom(EAccessModifier.Private)]
		public void RemoveChannelPointRedeems()
		{
			if (this.CurrentEventPreset != null)
			{
				this.CurrentEventPreset.RemoveChannelPointRedemptions(null);
			}
		}

		// Token: 0x0600C23E RID: 49726 RVA: 0x00473952 File Offset: 0x00471B52
		public void CleanupEventData()
		{
			this.RemoveChannelPointRedeems();
			this.CurrentEventPreset = null;
			this.EventPresets.Clear();
		}

		// Token: 0x0600C23F RID: 49727 RVA: 0x0047396C File Offset: 0x00471B6C
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

		// Token: 0x0600C240 RID: 49728 RVA: 0x00473A36 File Offset: 0x00471C36
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

		// Token: 0x0600C241 RID: 49729 RVA: 0x00473A68 File Offset: 0x00471C68
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

		// Token: 0x0600C242 RID: 49730 RVA: 0x00473CA8 File Offset: 0x00471EA8
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

		// Token: 0x0600C243 RID: 49731 RVA: 0x00473EE4 File Offset: 0x004720E4
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

		// Token: 0x0600C244 RID: 49732 RVA: 0x00473F68 File Offset: 0x00472168
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

		// Token: 0x0600C245 RID: 49733 RVA: 0x00473FCE File Offset: 0x004721CE
		public void WaitForOAuth()
		{
			this.updateTime = 10f;
			this.InitState = TwitchManager.InitStates.WaitingForOAuth;
		}

		// Token: 0x0600C246 RID: 49734 RVA: 0x00473FE2 File Offset: 0x004721E2
		public void WaitForPermission()
		{
			this.updateTime = 10f;
			this.InitState = TwitchManager.InitStates.WaitingForPermission;
		}

		// Token: 0x0600C247 RID: 49735 RVA: 0x00473FF6 File Offset: 0x004721F6
		public void DeniedPermission()
		{
			this.InitState = TwitchManager.InitStates.PermissionDenied;
		}

		// Token: 0x0600C248 RID: 49736 RVA: 0x00474000 File Offset: 0x00472200
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

		// Token: 0x0600C249 RID: 49737 RVA: 0x00474174 File Offset: 0x00472374
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

		// Token: 0x0600C24A RID: 49738 RVA: 0x004741EC File Offset: 0x004723EC
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

		// Token: 0x0600C24B RID: 49739 RVA: 0x004754FE File Offset: 0x004736FE
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleEndCooldown()
		{
			this.CurrentCooldownFill = 0f;
			Manager.BroadcastPlayByLocalPlayer(this.LocalPlayer.position, "twitch_cooldown_ended");
			this.ircClient.SendChannelMessage(this.chatOutput_CooldownComplete, true);
			this.HandleEndCooldownStateChanging();
		}

		// Token: 0x0600C24C RID: 49740 RVA: 0x00475538 File Offset: 0x00473738
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

		// Token: 0x0600C24D RID: 49741 RVA: 0x00475599 File Offset: 0x00473799
		public void AddToInGameChatQueue(string msg, string sound = null)
		{
			this.inGameChatQueue.Add(new TwitchMessageEntry(msg, sound));
		}

		// Token: 0x0600C24E RID: 49742 RVA: 0x004755B0 File Offset: 0x004737B0
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

		// Token: 0x0600C24F RID: 49743 RVA: 0x0047563B File Offset: 0x0047383B
		public void RefreshVoteLockedLevel()
		{
			this.VoteLockedLevel = this.LocalPlayer.HasTwitchVoteLockMember();
		}

		// Token: 0x0600C250 RID: 49744 RVA: 0x00475650 File Offset: 0x00473850
		public void SetupBloodMoonData()
		{
			ValueTuple<int, int> valueTuple = GameUtils.CalcDuskDawnHours(GameStats.GetInt(EnumGameStats.DayLightLength));
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			this.BMCooldownStart = item - this.CurrentCooldownPreset.BMStartOffset;
			this.BMCooldownEnd = item2 + this.CurrentCooldownPreset.BMEndOffset;
		}

		// Token: 0x0600C251 RID: 49745 RVA: 0x0047569C File Offset: 0x0047389C
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

		// Token: 0x0600C252 RID: 49746 RVA: 0x004756EE File Offset: 0x004738EE
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

		// Token: 0x0600C253 RID: 49747 RVA: 0x00475725 File Offset: 0x00473925
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

		// Token: 0x0600C254 RID: 49748 RVA: 0x00475758 File Offset: 0x00473958
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

		// Token: 0x0600C255 RID: 49749 RVA: 0x004757FC File Offset: 0x004739FC
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

		// Token: 0x0600C256 RID: 49750 RVA: 0x004759A8 File Offset: 0x00473BA8
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

		// Token: 0x0600C257 RID: 49751 RVA: 0x00475B08 File Offset: 0x00473D08
		public bool CheckCanRespawnEvent(EntityPlayer player)
		{
			return player != null && player.IsAlive() && player.TwitchActionsEnabled == EntityPlayer.TwitchActionsStates.Enabled && !player.TwitchSafe;
		}

		// Token: 0x0600C258 RID: 49752 RVA: 0x00475B30 File Offset: 0x00473D30
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

		// Token: 0x0600C259 RID: 49753 RVA: 0x00475D60 File Offset: 0x00473F60
		[PublicizedFrom(EAccessModifier.Private)]
		public void LocalPlayer_PartyChanged(Party _affectedParty, EntityPlayer _player)
		{
			if (this.extensionManager != null)
			{
				this.extensionManager.OnPartyChanged();
			}
			this.GetCooldownMax();
		}

		// Token: 0x0600C25A RID: 49754 RVA: 0x00475D60 File Offset: 0x00473F60
		[PublicizedFrom(EAccessModifier.Private)]
		public void LocalPlayer_PartyLeave(Party _affectedParty, EntityPlayer _player)
		{
			if (this.extensionManager != null)
			{
				this.extensionManager.OnPartyChanged();
			}
			this.GetCooldownMax();
		}

		// Token: 0x0600C25B RID: 49755 RVA: 0x00475D7C File Offset: 0x00473F7C
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

		// Token: 0x0600C25C RID: 49756 RVA: 0x00475DF0 File Offset: 0x00473FF0
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

		// Token: 0x0600C25D RID: 49757 RVA: 0x00475E84 File Offset: 0x00474084
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

		// Token: 0x0600C25E RID: 49758 RVA: 0x00475FB5 File Offset: 0x004741B5
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_GameEventAccessApproved()
		{
			if (this.InitState == TwitchManager.InitStates.None || this.InitState == TwitchManager.InitStates.WaitingForPermission)
			{
				this.StartTwitchIntegration();
			}
		}

		// Token: 0x0600C25F RID: 49759 RVA: 0x00475FD0 File Offset: 0x004741D0
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

		// Token: 0x0600C260 RID: 49760 RVA: 0x00476110 File Offset: 0x00474310
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

		// Token: 0x0600C261 RID: 49761 RVA: 0x004761BC File Offset: 0x004743BC
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

		// Token: 0x0600C262 RID: 49762 RVA: 0x00476344 File Offset: 0x00474544
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

		// Token: 0x0600C263 RID: 49763 RVA: 0x0047644C File Offset: 0x0047464C
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

		// Token: 0x0600C264 RID: 49764 RVA: 0x00476708 File Offset: 0x00474908
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

		// Token: 0x0600C265 RID: 49765 RVA: 0x0047691C File Offset: 0x00474B1C
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

		// Token: 0x0600C266 RID: 49766 RVA: 0x004769A8 File Offset: 0x00474BA8
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

		// Token: 0x0600C267 RID: 49767 RVA: 0x004769F8 File Offset: 0x00474BF8
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

		// Token: 0x0600C268 RID: 49768 RVA: 0x00476A70 File Offset: 0x00474C70
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

		// Token: 0x0600C269 RID: 49769 RVA: 0x00476B84 File Offset: 0x00474D84
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

		// Token: 0x0600C26A RID: 49770 RVA: 0x00477170 File Offset: 0x00475370
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

		// Token: 0x0600C26B RID: 49771 RVA: 0x004776E8 File Offset: 0x004758E8
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

		// Token: 0x0600C26C RID: 49772 RVA: 0x00477870 File Offset: 0x00475A70
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

		// Token: 0x0600C26D RID: 49773 RVA: 0x004778C0 File Offset: 0x00475AC0
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

		// Token: 0x0600C26E RID: 49774 RVA: 0x00477910 File Offset: 0x00475B10
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

		// Token: 0x0600C26F RID: 49775 RVA: 0x00477970 File Offset: 0x00475B70
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

		// Token: 0x0600C270 RID: 49776 RVA: 0x004779D0 File Offset: 0x00475BD0
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

		// Token: 0x0600C271 RID: 49777 RVA: 0x00477A98 File Offset: 0x00475C98
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

		// Token: 0x0600C272 RID: 49778 RVA: 0x00477AF4 File Offset: 0x00475CF4
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

		// Token: 0x0600C273 RID: 49779 RVA: 0x00477C78 File Offset: 0x00475E78
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

		// Token: 0x0600C274 RID: 49780 RVA: 0x00477CFA File Offset: 0x00475EFA
		public void ShowReimburseMessage(TwitchActionEntry entry, ViewerEntry viewerEntry = null)
		{
			this.ShowReimburseMessage(entry.UserName, entry.BitsUsed, entry.Action, viewerEntry);
		}

		// Token: 0x0600C275 RID: 49781 RVA: 0x00477D18 File Offset: 0x00475F18
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

		// Token: 0x0600C276 RID: 49782 RVA: 0x00477DAC File Offset: 0x00475FAC
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

		// Token: 0x0600C277 RID: 49783 RVA: 0x00477E20 File Offset: 0x00476020
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

		// Token: 0x0600C278 RID: 49784 RVA: 0x00477E6C File Offset: 0x0047606C
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

		// Token: 0x0600C279 RID: 49785 RVA: 0x00477F7F File Offset: 0x0047617F
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

		// Token: 0x0600C27A RID: 49786 RVA: 0x00477F9C File Offset: 0x0047619C
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

		// Token: 0x0600C27B RID: 49787 RVA: 0x00478110 File Offset: 0x00476310
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

		// Token: 0x0600C27C RID: 49788 RVA: 0x004781CC File Offset: 0x004763CC
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

		// Token: 0x0600C27D RID: 49789 RVA: 0x00478251 File Offset: 0x00476451
		public void ToggleTwitchActive()
		{
			this.twitchActive = !this.twitchActive;
			this.ActionMessages.Clear();
			this.HandleCooldownActionLocking();
		}

		// Token: 0x0600C27E RID: 49790 RVA: 0x00478273 File Offset: 0x00476473
		public void SetTwitchActive(bool newActive)
		{
			if (this.twitchActive != newActive)
			{
				this.twitchActive = newActive;
				this.ActionMessages.Clear();
				this.HandleCooldownActionLocking();
			}
		}

		// Token: 0x0600C27F RID: 49791 RVA: 0x00478298 File Offset: 0x00476498
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

		// Token: 0x0600C280 RID: 49792 RVA: 0x00478304 File Offset: 0x00476504
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

		// Token: 0x0600C281 RID: 49793 RVA: 0x00478374 File Offset: 0x00476574
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

		// Token: 0x0600C282 RID: 49794 RVA: 0x004783B1 File Offset: 0x004765B1
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

		// Token: 0x0600C283 RID: 49795 RVA: 0x004783D4 File Offset: 0x004765D4
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

		// Token: 0x0600C284 RID: 49796 RVA: 0x004784A4 File Offset: 0x004766A4
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

		// Token: 0x0600C285 RID: 49797 RVA: 0x0047851C File Offset: 0x0047671C
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

		// Token: 0x0600C286 RID: 49798 RVA: 0x00478E10 File Offset: 0x00477010
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

		// Token: 0x0600C287 RID: 49799 RVA: 0x00478F38 File Offset: 0x00477138
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

		// Token: 0x0600C288 RID: 49800 RVA: 0x00478FA9 File Offset: 0x004771A9
		public void ClearLeaderboard()
		{
			this.Leaderboard.Clear();
		}

		// Token: 0x0600C289 RID: 49801 RVA: 0x00478FB6 File Offset: 0x004771B6
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

		// Token: 0x0600C28A RID: 49802 RVA: 0x00478FF0 File Offset: 0x004771F0
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

		// Token: 0x0600C28B RID: 49803 RVA: 0x00479048 File Offset: 0x00477248
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

		// Token: 0x0600C28C RID: 49804 RVA: 0x00479086 File Offset: 0x00477286
		public void GetCooldownMax()
		{
			this.CurrentCooldownPreset = this.CooldownPresets[this.CooldownPresetIndex];
			this.CurrentCooldownPreset.SetupCooldownInfo(this.HighestGameStage, this.LocalPlayer);
			this.SetupBloodMoonData();
		}

		// Token: 0x0600C28D RID: 49805 RVA: 0x004790BC File Offset: 0x004772BC
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

		// Token: 0x0600C28E RID: 49806 RVA: 0x0047911C File Offset: 0x0047731C
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

		// Token: 0x0600C28F RID: 49807 RVA: 0x004791A4 File Offset: 0x004773A4
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

		// Token: 0x0600C290 RID: 49808 RVA: 0x004791E4 File Offset: 0x004773E4
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

		// Token: 0x0600C291 RID: 49809 RVA: 0x00479244 File Offset: 0x00477444
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

		// Token: 0x0600C292 RID: 49810 RVA: 0x00479298 File Offset: 0x00477498
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

		// Token: 0x0600C293 RID: 49811 RVA: 0x004792D8 File Offset: 0x004774D8
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

		// Token: 0x0600C294 RID: 49812 RVA: 0x00479338 File Offset: 0x00477538
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

		// Token: 0x0600C295 RID: 49813 RVA: 0x004793A8 File Offset: 0x004775A8
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

		// Token: 0x0600C296 RID: 49814 RVA: 0x004793F4 File Offset: 0x004775F4
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

		// Token: 0x0600C297 RID: 49815 RVA: 0x00479438 File Offset: 0x00477638
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

		// Token: 0x0600C298 RID: 49816 RVA: 0x0047953C File Offset: 0x0047773C
		public void EventSub_OnSubGifted(SubscriptionGiftEvent e)
		{
			if (e.UserName != null && !e.IsAnonymous)
			{
				this.ViewerData.AddGiftSubEntry(e.UserName.ToLower(), StringParsers.ParseSInt32(e.UserId, 0, -1, NumberStyles.Integer), TwitchSubEventEntry.GetSubTier(e.Tier), e.Total);
			}
		}

		// Token: 0x0600C299 RID: 49817 RVA: 0x0047958E File Offset: 0x0047778E
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

		// Token: 0x0600C29A RID: 49818 RVA: 0x004795C8 File Offset: 0x004777C8
		public string GetSubTierRewards(int subModifier)
		{
			if (subModifier == 0)
			{
				return Localization.Get("xuiLightPropShadowsNone", false, null);
			}
			return string.Format(this.subPointDisplay, this.ViewerData.GetSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier1) * subModifier, this.ViewerData.GetSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier2) * subModifier, this.ViewerData.GetSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier3) * subModifier);
		}

		// Token: 0x0600C29B RID: 49819 RVA: 0x0047962C File Offset: 0x0047782C
		public string GetGiftSubTierRewards(int subModifier)
		{
			if (subModifier == 0)
			{
				return Localization.Get("xuiLightPropShadowsNone", false, null);
			}
			return string.Format(this.subPointDisplay, this.ViewerData.GetGiftSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier1) * subModifier, this.ViewerData.GetGiftSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier2) * subModifier, this.ViewerData.GetGiftSubTierPoints(TwitchSubEventEntry.SubTierTypes.Tier3) * subModifier);
		}

		// Token: 0x0600C29C RID: 49820 RVA: 0x00479690 File Offset: 0x00477890
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

		// Token: 0x0600C29D RID: 49821 RVA: 0x0047971C File Offset: 0x0047791C
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

		// Token: 0x0600C29E RID: 49822 RVA: 0x004797A8 File Offset: 0x004779A8
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

		// Token: 0x0600C29F RID: 49823 RVA: 0x00479A7C File Offset: 0x00477C7C
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

		// Token: 0x0600C2A0 RID: 49824 RVA: 0x00479B60 File Offset: 0x00477D60
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

		// Token: 0x0600C2A1 RID: 49825 RVA: 0x00479C3C File Offset: 0x00477E3C
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

		// Token: 0x0600C2A2 RID: 49826 RVA: 0x00479CC9 File Offset: 0x00477EC9
		[PublicizedFrom(EAccessModifier.Private)]
		public void PubSub_OnChannelPointsRedeemed(object sender, PubSubChannelPointMessage.ChannelRedemptionData e)
		{
			this.HandleChannelPointsRedeem(e.redemption.reward.title, e.redemption.user.display_name.ToLower());
		}

		// Token: 0x0600C2A3 RID: 49827 RVA: 0x00479CF6 File Offset: 0x00477EF6
		[PublicizedFrom(EAccessModifier.Private)]
		public void EventSub_OnChannelPointsRedeemed(ChannelPointsRedemptionEvent e)
		{
			this.HandleChannelPointsRedeem(e.Reward.Title, e.UserLogin.ToLower());
		}

		// Token: 0x0600C2A4 RID: 49828 RVA: 0x00479D14 File Offset: 0x00477F14
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

		// Token: 0x0600C2A5 RID: 49829 RVA: 0x00479DB0 File Offset: 0x00477FB0
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

		// Token: 0x0600C2A6 RID: 49830 RVA: 0x00479E74 File Offset: 0x00478074
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

		// Token: 0x0600C2A7 RID: 49831 RVA: 0x00479F1C File Offset: 0x0047811C
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

		// Token: 0x0600C2A8 RID: 49832 RVA: 0x00479FC0 File Offset: 0x004781C0
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

		// Token: 0x0600C2A9 RID: 49833 RVA: 0x0047A04D File Offset: 0x0047824D
		public void StartHypeTrain()
		{
			this.HypeTrainLevel = 1;
			this.HandleHypeTrainRedeem(this.HypeTrainLevel);
		}

		// Token: 0x0600C2AA RID: 49834 RVA: 0x0047A062 File Offset: 0x00478262
		public void IncrementHypeTrainLevel()
		{
			this.HypeTrainLevel++;
			this.HandleHypeTrainRedeem(this.HypeTrainLevel);
		}

		// Token: 0x0600C2AB RID: 49835 RVA: 0x0047A07E File Offset: 0x0047827E
		public void EndHypeTrain()
		{
			this.HypeTrainLevel = 0;
		}

		// Token: 0x0600C2AC RID: 49836 RVA: 0x0047A088 File Offset: 0x00478288
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

		// Token: 0x0600C2AD RID: 49837 RVA: 0x0047A107 File Offset: 0x00478307
		[PublicizedFrom(EAccessModifier.Private)]
		public void PubSub_OnGoalAchieved(object sender, PubSubGoalMessage.Goal e)
		{
			this.HandleCreatorGoalRedeem(e.contributionType.ToLower());
		}

		// Token: 0x0600C2AE RID: 49838 RVA: 0x0047A11C File Offset: 0x0047831C
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

		// Token: 0x0600C2AF RID: 49839 RVA: 0x0047A198 File Offset: 0x00478398
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

		// Token: 0x17001883 RID: 6275
		// (get) Token: 0x0600C2B0 RID: 49840 RVA: 0x0047A278 File Offset: 0x00478478
		public bool TwitchActive
		{
			get
			{
				return this.twitchActive;
			}
		}

		// Token: 0x0600C2B1 RID: 49841 RVA: 0x0047A280 File Offset: 0x00478480
		public void SaveExportViewerData(string _filePath)
		{
			this.ViewerData.WriteExport(_filePath);
		}

		// Token: 0x0600C2B2 RID: 49842 RVA: 0x0047A290 File Offset: 0x00478490
		public void LoadExportViewerData(string _filePath)
		{
			using (StreamReader streamReader = File.OpenText(_filePath))
			{
				this.ViewerData.LoadExport(streamReader);
			}
		}

		// Token: 0x0600C2B3 RID: 49843 RVA: 0x0047A2CC File Offset: 0x004784CC
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

		// Token: 0x0600C2B4 RID: 49844 RVA: 0x0047A3BC File Offset: 0x004785BC
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

		// Token: 0x0600C2B5 RID: 49845 RVA: 0x0047A4D8 File Offset: 0x004786D8
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

		// Token: 0x0600C2B6 RID: 49846 RVA: 0x0047A5E4 File Offset: 0x004787E4
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

		// Token: 0x0600C2B7 RID: 49847 RVA: 0x0047A6F0 File Offset: 0x004788F0
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

		// Token: 0x0600C2B8 RID: 49848 RVA: 0x0047A71C File Offset: 0x0047891C
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

		// Token: 0x0600C2B9 RID: 49849 RVA: 0x0047A848 File Offset: 0x00478A48
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

		// Token: 0x0600C2BA RID: 49850 RVA: 0x0047A894 File Offset: 0x00478A94
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

		// Token: 0x0600C2BB RID: 49851 RVA: 0x0047A96C File Offset: 0x00478B6C
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

		// Token: 0x0600C2BC RID: 49852 RVA: 0x0047ADE8 File Offset: 0x00478FE8
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

		// Token: 0x0600C2BD RID: 49853 RVA: 0x0047AEE0 File Offset: 0x004790E0
		public void WriteSpecial(PooledBinaryWriter bw)
		{
			this.ViewerData.WriteSpecial(bw);
		}

		// Token: 0x0600C2BE RID: 49854 RVA: 0x0047AEEE File Offset: 0x004790EE
		public void WriteMain(PooledBinaryWriter bw)
		{
			bw.Write(TwitchManager.MainFileVersion);
			bw.Write(this.HasViewedSettings);
			this.ViewerData.WriteSpecial(bw);
		}

		// Token: 0x0600C2BF RID: 49855 RVA: 0x0047AF14 File Offset: 0x00479114
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

		// Token: 0x0600C2C0 RID: 49856 RVA: 0x0047B5A7 File Offset: 0x004797A7
		public void ReadSpecial(PooledBinaryReader br)
		{
			this.ViewerData.ReadSpecial(br, 1);
		}

		// Token: 0x0600C2C1 RID: 49857 RVA: 0x0047B5B6 File Offset: 0x004797B6
		public void ReadMain(PooledBinaryReader br)
		{
			this.CurrentMainFileVersion = br.ReadByte();
			this.HasViewedSettings = br.ReadBoolean();
			this.ViewerData.ReadSpecial(br, this.CurrentMainFileVersion);
		}

		// Token: 0x0600C2C2 RID: 49858 RVA: 0x0047B5E4 File Offset: 0x004797E4
		public void SendChannelPointOutputMessage(string name, ViewerEntry entry)
		{
			if (entry.SpecialPoints == 0f)
			{
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_PointsWithoutSpecial, name, entry.CombinedPoints), true);
				return;
			}
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_PointsWithSpecial, name, entry.CombinedPoints, entry.SpecialPoints), true);
		}

		// Token: 0x0600C2C3 RID: 49859 RVA: 0x0047B650 File Offset: 0x00479850
		public void SendChannelCreditOutputMessage(string name, ViewerEntry entry)
		{
			this.ircClient.SendChannelMessage(string.Format(this.chatOutput_BitCredits, name, entry.BitCredits), true);
		}

		// Token: 0x0600C2C4 RID: 49860 RVA: 0x0047B675 File Offset: 0x00479875
		public void SendChannelPointOutputMessage(string name)
		{
			this.SendChannelPointOutputMessage(name, this.ViewerData.GetViewerEntry(name));
		}

		// Token: 0x0600C2C5 RID: 49861 RVA: 0x0047B68A File Offset: 0x0047988A
		public void SendChannelCreditOutputMessage(string name)
		{
			this.SendChannelCreditOutputMessage(name, this.ViewerData.GetViewerEntry(name));
		}

		// Token: 0x0600C2C6 RID: 49862 RVA: 0x0047B69F File Offset: 0x0047989F
		public void SendChannelMessage(string message, bool useQueue = true)
		{
			this.ircClient.SendChannelMessage(message, useQueue);
		}

		// Token: 0x0600C2C7 RID: 49863 RVA: 0x0047B6B0 File Offset: 0x004798B0
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

		// Token: 0x0600C2C8 RID: 49864 RVA: 0x0047B718 File Offset: 0x00479918
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

		// Token: 0x0600C2C9 RID: 49865 RVA: 0x0047B9B4 File Offset: 0x00479BB4
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

		// Token: 0x0600C2CA RID: 49866 RVA: 0x0047BCDC File Offset: 0x00479EDC
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

		// Token: 0x0600C2CB RID: 49867 RVA: 0x0047BDB8 File Offset: 0x00479FB8
		public void PushBalanceToExtensionQueue(string userID, int creditBalance)
		{
			if (this.extensionManager != null)
			{
				this.extensionManager.PushUserBalance(new ValueTuple<string, int>(userID, creditBalance));
			}
		}

		// Token: 0x0600C2CC RID: 49868 RVA: 0x0047BDD4 File Offset: 0x00479FD4
		public void DisplayActions()
		{
			int count = this.ActionMessages.Count;
			if (this.CurrentUnityTime > this.nextDisplayCommandsTime)
			{
				this.nextDisplayCommandsTime = this.CurrentUnityTime + 15f;
				this.ircClient.SendChannelMessages(this.ActionMessages, true);
			}
		}

		// Token: 0x0600C2CD RID: 49869 RVA: 0x0047BE14 File Offset: 0x0047A014
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

		// Token: 0x0600C2CE RID: 49870 RVA: 0x0047BF10 File Offset: 0x0047A110
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

		// Token: 0x0600C2CF RID: 49871 RVA: 0x0047BF5E File Offset: 0x0047A15E
		public void DisplayGameStage()
		{
			if (this.LocalPlayer != null)
			{
				this.ircClient.SendChannelMessage(string.Format(this.chatOutput_Gamestage, this.LocalPlayer.unModifiedGameStage), true);
			}
		}

		// Token: 0x0600C2D0 RID: 49872 RVA: 0x0047BF95 File Offset: 0x0047A195
		public bool CheckIfTwitchKill(EntityPlayer player)
		{
			return this.twitchPlayerDeathsThisFrame.Contains(player);
		}

		// Token: 0x0600C2D1 RID: 49873 RVA: 0x0047BFA4 File Offset: 0x0047A1A4
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

		// Token: 0x0600C2D6 RID: 49878 RVA: 0x0047C116 File Offset: 0x0047A316
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool <LoadMainViewerData>g__LoadViewerDataFromStorage|386_0(UserDataStorageType storageType)
		{
			return this.LoadLatestMainViewerData(storageType) || this.LoadMainViewerData(storageType) || this.LoadSpecialViewerData(storageType);
		}

		// Token: 0x040091C6 RID: 37318
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchManager instance = null;

		// Token: 0x040091C7 RID: 37319
		[PublicizedFrom(EAccessModifier.Private)]
		public const float SAVE_TIME_SEC = 30f;

		// Token: 0x040091C8 RID: 37320
		public float saveTime;

		// Token: 0x040091C9 RID: 37321
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreadManager.ThreadInfo dataSaveThreadInfo;

		// Token: 0x040091CA RID: 37322
		public static byte FileVersion = 26;

		// Token: 0x040091CC RID: 37324
		public static byte MainFileVersion = 3;

		// Token: 0x040091CE RID: 37326
		public TwitchIRCClient ircClient;

		// Token: 0x040091CF RID: 37327
		public ExtensionManager extensionManager;

		// Token: 0x040091D0 RID: 37328
		[PublicizedFrom(EAccessModifier.Private)]
		public int resetClientAttempts;

		// Token: 0x040091D1 RID: 37329
		[PublicizedFrom(EAccessModifier.Private)]
		public bool overrideProgression;

		// Token: 0x040091D4 RID: 37332
		[PublicizedFrom(EAccessModifier.Private)]
		public int commandsAvailable = -1;

		// Token: 0x040091D5 RID: 37333
		public Dictionary<string, TwitchAction> AvailableCommands = new Dictionary<string, TwitchAction>();

		// Token: 0x040091D6 RID: 37334
		public Dictionary<string, string> AlternateCommands = new Dictionary<string, string>();

		// Token: 0x040091D7 RID: 37335
		public TwitchManager.PimpPotSettings PimpPotType = TwitchManager.PimpPotSettings.EnabledSP;

		// Token: 0x040091D8 RID: 37336
		public static int PimpPotDefault = 500;

		// Token: 0x040091D9 RID: 37337
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchManager.IntegrationSettings integrationSetting = TwitchManager.IntegrationSettings.Both;

		// Token: 0x040091DA RID: 37338
		[PublicizedFrom(EAccessModifier.Private)]
		public float actionCooldownModifier = 1f;

		// Token: 0x040091DB RID: 37339
		[PublicizedFrom(EAccessModifier.Private)]
		public const int HistoryItemMax = 500;

		// Token: 0x040091DC RID: 37340
		public float ActionPotPercentage = 0.15f;

		// Token: 0x040091DD RID: 37341
		public float BitPotPercentage = 0.25f;

		// Token: 0x040091DE RID: 37342
		public int RewardPot = TwitchManager.PimpPotDefault;

		// Token: 0x040091DF RID: 37343
		public int BitPot;

		// Token: 0x040091E0 RID: 37344
		public int PartyKillRewardMax = 250;

		// Token: 0x040091E1 RID: 37345
		public EntityPlayerLocal LocalPlayer;

		// Token: 0x040091E2 RID: 37346
		public bool LocalPlayerInLandClaim;

		// Token: 0x040091E3 RID: 37347
		public TwitchManager.CooldownTypes CooldownType = TwitchManager.CooldownTypes.Startup;

		// Token: 0x040091E4 RID: 37348
		public float CooldownTime = 300f;

		// Token: 0x040091E5 RID: 37349
		public float CurrentCooldownFill;

		// Token: 0x040091E6 RID: 37350
		public float CooldownFillMax = 50f;

		// Token: 0x040091E7 RID: 37351
		public int NextCooldownTime = 180;

		// Token: 0x040091E8 RID: 37352
		public bool AllowCrateSharing;

		// Token: 0x040091E9 RID: 37353
		public bool AllowBitEvents = true;

		// Token: 0x040091EA RID: 37354
		public bool AllowSubEvents = true;

		// Token: 0x040091EB RID: 37355
		public bool AllowGiftSubEvents = true;

		// Token: 0x040091EC RID: 37356
		public bool AllowCharityEvents = true;

		// Token: 0x040091ED RID: 37357
		public bool AllowRaidEvents = true;

		// Token: 0x040091EE RID: 37358
		public bool AllowHypeTrainEvents = true;

		// Token: 0x040091EF RID: 37359
		public bool AllowCreatorGoalEvents = true;

		// Token: 0x040091F0 RID: 37360
		public bool AllowChannelPointRedemptions = true;

		// Token: 0x040091F1 RID: 37361
		public List<CooldownPreset> CooldownPresets = new List<CooldownPreset>();

		// Token: 0x040091F2 RID: 37362
		public int CooldownPresetIndex;

		// Token: 0x040091F3 RID: 37363
		public CooldownPreset CurrentCooldownPreset;

		// Token: 0x040091F4 RID: 37364
		public List<TwitchActionPreset> ActionPresets = new List<TwitchActionPreset>();

		// Token: 0x040091F5 RID: 37365
		public List<TwitchVotePreset> VotePresets = new List<TwitchVotePreset>();

		// Token: 0x040091F6 RID: 37366
		public List<TwitchEventPreset> EventPresets = new List<TwitchEventPreset>();

		// Token: 0x040091F7 RID: 37367
		public int ActionPresetIndex;

		// Token: 0x040091F8 RID: 37368
		public int VotePresetIndex;

		// Token: 0x040091F9 RID: 37369
		public int EventPresetIndex;

		// Token: 0x040091FA RID: 37370
		public TwitchActionPreset CurrentActionPreset;

		// Token: 0x040091FB RID: 37371
		public TwitchVotePreset CurrentVotePreset;

		// Token: 0x040091FC RID: 37372
		public TwitchEventPreset CurrentEventPreset;

		// Token: 0x040091FD RID: 37373
		public bool UIDirty;

		// Token: 0x040091FE RID: 37374
		[PublicizedFrom(EAccessModifier.Private)]
		public float updateTime = 1f;

		// Token: 0x040091FF RID: 37375
		public float ExtensionCheckTime;

		// Token: 0x04009200 RID: 37376
		[PublicizedFrom(EAccessModifier.Private)]
		public World world;

		// Token: 0x04009201 RID: 37377
		public int lastGameDay = -1;

		// Token: 0x04009202 RID: 37378
		public int currentBMDayEnd = -1;

		// Token: 0x04009203 RID: 37379
		public int nextBMDay = -1;

		// Token: 0x04009204 RID: 37380
		public int BMCooldownStart;

		// Token: 0x04009205 RID: 37381
		public int BMCooldownEnd;

		// Token: 0x04009206 RID: 37382
		public int BitPointModifier = 1;

		// Token: 0x04009207 RID: 37383
		public int SubPointModifier = 1;

		// Token: 0x04009208 RID: 37384
		public int GiftSubPointModifier = 2;

		// Token: 0x04009209 RID: 37385
		public int RaidPointAdd = 1000;

		// Token: 0x0400920A RID: 37386
		public int RaidViewerMinimum = 10;

		// Token: 0x0400920B RID: 37387
		public int HypeTrainLevel;

		// Token: 0x0400920C RID: 37388
		[PublicizedFrom(EAccessModifier.Private)]
		public float bitPriceMultiplier = 1f;

		// Token: 0x0400920D RID: 37389
		public static TwitchLeaderboardStats LeaderboardStats = new TwitchLeaderboardStats();

		// Token: 0x0400920E RID: 37390
		public bool isBMActive;

		// Token: 0x0400920F RID: 37391
		public TwitchVoteLockTypes VoteLockedLevel;

		// Token: 0x04009210 RID: 37392
		public List<TwitchActionHistoryEntry> ActionHistory = new List<TwitchActionHistoryEntry>();

		// Token: 0x04009211 RID: 37393
		public List<TwitchActionHistoryEntry> VoteHistory = new List<TwitchActionHistoryEntry>();

		// Token: 0x04009212 RID: 37394
		public List<TwitchActionHistoryEntry> EventHistory = new List<TwitchActionHistoryEntry>();

		// Token: 0x04009213 RID: 37395
		public List<TwitchLeaderboardEntry> Leaderboard = new List<TwitchLeaderboardEntry>();

		// Token: 0x04009214 RID: 37396
		public List<TwitchRespawnEntry> RespawnEntries = new List<TwitchRespawnEntry>();

		// Token: 0x04009215 RID: 37397
		public int UseActionsDuringBloodmoon = 2;

		// Token: 0x04009216 RID: 37398
		public int UseActionsDuringQuests = 2;

		// Token: 0x04009217 RID: 37399
		public bool InitialCooldownSet;

		// Token: 0x0400921A RID: 37402
		public List<EntityPlayer> twitchPlayerDeathsThisFrame = new List<EntityPlayer>();

		// Token: 0x0400921B RID: 37403
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchManager.InitStates initState;

		// Token: 0x0400921C RID: 37404
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isLoaded;

		// Token: 0x0400921D RID: 37405
		public XUi LocalPlayerXUi;

		// Token: 0x0400921E RID: 37406
		public float CurrentUnityTime;

		// Token: 0x0400921F RID: 37407
		[PublicizedFrom(EAccessModifier.Private)]
		public bool resetCommandsNeeded;

		// Token: 0x04009220 RID: 37408
		[PublicizedFrom(EAccessModifier.Private)]
		public bool respawnEventNeeded;

		// Token: 0x04009221 RID: 37409
		[PublicizedFrom(EAccessModifier.Private)]
		public bool checkingExtensionInstalled;

		// Token: 0x04009222 RID: 37410
		[PublicizedFrom(EAccessModifier.Private)]
		public string broadcasterType = "";

		// Token: 0x04009223 RID: 37411
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchMessageEntry> inGameChatQueue = new List<TwitchMessageEntry>();

		// Token: 0x0400922A RID: 37418
		public TwitchAuthentication Authentication;

		// Token: 0x0400922B RID: 37419
		public EventSubClient EventSub;

		// Token: 0x0400922C RID: 37420
		public bool HasViewedSettings;

		// Token: 0x0400922D RID: 37421
		public string DeniedCrateEvent = "";

		// Token: 0x0400922E RID: 37422
		public string StealingCrateEvent = "";

		// Token: 0x0400922F RID: 37423
		public string PartyRespawnEvent = "";

		// Token: 0x04009230 RID: 37424
		public string OnPlayerDeathEvent = "";

		// Token: 0x04009231 RID: 37425
		public string OnPlayerRespawnEvent = "";

		// Token: 0x04009232 RID: 37426
		public List<string> tipTitleList = new List<string>();

		// Token: 0x04009233 RID: 37427
		public List<string> tipDescriptionList = new List<string>();

		// Token: 0x04009234 RID: 37428
		[PublicizedFrom(EAccessModifier.Private)]
		public int extensionActiveCheckFailures;

		// Token: 0x04009235 RID: 37429
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ActivatedAction;

		// Token: 0x04009236 RID: 37430
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ActivatedBitAction;

		// Token: 0x04009237 RID: 37431
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_BitCredits;

		// Token: 0x04009238 RID: 37432
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_BitEvent;

		// Token: 0x04009239 RID: 37433
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_BitPotBalance;

		// Token: 0x0400923A RID: 37434
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_ChannelPointEvent;

		// Token: 0x0400923B RID: 37435
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CharityEvent;

		// Token: 0x0400923C RID: 37436
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CooldownComplete;

		// Token: 0x0400923D RID: 37437
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CooldownStarted;

		// Token: 0x0400923E RID: 37438
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CooldownTime;

		// Token: 0x0400923F RID: 37439
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_Commands;

		// Token: 0x04009240 RID: 37440
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_CreatorGoalEvent;

		// Token: 0x04009241 RID: 37441
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_DonateBits;

		// Token: 0x04009242 RID: 37442
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_DonateCharity;

		// Token: 0x04009243 RID: 37443
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_Gamestage;

		// Token: 0x04009244 RID: 37444
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GiftSubEvent;

		// Token: 0x04009245 RID: 37445
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_GiftSubs;

		// Token: 0x04009246 RID: 37446
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_HypeTrainEvent;

		// Token: 0x04009247 RID: 37447
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledParty;

		// Token: 0x04009248 RID: 37448
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledStreamer;

		// Token: 0x04009249 RID: 37449
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledByBits;

		// Token: 0x0400924A RID: 37450
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledByHypeTrain;

		// Token: 0x0400924B RID: 37451
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_KilledByVote;

		// Token: 0x0400924C RID: 37452
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_NewActions;

		// Token: 0x0400924D RID: 37453
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_PimpPotBalance;

		// Token: 0x0400924E RID: 37454
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_PointsWithSpecial;

		// Token: 0x0400924F RID: 37455
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_PointsWithoutSpecial;

		// Token: 0x04009250 RID: 37456
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_QueuedBitAction;

		// Token: 0x04009251 RID: 37457
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_RaidEvent;

		// Token: 0x04009252 RID: 37458
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_RaidPoints;

		// Token: 0x04009253 RID: 37459
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_SubEvent;

		// Token: 0x04009254 RID: 37460
		[PublicizedFrom(EAccessModifier.Private)]
		public string chatOutput_Subscribed;

		// Token: 0x04009255 RID: 37461
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_ActivatedAction;

		// Token: 0x04009256 RID: 37462
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_BitRespawns;

		// Token: 0x04009257 RID: 37463
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_DonateBits;

		// Token: 0x04009258 RID: 37464
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_DonateCharity;

		// Token: 0x04009259 RID: 37465
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_GiftSubs;

		// Token: 0x0400925A RID: 37466
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledParty;

		// Token: 0x0400925B RID: 37467
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledStreamer;

		// Token: 0x0400925C RID: 37468
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledByBits;

		// Token: 0x0400925D RID: 37469
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledByHypeTrain;

		// Token: 0x0400925E RID: 37470
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_KilledByVote;

		// Token: 0x0400925F RID: 37471
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_RaidPoints;

		// Token: 0x04009260 RID: 37472
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_RefundedAction;

		// Token: 0x04009261 RID: 37473
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameOutput_Subscribed;

		// Token: 0x04009262 RID: 37474
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameDeathScreen_Message;

		// Token: 0x04009263 RID: 37475
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameBitsDeathScreen_Message;

		// Token: 0x04009264 RID: 37476
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameHypeTrainDeathScreen_Message;

		// Token: 0x04009265 RID: 37477
		[PublicizedFrom(EAccessModifier.Private)]
		public string ingameVoteDeathScreen_Message;

		// Token: 0x04009266 RID: 37478
		[PublicizedFrom(EAccessModifier.Private)]
		public string subPointDisplay;

		// Token: 0x04009267 RID: 37479
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, TwitchRandomActionGroup> randomGroups = new Dictionary<string, TwitchRandomActionGroup>();

		// Token: 0x04009268 RID: 37480
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, List<TwitchAction>> randomKeys = new Dictionary<string, List<TwitchAction>>();

		// Token: 0x04009269 RID: 37481
		public List<BaseTwitchCommand> TwitchCommandList = new List<BaseTwitchCommand>();

		// Token: 0x0400926A RID: 37482
		[PublicizedFrom(EAccessModifier.Protected)]
		public Dictionary<EntityPlayer, TwitchManager.TwitchPartyMemberInfo> PartyInfo = new Dictionary<EntityPlayer, TwitchManager.TwitchPartyMemberInfo>();

		// Token: 0x0400926B RID: 37483
		[PublicizedFrom(EAccessModifier.Private)]
		public bool lastAlive;

		// Token: 0x0400926C RID: 37484
		public static string DeathText = "";

		// Token: 0x0400926D RID: 37485
		[PublicizedFrom(EAccessModifier.Private)]
		public bool twitchActive = true;

		// Token: 0x0400926E RID: 37486
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchActionEntry> QueuedActionEntries = new List<TwitchActionEntry>();

		// Token: 0x0400926F RID: 37487
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchActionEntry> LiveActionEntries = new List<TwitchActionEntry>();

		// Token: 0x04009270 RID: 37488
		public List<TwitchEventActionEntry> EventQueue = new List<TwitchEventActionEntry>();

		// Token: 0x04009271 RID: 37489
		public List<TwitchEventActionEntry> LiveEvents = new List<TwitchEventActionEntry>();

		// Token: 0x04009272 RID: 37490
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSpawnedEntityEntry> liveList = new List<TwitchSpawnedEntityEntry>();

		// Token: 0x04009273 RID: 37491
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSpawnedBlocksEntry> liveBlockList = new List<TwitchSpawnedBlocksEntry>();

		// Token: 0x04009274 RID: 37492
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchRecentlyRemovedEntityEntry> recentlyDeadList = new List<TwitchRecentlyRemovedEntityEntry>();

		// Token: 0x04009275 RID: 37493
		public List<TwitchSpawnedEntityEntry> actionSpawnLiveList = new List<TwitchSpawnedEntityEntry>();

		// Token: 0x04009276 RID: 37494
		public bool HasDataChanges;

		// Token: 0x04009277 RID: 37495
		public List<string> changedActionList = new List<string>();

		// Token: 0x04009278 RID: 37496
		public List<string> changedEnabledVoteList = new List<string>();

		// Token: 0x04009279 RID: 37497
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> ActionMessages = new List<string>();

		// Token: 0x0400927A RID: 37498
		[PublicizedFrom(EAccessModifier.Private)]
		public float nextDisplayCommandsTime;

		// Token: 0x020018A6 RID: 6310
		public enum PimpPotSettings
		{
			// Token: 0x0400927C RID: 37500
			Disabled,
			// Token: 0x0400927D RID: 37501
			EnabledSP,
			// Token: 0x0400927E RID: 37502
			EnabledPP
		}

		// Token: 0x020018A7 RID: 6311
		public enum IntegrationSettings
		{
			// Token: 0x04009280 RID: 37504
			ExtensionOnly,
			// Token: 0x04009281 RID: 37505
			Both
		}

		// Token: 0x020018A8 RID: 6312
		public enum CooldownTypes
		{
			// Token: 0x04009283 RID: 37507
			None,
			// Token: 0x04009284 RID: 37508
			Startup,
			// Token: 0x04009285 RID: 37509
			Time,
			// Token: 0x04009286 RID: 37510
			MaxReached,
			// Token: 0x04009287 RID: 37511
			MaxReachedWaiting,
			// Token: 0x04009288 RID: 37512
			BloodMoonDisabled,
			// Token: 0x04009289 RID: 37513
			BloodMoonCooldown,
			// Token: 0x0400928A RID: 37514
			QuestDisabled,
			// Token: 0x0400928B RID: 37515
			QuestCooldown,
			// Token: 0x0400928C RID: 37516
			SafeCooldown,
			// Token: 0x0400928D RID: 37517
			SafeCooldownExit
		}

		// Token: 0x020018A9 RID: 6313
		public enum InitStates
		{
			// Token: 0x0400928F RID: 37519
			Setup,
			// Token: 0x04009290 RID: 37520
			None,
			// Token: 0x04009291 RID: 37521
			WaitingForPermission,
			// Token: 0x04009292 RID: 37522
			PermissionDenied,
			// Token: 0x04009293 RID: 37523
			WaitingForOAuth,
			// Token: 0x04009294 RID: 37524
			Authenticating,
			// Token: 0x04009295 RID: 37525
			Authenticated,
			// Token: 0x04009296 RID: 37526
			CheckingForExtension,
			// Token: 0x04009297 RID: 37527
			Ready,
			// Token: 0x04009298 RID: 37528
			ExtensionNotInstalled,
			// Token: 0x04009299 RID: 37529
			Failed
		}

		// Token: 0x020018AA RID: 6314
		public class TwitchPartyMemberInfo
		{
			// Token: 0x0400929A RID: 37530
			public bool LastOptedOut;

			// Token: 0x0400929B RID: 37531
			public bool LastAlive = true;

			// Token: 0x0400929C RID: 37532
			public float Cooldown;

			// Token: 0x0400929D RID: 37533
			public bool NeedsRespawnEvent;
		}
	}
}
