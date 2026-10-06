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
	// Token: 0x020017FC RID: 6140
	public class ExtensionConfigManager
	{
		// Token: 0x0600BE97 RID: 48791 RVA: 0x00464FF0 File Offset: 0x004631F0
		public void Init()
		{
			TwitchManager.Current.CommandsChanged += this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Combine(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Combine(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
			this.pushConfig();
		}

		// Token: 0x0600BE98 RID: 48792 RVA: 0x0046506F File Offset: 0x0046326F
		public bool UpdatedConfig()
		{
			if (this.hasUpdated)
			{
				this.hasUpdated = false;
				return true;
			}
			return false;
		}

		// Token: 0x0600BE99 RID: 48793 RVA: 0x00465084 File Offset: 0x00463284
		public void Cleanup()
		{
			TwitchManager.Current.CommandsChanged -= this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Remove(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Remove(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
		}

		// Token: 0x0600BE9A RID: 48794 RVA: 0x004650FD File Offset: 0x004632FD
		public void OnPartyChanged()
		{
			this.pushConfig();
		}

		// Token: 0x0600BE9B RID: 48795 RVA: 0x00465105 File Offset: 0x00463305
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

		// Token: 0x0600BE9C RID: 48796 RVA: 0x00465132 File Offset: 0x00463332
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

		// Token: 0x0600BE9D RID: 48797 RVA: 0x00465141 File Offset: 0x00463341
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

		// Token: 0x0600BE9E RID: 48798 RVA: 0x00465150 File Offset: 0x00463350
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

		// Token: 0x0600BE9F RID: 48799 RVA: 0x00465160 File Offset: 0x00463360
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> GetActiveCategories(List<string> categories)
		{
			return (from c in TwitchActionManager.Current.CategoryList
				select c.Name into name
				where categories.Contains(name)
				select name).ToList<string>();
		}

		// Token: 0x0600BEA0 RID: 48800 RVA: 0x004651C0 File Offset: 0x004633C0
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

		// Token: 0x0600BEA1 RID: 48801 RVA: 0x00465270 File Offset: 0x00463470
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

		// Token: 0x0600BEA2 RID: 48802 RVA: 0x004653D2 File Offset: 0x004635D2
		public bool CanUseBitCommands()
		{
			return this.broadcasterType == "affiliate" || this.broadcasterType == "partner";
		}

		// Token: 0x04008E58 RID: 36440
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<string> emptyParty = new List<string>(0);

		// Token: 0x04008E59 RID: 36441
		[PublicizedFrom(EAccessModifier.Private)]
		public string displayName = string.Empty;

		// Token: 0x04008E5A RID: 36442
		[PublicizedFrom(EAccessModifier.Private)]
		public string broadcasterType = string.Empty;

		// Token: 0x04008E5B RID: 36443
		[PublicizedFrom(EAccessModifier.Private)]
		public string jwt = string.Empty;

		// Token: 0x04008E5C RID: 36444
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasUpdated;

		// Token: 0x04008E5D RID: 36445
		[PublicizedFrom(EAccessModifier.Private)]
		public const float pushConfigTimeout = 1f;

		// Token: 0x04008E5E RID: 36446
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastPushTime = float.NegativeInfinity;

		// Token: 0x04008E5F RID: 36447
		[PublicizedFrom(EAccessModifier.Private)]
		public bool waitingToPush;

		// Token: 0x020017FD RID: 6141
		public class ConfigModel
		{
			// Token: 0x04008E60 RID: 36448
			public string displayName;

			// Token: 0x04008E61 RID: 36449
			public List<string> party;

			// Token: 0x04008E62 RID: 36450
			public List<string> categories;

			// Token: 0x04008E63 RID: 36451
			public string IdentityGrantHeader = Localization.Get("TwitchInfo_IdentityGrantHeader", false, null);

			// Token: 0x04008E64 RID: 36452
			public string IdentityGrantSubtext = Localization.Get("TwitchInfo_IdentityGrantSubtext", false, null);

			// Token: 0x04008E65 RID: 36453
			public string LoadingText = Localization.Get("loadActionLoading", false, null);

			// Token: 0x04008E66 RID: 36454
			public string OfflineHeader = Localization.Get("TwitchInfo_OfflineHeader", false, null);

			// Token: 0x04008E67 RID: 36455
			public string OfflineSubtext1 = Localization.Get("TwitchInfo_OfflineSubtext1", false, null);

			// Token: 0x04008E68 RID: 36456
			public string OfflineSubtext2 = Localization.Get("TwitchInfo_OfflineSubtext2", false, null);

			// Token: 0x04008E69 RID: 36457
			public string CommandDescriptionsText = Localization.Get("TwitchInfo_CommandDescriptionsText", false, null);

			// Token: 0x04008E6A RID: 36458
			public string ChatPromptHeader = Localization.Get("TwitchInfo_ChatPromptHeader", false, null);

			// Token: 0x04008E6B RID: 36459
			public string ChatPromptSubtext = Localization.Get("TwitchInfo_ChatPromptSubtext", false, null);

			// Token: 0x04008E6C RID: 36460
			public string ActionsOffText = Localization.Get("TwitchInfo_ActionsOffText", false, null);

			// Token: 0x04008E6D RID: 36461
			public string CooldownText = Localization.Get("TwitchInfo_CooldownText", false, null);

			// Token: 0x04008E6E RID: 36462
			public string PausedText = Localization.Get("TwitchCooldownStatus_Paused", false, null);

			// Token: 0x04008E6F RID: 36463
			public string ActionPresetLabel = Localization.Get("xuiOptionsTwitchActionPreset", false, null);

			// Token: 0x04008E70 RID: 36464
			public string VotePresetLabel = Localization.Get("xuiOptionsTwitchVotePreset", false, null);

			// Token: 0x04008E71 RID: 36465
			public string EventPresetLabel = Localization.Get("xuiOptionsTwitchCustomEvents", false, null);

			// Token: 0x04008E72 RID: 36466
			public string TopKillerLabel = Localization.Get("TwitchInfo_TopKiller", false, null);

			// Token: 0x04008E73 RID: 36467
			public string TopGoodLabel = Localization.Get("TwitchInfo_TopGood", false, null);

			// Token: 0x04008E74 RID: 36468
			public string TopBadLabel = Localization.Get("TwitchInfo_TopEvil", false, null);

			// Token: 0x04008E75 RID: 36469
			public string BestHelperLabel = string.Format(Localization.Get("TwitchInfo_CurrentGood", false, null), TwitchManager.LeaderboardStats.GoodRewardTime);

			// Token: 0x04008E76 RID: 36470
			public string TotalGoodActionsLabel = Localization.Get("TwitchInfo_TotalGood", false, null);

			// Token: 0x04008E77 RID: 36471
			public string TotalBadActionsLabel = Localization.Get("TwitchInfo_TotalBad", false, null);

			// Token: 0x04008E78 RID: 36472
			public string LargestPimpPotLabel = Localization.Get("TwitchInfo_LargestPimpPot", false, null);

			// Token: 0x04008E79 RID: 36473
			public string DifficultyLabel = Localization.Get("goDifficultyShort", false, null);

			// Token: 0x04008E7A RID: 36474
			public string DayCycleLabel = Localization.Get("goDayLength", false, null);

			// Token: 0x04008E7B RID: 36475
			public string ModdedLabel = Localization.Get("goModded", false, null);

			// Token: 0x04008E7C RID: 36476
			public string PPRateLabel = Localization.Get("TwitchInfo_PPRate", false, null);

			// Token: 0x04008E7D RID: 36477
			public Dictionary<string, List<ExtensionConfigManager.CommandModel>> commands;
		}

		// Token: 0x020017FE RID: 6142
		public class CommandModel
		{
			// Token: 0x04008E7E RID: 36478
			public string name;

			// Token: 0x04008E7F RID: 36479
			public string baseCommand;

			// Token: 0x04008E80 RID: 36480
			public string command;

			// Token: 0x04008E81 RID: 36481
			public bool isPositive;

			// Token: 0x04008E82 RID: 36482
			public string spends;

			// Token: 0x04008E83 RID: 36483
			public int cost;

			// Token: 0x04008E84 RID: 36484
			public string cooldownType;

			// Token: 0x04008E85 RID: 36485
			public int cooldownIndex;

			// Token: 0x04008E86 RID: 36486
			public byte bitPosition;

			// Token: 0x04008E87 RID: 36487
			public bool streamerOnly;
		}
	}
}
