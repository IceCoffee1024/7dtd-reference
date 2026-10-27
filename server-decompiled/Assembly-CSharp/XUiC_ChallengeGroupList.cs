using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x02000EF6 RID: 3830
[Preserve]
public class XUiC_ChallengeGroupList : XUiController
{
	// Token: 0x17000E2A RID: 3626
	// (get) Token: 0x0600777B RID: 30587 RVA: 0x002D1D59 File Offset: 0x002CFF59
	// (set) Token: 0x0600777C RID: 30588 RVA: 0x002D1D61 File Offset: 0x002CFF61
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

	// Token: 0x0600777D RID: 30589 RVA: 0x002D1D80 File Offset: 0x002CFF80
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

	// Token: 0x0600777E RID: 30590 RVA: 0x002D1DF2 File Offset: 0x002CFFF2
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

	// Token: 0x0600777F RID: 30591 RVA: 0x002D1E24 File Offset: 0x002D0024
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

	// Token: 0x06007780 RID: 30592 RVA: 0x002D1F08 File Offset: 0x002D0108
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

	// Token: 0x06007781 RID: 30593 RVA: 0x002D1F9C File Offset: 0x002D019C
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "display_key")
		{
			this.DisplayKey = value;
			return true;
		}
		return base.ParseAttribute(name, value);
	}

	// Token: 0x06007782 RID: 30594 RVA: 0x002D1FBC File Offset: 0x002D01BC
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x04005874 RID: 22644
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005875 RID: 22645
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeGroupEntry> entryList = new List<XUiC_ChallengeGroupEntry>();

	// Token: 0x04005876 RID: 22646
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeGroupEntry selectedGroup;

	// Token: 0x04005877 RID: 22647
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryChange;

	// Token: 0x04005878 RID: 22648
	public string DisplayKey = "";

	// Token: 0x04005879 RID: 22649
	public XUiC_CategoryList CategoryList;

	// Token: 0x0400587A RID: 22650
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChallengeGroupEntry> challengeGroupList;

	// Token: 0x0400587B RID: 22651
	public XUiC_ChallengeEntryListWindow ChallengeEntryListWindow;
}
