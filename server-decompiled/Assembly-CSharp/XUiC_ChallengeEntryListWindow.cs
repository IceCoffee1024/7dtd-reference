using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000EE9 RID: 3817
[Preserve]
public class XUiC_ChallengeEntryListWindow : XUiController
{
	// Token: 0x06007742 RID: 30530 RVA: 0x002D01AC File Offset: 0x002CE3AC
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

	// Token: 0x06007743 RID: 30531 RVA: 0x002D0208 File Offset: 0x002CE408
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

	// Token: 0x06007744 RID: 30532 RVA: 0x002D0268 File Offset: 0x002CE468
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

	// Token: 0x06007745 RID: 30533 RVA: 0x002D02C8 File Offset: 0x002CE4C8
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

	// Token: 0x06007746 RID: 30534 RVA: 0x002D0376 File Offset: 0x002CE576
	public override void OnClose()
	{
		if (this.categoryList != null)
		{
			this.categoryList.CategoryChanged -= this.CategoryList_CategoryChanged;
		}
		base.OnClose();
	}

	// Token: 0x06007747 RID: 30535 RVA: 0x002D039D File Offset: 0x002CE59D
	[PublicizedFrom(EAccessModifier.Private)]
	public void CategoryList_CategoryChanged(XUiC_CategoryEntry _categoryEntry)
	{
		this.displayKey = ChallengeCategory.s_ChallengeCategories[_categoryEntry.CategoryName].DisplayKey;
		this.IsDirty = true;
		this.categoryChange = true;
	}

	// Token: 0x06007748 RID: 30536 RVA: 0x002D03C8 File Offset: 0x002CE5C8
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

	// Token: 0x04005841 RID: 22593
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005842 RID: 22594
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeGroupList challengeList;

	// Token: 0x04005843 RID: 22595
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtInput;

	// Token: 0x04005844 RID: 22596
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04005845 RID: 22597
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryChange;

	// Token: 0x04005846 RID: 22598
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeGroupList> challengeGroupList = new List<XUiC_ChallengeGroupList>();

	// Token: 0x04005847 RID: 22599
	[PublicizedFrom(EAccessModifier.Private)]
	public string displayKey = "";
}
