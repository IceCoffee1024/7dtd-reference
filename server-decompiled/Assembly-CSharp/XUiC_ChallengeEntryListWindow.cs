using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000EF4 RID: 3828
[Preserve]
public class XUiC_ChallengeEntryListWindow : XUiController
{
	// Token: 0x06007768 RID: 30568 RVA: 0x002D1704 File Offset: 0x002CF904
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

	// Token: 0x06007769 RID: 30569 RVA: 0x002D1760 File Offset: 0x002CF960
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

	// Token: 0x0600776A RID: 30570 RVA: 0x002D17C0 File Offset: 0x002CF9C0
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

	// Token: 0x0600776B RID: 30571 RVA: 0x002D1820 File Offset: 0x002CFA20
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

	// Token: 0x0600776C RID: 30572 RVA: 0x002D18CE File Offset: 0x002CFACE
	public override void OnClose()
	{
		if (this.categoryList != null)
		{
			this.categoryList.CategoryChanged -= this.CategoryList_CategoryChanged;
		}
		base.OnClose();
	}

	// Token: 0x0600776D RID: 30573 RVA: 0x002D18F5 File Offset: 0x002CFAF5
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.displayKey = ChallengeCategory.s_ChallengeCategories[_categoryEntry.CategoryName].DisplayKey;
		this.IsDirty = true;
		this.categoryChange = true;
	}

	// Token: 0x0600776E RID: 30574 RVA: 0x002D1920 File Offset: 0x002CFB20
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

	// Token: 0x04005866 RID: 22630
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005867 RID: 22631
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeGroupList challengeList;

	// Token: 0x04005868 RID: 22632
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005869 RID: 22633
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x0400586A RID: 22634
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryChange;

	// Token: 0x0400586B RID: 22635
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeGroupList> challengeGroupList = new List<XUiC_ChallengeGroupList>();

	// Token: 0x0400586C RID: 22636
	[PublicizedFrom(EAccessModifier.Private)]
	public string displayKey = "";
}
