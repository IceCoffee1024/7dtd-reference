using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A6A RID: 2666
public class SelectionCategory
{
	// Token: 0x060050C4 RID: 20676 RVA: 0x001EDAAC File Offset: 0x001EBCAC
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

	// Token: 0x060050C5 RID: 20677 RVA: 0x001EDB2C File Offset: 0x001EBD2C
	public void SetCallback(ISelectionBoxCallback _callback)
	{
		this.BoxCallbacks = _callback;
	}

	// Token: 0x060050C6 RID: 20678 RVA: 0x001EDB35 File Offset: 0x001EBD35
	public bool IsVisible()
	{
		return this.transform.gameObject.activeSelf;
	}

	// Token: 0x060050C7 RID: 20679 RVA: 0x001EDB48 File Offset: 0x001EBD48
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

	// Token: 0x060050C8 RID: 20680 RVA: 0x001EDBA0 File Offset: 0x001EBDA0
	public void SetCaptionVisibility(bool _visible)
	{
		foreach (KeyValuePair<string, SelectionBox> keyValuePair in this.boxes)
		{
			keyValuePair.Value.SetCaptionVisibility(_visible);
		}
	}

	// Token: 0x060050C9 RID: 20681 RVA: 0x001EDBFC File Offset: 0x001EBDFC
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

	// Token: 0x060050CA RID: 20682 RVA: 0x001EDC78 File Offset: 0x001EBE78
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

	// Token: 0x060050CB RID: 20683 RVA: 0x001EDD2C File Offset: 0x001EBF2C
	public SelectionBox GetBox(string _name)
	{
		SelectionBox selectionBox;
		this.boxes.TryGetValue(_name, out selectionBox);
		return selectionBox;
	}

	// Token: 0x060050CC RID: 20684 RVA: 0x001EDD49 File Offset: 0x001EBF49
	public bool TryGetBox(string _name, out SelectionBox _box)
	{
		return this.boxes.TryGetValue(_name, out _box);
	}

	// Token: 0x060050CD RID: 20685 RVA: 0x001EDD58 File Offset: 0x001EBF58
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

	// Token: 0x060050CE RID: 20686 RVA: 0x001EDD9C File Offset: 0x001EBF9C
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

	// Token: 0x060050CF RID: 20687 RVA: 0x001EDDF4 File Offset: 0x001EBFF4
	public void RemoveBox(string _name)
	{
		SelectionBox selectionBox;
		if (!this.boxes.TryGetValue(_name, out selectionBox))
		{
			return;
		}
		this.RemoveBox(selectionBox);
	}

	// Token: 0x060050D0 RID: 20688 RVA: 0x001EDE19 File Offset: 0x001EC019
	public override string ToString()
	{
		return "Cat " + this.name;
	}

	// Token: 0x04003E14 RID: 15892
	public readonly string name;

	// Token: 0x04003E15 RID: 15893
	public readonly string LocalizedName;

	// Token: 0x04003E16 RID: 15894
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Transform transform;

	// Token: 0x04003E17 RID: 15895
	public readonly Color colActive;

	// Token: 0x04003E18 RID: 15896
	public readonly Color colInactive;

	// Token: 0x04003E19 RID: 15897
	public readonly Color colFaceSelected;

	// Token: 0x04003E1A RID: 15898
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool bCollider;

	// Token: 0x04003E1B RID: 15899
	public readonly string tag;

	// Token: 0x04003E1C RID: 15900
	public ISelectionBoxCallback BoxCallbacks;

	// Token: 0x04003E1D RID: 15901
	public ISelectionCategoryCallback CategoryCallbacks;

	// Token: 0x04003E1E RID: 15902
	public Action CheckKeysCallback;

	// Token: 0x04003E1F RID: 15903
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int layer;

	// Token: 0x04003E20 RID: 15904
	public readonly Dictionary<string, SelectionBox> boxes = new Dictionary<string, SelectionBox>();
}
