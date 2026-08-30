using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Twitch
{
	// Token: 0x02001812 RID: 6162
	public class TwitchEventPreset
	{
		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x0600BDB4 RID: 48564 RVA: 0x00463948 File Offset: 0x00461B48
		public bool HasCustomEvents
		{
			get
			{
				return this.BitEvents.Count > 0 || this.SubEvents.Count > 0 || this.GiftSubEvents.Count > 0 || this.RaidEvents.Count > 0 || this.CharityEvents.Count > 0 || this.ChannelPointEvents.Count > 0 || this.HypeTrainEvents.Count > 0 || this.CreatorGoalEvents.Count > 0;
			}
		}

		// Token: 0x17001703 RID: 5891
		// (get) Token: 0x0600BDB5 RID: 48565 RVA: 0x004639C7 File Offset: 0x00461BC7
		public bool HasBitEvents
		{
			get
			{
				return this.BitEvents.Count > 0;
			}
		}

		// Token: 0x17001704 RID: 5892
		// (get) Token: 0x0600BDB6 RID: 48566 RVA: 0x004639D7 File Offset: 0x00461BD7
		public bool HasSubEvents
		{
			get
			{
				return this.SubEvents.Count > 0;
			}
		}

		// Token: 0x17001705 RID: 5893
		// (get) Token: 0x0600BDB7 RID: 48567 RVA: 0x004639E7 File Offset: 0x00461BE7
		public bool HasGiftSubEvents
		{
			get
			{
				return this.GiftSubEvents.Count > 0;
			}
		}

		// Token: 0x17001706 RID: 5894
		// (get) Token: 0x0600BDB8 RID: 48568 RVA: 0x004639F7 File Offset: 0x00461BF7
		public bool HasRaidEvents
		{
			get
			{
				return this.RaidEvents.Count > 0;
			}
		}

		// Token: 0x17001707 RID: 5895
		// (get) Token: 0x0600BDB9 RID: 48569 RVA: 0x00463A07 File Offset: 0x00461C07
		public bool HasCharityEvents
		{
			get
			{
				return this.CharityEvents.Count > 0;
			}
		}

		// Token: 0x17001708 RID: 5896
		// (get) Token: 0x0600BDBA RID: 48570 RVA: 0x00463A17 File Offset: 0x00461C17
		public bool HasChannelPointEvents
		{
			get
			{
				return this.ChannelPointEvents.Count > 0;
			}
		}

		// Token: 0x17001709 RID: 5897
		// (get) Token: 0x0600BDBB RID: 48571 RVA: 0x00463A27 File Offset: 0x00461C27
		public bool HasHypeTrainEvents
		{
			get
			{
				return this.HypeTrainEvents.Count > 0;
			}
		}

		// Token: 0x1700170A RID: 5898
		// (get) Token: 0x0600BDBC RID: 48572 RVA: 0x00463A37 File Offset: 0x00461C37
		public bool HasCreatorGoalEvents
		{
			get
			{
				return this.CreatorGoalEvents.Count > 0;
			}
		}

		// Token: 0x0600BDBD RID: 48573 RVA: 0x00463A47 File Offset: 0x00461C47
		public void AddBitEvent(TwitchEventEntry entry)
		{
			this.BitEvents.Add(entry);
		}

		// Token: 0x0600BDBE RID: 48574 RVA: 0x00463A55 File Offset: 0x00461C55
		public void AddSubEvent(TwitchSubEventEntry entry)
		{
			this.SubEvents.Add(entry);
		}

		// Token: 0x0600BDBF RID: 48575 RVA: 0x00463A63 File Offset: 0x00461C63
		public void AddGiftSubEvent(TwitchSubEventEntry entry)
		{
			this.GiftSubEvents.Add(entry);
		}

		// Token: 0x0600BDC0 RID: 48576 RVA: 0x00463A71 File Offset: 0x00461C71
		public void AddRaidEvent(TwitchEventEntry entry)
		{
			this.RaidEvents.Add(entry);
		}

		// Token: 0x0600BDC1 RID: 48577 RVA: 0x00463A7F File Offset: 0x00461C7F
		public void AddCharityEvent(TwitchEventEntry entry)
		{
			this.CharityEvents.Add(entry);
		}

		// Token: 0x0600BDC2 RID: 48578 RVA: 0x00463A8D File Offset: 0x00461C8D
		public void AddChannelPointEvent(TwitchChannelPointEventEntry entry)
		{
			this.ChannelPointEvents.Add(entry);
		}

		// Token: 0x0600BDC3 RID: 48579 RVA: 0x00463A9B File Offset: 0x00461C9B
		public void AddHypeTrainEvent(TwitchHypeTrainEventEntry entry)
		{
			this.HypeTrainEvents.Add(entry);
		}

		// Token: 0x0600BDC4 RID: 48580 RVA: 0x00463AA9 File Offset: 0x00461CA9
		public void AddCreatorGoalEvent(TwitchCreatorGoalEventEntry entry)
		{
			this.CreatorGoalEvents.Add(entry);
		}

		// Token: 0x0600BDC5 RID: 48581 RVA: 0x00463AB8 File Offset: 0x00461CB8
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

		// Token: 0x0600BDC6 RID: 48582 RVA: 0x00463B04 File Offset: 0x00461D04
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

		// Token: 0x0600BDC7 RID: 48583 RVA: 0x00463B50 File Offset: 0x00461D50
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

		// Token: 0x0600BDC8 RID: 48584 RVA: 0x00463B9C File Offset: 0x00461D9C
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

		// Token: 0x0600BDC9 RID: 48585 RVA: 0x00463BE8 File Offset: 0x00461DE8
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

		// Token: 0x0600BDCA RID: 48586 RVA: 0x00463C34 File Offset: 0x00461E34
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

		// Token: 0x0600BDCB RID: 48587 RVA: 0x00463C80 File Offset: 0x00461E80
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

		// Token: 0x0600BDCC RID: 48588 RVA: 0x00463CCC File Offset: 0x00461ECC
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

		// Token: 0x0600BDCD RID: 48589 RVA: 0x00463D14 File Offset: 0x00461F14
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

		// Token: 0x0600BDCE RID: 48590 RVA: 0x00463DE0 File Offset: 0x00461FE0
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

		// Token: 0x04008E9B RID: 36507
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> BitEvents = new List<TwitchEventEntry>();

		// Token: 0x04008E9C RID: 36508
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSubEventEntry> SubEvents = new List<TwitchSubEventEntry>();

		// Token: 0x04008E9D RID: 36509
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchSubEventEntry> GiftSubEvents = new List<TwitchSubEventEntry>();

		// Token: 0x04008E9E RID: 36510
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> RaidEvents = new List<TwitchEventEntry>();

		// Token: 0x04008E9F RID: 36511
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchEventEntry> CharityEvents = new List<TwitchEventEntry>();

		// Token: 0x04008EA0 RID: 36512
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchChannelPointEventEntry> ChannelPointEvents = new List<TwitchChannelPointEventEntry>();

		// Token: 0x04008EA1 RID: 36513
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchHypeTrainEventEntry> HypeTrainEvents = new List<TwitchHypeTrainEventEntry>();

		// Token: 0x04008EA2 RID: 36514
		[PublicizedFrom(EAccessModifier.Private)]
		public List<TwitchCreatorGoalEventEntry> CreatorGoalEvents = new List<TwitchCreatorGoalEventEntry>();

		// Token: 0x04008EA3 RID: 36515
		public string Name;

		// Token: 0x04008EA4 RID: 36516
		public bool IsDefault;

		// Token: 0x04008EA5 RID: 36517
		public bool IsEmpty;

		// Token: 0x04008EA6 RID: 36518
		public bool ChannelPointsSetup;

		// Token: 0x04008EA7 RID: 36519
		public string Title;

		// Token: 0x04008EA8 RID: 36520
		public string Description;
	}
}
