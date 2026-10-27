using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FFE RID: 4094
[Preserve]
public class XUiC_QuestList : XUiController
{
	// Token: 0x17000F1E RID: 3870
	// (get) Token: 0x06008092 RID: 32914 RVA: 0x0030D91B File Offset: 0x0030BB1B
	// (set) Token: 0x06008093 RID: 32915 RVA: 0x0030D923 File Offset: 0x0030BB23
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

	// Token: 0x17000F1F RID: 3871
	// (get) Token: 0x06008094 RID: 32916 RVA: 0x0030D953 File Offset: 0x0030BB53
	// (set) Token: 0x06008095 RID: 32917 RVA: 0x0030D95C File Offset: 0x0030BB5C
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

	// Token: 0x17000F20 RID: 3872
	// (get) Token: 0x06008096 RID: 32918 RVA: 0x0030DA15 File Offset: 0x0030BC15
	// (set) Token: 0x06008097 RID: 32919 RVA: 0x0030DA1D File Offset: 0x0030BC1D
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

	// Token: 0x06008098 RID: 32920 RVA: 0x0030DA38 File Offset: 0x0030BC38
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

	// Token: 0x06008099 RID: 32921 RVA: 0x0030DAEC File Offset: 0x0030BCEC
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

	// Token: 0x0600809A RID: 32922 RVA: 0x0030DCC8 File Offset: 0x0030BEC8
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

	// Token: 0x0600809B RID: 32923 RVA: 0x0030DD6C File Offset: 0x0030BF6C
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

	// Token: 0x0600809C RID: 32924 RVA: 0x0030DD99 File Offset: 0x0030BF99
	public void SetQuestList(List<Quest> newQuestList)
	{
		this.Page = 0;
		this.questList = newQuestList;
		this.IsDirty = true;
	}

	// Token: 0x0600809D RID: 32925 RVA: 0x0030DDB0 File Offset: 0x0030BFB0
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.player.QuestChanged += this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged += this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x0600809E RID: 32926 RVA: 0x0030DE0C File Offset: 0x0030C00C
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestTracker_OnTrackedQuestChanged()
	{
		for (int i = 0; i < this.entryList.Count; i++)
		{
			this.entryList[i].IsDirty = true;
		}
	}

	// Token: 0x0600809F RID: 32927 RVA: 0x0030DE41 File Offset: 0x0030C041
	public override void OnClose()
	{
		base.OnClose();
		this.player.QuestChanged -= this.QuestJournal_QuestChanged;
		this.xui.QuestTracker.OnTrackedQuestChanged -= this.QuestTracker_OnTrackedQuestChanged;
	}

	// Token: 0x060080A0 RID: 32928 RVA: 0x0030DE7C File Offset: 0x0030C07C
	[PublicizedFrom(EAccessModifier.Private)]
	public void QuestJournal_QuestChanged(Quest q)
	{
		if (this.selectedEntry != null && this.selectedEntry.Quest == q)
		{
			this.selectedEntry.IsDirty = true;
		}
	}

	// Token: 0x060080A1 RID: 32929 RVA: 0x0030DEA0 File Offset: 0x0030C0A0
	public bool HasQuests()
	{
		return this.questList != null && this.questList.Count > 0;
	}

	// Token: 0x04005F55 RID: 24405
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005F56 RID: 24406
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_QuestEntry> entryList = new List<XUiC_QuestEntry>();

	// Token: 0x04005F57 RID: 24407
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04005F58 RID: 24408
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry selectedEntry;

	// Token: 0x04005F59 RID: 24409
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Quest> questList = new List<Quest>();

	// Token: 0x04005F5A RID: 24410
	public XUiC_QuestListWindow QuestListWindow;

	// Token: 0x04005F5B RID: 24411
	public XUiC_QuestSharedList SharedList;

	// Token: 0x04005F5C RID: 24412
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04005F5D RID: 24413
	[PublicizedFrom(EAccessModifier.Private)]
	public int visibleEntries;
}
