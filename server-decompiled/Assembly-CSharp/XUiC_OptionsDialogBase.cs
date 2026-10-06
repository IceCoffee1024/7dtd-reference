using System;
using System.Collections.Generic;
using InControl;

// Token: 0x02000DBB RID: 3515
public abstract class XUiC_OptionsDialogBase : XUiController, IXUiWindowConditionalClosing
{
	// Token: 0x17000BB0 RID: 2992
	// (get) Token: 0x06006ACE RID: 27342 RVA: 0x0029FE71 File Offset: 0x0029E071
	// (set) Token: 0x06006ACF RID: 27343 RVA: 0x0029FE79 File Offset: 0x0029E079
	[XuiXmlBinding("unsaved_changes")]
	public bool UnsavedChanges
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.unsavedChanges;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.unsavedChanges = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000BB1 RID: 2993
	// (get) Token: 0x06006AD0 RID: 27344 RVA: 0x00020119 File Offset: 0x0001E319
	[XuiXmlBinding("support_defaults")]
	public virtual bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x17000BB2 RID: 2994
	// (get) Token: 0x06006AD1 RID: 27345 RVA: 0x00020119 File Offset: 0x0001E319
	[XuiXmlBinding("allow_leaving_window")]
	public virtual bool AllowLeavingWindow
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06006AD2 RID: 27346 RVA: 0x0029FE89 File Offset: 0x0029E089
	[XuiXmlBinding("hovered_custom_attributes")]
	[PublicizedFrom(EAccessModifier.Private)]
	public ObservableDictionary<string, object> bindingCustomAttributes()
	{
		XUiC_OptionEntryAbs xuiC_OptionEntryAbs = this.HoveredOption;
		return ((xuiC_OptionEntryAbs != null) ? xuiC_OptionEntryAbs.CustomAttributes : null) ?? this.CustomAttributes;
	}

	// Token: 0x17000BB3 RID: 2995
	// (get) Token: 0x06006AD3 RID: 27347 RVA: 0x0029FEA7 File Offset: 0x0029E0A7
	// (set) Token: 0x06006AD4 RID: 27348 RVA: 0x0029FEAF File Offset: 0x0029E0AF
	public XUiC_OptionEntryAbs HoveredOption
	{
		get
		{
			return this.hoveredOption;
		}
		set
		{
			if (this.hoveredOption == value)
			{
				return;
			}
			this.hoveredOption = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06006AD5 RID: 27349 RVA: 0x0029FECC File Offset: 0x0029E0CC
	public override void Init()
	{
		base.Init();
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
		if (this.TabSelector != null)
		{
			foreach (XUiC_TabSelectorTab xuiC_TabSelectorTab in this.TabSelector.Tabs)
			{
				this.optionsByTab.Add(xuiC_TabSelectorTab, xuiC_TabSelectorTab.GetChildControllers<XUiC_OptionEntryAbs>("", null));
				foreach (XUiC_OptionEntryAbs xuiC_OptionEntryAbs in this.optionsByTab[xuiC_TabSelectorTab])
				{
					this.tabByOption[xuiC_OptionEntryAbs] = xuiC_TabSelectorTab;
				}
			}
		}
	}

	// Token: 0x06006AD6 RID: 27350 RVA: 0x0029FF62 File Offset: 0x0029E162
	public override void Cleanup()
	{
		base.Cleanup();
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
	}

	// Token: 0x06006AD7 RID: 27351 RVA: 0x0029FF7C File Offset: 0x0029E17C
	public override void OnOpen()
	{
		this.unsavedChanges = false;
		base.OnOpen();
		XUi.InGameMenuOpen = true;
		XUiC_OptionsMenuNew.ParentSelector.SetSelected(this.windowGroup.Id);
		this.xui.playerUI.windowManager.Open(XUiC_OptionsMenuNew.ParentSelector.WindowGroup, false);
		this.windowGroup.isEscClosable = false;
		this.updateUnsavedState();
	}

	// Token: 0x06006AD8 RID: 27352 RVA: 0x0029FFE4 File Offset: 0x0029E1E4
	public override void OnClose()
	{
		base.OnClose();
		XUi.InGameMenuOpen = false;
		this.xui.playerUI.windowManager.Close(XUiC_OptionsMenuNew.ParentSelector.WindowGroup, false);
		if (GameStats.GetInt(EnumGameStats.GameState) == 0)
		{
			this.xui.playerUI.windowManager.Open(XUiC_MainMenu.ID, true);
		}
	}

	// Token: 0x06006AD9 RID: 27353 RVA: 0x002A0040 File Offset: 0x0029E240
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			this.onDirtyUpdate();
			base.RefreshBindings();
			this.IsDirty = false;
		}
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			if (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
			{
				this.TryClose(null, null);
			}
			if (this.UnsavedChanges && this.xui.playerUI.playerInput.GUIActions.Apply.WasPressed)
			{
				this.saveChanges();
			}
		}
	}

	// Token: 0x06006ADA RID: 27354 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006ADB RID: 27355 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnGamePrefChanged(EnumGamePrefs _pref)
	{
	}

	// Token: 0x06006ADC RID: 27356 RVA: 0x002A00D8 File Offset: 0x0029E2D8
	public virtual void TryClose(Action _onClosed, Action _onCancelled)
	{
		if (this.unsavedChanges)
		{
			Action <>9__1;
			Action <>9__2;
			Action <>9__3;
			XUiC_MessageBoxWindowGroup.ShowCustom(this.xui, Localization.Get("xuiUnsavedChangesTitle", false, null), Localization.Get("xuiOptionsUnsavedChangesText", false, null), "", delegate(XUiC_MessageBoxWindowGroup _box)
			{
				XUiC_MessageBoxWindowGroup.MessageButton messageButton = _box.Buttons[0];
				string text = "xuiDiscard";
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate
					{
						this.closeOptions();
						Action onClosed2 = _onClosed;
						if (onClosed2 == null)
						{
							return;
						}
						onClosed2();
					});
				}
				messageButton.DefaultConfirm(text, action, true, 0f, 0f, false);
				XUiC_MessageBoxWindowGroup.MessageButton messageButton2 = _box.Buttons[1];
				string text2 = "xuiSaveAndClose";
				PlayerAction halfStack = this.xui.playerUI.playerInput.GUIActions.HalfStack;
				Action action2;
				if ((action2 = <>9__2) == null)
				{
					action2 = (<>9__2 = delegate
					{
						this.saveChanges();
						this.closeOptions();
						Action onClosed3 = _onClosed;
						if (onClosed3 == null)
						{
							return;
						}
						onClosed3();
					});
				}
				messageButton2.Set(text2, halfStack, action2, true, 0f, 0f, false);
				XUiC_MessageBoxWindowGroup.MessageButton messageButton3 = _box.Buttons[2];
				string text3 = "xuiCancel";
				Action action3;
				if ((action3 = <>9__3) == null)
				{
					action3 = (<>9__3 = delegate
					{
						Action onCancelled = _onCancelled;
						if (onCancelled == null)
						{
							return;
						}
						onCancelled();
					});
				}
				messageButton3.DefaultCancel(text3, action3, true, 0f, 0f, false);
			}, false, false, -1, -1);
			return;
		}
		this.closeOptions();
		Action onClosed = _onClosed;
		if (onClosed == null)
		{
			return;
		}
		onClosed();
	}

	// Token: 0x06006ADD RID: 27357 RVA: 0x002A0157 File Offset: 0x0029E357
	[XuiBindEvent("ValueChanged", "AllOptions")]
	public void Event_SettingChanged(XUiC_OptionEntryAbs _sender)
	{
		XUiC_OptionEntryAbs.DebugLog("SETTING CHANGED: " + _sender.GetXuiHierarchy());
		this.updateUnsavedState();
		base.RefreshBindingsSelfAndChildren();
	}

	// Token: 0x06006ADE RID: 27358 RVA: 0x0008A995 File Offset: 0x00088B95
	[XuiBindEvent("OnTabChanged", "TabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006ADF RID: 27359 RVA: 0x002A017A File Offset: 0x0029E37A
	[XuiBindEvent("OnPress", "btnApply")]
	public void Event_ApplyOnPress(XUiController _sender, int _mouseButton)
	{
		this.saveChanges();
	}

	// Token: 0x06006AE0 RID: 27360 RVA: 0x002A0182 File Offset: 0x0029E382
	[XuiBindEvent("OnPress", "btnDefaults")]
	public void Event_DefaultsOnPress(XUiController _sender, int _mouseButton)
	{
		this.resetToDefaults();
	}

	// Token: 0x06006AE1 RID: 27361 RVA: 0x002A018A File Offset: 0x0029E38A
	[XuiBindEvent("OnPress", "btnBack")]
	public void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.TryClose(null, null);
	}

	// Token: 0x06006AE2 RID: 27362 RVA: 0x002A0194 File Offset: 0x0029E394
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateUnsavedState()
	{
		bool flag = false;
		foreach (XUiC_OptionEntryAbs xuiC_OptionEntryAbs in this.AllOptions)
		{
			if (this.optionAllowed(xuiC_OptionEntryAbs) && xuiC_OptionEntryAbs.IsChanged)
			{
				flag = true;
				break;
			}
		}
		this.UnsavedChanges = flag;
		if (this.TabSelector == null)
		{
			return;
		}
		foreach (KeyValuePair<XUiC_TabSelectorTab, XUiC_OptionEntryAbs[]> keyValuePair in this.optionsByTab)
		{
			XUiC_OptionEntryAbs[] array;
			XUiC_TabSelectorTab xuiC_TabSelectorTab;
			keyValuePair.Deconstruct(out xuiC_TabSelectorTab, out array);
			XUiC_TabSelectorTab xuiC_TabSelectorTab2 = xuiC_TabSelectorTab;
			XUiC_OptionEntryAbs[] array2 = array;
			bool flag2 = false;
			foreach (XUiC_OptionEntryAbs xuiC_OptionEntryAbs2 in array2)
			{
				if (this.optionAllowed(xuiC_OptionEntryAbs2) && xuiC_OptionEntryAbs2.IsChanged)
				{
					flag2 = true;
					break;
				}
			}
			xuiC_TabSelectorTab2.TabHighlight = flag2;
		}
	}

	// Token: 0x06006AE3 RID: 27363 RVA: 0x002A026C File Offset: 0x0029E46C
	public void SetChanged()
	{
		this.UnsavedChanges = true;
	}

	// Token: 0x06006AE4 RID: 27364 RVA: 0x00089715 File Offset: 0x00087915
	[PublicizedFrom(EAccessModifier.Protected)]
	public void closeOptions()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006AE5 RID: 27365 RVA: 0x002A0278 File Offset: 0x0029E478
	[PublicizedFrom(EAccessModifier.Private)]
	public void saveChanges()
	{
		Log.Out("Save options " + base.GetType().Name);
		foreach (XUiC_OptionEntryAbs xuiC_OptionEntryAbs in this.AllOptions)
		{
			if (this.optionAllowed(xuiC_OptionEntryAbs))
			{
				xuiC_OptionEntryAbs.ApplySelection();
			}
		}
		this.doSaveChangesInternal();
		GamePrefs.Instance.Save();
		this.afterChangesSaved();
		this.updateUnsavedState();
		if (this.unsavedChanges)
		{
			Log.Warning("UnsavedChanges after saving settings!");
		}
		this.IsDirty = true;
	}

	// Token: 0x06006AE6 RID: 27366 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doSaveChangesInternal()
	{
	}

	// Token: 0x06006AE7 RID: 27367 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void afterChangesSaved()
	{
	}

	// Token: 0x06006AE8 RID: 27368 RVA: 0x002A02FC File Offset: 0x0029E4FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void discardChanges()
	{
		Log.Out("Discard changes " + base.GetType().Name);
		foreach (XUiC_OptionEntryAbs xuiC_OptionEntryAbs in this.AllOptions)
		{
			if (this.optionAllowed(xuiC_OptionEntryAbs))
			{
				xuiC_OptionEntryAbs.DiscardCurrentChange();
			}
		}
		this.doDiscardChangesInternal();
	}

	// Token: 0x06006AE9 RID: 27369 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doDiscardChangesInternal()
	{
	}

	// Token: 0x06006AEA RID: 27370 RVA: 0x002A0354 File Offset: 0x0029E554
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetToDefaults()
	{
		Log.Out("Reset to defaults " + base.GetType().Name);
		foreach (XUiC_OptionEntryAbs xuiC_OptionEntryAbs in this.AllOptions)
		{
			if (xuiC_OptionEntryAbs.ViewComponent.IsActiveInHierarchy)
			{
				xuiC_OptionEntryAbs.ResetToDefault();
			}
		}
		this.doResetToDefaultsInternal();
		base.RefreshBindings();
	}

	// Token: 0x06006AEB RID: 27371 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doResetToDefaultsInternal()
	{
	}

	// Token: 0x06006AEC RID: 27372 RVA: 0x002A03B4 File Offset: 0x0029E5B4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool optionAllowed(XUiC_OptionEntryAbs _option)
	{
		XUiC_TabSelectorTab xuiC_TabSelectorTab;
		return _option != null && _option.ViewComponent.IsVisible && (!this.tabByOption.TryGetValue(_option, out xuiC_TabSelectorTab) || xuiC_TabSelectorTab.TabVisible);
	}

	// Token: 0x06006AED RID: 27373 RVA: 0x002A03F0 File Offset: 0x0029E5F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionsDialogBase()
	{
	}

	// Token: 0x040051AE RID: 20910
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_TabSelectorTab, XUiC_OptionEntryAbs[]> optionsByTab = new Dictionary<XUiC_TabSelectorTab, XUiC_OptionEntryAbs[]>();

	// Token: 0x040051AF RID: 20911
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_OptionEntryAbs, XUiC_TabSelectorTab> tabByOption = new Dictionary<XUiC_OptionEntryAbs, XUiC_TabSelectorTab>();

	// Token: 0x040051B0 RID: 20912
	[PublicizedFrom(EAccessModifier.Private)]
	public bool unsavedChanges;

	// Token: 0x040051B1 RID: 20913
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_TabSelector TabSelector;

	// Token: 0x040051B2 RID: 20914
	[XuiBindComponent("buttons.btnApply", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnApply;

	// Token: 0x040051B3 RID: 20915
	[XuiBindComponent("buttons.btnDefaults", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x040051B4 RID: 20916
	[XuiBindComponent("buttons.btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040051B5 RID: 20917
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_OptionEntryAbs[] AllOptions;

	// Token: 0x040051B6 RID: 20918
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionEntryAbs hoveredOption;
}
