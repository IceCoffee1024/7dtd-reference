using System;
using UnityEngine.Scripting;

// Token: 0x02000F63 RID: 3939
[Preserve]
public class XUiC_LootContainer : XUiC_ItemStackGrid, ITileEntityChangedListener
{
	// Token: 0x17000E72 RID: 3698
	// (get) Token: 0x06007C22 RID: 31778 RVA: 0x002EFF61 File Offset: 0x002EE161
	// (set) Token: 0x06007C23 RID: 31779 RVA: 0x002EFF69 File Offset: 0x002EE169
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007C24 RID: 31780 RVA: 0x002EFF72 File Offset: 0x002EE172
	public override ItemStack[] GetSlots()
	{
		return this.localTileEntity.items;
	}

	// Token: 0x06007C25 RID: 31781 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007C26 RID: 31782 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007C27 RID: 31783 RVA: 0x002EFF80 File Offset: 0x002EE180
	public void SetSlots(ITileEntityLootable _lootContainer, ItemStack[] _stackList)
	{
		if (_stackList == null)
		{
			return;
		}
		this.localTileEntity = _lootContainer;
		this.items = this.localTileEntity.items;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		XUiV_Grid xuiV_Grid = (XUiV_Grid)this.viewComponent;
		xuiV_Grid.Columns = _lootContainer.GetContainerSize().x;
		xuiV_Grid.Rows = _lootContainer.GetContainerSize().y;
		int num = _stackList.Length;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.SlotNumber = i;
			xuiC_ItemStack.SlotChangedEvent -= this.HandleLootSlotChangedEvent;
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.LootContainer;
			if (i < num)
			{
				xuiC_ItemStack.ForceSetItemStack(this.localTileEntity.items[i].Clone());
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

	// Token: 0x06007C28 RID: 31784 RVA: 0x002F00A8 File Offset: 0x002EE2A8
	public override void Init()
	{
		base.Init();
		XUiV_Grid xuiV_Grid = (XUiV_Grid)this.viewComponent;
		this.GridCellSize = new Vector2i(xuiV_Grid.CellWidth, xuiV_Grid.CellHeight);
	}

	// Token: 0x06007C29 RID: 31785 RVA: 0x002F00DE File Offset: 0x002EE2DE
	public void HandleLootSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		this.localTileEntity.UpdateSlot(_slotNumber, _stack);
		this.localTileEntity.SetModified();
	}

	// Token: 0x06007C2A RID: 31786 RVA: 0x002F00F8 File Offset: 0x002EE2F8
	public void OnTileEntityChanged(ITileEntity _te)
	{
		ItemStack[] slots = this.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.SlotChangedEvent -= this.HandleLootSlotChangedEvent;
			xuiC_ItemStack.ItemStack = slots[i];
			xuiC_ItemStack.SlotChangedEvent += this.HandleLootSlotChangedEvent;
		}
	}

	// Token: 0x06007C2B RID: 31787 RVA: 0x002F0150 File Offset: 0x002EE350
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.localTileEntity.listeners.Contains(this))
		{
			this.localTileEntity.listeners.Add(this);
		}
		this.xui.LootContainer = this.localTileEntity;
		this.localTileEntity.Destroyed += this.LocalTileEntity_Destroyed;
		QuestEventManager.Current.OpenedContainer(this.localTileEntity.ToWorldPos(), this.localTileEntity);
	}

	// Token: 0x06007C2C RID: 31788 RVA: 0x002F01CC File Offset: 0x002EE3CC
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

	// Token: 0x06007C2D RID: 31789 RVA: 0x002F021C File Offset: 0x002EE41C
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

	// Token: 0x04005BE6 RID: 23526
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileEntityLootable localTileEntity;
}
