using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FD7 RID: 4055
[Preserve]
public class XUiC_QuestSharedList : XUiController
{
	// Token: 0x17000ECB RID: 3787
	// (get) Token: 0x06007FCD RID: 32717 RVA: 0x00307BA7 File Offset: 0x00305DA7
	// (set) Token: 0x06007FCE RID: 32718 RVA: 0x00307BAF File Offset: 0x00305DAF
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

	// Token: 0x17000ECC RID: 3788
	// (get) Token: 0x06007FCF RID: 32719 RVA: 0x00307BDF File Offset: 0x00305DDF
	// (set) Token: 0x06007FD0 RID: 32720 RVA: 0x00307BE7 File Offset: 0x00305DE7
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

	// Token: 0x06007FD1 RID: 32721 RVA: 0x00307C24 File Offset: 0x00305E24
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

	// Token: 0x06007FD2 RID: 32722 RVA: 0x00307CD8 File Offset: 0x00305ED8
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

	// Token: 0x06007FD3 RID: 32723 RVA: 0x00307E80 File Offset: 0x00306080
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

	// Token: 0x06007FD4 RID: 32724 RVA: 0x00307EB4 File Offset: 0x003060B4
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

	// Token: 0x06007FD5 RID: 32725 RVA: 0x00307EE1 File Offset: 0x003060E1
	public void SetSharedQuestList(List<SharedQuestEntry> newQuestList)
	{
		this.Page = 0;
		this.questList = newQuestList;
		this.IsDirty = true;
	}

	// Token: 0x06007FD6 RID: 32726 RVA: 0x00307EF8 File Offset: 0x003060F8
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged += this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x06007FD7 RID: 32727 RVA: 0x00307F54 File Offset: 0x00306154
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		for (int i = 0; i < this.entryList.Count; i++)
		{
			this.entryList[i].IsDirty = true;
		}
	}

	// Token: 0x06007FD8 RID: 32728 RVA: 0x00307F89 File Offset: 0x00306189
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestChanged -= this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged -= this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x06007FD9 RID: 32729 RVA: 0x00307FC4 File Offset: 0x003061C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.selectedEntry != null && this.selectedEntry.Quest == q)
		{
			this.selectedEntry.IsDirty = true;
		}
	}

	// Token: 0x06007FDA RID: 32730 RVA: 0x00307FE8 File Offset: 0x003061E8
	public bool HasQuests()
	{
		return this.questList != null && this.questList.Count > 0;
	}

	// Token: 0x04005EA3 RID: 24227
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005EA4 RID: 24228
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_QuestEntry> entryList = new List<XUiC_QuestEntry>();

	// Token: 0x04005EA5 RID: 24229
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005EA6 RID: 24230
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry selectedEntry;

	// Token: 0x04005EA7 RID: 24231
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SharedQuestEntry> questList = new List<SharedQuestEntry>();

	// Token: 0x04005EA8 RID: 24232
	public XUiC_QuestList QuestList;

	// Token: 0x04005EA9 RID: 24233
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
