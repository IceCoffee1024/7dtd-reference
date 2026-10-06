using System;
using Challenges;
using UnityEngine;

// Token: 0x020002E0 RID: 736
public class RecipeUnlockData
{
	// Token: 0x17000248 RID: 584
	// (get) Token: 0x0600152F RID: 5423 RVA: 0x0007FE00 File Offset: 0x0007E000
	// (set) Token: 0x06001530 RID: 5424 RVA: 0x0007FE20 File Offset: 0x0007E020
	public RecipeUnlockData.UnlockTypes UnlockType
	{
		get
		{
			if (this.unlockText != "")
			{
				this.Init();
			}
			return this.unlockType;
		}
		set
		{
			this.unlockType = value;
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06001531 RID: 5425 RVA: 0x0007FE29 File Offset: 0x0007E029
	// (set) Token: 0x06001532 RID: 5426 RVA: 0x0007FE31 File Offset: 0x0007E031
	public ItemClass Item
	{
		get
		{
			return this.item;
		}
		set
		{
			this.item = value;
			this.unlockType = RecipeUnlockData.UnlockTypes.Schematic;
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06001533 RID: 5427 RVA: 0x0007FE41 File Offset: 0x0007E041
	// (set) Token: 0x06001534 RID: 5428 RVA: 0x0007FE49 File Offset: 0x0007E049
	public ProgressionClass Perk
	{
		get
		{
			return this.perk;
		}
		set
		{
			this.perk = value;
			if (this.perk.IsBook)
			{
				this.unlockType = RecipeUnlockData.UnlockTypes.Book;
				return;
			}
			if (this.perk.IsCrafting)
			{
				this.unlockType = RecipeUnlockData.UnlockTypes.Skill;
				return;
			}
			this.unlockType = RecipeUnlockData.UnlockTypes.Perk;
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06001535 RID: 5429 RVA: 0x0007FE83 File Offset: 0x0007E083
	// (set) Token: 0x06001536 RID: 5430 RVA: 0x0007FE8B File Offset: 0x0007E08B
	public ChallengeGroup ChallengeGroup
	{
		get
		{
			return this.challengeGroup;
		}
		set
		{
			this.challengeGroup = value;
			this.unlockType = RecipeUnlockData.UnlockTypes.ChallengeGroup;
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06001537 RID: 5431 RVA: 0x0007FE9B File Offset: 0x0007E09B
	// (set) Token: 0x06001538 RID: 5432 RVA: 0x0007FEA3 File Offset: 0x0007E0A3
	public ChallengeClass Challenge
	{
		get
		{
			return this.challenge;
		}
		set
		{
			this.challenge = value;
			this.unlockType = RecipeUnlockData.UnlockTypes.Challenge;
		}
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x0007FEB3 File Offset: 0x0007E0B3
	public RecipeUnlockData(string unlock)
	{
		this.unlockText = unlock;
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x0007FED0 File Offset: 0x0007E0D0
	public void Init()
	{
		if (Progression.ProgressionClasses.ContainsKey(this.unlockText))
		{
			this.Perk = Progression.ProgressionClasses[this.unlockText];
			return;
		}
		ChallengeGroup group;
		if ((group = ChallengeGroup.GetGroup(this.unlockText)) != null)
		{
			this.ChallengeGroup = group;
			return;
		}
		ChallengeClass challengeClass;
		if ((challengeClass = ChallengeClass.GetChallenge(this.unlockText)) != null)
		{
			this.Challenge = challengeClass;
			return;
		}
		ItemClass itemClass = ItemClass.GetItemClass(this.unlockText, true);
		if (itemClass != null)
		{
			this.Item = itemClass;
			return;
		}
		this.UnlockType = (GameManager.Instance.IsEditMode() ? RecipeUnlockData.UnlockTypes.PrefabEditorInvalid : RecipeUnlockData.UnlockTypes.None);
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x0007FF68 File Offset: 0x0007E168
	public string GetName()
	{
		if (this.unlockText != "")
		{
			this.Init();
		}
		if (this.unlockType == RecipeUnlockData.UnlockTypes.Schematic)
		{
			return this.item.GetLocalizedItemName();
		}
		if (this.unlockType == RecipeUnlockData.UnlockTypes.ChallengeGroup)
		{
			return this.ChallengeGroup.Title;
		}
		if (this.unlockType == RecipeUnlockData.UnlockTypes.Challenge)
		{
			return this.Challenge.Title;
		}
		return Localization.Get(this.perk.NameKey, false, null);
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x0007FFE0 File Offset: 0x0007E1E0
	public string GetIcon()
	{
		if (this.unlockText != "")
		{
			this.Init();
		}
		if (this.unlockType == RecipeUnlockData.UnlockTypes.Schematic)
		{
			return "ui_game_symbol_book";
		}
		if (this.unlockType == RecipeUnlockData.UnlockTypes.Perk)
		{
			return "ui_game_symbol_skills";
		}
		if (this.unlockType == RecipeUnlockData.UnlockTypes.Skill)
		{
			return "ui_game_symbol_hammer";
		}
		if (this.unlockType == RecipeUnlockData.UnlockTypes.ChallengeGroup || this.unlockType == RecipeUnlockData.UnlockTypes.Challenge)
		{
			return "ui_game_symbol_challenge";
		}
		return "ui_game_symbol_book";
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x00080050 File Offset: 0x0007E250
	public string GetLevel(EntityPlayerLocal player, string recipeName)
	{
		if (this.unlockText != "")
		{
			this.Init();
		}
		if (this.unlockType == RecipeUnlockData.UnlockTypes.Skill)
		{
			for (int i = 0; i < this.perk.DisplayDataList.Count; i++)
			{
				ProgressionClass.DisplayData displayData = this.perk.DisplayDataList[i];
				if (displayData.Enabled)
				{
					for (int j = 0; j < displayData.UnlockDataList.Count; j++)
					{
						ProgressionClass.DisplayData.UnlockData unlockData = displayData.UnlockDataList[j];
						if (unlockData.ItemName == recipeName || (unlockData.RecipeList != null && unlockData.RecipeList.ContainsCaseInsensitive(recipeName)))
						{
							int unlockTier = unlockData.UnlockTier;
							ProgressionValue progressionValue = player.Progression.GetProgressionValue(this.perk.Name);
							return string.Format("{0}/{1}", progressionValue.Level, displayData.QualityStarts[unlockTier].ToString());
						}
					}
				}
			}
			return "--";
		}
		return "--";
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x0008015D File Offset: 0x0007E35D
	public string GetIconAtlas()
	{
		return "UIAtlas";
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x00080164 File Offset: 0x0007E364
	public Color GetItemTint()
	{
		if (this.unlockText != "")
		{
			this.Init();
		}
		if (this.unlockType == RecipeUnlockData.UnlockTypes.Schematic && this.item != null)
		{
			return this.item.GetIconTint(null);
		}
		return Color.white;
	}

	// Token: 0x04000E2B RID: 3627
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData.UnlockTypes unlockType;

	// Token: 0x04000E2C RID: 3628
	[PublicizedFrom(EAccessModifier.Private)]
	public string unlockText = "";

	// Token: 0x04000E2D RID: 3629
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass item;

	// Token: 0x04000E2E RID: 3630
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass perk;

	// Token: 0x04000E2F RID: 3631
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroup challengeGroup;

	// Token: 0x04000E30 RID: 3632
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challenge;

	// Token: 0x020002E1 RID: 737
	public enum UnlockTypes
	{
		// Token: 0x04000E32 RID: 3634
		None,
		// Token: 0x04000E33 RID: 3635
		Perk,
		// Token: 0x04000E34 RID: 3636
		Book,
		// Token: 0x04000E35 RID: 3637
		Skill,
		// Token: 0x04000E36 RID: 3638
		Schematic,
		// Token: 0x04000E37 RID: 3639
		ChallengeGroup,
		// Token: 0x04000E38 RID: 3640
		Challenge,
		// Token: 0x04000E39 RID: 3641
		PrefabEditorInvalid
	}
}
