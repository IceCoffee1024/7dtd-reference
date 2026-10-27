using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F45 RID: 3909
[Preserve]
public class XUiC_DragAndDropWindow : XUiController
{
	// Token: 0x17000E61 RID: 3681
	// (get) Token: 0x060079FE RID: 31230 RVA: 0x002E2B80 File Offset: 0x002E0D80
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

	// Token: 0x060079FF RID: 31231 RVA: 0x002E2B9E File Offset: 0x002E0D9E
	[PublicizedFrom(EAccessModifier.Internal)]
	public void DropCurrentItem()
	{
		this.xui.PlayerInventory.DropItem(this.CurrentStack);
		this.SetCurrentStack(ItemStack.Empty, true);
	}

	// Token: 0x06007A00 RID: 31232 RVA: 0x002E2BC4 File Offset: 0x002E0DC4
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

	// Token: 0x17000E62 RID: 3682
	// (get) Token: 0x06007A01 RID: 31233 RVA: 0x002E2C14 File Offset: 0x002E0E14
	public ItemStack CurrentStack
	{
		get
		{
			return this.grid[0].Clone();
		}
	}

	// Token: 0x06007A02 RID: 31234 RVA: 0x002E2C27 File Offset: 0x002E0E27
	public bool IsEmpty()
	{
		return this.grid[0].IsEmpty();
	}

	// Token: 0x06007A03 RID: 31235 RVA: 0x002E2C3A File Offset: 0x002E0E3A
	public void SetCurrentStack(ItemStack stack, bool playPickupSound = true)
	{
		this.lastItemStack = this.CurrentStack;
		this.grid.SetItem(0, stack);
		if (!this.IsEmpty() && playPickupSound)
		{
			this.ItemStackControl.PlayPickupSound(null);
		}
	}

	// Token: 0x06007A04 RID: 31236 RVA: 0x002E2C6E File Offset: 0x002E0E6E
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

	// Token: 0x17000E63 RID: 3683
	// (get) Token: 0x06007A05 RID: 31237 RVA: 0x002E2CA6 File Offset: 0x002E0EA6
	public ItemStack LastStack
	{
		get
		{
			return this.lastItemStack;
		}
	}

	// Token: 0x06007A06 RID: 31238 RVA: 0x002E2CB0 File Offset: 0x002E0EB0
	public override void Init()
	{
		base.Init();
		this.ItemStackControl = base.GetChildByType<XUiC_ItemStack>();
		this.ItemStackControl.IsDragAndDrop = true;
		this.ItemStackControl.ItemStack = ItemStack.Empty;
		this.grid.SlotChanged += this.onStackChanged;
		this.xui.DragAndDropWindow = this;
	}

	// Token: 0x06007A07 RID: 31239 RVA: 0x002E2D0E File Offset: 0x002E0F0E
	public override void Cleanup()
	{
		base.Cleanup();
		this.grid.SlotChanged -= this.onStackChanged;
		this.xui.DragAndDropWindow = null;
	}

	// Token: 0x06007A08 RID: 31240 RVA: 0x002E2D3C File Offset: 0x002E0F3C
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

	// Token: 0x06007A09 RID: 31241 RVA: 0x002E2D90 File Offset: 0x002E0F90
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

	// Token: 0x06007A0A RID: 31242 RVA: 0x002E2DE2 File Offset: 0x002E0FE2
	public override void OnClose()
	{
		base.OnClose();
		this.PlaceItemBackInInventory();
	}

	// Token: 0x06007A0B RID: 31243 RVA: 0x002E2DF0 File Offset: 0x002E0FF0
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

	// Token: 0x06007A0C RID: 31244 RVA: 0x002E2E5C File Offset: 0x002E105C
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

	// Token: 0x06007A0D RID: 31245 RVA: 0x002E2E94 File Offset: 0x002E1094
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

	// Token: 0x04005A6A RID: 23146
	public XUiC_ItemStack ItemStackControl;

	// Token: 0x04005A6B RID: 23147
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x04005A6C RID: 23148
	public Vector2i PositionOffset = new Vector2i(0, 0);

	// Token: 0x04005A6D RID: 23149
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly ItemStackGrid grid = ItemStackGrid.Create(new Vector2i(1, 1), XUiC_ItemStack.StackLocationTypes.None, false, false, null);

	// Token: 0x04005A6E RID: 23150
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack lastItemStack = new ItemStack(new ItemValue(0, false), 0);

	// Token: 0x04005A6F RID: 23151
	public bool InMenu;

	// Token: 0x04005A70 RID: 23152
	public XUiC_ItemStack.StackLocationTypes PickUpType;
}
