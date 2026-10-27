using System;
using UnityEngine.Scripting;

// Token: 0x02000F9B RID: 3995
[Preserve]
public class XUiC_LootContainer : XUiC_ItemStackGrid, ITileEntityChangedListener
{
	// Token: 0x17000EDD RID: 3805
	// (get) Token: 0x06007D73 RID: 32115 RVA: 0x002F94CD File Offset: 0x002F76CD
	// (set) Token: 0x06007D74 RID: 32116 RVA: 0x002F94D5 File Offset: 0x002F76D5
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007D75 RID: 32117 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007D76 RID: 32118 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007D77 RID: 32119 RVA: 0x002F94E0 File Offset: 0x002F76E0
	public void SetSlots(TEFeatureStorage _lootContainer)
	{
		this.localTileEntity = _lootContainer;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		this.grid.Columns = _lootContainer.ItemGrid.ContainerSize.x;
		this.grid.Rows = _lootContainer.ItemGrid.ContainerSize.y;
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
				xuiC_ItemStack.SlotChangedEvent += this.HandleLootSlotChangedEvent;
			}
			else
			{
				xuiC_ItemStack.ItemStack = ItemStack.Empty;
			}
		}
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007D78 RID: 32120 RVA: 0x002F95E3 File Offset: 0x002F77E3
	public override void Init()
	{
		base.Init();
		this.GridCellSize = new Vector2i(this.grid.CellWidth, this.grid.CellHeight);
	}

	// Token: 0x06007D79 RID: 32121 RVA: 0x002F960C File Offset: 0x002F780C
	public void HandleLootSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		this.localTileEntity.UpdateSlot(_slotNumber, _stack);
		this.localTileEntity.SetModified();
		base.updateSearch();
	}

	// Token: 0x06007D7A RID: 32122 RVA: 0x002F962C File Offset: 0x002F782C
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
		base.updateSearch();
	}

	// Token: 0x06007D7B RID: 32123 RVA: 0x002F9694 File Offset: 0x002F7894
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

	// Token: 0x06007D7C RID: 32124 RVA: 0x002F96F4 File Offset: 0x002F78F4
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

	// Token: 0x06007D7D RID: 32125 RVA: 0x002F9744 File Offset: 0x002F7944
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

	// Token: 0x06007D7E RID: 32126 RVA: 0x002F97C9 File Offset: 0x002F79C9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void dropItemToGrid(ItemStack _dropStack)
	{
		base.dropItemToGrid(_dropStack);
		if (XUiM_LootContainer.AddItem(_dropStack, this.xui))
		{
			_dropStack.count = 0;
		}
	}

	// Token: 0x04005D10 RID: 23824
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureStorage localTileEntity;

	// Token: 0x04005D11 RID: 23825
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiV_Grid grid;
}
