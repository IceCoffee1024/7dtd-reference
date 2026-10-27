using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000F3A RID: 3898
[Preserve]
public class XUiC_DewCollectorContainer : XUiC_VariableHeightGrid, ITileEntityChangedListener
{
	// Token: 0x17000E55 RID: 3669
	// (get) Token: 0x06007990 RID: 31120 RVA: 0x002E0B82 File Offset: 0x002DED82
	// (set) Token: 0x06007991 RID: 31121 RVA: 0x002E0B8A File Offset: 0x002DED8A
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007992 RID: 31122 RVA: 0x002E0B93 File Offset: 0x002DED93
	public override ItemStack[] GetSlots()
	{
		return this.localTileEntity.Items;
	}

	// Token: 0x06007993 RID: 31123 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007994 RID: 31124 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
	}

	// Token: 0x06007995 RID: 31125 RVA: 0x002E0BA0 File Offset: 0x002DEDA0
	public void SetTileEntity(TileEntityCollector _te)
	{
		if (_te != null)
		{
			base.SetHeight(_te.OutputWindowHeight);
		}
	}

	// Token: 0x06007996 RID: 31126 RVA: 0x002E0BB4 File Offset: 0x002DEDB4
	public void SetSlots(TileEntityCollector lootContainer, ItemStack[] stackList)
	{
		if (stackList == null)
		{
			return;
		}
		this.localTileEntity = lootContainer;
		this.items = this.localTileEntity.Items;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		XUiV_Grid xuiV_Grid = (XUiV_Grid)this.viewComponent;
		xuiV_Grid.Columns = lootContainer.GetContainerSize().x;
		xuiV_Grid.Rows = lootContainer.GetContainerSize().y;
		int num = stackList.Length;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.SlotNumber = i;
			xuiC_ItemStack.SlotChangedEvent -= this.HandleLootSlotChangedEvent;
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.OverrideStackCount = 1;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.LootContainer;
			if (i < num)
			{
				this.SetItemInSlot(i, this.localTileEntity.Items[i], false);
				this.itemControllers[i].ViewComponent.IsVisible = true;
				xuiC_ItemStack.SlotChangedEvent += this.HandleLootSlotChangedEvent;
			}
			else
			{
				xuiC_ItemStack.ItemStack = ItemStack.Empty;
				this.itemControllers[i].ViewComponent.IsVisible = false;
			}
		}
		this.localTileEntity.SetModified();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007997 RID: 31127 RVA: 0x002E0CEC File Offset: 0x002DEEEC
	public override void Init()
	{
		base.Init();
		XUiV_Grid xuiV_Grid = (XUiV_Grid)this.viewComponent;
		this.GridCellSize = new Vector2i(xuiV_Grid.CellWidth, xuiV_Grid.CellHeight);
		base.InitVariableHeightGrids(new XUiV_Grid[] { xuiV_Grid });
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_DewCollectorStack xuiC_DewCollectorStack = (XUiC_DewCollectorStack)this.itemControllers[i];
			xuiC_DewCollectorStack.SetAllowedItemClassSingle(this.requiredItem);
			xuiC_DewCollectorStack.RequiredItemOnly = true;
			xuiC_DewCollectorStack.TakeOnly = true;
		}
	}

	// Token: 0x06007998 RID: 31128 RVA: 0x002E0D6B File Offset: 0x002DEF6B
	public void HandleLootSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		this.localTileEntity.UpdateSlot(slotNumber, stack);
		this.localTileEntity.SetModified();
	}

	// Token: 0x06007999 RID: 31129 RVA: 0x002E0D88 File Offset: 0x002DEF88
	public void OnTileEntityChanged(ITileEntity _te)
	{
		TileEntityCollector tileEntityCollector = _te as TileEntityCollector;
		base.SetHeight(tileEntityCollector.OutputWindowHeight);
		ItemStack[] slots = this.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.SlotChangedEvent -= this.HandleLootSlotChangedEvent;
			this.SetItemInSlot(i, slots[i], true);
			xuiC_ItemStack.SlotChangedEvent += this.HandleLootSlotChangedEvent;
		}
	}

	// Token: 0x0600799A RID: 31130 RVA: 0x002E0DF4 File Offset: 0x002DEFF4
	public void SetItemInSlot(int i, ItemStack stack, bool onTEChanged)
	{
		ItemStack itemStack = ItemStack.Empty;
		if (stack != null)
		{
			itemStack = stack.Clone();
		}
		XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
		ItemStack itemStack2 = ItemStack.Empty;
		if (xuiC_ItemStack != null)
		{
			if (xuiC_ItemStack.ItemStack != null)
			{
				itemStack2 = xuiC_ItemStack.ItemStack;
			}
			xuiC_ItemStack.ItemStack = itemStack.Clone();
		}
		if (onTEChanged && itemStack2.IsEmpty() && !itemStack.IsEmpty())
		{
			Manager.PlayInsidePlayerHead(this.localTileEntity.GetSlotOutputType(i).ConvertSound, -1, 0f, false, false);
		}
		XUiC_DewCollectorStack xuiC_DewCollectorStack = (XUiC_DewCollectorStack)xuiC_ItemStack;
		if (xuiC_DewCollectorStack != null)
		{
			TileEntityCollector.FillData slotFillData = this.localTileEntity.GetSlotFillData(i);
			if (slotFillData != null)
			{
				xuiC_DewCollectorStack.FillAmount = (float)(slotFillData.fillTime - slotFillData.fillTimeLeft);
				xuiC_DewCollectorStack.MaxFill = (float)slotFillData.fillTime;
			}
			xuiC_DewCollectorStack.IsCurrentStack = this.localTileEntity.IsCurrentStack(i);
			xuiC_DewCollectorStack.IsBlocked = this.localTileEntity.IsSlotDisabled(i);
			xuiC_DewCollectorStack.IsModded = this.localTileEntity.HasModConvert;
			xuiC_DewCollectorStack.RefreshBindings();
		}
	}

	// Token: 0x0600799B RID: 31131 RVA: 0x002E0EEC File Offset: 0x002DF0EC
	public override void OnOpen()
	{
		base.OnOpen();
		if (!this.localTileEntity.listeners.Contains(this))
		{
			this.localTileEntity.listeners.Add(this);
		}
		this.localTileEntity.Destroyed += this.LocalTileEntity_Destroyed;
		this.blockValue = GameManager.Instance.World.GetBlock(this.localTileEntity.ToWorldPos());
	}

	// Token: 0x0600799C RID: 31132 RVA: 0x002E0F5A File Offset: 0x002DF15A
	[PublicizedFrom(EAccessModifier.Private)]
	public void LocalTileEntity_Destroyed(ITileEntity te)
	{
		if (GameManager.Instance != null)
		{
			if (te == this.localTileEntity)
			{
				XUiC_DewCollectorWindowGroup.CloseIfOpenAtPos(te.ToWorldPos(), null);
				return;
			}
			te.Destroyed -= this.LocalTileEntity_Destroyed;
		}
	}

	// Token: 0x0600799D RID: 31133 RVA: 0x002E0F94 File Offset: 0x002DF194
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
			this.localTileEntity = null;
		}
	}

	// Token: 0x0600799E RID: 31134 RVA: 0x002E0FFE File Offset: 0x002DF1FE
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "required_item")
		{
			this.requiredItem = _value;
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x04005A22 RID: 23074
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector localTileEntity;

	// Token: 0x04005A23 RID: 23075
	[PublicizedFrom(EAccessModifier.Private)]
	public string requiredItem = "";

	// Token: 0x04005A25 RID: 23077
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;
}
