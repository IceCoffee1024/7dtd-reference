using System;
using System.IO;

// Token: 0x020009DB RID: 2523
public abstract class BaseReward
{
	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x06004B3C RID: 19260 RVA: 0x001D42FA File Offset: 0x001D24FA
	// (set) Token: 0x06004B3D RID: 19261 RVA: 0x001D4302 File Offset: 0x001D2502
	public string ID { get; set; }

	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x06004B3E RID: 19262 RVA: 0x001D430B File Offset: 0x001D250B
	// (set) Token: 0x06004B3F RID: 19263 RVA: 0x001D4313 File Offset: 0x001D2513
	public string Value { get; set; }

	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x06004B40 RID: 19264 RVA: 0x001D431C File Offset: 0x001D251C
	// (set) Token: 0x06004B41 RID: 19265 RVA: 0x001D4324 File Offset: 0x001D2524
	public Quest OwnerQuest { get; set; }

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x06004B42 RID: 19266 RVA: 0x001D432D File Offset: 0x001D252D
	// (set) Token: 0x06004B43 RID: 19267 RVA: 0x001D434A File Offset: 0x001D254A
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

	// Token: 0x170007FB RID: 2043
	// (get) Token: 0x06004B44 RID: 19268 RVA: 0x001D4353 File Offset: 0x001D2553
	// (set) Token: 0x06004B45 RID: 19269 RVA: 0x001D4370 File Offset: 0x001D2570
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

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x06004B46 RID: 19270 RVA: 0x001D4379 File Offset: 0x001D2579
	// (set) Token: 0x06004B47 RID: 19271 RVA: 0x001D4396 File Offset: 0x001D2596
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

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x06004B48 RID: 19272 RVA: 0x001D439F File Offset: 0x001D259F
	// (set) Token: 0x06004B49 RID: 19273 RVA: 0x001D43A7 File Offset: 0x001D25A7
	public string IconAtlas { get; set; }

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x06004B4A RID: 19274 RVA: 0x001D43B0 File Offset: 0x001D25B0
	// (set) Token: 0x06004B4B RID: 19275 RVA: 0x001D43B8 File Offset: 0x001D25B8
	public bool HiddenReward { get; set; }

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x06004B4C RID: 19276 RVA: 0x001D43C1 File Offset: 0x001D25C1
	// (set) Token: 0x06004B4D RID: 19277 RVA: 0x001D43C9 File Offset: 0x001D25C9
	public bool Optional { get; set; }

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x06004B4E RID: 19278 RVA: 0x001D43D2 File Offset: 0x001D25D2
	// (set) Token: 0x06004B4F RID: 19279 RVA: 0x001D43DA File Offset: 0x001D25DA
	public bool isChosenReward { get; set; }

	// Token: 0x17000801 RID: 2049
	// (get) Token: 0x06004B50 RID: 19280 RVA: 0x001D43E3 File Offset: 0x001D25E3
	// (set) Token: 0x06004B51 RID: 19281 RVA: 0x001D43EB File Offset: 0x001D25EB
	public bool isChainReward { get; set; }

	// Token: 0x17000802 RID: 2050
	// (get) Token: 0x06004B52 RID: 19282 RVA: 0x001D43F4 File Offset: 0x001D25F4
	// (set) Token: 0x06004B53 RID: 19283 RVA: 0x001D43FC File Offset: 0x001D25FC
	public bool isFixedLocation { get; set; }

	// Token: 0x17000803 RID: 2051
	// (get) Token: 0x06004B54 RID: 19284 RVA: 0x001D4405 File Offset: 0x001D2605
	// (set) Token: 0x06004B55 RID: 19285 RVA: 0x001D440D File Offset: 0x001D260D
	public BaseReward.ReceiveStages ReceiveStage { get; set; }

	// Token: 0x17000804 RID: 2052
	// (get) Token: 0x06004B56 RID: 19286 RVA: 0x001D4416 File Offset: 0x001D2616
	// (set) Token: 0x06004B57 RID: 19287 RVA: 0x001D441E File Offset: 0x001D261E
	public byte RewardIndex { get; set; }

	// Token: 0x06004B58 RID: 19288 RVA: 0x001D4428 File Offset: 0x001D2628
	public BaseReward()
	{
		this.IconAtlas = "UIAtlas";
		this.ReceiveStage = BaseReward.ReceiveStages.QuestCompletion;
		this.isFixedLocation = false;
	}

	// Token: 0x06004B59 RID: 19289 RVA: 0x001D4478 File Offset: 0x001D2678
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

	// Token: 0x06004B5A RID: 19290 RVA: 0x001D44F1 File Offset: 0x001D26F1
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x06004B5B RID: 19291 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupReward()
	{
	}

	// Token: 0x06004B5C RID: 19292 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GiveReward(EntityPlayer player)
	{
	}

	// Token: 0x06004B5D RID: 19293 RVA: 0x001D4521 File Offset: 0x001D2721
	public void GiveReward()
	{
		this.GiveReward(this.OwnerQuest.OwnerJournal.OwnerPlayer);
	}

	// Token: 0x06004B5E RID: 19294 RVA: 0x001D4539 File Offset: 0x001D2739
	public virtual ItemStack GetRewardItem()
	{
		return ItemStack.Empty;
	}

	// Token: 0x06004B5F RID: 19295 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual BaseReward Clone()
	{
		return null;
	}

	// Token: 0x06004B60 RID: 19296 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupGlobalRewardSettings()
	{
	}

	// Token: 0x06004B61 RID: 19297 RVA: 0x001D4540 File Offset: 0x001D2740
	public virtual void Read(BinaryReader _br)
	{
		this.RewardIndex = _br.ReadByte();
	}

	// Token: 0x06004B62 RID: 19298 RVA: 0x001D454E File Offset: 0x001D274E
	public virtual void Write(BinaryWriter _bw)
	{
		_bw.Write(this.RewardIndex);
	}

	// Token: 0x06004B63 RID: 19299 RVA: 0x001D455C File Offset: 0x001D275C
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

	// Token: 0x06004B64 RID: 19300 RVA: 0x00031E8B File Offset: 0x0003008B
	public virtual string GetRewardText()
	{
		return "";
	}

	// Token: 0x04003B0F RID: 15119
	[PublicizedFrom(EAccessModifier.Private)]
	public bool displaySetup;

	// Token: 0x04003B10 RID: 15120
	[PublicizedFrom(EAccessModifier.Private)]
	public string description = "";

	// Token: 0x04003B11 RID: 15121
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueText = "";

	// Token: 0x04003B12 RID: 15122
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon = "";

	// Token: 0x04003B1E RID: 15134
	public static string PropID = "id";

	// Token: 0x04003B1F RID: 15135
	public static string PropValue = "value";

	// Token: 0x04003B20 RID: 15136
	public static string PropOptional = "optional";

	// Token: 0x04003B21 RID: 15137
	public static string PropReceiveStage = "stage";

	// Token: 0x04003B22 RID: 15138
	public static string PropHidden = "hidden";

	// Token: 0x04003B23 RID: 15139
	public static string PropIsChosen = "ischosen";

	// Token: 0x04003B24 RID: 15140
	public static string PropIsChain = "chainreward";

	// Token: 0x04003B25 RID: 15141
	public static string PropIsFixed = "isfixed";

	// Token: 0x020009DC RID: 2524
	public enum RewardTypes
	{
		// Token: 0x04003B27 RID: 15143
		Exp,
		// Token: 0x04003B28 RID: 15144
		Item,
		// Token: 0x04003B29 RID: 15145
		Level,
		// Token: 0x04003B2A RID: 15146
		Quest,
		// Token: 0x04003B2B RID: 15147
		Recipe,
		// Token: 0x04003B2C RID: 15148
		ShowTip,
		// Token: 0x04003B2D RID: 15149
		Skill,
		// Token: 0x04003B2E RID: 15150
		SkillPoints
	}

	// Token: 0x020009DD RID: 2525
	public enum ReceiveStages
	{
		// Token: 0x04003B30 RID: 15152
		QuestStart,
		// Token: 0x04003B31 RID: 15153
		QuestCompletion,
		// Token: 0x04003B32 RID: 15154
		AfterCompleteNotification
	}
}
