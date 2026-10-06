using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010D2 RID: 4306
[Preserve]
public class XUiC_WoPropsSleeperVolume : XUiController, ISelectionBoxCallback, ISelectionCategoryCallback
{
	// Token: 0x17001020 RID: 4128
	// (get) Token: 0x060088FF RID: 35071 RVA: 0x00346B90 File Offset: 0x00344D90
	// (set) Token: 0x06008900 RID: 35072 RVA: 0x00346B97 File Offset: 0x00344D97
	public static XUiC_WoPropsSleeperVolume Instance
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17001021 RID: 4129
	// (get) Token: 0x06008901 RID: 35073 RVA: 0x00346BA0 File Offset: 0x00344DA0
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

	// Token: 0x06008902 RID: 35074 RVA: 0x00346BD0 File Offset: 0x00344DD0
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

	// Token: 0x06008903 RID: 35075 RVA: 0x00347054 File Offset: 0x00345254
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

	// Token: 0x06008904 RID: 35076 RVA: 0x003470A4 File Offset: 0x003452A4
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

	// Token: 0x06008905 RID: 35077 RVA: 0x003470F0 File Offset: 0x003452F0
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

	// Token: 0x06008906 RID: 35078 RVA: 0x00347130 File Offset: 0x00345330
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxPriority_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.isPriority = _newValue;
		});
	}

	// Token: 0x06008907 RID: 35079 RVA: 0x00347160 File Offset: 0x00345360
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxQuestExclude_OnValueChanged(XUiController _sender, bool _oldValue, bool _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.isQuestExclude = _newValue;
		});
	}

	// Token: 0x06008908 RID: 35080 RVA: 0x00347190 File Offset: 0x00345390
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

	// Token: 0x06008909 RID: 35081 RVA: 0x003471F0 File Offset: 0x003453F0
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

	// Token: 0x0600890A RID: 35082 RVA: 0x00347260 File Offset: 0x00345460
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

	// Token: 0x0600890B RID: 35083 RVA: 0x003472A0 File Offset: 0x003454A0
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

	// Token: 0x0600890C RID: 35084 RVA: 0x003472EC File Offset: 0x003454EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxTrigger_OnValueChanged(XUiController _sender, SleeperVolume.ETriggerType _oldValue, SleeperVolume.ETriggerType _newValue)
	{
		this.modifyCurrentVolume(delegate(PrefabSleeperVolume _volumeCopy)
		{
			_volumeCopy.SetTrigger(_newValue);
		});
		this.triggersBox.IsVisible = true;
	}

	// Token: 0x0600890D RID: 35085 RVA: 0x00347328 File Offset: 0x00345528
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

	// Token: 0x0600890E RID: 35086 RVA: 0x0034735C File Offset: 0x0034555C
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

	// Token: 0x0600890F RID: 35087 RVA: 0x00347398 File Offset: 0x00345598
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

	// Token: 0x06008910 RID: 35088 RVA: 0x003473DC File Offset: 0x003455DC
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

	// Token: 0x06008911 RID: 35089 RVA: 0x0034741C File Offset: 0x0034561C
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

	// Token: 0x06008912 RID: 35090 RVA: 0x00347478 File Offset: 0x00345678
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

	// Token: 0x06008913 RID: 35091 RVA: 0x003475C2 File Offset: 0x003457C2
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

	// Token: 0x06008914 RID: 35092 RVA: 0x003475FB File Offset: 0x003457FB
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		if (_bActivated)
		{
			this.bSleeperVolumeChanged = true;
		}
		SleeperVolumeToolManager.SelectionChanged(_bActivated ? _box : null);
		return true;
	}

	// Token: 0x06008915 RID: 35093 RVA: 0x00347614 File Offset: 0x00345814
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

	// Token: 0x06008916 RID: 35094 RVA: 0x00347650 File Offset: 0x00345850
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		PrefabVolumeManager.Instance.SelectionBoxMoved(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _moveVector);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x06008917 RID: 35095 RVA: 0x00347664 File Offset: 0x00345864
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		PrefabVolumeManager.Instance.SelectionBoxSized(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x06008918 RID: 35096 RVA: 0x0034768C File Offset: 0x0034588C
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x06008919 RID: 35097 RVA: 0x00347694 File Offset: 0x00345894
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		int num;
		PrefabInstance prefabInstance;
		return PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance) && PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Sleeper, _box, _checkCanDeleteOnly);
	}

	// Token: 0x0600891A RID: 35098 RVA: 0x0033B509 File Offset: 0x00339709
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x0600891B RID: 35099 RVA: 0x003468D9 File Offset: 0x00344AD9
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		_windowManager.SwitchVisible(this.windowGroup, true);
	}

	// Token: 0x0600891C RID: 35100 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x0600891D RID: 35101 RVA: 0x003476C1 File Offset: 0x003458C1
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
		SleeperVolumeToolManager.RegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x0600891E RID: 35102 RVA: 0x003476CE File Offset: 0x003458CE
	public void OnSelectionCategoryVisibilityChanged(SelectionCategory _category, bool _visible)
	{
		SleeperVolumeToolManager.SetVisible(_visible);
	}

	// Token: 0x0600891F RID: 35103 RVA: 0x003476C1 File Offset: 0x003458C1
	public void OnSelectionCategoryBoxAdded(SelectionBox _box)
	{
		SleeperVolumeToolManager.RegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x06008920 RID: 35104 RVA: 0x003476D6 File Offset: 0x003458D6
	public void OnSelectionCategoryBoxRemoved(SelectionBox _box)
	{
		SleeperVolumeToolManager.UnRegisterSleeperVolume(_box);
		SleeperVolumeToolManager.UpdateAllVisuals();
	}

	// Token: 0x06008921 RID: 35105 RVA: 0x003476E3 File Offset: 0x003458E3
	public void OnSelectionCategoryCleared(SelectionCategory _category)
	{
		SleeperVolumeToolManager.ClearSleeperVolumes();
	}

	// Token: 0x06008922 RID: 35106 RVA: 0x003476EC File Offset: 0x003458EC
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

	// Token: 0x06008923 RID: 35107 RVA: 0x00347748 File Offset: 0x00345948
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

	// Token: 0x06008924 RID: 35108 RVA: 0x003477E9 File Offset: 0x003459E9
	public static bool GetSelectedVolumeStats(out XUiC_WoPropsSleeperVolume.VolumeStats _stats)
	{
		return XUiC_WoPropsSleeperVolume.Instance.getSelectedVolumeStats(out _stats);
	}

	// Token: 0x0600892A RID: 35114 RVA: 0x00347820 File Offset: 0x00345A20
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

	// Token: 0x04006609 RID: 26121
	public static string ID = "";

	// Token: 0x0400660B RID: 26123
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView triggersBox;

	// Token: 0x0400660C RID: 26124
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelIndex;

	// Token: 0x0400660D RID: 26125
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelPosition;

	// Token: 0x0400660E RID: 26126
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelSize;

	// Token: 0x0400660F RID: 26127
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelSleeperCount;

	// Token: 0x04006610 RID: 26128
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label labelGroup;

	// Token: 0x04006611 RID: 26129
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtGroupId;

	// Token: 0x04006612 RID: 26130
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxPriority;

	// Token: 0x04006613 RID: 26131
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxBool cbxQuestExclude;

	// Token: 0x04006614 RID: 26132
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<XUiC_WoPropsSleeperVolume.CountPreset> cbxCountPreset;

	// Token: 0x04006615 RID: 26133
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSpawnMin;

	// Token: 0x04006616 RID: 26134
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtSpawnMax;

	// Token: 0x04006617 RID: 26135
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<SleeperVolume.ETriggerType> cbxTrigger;

	// Token: 0x04006618 RID: 26136
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtMinScript;

	// Token: 0x04006619 RID: 26137
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SpawnersList spawnersList;

	// Token: 0x0400661A RID: 26138
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabTriggerEditorList triggeredByList;

	// Token: 0x0400661B RID: 26139
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bSleeperVolumeChanged;

	// Token: 0x020010D3 RID: 4307
	public struct VolumeStats
	{
		// Token: 0x0600892B RID: 35115 RVA: 0x003478E4 File Offset: 0x00345AE4
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

		// Token: 0x0400661C RID: 26140
		public readonly int Index;

		// Token: 0x0400661D RID: 26141
		public readonly Vector3i VolumePos;

		// Token: 0x0400661E RID: 26142
		public readonly Vector3i WorldPos;

		// Token: 0x0400661F RID: 26143
		public readonly Vector3i Size;

		// Token: 0x04006620 RID: 26144
		public readonly short GroupId;

		// Token: 0x04006621 RID: 26145
		public readonly string GroupName;

		// Token: 0x04006622 RID: 26146
		public readonly int SleeperCount;

		// Token: 0x04006623 RID: 26147
		public readonly int SpawnCountMin;

		// Token: 0x04006624 RID: 26148
		public readonly int SpawnCountMax;

		// Token: 0x04006625 RID: 26149
		public readonly bool IsPriority;

		// Token: 0x04006626 RID: 26150
		public readonly bool IsQuestExclude;

		// Token: 0x04006627 RID: 26151
		public readonly SleeperVolume.ETriggerType TriggerType;

		// Token: 0x04006628 RID: 26152
		public readonly string MinScript;
	}

	// Token: 0x020010D4 RID: 4308
	public readonly struct CountPreset
	{
		// Token: 0x0600892C RID: 35116 RVA: 0x00347956 File Offset: 0x00345B56
		public CountPreset(short _min, short _max, string _name)
		{
			this.Min = _min;
			this.Max = _max;
			this.name = _name;
		}

		// Token: 0x0600892D RID: 35117 RVA: 0x0034796D File Offset: 0x00345B6D
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04006629 RID: 26153
		public readonly short Min;

		// Token: 0x0400662A RID: 26154
		public readonly short Max;

		// Token: 0x0400662B RID: 26155
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string name;
	}
}
