using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001838 RID: 6200
	public class TwitchEventPreset
	{
		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x0600BF39 RID: 48953 RVA: 0x004696E4 File Offset: 0x004678E4
		public bool HasCustomEvents
		{
			get
			{
				return this.BitEvents.Count > 0 || this.SubEvents.Count > 0 || this.GiftSubEvents.Count > 0 || this.RaidEvents.Count > 0 || this.CharityEvents.Count > 0 || this.ChannelPointEvents.Count > 0 || this.HypeTrainEvents.Count > 0 || this.CreatorGoalEvents.Count > 0;
			}
		}

		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x0600BF3A RID: 48954 RVA: 0x00469763 File Offset: 0x00467963
		public bool HasBitEvents
		{
			get
			{
				return this.BitEvents.Count > 0;
			}
		}

		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x0600BF3B RID: 48955 RVA: 0x00469773 File Offset: 0x00467973
		public bool HasSubEvents
		{
			get
			{
				return this.SubEvents.Count > 0;
			}
		}

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x0600BF3C RID: 48956 RVA: 0x00469783 File Offset: 0x00467983
		public bool HasGiftSubEvents
		{
			get
			{
				return this.GiftSubEvents.Count > 0;
			}
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x0600BF3D RID: 48957 RVA: 0x00469793 File Offset: 0x00467993
		public bool HasRaidEvents
		{
			get
			{
				return this.RaidEvents.Count > 0;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x0600BF3E RID: 48958 RVA: 0x004697A3 File Offset: 0x004679A3
		public bool HasCharityEvents
		{
			get
			{
				return this.CharityEvents.Count > 0;
			}
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x0600BF3F RID: 48959 RVA: 0x004697B3 File Offset: 0x004679B3
		public bool HasChannelPointEvents
		{
			get
			{
				return this.ChannelPointEvents.Count > 0;
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x0600BF40 RID: 48960 RVA: 0x004697C3 File Offset: 0x004679C3
		public bool HasHypeTrainEvents
		{
			get
			{
				return this.HypeTrainEvents.Count > 0;
			}
		}

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x0600BF41 RID: 48961 RVA: 0x004697D3 File Offset: 0x004679D3
		public bool HasCreatorGoalEvents
		{
			get
			{
				return this.CreatorGoalEvents.Count > 0;
			}
		}

		// Token: 0x0600BF42 RID: 48962 RVA: 0x004697E3 File Offset: 0x004679E3
		public void AddBitEvent(TwitchEventEntry entry)
		{
			this.BitEvents.Add(entry);
		}

		// Token: 0x0600BF43 RID: 48963 RVA: 0x004697F1 File Offset: 0x004679F1
		public void AddSubEvent(TwitchSubEventEntry entry)
		{
			this.SubEvents.Add(entry);
		}

		// Token: 0x0600BF44 RID: 48964 RVA: 0x004697FF File Offset: 0x004679FF
		public void AddGiftSubEvent(TwitchSubEventEntry entry)
		{
			this.GiftSubEvents.Add(entry);
		}

		// Token: 0x0600BF45 RID: 48965 RVA: 0x0046980D File Offset: 0x00467A0D
		public void AddRaidEvent(TwitchEventEntry entry)
		{
			this.RaidEvents.Add(entry);
		}

		// Token: 0x0600BF46 RID: 48966 RVA: 0x0046981B File Offset: 0x00467A1B
		public void AddCharityEvent(TwitchEventEntry entry)
		{
			this.CharityEvents.Add(entry);
		}

		// Token: 0x0600BF47 RID: 48967 RVA: 0x00469829 File Offset: 0x00467A29
		public void AddChannelPointEvent(TwitchChannelPointEventEntry entry)
		{
			this.ChannelPointEvents.Add(entry);
		}

		// Token: 0x0600BF48 RID: 48968 RVA: 0x00469837 File Offset: 0x00467A37
		public void AddHypeTrainEvent(TwitchHypeTrainEventEntry entry)
		{
			this.HypeTrainEvents.Add(entry);
		}

		// Token: 0x0600BF49 RID: 48969 RVA: 0x00469845 File Offset: 0x00467A45
		public void AddCreatorGoalEvent(TwitchCreatorGoalEventEntry entry)
		{
			this.CreatorGoalEvents.Add(entry);
		}

		// Token: 0x0600BF4A RID: 48970 RVA: 0x00469854 File Offset: 0x00467A54
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

		// Token: 0x0600BF4B RID: 48971 RVA: 0x004698A0 File Offset: 0x00467AA0
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

		// Token: 0x0600BF4C RID: 48972 RVA: 0x004698EC File Offset: 0x00467AEC
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

		// Token: 0x0600BF4D RID: 48973 RVA: 0x00469938 File Offset: 0x00467B38
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

		// Token: 0x0600BF4E RID: 48974 RVA: 0x00469984 File Offset: 0x00467B84
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

		// Token: 0x0600BF4F RID: 48975 RVA: 0x004699D0 File Offset: 0x00467BD0
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

		// Token: 0x0600BF50 RID: 48976 RVA: 0x00469A1C File Offset: 0x00467C1C
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

		// Token: 0x0600BF51 RID: 48977 RVA: 0x00469A68 File Offset: 0x00467C68
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

		// Token: 0x0600BF52 RID: 48978 RVA: 0x00469AB0 File Offset: 0x00467CB0
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

		// Token: 0x0600BF53 RID: 48979 RVA: 0x00469B7C File Offset: 0x00467D7C
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

		// Token: 0x04008FB7 RID: 36791
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> BitEvents = new List<TwitchEventEntry>();

		// Token: 0x04008FB8 RID: 36792
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSubEventEntry> SubEvents = new List<TwitchSubEventEntry>();

		// Token: 0x04008FB9 RID: 36793
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSubEventEntry> GiftSubEvents = new List<TwitchSubEventEntry>();

		// Token: 0x04008FBA RID: 36794
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> RaidEvents = new List<TwitchEventEntry>();

		// Token: 0x04008FBB RID: 36795
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> CharityEvents = new List<TwitchEventEntry>();

		// Token: 0x04008FBC RID: 36796
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchChannelPointEventEntry> ChannelPointEvents = new List<TwitchChannelPointEventEntry>();

		// Token: 0x04008FBD RID: 36797
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchHypeTrainEventEntry> HypeTrainEvents = new List<TwitchHypeTrainEventEntry>();

		// Token: 0x04008FBE RID: 36798
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchCreatorGoalEventEntry> CreatorGoalEvents = new List<TwitchCreatorGoalEventEntry>();

		// Token: 0x04008FBF RID: 36799
		public string Name;

		// Token: 0x04008FC0 RID: 36800
		public bool IsDefault;

		// Token: 0x04008FC1 RID: 36801
		public bool IsEmpty;

		// Token: 0x04008FC2 RID: 36802
		public bool ChannelPointsSetup;

		// Token: 0x04008FC3 RID: 36803
		public string Title;

		// Token: 0x04008FC4 RID: 36804
		public string Description;
	}
}
