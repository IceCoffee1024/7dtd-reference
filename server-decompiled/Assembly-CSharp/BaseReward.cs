using System;

// Token: 0x020009F6 RID: 2550
public abstract class BaseReward
{
	// Token: 0x17000829 RID: 2089
	// (get) Token: 0x06004B91 RID: 19345 RVA: 0x001D7D86 File Offset: 0x001D5F86
	// (set) Token: 0x06004B92 RID: 19346 RVA: 0x001D7D8E File Offset: 0x001D5F8E
	public string ID { get; set; }

	// Token: 0x1700082A RID: 2090
	// (get) Token: 0x06004B93 RID: 19347 RVA: 0x001D7D97 File Offset: 0x001D5F97
	// (set) Token: 0x06004B94 RID: 19348 RVA: 0x001D7D9F File Offset: 0x001D5F9F
	public string Value { get; set; }

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x06004B95 RID: 19349 RVA: 0x001D7DA8 File Offset: 0x001D5FA8
	// (set) Token: 0x06004B96 RID: 19350 RVA: 0x001D7DB0 File Offset: 0x001D5FB0
	public Quest OwnerQuest { get; set; }

	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x06004B97 RID: 19351 RVA: 0x001D7DB9 File Offset: 0x001D5FB9
	// (set) Token: 0x06004B98 RID: 19352 RVA: 0x001D7DD6 File Offset: 0x001D5FD6
	public string Description
	{
		get
		{
			if (!this.displaySetup)
			{
				this.SetupReward();
				this.displaySetup = true;
			}
			return this.description;
		}
		set
		{
			this.description = value;
		}
	}

	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x06004B99 RID: 19353 RVA: 0x001D7DDF File Offset: 0x001D5FDF
	// (set) Token: 0x06004B9A RID: 19354 RVA: 0x001D7DFC File Offset: 0x001D5FFC
	public string ValueText
	{
		get
		{
			if (!this.displaySetup)
			{
				this.SetupReward();
				this.displaySetup = true;
			}
			return this.valueText;
		}
		set
		{
			this.valueText = value;
		}
	}

	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x06004B9B RID: 19355 RVA: 0x001D7E05 File Offset: 0x001D6005
	// (set) Token: 0x06004B9C RID: 19356 RVA: 0x001D7E22 File Offset: 0x001D6022
	public string Icon
	{
		get
		{
			if (!this.displaySetup)
			{
				this.SetupReward();
				this.displaySetup = true;
			}
			return this.icon;
		}
		set
		{
			this.icon = value;
		}
	}

	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x06004B9D RID: 19357 RVA: 0x001D7E2B File Offset: 0x001D602B
	// (set) Token: 0x06004B9E RID: 19358 RVA: 0x001D7E33 File Offset: 0x001D6033
	public string IconAtlas { get; set; }

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x06004B9F RID: 19359 RVA: 0x001D7E3C File Offset: 0x001D603C
	// (set) Token: 0x06004BA0 RID: 19360 RVA: 0x001D7E44 File Offset: 0x001D6044
	public bool HiddenReward { get; set; }

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x06004BA1 RID: 19361 RVA: 0x001D7E4D File Offset: 0x001D604D
	// (set) Token: 0x06004BA2 RID: 19362 RVA: 0x001D7E55 File Offset: 0x001D6055
	public bool Optional { get; set; }

	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x06004BA3 RID: 19363 RVA: 0x001D7E5E File Offset: 0x001D605E
	// (set) Token: 0x06004BA4 RID: 19364 RVA: 0x001D7E66 File Offset: 0x001D6066
	public bool isChosenReward { get; set; }

	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x06004BA5 RID: 19365 RVA: 0x001D7E6F File Offset: 0x001D606F
	// (set) Token: 0x06004BA6 RID: 19366 RVA: 0x001D7E77 File Offset: 0x001D6077
	public bool isChainReward { get; set; }

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x06004BA7 RID: 19367 RVA: 0x001D7E80 File Offset: 0x001D6080
	// (set) Token: 0x06004BA8 RID: 19368 RVA: 0x001D7E88 File Offset: 0x001D6088
	public bool isFixedLocation { get; set; }

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x06004BA9 RID: 19369 RVA: 0x001D7E91 File Offset: 0x001D6091
	// (set) Token: 0x06004BAA RID: 19370 RVA: 0x001D7E99 File Offset: 0x001D6099
	public BaseReward.ReceiveStages ReceiveStage { get; set; }

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x06004BAB RID: 19371 RVA: 0x001D7EA2 File Offset: 0x001D60A2
	// (set) Token: 0x06004BAC RID: 19372 RVA: 0x001D7EAA File Offset: 0x001D60AA
	public byte RewardIndex { get; set; }

	// Token: 0x06004BAD RID: 19373 RVA: 0x001D7EB4 File Offset: 0x001D60B4
	public BaseReward()
	{
		this.IconAtlas = "UIAtlas";
		this.ReceiveStage = BaseReward.ReceiveStages.QuestCompletion;
		this.isFixedLocation = false;
	}

	// Token: 0x06004BAE RID: 19374 RVA: 0x001D7F04 File Offset: 0x001D6104
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CopyValues(BaseReward reward)
	{
		reward.ID = this.ID;
		reward.Value = this.Value;
		reward.ReceiveStage = this.ReceiveStage;
		reward.HiddenReward = this.HiddenReward;
		reward.Optional = this.Optional;
		reward.isChosenReward = this.isChosenReward;
		reward.isChainReward = this.isChainReward;
		reward.isFixedLocation = this.isFixedLocation;
		reward.RewardIndex = this.RewardIndex;
	}

	// Token: 0x06004BAF RID: 19375 RVA: 0x001D7F7D File Offset: 0x001D617D
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x06004BB0 RID: 19376 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupReward()
	{
	}

	// Token: 0x06004BB1 RID: 19377 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GiveReward(EntityPlayer player)
	{
	}

	// Token: 0x06004BB2 RID: 19378 RVA: 0x001D7FAD File Offset: 0x001D61AD
	public void GiveReward()
	{
		this.GiveReward(this.OwnerQuest.OwnerJournal.OwnerPlayer);
	}

	// Token: 0x06004BB3 RID: 19379 RVA: 0x001D7FC5 File Offset: 0x001D61C5
	public virtual ItemStack GetRewardItem()
	{
		return ItemStack.Empty;
	}

	// Token: 0x06004BB4 RID: 19380 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseReward Clone()
	{
		return null;
	}

	// Token: 0x06004BB5 RID: 19381 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupGlobalRewardSettings()
	{
	}

	// Token: 0x06004BB6 RID: 19382 RVA: 0x001D7FCC File Offset: 0x001D61CC
	public virtual void Read(PooledBinaryReader _br)
	{
		this.RewardIndex = _br.ReadByte();
	}

	// Token: 0x06004BB7 RID: 19383 RVA: 0x001D7FDA File Offset: 0x001D61DA
	public virtual void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.RewardIndex);
	}

	// Token: 0x06004BB8 RID: 19384 RVA: 0x001D7FE8 File Offset: 0x001D61E8
	public virtual void ParseProperties(DynamicProperties properties)
	{
		if (properties.Values.ContainsKey(BaseReward.PropID))
		{
			this.ID = properties.Values[BaseReward.PropID];
		}
		if (properties.Values.ContainsKey(BaseReward.PropValue))
		{
			this.Value = properties.Values[BaseReward.PropValue];
		}
		if (properties.Values.ContainsKey(BaseReward.PropReceiveStage))
		{
			string text = properties.Values[BaseReward.PropReceiveStage];
			if (!(text == "start"))
			{
				if (!(text == "complete"))
				{
					if (text == "aftercomplete")
					{
						this.ReceiveStage = BaseReward.ReceiveStages.AfterCompleteNotification;
					}
				}
				else
				{
					this.ReceiveStage = BaseReward.ReceiveStages.QuestCompletion;
				}
			}
			else
			{
				this.ReceiveStage = BaseReward.ReceiveStages.QuestStart;
			}
		}
		if (properties.Values.ContainsKey(BaseReward.PropOptional))
		{
			bool flag;
			StringParsers.TryParseBool(properties.Values[BaseReward.PropOptional], out flag);
			this.Optional = flag;
		}
		if (properties.Values.ContainsKey(BaseReward.PropHidden))
		{
			bool flag2;
			StringParsers.TryParseBool(properties.Values[BaseReward.PropHidden], out flag2);
			this.HiddenReward = flag2;
		}
		if (properties.Values.ContainsKey(BaseReward.PropIsChosen))
		{
			bool flag3;
			StringParsers.TryParseBool(properties.Values[BaseReward.PropIsChosen], out flag3);
			this.isChosenReward = flag3;
		}
		if (properties.Values.ContainsKey(BaseReward.PropIsFixed))
		{
			bool flag4;
			StringParsers.TryParseBool(properties.Values[BaseReward.PropIsFixed], out flag4);
			this.isFixedLocation = flag4;
		}
		if (properties.Values.ContainsKey(BaseReward.PropIsChain))
		{
			bool flag5;
			StringParsers.TryParseBool(properties.Values[BaseReward.PropIsChain], out flag5);
			this.isChainReward = flag5;
		}
	}

	// Token: 0x06004BB9 RID: 19385 RVA: 0x0003223F File Offset: 0x0003043F
	public virtual string GetRewardText()
	{
		return "";
	}

	// Token: 0x04003BAD RID: 15277
	[PublicizedFrom(EAccessModifier.Private)]
	public bool displaySetup;

	// Token: 0x04003BAE RID: 15278
	[PublicizedFrom(EAccessModifier.Private)]
	public string description = "";

	// Token: 0x04003BAF RID: 15279
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueText = "";

	// Token: 0x04003BB0 RID: 15280
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon = "";

	// Token: 0x04003BBC RID: 15292
	public static string PropID = "id";

	// Token: 0x04003BBD RID: 15293
	public static string PropValue = "value";

	// Token: 0x04003BBE RID: 15294
	public static string PropOptional = "optional";

	// Token: 0x04003BBF RID: 15295
	public static string PropReceiveStage = "stage";

	// Token: 0x04003BC0 RID: 15296
	public static string PropHidden = "hidden";

	// Token: 0x04003BC1 RID: 15297
	public static string PropIsChosen = "ischosen";

	// Token: 0x04003BC2 RID: 15298
	public static string PropIsChain = "chainreward";

	// Token: 0x04003BC3 RID: 15299
	public static string PropIsFixed = "isfixed";

	// Token: 0x020009F7 RID: 2551
	public enum RewardTypes
	{
		// Token: 0x04003BC5 RID: 15301
		Exp,
		// Token: 0x04003BC6 RID: 15302
		Item,
		// Token: 0x04003BC7 RID: 15303
		Level,
		// Token: 0x04003BC8 RID: 15304
		Quest,
		// Token: 0x04003BC9 RID: 15305
		Recipe,
		// Token: 0x04003BCA RID: 15306
		ShowTip,
		// Token: 0x04003BCB RID: 15307
		Skill,
		// Token: 0x04003BCC RID: 15308
		SkillPoints
	}

	// Token: 0x020009F8 RID: 2552
	public enum ReceiveStages
	{
		// Token: 0x04003BCE RID: 15310
		QuestStart,
		// Token: 0x04003BCF RID: 15311
		QuestCompletion,
		// Token: 0x04003BD0 RID: 15312
		AfterCompleteNotification
	}
}
