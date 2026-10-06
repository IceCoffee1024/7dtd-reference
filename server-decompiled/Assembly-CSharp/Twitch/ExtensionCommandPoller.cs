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
	// Token: 0x020017F2 RID: 6130
	public class ExtensionCommandPoller
	{
		// Token: 0x0600BE6B RID: 48747 RVA: 0x004645B0 File Offset: 0x004627B0
		public void Init()
		{
			this.commandQueue = new Queue<ExtensionAction>();
			this.transactionHistory = new HashSet<string>();
			this.lastPollTime = Time.time;
			this.login = TwitchManager.Current.Authentication.userName;
			GameManager.Instance.StartCoroutine(this.CreateQueue());
		}

		// Token: 0x0600BE6C RID: 48748 RVA: 0x00464604 File Offset: 0x00462804
		public void Cleanup()
		{
			this.commandQueue.Clear();
			this.commandQueue = null;
		}

		// Token: 0x0600BE6D RID: 48749 RVA: 0x00464618 File Offset: 0x00462818
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

		// Token: 0x0600BE6E RID: 48750 RVA: 0x00464659 File Offset: 0x00462859
		[PublicizedFrom(EAccessModifier.Private)]
		public bool cooldownUpdateable()
		{
			return (TwitchManager.Current.CooldownType == TwitchManager.CooldownTypes.Startup || TwitchManager.Current.CooldownType == TwitchManager.CooldownTypes.Time) && Time.realtimeSinceStartup - this.lastPollTime > 30f;
		}

		// Token: 0x0600BE6F RID: 48751 RVA: 0x0046468C File Offset: 0x0046288C
		public void Update()
		{
			if (this.queueExists && TwitchManager.Current.AllowActions && Time.realtimeSinceStartup - this.lastPollTime > 3f && !this.isInCooldown() && TwitchManager.Current.Authentication.oauth != "")
			{
				GameManager.Instance.StartCoroutine(this.PollQueue());
				this.lastPollTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x0600BE70 RID: 48752 RVA: 0x004646FF File Offset: 0x004628FF
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

		// Token: 0x0600BE71 RID: 48753 RVA: 0x0046470E File Offset: 0x0046290E
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

		// Token: 0x0600BE72 RID: 48754 RVA: 0x0046471D File Offset: 0x0046291D
		[PublicizedFrom(EAccessModifier.Private)]
		public void DeleteTransactionFromTable(List<string> transactions)
		{
			GameManager.Instance.StartCoroutine(this.DeleteTransactionFromTableCoroutine(transactions));
		}

		// Token: 0x0600BE73 RID: 48755 RVA: 0x00464731 File Offset: 0x00462931
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

		// Token: 0x0600BE74 RID: 48756 RVA: 0x00464740 File Offset: 0x00462940
		public bool HasCommand()
		{
			return this.commandQueue.Count > 0;
		}

		// Token: 0x0600BE75 RID: 48757 RVA: 0x00464750 File Offset: 0x00462950
		public ExtensionAction GetCommand()
		{
			return this.commandQueue.Dequeue();
		}

		// Token: 0x04008E38 RID: 36408
		[PublicizedFrom(EAccessModifier.Private)]
		public const long BIT_ACTION_TIMEOUT_MS = 30000L;

		// Token: 0x04008E39 RID: 36409
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<ExtensionAction> commandQueue;

		// Token: 0x04008E3A RID: 36410
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<string> transactionHistory;

		// Token: 0x04008E3B RID: 36411
		[PublicizedFrom(EAccessModifier.Private)]
		public string login;

		// Token: 0x04008E3C RID: 36412
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastPollTime;

		// Token: 0x04008E3D RID: 36413
		[PublicizedFrom(EAccessModifier.Private)]
		public bool queueExists;
	}
}
