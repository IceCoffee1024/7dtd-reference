using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FDF RID: 4063
[Preserve]
public class XUiC_QuestList : XUiController
{
	// Token: 0x17000EEC RID: 3820
	// (get) Token: 0x06008034 RID: 32820 RVA: 0x00308E27 File Offset: 0x00307027
	// (set) Token: 0x06008035 RID: 32821 RVA: 0x00308E2F File Offset: 0x0030702F
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

	// Token: 0x17000EED RID: 3821
	// (get) Token: 0x06008036 RID: 32822 RVA: 0x00308E5F File Offset: 0x0030705F
	// (set) Token: 0x06008037 RID: 32823 RVA: 0x00308E68 File Offset: 0x00307068
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

	// Token: 0x17000EEE RID: 3822
	// (get) Token: 0x06008038 RID: 32824 RVA: 0x00308F21 File Offset: 0x00307121
	// (set) Token: 0x06008039 RID: 32825 RVA: 0x00308F29 File Offset: 0x00307129
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

	// Token: 0x0600803A RID: 32826 RVA: 0x00308F44 File Offset: 0x00307144
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

	// Token: 0x0600803B RID: 32827 RVA: 0x00308FF8 File Offset: 0x003071F8
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

	// Token: 0x0600803C RID: 32828 RVA: 0x003091D4 File Offset: 0x003073D4
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

	// Token: 0x0600803D RID: 32829 RVA: 0x00309278 File Offset: 0x00307478
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

	// Token: 0x0600803E RID: 32830 RVA: 0x003092A5 File Offset: 0x003074A5
	public void SetQuestList(List<Quest> newQuestList)
	{
		this.Page = 0;
		this.questList = newQuestList;
		this.IsDirty = true;
	}

	// Token: 0x0600803F RID: 32831 RVA: 0x003092BC File Offset: 0x003074BC
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged += this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x06008040 RID: 32832 RVA: 0x00309318 File Offset: 0x00307518
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		for (int i = 0; i < this.entryList.Count; i++)
		{
			this.entryList[i].IsDirty = true;
		}
	}

	// Token: 0x06008041 RID: 32833 RVA: 0x0030934D File Offset: 0x0030754D
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestChanged -= this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged -= this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x06008042 RID: 32834 RVA: 0x00309388 File Offset: 0x00307588
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.selectedEntry != null && this.selectedEntry.Quest == q)
		{
			this.selectedEntry.IsDirty = true;
		}
	}

	// Token: 0x06008043 RID: 32835 RVA: 0x003093AC File Offset: 0x003075AC
	public bool HasQuests()
	{
		return this.questList != null && this.questList.Count > 0;
	}

	// Token: 0x04005EF1 RID: 24305
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005EF2 RID: 24306
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_QuestEntry> entryList = new List<XUiC_QuestEntry>();

	// Token: 0x04005EF3 RID: 24307
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005EF4 RID: 24308
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry selectedEntry;

	// Token: 0x04005EF5 RID: 24309
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Quest> questList = new List<Quest>();

	// Token: 0x04005EF6 RID: 24310
	public XUiC_QuestListWindow QuestListWindow;

	// Token: 0x04005EF7 RID: 24311
	public XUiC_QuestSharedList SharedList;

	// Token: 0x04005EF8 RID: 24312
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04005EF9 RID: 24313
	[PublicizedFrom(EAccessModifier.Private)]
	public int visibleEntries;
}
