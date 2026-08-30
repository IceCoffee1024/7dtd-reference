using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F27 RID: 3879
[Preserve]
public class XUiC_DragAndDropWindow : XUiController
{
	// Token: 0x17000E3A RID: 3642
	// (get) Token: 0x060079C0 RID: 31168 RVA: 0x002DDEF4 File Offset: 0x002DC0F4
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

	// Token: 0x060079C1 RID: 31169 RVA: 0x002DDF12 File Offset: 0x002DC112
	[PublicizedFrom(EAccessModifier.Internal)]
	public void DropCurrentItem()
	{
		this.xui.PlayerInventory.DropItem(this.CurrentStack);
		this.CurrentStack = ItemStack.Empty;
	}

	// Token: 0x060079C2 RID: 31170 RVA: 0x002DDF38 File Offset: 0x002DC138
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

	// Token: 0x17000E3B RID: 3643
	// (get) Token: 0x060079C3 RID: 31171 RVA: 0x002DDF9D File Offset: 0x002DC19D
	// (set) Token: 0x060079C4 RID: 31172 RVA: 0x002DDFA8 File Offset: 0x002DC1A8
	public ItemStack CurrentStack
	{
		get
		{
			return this.itemStack;
		}
		set
		{
			this.lastItemStack = this.itemStack;
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

	// Token: 0x17000E3C RID: 3644
	// (get) Token: 0x060079C5 RID: 31173 RVA: 0x002DE00B File Offset: 0x002DC20B
	public ItemStack LastStack
	{
		get
		{
			return this.lastItemStack;
		}
	}

	// Token: 0x060079C6 RID: 31174 RVA: 0x002DE013 File Offset: 0x002DC213
	public override void Init()
	{
		base.Init();
		this.ItemStackControl = base.GetChildByType<XUiC_ItemStack>();
		this.ItemStackControl.IsDragAndDrop = true;
		this.ItemStackControl.ItemStack = ItemStack.Empty;
		this.xui.DragAndDropWindow = this;
	}

	// Token: 0x060079C7 RID: 31175 RVA: 0x002DE04F File Offset: 0x002DC24F
	public override void Cleanup()
	{
		base.Cleanup();
		this.xui.DragAndDropWindow = null;
	}

	// Token: 0x060079C8 RID: 31176 RVA: 0x002DE064 File Offset: 0x002DC264
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

	// Token: 0x060079C9 RID: 31177 RVA: 0x002DE0C4 File Offset: 0x002DC2C4
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

	// Token: 0x060079CA RID: 31178 RVA: 0x002DE115 File Offset: 0x002DC315
	public override void OnClose()
	{
		base.OnClose();
		this.PlaceItemBackInInventory();
	}

	// Token: 0x060079CB RID: 31179 RVA: 0x002DE124 File Offset: 0x002DC324
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

	// Token: 0x060079CC RID: 31180 RVA: 0x002DE194 File Offset: 0x002DC394
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

	// Token: 0x060079CD RID: 31181 RVA: 0x002DE1DC File Offset: 0x002DC3DC
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

	// Token: 0x040059FF RID: 23039
	public XUiC_ItemStack ItemStackControl;

	// Token: 0x04005A00 RID: 23040
	[PublicizedFrom(EAccessModifier.Private)]
	public AudioClip placeSound;

	// Token: 0x04005A01 RID: 23041
	public Vector2i PositionOffset = new Vector2i(0, 0);

	// Token: 0x04005A02 RID: 23042
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack = new ItemStack(new ItemValue(0, false), 0);

	// Token: 0x04005A03 RID: 23043
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack lastItemStack = new ItemStack(new ItemValue(0, false), 0);

	// Token: 0x04005A04 RID: 23044
	public bool InMenu;

	// Token: 0x04005A05 RID: 23045
	public XUiC_ItemStack.StackLocationTypes PickUpType;
}
