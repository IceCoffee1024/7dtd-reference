using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02001091 RID: 4241
[Preserve]
public class XUiC_TwitchVoteInfoEntryList : XUiController
{
	// Token: 0x17000FA3 RID: 4003
	// (get) Token: 0x06008700 RID: 34560 RVA: 0x00339C8D File Offset: 0x00337E8D
	// (set) Token: 0x06008701 RID: 34561 RVA: 0x00339C95 File Offset: 0x00337E95
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

	// Token: 0x17000FA4 RID: 4004
	// (get) Token: 0x06008702 RID: 34562 RVA: 0x00339CC4 File Offset: 0x00337EC4
	// (set) Token: 0x06008703 RID: 34563 RVA: 0x00339CCC File Offset: 0x00337ECC
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

	// Token: 0x06008704 RID: 34564 RVA: 0x00339D00 File Offset: 0x00337F00
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

	// Token: 0x06008705 RID: 34565 RVA: 0x00339D88 File Offset: 0x00337F88
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

	// Token: 0x06008706 RID: 34566 RVA: 0x00339E68 File Offset: 0x00338068
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

	// Token: 0x06008707 RID: 34567 RVA: 0x00339FA8 File Offset: 0x003381A8
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

	// Token: 0x06008708 RID: 34568 RVA: 0x00339FDC File Offset: 0x003381DC
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

	// Token: 0x06008709 RID: 34569 RVA: 0x0033A009 File Offset: 0x00338209
	public void SetTwitchVoteList(List<TwitchVote> newVoteEntryList)
	{
		this.Page = 0;
		this.voteList = newVoteEntryList;
		this.setFirstEntry = true;
		this.IsDirty = true;
	}

	// Token: 0x0600870A RID: 34570 RVA: 0x0033A027 File Offset: 0x00338227
	public override void OnOpen()
	{
		base.OnOpen();
		this.setFirstEntry = true;
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x0600870B RID: 34571 RVA: 0x0033A04C File Offset: 0x0033824C
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x04006481 RID: 25729
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04006482 RID: 25730
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchVoteInfoEntry> entryList = new List<XUiC_TwitchVoteInfoEntry>();

	// Token: 0x04006483 RID: 25731
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006484 RID: 25732
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchVoteInfoEntry selectedEntry;

	// Token: 0x04006485 RID: 25733
	public bool setFirstEntry = true;

	// Token: 0x04006486 RID: 25734
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchVote> voteList = new List<TwitchVote>();

	// Token: 0x04006487 RID: 25735
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x04006488 RID: 25736
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
