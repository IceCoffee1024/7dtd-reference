using System;
using System.Collections.Generic;
using SandboxOptions;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000E24 RID: 3620
[Preserve]
public class XUiC_SandboxOptions : XUiController
{
	// Token: 0x17000C82 RID: 3202
	// (get) Token: 0x06006F45 RID: 28485 RVA: 0x002AED96 File Offset: 0x002ACF96
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

	// Token: 0x17000C83 RID: 3203
	// (get) Token: 0x06006F46 RID: 28486 RVA: 0x002AEDC2 File Offset: 0x002ACFC2
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

	// Token: 0x17000C84 RID: 3204
	// (get) Token: 0x06006F47 RID: 28487 RVA: 0x002AEDDF File Offset: 0x002ACFDF
	[XuiXmlBinding("is_custom")]
	public bool CurrentIsCustom
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentPreset.IsCustomPreset;
		}
	}

	// Token: 0x17000C85 RID: 3205
	// (get) Token: 0x06006F48 RID: 28488 RVA: 0x002AEDEC File Offset: 0x002ACFEC
	[XuiXmlBinding("is_userpreset")]
	public bool CurrentIsUserPreset
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.currentPreset.IsUserPreset;
		}
	}

	// Token: 0x17000C86 RID: 3206
	// (get) Token: 0x06006F49 RID: 28489 RVA: 0x002AEDFC File Offset: 0x002ACFFC
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

	// Token: 0x17000C87 RID: 3207
	// (get) Token: 0x06006F4A RID: 28490 RVA: 0x002AEE64 File Offset: 0x002AD064
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

	// Token: 0x06006F4B RID: 28491 RVA: 0x002AEE89 File Offset: 0x002AD089
	public override void Init()
	{
		base.Init();
		this.setupOptions();
	}

	// Token: 0x06006F4C RID: 28492 RVA: 0x002AEE97 File Offset: 0x002AD097
	[XuiBindEvent("OnTabChanged", "tabSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void TabSelector_OnTabChanged(int _tabIndex, XUiC_TabSelectorTab _tab)
	{
		this.resetTabHeaders();
	}

	// Token: 0x06006F4D RID: 28493 RVA: 0x002AEE9F File Offset: 0x002AD09F
	[XuiBindEvent("OnPress", "btnCopyCode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopyCode_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.updateCode();
		GUIUtility.systemCopyBuffer = this.currentPreset.SandboxCode;
	}

	// Token: 0x06006F4E RID: 28494 RVA: 0x002AEEB7 File Offset: 0x002AD0B7
	[XuiBindEvent("OnPress", "btnPasteCode")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPasteCode_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.sandboxManager.LoadOptionsFromCode(GUIUtility.systemCopyBuffer, SandboxOptionManager.CustomPreset))
		{
			return;
		}
		this.presetSelector.SelectPresetByName("Custom", null, false);
		this.IsDirty = true;
	}

	// Token: 0x06006F4F RID: 28495 RVA: 0x002AEEEA File Offset: 0x002AD0EA
	[XuiBindEvent("OnPress", "btnCopy")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.duplicateCurrentSettingsToCustomPreset();
		this.IsDirty = true;
	}

	// Token: 0x06006F50 RID: 28496 RVA: 0x002AEEF9 File Offset: 0x002AD0F9
	[XuiBindEvent("OnPress", "btnCreatePreset")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCreatePreset_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.sandboxManager.ResetAllToDefault();
		this.sandboxManager.SaveCurrentToPreset(SandboxOptionManager.CustomPreset);
		this.presetSelector.SelectPresetByName("Custom", null, false);
		this.IsDirty = true;
	}

	// Token: 0x06006F51 RID: 28497 RVA: 0x002AEF30 File Offset: 0x002AD130
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

	// Token: 0x06006F52 RID: 28498 RVA: 0x002AEFF4 File Offset: 0x002AD1F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyPresetSettings()
	{
		GamePrefs.Set(EnumGamePrefs.SandboxPreset, this.currentPreset.InternalName);
		GamePrefs.Set(EnumGamePrefs.SandboxCode, this.currentPreset.IsCustomPreset ? this.currentPreset.SandboxCode : "");
	}

	// Token: 0x06006F53 RID: 28499 RVA: 0x002AF034 File Offset: 0x002AD234
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

	// Token: 0x06006F54 RID: 28500 RVA: 0x002AF08C File Offset: 0x002AD28C
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

	// Token: 0x06006F55 RID: 28501 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[XuiBindEvent("OnPress", "btnBack")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBack_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06006F56 RID: 28502 RVA: 0x002AF118 File Offset: 0x002AD318
	[XuiBindEvent("SandboxPresetSelectionChanged", "presetSelector")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void Presets_OnValueChanged(SandboxPresetInfo _oldValue, SandboxPresetInfo _newValue)
	{
		SandboxOptionPreset preset = this.sandboxManager.GetPreset(_newValue.InternalName);
		if (_oldValue.IsCustomPreset)
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

	// Token: 0x06006F57 RID: 28503 RVA: 0x002AF178 File Offset: 0x002AD378
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

	// Token: 0x06006F58 RID: 28504 RVA: 0x002AF1C6 File Offset: 0x002AD3C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void duplicateCurrentSettingsToCustomPreset()
	{
		this.applyEntryChanges(true);
		this.sandboxManager.SaveCurrentToPreset(SandboxOptionManager.CustomPreset);
		this.presetSelector.SelectPresetByName("Custom", null, false);
	}

	// Token: 0x06006F59 RID: 28505 RVA: 0x002AF1F4 File Offset: 0x002AD3F4
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

	// Token: 0x06006F5A RID: 28506 RVA: 0x002AF244 File Offset: 0x002AD444
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
					if (list.Count > childControllers.Length)
					{
						Log.Error(string.Format("[XUi] More sandbox options ({0}) in category '{1}' than XUiC_SandBoxOptionEntries in respective tab ({2})!", list.Count, categoryName, childControllers.Length));
					}
					int j = 0;
					for (int k = 0; k < list.Count; k++)
					{
						if (j >= childControllers.Length)
						{
							IL_0218:
							while (j < childControllers.Length)
							{
								childControllers[j].setupOption(null);
								j++;
							}
							goto IL_0220;
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
					goto IL_0218;
				}
				xuiC_TabSelectorTab.TabVisible = false;
			}
			IL_0220:;
		}
		this.resetTabHeaders();
	}

	// Token: 0x06006F5B RID: 28507 RVA: 0x002AF48C File Offset: 0x002AD68C
	[PublicizedFrom(EAccessModifier.Private)]
	public void applyEntryChanges(bool _forceChange)
	{
		foreach (XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry in this.sandboxOptionDict.Values)
		{
			xuiC_SandBoxOptionEntry.ApplyChange(_forceChange);
		}
	}

	// Token: 0x06006F5C RID: 28508 RVA: 0x002AF4E4 File Offset: 0x002AD6E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void setEntriesDirty()
	{
		foreach (XUiC_SandBoxOptionEntry xuiC_SandBoxOptionEntry in this.sandboxOptionDict.Values)
		{
			xuiC_SandBoxOptionEntry.IsDirty = true;
		}
	}

	// Token: 0x06006F5D RID: 28509 RVA: 0x002AF53C File Offset: 0x002AD73C
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

	// Token: 0x06006F5E RID: 28510 RVA: 0x002AF68C File Offset: 0x002AD88C
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetTabHeaders()
	{
		foreach (XUiC_SandboxOptions.TabChangeHelper tabChangeHelper in this.tabHelpers.Values)
		{
			tabChangeHelper.UpdateTab();
		}
	}

	// Token: 0x06006F5F RID: 28511 RVA: 0x002AF6E4 File Offset: 0x002AD8E4
	public override void OnOpen()
	{
		this.windowGroup.isEscClosable = false;
		string @string = GamePrefs.GetString(EnumGamePrefs.SandboxPreset);
		base.OnOpen();
		this.presetSelector.SelectPresetByName(@string, null, true);
		this.resetTabHeaders();
		this.tabSelector.SelectedTabIndex = 0;
	}

	// Token: 0x06006F60 RID: 28512 RVA: 0x002AF72E File Offset: 0x002AD92E
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

	// Token: 0x06006F61 RID: 28513 RVA: 0x002AF750 File Offset: 0x002AD950
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

	// Token: 0x06006F62 RID: 28514 RVA: 0x002AF7DC File Offset: 0x002AD9DC
	public static void Open(XUi _xui, Action _onClose)
	{
		XUiC_SandboxOptions childByType = _xui.GetChildByType<XUiC_SandboxOptions>();
		childByType.onClose = _onClose;
		_xui.playerUI.windowManager.Open(childByType.windowGroup, false);
	}

	// Token: 0x040053F9 RID: 21497
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelector tabSelector;

	// Token: 0x040053FA RID: 21498
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TabSelectorTab[] tabs;

	// Token: 0x040053FB RID: 21499
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_SandboxPresetSelector presetSelector;

	// Token: 0x040053FC RID: 21500
	[XuiBindComponent("btnSave", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSavePreset;

	// Token: 0x040053FD RID: 21501
	[XuiBindComponent("btnCopyCode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopyCode;

	// Token: 0x040053FE RID: 21502
	[XuiBindComponent("btnCopy", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCopy;

	// Token: 0x040053FF RID: 21503
	[XuiBindComponent("btnCreatePreset", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCreatePreset;

	// Token: 0x04005400 RID: 21504
	[XuiBindComponent("btnDelete", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDelete;

	// Token: 0x04005401 RID: 21505
	[XuiBindComponent("btnPasteCode", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnPasteCode;

	// Token: 0x04005402 RID: 21506
	[XuiBindComponent("btnBack", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnBack;

	// Token: 0x04005403 RID: 21507
	[XuiBindComponent("btnDefaults", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDefaults;

	// Token: 0x04005404 RID: 21508
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<global::SandboxOptions.SandboxOptions, XUiC_SandBoxOptionEntry> sandboxOptionDict = new Dictionary<global::SandboxOptions.SandboxOptions, XUiC_SandBoxOptionEntry>();

	// Token: 0x04005405 RID: 21509
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SandboxOptionManager sandboxManager = SandboxOptionManager.Current;

	// Token: 0x04005406 RID: 21510
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, XUiC_SandboxOptions.TabChangeHelper> tabHelpers = new Dictionary<string, XUiC_SandboxOptions.TabChangeHelper>();

	// Token: 0x04005407 RID: 21511
	[PublicizedFrom(EAccessModifier.Private)]
	public SandboxPresetInfo currentPreset;

	// Token: 0x04005408 RID: 21512
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onClose;

	// Token: 0x02000E25 RID: 3621
	[PublicizedFrom(EAccessModifier.Private)]
	public class TabChangeHelper
	{
		// Token: 0x06006F67 RID: 28519 RVA: 0x002AF8F9 File Offset: 0x002ADAF9
		public TabChangeHelper(XUiC_TabSelectorTab _tab, XUiC_SandBoxOptionEntry[] _tabOptions, string _localizationKey)
		{
			this.tab = _tab;
			this.tabOptions = _tabOptions;
			this.localizationKey = _localizationKey;
		}

		// Token: 0x06006F68 RID: 28520 RVA: 0x002AF918 File Offset: 0x002ADB18
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

		// Token: 0x06006F69 RID: 28521 RVA: 0x002AF94C File Offset: 0x002ADB4C
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

		// Token: 0x06006F6A RID: 28522 RVA: 0x002AF980 File Offset: 0x002ADB80
		public void UpdateTab()
		{
			this.tab.TabHighlight = this.hasAnyNonDefault();
			this.tab.TabHeaderText = Localization.Get(this.localizationKey, false, null) + (this.hasAnyUnsavedChange() ? "[FF]" : "[00]") + "*[FF]";
		}

		// Token: 0x04005409 RID: 21513
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_TabSelectorTab tab;

		// Token: 0x0400540A RID: 21514
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly XUiC_SandBoxOptionEntry[] tabOptions;

		// Token: 0x0400540B RID: 21515
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string localizationKey;
	}
}
