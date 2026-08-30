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
	// Token: 0x020017D0 RID: 6096
	public class ExtensionConfigManager
	{
		// Token: 0x0600BD3F RID: 48447 RVA: 0x00460AB4 File Offset: 0x0045ECB4
		public void Init()
		{
			TwitchManager.Current.CommandsChanged += this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Combine(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Combine(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
			this.pushConfig();
		}

		// Token: 0x0600BD40 RID: 48448 RVA: 0x00460B33 File Offset: 0x0045ED33
		public bool UpdatedConfig()
		{
			if (this.hasUpdated)
			{
				this.hasUpdated = false;
				return true;
			}
			return false;
		}

		// Token: 0x0600BD41 RID: 48449 RVA: 0x00460B48 File Offset: 0x0045ED48
		public void Cleanup()
		{
			TwitchManager.Current.CommandsChanged -= this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Remove(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Remove(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
		}

		// Token: 0x0600BD42 RID: 48450 RVA: 0x00460BC1 File Offset: 0x0045EDC1
		public void OnPartyChanged()
		{
			this.pushConfig();
		}

		// Token: 0x0600BD43 RID: 48451 RVA: 0x00460BC9 File Offset: 0x0045EDC9
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

		// Token: 0x0600BD44 RID: 48452 RVA: 0x00460BF6 File Offset: 0x0045EDF6
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

		// Token: 0x0600BD45 RID: 48453 RVA: 0x00460C05 File Offset: 0x0045EE05
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

		// Token: 0x0600BD46 RID: 48454 RVA: 0x00460C14 File Offset: 0x0045EE14
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

		// Token: 0x0600BD47 RID: 48455 RVA: 0x00460C24 File Offset: 0x0045EE24
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> GetActiveCategories(List<string> categories)
		{
			return (from c in TwitchActionManager.Current.CategoryList
				select c.Name into name
				where categories.Contains(name)
				select name).ToList<string>();
		}

		// Token: 0x0600BD48 RID: 48456 RVA: 0x00460C84 File Offset: 0x0045EE84
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

		// Token: 0x0600BD49 RID: 48457 RVA: 0x00460D34 File Offset: 0x0045EF34
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

		// Token: 0x0600BD4A RID: 48458 RVA: 0x00460E96 File Offset: 0x0045F096
		public bool CanUseBitCommands()
		{
			return this.broadcasterType == "affiliate" || this.broadcasterType == "partner";
		}

		// Token: 0x04008D92 RID: 36242
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<string> emptyParty = new List<string>(0);

		// Token: 0x04008D93 RID: 36243
		[PublicizedFrom(EAccessModifier.Private)]
		public string displayName = string.Empty;

		// Token: 0x04008D94 RID: 36244
		[PublicizedFrom(EAccessModifier.Private)]
		public string broadcasterType = string.Empty;

		// Token: 0x04008D95 RID: 36245
		[PublicizedFrom(EAccessModifier.Private)]
		public string jwt = string.Empty;

		// Token: 0x04008D96 RID: 36246
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasUpdated;

		// Token: 0x04008D97 RID: 36247
		[PublicizedFrom(EAccessModifier.Private)]
		public const float pushConfigTimeout = 1f;

		// Token: 0x04008D98 RID: 36248
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastPushTime = float.NegativeInfinity;

		// Token: 0x04008D99 RID: 36249
		[PublicizedFrom(EAccessModifier.Private)]
		public bool waitingToPush;

		// Token: 0x020017D1 RID: 6097
		public class ConfigModel
		{
			// Token: 0x04008D9A RID: 36250
			public string displayName;

			// Token: 0x04008D9B RID: 36251
			public List<string> party;

			// Token: 0x04008D9C RID: 36252
			public List<string> categories;

			// Token: 0x04008D9D RID: 36253
			public string IdentityGrantHeader = Localization.Get("TwitchInfo_IdentityGrantHeader", false, null);

			// Token: 0x04008D9E RID: 36254
			public string IdentityGrantSubtext = Localization.Get("TwitchInfo_IdentityGrantSubtext", false, null);

			// Token: 0x04008D9F RID: 36255
			public string LoadingText = Localization.Get("loadActionLoading", false, null);

			// Token: 0x04008DA0 RID: 36256
			public string OfflineHeader = Localization.Get("TwitchInfo_OfflineHeader", false, null);

			// Token: 0x04008DA1 RID: 36257
			public string OfflineSubtext1 = Localization.Get("TwitchInfo_OfflineSubtext1", false, null);

			// Token: 0x04008DA2 RID: 36258
			public string OfflineSubtext2 = Localization.Get("TwitchInfo_OfflineSubtext2", false, null);

			// Token: 0x04008DA3 RID: 36259
			public string CommandDescriptionsText = Localization.Get("TwitchInfo_CommandDescriptionsText", false, null);

			// Token: 0x04008DA4 RID: 36260
			public string ChatPromptHeader = Localization.Get("TwitchInfo_ChatPromptHeader", false, null);

			// Token: 0x04008DA5 RID: 36261
			public string ChatPromptSubtext = Localization.Get("TwitchInfo_ChatPromptSubtext", false, null);

			// Token: 0x04008DA6 RID: 36262
			public string ActionsOffText = Localization.Get("TwitchInfo_ActionsOffText", false, null);

			// Token: 0x04008DA7 RID: 36263
			public string CooldownText = Localization.Get("TwitchInfo_CooldownText", false, null);

			// Token: 0x04008DA8 RID: 36264
			public string PausedText = Localization.Get("TwitchCooldownStatus_Paused", false, null);

			// Token: 0x04008DA9 RID: 36265
			public string ActionPresetLabel = Localization.Get("xuiOptionsTwitchActionPreset", false, null);

			// Token: 0x04008DAA RID: 36266
			public string VotePresetLabel = Localization.Get("xuiOptionsTwitchVotePreset", false, null);

			// Token: 0x04008DAB RID: 36267
			public string EventPresetLabel = Localization.Get("xuiOptionsTwitchCustomEvents", false, null);

			// Token: 0x04008DAC RID: 36268
			public string TopKillerLabel = Localization.Get("TwitchInfo_TopKiller", false, null);

			// Token: 0x04008DAD RID: 36269
			public string TopGoodLabel = Localization.Get("TwitchInfo_TopGood", false, null);

			// Token: 0x04008DAE RID: 36270
			public string TopBadLabel = Localization.Get("TwitchInfo_TopEvil", false, null);

			// Token: 0x04008DAF RID: 36271
			public string BestHelperLabel = string.Format(Localization.Get("TwitchInfo_CurrentGood", false, null), TwitchManager.LeaderboardStats.GoodRewardTime);

			// Token: 0x04008DB0 RID: 36272
			public string TotalGoodActionsLabel = Localization.Get("TwitchInfo_TotalGood", false, null);

			// Token: 0x04008DB1 RID: 36273
			public string TotalBadActionsLabel = Localization.Get("TwitchInfo_TotalBad", false, null);

			// Token: 0x04008DB2 RID: 36274
			public string LargestPimpPotLabel = Localization.Get("TwitchInfo_LargestPimpPot", false, null);

			// Token: 0x04008DB3 RID: 36275
			public string DifficultyLabel = Localization.Get("goDifficultyShort", false, null);

			// Token: 0x04008DB4 RID: 36276
			public string DayCycleLabel = Localization.Get("goDayLength", false, null);

			// Token: 0x04008DB5 RID: 36277
			public string ModdedLabel = Localization.Get("goModded", false, null);

			// Token: 0x04008DB6 RID: 36278
			public string PPRateLabel = Localization.Get("TwitchInfo_PPRate", false, null);

			// Token: 0x04008DB7 RID: 36279
			public Dictionary<string, List<ExtensionConfigManager.CommandModel>> commands;
		}

		// Token: 0x020017D2 RID: 6098
		public class CommandModel
		{
			// Token: 0x04008DB8 RID: 36280
			public string name;

			// Token: 0x04008DB9 RID: 36281
			public string baseCommand;

			// Token: 0x04008DBA RID: 36282
			public string command;

			// Token: 0x04008DBB RID: 36283
			public bool isPositive;

			// Token: 0x04008DBC RID: 36284
			public string spends;

			// Token: 0x04008DBD RID: 36285
			public int cost;

			// Token: 0x04008DBE RID: 36286
			public string cooldownType;

			// Token: 0x04008DBF RID: 36287
			public int cooldownIndex;

			// Token: 0x04008DC0 RID: 36288
			public byte bitPosition;

			// Token: 0x04008DC1 RID: 36289
			public bool streamerOnly;
		}
	}
}
