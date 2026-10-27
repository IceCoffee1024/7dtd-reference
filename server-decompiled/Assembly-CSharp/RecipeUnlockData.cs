using System;
using Challenges;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class RecipeUnlockData
{
	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06001533 RID: 5427 RVA: 0x0007FFD0 File Offset: 0x0007E1D0
	// (set) Token: 0x06001534 RID: 5428 RVA: 0x0007FFF0 File Offset: 0x0007E1F0
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
	// (get) Token: 0x06001535 RID: 5429 RVA: 0x0007FFF9 File Offset: 0x0007E1F9
	// (set) Token: 0x06001536 RID: 5430 RVA: 0x00080001 File Offset: 0x0007E201
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
	// (get) Token: 0x06001537 RID: 5431 RVA: 0x00080011 File Offset: 0x0007E211
	// (set) Token: 0x06001538 RID: 5432 RVA: 0x00080019 File Offset: 0x0007E219
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
	// (get) Token: 0x06001539 RID: 5433 RVA: 0x00080053 File Offset: 0x0007E253
	// (set) Token: 0x0600153A RID: 5434 RVA: 0x0008005B File Offset: 0x0007E25B
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
	// (get) Token: 0x0600153B RID: 5435 RVA: 0x0008006B File Offset: 0x0007E26B
	// (set) Token: 0x0600153C RID: 5436 RVA: 0x00080073 File Offset: 0x0007E273
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

	// Token: 0x0600153D RID: 5437 RVA: 0x00080083 File Offset: 0x0007E283
	public RecipeUnlockData(string unlock)
	{
		this.unlockText = unlock;
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x000800A0 File Offset: 0x0007E2A0
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

	// Token: 0x0600153F RID: 5439 RVA: 0x00080138 File Offset: 0x0007E338
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

	// Token: 0x06001540 RID: 5440 RVA: 0x000801B0 File Offset: 0x0007E3B0
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

	// Token: 0x06001541 RID: 5441 RVA: 0x00080220 File Offset: 0x0007E420
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

	// Token: 0x06001542 RID: 5442 RVA: 0x0008032D File Offset: 0x0007E52D
	public string GetIconAtlas()
	{
		return "UIAtlas";
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x00080334 File Offset: 0x0007E534
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

	// Token: 0x04000E2C RID: 3628
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData.UnlockTypes unlockType;

	// Token: 0x04000E2D RID: 3629
	[PublicizedFrom(EAccessModifier.Private)]
	public string unlockText = "";

	// Token: 0x04000E2E RID: 3630
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass item;

	// Token: 0x04000E2F RID: 3631
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass perk;

	// Token: 0x04000E30 RID: 3632
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroup challengeGroup;

	// Token: 0x04000E31 RID: 3633
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challenge;

	// Token: 0x020002E3 RID: 739
	public enum UnlockTypes
	{
		// Token: 0x04000E33 RID: 3635
		None,
		// Token: 0x04000E34 RID: 3636
		Perk,
		// Token: 0x04000E35 RID: 3637
		Book,
		// Token: 0x04000E36 RID: 3638
		Skill,
		// Token: 0x04000E37 RID: 3639
		Schematic,
		// Token: 0x04000E38 RID: 3640
		ChallengeGroup,
		// Token: 0x04000E39 RID: 3641
		Challenge,
		// Token: 0x04000E3A RID: 3642
		PrefabEditorInvalid
	}
}
