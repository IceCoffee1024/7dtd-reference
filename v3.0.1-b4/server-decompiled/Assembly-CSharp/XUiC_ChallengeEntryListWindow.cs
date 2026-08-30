using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000EC5 RID: 3781
[Preserve]
public class XUiC_ChallengeEntryListWindow : XUiController
{
	// Token: 0x060076A6 RID: 30374 RVA: 0x002C98B0 File Offset: 0x002C7AB0
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

	// Token: 0x060076A7 RID: 30375 RVA: 0x002C990C File Offset: 0x002C7B0C
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

	// Token: 0x060076A8 RID: 30376 RVA: 0x002C996C File Offset: 0x002C7B6C
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

	// Token: 0x060076A9 RID: 30377 RVA: 0x002C99CC File Offset: 0x002C7BCC
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

	// Token: 0x060076AA RID: 30378 RVA: 0x002C9A7A File Offset: 0x002C7C7A
	public override void OnClose()
	{
		if (this.categoryList != null)
		{
			this.categoryList.CategoryChanged -= this.CategoryList_CategoryChanged;
		}
		base.OnClose();
	}

	// Token: 0x060076AB RID: 30379 RVA: 0x002C9AA1 File Offset: 0x002C7CA1
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.displayKey = ChallengeCategory.s_ChallengeCategories[_categoryEntry.CategoryName].DisplayKey;
		this.IsDirty = true;
		this.categoryChange = true;
	}

	// Token: 0x060076AC RID: 30380 RVA: 0x002C9ACC File Offset: 0x002C7CCC
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

	// Token: 0x04005764 RID: 22372
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005765 RID: 22373
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeGroupList challengeList;

	// Token: 0x04005766 RID: 22374
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005767 RID: 22375
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04005768 RID: 22376
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryChange;

	// Token: 0x04005769 RID: 22377
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeGroupList> challengeGroupList = new List<XUiC_ChallengeGroupList>();

	// Token: 0x0400576A RID: 22378
	[PublicizedFrom(EAccessModifier.Private)]
	public string displayKey = "";
}
