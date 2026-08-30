using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000A65 RID: 2661
public class SelectionBoxManager : MonoBehaviour
{
	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x06005084 RID: 20612 RVA: 0x001EC53B File Offset: 0x001EA73B
	// (set) Token: 0x06005085 RID: 20613 RVA: 0x001EC543 File Offset: 0x001EA743
	public SelectionCategory CategoryDynamicPrefab
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x06005086 RID: 20614 RVA: 0x001EC54C File Offset: 0x001EA74C
	// (set) Token: 0x06005087 RID: 20615 RVA: 0x001EC554 File Offset: 0x001EA754
	public SelectionCategory CategoryStartPoint
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x06005088 RID: 20616 RVA: 0x001EC55D File Offset: 0x001EA75D
	// (set) Token: 0x06005089 RID: 20617 RVA: 0x001EC565 File Offset: 0x001EA765
	public SelectionCategory CategorySelection
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x0600508A RID: 20618 RVA: 0x001EC56E File Offset: 0x001EA76E
	// (set) Token: 0x0600508B RID: 20619 RVA: 0x001EC576 File Offset: 0x001EA776
	public SelectionCategory CategoryTraderTeleport
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x0600508C RID: 20620 RVA: 0x001EC57F File Offset: 0x001EA77F
	// (set) Token: 0x0600508D RID: 20621 RVA: 0x001EC587 File Offset: 0x001EA787
	public SelectionCategory CategoryInfoVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x0600508E RID: 20622 RVA: 0x001EC590 File Offset: 0x001EA790
	// (set) Token: 0x0600508F RID: 20623 RVA: 0x001EC598 File Offset: 0x001EA798
	public SelectionCategory CategoryWallVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06005090 RID: 20624 RVA: 0x001EC5A1 File Offset: 0x001EA7A1
	// (set) Token: 0x06005091 RID: 20625 RVA: 0x001EC5A9 File Offset: 0x001EA7A9
	public SelectionCategory CategoryTriggerVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x06005092 RID: 20626 RVA: 0x001EC5B2 File Offset: 0x001EA7B2
	// (set) Token: 0x06005093 RID: 20627 RVA: 0x001EC5BA File Offset: 0x001EA7BA
	public SelectionCategory CategorySleeperVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x06005094 RID: 20628 RVA: 0x001EC5C3 File Offset: 0x001EA7C3
	// (set) Token: 0x06005095 RID: 20629 RVA: 0x001EC5CB File Offset: 0x001EA7CB
	public SelectionCategory CategoryPOIMarker
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008B2 RID: 2226
	// (get) Token: 0x06005096 RID: 20630 RVA: 0x001EC5D4 File Offset: 0x001EA7D4
	// (set) Token: 0x06005097 RID: 20631 RVA: 0x001EC5DC File Offset: 0x001EA7DC
	public SelectionCategory CategoryPrefabFacingVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x06005098 RID: 20632 RVA: 0x001EC5E5 File Offset: 0x001EA7E5
	// (set) Token: 0x06005099 RID: 20633 RVA: 0x001EC5ED File Offset: 0x001EA7ED
	public SelectionBox Selection
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008B4 RID: 2228
	// (get) Token: 0x0600509A RID: 20634 RVA: 0x001EC5F6 File Offset: 0x001EA7F6
	// (set) Token: 0x0600509B RID: 20635 RVA: 0x001EC5FE File Offset: 0x001EA7FE
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

	// Token: 0x0600509C RID: 20636 RVA: 0x001EC622 File Offset: 0x001EA822
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		SelectionBoxManager.Instance = this;
		Origin.Add(base.transform, 1);
		this.alphaMultiplier = GamePrefs.GetFloat(EnumGamePrefs.OptionsSelectionBoxAlphaMultiplier);
	}

	// Token: 0x0600509D RID: 20637 RVA: 0x001EC648 File Offset: 0x001EA848
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

	// Token: 0x0600509E RID: 20638 RVA: 0x001EC968 File Offset: 0x001EAB68
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		Origin.Remove(base.transform);
	}

	// Token: 0x0600509F RID: 20639 RVA: 0x001EC978 File Offset: 0x001EAB78
	public SelectionCategory GetCategory(string _name)
	{
		SelectionCategory selectionCategory;
		this.categories.TryGetValue(_name, out selectionCategory);
		return selectionCategory;
	}

	// Token: 0x060050A0 RID: 20640 RVA: 0x001EC995 File Offset: 0x001EAB95
	public bool TryGetSelectionBox(string _category, string _name, out SelectionBox _selectionBox)
	{
		SelectionCategory category = this.GetCategory(_category);
		_selectionBox = ((category != null) ? category.GetBox(_name) : null);
		return _selectionBox != null;
	}

	// Token: 0x060050A1 RID: 20641 RVA: 0x001EC9B8 File Offset: 0x001EABB8
	public SelectionCategory CreateCategory(string _name, Color _colSelected, Color _colUnselected, Color _colFaceSelected, bool _bCollider, string _tag, int _layer = 0)
	{
		Transform transform = new GameObject(_name).transform;
		transform.parent = base.transform;
		SelectionCategory selectionCategory = new SelectionCategory(_name, transform, _colSelected, _colUnselected, _colFaceSelected, _bCollider, _tag, null, _layer);
		selectionCategory.SetVisible(false);
		this.categories[_name] = selectionCategory;
		return selectionCategory;
	}

	// Token: 0x060050A2 RID: 20642 RVA: 0x001ECA05 File Offset: 0x001EAC05
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

	// Token: 0x060050A3 RID: 20643 RVA: 0x001ECA38 File Offset: 0x001EAC38
	public void Deactivate()
	{
		this.activate(null);
	}

	// Token: 0x060050A4 RID: 20644 RVA: 0x001ECA42 File Offset: 0x001EAC42
	public bool TryGetSelected(out SelectionBox _selectedBox)
	{
		_selectedBox = this.Selection;
		return _selectedBox != null;
	}

	// Token: 0x060050A5 RID: 20645 RVA: 0x001ECA54 File Offset: 0x001EAC54
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

	// Token: 0x060050A6 RID: 20646 RVA: 0x001ECB48 File Offset: 0x001EAD48
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

	// Token: 0x060050A7 RID: 20647 RVA: 0x001ECC14 File Offset: 0x001EAE14
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

	// Token: 0x060050A8 RID: 20648 RVA: 0x001ECCE8 File Offset: 0x001EAEE8
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

	// Token: 0x060050A9 RID: 20649 RVA: 0x001ECD48 File Offset: 0x001EAF48
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

	// Token: 0x060050AA RID: 20650 RVA: 0x001ECD98 File Offset: 0x001EAF98
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

	// Token: 0x060050AB RID: 20651 RVA: 0x001ECDF8 File Offset: 0x001EAFF8
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

	// Token: 0x060050AC RID: 20652 RVA: 0x001ECE38 File Offset: 0x001EB038
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

	// Token: 0x060050AD RID: 20653 RVA: 0x001ECE9C File Offset: 0x001EB09C
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

	// Token: 0x060050AE RID: 20654 RVA: 0x001ECEF4 File Offset: 0x001EB0F4
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

	// Token: 0x060050AF RID: 20655 RVA: 0x001ECF64 File Offset: 0x001EB164
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

	// Token: 0x060050B0 RID: 20656 RVA: 0x001ECF9C File Offset: 0x001EB19C
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

	// Token: 0x060050B1 RID: 20657 RVA: 0x001ED09C File Offset: 0x001EB29C
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

	// Token: 0x060050B2 RID: 20658 RVA: 0x001ED600 File Offset: 0x001EB800
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

	// Token: 0x060050B3 RID: 20659 RVA: 0x001EDA00 File Offset: 0x001EBC00
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

	// Token: 0x04003E04 RID: 15876
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, SelectionCategory> categories = new Dictionary<string, SelectionCategory>();

	// Token: 0x04003E06 RID: 15878
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float alphaMultiplier = 1f;

	// Token: 0x04003E07 RID: 15879
	public static SelectionBoxManager Instance;

	// Token: 0x04003E08 RID: 15880
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bMousedPressed;

	// Token: 0x04003E09 RID: 15881
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 highlightedAxisScreenDir;

	// Token: 0x04003E0A RID: 15882
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i highlightedAxis = Vector3i.zero;

	// Token: 0x04003E0B RID: 15883
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 mouseMoveDir = Vector3.zero;

	// Token: 0x04003E0C RID: 15884
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int lastSelOpMode;

	// Token: 0x04003E0D RID: 15885
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bWaitForRelease;
}
