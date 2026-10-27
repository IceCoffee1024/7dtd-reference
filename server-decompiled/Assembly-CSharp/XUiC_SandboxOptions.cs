using System;
using System.Collections.Generic;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E53 RID: 3667
[Preserve]
public class XUiC_SandboxOptions : XUiController
{
	// Token: 0x17000CBC RID: 3260
	// (get) Token: 0x06007002 RID: 28674 RVA: 0x002B6BDA File Offset: 0x002B4DDA
	[XuiXmlBinding("gamename")]
	public string GameName
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			SandboxOptionManager sandboxOptionManager = this.sandboxManager;
			if (!string.IsNullOrEmpty((sandboxOptionManager != null) ? sandboxOptionManager.GameName : null))
			{
				return this.sandboxManager.GameName;
			}
			return "New World";
		}
	}

	// Token: 0x17000CBD RID: 3261
	// (get) Token: 0x06007003 RID: 28675 RVA: 0x002B6C06 File Offset: 0x002B4E06
	[XuiXmlBinding("worldname")]
	public string WorldName
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			SandboxOptionManager sandboxOptionManager = this.sandboxManager;
			return ((sandboxOptionManager != null) ? sandboxOptionManager.WorldName : null) ?? "";
		}
	}

	// Token: 0x17000CBE RID: 3262
	// (get) Token: 0x06007004 RID: 28676 RVA: 0x002B6C23 File Offset: 0x002B4E23
	[XuiXmlBinding("is_custom")]
	public bool CurrentIsCustom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentPreset.IsCustomPreset;
		}
	}

	// Token: 0x17000CBF RID: 3263
	// (get) Token: 0x06007005 RID: 28677 RVA: 0x002B6C30 File Offset: 0x002B4E30
	[XuiXmlBinding("is_userpreset")]
	public bool CurrentIsUserPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentPreset.IsUserPreset;
		}
	}

	// Token: 0x17000CC0 RID: 3264
	// (get) Token: 0x06007006 RID: 28678 RVA: 0x002B6C40 File Offset: 0x002B4E40
	[XuiXmlBinding("has_changes")]
	public bool HasChanges
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			foreach (KeyValuePair<global::SandboxOptions.SandboxOptions, XUiC_SandBoxOptionEntry> keyValuePair in this.sandboxOptionDict)
			{
				global::SandboxOptions.SandboxOptions sandboxOptions;
				XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry;
				keyValuePair.Deconstruct(out sandboxOptions, out xuiC_SandBoxOptionEntry);
				if (xuiC_SandBoxOptionEntry.IsChanged)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x17000CC1 RID: 3265
	// (get) Token: 0x06007007 RID: 28679 RVA: 0x002B6CA8 File Offset: 0x002B4EA8
	[XuiXmlBinding("sandboxcode")]
	public string SandBoxCode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (string.IsNullOrEmpty(this.currentPreset.SandboxCode))
			{
				return "";
			}
			return this.currentPreset.SandboxCode;
		}
	}

	// Token: 0x06007008 RID: 28680 RVA: 0x002B6CCD File Offset: 0x002B4ECD
	public override void Init()
	{
		base.Init();
		this.setupOptions();
	}

	// Token: 0x06007009 RID: 28681 RVA: 0x002B6CDB File Offset: 0x002B4EDB
	[XuiBindEvent("OnTabChanged", "tabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TabSelector_OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.resetTabHeaders();
	}

	// Token: 0x0600700A RID: 28682 RVA: 0x002B6CE3 File Offset: 0x002B4EE3
	[XuiBindEvent("OnPress", "btnCopyCode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopyCode_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.updateCode();
		GUIUtility.systemCopyBuffer = this.currentPreset.SandboxCode;
	}

	// Token: 0x0600700B RID: 28683 RVA: 0x002B6CFB File Offset: 0x002B4EFB
	[XuiBindEvent("OnPress", "btnPasteCode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPasteCode_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.sandboxManager.LoadOptionsFromCode(GUIUtility.systemCopyBuffer, SandboxOptionManager.CustomPreset))
		{
			return;
		}
		this.presetSelector.SelectPresetByName("Custom", null, true);
		this.IsDirty = true;
	}

	// Token: 0x0600700C RID: 28684 RVA: 0x002B6D2E File Offset: 0x002B4F2E
	[XuiBindEvent("OnPress", "btnCopy")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.duplicateCurrentSettingsToCustomPreset();
		this.IsDirty = true;
	}

	// Token: 0x0600700D RID: 28685 RVA: 0x002B6D3D File Offset: 0x002B4F3D
	[XuiBindEvent("OnPress", "btnCreatePreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCreatePreset_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.sandboxManager.ResetAllToDefault();
		this.sandboxManager.SaveCurrentToPreset(SandboxOptionManager.CustomPreset);
		this.presetSelector.SelectPresetByName("Custom", null, false);
		this.IsDirty = true;
	}

	// Token: 0x0600700E RID: 28686 RVA: 0x002B6D74 File Offset: 0x002B4F74
	[XuiBindEvent("OnPress", "btnSavePreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.btnSavePreset.ViewComponent.Enabled)
		{
			return;
		}
		this.applyEntryChanges(true);
		if (this.currentPreset.IsCustomPreset)
		{
			SandboxOptionPreset sandboxOptionPreset = this.sandboxManager.SaveCurrentToNewPreset("tempPreset", "", true);
			XUiC_SandboxSettingsSaveAsPreset.Open(this.xui, sandboxOptionPreset.SandboxCode, "", delegate(string _presetName)
			{
				this.presetSelector.SelectPresetByName(_presetName, null, false);
				this.IsDirty = true;
			});
			return;
		}
		SandboxOptionPreset preset = this.sandboxManager.GetPreset(this.currentPreset.InternalName);
		this.sandboxManager.SaveCurrentToPreset(preset);
		this.sandboxManager.SavePreset(preset, true, true);
		this.updateOptionsToPreset(preset);
		this.currentPreset.SandboxCode = preset.SandboxCode;
		this.applyPresetSettings();
		this.IsDirty = true;
	}

	// Token: 0x0600700F RID: 28687 RVA: 0x002B6E38 File Offset: 0x002B5038
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyPresetSettings()
	{
		GamePrefs.Set(EnumGamePrefs.SandboxPreset, this.currentPreset.InternalName);
		GamePrefs.Set(EnumGamePrefs.SandboxCode, this.currentPreset.IsCustomPreset ? this.currentPreset.SandboxCode : "");
	}

	// Token: 0x06007010 RID: 28688 RVA: 0x002B6E78 File Offset: 0x002B5078
	[XuiBindEvent("OnPress", "btnDelete")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDelete_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_MessageBoxWindowGroup.ShowCustom(this.xui, Localization.Get("xuiDeleteSandboxPreset", false, null), string.Format(Localization.Get("xuiDeleteSandboxConfirmation", false, null), this.currentPreset.FormattedName), "", delegate(XUiC_MessageBoxWindowGroup _box)
		{
			_box.Buttons[0].DefaultConfirm("btnConfirm", delegate
			{
				this.sandboxManager.DeletePreset(this.currentPreset.InternalName);
				this.presetSelector.SelectPresetByName("", "User", false);
				this.IsDirty = true;
			}, true, 0f, 1.5f, false);
			_box.Buttons[2].DefaultCancel("xuiCancel", delegate
			{
			}, true, 0f, 0f, false);
		}, false, false, -1, -1);
	}

	// Token: 0x06007011 RID: 28689 RVA: 0x002B6ED0 File Offset: 0x002B50D0
	[XuiBindEvent("OnPress", "btnDefaults")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDefaults_OnOnPressed(XUiController _sender, int _mouseButton)
	{
		foreach (KeyValuePair<global::SandboxOptions.SandboxOptions, XUiC_SandBoxOptionEntry> keyValuePair in this.sandboxOptionDict)
		{
			global::SandboxOptions.SandboxOptions sandboxOptions;
			XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry;
			keyValuePair.Deconstruct(out sandboxOptions, out xuiC_SandBoxOptionEntry);
			XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry2 = xuiC_SandBoxOptionEntry;
			if (!(xuiC_SandBoxOptionEntry2.Option.CategoryName != this.tabSelector.SelectedTab.TabKey))
			{
				xuiC_SandBoxOptionEntry2.ResetToDefault(false);
			}
		}
		this.IsDirty = true;
	}

	// Token: 0x06007012 RID: 28690 RVA: 0x00089949 File Offset: 0x00087B49
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007013 RID: 28691 RVA: 0x002B6F5C File Offset: 0x002B515C
	[XuiBindEvent("SandboxPresetSelectionChanged", "presetSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Presets_OnValueChanged(SandboxPresetInfo _oldValue, SandboxPresetInfo _newValue)
	{
		SandboxOptionPreset preset = this.sandboxManager.GetPreset(_newValue.InternalName);
		if (_oldValue.IsCustomPreset && !_newValue.IsCustomPreset)
		{
			this.applyEntryChanges(false);
			this.sandboxManager.SaveCurrentToPreset(SandboxOptionManager.CustomPreset);
		}
		this.updateOptionsToPreset(preset);
		this.currentPreset = _newValue;
		this.updateCode();
		this.applyPresetSettings();
		this.IsDirty = true;
	}

	// Token: 0x06007014 RID: 28692 RVA: 0x002B6FC4 File Offset: 0x002B51C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SandboxOptionEntry_OnChangeHandler(XUiC_SandBoxOptionEntry _entry, global::SandboxOptions.SandboxOptions _option)
	{
		if (!this.currentPreset.IsUserPreset && !this.currentPreset.IsCustomPreset)
		{
			XUiC_SandBoxOptionEntry.SandboxOptionValue originalValue = _entry.OriginalValue;
			this.duplicateCurrentSettingsToCustomPreset();
			_entry.ForceOriginalValue(originalValue);
		}
		this.setEntriesDirty();
		this.IsDirty = true;
		this.resetTabHeaders();
	}

	// Token: 0x06007015 RID: 28693 RVA: 0x002B7012 File Offset: 0x002B5212
	[PublicizedFrom(EAccessModifier.Private)]
	public void duplicateCurrentSettingsToCustomPreset()
	{
		this.applyEntryChanges(true);
		this.sandboxManager.SaveCurrentToPreset(SandboxOptionManager.CustomPreset);
		this.presetSelector.SelectPresetByName("Custom", null, false);
	}

	// Token: 0x06007016 RID: 28694 RVA: 0x002B7040 File Offset: 0x002B5240
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCode()
	{
		this.applyEntryChanges(true);
		SandboxOptionPreset sandboxOptionPreset = this.sandboxManager.SaveCurrentToNewPreset("temppreset", "temp", false);
		this.currentPreset.SandboxCode = sandboxOptionPreset.SandboxCode;
		if (this.currentPreset.IsCustomPreset)
		{
			this.applyPresetSettings();
		}
	}

	// Token: 0x06007017 RID: 28695 RVA: 0x002B7090 File Offset: 0x002B5290
	[PublicizedFrom(EAccessModifier.Private)]
	public void setupOptions()
	{
		DictionaryList<string, List<BaseSandboxOption>> optionsByCategory = this.sandboxManager.OptionsByCategory;
		for (int i = 0; i < this.tabs.Length; i++)
		{
			XUiC_TabSelectorTab xuiC_TabSelectorTab = this.tabs[i];
			if (i >= optionsByCategory.Count)
			{
				xuiC_TabSelectorTab.TabVisible = false;
			}
			else
			{
				List<BaseSandboxOption> list = optionsByCategory.list[i];
				if (list.Count != 0)
				{
					string categoryName = list[0].CategoryName;
					xuiC_TabSelectorTab.TabKey = categoryName;
					XUiC_SandBoxOptionEntry[] childControllers = xuiC_TabSelectorTab.GetChildControllers<XUiC_SandBoxOptionEntry>("", null);
					this.tabHelpers.Add(categoryName, new XUiC_SandboxOptions.TabChangeHelper(xuiC_TabSelectorTab, childControllers, "sandboxOptionCategory" + categoryName));
					int num = -1;
					XUiV_Grid xuiV_Grid;
					if (xuiC_TabSelectorTab.TryGetChildView<XUiV_Grid>("", out xuiV_Grid) && xuiV_Grid.Arrangement == UIGrid.Arrangement.Horizontal)
					{
						num = xuiV_Grid.Columns;
					}
					int j = 0;
					for (int k = 0; k < list.Count; k++)
					{
						if (j >= childControllers.Length)
						{
							Log.Error(string.Format("[XUi] More sandbox options ({0}) + separators in category '{1}' than XUiC_SandBoxOptionEntries in respective tab ({2})!", list.Count, categoryName, childControllers.Length));
							IL_020E:
							while (j < childControllers.Length)
							{
								childControllers[j].setupOption(null);
								j++;
							}
							goto IL_0216;
						}
						BaseSandboxOption baseSandboxOption = list[k];
						if (baseSandboxOption.NewUISection)
						{
							if (num <= 0)
							{
								childControllers[j++].setupSeparator();
							}
							else
							{
								int num2 = j % num;
								if (num2 > 0)
								{
									int num3 = num - num2;
									for (int l = 0; l < num3; l++)
									{
										childControllers[j++].setupSeparator();
									}
								}
								for (int m = 0; m < num; m++)
								{
									childControllers[j++].setupSeparator();
								}
							}
						}
						XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry = childControllers[j++];
						XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry2 = xuiC_SandBoxOptionEntry;
						xuiC_SandBoxOptionEntry2.OnValueChanged = (Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>)Delegate.Remove(xuiC_SandBoxOptionEntry2.OnValueChanged, new Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>(this.SandboxOptionEntry_OnChangeHandler));
						xuiC_SandBoxOptionEntry.setupOption(baseSandboxOption);
						XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry3 = xuiC_SandBoxOptionEntry;
						xuiC_SandBoxOptionEntry3.OnValueChanged = (Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>)Delegate.Combine(xuiC_SandBoxOptionEntry3.OnValueChanged, new Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>(this.SandboxOptionEntry_OnChangeHandler));
						this.sandboxOptionDict.Add(xuiC_SandBoxOptionEntry.Option.Option, xuiC_SandBoxOptionEntry);
					}
					goto IL_020E;
				}
				xuiC_TabSelectorTab.TabVisible = false;
			}
			IL_0216:;
		}
		this.resetTabHeaders();
	}

	// Token: 0x06007018 RID: 28696 RVA: 0x002B72CC File Offset: 0x002B54CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyEntryChanges(bool _forceChange)
	{
		foreach (XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry in this.sandboxOptionDict.Values)
		{
			xuiC_SandBoxOptionEntry.ApplyChange(_forceChange);
		}
	}

	// Token: 0x06007019 RID: 28697 RVA: 0x002B7324 File Offset: 0x002B5524
	[PublicizedFrom(EAccessModifier.Private)]
	public void setEntriesDirty()
	{
		foreach (XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry in this.sandboxOptionDict.Values)
		{
			xuiC_SandBoxOptionEntry.IsDirty = true;
		}
	}

	// Token: 0x0600701A RID: 28698 RVA: 0x002B737C File Offset: 0x002B557C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateOptionsToPreset(SandboxOptionPreset _preset)
	{
		foreach (XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry in this.sandboxOptionDict.Values)
		{
			xuiC_SandBoxOptionEntry.OnValueChanged = (Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>)Delegate.Remove(xuiC_SandBoxOptionEntry.OnValueChanged, new Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>(this.SandboxOptionEntry_OnChangeHandler));
			xuiC_SandBoxOptionEntry.ResetToDefault(true);
			xuiC_SandBoxOptionEntry.OnValueChanged = (Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>)Delegate.Combine(xuiC_SandBoxOptionEntry.OnValueChanged, new Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>(this.SandboxOptionEntry_OnChangeHandler));
		}
		foreach (global::SandboxOptions.SandboxOptions sandboxOptions in _preset.PresetValues.Keys)
		{
			XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry2 = this.sandboxOptionDict[sandboxOptions];
			xuiC_SandBoxOptionEntry2.OnValueChanged = (Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>)Delegate.Remove(xuiC_SandBoxOptionEntry2.OnValueChanged, new Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>(this.SandboxOptionEntry_OnChangeHandler));
			xuiC_SandBoxOptionEntry2.SetComboBoxIndex(_preset.PresetValues[sandboxOptions]);
			xuiC_SandBoxOptionEntry2.ApplyChange(true);
			xuiC_SandBoxOptionEntry2.OnValueChanged = (Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>)Delegate.Combine(xuiC_SandBoxOptionEntry2.OnValueChanged, new Action<XUiC_SandBoxOptionEntry, global::SandboxOptions.SandboxOptions>(this.SandboxOptionEntry_OnChangeHandler));
		}
		this.resetTabHeaders();
		this.IsDirty = true;
	}

	// Token: 0x0600701B RID: 28699 RVA: 0x002B74CC File Offset: 0x002B56CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetTabHeaders()
	{
		foreach (XUiC_SandboxOptions.TabChangeHelper tabChangeHelper in this.tabHelpers.Values)
		{
			tabChangeHelper.UpdateTab();
		}
	}

	// Token: 0x0600701C RID: 28700 RVA: 0x002B7524 File Offset: 0x002B5724
	public override void OnOpen()
	{
		this.windowGroup.isEscClosable = false;
		string @string = GamePrefs.GetString(EnumGamePrefs.SandboxPreset);
		base.OnOpen();
		this.presetSelector.SelectPresetByName(@string, null, true);
		this.resetTabHeaders();
		this.tabSelector.SelectedTabIndex = 0;
	}

	// Token: 0x0600701D RID: 28701 RVA: 0x002B756E File Offset: 0x002B576E
	public override void OnClose()
	{
		base.OnClose();
		Action action = this.onClose;
		if (action != null)
		{
			action();
		}
		this.onClose = null;
	}

	// Token: 0x0600701E RID: 28702 RVA: 0x002B7590 File Offset: 0x002B5790
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent, false))
		{
			if (this.xui.playerUI.playerInput.PermanentActions.Cancel.WasReleased)
			{
				this.BtnBack_OnPressed(this, -1);
			}
			if (this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
			{
				this.BtnSave_OnPressed(null, 0);
			}
		}
		if (this.IsDirty)
		{
			this.updateCode();
		}
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600701F RID: 28703 RVA: 0x002B761C File Offset: 0x002B581C
	public static void Open(XUi _xui, Action _onClose)
	{
		XUiC_SandboxOptions childByType = _xui.GetChildByType<XUiC_SandboxOptions>();
		childByType.onClose = _onClose;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040054F9 RID: 21753
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector tabSelector;

	// Token: 0x040054FA RID: 21754
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelectorTab[] tabs;

	// Token: 0x040054FB RID: 21755
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SandboxPresetSelector presetSelector;

	// Token: 0x040054FC RID: 21756
	[XuiBindComponent("btnSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSavePreset;

	// Token: 0x040054FD RID: 21757
	[XuiBindComponent("btnCopyCode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopyCode;

	// Token: 0x040054FE RID: 21758
	[XuiBindComponent("btnCopy", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopy;

	// Token: 0x040054FF RID: 21759
	[XuiBindComponent("btnCreatePreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCreatePreset;

	// Token: 0x04005500 RID: 21760
	[XuiBindComponent("btnDelete", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDelete;

	// Token: 0x04005501 RID: 21761
	[XuiBindComponent("btnPasteCode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnPasteCode;

	// Token: 0x04005502 RID: 21762
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x04005503 RID: 21763
	[XuiBindComponent("btnDefaults", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x04005504 RID: 21764
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<global::SandboxOptions.SandboxOptions, XUiC_SandBoxOptionEntry> sandboxOptionDict = new Dictionary<global::SandboxOptions.SandboxOptions, XUiC_SandBoxOptionEntry>();

	// Token: 0x04005505 RID: 21765
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005506 RID: 21766
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_SandboxOptions.TabChangeHelper> tabHelpers = new Dictionary<string, XUiC_SandboxOptions.TabChangeHelper>();

	// Token: 0x04005507 RID: 21767
	[PublicizedFrom(EAccessModifier.Private)]
	public SandboxPresetInfo currentPreset;

	// Token: 0x04005508 RID: 21768
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClose;

	// Token: 0x02000E54 RID: 3668
	[PublicizedFrom(EAccessModifier.Private)]
	public class TabChangeHelper
	{
		// Token: 0x06007024 RID: 28708 RVA: 0x002B7739 File Offset: 0x002B5939
		public TabChangeHelper(XUiC_TabSelectorTab _tab, XUiC_SandBoxOptionEntry[] _tabOptions, string _localizationKey)
		{
			this.tab = _tab;
			this.tabOptions = _tabOptions;
			this.localizationKey = _localizationKey;
		}

		// Token: 0x06007025 RID: 28709 RVA: 0x002B7758 File Offset: 0x002B5958
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasAnyNonDefault()
		{
			for (int i = 0; i < this.tabOptions.Length; i++)
			{
				if (!this.tabOptions[i].IsDefaultValue())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007026 RID: 28710 RVA: 0x002B778C File Offset: 0x002B598C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasAnyUnsavedChange()
		{
			for (int i = 0; i < this.tabOptions.Length; i++)
			{
				if (this.tabOptions[i].IsChanged)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007027 RID: 28711 RVA: 0x002B77C0 File Offset: 0x002B59C0
		public void UpdateTab()
		{
			this.tab.TabHighlight = this.hasAnyNonDefault();
			this.tab.TabHeaderText = Localization.Get(this.localizationKey, false, null) + (this.hasAnyUnsavedChange() ? "[FF]" : "[00]") + "*[FF]";
		}

		// Token: 0x04005509 RID: 21769
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_TabSelectorTab tab;

		// Token: 0x0400550A RID: 21770
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_SandBoxOptionEntry[] tabOptions;

		// Token: 0x0400550B RID: 21771
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string localizationKey;
	}
}
