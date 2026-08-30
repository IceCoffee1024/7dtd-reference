using System;
using UnityEngine.Scripting;

// Token: 0x02000E85 RID: 3717
[Preserve]
public class XUiC_TabSelector : XUiController
{
	// Token: 0x140000B6 RID: 182
	// (add) Token: 0x060073A8 RID: 29608 RVA: 0x002BC3D4 File Offset: 0x002BA5D4
	// (remove) Token: 0x060073A9 RID: 29609 RVA: 0x002BC40C File Offset: 0x002BA60C
	public event XUiC_TabSelector.TabChangedDelegate OnTabChanged;

	// Token: 0x17000D7C RID: 3452
	// (get) Token: 0x060073AA RID: 29610 RVA: 0x002BC441 File Offset: 0x002BA641
	// (set) Token: 0x060073AB RID: 29611 RVA: 0x002BC449 File Offset: 0x002BA649
	[XuiXmlAttribute("use_page_buttons", false)]
	public bool UsePageButtons
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000D7D RID: 3453
	// (get) Token: 0x060073AC RID: 29612 RVA: 0x002BC452 File Offset: 0x002BA652
	// (set) Token: 0x060073AD RID: 29613 RVA: 0x002BC45C File Offset: 0x002BA65C
	public int SelectedTabIndex
	{
		get
		{
			return this.selectedTabIndex;
		}
		set
		{
			if (!this.enabled)
			{
				return;
			}
			if (value >= this.Tabs.Length)
			{
				value = this.Tabs.Length - 1;
			}
			if (this.selectedTabIndex == value)
			{
				return;
			}
			this.IsDirty = true;
			if (value < 0)
			{
				if (this.SelectedTab != null)
				{
					this.SelectedTab.TabSelected = false;
				}
				this.selectedTabIndex = value;
				this.updateTabVisibility();
				XUiC_TabSelector.TabChangedDelegate onTabChanged = this.OnTabChanged;
				if (onTabChanged == null)
				{
					return;
				}
				onTabChanged(this.selectedTabIndex, this.SelectedTab);
				return;
			}
			else
			{
				if (!this.Tabs[value].TabVisible)
				{
					return;
				}
				if (this.SelectedTab != null)
				{
					this.SelectedTab.TabSelected = false;
				}
				this.selectedTabIndex = value;
				this.SelectedTab.TabSelected = true;
				this.updateTabVisibility();
				XUiC_TabSelector.TabChangedDelegate onTabChanged2 = this.OnTabChanged;
				if (onTabChanged2 != null)
				{
					onTabChanged2(this.selectedTabIndex, this.SelectedTab);
				}
				if (this.xui.playerUI.CursorController.navigationTarget != null && this.xui.playerUI.CursorController.navigationTarget.Controller.IsChildOf(this))
				{
					this.xui.playerUI.CursorController.SetNavigationTarget(null);
				}
				if (this.SelectTabContentsOnChange)
				{
					this.pendingOnChangeSelection = true;
				}
				return;
			}
		}
	}

	// Token: 0x17000D7E RID: 3454
	// (get) Token: 0x060073AE RID: 29614 RVA: 0x002BC594 File Offset: 0x002BA794
	// (set) Token: 0x060073AF RID: 29615 RVA: 0x002BC5C8 File Offset: 0x002BA7C8
	public XUiC_TabSelectorTab SelectedTab
	{
		get
		{
			if (this.Tabs == null)
			{
				return null;
			}
			if (this.selectedTabIndex < 0 || this.selectedTabIndex >= this.Tabs.Length)
			{
				return null;
			}
			return this.Tabs[this.selectedTabIndex];
		}
		set
		{
			if (value == this.SelectedTab)
			{
				return;
			}
			int num = Array.IndexOf<XUiC_TabSelectorTab>(this.Tabs, value);
			if (num >= 0)
			{
				this.SelectedTabIndex = num;
			}
		}
	}

	// Token: 0x17000D7F RID: 3455
	// (get) Token: 0x060073B0 RID: 29616 RVA: 0x002BC5F7 File Offset: 0x002BA7F7
	[XuiXmlBinding("selected_tab")]
	public string SelectedTabKey
	{
		get
		{
			XUiC_TabSelectorTab selectedTab = this.SelectedTab;
			return ((selectedTab != null) ? selectedTab.TabKey : null) ?? "";
		}
	}

	// Token: 0x17000D80 RID: 3456
	// (get) Token: 0x060073B1 RID: 29617 RVA: 0x002BC614 File Offset: 0x002BA814
	public XUiC_TabSelectorButton SelectedTabButton
	{
		get
		{
			XUiC_TabSelectorTab selectedTab = this.SelectedTab;
			if (selectedTab == null)
			{
				return null;
			}
			return selectedTab.TabButton;
		}
	}

	// Token: 0x060073B2 RID: 29618 RVA: 0x002BC628 File Offset: 0x002BA828
	public override void Init()
	{
		base.Init();
		if (this.tabButtons.Length == 0)
		{
			Log.Error("[XUi] TabSelector without any TabSelectorButtons in 'tabsHeader' in windowGroup '" + this.windowGroup.Id + "'");
			return;
		}
		if (this.Tabs.Length == 0)
		{
			Log.Error("[XUi] TabSelector without any TabSelectorTabs in 'tabsContent' in windowGroup '" + this.windowGroup.Id + "'");
			return;
		}
		for (int i = 0; i < this.Tabs.Length; i++)
		{
			if (i >= this.tabButtons.Length)
			{
				Log.Warning(string.Format("More tabs ({0}) than tab buttons ({1}) in windowGroup '{2}'", this.Tabs.Length, this.tabButtons.Length, base.WindowGroup.Id));
				break;
			}
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.Tabs[i];
			XUiC_TabSelectorButton xuiC_TabSelectorButton = this.tabButtons[i];
			xuiC_TabSelectorButton.Tab = xuiC_TabSelectorTab;
			xuiC_TabSelectorTab.TabButton = xuiC_TabSelectorButton;
		}
		if (this.tabHeaderBackground != null && this.tabSelectorBorder != null)
		{
			this.tabHeaderBackground.Sprite.rightAnchor.target = this.tabSelectorBorder.UiTransform;
			this.tabHeaderBackground.Sprite.rightAnchor.relative = 1f;
		}
		this.Tabs[0].TabSelected = true;
	}

	// Token: 0x060073B3 RID: 29619 RVA: 0x002BC75C File Offset: 0x002BA95C
	public override void OnOpen()
	{
		base.OnOpen();
		this.updateTabVisibility();
		this.pendingOnOpenSelection = this.selectOnOpen;
		this.pendingOnChangeSelection = false;
		if (this.UsePageButtons)
		{
			this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory, 0f);
		}
		base.RefreshBindings();
	}

	// Token: 0x060073B4 RID: 29620 RVA: 0x002BC7AC File Offset: 0x002BA9AC
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
	}

	// Token: 0x060073B5 RID: 29621 RVA: 0x002BC7C8 File Offset: 0x002BA9C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateTabVisibility()
	{
		int num = -1;
		for (int i = 0; i < this.tabButtons.Length; i++)
		{
			if (i >= this.Tabs.Length)
			{
				this.tabButtons[i].ViewComponent.IsVisible = false;
			}
			else
			{
				bool tabVisible = this.Tabs[i].TabVisible;
				bool flag = i == this.selectedTabIndex;
				this.tabButtons[i].ViewComponent.IsVisible = tabVisible;
				this.Tabs[i].ViewComponent.IsVisible = flag;
				if (tabVisible)
				{
					num = i;
				}
			}
		}
		if (this.tabHeaderBackground != null)
		{
			UIRect.AnchorPoint leftAnchor = this.tabHeaderBackground.Sprite.leftAnchor;
			if (num >= 0)
			{
				leftAnchor.target = this.tabButtons[num].OuterDimensionsTransform;
				leftAnchor.relative = 1f;
			}
			else
			{
				UIRect.AnchorPoint anchorPoint = leftAnchor;
				XUiV_Sprite xuiV_Sprite = this.tabSelectorBorder;
				anchorPoint.target = ((xuiV_Sprite != null) ? xuiV_Sprite.UiTransform : null);
				leftAnchor.relative = 0f;
			}
			this.tabHeaderBackground.Sprite.ResetAndUpdateAnchors();
		}
		XUiV_Table xuiV_Table = this.headerButtonsTable;
		if (xuiV_Table == null)
		{
			return;
		}
		xuiV_Table.Reposition();
	}

	// Token: 0x060073B6 RID: 29622 RVA: 0x002BC8D4 File Offset: 0x002BAAD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void toggleCategory(int _dir, bool _playSound = true)
	{
		int num = this.selectedTabIndex;
		do
		{
			num = NGUIMath.RepeatIndex(num + _dir, this.Tabs.Length);
		}
		while (!this.Tabs[num].TabVisible && num != this.selectedTabIndex);
		this.SelectedTabIndex = num;
		if (!this.SelectedTab.TabVisible)
		{
			this.SelectedTabIndex = -1;
		}
		if (_playSound)
		{
			XUiC_TabSelectorButton selectedTabButton = this.SelectedTabButton;
			if (selectedTabButton == null)
			{
				return;
			}
			selectedTabButton.PlayClickSound();
		}
	}

	// Token: 0x060073B7 RID: 29623 RVA: 0x002BC940 File Offset: 0x002BAB40
	public override void Update(float _dt)
	{
		base.Update(_dt);
		bool flag = XUiUtils.HotkeysAllowedFor(this.viewComponent, false);
		LocalPlayerUI playerUI = this.xui.playerUI;
		PlayerActionsGUI guiactions = playerUI.playerInput.GUIActions;
		GUIWindowManager windowManager = playerUI.windowManager;
		if (flag)
		{
			if (((!this.UsePageButtons && guiactions.WindowPagingLeft.WasReleased) || (this.UsePageButtons && guiactions.PageDown.WasReleased)) && windowManager.IsWindowOpen(this.windowGroup))
			{
				this.toggleCategory(-1, true);
			}
			if (((!this.UsePageButtons && guiactions.WindowPagingRight.WasReleased) || (this.UsePageButtons && guiactions.PageUp.WasReleased)) && windowManager.IsWindowOpen(this.windowGroup))
			{
				this.toggleCategory(1, true);
			}
		}
		XUiC_TabSelectorTab selectedTab = this.SelectedTab;
		if (selectedTab != null)
		{
			if (this.pendingOnOpenSelection)
			{
				selectedTab.SelectCursorElement(true, false);
				this.pendingOnOpenSelection = false;
			}
			if (this.pendingOnChangeSelection)
			{
				selectedTab.SelectCursorElement(true, false);
				this.pendingOnChangeSelection = false;
			}
		}
		if (this.IsDirty)
		{
			this.IsDirty = false;
			base.RefreshBindings();
		}
	}

	// Token: 0x060073B8 RID: 29624 RVA: 0x002BCA51 File Offset: 0x002BAC51
	public void SetTabCaption(int _index, string _name)
	{
		if (_index >= this.Tabs.Length)
		{
			throw new ArgumentOutOfRangeException("_index");
		}
		this.Tabs[_index].TabHeaderText = _name;
	}

	// Token: 0x060073B9 RID: 29625 RVA: 0x002BCA77 File Offset: 0x002BAC77
	public XUiC_TabSelectorTab GetTab(int _index)
	{
		if (_index >= this.Tabs.Length)
		{
			throw new ArgumentOutOfRangeException("_index");
		}
		return this.Tabs[_index];
	}

	// Token: 0x060073BA RID: 29626 RVA: 0x002BCA98 File Offset: 0x002BAC98
	public void SelectTabByName(string _tabKey)
	{
		foreach (XUiC_TabSelectorTab xuiC_TabSelectorTab in this.Tabs)
		{
			if (!(xuiC_TabSelectorTab.TabKey != _tabKey))
			{
				this.SelectedTab = xuiC_TabSelectorTab;
				return;
			}
		}
	}

	// Token: 0x060073BB RID: 29627 RVA: 0x002BCAD4 File Offset: 0x002BACD4
	public bool IsSelected(string _tabKey)
	{
		XUiC_TabSelectorTab selectedTab = this.SelectedTab;
		return selectedTab != null && selectedTab.TabKey.Equals(_tabKey);
	}

	// Token: 0x060073BC RID: 29628 RVA: 0x002BCAED File Offset: 0x002BACED
	public void TabVisibilityChanged(XUiC_TabSelectorTab _tab, bool _visible)
	{
		if (_tab == this.SelectedTab && !_visible)
		{
			this.toggleCategory(-1, false);
		}
		else if (_visible && this.SelectedTabIndex < 0)
		{
			this.SelectedTab = _tab;
		}
		this.updateTabVisibility();
	}

	// Token: 0x060073BD RID: 29629 RVA: 0x002BCB1E File Offset: 0x002BAD1E
	public void TabButtonClicked(XUiC_TabSelectorButton _tabButton)
	{
		if (!this.enabled)
		{
			return;
		}
		this.SelectedTabIndex = Array.IndexOf<XUiC_TabSelectorButton>(this.tabButtons, _tabButton);
	}

	// Token: 0x060073BE RID: 29630 RVA: 0x002BCB3B File Offset: 0x002BAD3B
	[XuiXmlAttribute("select_tab_contents_on_open", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSelectTabContentsOnOpen(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.selectOnOpen = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x060073BF RID: 29631 RVA: 0x002BCB54 File Offset: 0x002BAD54
	[XuiXmlAttribute("select_tab_contents_on_change", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSelectTabContentsOnChange(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.SelectTabContentsOnChange = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x040055E3 RID: 21987
	[XuiBindComponent("tabsHeader.", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelectorButton[] tabButtons;

	// Token: 0x040055E4 RID: 21988
	[XuiBindComponent("tabsContents.", true)]
	public readonly XUiC_TabSelectorTab[] Tabs;

	// Token: 0x040055E5 RID: 21989
	[XuiBindComponent("tabsHeader.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Table headerButtonsTable;

	// Token: 0x040055E6 RID: 21990
	[XuiBindComponent("border", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite tabSelectorBorder;

	// Token: 0x040055E7 RID: 21991
	[XuiBindComponent("backgroundMainTabs", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite tabHeaderBackground;

	// Token: 0x040055E8 RID: 21992
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x040055E9 RID: 21993
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedTabIndex;

	// Token: 0x040055EA RID: 21994
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selectOnOpen;

	// Token: 0x040055EB RID: 21995
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingOnOpenSelection;

	// Token: 0x040055ED RID: 21997
	public bool SelectTabContentsOnChange = true;

	// Token: 0x040055EE RID: 21998
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingOnChangeSelection;

	// Token: 0x02000E86 RID: 3718
	// (Invoke) Token: 0x060073C2 RID: 29634
	public delegate void TabChangedDelegate(int _tabIndex, XUiC_TabSelectorTab _tab);
}
