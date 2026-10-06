using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A8 RID: 4264
[Preserve]
public class XUiC_TwitchActionHistoryEntryList : XUiController
{
	// Token: 0x17000FFC RID: 4092
	// (get) Token: 0x06008777 RID: 34679 RVA: 0x0033C299 File Offset: 0x0033A499
	// (set) Token: 0x06008778 RID: 34680 RVA: 0x0033C2A1 File Offset: 0x0033A4A1
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

	// Token: 0x17000FFD RID: 4093
	// (get) Token: 0x06008779 RID: 34681 RVA: 0x0033C2D0 File Offset: 0x0033A4D0
	// (set) Token: 0x0600877A RID: 34682 RVA: 0x0033C2D8 File Offset: 0x0033A4D8
	public XUiC_TwitchActionHistoryEntry SelectedEntry
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

	// Token: 0x0600877B RID: 34683 RVA: 0x0033C30C File Offset: 0x0033A50C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFirstEntry()
	{
		if (this.entryList[0].HistoryItem != null)
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

	// Token: 0x0600877C RID: 34684 RVA: 0x0033C394 File Offset: 0x0033A594
	public override void Init()
	{
		base.Init();
		XUiC_TwitchInfoWindowGroup xuiC_TwitchInfoWindowGroup = (XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller;
		XUiController childById = xuiC_TwitchInfoWindowGroup.GetChildByType<XUiC_TwitchEntryDescriptionWindow>().GetChildById("btnRefund");
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i] is XUiC_TwitchActionHistoryEntry)
			{
				XUiC_TwitchActionHistoryEntry xuiC_TwitchActionHistoryEntry = (XUiC_TwitchActionHistoryEntry)this.children[i];
				xuiC_TwitchActionHistoryEntry.Owner = this;
				xuiC_TwitchActionHistoryEntry.TwitchInfoUIHandler = xuiC_TwitchInfoWindowGroup;
				xuiC_TwitchActionHistoryEntry.OnScroll += this.OnScrollEntry;
				xuiC_TwitchActionHistoryEntry.ViewComponent.NavRightTarget = childById.ViewComponent;
				this.entryList.Add(xuiC_TwitchActionHistoryEntry);
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
					this.SelectedEntry = null;
					this.setFirstEntry = true;
				}
			};
		}
	}

	// Token: 0x0600877D RID: 34685 RVA: 0x0033C474 File Offset: 0x0033A674
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			if (this.entryList != null)
			{
				TwitchActionHistoryEntry twitchActionHistoryEntry = ((this.selectedEntry != null) ? this.selectedEntry.HistoryItem : null);
				bool flag = false;
				int num = this.GetPage(twitchActionHistoryEntry);
				int num2 = this.page;
				if (num != -1 && num != this.page)
				{
					flag = true;
					num2 = num;
				}
				for (int i = 0; i < this.entryList.Count; i++)
				{
					int num3 = i + this.entryList.Count * num2;
					XUiC_TwitchActionHistoryEntry xuiC_TwitchActionHistoryEntry = this.entryList[i];
					if (xuiC_TwitchActionHistoryEntry != null)
					{
						xuiC_TwitchActionHistoryEntry.OnPress -= this.OnPressEntry;
						if (num3 < this.redemptionList.Count)
						{
							xuiC_TwitchActionHistoryEntry.HistoryItem = this.redemptionList[num3];
							xuiC_TwitchActionHistoryEntry.OnPress += this.OnPressEntry;
							xuiC_TwitchActionHistoryEntry.ViewComponent.SoundPlayOnClick = true;
							xuiC_TwitchActionHistoryEntry.Selected = xuiC_TwitchActionHistoryEntry.HistoryItem == twitchActionHistoryEntry;
							if (xuiC_TwitchActionHistoryEntry.Selected)
							{
								this.SelectedEntry = xuiC_TwitchActionHistoryEntry;
								((XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller).SetEntry(this.selectedEntry);
							}
							xuiC_TwitchActionHistoryEntry.ViewComponent.IsNavigatable = true;
						}
						else
						{
							xuiC_TwitchActionHistoryEntry.HistoryItem = null;
							xuiC_TwitchActionHistoryEntry.ViewComponent.SoundPlayOnClick = false;
							xuiC_TwitchActionHistoryEntry.Selected = false;
							xuiC_TwitchActionHistoryEntry.ViewComponent.IsNavigatable = false;
						}
					}
				}
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging != null)
				{
					xuiC_Paging.SetLastPageByElementsAndPageLength(this.redemptionList.Count, this.entryList.Count);
				}
				if (flag)
				{
					this.Page = num2;
					if (this.pager != null)
					{
						this.pager.RefreshBindings();
					}
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

	// Token: 0x0600877E RID: 34686 RVA: 0x0033C650 File Offset: 0x0033A850
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetPage(TwitchActionHistoryEntry historyItem)
	{
		for (int i = 0; i < this.redemptionList.Count; i++)
		{
			if (this.redemptionList[i] == historyItem)
			{
				return i / this.entryList.Count;
			}
		}
		return -1;
	}

	// Token: 0x0600877F RID: 34687 RVA: 0x0033C694 File Offset: 0x0033A894
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchActionHistoryEntry xuiC_TwitchActionHistoryEntry = _sender as XUiC_TwitchActionHistoryEntry;
		if (xuiC_TwitchActionHistoryEntry != null)
		{
			this.SelectedEntry = xuiC_TwitchActionHistoryEntry;
			this.SelectedEntry.TwitchInfoUIHandler.SetEntry(this.SelectedEntry);
		}
	}

	// Token: 0x06008780 RID: 34688 RVA: 0x0033C6C8 File Offset: 0x0033A8C8
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

	// Token: 0x06008781 RID: 34689 RVA: 0x0033C6F5 File Offset: 0x0033A8F5
	public void SetTwitchActionHistoryList(List<TwitchActionHistoryEntry> newRedemptionList)
	{
		this.redemptionList = newRedemptionList;
		if (this.SelectedEntry == null)
		{
			this.Page = 0;
			this.setFirstEntry = true;
		}
		this.IsDirty = true;
		((XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller).ClearEntries();
	}

	// Token: 0x06008782 RID: 34690 RVA: 0x0033C730 File Offset: 0x0033A930
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06008783 RID: 34691 RVA: 0x0033C74E File Offset: 0x0033A94E
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x040064B1 RID: 25777
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040064B2 RID: 25778
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchActionHistoryEntry> entryList = new List<XUiC_TwitchActionHistoryEntry>();

	// Token: 0x040064B3 RID: 25779
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040064B4 RID: 25780
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionHistoryEntry selectedEntry;

	// Token: 0x040064B5 RID: 25781
	public bool setFirstEntry = true;

	// Token: 0x040064B6 RID: 25782
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchActionHistoryEntry> redemptionList = new List<TwitchActionHistoryEntry>();

	// Token: 0x040064B7 RID: 25783
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x040064B8 RID: 25784
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
