using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000A6F RID: 2671
public class SelectionBoxManager : MonoBehaviour
{
	// Token: 0x170008C9 RID: 2249
	// (get) Token: 0x06005058 RID: 20568 RVA: 0x001EDA87 File Offset: 0x001EBC87
	// (set) Token: 0x06005059 RID: 20569 RVA: 0x001EDA8F File Offset: 0x001EBC8F
	public SelectionCategory CategoryDynamicPrefab
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008CA RID: 2250
	// (get) Token: 0x0600505A RID: 20570 RVA: 0x001EDA98 File Offset: 0x001EBC98
	// (set) Token: 0x0600505B RID: 20571 RVA: 0x001EDAA0 File Offset: 0x001EBCA0
	public SelectionCategory CategoryStartPoint
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008CB RID: 2251
	// (get) Token: 0x0600505C RID: 20572 RVA: 0x001EDAA9 File Offset: 0x001EBCA9
	// (set) Token: 0x0600505D RID: 20573 RVA: 0x001EDAB1 File Offset: 0x001EBCB1
	public SelectionCategory CategorySelection
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x0600505E RID: 20574 RVA: 0x001EDABA File Offset: 0x001EBCBA
	// (set) Token: 0x0600505F RID: 20575 RVA: 0x001EDAC2 File Offset: 0x001EBCC2
	public SelectionCategory CategoryTraderTeleport
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x06005060 RID: 20576 RVA: 0x001EDACB File Offset: 0x001EBCCB
	// (set) Token: 0x06005061 RID: 20577 RVA: 0x001EDAD3 File Offset: 0x001EBCD3
	public SelectionCategory CategoryInfoVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x06005062 RID: 20578 RVA: 0x001EDADC File Offset: 0x001EBCDC
	// (set) Token: 0x06005063 RID: 20579 RVA: 0x001EDAE4 File Offset: 0x001EBCE4
	public SelectionCategory CategoryWallVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x06005064 RID: 20580 RVA: 0x001EDAED File Offset: 0x001EBCED
	// (set) Token: 0x06005065 RID: 20581 RVA: 0x001EDAF5 File Offset: 0x001EBCF5
	public SelectionCategory CategoryTriggerVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x06005066 RID: 20582 RVA: 0x001EDAFE File Offset: 0x001EBCFE
	// (set) Token: 0x06005067 RID: 20583 RVA: 0x001EDB06 File Offset: 0x001EBD06
	public SelectionCategory CategorySleeperVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x06005068 RID: 20584 RVA: 0x001EDB0F File Offset: 0x001EBD0F
	// (set) Token: 0x06005069 RID: 20585 RVA: 0x001EDB17 File Offset: 0x001EBD17
	public SelectionCategory CategoryPOIMarker
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x0600506A RID: 20586 RVA: 0x001EDB20 File Offset: 0x001EBD20
	// (set) Token: 0x0600506B RID: 20587 RVA: 0x001EDB28 File Offset: 0x001EBD28
	public SelectionCategory CategoryPrefabFacingVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x0600506C RID: 20588 RVA: 0x001EDB31 File Offset: 0x001EBD31
	// (set) Token: 0x0600506D RID: 20589 RVA: 0x001EDB39 File Offset: 0x001EBD39
	public SelectionBox Selection
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x0600506E RID: 20590 RVA: 0x001EDB42 File Offset: 0x001EBD42
	// (set) Token: 0x0600506F RID: 20591 RVA: 0x001EDB4A File Offset: 0x001EBD4A
	public float AlphaMultiplier
	{
		get
		{
			return this.alphaMultiplier;
		}
		set
		{
			this.alphaMultiplier = Mathf.Clamp01(value);
			GamePrefs.Set(EnumGamePrefs.OptionsSelectionBoxAlphaMultiplier, this.alphaMultiplier);
			this.UpdateAllColors();
		}
	}

	// Token: 0x06005070 RID: 20592 RVA: 0x001EDB6E File Offset: 0x001EBD6E
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		SelectionBoxManager.Instance = this;
		Origin.Add(base.transform, 1);
		this.alphaMultiplier = GamePrefs.GetFloat(EnumGamePrefs.OptionsSelectionBoxAlphaMultiplier);
	}

	// Token: 0x06005071 RID: 20593 RVA: 0x001EDB94 File Offset: 0x001EBD94
	public void SetupCategories()
	{
		Color color = new Color(0f, 0f, 1f, 0.5f);
		Color color2 = new Color(0f, 0f, 1f, 0.5f);
		Color color3 = new Color(1f, 1f, 0f, 0.4f);
		Color color4 = new Color(1f, 1f, 1f, 0.5f);
		Color color5 = new Color(1f, 1f, 0f, 0.8f);
		Color color6 = new Color(0f, 0.4f, 0f, 0.6f);
		Color color7 = new Color(0.6f, 1f, 0f, 0.15f);
		Color color8 = new Color(0f, 1f, 0f, 0.6f);
		Color color9 = new Color(0.5f, 0f, 0.5f, 0.6f);
		Color color10 = new Color(1f, 0f, 1f, 0.3f);
		Color color11 = new Color(0.7f, 0.75f, 1f, 0.3f);
		Color color12 = new Color(0.25f, 0.25f, 0.5f, 0.6f);
		Color color13 = new Color(0f, 1f, 1f, 0.4f);
		Color color14 = new Color(0f, 0.6f, 0.6f, 0.6f);
		Color color15 = new Color(0.5f, 1f, 1f, 0.4f);
		Color color16 = new Color(0.5f, 0.6f, 0.6f, 0.6f);
		Color color17 = new Color(1f, 0f, 0f, 0.4f);
		Color color18 = new Color(0.6f, 0f, 0f, 0.6f);
		this.CategorySelection = this.CreateCategory("Selection", color, color2, color3, false, null, 0);
		this.CategoryStartPoint = this.CreateCategory("StartPoint", color5, color4, color5, true, "SB_StartPoint", 31);
		this.CategoryDynamicPrefab = this.CreateCategory("DynamicPrefabs", color7, color6, color8, true, "SB_Prefabs", 31);
		this.CategoryTraderTeleport = this.CreateCategory("TraderTeleport", color10, color9, color10, true, "SB_TraderTeleport", 31);
		this.CategorySleeperVolume = this.CreateCategory("SleeperVolume", color11, color12, color11, true, "SB_SleeperVolume", 31);
		this.CategoryInfoVolume = this.CreateCategory("InfoVolume", color13, color14, color13, true, "SB_InfoVolume", 31);
		this.CategoryWallVolume = this.CreateCategory("WallVolume", color15, color16, color15, true, "SB_WallVolume", 31);
		this.CategoryTriggerVolume = this.CreateCategory("TriggerVolume", color17, color18, color17, true, "SB_TriggerVolume", 31);
		this.CategoryPOIMarker = this.CreateCategory("POIMarker", color7, color6, color8, true, "SB_Prefabs", 31);
		this.CategoryPrefabFacingVolume = this.CreateCategory("PrefabFacing", color11, color12, color11, true, "SB_SleeperVolume", 31);
	}

	// Token: 0x06005072 RID: 20594 RVA: 0x001EDEB4 File Offset: 0x001EC0B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		Origin.Remove(base.transform);
	}

	// Token: 0x06005073 RID: 20595 RVA: 0x001EDEC4 File Offset: 0x001EC0C4
	public SelectionCategory GetCategory(string _name)
	{
		SelectionCategory selectionCategory;
		this.categories.TryGetValue(_name, out selectionCategory);
		return selectionCategory;
	}

	// Token: 0x06005074 RID: 20596 RVA: 0x001EDEE1 File Offset: 0x001EC0E1
	public bool TryGetSelectionBox(string _category, string _name, out SelectionBox _selectionBox)
	{
		SelectionCategory category = this.GetCategory(_category);
		_selectionBox = ((category != null) ? category.GetBox(_name) : null);
		return _selectionBox != null;
	}

	// Token: 0x06005075 RID: 20597 RVA: 0x001EDF04 File Offset: 0x001EC104
	public SelectionCategory CreateCategory(string _name, Color _colSelected, Color _colUnselected, Color _colFaceSelected, bool _bCollider, string _tag, int _layer = 0)
	{
		Transform transform = new GameObject(_name).transform;
		transform.parent = base.transform;
		SelectionCategory selectionCategory = new SelectionCategory(_name, transform, _colSelected, _colUnselected, _colFaceSelected, _bCollider, _tag, null, _layer);
		selectionCategory.SetVisible(false);
		this.categories[_name] = selectionCategory;
		return selectionCategory;
	}

	// Token: 0x06005076 RID: 20598 RVA: 0x001EDF51 File Offset: 0x001EC151
	public void SetActive(SelectionBox _box, bool _bActive)
	{
		if (_box == null)
		{
			return;
		}
		if (!_box.Category.IsVisible())
		{
			_box.Category.SetVisible(true);
		}
		this.activate(_bActive ? _box : null);
	}

	// Token: 0x06005077 RID: 20599 RVA: 0x001EDF84 File Offset: 0x001EC184
	public void Deactivate()
	{
		this.activate(null);
	}

	// Token: 0x06005078 RID: 20600 RVA: 0x001EDF8E File Offset: 0x001EC18E
	public bool TryGetSelected(out SelectionBox _selectedBox)
	{
		_selectedBox = this.Selection;
		return _selectedBox != null;
	}

	// Token: 0x06005079 RID: 20601 RVA: 0x001EDFA0 File Offset: 0x001EC1A0
	public bool Select(WorldRayHitInfo _hitInfo)
	{
		if (_hitInfo.tag == null)
		{
			return false;
		}
		foreach (KeyValuePair<string, SelectionCategory> keyValuePair in this.categories)
		{
			string text;
			SelectionCategory selectionCategory;
			keyValuePair.Deconstruct(out text, out selectionCategory);
			SelectionCategory selectionCategory2 = selectionCategory;
			if (_hitInfo.tag.Equals(selectionCategory2.tag))
			{
				foreach (KeyValuePair<string, SelectionBox> keyValuePair2 in selectionCategory2.boxes)
				{
					SelectionBox selectionBox;
					keyValuePair2.Deconstruct(out text, out selectionBox);
					SelectionBox selectionBox2 = selectionBox;
					if (!(selectionBox2.GetBoxTransform() != _hitInfo.transform))
					{
						Manager.PlayButtonClick();
						return this.activate(selectionBox2);
					}
				}
			}
		}
		return false;
	}

	// Token: 0x0600507A RID: 20602 RVA: 0x001EE094 File Offset: 0x001EC294
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activate(SelectionBox _box)
	{
		SelectionBox selection = this.Selection;
		bool flag = true;
		if (selection == _box || _box == null)
		{
			this.Selection = null;
		}
		else
		{
			this.Selection = _box;
			_box.SetFrameActive(true);
			_box.SetAllFacesColor(_box.Category.colActive, true);
		}
		if (selection != null)
		{
			selection.SetFrameActive(false);
			selection.SetAllFacesColor(selection.Category.colInactive, true);
			ISelectionBoxCallback boxCallbacks = selection.Category.BoxCallbacks;
			if (boxCallbacks != null)
			{
				boxCallbacks.OnSelectionBoxActivated(selection, false);
			}
		}
		if (this.Selection != null && this.Selection.Category.BoxCallbacks != null)
		{
			flag = this.Selection.Category.BoxCallbacks.OnSelectionBoxActivated(this.Selection, true);
		}
		return flag;
	}

	// Token: 0x0600507B RID: 20603 RVA: 0x001EE160 File Offset: 0x001EC360
	public void UpdateAllColors()
	{
		foreach (KeyValuePair<string, SelectionCategory> keyValuePair in this.categories)
		{
			string text;
			SelectionCategory selectionCategory;
			keyValuePair.Deconstruct(out text, out selectionCategory);
			SelectionCategory selectionCategory2 = selectionCategory;
			foreach (KeyValuePair<string, SelectionBox> keyValuePair2 in selectionCategory2.boxes)
			{
				SelectionBox selectionBox;
				keyValuePair2.Deconstruct(out text, out selectionBox);
				SelectionBox selectionBox2 = selectionBox;
				Color color = ((this.Selection == selectionBox2) ? selectionCategory2.colActive : selectionCategory2.colInactive);
				selectionBox2.SetAllFacesColor(color, true);
			}
		}
	}

	// Token: 0x0600507C RID: 20604 RVA: 0x001EE234 File Offset: 0x001EC434
	public void Clear()
	{
		foreach (KeyValuePair<string, SelectionCategory> keyValuePair in this.categories)
		{
			string text;
			SelectionCategory selectionCategory;
			keyValuePair.Deconstruct(out text, out selectionCategory);
			selectionCategory.Clear();
		}
	}

	// Token: 0x0600507D RID: 20605 RVA: 0x001EE294 File Offset: 0x001EC494
	public void OpenPropertiesWindow(GUIWindowManager _windowManager)
	{
		if (this.Selection == null)
		{
			return;
		}
		SelectionCategory category = this.Selection.Category;
		ISelectionBoxCallback boxCallbacks = category.BoxCallbacks;
		if (boxCallbacks == null || !boxCallbacks.OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities.CanShowProperties))
		{
			return;
		}
		Manager.PlayButtonClick();
		category.BoxCallbacks.OnSelectionBoxShowProperties(true, _windowManager);
	}

	// Token: 0x0600507E RID: 20606 RVA: 0x001EE2E4 File Offset: 0x001EC4E4
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i createBlockMoveVector(Vector3 _relPlayerAxis)
	{
		Vector3i vector3i;
		if (Math.Abs(_relPlayerAxis.x) > Math.Abs(_relPlayerAxis.z))
		{
			vector3i = new Vector3i(Mathf.Sign(_relPlayerAxis.x), 0f, 0f);
		}
		else
		{
			vector3i = new Vector3i(0f, 0f, Mathf.Sign(_relPlayerAxis.z));
		}
		return vector3i;
	}

	// Token: 0x0600507F RID: 20607 RVA: 0x001EE344 File Offset: 0x001EC544
	[PublicizedFrom(EAccessModifier.Private)]
	public void moveSelection(Vector3i _deltaVec)
	{
		SelectionBox selection = this.Selection;
		if (((selection != null) ? selection.Category.BoxCallbacks : null) == null)
		{
			return;
		}
		this.Selection.Category.BoxCallbacks.OnSelectionBoxMoved(this.Selection, _deltaVec.ToVector3());
	}

	// Token: 0x06005080 RID: 20608 RVA: 0x001EE384 File Offset: 0x001EC584
	[PublicizedFrom(EAccessModifier.Private)]
	public void incSelection(int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		SelectionBox selection = this.Selection;
		if (((selection != null) ? selection.Category.BoxCallbacks : null) == null)
		{
			return;
		}
		if (!this.Selection.Category.BoxCallbacks.OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities.CanResize))
		{
			return;
		}
		this.Selection.Category.BoxCallbacks.OnSelectionBoxSized(this.Selection, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
	}

	// Token: 0x06005081 RID: 20609 RVA: 0x001EE3E8 File Offset: 0x001EC5E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void mirrorSelection(Vector3i _axis)
	{
		SelectionBox selection = this.Selection;
		if (((selection != null) ? selection.Category.BoxCallbacks : null) == null)
		{
			return;
		}
		if (!this.Selection.Category.BoxCallbacks.OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities.CanMirror))
		{
			return;
		}
		this.Selection.Category.BoxCallbacks.OnSelectionBoxMirrored(_axis);
	}

	// Token: 0x06005082 RID: 20610 RVA: 0x001EE440 File Offset: 0x001EC640
	[PublicizedFrom(EAccessModifier.Private)]
	public void showThroughWalls(SelectionCategory _category, bool _isThroughWalls, bool _isAll)
	{
		if (_isAll)
		{
			using (Dictionary<string, SelectionBox>.Enumerator enumerator = _category.boxes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, SelectionBox> keyValuePair = enumerator.Current;
					string text;
					SelectionBox selectionBox;
					keyValuePair.Deconstruct(out text, out selectionBox);
					selectionBox.ShowThroughWalls(_isThroughWalls);
				}
				return;
			}
		}
		this.Selection.ShowThroughWalls(_isThroughWalls);
	}

	// Token: 0x06005083 RID: 20611 RVA: 0x001EE4B0 File Offset: 0x001EC6B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void deleteBox(SelectionBox _selBox)
	{
		this.SetActive(_selBox, false);
		if (_selBox.Category.BoxCallbacks.OnSelectionBoxDelete(_selBox, false))
		{
			Manager.PlayButtonClick();
			_selBox.Category.RemoveBox(_selBox);
			this.Selection = null;
		}
	}

	// Token: 0x06005084 RID: 20612 RVA: 0x001EE4E8 File Offset: 0x001EC6E8
	public bool ConsumeScrollWheel(float _scrollWheel, PlayerActionsLocal _playerActions)
	{
		SelectionBox selection = this.Selection;
		if (((selection != null) ? selection.Category.BoxCallbacks : null) == null)
		{
			return false;
		}
		if (Mathf.Abs(_scrollWheel) < 0.1f)
		{
			return false;
		}
		if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) != 2)
		{
			return false;
		}
		int num = Mathf.RoundToInt(_scrollWheel * 10f);
		bool controlKeyPressed = InputUtils.ControlKeyPressed;
		bool flag = false;
		if (_playerActions.Jump.IsPressed)
		{
			this.incSelection(num, 0, 0, 0, 0, 0);
			flag = true;
		}
		if (_playerActions.Crouch.IsPressed && !controlKeyPressed)
		{
			this.incSelection(0, num, 0, 0, 0, 0);
			flag = true;
		}
		if (_playerActions.MoveLeft.IsPressed)
		{
			this.incSelection(0, 0, 0, 0, num, 0);
			flag = true;
		}
		if (_playerActions.MoveRight.IsPressed)
		{
			this.incSelection(0, 0, 0, 0, 0, num);
			flag = true;
		}
		if (_playerActions.MoveForward.IsPressed)
		{
			this.incSelection(0, 0, num, 0, 0, 0);
			flag = true;
		}
		if (_playerActions.MoveBack.IsPressed)
		{
			this.incSelection(0, 0, 0, num, 0, 0);
			flag = true;
		}
		return flag;
	}

	// Token: 0x06005085 RID: 20613 RVA: 0x001EE5E8 File Offset: 0x001EC7E8
	public void CheckKeys(GameManager _gameManager, PlayerActionsLocal _playerActions, WorldRayHitInfo _hitInfo)
	{
		bool altKeyPressed = InputUtils.AltKeyPressed;
		GameManager.bVolumeBlocksEditing = !altKeyPressed;
		foreach (KeyValuePair<string, SelectionCategory> keyValuePair in this.categories)
		{
			string text;
			SelectionCategory selectionCategory;
			keyValuePair.Deconstruct(out text, out selectionCategory);
			SelectionCategory selectionCategory2 = selectionCategory;
			if (selectionCategory2.IsVisible())
			{
				this.showThroughWalls(selectionCategory2, altKeyPressed, true);
			}
		}
		if (_playerActions.SelectionRotate.WasPressed && !Input.GetKey(KeyCode.Tab))
		{
			SelectionBox selection = this.Selection;
			if (((selection != null) ? selection.Category.BoxCallbacks : null) == null)
			{
				BlockToolSelection.Instance.RotateFocusedBlock(_hitInfo, _playerActions);
			}
			else
			{
				SelectionBox selection2 = this.Selection;
				selection2.Category.BoxCallbacks.OnSelectionBoxRotated(selection2);
			}
		}
		SelectionBox selection3 = this.Selection;
		if (((selection3 != null) ? selection3.Category.BoxCallbacks : null) == null)
		{
			return;
		}
		SelectionBox selBox = this.Selection;
		bool controlKeyPressed = InputUtils.ControlKeyPressed;
		if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1 && GamePrefs.GetInt(EnumGamePrefs.SelectionContextMode) == 1)
		{
			if (_playerActions.MoveBack.WasPressed)
			{
				this.moveSelection(-1 * this.createBlockMoveVector(_gameManager.World.GetPrimaryPlayer().transform.forward));
			}
			if (_playerActions.MoveForward.WasPressed)
			{
				this.moveSelection(this.createBlockMoveVector(_gameManager.World.GetPrimaryPlayer().transform.forward));
			}
			if (_playerActions.MoveLeft.WasPressed)
			{
				this.moveSelection(-1 * this.createBlockMoveVector(_gameManager.World.GetPrimaryPlayer().transform.right));
			}
			if (_playerActions.MoveRight.WasPressed)
			{
				this.moveSelection(this.createBlockMoveVector(_gameManager.World.GetPrimaryPlayer().transform.right));
			}
		}
		else if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1 && GamePrefs.GetInt(EnumGamePrefs.SelectionContextMode) == 0)
		{
			if (_playerActions.MoveBack.WasPressed)
			{
				this.moveSelection(-1 * Vector3i.forward);
			}
			if (_playerActions.MoveForward.WasPressed)
			{
				this.moveSelection(Vector3i.forward);
			}
			if (_playerActions.MoveLeft.WasPressed)
			{
				this.moveSelection(-1 * Vector3i.right);
			}
			if (_playerActions.MoveRight.WasPressed)
			{
				this.moveSelection(Vector3i.right);
			}
		}
		if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1)
		{
			if (_playerActions.Jump.WasPressed)
			{
				this.moveSelection(new Vector3i(0, 1, 0));
			}
			if (_playerActions.Crouch.WasPressed && !controlKeyPressed)
			{
				this.moveSelection(new Vector3i(0, -1, 0));
			}
		}
		if (_playerActions.SelectionMoveMode.WasPressed)
		{
			GamePrefs.Set(EnumGamePrefs.SelectionContextMode, (GamePrefs.GetInt(EnumGamePrefs.SelectionContextMode) + 1) % 2);
		}
		if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 2)
		{
			Color colFaceSelected = selBox.Category.colFaceSelected;
			if (_playerActions.Jump.WasPressed)
			{
				selBox.SetFaceColor(BlockFace.Top, colFaceSelected);
			}
			else if (_playerActions.Jump.WasReleased)
			{
				selBox.ResetAllFacesColor();
			}
			if (_playerActions.Crouch.WasPressed && !controlKeyPressed)
			{
				selBox.SetFaceColor(BlockFace.Bottom, colFaceSelected);
			}
			else if (_playerActions.Crouch.WasReleased)
			{
				selBox.ResetAllFacesColor();
			}
			if (_playerActions.MoveLeft.WasPressed)
			{
				selBox.SetFaceColor(BlockFace.East, colFaceSelected);
			}
			else if (_playerActions.MoveLeft.WasReleased)
			{
				selBox.ResetAllFacesColor();
			}
			if (_playerActions.MoveRight.WasPressed)
			{
				selBox.SetFaceColor(BlockFace.West, colFaceSelected);
			}
			else if (_playerActions.MoveRight.WasReleased)
			{
				selBox.ResetAllFacesColor();
			}
			if (_playerActions.MoveForward.WasPressed)
			{
				selBox.SetFaceColor(BlockFace.North, colFaceSelected);
			}
			else if (_playerActions.MoveForward.WasReleased)
			{
				selBox.ResetAllFacesColor();
			}
			if (_playerActions.MoveBack.WasPressed)
			{
				selBox.SetFaceColor(BlockFace.South, colFaceSelected);
			}
			else if (_playerActions.MoveBack.WasReleased)
			{
				selBox.ResetAllFacesColor();
			}
		}
		if (_playerActions.SelectionDelete.WasPressed)
		{
			if (GamePrefs.GetBool(EnumGamePrefs.OptionsPoiVolumesSkipDeleteConfirmation))
			{
				this.deleteBox(selBox);
			}
			else if (selBox.Category.BoxCallbacks.OnSelectionBoxDelete(selBox, true))
			{
				string text2 = string.Format(Localization.Get("xuiConfirmDeleteVolumeText", false, null), new object[]
				{
					selBox.Category.LocalizedName,
					selBox.Size.ToString(),
					selBox.Position.ToString(),
					selBox.name
				});
				XUiC_MessageBoxWindowGroup.ShowOkCancel(GameManager.Instance.World.GetPrimaryPlayer().PlayerUI.xui, Localization.Get("xuiConfirmDeleteVolumeTitle", false, null), text2, "", delegate
				{
					this.deleteBox(selBox);
				}, null, false, true, false);
			}
			else
			{
				this.deleteBox(selBox);
			}
		}
		SelectionBox selection4 = this.Selection;
		if (selection4 == null)
		{
			return;
		}
		Action checkKeysCallback = selection4.Category.CheckKeysCallback;
		if (checkKeysCallback == null)
		{
			return;
		}
		checkKeysCallback();
	}

	// Token: 0x06005086 RID: 20614 RVA: 0x001EEB4C File Offset: 0x001ECD4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		EntityPlayerLocal primaryPlayer;
		if (this.Selection == null || GameManager.Instance == null || GameManager.Instance.World == null || (primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer()) == null)
		{
			return;
		}
		Camera finalCamera = primaryPlayer.finalCamera;
		if (finalCamera == null)
		{
			return;
		}
		SelectionBox selection = this.Selection;
		if (selection.Axises.Count == 0)
		{
			return;
		}
		if (this.lastSelOpMode != GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode))
		{
			this.lastSelOpMode = GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode);
			this.mouseMoveDir = Vector3.zero;
		}
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 0f;
		Vector3 vector = finalCamera.WorldToScreenPoint(selection.AxisOrigin);
		vector.z = 0f;
		bool flag = true;
		if (!this.bMousedPressed)
		{
			for (int i = 0; i < selection.Axises.Count; i++)
			{
				Vector3 vector2 = finalCamera.WorldToScreenPoint(selection.Axises[i]);
				vector2.z = 0f;
				if (SelectionBoxManager.GetLineDistanceSq(vector, vector2, mousePosition) < 225f)
				{
					this.highlightedAxis = selection.AxisesI[i];
					this.highlightedAxisScreenDir = (vector - vector2).normalized;
					flag = false;
					break;
				}
			}
		}
		if (!this.bMousedPressed && flag)
		{
			this.highlightedAxis = Vector3i.zero;
		}
		selection.HighlightAxis(this.highlightedAxis);
		if (this.bWaitForRelease && Input.GetMouseButton(0))
		{
			return;
		}
		this.bWaitForRelease = false;
		if (!this.highlightedAxis.Equals(Vector3i.zero))
		{
			if (!this.bMousedPressed && Input.GetMouseButtonDown(0))
			{
				this.bMousedPressed = true;
			}
			float magnitude = (primaryPlayer.cameraTransform.position - selection.AxisOrigin).magnitude;
			float num = Math.Max(0.5f, magnitude / 35f);
			this.mouseMoveDir += new Vector3(-Input.GetAxis("Mouse X") * 5f, -Input.GetAxis("Mouse Y") * 5f, 0f) * num;
			float magnitude2 = this.mouseMoveDir.magnitude;
			if (this.bMousedPressed && !Input.GetMouseButtonUp(0) && GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 3)
			{
				this.mirrorSelection(this.highlightedAxis);
				this.bWaitForRelease = true;
			}
			if (this.bMousedPressed && magnitude2 > 5f)
			{
				float num2 = ((!this.highlightedAxis.Equals(Vector3i.one)) ? Vector3.Dot(this.highlightedAxisScreenDir, this.mouseMoveDir) : (-1f * Mathf.Sign(this.mouseMoveDir.y)));
				num2 *= magnitude2 * 0.05f;
				if (Mathf.Abs(num2) > 1f)
				{
					this.mouseMoveDir = Vector3.zero;
					int num3 = (int)Mathf.Sign(num2);
					if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1)
					{
						this.moveSelection(this.highlightedAxis * num3);
					}
					else if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 2)
					{
						this.incSelection((this.highlightedAxis.y > 0) ? (this.highlightedAxis.y * num3) : 0, (this.highlightedAxis.y < 0) ? (-1 * this.highlightedAxis.y * num3) : 0, (this.highlightedAxis.z > 0) ? (this.highlightedAxis.z * num3) : 0, (this.highlightedAxis.z < 0) ? (-1 * this.highlightedAxis.z * num3) : 0, (this.highlightedAxis.x > 0) ? (this.highlightedAxis.x * num3) : 0, (this.highlightedAxis.x < 0) ? (-1 * this.highlightedAxis.x * num3) : 0);
					}
				}
			}
		}
		if (!Input.GetMouseButton(0))
		{
			this.bMousedPressed = false;
			this.mouseMoveDir = Vector3.zero;
		}
	}

	// Token: 0x06005087 RID: 20615 RVA: 0x001EEF4C File Offset: 0x001ED14C
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetLineDistanceSq(Vector3 _lineStart, Vector3 _lineEnd, Vector3 _point)
	{
		Vector3 vector = _lineEnd - _lineStart;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude < 1E-06f)
		{
			return (_point - _lineStart).sqrMagnitude;
		}
		float num = Mathf.Clamp01(Vector3.Dot(_point - _lineStart, vector) / sqrMagnitude);
		return (_lineStart + vector * num - _point).sqrMagnitude;
	}

	// Token: 0x04003E12 RID: 15890
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, SelectionCategory> categories = new Dictionary<string, SelectionCategory>();

	// Token: 0x04003E14 RID: 15892
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float alphaMultiplier = 1f;

	// Token: 0x04003E15 RID: 15893
	public static SelectionBoxManager Instance;

	// Token: 0x04003E16 RID: 15894
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bMousedPressed;

	// Token: 0x04003E17 RID: 15895
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 highlightedAxisScreenDir;

	// Token: 0x04003E18 RID: 15896
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i highlightedAxis = Vector3i.zero;

	// Token: 0x04003E19 RID: 15897
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 mouseMoveDir = Vector3.zero;

	// Token: 0x04003E1A RID: 15898
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int lastSelOpMode;

	// Token: 0x04003E1B RID: 15899
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bWaitForRelease;
}
