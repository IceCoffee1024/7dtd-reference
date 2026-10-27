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
	// Token: 0x02001823 RID: 6179
	public class ExtensionCommandPoller
	{
		// Token: 0x0600BF7F RID: 49023 RVA: 0x00467F5C File Offset: 0x0046615C
		public void Init()
		{
			this.commandQueue = new Queue<ExtensionAction>();
			this.transactionHistory = new HashSet<string>();
			this.lastPollTime = Time.time;
			this.login = TwitchManager.Current.Authentication.userName;
			GameManager.Instance.StartCoroutine(this.CreateQueue());
		}

		// Token: 0x0600BF80 RID: 49024 RVA: 0x00467FB0 File Offset: 0x004661B0
		public void Cleanup()
		{
			this.commandQueue.Clear();
			this.commandQueue = null;
		}

		// Token: 0x0600BF81 RID: 49025 RVA: 0x00467FC4 File Offset: 0x004661C4
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isInCooldown()
		{
			switch (TwitchManager.Current.CooldownType)
			{
			case TwitchManager.CooldownTypes.Startup:
			case TwitchManager.CooldownTypes.Time:
			case TwitchManager.CooldownTypes.BloodMoonDisabled:
			case TwitchManager.CooldownTypes.QuestDisabled:
				return true;
			}
			return false;
		}

		// Token: 0x0600BF82 RID: 49026 RVA: 0x00468005 File Offset: 0x00466205
		[PublicizedFrom(EAccessModifier.Private)]
		public bool cooldownUpdateable()
		{
			return (TwitchManager.Current.CooldownType == TwitchManager.CooldownTypes.Startup || TwitchManager.Current.CooldownType == TwitchManager.CooldownTypes.Time) && Time.realtimeSinceStartup - this.lastPollTime > 30f;
		}

		// Token: 0x0600BF83 RID: 49027 RVA: 0x00468038 File Offset: 0x00466238
		public void Update()
		{
			if (this.queueExists && TwitchManager.Current.AllowActions && Time.realtimeSinceStartup - this.lastPollTime > 3f && !this.isInCooldown() && TwitchManager.Current.Authentication.oauth != "")
			{
				GameManager.Instance.StartCoroutine(this.PollQueue());
				this.lastPollTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x0600BF84 RID: 49028 RVA: 0x004680AB File Offset: 0x004662AB
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator PollQueue()
		{
			using (UnityWebRequest req = UnityWebRequest.Get("https://2v3d0ewjcg.execute-api.us-east-1.amazonaws.com/prod/broadcaster/actions"))
			{
				req.SetRequestHeader("Authorization", TwitchManager.Current.Authentication.userID + " " + TwitchManager.Current.Authentication.oauth.Substring(6));
				req.SetRequestHeader("Content-Type", "application/json");
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning(string.Format("Could not retrieve commands with status code {0}: {1}", req.responseCode, req.downloadHandler.text));
				}
				else
				{
					try
					{
						ExtensionActionResponse extensionActionResponse = JsonConvert.DeserializeObject<ExtensionActionResponse>(req.downloadHandler.text);
						if (this.commandQueue != null)
						{
							if (extensionActionResponse.bitActions.Count > 0)
							{
								long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
								extensionActionResponse.bitActions.Sort((ExtensionBitAction a, ExtensionBitAction b) => a.time_created.CompareTo(b.time_created));
								extensionActionResponse.bitActions.ForEach(delegate(ExtensionBitAction bitAction)
								{
									if (this.transactionHistory.Contains(bitAction.txn_id))
									{
										Log.Warning("duplicate transaction received with id " + bitAction.txn_id);
										return;
									}
									this.transactionHistory.Add(bitAction.txn_id);
									if (currentTime - bitAction.time_created <= 30000L)
									{
										Log.Out(string.Concat(new string[] { "bit action ", bitAction.command, " received from ", bitAction.username, " with txn_id ", bitAction.txn_id }));
										this.commandQueue.Enqueue(bitAction);
										return;
									}
									int num;
									string text;
									if (int.TryParse(bitAction.username, out num) && TwitchManager.Current.ViewerData.IdToUsername.TryGetValue(num, out text))
									{
										TwitchManager twitchManager = TwitchManager.Current;
										twitchManager.AddToBitPot((int)((float)bitAction.cost * twitchManager.BitPotPercentage));
										twitchManager.ViewerData.AddCredit(text, bitAction.cost, false);
										ViewerEntry viewerEntry = TwitchManager.Current.ViewerData.GetViewerEntry(text);
										twitchManager.extensionManager.PushUserBalance(new ValueTuple<string, int>(bitAction.username, viewerEntry.BitCredits));
										return;
									}
									Log.Warning("could not give credit to user id " + bitAction.username);
								});
								this.DeleteTransactionFromTable(extensionActionResponse.bitActions.Select<ExtensionBitAction, string>((ExtensionBitAction a) => a.txn_id).ToList<string>());
							}
							extensionActionResponse.standardActions.ForEach(delegate(ExtensionAction cmd)
							{
								if (!cmd.command.Equals("#refreshcredit"))
								{
									this.commandQueue.Enqueue(cmd);
									return;
								}
								int num2;
								string text2;
								if (int.TryParse(cmd.username, out num2) && TwitchManager.Current.ViewerData.IdToUsername.TryGetValue(num2, out text2))
								{
									ViewerEntry viewerEntry2 = TwitchManager.Current.ViewerData.GetViewerEntry(text2);
									TwitchManager.Current.extensionManager.PushUserBalance(new ValueTuple<string, int>(cmd.username, viewerEntry2.BitCredits));
									TwitchManager.Current.extensionManager.PushViewerChatState(cmd.username, true);
									Log.Out("added " + cmd.username + " to new chatters");
									return;
								}
								TwitchManager.Current.extensionManager.PushViewerChatState(cmd.username, false);
							});
						}
					}
					catch (Exception ex)
					{
						Log.Warning("command poller encountered issue receving this data: " + req.downloadHandler.text + "\n excption thrown: " + ex.Message);
					}
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BF85 RID: 49029 RVA: 0x004680BA File Offset: 0x004662BA
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator CreateQueue()
		{
			using (UnityWebRequest req = UnityWebRequest.Put("https://2v3d0ewjcg.execute-api.us-east-1.amazonaws.com/prod/command-queue", "{}"))
			{
				req.SetRequestHeader("Authorization", this.login + " " + TwitchManager.Current.Authentication.oauth.Substring(6));
				req.SetRequestHeader("Content-Type", "application/json");
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning("Could not create queue");
				}
				else
				{
					JObject jobject = JObject.Parse(req.downloadHandler.text);
					JToken jtoken;
					this.queueExists = jobject != null && jobject.TryGetValue("message", out jtoken) && jtoken.ToString() == "success";
					if (!this.queueExists)
					{
						Log.Warning("Could not create queue");
					}
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BF86 RID: 49030 RVA: 0x004680C9 File Offset: 0x004662C9
		[PublicizedFrom(EAccessModifier.Private)]
		public void DeleteTransactionFromTable(List<string> transactions)
		{
			GameManager.Instance.StartCoroutine(this.DeleteTransactionFromTableCoroutine(transactions));
		}

		// Token: 0x0600BF87 RID: 49031 RVA: 0x004680DD File Offset: 0x004662DD
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator DeleteTransactionFromTableCoroutine(List<string> transactions)
		{
			string text = JsonConvert.SerializeObject(new ExtensionDeleteBitActionsRequestData
			{
				transactions = transactions
			});
			using (UnityWebRequest req = UnityWebRequest.Put("https://2v3d0ewjcg.execute-api.us-east-1.amazonaws.com/prod/broadcaster/actions", text))
			{
				req.method = "DELETE";
				req.SetRequestHeader("Authorization", TwitchManager.Current.Authentication.userID + " " + TwitchManager.Current.Authentication.oauth.Substring(6));
				req.SetRequestHeader("Content-Type", "application/json");
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Log.Warning("Failed to delete the transactions");
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BF88 RID: 49032 RVA: 0x004680EC File Offset: 0x004662EC
		public bool HasCommand()
		{
			return this.commandQueue.Count > 0;
		}

		// Token: 0x0600BF89 RID: 49033 RVA: 0x004680FC File Offset: 0x004662FC
		public ExtensionAction GetCommand()
		{
			return this.commandQueue.Dequeue();
		}

		// Token: 0x04008F21 RID: 36641
		[PublicizedFrom(EAccessModifier.Private)]
		public const long BIT_ACTION_TIMEOUT_MS = 30000L;

		// Token: 0x04008F22 RID: 36642
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<ExtensionAction> commandQueue;

		// Token: 0x04008F23 RID: 36643
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<string> transactionHistory;

		// Token: 0x04008F24 RID: 36644
		[PublicizedFrom(EAccessModifier.Private)]
		public string login;

		// Token: 0x04008F25 RID: 36645
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastPollTime;

		// Token: 0x04008F26 RID: 36646
		[PublicizedFrom(EAccessModifier.Private)]
		public bool queueExists;
	}
}
