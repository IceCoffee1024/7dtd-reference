using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x02001859 RID: 6233
	public class TwitchChannelPointEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600C072 RID: 49266 RVA: 0x0046D58E File Offset: 0x0046B78E
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return this.ChannelPointTitle == name;
		}

		// Token: 0x0600C073 RID: 49267 RVA: 0x0046D59C File Offset: 0x0046B79C
		public TwitchChannelPointEventEntry.CreateCustomReward SetupRewardEntry(string channelID)
		{
			return new TwitchChannelPointEventEntry.CreateCustomReward
			{
				broadcaster_id = channelID,
				title = this.ChannelPointTitle,
				cost = this.Cost,
				is_max_per_user_per_stream_enabled = (this.MaxPerUserPerStream > 0),
				max_per_user_per_stream = this.MaxPerUserPerStream,
				is_max_per_stream_enabled = (this.MaxPerStream > 0),
				max_per_stream = this.MaxPerStream,
				is_global_cooldown_enabled = (this.GlobalCooldown > 0),
				global_cooldown_seconds = this.GlobalCooldown
			};
		}

		// Token: 0x0600C074 RID: 49268 RVA: 0x0046D61E File Offset: 0x0046B81E
		public static IEnumerator CreateCustomRewardPost(TwitchChannelPointEventEntry.CreateCustomReward _rd, Action<string> _onSucess, Action<string> _onFail)
		{
			yield return new WaitUntil(() => TwitchManager.Current.Authentication != null && TwitchManager.Current.Authentication.oauth != "" && TwitchManager.Current.Authentication.userID != "");
			Log.Out("creating Custom reward on: " + TwitchManager.Current.Authentication.userID);
			string text = "https://api.twitch.tv/helix/channel_points/custom_rewards?broadcaster_id=" + TwitchManager.Current.Authentication.userID;
			string text2 = JsonUtility.ToJson(_rd);
			using (UnityWebRequest req = UnityWebRequest.Put(text, text2))
			{
				req.method = "POST";
				req.SetRequestHeader("Authorization", "Bearer " + TwitchManager.Current.Authentication.oauth.Substring(6));
				req.SetRequestHeader("Client-Id", TwitchAuthentication.client_id);
				req.SetRequestHeader("Content-Type", "application/json");
				yield return req.SendWebRequest();
				if (req.result == UnityWebRequest.Result.Success)
				{
					Log.Out("sucessfully created Custom Channel Points Reward");
					_onSucess(req.downloadHandler.text);
				}
				else
				{
					TwitchChannelPointEventEntry.ErrorResponse errorResponse = JsonConvert.DeserializeObject<TwitchChannelPointEventEntry.ErrorResponse>(req.downloadHandler.text);
					if (errorResponse != null)
					{
						_onFail("response code: " + errorResponse.status + "\nmessage: " + errorResponse.message);
					}
					else
					{
						_onFail("Something went wrong. Please Try again.");
					}
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x0600C075 RID: 49269 RVA: 0x0046D63B File Offset: 0x0046B83B
		public static IEnumerator DeleteCustomRewardsDelete(string id, Action<string> _onSucess, Action<string> _onFail)
		{
			string text = string.Format("https://api.twitch.tv/helix/channel_points/custom_rewards?broadcaster_id={0}&id={1}", TwitchManager.Current.Authentication.userID, id);
			using (UnityWebRequest req = UnityWebRequest.Delete(text))
			{
				req.method = "DELETE";
				req.SetRequestHeader("Authorization", "Bearer " + TwitchManager.Current.Authentication.oauth.Substring(6));
				req.SetRequestHeader("Client-Id", TwitchAuthentication.client_id);
				yield return req.SendWebRequest();
				if (req.result == UnityWebRequest.Result.Success)
				{
					_onSucess("Success");
				}
				else
				{
					Debug.Log(string.Format("response code: {0}", req.responseCode));
					if (req.responseCode == 404L)
					{
						_onSucess("Not Found");
					}
					else
					{
						TwitchChannelPointEventEntry.ErrorResponse errorResponse = JsonConvert.DeserializeObject<TwitchChannelPointEventEntry.ErrorResponse>(req.downloadHandler.text);
						if (errorResponse != null)
						{
							_onFail(errorResponse.message);
						}
						else
						{
							_onFail("Something went wrong. Please Try again.");
						}
					}
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x04009053 RID: 36947
		public string ChannelPointTitle = "";

		// Token: 0x04009054 RID: 36948
		public int Cost = 1000;

		// Token: 0x04009055 RID: 36949
		public int MaxPerUserPerStream;

		// Token: 0x04009056 RID: 36950
		public int MaxPerStream;

		// Token: 0x04009057 RID: 36951
		public int GlobalCooldown;

		// Token: 0x04009058 RID: 36952
		public string ChannelPointID = "";

		// Token: 0x04009059 RID: 36953
		public bool AutoCreate = true;

		// Token: 0x0200185A RID: 6234
		[Serializable]
		public class CreateCustomRewards
		{
			// Token: 0x0400905A RID: 36954
			public List<TwitchChannelPointEventEntry.CreateCustomReward> data = new List<TwitchChannelPointEventEntry.CreateCustomReward>();
		}

		// Token: 0x0200185B RID: 6235
		[Serializable]
		public class CreateCustomReward
		{
			// Token: 0x0400905B RID: 36955
			public string broadcaster_id;

			// Token: 0x0400905C RID: 36956
			public string title;

			// Token: 0x0400905D RID: 36957
			public string background_color = "#F13030";

			// Token: 0x0400905E RID: 36958
			public int cost;

			// Token: 0x0400905F RID: 36959
			public int max_per_user_per_stream;

			// Token: 0x04009060 RID: 36960
			public bool is_max_per_user_per_stream_enabled;

			// Token: 0x04009061 RID: 36961
			public int max_per_stream;

			// Token: 0x04009062 RID: 36962
			public bool is_max_per_stream_enabled;

			// Token: 0x04009063 RID: 36963
			public int global_cooldown_seconds;

			// Token: 0x04009064 RID: 36964
			public bool is_global_cooldown_enabled;
		}

		// Token: 0x0200185C RID: 6236
		[Serializable]
		public class CreateCustomRewardResponses
		{
			// Token: 0x04009065 RID: 36965
			public List<TwitchChannelPointEventEntry.CreateCustomRewardResponse> data;
		}

		// Token: 0x0200185D RID: 6237
		[Serializable]
		public class CreateCustomRewardResponse
		{
			// Token: 0x04009066 RID: 36966
			public string id;

			// Token: 0x04009067 RID: 36967
			public string title;
		}

		// Token: 0x0200185E RID: 6238
		[Serializable]
		public class ErrorResponse
		{
			// Token: 0x04009068 RID: 36968
			public string status;

			// Token: 0x04009069 RID: 36969
			public string message;
		}
	}
}
