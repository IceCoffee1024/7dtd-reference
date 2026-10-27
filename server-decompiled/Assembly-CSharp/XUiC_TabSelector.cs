using System;
using UnityEngine.Scripting;

// Token: 0x02000EB4 RID: 3764
[Preserve]
public class XUiC_TabSelector : XUiController
{
	// Token: 0x140000B6 RID: 182
	// (add) Token: 0x06007465 RID: 29797 RVA: 0x002C421C File Offset: 0x002C241C
	// (remove) Token: 0x06007466 RID: 29798 RVA: 0x002C4254 File Offset: 0x002C2454
	public event XUiC_TabSelector.TabChangedDelegate OnTabChanged;

	// Token: 0x17000DB6 RID: 3510
	// (get) Token: 0x06007467 RID: 29799 RVA: 0x002C4289 File Offset: 0x002C2489
	// (set) Token: 0x06007468 RID: 29800 RVA: 0x002C4291 File Offset: 0x002C2491
	[XuiXmlAttribute("use_page_buttons", false)]
	public bool UsePageButtons
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DB7 RID: 3511
	// (get) Token: 0x06007469 RID: 29801 RVA: 0x002C429A File Offset: 0x002C249A
	// (set) Token: 0x0600746A RID: 29802 RVA: 0x002C42A4 File Offset: 0x002C24A4
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

	// Token: 0x17000DB8 RID: 3512
	// (get) Token: 0x0600746B RID: 29803 RVA: 0x002C43DC File Offset: 0x002C25DC
	// (set) Token: 0x0600746C RID: 29804 RVA: 0x002C4410 File Offset: 0x002C2610
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

	// Token: 0x17000DB9 RID: 3513
	// (get) Token: 0x0600746D RID: 29805 RVA: 0x002C443F File Offset: 0x002C263F
	[XuiXmlBinding("selected_tab")]
	public string SelectedTabKey
	{
		get
		{
			XUiC_TabSelectorTab selectedTab = this.SelectedTab;
			return ((selectedTab != null) ? selectedTab.TabKey : null) ?? "";
		}
	}

	// Token: 0x17000DBA RID: 3514
	// (get) Token: 0x0600746E RID: 29806 RVA: 0x002C445C File Offset: 0x002C265C
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

	// Token: 0x0600746F RID: 29807 RVA: 0x002C4470 File Offset: 0x002C2670
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

	// Token: 0x06007470 RID: 29808 RVA: 0x002C45A4 File Offset: 0x002C27A4
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

	// Token: 0x06007471 RID: 29809 RVA: 0x002C45F4 File Offset: 0x002C27F4
	public override void OnClose()
	{
		base.OnClose();
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuCategory);
	}

	// Token: 0x06007472 RID: 29810 RVA: 0x002C4610 File Offset: 0x002C2810
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

	// Token: 0x06007473 RID: 29811 RVA: 0x002C471C File Offset: 0x002C291C
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

	// Token: 0x06007474 RID: 29812 RVA: 0x002C4788 File Offset: 0x002C2988
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

	// Token: 0x06007475 RID: 29813 RVA: 0x002C4899 File Offset: 0x002C2A99
	public void SetTabCaption(int _index, string _name)
	{
		if (_index >= this.Tabs.Length)
		{
			throw new ArgumentOutOfRangeException("_index");
		}
		this.Tabs[_index].TabHeaderText = _name;
	}

	// Token: 0x06007476 RID: 29814 RVA: 0x002C48BF File Offset: 0x002C2ABF
	public XUiC_TabSelectorTab GetTab(int _index)
	{
		if (_index >= this.Tabs.Length)
		{
			throw new ArgumentOutOfRangeException("_index");
		}
		return this.Tabs[_index];
	}

	// Token: 0x06007477 RID: 29815 RVA: 0x002C48E0 File Offset: 0x002C2AE0
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

	// Token: 0x06007478 RID: 29816 RVA: 0x002C4923 File Offset: 0x002C2B23
	public bool IsSelected(string _tabKey)
	{
		XUiC_TabSelectorTab selectedTab = this.SelectedTab;
		return selectedTab != null && selectedTab.TabKey.Equals(_tabKey);
	}

	// Token: 0x06007479 RID: 29817 RVA: 0x002C493C File Offset: 0x002C2B3C
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

	// Token: 0x0600747A RID: 29818 RVA: 0x002C496D File Offset: 0x002C2B6D
	public void TabButtonClicked(XUiC_TabSelectorButton _tabButton)
	{
		if (!this.enabled)
		{
			return;
		}
		this.SelectedTabIndex = Array.IndexOf<XUiC_TabSelectorButton>(this.tabButtons, _tabButton);
	}

	// Token: 0x0600747B RID: 29819 RVA: 0x002C498A File Offset: 0x002C2B8A
	[XuiXmlAttribute("select_tab_contents_on_open", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSelectTabContentsOnOpen(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.selectOnOpen = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x0600747C RID: 29820 RVA: 0x002C49A3 File Offset: 0x002C2BA3
	[XuiXmlAttribute("select_tab_contents_on_change", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeSelectTabContentsOnChange(string _value)
	{
		if (!string.IsNullOrEmpty(_value))
		{
			this.SelectTabContentsOnChange = StringParsers.ParseBool(_value, 0, -1, true);
		}
	}

	// Token: 0x040056E3 RID: 22243
	[XuiBindComponent("tabsHeader.", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelectorButton[] tabButtons;

	// Token: 0x040056E4 RID: 22244
	[XuiBindComponent("tabsContents.", true)]
	public readonly XUiC_TabSelectorTab[] Tabs;

	// Token: 0x040056E5 RID: 22245
	[XuiBindComponent("tabsHeader.", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Table headerButtonsTable;

	// Token: 0x040056E6 RID: 22246
	[XuiBindComponent("border", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite tabSelectorBorder;

	// Token: 0x040056E7 RID: 22247
	[XuiBindComponent("backgroundMainTabs", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Sprite tabHeaderBackground;

	// Token: 0x040056E8 RID: 22248
	[PublicizedFrom(EAccessModifier.Private)]
	public bool enabled = true;

	// Token: 0x040056E9 RID: 22249
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedTabIndex;

	// Token: 0x040056EA RID: 22250
	[PublicizedFrom(EAccessModifier.Private)]
	public bool selectOnOpen;

	// Token: 0x040056EB RID: 22251
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingOnOpenSelection;

	// Token: 0x040056ED RID: 22253
	public bool SelectTabContentsOnChange = true;

	// Token: 0x040056EE RID: 22254
	[PublicizedFrom(EAccessModifier.Private)]
	public bool pendingOnChangeSelection;

	// Token: 0x02000EB5 RID: 3765
	// (Invoke) Token: 0x0600747F RID: 29823
	public delegate void TabChangedDelegate(int _tabIndex, XUiC_TabSelectorTab _tab);
}
