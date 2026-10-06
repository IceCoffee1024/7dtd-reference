using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010BB RID: 4283
[Preserve]
public class XUiC_TwitchVoteInfoEntryList : XUiController
{
	// Token: 0x17001011 RID: 4113
	// (get) Token: 0x06008841 RID: 34881 RVA: 0x003422CD File Offset: 0x003404CD
	// (set) Token: 0x06008842 RID: 34882 RVA: 0x003422D5 File Offset: 0x003404D5
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
				this.IsDirty = true;
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging == null)
				{
					return;
				}
				xuiC_Paging.SetPage(this.page);
			}
		}
	}

	// Token: 0x17001012 RID: 4114
	// (get) Token: 0x06008843 RID: 34883 RVA: 0x00342304 File Offset: 0x00340504
	// (set) Token: 0x06008844 RID: 34884 RVA: 0x0034230C File Offset: 0x0034050C
	public XUiC_TwitchVoteInfoEntry SelectedEntry
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
			}
		}
	}

	// Token: 0x06008845 RID: 34885 RVA: 0x00342340 File Offset: 0x00340540
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFirstEntry()
	{
		if (this.entryList[0].Vote != null)
		{
			this.SelectedEntry = this.entryList[0];
			this.entryList[0].SelectCursorElement(true, false);
		}
		else
		{
			this.SelectedEntry = null;
			base.WindowGroup.Controller.GetChildById("searchControls").SelectCursorElement(true, false);
		}
		((XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller).SetEntry(this.selectedEntry);
	}

	// Token: 0x06008846 RID: 34886 RVA: 0x003423C8 File Offset: 0x003405C8
	public override void Init()
	{
		base.Init();
		XUiC_TwitchInfoWindowGroup xuiC_TwitchInfoWindowGroup = (XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller;
		XUiController childById = xuiC_TwitchInfoWindowGroup.GetChildByType<XUiC_TwitchEntryDescriptionWindow>().GetChildById("btnEnable");
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i] is XUiC_TwitchVoteInfoEntry)
			{
				XUiC_TwitchVoteInfoEntry xuiC_TwitchVoteInfoEntry = (XUiC_TwitchVoteInfoEntry)this.children[i];
				xuiC_TwitchVoteInfoEntry.Owner = this;
				xuiC_TwitchVoteInfoEntry.TwitchInfoUIHandler = xuiC_TwitchInfoWindowGroup;
				xuiC_TwitchVoteInfoEntry.OnScroll += this.OnScrollEntry;
				xuiC_TwitchVoteInfoEntry.ViewComponent.NavRightTarget = childById.ViewComponent;
				this.entryList.Add(xuiC_TwitchVoteInfoEntry);
			}
		}
		this.pager = base.Parent.GetChildByType<XUiC_Paging>();
		if (this.pager != null)
		{
			this.pager.OnPageChanged += delegate
			{
				if (this.viewComponent.IsVisible)
				{
					this.Page = this.pager.CurrentPageNumber;
				}
			};
		}
	}

	// Token: 0x06008847 RID: 34887 RVA: 0x003424A8 File Offset: 0x003406A8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			Log.Out("Vote list update");
			if (this.entryList != null)
			{
				for (int i = 0; i < this.entryList.Count; i++)
				{
					int num = i + this.entryList.Count * this.page;
					XUiC_TwitchVoteInfoEntry xuiC_TwitchVoteInfoEntry = this.entryList[i];
					if (xuiC_TwitchVoteInfoEntry != null)
					{
						xuiC_TwitchVoteInfoEntry.OnPress -= this.OnPressEntry;
						xuiC_TwitchVoteInfoEntry.Selected = false;
						if (num < this.voteList.Count)
						{
							xuiC_TwitchVoteInfoEntry.Vote = this.voteList[num];
							xuiC_TwitchVoteInfoEntry.OnPress += this.OnPressEntry;
							xuiC_TwitchVoteInfoEntry.ViewComponent.SoundPlayOnClick = true;
							xuiC_TwitchVoteInfoEntry.ViewComponent.IsNavigatable = true;
						}
						else
						{
							xuiC_TwitchVoteInfoEntry.Vote = null;
							xuiC_TwitchVoteInfoEntry.ViewComponent.SoundPlayOnClick = false;
							xuiC_TwitchVoteInfoEntry.ViewComponent.IsNavigatable = false;
						}
					}
				}
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging != null)
				{
					xuiC_Paging.SetLastPageByElementsAndPageLength(this.voteList.Count, this.entryList.Count);
				}
			}
			if (this.setFirstEntry)
			{
				this.SetFirstEntry();
				this.setFirstEntry = false;
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x06008848 RID: 34888 RVA: 0x003425E8 File Offset: 0x003407E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchVoteInfoEntry xuiC_TwitchVoteInfoEntry = _sender as XUiC_TwitchVoteInfoEntry;
		if (xuiC_TwitchVoteInfoEntry != null)
		{
			this.SelectedEntry = xuiC_TwitchVoteInfoEntry;
			this.SelectedEntry.TwitchInfoUIHandler.SetEntry(this.SelectedEntry);
		}
	}

	// Token: 0x06008849 RID: 34889 RVA: 0x0034261C File Offset: 0x0034081C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnScrollEntry(XUiController _sender, float _delta)
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

	// Token: 0x0600884A RID: 34890 RVA: 0x00342649 File Offset: 0x00340849
	public void SetTwitchVoteList(List<TwitchVote> newVoteEntryList)
	{
		this.Page = 0;
		this.voteList = newVoteEntryList;
		this.setFirstEntry = true;
		this.IsDirty = true;
	}

	// Token: 0x0600884B RID: 34891 RVA: 0x00342667 File Offset: 0x00340867
	public override void OnOpen()
	{
		base.OnOpen();
		this.setFirstEntry = true;
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x0600884C RID: 34892 RVA: 0x0034268C File Offset: 0x0034088C
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x0400658A RID: 25994
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400658B RID: 25995
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchVoteInfoEntry> entryList = new List<XUiC_TwitchVoteInfoEntry>();

	// Token: 0x0400658C RID: 25996
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400658D RID: 25997
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchVoteInfoEntry selectedEntry;

	// Token: 0x0400658E RID: 25998
	public bool setFirstEntry = true;

	// Token: 0x0400658F RID: 25999
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchVote> voteList = new List<TwitchVote>();

	// Token: 0x04006590 RID: 26000
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x04006591 RID: 26001
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
