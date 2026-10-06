using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A6 RID: 4262
[Preserve]
public class XUiC_TwitchActionEntryList : XUiController
{
	// Token: 0x17000FF8 RID: 4088
	// (get) Token: 0x0600875D RID: 34653 RVA: 0x0033BA71 File Offset: 0x00339C71
	// (set) Token: 0x0600875E RID: 34654 RVA: 0x0033BA79 File Offset: 0x00339C79
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
				this.setFirstEntry = true;
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

	// Token: 0x17000FF9 RID: 4089
	// (get) Token: 0x0600875F RID: 34655 RVA: 0x0033BAAF File Offset: 0x00339CAF
	// (set) Token: 0x06008760 RID: 34656 RVA: 0x0033BAB7 File Offset: 0x00339CB7
	public XUiC_TwitchActionEntry SelectedEntry
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

	// Token: 0x06008761 RID: 34657 RVA: 0x0033BAE8 File Offset: 0x00339CE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetFirstEntry()
	{
		if (this.entryList[0].Action != null)
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

	// Token: 0x06008762 RID: 34658 RVA: 0x0033BB70 File Offset: 0x00339D70
	public override void Init()
	{
		base.Init();
		XUiC_TwitchInfoWindowGroup xuiC_TwitchInfoWindowGroup = (XUiC_TwitchInfoWindowGroup)base.WindowGroup.Controller;
		XUiController childById = xuiC_TwitchInfoWindowGroup.GetChildByType<XUiC_TwitchEntryDescriptionWindow>().GetChildById("btnDecrease");
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i] is XUiC_TwitchActionEntry)
			{
				XUiC_TwitchActionEntry xuiC_TwitchActionEntry = (XUiC_TwitchActionEntry)this.children[i];
				xuiC_TwitchActionEntry.Owner = this;
				xuiC_TwitchActionEntry.TwitchInfoUIHandler = xuiC_TwitchInfoWindowGroup;
				xuiC_TwitchActionEntry.OnScroll += this.OnScrollEntry;
				xuiC_TwitchActionEntry.ViewComponent.NavRightTarget = childById.ViewComponent;
				this.entryList.Add(xuiC_TwitchActionEntry);
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

	// Token: 0x06008763 RID: 34659 RVA: 0x0033BC50 File Offset: 0x00339E50
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
					XUiC_TwitchActionEntry xuiC_TwitchActionEntry = this.entryList[i];
					if (xuiC_TwitchActionEntry != null)
					{
						xuiC_TwitchActionEntry.OnPress -= this.OnPressEntry;
						xuiC_TwitchActionEntry.Selected = false;
						if (num < this.actionList.Count)
						{
							xuiC_TwitchActionEntry.Action = this.actionList[num];
							xuiC_TwitchActionEntry.OnPress += this.OnPressEntry;
							xuiC_TwitchActionEntry.ViewComponent.SoundPlayOnClick = true;
							xuiC_TwitchActionEntry.ViewComponent.IsNavigatable = true;
						}
						else
						{
							xuiC_TwitchActionEntry.Action = null;
							xuiC_TwitchActionEntry.ViewComponent.SoundPlayOnClick = false;
							xuiC_TwitchActionEntry.ViewComponent.IsNavigatable = false;
						}
					}
				}
				XUiC_Paging xuiC_Paging = this.pager;
				if (xuiC_Paging != null)
				{
					xuiC_Paging.SetLastPageByElementsAndPageLength(this.actionList.Count, this.entryList.Count);
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

	// Token: 0x06008764 RID: 34660 RVA: 0x0033BD88 File Offset: 0x00339F88
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPressEntry(XUiController _sender, int _mouseButton)
	{
		XUiC_TwitchActionEntry xuiC_TwitchActionEntry = _sender as XUiC_TwitchActionEntry;
		if (xuiC_TwitchActionEntry != null)
		{
			this.SelectedEntry = xuiC_TwitchActionEntry;
			this.SelectedEntry.TwitchInfoUIHandler.SetEntry(this.SelectedEntry);
		}
	}

	// Token: 0x06008765 RID: 34661 RVA: 0x0033BDBC File Offset: 0x00339FBC
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

	// Token: 0x06008766 RID: 34662 RVA: 0x0033BDE9 File Offset: 0x00339FE9
	public void SetTwitchActionList(List<TwitchAction> newActionEntryList, TwitchActionPreset currentPreset)
	{
		this.CurrentPreset = currentPreset;
		this.Page = 0;
		this.actionList = newActionEntryList;
		this.setFirstEntry = true;
		this.IsDirty = true;
	}

	// Token: 0x06008767 RID: 34663 RVA: 0x0033BE0E File Offset: 0x0033A00E
	public override void OnOpen()
	{
		base.OnOpen();
		this.setFirstEntry = true;
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06008768 RID: 34664 RVA: 0x0033BE33 File Offset: 0x0033A033
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x0400649D RID: 25757
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400649E RID: 25758
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchActionEntry> entryList = new List<XUiC_TwitchActionEntry>();

	// Token: 0x0400649F RID: 25759
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040064A0 RID: 25760
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionEntry selectedEntry;

	// Token: 0x040064A1 RID: 25761
	public bool setFirstEntry = true;

	// Token: 0x040064A2 RID: 25762
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchAction> actionList = new List<TwitchAction>();

	// Token: 0x040064A3 RID: 25763
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x040064A4 RID: 25764
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x040064A5 RID: 25765
	public TwitchActionPreset CurrentPreset;
}
