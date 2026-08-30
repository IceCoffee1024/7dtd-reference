using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x0200107C RID: 4220
[Preserve]
public class XUiC_TwitchActionEntryList : XUiController
{
	// Token: 0x17000F8A RID: 3978
	// (get) Token: 0x0600861C RID: 34332 RVA: 0x00333431 File Offset: 0x00331631
	// (set) Token: 0x0600861D RID: 34333 RVA: 0x00333439 File Offset: 0x00331639
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

	// Token: 0x17000F8B RID: 3979
	// (get) Token: 0x0600861E RID: 34334 RVA: 0x0033346F File Offset: 0x0033166F
	// (set) Token: 0x0600861F RID: 34335 RVA: 0x00333477 File Offset: 0x00331677
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

	// Token: 0x06008620 RID: 34336 RVA: 0x003334A8 File Offset: 0x003316A8
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

	// Token: 0x06008621 RID: 34337 RVA: 0x00333530 File Offset: 0x00331730
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

	// Token: 0x06008622 RID: 34338 RVA: 0x00333610 File Offset: 0x00331810
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

	// Token: 0x06008623 RID: 34339 RVA: 0x00333748 File Offset: 0x00331948
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

	// Token: 0x06008624 RID: 34340 RVA: 0x0033377C File Offset: 0x0033197C
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

	// Token: 0x06008625 RID: 34341 RVA: 0x003337A9 File Offset: 0x003319A9
	public void SetTwitchActionList(List<TwitchAction> newActionEntryList, TwitchActionPreset currentPreset)
	{
		this.CurrentPreset = currentPreset;
		this.Page = 0;
		this.actionList = newActionEntryList;
		this.setFirstEntry = true;
		this.IsDirty = true;
	}

	// Token: 0x06008626 RID: 34342 RVA: 0x003337CE File Offset: 0x003319CE
	public override void OnOpen()
	{
		base.OnOpen();
		this.setFirstEntry = true;
		this.player = this.xui.playerUI.entityPlayer;
	}

	// Token: 0x06008627 RID: 34343 RVA: 0x003337F3 File Offset: 0x003319F3
	public override void OnClose()
	{
		base.OnClose();
		this.SelectedEntry = null;
	}

	// Token: 0x04006394 RID: 25492
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04006395 RID: 25493
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_TwitchActionEntry> entryList = new List<XUiC_TwitchActionEntry>();

	// Token: 0x04006396 RID: 25494
	[PublicizedFrom(EAccessModifier.Private)]
	public int page;

	// Token: 0x04006397 RID: 25495
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TwitchActionEntry selectedEntry;

	// Token: 0x04006398 RID: 25496
	public bool setFirstEntry = true;

	// Token: 0x04006399 RID: 25497
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TwitchAction> actionList = new List<TwitchAction>();

	// Token: 0x0400639A RID: 25498
	public XUiC_TwitchEntryListWindow TwitchEntryListWindow;

	// Token: 0x0400639B RID: 25499
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Paging pager;

	// Token: 0x0400639C RID: 25500
	public TwitchActionPreset CurrentPreset;
}
