using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000A79 RID: 2681
public class SelectionBoxManager : MonoBehaviour
{
	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x0600507C RID: 20604 RVA: 0x001EEB63 File Offset: 0x001ECD63
	// (set) Token: 0x0600507D RID: 20605 RVA: 0x001EEB6B File Offset: 0x001ECD6B
	public SelectionCategory CategoryDynamicPrefab
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x0600507E RID: 20606 RVA: 0x001EEB74 File Offset: 0x001ECD74
	// (set) Token: 0x0600507F RID: 20607 RVA: 0x001EEB7C File Offset: 0x001ECD7C
	public SelectionCategory CategoryStartPoint
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x06005080 RID: 20608 RVA: 0x001EEB85 File Offset: 0x001ECD85
	// (set) Token: 0x06005081 RID: 20609 RVA: 0x001EEB8D File Offset: 0x001ECD8D
	public SelectionCategory CategorySelection
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x06005082 RID: 20610 RVA: 0x001EEB96 File Offset: 0x001ECD96
	// (set) Token: 0x06005083 RID: 20611 RVA: 0x001EEB9E File Offset: 0x001ECD9E
	public SelectionCategory CategoryTraderTeleport
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x06005084 RID: 20612 RVA: 0x001EEBA7 File Offset: 0x001ECDA7
	// (set) Token: 0x06005085 RID: 20613 RVA: 0x001EEBAF File Offset: 0x001ECDAF
	public SelectionCategory CategoryInfoVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x06005086 RID: 20614 RVA: 0x001EEBB8 File Offset: 0x001ECDB8
	// (set) Token: 0x06005087 RID: 20615 RVA: 0x001EEBC0 File Offset: 0x001ECDC0
	public SelectionCategory CategoryWallVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D5 RID: 2261
	// (get) Token: 0x06005088 RID: 20616 RVA: 0x001EEBC9 File Offset: 0x001ECDC9
	// (set) Token: 0x06005089 RID: 20617 RVA: 0x001EEBD1 File Offset: 0x001ECDD1
	public SelectionCategory CategoryTriggerVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x0600508A RID: 20618 RVA: 0x001EEBDA File Offset: 0x001ECDDA
	// (set) Token: 0x0600508B RID: 20619 RVA: 0x001EEBE2 File Offset: 0x001ECDE2
	public SelectionCategory CategorySleeperVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x0600508C RID: 20620 RVA: 0x001EEBEB File Offset: 0x001ECDEB
	// (set) Token: 0x0600508D RID: 20621 RVA: 0x001EEBF3 File Offset: 0x001ECDF3
	public SelectionCategory CategoryPOIMarker
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x0600508E RID: 20622 RVA: 0x001EEBFC File Offset: 0x001ECDFC
	// (set) Token: 0x0600508F RID: 20623 RVA: 0x001EEC04 File Offset: 0x001ECE04
	public SelectionCategory CategoryPrefabFacingVolume
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x06005090 RID: 20624 RVA: 0x001EEC0D File Offset: 0x001ECE0D
	// (set) Token: 0x06005091 RID: 20625 RVA: 0x001EEC15 File Offset: 0x001ECE15
	public SelectionBox Selection
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x06005092 RID: 20626 RVA: 0x001EEC1E File Offset: 0x001ECE1E
	// (set) Token: 0x06005093 RID: 20627 RVA: 0x001EEC26 File Offset: 0x001ECE26
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

	// Token: 0x06005094 RID: 20628 RVA: 0x001EEC4A File Offset: 0x001ECE4A
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		SelectionBoxManager.Instance = this;
		Origin.Add(base.transform, 1);
		this.alphaMultiplier = GamePrefs.GetFloat(EnumGamePrefs.OptionsSelectionBoxAlphaMultiplier);
	}

	// Token: 0x06005095 RID: 20629 RVA: 0x001EEC70 File Offset: 0x001ECE70
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

	// Token: 0x06005096 RID: 20630 RVA: 0x001EEF90 File Offset: 0x001ED190
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		Origin.Remove(base.transform);
	}

	// Token: 0x06005097 RID: 20631 RVA: 0x001EEFA0 File Offset: 0x001ED1A0
	public SelectionCategory GetCategory(string _name)
	{
		SelectionCategory selectionCategory;
		this.categories.TryGetValue(_name, out selectionCategory);
		return selectionCategory;
	}

	// Token: 0x06005098 RID: 20632 RVA: 0x001EEFBD File Offset: 0x001ED1BD
	public bool TryGetSelectionBox(string _category, string _name, out SelectionBox _selectionBox)
	{
		SelectionCategory category = this.GetCategory(_category);
		_selectionBox = ((category != null) ? category.GetBox(_name) : null);
		return _selectionBox != null;
	}

	// Token: 0x06005099 RID: 20633 RVA: 0x001EEFE0 File Offset: 0x001ED1E0
	public SelectionCategory CreateCategory(string _name, Color _colSelected, Color _colUnselected, Color _colFaceSelected, bool _bCollider, string _tag, int _layer = 0)
	{
		Transform transform = new GameObject(_name).transform;
		transform.parent = base.transform;
		SelectionCategory selectionCategory = new SelectionCategory(_name, transform, _colSelected, _colUnselected, _colFaceSelected, _bCollider, _tag, null, _layer);
		selectionCategory.SetVisible(false);
		this.categories[_name] = selectionCategory;
		return selectionCategory;
	}

	// Token: 0x0600509A RID: 20634 RVA: 0x001EF02D File Offset: 0x001ED22D
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

	// Token: 0x0600509B RID: 20635 RVA: 0x001EF060 File Offset: 0x001ED260
	public void Deactivate()
	{
		this.activate(null);
	}

	// Token: 0x0600509C RID: 20636 RVA: 0x001EF06A File Offset: 0x001ED26A
	public bool TryGetSelected(out SelectionBox _selectedBox)
	{
		_selectedBox = this.Selection;
		return _selectedBox != null;
	}

	// Token: 0x0600509D RID: 20637 RVA: 0x001EF07C File Offset: 0x001ED27C
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

	// Token: 0x0600509E RID: 20638 RVA: 0x001EF170 File Offset: 0x001ED370
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

	// Token: 0x0600509F RID: 20639 RVA: 0x001EF23C File Offset: 0x001ED43C
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

	// Token: 0x060050A0 RID: 20640 RVA: 0x001EF310 File Offset: 0x001ED510
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

	// Token: 0x060050A1 RID: 20641 RVA: 0x001EF370 File Offset: 0x001ED570
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

	// Token: 0x060050A2 RID: 20642 RVA: 0x001EF3C0 File Offset: 0x001ED5C0
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

	// Token: 0x060050A3 RID: 20643 RVA: 0x001EF420 File Offset: 0x001ED620
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

	// Token: 0x060050A4 RID: 20644 RVA: 0x001EF460 File Offset: 0x001ED660
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

	// Token: 0x060050A5 RID: 20645 RVA: 0x001EF4C4 File Offset: 0x001ED6C4
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

	// Token: 0x060050A6 RID: 20646 RVA: 0x001EF51C File Offset: 0x001ED71C
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

	// Token: 0x060050A7 RID: 20647 RVA: 0x001EF58C File Offset: 0x001ED78C
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

	// Token: 0x060050A8 RID: 20648 RVA: 0x001EF5C4 File Offset: 0x001ED7C4
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

	// Token: 0x060050A9 RID: 20649 RVA: 0x001EF6C4 File Offset: 0x001ED8C4
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

	// Token: 0x060050AA RID: 20650 RVA: 0x001EFC28 File Offset: 0x001EDE28
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

	// Token: 0x060050AB RID: 20651 RVA: 0x001F0028 File Offset: 0x001EE228
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

	// Token: 0x04003E33 RID: 15923
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly Dictionary<string, SelectionCategory> categories = new Dictionary<string, SelectionCategory>();

	// Token: 0x04003E35 RID: 15925
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float alphaMultiplier = 1f;

	// Token: 0x04003E36 RID: 15926
	public static SelectionBoxManager Instance;

	// Token: 0x04003E37 RID: 15927
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bMousedPressed;

	// Token: 0x04003E38 RID: 15928
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 highlightedAxisScreenDir;

	// Token: 0x04003E39 RID: 15929
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i highlightedAxis = Vector3i.zero;

	// Token: 0x04003E3A RID: 15930
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 mouseMoveDir = Vector3.zero;

	// Token: 0x04003E3B RID: 15931
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int lastSelOpMode;

	// Token: 0x04003E3C RID: 15932
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bWaitForRelease;
}
