using System;
using UnityEngine.Scripting;

// Token: 0x02000EEC RID: 3820
[Preserve]
public class XUiC_ContainerStandardControls : XUiController
{
	// Token: 0x17000E03 RID: 3587
	// (get) Token: 0x060077CB RID: 30667 RVA: 0x002D3FEB File Offset: 0x002D21EB
	// (set) Token: 0x060077CC RID: 30668 RVA: 0x002D3FFE File Offset: 0x002D21FE
	public PackedBoolArray LockedSlots
	{
		get
		{
			Func<PackedBoolArray> getLockedSlotsFromStorage = this.GetLockedSlotsFromStorage;
			if (getLockedSlotsFromStorage == null)
			{
				return null;
			}
			return getLockedSlotsFromStorage();
		}
		set
		{
			Action<PackedBoolArray> setLockedSlotsToStorage = this.SetLockedSlotsToStorage;
			if (setLockedSlotsToStorage == null)
			{
				return;
			}
			setLockedSlotsToStorage(value);
		}
	}

	// Token: 0x060077CD RID: 30669 RVA: 0x002D4014 File Offset: 0x002D2214
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildById("btnSort");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.Sort();
			};
		}
		xuiController = base.GetChildById("btnMoveAll");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.MoveAll();
			};
		}
		xuiController = base.GetChildById("btnMoveFillAndSmart");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.MoveFillAndSmart();
			};
		}
		xuiController = base.GetChildById("btnMoveFillStacks");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.MoveFillStacks();
			};
		}
		xuiController = base.GetChildById("btnMoveSmart");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.MoveSmart();
			};
		}
		xuiController = base.GetChildById("btnToggleLockMode");
		if (xuiController != null)
		{
			xuiController.OnPress += delegate(XUiController _, int _)
			{
				this.ToggleLockMode();
			};
			XUiV_Button xuiV_Button = xuiController.ViewComponent as XUiV_Button;
			if (xuiV_Button != null)
			{
				this.lockModeButtonColorTweener = xuiV_Button.UiTransform.gameObject.GetOrAddComponent<TweenColor>();
				this.lockModeButtonColorTweener.from = xuiV_Button.DefaultSpriteColor;
				this.lockModeButtonColorTweener.to = xuiV_Button.SelectedSpriteColor;
				this.lockModeButtonColorTweener.style = UITweener.Style.PingPong;
				this.lockModeButtonColorTweener.enabled = false;
				this.lockModeButtonColorTweener.duration = 0.4f;
			}
		}
	}

	// Token: 0x060077CE RID: 30670 RVA: 0x002D415F File Offset: 0x002D235F
	public override void OnOpen()
	{
		base.OnOpen();
		Action<PackedBoolArray> applyLockedSlotStates = this.ApplyLockedSlotStates;
		if (applyLockedSlotStates == null)
		{
			return;
		}
		applyLockedSlotStates(this.LockedSlots);
	}

	// Token: 0x060077CF RID: 30671 RVA: 0x002D417D File Offset: 0x002D237D
	public void Sort()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		Action<PackedBoolArray> sortPressed = this.SortPressed;
		if (sortPressed == null)
		{
			return;
		}
		sortPressed(this.LockedSlots);
	}

	// Token: 0x060077D0 RID: 30672 RVA: 0x002D41A8 File Offset: 0x002D23A8
	public void MoveAll()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		XUiController xuiController;
		XUiC_ItemStackGrid xuiC_ItemStackGrid;
		IInventory inventory;
		if (this.MoveAllowed(out xuiController, out xuiC_ItemStackGrid, out inventory))
		{
			ValueTuple<bool, bool> valueTuple = XUiM_LootContainer.StashItems(xuiController, xuiC_ItemStackGrid, inventory, 0, this.LockedSlots, XUiM_LootContainer.EItemMoveKind.All, this.MoveStartBottomRight);
			bool item = valueTuple.Item1;
			bool item2 = valueTuple.Item2;
			Action<bool, bool> moveAllDone = this.MoveAllDone;
			if (moveAllDone == null)
			{
				return;
			}
			moveAllDone(item, item2);
		}
	}

	// Token: 0x060077D1 RID: 30673 RVA: 0x002D4214 File Offset: 0x002D2414
	public void MoveFillAndSmart()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		XUiController xuiController;
		XUiC_ItemStackGrid xuiC_ItemStackGrid;
		IInventory inventory;
		if (this.MoveAllowed(out xuiController, out xuiC_ItemStackGrid, out inventory))
		{
			XUiM_LootContainer.StashItems(xuiController, xuiC_ItemStackGrid, inventory, 0, this.LockedSlots, XUiM_LootContainer.EItemMoveKind.FillOnlyFirstCreateSecond, this.MoveStartBottomRight);
		}
	}

	// Token: 0x060077D2 RID: 30674 RVA: 0x002D4260 File Offset: 0x002D2460
	public void MoveFillStacks()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		XUiController xuiController;
		XUiC_ItemStackGrid xuiC_ItemStackGrid;
		IInventory inventory;
		if (this.MoveAllowed(out xuiController, out xuiC_ItemStackGrid, out inventory))
		{
			XUiM_LootContainer.StashItems(xuiController, xuiC_ItemStackGrid, inventory, 0, this.LockedSlots, XUiM_LootContainer.EItemMoveKind.FillOnly, this.MoveStartBottomRight);
		}
	}

	// Token: 0x060077D3 RID: 30675 RVA: 0x002D42AC File Offset: 0x002D24AC
	public void MoveSmart()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		XUiController xuiController;
		XUiC_ItemStackGrid xuiC_ItemStackGrid;
		IInventory inventory;
		if (this.MoveAllowed(out xuiController, out xuiC_ItemStackGrid, out inventory))
		{
			XUiM_LootContainer.StashItems(xuiController, xuiC_ItemStackGrid, inventory, 0, this.LockedSlots, XUiM_LootContainer.EItemMoveKind.FillAndCreate, this.MoveStartBottomRight);
		}
	}

	// Token: 0x060077D4 RID: 30676 RVA: 0x002D42F5 File Offset: 0x002D24F5
	public void ToggleLockMode()
	{
		Action<XUiC_ContainerStandardControls> updateLockedSlotStates = this.UpdateLockedSlotStates;
		if (updateLockedSlotStates != null)
		{
			updateLockedSlotStates(this);
		}
		Action lockModeToggled = this.LockModeToggled;
		if (lockModeToggled == null)
		{
			return;
		}
		lockModeToggled();
	}

	// Token: 0x060077D5 RID: 30677 RVA: 0x002D4319 File Offset: 0x002D2519
	public void LockModeChanged(bool _state)
	{
		if (this.lockModeButtonColorTweener != null)
		{
			this.lockModeButtonColorTweener.enabled = _state;
		}
	}

	// Token: 0x060077D6 RID: 30678 RVA: 0x002D4335 File Offset: 0x002D2535
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "move_start_bottom_left")
		{
			this.MoveStartBottomRight = StringParsers.ParseBool(_value, 0, -1, true);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x0400586E RID: 22638
	public Func<PackedBoolArray> GetLockedSlotsFromStorage;

	// Token: 0x0400586F RID: 22639
	public Action<PackedBoolArray> SetLockedSlotsToStorage;

	// Token: 0x04005870 RID: 22640
	public Action<PackedBoolArray> ApplyLockedSlotStates;

	// Token: 0x04005871 RID: 22641
	public Action<XUiC_ContainerStandardControls> UpdateLockedSlotStates;

	// Token: 0x04005872 RID: 22642
	public Action<PackedBoolArray> SortPressed;

	// Token: 0x04005873 RID: 22643
	public Action LockModeToggled;

	// Token: 0x04005874 RID: 22644
	public XUiC_ContainerStandardControls.MoveAllowedDelegate MoveAllowed;

	// Token: 0x04005875 RID: 22645
	public Action<bool, bool> MoveAllDone;

	// Token: 0x04005876 RID: 22646
	public bool MoveStartBottomRight;

	// Token: 0x04005877 RID: 22647
	[PublicizedFrom(EAccessModifier.Private)]
	public bool LockModeEnabled;

	// Token: 0x04005878 RID: 22648
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenColor lockModeButtonColorTweener;

	// Token: 0x02000EED RID: 3821
	// (Invoke) Token: 0x060077DF RID: 30687
	public delegate bool MoveAllowedDelegate(out XUiController _parentWindow, out XUiC_ItemStackGrid _sourceGrid, out IInventory _destinationInventory);
}
