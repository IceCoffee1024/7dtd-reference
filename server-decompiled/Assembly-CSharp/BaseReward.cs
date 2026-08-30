using System;
using System.IO;

// Token: 0x020009EC RID: 2540
public abstract class BaseReward
{
	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x06004BC2 RID: 19394 RVA: 0x001D6AE2 File Offset: 0x001D4CE2
	// (set) Token: 0x06004BC3 RID: 19395 RVA: 0x001D6AEA File Offset: 0x001D4CEA
	public string ID { get; set; }

	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x06004BC4 RID: 19396 RVA: 0x001D6AF3 File Offset: 0x001D4CF3
	// (set) Token: 0x06004BC5 RID: 19397 RVA: 0x001D6AFB File Offset: 0x001D4CFB
	public string Value { get; set; }

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x06004BC6 RID: 19398 RVA: 0x001D6B04 File Offset: 0x001D4D04
	// (set) Token: 0x06004BC7 RID: 19399 RVA: 0x001D6B0C File Offset: 0x001D4D0C
	public Quest OwnerQuest { get; set; }

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x06004BC8 RID: 19400 RVA: 0x001D6B15 File Offset: 0x001D4D15
	// (set) Token: 0x06004BC9 RID: 19401 RVA: 0x001D6B32 File Offset: 0x001D4D32
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

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x06004BCA RID: 19402 RVA: 0x001D6B3B File Offset: 0x001D4D3B
	// (set) Token: 0x06004BCB RID: 19403 RVA: 0x001D6B58 File Offset: 0x001D4D58
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

	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x06004BCC RID: 19404 RVA: 0x001D6B61 File Offset: 0x001D4D61
	// (set) Token: 0x06004BCD RID: 19405 RVA: 0x001D6B7E File Offset: 0x001D4D7E
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

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x06004BCE RID: 19406 RVA: 0x001D6B87 File Offset: 0x001D4D87
	// (set) Token: 0x06004BCF RID: 19407 RVA: 0x001D6B8F File Offset: 0x001D4D8F
	public string IconAtlas { get; set; }

	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x06004BD0 RID: 19408 RVA: 0x001D6B98 File Offset: 0x001D4D98
	// (set) Token: 0x06004BD1 RID: 19409 RVA: 0x001D6BA0 File Offset: 0x001D4DA0
	public bool HiddenReward { get; set; }

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x06004BD2 RID: 19410 RVA: 0x001D6BA9 File Offset: 0x001D4DA9
	// (set) Token: 0x06004BD3 RID: 19411 RVA: 0x001D6BB1 File Offset: 0x001D4DB1
	public bool Optional { get; set; }

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x06004BD4 RID: 19412 RVA: 0x001D6BBA File Offset: 0x001D4DBA
	// (set) Token: 0x06004BD5 RID: 19413 RVA: 0x001D6BC2 File Offset: 0x001D4DC2
	public bool isChosenReward { get; set; }

	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x06004BD6 RID: 19414 RVA: 0x001D6BCB File Offset: 0x001D4DCB
	// (set) Token: 0x06004BD7 RID: 19415 RVA: 0x001D6BD3 File Offset: 0x001D4DD3
	public bool isChainReward { get; set; }

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x06004BD8 RID: 19416 RVA: 0x001D6BDC File Offset: 0x001D4DDC
	// (set) Token: 0x06004BD9 RID: 19417 RVA: 0x001D6BE4 File Offset: 0x001D4DE4
	public bool isFixedLocation { get; set; }

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x06004BDA RID: 19418 RVA: 0x001D6BED File Offset: 0x001D4DED
	// (set) Token: 0x06004BDB RID: 19419 RVA: 0x001D6BF5 File Offset: 0x001D4DF5
	public BaseReward.ReceiveStages ReceiveStage { get; set; }

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x06004BDC RID: 19420 RVA: 0x001D6BFE File Offset: 0x001D4DFE
	// (set) Token: 0x06004BDD RID: 19421 RVA: 0x001D6C06 File Offset: 0x001D4E06
	public byte RewardIndex { get; set; }

	// Token: 0x06004BDE RID: 19422 RVA: 0x001D6C10 File Offset: 0x001D4E10
	public BaseReward()
	{
		this.IconAtlas = "UIAtlas";
		this.ReceiveStage = BaseReward.ReceiveStages.QuestCompletion;
		this.isFixedLocation = false;
	}

	// Token: 0x06004BDF RID: 19423 RVA: 0x001D6C60 File Offset: 0x001D4E60
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

	// Token: 0x06004BE0 RID: 19424 RVA: 0x001D6CD9 File Offset: 0x001D4ED9
	public virtual void HandleVariables()
	{
		this.ID = this.OwnerQuest.ParseVariable(this.ID);
		this.Value = this.OwnerQuest.ParseVariable(this.Value);
	}

	// Token: 0x06004BE1 RID: 19425 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupReward()
	{
	}

	// Token: 0x06004BE2 RID: 19426 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GiveReward(EntityPlayer player)
	{
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x001D6D09 File Offset: 0x001D4F09
	public void GiveReward()
	{
		this.GiveReward(this.OwnerQuest.OwnerJournal.OwnerPlayer);
	}

	// Token: 0x06004BE4 RID: 19428 RVA: 0x001D6D21 File Offset: 0x001D4F21
	public virtual ItemStack GetRewardItem()
	{
		return ItemStack.Empty;
	}

	// Token: 0x06004BE5 RID: 19429 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public virtual BaseReward Clone()
	{
		return null;
	}

	// Token: 0x06004BE6 RID: 19430 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupGlobalRewardSettings()
	{
	}

	// Token: 0x06004BE7 RID: 19431 RVA: 0x001D6D28 File Offset: 0x001D4F28
	public virtual void Read(BinaryReader _br)
	{
		this.RewardIndex = _br.ReadByte();
	}

	// Token: 0x06004BE8 RID: 19432 RVA: 0x001D6D36 File Offset: 0x001D4F36
	public virtual void Write(BinaryWriter _bw)
	{
		_bw.Write(this.RewardIndex);
	}

	// Token: 0x06004BE9 RID: 19433 RVA: 0x001D6D44 File Offset: 0x001D4F44
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

	// Token: 0x06004BEA RID: 19434 RVA: 0x00032147 File Offset: 0x00030347
	public virtual string GetRewardText()
	{
		return "";
	}

	// Token: 0x04003BA0 RID: 15264
	[PublicizedFrom(EAccessModifier.Private)]
	public bool displaySetup;

	// Token: 0x04003BA1 RID: 15265
	[PublicizedFrom(EAccessModifier.Private)]
	public string description = "";

	// Token: 0x04003BA2 RID: 15266
	[PublicizedFrom(EAccessModifier.Private)]
	public string valueText = "";

	// Token: 0x04003BA3 RID: 15267
	[PublicizedFrom(EAccessModifier.Private)]
	public string icon = "";

	// Token: 0x04003BAF RID: 15279
	public static string PropID = "id";

	// Token: 0x04003BB0 RID: 15280
	public static string PropValue = "value";

	// Token: 0x04003BB1 RID: 15281
	public static string PropOptional = "optional";

	// Token: 0x04003BB2 RID: 15282
	public static string PropReceiveStage = "stage";

	// Token: 0x04003BB3 RID: 15283
	public static string PropHidden = "hidden";

	// Token: 0x04003BB4 RID: 15284
	public static string PropIsChosen = "ischosen";

	// Token: 0x04003BB5 RID: 15285
	public static string PropIsChain = "chainreward";

	// Token: 0x04003BB6 RID: 15286
	public static string PropIsFixed = "isfixed";

	// Token: 0x020009ED RID: 2541
	public enum RewardTypes
	{
		// Token: 0x04003BB8 RID: 15288
		Exp,
		// Token: 0x04003BB9 RID: 15289
		Item,
		// Token: 0x04003BBA RID: 15290
		Level,
		// Token: 0x04003BBB RID: 15291
		Quest,
		// Token: 0x04003BBC RID: 15292
		Recipe,
		// Token: 0x04003BBD RID: 15293
		ShowTip,
		// Token: 0x04003BBE RID: 15294
		Skill,
		// Token: 0x04003BBF RID: 15295
		SkillPoints
	}

	// Token: 0x020009EE RID: 2542
	public enum ReceiveStages
	{
		// Token: 0x04003BC1 RID: 15297
		QuestStart,
		// Token: 0x04003BC2 RID: 15298
		QuestCompletion,
		// Token: 0x04003BC3 RID: 15299
		AfterCompleteNotification
	}
}
