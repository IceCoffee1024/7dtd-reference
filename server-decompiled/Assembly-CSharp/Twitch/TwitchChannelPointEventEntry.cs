using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x0200188A RID: 6282
	public class TwitchChannelPointEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600C186 RID: 49542 RVA: 0x00470F3A File Offset: 0x0046F13A
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return this.ChannelPointTitle == name;
		}

		// Token: 0x0600C187 RID: 49543 RVA: 0x00470F48 File Offset: 0x0046F148
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

		// Token: 0x0600C188 RID: 49544 RVA: 0x00470FCA File Offset: 0x0046F1CA
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

		// Token: 0x0600C189 RID: 49545 RVA: 0x00470FE7 File Offset: 0x0046F1E7
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

		// Token: 0x0400913C RID: 37180
		public string ChannelPointTitle = "";

		// Token: 0x0400913D RID: 37181
		public int Cost = 1000;

		// Token: 0x0400913E RID: 37182
		public int MaxPerUserPerStream;

		// Token: 0x0400913F RID: 37183
		public int MaxPerStream;

		// Token: 0x04009140 RID: 37184
		public int GlobalCooldown;

		// Token: 0x04009141 RID: 37185
		public string ChannelPointID = "";

		// Token: 0x04009142 RID: 37186
		public bool AutoCreate = true;

		// Token: 0x0200188B RID: 6283
		[Serializable]
		public class CreateCustomRewards
		{
			// Token: 0x04009143 RID: 37187
			public List<TwitchChannelPointEventEntry.CreateCustomReward> data = new List<TwitchChannelPointEventEntry.CreateCustomReward>();
		}

		// Token: 0x0200188C RID: 6284
		[Serializable]
		public class CreateCustomReward
		{
			// Token: 0x04009144 RID: 37188
			public string broadcaster_id;

			// Token: 0x04009145 RID: 37189
			public string title;

			// Token: 0x04009146 RID: 37190
			public string background_color = "#F13030";

			// Token: 0x04009147 RID: 37191
			public int cost;

			// Token: 0x04009148 RID: 37192
			public int max_per_user_per_stream;

			// Token: 0x04009149 RID: 37193
			public bool is_max_per_user_per_stream_enabled;

			// Token: 0x0400914A RID: 37194
			public int max_per_stream;

			// Token: 0x0400914B RID: 37195
			public bool is_max_per_stream_enabled;

			// Token: 0x0400914C RID: 37196
			public int global_cooldown_seconds;

			// Token: 0x0400914D RID: 37197
			public bool is_global_cooldown_enabled;
		}

		// Token: 0x0200188D RID: 6285
		[Serializable]
		public class CreateCustomRewardResponses
		{
			// Token: 0x0400914E RID: 37198
			public List<TwitchChannelPointEventEntry.CreateCustomRewardResponse> data;
		}

		// Token: 0x0200188E RID: 6286
		[Serializable]
		public class CreateCustomRewardResponse
		{
			// Token: 0x0400914F RID: 37199
			public string id;

			// Token: 0x04009150 RID: 37200
			public string title;
		}

		// Token: 0x0200188F RID: 6287
		[Serializable]
		public class ErrorResponse
		{
			// Token: 0x04009151 RID: 37201
			public string status;

			// Token: 0x04009152 RID: 37202
			public string message;
		}
	}
}
