using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010B8 RID: 4280
[Preserve]
public class XUiC_WoPropsPOIMarker : XUiController, ISelectionBoxCallback, ISelectionCategoryCallback
{
	// Token: 0x06008874 RID: 34932 RVA: 0x00341DB4 File Offset: 0x0033FFB4
	public override void Init()
	{
		base.Init();
		XUiC_WoPropsPOIMarker.ID = base.WindowGroup.Id;
		XUiC_WoPropsPOIMarker.Instance = this;
		this.markerList = base.GetChildById("markers") as XUiC_PrefabMarkerList;
		if (this.markerList != null)
		{
			this.markerList.SelectionChanged += this.MarkerList_SelectionChanged;
		}
		this.startX = base.GetChildById("txtStartX") as XUiC_TextInput;
		if (this.startX != null)
		{
			this.startX.OnChangeHandler += delegate(XUiController _, string _text, bool _changeFromCode)
			{
				this.modifyStartPos(_text, _changeFromCode, 0);
			};
		}
		this.startY = base.GetChildById("txtStartY") as XUiC_TextInput;
		if (this.startY != null)
		{
			this.startY.OnChangeHandler += delegate(XUiController _, string _text, bool _changeFromCode)
			{
				this.modifyStartPos(_text, _changeFromCode, 1);
			};
		}
		this.startZ = base.GetChildById("txtStartZ") as XUiC_TextInput;
		if (this.startZ != null)
		{
			this.startZ.OnChangeHandler += delegate(XUiController _, string _text, bool _changeFromCode)
			{
				this.modifyStartPos(_text, _changeFromCode, 2);
			};
		}
		this.sizeX = base.GetChildById("txtSizeX") as XUiC_TextInput;
		if (this.sizeX != null)
		{
			this.sizeX.OnChangeHandler += delegate(XUiController _, string _text, bool _changeFromCode)
			{
				this.modifySize(_text, _changeFromCode, 0);
			};
		}
		this.sizeY = base.GetChildById("txtSizeY") as XUiC_TextInput;
		if (this.sizeY != null)
		{
			this.sizeY.OnChangeHandler += delegate(XUiController _, string _text, bool _changeFromCode)
			{
				this.modifySize(_text, _changeFromCode, 1);
			};
		}
		this.sizeZ = base.GetChildById("txtSizeZ") as XUiC_TextInput;
		if (this.sizeZ != null)
		{
			this.sizeZ.OnChangeHandler += delegate(XUiController _, string _text, bool _changeFromCode)
			{
				this.modifySize(_text, _changeFromCode, 2);
			};
		}
		this.rotations = base.GetChildById("txtMarkerRotations") as XUiC_ComboBoxInt;
		if (this.rotations != null)
		{
			this.rotations.OnValueChanged += this.Rotations_OnValueChanged;
		}
		this.partSpawnChance = base.GetChildById("cbxPartSpawnChance") as XUiC_ComboBoxFloat;
		if (this.partSpawnChance != null)
		{
			this.partSpawnChance.OnValueChanged += this.PartSpawnChance_OnValueChanged;
		}
		this.markerSize = base.GetChildById("cbxPOIMarkerSize") as XUiC_ComboBoxEnum<Marker.MarkerSize>;
		if (this.markerSize != null)
		{
			this.markerSize.OnValueChanged += this.MarkerSize_OnValueChanged;
		}
		this.markerType = base.GetChildById("cbxPOIMarkerType") as XUiC_ComboBoxEnum<Marker.MarkerTypes>;
		if (this.markerType != null)
		{
			this.markerType.OnValueChanged += this.MarkerType_OnValueChanged;
		}
		this.groupName = base.GetChildById("txtGroup") as XUiC_TextInput;
		if (this.groupName != null)
		{
			this.groupName.OnChangeHandler += this.GroupName_OnChangeHandler;
		}
		this.tags = base.GetChildById("txtTags") as XUiC_TextInput;
		if (this.tags != null)
		{
			this.tags.OnChangeHandler += this.Tags_OnChangeHandler;
		}
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnCreateMarker") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += this.BtnCreateMarker_OnPress;
		}
		XUiC_SimpleButton xuiC_SimpleButton2 = base.GetChildById("btnDeleteMarker") as XUiC_SimpleButton;
		if (xuiC_SimpleButton2 != null)
		{
			xuiC_SimpleButton2.OnPressed += this.BtnDeleteMarker_OnPress;
		}
		this.txtPoiMarkerPartName = base.GetChildById("txtPoiMarkerPartName") as XUiC_DropDown;
		if (this.txtPoiMarkerPartName != null)
		{
			foreach (PathAbstractions.AbstractedLocation abstractedLocation in PathAbstractions.PrefabsSearchPaths.GetAvailablePathsList(null, true, null))
			{
				if (abstractedLocation.RelativePath.EqualsCaseInsensitive("parts"))
				{
					this.txtPoiMarkerPartName.AllEntries.Add(abstractedLocation.Name);
				}
			}
			this.txtPoiMarkerPartName.UpdateFilteredList();
			this.txtPoiMarkerPartName.OnChangeHandler += this.txtPoiMarkerPartName_OnValueChanged;
		}
		if (SelectionBoxManager.Instance != null)
		{
			SelectionCategory categoryPOIMarker = SelectionBoxManager.Instance.CategoryPOIMarker;
			categoryPOIMarker.BoxCallbacks = this;
			categoryPOIMarker.CategoryCallbacks = this;
		}
	}

	// Token: 0x06008875 RID: 34933 RVA: 0x003421BC File Offset: 0x003403BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCreateMarker_OnPress(XUiController _sender, int _mouseButton)
	{
		this.spawnNewMarker();
	}

	// Token: 0x06008876 RID: 34934 RVA: 0x003421C4 File Offset: 0x003403C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDeleteMarker_OnPress(XUiController _sender, int _mouseButton)
	{
		SelectionBox selectionBox;
		Marker marker;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<Marker>(SelectionBoxManager.Instance.CategoryPOIMarker, out selectionBox, out marker, out prefabInstance, out num))
		{
			return;
		}
		PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Marker, selectionBox, false);
	}

	// Token: 0x06008877 RID: 34935 RVA: 0x003421FC File Offset: 0x003403FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkerList_SelectionChanged(XUiC_List<XUiC_PrefabMarkerList.PrefabMarkerEntry> _list, XUiC_PrefabMarkerList.PrefabMarkerEntry _previousEntry, XUiC_PrefabMarkerList.PrefabMarkerEntry _newEntry)
	{
		if (this.ignoreSelectionChange || !this.windowGroup.isShowing)
		{
			return;
		}
		SelectionBox selectionBox = ((_newEntry != null) ? _newEntry.Box : null);
		if (selectionBox == null)
		{
			SelectionBox selection = SelectionBoxManager.Instance.Selection;
			if (((selection != null) ? selection.Category : null) == SelectionBoxManager.Instance.CategoryPOIMarker)
			{
				SelectionBoxManager.Instance.Deactivate();
				return;
			}
		}
		else if (!selectionBox.IsActive)
		{
			SelectionBoxManager.Instance.SetActive(selectionBox, true);
		}
	}

	// Token: 0x06008878 RID: 34936 RVA: 0x00342278 File Offset: 0x00340478
	[PublicizedFrom(EAccessModifier.Private)]
	public bool modifyCurrentMarker(Action<Marker> _modifyAction)
	{
		SelectionBox selectionBox;
		Marker marker;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<Marker>(SelectionBoxManager.Instance.CategoryPOIMarker, out selectionBox, out marker, out prefabInstance, out num))
		{
			return false;
		}
		Marker marker2 = marker.CloneGeneric();
		_modifyAction(marker2);
		PrefabVolumeManager.Instance.UpdatePropertiesServer(prefabInstance.id, num, marker2);
		return true;
	}

	// Token: 0x06008879 RID: 34937 RVA: 0x003422C4 File Offset: 0x003404C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Tags_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		this.modifyCurrentMarker(delegate(Marker _volumeCopy)
		{
			_volumeCopy.Tags = FastTags<TagGroup.Poi>.Parse(_text);
		});
	}

	// Token: 0x0600887A RID: 34938 RVA: 0x003422F4 File Offset: 0x003404F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void PartSpawnChance_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.modifyCurrentMarker(delegate(Marker _volumeCopy)
		{
			_volumeCopy.PartChanceToSpawn = (float)Mathf.RoundToInt((float)_newValue * 100f) / 100f;
		});
	}

	// Token: 0x0600887B RID: 34939 RVA: 0x00342324 File Offset: 0x00340524
	[PublicizedFrom(EAccessModifier.Private)]
	public void Rotations_OnValueChanged(XUiController _sender, long _oldValue, long _newValue)
	{
		this.modifyCurrentMarker(delegate(Marker _volumeCopy)
		{
			_volumeCopy.Rotations = (byte)_newValue;
		});
	}

	// Token: 0x0600887C RID: 34940 RVA: 0x00342354 File Offset: 0x00340554
	[PublicizedFrom(EAccessModifier.Private)]
	public void txtPoiMarkerPartName_OnValueChanged(XUiController _sender, string _text, bool _validInput, bool _changeFromCode)
	{
		if (_changeFromCode || !_validInput)
		{
			return;
		}
		this.modifyCurrentMarker(delegate(Marker _volumeCopy)
		{
			_volumeCopy.PartToSpawn = _text;
		});
	}

	// Token: 0x0600887D RID: 34941 RVA: 0x0034238C File Offset: 0x0034058C
	[PublicizedFrom(EAccessModifier.Private)]
	public void GroupName_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (_changeFromCode)
		{
			return;
		}
		if (!this.modifyCurrentMarker(delegate(Marker _volumeCopy)
		{
			_volumeCopy.GroupName = _text;
		}))
		{
			return;
		}
		POIMarkerToolManager.UpdateAllColors();
		this.markerList.RefreshBindingsSelfAndChildren();
	}

	// Token: 0x0600887E RID: 34942 RVA: 0x003423D0 File Offset: 0x003405D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkerType_OnValueChanged(XUiController _sender, Marker.MarkerTypes _oldValue, Marker.MarkerTypes _newValue)
	{
		this.modifyCurrentMarker(delegate(Marker _volumeCopy)
		{
			_volumeCopy.MarkerType = _newValue;
			if (_volumeCopy.MarkerType == Marker.MarkerTypes.POISpawn)
			{
				Vector3i size = _volumeCopy.size;
				size.y = 0;
				_volumeCopy.size = size;
			}
		});
	}

	// Token: 0x0600887F RID: 34943 RVA: 0x00342400 File Offset: 0x00340600
	[PublicizedFrom(EAccessModifier.Private)]
	public void MarkerSize_OnValueChanged(XUiController _sender, Marker.MarkerSize _oldValue, Marker.MarkerSize _newValue)
	{
		if (_newValue == Marker.MarkerSize.Custom)
		{
			return;
		}
		this.modifyCurrentMarker(delegate(Marker _volumeCopy)
		{
			_volumeCopy.size = Marker.MarkerSizes[(int)_newValue];
		});
	}

	// Token: 0x06008880 RID: 34944 RVA: 0x00342438 File Offset: 0x00340638
	[PublicizedFrom(EAccessModifier.Private)]
	public bool StartSize_ModifyVector(string _text, bool _changeFromCode, int _dimension, Vector3i _currentValue, out Vector3i _newValue)
	{
		_newValue = _currentValue;
		if (_text.Length == 0 || _changeFromCode)
		{
			return false;
		}
		int num;
		if (!StringParsers.TryParseSInt32(_text, out num))
		{
			return false;
		}
		switch (_dimension)
		{
		case 0:
			_newValue.x = num;
			break;
		case 1:
			_newValue.y = num;
			break;
		case 2:
			_newValue.z = num;
			break;
		default:
			throw new ArgumentOutOfRangeException("_dimension", _dimension, "_dimension must be between 0 and 2");
		}
		return true;
	}

	// Token: 0x06008881 RID: 34945 RVA: 0x003424B4 File Offset: 0x003406B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void modifyStartPos(string _text, bool _changeFromCode, int _dimension)
	{
		SelectionBox selectionBox;
		Marker marker;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<Marker>(SelectionBoxManager.Instance.CategoryPOIMarker, out selectionBox, out marker, out prefabInstance, out num))
		{
			return;
		}
		Vector3i vector3i;
		if (!this.StartSize_ModifyVector(_text, _changeFromCode, _dimension, marker.startPos, out vector3i))
		{
			return;
		}
		Marker marker2 = marker.CloneGeneric();
		marker2.startPos = vector3i;
		PrefabVolumeManager.Instance.UpdatePropertiesServer(prefabInstance.id, num, marker2);
	}

	// Token: 0x06008882 RID: 34946 RVA: 0x00342514 File Offset: 0x00340714
	[PublicizedFrom(EAccessModifier.Private)]
	public void modifySize(string _text, bool _changeFromCode, int _dimension)
	{
		SelectionBox selectionBox;
		Marker marker;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<Marker>(SelectionBoxManager.Instance.CategoryPOIMarker, out selectionBox, out marker, out prefabInstance, out num))
		{
			return;
		}
		Vector3i vector3i;
		if (!this.StartSize_ModifyVector(_text, _changeFromCode, _dimension, marker.size, out vector3i))
		{
			return;
		}
		if (marker.MarkerType == Marker.MarkerTypes.POISpawn)
		{
			vector3i.y = 0;
			return;
		}
		Marker marker2 = marker.CloneGeneric();
		marker2.size = vector3i;
		PrefabVolumeManager.Instance.UpdatePropertiesServer(prefabInstance.id, num, marker2);
	}

	// Token: 0x06008883 RID: 34947 RVA: 0x00342584 File Offset: 0x00340784
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		base.Update(_dt);
	}

	// Token: 0x06008884 RID: 34948 RVA: 0x00342594 File Offset: 0x00340794
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "iscustomsize")
		{
			_value = (this.markerSize != null && this.markerSize.Value == Marker.MarkerSize.Custom).ToString();
			return true;
		}
		if (_bindingName == "markertype")
		{
			_value = ((this.markerType != null) ? this.markerType.Value.ToStringCached<Marker.MarkerTypes>() : "None");
			return true;
		}
		if (!(_bindingName == "markersize"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = ((this.markerSize != null) ? this.markerSize.Value.ToStringCached<Marker.MarkerSize>() : "One");
		return true;
	}

	// Token: 0x06008885 RID: 34949 RVA: 0x0034263D File Offset: 0x0034083D
	public override void OnOpen()
	{
		this.ignoreSelectionChange = true;
		base.OnOpen();
		this.ignoreSelectionChange = false;
		this.updateValues();
		base.RefreshBindings();
	}

	// Token: 0x06008886 RID: 34950 RVA: 0x00342660 File Offset: 0x00340860
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateValues()
	{
		SelectionBox selectionBox;
		Marker marker;
		PrefabInstance prefabInstance;
		int num;
		PrefabVolumeManager.TryGetSelectedVolume<Marker>(SelectionBoxManager.Instance.CategoryPOIMarker, out selectionBox, out marker, out prefabInstance, out num);
		XUiC_TextInput xuiC_TextInput = this.startX;
		string text;
		if (marker == null)
		{
			text = null;
		}
		else
		{
			Vector3i vector3i = marker.startPos;
			text = vector3i.x.ToString();
		}
		xuiC_TextInput.Text = text ?? "";
		XUiC_TextInput xuiC_TextInput2 = this.startY;
		string text2;
		if (marker == null)
		{
			text2 = null;
		}
		else
		{
			Vector3i vector3i = marker.startPos;
			text2 = vector3i.y.ToString();
		}
		xuiC_TextInput2.Text = text2 ?? "";
		XUiC_TextInput xuiC_TextInput3 = this.startZ;
		string text3;
		if (marker == null)
		{
			text3 = null;
		}
		else
		{
			Vector3i vector3i = marker.startPos;
			text3 = vector3i.z.ToString();
		}
		xuiC_TextInput3.Text = text3 ?? "";
		if (this.markerSize != null)
		{
			if (marker != null && Marker.MarkerSizes.Contains(marker.size))
			{
				this.markerSize.Value = (Marker.MarkerSize)Marker.MarkerSizes.IndexOf(marker.size);
			}
			else
			{
				this.markerSize.Value = Marker.MarkerSize.Custom;
			}
		}
		if (this.sizeX != null)
		{
			XUiC_TextInput xuiC_TextInput4 = this.sizeX;
			string text4;
			if (marker == null)
			{
				text4 = null;
			}
			else
			{
				Vector3i vector3i = marker.size;
				text4 = vector3i.x.ToString();
			}
			xuiC_TextInput4.Text = text4 ?? "";
		}
		if (this.sizeY != null)
		{
			XUiC_TextInput xuiC_TextInput5 = this.sizeY;
			string text5;
			if (marker == null)
			{
				text5 = null;
			}
			else
			{
				Vector3i vector3i = marker.size;
				text5 = vector3i.y.ToString();
			}
			xuiC_TextInput5.Text = text5 ?? "";
		}
		if (this.sizeZ != null)
		{
			XUiC_TextInput xuiC_TextInput6 = this.sizeZ;
			string text6;
			if (marker == null)
			{
				text6 = null;
			}
			else
			{
				Vector3i vector3i = marker.size;
				text6 = vector3i.z.ToString();
			}
			xuiC_TextInput6.Text = text6 ?? "";
		}
		if (this.markerType != null)
		{
			this.markerType.Value = ((marker != null) ? marker.MarkerType : Marker.MarkerTypes.None);
		}
		if (this.groupName != null)
		{
			this.groupName.Text = ((marker != null) ? marker.GroupName : null) ?? "";
		}
		if (this.tags != null)
		{
			this.tags.Text = ((marker != null) ? marker.Tags.ToString() : null) ?? "";
		}
		if (this.txtPoiMarkerPartName != null)
		{
			this.txtPoiMarkerPartName.Text = ((marker != null) ? marker.PartToSpawn : null) ?? "";
		}
		if (this.rotations != null)
		{
			this.rotations.Value = (long)((ulong)((marker != null) ? marker.Rotations : 0));
		}
		if (this.partSpawnChance != null)
		{
			this.partSpawnChance.Value = (double)((float)Mathf.RoundToInt(((marker != null) ? marker.PartChanceToSpawn : 0f) * 100f) / 100f);
		}
		if (this.markerList != null && marker != null)
		{
			this.markerList.SelectByMarker(marker);
		}
	}

	// Token: 0x06008887 RID: 34951 RVA: 0x0034290C File Offset: 0x00340B0C
	public void CheckSpecialKeys(Event _ev, PlayerActionsLocal _playerActions)
	{
		if ((_ev.modifiers & EventModifiers.Control) != EventModifiers.None && (_ev.modifiers & EventModifiers.Shift) != EventModifiers.None && _ev.keyCode == KeyCode.Return)
		{
			this.spawnNewMarker();
			_ev.Use();
		}
		if ((_ev.modifiers & EventModifiers.Control) != EventModifiers.None && (_ev.modifiers & EventModifiers.Shift) != EventModifiers.None && _ev.keyCode == KeyCode.A)
		{
			_ev.Use();
			if (PrefabEditModeManager.Instance.VoxelPrefab != null)
			{
				foreach (KeyValuePair<string, SelectionBox> keyValuePair in SelectionBoxManager.Instance.CategoryPOIMarker.boxes)
				{
					string text;
					SelectionBox selectionBox;
					keyValuePair.Deconstruct(out text, out selectionBox);
					SelectionBox selectionBox2 = selectionBox;
					Marker marker = selectionBox2.UserData as Marker;
					if (marker != null && marker.MarkerType == Marker.MarkerTypes.POISpawn)
					{
						POIMarkerToolManager.DisplayPrefabPreviewForMarker(selectionBox2);
					}
				}
			}
		}
	}

	// Token: 0x06008888 RID: 34952 RVA: 0x003429F0 File Offset: 0x00340BF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void spawnNewMarker()
	{
		Vector3 raycastHitPoint = XUiC_LevelTools3Window.getRaycastHitPoint(1000f, 0f);
		if (raycastHitPoint.Equals(Vector3.zero))
		{
			return;
		}
		Vector3i vector3i = new Vector3i(1, 1, 1);
		Vector3i vector3i2 = World.worldToBlockPos(raycastHitPoint) - new Vector3i(vector3i.x / 2, 0, vector3i.z / 2);
		PrefabVolumeManager.Instance.AddVolumeServer(PrefabVolumeAbs.EVolumeType.Marker, vector3i2, vector3i);
	}

	// Token: 0x06008889 RID: 34953 RVA: 0x00342A55 File Offset: 0x00340C55
	public override void Cleanup()
	{
		base.Cleanup();
		if (SelectionBoxManager.Instance != null)
		{
			SelectionCategory categoryPOIMarker = SelectionBoxManager.Instance.CategoryPOIMarker;
			categoryPOIMarker.BoxCallbacks = null;
			categoryPOIMarker.CategoryCallbacks = null;
		}
		POIMarkerToolManager.CleanUp();
		XUiC_WoPropsPOIMarker.Instance = null;
	}

	// Token: 0x0600888A RID: 34954 RVA: 0x00342A8C File Offset: 0x00340C8C
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		this.updateValues();
		POIMarkerToolManager.SelectionChanged(_bActivated ? _box : null);
		return true;
	}

	// Token: 0x0600888B RID: 34955 RVA: 0x00342AA1 File Offset: 0x00340CA1
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		PrefabVolumeManager.Instance.SelectionBoxMoved(PrefabVolumeAbs.EVolumeType.Marker, _box, _moveVector);
		POIMarkerToolManager.UpdateAllColors();
	}

	// Token: 0x0600888C RID: 34956 RVA: 0x00342AB8 File Offset: 0x00340CB8
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		SelectionBox selectionBox;
		Marker marker;
		PrefabInstance prefabInstance;
		int num;
		if (!PrefabVolumeManager.TryGetSelectedVolume<Marker>(SelectionBoxManager.Instance.CategoryPOIMarker, out selectionBox, out marker, out prefabInstance, out num))
		{
			return;
		}
		if (marker.MarkerType == Marker.MarkerTypes.PartSpawn)
		{
			return;
		}
		if (marker.MarkerType == Marker.MarkerTypes.POISpawn)
		{
			_dTop = 0;
			_dBottom = 0;
		}
		PrefabVolumeManager.Instance.SelectionBoxSized(PrefabVolumeAbs.EVolumeType.Marker, _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
		POIMarkerToolManager.UpdateAllColors();
	}

	// Token: 0x0600888D RID: 34957 RVA: 0x00342B13 File Offset: 0x00340D13
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
		POIMarkerToolManager.UpdateAllColors();
	}

	// Token: 0x0600888E RID: 34958 RVA: 0x00342B1A File Offset: 0x00340D1A
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		return PrefabVolumeManager.Instance.SelectionBoxDelete(PrefabVolumeAbs.EVolumeType.Marker, _box, _checkCanDeleteOnly);
	}

	// Token: 0x0600888F RID: 34959 RVA: 0x003376F1 File Offset: 0x003358F1
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties || _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x06008890 RID: 34960 RVA: 0x00342B29 File Offset: 0x00340D29
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		_windowManager.SwitchVisible(this.windowGroup, true);
	}

	// Token: 0x06008891 RID: 34961 RVA: 0x00342B38 File Offset: 0x00340D38
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
		this.modifyCurrentMarker(delegate(Marker _volumeCopy)
		{
			_volumeCopy.Rotations = (_volumeCopy.Rotations + 1) % 4;
		});
	}

	// Token: 0x06008892 RID: 34962 RVA: 0x00342B60 File Offset: 0x00340D60
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
		POIMarkerToolManager.RegisterPoiMarker(_box);
		POIMarkerToolManager.UpdateAllColors();
		this.ignoreSelectionChange = true;
		this.markerList.RebuildList(false);
		this.ignoreSelectionChange = false;
		this.updatePartSpawnerSize(_box);
		this.updateValues();
	}

	// Token: 0x06008893 RID: 34963 RVA: 0x00342B94 File Offset: 0x00340D94
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePartSpawnerSize(SelectionBox _selBox)
	{
		Marker marker = _selBox.UserData as Marker;
		if (marker == null)
		{
			return;
		}
		if (marker.MarkerType != Marker.MarkerTypes.PartSpawn)
		{
			return;
		}
		if (string.IsNullOrEmpty(marker.PartToSpawn))
		{
			return;
		}
		PrefabData prefabData;
		if (!POIMarkerToolManager.AllPrefabData.TryGetValue(marker.PartToSpawn, out prefabData))
		{
			Log.Error("Part '" + marker.PartToSpawn + "' not found!");
			return;
		}
		Vector3i size;
		if ((prefabData.rotationToFaceNorth + marker.Rotations) % 2 == 1)
		{
			size = new Vector3i(prefabData.size.z, prefabData.size.y, prefabData.size.x);
		}
		else
		{
			size = prefabData.size;
		}
		int num;
		PrefabInstance prefabInstance;
		if (marker.size == size || !PrefabVolumeManager.GetPrefabIdAndVolumeId(_selBox.name, out num, out prefabInstance))
		{
			return;
		}
		Marker marker2 = marker.CloneGeneric();
		marker2.size = size;
		PrefabVolumeManager.Instance.UpdatePropertiesServer(prefabInstance.id, num, marker2);
	}

	// Token: 0x06008894 RID: 34964 RVA: 0x00342B13 File Offset: 0x00340D13
	public void OnSelectionCategoryVisibilityChanged(SelectionCategory _category, bool _visible)
	{
		POIMarkerToolManager.UpdateAllColors();
	}

	// Token: 0x06008895 RID: 34965 RVA: 0x00342C7F File Offset: 0x00340E7F
	public void OnSelectionCategoryBoxAdded(SelectionBox _box)
	{
		POIMarkerToolManager.RegisterPoiMarker(_box);
		this.ignoreSelectionChange = true;
		this.markerList.RebuildList(false);
		this.ignoreSelectionChange = false;
	}

	// Token: 0x06008896 RID: 34966 RVA: 0x00342CA1 File Offset: 0x00340EA1
	public void OnSelectionCategoryBoxRemoved(SelectionBox _box)
	{
		POIMarkerToolManager.UnRegisterPoiMarker(_box);
		this.ignoreSelectionChange = true;
		this.markerList.RebuildList(false);
		this.ignoreSelectionChange = false;
	}

	// Token: 0x06008897 RID: 34967 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionCategoryCleared(SelectionCategory _category)
	{
	}

	// Token: 0x040065AD RID: 26029
	public static string ID = "";

	// Token: 0x040065AE RID: 26030
	public static XUiC_WoPropsPOIMarker Instance;

	// Token: 0x040065AF RID: 26031
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput startX;

	// Token: 0x040065B0 RID: 26032
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput startY;

	// Token: 0x040065B1 RID: 26033
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput startZ;

	// Token: 0x040065B2 RID: 26034
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput sizeX;

	// Token: 0x040065B3 RID: 26035
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput sizeY;

	// Token: 0x040065B4 RID: 26036
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput sizeZ;

	// Token: 0x040065B5 RID: 26037
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput groupName;

	// Token: 0x040065B6 RID: 26038
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput tags;

	// Token: 0x040065B7 RID: 26039
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<Marker.MarkerSize> markerSize;

	// Token: 0x040065B8 RID: 26040
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<Marker.MarkerTypes> markerType;

	// Token: 0x040065B9 RID: 26041
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DropDown txtPoiMarkerPartName;

	// Token: 0x040065BA RID: 26042
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabMarkerList markerList;

	// Token: 0x040065BB RID: 26043
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxInt rotations;

	// Token: 0x040065BC RID: 26044
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat partSpawnChance;

	// Token: 0x040065BD RID: 26045
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreSelectionChange;
}
