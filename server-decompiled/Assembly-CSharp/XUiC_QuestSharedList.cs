using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200100F RID: 4111
[Preserve]
public class XUiC_QuestSharedList : XUiController
{
	// Token: 0x17000F36 RID: 3894
	// (get) Token: 0x0600811D RID: 33053 RVA: 0x00310EDF File Offset: 0x0030F0DF
	// (set) Token: 0x0600811E RID: 33054 RVA: 0x00310EE7 File Offset: 0x0030F0E7
	public int Page
	{
		get
		{
			return this.page;
		}
		set
		{
			if (this.page != value)
			{
				this.page = value;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging != null)
				{
					xuiC_Paging.SetPage(this.page);
				}
				this.IsDirty = true;
			}
		}
	}

	// Token: 0x17000F37 RID: 3895
	// (get) Token: 0x0600811F RID: 33055 RVA: 0x00310F17 File Offset: 0x0030F117
	// (set) Token: 0x06008120 RID: 33056 RVA: 0x00310F1F File Offset: 0x0030F11F
	public XUiC_QuestEntry SelectedEntry
	{
		get
		{
			return this.selectedEntry;
		}
		set
		{
			if (this.selectedEntry != null)
			{
				this.selectedEntry.Selected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry != null)
			{
				this.selectedEntry.Selected = true;
				this.QuestList.SelectedEntry = null;
			}
		}
	}

	// Token: 0x06008121 RID: 33057 RVA: 0x00310F5C File Offset: 0x0030F15C
	public override void Init()
	{
		base.Init();
		XUiC_QuestWindowGroup xuiC_QuestWindowGroup = (XUiC_QuestWindowGroup)base.WindowGroup.Controller;
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i] is XUiC_QuestEntry)
			{
				XUiC_QuestEntry xuiC_QuestEntry = (XUiC_QuestEntry)this.children[i];
				xuiC_QuestEntry.QuestUIHandler = xuiC_QuestWindowGroup;
				xuiC_QuestEntry.OnScroll += this.OnScrollQuest;
				this.entryList.Add(xuiC_QuestEntry);
			}
		}
		this.pager = base.Parent.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += delegate
			{
				this.Page = this.pager.CurrentPageNumber;
			};
		}
	}

	// Token: 0x06008122 RID: 33058 RVA: 0x00311010 File Offset: 0x0030F210
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			XUiC_QuestEntry xuiC_QuestEntry = this.selectedEntry;
			Quest quest = ((xuiC_QuestEntry != null) ? xuiC_QuestEntry.Quest : null);
			for (int i = 0; i < this.entryList.Count; i++)
			{
				int num = i + this.entryList.Count * this.page;
				XUiC_QuestEntry xuiC_QuestEntry2 = this.entryList[i];
				if (xuiC_QuestEntry2 != null)
				{
					xuiC_QuestEntry2.OnPress -= this.OnPressQuest;
					if (num < this.questList.Count)
					{
						xuiC_QuestEntry2.Quest = this.questList[num].Quest;
						xuiC_QuestEntry2.SharedQuestEntry = this.questList[num];
						xuiC_QuestEntry2.OnPress += this.OnPressQuest;
						xuiC_QuestEntry2.ViewComponent.SoundPlayOnClick = true;
						xuiC_QuestEntry2.Selected = this.questList[num].Quest == quest;
						if (xuiC_QuestEntry2.Selected)
						{
							this.SelectedEntry = xuiC_QuestEntry2;
						}
					}
					else
					{
						xuiC_QuestEntry2.Quest = null;
						xuiC_QuestEntry2.ViewComponent.SoundPlayOnClick = false;
						xuiC_QuestEntry2.Selected = false;
					}
				}
			}
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging != null)
			{
				xuiC_Paging.SetLastPageByElementsAndPageLength(this.questList.Count, this.entryList.Count);
			}
			if (this.selectedEntry != null && this.selectedEntry.Quest == null)
			{
				this.selectedEntry = null;
				if (this.questList.Count == 0 && this.selectedEntry == null)
				{
					this.SelectedEntry = null;
					((XUiC_QuestWindowGroup)base.WindowGroup.Controller).SetQuest(this.selectedEntry);
				}
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x06008123 RID: 33059 RVA: 0x003111B8 File Offset: 0x0030F3B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressQuest(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestEntry xuiC_QuestEntry = _sender as XUiC_QuestEntry;
		if (xuiC_QuestEntry != null)
		{
			this.SelectedEntry = xuiC_QuestEntry;
			this.SelectedEntry.QuestUIHandler.SetQuest(this.SelectedEntry);
		}
	}

	// Token: 0x06008124 RID: 33060 RVA: 0x003111EC File Offset: 0x0030F3EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrollQuest(XUiController _sender, float _delta)
	{
		if (_delta > 0f)
		{
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging == null)
			{
				return;
			}
			xuiC_Paging.PageDown();
			return;
		}
		else
		{
			XUiC_Paging xuiC_Paging2 = this.pager;
			if (xuiC_Paging2 == null)
			{
				return;
			}
			xuiC_Paging2.PageUp();
			return;
		}
	}

	// Token: 0x06008125 RID: 33061 RVA: 0x00311219 File Offset: 0x0030F419
	public void SetSharedQuestList(List<SharedQuestEntry> newQuestList)
	{
		this.Page = 0;
		this.questList = newQuestList;
		this.IsDirty = true;
	}

	// Token: 0x06008126 RID: 33062 RVA: 0x00311230 File Offset: 0x0030F430
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged += this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x06008127 RID: 33063 RVA: 0x0031128C File Offset: 0x0030F48C
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		for (int i = 0; i < this.entryList.Count; i++)
		{
			this.entryList[i].IsDirty = true;
		}
	}

	// Token: 0x06008128 RID: 33064 RVA: 0x003112C1 File Offset: 0x0030F4C1
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestChanged -= this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged -= this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x06008129 RID: 33065 RVA: 0x003112FC File Offset: 0x0030F4FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.selectedEntry != null && this.selectedEntry.Quest == q)
		{
			this.selectedEntry.IsDirty = true;
		}
	}

	// Token: 0x0600812A RID: 33066 RVA: 0x00311320 File Offset: 0x0030F520
	public bool HasQuests()
	{
		return this.questList != null && this.questList.Count > 0;
	}

	// Token: 0x04005FCE RID: 24526
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005FCF RID: 24527
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_QuestEntry> entryList = new List<XUiC_QuestEntry>();

	// Token: 0x04005FD0 RID: 24528
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005FD1 RID: 24529
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry selectedEntry;

	// Token: 0x04005FD2 RID: 24530
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SharedQuestEntry> questList = new List<SharedQuestEntry>();

	// Token: 0x04005FD3 RID: 24531
	public XUiC_QuestList QuestList;

	// Token: 0x04005FD4 RID: 24532
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
