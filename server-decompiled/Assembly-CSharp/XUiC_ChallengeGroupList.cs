using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x02000EEB RID: 3819
[Preserve]
public class XUiC_ChallengeGroupList : XUiController
{
	// Token: 0x17000E26 RID: 3622
	// (get) Token: 0x06007755 RID: 30549 RVA: 0x002D0801 File Offset: 0x002CEA01
	// (set) Token: 0x06007756 RID: 30550 RVA: 0x002D0809 File Offset: 0x002CEA09
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

	// Token: 0x06007757 RID: 30551 RVA: 0x002D0828 File Offset: 0x002CEA28
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

	// Token: 0x06007758 RID: 30552 RVA: 0x002D089A File Offset: 0x002CEA9A
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

	// Token: 0x06007759 RID: 30553 RVA: 0x002D08CC File Offset: 0x002CEACC
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

	// Token: 0x0600775A RID: 30554 RVA: 0x002D09B0 File Offset: 0x002CEBB0
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

	// Token: 0x0600775B RID: 30555 RVA: 0x002D0A44 File Offset: 0x002CEC44
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "display_key")
		{
			this.DisplayKey = value;
			return true;
		}
		return base.ParseAttribute(name, value);
	}

	// Token: 0x0600775C RID: 30556 RVA: 0x002D0A64 File Offset: 0x002CEC64
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x0400584F RID: 22607
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005850 RID: 22608
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeGroupEntry> entryList = new List<XUiC_ChallengeGroupEntry>();

	// Token: 0x04005851 RID: 22609
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeGroupEntry selectedGroup;

	// Token: 0x04005852 RID: 22610
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryChange;

	// Token: 0x04005853 RID: 22611
	public string DisplayKey = "";

	// Token: 0x04005854 RID: 22612
	public XUiC_CategoryList CategoryList;

	// Token: 0x04005855 RID: 22613
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChallengeGroupEntry> challengeGroupList;

	// Token: 0x04005856 RID: 22614
	public XUiC_ChallengeEntryListWindow ChallengeEntryListWindow;
}
