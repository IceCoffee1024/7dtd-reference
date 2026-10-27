using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001895 RID: 6293
	public class TwitchEventPreset
	{
		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x0600C1A5 RID: 49573 RVA: 0x004715CC File Offset: 0x0046F7CC
		public bool HasCustomEvents
		{
			get
			{
				return this.BitEvents.Count > 0 || this.SubEvents.Count > 0 || this.GiftSubEvents.Count > 0 || this.RaidEvents.Count > 0 || this.CharityEvents.Count > 0 || this.ChannelPointEvents.Count > 0 || this.HypeTrainEvents.Count > 0 || this.CreatorGoalEvents.Count > 0;
			}
		}

		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x0600C1A6 RID: 49574 RVA: 0x0047164B File Offset: 0x0046F84B
		public bool HasBitEvents
		{
			get
			{
				return this.BitEvents.Count > 0;
			}
		}

		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x0600C1A7 RID: 49575 RVA: 0x0047165B File Offset: 0x0046F85B
		public bool HasSubEvents
		{
			get
			{
				return this.SubEvents.Count > 0;
			}
		}

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x0600C1A8 RID: 49576 RVA: 0x0047166B File Offset: 0x0046F86B
		public bool HasGiftSubEvents
		{
			get
			{
				return this.GiftSubEvents.Count > 0;
			}
		}

		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x0600C1A9 RID: 49577 RVA: 0x0047167B File Offset: 0x0046F87B
		public bool HasRaidEvents
		{
			get
			{
				return this.RaidEvents.Count > 0;
			}
		}

		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x0600C1AA RID: 49578 RVA: 0x0047168B File Offset: 0x0046F88B
		public bool HasCharityEvents
		{
			get
			{
				return this.CharityEvents.Count > 0;
			}
		}

		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x0600C1AB RID: 49579 RVA: 0x0047169B File Offset: 0x0046F89B
		public bool HasChannelPointEvents
		{
			get
			{
				return this.ChannelPointEvents.Count > 0;
			}
		}

		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x0600C1AC RID: 49580 RVA: 0x004716AB File Offset: 0x0046F8AB
		public bool HasHypeTrainEvents
		{
			get
			{
				return this.HypeTrainEvents.Count > 0;
			}
		}

		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x0600C1AD RID: 49581 RVA: 0x004716BB File Offset: 0x0046F8BB
		public bool HasCreatorGoalEvents
		{
			get
			{
				return this.CreatorGoalEvents.Count > 0;
			}
		}

		// Token: 0x0600C1AE RID: 49582 RVA: 0x004716CB File Offset: 0x0046F8CB
		public void AddBitEvent(TwitchEventEntry entry)
		{
			this.BitEvents.Add(entry);
		}

		// Token: 0x0600C1AF RID: 49583 RVA: 0x004716D9 File Offset: 0x0046F8D9
		public void AddSubEvent(TwitchSubEventEntry entry)
		{
			this.SubEvents.Add(entry);
		}

		// Token: 0x0600C1B0 RID: 49584 RVA: 0x004716E7 File Offset: 0x0046F8E7
		public void AddGiftSubEvent(TwitchSubEventEntry entry)
		{
			this.GiftSubEvents.Add(entry);
		}

		// Token: 0x0600C1B1 RID: 49585 RVA: 0x004716F5 File Offset: 0x0046F8F5
		public void AddRaidEvent(TwitchEventEntry entry)
		{
			this.RaidEvents.Add(entry);
		}

		// Token: 0x0600C1B2 RID: 49586 RVA: 0x00471703 File Offset: 0x0046F903
		public void AddCharityEvent(TwitchEventEntry entry)
		{
			this.CharityEvents.Add(entry);
		}

		// Token: 0x0600C1B3 RID: 49587 RVA: 0x00471711 File Offset: 0x0046F911
		public void AddChannelPointEvent(TwitchChannelPointEventEntry entry)
		{
			this.ChannelPointEvents.Add(entry);
		}

		// Token: 0x0600C1B4 RID: 49588 RVA: 0x0047171F File Offset: 0x0046F91F
		public void AddHypeTrainEvent(TwitchHypeTrainEventEntry entry)
		{
			this.HypeTrainEvents.Add(entry);
		}

		// Token: 0x0600C1B5 RID: 49589 RVA: 0x0047172D File Offset: 0x0046F92D
		public void AddCreatorGoalEvent(TwitchCreatorGoalEventEntry entry)
		{
			this.CreatorGoalEvents.Add(entry);
		}

		// Token: 0x0600C1B6 RID: 49590 RVA: 0x0047173C File Offset: 0x0046F93C
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

		// Token: 0x0600C1B7 RID: 49591 RVA: 0x00471788 File Offset: 0x0046F988
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

		// Token: 0x0600C1B8 RID: 49592 RVA: 0x004717D4 File Offset: 0x0046F9D4
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

		// Token: 0x0600C1B9 RID: 49593 RVA: 0x00471820 File Offset: 0x0046FA20
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

		// Token: 0x0600C1BA RID: 49594 RVA: 0x0047186C File Offset: 0x0046FA6C
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

		// Token: 0x0600C1BB RID: 49595 RVA: 0x004718B8 File Offset: 0x0046FAB8
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

		// Token: 0x0600C1BC RID: 49596 RVA: 0x00471904 File Offset: 0x0046FB04
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

		// Token: 0x0600C1BD RID: 49597 RVA: 0x00471950 File Offset: 0x0046FB50
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

		// Token: 0x0600C1BE RID: 49598 RVA: 0x00471998 File Offset: 0x0046FB98
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

		// Token: 0x0600C1BF RID: 49599 RVA: 0x00471A64 File Offset: 0x0046FC64
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

		// Token: 0x04009166 RID: 37222
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> BitEvents = new List<TwitchEventEntry>();

		// Token: 0x04009167 RID: 37223
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSubEventEntry> SubEvents = new List<TwitchSubEventEntry>();

		// Token: 0x04009168 RID: 37224
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSubEventEntry> GiftSubEvents = new List<TwitchSubEventEntry>();

		// Token: 0x04009169 RID: 37225
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> RaidEvents = new List<TwitchEventEntry>();

		// Token: 0x0400916A RID: 37226
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> CharityEvents = new List<TwitchEventEntry>();

		// Token: 0x0400916B RID: 37227
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchChannelPointEventEntry> ChannelPointEvents = new List<TwitchChannelPointEventEntry>();

		// Token: 0x0400916C RID: 37228
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchHypeTrainEventEntry> HypeTrainEvents = new List<TwitchHypeTrainEventEntry>();

		// Token: 0x0400916D RID: 37229
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchCreatorGoalEventEntry> CreatorGoalEvents = new List<TwitchCreatorGoalEventEntry>();

		// Token: 0x0400916E RID: 37230
		public string Name;

		// Token: 0x0400916F RID: 37231
		public bool IsDefault;

		// Token: 0x04009170 RID: 37232
		public bool IsEmpty;

		// Token: 0x04009171 RID: 37233
		public bool ChannelPointsSetup;

		// Token: 0x04009172 RID: 37234
		public string Title;

		// Token: 0x04009173 RID: 37235
		public string Description;
	}
}
