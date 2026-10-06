using System;
using UnityEngine.Scripting;

// Token: 0x02000F12 RID: 3858
[Preserve]
public class XUiC_ContainerStandardControls : XUiController
{
	// Token: 0x17000E3A RID: 3642
	// (get) Token: 0x0600787E RID: 30846 RVA: 0x002DB30B File Offset: 0x002D950B
	// (set) Token: 0x0600787F RID: 30847 RVA: 0x002DB31E File Offset: 0x002D951E
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

	// Token: 0x06007880 RID: 30848 RVA: 0x002DB334 File Offset: 0x002D9534
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

	// Token: 0x06007881 RID: 30849 RVA: 0x002DB47F File Offset: 0x002D967F
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

	// Token: 0x06007882 RID: 30850 RVA: 0x002DB49D File Offset: 0x002D969D
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

	// Token: 0x06007883 RID: 30851 RVA: 0x002DB4C8 File Offset: 0x002D96C8
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

	// Token: 0x06007884 RID: 30852 RVA: 0x002DB534 File Offset: 0x002D9734
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

	// Token: 0x06007885 RID: 30853 RVA: 0x002DB580 File Offset: 0x002D9780
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

	// Token: 0x06007886 RID: 30854 RVA: 0x002DB5CC File Offset: 0x002D97CC
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

	// Token: 0x06007887 RID: 30855 RVA: 0x002DB615 File Offset: 0x002D9815
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

	// Token: 0x06007888 RID: 30856 RVA: 0x002DB639 File Offset: 0x002D9839
	public void LockModeChanged(bool _state)
	{
		if (this.lockModeButtonColorTweener != null)
		{
			this.lockModeButtonColorTweener.enabled = _state;
		}
	}

	// Token: 0x06007889 RID: 30857 RVA: 0x002DB655 File Offset: 0x002D9855
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "move_start_bottom_left")
		{
			this.MoveStartBottomRight = StringParsers.ParseBool(_value, 0, -1, true);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x04005958 RID: 22872
	public Func<PackedBoolArray> GetLockedSlotsFromStorage;

	// Token: 0x04005959 RID: 22873
	public Action<PackedBoolArray> SetLockedSlotsToStorage;

	// Token: 0x0400595A RID: 22874
	public Action<PackedBoolArray> ApplyLockedSlotStates;

	// Token: 0x0400595B RID: 22875
	public Action<XUiC_ContainerStandardControls> UpdateLockedSlotStates;

	// Token: 0x0400595C RID: 22876
	public Action<PackedBoolArray> SortPressed;

	// Token: 0x0400595D RID: 22877
	public Action LockModeToggled;

	// Token: 0x0400595E RID: 22878
	public XUiC_ContainerStandardControls.MoveAllowedDelegate MoveAllowed;

	// Token: 0x0400595F RID: 22879
	public Action<bool, bool> MoveAllDone;

	// Token: 0x04005960 RID: 22880
	public bool MoveStartBottomRight;

	// Token: 0x04005961 RID: 22881
	[PublicizedFrom(EAccessModifier.Private)]
	public bool LockModeEnabled;

	// Token: 0x04005962 RID: 22882
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenColor lockModeButtonColorTweener;

	// Token: 0x02000F13 RID: 3859
	// (Invoke) Token: 0x06007892 RID: 30866
	public delegate bool MoveAllowedDelegate(out XUiController _parentWindow, out XUiC_ItemStackGrid _sourceGrid, out IInventory _destinationInventory);
}
