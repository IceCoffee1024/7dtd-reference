using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class BlockToolTerrainPaint : IBlockTool
{
	// Token: 0x06000DBC RID: 3516 RVA: 0x0005AA95 File Offset: 0x00058C95
	public BlockToolTerrainPaint(BlockTools.Brush _brush, NguiWdwTerrainEditor _parentWindow)
	{
		this.brush = _brush;
		this.parentWindow = _parentWindow;
		this.isButtonDown = false;
		this.paintBlock = BlockValue.Air;
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x000027FC File Offset: 0x000009FC
	public void CheckSpecialKeys(Event ev, PlayerActionsLocal _playerAction)
	{
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x0005AAC8 File Offset: 0x00058CC8
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

	// Token: 0x06000DBF RID: 3519 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool ConsumeScrollWheel(ItemInventoryData _data, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x00031E8B File Offset: 0x0003008B
	public string GetDebugOutput()
	{
		return "";
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x0005AC12 File Offset: 0x00058E12
	public virtual bool ExecuteAttackAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal _playerAction)
	{
		this.isButtonDown = true;
		this.lastData = _data;
		return true;
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x0005AC12 File Offset: 0x00058E12
	public bool ExecuteUseAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal _playerAction)
	{
		this.isButtonDown = true;
		this.lastData = _data;
		return true;
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x0005AC24 File Offset: 0x00058E24
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

	// Token: 0x06000DC4 RID: 3524 RVA: 0x0005AA8E File Offset: 0x00058C8E
	public override string ToString()
	{
		return "Dig/Place Terrain";
	}

	// Token: 0x04000BDE RID: 3038
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTools.Brush brush;

	// Token: 0x04000BDF RID: 3039
	[PublicizedFrom(EAccessModifier.Private)]
	public NguiWdwTerrainEditor parentWindow;

	// Token: 0x04000BE0 RID: 3040
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData lastData;

	// Token: 0x04000BE1 RID: 3041
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isButtonDown;

	// Token: 0x04000BE2 RID: 3042
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();

	// Token: 0x04000BE3 RID: 3043
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue paintBlock;
}
