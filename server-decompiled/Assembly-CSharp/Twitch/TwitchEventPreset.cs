using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001864 RID: 6244
	public class TwitchEventPreset
	{
		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x0600C091 RID: 49297 RVA: 0x0046DC20 File Offset: 0x0046BE20
		public bool HasCustomEvents
		{
			get
			{
				return this.BitEvents.Count > 0 || this.SubEvents.Count > 0 || this.GiftSubEvents.Count > 0 || this.RaidEvents.Count > 0 || this.CharityEvents.Count > 0 || this.ChannelPointEvents.Count > 0 || this.HypeTrainEvents.Count > 0 || this.CreatorGoalEvents.Count > 0;
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x0600C092 RID: 49298 RVA: 0x0046DC9F File Offset: 0x0046BE9F
		public bool HasBitEvents
		{
			get
			{
				return this.BitEvents.Count > 0;
			}
		}

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x0600C093 RID: 49299 RVA: 0x0046DCAF File Offset: 0x0046BEAF
		public bool HasSubEvents
		{
			get
			{
				return this.SubEvents.Count > 0;
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x0600C094 RID: 49300 RVA: 0x0046DCBF File Offset: 0x0046BEBF
		public bool HasGiftSubEvents
		{
			get
			{
				return this.GiftSubEvents.Count > 0;
			}
		}

		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x0600C095 RID: 49301 RVA: 0x0046DCCF File Offset: 0x0046BECF
		public bool HasRaidEvents
		{
			get
			{
				return this.RaidEvents.Count > 0;
			}
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x0600C096 RID: 49302 RVA: 0x0046DCDF File Offset: 0x0046BEDF
		public bool HasCharityEvents
		{
			get
			{
				return this.CharityEvents.Count > 0;
			}
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x0600C097 RID: 49303 RVA: 0x0046DCEF File Offset: 0x0046BEEF
		public bool HasChannelPointEvents
		{
			get
			{
				return this.ChannelPointEvents.Count > 0;
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x0600C098 RID: 49304 RVA: 0x0046DCFF File Offset: 0x0046BEFF
		public bool HasHypeTrainEvents
		{
			get
			{
				return this.HypeTrainEvents.Count > 0;
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x0600C099 RID: 49305 RVA: 0x0046DD0F File Offset: 0x0046BF0F
		public bool HasCreatorGoalEvents
		{
			get
			{
				return this.CreatorGoalEvents.Count > 0;
			}
		}

		// Token: 0x0600C09A RID: 49306 RVA: 0x0046DD1F File Offset: 0x0046BF1F
		public void AddBitEvent(TwitchEventEntry entry)
		{
			this.BitEvents.Add(entry);
		}

		// Token: 0x0600C09B RID: 49307 RVA: 0x0046DD2D File Offset: 0x0046BF2D
		public void AddSubEvent(TwitchSubEventEntry entry)
		{
			this.SubEvents.Add(entry);
		}

		// Token: 0x0600C09C RID: 49308 RVA: 0x0046DD3B File Offset: 0x0046BF3B
		public void AddGiftSubEvent(TwitchSubEventEntry entry)
		{
			this.GiftSubEvents.Add(entry);
		}

		// Token: 0x0600C09D RID: 49309 RVA: 0x0046DD49 File Offset: 0x0046BF49
		public void AddRaidEvent(TwitchEventEntry entry)
		{
			this.RaidEvents.Add(entry);
		}

		// Token: 0x0600C09E RID: 49310 RVA: 0x0046DD57 File Offset: 0x0046BF57
		public void AddCharityEvent(TwitchEventEntry entry)
		{
			this.CharityEvents.Add(entry);
		}

		// Token: 0x0600C09F RID: 49311 RVA: 0x0046DD65 File Offset: 0x0046BF65
		public void AddChannelPointEvent(TwitchChannelPointEventEntry entry)
		{
			this.ChannelPointEvents.Add(entry);
		}

		// Token: 0x0600C0A0 RID: 49312 RVA: 0x0046DD73 File Offset: 0x0046BF73
		public void AddHypeTrainEvent(TwitchHypeTrainEventEntry entry)
		{
			this.HypeTrainEvents.Add(entry);
		}

		// Token: 0x0600C0A1 RID: 49313 RVA: 0x0046DD81 File Offset: 0x0046BF81
		public void AddCreatorGoalEvent(TwitchCreatorGoalEventEntry entry)
		{
			this.CreatorGoalEvents.Add(entry);
		}

		// Token: 0x0600C0A2 RID: 49314 RVA: 0x0046DD90 File Offset: 0x0046BF90
		public TwitchSubEventEntry HandleSubEvent(int months, TwitchSubEventEntry.SubTierTypes tier)
		{
			for (int i = 0; i < this.SubEvents.Count; i++)
			{
				if (this.SubEvents[i].IsValid(months, "", tier))
				{
					return this.SubEvents[i];
				}
			}
			return null;
		}

		// Token: 0x0600C0A3 RID: 49315 RVA: 0x0046DDDC File Offset: 0x0046BFDC
		public TwitchSubEventEntry HandleGiftSubEvent(int giftCounts, TwitchSubEventEntry.SubTierTypes tier)
		{
			for (int i = 0; i < this.GiftSubEvents.Count; i++)
			{
				if (this.GiftSubEvents[i].IsValid(giftCounts, "", tier))
				{
					return this.GiftSubEvents[i];
				}
			}
			return null;
		}

		// Token: 0x0600C0A4 RID: 49316 RVA: 0x0046DE28 File Offset: 0x0046C028
		public TwitchEventEntry HandleBitRedeem(int bitAmount)
		{
			for (int i = 0; i < this.BitEvents.Count; i++)
			{
				if (this.BitEvents[i].IsValid(bitAmount, "", TwitchSubEventEntry.SubTierTypes.Any))
				{
					return this.BitEvents[i];
				}
			}
			return null;
		}

		// Token: 0x0600C0A5 RID: 49317 RVA: 0x0046DE74 File Offset: 0x0046C074
		public TwitchChannelPointEventEntry HandleChannelPointsRedeem(string title)
		{
			for (int i = 0; i < this.ChannelPointEvents.Count; i++)
			{
				if (this.ChannelPointEvents[i].ChannelPointTitle == title)
				{
					return this.ChannelPointEvents[i];
				}
			}
			return null;
		}

		// Token: 0x0600C0A6 RID: 49318 RVA: 0x0046DEC0 File Offset: 0x0046C0C0
		public TwitchEventEntry HandleRaid(int viewerAmount)
		{
			for (int i = 0; i < this.RaidEvents.Count; i++)
			{
				if (this.RaidEvents[i].IsValid(viewerAmount, "", TwitchSubEventEntry.SubTierTypes.Any))
				{
					return this.RaidEvents[i];
				}
			}
			return null;
		}

		// Token: 0x0600C0A7 RID: 49319 RVA: 0x0046DF0C File Offset: 0x0046C10C
		public TwitchEventEntry HandleCharityRedeem(int charityAmount)
		{
			for (int i = 0; i < this.CharityEvents.Count; i++)
			{
				if (this.CharityEvents[i].IsValid(charityAmount, "", TwitchSubEventEntry.SubTierTypes.Any))
				{
					return this.CharityEvents[i];
				}
			}
			return null;
		}

		// Token: 0x0600C0A8 RID: 49320 RVA: 0x0046DF58 File Offset: 0x0046C158
		public TwitchEventEntry HandleHypeTrainRedeem(int hypeTrainLevel)
		{
			for (int i = 0; i < this.HypeTrainEvents.Count; i++)
			{
				if (this.HypeTrainEvents[i].IsValid(hypeTrainLevel, "", TwitchSubEventEntry.SubTierTypes.Any))
				{
					return this.HypeTrainEvents[i];
				}
			}
			return null;
		}

		// Token: 0x0600C0A9 RID: 49321 RVA: 0x0046DFA4 File Offset: 0x0046C1A4
		public TwitchCreatorGoalEventEntry HandleCreatorGoalEvent(string goalType)
		{
			for (int i = 0; i < this.HypeTrainEvents.Count; i++)
			{
				if (this.CreatorGoalEvents[i].IsValid(-1, goalType, TwitchSubEventEntry.SubTierTypes.Any))
				{
					return this.CreatorGoalEvents[i];
				}
			}
			return null;
		}

		// Token: 0x0600C0AA RID: 49322 RVA: 0x0046DFEC File Offset: 0x0046C1EC
		public void AddChannelPointRedemptions()
		{
			if (TwitchManager.Current.Authentication != null)
			{
				string userID = TwitchManager.Current.Authentication.userID;
				for (int i = 0; i < this.ChannelPointEvents.Count; i++)
				{
					if (this.ChannelPointEvents[i].ChannelPointID == "" && this.ChannelPointEvents[i].AutoCreate)
					{
						GameManager.Instance.StartCoroutine(TwitchChannelPointEventEntry.CreateCustomRewardPost(this.ChannelPointEvents[i].SetupRewardEntry(userID), delegate(string res)
						{
							TwitchChannelPointEventEntry.CreateCustomRewardResponses createCustomRewardResponses = JsonConvert.DeserializeObject<TwitchChannelPointEventEntry.CreateCustomRewardResponses>(res);
							for (int j = 0; j < this.ChannelPointEvents.Count; j++)
							{
								if (this.ChannelPointEvents[j].ChannelPointTitle == createCustomRewardResponses.data[0].title)
								{
									this.ChannelPointEvents[j].ChannelPointID = createCustomRewardResponses.data[0].id;
									return;
								}
							}
						}, delegate(string err)
						{
							Log.Out(err);
						}));
					}
				}
				this.ChannelPointsSetup = true;
			}
		}

		// Token: 0x0600C0AB RID: 49323 RVA: 0x0046E0B8 File Offset: 0x0046C2B8
		public void RemoveChannelPointRedemptions(TwitchEventPreset newPreset = null)
		{
			for (int i = 0; i < this.ChannelPointEvents.Count; i++)
			{
				TwitchChannelPointEventEntry twitchChannelPointEventEntry = this.ChannelPointEvents[i];
				if (!(twitchChannelPointEventEntry.ChannelPointID == "") && this.ChannelPointEvents[i].AutoCreate && (newPreset == null || !newPreset.ChannelPointEvents.Contains(twitchChannelPointEventEntry)))
				{
					GameManager.Instance.StartCoroutine(TwitchChannelPointEventEntry.DeleteCustomRewardsDelete(this.ChannelPointEvents[i].ChannelPointID, delegate(string res)
					{
					}, delegate(string err)
					{
						Debug.LogWarning("Remove Channel Point Redeem Failed: " + err);
					}));
					this.ChannelPointEvents[i].ChannelPointID = "";
				}
			}
			this.ChannelPointsSetup = false;
		}

		// Token: 0x0400907D RID: 36989
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> BitEvents = new List<TwitchEventEntry>();

		// Token: 0x0400907E RID: 36990
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSubEventEntry> SubEvents = new List<TwitchSubEventEntry>();

		// Token: 0x0400907F RID: 36991
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSubEventEntry> GiftSubEvents = new List<TwitchSubEventEntry>();

		// Token: 0x04009080 RID: 36992
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> RaidEvents = new List<TwitchEventEntry>();

		// Token: 0x04009081 RID: 36993
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> CharityEvents = new List<TwitchEventEntry>();

		// Token: 0x04009082 RID: 36994
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchChannelPointEventEntry> ChannelPointEvents = new List<TwitchChannelPointEventEntry>();

		// Token: 0x04009083 RID: 36995
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchHypeTrainEventEntry> HypeTrainEvents = new List<TwitchHypeTrainEventEntry>();

		// Token: 0x04009084 RID: 36996
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchCreatorGoalEventEntry> CreatorGoalEvents = new List<TwitchCreatorGoalEventEntry>();

		// Token: 0x04009085 RID: 36997
		public string Name;

		// Token: 0x04009086 RID: 36998
		public bool IsDefault;

		// Token: 0x04009087 RID: 36999
		public bool IsEmpty;

		// Token: 0x04009088 RID: 37000
		public bool ChannelPointsSetup;

		// Token: 0x04009089 RID: 37001
		public string Title;

		// Token: 0x0400908A RID: 37002
		public string Description;
	}
}
