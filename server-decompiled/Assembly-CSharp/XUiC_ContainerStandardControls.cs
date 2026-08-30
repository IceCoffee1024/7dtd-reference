using System;
using UnityEngine.Scripting;

// Token: 0x02000F03 RID: 3843
[Preserve]
public class XUiC_ContainerStandardControls : XUiController
{
	// Token: 0x17000E1D RID: 3613
	// (get) Token: 0x06007883 RID: 30851 RVA: 0x002D7F77 File Offset: 0x002D6177
	// (set) Token: 0x06007884 RID: 30852 RVA: 0x002D7F8A File Offset: 0x002D618A
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

	// Token: 0x06007885 RID: 30853 RVA: 0x002D7FA0 File Offset: 0x002D61A0
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

	// Token: 0x06007886 RID: 30854 RVA: 0x002D80EB File Offset: 0x002D62EB
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

	// Token: 0x06007887 RID: 30855 RVA: 0x002D8109 File Offset: 0x002D6309
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

	// Token: 0x06007888 RID: 30856 RVA: 0x002D8134 File Offset: 0x002D6334
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

	// Token: 0x06007889 RID: 30857 RVA: 0x002D81A0 File Offset: 0x002D63A0
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

	// Token: 0x0600788A RID: 30858 RVA: 0x002D81EC File Offset: 0x002D63EC
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

	// Token: 0x0600788B RID: 30859 RVA: 0x002D8238 File Offset: 0x002D6438
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

	// Token: 0x0600788C RID: 30860 RVA: 0x002D8281 File Offset: 0x002D6481
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

	// Token: 0x0600788D RID: 30861 RVA: 0x002D82A5 File Offset: 0x002D64A5
	public void LockModeChanged(bool _state)
	{
		if (this.lockModeButtonColorTweener != null)
		{
			this.lockModeButtonColorTweener.enabled = _state;
		}
	}

	// Token: 0x0600788E RID: 30862 RVA: 0x002D82C1 File Offset: 0x002D64C1
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "move_start_bottom_left")
		{
			this.MoveStartBottomRight = StringParsers.ParseBool(_value, 0, -1, true);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x04005920 RID: 22816
	public Func<PackedBoolArray> GetLockedSlotsFromStorage;

	// Token: 0x04005921 RID: 22817
	public Action<PackedBoolArray> SetLockedSlotsToStorage;

	// Token: 0x04005922 RID: 22818
	public Action<PackedBoolArray> ApplyLockedSlotStates;

	// Token: 0x04005923 RID: 22819
	public Action<XUiC_ContainerStandardControls> UpdateLockedSlotStates;

	// Token: 0x04005924 RID: 22820
	public Action<PackedBoolArray> SortPressed;

	// Token: 0x04005925 RID: 22821
	public Action LockModeToggled;

	// Token: 0x04005926 RID: 22822
	public XUiC_ContainerStandardControls.MoveAllowedDelegate MoveAllowed;

	// Token: 0x04005927 RID: 22823
	public Action<bool, bool> MoveAllDone;

	// Token: 0x04005928 RID: 22824
	public bool MoveStartBottomRight;

	// Token: 0x04005929 RID: 22825
	[PublicizedFrom(EAccessModifier.Private)]
	public bool LockModeEnabled;

	// Token: 0x0400592A RID: 22826
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenColor lockModeButtonColorTweener;

	// Token: 0x02000F04 RID: 3844
	// (Invoke) Token: 0x06007897 RID: 30871
	public delegate bool MoveAllowedDelegate(out XUiController _parentWindow, out XUiC_ItemStackGrid _sourceGrid, out IInventory _destinationInventory);
}
