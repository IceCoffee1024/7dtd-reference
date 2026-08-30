using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine.Scripting;

// Token: 0x02000EDC RID: 3804
[Preserve]
public class XUiC_ChallengeGroupList : XUiController
{
	// Token: 0x17000E09 RID: 3593
	// (get) Token: 0x0600775A RID: 30554 RVA: 0x002CD429 File Offset: 0x002CB629
	// (set) Token: 0x0600775B RID: 30555 RVA: 0x002CD431 File Offset: 0x002CB631
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

	// Token: 0x0600775C RID: 30556 RVA: 0x002CD450 File Offset: 0x002CB650
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

	// Token: 0x0600775D RID: 30557 RVA: 0x002CD4C2 File Offset: 0x002CB6C2
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

	// Token: 0x0600775E RID: 30558 RVA: 0x002CD4F4 File Offset: 0x002CB6F4
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

	// Token: 0x0600775F RID: 30559 RVA: 0x002CD5D8 File Offset: 0x002CB7D8
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

	// Token: 0x06007760 RID: 30560 RVA: 0x002CD66C File Offset: 0x002CB86C
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "display_key")
		{
			this.DisplayKey = value;
			return true;
		}
		return base.ParseAttribute(name, value);
	}

	// Token: 0x06007761 RID: 30561 RVA: 0x002CD68C File Offset: 0x002CB88C
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x04005817 RID: 22551
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005818 RID: 22552
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_ChallengeGroupEntry> entryList = new List<XUiC_ChallengeGroupEntry>();

	// Token: 0x04005819 RID: 22553
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ChallengeGroupEntry selectedGroup;

	// Token: 0x0400581A RID: 22554
	[PublicizedFrom(EAccessModifier.Private)]
	public bool categoryChange;

	// Token: 0x0400581B RID: 22555
	public string DisplayKey = "";

	// Token: 0x0400581C RID: 22556
	public XUiC_CategoryList CategoryList;

	// Token: 0x0400581D RID: 22557
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ChallengeGroupEntry> challengeGroupList;

	// Token: 0x0400581E RID: 22558
	public XUiC_ChallengeEntryListWindow ChallengeEntryListWindow;
}
