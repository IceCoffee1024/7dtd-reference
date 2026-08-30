using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A59 RID: 2649
public class SelectionCategory
{
	// Token: 0x0600503E RID: 20542 RVA: 0x001EB014 File Offset: 0x001E9214
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

	// Token: 0x0600503F RID: 20543 RVA: 0x001EB094 File Offset: 0x001E9294
	public void SetCallback(ISelectionBoxCallback _callback)
	{
		this.BoxCallbacks = _callback;
	}

	// Token: 0x06005040 RID: 20544 RVA: 0x001EB09D File Offset: 0x001E929D
	public bool IsVisible()
	{
		return this.transform.gameObject.activeSelf;
	}

	// Token: 0x06005041 RID: 20545 RVA: 0x001EB0B0 File Offset: 0x001E92B0
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

	// Token: 0x06005042 RID: 20546 RVA: 0x001EB108 File Offset: 0x001E9308
	public void SetCaptionVisibility(bool _visible)
	{
		foreach (KeyValuePair<string, SelectionBox> keyValuePair in this.boxes)
		{
			keyValuePair.Value.SetCaptionVisibility(_visible);
		}
	}

	// Token: 0x06005043 RID: 20547 RVA: 0x001EB164 File Offset: 0x001E9364
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

	// Token: 0x06005044 RID: 20548 RVA: 0x001EB1E0 File Offset: 0x001E93E0
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

	// Token: 0x06005045 RID: 20549 RVA: 0x001EB294 File Offset: 0x001E9494
	public SelectionBox GetBox(string _name)
	{
		SelectionBox selectionBox;
		this.boxes.TryGetValue(_name, out selectionBox);
		return selectionBox;
	}

	// Token: 0x06005046 RID: 20550 RVA: 0x001EB2B1 File Offset: 0x001E94B1
	public bool TryGetBox(string _name, out SelectionBox _box)
	{
		return this.boxes.TryGetValue(_name, out _box);
	}

	// Token: 0x06005047 RID: 20551 RVA: 0x001EB2C0 File Offset: 0x001E94C0
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

	// Token: 0x06005048 RID: 20552 RVA: 0x001EB304 File Offset: 0x001E9504
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

	// Token: 0x06005049 RID: 20553 RVA: 0x001EB35C File Offset: 0x001E955C
	public void RemoveBox(string _name)
	{
		SelectionBox selectionBox;
		if (!this.boxes.TryGetValue(_name, out selectionBox))
		{
			return;
		}
		this.RemoveBox(selectionBox);
	}

	// Token: 0x0600504A RID: 20554 RVA: 0x001EB381 File Offset: 0x001E9581
	public override string ToString()
	{
		return "Cat " + this.name;
	}

	// Token: 0x04003D83 RID: 15747
	public readonly string name;

	// Token: 0x04003D84 RID: 15748
	public readonly string LocalizedName;

	// Token: 0x04003D85 RID: 15749
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Transform transform;

	// Token: 0x04003D86 RID: 15750
	public readonly Color colActive;

	// Token: 0x04003D87 RID: 15751
	public readonly Color colInactive;

	// Token: 0x04003D88 RID: 15752
	public readonly Color colFaceSelected;

	// Token: 0x04003D89 RID: 15753
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool bCollider;

	// Token: 0x04003D8A RID: 15754
	public readonly string tag;

	// Token: 0x04003D8B RID: 15755
	public ISelectionBoxCallback BoxCallbacks;

	// Token: 0x04003D8C RID: 15756
	public ISelectionCategoryCallback CategoryCallbacks;

	// Token: 0x04003D8D RID: 15757
	public Action CheckKeysCallback;

	// Token: 0x04003D8E RID: 15758
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int layer;

	// Token: 0x04003D8F RID: 15759
	public readonly Dictionary<string, SelectionBox> boxes = new Dictionary<string, SelectionBox>();
}
