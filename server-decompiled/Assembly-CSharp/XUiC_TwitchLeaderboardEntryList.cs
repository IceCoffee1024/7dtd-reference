using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010B7 RID: 4279
[Preserve]
public class XUiC_TwitchLeaderboardEntryList : XUiController
{
	// Token: 0x1700100A RID: 4106
	// (get) Token: 0x06008819 RID: 34841 RVA: 0x00341599 File Offset: 0x0033F799
	// (set) Token: 0x0600881A RID: 34842 RVA: 0x003415A1 File Offset: 0x0033F7A1
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

	// Token: 0x1700100B RID: 4107
	// (get) Token: 0x0600881B RID: 34843 RVA: 0x003415D0 File Offset: 0x0033F7D0
	// (set) Token: 0x0600881C RID: 34844 RVA: 0x003415D8 File Offset: 0x0033F7D8
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

	// Token: 0x0600881D RID: 34845 RVA: 0x0034160C File Offset: 0x0033F80C
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

	// Token: 0x0600881E RID: 34846 RVA: 0x00341690 File Offset: 0x0033F890
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

	// Token: 0x0600881F RID: 34847 RVA: 0x00341770 File Offset: 0x0033F970
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

	// Token: 0x06008820 RID: 34848 RVA: 0x00341870 File Offset: 0x0033FA70
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchLeaderboardEntry xuiC_TwitchLeaderboardEntry = _sender as XUiC_TwitchLeaderboardEntry;
		if (xuiC_TwitchLeaderboardEntry != null)
		{
			this.SelectedEntry = xuiC_TwitchLeaderboardEntry;
		}
	}

	// Token: 0x06008821 RID: 34849 RVA: 0x0034188E File Offset: 0x0033FA8E
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

	// Token: 0x06008822 RID: 34850 RVA: 0x003418BB File Offset: 0x0033FABB
	public void SetTwitchLeaderboardList(List<TwitchLeaderboardEntry> newLeaderboardList)
	{
		this.Page = 0;
		this.leaderboardList = newLeaderboardList;
		this.IsDirty = true;
		((XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller).ClearEntries();
	}

	// Token: 0x06008823 RID: 34851 RVA: 0x003418E7 File Offset: 0x0033FAE7
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06008824 RID: 34852 RVA: 0x00341905 File Offset: 0x0033FB05
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x0400656A RID: 25962
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400656B RID: 25963
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchLeaderboardEntry> entryList = new List<XUiC_TwitchLeaderboardEntry>();

	// Token: 0x0400656C RID: 25964
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400656D RID: 25965
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchLeaderboardEntry selectedEntry;

	// Token: 0x0400656E RID: 25966
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchLeaderboardEntry> leaderboardList = new List<TwitchLeaderboardEntry>();

	// Token: 0x0400656F RID: 25967
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x04006570 RID: 25968
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
