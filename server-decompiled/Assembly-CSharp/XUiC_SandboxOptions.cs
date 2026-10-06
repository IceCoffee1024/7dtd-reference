using System;
using System.Collections.Generic;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E48 RID: 3656
[Preserve]
public class XUiC_SandboxOptions : XUiController
{
	// Token: 0x17000CB6 RID: 3254
	// (get) Token: 0x06006FDD RID: 28637 RVA: 0x002B566E File Offset: 0x002B386E
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

	// Token: 0x17000CB7 RID: 3255
	// (get) Token: 0x06006FDE RID: 28638 RVA: 0x002B569A File Offset: 0x002B389A
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

	// Token: 0x17000CB8 RID: 3256
	// (get) Token: 0x06006FDF RID: 28639 RVA: 0x002B56B7 File Offset: 0x002B38B7
	[XuiXmlBinding("is_custom")]
	public bool CurrentIsCustom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentPreset.IsCustomPreset;
		}
	}

	// Token: 0x17000CB9 RID: 3257
	// (get) Token: 0x06006FE0 RID: 28640 RVA: 0x002B56C4 File Offset: 0x002B38C4
	[XuiXmlBinding("is_userpreset")]
	public bool CurrentIsUserPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentPreset.IsUserPreset;
		}
	}

	// Token: 0x17000CBA RID: 3258
	// (get) Token: 0x06006FE1 RID: 28641 RVA: 0x002B56D4 File Offset: 0x002B38D4
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

	// Token: 0x17000CBB RID: 3259
	// (get) Token: 0x06006FE2 RID: 28642 RVA: 0x002B573C File Offset: 0x002B393C
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

	// Token: 0x06006FE3 RID: 28643 RVA: 0x002B5761 File Offset: 0x002B3961
	public override void Init()
	{
		base.Init();
		this.setupOptions();
	}

	// Token: 0x06006FE4 RID: 28644 RVA: 0x002B576F File Offset: 0x002B396F
	[XuiBindEvent("OnTabChanged", "tabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TabSelector_OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.resetTabHeaders();
	}

	// Token: 0x06006FE5 RID: 28645 RVA: 0x002B5777 File Offset: 0x002B3977
	[XuiBindEvent("OnPress", "btnCopyCode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopyCode_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.updateCode();
		GUIUtility.systemCopyBuffer = this.currentPreset.SandboxCode;
	}

	// Token: 0x06006FE6 RID: 28646 RVA: 0x002B578F File Offset: 0x002B398F
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

	// Token: 0x06006FE7 RID: 28647 RVA: 0x002B57C2 File Offset: 0x002B39C2
	[XuiBindEvent("OnPress", "btnCopy")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.duplicateCurrentSettingsToCustomPreset();
		this.IsDirty = true;
	}

	// Token: 0x06006FE8 RID: 28648 RVA: 0x002B57D1 File Offset: 0x002B39D1
	[XuiBindEvent("OnPress", "btnCreatePreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCreatePreset_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.sandboxManager.ResetAllToDefault();
		this.sandboxManager.SaveCurrentToPreset(SandboxOptionManager.CustomPreset);
		this.presetSelector.SelectPresetByName("Custom", null, false);
		this.IsDirty = true;
	}

	// Token: 0x06006FE9 RID: 28649 RVA: 0x002B5808 File Offset: 0x002B3A08
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

	// Token: 0x06006FEA RID: 28650 RVA: 0x002B58CC File Offset: 0x002B3ACC
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyPresetSettings()
	{
		GamePrefs.Set(EnumGamePrefs.SandboxPreset, this.currentPreset.InternalName);
		GamePrefs.Set(EnumGamePrefs.SandboxCode, this.currentPreset.IsCustomPreset ? this.currentPreset.SandboxCode : "");
	}

	// Token: 0x06006FEB RID: 28651 RVA: 0x002B590C File Offset: 0x002B3B0C
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

	// Token: 0x06006FEC RID: 28652 RVA: 0x002B5964 File Offset: 0x002B3B64
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

	// Token: 0x06006FED RID: 28653 RVA: 0x00089715 File Offset: 0x00087915
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006FEE RID: 28654 RVA: 0x002B59F0 File Offset: 0x002B3BF0
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

	// Token: 0x06006FEF RID: 28655 RVA: 0x002B5A58 File Offset: 0x002B3C58
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

	// Token: 0x06006FF0 RID: 28656 RVA: 0x002B5AA6 File Offset: 0x002B3CA6
	[PublicizedFrom(EAccessModifier.Private)]
	public void duplicateCurrentSettingsToCustomPreset()
	{
		this.applyEntryChanges(true);
		this.sandboxManager.SaveCurrentToPreset(SandboxOptionManager.CustomPreset);
		this.presetSelector.SelectPresetByName("Custom", null, false);
	}

	// Token: 0x06006FF1 RID: 28657 RVA: 0x002B5AD4 File Offset: 0x002B3CD4
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

	// Token: 0x06006FF2 RID: 28658 RVA: 0x002B5B24 File Offset: 0x002B3D24
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

	// Token: 0x06006FF3 RID: 28659 RVA: 0x002B5D60 File Offset: 0x002B3F60
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyEntryChanges(bool _forceChange)
	{
		foreach (XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry in this.sandboxOptionDict.Values)
		{
			xuiC_SandBoxOptionEntry.ApplyChange(_forceChange);
		}
	}

	// Token: 0x06006FF4 RID: 28660 RVA: 0x002B5DB8 File Offset: 0x002B3FB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void setEntriesDirty()
	{
		foreach (XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry in this.sandboxOptionDict.Values)
		{
			xuiC_SandBoxOptionEntry.IsDirty = true;
		}
	}

	// Token: 0x06006FF5 RID: 28661 RVA: 0x002B5E10 File Offset: 0x002B4010
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

	// Token: 0x06006FF6 RID: 28662 RVA: 0x002B5F60 File Offset: 0x002B4160
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetTabHeaders()
	{
		foreach (XUiC_SandboxOptions.TabChangeHelper tabChangeHelper in this.tabHelpers.Values)
		{
			tabChangeHelper.UpdateTab();
		}
	}

	// Token: 0x06006FF7 RID: 28663 RVA: 0x002B5FB8 File Offset: 0x002B41B8
	public override void OnOpen()
	{
		this.windowGroup.isEscClosable = false;
		string @string = GamePrefs.GetString(EnumGamePrefs.SandboxPreset);
		base.OnOpen();
		this.presetSelector.SelectPresetByName(@string, null, true);
		this.resetTabHeaders();
		this.tabSelector.SelectedTabIndex = 0;
	}

	// Token: 0x06006FF8 RID: 28664 RVA: 0x002B6002 File Offset: 0x002B4202
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

	// Token: 0x06006FF9 RID: 28665 RVA: 0x002B6024 File Offset: 0x002B4224
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

	// Token: 0x06006FFA RID: 28666 RVA: 0x002B60B0 File Offset: 0x002B42B0
	public static void Open(XUi _xui, Action _onClose)
	{
		XUiC_SandboxOptions childByType = _xui.GetChildByType<XUiC_SandboxOptions>();
		childByType.onClose = _onClose;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040054D4 RID: 21716
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector tabSelector;

	// Token: 0x040054D5 RID: 21717
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelectorTab[] tabs;

	// Token: 0x040054D6 RID: 21718
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SandboxPresetSelector presetSelector;

	// Token: 0x040054D7 RID: 21719
	[XuiBindComponent("btnSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSavePreset;

	// Token: 0x040054D8 RID: 21720
	[XuiBindComponent("btnCopyCode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopyCode;

	// Token: 0x040054D9 RID: 21721
	[XuiBindComponent("btnCopy", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopy;

	// Token: 0x040054DA RID: 21722
	[XuiBindComponent("btnCreatePreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCreatePreset;

	// Token: 0x040054DB RID: 21723
	[XuiBindComponent("btnDelete", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDelete;

	// Token: 0x040054DC RID: 21724
	[XuiBindComponent("btnPasteCode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnPasteCode;

	// Token: 0x040054DD RID: 21725
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x040054DE RID: 21726
	[XuiBindComponent("btnDefaults", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x040054DF RID: 21727
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<global::SandboxOptions.SandboxOptions, XUiC_SandBoxOptionEntry> sandboxOptionDict = new Dictionary<global::SandboxOptions.SandboxOptions, XUiC_SandBoxOptionEntry>();

	// Token: 0x040054E0 RID: 21728
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x040054E1 RID: 21729
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_SandboxOptions.TabChangeHelper> tabHelpers = new Dictionary<string, XUiC_SandboxOptions.TabChangeHelper>();

	// Token: 0x040054E2 RID: 21730
	[PublicizedFrom(EAccessModifier.Private)]
	public SandboxPresetInfo currentPreset;

	// Token: 0x040054E3 RID: 21731
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClose;

	// Token: 0x02000E49 RID: 3657
	[PublicizedFrom(EAccessModifier.Private)]
	public class TabChangeHelper
	{
		// Token: 0x06006FFF RID: 28671 RVA: 0x002B61CD File Offset: 0x002B43CD
		public TabChangeHelper(XUiC_TabSelectorTab _tab, XUiC_SandBoxOptionEntry[] _tabOptions, string _localizationKey)
		{
			this.tab = _tab;
			this.tabOptions = _tabOptions;
			this.localizationKey = _localizationKey;
		}

		// Token: 0x06007000 RID: 28672 RVA: 0x002B61EC File Offset: 0x002B43EC
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

		// Token: 0x06007001 RID: 28673 RVA: 0x002B6220 File Offset: 0x002B4420
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

		// Token: 0x06007002 RID: 28674 RVA: 0x002B6254 File Offset: 0x002B4454
		public void UpdateTab()
		{
			this.tab.TabHighlight = this.hasAnyNonDefault();
			this.tab.TabHeaderText = Localization.Get(this.localizationKey, false, null) + (this.hasAnyUnsavedChange() ? "[FF]" : "[00]") + "*[FF]";
		}

		// Token: 0x040054E4 RID: 21732
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_TabSelectorTab tab;

		// Token: 0x040054E5 RID: 21733
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_SandBoxOptionEntry[] tabOptions;

		// Token: 0x040054E6 RID: 21734
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string localizationKey;
	}
}
