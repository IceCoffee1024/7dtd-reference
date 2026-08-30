using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101D RID: 4125
[Preserve]
public class XUiC_SignLayerGrid : XUiController
{
	// Token: 0x17000F15 RID: 3861
	// (get) Token: 0x060082A0 RID: 33440 RVA: 0x0031CDB0 File Offset: 0x0031AFB0
	// (set) Token: 0x060082A1 RID: 33441 RVA: 0x0031CDB8 File Offset: 0x0031AFB8
	public int SelectedLayerIndex
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	} = -1;

	// Token: 0x17000F16 RID: 3862
	// (get) Token: 0x060082A2 RID: 33442 RVA: 0x0031CDC1 File Offset: 0x0031AFC1
	// (set) Token: 0x060082A3 RID: 33443 RVA: 0x0031CDC9 File Offset: 0x0031AFC9
	public int SlotCount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F17 RID: 3863
	// (get) Token: 0x060082A4 RID: 33444 RVA: 0x0031CDD2 File Offset: 0x0031AFD2
	// (set) Token: 0x060082A5 RID: 33445 RVA: 0x0031CDDA File Offset: 0x0031AFDA
	public int LayerCount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F18 RID: 3864
	// (get) Token: 0x060082A6 RID: 33446 RVA: 0x0031CDE3 File Offset: 0x0031AFE3
	public int ItemCount
	{
		get
		{
			return this.LayerCount + (this.HasPlaceholder ? 1 : 0);
		}
	}

	// Token: 0x17000F19 RID: 3865
	// (get) Token: 0x060082A7 RID: 33447 RVA: 0x0031CDF8 File Offset: 0x0031AFF8
	// (set) Token: 0x060082A8 RID: 33448 RVA: 0x0031CE00 File Offset: 0x0031B000
	public Action OnAddLayerSelected
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x17000F1A RID: 3866
	// (get) Token: 0x060082A9 RID: 33449 RVA: 0x0031CE09 File Offset: 0x0031B009
	public bool HasPlaceholder
	{
		get
		{
			return this.placeholderIndex >= 0;
		}
	}

	// Token: 0x060082AA RID: 33450 RVA: 0x0031CE18 File Offset: 0x0031B018
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

	// Token: 0x060082AB RID: 33451 RVA: 0x0031D018 File Offset: 0x0031B218
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

	// Token: 0x060082AC RID: 33452 RVA: 0x0031D0E0 File Offset: 0x0031B2E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnNextLayerOnPress(XUiController sender, int mouseButton)
	{
		this.TryScroll(1);
	}

	// Token: 0x060082AD RID: 33453 RVA: 0x0031D0EA File Offset: 0x0031B2EA
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPreviousLayerOnPress(XUiController sender, int mouseButton)
	{
		this.TryScroll(-1);
	}

	// Token: 0x060082AE RID: 33454 RVA: 0x0031D0F4 File Offset: 0x0031B2F4
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

	// Token: 0x060082AF RID: 33455 RVA: 0x0031D1E0 File Offset: 0x0031B3E0
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

	// Token: 0x060082B0 RID: 33456 RVA: 0x0031D220 File Offset: 0x0031B420
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

	// Token: 0x060082B1 RID: 33457 RVA: 0x0031D3F8 File Offset: 0x0031B5F8
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

	// Token: 0x060082B2 RID: 33458 RVA: 0x0031D43C File Offset: 0x0031B63C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnScroll(XUiController _sender, float _delta)
	{
		this.TryScroll((_delta > 0f) ? (-1) : 1);
	}

	// Token: 0x060082B3 RID: 33459 RVA: 0x0031D454 File Offset: 0x0031B654
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

	// Token: 0x060082B4 RID: 33460 RVA: 0x0031D4D2 File Offset: 0x0031B6D2
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

	// Token: 0x060082B5 RID: 33461 RVA: 0x0031D50B File Offset: 0x0031B70B
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

	// Token: 0x060082B6 RID: 33462 RVA: 0x0031D523 File Offset: 0x0031B723
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

	// Token: 0x060082B7 RID: 33463 RVA: 0x0031D548 File Offset: 0x0031B748
	public void RefreshSelectedLayerBindings()
	{
		int num = this.SelectedLayerIndex - this.baseLayerIndex;
		if (num < 0 || num >= this.SlotCount)
		{
			return;
		}
		this.signLayerControllers[num].RefreshBindings();
	}

	// Token: 0x060082B8 RID: 33464 RVA: 0x0031D580 File Offset: 0x0031B780
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

	// Token: 0x060082B9 RID: 33465 RVA: 0x0031D70A File Offset: 0x0031B90A
	public void SetLayerName(string name)
	{
		this.signLayerControllers[this.SelectedLayerIndex - this.baseLayerIndex].layer.name = name;
		this.signLayerControllers[this.SelectedLayerIndex - this.baseLayerIndex].RefreshTooltip();
	}

	// Token: 0x060082BA RID: 33466 RVA: 0x0031D744 File Offset: 0x0031B944
	public bool MousePositionInsideActiveArea()
	{
		Vector3 vector = this.xui.playerUI.camera.ScreenToWorldPoint(Input.mousePosition);
		vector.z = this.activeArea.ViewComponent.ColliderCenter.z;
		return this.activeArea.ViewComponent.ColliderBounds.Contains(vector);
	}

	// Token: 0x060082BB RID: 33467 RVA: 0x002F7F7E File Offset: 0x002F617E
	public override void OnOpen()
	{
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = true;
		}
	}

	// Token: 0x060082BC RID: 33468 RVA: 0x0031D7A1 File Offset: 0x0031B9A1
	public override void OnClose()
	{
		this.MultiSelectedLayerIndices.Clear();
		if (base.ViewComponent != null && base.ViewComponent.IsVisible)
		{
			base.ViewComponent.IsVisible = false;
		}
	}

	// Token: 0x060082BD RID: 33469 RVA: 0x0031D7D0 File Offset: 0x0031B9D0
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

	// Token: 0x060082BE RID: 33470 RVA: 0x0031D970 File Offset: 0x0031BB70
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

	// Token: 0x060082BF RID: 33471 RVA: 0x0031DA3B File Offset: 0x0031BC3B
	[PublicizedFrom(EAccessModifier.Private)]
	public float GetInsertPositionX(int visibleSlot)
	{
		if (visibleSlot != 0)
		{
			return this.signLayerControllers[visibleSlot - 1].RightInsertX;
		}
		return this.signLayerControllers[0].LeftInsertX;
	}

	// Token: 0x060082C0 RID: 33472 RVA: 0x0031DA60 File Offset: 0x0031BC60
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

	// Token: 0x060082C1 RID: 33473 RVA: 0x000027FC File Offset: 0x000009FC
	[Conditional("DEBUG_LOG_SIGN_LAYER_GRID")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugLog(string message)
	{
	}

	// Token: 0x040060D2 RID: 24786
	[PublicizedFrom(EAccessModifier.Private)]
	public int baseLayerIndex;

	// Token: 0x040060D3 RID: 24787
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignLayer[] signLayerControllers;

	// Token: 0x040060D4 RID: 24788
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId currentId;

	// Token: 0x040060D5 RID: 24789
	[PublicizedFrom(EAccessModifier.Private)]
	public int hoveredLayerIndex = -1;

	// Token: 0x040060D6 RID: 24790
	public Action<int> OnLayerSelected;

	// Token: 0x040060D7 RID: 24791
	public Action<int> OnLayerHovered;

	// Token: 0x040060D8 RID: 24792
	public XUiC_SignLayerGrid.MultiSelectionDeselectCallback OnMultiSelectionDeselect;

	// Token: 0x040060D9 RID: 24793
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SignLayer selectionOverrideLayer;

	// Token: 0x040060DA RID: 24794
	public readonly List<int> MultiSelectedLayerIndices = new List<int>();

	// Token: 0x040060DE RID: 24798
	public readonly List<int> draggedLayerIndices = new List<int>();

	// Token: 0x040060E0 RID: 24800
	[PublicizedFrom(EAccessModifier.Private)]
	public int placeholderIndex = -1;

	// Token: 0x040060E1 RID: 24801
	public Action<int> OnDragAndDropStarted;

	// Token: 0x040060E2 RID: 24802
	public Action<int> OnDragAndDropReleased;

	// Token: 0x040060E3 RID: 24803
	public Action<int> OnAddLayerPressed;

	// Token: 0x040060E4 RID: 24804
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController activeArea;

	// Token: 0x040060E5 RID: 24805
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInsideActiveArea;

	// Token: 0x040060E6 RID: 24806
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnAddLayer;

	// Token: 0x040060E7 RID: 24807
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBtnAddLayerHovered;

	// Token: 0x040060E8 RID: 24808
	[PublicizedFrom(EAccessModifier.Private)]
	public int nearestInsertIndex = -1;

	// Token: 0x040060E9 RID: 24809
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController dragAndDropIndicator;

	// Token: 0x040060EA RID: 24810
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController insertIndicator;

	// Token: 0x040060EB RID: 24811
	public bool isDraggingLayers;

	// Token: 0x040060EC RID: 24812
	[PublicizedFrom(EAccessModifier.Private)]
	public int dragStartIdx;

	// Token: 0x0200101E RID: 4126
	// (Invoke) Token: 0x060082C5 RID: 33477
	public delegate void MultiSelectionDeselectCallback(bool wasPrimary);
}
