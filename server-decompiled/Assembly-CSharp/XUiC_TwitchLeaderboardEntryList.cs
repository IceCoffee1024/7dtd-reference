using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A6 RID: 4262
[Preserve]
public class XUiC_TwitchLeaderboardEntryList : XUiController
{
	// Token: 0x17000FD5 RID: 4053
	// (get) Token: 0x060087CA RID: 34762 RVA: 0x0033D781 File Offset: 0x0033B981
	// (set) Token: 0x060087CB RID: 34763 RVA: 0x0033D789 File Offset: 0x0033B989
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

	// Token: 0x17000FD6 RID: 4054
	// (get) Token: 0x060087CC RID: 34764 RVA: 0x0033D7B8 File Offset: 0x0033B9B8
	// (set) Token: 0x060087CD RID: 34765 RVA: 0x0033D7C0 File Offset: 0x0033B9C0
	public XUiC_TwitchLeaderboardEntry SelectedEntry
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

	// Token: 0x060087CE RID: 34766 RVA: 0x0033D7F4 File Offset: 0x0033B9F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFirstEntry()
	{
		if (this.entryList[0].LeaderboardEntry != null)
		{
			this.SelectedEntry = this.entryList[0];
			this.entryList[0].SelectCursorElement(true, false);
		}
		else
		{
			this.SelectedEntry = null;
			base.WindowGroup.Controller.GetChildById("searchControls").SelectCursorElement(true, false);
		}
		((XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller).ClearEntries();
	}

	// Token: 0x060087CF RID: 34767 RVA: 0x0033D878 File Offset: 0x0033BA78
	public override void Init()
	{
		base.Init();
		XUiC_TwitchInfoWindowGroup xuiC_TwitchInfoWindowGroup = (XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller;
		XUiController childById = xuiC_TwitchInfoWindowGroup.GetChildByType<XUiC_TwitchHowToWindow>().GetChildById("leftButton");
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i] is XUiC_TwitchLeaderboardEntry)
			{
				XUiC_TwitchLeaderboardEntry xuiC_TwitchLeaderboardEntry = (XUiC_TwitchLeaderboardEntry)this.children[i];
				xuiC_TwitchLeaderboardEntry.Owner = this;
				xuiC_TwitchLeaderboardEntry.TwitchInfoUIHandler = xuiC_TwitchInfoWindowGroup;
				xuiC_TwitchLeaderboardEntry.OnScroll += this.OnScrollEntry;
				xuiC_TwitchLeaderboardEntry.ViewComponent.NavRightTarget = childById.ViewComponent;
				this.entryList.Add(xuiC_TwitchLeaderboardEntry);
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

	// Token: 0x060087D0 RID: 34768 RVA: 0x0033D958 File Offset: 0x0033BB58
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			if (this.entryList != null)
			{
				for (int i = 0; i < this.entryList.Count; i++)
				{
					int num = i + this.entryList.Count * this.page;
					XUiC_TwitchLeaderboardEntry xuiC_TwitchLeaderboardEntry = this.entryList[i];
					if (xuiC_TwitchLeaderboardEntry != null)
					{
						xuiC_TwitchLeaderboardEntry.OnPress -= this.OnPressEntry;
						xuiC_TwitchLeaderboardEntry.Selected = false;
						xuiC_TwitchLeaderboardEntry.ViewComponent.SoundPlayOnClick = false;
						if (num < this.leaderboardList.Count)
						{
							xuiC_TwitchLeaderboardEntry.LeaderboardEntry = this.leaderboardList[num];
							xuiC_TwitchLeaderboardEntry.ViewComponent.IsNavigatable = true;
						}
						else
						{
							xuiC_TwitchLeaderboardEntry.LeaderboardEntry = null;
							xuiC_TwitchLeaderboardEntry.ViewComponent.IsNavigatable = false;
						}
					}
				}
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging != null)
				{
					xuiC_Paging.SetLastPageByElementsAndPageLength(this.leaderboardList.Count, this.entryList.Count);
				}
			}
			this.IsDirty = false;
		}
	}

	// Token: 0x060087D1 RID: 34769 RVA: 0x0033DA58 File Offset: 0x0033BC58
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchLeaderboardEntry xuiC_TwitchLeaderboardEntry = _sender as XUiC_TwitchLeaderboardEntry;
		if (xuiC_TwitchLeaderboardEntry != null)
		{
			this.SelectedEntry = xuiC_TwitchLeaderboardEntry;
		}
	}

	// Token: 0x060087D2 RID: 34770 RVA: 0x0033DA76 File Offset: 0x0033BC76
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

	// Token: 0x060087D3 RID: 34771 RVA: 0x0033DAA3 File Offset: 0x0033BCA3
	public void SetTwitchLeaderboardList(List<TwitchLeaderboardEntry> newLeaderboardList)
	{
		this.Page = 0;
		this.leaderboardList = newLeaderboardList;
		this.IsDirty = true;
		((XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller).ClearEntries();
	}

	// Token: 0x060087D4 RID: 34772 RVA: 0x0033DACF File Offset: 0x0033BCCF
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x060087D5 RID: 34773 RVA: 0x0033DAED File Offset: 0x0033BCED
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x04006528 RID: 25896
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04006529 RID: 25897
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchLeaderboardEntry> entryList = new List<XUiC_TwitchLeaderboardEntry>();

	// Token: 0x0400652A RID: 25898
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400652B RID: 25899
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchLeaderboardEntry selectedEntry;

	// Token: 0x0400652C RID: 25900
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchLeaderboardEntry> leaderboardList = new List<TwitchLeaderboardEntry>();

	// Token: 0x0400652D RID: 25901
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x0400652E RID: 25902
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
