using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F10 RID: 3856
[Preserve]
public class XUiC_DragAndDropWindow : XUiController
{
	// Token: 0x17000E20 RID: 3616
	// (get) Token: 0x06007908 RID: 30984 RVA: 0x002D9F68 File Offset: 0x002D8168
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

	// Token: 0x06007909 RID: 30985 RVA: 0x002D9F86 File Offset: 0x002D8186
	[PublicizedFrom(EAccessModifier.Internal)]
	public void DropCurrentItem()
	{
		this.xui.PlayerInventory.DropItem(this.CurrentStack);
		this.CurrentStack = ItemStack.Empty;
	}

	// Token: 0x0600790A RID: 30986 RVA: 0x002D9FAC File Offset: 0x002D81AC
	[PublicizedFrom(EAccessModifier.Internal)]
	public void DropCurrentItem(int _count)
	{
		if (_count < this.CurrentStack.count)
		{
			ItemStack itemStack = this.CurrentStack.Clone();
			itemStack.count = _count;
			this.CurrentStack.count -= _count;
			this.xui.PlayerInventory.DropItem(itemStack);
			this.CurrentStack = this.itemStack;
			return;
		}
		this.DropCurrentItem();
	}

	// Token: 0x17000E21 RID: 3617
	// (get) Token: 0x0600790B RID: 30987 RVA: 0x002DA011 File Offset: 0x002D8211
	// (set) Token: 0x0600790C RID: 30988 RVA: 0x002DA01C File Offset: 0x002D821C
	public ItemStack CurrentStack
	{
		get
		{
			return this.itemStack;
		}
		set
		{
			this.itemStack = value;
			this.ItemStackControl.ItemStack = value;
			if (value != null && !value.IsEmpty())
			{
				this.ItemStackControl.PlayPickupSound(null);
			}
			if (this.entityPlayer != null)
			{
				this.entityPlayer.DragAndDropItem = value;
				base.RefreshBindings();
			}
		}
	}

	// Token: 0x0600790D RID: 30989 RVA: 0x002DA073 File Offset: 0x002D8273
	public override void Init()
	{
		base.Init();
		this.ItemStackControl = base.GetChildByType<XUiC_ItemStack>();
		this.ItemStackControl.IsDragAndDrop = true;
		this.ItemStackControl.ItemStack = ItemStack.Empty;
		this.xui.DragAndDropWindow = this;
	}

	// Token: 0x0600790E RID: 30990 RVA: 0x002DA0AF File Offset: 0x002D82AF
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.DragAndDropWindow = null;
	}

	// Token: 0x0600790F RID: 30991 RVA: 0x002DA0C4 File Offset: 0x002D82C4
	public override void Update(float _dt)
	{
		if (!this.InMenu)
		{
			this.PlaceItemBackInInventory();
		}
		if (this.itemStack != null && !this.itemStack.IsEmpty())
		{
			base.ViewComponent.Position = this.xui.GetMouseXUiPosition() + this.PositionOffset;
		}
		base.RefreshBindings();
		base.Update(_dt);
	}

	// Token: 0x06007910 RID: 30992 RVA: 0x002DA124 File Offset: 0x002D8324
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.entityPlayer != null)
		{
			ItemStack dragAndDropItem = this.entityPlayer.DragAndDropItem;
			if (dragAndDropItem != null && !dragAndDropItem.IsEmpty())
			{
				this.CurrentStack = this.entityPlayer.DragAndDropItem;
				this.PlaceItemBackInInventory();
			}
		}
	}

	// Token: 0x06007911 RID: 30993 RVA: 0x002DA175 File Offset: 0x002D8375
	public override void OnClose()
	{
		base.OnClose();
		this.PlaceItemBackInInventory();
	}

	// Token: 0x06007912 RID: 30994 RVA: 0x002DA184 File Offset: 0x002D8384
	public void PlaceItemBackInInventory()
	{
		if (!this.CurrentStack.IsEmpty())
		{
			if (this.xui.PlayerInventory.AddItem(this.itemStack))
			{
				Manager.PlayXUiSound(this.placeSound, 0.75f);
				this.CurrentStack = ItemStack.Empty;
				return;
			}
			this.xui.PlayerInventory.DropItem(this.itemStack);
			this.CurrentStack = ItemStack.Empty;
		}
	}

	// Token: 0x06007913 RID: 30995 RVA: 0x002DA1F4 File Offset: 0x002D83F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "hasItemStack")
		{
			_value = (this.itemStack != null && !this.itemStack.IsEmpty()).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06007914 RID: 30996 RVA: 0x002DA23C File Offset: 0x002D843C
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

	// Token: 0x0400594D RID: 22861
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack ItemStackControl;

	// Token: 0x0400594E RID: 22862
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x0400594F RID: 22863
	public Vector2i PositionOffset = new Vector2i(0, 0);

	// Token: 0x04005950 RID: 22864
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack = new ItemStack(new ItemValue(0, false), 0);

	// Token: 0x04005951 RID: 22865
	public bool InMenu;

	// Token: 0x04005952 RID: 22866
	public XUiC_ItemStack.StackLocationTypes PickUpType;
}
