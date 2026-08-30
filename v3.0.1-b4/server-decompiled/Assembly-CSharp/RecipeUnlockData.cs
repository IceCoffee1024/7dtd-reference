using System;
using Challenges;
using UnityEngine;

// Token: 0x020002D9 RID: 729
public class RecipeUnlockData
{
	// Token: 0x1700024F RID: 591
	// (get) Token: 0x0600151F RID: 5407 RVA: 0x0007F490 File Offset: 0x0007D690
	// (set) Token: 0x06001520 RID: 5408 RVA: 0x0007F4B0 File Offset: 0x0007D6B0
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

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06001521 RID: 5409 RVA: 0x0007F4B9 File Offset: 0x0007D6B9
	// (set) Token: 0x06001522 RID: 5410 RVA: 0x0007F4C1 File Offset: 0x0007D6C1
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

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06001523 RID: 5411 RVA: 0x0007F4D1 File Offset: 0x0007D6D1
	// (set) Token: 0x06001524 RID: 5412 RVA: 0x0007F4D9 File Offset: 0x0007D6D9
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

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06001525 RID: 5413 RVA: 0x0007F513 File Offset: 0x0007D713
	// (set) Token: 0x06001526 RID: 5414 RVA: 0x0007F51B File Offset: 0x0007D71B
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

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001527 RID: 5415 RVA: 0x0007F52B File Offset: 0x0007D72B
	// (set) Token: 0x06001528 RID: 5416 RVA: 0x0007F533 File Offset: 0x0007D733
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

	// Token: 0x06001529 RID: 5417 RVA: 0x0007F543 File Offset: 0x0007D743
	public RecipeUnlockData(string unlock)
	{
		this.unlockText = unlock;
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x0007F560 File Offset: 0x0007D760
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

	// Token: 0x0600152B RID: 5419 RVA: 0x0007F5F8 File Offset: 0x0007D7F8
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

	// Token: 0x0600152C RID: 5420 RVA: 0x0007F670 File Offset: 0x0007D870
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

	// Token: 0x0600152D RID: 5421 RVA: 0x0007F6E0 File Offset: 0x0007D8E0
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

	// Token: 0x0600152E RID: 5422 RVA: 0x0007F7ED File Offset: 0x0007D9ED
	public string GetIconAtlas()
	{
		return "UIAtlas";
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x0007F7F4 File Offset: 0x0007D9F4
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

	// Token: 0x04000E21 RID: 3617
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData.UnlockTypes unlockType;

	// Token: 0x04000E22 RID: 3618
	[PublicizedFrom(EAccessModifier.Private)]
	public string unlockText = "";

	// Token: 0x04000E23 RID: 3619
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass item;

	// Token: 0x04000E24 RID: 3620
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass perk;

	// Token: 0x04000E25 RID: 3621
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroup challengeGroup;

	// Token: 0x04000E26 RID: 3622
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challenge;

	// Token: 0x020002DA RID: 730
	public enum UnlockTypes
	{
		// Token: 0x04000E28 RID: 3624
		None,
		// Token: 0x04000E29 RID: 3625
		Perk,
		// Token: 0x04000E2A RID: 3626
		Book,
		// Token: 0x04000E2B RID: 3627
		Skill,
		// Token: 0x04000E2C RID: 3628
		Schematic,
		// Token: 0x04000E2D RID: 3629
		ChallengeGroup,
		// Token: 0x04000E2E RID: 3630
		Challenge,
		// Token: 0x04000E2F RID: 3631
		PrefabEditorInvalid
	}
}
