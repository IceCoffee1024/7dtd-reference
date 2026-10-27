using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001055 RID: 4181
[Preserve]
public class XUiC_SignLayerGrid : XUiController
{
	// Token: 0x17000F86 RID: 3974
	// (get) Token: 0x0600840D RID: 33805 RVA: 0x00326CC4 File Offset: 0x00324EC4
	// (set) Token: 0x0600840E RID: 33806 RVA: 0x00326CCC File Offset: 0x00324ECC
	public int SelectedLayerIndex
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x17000F87 RID: 3975
	// (get) Token: 0x0600840F RID: 33807 RVA: 0x00326CD5 File Offset: 0x00324ED5
	// (set) Token: 0x06008410 RID: 33808 RVA: 0x00326CDD File Offset: 0x00324EDD
	public int SlotCount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F88 RID: 3976
	// (get) Token: 0x06008411 RID: 33809 RVA: 0x00326CE6 File Offset: 0x00324EE6
	// (set) Token: 0x06008412 RID: 33810 RVA: 0x00326CEE File Offset: 0x00324EEE
	public int LayerCount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F89 RID: 3977
	// (get) Token: 0x06008413 RID: 33811 RVA: 0x00326CF7 File Offset: 0x00324EF7
	public int ItemCount
	{
		get
		{
			return this.LayerCount + (this.HasPlaceholder ? 1 : 0);
		}
	}

	// Token: 0x17000F8A RID: 3978
	// (get) Token: 0x06008414 RID: 33812 RVA: 0x00326D0C File Offset: 0x00324F0C
	// (set) Token: 0x06008415 RID: 33813 RVA: 0x00326D14 File Offset: 0x00324F14
	public Action OnAddLayerSelected
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x17000F8B RID: 3979
	// (get) Token: 0x06008416 RID: 33814 RVA: 0x00326D1D File Offset: 0x00324F1D
	public bool HasPlaceholder
	{
		get
		{
			return this.placeholderIndex >= 0;
		}
	}

	// Token: 0x06008417 RID: 33815 RVA: 0x00326D2C File Offset: 0x00324F2C
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("grid");
		this.signLayerControllers = childById.GetChildrenByType<XUiC_SignLayer>(null);
		this.SlotCount = this.signLayerControllers.Length;
		this.IsDirty = false;
		for (int i = 0; i < childById.Children.Count; i++)
		{
			childById.Children[i].OnScroll += this.HandleOnScroll;
		}
		for (int j = 0; j < this.signLayerControllers.Length; j++)
		{
			XUiC_SignLayer xuiC_SignLayer = this.signLayerControllers[j];
			xuiC_SignLayer.OnClicked = (Action<XUiC_SignLayer>)Delegate.Combine(xuiC_SignLayer.OnClicked, new Action<XUiC_SignLayer>(this.HandleOnClick));
			this.signLayerControllers[j].OnHover += this.HandleOnHover;
			XUiC_SignLayer xuiC_SignLayer2 = this.signLayerControllers[j];
			xuiC_SignLayer2.OnBecameSelected = (Action<XUiC_SignGridEntry>)Delegate.Combine(xuiC_SignLayer2.OnBecameSelected, new Action<XUiC_SignGridEntry>(this.HandleOnSelect));
			XUiC_SignLayer xuiC_SignLayer3 = this.signLayerControllers[j];
			xuiC_SignLayer3.OnBecameCursorSelected = (Action<XUiC_SignLayer>)Delegate.Combine(xuiC_SignLayer3.OnBecameCursorSelected, new Action<XUiC_SignLayer>(this.HandleOnCursorSelect));
			this.signLayerControllers[j].OnDrag += this.HandleOnDrag;
		}
		childById.OnScroll += this.HandleOnScroll;
		XUiController childById2 = base.GetChildById("btnLayerPrevious");
		XUiController childById3 = base.GetChildById("btnLayerNext");
		childById2.OnPress += this.BtnPreviousLayerOnPress;
		childById3.OnPress += this.BtnNextLayerOnPress;
		this.activeArea = base.GetChildById("activeArea");
		this.btnAddLayer = base.GetChildById("btnAddLayer") as XUiC_SimpleButton;
		this.btnAddLayer.OnPressed += this.HandleAddLayer;
		this.btnAddLayer.OnHovered += delegate(XUiController sender, bool isOver)
		{
			this.isBtnAddLayerHovered = isOver;
			base.RefreshBindings();
		};
		this.dragAndDropIndicator = base.GetChildById("dragAndDropIndicator");
		this.insertIndicator = base.GetChildById("insertIndicator");
	}

	// Token: 0x06008418 RID: 33816 RVA: 0x00326F2C File Offset: 0x0032512C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnDrag(XUiController sender, EDragType dragType, Vector2 mousePositionDelta)
	{
		if (!this.isDraggingLayers && this.hoveredLayerIndex > -1 && this.signLayerControllers[this.hoveredLayerIndex - this.baseLayerIndex].layer != null)
		{
			this.dragStartIdx = this.hoveredLayerIndex;
			this.isDraggingLayers = true;
			this.draggedLayerIndices.Clear();
			this.draggedLayerIndices.AddRange(this.MultiSelectedLayerIndices);
			if (!this.draggedLayerIndices.Contains(this.hoveredLayerIndex))
			{
				if (!InputUtils.ControlKeyPressed)
				{
					this.draggedLayerIndices.Clear();
				}
				this.draggedLayerIndices.Add(this.hoveredLayerIndex);
			}
			this.draggedLayerIndices.Sort();
			Action<int> onDragAndDropStarted = this.OnDragAndDropStarted;
			if (onDragAndDropStarted == null)
			{
				return;
			}
			onDragAndDropStarted(this.dragStartIdx);
		}
	}

	// Token: 0x06008419 RID: 33817 RVA: 0x00326FF4 File Offset: 0x003251F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnNextLayerOnPress(XUiController sender, int mouseButton)
	{
		this.TryScroll(1);
	}

	// Token: 0x0600841A RID: 33818 RVA: 0x00326FFE File Offset: 0x003251FE
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPreviousLayerOnPress(XUiController sender, int mouseButton)
	{
		this.TryScroll(-1);
	}

	// Token: 0x0600841B RID: 33819 RVA: 0x00327008 File Offset: 0x00325208
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCursorSelect(XUiC_SignLayer layer)
	{
		XUiView navigationTarget = this.xui.playerUI.CursorController.navigationTarget;
		XUiC_SignLayer xuiC_SignLayer = ((navigationTarget != null) ? navigationTarget.Controller : null) as XUiC_SignLayer;
		if (xuiC_SignLayer != null)
		{
			if (layer == this.signLayerControllers[0] && xuiC_SignLayer == this.signLayerControllers[this.SlotCount - 1])
			{
				this.TryScroll(-this.ItemCount);
				return;
			}
			if (xuiC_SignLayer == this.signLayerControllers[0] && layer == this.signLayerControllers[this.SlotCount - 1])
			{
				this.TryScroll(this.ItemCount);
				return;
			}
		}
		if (layer.index < this.baseLayerIndex + 1)
		{
			if (this.TryScroll(-1))
			{
				this.selectionOverrideLayer = this.signLayerControllers[1];
				return;
			}
		}
		else if (layer.index > this.baseLayerIndex + this.SlotCount - 2 && this.TryScroll(1))
		{
			this.selectionOverrideLayer = this.signLayerControllers[this.SlotCount - 2];
		}
	}

	// Token: 0x0600841C RID: 33820 RVA: 0x003270F4 File Offset: 0x003252F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnHover(XUiController _sender, bool _isOver)
	{
		XUiC_SignLayer xuiC_SignLayer = _sender as XUiC_SignLayer;
		if (xuiC_SignLayer != null)
		{
			this.hoveredLayerIndex = (_isOver ? xuiC_SignLayer.index : (-1));
			Action<int> onLayerHovered = this.OnLayerHovered;
			if (onLayerHovered == null)
			{
				return;
			}
			onLayerHovered(this.hoveredLayerIndex);
		}
	}

	// Token: 0x0600841D RID: 33821 RVA: 0x00327134 File Offset: 0x00325334
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnClick(XUiC_SignLayer layerthumb)
	{
		if (!InputUtils.ControlKeyPressed)
		{
			this.MultiSelectedLayerIndices.Clear();
			if (InputUtils.ShiftKeyPressed)
			{
				int num = math.min(this.SelectedLayerIndex, layerthumb.index);
				int num2 = math.max(this.SelectedLayerIndex, layerthumb.index);
				for (int i = num; i <= num2; i++)
				{
					this.MultiSelectedLayerIndices.Add(i);
				}
			}
			else
			{
				this.MultiSelectedLayerIndices.Add(layerthumb.index);
			}
			layerthumb.IsSelected = true;
			return;
		}
		if (InputUtils.ShiftKeyPressed)
		{
			int num3 = math.min(this.SelectedLayerIndex, layerthumb.index);
			int num4 = math.min(math.max(this.SelectedLayerIndex, layerthumb.index), this.LayerCount - 1);
			for (int j = num3; j <= num4; j++)
			{
				if (!this.MultiSelectedLayerIndices.Contains(layerthumb.index))
				{
					this.MultiSelectedLayerIndices.Add(j);
				}
			}
			layerthumb.IsSelected = true;
			return;
		}
		if (!this.MultiSelectedLayerIndices.Contains(layerthumb.index))
		{
			this.MultiSelectedLayerIndices.Add(layerthumb.index);
			layerthumb.IsSelected = true;
			return;
		}
		if (this.MultiSelectedLayerIndices.Count > 1)
		{
			this.MultiSelectedLayerIndices.Remove(layerthumb.index);
			if (this.SelectedLayerIndex == layerthumb.index)
			{
				this.SelectedLayerIndex = this.MultiSelectedLayerIndices[this.MultiSelectedLayerIndices.Count - 1];
				if (this.SelectedLayerIndex > this.baseLayerIndex && this.SelectedLayerIndex < this.baseLayerIndex + this.ItemCount)
				{
					this.signLayerControllers[this.SelectedLayerIndex - this.baseLayerIndex].IsSelected = true;
				}
				else
				{
					layerthumb.IsSelected = false;
					Action<int> onLayerSelected = this.OnLayerSelected;
					if (onLayerSelected != null)
					{
						onLayerSelected(this.SelectedLayerIndex);
					}
				}
				this.OnMultiSelectionDeselect(true);
				return;
			}
			this.OnMultiSelectionDeselect(false);
		}
	}

	// Token: 0x0600841E RID: 33822 RVA: 0x0032730C File Offset: 0x0032550C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnSelect(XUiC_SignGridEntry gridEntry)
	{
		XUiC_SignLayer xuiC_SignLayer = gridEntry as XUiC_SignLayer;
		if (xuiC_SignLayer == null)
		{
			Log.Error("Failed to cast selected element as XUiC_SignLayer.");
			return;
		}
		this.SelectedLayerIndex = xuiC_SignLayer.index;
		Action<int> onLayerSelected = this.OnLayerSelected;
		if (onLayerSelected == null)
		{
			return;
		}
		onLayerSelected(this.SelectedLayerIndex);
	}

	// Token: 0x0600841F RID: 33823 RVA: 0x00327350 File Offset: 0x00325550
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		this.TryScroll((_delta > 0f) ? (-1) : 1);
	}

	// Token: 0x06008420 RID: 33824 RVA: 0x00327368 File Offset: 0x00325568
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryScroll(int _delta)
	{
		int num = Mathf.Clamp(this.baseLayerIndex + _delta, 0, Mathf.Max(this.ItemCount - this.SlotCount, 0));
		if (num == this.baseLayerIndex)
		{
			return false;
		}
		if (this.hoveredLayerIndex > -1)
		{
			this.hoveredLayerIndex += num - this.baseLayerIndex;
			Action<int> onLayerHovered = this.OnLayerHovered;
			if (onLayerHovered != null)
			{
				onLayerHovered(this.hoveredLayerIndex);
			}
		}
		this.baseLayerIndex = num;
		this.IsDirty = true;
		return true;
	}

	// Token: 0x06008421 RID: 33825 RVA: 0x003273E6 File Offset: 0x003255E6
	public void ReleaseDragAndDrop()
	{
		if (this.isDraggingLayers)
		{
			this.isDraggingLayers = false;
			Action<int> onDragAndDropReleased = this.OnDragAndDropReleased;
			if (onDragAndDropReleased != null)
			{
				onDragAndDropReleased(this.isInsideActiveArea ? this.nearestInsertIndex : (-1));
			}
			base.RefreshBindings();
		}
	}

	// Token: 0x06008422 RID: 33826 RVA: 0x0032741F File Offset: 0x0032561F
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleAddLayer(XUiController sender, int mouseButton)
	{
		Action<int> onAddLayerPressed = this.OnAddLayerPressed;
		if (onAddLayerPressed == null)
		{
			return;
		}
		onAddLayerPressed(this.nearestInsertIndex);
	}

	// Token: 0x06008423 RID: 33827 RVA: 0x00327437 File Offset: 0x00325637
	public int GridIndexToLayerIndex(int gridIndex)
	{
		if (!this.HasPlaceholder)
		{
			return gridIndex;
		}
		if (gridIndex < this.placeholderIndex)
		{
			return gridIndex;
		}
		if (gridIndex == this.placeholderIndex)
		{
			return -1;
		}
		return gridIndex - 1;
	}

	// Token: 0x06008424 RID: 33828 RVA: 0x0032745C File Offset: 0x0032565C
	public void RefreshSelectedLayerBindings()
	{
		int num = this.SelectedLayerIndex - this.baseLayerIndex;
		if (num < 0 || num >= this.SlotCount)
		{
			return;
		}
		this.signLayerControllers[num].RefreshBindings();
	}

	// Token: 0x06008425 RID: 33829 RVA: 0x00327494 File Offset: 0x00325694
	public void UpdateLayers(GlobalSignId signId, int selectedIndex, bool placeholderActive, bool autoScroll = true)
	{
		int num = this.placeholderIndex;
		this.placeholderIndex = (placeholderActive ? selectedIndex : (-1));
		bool flag = num != this.placeholderIndex;
		if (placeholderActive)
		{
			this.MultiSelectedLayerIndices.Clear();
		}
		this.xui.GetChildByType<XUiC_SignGalleryWindow>();
		this.currentId = signId;
		SignData signData;
		SignDataManager.Instance.TryGetSignData(signId, out signData);
		this.LayerCount = ((signData != null) ? signData.layers.Count : 0);
		if (autoScroll)
		{
			int num2 = selectedIndex - this.baseLayerIndex;
			if (num2 < 0)
			{
				this.TryScroll(num2);
			}
			else if (num2 >= this.SlotCount)
			{
				this.TryScroll(num2 - this.SlotCount + 1);
			}
		}
		for (int i = 0; i < this.SlotCount; i++)
		{
			int num3 = this.baseLayerIndex + i;
			XUiC_SignLayer xuiC_SignLayer = this.signLayerControllers[i];
			if (this.HasPlaceholder && num3 == this.placeholderIndex)
			{
				xuiC_SignLayer.SetAsPlaceholder(signId, num3);
			}
			else
			{
				int num4 = this.GridIndexToLayerIndex(num3);
				if (num4 >= 0 && num4 < this.LayerCount)
				{
					xuiC_SignLayer.SetLayer(signId, signData.layers[num4], num3);
				}
				else
				{
					xuiC_SignLayer.SetLayer(GlobalSignId.InvalidId, null, num3);
				}
			}
			if (num3 == selectedIndex)
			{
				xuiC_SignLayer.IsSelected = true;
			}
			else if (xuiC_SignLayer.IsSelected)
			{
				xuiC_SignLayer.IsSelected = false;
			}
		}
		if (this.SelectedLayerIndex != selectedIndex)
		{
			Log.Error(string.Format("Error in sign layer grid: SelectedLayerIndex '{0}' does not match the input index '{1}' after updating all layer controllers.", this.SelectedLayerIndex, selectedIndex));
		}
		if (flag)
		{
			base.RefreshBindings();
		}
		this.IsDirty = false;
	}

	// Token: 0x06008426 RID: 33830 RVA: 0x0032761E File Offset: 0x0032581E
	public void SetLayerName(string name)
	{
		this.signLayerControllers[this.SelectedLayerIndex - this.baseLayerIndex].layer.name = name;
		this.signLayerControllers[this.SelectedLayerIndex - this.baseLayerIndex].RefreshTooltip();
	}

	// Token: 0x06008427 RID: 33831 RVA: 0x00327658 File Offset: 0x00325858
	public bool MousePositionInsideActiveArea()
	{
		Vector3 vector = this.xui.playerUI.camera.ScreenToWorldPoint(Input.mousePosition);
		vector.z = this.activeArea.ViewComponent.ColliderCenter.z;
		return this.activeArea.ViewComponent.ColliderBounds.Contains(vector);
	}

	// Token: 0x06008428 RID: 33832 RVA: 0x003012C2 File Offset: 0x002FF4C2
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x06008429 RID: 33833 RVA: 0x003276B5 File Offset: 0x003258B5
	public override void OnClose()
	{
		this.MultiSelectedLayerIndices.Clear();
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x0600842A RID: 33834 RVA: 0x003276E4 File Offset: 0x003258E4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		bool flag = this.isInsideActiveArea;
		this.isInsideActiveArea = this.MousePositionInsideActiveArea();
		if (this.isInsideActiveArea)
		{
			int nearestVisibleInsertSlot = this.GetNearestVisibleInsertSlot();
			float insertPositionX = this.GetInsertPositionX(nearestVisibleInsertSlot);
			int num = this.baseLayerIndex + nearestVisibleInsertSlot;
			float num2 = 1f - Mathf.Exp(-20f * _dt);
			float num3 = Mathf.Lerp(this.btnAddLayer.ViewComponent.UiTransform.position.x, insertPositionX, num2);
			this.btnAddLayer.ViewComponent.UiTransform.position = new Vector2(num3, this.btnAddLayer.ViewComponent.UiTransform.position.y);
			this.insertIndicator.ViewComponent.UiTransform.position = new Vector2(insertPositionX, this.insertIndicator.ViewComponent.UiTransform.position.y);
			this.nearestInsertIndex = num;
			if (this.isDraggingLayers)
			{
				this.dragAndDropIndicator.ViewComponent.UiTransform.position = new Vector2(insertPositionX, this.dragAndDropIndicator.ViewComponent.UiTransform.position.y);
			}
		}
		if (this.isDraggingLayers || flag != this.isInsideActiveArea)
		{
			base.RefreshBindings();
		}
		if (this.IsDirty)
		{
			XUiC_SignLayer xuiC_SignLayer = this.selectionOverrideLayer;
			if (xuiC_SignLayer != null)
			{
				xuiC_SignLayer.SelectCursorElement(false, true);
			}
			this.selectionOverrideLayer = null;
			this.UpdateLayers(this.currentId, this.SelectedLayerIndex, this.HasPlaceholder, false);
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0600842B RID: 33835 RVA: 0x00327884 File Offset: 0x00325A84
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetNearestVisibleInsertSlot()
	{
		float x = this.xui.playerUI.camera.ScreenToWorldPoint(Input.mousePosition).x;
		float leftInsertX = this.signLayerControllers[0].LeftInsertX;
		float num = ((this.SlotCount > 1) ? ((this.signLayerControllers[this.SlotCount - 1].LeftInsertX - this.signLayerControllers[0].LeftInsertX) / (float)(this.SlotCount - 1)) : (this.signLayerControllers[0].RightInsertX - this.signLayerControllers[0].LeftInsertX));
		int num2 = Mathf.RoundToInt((x - leftInsertX) / num);
		int num3 = 0;
		int num4 = 0;
		while (num4 < this.SlotCount && this.signLayerControllers[num4].LayerValid)
		{
			num3 = num4 + 1;
			num4++;
		}
		return Mathf.Clamp(num2, 0, num3);
	}

	// Token: 0x0600842C RID: 33836 RVA: 0x0032794F File Offset: 0x00325B4F
	[PublicizedFrom(EAccessModifier.Private)]
	public float GetInsertPositionX(int visibleSlot)
	{
		if (visibleSlot != 0)
		{
			return this.signLayerControllers[visibleSlot - 1].RightInsertX;
		}
		return this.signLayerControllers[0].LeftInsertX;
	}

	// Token: 0x0600842D RID: 33837 RVA: 0x00327974 File Offset: 0x00325B74
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (base.GetBindingValueInternal(ref _value, _bindingName))
		{
			return true;
		}
		if (_bindingName == "showlayernext")
		{
			_value = (this.ItemCount > this.baseLayerIndex + this.SlotCount).ToString();
			return true;
		}
		if (_bindingName == "showlayerprev")
		{
			_value = (this.baseLayerIndex > 0).ToString();
			return true;
		}
		if (_bindingName == "showaddlayer")
		{
			_value = (!this.isDraggingLayers && this.isInsideActiveArea && !this.HasPlaceholder).ToString();
			return true;
		}
		if (_bindingName == "showinsertindicator")
		{
			_value = (!this.isDraggingLayers && this.isInsideActiveArea && this.isBtnAddLayerHovered).ToString();
			return true;
		}
		if (!(_bindingName == "isdraganddropvalid"))
		{
			return false;
		}
		_value = (this.isDraggingLayers && this.isInsideActiveArea).ToString();
		return true;
	}

	// Token: 0x0600842E RID: 33838 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_LOG_SIGN_LAYER_GRID")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugLog(string message)
	{
	}

	// Token: 0x04006212 RID: 25106
	[PublicizedFrom(EAccessModifier.Private)]
	public int baseLayerIndex;

	// Token: 0x04006213 RID: 25107
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignLayer[] signLayerControllers;

	// Token: 0x04006214 RID: 25108
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId currentId;

	// Token: 0x04006215 RID: 25109
	[PublicizedFrom(EAccessModifier.Private)]
	public int hoveredLayerIndex = -1;

	// Token: 0x04006216 RID: 25110
	public Action<int> OnLayerSelected;

	// Token: 0x04006217 RID: 25111
	public Action<int> OnLayerHovered;

	// Token: 0x04006218 RID: 25112
	public XUiC_SignLayerGrid.MultiSelectionDeselectCallback OnMultiSelectionDeselect;

	// Token: 0x04006219 RID: 25113
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayer selectionOverrideLayer;

	// Token: 0x0400621A RID: 25114
	public readonly List<int> MultiSelectedLayerIndices = new List<int>();

	// Token: 0x0400621E RID: 25118
	public readonly List<int> draggedLayerIndices = new List<int>();

	// Token: 0x04006220 RID: 25120
	[PublicizedFrom(EAccessModifier.Private)]
	public int placeholderIndex = -1;

	// Token: 0x04006221 RID: 25121
	public Action<int> OnDragAndDropStarted;

	// Token: 0x04006222 RID: 25122
	public Action<int> OnDragAndDropReleased;

	// Token: 0x04006223 RID: 25123
	public Action<int> OnAddLayerPressed;

	// Token: 0x04006224 RID: 25124
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController activeArea;

	// Token: 0x04006225 RID: 25125
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInsideActiveArea;

	// Token: 0x04006226 RID: 25126
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnAddLayer;

	// Token: 0x04006227 RID: 25127
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBtnAddLayerHovered;

	// Token: 0x04006228 RID: 25128
	[PublicizedFrom(EAccessModifier.Private)]
	public int nearestInsertIndex = -1;

	// Token: 0x04006229 RID: 25129
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController dragAndDropIndicator;

	// Token: 0x0400622A RID: 25130
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController insertIndicator;

	// Token: 0x0400622B RID: 25131
	public bool isDraggingLayers;

	// Token: 0x0400622C RID: 25132
	[PublicizedFrom(EAccessModifier.Private)]
	public int dragStartIdx;

	// Token: 0x02001056 RID: 4182
	// (Invoke) Token: 0x06008432 RID: 33842
	public delegate void MultiSelectionDeselectCallback(bool wasPrimary);
}
