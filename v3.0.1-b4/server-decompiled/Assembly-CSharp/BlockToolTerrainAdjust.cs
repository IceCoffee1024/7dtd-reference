using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class BlockToolTerrainAdjust : IBlockTool
{
	// Token: 0x06000DB3 RID: 3507 RVA: 0x0005A800 File Offset: 0x00058A00
	public BlockToolTerrainAdjust(BlockTools.Brush _brush, NguiWdwTerrainEditor _parentWindow)
	{
		this.brush = _brush;
		this.parentWindow = _parentWindow;
		this.isButtonDown = false;
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x000027FC File Offset: 0x000009FC
	public void CheckSpecialKeys(Event ev, PlayerActionsLocal playerActions)
	{
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x0005A828 File Offset: 0x00058A28
	public void CheckKeys(ItemInventoryData _data, WorldRayHitInfo _hitInfo, PlayerActionsLocal playerActions)
	{
		if (_data == null || _data.actionData == null || _data.actionData[0] == null)
		{
			return;
		}
		this.parentWindow.lastPosition = new Vector3i(_hitInfo.hit.pos);
		this.parentWindow.lastDirection = _hitInfo.ray.direction;
		if (Input.GetMouseButton(0) && this.isButtonDown)
		{
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
			BlockTools.PlaceTerrain(_data.world, _hitInfo.hit.blockPos + new Vector3i(this.parentWindow.lastDirection.x * 2f, this.parentWindow.lastDirection.y * 2f, this.parentWindow.lastDirection.z * 2f), this.brush, this.blockChanges);
			_data.world.SetBlocksRPC(this.blockChanges);
			return;
		}
		else
		{
			if (!Input.GetMouseButton(1) || !this.isButtonDown)
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
			BlockTools.RemoveTerrain(_data.world, _hitInfo.hit.blockPos, this.brush, this.blockChanges);
			_data.world.SetBlocksRPC(this.blockChanges);
			return;
		}
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool ConsumeScrollWheel(ItemInventoryData _data, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x00031E8B File Offset: 0x0003008B
	public string GetDebugOutput()
	{
		return "";
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x0005AA2B File Offset: 0x00058C2B
	public virtual bool ExecuteAttackAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions)
	{
		this.isButtonDown = true;
		this.lastData = _data;
		return true;
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0005AA2B File Offset: 0x00058C2B
	public bool ExecuteUseAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions)
	{
		this.isButtonDown = true;
		this.lastData = _data;
		return true;
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0005AA3C File Offset: 0x00058C3C
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

	// Token: 0x06000DBB RID: 3515 RVA: 0x0005AA8E File Offset: 0x00058C8E
	public override string ToString()
	{
		return "Dig/Place Terrain";
	}

	// Token: 0x04000BD9 RID: 3033
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTools.Brush brush;

	// Token: 0x04000BDA RID: 3034
	[PublicizedFrom(EAccessModifier.Private)]
	public NguiWdwTerrainEditor parentWindow;

	// Token: 0x04000BDB RID: 3035
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData lastData;

	// Token: 0x04000BDC RID: 3036
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isButtonDown;

	// Token: 0x04000BDD RID: 3037
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();
}
