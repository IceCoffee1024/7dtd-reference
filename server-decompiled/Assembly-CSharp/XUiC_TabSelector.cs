using System;
using UnityEngine.Scripting;

// Token: 0x02000EA9 RID: 3753
[Preserve]
public class XUiC_TabSelector : XUiController
{
	// Token: 0x140000B6 RID: 182
	// (add) Token: 0x06007440 RID: 29760 RVA: 0x002C2CB0 File Offset: 0x002C0EB0
	// (remove) Token: 0x06007441 RID: 29761 RVA: 0x002C2CE8 File Offset: 0x002C0EE8
	public event XUiC_TabSelector.TabChangedDelegate OnTabChanged;

	// Token: 0x17000DB0 RID: 3504
	// (get) Token: 0x06007442 RID: 29762 RVA: 0x002C2D1D File Offset: 0x002C0F1D
	// (set) Token: 0x06007443 RID: 29763 RVA: 0x002C2D25 File Offset: 0x002C0F25
	[XuiXmlAttribute("use_page_buttons", false)]
	public bool UsePageButtons
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DB1 RID: 3505
	// (get) Token: 0x06007444 RID: 29764 RVA: 0x002C2D2E File Offset: 0x002C0F2E
	// (set) Token: 0x06007445 RID: 29765 RVA: 0x002C2D38 File Offset: 0x002C0F38
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

	// Token: 0x17000DB2 RID: 3506
	// (get) Token: 0x06007446 RID: 29766 RVA: 0x002C2E70 File Offset: 0x002C1070
	// (set) Token: 0x06007447 RID: 29767 RVA: 0x002C2EA4 File Offset: 0x002C10A4
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

	// Token: 0x17000DB3 RID: 3507
	// (get) Token: 0x06007448 RID: 29768 RVA: 0x002C2ED3 File Offset: 0x002C10D3
	[XuiXmlBinding("selected_tab")]
	public string SelectedTabKey
	{
		get
		{
			XUiC_TabSelectorTab selectedTab = this.SelectedTab;
			return ((selectedTab != null) ? selectedTab.TabKey : null) ?? "";
		}
	}

	// Token: 0x17000DB4 RID: 3508
	// (get) Token: 0x06007449 RID: 29769 RVA: 0x002C2EF0 File Offset: 0x002C10F0
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

	// Token: 0x0600744A RID: 29770 RVA: 0x002C2F04 File Offset: 0x002C1104
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

	// Token: 0x0600744B RID: 29771 RVA: 0x002C3038 File Offset: 0x002C1238
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

	// Token: 0x0600744C RID: 29772 RVA: 0x002C3088 File Offset: 0x002C1288
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
	}

	// Token: 0x0600744D RID: 29773 RVA: 0x002C30A4 File Offset: 0x002C12A4
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

	// Token: 0x0600744E RID: 29774 RVA: 0x002C31B0 File Offset: 0x002C13B0
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

	// Token: 0x0600744F RID: 29775 RVA: 0x002C321C File Offset: 0x002C141C
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

	// Token: 0x06007450 RID: 29776 RVA: 0x002C332D File Offset: 0x002C152D
	public void SetTabCaption(int _index, string _name)
	{
		if (_index >= this.Tabs.Length)
		{
			throw new ArgumentOutOfRangeException("_index");
		}
		this.Tabs[_index].TabHeaderText = _name;
	}

	// Token: 0x06007451 RID: 29777 RVA: 0x002C3353 File Offset: 0x002C1553
	public XUiC_TabSelectorTab GetTab(int _index)
	{
		if (_index >= this.Tabs.Length)
		{
			throw new ArgumentOutOfRangeException("_index");
		}
		return this.Tabs[_index];
	}

	// Token: 0x06007452 RID: 29778 RVA: 0x002C3374 File Offset: 0x002C1574
	public void SelectTabByName(string _tabKey, bool _ignoreCase = false)
	{
		foreach (XUiC_TabSelectorTab xuiC_TabSelectorTab in this.Tabs)
		{
			if (string.Equals(xuiC_TabSelectorTab.TabKey, _tabKey, _ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
			{
				this.SelectedTab = xuiC_TabSelectorTab;
				return;
			}
		}
	}

	// Token: 0x06007453 RID: 29779 RVA: 0x002C33B7 File Offset: 0x002C15B7
	public bool IsSelected(string _tabKey)
	{
		XUiC_TabSelectorTab selectedTab = this.SelectedTab;
		return selectedTab != null && selectedTab.TabKey.Equals(_tabKey);
	}

	// Token: 0x06007454 RID: 29780 RVA: 0x002C33D0 File Offset: 0x002C15D0
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

	// Token: 0x06007455 RID: 29781 RVA: 0x002C3401 File Offset: 0x002C1601
	public void TabButtonClicked(XUiC_TabSelectorButton _tabButton)
	{
		if (!this.enabled)
		{
			return;
		}
		this.SelectedTabIndex = Array.IndexOf<XUiC_TabSelectorButton>(this.tabButtons, _tabButton);
	}

	// Token: 0x06007456 RID: 29782 RVA: 0x002C341E File Offset: 0x002C161E
	[XuiXmlAttribute("select_tab_contents_on_open", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSelectTabContentsOnOpen(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.selectOnOpen = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x06007457 RID: 29783 RVA: 0x002C3437 File Offset: 0x002C1637
	[XuiXmlAttribute("select_tab_contents_on_change", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSelectTabContentsOnChange(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.SelectTabContentsOnChange = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x040056BE RID: 22206
	[XuiBindComponent("tabsHeader.", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelectorButton[] tabButtons;

	// Token: 0x040056BF RID: 22207
	[XuiBindComponent("tabsContents.", true)]
	public readonly XUiC_TabSelectorTab[] Tabs;

	// Token: 0x040056C0 RID: 22208
	[XuiBindComponent("tabsHeader.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Table headerButtonsTable;

	// Token: 0x040056C1 RID: 22209
	[XuiBindComponent("border", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite tabSelectorBorder;

	// Token: 0x040056C2 RID: 22210
	[XuiBindComponent("backgroundMainTabs", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite tabHeaderBackground;

	// Token: 0x040056C3 RID: 22211
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x040056C4 RID: 22212
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedTabIndex;

	// Token: 0x040056C5 RID: 22213
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selectOnOpen;

	// Token: 0x040056C6 RID: 22214
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingOnOpenSelection;

	// Token: 0x040056C8 RID: 22216
	public bool SelectTabContentsOnChange = true;

	// Token: 0x040056C9 RID: 22217
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingOnChangeSelection;

	// Token: 0x02000EAA RID: 3754
	// (Invoke) Token: 0x0600745A RID: 29786
	public delegate void TabChangedDelegate(int _tabIndex, XUiC_TabSelectorTab _tab);
}
