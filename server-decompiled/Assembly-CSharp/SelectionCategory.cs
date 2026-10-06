using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A74 RID: 2676
public class SelectionCategory
{
	// Token: 0x06005098 RID: 20632 RVA: 0x001EEFF8 File Offset: 0x001ED1F8
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

	// Token: 0x06005099 RID: 20633 RVA: 0x001EF078 File Offset: 0x001ED278
	public void SetCallback(ISelectionBoxCallback _callback)
	{
		this.BoxCallbacks = _callback;
	}

	// Token: 0x0600509A RID: 20634 RVA: 0x001EF081 File Offset: 0x001ED281
	public bool IsVisible()
	{
		return this.transform.gameObject.activeSelf;
	}

	// Token: 0x0600509B RID: 20635 RVA: 0x001EF094 File Offset: 0x001ED294
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

	// Token: 0x0600509C RID: 20636 RVA: 0x001EF0EC File Offset: 0x001ED2EC
	public void SetCaptionVisibility(bool _visible)
	{
		foreach (KeyValuePair<string, SelectionBox> keyValuePair in this.boxes)
		{
			keyValuePair.Value.SetCaptionVisibility(_visible);
		}
	}

	// Token: 0x0600509D RID: 20637 RVA: 0x001EF148 File Offset: 0x001ED348
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

	// Token: 0x0600509E RID: 20638 RVA: 0x001EF1C4 File Offset: 0x001ED3C4
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

	// Token: 0x0600509F RID: 20639 RVA: 0x001EF278 File Offset: 0x001ED478
	public SelectionBox GetBox(string _name)
	{
		SelectionBox selectionBox;
		this.boxes.TryGetValue(_name, out selectionBox);
		return selectionBox;
	}

	// Token: 0x060050A0 RID: 20640 RVA: 0x001EF295 File Offset: 0x001ED495
	public bool TryGetBox(string _name, out SelectionBox _box)
	{
		return this.boxes.TryGetValue(_name, out _box);
	}

	// Token: 0x060050A1 RID: 20641 RVA: 0x001EF2A4 File Offset: 0x001ED4A4
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

	// Token: 0x060050A2 RID: 20642 RVA: 0x001EF2E8 File Offset: 0x001ED4E8
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

	// Token: 0x060050A3 RID: 20643 RVA: 0x001EF340 File Offset: 0x001ED540
	public void RemoveBox(string _name)
	{
		SelectionBox selectionBox;
		if (!this.boxes.TryGetValue(_name, out selectionBox))
		{
			return;
		}
		this.RemoveBox(selectionBox);
	}

	// Token: 0x060050A4 RID: 20644 RVA: 0x001EF365 File Offset: 0x001ED565
	public override string ToString()
	{
		return "Cat " + this.name;
	}

	// Token: 0x04003E22 RID: 15906
	public readonly string name;

	// Token: 0x04003E23 RID: 15907
	public readonly string LocalizedName;

	// Token: 0x04003E24 RID: 15908
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Transform transform;

	// Token: 0x04003E25 RID: 15909
	public readonly Color colActive;

	// Token: 0x04003E26 RID: 15910
	public readonly Color colInactive;

	// Token: 0x04003E27 RID: 15911
	public readonly Color colFaceSelected;

	// Token: 0x04003E28 RID: 15912
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool bCollider;

	// Token: 0x04003E29 RID: 15913
	public readonly string tag;

	// Token: 0x04003E2A RID: 15914
	public ISelectionBoxCallback BoxCallbacks;

	// Token: 0x04003E2B RID: 15915
	public ISelectionCategoryCallback CategoryCallbacks;

	// Token: 0x04003E2C RID: 15916
	public Action CheckKeysCallback;

	// Token: 0x04003E2D RID: 15917
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int layer;

	// Token: 0x04003E2E RID: 15918
	public readonly Dictionary<string, SelectionBox> boxes = new Dictionary<string, SelectionBox>();
}
