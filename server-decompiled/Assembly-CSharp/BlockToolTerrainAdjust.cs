using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class BlockToolTerrainAdjust : IBlockTool
{
	// Token: 0x06000DB5 RID: 3509 RVA: 0x0005AC20 File Offset: 0x00058E20
	public BlockToolTerrainAdjust(BlockTools.Brush _brush, NguiWdwTerrainEditor _parentWindow)
	{
		this.brush = _brush;
		this.parentWindow = _parentWindow;
		this.isButtonDown = false;
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x000027FC File Offset: 0x000009FC
	public void CheckSpecialKeys(Event ev, PlayerActionsLocal playerActions)
	{
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x0005AC48 File Offset: 0x00058E48
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

	// Token: 0x06000DB8 RID: 3512 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool ConsumeScrollWheel(ItemInventoryData _data, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x000321FF File Offset: 0x000303FF
	public string GetDebugOutput()
	{
		return "";
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0005AE4B File Offset: 0x0005904B
	public virtual bool ExecuteAttackAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions)
	{
		this.isButtonDown = true;
		this.lastData = _data;
		return true;
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x0005AE4B File Offset: 0x0005904B
	public bool ExecuteUseAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions)
	{
		this.isButtonDown = true;
		this.lastData = _data;
		return true;
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0005AE5C File Offset: 0x0005905C
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

	// Token: 0x06000DBD RID: 3517 RVA: 0x0005AEAE File Offset: 0x000590AE
	public override string ToString()
	{
		return "Dig/Place Terrain";
	}

	// Token: 0x04000BDF RID: 3039
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTools.Brush brush;

	// Token: 0x04000BE0 RID: 3040
	[PublicizedFrom(EAccessModifier.Private)]
	public NguiWdwTerrainEditor parentWindow;

	// Token: 0x04000BE1 RID: 3041
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData lastData;

	// Token: 0x04000BE2 RID: 3042
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isButtonDown;

	// Token: 0x04000BE3 RID: 3043
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> blockChanges = new List<BlockChangeInfo>();
}
