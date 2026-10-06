using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using UniLinq;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x02001815 RID: 6165
	public class ExtensionPubSubManager
	{
		// Token: 0x0600BF12 RID: 48914 RVA: 0x004673E0 File Offset: 0x004655E0
		public void SetJWT(string jwt)
		{
			this.jwt = jwt;
		}

		// Token: 0x0600BF13 RID: 48915 RVA: 0x004673EC File Offset: 0x004655EC
		public void Update(bool updatedViewerConfig)
		{
			if (updatedViewerConfig)
			{
				this.updateSignature = Guid.NewGuid().ToString();
			}
			this.SendUpdate();
		}

		// Token: 0x0600BF14 RID: 48916 RVA: 0x0046741B File Offset: 0x0046561B
		public void PushUserBalance(ValueTuple<string, int> userBalance)
		{
			Log.Out(string.Format("Adding balance of {0} to user {1}", userBalance.Item2, userBalance.Item1));
			Log.Out(new StackTrace(true).ToString());
			this.UserBitBalances.Enqueue(userBalance);
		}

		// Token: 0x0600BF15 RID: 48917 RVA: 0x00467459 File Offset: 0x00465659
		public void PushViewerChatState(string id, bool hasChatted)
		{
			this.viewerChatStateQueue.Enqueue(new ValueTuple<string, bool>(id, hasChatted));
		}

		// Token: 0x0600BF16 RID: 48918 RVA: 0x0046746D File Offset: 0x0046566D
		[PublicizedFrom(EAccessModifier.Private)]
		public void SendUpdate()
		{
			GameManager.Instance.StartCoroutine(this.UpdateViewers());
		}

		// Token: 0x0600BF17 RID: 48919 RVA: 0x00467480 File Offset: 0x00465680
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator UpdateViewers()
		{
			int num = Utils.FastMin(this.UserBitBalances.Count, 100);
			for (int i = 0; i < num; i++)
			{
				ValueTuple<string, int> valueTuple = this.UserBitBalances.Dequeue();
				if (this.bitBalancesByUser.ContainsKey(valueTuple.Item1))
				{
					this.bitBalancesByUser[valueTuple.Item1] = valueTuple.Item2;
				}
				else
				{
					this.bitBalancesByUser.Add(valueTuple.Item1, valueTuple.Item2);
				}
			}
			num = Utils.FastMin(this.viewerChatStateQueue.Count, 100);
			for (int j = 0; j < num; j++)
			{
				ValueTuple<string, bool> valueTuple2 = this.viewerChatStateQueue.Dequeue();
				if (!this.chattersToSend.ContainsKey(valueTuple2.Item1))
				{
					this.chattersToSend.Add(valueTuple2.Item1, valueTuple2.Item2);
				}
			}
			string text = JsonConvert.SerializeObject(new UpdateMessage
			{
				updateSignature = this.updateSignature,
				status = this.getStatus(),
				actionCooldowns = this.getActionCooldowns(),
				bitBalances = this.bitBalancesByUser,
				hasChatted = this.chattersToSend
			});
			this.bitBalancesByUser.Clear();
			this.chattersToSend.Clear();
			string text2 = JsonConvert.SerializeObject(new PubSubStatusRequestData
			{
				broadcaster_id = TwitchManager.Current.Authentication.userID,
				target = ExtensionPubSubManager.target,
				message = text
			});
			using (UnityWebRequest request = UnityWebRequest.Put("https://api.twitch.tv/helix/extensions/pubsub", text2))
			{
				request.method = "POST";
				request.SetRequestHeader("Content-Type", "application/json");
				request.SetRequestHeader("Client-Id", "k6ji189bf7i4ge8il4iczzw7kpgmjt");
				request.SetRequestHeader("Authorization", "Bearer " + this.jwt);
				yield return request.SendWebRequest();
				if (request.result != UnityWebRequest.Result.Success)
				{
					Log.Warning("Failed to broadcast status change: " + request.downloadHandler.text);
				}
				else if (request.responseCode == 403L)
				{
					TwitchManager.Current.extensionManager.RetrieveJWT();
				}
			}
			UnityWebRequest request = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BF18 RID: 48920 RVA: 0x00467490 File Offset: 0x00465690
		[PublicizedFrom(EAccessModifier.Private)]
		public string getStatus()
		{
			TwitchManager twitchManager = TwitchManager.Current;
			if (!twitchManager.TwitchActive)
			{
				return "paused";
			}
			if (twitchManager.CurrentActionPreset.IsEmpty || twitchManager.VoteLockedLevel == TwitchVoteLockTypes.ActionsLocked)
			{
				return "actionsDisabled";
			}
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

		// Token: 0x0600BF19 RID: 48921 RVA: 0x00467550 File Offset: 0x00465750
		[PublicizedFrom(EAccessModifier.Private)]
		public int[] getActionCooldowns()
		{
			TwitchAction[] array = TwitchManager.Current.AvailableCommands.Values.Where<TwitchAction>((TwitchAction a) => a.HasExtraConditions() && (TwitchManager.Current.extensionManager.CanUseBitCommands() || a.PointType != TwitchAction.PointTypes.Bits)).ToArray<TwitchAction>();
			int[] array2 = new int[array.Count<TwitchAction>() / 32 + ((array.Count<TwitchAction>() % 32 != 0) ? 1 : 0)];
			int num = 0;
			foreach (TwitchAction twitchAction in array)
			{
				array2[num / 32] |= (twitchAction.IsReady(TwitchManager.Current) ? 0 : (1 << num % 32));
				num++;
			}
			return array2;
		}

		// Token: 0x04008EE2 RID: 36578
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<string> target = new List<string> { "broadcast" };

		// Token: 0x04008EE3 RID: 36579
		[PublicizedFrom(EAccessModifier.Private)]
		public string jwt;

		// Token: 0x04008EE4 RID: 36580
		[PublicizedFrom(EAccessModifier.Private)]
		public string updateSignature;

		// Token: 0x04008EE5 RID: 36581
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<ValueTuple<string, int>> UserBitBalances = new Queue<ValueTuple<string, int>>();

		// Token: 0x04008EE6 RID: 36582
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, int> bitBalancesByUser = new Dictionary<string, int>();

		// Token: 0x04008EE7 RID: 36583
		[PublicizedFrom(EAccessModifier.Private)]
		public Queue<ValueTuple<string, bool>> viewerChatStateQueue = new Queue<ValueTuple<string, bool>>();

		// Token: 0x04008EE8 RID: 36584
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<string, bool> chattersToSend = new Dictionary<string, bool>();
	}
}
