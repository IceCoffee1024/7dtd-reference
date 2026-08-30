using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010AA RID: 4266
[Preserve]
public class XUiC_TwitchVoteInfoEntryList : XUiController
{
	// Token: 0x17000FDC RID: 4060
	// (get) Token: 0x060087F2 RID: 34802 RVA: 0x0033E4B5 File Offset: 0x0033C6B5
	// (set) Token: 0x060087F3 RID: 34803 RVA: 0x0033E4BD File Offset: 0x0033C6BD
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

	// Token: 0x17000FDD RID: 4061
	// (get) Token: 0x060087F4 RID: 34804 RVA: 0x0033E4EC File Offset: 0x0033C6EC
	// (set) Token: 0x060087F5 RID: 34805 RVA: 0x0033E4F4 File Offset: 0x0033C6F4
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

	// Token: 0x060087F6 RID: 34806 RVA: 0x0033E528 File Offset: 0x0033C728
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

	// Token: 0x060087F7 RID: 34807 RVA: 0x0033E5B0 File Offset: 0x0033C7B0
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

	// Token: 0x060087F8 RID: 34808 RVA: 0x0033E690 File Offset: 0x0033C890
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

	// Token: 0x060087F9 RID: 34809 RVA: 0x0033E7D0 File Offset: 0x0033C9D0
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

	// Token: 0x060087FA RID: 34810 RVA: 0x0033E804 File Offset: 0x0033CA04
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

	// Token: 0x060087FB RID: 34811 RVA: 0x0033E831 File Offset: 0x0033CA31
	public void SetTwitchVoteList(List<TwitchVote> newVoteEntryList)
	{
		this.Page = 0;
		this.voteList = newVoteEntryList;
		this.setFirstEntry = true;
		this.IsDirty = true;
	}

	// Token: 0x060087FC RID: 34812 RVA: 0x0033E84F File Offset: 0x0033CA4F
	public override void OnOpen()
	{
		base.OnOpen();
		this.setFirstEntry = true;
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x060087FD RID: 34813 RVA: 0x0033E874 File Offset: 0x0033CA74
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x04006548 RID: 25928
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04006549 RID: 25929
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchVoteInfoEntry> entryList = new List<XUiC_TwitchVoteInfoEntry>();

	// Token: 0x0400654A RID: 25930
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400654B RID: 25931
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchVoteInfoEntry selectedEntry;

	// Token: 0x0400654C RID: 25932
	public bool setFirstEntry = true;

	// Token: 0x0400654D RID: 25933
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchVote> voteList = new List<TwitchVote>();

	// Token: 0x0400654E RID: 25934
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x0400654F RID: 25935
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
