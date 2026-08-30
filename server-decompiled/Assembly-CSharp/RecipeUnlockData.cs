using System;
using Challenges;
using UnityEngine;

// Token: 0x020002DD RID: 733
public class RecipeUnlockData
{
	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06001532 RID: 5426 RVA: 0x0007FB4C File Offset: 0x0007DD4C
	// (set) Token: 0x06001533 RID: 5427 RVA: 0x0007FB6C File Offset: 0x0007DD6C
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

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06001534 RID: 5428 RVA: 0x0007FB75 File Offset: 0x0007DD75
	// (set) Token: 0x06001535 RID: 5429 RVA: 0x0007FB7D File Offset: 0x0007DD7D
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

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06001536 RID: 5430 RVA: 0x0007FB8D File Offset: 0x0007DD8D
	// (set) Token: 0x06001537 RID: 5431 RVA: 0x0007FB95 File Offset: 0x0007DD95
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

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001538 RID: 5432 RVA: 0x0007FBCF File Offset: 0x0007DDCF
	// (set) Token: 0x06001539 RID: 5433 RVA: 0x0007FBD7 File Offset: 0x0007DDD7
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

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x0600153A RID: 5434 RVA: 0x0007FBE7 File Offset: 0x0007DDE7
	// (set) Token: 0x0600153B RID: 5435 RVA: 0x0007FBEF File Offset: 0x0007DDEF
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

	// Token: 0x0600153C RID: 5436 RVA: 0x0007FBFF File Offset: 0x0007DDFF
	public RecipeUnlockData(string unlock)
	{
		this.unlockText = unlock;
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x0007FC1C File Offset: 0x0007DE1C
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

	// Token: 0x0600153E RID: 5438 RVA: 0x0007FCB4 File Offset: 0x0007DEB4
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

	// Token: 0x0600153F RID: 5439 RVA: 0x0007FD2C File Offset: 0x0007DF2C
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

	// Token: 0x06001540 RID: 5440 RVA: 0x0007FD9C File Offset: 0x0007DF9C
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

	// Token: 0x06001541 RID: 5441 RVA: 0x0007FEA9 File Offset: 0x0007E0A9
	public string GetIconAtlas()
	{
		return "UIAtlas";
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x0007FEB0 File Offset: 0x0007E0B0
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

	// Token: 0x04000E32 RID: 3634
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeUnlockData.UnlockTypes unlockType;

	// Token: 0x04000E33 RID: 3635
	[PublicizedFrom(EAccessModifier.Private)]
	public string unlockText = "";

	// Token: 0x04000E34 RID: 3636
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass item;

	// Token: 0x04000E35 RID: 3637
	[PublicizedFrom(EAccessModifier.Private)]
	public ProgressionClass perk;

	// Token: 0x04000E36 RID: 3638
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroup challengeGroup;

	// Token: 0x04000E37 RID: 3639
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeClass challenge;

	// Token: 0x020002DE RID: 734
	public enum UnlockTypes
	{
		// Token: 0x04000E39 RID: 3641
		None,
		// Token: 0x04000E3A RID: 3642
		Perk,
		// Token: 0x04000E3B RID: 3643
		Book,
		// Token: 0x04000E3C RID: 3644
		Skill,
		// Token: 0x04000E3D RID: 3645
		Schematic,
		// Token: 0x04000E3E RID: 3646
		ChallengeGroup,
		// Token: 0x04000E3F RID: 3647
		Challenge,
		// Token: 0x04000E40 RID: 3648
		PrefabEditorInvalid
	}
}
