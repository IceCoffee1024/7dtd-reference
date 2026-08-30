using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010C1 RID: 4289
[Preserve]
public class XUiC_WoPropsSleeperVolume : XUiController, ISelectionBoxCallback, ISelectionCategoryCallback
{
	// Token: 0x17000FEB RID: 4075
	// (get) Token: 0x060088B1 RID: 34993 RVA: 0x00342DE0 File Offset: 0x00340FE0
	// (set) Token: 0x060088B2 RID: 34994 RVA: 0x00342DE7 File Offset: 0x00340FE7
	public static XUiC_WoPropsSleeperVolume Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FEC RID: 4076
	// (get) Token: 0x060088B3 RID: 34995 RVA: 0x00342DF0 File Offset: 0x00340FF0
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

	// Token: 0x060088B4 RID: 34996 RVA: 0x00342E20 File Offset: 0x00341020
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

	// Token: 0x060088B5 RID: 34997 RVA: 0x003432A4 File Offset: 0x003414A4
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

	// Token: 0x060088B6 RID: 34998 RVA: 0x003432F4 File Offset: 0x003414F4
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

	// Token: 0x060088B7 RID: 34999 RVA: 0x00343340 File Offset: 0x00341540
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

	// Token: 0x060088B8 RID: 35000 RVA: 0x00343380 File Offset: 0x00341580
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxPriority_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.isPriority = _newValue;
		});
	}

	// Token: 0x060088B9 RID: 35001 RVA: 0x003433B0 File Offset: 0x003415B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxQuestExclude_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.isQuestExclude = _newValue;
		});
	}

	// Token: 0x060088BA RID: 35002 RVA: 0x003433E0 File Offset: 0x003415E0
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

	// Token: 0x060088BB RID: 35003 RVA: 0x00343440 File Offset: 0x00341640
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

	// Token: 0x060088BC RID: 35004 RVA: 0x003434B0 File Offset: 0x003416B0
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

	// Token: 0x060088BD RID: 35005 RVA: 0x003434F0 File Offset: 0x003416F0
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

	// Token: 0x060088BE RID: 35006 RVA: 0x0034353C File Offset: 0x0034173C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxTrigger_OnValueChanged(XUiController _sender, SleeperVolume.ETriggerType _oldValue, SleeperVolume.ETriggerType _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.SetTrigger(_newValue);
		});
		this.triggersBox.IsVisible = true;
	}

	// Token: 0x060088BF RID: 35007 RVA: 0x00343578 File Offset: 0x00341778
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

	// Token: 0x060088C0 RID: 35008 RVA: 0x003435AC File Offset: 0x003417AC
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

	// Token: 0x060088C1 RID: 35009 RVA: 0x003435E8 File Offset: 0x003417E8
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

	// Token: 0x060088C2 RID: 35010 RVA: 0x0034362C File Offset: 0x0034182C
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

	// Token: 0x060088C3 RID: 35011 RVA: 0x0034366C File Offset: 0x0034186C
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

	// Token: 0x060088C4 RID: 35012 RVA: 0x003436C8 File Offset: 0x003418C8
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

	// Token: 0x060088C5 RID: 35013 RVA: 0x00343812 File Offset: 0x00341A12
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

	// Token: 0x060088C6 RID: 35014 RVA: 0x0034384B File Offset: 0x00341A4B
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		if (_bActivated)
		{
			this.bSleeperVolumeChanged = true;
		}
		SleeperVolumeToolManager.SelectionChanged(_bActivated ? _box : null);
		return true;
	}

	// Token: 0x060088C7 RID: 35015 RVA: 0x00343864 File Offset: 0x00341A64
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

	// Token: 0x060088C8 RID: 35016 RVA: 0x003438A0 File Offset: 0x00341AA0
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		PrefabVolumeManager.Instance.SelectionBoxMoved(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _moveVector);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060088C9 RID: 35017 RVA: 0x003438B4 File Offset: 0x00341AB4
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		PrefabVolumeManager.Instance.SelectionBoxSized(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060088CA RID: 35018 RVA: 0x003438DC File Offset: 0x00341ADC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060088CB RID: 35019 RVA: 0x003438E4 File Offset: 0x00341AE4
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		int num;
		PrefabInstance prefabInstance;
		return PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance) && PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _checkCanDeleteOnly);
	}

	// Token: 0x060088CC RID: 35020 RVA: 0x003376F1 File Offset: 0x003358F1
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x060088CD RID: 35021 RVA: 0x00342B29 File Offset: 0x00340D29
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		_windowManager.SwitchVisible(this.windowGroup, true);
	}

	// Token: 0x060088CE RID: 35022 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x060088CF RID: 35023 RVA: 0x00343911 File Offset: 0x00341B11
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
		SleeperVolumeToolManager.RegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060088D0 RID: 35024 RVA: 0x0034391E File Offset: 0x00341B1E
	public void OnSelectionCategoryVisibilityChanged(SelectionCategory _category, bool _visible)
	{
		SleeperVolumeToolManager.SetVisible(_visible);
	}

	// Token: 0x060088D1 RID: 35025 RVA: 0x00343911 File Offset: 0x00341B11
	public void OnSelectionCategoryBoxAdded(SelectionBox _box)
	{
		SleeperVolumeToolManager.RegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060088D2 RID: 35026 RVA: 0x00343926 File Offset: 0x00341B26
	public void OnSelectionCategoryBoxRemoved(SelectionBox _box)
	{
		SleeperVolumeToolManager.UnRegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x060088D3 RID: 35027 RVA: 0x00343933 File Offset: 0x00341B33
	public void OnSelectionCategoryCleared(SelectionCategory _category)
	{
		SleeperVolumeToolManager.ClearSleeperVolumes();
	}

	// Token: 0x060088D4 RID: 35028 RVA: 0x0034393C File Offset: 0x00341B3C
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

	// Token: 0x060088D5 RID: 35029 RVA: 0x00343998 File Offset: 0x00341B98
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

	// Token: 0x060088D6 RID: 35030 RVA: 0x00343A39 File Offset: 0x00341C39
	public static bool GetSelectedVolumeStats(out XUiC_WoPropsSleeperVolume.VolumeStats _stats)
	{
		return XUiC_WoPropsSleeperVolume.Instance.getSelectedVolumeStats(out _stats);
	}

	// Token: 0x060088DC RID: 35036 RVA: 0x00343A70 File Offset: 0x00341C70
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

	// Token: 0x040065C7 RID: 26055
	public static string ID = "";

	// Token: 0x040065C9 RID: 26057
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView triggersBox;

	// Token: 0x040065CA RID: 26058
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelIndex;

	// Token: 0x040065CB RID: 26059
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelPosition;

	// Token: 0x040065CC RID: 26060
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelSize;

	// Token: 0x040065CD RID: 26061
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelSleeperCount;

	// Token: 0x040065CE RID: 26062
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelGroup;

	// Token: 0x040065CF RID: 26063
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtGroupId;

	// Token: 0x040065D0 RID: 26064
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxPriority;

	// Token: 0x040065D1 RID: 26065
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxQuestExclude;

	// Token: 0x040065D2 RID: 26066
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_WoPropsSleeperVolume.CountPreset> cbxCountPreset;

	// Token: 0x040065D3 RID: 26067
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSpawnMin;

	// Token: 0x040065D4 RID: 26068
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSpawnMax;

	// Token: 0x040065D5 RID: 26069
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<SleeperVolume.ETriggerType> cbxTrigger;

	// Token: 0x040065D6 RID: 26070
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtMinScript;

	// Token: 0x040065D7 RID: 26071
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SpawnersList spawnersList;

	// Token: 0x040065D8 RID: 26072
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggeredByList;

	// Token: 0x040065D9 RID: 26073
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bSleeperVolumeChanged;

	// Token: 0x020010C2 RID: 4290
	public struct VolumeStats
	{
		// Token: 0x060088DD RID: 35037 RVA: 0x00343B34 File Offset: 0x00341D34
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

		// Token: 0x040065DA RID: 26074
		public readonly int Index;

		// Token: 0x040065DB RID: 26075
		public readonly Vector3i VolumePos;

		// Token: 0x040065DC RID: 26076
		public readonly Vector3i WorldPos;

		// Token: 0x040065DD RID: 26077
		public readonly Vector3i Size;

		// Token: 0x040065DE RID: 26078
		public readonly short GroupId;

		// Token: 0x040065DF RID: 26079
		public readonly string GroupName;

		// Token: 0x040065E0 RID: 26080
		public readonly int SleeperCount;

		// Token: 0x040065E1 RID: 26081
		public readonly int SpawnCountMin;

		// Token: 0x040065E2 RID: 26082
		public readonly int SpawnCountMax;

		// Token: 0x040065E3 RID: 26083
		public readonly bool IsPriority;

		// Token: 0x040065E4 RID: 26084
		public readonly bool IsQuestExclude;

		// Token: 0x040065E5 RID: 26085
		public readonly SleeperVolume.ETriggerType TriggerType;

		// Token: 0x040065E6 RID: 26086
		public readonly string MinScript;
	}

	// Token: 0x020010C3 RID: 4291
	public readonly struct CountPreset
	{
		// Token: 0x060088DE RID: 35038 RVA: 0x00343BA6 File Offset: 0x00341DA6
		public CountPreset(short _min, short _max, string _name)
		{
			this.Min = _min;
			this.Max = _max;
			this.name = _name;
		}

		// Token: 0x060088DF RID: 35039 RVA: 0x00343BBD File Offset: 0x00341DBD
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x040065E7 RID: 26087
		public readonly short Min;

		// Token: 0x040065E8 RID: 26088
		public readonly short Max;

		// Token: 0x040065E9 RID: 26089
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string name;
	}
}
