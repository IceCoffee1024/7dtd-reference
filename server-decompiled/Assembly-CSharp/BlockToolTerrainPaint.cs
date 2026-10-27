using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class BlockToolTerrainPaint : IBlockTool
{
	// Token: 0x06000DBE RID: 3518 RVA: 0x0005AEB5 File Offset: 0x000590B5
	public BlockToolTerrainPaint(BlockTools.Brush _brush, NguiWdwTerrainEditor _parentWindow)
	{
		this.brush = _brush;
		this.parentWindow = _parentWindow;
		this.isButtonDown = false;
		this.paintBlock = BlockValue.Air;
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x000027FC File Offset: 0x000009FC
	public void CheckSpecialKeys(Event ev, PlayerActionsLocal _playerAction)
	{
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0005AEE8 File Offset: 0x000590E8
	public void CheckKeys(ItemInventoryData _data, WorldRayHitInfo _hitInfo, PlayerActionsLocal _playerAction)
	{
		if (_data == null || _data.actionData == null || _data.actionData[0] == null)
		{
			return;
		}
		ItemStack item = GameManager.Instance.World.GetPrimaryPlayer().inventory.GetItem(7);
		this.paintBlock = item.itemValue.ToBlockValue(false);
		this.parentWindow.lastPosition = new Vector3i(_hitInfo.hit.pos);
		this.parentWindow.lastDirection = _hitInfo.ray.direction;
		if (!Input.GetMouseButton(0) || !this.isButtonDown)
		{
			if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && this.isButtonDown)
			{
				this.isButtonDown = false;
			}
			return;
		}
		if (Time.time - _data.actionData[0].lastUseTime < 0.1f)
		{
			return;
		}
		_data.actionData[0].lastUseTime = Time.time;
		if (!_hitInfo.bHitValid || !GameUtils.IsBlockOrTerrain(_hitInfo.tag))
		{
			return;
		}
		this.blockChanges.Clear();
		BlockTools.PaintTerrain(_data.world, _hitInfo.hit.blockPos, this.brush, this.blockChanges, this.paintBlock);
		_data.world.SetBlocksRPC(this.blockChanges);
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool ConsumeScrollWheel(ItemInventoryData _data, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x000321FF File Offset: 0x000303FF
	public string GetDebugOutput()
	{
		return "";
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x0005B032 File Offset: 0x00059232
	public virtual bool ExecuteAttackAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal _playerAction)
	{
		this.isButtonDown = true;
		this.lastData = _data;
		return true;
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x0005B032 File Offset: 0x00059232
	public bool ExecuteUseAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal _playerAction)
	{
		this.isButtonDown = true;
		this.lastData = _data;
		return true;
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x0005B044 File Offset: 0x00059244
	[PublicizedFrom(EAccessModifier.Private)]
	public sbyte[] GetLocalDensityMap(WorldBase _world, Vector3i blockTargetPos, Vector3i[] localArea)
	{
		sbyte[] array = new sbyte[localArea.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = _world.GetDensity(localArea[i].x, localArea[i].y, localArea[i].z);
		}
		return array;
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x0005AEAE File Offset: 0x000590AE
	public override string ToString()
	{
		return "Dig/Place Terrain";
	}

	// Token: 0x04000BE4 RID: 3044
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTools.Brush brush;

	// Token: 0x04000BE5 RID: 3045
	[PublicizedFrom(EAccessModifier.Private)]
	public NguiWdwTerrainEditor parentWindow;

	// Token: 0x04000BE6 RID: 3046
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData lastData;

	// Token: 0x04000BE7 RID: 3047
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isButtonDown;

	// Token: 0x04000BE8 RID: 3048
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();

	// Token: 0x04000BE9 RID: 3049
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue paintBlock;
}
