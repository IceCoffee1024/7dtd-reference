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
	// Token: 0x020017AA RID: 6058
	public class ExtensionConfigManager
	{
		// Token: 0x0600BBBA RID: 48058 RVA: 0x0045AD18 File Offset: 0x00458F18
		public void Init()
		{
			TwitchManager.Current.CommandsChanged += this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Combine(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Combine(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
			this.pushConfig();
		}

		// Token: 0x0600BBBB RID: 48059 RVA: 0x0045AD97 File Offset: 0x00458F97
		public bool UpdatedConfig()
		{
			if (this.hasUpdated)
			{
				this.hasUpdated = false;
				return true;
			}
			return false;
		}

		// Token: 0x0600BBBC RID: 48060 RVA: 0x0045ADAC File Offset: 0x00458FAC
		public void Cleanup()
		{
			TwitchManager.Current.CommandsChanged -= this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Remove(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Remove(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
		}

		// Token: 0x0600BBBD RID: 48061 RVA: 0x0045AE25 File Offset: 0x00459025
		public void OnPartyChanged()
		{
			this.pushConfig();
		}

		// Token: 0x0600BBBE RID: 48062 RVA: 0x0045AE2D File Offset: 0x0045902D
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

		// Token: 0x0600BBBF RID: 48063 RVA: 0x0045AE5A File Offset: 0x0045905A
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

		// Token: 0x0600BBC0 RID: 48064 RVA: 0x0045AE69 File Offset: 0x00459069
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

		// Token: 0x0600BBC1 RID: 48065 RVA: 0x0045AE78 File Offset: 0x00459078
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

		// Token: 0x0600BBC2 RID: 48066 RVA: 0x0045AE88 File Offset: 0x00459088
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> GetActiveCategories(List<string> categories)
		{
			return (from c in TwitchActionManager.Current.CategoryList
				select c.Name into name
				where categories.Contains(name)
				select name).ToList<string>();
		}

		// Token: 0x0600BBC3 RID: 48067 RVA: 0x0045AEE8 File Offset: 0x004590E8
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

		// Token: 0x0600BBC4 RID: 48068 RVA: 0x0045AF98 File Offset: 0x00459198
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

		// Token: 0x0600BBC5 RID: 48069 RVA: 0x0045B0FA File Offset: 0x004592FA
		public bool CanUseBitCommands()
		{
			return this.broadcasterType == "affiliate" || this.broadcasterType == "partner";
		}

		// Token: 0x04008C76 RID: 35958
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<string> emptyParty = new List<string>(0);

		// Token: 0x04008C77 RID: 35959
		[PublicizedFrom(EAccessModifier.Private)]
		public string displayName = string.Empty;

		// Token: 0x04008C78 RID: 35960
		[PublicizedFrom(EAccessModifier.Private)]
		public string broadcasterType = string.Empty;

		// Token: 0x04008C79 RID: 35961
		[PublicizedFrom(EAccessModifier.Private)]
		public string jwt = string.Empty;

		// Token: 0x04008C7A RID: 35962
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasUpdated;

		// Token: 0x04008C7B RID: 35963
		[PublicizedFrom(EAccessModifier.Private)]
		public const float pushConfigTimeout = 1f;

		// Token: 0x04008C7C RID: 35964
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastPushTime = float.NegativeInfinity;

		// Token: 0x04008C7D RID: 35965
		[PublicizedFrom(EAccessModifier.Private)]
		public bool waitingToPush;

		// Token: 0x020017AB RID: 6059
		public class ConfigModel
		{
			// Token: 0x04008C7E RID: 35966
			public string displayName;

			// Token: 0x04008C7F RID: 35967
			public List<string> party;

			// Token: 0x04008C80 RID: 35968
			public List<string> categories;

			// Token: 0x04008C81 RID: 35969
			public string IdentityGrantHeader = Localization.Get("TwitchInfo_IdentityGrantHeader", false, null);

			// Token: 0x04008C82 RID: 35970
			public string IdentityGrantSubtext = Localization.Get("TwitchInfo_IdentityGrantSubtext", false, null);

			// Token: 0x04008C83 RID: 35971
			public string LoadingText = Localization.Get("loadActionLoading", false, null);

			// Token: 0x04008C84 RID: 35972
			public string OfflineHeader = Localization.Get("TwitchInfo_OfflineHeader", false, null);

			// Token: 0x04008C85 RID: 35973
			public string OfflineSubtext1 = Localization.Get("TwitchInfo_OfflineSubtext1", false, null);

			// Token: 0x04008C86 RID: 35974
			public string OfflineSubtext2 = Localization.Get("TwitchInfo_OfflineSubtext2", false, null);

			// Token: 0x04008C87 RID: 35975
			public string CommandDescriptionsText = Localization.Get("TwitchInfo_CommandDescriptionsText", false, null);

			// Token: 0x04008C88 RID: 35976
			public string ChatPromptHeader = Localization.Get("TwitchInfo_ChatPromptHeader", false, null);

			// Token: 0x04008C89 RID: 35977
			public string ChatPromptSubtext = Localization.Get("TwitchInfo_ChatPromptSubtext", false, null);

			// Token: 0x04008C8A RID: 35978
			public string ActionsOffText = Localization.Get("TwitchInfo_ActionsOffText", false, null);

			// Token: 0x04008C8B RID: 35979
			public string CooldownText = Localization.Get("TwitchInfo_CooldownText", false, null);

			// Token: 0x04008C8C RID: 35980
			public string PausedText = Localization.Get("TwitchCooldownStatus_Paused", false, null);

			// Token: 0x04008C8D RID: 35981
			public string ActionPresetLabel = Localization.Get("xuiOptionsTwitchActionPreset", false, null);

			// Token: 0x04008C8E RID: 35982
			public string VotePresetLabel = Localization.Get("xuiOptionsTwitchVotePreset", false, null);

			// Token: 0x04008C8F RID: 35983
			public string EventPresetLabel = Localization.Get("xuiOptionsTwitchCustomEvents", false, null);

			// Token: 0x04008C90 RID: 35984
			public string TopKillerLabel = Localization.Get("TwitchInfo_TopKiller", false, null);

			// Token: 0x04008C91 RID: 35985
			public string TopGoodLabel = Localization.Get("TwitchInfo_TopGood", false, null);

			// Token: 0x04008C92 RID: 35986
			public string TopBadLabel = Localization.Get("TwitchInfo_TopEvil", false, null);

			// Token: 0x04008C93 RID: 35987
			public string BestHelperLabel = string.Format(Localization.Get("TwitchInfo_CurrentGood", false, null), TwitchManager.LeaderboardStats.GoodRewardTime);

			// Token: 0x04008C94 RID: 35988
			public string TotalGoodActionsLabel = Localization.Get("TwitchInfo_TotalGood", false, null);

			// Token: 0x04008C95 RID: 35989
			public string TotalBadActionsLabel = Localization.Get("TwitchInfo_TotalBad", false, null);

			// Token: 0x04008C96 RID: 35990
			public string LargestPimpPotLabel = Localization.Get("TwitchInfo_LargestPimpPot", false, null);

			// Token: 0x04008C97 RID: 35991
			public string DifficultyLabel = Localization.Get("goDifficultyShort", false, null);

			// Token: 0x04008C98 RID: 35992
			public string DayCycleLabel = Localization.Get("goDayLength", false, null);

			// Token: 0x04008C99 RID: 35993
			public string ModdedLabel = Localization.Get("goModded", false, null);

			// Token: 0x04008C9A RID: 35994
			public string PPRateLabel = Localization.Get("TwitchInfo_PPRate", false, null);

			// Token: 0x04008C9B RID: 35995
			public Dictionary<string, List<ExtensionConfigManager.CommandModel>> commands;
		}

		// Token: 0x020017AC RID: 6060
		public class CommandModel
		{
			// Token: 0x04008C9C RID: 35996
			public string name;

			// Token: 0x04008C9D RID: 35997
			public string baseCommand;

			// Token: 0x04008C9E RID: 35998
			public string command;

			// Token: 0x04008C9F RID: 35999
			public bool isPositive;

			// Token: 0x04008CA0 RID: 36000
			public string spends;

			// Token: 0x04008CA1 RID: 36001
			public int cost;

			// Token: 0x04008CA2 RID: 36002
			public string cooldownType;

			// Token: 0x04008CA3 RID: 36003
			public int cooldownIndex;

			// Token: 0x04008CA4 RID: 36004
			public byte bitPosition;

			// Token: 0x04008CA5 RID: 36005
			public bool streamerOnly;
		}
	}
}
