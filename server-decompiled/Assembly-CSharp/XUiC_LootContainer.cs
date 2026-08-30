using System;
using UnityEngine.Scripting;

// Token: 0x02000F7C RID: 3964
[Preserve]
public class XUiC_LootContainer : XUiC_ItemStackGrid, ITileEntityChangedListener
{
	// Token: 0x17000EAB RID: 3755
	// (get) Token: 0x06007D14 RID: 32020 RVA: 0x002F4735 File Offset: 0x002F2935
	// (set) Token: 0x06007D15 RID: 32021 RVA: 0x002F473D File Offset: 0x002F293D
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007D16 RID: 32022 RVA: 0x002F4746 File Offset: 0x002F2946
	public override ItemStack[] GetSlots()
	{
		return this.localTileEntity.items;
	}

	// Token: 0x06007D17 RID: 32023 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007D18 RID: 32024 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007D19 RID: 32025 RVA: 0x002F4754 File Offset: 0x002F2954
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

	// Token: 0x06007D1A RID: 32026 RVA: 0x002F487C File Offset: 0x002F2A7C
	public override void Init()
	{
		base.Init();
		XUiV_Grid xuiV_Grid = (XUiV_Grid)this.viewComponent;
		this.GridCellSize = new Vector2i(xuiV_Grid.CellWidth, xuiV_Grid.CellHeight);
	}

	// Token: 0x06007D1B RID: 32027 RVA: 0x002F48B2 File Offset: 0x002F2AB2
	public void HandleLootSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		this.localTileEntity.UpdateSlot(_slotNumber, _stack);
		this.localTileEntity.SetModified();
	}

	// Token: 0x06007D1C RID: 32028 RVA: 0x002F48CC File Offset: 0x002F2ACC
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

	// Token: 0x06007D1D RID: 32029 RVA: 0x002F4924 File Offset: 0x002F2B24
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

	// Token: 0x06007D1E RID: 32030 RVA: 0x002F49A0 File Offset: 0x002F2BA0
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

	// Token: 0x06007D1F RID: 32031 RVA: 0x002F49F0 File Offset: 0x002F2BF0
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

	// Token: 0x04005CAD RID: 23725
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileEntityLootable localTileEntity;
}
