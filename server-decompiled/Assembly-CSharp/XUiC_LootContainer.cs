using System;
using UnityEngine.Scripting;

// Token: 0x02000F8B RID: 3979
[Preserve]
public class XUiC_LootContainer : XUiC_ItemStackGrid, ITileEntityChangedListener
{
	// Token: 0x17000ED1 RID: 3793
	// (get) Token: 0x06007D24 RID: 32036 RVA: 0x002F7845 File Offset: 0x002F5A45
	// (set) Token: 0x06007D25 RID: 32037 RVA: 0x002F784D File Offset: 0x002F5A4D
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007D26 RID: 32038 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007D27 RID: 32039 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007D28 RID: 32040 RVA: 0x002F7858 File Offset: 0x002F5A58
	public void SetSlots(TEFeatureStorage _lootContainer)
	{
		this.localTileEntity = _lootContainer;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		XUiV_Grid xuiV_Grid = (XUiV_Grid)this.viewComponent;
		xuiV_Grid.Columns = _lootContainer.ItemGrid.ContainerSize.x;
		xuiV_Grid.Rows = _lootContainer.ItemGrid.ContainerSize.y;
		int length = this.localTileEntity.ItemGrid.Length;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.SlotNumber = i;
			xuiC_ItemStack.SlotChangedEvent -= this.HandleLootSlotChangedEvent;
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.LootContainer;
			if (i < length)
			{
				xuiC_ItemStack.ForceSetItemStack(this.localTileEntity.ItemGrid[i].Clone());
				this.itemControllers[i].ViewComponent.IsVisible = true;
				xuiC_ItemStack.SlotChangedEvent += this.HandleLootSlotChangedEvent;
			}
			else
			{
				xuiC_ItemStack.ItemStack = ItemStack.Empty;
				this.itemControllers[i].ViewComponent.IsVisible = false;
			}
		}
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007D29 RID: 32041 RVA: 0x002F7984 File Offset: 0x002F5B84
	public override void Init()
	{
		base.Init();
		XUiV_Grid xuiV_Grid = (XUiV_Grid)this.viewComponent;
		this.GridCellSize = new Vector2i(xuiV_Grid.CellWidth, xuiV_Grid.CellHeight);
	}

	// Token: 0x06007D2A RID: 32042 RVA: 0x002F79BA File Offset: 0x002F5BBA
	public void HandleLootSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		this.localTileEntity.UpdateSlot(_slotNumber, _stack);
		this.localTileEntity.SetModified();
	}

	// Token: 0x06007D2B RID: 32043 RVA: 0x002F79D4 File Offset: 0x002F5BD4
	public void OnTileEntityChanged(ITileEntity _te)
	{
		ItemStackGrid itemGrid = this.localTileEntity.ItemGrid;
		for (int i = 0; i < itemGrid.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.SlotChangedEvent -= this.HandleLootSlotChangedEvent;
			xuiC_ItemStack.ItemStack = itemGrid[i];
			xuiC_ItemStack.SlotChangedEvent += this.HandleLootSlotChangedEvent;
		}
	}

	// Token: 0x06007D2C RID: 32044 RVA: 0x002F7A38 File Offset: 0x002F5C38
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.localTileEntity.listeners.Contains(this))
		{
			this.localTileEntity.listeners.Add(this);
		}
		this.xui.LootContainer = this.localTileEntity;
		this.localTileEntity.Destroyed += this.LocalTileEntity_Destroyed;
	}

	// Token: 0x06007D2D RID: 32045 RVA: 0x002F7A98 File Offset: 0x002F5C98
	[PublicizedFrom(EAccessModifier.Private)]
	public void LocalTileEntity_Destroyed(ITileEntity _te)
	{
		if (GameManager.Instance != null)
		{
			if (_te == this.localTileEntity)
			{
				this.xui.playerUI.windowManager.Close(XUiC_LootWindowGroup.ID);
				return;
			}
			_te.Destroyed -= this.LocalTileEntity_Destroyed;
		}
	}

	// Token: 0x06007D2E RID: 32046 RVA: 0x002F7AE8 File Offset: 0x002F5CE8
	public override void OnClose()
	{
		base.OnClose();
		this.xui.LootContainer = null;
		if (this.localTileEntity != null)
		{
			this.localTileEntity.Destroyed -= this.LocalTileEntity_Destroyed;
			if (this.localTileEntity.listeners.Contains(this))
			{
				this.localTileEntity.listeners.Remove(this);
			}
			QuestEventManager.Current.ClosedContainer(this.localTileEntity.ToWorldPos(), this.localTileEntity);
			this.localTileEntity = null;
		}
	}

	// Token: 0x04005CD6 RID: 23766
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureStorage localTileEntity;
}
