using System;
using UnityEngine.Scripting;

// Token: 0x02000EE1 RID: 3809
[Preserve]
public class XUiC_CollectorFuelGrid : XUiC_VariableHeightGrid, ITileEntityChangedListener
{
	// Token: 0x17000DFA RID: 3578
	// (get) Token: 0x06007777 RID: 30583 RVA: 0x002D0964 File Offset: 0x002CEB64
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			XUiC_ItemStack.StackLocationTypes stackLocationTypes = XUiC_ItemStack.StackLocationTypes.CollectorFuel;
			if (this.fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
			{
				stackLocationTypes = XUiC_ItemStack.StackLocationTypes.CollectorCatalysts;
			}
			return stackLocationTypes;
		}
	}

	// Token: 0x06007778 RID: 30584 RVA: 0x002D0984 File Offset: 0x002CEB84
	public override void OnOpen()
	{
		base.OnOpen();
		XUiC_CollectorFuelGrid.FuelBehavior fuelBehavior = this.fuelBehavior;
		if (fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Fuel)
		{
			this.xui.CurrentCollectorFuelGrid = this;
			return;
		}
		if (fuelBehavior != XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
		{
			return;
		}
		this.xui.currentCollectorCatalystGrid = this;
	}

	// Token: 0x06007779 RID: 30585 RVA: 0x002D09C0 File Offset: 0x002CEBC0
	public override void OnClose()
	{
		base.OnClose();
		XUiC_CollectorFuelGrid.FuelBehavior fuelBehavior = this.fuelBehavior;
		if (fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Fuel)
		{
			this.xui.CurrentCollectorFuelGrid = null;
			return;
		}
		if (fuelBehavior != XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
		{
			return;
		}
		this.xui.currentCollectorCatalystGrid = null;
	}

	// Token: 0x0600777A RID: 30586 RVA: 0x002D09FC File Offset: 0x002CEBFC
	public override bool ParseAttribute(string _name, string _value)
	{
		bool flag;
		if (_name == "fuelbehavior")
		{
			if (!Enum.TryParse<XUiC_CollectorFuelGrid.FuelBehavior>(_value, out this.fuelBehavior))
			{
				this.fuelBehavior = XUiC_CollectorFuelGrid.FuelBehavior.Fuel;
			}
			flag = true;
		}
		else
		{
			flag = base.ParseAttribute(_name, _value);
		}
		return flag;
	}

	// Token: 0x0600777B RID: 30587 RVA: 0x002D0A3C File Offset: 0x002CEC3C
	public bool TryAddFuel(ItemClass newItemClass, ItemStack newItemStack)
	{
		string text = "";
		string[] array = new string[0];
		XUiC_CollectorFuelGrid.FuelBehavior fuelBehavior = this.fuelBehavior;
		if (fuelBehavior != XUiC_CollectorFuelGrid.FuelBehavior.Fuel)
		{
			if (fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
			{
				array = this.tileEntity.GetCatalystTypes();
			}
		}
		else
		{
			array = this.tileEntity.GetFuelTypes();
		}
		foreach (string text2 in array)
		{
			if (newItemClass.Name == text2)
			{
				text = text2;
				break;
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			int num = 0;
			fuelBehavior = this.fuelBehavior;
			if (fuelBehavior != XUiC_CollectorFuelGrid.FuelBehavior.Fuel)
			{
				if (fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
				{
					num = this.tileEntity.CatalystGridLength;
				}
			}
			else
			{
				num = this.tileEntity.FuelGridLength;
			}
			for (int j = 0; j < num; j++)
			{
				XUiC_ItemStack xuiC_ItemStack = this.itemControllers[j];
				if (xuiC_ItemStack.ItemStack.IsEmpty())
				{
					xuiC_ItemStack.ItemStack = newItemStack.Clone();
					newItemStack.count = 0;
					break;
				}
				int num2 = xuiC_ItemStack.ItemStack.StackTransferCount(newItemStack);
				xuiC_ItemStack.ItemStack.count += num2;
				newItemStack.count -= num2;
				if (newItemStack.count == 0)
				{
					break;
				}
			}
		}
		this.UpdateBackend(this.getUISlots());
		return newItemStack.count == 0;
	}

	// Token: 0x0600777C RID: 30588 RVA: 0x002D0B78 File Offset: 0x002CED78
	public override void Init()
	{
		base.Init();
		XUiV_Grid xuiV_Grid = this.viewComponent as XUiV_Grid;
		XUiController childById = this.parent.Parent.GetChildById("slot_preview");
		XUiV_Grid xuiV_Grid2 = childById.ViewComponent as XUiV_Grid;
		base.InitVariableHeightGrids(new XUiV_Grid[] { xuiV_Grid, xuiV_Grid2 });
		XUiController[] childrenById = childById.GetChildrenById("slot", null);
		int num = childrenById.Length;
		this.previewSprites = new XUiV_Sprite[num];
		for (int i = 0; i < num; i++)
		{
			this.previewSprites[i] = childrenById[i].ViewComponent as XUiV_Sprite;
		}
	}

	// Token: 0x0600777D RID: 30589 RVA: 0x002D0C14 File Offset: 0x002CEE14
	public void SetTileEntity(TileEntityCollector te)
	{
		this.tileEntity = te;
		if (!this.tileEntity.listeners.Contains(this))
		{
			this.tileEntity.listeners.Add(this);
		}
		this.SetStacks(te.FuelSlots);
		this.blockCollector = te.blockValue.Block as BlockCollector;
		string[] array = new string[0];
		string[] array2 = new string[0];
		if (this.blockCollector != null)
		{
			if (this.fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
			{
				base.SetHeight(te.CatalystGridHeight);
				array = this.blockCollector.CatalystTypesSprites;
				array2 = this.blockCollector.CatalystTypesSpriteAtlases;
			}
			else
			{
				base.SetHeight(te.FuelGridHeight);
				array = this.blockCollector.FuelTypesSprites;
				array2 = this.blockCollector.FuelTypesSpriteAtlases;
			}
		}
		int num = array.Length;
		if (num > 0)
		{
			for (int i = 0; i < this.previewSprites.Length; i++)
			{
				int num2 = i % num;
				this.previewSprites[i].SpriteName = array[num2];
				this.previewSprites[i].UIAtlas = array2[num2];
			}
		}
	}

	// Token: 0x0600777E RID: 30590 RVA: 0x002D0D20 File Offset: 0x002CEF20
	public void OnTileEntityChanged(ITileEntity te)
	{
		TileEntityCollector tileEntityCollector = te as TileEntityCollector;
		if (tileEntityCollector != null && tileEntityCollector == this.tileEntity)
		{
			XUiC_CollectorFuelGrid.FuelBehavior fuelBehavior = this.fuelBehavior;
			if (fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Fuel)
			{
				this.SetStacks(tileEntityCollector.FuelSlots);
				base.SetHeight(tileEntityCollector.FuelGridHeight);
				return;
			}
			if (fuelBehavior != XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
			{
				return;
			}
			this.SetStacks(tileEntityCollector.CatalystSlots);
			base.SetHeight(tileEntityCollector.CatalystGridHeight);
		}
	}

	// Token: 0x0600777F RID: 30591 RVA: 0x002D0D80 File Offset: 0x002CEF80
	public override void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		ItemStack[] uislots = this.getUISlots();
		XUiC_CollectorFuelGrid.FuelBehavior fuelBehavior = this.fuelBehavior;
		if (fuelBehavior != XUiC_CollectorFuelGrid.FuelBehavior.Fuel && fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
		{
			BlockCollector.CatalystConvert[] catalystConverts = this.blockCollector.CatalystConverts;
			for (int i = 0; i < catalystConverts.Length; i++)
			{
				ItemStack itemStack = catalystConverts[i].Convert(stack);
				if (itemStack != null)
				{
					uislots[slotNumber] = itemStack;
					break;
				}
			}
		}
		if (this.items != null)
		{
			this.items[slotNumber] = stack.Clone();
		}
		this.UpdateBackend(uislots);
		fuelBehavior = this.fuelBehavior;
		if (fuelBehavior != XUiC_CollectorFuelGrid.FuelBehavior.Fuel && fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
		{
			this.SetStacks(this.tileEntity.CatalystSlots);
		}
	}

	// Token: 0x06007780 RID: 30592 RVA: 0x002D0E10 File Offset: 0x002CF010
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		base.UpdateBackend(stackList);
		XUiC_CollectorFuelGrid.FuelBehavior fuelBehavior = this.fuelBehavior;
		if (fuelBehavior != XUiC_CollectorFuelGrid.FuelBehavior.Fuel)
		{
			if (fuelBehavior == XUiC_CollectorFuelGrid.FuelBehavior.Catalyst)
			{
				this.tileEntity.CatalystSlots = stackList;
			}
		}
		else
		{
			this.tileEntity.FuelSlots = stackList;
		}
		this.tileEntity.SetModified();
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06007781 RID: 30593 RVA: 0x002D0E69 File Offset: 0x002CF069
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "container_slots")
		{
			_value = this.collectorFuelGridLength.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x0400582D RID: 22573
	[PublicizedFrom(EAccessModifier.Protected)]
	public new TileEntityCollector tileEntity;

	// Token: 0x0400582E RID: 22574
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockCollector blockCollector;

	// Token: 0x0400582F RID: 22575
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CollectorFuelGrid.FuelBehavior fuelBehavior;

	// Token: 0x04005830 RID: 22576
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite[] previewSprites;

	// Token: 0x04005831 RID: 22577
	[PublicizedFrom(EAccessModifier.Private)]
	public int collectorFuelGridLength;

	// Token: 0x02000EE2 RID: 3810
	[PublicizedFrom(EAccessModifier.Private)]
	public enum FuelBehavior
	{
		// Token: 0x04005833 RID: 22579
		Fuel,
		// Token: 0x04005834 RID: 22580
		Catalyst
	}
}
