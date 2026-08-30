using System;
using System.Collections.Generic;
using InControl;

// Token: 0x02000DAC RID: 3500
public abstract class XUiC_OptionsDialogBase : XUiController, IXUiWindowConditionalClosing
{
	// Token: 0x17000B95 RID: 2965
	// (get) Token: 0x06006AD6 RID: 27350 RVA: 0x0029CAE1 File Offset: 0x0029ACE1
	// (set) Token: 0x06006AD7 RID: 27351 RVA: 0x0029CAE9 File Offset: 0x0029ACE9
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

	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06006AD8 RID: 27352 RVA: 0x00020021 File Offset: 0x0001E221
	[XuiXmlBinding("support_defaults")]
	public virtual bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x06006AD9 RID: 27353 RVA: 0x00020021 File Offset: 0x0001E221
	[XuiXmlBinding("allow_leaving_window")]
	public virtual bool AllowLeavingWindow
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06006ADA RID: 27354 RVA: 0x0029CAF9 File Offset: 0x0029ACF9
	[XuiXmlBinding("hovered_custom_attributes")]
	[PublicizedFrom(EAccessModifier.Private)]
	public ObservableDictionary<string, object> bindingCustomAttributes()
	{
		XUiC_OptionEntryAbs xuiC_OptionEntryAbs = this.HoveredOption;
		return ((xuiC_OptionEntryAbs != null) ? xuiC_OptionEntryAbs.CustomAttributes : null) ?? this.CustomAttributes;
	}

	// Token: 0x17000B98 RID: 2968
	// (get) Token: 0x06006ADB RID: 27355 RVA: 0x0029CB17 File Offset: 0x0029AD17
	// (set) Token: 0x06006ADC RID: 27356 RVA: 0x0029CB1F File Offset: 0x0029AD1F
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

	// Token: 0x06006ADD RID: 27357 RVA: 0x0029CB3C File Offset: 0x0029AD3C
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

	// Token: 0x06006ADE RID: 27358 RVA: 0x0029CBD2 File Offset: 0x0029ADD2
	public override void Cleanup()
	{
		base.Cleanup();
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
	}

	// Token: 0x06006ADF RID: 27359 RVA: 0x0029CBEC File Offset: 0x0029ADEC
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

	// Token: 0x06006AE0 RID: 27360 RVA: 0x0029CC54 File Offset: 0x0029AE54
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

	// Token: 0x06006AE1 RID: 27361 RVA: 0x0029CCB0 File Offset: 0x0029AEB0
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

	// Token: 0x06006AE2 RID: 27362 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006AE3 RID: 27363 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnGamePrefChanged(EnumGamePrefs _pref)
	{
	}

	// Token: 0x06006AE4 RID: 27364 RVA: 0x0029CD48 File Offset: 0x0029AF48
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

	// Token: 0x06006AE5 RID: 27365 RVA: 0x0029CDC7 File Offset: 0x0029AFC7
	[XuiBindEvent("ValueChanged", "AllOptions")]
	public void Event_SettingChanged(XUiC_OptionEntryAbs _sender)
	{
		XUiC_OptionEntryAbs.DebugLog("SETTING CHANGED: " + _sender.GetXuiHierarchy());
		this.updateUnsavedState();
		base.RefreshBindingsSelfAndChildren();
	}

	// Token: 0x06006AE6 RID: 27366 RVA: 0x0008A715 File Offset: 0x00088915
	[XuiBindEvent("OnTabChanged", "TabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006AE7 RID: 27367 RVA: 0x0029CDEA File Offset: 0x0029AFEA
	[XuiBindEvent("OnPress", "btnApply")]
	public void Event_ApplyOnPress(XUiController _sender, int _mouseButton)
	{
		this.saveChanges();
	}

	// Token: 0x06006AE8 RID: 27368 RVA: 0x0029CDF2 File Offset: 0x0029AFF2
	[XuiBindEvent("OnPress", "btnDefaults")]
	public void Event_DefaultsOnPress(XUiController _sender, int _mouseButton)
	{
		this.resetToDefaults();
	}

	// Token: 0x06006AE9 RID: 27369 RVA: 0x0029CDFA File Offset: 0x0029AFFA
	[XuiBindEvent("OnPress", "btnBack")]
	public void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.TryClose(null, null);
	}

	// Token: 0x06006AEA RID: 27370 RVA: 0x0029CE04 File Offset: 0x0029B004
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

	// Token: 0x06006AEB RID: 27371 RVA: 0x0029CEDC File Offset: 0x0029B0DC
	public void SetChanged()
	{
		this.UnsavedChanges = true;
	}

	// Token: 0x06006AEC RID: 27372 RVA: 0x00089495 File Offset: 0x00087695
	[PublicizedFrom(EAccessModifier.Protected)]
	public void closeOptions()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006AED RID: 27373 RVA: 0x0029CEE8 File Offset: 0x0029B0E8
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

	// Token: 0x06006AEE RID: 27374 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doSaveChangesInternal()
	{
	}

	// Token: 0x06006AEF RID: 27375 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void afterChangesSaved()
	{
	}

	// Token: 0x06006AF0 RID: 27376 RVA: 0x0029CF6C File Offset: 0x0029B16C
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

	// Token: 0x06006AF1 RID: 27377 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doDiscardChangesInternal()
	{
	}

	// Token: 0x06006AF2 RID: 27378 RVA: 0x0029CFC4 File Offset: 0x0029B1C4
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

	// Token: 0x06006AF3 RID: 27379 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doResetToDefaultsInternal()
	{
	}

	// Token: 0x06006AF4 RID: 27380 RVA: 0x0029D024 File Offset: 0x0029B224
	[PublicizedFrom(EAccessModifier.Private)]
	public bool optionAllowed(XUiC_OptionEntryAbs _option)
	{
		XUiC_TabSelectorTab xuiC_TabSelectorTab;
		return _option != null && _option.ViewComponent.IsVisible && (!this.tabByOption.TryGetValue(_option, out xuiC_TabSelectorTab) || xuiC_TabSelectorTab.TabVisible);
	}

	// Token: 0x06006AF5 RID: 27381 RVA: 0x0029D060 File Offset: 0x0029B260
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionsDialogBase()
	{
	}

	// Token: 0x04005177 RID: 20855
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_TabSelectorTab, XUiC_OptionEntryAbs[]> optionsByTab = new Dictionary<XUiC_TabSelectorTab, XUiC_OptionEntryAbs[]>();

	// Token: 0x04005178 RID: 20856
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_OptionEntryAbs, XUiC_TabSelectorTab> tabByOption = new Dictionary<XUiC_OptionEntryAbs, XUiC_TabSelectorTab>();

	// Token: 0x04005179 RID: 20857
	[PublicizedFrom(EAccessModifier.Private)]
	public bool unsavedChanges;

	// Token: 0x0400517A RID: 20858
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_TabSelector TabSelector;

	// Token: 0x0400517B RID: 20859
	[XuiBindComponent("buttons.btnApply", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnApply;

	// Token: 0x0400517C RID: 20860
	[XuiBindComponent("buttons.btnDefaults", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x0400517D RID: 20861
	[XuiBindComponent("buttons.btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x0400517E RID: 20862
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_OptionEntryAbs[] AllOptions;

	// Token: 0x0400517F RID: 20863
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionEntryAbs hoveredOption;
}
