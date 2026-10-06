using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F36 RID: 3894
[Preserve]
public class XUiC_DragAndDropWindow : XUiController
{
	// Token: 0x17000E57 RID: 3671
	// (get) Token: 0x060079BB RID: 31163 RVA: 0x002E1290 File Offset: 0x002DF490
	public EntityPlayerLocal entityPlayer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUi xui = this.xui;
			if (xui == null)
			{
				return null;
			}
			LocalPlayerUI playerUI = xui.playerUI;
			if (playerUI == null)
			{
				return null;
			}
			return playerUI.entityPlayer;
		}
	}

	// Token: 0x060079BC RID: 31164 RVA: 0x002E12AE File Offset: 0x002DF4AE
	[PublicizedFrom(EAccessModifier.Internal)]
	public void DropCurrentItem()
	{
		this.xui.PlayerInventory.DropItem(this.CurrentStack);
		this.SetCurrentStack(ItemStack.Empty, true);
	}

	// Token: 0x060079BD RID: 31165 RVA: 0x002E12D4 File Offset: 0x002DF4D4
	[PublicizedFrom(EAccessModifier.Internal)]
	public void DropCurrentItem(int _count)
	{
		if (_count < this.CurrentStack.count)
		{
			ItemStack currentStack = this.CurrentStack;
			currentStack.count = _count;
			this.grid.ChangeCount(0, -_count);
			this.xui.PlayerInventory.DropItem(currentStack);
			return;
		}
		this.DropCurrentItem();
	}

	// Token: 0x17000E58 RID: 3672
	// (get) Token: 0x060079BE RID: 31166 RVA: 0x002E1324 File Offset: 0x002DF524
	public ItemStack CurrentStack
	{
		get
		{
			return this.grid[0].Clone();
		}
	}

	// Token: 0x060079BF RID: 31167 RVA: 0x002E1337 File Offset: 0x002DF537
	public bool IsEmpty()
	{
		return this.grid[0].IsEmpty();
	}

	// Token: 0x060079C0 RID: 31168 RVA: 0x002E134A File Offset: 0x002DF54A
	public void SetCurrentStack(ItemStack stack, bool playPickupSound = true)
	{
		this.lastItemStack = this.CurrentStack;
		this.grid.SetItem(0, stack);
		if (!this.IsEmpty() && playPickupSound)
		{
			this.ItemStackControl.PlayPickupSound(null);
		}
	}

	// Token: 0x060079C1 RID: 31169 RVA: 0x002E137E File Offset: 0x002DF57E
	[PublicizedFrom(EAccessModifier.Private)]
	public void onStackChanged(int _index)
	{
		this.ItemStackControl.ItemStack = this.CurrentStack;
		if (this.entityPlayer != null)
		{
			this.entityPlayer.DragAndDropItem = this.CurrentStack;
			base.RefreshBindings();
		}
	}

	// Token: 0x17000E59 RID: 3673
	// (get) Token: 0x060079C2 RID: 31170 RVA: 0x002E13B6 File Offset: 0x002DF5B6
	public ItemStack LastStack
	{
		get
		{
			return this.lastItemStack;
		}
	}

	// Token: 0x060079C3 RID: 31171 RVA: 0x002E13C0 File Offset: 0x002DF5C0
	public override void Init()
	{
		base.Init();
		this.ItemStackControl = base.GetChildByType<XUiC_ItemStack>();
		this.ItemStackControl.IsDragAndDrop = true;
		this.ItemStackControl.ItemStack = ItemStack.Empty;
		this.grid.SlotChanged += this.onStackChanged;
		this.xui.DragAndDropWindow = this;
	}

	// Token: 0x060079C4 RID: 31172 RVA: 0x002E141E File Offset: 0x002DF61E
	public override void Cleanup()
	{
		base.Cleanup();
		this.grid.SlotChanged -= this.onStackChanged;
		this.xui.DragAndDropWindow = null;
	}

	// Token: 0x060079C5 RID: 31173 RVA: 0x002E144C File Offset: 0x002DF64C
	public override void Update(float _dt)
	{
		if (!this.InMenu)
		{
			this.PlaceItemBackInInventory();
		}
		if (!this.IsEmpty())
		{
			base.ViewComponent.Position = this.xui.GetMouseXUiPosition() + this.PositionOffset;
		}
		base.RefreshBindings();
		base.Update(_dt);
	}

	// Token: 0x060079C6 RID: 31174 RVA: 0x002E14A0 File Offset: 0x002DF6A0
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.entityPlayer != null)
		{
			ItemStack dragAndDropItem = this.entityPlayer.DragAndDropItem;
			if (dragAndDropItem != null && !dragAndDropItem.IsEmpty())
			{
				this.SetCurrentStack(this.entityPlayer.DragAndDropItem, true);
				this.PlaceItemBackInInventory();
			}
		}
	}

	// Token: 0x060079C7 RID: 31175 RVA: 0x002E14F2 File Offset: 0x002DF6F2
	public override void OnClose()
	{
		base.OnClose();
		this.PlaceItemBackInInventory();
	}

	// Token: 0x060079C8 RID: 31176 RVA: 0x002E1500 File Offset: 0x002DF700
	public void PlaceItemBackInInventory()
	{
		if (!this.IsEmpty())
		{
			if (this.xui.PlayerInventory.AddItem(this.CurrentStack))
			{
				Manager.PlayXUiSound(this.placeSound, 0.75f);
				this.SetCurrentStack(ItemStack.Empty, true);
				return;
			}
			this.xui.PlayerInventory.DropItem(this.CurrentStack);
			this.SetCurrentStack(ItemStack.Empty, true);
		}
	}

	// Token: 0x060079C9 RID: 31177 RVA: 0x002E156C File Offset: 0x002DF76C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "hasItemStack")
		{
			_value = (!this.IsEmpty()).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x060079CA RID: 31178 RVA: 0x002E15A4 File Offset: 0x002DF7A4
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "position_offset")
		{
			this.PositionOffset = StringParsers.ParseVector2i(_value, ',');
			return true;
		}
		if (!(_name == "place_sound"))
		{
			return base.ParseAttribute(_name, _value);
		}
		this.xui.LoadData<AudioClip>(_value, delegate(AudioClip _o)
		{
			this.placeSound = _o;
		});
		return true;
	}

	// Token: 0x04005A37 RID: 23095
	public XUiC_ItemStack ItemStackControl;

	// Token: 0x04005A38 RID: 23096
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x04005A39 RID: 23097
	public Vector2i PositionOffset = new Vector2i(0, 0);

	// Token: 0x04005A3A RID: 23098
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ItemStackGrid grid = ItemStackGrid.Create(new Vector2i(1, 1), XUiC_ItemStack.StackLocationTypes.None, false, false, null);

	// Token: 0x04005A3B RID: 23099
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack lastItemStack = new ItemStack(new ItemValue(0, false), 0);

	// Token: 0x04005A3C RID: 23100
	public bool InMenu;

	// Token: 0x04005A3D RID: 23101
	public XUiC_ItemStack.StackLocationTypes PickUpType;
}
