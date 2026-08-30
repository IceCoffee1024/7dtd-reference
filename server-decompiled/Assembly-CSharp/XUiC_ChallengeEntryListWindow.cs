using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000EDA RID: 3802
[Preserve]
public class XUiC_ChallengeEntryListWindow : XUiController
{
	// Token: 0x06007747 RID: 30535 RVA: 0x002CCDD4 File Offset: 0x002CAFD4
	public override void Init()
	{
		base.Init();
		this.categoryList = base.GetChildByType<XUiC_CategoryList>();
		XUiC_ChallengeGroupList[] childrenByType = base.GetChildrenByType<XUiC_ChallengeGroupList>(null);
		if (childrenByType != null)
		{
			foreach (XUiC_ChallengeGroupList xuiC_ChallengeGroupList in childrenByType)
			{
				xuiC_ChallengeGroupList.ChallengeEntryListWindow = this;
				xuiC_ChallengeGroupList.CategoryList = this.categoryList;
				this.challengeGroupList.Add(xuiC_ChallengeGroupList);
			}
		}
	}

	// Token: 0x06007748 RID: 30536 RVA: 0x002CCE30 File Offset: 0x002CB030
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeGroupList GetListFromKey(string key)
	{
		XUiC_ChallengeGroupList xuiC_ChallengeGroupList = null;
		for (int i = 0; i < this.challengeGroupList.Count; i++)
		{
			XUiC_ChallengeGroupList xuiC_ChallengeGroupList2 = this.challengeGroupList[i];
			if (xuiC_ChallengeGroupList2.DisplayKey.EqualsCaseInsensitive(key))
			{
				xuiC_ChallengeGroupList = xuiC_ChallengeGroupList2;
				xuiC_ChallengeGroupList2.ViewComponent.IsVisible = true;
			}
			else
			{
				xuiC_ChallengeGroupList2.ViewComponent.IsVisible = false;
			}
		}
		return xuiC_ChallengeGroupList;
	}

	// Token: 0x06007749 RID: 30537 RVA: 0x002CCE90 File Offset: 0x002CB090
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			this.challengeList = this.GetListFromKey(this.displayKey);
			this.challengeList.SetChallengeGroupEntryList(this.player.challengeJournal.ChallengeGroups, this.categoryChange);
			this.IsDirty = false;
			this.categoryChange = false;
		}
		base.Update(_dt);
	}

	// Token: 0x0600774A RID: 30538 RVA: 0x002CCEF0 File Offset: 0x002CB0F0
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		if (this.categoryList != null)
		{
			this.categoryList.SetupCategoriesBasedOnChallengeCategories(ChallengeCategory.s_ChallengeCategories.Values.ToList<ChallengeCategory>());
		}
		if (this.categoryList != null)
		{
			this.categoryList.CategoryChanged += this.CategoryList_CategoryChanged;
		}
		if (this.categoryList != null && (this.categoryList.CurrentCategory == null || this.categoryList.CurrentCategory.SpriteName == ""))
		{
			this.categoryList.SetCategoryToFirst();
		}
		this.IsDirty = true;
	}

	// Token: 0x0600774B RID: 30539 RVA: 0x002CCF9E File Offset: 0x002CB19E
	public override void OnClose()
	{
		if (this.categoryList != null)
		{
			this.categoryList.CategoryChanged -= this.CategoryList_CategoryChanged;
		}
		base.OnClose();
	}

	// Token: 0x0600774C RID: 30540 RVA: 0x002CCFC5 File Offset: 0x002CB1C5
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.displayKey = ChallengeCategory.s_ChallengeCategories[_categoryEntry.CategoryName].DisplayKey;
		this.IsDirty = true;
		this.categoryChange = true;
	}

	// Token: 0x0600774D RID: 30541 RVA: 0x002CCFF0 File Offset: 0x002CB1F0
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetSelectedByUnlockData(RecipeUnlockData unlockData)
	{
		string text = "";
		RecipeUnlockData.UnlockTypes unlockType = unlockData.UnlockType;
		if (unlockType != RecipeUnlockData.UnlockTypes.ChallengeGroup)
		{
			if (unlockType == RecipeUnlockData.UnlockTypes.Challenge)
			{
				this.displayKey = ChallengeCategory.s_ChallengeCategories[unlockData.Challenge.ChallengeGroup.Category].DisplayKey;
				text = unlockData.Challenge.ChallengeGroup.Category;
			}
		}
		else
		{
			this.displayKey = ChallengeCategory.s_ChallengeCategories[unlockData.ChallengeGroup.Category].DisplayKey;
			text = unlockData.ChallengeGroup.Category;
		}
		if (this.categoryList != null)
		{
			this.categoryList.CategoryChanged -= this.CategoryList_CategoryChanged;
			this.categoryList.SetCategory(text);
			this.categoryList.CategoryChanged += this.CategoryList_CategoryChanged;
		}
		this.IsDirty = true;
		this.categoryChange = true;
	}

	// Token: 0x04005809 RID: 22537
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400580A RID: 22538
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeGroupList challengeList;

	// Token: 0x0400580B RID: 22539
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x0400580C RID: 22540
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x0400580D RID: 22541
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryChange;

	// Token: 0x0400580E RID: 22542
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeGroupList> challengeGroupList = new List<XUiC_ChallengeGroupList>();

	// Token: 0x0400580F RID: 22543
	[PublicizedFrom(EAccessModifier.Private)]
	public string displayKey = "";
}
