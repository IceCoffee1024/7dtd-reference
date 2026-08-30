using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010A8 RID: 4264
[Preserve]
public class XUiC_WoPropsSleeperVolume : XUiController, ISelectionBoxCallback, ISelectionCategoryCallback
{
	// Token: 0x17000FB2 RID: 4018
	// (get) Token: 0x060087BF RID: 34751 RVA: 0x0033E5B8 File Offset: 0x0033C7B8
	// (set) Token: 0x060087C0 RID: 34752 RVA: 0x0033E5BF File Offset: 0x0033C7BF
	public static XUiC_WoPropsSleeperVolume Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FB3 RID: 4019
	// (get) Token: 0x060087C1 RID: 34753 RVA: 0x0033E5C8 File Offset: 0x0033C7C8
	public List<byte> TriggeredByIndices
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			SelectionBox selectionBox;
			PrefabSleeperVolume prefabSleeperVolume;
			PrefabInstance prefabInstance;
			int num;
			if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num))
			{
				return null;
			}
			return prefabSleeperVolume.triggeredByIndices;
		}
	}

	// Token: 0x060087C2 RID: 34754 RVA: 0x0033E5F8 File Offset: 0x0033C7F8
	public override void Init()
	{
		base.Init();
		XUiC_WoPropsSleeperVolume.ID = base.WindowGroup.Id;
		XUiC_WoPropsSleeperVolume.Instance = this;
		this.labelIndex = base.GetChildById("labelIndex").ViewComponent as XUiV_Label;
		this.labelPosition = base.GetChildById("labelPosition").ViewComponent as XUiV_Label;
		this.labelSize = base.GetChildById("labelSize").ViewComponent as XUiV_Label;
		this.labelSleeperCount = base.GetChildById("labelSleeperCount").ViewComponent as XUiV_Label;
		this.labelGroup = base.GetChildById("labelGroup").ViewComponent as XUiV_Label;
		this.txtGroupId = (XUiC_TextInput)base.GetChildById("groupId");
		this.txtGroupId.OnChangeHandler += this.TxtGroupId_OnChangeHandler;
		this.cbxPriority = (XUiC_ComboBoxBool)base.GetChildById("cbxPriority");
		this.cbxPriority.OnValueChanged += this.CbxPriority_OnValueChanged;
		this.cbxQuestExclude = (XUiC_ComboBoxBool)base.GetChildById("cbxQuestExclude");
		this.cbxQuestExclude.OnValueChanged += this.CbxQuestExclude_OnValueChanged;
		this.cbxCountPreset = (XUiC_ComboBoxList<XUiC_WoPropsSleeperVolume.CountPreset>)base.GetChildById("cbxCountPreset");
		this.cbxCountPreset.OnValueChanged += this.CbxCountPreset_OnValueChanged;
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(-1, -1, "Custom"));
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(1, 2, "12"));
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(2, 3, "23"));
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(3, 4, "34"));
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(4, 5, "45"));
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(5, 6, "56"));
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(6, 7, "67"));
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(7, 8, "78"));
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(8, 9, "89"));
		this.cbxCountPreset.Elements.Add(new XUiC_WoPropsSleeperVolume.CountPreset(9, 10, "910"));
		this.cbxCountPreset.MinIndex = 1;
		this.txtSpawnMin = (XUiC_TextInput)base.GetChildById("spawnMin");
		this.txtSpawnMin.OnChangeHandler += delegate(XUiController _, string _text, bool _fromCode)
		{
			this.TxtSpawnMinMax_OnChangeHandler(_text, _fromCode, false);
		};
		this.txtSpawnMax = (XUiC_TextInput)base.GetChildById("spawnMax");
		this.txtSpawnMax.OnChangeHandler += delegate(XUiController _, string _text, bool _fromCode)
		{
			this.TxtSpawnMinMax_OnChangeHandler(_text, _fromCode, true);
		};
		this.cbxTrigger = (XUiC_ComboBoxEnum<SleeperVolume.ETriggerType>)base.GetChildById("cbxTrigger");
		this.cbxTrigger.OnValueChanged += this.CbxTrigger_OnValueChanged;
		this.txtMinScript = (XUiC_TextInput)base.GetChildById("script");
		this.txtMinScript.OnChangeHandler += this.TxtMinScript_OnChangeHandler;
		this.spawnersList = (XUiC_SpawnersList)base.GetChildById("spawners");
		this.spawnersList.SelectionChanged += this.SpawnersList_SelectionChanged;
		this.spawnersList.SelectableEntries = false;
		this.triggersBox = base.GetChildById("triggersBox").ViewComponent;
		this.triggeredByList = base.GetChildById("triggeredBy") as XUiC_PrefabTriggerEditorList;
		if (this.triggeredByList != null)
		{
			this.triggeredByList.SelectionChanged += this.TriggeredByList_SelectionChanged;
			this.triggeredByList.GetCurrentTriggerIndicesList = () => this.TriggeredByIndices;
			this.triggeredByList.GetCurrentPrefabTriggerLayers = delegate
			{
				SelectionBox selectionBox;
				PrefabSleeperVolume prefabSleeperVolume;
				PrefabInstance prefabInstance;
				int num;
				if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num))
				{
					return null;
				}
				return prefabInstance.prefab.TriggerLayers;
			};
		}
		XUiController childById = base.GetChildById("addTriggeredByButton");
		if (childById != null)
		{
			childById.OnPress += this.TriggerOnAddTriggersPressed;
		}
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnCloneVolume") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += this.BtnCloneVolume_OnPress;
		}
		if (SelectionBoxManager.Instance != null)
		{
			SelectionCategory categorySleeperVolume = SelectionBoxManager.Instance.CategorySleeperVolume;
			categorySleeperVolume.BoxCallbacks = this;
			categorySleeperVolume.CategoryCallbacks = this;
			categorySleeperVolume.CheckKeysCallback = new Action(SleeperVolumeToolManager.CheckKeys);
		}
	}

	// Token: 0x060087C3 RID: 34755 RVA: 0x0033EA7C File Offset: 0x0033CC7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCloneVolume_OnPress(XUiController _sender, int _mouseButton)
	{
		SelectionBox selectionBox;
		PrefabSleeperVolume prefabSleeperVolume;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num))
		{
			return;
		}
		PrefabVolumeManager.Instance.CloneVolumeServer(PrefabVolumeAbs.EVolumeType.Sleeper, prefabInstance.id, num, new Vector3i(0, prefabSleeperVolume.size.y + 1, 0));
	}

	// Token: 0x060087C4 RID: 34756 RVA: 0x0033EACC File Offset: 0x0033CCCC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool modifyCurrentVolume(Action<PrefabSleeperVolume> _modifyAction)
	{
		SelectionBox selectionBox;
		PrefabSleeperVolume prefabSleeperVolume;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num))
		{
			return false;
		}
		PrefabSleeperVolume prefabSleeperVolume2 = prefabSleeperVolume.CloneGeneric();
		_modifyAction(prefabSleeperVolume2);
		PrefabVolumeManager.Instance.UpdatePropertiesServer(prefabInstance.id, num, prefabSleeperVolume2);
		return true;
	}

	// Token: 0x060087C5 RID: 34757 RVA: 0x0033EB18 File Offset: 0x0033CD18
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtGroupId_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (_changeFromCode || _text.Length <= 0)
		{
			return;
		}
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.groupId = StringParsers.ParseSInt16(_text, 0, -1, NumberStyles.Integer);
		});
	}

	// Token: 0x060087C6 RID: 34758 RVA: 0x0033EB58 File Offset: 0x0033CD58
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxPriority_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.isPriority = _newValue;
		});
	}

	// Token: 0x060087C7 RID: 34759 RVA: 0x0033EB88 File Offset: 0x0033CD88
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxQuestExclude_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.isQuestExclude = _newValue;
		});
	}

	// Token: 0x060087C8 RID: 34760 RVA: 0x0033EBB8 File Offset: 0x0033CDB8
	[PublicizedFrom(EAccessModifier.Private)]
	public int findCountPresetIndex(int _min, int _max)
	{
		for (int i = 0; i < this.cbxCountPreset.Elements.Count; i++)
		{
			if ((int)this.cbxCountPreset.Elements[i].Min == _min && (int)this.cbxCountPreset.Elements[i].Max == _max)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060087C9 RID: 34761 RVA: 0x0033EC18 File Offset: 0x0033CE18
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCountPresetLabel()
	{
		SelectionBox selectionBox;
		PrefabSleeperVolume prefabSleeperVolume;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num))
		{
			return;
		}
		int num2 = this.findCountPresetIndex((int)prefabSleeperVolume.spawnCountMin, (int)prefabSleeperVolume.spawnCountMax);
		if (num2 < 0)
		{
			this.cbxCountPreset.MinIndex = 0;
			this.cbxCountPreset.SelectedIndex = 0;
			return;
		}
		this.cbxCountPreset.MinIndex = 1;
		this.cbxCountPreset.SelectedIndex = num2;
	}

	// Token: 0x060087CA RID: 34762 RVA: 0x0033EC88 File Offset: 0x0033CE88
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxCountPreset_OnValueChanged(XUiController _sender, XUiC_WoPropsSleeperVolume.CountPreset _oldValue, XUiC_WoPropsSleeperVolume.CountPreset _newValue)
	{
		this.cbxCountPreset.MinIndex = 1;
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.spawnCountMin = _newValue.Min;
			_volumeCopy.spawnCountMax = _newValue.Max;
		});
		this.updateCountPresetLabel();
	}

	// Token: 0x060087CB RID: 34763 RVA: 0x0033ECC8 File Offset: 0x0033CEC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtSpawnMinMax_OnChangeHandler(string _text, bool _changeFromCode, bool _isMax)
	{
		if (_changeFromCode || _text.Length <= 0)
		{
			return;
		}
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			short num = StringParsers.ParseSInt16(_text, 0, -1, NumberStyles.Integer);
			if (_isMax)
			{
				_volumeCopy.spawnCountMax = num;
				return;
			}
			_volumeCopy.spawnCountMin = num;
		});
		this.updateCountPresetLabel();
	}

	// Token: 0x060087CC RID: 34764 RVA: 0x0033ED14 File Offset: 0x0033CF14
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxTrigger_OnValueChanged(XUiController _sender, SleeperVolume.ETriggerType _oldValue, SleeperVolume.ETriggerType _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.SetTrigger(_newValue);
		});
		this.triggersBox.IsVisible = true;
	}

	// Token: 0x060087CD RID: 34765 RVA: 0x0033ED50 File Offset: 0x0033CF50
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtMinScript_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (_changeFromCode)
		{
			return;
		}
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.minScript = MinScript.ConvertFromUIText(_text);
		});
	}

	// Token: 0x060087CE RID: 34766 RVA: 0x0033ED84 File Offset: 0x0033CF84
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnersList_SelectionChanged(XUiC_List<XUiC_SpawnersList.SpawnerEntry> _list, XUiC_SpawnersList.SpawnerEntry _previousEntry, XUiC_SpawnersList.SpawnerEntry _newEntry)
	{
		string spawnerName = null;
		if (_newEntry != null)
		{
			spawnerName = _newEntry.name;
		}
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.groupName = spawnerName;
		});
	}

	// Token: 0x060087CF RID: 34767 RVA: 0x0033EDC0 File Offset: 0x0033CFC0
	[PublicizedFrom(EAccessModifier.Private)]
	public void TriggeredByList_SelectionChanged(XUiC_List<XUiC_PrefabTriggerEditorList.PrefabTriggerEntry> _list, XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _previousEntry, XUiC_PrefabTriggerEditorList.PrefabTriggerEntry _newEntry)
	{
		if (_newEntry == null)
		{
			return;
		}
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.ToggleTriggeredByFlag(_newEntry.TriggerLayer);
		});
		_newEntry.UiDirty = true;
	}

	// Token: 0x060087D0 RID: 34768 RVA: 0x0033EE04 File Offset: 0x0033D004
	[PublicizedFrom(EAccessModifier.Private)]
	public void TriggerOnAddTriggersPressed(XUiController _sender, int _mouseButton)
	{
		SelectionBox selectionBox;
		PrefabSleeperVolume prefabSleeperVolume;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num))
		{
			return;
		}
		prefabInstance.prefab.AddNewTriggerLayer();
		this.triggeredByList.RebuildList(false);
	}

	// Token: 0x060087D1 RID: 34769 RVA: 0x0033EE44 File Offset: 0x0033D044
	public override void OnOpen()
	{
		base.OnOpen();
		SelectionBox selectionBox;
		PrefabSleeperVolume prefabSleeperVolume;
		PrefabInstance prefabInstance;
		int num;
		PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num);
		if (((prefabInstance != null) ? prefabInstance.prefab : null) != null && prefabInstance.prefab.TriggerLayers.Count == 0)
		{
			prefabInstance.prefab.AddInitialTriggerLayers();
		}
		this.updateCountPresetLabel();
	}

	// Token: 0x060087D2 RID: 34770 RVA: 0x0033EEA0 File Offset: 0x0033D0A0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		XUiC_WoPropsSleeperVolume.VolumeStats volumeStats;
		if (this.getSelectedVolumeStats(out volumeStats))
		{
			this.txtGroupId.Text = volumeStats.GroupId.ToString();
			this.cbxPriority.Value = volumeStats.IsPriority;
			this.cbxQuestExclude.Value = volumeStats.IsQuestExclude;
			this.labelIndex.Text = volumeStats.Index.ToString();
			this.labelPosition.Text = volumeStats.VolumePos.ToString();
			this.labelSize.Text = volumeStats.Size.ToString();
			this.labelSleeperCount.Text = volumeStats.SleeperCount.ToString();
			this.labelGroup.Text = volumeStats.GroupName;
			this.txtSpawnMin.Text = volumeStats.SpawnCountMin.ToString();
			this.txtSpawnMax.Text = volumeStats.SpawnCountMax.ToString();
			this.cbxTrigger.Value = volumeStats.TriggerType;
			this.txtMinScript.Text = MinScript.ConvertToUIText(volumeStats.MinScript);
		}
		else
		{
			this.<Update>g__SetDefaultBoxContents|42_0();
		}
		this.triggersBox.IsVisible = true;
	}

	// Token: 0x060087D3 RID: 34771 RVA: 0x0033EFEA File Offset: 0x0033D1EA
	public override void Cleanup()
	{
		base.Cleanup();
		if (SelectionBoxManager.Instance != null)
		{
			SelectionCategory categorySleeperVolume = SelectionBoxManager.Instance.CategorySleeperVolume;
			categorySleeperVolume.BoxCallbacks = null;
			categorySleeperVolume.CategoryCallbacks = null;
			categorySleeperVolume.CheckKeysCallback = null;
		}
		XUiC_WoPropsSleeperVolume.Instance = null;
	}

	// Token: 0x060087D4 RID: 34772 RVA: 0x0033F023 File Offset: 0x0033D223
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		if (_bActivated)
		{
			this.bSleeperVolumeChanged = true;
		}
		SleeperVolumeToolManager.SelectionChanged(_bActivated ? _box : null);
		return true;
	}

	// Token: 0x060087D5 RID: 34773 RVA: 0x0033F03C File Offset: 0x0033D23C
	public void SleeperVolumeChanged(int _prefabInstanceId, int _volumeId)
	{
		SelectionBox selectionBox;
		PrefabSleeperVolume prefabSleeperVolume;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num))
		{
			return;
		}
		if (prefabInstance.id != _prefabInstanceId || num != _volumeId)
		{
			return;
		}
		this.bSleeperVolumeChanged = true;
	}

	// Token: 0x060087D6 RID: 34774 RVA: 0x0033F078 File Offset: 0x0033D278
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		PrefabVolumeManager.Instance.SelectionBoxMoved(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _moveVector);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060087D7 RID: 34775 RVA: 0x0033F08C File Offset: 0x0033D28C
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		PrefabVolumeManager.Instance.SelectionBoxSized(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060087D8 RID: 34776 RVA: 0x0033F0B4 File Offset: 0x0033D2B4
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060087D9 RID: 34777 RVA: 0x0033F0BC File Offset: 0x0033D2BC
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		int num;
		PrefabInstance prefabInstance;
		return PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance) && PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _checkCanDeleteOnly);
	}

	// Token: 0x060087DA RID: 34778 RVA: 0x00332EC9 File Offset: 0x003310C9
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x060087DB RID: 34779 RVA: 0x0033E301 File Offset: 0x0033C501
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		_windowManager.SwitchVisible(this.windowGroup, true);
	}

	// Token: 0x060087DC RID: 34780 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x060087DD RID: 34781 RVA: 0x0033F0E9 File Offset: 0x0033D2E9
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
		SleeperVolumeToolManager.RegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060087DE RID: 34782 RVA: 0x0033F0F6 File Offset: 0x0033D2F6
	public void OnSelectionCategoryVisibilityChanged(SelectionCategory _category, bool _visible)
	{
		SleeperVolumeToolManager.SetVisible(_visible);
	}

	// Token: 0x060087DF RID: 34783 RVA: 0x0033F0E9 File Offset: 0x0033D2E9
	public void OnSelectionCategoryBoxAdded(SelectionBox _box)
	{
		SleeperVolumeToolManager.RegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060087E0 RID: 34784 RVA: 0x0033F0FE File Offset: 0x0033D2FE
	public void OnSelectionCategoryBoxRemoved(SelectionBox _box)
	{
		SleeperVolumeToolManager.UnRegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060087E1 RID: 34785 RVA: 0x0033F10B File Offset: 0x0033D30B
	public void OnSelectionCategoryCleared(SelectionCategory _category)
	{
		SleeperVolumeToolManager.ClearSleeperVolumes();
	}

	// Token: 0x060087E2 RID: 34786 RVA: 0x0033F114 File Offset: 0x0033D314
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleSleeperVolumeChanged()
	{
		if (!this.bSleeperVolumeChanged)
		{
			return;
		}
		SelectionBox selectionBox;
		PrefabSleeperVolume prefabSleeperVolume;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num))
		{
			return;
		}
		this.bSleeperVolumeChanged = false;
		prefabInstance.prefab.CountSleeperSpawnsInVolume(GameManager.Instance.World, prefabInstance.boundingBoxPosition, num);
		this.updateCountPresetLabel();
	}

	// Token: 0x060087E3 RID: 34787 RVA: 0x0033F170 File Offset: 0x0033D370
	[PublicizedFrom(EAccessModifier.Private)]
	public bool getSelectedVolumeStats(out XUiC_WoPropsSleeperVolume.VolumeStats _stats)
	{
		_stats = default(XUiC_WoPropsSleeperVolume.VolumeStats);
		SelectionBox selectionBox;
		PrefabSleeperVolume prefabSleeperVolume;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<PrefabSleeperVolume>(SelectionBoxManager.Instance.CategorySleeperVolume, out selectionBox, out prefabSleeperVolume, out prefabInstance, out num))
		{
			return false;
		}
		this.handleSleeperVolumeChanged();
		_stats = new XUiC_WoPropsSleeperVolume.VolumeStats(num, prefabSleeperVolume.startPos, prefabInstance.boundingBoxPosition + prefabSleeperVolume.startPos, prefabSleeperVolume.size, prefabSleeperVolume.groupId, GameStageGroup.MakeDisplayName(prefabSleeperVolume.groupName), prefabInstance.prefab.Transient_NumSleeperSpawns, (int)prefabSleeperVolume.spawnCountMin, (int)prefabSleeperVolume.spawnCountMax, prefabSleeperVolume.isPriority, prefabSleeperVolume.isQuestExclude, (SleeperVolume.ETriggerType)(prefabSleeperVolume.flags & 7), prefabSleeperVolume.minScript);
		return true;
	}

	// Token: 0x060087E4 RID: 34788 RVA: 0x0033F211 File Offset: 0x0033D411
	public static bool GetSelectedVolumeStats(out XUiC_WoPropsSleeperVolume.VolumeStats _stats)
	{
		return XUiC_WoPropsSleeperVolume.Instance.getSelectedVolumeStats(out _stats);
	}

	// Token: 0x060087EA RID: 34794 RVA: 0x0033F248 File Offset: 0x0033D448
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <Update>g__SetDefaultBoxContents|42_0()
	{
		this.txtGroupId.Text = string.Empty;
		this.cbxPriority.Value = false;
		this.cbxQuestExclude.Value = false;
		this.labelIndex.Text = string.Empty;
		this.labelPosition.Text = string.Empty;
		this.labelSize.Text = string.Empty;
		this.labelSleeperCount.Text = string.Empty;
		this.labelGroup.Text = string.Empty;
		this.txtSpawnMin.Text = string.Empty;
		this.txtSpawnMax.Text = string.Empty;
		this.cbxTrigger.Value = SleeperVolume.ETriggerType.Active;
		this.txtMinScript.Text = string.Empty;
	}

	// Token: 0x04006500 RID: 25856
	public static string ID = "";

	// Token: 0x04006502 RID: 25858
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView triggersBox;

	// Token: 0x04006503 RID: 25859
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelIndex;

	// Token: 0x04006504 RID: 25860
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelPosition;

	// Token: 0x04006505 RID: 25861
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelSize;

	// Token: 0x04006506 RID: 25862
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelSleeperCount;

	// Token: 0x04006507 RID: 25863
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelGroup;

	// Token: 0x04006508 RID: 25864
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtGroupId;

	// Token: 0x04006509 RID: 25865
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxPriority;

	// Token: 0x0400650A RID: 25866
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxQuestExclude;

	// Token: 0x0400650B RID: 25867
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_WoPropsSleeperVolume.CountPreset> cbxCountPreset;

	// Token: 0x0400650C RID: 25868
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSpawnMin;

	// Token: 0x0400650D RID: 25869
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSpawnMax;

	// Token: 0x0400650E RID: 25870
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<SleeperVolume.ETriggerType> cbxTrigger;

	// Token: 0x0400650F RID: 25871
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtMinScript;

	// Token: 0x04006510 RID: 25872
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SpawnersList spawnersList;

	// Token: 0x04006511 RID: 25873
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggeredByList;

	// Token: 0x04006512 RID: 25874
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bSleeperVolumeChanged;

	// Token: 0x020010A9 RID: 4265
	public struct VolumeStats
	{
		// Token: 0x060087EB RID: 34795 RVA: 0x0033F30C File Offset: 0x0033D50C
		public VolumeStats(int _index, Vector3i _volumePos, Vector3i _worldPos, Vector3i _size, short _groupId, string _groupName, int _sleeperCount, int _spawnCountMin, int _spawnCountMax, bool _isPriority, bool _isQuestExclude, SleeperVolume.ETriggerType _triggerType, string _minScript)
		{
			this.Index = _index;
			this.VolumePos = _volumePos;
			this.WorldPos = _worldPos;
			this.Size = _size;
			this.GroupId = _groupId;
			this.GroupName = _groupName;
			this.SleeperCount = _sleeperCount;
			this.SpawnCountMin = _spawnCountMin;
			this.SpawnCountMax = _spawnCountMax;
			this.IsPriority = _isPriority;
			this.IsQuestExclude = _isQuestExclude;
			this.TriggerType = _triggerType;
			this.MinScript = _minScript;
		}

		// Token: 0x04006513 RID: 25875
		public readonly int Index;

		// Token: 0x04006514 RID: 25876
		public readonly Vector3i VolumePos;

		// Token: 0x04006515 RID: 25877
		public readonly Vector3i WorldPos;

		// Token: 0x04006516 RID: 25878
		public readonly Vector3i Size;

		// Token: 0x04006517 RID: 25879
		public readonly short GroupId;

		// Token: 0x04006518 RID: 25880
		public readonly string GroupName;

		// Token: 0x04006519 RID: 25881
		public readonly int SleeperCount;

		// Token: 0x0400651A RID: 25882
		public readonly int SpawnCountMin;

		// Token: 0x0400651B RID: 25883
		public readonly int SpawnCountMax;

		// Token: 0x0400651C RID: 25884
		public readonly bool IsPriority;

		// Token: 0x0400651D RID: 25885
		public readonly bool IsQuestExclude;

		// Token: 0x0400651E RID: 25886
		public readonly SleeperVolume.ETriggerType TriggerType;

		// Token: 0x0400651F RID: 25887
		public readonly string MinScript;
	}

	// Token: 0x020010AA RID: 4266
	public readonly struct CountPreset
	{
		// Token: 0x060087EC RID: 34796 RVA: 0x0033F37E File Offset: 0x0033D57E
		public CountPreset(short _min, short _max, string _name)
		{
			this.Min = _min;
			this.Max = _max;
			this.name = _name;
		}

		// Token: 0x060087ED RID: 34797 RVA: 0x0033F395 File Offset: 0x0033D595
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04006520 RID: 25888
		public readonly short Min;

		// Token: 0x04006521 RID: 25889
		public readonly short Max;

		// Token: 0x04006522 RID: 25890
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string name;
	}
}
