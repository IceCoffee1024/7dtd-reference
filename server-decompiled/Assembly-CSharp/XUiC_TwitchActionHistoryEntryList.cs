using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200107E RID: 4222
[Preserve]
public class XUiC_TwitchActionHistoryEntryList : XUiController
{
	// Token: 0x17000F8E RID: 3982
	// (get) Token: 0x06008636 RID: 34358 RVA: 0x00333C59 File Offset: 0x00331E59
	// (set) Token: 0x06008637 RID: 34359 RVA: 0x00333C61 File Offset: 0x00331E61
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

	// Token: 0x17000F8F RID: 3983
	// (get) Token: 0x06008638 RID: 34360 RVA: 0x00333C90 File Offset: 0x00331E90
	// (set) Token: 0x06008639 RID: 34361 RVA: 0x00333C98 File Offset: 0x00331E98
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

	// Token: 0x0600863A RID: 34362 RVA: 0x00333CCC File Offset: 0x00331ECC
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

	// Token: 0x0600863B RID: 34363 RVA: 0x00333D54 File Offset: 0x00331F54
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

	// Token: 0x0600863C RID: 34364 RVA: 0x00333E34 File Offset: 0x00332034
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

	// Token: 0x0600863D RID: 34365 RVA: 0x00334010 File Offset: 0x00332210
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

	// Token: 0x0600863E RID: 34366 RVA: 0x00334054 File Offset: 0x00332254
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

	// Token: 0x0600863F RID: 34367 RVA: 0x00334088 File Offset: 0x00332288
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

	// Token: 0x06008640 RID: 34368 RVA: 0x003340B5 File Offset: 0x003322B5
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

	// Token: 0x06008641 RID: 34369 RVA: 0x003340F0 File Offset: 0x003322F0
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06008642 RID: 34370 RVA: 0x0033410E File Offset: 0x0033230E
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x040063A8 RID: 25512
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040063A9 RID: 25513
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchActionHistoryEntry> entryList = new List<XUiC_TwitchActionHistoryEntry>();

	// Token: 0x040063AA RID: 25514
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040063AB RID: 25515
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionHistoryEntry selectedEntry;

	// Token: 0x040063AC RID: 25516
	public bool setFirstEntry = true;

	// Token: 0x040063AD RID: 25517
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchActionHistoryEntry> redemptionList = new List<TwitchActionHistoryEntry>();

	// Token: 0x040063AE RID: 25518
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x040063AF RID: 25519
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;
}
