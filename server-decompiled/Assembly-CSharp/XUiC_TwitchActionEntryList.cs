using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x02001095 RID: 4245
[Preserve]
public class XUiC_TwitchActionEntryList : XUiController
{
	// Token: 0x17000FC3 RID: 4035
	// (get) Token: 0x0600870E RID: 34574 RVA: 0x00337C59 File Offset: 0x00335E59
	// (set) Token: 0x0600870F RID: 34575 RVA: 0x00337C61 File Offset: 0x00335E61
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

	// Token: 0x17000FC4 RID: 4036
	// (get) Token: 0x06008710 RID: 34576 RVA: 0x00337C97 File Offset: 0x00335E97
	// (set) Token: 0x06008711 RID: 34577 RVA: 0x00337C9F File Offset: 0x00335E9F
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

	// Token: 0x06008712 RID: 34578 RVA: 0x00337CD0 File Offset: 0x00335ED0
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

	// Token: 0x06008713 RID: 34579 RVA: 0x00337D58 File Offset: 0x00335F58
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

	// Token: 0x06008714 RID: 34580 RVA: 0x00337E38 File Offset: 0x00336038
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

	// Token: 0x06008715 RID: 34581 RVA: 0x00337F70 File Offset: 0x00336170
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

	// Token: 0x06008716 RID: 34582 RVA: 0x00337FA4 File Offset: 0x003361A4
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

	// Token: 0x06008717 RID: 34583 RVA: 0x00337FD1 File Offset: 0x003361D1
	public void SetTwitchActionList(List<TwitchAction> newActionEntryList, TwitchActionPreset currentPreset)
	{
		this.CurrentPreset = currentPreset;
		this.Page = 0;
		this.actionList = newActionEntryList;
		this.setFirstEntry = true;
		this.IsDirty = true;
	}

	// Token: 0x06008718 RID: 34584 RVA: 0x00337FF6 File Offset: 0x003361F6
	public override void OnOpen()
	{
		base.OnOpen();
		this.setFirstEntry = true;
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06008719 RID: 34585 RVA: 0x0033801B File Offset: 0x0033621B
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x0400645B RID: 25691
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400645C RID: 25692
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchActionEntry> entryList = new List<XUiC_TwitchActionEntry>();

	// Token: 0x0400645D RID: 25693
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x0400645E RID: 25694
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionEntry selectedEntry;

	// Token: 0x0400645F RID: 25695
	public bool setFirstEntry = true;

	// Token: 0x04006460 RID: 25696
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchAction> actionList = new List<TwitchAction>();

	// Token: 0x04006461 RID: 25697
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x04006462 RID: 25698
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x04006463 RID: 25699
	public TwitchActionPreset CurrentPreset;
}
