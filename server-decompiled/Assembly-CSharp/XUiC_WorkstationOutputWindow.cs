using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010D5 RID: 4309
[Preserve]
public class XUiC_WorkstationOutputWindow : XUiController
{
	// Token: 0x0600893E RID: 35134 RVA: 0x00345354 File Offset: 0x00343554
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

	// Token: 0x0600893F RID: 35135 RVA: 0x003453CC File Offset: 0x003435CC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!this.xui.playerUI.windowManager.IsInputActive() && (this.xui.playerUI.playerInput.GUIActions.LeftStick.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Reload.WasPressed))
		{
			this.controls.MoveAll();
			this.windowGroup.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x0400660C RID: 26124
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationOutputGrid outputGrid;

	// Token: 0x0400660D RID: 26125
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls controls;
}
