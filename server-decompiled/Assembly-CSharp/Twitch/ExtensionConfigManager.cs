using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UniLinq;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x0200182D RID: 6189
	public class ExtensionConfigManager
	{
		// Token: 0x0600BFAB RID: 49067 RVA: 0x0046899C File Offset: 0x00466B9C
		public void Init()
		{
			TwitchManager.Current.CommandsChanged += this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Combine(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Combine(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
			this.pushConfig();
		}

		// Token: 0x0600BFAC RID: 49068 RVA: 0x00468A1B File Offset: 0x00466C1B
		public bool UpdatedConfig()
		{
			if (this.hasUpdated)
			{
				this.hasUpdated = false;
				return true;
			}
			return false;
		}

		// Token: 0x0600BFAD RID: 49069 RVA: 0x00468A30 File Offset: 0x00466C30
		public void Cleanup()
		{
			TwitchManager.Current.CommandsChanged -= this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Remove(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Remove(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
		}

		// Token: 0x0600BFAE RID: 49070 RVA: 0x00468AA9 File Offset: 0x00466CA9
		public void OnPartyChanged()
		{
			this.pushConfig();
		}

		// Token: 0x0600BFAF RID: 49071 RVA: 0x00468AB1 File Offset: 0x00466CB1
		[PublicizedFrom(EAccessModifier.Private)]
		public void pushConfig()
		{
			this.lastPushTime = Time.time;
			if (!this.waitingToPush)
			{
				this.waitingToPush = true;
				GameManager.Instance.StartCoroutine(this.pushConfigAfterTimeout());
			}
		}

		// Token: 0x0600BFB0 RID: 49072 RVA: 0x00468ADE File Offset: 0x00466CDE
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator pushConfigAfterTimeout()
		{
			while (Time.time - this.lastPushTime < 1f)
			{
				yield return null;
			}
			this.waitingToPush = false;
			yield return this.UpdateConfig();
			yield break;
		}

		// Token: 0x0600BFB1 RID: 49073 RVA: 0x00468AED File Offset: 0x00466CED
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator UpdateConfig()
		{
			if (this.displayName == string.Empty)
			{
				yield return this.GetDisplayName();
			}
			if (TwitchManager.Current == null || TwitchManager.Current.Authentication == null)
			{
				Log.Warning("attempted to updated config with no Auth object");
				yield break;
			}
			Dictionary<string, List<ExtensionConfigManager.CommandModel>> commands = this.GetCommands();
			List<string> activeCategories = this.GetActiveCategories(commands.Keys.ToList<string>());
			string text = JsonConvert.SerializeObject(new ExtensionConfigManager.ConfigModel
			{
				displayName = TwitchManager.Current.Authentication.userName,
				party = this.GetPlayers(),
				categories = activeCategories,
				commands = commands
			});
			using (UnityWebRequest req = UnityWebRequest.Put("https://2v3d0ewjcg.execute-api.us-east-1.amazonaws.com/prod/broadcaster/config", text))
			{
				req.SetRequestHeader("Authorization", TwitchManager.Current.Authentication.userID + " " + TwitchManager.Current.Authentication.oauth.Substring(6));
				req.SetRequestHeader("Content-Type", "application/json");
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning(string.Format("Could not update config on backend: {0}", req.result));
				}
				else
				{
					Log.Out("Successfully updated broadcaster config");
					this.hasUpdated = true;
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BFB2 RID: 49074 RVA: 0x00468AFC File Offset: 0x00466CFC
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator GetDisplayName()
		{
			using (UnityWebRequest req = UnityWebRequest.Get("https://api.twitch.tv/helix/users"))
			{
				req.SetRequestHeader("Content-Type", "application/json");
				req.SetRequestHeader("Client-Id", TwitchAuthentication.client_id);
				req.SetRequestHeader("Authorization", "Bearer " + TwitchManager.Current.Authentication.oauth.Substring(6));
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning(string.Format("Could not get user data from Twitch: {0}", req.result));
				}
				else
				{
					Log.Out("Successfully retrieved user data from Twitch");
					JObject jobject = JObject.Parse(req.downloadHandler.text);
					this.displayName = jobject["data"][0]["display_name"].ToString();
					this.broadcasterType = jobject["data"][0]["broadcaster_type"].ToString();
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BFB3 RID: 49075 RVA: 0x00468B0C File Offset: 0x00466D0C
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> GetActiveCategories(List<string> categories)
		{
			return (from c in TwitchActionManager.Current.CategoryList
				select c.Name into name
				where categories.Contains(name)
				select name).ToList<string>();
		}

		// Token: 0x0600BFB4 RID: 49076 RVA: 0x00468B6C File Offset: 0x00466D6C
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> GetPlayers()
		{
			if (TwitchManager.Current.LocalPlayer != null && TwitchManager.Current.LocalPlayer.Party != null && TwitchManager.Current.LocalPlayer.Party.MemberList != null)
			{
				return (from m in TwitchManager.Current.LocalPlayer.Party.MemberList
					where !(m is EntityPlayerLocal) && !m.TwitchEnabled
					select m into e
					select e.EntityName).ToList<string>();
			}
			return ExtensionConfigManager.emptyParty;
		}

		// Token: 0x0600BFB5 RID: 49077 RVA: 0x00468C1C File Offset: 0x00466E1C
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, List<ExtensionConfigManager.CommandModel>> GetCommands()
		{
			Dictionary<string, List<ExtensionConfigManager.CommandModel>> dictionary = new Dictionary<string, List<ExtensionConfigManager.CommandModel>>();
			TwitchAction[] array = TwitchManager.Current.AvailableCommands.Values.Where<TwitchAction>((TwitchAction a) => a.HasExtraConditions() && (this.CanUseBitCommands() || a.PointType != TwitchAction.PointTypes.Bits)).ToArray<TwitchAction>();
			int num = 0;
			foreach (TwitchAction twitchAction in array)
			{
				ExtensionConfigManager.CommandModel commandModel = new ExtensionConfigManager.CommandModel
				{
					name = twitchAction.Command.Replace("#", string.Empty).Replace("_", " ").ToUpper(),
					baseCommand = twitchAction.BaseCommand,
					command = twitchAction.Command,
					isPositive = twitchAction.IsPositive,
					spends = twitchAction.PointType.ToString(),
					cost = twitchAction.CurrentCost,
					cooldownType = (twitchAction.WaitingBlocked ? "wait" : (twitchAction.CooldownBlocked ? "regular" : "full")),
					cooldownIndex = num / 32,
					bitPosition = (byte)(num % 32),
					streamerOnly = twitchAction.StreamerOnly
				};
				List<ExtensionConfigManager.CommandModel> list;
				if (!dictionary.TryGetValue(twitchAction.MainCategory.Name, out list))
				{
					dictionary.Add(twitchAction.MainCategory.Name, list = new List<ExtensionConfigManager.CommandModel>());
				}
				list.Add(commandModel);
				num++;
			}
			return dictionary;
		}

		// Token: 0x0600BFB6 RID: 49078 RVA: 0x00468D7E File Offset: 0x00466F7E
		public bool CanUseBitCommands()
		{
			return this.broadcasterType == "affiliate" || this.broadcasterType == "partner";
		}

		// Token: 0x04008F41 RID: 36673
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<string> emptyParty = new List<string>(0);

		// Token: 0x04008F42 RID: 36674
		[PublicizedFrom(EAccessModifier.Private)]
		public string displayName = string.Empty;

		// Token: 0x04008F43 RID: 36675
		[PublicizedFrom(EAccessModifier.Private)]
		public string broadcasterType = string.Empty;

		// Token: 0x04008F44 RID: 36676
		[PublicizedFrom(EAccessModifier.Private)]
		public string jwt = string.Empty;

		// Token: 0x04008F45 RID: 36677
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasUpdated;

		// Token: 0x04008F46 RID: 36678
		[PublicizedFrom(EAccessModifier.Private)]
		public const float pushConfigTimeout = 1f;

		// Token: 0x04008F47 RID: 36679
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastPushTime = float.NegativeInfinity;

		// Token: 0x04008F48 RID: 36680
		[PublicizedFrom(EAccessModifier.Private)]
		public bool waitingToPush;

		// Token: 0x0200182E RID: 6190
		public class ConfigModel
		{
			// Token: 0x04008F49 RID: 36681
			public string displayName;

			// Token: 0x04008F4A RID: 36682
			public List<string> party;

			// Token: 0x04008F4B RID: 36683
			public List<string> categories;

			// Token: 0x04008F4C RID: 36684
			public string IdentityGrantHeader = Localization.Get("TwitchInfo_IdentityGrantHeader", false, null);

			// Token: 0x04008F4D RID: 36685
			public string IdentityGrantSubtext = Localization.Get("TwitchInfo_IdentityGrantSubtext", false, null);

			// Token: 0x04008F4E RID: 36686
			public string LoadingText = Localization.Get("loadActionLoading", false, null);

			// Token: 0x04008F4F RID: 36687
			public string OfflineHeader = Localization.Get("TwitchInfo_OfflineHeader", false, null);

			// Token: 0x04008F50 RID: 36688
			public string OfflineSubtext1 = Localization.Get("TwitchInfo_OfflineSubtext1", false, null);

			// Token: 0x04008F51 RID: 36689
			public string OfflineSubtext2 = Localization.Get("TwitchInfo_OfflineSubtext2", false, null);

			// Token: 0x04008F52 RID: 36690
			public string CommandDescriptionsText = Localization.Get("TwitchInfo_CommandDescriptionsText", false, null);

			// Token: 0x04008F53 RID: 36691
			public string ChatPromptHeader = Localization.Get("TwitchInfo_ChatPromptHeader", false, null);

			// Token: 0x04008F54 RID: 36692
			public string ChatPromptSubtext = Localization.Get("TwitchInfo_ChatPromptSubtext", false, null);

			// Token: 0x04008F55 RID: 36693
			public string ActionsOffText = Localization.Get("TwitchInfo_ActionsOffText", false, null);

			// Token: 0x04008F56 RID: 36694
			public string CooldownText = Localization.Get("TwitchInfo_CooldownText", false, null);

			// Token: 0x04008F57 RID: 36695
			public string PausedText = Localization.Get("TwitchCooldownStatus_Paused", false, null);

			// Token: 0x04008F58 RID: 36696
			public string ActionPresetLabel = Localization.Get("xuiOptionsTwitchActionPreset", false, null);

			// Token: 0x04008F59 RID: 36697
			public string VotePresetLabel = Localization.Get("xuiOptionsTwitchVotePreset", false, null);

			// Token: 0x04008F5A RID: 36698
			public string EventPresetLabel = Localization.Get("xuiOptionsTwitchCustomEvents", false, null);

			// Token: 0x04008F5B RID: 36699
			public string TopKillerLabel = Localization.Get("TwitchInfo_TopKiller", false, null);

			// Token: 0x04008F5C RID: 36700
			public string TopGoodLabel = Localization.Get("TwitchInfo_TopGood", false, null);

			// Token: 0x04008F5D RID: 36701
			public string TopBadLabel = Localization.Get("TwitchInfo_TopEvil", false, null);

			// Token: 0x04008F5E RID: 36702
			public string BestHelperLabel = string.Format(Localization.Get("TwitchInfo_CurrentGood", false, null), TwitchManager.LeaderboardStats.GoodRewardTime);

			// Token: 0x04008F5F RID: 36703
			public string TotalGoodActionsLabel = Localization.Get("TwitchInfo_TotalGood", false, null);

			// Token: 0x04008F60 RID: 36704
			public string TotalBadActionsLabel = Localization.Get("TwitchInfo_TotalBad", false, null);

			// Token: 0x04008F61 RID: 36705
			public string LargestPimpPotLabel = Localization.Get("TwitchInfo_LargestPimpPot", false, null);

			// Token: 0x04008F62 RID: 36706
			public string DifficultyLabel = Localization.Get("goDifficultyShort", false, null);

			// Token: 0x04008F63 RID: 36707
			public string DayCycleLabel = Localization.Get("goDayLength", false, null);

			// Token: 0x04008F64 RID: 36708
			public string ModdedLabel = Localization.Get("goModded", false, null);

			// Token: 0x04008F65 RID: 36709
			public string PPRateLabel = Localization.Get("TwitchInfo_PPRate", false, null);

			// Token: 0x04008F66 RID: 36710
			public Dictionary<string, List<ExtensionConfigManager.CommandModel>> commands;
		}

		// Token: 0x0200182F RID: 6191
		public class CommandModel
		{
			// Token: 0x04008F67 RID: 36711
			public string name;

			// Token: 0x04008F68 RID: 36712
			public string baseCommand;

			// Token: 0x04008F69 RID: 36713
			public string command;

			// Token: 0x04008F6A RID: 36714
			public bool isPositive;

			// Token: 0x04008F6B RID: 36715
			public string spends;

			// Token: 0x04008F6C RID: 36716
			public int cost;

			// Token: 0x04008F6D RID: 36717
			public string cooldownType;

			// Token: 0x04008F6E RID: 36718
			public int cooldownIndex;

			// Token: 0x04008F6F RID: 36719
			public byte bitPosition;

			// Token: 0x04008F70 RID: 36720
			public bool streamerOnly;
		}
	}
}
