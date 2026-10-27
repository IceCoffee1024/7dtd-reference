using System;
using System.Collections.Generic;
using InControl;

// Token: 0x02000DC6 RID: 3526
public abstract class XUiC_OptionsDialogBase : XUiController, IXUiWindowConditionalClosing
{
	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x06006AF3 RID: 27379 RVA: 0x002A1399 File Offset: 0x0029F599
	// (set) Token: 0x06006AF4 RID: 27380 RVA: 0x002A13A1 File Offset: 0x0029F5A1
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

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x06006AF5 RID: 27381 RVA: 0x00020119 File Offset: 0x0001E319
	[XuiXmlBinding("support_defaults")]
	public virtual bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x17000BB8 RID: 3000
	// (get) Token: 0x06006AF6 RID: 27382 RVA: 0x00020119 File Offset: 0x0001E319
	[XuiXmlBinding("allow_leaving_window")]
	public virtual bool AllowLeavingWindow
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06006AF7 RID: 27383 RVA: 0x002A13B1 File Offset: 0x0029F5B1
	[XuiXmlBinding("hovered_custom_attributes")]
	[PublicizedFrom(EAccessModifier.Private)]
	public ObservableDictionary<string, object> bindingCustomAttributes()
	{
		XUiC_OptionEntryAbs xuiC_OptionEntryAbs = this.HoveredOption;
		return ((xuiC_OptionEntryAbs != null) ? xuiC_OptionEntryAbs.CustomAttributes : null) ?? this.CustomAttributes;
	}

	// Token: 0x17000BB9 RID: 3001
	// (get) Token: 0x06006AF8 RID: 27384 RVA: 0x002A13CF File Offset: 0x0029F5CF
	// (set) Token: 0x06006AF9 RID: 27385 RVA: 0x002A13D7 File Offset: 0x0029F5D7
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

	// Token: 0x06006AFA RID: 27386 RVA: 0x002A13F4 File Offset: 0x0029F5F4
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

	// Token: 0x06006AFB RID: 27387 RVA: 0x002A148A File Offset: 0x0029F68A
	public override void Cleanup()
	{
		base.Cleanup();
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
	}

	// Token: 0x06006AFC RID: 27388 RVA: 0x002A14A4 File Offset: 0x0029F6A4
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

	// Token: 0x06006AFD RID: 27389 RVA: 0x002A150C File Offset: 0x0029F70C
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

	// Token: 0x06006AFE RID: 27390 RVA: 0x002A1568 File Offset: 0x0029F768
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

	// Token: 0x06006AFF RID: 27391 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006B00 RID: 27392 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnGamePrefChanged(EnumGamePrefs _pref)
	{
	}

	// Token: 0x06006B01 RID: 27393 RVA: 0x002A1600 File Offset: 0x0029F800
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

	// Token: 0x06006B02 RID: 27394 RVA: 0x002A167F File Offset: 0x0029F87F
	[XuiBindEvent("ValueChanged", "AllOptions")]
	public void Event_SettingChanged(XUiC_OptionEntryAbs _sender)
	{
		XUiC_OptionEntryAbs.DebugLog("SETTING CHANGED: " + _sender.GetXuiHierarchy());
		this.updateUnsavedState();
		base.RefreshBindingsSelfAndChildren();
	}

	// Token: 0x06006B03 RID: 27395 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[XuiBindEvent("OnTabChanged", "TabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006B04 RID: 27396 RVA: 0x002A16A2 File Offset: 0x0029F8A2
	[XuiBindEvent("OnPress", "btnApply")]
	public void Event_ApplyOnPress(XUiController _sender, int _mouseButton)
	{
		this.saveChanges();
	}

	// Token: 0x06006B05 RID: 27397 RVA: 0x002A16AA File Offset: 0x0029F8AA
	[XuiBindEvent("OnPress", "btnDefaults")]
	public void Event_DefaultsOnPress(XUiController _sender, int _mouseButton)
	{
		this.resetToDefaults();
	}

	// Token: 0x06006B06 RID: 27398 RVA: 0x002A16B2 File Offset: 0x0029F8B2
	[XuiBindEvent("OnPress", "btnBack")]
	public void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.TryClose(null, null);
	}

	// Token: 0x06006B07 RID: 27399 RVA: 0x002A16BC File Offset: 0x0029F8BC
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

	// Token: 0x06006B08 RID: 27400 RVA: 0x002A1794 File Offset: 0x0029F994
	public void SetChanged()
	{
		this.UnsavedChanges = true;
	}

	// Token: 0x06006B09 RID: 27401 RVA: 0x00089949 File Offset: 0x00087B49
	[PublicizedFrom(EAccessModifier.Protected)]
	public void closeOptions()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006B0A RID: 27402 RVA: 0x002A17A0 File Offset: 0x0029F9A0
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

	// Token: 0x06006B0B RID: 27403 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doSaveChangesInternal()
	{
	}

	// Token: 0x06006B0C RID: 27404 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void afterChangesSaved()
	{
	}

	// Token: 0x06006B0D RID: 27405 RVA: 0x002A1824 File Offset: 0x0029FA24
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

	// Token: 0x06006B0E RID: 27406 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doDiscardChangesInternal()
	{
	}

	// Token: 0x06006B0F RID: 27407 RVA: 0x002A187C File Offset: 0x0029FA7C
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

	// Token: 0x06006B10 RID: 27408 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doResetToDefaultsInternal()
	{
	}

	// Token: 0x06006B11 RID: 27409 RVA: 0x002A18DC File Offset: 0x0029FADC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool optionAllowed(XUiC_OptionEntryAbs _option)
	{
		XUiC_TabSelectorTab xuiC_TabSelectorTab;
		return _option != null && _option.ViewComponent.IsVisible && (!this.tabByOption.TryGetValue(_option, out xuiC_TabSelectorTab) || xuiC_TabSelectorTab.TabVisible);
	}

	// Token: 0x06006B12 RID: 27410 RVA: 0x002A1918 File Offset: 0x0029FB18
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionsDialogBase()
	{
	}

	// Token: 0x040051D3 RID: 20947
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_TabSelectorTab, XUiC_OptionEntryAbs[]> optionsByTab = new Dictionary<XUiC_TabSelectorTab, XUiC_OptionEntryAbs[]>();

	// Token: 0x040051D4 RID: 20948
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_OptionEntryAbs, XUiC_TabSelectorTab> tabByOption = new Dictionary<XUiC_OptionEntryAbs, XUiC_TabSelectorTab>();

	// Token: 0x040051D5 RID: 20949
	[PublicizedFrom(EAccessModifier.Private)]
	public bool unsavedChanges;

	// Token: 0x040051D6 RID: 20950
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_TabSelector TabSelector;

	// Token: 0x040051D7 RID: 20951
	[XuiBindComponent("buttons.btnApply", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnApply;

	// Token: 0x040051D8 RID: 20952
	[XuiBindComponent("buttons.btnDefaults", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x040051D9 RID: 20953
	[XuiBindComponent("buttons.btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040051DA RID: 20954
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_OptionEntryAbs[] AllOptions;

	// Token: 0x040051DB RID: 20955
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionEntryAbs hoveredOption;
}
