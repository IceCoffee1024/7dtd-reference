using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x02000EC7 RID: 3783
[Preserve]
public class XUiC_ChallengeGroupList : XUiController
{
	// Token: 0x17000DEF RID: 3567
	// (get) Token: 0x060076B9 RID: 30393 RVA: 0x002C9F05 File Offset: 0x002C8105
	// (set) Token: 0x060076BA RID: 30394 RVA: 0x002C9F0D File Offset: 0x002C810D
	public XUiC_ChallengeGroupEntry SelectedGroup
	{
		get
		{
			return this.selectedGroup;
		}
		set
		{
			if (this.selectedGroup != null)
			{
				this.selectedGroup.UnSelect();
			}
			this.selectedGroup = value;
		}
	}

	// Token: 0x060076BB RID: 30395 RVA: 0x002C9F2C File Offset: 0x002C812C
	public override void Init()
	{
		base.Init();
		XUiC_ChallengeWindowGroup xuiC_ChallengeWindowGroup = (XUiC_ChallengeWindowGroup)base.WindowGroup.Controller;
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i] is XUiC_ChallengeGroupEntry)
			{
				XUiC_ChallengeGroupEntry xuiC_ChallengeGroupEntry = (XUiC_ChallengeGroupEntry)this.children[i];
				xuiC_ChallengeGroupEntry.Owner = this;
				this.entryList.Add(xuiC_ChallengeGroupEntry);
			}
		}
	}

	// Token: 0x060076BC RID: 30396 RVA: 0x002C9F9E File Offset: 0x002C819E
	public override void Update(float _dt)
	{
		if (this.CategoryList.CurrentCategory != null && this.IsDirty)
		{
			this.updateChallengeEntries();
			this.IsDirty = false;
			this.categoryChange = false;
		}
		base.Update(_dt);
	}

	// Token: 0x060076BD RID: 30397 RVA: 0x002C9FD0 File Offset: 0x002C81D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateChallengeEntries()
	{
		if (this.challengeGroupList == null)
		{
			return;
		}
		string categoryName = this.CategoryList.CurrentCategory.CategoryName;
		int num = 0;
		for (int i = 0; i < this.challengeGroupList.Count; i++)
		{
			XUiC_ChallengeGroupEntry xuiC_ChallengeGroupEntry = this.entryList[num];
			if (xuiC_ChallengeGroupEntry != null && this.challengeGroupList[i].ChallengeGroup.Category.EqualsCaseInsensitive(categoryName))
			{
				xuiC_ChallengeGroupEntry.Entry = this.challengeGroupList[i];
				xuiC_ChallengeGroupEntry.ViewComponent.SoundPlayOnClick = true;
				if (this.categoryChange)
				{
					XUiC_ChallengeEntryList.SelectedEntry = null;
				}
				num++;
			}
			if (num >= this.entryList.Count)
			{
				break;
			}
		}
		for (int j = num; j < this.entryList.Count; j++)
		{
			XUiC_ChallengeGroupEntry xuiC_ChallengeGroupEntry2 = this.entryList[j];
			xuiC_ChallengeGroupEntry2.Entry = null;
			xuiC_ChallengeGroupEntry2.ViewComponent.SoundPlayOnClick = false;
		}
	}

	// Token: 0x060076BE RID: 30398 RVA: 0x002CA0B4 File Offset: 0x002C82B4
	public void SetChallengeGroupEntryList(List<ChallengeGroupEntry> newChallengeGroupList, bool newCategoryChange)
	{
		this.challengeGroupList = newChallengeGroupList;
		if (this.CategoryList != null && this.CategoryList.CurrentCategory == null)
		{
			this.CategoryList.SetCategoryToFirst();
		}
		this.categoryChange = newCategoryChange;
		if (this.xui.QuestTracker.TrackedChallenge != null)
		{
			ChallengeGroup challengeGroup = this.xui.QuestTracker.TrackedChallenge.ChallengeGroup;
			int num = 0;
			while (num < this.challengeGroupList.Count && this.challengeGroupList[num].ChallengeGroup != challengeGroup)
			{
				num++;
			}
		}
		this.IsDirty = true;
	}

	// Token: 0x060076BF RID: 30399 RVA: 0x002CA148 File Offset: 0x002C8348
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "display_key")
		{
			this.DisplayKey = value;
			return true;
		}
		return base.ParseAttribute(name, value);
	}

	// Token: 0x060076C0 RID: 30400 RVA: 0x002CA168 File Offset: 0x002C8368
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x04005772 RID: 22386
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005773 RID: 22387
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeGroupEntry> entryList = new List<XUiC_ChallengeGroupEntry>();

	// Token: 0x04005774 RID: 22388
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeGroupEntry selectedGroup;

	// Token: 0x04005775 RID: 22389
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryChange;

	// Token: 0x04005776 RID: 22390
	public string DisplayKey = "";

	// Token: 0x04005777 RID: 22391
	public XUiC_CategoryList CategoryList;

	// Token: 0x04005778 RID: 22392
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChallengeGroupEntry> challengeGroupList;

	// Token: 0x04005779 RID: 22393
	public XUiC_ChallengeEntryListWindow ChallengeEntryListWindow;
}
