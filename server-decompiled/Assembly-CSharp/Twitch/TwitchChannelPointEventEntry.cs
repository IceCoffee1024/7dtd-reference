using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch
{
	// Token: 0x0200182D RID: 6189
	public class TwitchChannelPointEventEntry : BaseTwitchEventEntry
	{
		// Token: 0x0600BF1A RID: 48922 RVA: 0x00469052 File Offset: 0x00467252
		public override bool IsValid(int amount = -1, string name = "", TwitchSubEventEntry.SubTierTypes subTier = TwitchSubEventEntry.SubTierTypes.Any)
		{
			return this.ChannelPointTitle == name;
		}

		// Token: 0x0600BF1B RID: 48923 RVA: 0x00469060 File Offset: 0x00467260
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

		// Token: 0x0600BF1C RID: 48924 RVA: 0x004690E2 File Offset: 0x004672E2
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

		// Token: 0x0600BF1D RID: 48925 RVA: 0x004690FF File Offset: 0x004672FF
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

		// Token: 0x04008F8D RID: 36749
		public string ChannelPointTitle = "";

		// Token: 0x04008F8E RID: 36750
		public int Cost = 1000;

		// Token: 0x04008F8F RID: 36751
		public int MaxPerUserPerStream;

		// Token: 0x04008F90 RID: 36752
		public int MaxPerStream;

		// Token: 0x04008F91 RID: 36753
		public int GlobalCooldown;

		// Token: 0x04008F92 RID: 36754
		public string ChannelPointID = "";

		// Token: 0x04008F93 RID: 36755
		public bool AutoCreate = true;

		// Token: 0x0200182E RID: 6190
		[Serializable]
		public class CreateCustomRewards
		{
			// Token: 0x04008F94 RID: 36756
			public List<TwitchChannelPointEventEntry.CreateCustomReward> data = new List<TwitchChannelPointEventEntry.CreateCustomReward>();
		}

		// Token: 0x0200182F RID: 6191
		[Serializable]
		public class CreateCustomReward
		{
			// Token: 0x04008F95 RID: 36757
			public string broadcaster_id;

			// Token: 0x04008F96 RID: 36758
			public string title;

			// Token: 0x04008F97 RID: 36759
			public string background_color = "#F13030";

			// Token: 0x04008F98 RID: 36760
			public int cost;

			// Token: 0x04008F99 RID: 36761
			public int max_per_user_per_stream;

			// Token: 0x04008F9A RID: 36762
			public bool is_max_per_user_per_stream_enabled;

			// Token: 0x04008F9B RID: 36763
			public int max_per_stream;

			// Token: 0x04008F9C RID: 36764
			public bool is_max_per_stream_enabled;

			// Token: 0x04008F9D RID: 36765
			public int global_cooldown_seconds;

			// Token: 0x04008F9E RID: 36766
			public bool is_global_cooldown_enabled;
		}

		// Token: 0x02001830 RID: 6192
		[Serializable]
		public class CreateCustomRewardResponses
		{
			// Token: 0x04008F9F RID: 36767
			public List<TwitchChannelPointEventEntry.CreateCustomRewardResponse> data;
		}

		// Token: 0x02001831 RID: 6193
		[Serializable]
		public class CreateCustomRewardResponse
		{
			// Token: 0x04008FA0 RID: 36768
			public string id;

			// Token: 0x04008FA1 RID: 36769
			public string title;
		}

		// Token: 0x02001832 RID: 6194
		[Serializable]
		public class ErrorResponse
		{
			// Token: 0x04008FA2 RID: 36770
			public string status;

			// Token: 0x04008FA3 RID: 36771
			public string message;
		}
	}
}
