using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000F05 RID: 3845
[Preserve]
public class XUiC_DewCollectorContainer : XUiC_VariableHeightGrid, ITileEntityChangedListener
{
	// Token: 0x17000E14 RID: 3604
	// (get) Token: 0x0600789A RID: 30874 RVA: 0x002D7F6A File Offset: 0x002D616A
	// (set) Token: 0x0600789B RID: 30875 RVA: 0x002D7F72 File Offset: 0x002D6172
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600789C RID: 30876 RVA: 0x002D7F7B File Offset: 0x002D617B
	public override ItemStack[] GetSlots()
	{
		return this.localTileEntity.Items;
	}

	// Token: 0x0600789D RID: 30877 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x0600789E RID: 30878 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
	}

	// Token: 0x0600789F RID: 30879 RVA: 0x002D7F88 File Offset: 0x002D6188
	public void SetTileEntity(TileEntityCollector _te)
	{
		if (_te != null)
		{
			base.SetHeight(_te.OutputWindowHeight);
		}
	}

	// Token: 0x060078A0 RID: 30880 RVA: 0x002D7F9C File Offset: 0x002D619C
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

	// Token: 0x060078A1 RID: 30881 RVA: 0x002D80D4 File Offset: 0x002D62D4
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

	// Token: 0x060078A2 RID: 30882 RVA: 0x002D8153 File Offset: 0x002D6353
	public void HandleLootSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		this.localTileEntity.UpdateSlot(slotNumber, stack);
		this.localTileEntity.SetModified();
	}

	// Token: 0x060078A3 RID: 30883 RVA: 0x002D8170 File Offset: 0x002D6370
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

	// Token: 0x060078A4 RID: 30884 RVA: 0x002D81DC File Offset: 0x002D63DC
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

	// Token: 0x060078A5 RID: 30885 RVA: 0x002D82D4 File Offset: 0x002D64D4
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

	// Token: 0x060078A6 RID: 30886 RVA: 0x002D8342 File Offset: 0x002D6542
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

	// Token: 0x060078A7 RID: 30887 RVA: 0x002D837C File Offset: 0x002D657C
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

	// Token: 0x060078A8 RID: 30888 RVA: 0x002D83E6 File Offset: 0x002D65E6
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "required_item")
		{
			this.requiredItem = _value;
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x04005905 RID: 22789
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector localTileEntity;

	// Token: 0x04005906 RID: 22790
	[PublicizedFrom(EAccessModifier.Private)]
	public string requiredItem = "";

	// Token: 0x04005908 RID: 22792
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;
}
