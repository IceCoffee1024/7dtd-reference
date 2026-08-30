using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FC6 RID: 4038
[Preserve]
public class XUiC_QuestList : XUiController
{
	// Token: 0x17000EB3 RID: 3763
	// (get) Token: 0x06007F42 RID: 32578 RVA: 0x00304653 File Offset: 0x00302853
	// (set) Token: 0x06007F43 RID: 32579 RVA: 0x0030465B File Offset: 0x0030285B
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

	// Token: 0x17000EB4 RID: 3764
	// (get) Token: 0x06007F44 RID: 32580 RVA: 0x0030468B File Offset: 0x0030288B
	// (set) Token: 0x06007F45 RID: 32581 RVA: 0x00304694 File Offset: 0x00302894
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

	// Token: 0x17000EB5 RID: 3765
	// (get) Token: 0x06007F46 RID: 32582 RVA: 0x0030474D File Offset: 0x0030294D
	// (set) Token: 0x06007F47 RID: 32583 RVA: 0x00304755 File Offset: 0x00302955
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

	// Token: 0x06007F48 RID: 32584 RVA: 0x00304770 File Offset: 0x00302970
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

	// Token: 0x06007F49 RID: 32585 RVA: 0x00304824 File Offset: 0x00302A24
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

	// Token: 0x06007F4A RID: 32586 RVA: 0x00304A00 File Offset: 0x00302C00
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

	// Token: 0x06007F4B RID: 32587 RVA: 0x00304AA4 File Offset: 0x00302CA4
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

	// Token: 0x06007F4C RID: 32588 RVA: 0x00304AD1 File Offset: 0x00302CD1
	public void SetQuestList(List<Quest> newQuestList)
	{
		this.Page = 0;
		this.questList = newQuestList;
		this.IsDirty = true;
	}

	// Token: 0x06007F4D RID: 32589 RVA: 0x00304AE8 File Offset: 0x00302CE8
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged += this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x06007F4E RID: 32590 RVA: 0x00304B44 File Offset: 0x00302D44
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		for (int i = 0; i < this.entryList.Count; i++)
		{
			this.entryList[i].IsDirty = true;
		}
	}

	// Token: 0x06007F4F RID: 32591 RVA: 0x00304B79 File Offset: 0x00302D79
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestChanged -= this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged -= this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x06007F50 RID: 32592 RVA: 0x00304BB4 File Offset: 0x00302DB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.selectedEntry != null && this.selectedEntry.Quest == q)
		{
			this.selectedEntry.IsDirty = true;
		}
	}

	// Token: 0x06007F51 RID: 32593 RVA: 0x00304BD8 File Offset: 0x00302DD8
	public bool HasQuests()
	{
		return this.questList != null && this.questList.Count > 0;
	}

	// Token: 0x04005E2A RID: 24106
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005E2B RID: 24107
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_QuestEntry> entryList = new List<XUiC_QuestEntry>();

	// Token: 0x04005E2C RID: 24108
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005E2D RID: 24109
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry selectedEntry;

	// Token: 0x04005E2E RID: 24110
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Quest> questList = new List<Quest>();

	// Token: 0x04005E2F RID: 24111
	public XUiC_QuestListWindow QuestListWindow;

	// Token: 0x04005E30 RID: 24112
	public XUiC_QuestSharedList SharedList;

	// Token: 0x04005E31 RID: 24113
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04005E32 RID: 24114
	[PublicizedFrom(EAccessModifier.Private)]
	public int visibleEntries;
}
