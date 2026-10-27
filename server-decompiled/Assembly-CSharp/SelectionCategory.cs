using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A7E RID: 2686
public class SelectionCategory
{
	// Token: 0x060050BC RID: 20668 RVA: 0x001F00D4 File Offset: 0x001EE2D4
	public SelectionCategory(string _name, Transform _transform, Color _colActive, Color _colInactive, Color _colFaceSelected, bool _bCollider, string _tag, ISelectionBoxCallback _boxCallbacks, int _layer = 0)
	{
		this.name = _name;
		this.LocalizedName = Localization.Get("selectionCategory" + this.name, false, null);
		this.transform = _transform;
		this.colActive = _colActive;
		this.colInactive = _colInactive;
		this.colFaceSelected = _colFaceSelected;
		this.bCollider = _bCollider;
		this.tag = _tag;
		this.BoxCallbacks = _boxCallbacks;
		this.layer = _layer;
	}

	// Token: 0x060050BD RID: 20669 RVA: 0x001F0154 File Offset: 0x001EE354
	public void SetCallback(ISelectionBoxCallback _callback)
	{
		this.BoxCallbacks = _callback;
	}

	// Token: 0x060050BE RID: 20670 RVA: 0x001F015D File Offset: 0x001EE35D
	public bool IsVisible()
	{
		return this.transform.gameObject.activeSelf;
	}

	// Token: 0x060050BF RID: 20671 RVA: 0x001F0170 File Offset: 0x001EE370
	public void SetVisible(bool _bVisible)
	{
		this.transform.gameObject.SetActive(_bVisible);
		ISelectionCategoryCallback categoryCallbacks = this.CategoryCallbacks;
		if (categoryCallbacks != null)
		{
			categoryCallbacks.OnSelectionCategoryVisibilityChanged(this, _bVisible);
		}
		if (!_bVisible)
		{
			SelectionBox selection = SelectionBoxManager.Instance.Selection;
			if (((selection != null) ? selection.Category : null) == this)
			{
				SelectionBoxManager.Instance.Deactivate();
			}
		}
	}

	// Token: 0x060050C0 RID: 20672 RVA: 0x001F01C8 File Offset: 0x001EE3C8
	public void SetCaptionVisibility(bool _visible)
	{
		foreach (KeyValuePair<string, SelectionBox> keyValuePair in this.boxes)
		{
			keyValuePair.Value.SetCaptionVisibility(_visible);
		}
	}

	// Token: 0x060050C1 RID: 20673 RVA: 0x001F0224 File Offset: 0x001EE424
	public void Clear()
	{
		foreach (KeyValuePair<string, SelectionBox> keyValuePair in this.boxes)
		{
			global::UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
		}
		this.boxes.Clear();
		ISelectionCategoryCallback categoryCallbacks = this.CategoryCallbacks;
		if (categoryCallbacks == null)
		{
			return;
		}
		categoryCallbacks.OnSelectionCategoryCleared(this);
	}

	// Token: 0x060050C2 RID: 20674 RVA: 0x001F02A0 File Offset: 0x001EE4A0
	public SelectionBox AddBox(string _name, Vector3i _pos, Vector3i _size, bool _bDrawDirection = false, bool _bAlwaysDrawDirection = false)
	{
		SelectionBox selectionBox;
		if (this.boxes.TryGetValue(_name, out selectionBox))
		{
			this.RemoveBox(_name);
		}
		Transform transform = new GameObject().transform;
		transform.parent = this.transform;
		SelectionBox selectionBox2 = transform.gameObject.AddComponent<SelectionBox>();
		selectionBox2.name = _name;
		selectionBox2.SetOwner(this);
		selectionBox2.SetAllFacesColor(this.colInactive, true);
		selectionBox2.bDrawDirection = _bDrawDirection;
		selectionBox2.bAlwaysDrawDirection = _bAlwaysDrawDirection;
		selectionBox2.SetPositionAndSize(_pos, _size);
		if (this.bCollider)
		{
			selectionBox2.EnableCollider(this.tag, this.layer);
		}
		this.boxes[_name] = selectionBox2;
		ISelectionCategoryCallback categoryCallbacks = this.CategoryCallbacks;
		if (categoryCallbacks != null)
		{
			categoryCallbacks.OnSelectionCategoryBoxAdded(selectionBox2);
		}
		return selectionBox2;
	}

	// Token: 0x060050C3 RID: 20675 RVA: 0x001F0354 File Offset: 0x001EE554
	public SelectionBox GetBox(string _name)
	{
		SelectionBox selectionBox;
		this.boxes.TryGetValue(_name, out selectionBox);
		return selectionBox;
	}

	// Token: 0x060050C4 RID: 20676 RVA: 0x001F0371 File Offset: 0x001EE571
	public bool TryGetBox(string _name, out SelectionBox _box)
	{
		return this.boxes.TryGetValue(_name, out _box);
	}

	// Token: 0x060050C5 RID: 20677 RVA: 0x001F0380 File Offset: 0x001EE580
	public void RenameBox(SelectionBox _box, string _newName)
	{
		if (_box.name.Equals(_newName))
		{
			return;
		}
		string text = _box.name;
		_box.name = _newName;
		this.boxes[_newName] = _box;
		this.boxes.Remove(text);
	}

	// Token: 0x060050C6 RID: 20678 RVA: 0x001F03C4 File Offset: 0x001EE5C4
	public void RemoveBox(SelectionBox _box)
	{
		if (SelectionBoxManager.Instance.Selection == _box)
		{
			SelectionBoxManager.Instance.Deactivate();
		}
		ISelectionCategoryCallback categoryCallbacks = this.CategoryCallbacks;
		if (categoryCallbacks != null)
		{
			categoryCallbacks.OnSelectionCategoryBoxRemoved(_box);
		}
		global::UnityEngine.Object.Destroy(_box.gameObject);
		this.boxes.Remove(_box.name);
	}

	// Token: 0x060050C7 RID: 20679 RVA: 0x001F041C File Offset: 0x001EE61C
	public void RemoveBox(string _name)
	{
		SelectionBox selectionBox;
		if (!this.boxes.TryGetValue(_name, out selectionBox))
		{
			return;
		}
		this.RemoveBox(selectionBox);
	}

	// Token: 0x060050C8 RID: 20680 RVA: 0x001F0441 File Offset: 0x001EE641
	public override string ToString()
	{
		return "Cat " + this.name;
	}

	// Token: 0x04003E43 RID: 15939
	public readonly string name;

	// Token: 0x04003E44 RID: 15940
	public readonly string LocalizedName;

	// Token: 0x04003E45 RID: 15941
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Transform transform;

	// Token: 0x04003E46 RID: 15942
	public readonly Color colActive;

	// Token: 0x04003E47 RID: 15943
	public readonly Color colInactive;

	// Token: 0x04003E48 RID: 15944
	public readonly Color colFaceSelected;

	// Token: 0x04003E49 RID: 15945
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool bCollider;

	// Token: 0x04003E4A RID: 15946
	public readonly string tag;

	// Token: 0x04003E4B RID: 15947
	public ISelectionBoxCallback BoxCallbacks;

	// Token: 0x04003E4C RID: 15948
	public ISelectionCategoryCallback CategoryCallbacks;

	// Token: 0x04003E4D RID: 15949
	public Action CheckKeysCallback;

	// Token: 0x04003E4E RID: 15950
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int layer;

	// Token: 0x04003E4F RID: 15951
	public readonly Dictionary<string, SelectionBox> boxes = new Dictionary<string, SelectionBox>();
}
