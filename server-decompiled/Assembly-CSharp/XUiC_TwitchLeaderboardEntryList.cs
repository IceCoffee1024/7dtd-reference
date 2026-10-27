using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010C7 RID: 4295
[Preserve]
public class XUiC_TwitchLeaderboardEntryList : XUiController
{
	// Token: 0x17001015 RID: 4117
	// (get) Token: 0x0600886C RID: 34924 RVA: 0x0034361D File Offset: 0x0034181D
	// (set) Token: 0x0600886D RID: 34925 RVA: 0x00343625 File Offset: 0x00341825
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

	// Token: 0x17001016 RID: 4118
	// (get) Token: 0x0600886E RID: 34926 RVA: 0x00343654 File Offset: 0x00341854
	// (set) Token: 0x0600886F RID: 34927 RVA: 0x0034365C File Offset: 0x0034185C
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

	// Token: 0x06008870 RID: 34928 RVA: 0x00343690 File Offset: 0x00341890
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

	// Token: 0x06008871 RID: 34929 RVA: 0x00343714 File Offset: 0x00341914
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

	// Token: 0x06008872 RID: 34930 RVA: 0x003437F4 File Offset: 0x003419F4
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

	// Token: 0x06008873 RID: 34931 RVA: 0x003438F4 File Offset: 0x00341AF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchLeaderboardEntry xuiC_TwitchLeaderboardEntry = _sender as XUiC_TwitchLeaderboardEntry;
		if (xuiC_TwitchLeaderboardEntry != null)
		{
			this.SelectedEntry = xuiC_TwitchLeaderboardEntry;
		}
	}

	// Token: 0x06008874 RID: 34932 RVA: 0x00343912 File Offset: 0x00341B12
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

	// Token: 0x06008875 RID: 34933 RVA: 0x0034393F File Offset: 0x00341B3F
	public void SetTwitchLeaderboardList(List<TwitchLeaderboardEntry> newLeaderboardList)
	{
		this.Page = 0;
		this.leaderboardList = newLeaderboardList;
		this.IsDirty = true;
		((XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller).ClearEntries();
	}

	// Token: 0x06008876 RID: 34934 RVA: 0x0034396B File Offset: 0x00341B6B
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06008877 RID: 34935 RVA: 0x00343989 File Offset: 0x00341B89
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x040065A9 RID: 26025
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040065AA RID: 26026
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchLeaderboardEntry> entryList = new List<XUiC_TwitchLeaderboardEntry>();

	// Token: 0x040065AB RID: 26027
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040065AC RID: 26028
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchLeaderboardEntry selectedEntry;

	// Token: 0x040065AD RID: 26029
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchLeaderboardEntry> leaderboardList = new List<TwitchLeaderboardEntry>();

	// Token: 0x040065AE RID: 26030
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x040065AF RID: 26031
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
