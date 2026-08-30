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
	// Token: 0x020017C6 RID: 6086
	public class ExtensionCommandPoller
	{
		// Token: 0x0600BD13 RID: 48403 RVA: 0x00460074 File Offset: 0x0045E274
		public void Init()
		{
			this.commandQueue = new Queue<ExtensionAction>();
			this.transactionHistory = new HashSet<string>();
			this.lastPollTime = Time.time;
			this.login = TwitchManager.Current.Authentication.userName;
			GameManager.Instance.StartCoroutine(this.CreateQueue());
		}

		// Token: 0x0600BD14 RID: 48404 RVA: 0x004600C8 File Offset: 0x0045E2C8
		public void Cleanup()
		{
			this.commandQueue.Clear();
			this.commandQueue = null;
		}

		// Token: 0x0600BD15 RID: 48405 RVA: 0x004600DC File Offset: 0x0045E2DC
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

		// Token: 0x0600BD16 RID: 48406 RVA: 0x0046011D File Offset: 0x0045E31D
		[PublicizedFrom(EAccessModifier.Private)]
		public bool cooldownUpdateable()
		{
			return (TwitchManager.Current.CooldownType == TwitchManager.CooldownTypes.Startup || TwitchManager.Current.CooldownType == TwitchManager.CooldownTypes.Time) && Time.realtimeSinceStartup - this.lastPollTime > 30f;
		}

		// Token: 0x0600BD17 RID: 48407 RVA: 0x00460150 File Offset: 0x0045E350
		public void Update()
		{
			if (this.queueExists && TwitchManager.Current.AllowActions && Time.realtimeSinceStartup - this.lastPollTime > 3f && !this.isInCooldown() && TwitchManager.Current.Authentication.oauth != "")
			{
				GameManager.Instance.StartCoroutine(this.PollQueue());
				this.lastPollTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x0600BD18 RID: 48408 RVA: 0x004601C3 File Offset: 0x0045E3C3
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

		// Token: 0x0600BD19 RID: 48409 RVA: 0x004601D2 File Offset: 0x0045E3D2
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

		// Token: 0x0600BD1A RID: 48410 RVA: 0x004601E1 File Offset: 0x0045E3E1
		[PublicizedFrom(EAccessModifier.Private)]
		public void DeleteTransactionFromTable(List<string> transactions)
		{
			GameManager.Instance.StartCoroutine(this.DeleteTransactionFromTableCoroutine(transactions));
		}

		// Token: 0x0600BD1B RID: 48411 RVA: 0x004601F5 File Offset: 0x0045E3F5
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

		// Token: 0x0600BD1C RID: 48412 RVA: 0x00460204 File Offset: 0x0045E404
		public bool HasCommand()
		{
			return this.commandQueue.Count > 0;
		}

		// Token: 0x0600BD1D RID: 48413 RVA: 0x00460214 File Offset: 0x0045E414
		public ExtensionAction GetCommand()
		{
			return this.commandQueue.Dequeue();
		}

		// Token: 0x04008D72 RID: 36210
		[PublicizedFrom(EAccessModifier.Private)]
		public const long BIT_ACTION_TIMEOUT_MS = 30000L;

		// Token: 0x04008D73 RID: 36211
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<ExtensionAction> commandQueue;

		// Token: 0x04008D74 RID: 36212
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<string> transactionHistory;

		// Token: 0x04008D75 RID: 36213
		[PublicizedFrom(EAccessModifier.Private)]
		public string login;

		// Token: 0x04008D76 RID: 36214
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastPollTime;

		// Token: 0x04008D77 RID: 36215
		[PublicizedFrom(EAccessModifier.Private)]
		public bool queueExists;
	}
}
