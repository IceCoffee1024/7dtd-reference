using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x02001807 RID: 6151
	public class TwitchChannelPointEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600BD95 RID: 48533 RVA: 0x004632B6 File Offset: 0x004614B6
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return this.ChannelPointTitle == name;
		}

		// Token: 0x0600BD96 RID: 48534 RVA: 0x004632C4 File Offset: 0x004614C4
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

		// Token: 0x0600BD97 RID: 48535 RVA: 0x00463346 File Offset: 0x00461546
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

		// Token: 0x0600BD98 RID: 48536 RVA: 0x00463363 File Offset: 0x00461563
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

		// Token: 0x04008E71 RID: 36465
		public string ChannelPointTitle = "";

		// Token: 0x04008E72 RID: 36466
		public int Cost = 1000;

		// Token: 0x04008E73 RID: 36467
		public int MaxPerUserPerStream;

		// Token: 0x04008E74 RID: 36468
		public int MaxPerStream;

		// Token: 0x04008E75 RID: 36469
		public int GlobalCooldown;

		// Token: 0x04008E76 RID: 36470
		public string ChannelPointID = "";

		// Token: 0x04008E77 RID: 36471
		public bool AutoCreate = true;

		// Token: 0x02001808 RID: 6152
		[Serializable]
		public class CreateCustomRewards
		{
			// Token: 0x04008E78 RID: 36472
			public List<TwitchChannelPointEventEntry.CreateCustomReward> data = new List<TwitchChannelPointEventEntry.CreateCustomReward>();
		}

		// Token: 0x02001809 RID: 6153
		[Serializable]
		public class CreateCustomReward
		{
			// Token: 0x04008E79 RID: 36473
			public string broadcaster_id;

			// Token: 0x04008E7A RID: 36474
			public string title;

			// Token: 0x04008E7B RID: 36475
			public string background_color = "#F13030";

			// Token: 0x04008E7C RID: 36476
			public int cost;

			// Token: 0x04008E7D RID: 36477
			public int max_per_user_per_stream;

			// Token: 0x04008E7E RID: 36478
			public bool is_max_per_user_per_stream_enabled;

			// Token: 0x04008E7F RID: 36479
			public int max_per_stream;

			// Token: 0x04008E80 RID: 36480
			public bool is_max_per_stream_enabled;

			// Token: 0x04008E81 RID: 36481
			public int global_cooldown_seconds;

			// Token: 0x04008E82 RID: 36482
			public bool is_global_cooldown_enabled;
		}

		// Token: 0x0200180A RID: 6154
		[Serializable]
		public class CreateCustomRewardResponses
		{
			// Token: 0x04008E83 RID: 36483
			public List<TwitchChannelPointEventEntry.CreateCustomRewardResponse> data;
		}

		// Token: 0x0200180B RID: 6155
		[Serializable]
		public class CreateCustomRewardResponse
		{
			// Token: 0x04008E84 RID: 36484
			public string id;

			// Token: 0x04008E85 RID: 36485
			public string title;
		}

		// Token: 0x0200180C RID: 6156
		[Serializable]
		public class ErrorResponse
		{
			// Token: 0x04008E86 RID: 36486
			public string status;

			// Token: 0x04008E87 RID: 36487
			public string message;
		}
	}
}
