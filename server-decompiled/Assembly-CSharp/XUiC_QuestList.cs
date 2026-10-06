using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FEE RID: 4078
[Preserve]
public class XUiC_QuestList : XUiController
{
	// Token: 0x17000F13 RID: 3859
	// (get) Token: 0x06008043 RID: 32835 RVA: 0x0030BDFF File Offset: 0x00309FFF
	// (set) Token: 0x06008044 RID: 32836 RVA: 0x0030BE07 File Offset: 0x0030A007
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

	// Token: 0x17000F14 RID: 3860
	// (get) Token: 0x06008045 RID: 32837 RVA: 0x0030BE37 File Offset: 0x0030A037
	// (set) Token: 0x06008046 RID: 32838 RVA: 0x0030BE40 File Offset: 0x0030A040
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
			XUiC_QuestListWindow questListWindow = this.QuestListWindow;
			bool flag;
			if (this.xui.playerUI.entityPlayer.IsInParty())
			{
				XUiC_QuestEntry xuiC_QuestEntry = this.selectedEntry;
				flag = xuiC_QuestEntry != null && xuiC_QuestEntry.Quest.IsShareable;
			}
			else
			{
				flag = false;
			}
			questListWindow.ShowShareQuest(flag);
			if (this.selectedEntry != null)
			{
				this.selectedEntry.Selected = true;
				if (this.SharedList != null)
				{
					this.SharedList.SelectedEntry = null;
				}
				this.QuestListWindow.ShowRemoveQuest(this.selectedEntry.Quest.QuestClass.AllowRemove);
				return;
			}
			this.QuestListWindow.ShowRemoveQuest(true);
		}
	}

	// Token: 0x17000F15 RID: 3861
	// (get) Token: 0x06008047 RID: 32839 RVA: 0x0030BEF9 File Offset: 0x0030A0F9
	// (set) Token: 0x06008048 RID: 32840 RVA: 0x0030BF01 File Offset: 0x0030A101
	public int VisibleEntries
	{
		get
		{
			return this.visibleEntries;
		}
		set
		{
			if (value != this.visibleEntries)
			{
				this.IsDirty = true;
				this.visibleEntries = value;
			}
		}
	}

	// Token: 0x06008049 RID: 32841 RVA: 0x0030BF1C File Offset: 0x0030A11C
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

	// Token: 0x0600804A RID: 32842 RVA: 0x0030BFD0 File Offset: 0x0030A1D0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			XUiC_QuestEntry xuiC_QuestEntry = this.selectedEntry;
			Quest quest = ((xuiC_QuestEntry != null) ? xuiC_QuestEntry.Quest : null);
			for (int i = 0; i < this.entryList.Count; i++)
			{
				int num = i + this.visibleEntries * this.page;
				XUiC_QuestEntry xuiC_QuestEntry2 = this.entryList[i];
				xuiC_QuestEntry2.OnPress -= this.OnPressQuest;
				xuiC_QuestEntry2.ViewComponent.IsVisible = i < this.visibleEntries;
				if (num < this.questList.Count && i < this.visibleEntries)
				{
					xuiC_QuestEntry2.Quest = this.questList[num];
					xuiC_QuestEntry2.OnPress += this.OnPressQuest;
					xuiC_QuestEntry2.ViewComponent.SoundPlayOnClick = true;
					xuiC_QuestEntry2.Selected = this.questList[num] == quest;
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
			XUiC_Paging xuiC_Paging = this.pager;
			if (xuiC_Paging != null)
			{
				xuiC_Paging.SetLastPageByElementsAndPageLength(this.questList.Count, this.visibleEntries);
			}
			if (this.selectedEntry != null && this.selectedEntry.Quest == null)
			{
				this.selectedEntry = null;
			}
			if (this.selectedEntry == null && this.questList.Count > 0)
			{
				this.SelectedEntry = this.entryList[0];
				((XUiC_QuestWindowGroup)base.WindowGroup.Controller).SetQuest(this.selectedEntry);
			}
			else if (this.questList.Count == 0 && this.selectedEntry == null)
			{
				this.SelectedEntry = null;
				((XUiC_QuestWindowGroup)base.WindowGroup.Controller).SetQuest(this.selectedEntry);
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x0600804B RID: 32843 RVA: 0x0030C1AC File Offset: 0x0030A3AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressQuest(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestEntry xuiC_QuestEntry = _sender as XUiC_QuestEntry;
		if (xuiC_QuestEntry != null)
		{
			this.SelectedEntry = xuiC_QuestEntry;
			this.SelectedEntry.QuestUIHandler.SetQuest(this.SelectedEntry);
			if (InputUtils.ShiftKeyPressed)
			{
				Quest quest = xuiC_QuestEntry.Quest;
				if (quest.Active && !quest.Tracked)
				{
					quest.Tracked = !quest.Tracked;
					this.xui.playerUI.entityPlayer.QuestJournal.TrackedQuest = (quest.Tracked ? quest : null);
					this.xui.playerUI.entityPlayer.QuestJournal.RefreshTracked();
				}
			}
		}
	}

	// Token: 0x0600804C RID: 32844 RVA: 0x0030C250 File Offset: 0x0030A450
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

	// Token: 0x0600804D RID: 32845 RVA: 0x0030C27D File Offset: 0x0030A47D
	public void SetQuestList(List<Quest> newQuestList)
	{
		this.Page = 0;
		this.questList = newQuestList;
		this.IsDirty = true;
	}

	// Token: 0x0600804E RID: 32846 RVA: 0x0030C294 File Offset: 0x0030A494
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged += this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x0600804F RID: 32847 RVA: 0x0030C2F0 File Offset: 0x0030A4F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		for (int i = 0; i < this.entryList.Count; i++)
		{
			this.entryList[i].IsDirty = true;
		}
	}

	// Token: 0x06008050 RID: 32848 RVA: 0x0030C325 File Offset: 0x0030A525
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestChanged -= this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged -= this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x06008051 RID: 32849 RVA: 0x0030C360 File Offset: 0x0030A560
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.selectedEntry != null && this.selectedEntry.Quest == q)
		{
			this.selectedEntry.IsDirty = true;
		}
	}

	// Token: 0x06008052 RID: 32850 RVA: 0x0030C384 File Offset: 0x0030A584
	public bool HasQuests()
	{
		return this.questList != null && this.questList.Count > 0;
	}

	// Token: 0x04005F1A RID: 24346
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005F1B RID: 24347
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_QuestEntry> entryList = new List<XUiC_QuestEntry>();

	// Token: 0x04005F1C RID: 24348
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005F1D RID: 24349
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry selectedEntry;

	// Token: 0x04005F1E RID: 24350
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Quest> questList = new List<Quest>();

	// Token: 0x04005F1F RID: 24351
	public XUiC_QuestListWindow QuestListWindow;

	// Token: 0x04005F20 RID: 24352
	public XUiC_QuestSharedList SharedList;

	// Token: 0x04005F21 RID: 24353
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04005F22 RID: 24354
	[PublicizedFrom(EAccessModifier.Private)]
	public int visibleEntries;
}
