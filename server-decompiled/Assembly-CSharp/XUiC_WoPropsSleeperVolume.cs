using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010E2 RID: 4322
[Preserve]
public class XUiC_WoPropsSleeperVolume : XUiController, ISelectionBoxCallback, ISelectionCategoryCallback
{
	// Token: 0x1700102B RID: 4139
	// (get) Token: 0x06008952 RID: 35154 RVA: 0x00348C14 File Offset: 0x00346E14
	// (set) Token: 0x06008953 RID: 35155 RVA: 0x00348C1B File Offset: 0x00346E1B
	public static XUiC_WoPropsSleeperVolume Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700102C RID: 4140
	// (get) Token: 0x06008954 RID: 35156 RVA: 0x00348C24 File Offset: 0x00346E24
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

	// Token: 0x06008955 RID: 35157 RVA: 0x00348C54 File Offset: 0x00346E54
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

	// Token: 0x06008956 RID: 35158 RVA: 0x003490D8 File Offset: 0x003472D8
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

	// Token: 0x06008957 RID: 35159 RVA: 0x00349128 File Offset: 0x00347328
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

	// Token: 0x06008958 RID: 35160 RVA: 0x00349174 File Offset: 0x00347374
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

	// Token: 0x06008959 RID: 35161 RVA: 0x003491B4 File Offset: 0x003473B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxPriority_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.isPriority = _newValue;
		});
	}

	// Token: 0x0600895A RID: 35162 RVA: 0x003491E4 File Offset: 0x003473E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxQuestExclude_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.isQuestExclude = _newValue;
		});
	}

	// Token: 0x0600895B RID: 35163 RVA: 0x00349214 File Offset: 0x00347414
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

	// Token: 0x0600895C RID: 35164 RVA: 0x00349274 File Offset: 0x00347474
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

	// Token: 0x0600895D RID: 35165 RVA: 0x003492E4 File Offset: 0x003474E4
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

	// Token: 0x0600895E RID: 35166 RVA: 0x00349324 File Offset: 0x00347524
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

	// Token: 0x0600895F RID: 35167 RVA: 0x00349370 File Offset: 0x00347570
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxTrigger_OnValueChanged(XUiController _sender, SleeperVolume.ETriggerType _oldValue, SleeperVolume.ETriggerType _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.SetTrigger(_newValue);
		});
		this.triggersBox.IsVisible = true;
	}

	// Token: 0x06008960 RID: 35168 RVA: 0x003493AC File Offset: 0x003475AC
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

	// Token: 0x06008961 RID: 35169 RVA: 0x003493E0 File Offset: 0x003475E0
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

	// Token: 0x06008962 RID: 35170 RVA: 0x0034941C File Offset: 0x0034761C
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

	// Token: 0x06008963 RID: 35171 RVA: 0x00349460 File Offset: 0x00347660
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

	// Token: 0x06008964 RID: 35172 RVA: 0x003494A0 File Offset: 0x003476A0
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

	// Token: 0x06008965 RID: 35173 RVA: 0x003494FC File Offset: 0x003476FC
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

	// Token: 0x06008966 RID: 35174 RVA: 0x00349646 File Offset: 0x00347846
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

	// Token: 0x06008967 RID: 35175 RVA: 0x0034967F File Offset: 0x0034787F
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		if (_bActivated)
		{
			this.bSleeperVolumeChanged = true;
		}
		SleeperVolumeToolManager.SelectionChanged(_bActivated ? _box : null);
		return true;
	}

	// Token: 0x06008968 RID: 35176 RVA: 0x00349698 File Offset: 0x00347898
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

	// Token: 0x06008969 RID: 35177 RVA: 0x003496D4 File Offset: 0x003478D4
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		PrefabVolumeManager.Instance.SelectionBoxMoved(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _moveVector);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x0600896A RID: 35178 RVA: 0x003496E8 File Offset: 0x003478E8
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		PrefabVolumeManager.Instance.SelectionBoxSized(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x0600896B RID: 35179 RVA: 0x00349710 File Offset: 0x00347910
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x0600896C RID: 35180 RVA: 0x00349718 File Offset: 0x00347918
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		int num;
		PrefabInstance prefabInstance;
		return PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance) && PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _checkCanDeleteOnly);
	}

	// Token: 0x0600896D RID: 35181 RVA: 0x0033D58D File Offset: 0x0033B78D
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x0600896E RID: 35182 RVA: 0x0034895D File Offset: 0x00346B5D
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		_windowManager.SwitchVisible(this.windowGroup, true);
	}

	// Token: 0x0600896F RID: 35183 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x06008970 RID: 35184 RVA: 0x00349745 File Offset: 0x00347945
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
		SleeperVolumeToolManager.RegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x06008971 RID: 35185 RVA: 0x00349752 File Offset: 0x00347952
	public void OnSelectionCategoryVisibilityChanged(SelectionCategory _category, bool _visible)
	{
		SleeperVolumeToolManager.SetVisible(_visible);
	}

	// Token: 0x06008972 RID: 35186 RVA: 0x00349745 File Offset: 0x00347945
	public void OnSelectionCategoryBoxAdded(SelectionBox _box)
	{
		SleeperVolumeToolManager.RegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x06008973 RID: 35187 RVA: 0x0034975A File Offset: 0x0034795A
	public void OnSelectionCategoryBoxRemoved(SelectionBox _box)
	{
		SleeperVolumeToolManager.UnRegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x06008974 RID: 35188 RVA: 0x00349767 File Offset: 0x00347967
	public void OnSelectionCategoryCleared(SelectionCategory _category)
	{
		SleeperVolumeToolManager.ClearSleeperVolumes();
	}

	// Token: 0x06008975 RID: 35189 RVA: 0x00349770 File Offset: 0x00347970
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

	// Token: 0x06008976 RID: 35190 RVA: 0x003497CC File Offset: 0x003479CC
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

	// Token: 0x06008977 RID: 35191 RVA: 0x0034986D File Offset: 0x00347A6D
	public static bool GetSelectedVolumeStats(out XUiC_WoPropsSleeperVolume.VolumeStats _stats)
	{
		return XUiC_WoPropsSleeperVolume.Instance.getSelectedVolumeStats(out _stats);
	}

	// Token: 0x0600897D RID: 35197 RVA: 0x003498A4 File Offset: 0x00347AA4
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

	// Token: 0x04006648 RID: 26184
	public static string ID = "";

	// Token: 0x0400664A RID: 26186
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView triggersBox;

	// Token: 0x0400664B RID: 26187
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelIndex;

	// Token: 0x0400664C RID: 26188
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelPosition;

	// Token: 0x0400664D RID: 26189
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelSize;

	// Token: 0x0400664E RID: 26190
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelSleeperCount;

	// Token: 0x0400664F RID: 26191
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelGroup;

	// Token: 0x04006650 RID: 26192
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtGroupId;

	// Token: 0x04006651 RID: 26193
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxPriority;

	// Token: 0x04006652 RID: 26194
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxQuestExclude;

	// Token: 0x04006653 RID: 26195
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_WoPropsSleeperVolume.CountPreset> cbxCountPreset;

	// Token: 0x04006654 RID: 26196
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSpawnMin;

	// Token: 0x04006655 RID: 26197
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSpawnMax;

	// Token: 0x04006656 RID: 26198
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<SleeperVolume.ETriggerType> cbxTrigger;

	// Token: 0x04006657 RID: 26199
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtMinScript;

	// Token: 0x04006658 RID: 26200
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SpawnersList spawnersList;

	// Token: 0x04006659 RID: 26201
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggeredByList;

	// Token: 0x0400665A RID: 26202
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bSleeperVolumeChanged;

	// Token: 0x020010E3 RID: 4323
	public struct VolumeStats
	{
		// Token: 0x0600897E RID: 35198 RVA: 0x00349968 File Offset: 0x00347B68
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

		// Token: 0x0400665B RID: 26203
		public readonly int Index;

		// Token: 0x0400665C RID: 26204
		public readonly Vector3i VolumePos;

		// Token: 0x0400665D RID: 26205
		public readonly Vector3i WorldPos;

		// Token: 0x0400665E RID: 26206
		public readonly Vector3i Size;

		// Token: 0x0400665F RID: 26207
		public readonly short GroupId;

		// Token: 0x04006660 RID: 26208
		public readonly string GroupName;

		// Token: 0x04006661 RID: 26209
		public readonly int SleeperCount;

		// Token: 0x04006662 RID: 26210
		public readonly int SpawnCountMin;

		// Token: 0x04006663 RID: 26211
		public readonly int SpawnCountMax;

		// Token: 0x04006664 RID: 26212
		public readonly bool IsPriority;

		// Token: 0x04006665 RID: 26213
		public readonly bool IsQuestExclude;

		// Token: 0x04006666 RID: 26214
		public readonly SleeperVolume.ETriggerType TriggerType;

		// Token: 0x04006667 RID: 26215
		public readonly string MinScript;
	}

	// Token: 0x020010E4 RID: 4324
	public readonly struct CountPreset
	{
		// Token: 0x0600897F RID: 35199 RVA: 0x003499DA File Offset: 0x00347BDA
		public CountPreset(short _min, short _max, string _name)
		{
			this.Min = _min;
			this.Max = _max;
			this.name = _name;
		}

		// Token: 0x06008980 RID: 35200 RVA: 0x003499F1 File Offset: 0x00347BF1
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04006668 RID: 26216
		public readonly short Min;

		// Token: 0x04006669 RID: 26217
		public readonly short Max;

		// Token: 0x0400666A RID: 26218
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string name;
	}
}
