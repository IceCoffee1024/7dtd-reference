using System;

// Token: 0x02000A00 RID: 2560
public abstract class BaseReward
{
	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x06004BB5 RID: 19381 RVA: 0x001D8E6E File Offset: 0x001D706E
	// (set) Token: 0x06004BB6 RID: 19382 RVA: 0x001D8E76 File Offset: 0x001D7076
	public string ID { get; set; }

	// Token: 0x17000830 RID: 2096
	// (get) Token: 0x06004BB7 RID: 19383 RVA: 0x001D8E7F File Offset: 0x001D707F
	// (set) Token: 0x06004BB8 RID: 19384 RVA: 0x001D8E87 File Offset: 0x001D7087
	public string Value { get; set; }

	// Token: 0x17000831 RID: 2097
	// (get) Token: 0x06004BB9 RID: 19385 RVA: 0x001D8E90 File Offset: 0x001D7090
	// (set) Token: 0x06004BBA RID: 19386 RVA: 0x001D8E98 File Offset: 0x001D7098
	public Quest OwnerQuest { get; set; }

	// Token: 0x17000832 RID: 2098
	// (get) Token: 0x06004BBB RID: 19387 RVA: 0x001D8EA1 File Offset: 0x001D70A1
	// (set) Token: 0x06004BBC RID: 19388 RVA: 0x001D8EBE File Offset: 0x001D70BE
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

	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x06004BBD RID: 19389 RVA: 0x001D8EC7 File Offset: 0x001D70C7
	// (set) Token: 0x06004BBE RID: 19390 RVA: 0x001D8EE4 File Offset: 0x001D70E4
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

	// Token: 0x17000834 RID: 2100
	// (get) Token: 0x06004BBF RID: 19391 RVA: 0x001D8EED File Offset: 0x001D70ED
	// (set) Token: 0x06004BC0 RID: 19392 RVA: 0x001D8F0A File Offset: 0x001D710A
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

	// Token: 0x17000835 RID: 2101
	// (get) Token: 0x06004BC1 RID: 19393 RVA: 0x001D8F13 File Offset: 0x001D7113
	// (set) Token: 0x06004BC2 RID: 19394 RVA: 0x001D8F1B File Offset: 0x001D711B
	public string IconAtlas { get; set; }

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x06004BC3 RID: 19395 RVA: 0x001D8F24 File Offset: 0x001D7124
	// (set) Token: 0x06004BC4 RID: 19396 RVA: 0x001D8F2C File Offset: 0x001D712C
	public bool HiddenReward { get; set; }

	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x06004BC5 RID: 19397 RVA: 0x001D8F35 File Offset: 0x001D7135
	// (set) Token: 0x06004BC6 RID: 19398 RVA: 0x001D8F3D File Offset: 0x001D713D
	public bool Optional { get; set; }

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x06004BC7 RID: 19399 RVA: 0x001D8F46 File Offset: 0x001D7146
	// (set) Token: 0x06004BC8 RID: 19400 RVA: 0x001D8F4E File Offset: 0x001D714E
	public bool isChosenReward { get; set; }

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x06004BC9 RID: 19401 RVA: 0x001D8F57 File Offset: 0x001D7157
	// (set) Token: 0x06004BCA RID: 19402 RVA: 0x001D8F5F File Offset: 0x001D715F
	public bool isChainReward { get; set; }

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x06004BCB RID: 19403 RVA: 0x001D8F68 File Offset: 0x001D7168
	// (set) Token: 0x06004BCC RID: 19404 RVA: 0x001D8F70 File Offset: 0x001D7170
	public bool isFixedLocation { get; set; }

	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x06004BCD RID: 19405 RVA: 0x001D8F79 File Offset: 0x001D7179
	// (set) Token: 0x06004BCE RID: 19406 RVA: 0x001D8F81 File Offset: 0x001D7181
	public BaseReward.ReceiveStages ReceiveStage { get; set; }

	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x06004BCF RID: 19407 RVA: 0x001D8F8A File Offset: 0x001D718A
	// (set) Token: 0x06004BD0 RID: 19408 RVA: 0x001D8F92 File Offset: 0x001D7192
	public byte RewardIndex { get; set; }

	// Token: 0x06004BD1 RID: 19409 RVA: 0x001D8F9C File Offset: 0x001D719C
	public BaseReward()
	{
		this.IconAtlas = "UIAtlas";
		this.ReceiveStage = BaseReward.ReceiveStages.QuestCompletion;
		this.isFixedLocation = false;
	}

	// Token: 0x06004BD2 RID: 19410 RVA: 0x001D8FEC File Offset: 0x001D71EC
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

	// Token: 0x06004BD3 RID: 19411 RVA: 0x001D9065 File Offset: 0x001D7265
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x06004BD4 RID: 19412 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupReward()
	{
	}

	// Token: 0x06004BD5 RID: 19413 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GiveReward(EntityPlayer player)
	{
	}

	// Token: 0x06004BD6 RID: 19414 RVA: 0x001D9095 File Offset: 0x001D7295
	public void GiveReward()
	{
		this.GiveReward(this.OwnerQuest.OwnerJournal.OwnerPlayer);
	}

	// Token: 0x06004BD7 RID: 19415 RVA: 0x001D90AD File Offset: 0x001D72AD
	public virtual ItemStack GetRewardItem()
	{
		return ItemStack.Empty;
	}

	// Token: 0x06004BD8 RID: 19416 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseReward Clone()
	{
		return null;
	}

	// Token: 0x06004BD9 RID: 19417 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupGlobalRewardSettings()
	{
	}

	// Token: 0x06004BDA RID: 19418 RVA: 0x001D90B4 File Offset: 0x001D72B4
	public virtual void Read(PooledBinaryReader _br)
	{
		this.RewardIndex = _br.ReadByte();
	}

	// Token: 0x06004BDB RID: 19419 RVA: 0x001D90C2 File Offset: 0x001D72C2
	public virtual void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(this.RewardIndex);
	}

	// Token: 0x06004BDC RID: 19420 RVA: 0x001D90D0 File Offset: 0x001D72D0
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

	// Token: 0x06004BDD RID: 19421 RVA: 0x000321FF File Offset: 0x000303FF
	public virtual string GetRewardText()
	{
		return "";
	}

	// Token: 0x04003BCE RID: 15310
	[PublicizedFrom(EAccessModifier.Private)]
	public bool displaySetup;

	// Token: 0x04003BCF RID: 15311
	[PublicizedFrom(EAccessModifier.Private)]
	public string description = "";

	// Token: 0x04003BD0 RID: 15312
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueText = "";

	// Token: 0x04003BD1 RID: 15313
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon = "";

	// Token: 0x04003BDD RID: 15325
	public static string PropID = "id";

	// Token: 0x04003BDE RID: 15326
	public static string PropValue = "value";

	// Token: 0x04003BDF RID: 15327
	public static string PropOptional = "optional";

	// Token: 0x04003BE0 RID: 15328
	public static string PropReceiveStage = "stage";

	// Token: 0x04003BE1 RID: 15329
	public static string PropHidden = "hidden";

	// Token: 0x04003BE2 RID: 15330
	public static string PropIsChosen = "ischosen";

	// Token: 0x04003BE3 RID: 15331
	public static string PropIsChain = "chainreward";

	// Token: 0x04003BE4 RID: 15332
	public static string PropIsFixed = "isfixed";

	// Token: 0x02000A01 RID: 2561
	public enum RewardTypes
	{
		// Token: 0x04003BE6 RID: 15334
		Exp,
		// Token: 0x04003BE7 RID: 15335
		Item,
		// Token: 0x04003BE8 RID: 15336
		Level,
		// Token: 0x04003BE9 RID: 15337
		Quest,
		// Token: 0x04003BEA RID: 15338
		Recipe,
		// Token: 0x04003BEB RID: 15339
		ShowTip,
		// Token: 0x04003BEC RID: 15340
		Skill,
		// Token: 0x04003BED RID: 15341
		SkillPoints
	}

	// Token: 0x02000A02 RID: 2562
	public enum ReceiveStages
	{
		// Token: 0x04003BEF RID: 15343
		QuestStart,
		// Token: 0x04003BF0 RID: 15344
		QuestCompletion,
		// Token: 0x04003BF1 RID: 15345
		AfterCompleteNotification
	}
}
