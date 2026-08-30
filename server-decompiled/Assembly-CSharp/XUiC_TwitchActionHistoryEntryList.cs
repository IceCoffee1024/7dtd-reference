using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02001097 RID: 4247
[Preserve]
public class XUiC_TwitchActionHistoryEntryList : XUiController
{
	// Token: 0x17000FC7 RID: 4039
	// (get) Token: 0x06008728 RID: 34600 RVA: 0x00338481 File Offset: 0x00336681
	// (set) Token: 0x06008729 RID: 34601 RVA: 0x00338489 File Offset: 0x00336689
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

	// Token: 0x17000FC8 RID: 4040
	// (get) Token: 0x0600872A RID: 34602 RVA: 0x003384B8 File Offset: 0x003366B8
	// (set) Token: 0x0600872B RID: 34603 RVA: 0x003384C0 File Offset: 0x003366C0
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

	// Token: 0x0600872C RID: 34604 RVA: 0x003384F4 File Offset: 0x003366F4
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

	// Token: 0x0600872D RID: 34605 RVA: 0x0033857C File Offset: 0x0033677C
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

	// Token: 0x0600872E RID: 34606 RVA: 0x0033865C File Offset: 0x0033685C
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

	// Token: 0x0600872F RID: 34607 RVA: 0x00338838 File Offset: 0x00336A38
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

	// Token: 0x06008730 RID: 34608 RVA: 0x0033887C File Offset: 0x00336A7C
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

	// Token: 0x06008731 RID: 34609 RVA: 0x003388B0 File Offset: 0x00336AB0
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

	// Token: 0x06008732 RID: 34610 RVA: 0x003388DD File Offset: 0x00336ADD
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

	// Token: 0x06008733 RID: 34611 RVA: 0x00338918 File Offset: 0x00336B18
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06008734 RID: 34612 RVA: 0x00338936 File Offset: 0x00336B36
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x0400646F RID: 25711
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04006470 RID: 25712
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchActionHistoryEntry> entryList = new List<XUiC_TwitchActionHistoryEntry>();

	// Token: 0x04006471 RID: 25713
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006472 RID: 25714
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionHistoryEntry selectedEntry;

	// Token: 0x04006473 RID: 25715
	public bool setFirstEntry = true;

	// Token: 0x04006474 RID: 25716
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchActionHistoryEntry> redemptionList = new List<TwitchActionHistoryEntry>();

	// Token: 0x04006475 RID: 25717
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x04006476 RID: 25718
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
