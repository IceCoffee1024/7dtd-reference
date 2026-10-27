using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010B6 RID: 4278
[Preserve]
public class XUiC_TwitchActionEntryList : XUiController
{
	// Token: 0x17001003 RID: 4099
	// (get) Token: 0x060087B0 RID: 34736 RVA: 0x0033DAF5 File Offset: 0x0033BCF5
	// (set) Token: 0x060087B1 RID: 34737 RVA: 0x0033DAFD File Offset: 0x0033BCFD
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

	// Token: 0x17001004 RID: 4100
	// (get) Token: 0x060087B2 RID: 34738 RVA: 0x0033DB33 File Offset: 0x0033BD33
	// (set) Token: 0x060087B3 RID: 34739 RVA: 0x0033DB3B File Offset: 0x0033BD3B
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

	// Token: 0x060087B4 RID: 34740 RVA: 0x0033DB6C File Offset: 0x0033BD6C
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

	// Token: 0x060087B5 RID: 34741 RVA: 0x0033DBF4 File Offset: 0x0033BDF4
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

	// Token: 0x060087B6 RID: 34742 RVA: 0x0033DCD4 File Offset: 0x0033BED4
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

	// Token: 0x060087B7 RID: 34743 RVA: 0x0033DE0C File Offset: 0x0033C00C
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

	// Token: 0x060087B8 RID: 34744 RVA: 0x0033DE40 File Offset: 0x0033C040
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

	// Token: 0x060087B9 RID: 34745 RVA: 0x0033DE6D File Offset: 0x0033C06D
	public void SetTwitchActionList(List<TwitchAction> newActionEntryList, TwitchActionPreset currentPreset)
	{
		this.CurrentPreset = currentPreset;
		this.Page = 0;
		this.actionList = newActionEntryList;
		this.setFirstEntry = true;
		this.IsDirty = true;
	}

	// Token: 0x060087BA RID: 34746 RVA: 0x0033DE92 File Offset: 0x0033C092
	public override void OnOpen()
	{
		base.OnOpen();
		this.setFirstEntry = true;
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x060087BB RID: 34747 RVA: 0x0033DEB7 File Offset: 0x0033C0B7
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x040064DC RID: 25820
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040064DD RID: 25821
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchActionEntry> entryList = new List<XUiC_TwitchActionEntry>();

	// Token: 0x040064DE RID: 25822
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x040064DF RID: 25823
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionEntry selectedEntry;

	// Token: 0x040064E0 RID: 25824
	public bool setFirstEntry = true;

	// Token: 0x040064E1 RID: 25825
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchAction> actionList = new List<TwitchAction>();

	// Token: 0x040064E2 RID: 25826
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x040064E3 RID: 25827
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x040064E4 RID: 25828
	public TwitchActionPreset CurrentPreset;
}
