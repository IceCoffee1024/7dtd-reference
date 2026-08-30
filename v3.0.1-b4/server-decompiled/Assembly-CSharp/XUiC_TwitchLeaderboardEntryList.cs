using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200108D RID: 4237
[Preserve]
public class XUiC_TwitchLeaderboardEntryList : XUiController
{
	// Token: 0x17000F9C RID: 3996
	// (get) Token: 0x060086D8 RID: 34520 RVA: 0x00338F59 File Offset: 0x00337159
	// (set) Token: 0x060086D9 RID: 34521 RVA: 0x00338F61 File Offset: 0x00337161
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

	// Token: 0x17000F9D RID: 3997
	// (get) Token: 0x060086DA RID: 34522 RVA: 0x00338F90 File Offset: 0x00337190
	// (set) Token: 0x060086DB RID: 34523 RVA: 0x00338F98 File Offset: 0x00337198
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

	// Token: 0x060086DC RID: 34524 RVA: 0x00338FCC File Offset: 0x003371CC
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

	// Token: 0x060086DD RID: 34525 RVA: 0x00339050 File Offset: 0x00337250
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

	// Token: 0x060086DE RID: 34526 RVA: 0x00339130 File Offset: 0x00337330
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

	// Token: 0x060086DF RID: 34527 RVA: 0x00339230 File Offset: 0x00337430
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchLeaderboardEntry xuiC_TwitchLeaderboardEntry = _sender as XUiC_TwitchLeaderboardEntry;
		if (xuiC_TwitchLeaderboardEntry != null)
		{
			this.SelectedEntry = xuiC_TwitchLeaderboardEntry;
		}
	}

	// Token: 0x060086E0 RID: 34528 RVA: 0x0033924E File Offset: 0x0033744E
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

	// Token: 0x060086E1 RID: 34529 RVA: 0x0033927B File Offset: 0x0033747B
	public void SetTwitchLeaderboardList(List<TwitchLeaderboardEntry> newLeaderboardList)
	{
		this.Page = 0;
		this.leaderboardList = newLeaderboardList;
		this.IsDirty = true;
		((XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller).ClearEntries();
	}

	// Token: 0x060086E2 RID: 34530 RVA: 0x003392A7 File Offset: 0x003374A7
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x060086E3 RID: 34531 RVA: 0x003392C5 File Offset: 0x003374C5
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x04006461 RID: 25697
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04006462 RID: 25698
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchLeaderboardEntry> entryList = new List<XUiC_TwitchLeaderboardEntry>();

	// Token: 0x04006463 RID: 25699
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006464 RID: 25700
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchLeaderboardEntry selectedEntry;

	// Token: 0x04006465 RID: 25701
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchLeaderboardEntry> leaderboardList = new List<TwitchLeaderboardEntry>();

	// Token: 0x04006466 RID: 25702
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x04006467 RID: 25703
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
