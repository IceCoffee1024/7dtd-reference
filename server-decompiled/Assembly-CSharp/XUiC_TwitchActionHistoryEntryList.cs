using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010B8 RID: 4280
[Preserve]
public class XUiC_TwitchActionHistoryEntryList : XUiController
{
	// Token: 0x17001007 RID: 4103
	// (get) Token: 0x060087CA RID: 34762 RVA: 0x0033E31D File Offset: 0x0033C51D
	// (set) Token: 0x060087CB RID: 34763 RVA: 0x0033E325 File Offset: 0x0033C525
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

	// Token: 0x17001008 RID: 4104
	// (get) Token: 0x060087CC RID: 34764 RVA: 0x0033E354 File Offset: 0x0033C554
	// (set) Token: 0x060087CD RID: 34765 RVA: 0x0033E35C File Offset: 0x0033C55C
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

	// Token: 0x060087CE RID: 34766 RVA: 0x0033E390 File Offset: 0x0033C590
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

	// Token: 0x060087CF RID: 34767 RVA: 0x0033E418 File Offset: 0x0033C618
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

	// Token: 0x060087D0 RID: 34768 RVA: 0x0033E4F8 File Offset: 0x0033C6F8
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

	// Token: 0x060087D1 RID: 34769 RVA: 0x0033E6D4 File Offset: 0x0033C8D4
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

	// Token: 0x060087D2 RID: 34770 RVA: 0x0033E718 File Offset: 0x0033C918
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

	// Token: 0x060087D3 RID: 34771 RVA: 0x0033E74C File Offset: 0x0033C94C
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

	// Token: 0x060087D4 RID: 34772 RVA: 0x0033E779 File Offset: 0x0033C979
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

	// Token: 0x060087D5 RID: 34773 RVA: 0x0033E7B4 File Offset: 0x0033C9B4
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x060087D6 RID: 34774 RVA: 0x0033E7D2 File Offset: 0x0033C9D2
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x040064F0 RID: 25840
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040064F1 RID: 25841
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchActionHistoryEntry> entryList = new List<XUiC_TwitchActionHistoryEntry>();

	// Token: 0x040064F2 RID: 25842
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040064F3 RID: 25843
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionHistoryEntry selectedEntry;

	// Token: 0x040064F4 RID: 25844
	public bool setFirstEntry = true;

	// Token: 0x040064F5 RID: 25845
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchActionHistoryEntry> redemptionList = new List<TwitchActionHistoryEntry>();

	// Token: 0x040064F6 RID: 25846
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x040064F7 RID: 25847
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
