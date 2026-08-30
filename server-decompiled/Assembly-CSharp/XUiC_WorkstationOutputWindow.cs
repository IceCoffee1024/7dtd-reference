using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010BC RID: 4284
[Preserve]
public class XUiC_WorkstationOutputWindow : XUiController
{
	// Token: 0x0600884C RID: 34892 RVA: 0x00340B30 File Offset: 0x0033ED30
	public override void Init()
	{
		base.Init();
		this.outputGrid = base.GetChildByType<XUiC_WorkstationOutputGrid>();
		this.controls = base.GetChildByType<XUiC_ContainerStandardControls>();
		if (this.controls != null)
		{
			this.controls.SortPressed = delegate(PackedBoolArray _ignoredSlots)
			{
				ItemStack[] array = StackSortUtil.CombineAndSortStacks(this.outputGrid.GetSlots(), 0, _ignoredSlots);
				this.outputGrid.SetSlots(array);
			};
			this.controls.MoveAllowed = delegate(out XUiController _parentWindow, out XUiC_ItemStackGrid _grid, out IInventory _inventory)
			{
				_parentWindow = this;
				_grid = this.outputGrid;
				_inventory = this.xui.PlayerInventory;
				return true;
			};
			this.controls.MoveAllDone = delegate(bool _allMoved, bool _anyMoved)
			{
				if (_anyMoved)
				{
					Manager.BroadcastPlayByLocalPlayer(this.xui.playerUI.entityPlayer.position + Vector3.one * 0.5f, "UseActions/takeall1");
				}
			};
		}
	}

	// Token: 0x0600884D RID: 34893 RVA: 0x00340BA8 File Offset: 0x0033EDA8
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!this.xui.playerUI.windowManager.IsInputActive() && (this.xui.playerUI.playerInput.GUIActions.LeftStick.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Reload.WasPressed))
		{
			this.controls.MoveAll();
			this.windowGroup.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x04006545 RID: 25925
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationOutputGrid outputGrid;

	// Token: 0x04006546 RID: 25926
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls controls;
}
