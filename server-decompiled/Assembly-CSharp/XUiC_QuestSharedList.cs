using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FFF RID: 4095
[Preserve]
public class XUiC_QuestSharedList : XUiController
{
	// Token: 0x17000F2B RID: 3883
	// (get) Token: 0x060080CE RID: 32974 RVA: 0x0030F3C3 File Offset: 0x0030D5C3
	// (set) Token: 0x060080CF RID: 32975 RVA: 0x0030F3CB File Offset: 0x0030D5CB
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

	// Token: 0x17000F2C RID: 3884
	// (get) Token: 0x060080D0 RID: 32976 RVA: 0x0030F3FB File Offset: 0x0030D5FB
	// (set) Token: 0x060080D1 RID: 32977 RVA: 0x0030F403 File Offset: 0x0030D603
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

	// Token: 0x060080D2 RID: 32978 RVA: 0x0030F440 File Offset: 0x0030D640
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

	// Token: 0x060080D3 RID: 32979 RVA: 0x0030F4F4 File Offset: 0x0030D6F4
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

	// Token: 0x060080D4 RID: 32980 RVA: 0x0030F69C File Offset: 0x0030D89C
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

	// Token: 0x060080D5 RID: 32981 RVA: 0x0030F6D0 File Offset: 0x0030D8D0
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

	// Token: 0x060080D6 RID: 32982 RVA: 0x0030F6FD File Offset: 0x0030D8FD
	public void SetSharedQuestList(List<SharedQuestEntry> newQuestList)
	{
		this.Page = 0;
		this.questList = newQuestList;
		this.IsDirty = true;
	}

	// Token: 0x060080D7 RID: 32983 RVA: 0x0030F714 File Offset: 0x0030D914
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged += this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x060080D8 RID: 32984 RVA: 0x0030F770 File Offset: 0x0030D970
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		for (int i = 0; i < this.entryList.Count; i++)
		{
			this.entryList[i].IsDirty = true;
		}
	}

	// Token: 0x060080D9 RID: 32985 RVA: 0x0030F7A5 File Offset: 0x0030D9A5
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestChanged -= this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged -= this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x060080DA RID: 32986 RVA: 0x0030F7E0 File Offset: 0x0030D9E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.selectedEntry != null && this.selectedEntry.Quest == q)
		{
			this.selectedEntry.IsDirty = true;
		}
	}

	// Token: 0x060080DB RID: 32987 RVA: 0x0030F804 File Offset: 0x0030DA04
	public bool HasQuests()
	{
		return this.questList != null && this.questList.Count > 0;
	}

	// Token: 0x04005F93 RID: 24467
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005F94 RID: 24468
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_QuestEntry> entryList = new List<XUiC_QuestEntry>();

	// Token: 0x04005F95 RID: 24469
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005F96 RID: 24470
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry selectedEntry;

	// Token: 0x04005F97 RID: 24471
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SharedQuestEntry> questList = new List<SharedQuestEntry>();

	// Token: 0x04005F98 RID: 24472
	public XUiC_QuestList QuestList;

	// Token: 0x04005F99 RID: 24473
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
