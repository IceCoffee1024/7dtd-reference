using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UniLinq;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x02001835 RID: 6197
	public class ExtensionListener
	{
		// Token: 0x0600BFD7 RID: 49111 RVA: 0x00469554 File Offset: 0x00467754
		public ExtensionListener()
		{
			this.listener = new HttpListener();
			this.listener.Prefixes.Add("http://localhost:52775/");
		}

		// Token: 0x0600BFD8 RID: 49112 RVA: 0x004695C0 File Offset: 0x004677C0
		public void StopListener()
		{
			this.listener.Stop();
			this.setConfig("offline");
			this.displayName = string.Empty;
			TwitchManager.Current.CommandsChanged -= this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Remove(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Remove(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
		}

		// Token: 0x0600BFD9 RID: 49113 RVA: 0x0046965A File Offset: 0x0046785A
		public void RunListener()
		{
			Task.Run(delegate
			{
				this.StartListener();
			});
		}

		// Token: 0x0600BFDA RID: 49114 RVA: 0x00469670 File Offset: 0x00467870
		[PublicizedFrom(EAccessModifier.Private)]
		public void StartListener()
		{
			TwitchManager.Current.CommandsChanged += this.pushConfig;
			TwitchVotingManager votingManager = TwitchManager.Current.VotingManager;
			votingManager.VoteEventEnded = (OnGameEventVoteAction)Delegate.Combine(votingManager.VoteEventEnded, new OnGameEventVoteAction(this.pushConfig));
			TwitchVotingManager votingManager2 = TwitchManager.Current.VotingManager;
			votingManager2.VoteStarted = (OnGameEventVoteAction)Delegate.Combine(votingManager2.VoteStarted, new OnGameEventVoteAction(this.pushConfig));
			this.listener.Start();
			while (this.listener.IsListening)
			{
				HttpListenerContext context = this.listener.GetContext();
				HttpListenerRequest request = context.Request;
				Log.Out(request.Url.LocalPath);
				HttpListenerResponse response = context.Response;
				response.AddHeader("Access-Control-Allow-Origin", "*");
				response.ContentType = "application/json";
				string localPath = request.Url.LocalPath;
				if (!(localPath == "/command"))
				{
					if (!(localPath == "/connect"))
					{
						if (localPath == "/bitCmdVerification")
						{
							string text = request.QueryString["command"];
							string text2 = request.QueryString["userId"];
							response.ContentType = "application/json";
							string text3 = JsonConvert.SerializeObject(new ExtensionListener.BitCmdVerifyResponse
							{
								canSend = TwitchManager.Current.IsActionAvailable(text),
								userId = text2
							});
							byte[] bytes = Encoding.ASCII.GetBytes(text3);
							response.ContentLength64 = (long)bytes.Length;
							response.OutputStream.Write(bytes, 0, bytes.Length);
						}
					}
					else
					{
						if (request.HttpMethod == "GET")
						{
							this.JWT = request.QueryString["token"];
							this.userId = request.QueryString["userId"];
							this.opaqueId = request.QueryString["opaqueId"];
							Log.Out("TOKEN: " + this.JWT);
							this.pushConfigChanges = (this.isConnected = this.JWT != "" && this.userId != "" && this.opaqueId != "");
							if (!this.pushConfigChanges)
							{
								Log.Warning("Query string missing from connection call");
							}
						}
						response.ContentLength64 = 0L;
					}
				}
				else
				{
					if (request.HttpMethod == "POST")
					{
						this.commands.Enqueue(new TwitchExtensionCommand(request));
					}
					response.ContentLength64 = 0L;
				}
				response.OutputStream.Close();
			}
		}

		// Token: 0x0600BFDB RID: 49115 RVA: 0x00469912 File Offset: 0x00467B12
		public void OnPartyChanged()
		{
			this.pushConfigChanges = true;
		}

		// Token: 0x0600BFDC RID: 49116 RVA: 0x0046991B File Offset: 0x00467B1B
		public bool HasCommand()
		{
			return this.commands.Count > 0;
		}

		// Token: 0x0600BFDD RID: 49117 RVA: 0x0046992C File Offset: 0x00467B2C
		public TwitchExtensionCommand GetCommand()
		{
			Queue<TwitchExtensionCommand> queue = this.commands;
			TwitchExtensionCommand twitchExtensionCommand;
			lock (queue)
			{
				twitchExtensionCommand = this.commands.Dequeue();
			}
			return twitchExtensionCommand;
		}

		// Token: 0x0600BFDE RID: 49118 RVA: 0x00469974 File Offset: 0x00467B74
		public void Update()
		{
			if (!this.isConnected)
			{
				return;
			}
			if (this.pushConfigChanges || Time.time - this.lastUpdate > 15f)
			{
				this.UpdateCooldown();
				this.pushConfigChanges = false;
				this.lastUpdate = Time.time;
			}
		}

		// Token: 0x0600BFDF RID: 49119 RVA: 0x004699B2 File Offset: 0x00467BB2
		public void UpdateCooldown()
		{
			if (!this.JWT.Equals(string.Empty))
			{
				this.setConfig(this.GetPubSubStatus());
				this.lastUpdate = Time.time;
			}
		}

		// Token: 0x0600BFE0 RID: 49120 RVA: 0x004699E0 File Offset: 0x00467BE0
		[PublicizedFrom(EAccessModifier.Private)]
		public string GetPubSubStatus()
		{
			TwitchManager twitchManager = TwitchManager.Current;
			if (twitchManager.IsVoting || (twitchManager.LocalPlayer != null && twitchManager.LocalPlayer.TwitchVoteLock == TwitchVoteLockTypes.ActionsLocked))
			{
				return "full";
			}
			switch (TwitchManager.Current.CooldownType)
			{
			case TwitchManager.CooldownTypes.Startup:
			case TwitchManager.CooldownTypes.Time:
			case TwitchManager.CooldownTypes.BloodMoonDisabled:
			case TwitchManager.CooldownTypes.QuestDisabled:
				return "full";
			case TwitchManager.CooldownTypes.MaxReached:
			case TwitchManager.CooldownTypes.BloodMoonCooldown:
			case TwitchManager.CooldownTypes.QuestCooldown:
				return "regular";
			case TwitchManager.CooldownTypes.MaxReachedWaiting:
			case TwitchManager.CooldownTypes.SafeCooldown:
			case TwitchManager.CooldownTypes.SafeCooldownExit:
				return "wait";
			}
			return "online";
		}

		// Token: 0x0600BFE1 RID: 49121 RVA: 0x00469912 File Offset: 0x00467B12
		[PublicizedFrom(EAccessModifier.Private)]
		public void pushConfig()
		{
			this.pushConfigChanges = true;
		}

		// Token: 0x0600BFE2 RID: 49122 RVA: 0x00469A73 File Offset: 0x00467C73
		[PublicizedFrom(EAccessModifier.Private)]
		public void setConfig(string status)
		{
			GameManager.Instance.StartCoroutine(this.pushConfig(status, (status != "offline") ? this.getCommands() : string.Empty));
		}

		// Token: 0x0600BFE3 RID: 49123 RVA: 0x00469AA4 File Offset: 0x00467CA4
		[PublicizedFrom(EAccessModifier.Private)]
		public string getCommands()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.tempCommandList.Clear();
			using (Dictionary<string, TwitchAction>.ValueCollection.Enumerator enumerator = TwitchManager.Current.AvailableCommands.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TwitchAction action2 = enumerator.Current;
					if (action2.HasExtraConditions())
					{
						if (MathUtils.Min(TwitchActionManager.Current.CategoryList.FindIndex((TwitchActionManager.ActionCategory c) => c.Name == action2.CategoryNames.Last<string>()), 6) == -1)
						{
							Log.Warning("no type index found for " + action2.Name);
						}
						else if (action2.PointType == TwitchAction.PointTypes.Bits)
						{
							if (this.broadcaster_type == "partner" || this.broadcaster_type == "affiliate")
							{
								this.tempCommandList.Add(action2);
							}
						}
						else
						{
							this.tempCommandList.Add(action2);
						}
					}
				}
			}
			this.tempCommandList = (from c in this.tempCommandList
				orderby c.PointType, c.Command
				select c).ToList<TwitchAction>();
			for (int i = 0; i < this.tempCommandList.Count; i++)
			{
				TwitchAction action = this.tempCommandList[i];
				string text = "";
				int num = TwitchActionManager.Current.CategoryList.FindIndex((TwitchActionManager.ActionCategory c) => c.Name == action.CategoryNames.Last<string>());
				num = MathUtils.Min(num, 6);
				switch (action.PointType)
				{
				case TwitchAction.PointTypes.PP:
					if (action.WaitingBlocked)
					{
						text = "*";
					}
					else if (action.CooldownBlocked)
					{
						text = "&";
					}
					else
					{
						text = "!";
					}
					break;
				case TwitchAction.PointTypes.SP:
					if (action.WaitingBlocked)
					{
						text = "+";
					}
					else if (action.CooldownBlocked)
					{
						text = "(";
					}
					else
					{
						text = "#";
					}
					break;
				case TwitchAction.PointTypes.Bits:
					if (action.WaitingBlocked)
					{
						text = "-";
					}
					else if (action.CooldownBlocked)
					{
						text = ")";
					}
					else
					{
						text = "$";
					}
					break;
				}
				if (!(text == ""))
				{
					stringBuilder.Append(num);
					string text2 = action.Command.Replace("#", string.Empty);
					if (action.IsPositive)
					{
						text2 = text2.ToUpper();
					}
					stringBuilder.Append(text2);
					stringBuilder.Append(text);
					stringBuilder.Append(action.CurrentCost);
					stringBuilder.Append(",");
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600BFE4 RID: 49124 RVA: 0x00469DE8 File Offset: 0x00467FE8
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator pushConfig(string status, string cmds)
		{
			if (this.displayName == string.Empty)
			{
				using (UnityWebRequest req = UnityWebRequest.Get("https://api.twitch.tv/helix/users?id=" + this.userId))
				{
					req.SetRequestHeader("Content-Type", "application/json");
					req.SetRequestHeader("Client-Id", TwitchAuthentication.client_id);
					req.SetRequestHeader("Authorization", "Bearer " + TwitchManager.Current.Authentication.oauth.Substring(6));
					yield return req.SendWebRequest();
					if (req.result != UnityWebRequest.Result.Success)
					{
						Log.Warning("Could not get data from Twitch 'User' endpoint");
					}
					else
					{
						TwitchUserDataContainer twitchUserDataContainer = JsonConvert.DeserializeObject<TwitchUserDataContainer>(req.downloadHandler.text);
						this.displayName = twitchUserDataContainer.data[0].display_name;
						TwitchManager.Current.BroadcasterType = (this.broadcaster_type = twitchUserDataContainer.data[0].broadcaster_type);
					}
				}
				UnityWebRequest req = null;
				using (UnityWebRequest req = UnityWebRequest.Get("https://2v3d0ewjcg.execute-api.us-east-1.amazonaws.com/prod/allowlist?displayName=" + this.displayName))
				{
					yield return req.SendWebRequest();
					if (req.result != UnityWebRequest.Result.Success)
					{
						Log.Warning("InBeta Check Failed: " + req.downloadHandler.text);
					}
					else
					{
						ExtensionListener.InBetaResponse inBetaResponse = JsonConvert.DeserializeObject<ExtensionListener.InBetaResponse>(req.downloadHandler.text);
						this.inBeta = inBetaResponse.inBeta;
						Log.Out(string.Format("inBeta: {0}", this.inBeta));
					}
				}
				req = null;
			}
			if (this.inBeta)
			{
				List<string> list = (from c in TwitchActionManager.Current.CategoryList.GetRange(1, 5)
					select c.DisplayName).ToList<string>();
				list.Add("Other");
				List<string> players = new List<string>();
				if (TwitchManager.Current.LocalPlayer != null && TwitchManager.Current.LocalPlayer.Party != null)
				{
					foreach (EntityPlayer entityPlayer in TwitchManager.Current.LocalPlayer.Party.MemberList)
					{
						if (!(entityPlayer is EntityPlayerLocal))
						{
							players.Add(entityPlayer.EntityName);
						}
					}
					players.Insert(0, this.displayName);
				}
				using (UnityWebRequest req = UnityWebRequest.Put("https://2v3d0ewjcg.execute-api.us-east-1.amazonaws.com/prod/set-developer-config", this.constructDevConfigContent(list, players)))
				{
					req.SetRequestHeader("Content-Type", "application/json");
					req.SetRequestHeader("Client-Id", "k6ji189bf7i4ge8il4iczzw7kpgmjt");
					req.SetRequestHeader("Authorization", "Bearer " + this.JWT);
					yield return req.SendWebRequest();
					if (req.result != UnityWebRequest.Result.Success)
					{
						Log.Warning("Failed to set the extension configuration: " + req.downloadHandler.text);
					}
					else
					{
						Log.Out("Extension Configuration set successfully");
					}
				}
				UnityWebRequest req = null;
				using (UnityWebRequest req = UnityWebRequest.Put("https://api.twitch.tv/helix/extensions/configurations", this.constructBroadcasterConfigContent(status, cmds)))
				{
					req.SetRequestHeader("Content-Type", "application/json");
					req.SetRequestHeader("Client-Id", "k6ji189bf7i4ge8il4iczzw7kpgmjt");
					req.SetRequestHeader("Authorization", "Bearer " + this.JWT);
					yield return req.SendWebRequest();
					if (req.result != UnityWebRequest.Result.Success)
					{
						Log.Warning(string.Format("Failed to set the extension configuration: {0}", req.downloadHandler.data));
					}
					else
					{
						Log.Out("Extension Configuration set successfully");
					}
				}
				req = null;
				yield return this.notifyStatusChange(this.constructStatusMessage(status, cmds, players));
				players = null;
			}
			else
			{
				Log.Warning("user is not a part of the extension beta. Cannot set configs.");
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BFE5 RID: 49125 RVA: 0x00469E05 File Offset: 0x00468005
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator notifyStatusChange(string body)
		{
			using (UnityWebRequest request = UnityWebRequest.Put("https://api.twitch.tv/helix/extensions/pubsub", body))
			{
				request.method = "POST";
				request.SetRequestHeader("Content-Type", "application/json");
				request.SetRequestHeader("Client-Id", "k6ji189bf7i4ge8il4iczzw7kpgmjt");
				request.SetRequestHeader("Authorization", "Bearer " + this.JWT);
				yield return request.SendWebRequest();
				if (request.result != UnityWebRequest.Result.Success)
				{
					Log.Warning("Failed to broadcast status change: " + request.downloadHandler.text);
				}
				else
				{
					Log.Out("Status Change Pubsub Message sent successfully");
				}
			}
			UnityWebRequest request = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BFE6 RID: 49126 RVA: 0x00469E1B File Offset: 0x0046801B
		[PublicizedFrom(EAccessModifier.Private)]
		public string constructDevConfigContent(List<string> eventTypes, List<string> players)
		{
			return JsonConvert.SerializeObject(new SetDevConfigRequestData
			{
				actionTypes = eventTypes,
				players = players
			});
		}

		// Token: 0x0600BFE7 RID: 49127 RVA: 0x00469E38 File Offset: 0x00468038
		[PublicizedFrom(EAccessModifier.Private)]
		public string constructBroadcasterConfigContent(string status, string cmds)
		{
			SetConfigRequestData setConfigRequestData = new SetConfigRequestData();
			setConfigRequestData.extension_id = "k6ji189bf7i4ge8il4iczzw7kpgmjt";
			setConfigRequestData.segment = "broadcaster";
			setConfigRequestData.broadcaster_id = this.userId;
			setConfigRequestData.version = "0.0.1";
			ConfigContent configContent = new ConfigContent();
			configContent.o = this.opaqueId;
			configContent.d = this.displayName;
			configContent.l = Array.FindIndex<string>(Localization.KnownLanguages, (string l) => l == Localization.ActiveLanguage).ToString();
			configContent.s = status;
			configContent.c = cmds;
			setConfigRequestData.content = JsonConvert.SerializeObject(configContent);
			return JsonConvert.SerializeObject(setConfigRequestData);
		}

		// Token: 0x0600BFE8 RID: 49128 RVA: 0x00469EEC File Offset: 0x004680EC
		[PublicizedFrom(EAccessModifier.Private)]
		public string constructStatusMessage(string status, string cmds, List<string> party)
		{
			PubSubStatusRequestData pubSubStatusRequestData = new PubSubStatusRequestData();
			pubSubStatusRequestData.broadcaster_id = this.userId;
			pubSubStatusRequestData.target = new List<string> { "broadcast" };
			PubSubStatusMessage pubSubStatusMessage = new PubSubStatusMessage();
			pubSubStatusMessage.opaqueId = this.opaqueId;
			pubSubStatusMessage.displayName = this.displayName;
			pubSubStatusMessage.language = Array.FindIndex<string>(Localization.KnownLanguages, (string l) => l == Localization.ActiveLanguage).ToString();
			pubSubStatusMessage.status = status;
			pubSubStatusMessage.commands = cmds;
			pubSubStatusMessage.party = party;
			pubSubStatusMessage.actionTypes = (from c in TwitchActionManager.Current.CategoryList.GetRange(1, 5)
				select c.DisplayName).ToList<string>();
			pubSubStatusRequestData.message = JsonConvert.SerializeObject(pubSubStatusMessage);
			return JsonConvert.SerializeObject(pubSubStatusRequestData);
		}

		// Token: 0x04008F81 RID: 36737
		[PublicizedFrom(EAccessModifier.Private)]
		public const string EXTENSION_ID = "k6ji189bf7i4ge8il4iczzw7kpgmjt";

		// Token: 0x04008F82 RID: 36738
		[PublicizedFrom(EAccessModifier.Private)]
		public const string URL = "http://localhost:52775/";

		// Token: 0x04008F83 RID: 36739
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HttpListener listener;

		// Token: 0x04008F84 RID: 36740
		[PublicizedFrom(EAccessModifier.Private)]
		public bool pushConfigChanges;

		// Token: 0x04008F85 RID: 36741
		[PublicizedFrom(EAccessModifier.Private)]
		public string JWT = string.Empty;

		// Token: 0x04008F86 RID: 36742
		[PublicizedFrom(EAccessModifier.Private)]
		public string opaqueId;

		// Token: 0x04008F87 RID: 36743
		[PublicizedFrom(EAccessModifier.Private)]
		public string userId;

		// Token: 0x04008F88 RID: 36744
		[PublicizedFrom(EAccessModifier.Private)]
		public string displayName = string.Empty;

		// Token: 0x04008F89 RID: 36745
		[PublicizedFrom(EAccessModifier.Private)]
		public string broadcaster_type = string.Empty;

		// Token: 0x04008F8A RID: 36746
		[PublicizedFrom(EAccessModifier.Private)]
		public bool inBeta;

		// Token: 0x04008F8B RID: 36747
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastUpdate;

		// Token: 0x04008F8C RID: 36748
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isConnected;

		// Token: 0x04008F8D RID: 36749
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<TwitchExtensionCommand> commands = new Queue<TwitchExtensionCommand>();

		// Token: 0x04008F8E RID: 36750
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchAction> tempCommandList = new List<TwitchAction>();

		// Token: 0x02001836 RID: 6198
		[Serializable]
		public class BitCmdVerifyResponse
		{
			// Token: 0x04008F8F RID: 36751
			public bool canSend;

			// Token: 0x04008F90 RID: 36752
			public string userId;

			// Token: 0x04008F91 RID: 36753
			public string command;
		}

		// Token: 0x02001837 RID: 6199
		[Serializable]
		public class InBetaResponse
		{
			// Token: 0x04008F92 RID: 36754
			public bool inBeta;
		}
	}
}
