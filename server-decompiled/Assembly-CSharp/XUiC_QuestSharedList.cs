using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FF0 RID: 4080
[Preserve]
public class XUiC_QuestSharedList : XUiController
{
	// Token: 0x17000F04 RID: 3844
	// (get) Token: 0x060080BF RID: 32959 RVA: 0x0030C3DB File Offset: 0x0030A5DB
	// (set) Token: 0x060080C0 RID: 32960 RVA: 0x0030C3E3 File Offset: 0x0030A5E3
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

	// Token: 0x17000F05 RID: 3845
	// (get) Token: 0x060080C1 RID: 32961 RVA: 0x0030C413 File Offset: 0x0030A613
	// (set) Token: 0x060080C2 RID: 32962 RVA: 0x0030C41B File Offset: 0x0030A61B
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

	// Token: 0x060080C3 RID: 32963 RVA: 0x0030C458 File Offset: 0x0030A658
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

	// Token: 0x060080C4 RID: 32964 RVA: 0x0030C50C File Offset: 0x0030A70C
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

	// Token: 0x060080C5 RID: 32965 RVA: 0x0030C6B4 File Offset: 0x0030A8B4
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

	// Token: 0x060080C6 RID: 32966 RVA: 0x0030C6E8 File Offset: 0x0030A8E8
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

	// Token: 0x060080C7 RID: 32967 RVA: 0x0030C715 File Offset: 0x0030A915
	public void SetSharedQuestList(List<SharedQuestEntry> newQuestList)
	{
		this.Page = 0;
		this.questList = newQuestList;
		this.IsDirty = true;
	}

	// Token: 0x060080C8 RID: 32968 RVA: 0x0030C72C File Offset: 0x0030A92C
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged += this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x060080C9 RID: 32969 RVA: 0x0030C788 File Offset: 0x0030A988
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		for (int i = 0; i < this.entryList.Count; i++)
		{
			this.entryList[i].IsDirty = true;
		}
	}

	// Token: 0x060080CA RID: 32970 RVA: 0x0030C7BD File Offset: 0x0030A9BD
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestChanged -= this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged -= this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x060080CB RID: 32971 RVA: 0x0030C7F8 File Offset: 0x0030A9F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.selectedEntry != null && this.selectedEntry.Quest == q)
		{
			this.selectedEntry.IsDirty = true;
		}
	}

	// Token: 0x060080CC RID: 32972 RVA: 0x0030C81C File Offset: 0x0030AA1C
	public bool HasQuests()
	{
		return this.questList != null && this.questList.Count > 0;
	}

	// Token: 0x04005F6A RID: 24426
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005F6B RID: 24427
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_QuestEntry> entryList = new List<XUiC_QuestEntry>();

	// Token: 0x04005F6C RID: 24428
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005F6D RID: 24429
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry selectedEntry;

	// Token: 0x04005F6E RID: 24430
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SharedQuestEntry> questList = new List<SharedQuestEntry>();

	// Token: 0x04005F6F RID: 24431
	public XUiC_QuestList QuestList;

	// Token: 0x04005F70 RID: 24432
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
