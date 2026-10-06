using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000F2B RID: 3883
[Preserve]
public class XUiC_DewCollectorContainer : XUiC_VariableHeightGrid, ITileEntityChangedListener
{
	// Token: 0x17000E4B RID: 3659
	// (get) Token: 0x0600794D RID: 31053 RVA: 0x002DF292 File Offset: 0x002DD492
	// (set) Token: 0x0600794E RID: 31054 RVA: 0x002DF29A File Offset: 0x002DD49A
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600794F RID: 31055 RVA: 0x002DF2A3 File Offset: 0x002DD4A3
	public override ItemStack[] GetSlots()
	{
		return this.localTileEntity.Items;
	}

	// Token: 0x06007950 RID: 31056 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007951 RID: 31057 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
	}

	// Token: 0x06007952 RID: 31058 RVA: 0x002DF2B0 File Offset: 0x002DD4B0
	public void SetTileEntity(TileEntityCollector _te)
	{
		if (_te != null)
		{
			base.SetHeight(_te.OutputWindowHeight);
		}
	}

	// Token: 0x06007953 RID: 31059 RVA: 0x002DF2C4 File Offset: 0x002DD4C4
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

	// Token: 0x06007954 RID: 31060 RVA: 0x002DF3FC File Offset: 0x002DD5FC
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

	// Token: 0x06007955 RID: 31061 RVA: 0x002DF47B File Offset: 0x002DD67B
	public void HandleLootSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		this.localTileEntity.UpdateSlot(slotNumber, stack);
		this.localTileEntity.SetModified();
	}

	// Token: 0x06007956 RID: 31062 RVA: 0x002DF498 File Offset: 0x002DD698
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

	// Token: 0x06007957 RID: 31063 RVA: 0x002DF504 File Offset: 0x002DD704
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

	// Token: 0x06007958 RID: 31064 RVA: 0x002DF5FC File Offset: 0x002DD7FC
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

	// Token: 0x06007959 RID: 31065 RVA: 0x002DF66A File Offset: 0x002DD86A
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

	// Token: 0x0600795A RID: 31066 RVA: 0x002DF6A4 File Offset: 0x002DD8A4
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

	// Token: 0x0600795B RID: 31067 RVA: 0x002DF70E File Offset: 0x002DD90E
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "required_item")
		{
			this.requiredItem = _value;
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x040059EF RID: 23023
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector localTileEntity;

	// Token: 0x040059F0 RID: 23024
	[PublicizedFrom(EAccessModifier.Private)]
	public string requiredItem = "";

	// Token: 0x040059F2 RID: 23026
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;
}
