using System;
using System.Collections.Generic;
using InControl;

// Token: 0x02000D98 RID: 3480
public abstract class XUiC_OptionsDialogBase : XUiController, IXUiWindowConditionalClosing
{
	// Token: 0x17000B7D RID: 2941
	// (get) Token: 0x06006A3C RID: 27196 RVA: 0x00299A2D File Offset: 0x00297C2D
	// (set) Token: 0x06006A3D RID: 27197 RVA: 0x00299A35 File Offset: 0x00297C35
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

	// Token: 0x17000B7E RID: 2942
	// (get) Token: 0x06006A3E RID: 27198 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[XuiXmlBinding("support_defaults")]
	public virtual bool SupportsDefaults
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x17000B7F RID: 2943
	// (get) Token: 0x06006A3F RID: 27199 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[XuiXmlBinding("allow_leaving_window")]
	public virtual bool AllowLeavingWindow
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06006A40 RID: 27200 RVA: 0x00299A45 File Offset: 0x00297C45
	[XuiXmlBinding("hovered_custom_attributes")]
	[PublicizedFrom(EAccessModifier.Private)]
	public ObservableDictionary<string, object> bindingCustomAttributes()
	{
		XUiC_OptionEntryAbs xuiC_OptionEntryAbs = this.HoveredOption;
		return ((xuiC_OptionEntryAbs != null) ? xuiC_OptionEntryAbs.CustomAttributes : null) ?? this.CustomAttributes;
	}

	// Token: 0x17000B80 RID: 2944
	// (get) Token: 0x06006A41 RID: 27201 RVA: 0x00299A63 File Offset: 0x00297C63
	// (set) Token: 0x06006A42 RID: 27202 RVA: 0x00299A6B File Offset: 0x00297C6B
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

	// Token: 0x06006A43 RID: 27203 RVA: 0x00299A88 File Offset: 0x00297C88
	public override void Init()
	{
		base.Init();
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
		if (this.TabSelector != null)
		{
			foreach (XUiC_TabSelectorTab xuiC_TabSelectorTab in this.TabSelector.Tabs)
			{
				this.optionsByTab.Add(xuiC_TabSelectorTab, xuiC_TabSelectorTab.GetChildControllers<XUiC_OptionEntryAbs>("", null));
			}
		}
	}

	// Token: 0x06006A44 RID: 27204 RVA: 0x00299AEB File Offset: 0x00297CEB
	public override void Cleanup()
	{
		base.Cleanup();
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
	}

	// Token: 0x06006A45 RID: 27205 RVA: 0x00299B08 File Offset: 0x00297D08
	public override void OnOpen()
	{
		this.unsavedChanges = false;
		base.OnOpen();
		XUi.InGameMenuOpen = true;
		XUiC_OptionsMenuNew.ParentSelector.SetSelected(this.windowGroup.Id);
		this.xui.playerUI.windowManager.Open(XUiC_OptionsMenuNew.ParentSelector.WindowGroup, false);
		this.windowGroup.isEscClosable = false;
	}

	// Token: 0x06006A46 RID: 27206 RVA: 0x00299B6C File Offset: 0x00297D6C
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

	// Token: 0x06006A47 RID: 27207 RVA: 0x00299BC8 File Offset: 0x00297DC8
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

	// Token: 0x06006A48 RID: 27208 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onDirtyUpdate()
	{
	}

	// Token: 0x06006A49 RID: 27209 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnGamePrefChanged(EnumGamePrefs _pref)
	{
	}

	// Token: 0x06006A4A RID: 27210 RVA: 0x00299C60 File Offset: 0x00297E60
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

	// Token: 0x06006A4B RID: 27211 RVA: 0x00299CDF File Offset: 0x00297EDF
	[XuiBindEvent("ValueChanged", "AllOptions")]
	public void Event_SettingChanged(XUiC_OptionEntryAbs _sender)
	{
		XUiC_OptionEntryAbs.DebugLog("SETTING CHANGED: " + _sender.GetXuiHierarchy());
		this.updateUnsavedState();
		base.RefreshBindingsSelfAndChildren();
	}

	// Token: 0x06006A4C RID: 27212 RVA: 0x0008A069 File Offset: 0x00088269
	[XuiBindEvent("OnTabChanged", "TabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.IsDirty = true;
	}

	// Token: 0x06006A4D RID: 27213 RVA: 0x00299D02 File Offset: 0x00297F02
	[XuiBindEvent("OnPress", "btnApply")]
	public void Event_ApplyOnPress(XUiController _sender, int _mouseButton)
	{
		this.saveChanges();
	}

	// Token: 0x06006A4E RID: 27214 RVA: 0x00299D0A File Offset: 0x00297F0A
	[XuiBindEvent("OnPress", "btnDefaults")]
	public void Event_DefaultsOnPress(XUiController _sender, int _mouseButton)
	{
		this.resetToDefaults();
	}

	// Token: 0x06006A4F RID: 27215 RVA: 0x00299D12 File Offset: 0x00297F12
	[XuiBindEvent("OnPress", "btnBack")]
	public void Event_BackOnPress(XUiController _sender, int _mouseButton)
	{
		this.TryClose(null, null);
	}

	// Token: 0x06006A50 RID: 27216 RVA: 0x00299D1C File Offset: 0x00297F1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateUnsavedState()
	{
		bool flag = false;
		XUiC_OptionEntryAbs[] array = this.AllOptions;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].IsChanged)
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
			XUiC_TabSelectorTab xuiC_TabSelectorTab;
			keyValuePair.Deconstruct(out xuiC_TabSelectorTab, out array);
			XUiC_TabSelectorTab xuiC_TabSelectorTab2 = xuiC_TabSelectorTab;
			XUiC_OptionEntryAbs[] array2 = array;
			bool flag2 = false;
			array = array2;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsChanged)
				{
					flag2 = true;
					break;
				}
			}
			xuiC_TabSelectorTab2.TabHighlight = flag2;
		}
	}

	// Token: 0x06006A51 RID: 27217 RVA: 0x00299DD8 File Offset: 0x00297FD8
	public void SetChanged()
	{
		this.UnsavedChanges = true;
	}

	// Token: 0x06006A52 RID: 27218 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[PublicizedFrom(EAccessModifier.Protected)]
	public void closeOptions()
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006A53 RID: 27219 RVA: 0x00299DE4 File Offset: 0x00297FE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void saveChanges()
	{
		Log.Out("Save options " + base.GetType().Name);
		XUiC_OptionEntryAbs[] allOptions = this.AllOptions;
		for (int i = 0; i < allOptions.Length; i++)
		{
			allOptions[i].ApplySelection();
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

	// Token: 0x06006A54 RID: 27220 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doSaveChangesInternal()
	{
	}

	// Token: 0x06006A55 RID: 27221 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void afterChangesSaved()
	{
	}

	// Token: 0x06006A56 RID: 27222 RVA: 0x00299E60 File Offset: 0x00298060
	[PublicizedFrom(EAccessModifier.Private)]
	public void discardChanges()
	{
		Log.Out("Discard changes " + base.GetType().Name);
		XUiC_OptionEntryAbs[] allOptions = this.AllOptions;
		for (int i = 0; i < allOptions.Length; i++)
		{
			allOptions[i].DiscardCurrentChange();
		}
		this.doDiscardChangesInternal();
	}

	// Token: 0x06006A57 RID: 27223 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doDiscardChangesInternal()
	{
	}

	// Token: 0x06006A58 RID: 27224 RVA: 0x00299EAC File Offset: 0x002980AC
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

	// Token: 0x06006A59 RID: 27225 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void doResetToDefaultsInternal()
	{
	}

	// Token: 0x06006A5A RID: 27226 RVA: 0x00299F0B File Offset: 0x0029810B
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_OptionsDialogBase()
	{
	}

	// Token: 0x040050D6 RID: 20694
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<XUiC_TabSelectorTab, XUiC_OptionEntryAbs[]> optionsByTab = new Dictionary<XUiC_TabSelectorTab, XUiC_OptionEntryAbs[]>();

	// Token: 0x040050D7 RID: 20695
	[PublicizedFrom(EAccessModifier.Private)]
	public bool unsavedChanges;

	// Token: 0x040050D8 RID: 20696
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_TabSelector TabSelector;

	// Token: 0x040050D9 RID: 20697
	[XuiBindComponent("buttons.btnApply", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnApply;

	// Token: 0x040050DA RID: 20698
	[XuiBindComponent("buttons.btnDefaults", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x040050DB RID: 20699
	[XuiBindComponent("buttons.btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040050DC RID: 20700
	[XuiBindComponent(false)]
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly XUiC_OptionEntryAbs[] AllOptions;

	// Token: 0x040050DD RID: 20701
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_OptionEntryAbs hoveredOption;
}
