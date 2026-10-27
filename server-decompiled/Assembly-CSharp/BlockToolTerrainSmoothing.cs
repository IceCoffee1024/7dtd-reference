using System;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class BlockToolTerrainSmoothing : IBlockTool
{
	// Token: 0x06000DC7 RID: 3527 RVA: 0x0005B096 File Offset: 0x00059296
	public BlockToolTerrainSmoothing(BlockTools.Brush _brush, NguiWdwTerrainEditor _parentWindow)
	{
		this.brush = _brush;
		this.parentWindow = _parentWindow;
		this.isButtonDown = false;
		this.SetUp22DegreeRules();
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x000027FC File Offset: 0x000009FC
	public void CheckSpecialKeys(Event ev, PlayerActionsLocal playerActions)
	{
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0005B0BC File Offset: 0x000592BC
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
			this.SmoothTerrain(_hitInfo);
			return;
		}
		if (!Input.GetMouseButton(0) && this.isButtonDown)
		{
			this.isButtonDown = false;
		}
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool ConsumeScrollWheel(ItemInventoryData _data, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x000321FF File Offset: 0x000303FF
	public string GetDebugOutput()
	{
		return "";
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x0005B143 File Offset: 0x00059343
	public bool ExecuteAttackAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions)
	{
		this.isButtonDown = true;
		this.lastData = _data;
		return true;
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x0005B154 File Offset: 0x00059354
	[PublicizedFrom(EAccessModifier.Private)]
	public void SmoothTerrain(WorldRayHitInfo _hitInfo)
	{
		ItemInventoryData itemInventoryData = this.lastData;
		if (Time.time - itemInventoryData.actionData[0].lastUseTime > Constants.cBuildIntervall && _hitInfo.bHitValid && GameUtils.IsBlockOrTerrain(_hitInfo.tag))
		{
			Vector3i blockPos = _hitInfo.hit.blockPos;
			BlockValue block = itemInventoryData.world.GetBlock(blockPos);
			Vector3i[] cubesInBrush = this.brush.GetCubesInBrush();
			for (int i = 0; i < cubesInBrush.Length; i++)
			{
				Vector3i vector3i = blockPos + cubesInBrush[i];
				if (this.HasValidNeighbor(vector3i, 0, itemInventoryData))
				{
					BlockValue block2 = itemInventoryData.world.GetBlock(vector3i);
					bool flag = block2.Equals(BlockValue.Air);
					sbyte averageDensity = this.GetAverageDensity(vector3i, itemInventoryData);
					if (averageDensity <= -1 && flag)
					{
						itemInventoryData.world.SetBlockRPC(vector3i, block, averageDensity);
					}
					else if (averageDensity >= 0 && !flag)
					{
						itemInventoryData.world.SetBlockRPC(vector3i, BlockValue.Air, averageDensity);
					}
					else
					{
						itemInventoryData.world.SetBlockRPC(vector3i, block2, averageDensity);
					}
				}
			}
		}
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x0005B288 File Offset: 0x00059488
	[PublicizedFrom(EAccessModifier.Private)]
	public void SnapTerrain22(WorldRayHitInfo _hitInfo)
	{
		ItemInventoryData itemInventoryData = this.lastData;
		if (Time.time - itemInventoryData.actionData[0].lastUseTime > Constants.cBuildIntervall && _hitInfo.bHitValid && GameUtils.IsBlockOrTerrain(_hitInfo.tag))
		{
			Vector3i blockPos = _hitInfo.hit.blockPos;
			BlockValue block = itemInventoryData.world.GetBlock(blockPos);
			Vector3i[] cubesInBrush = this.brush.GetCubesInBrush();
			for (int i = 0; i < cubesInBrush.Length; i++)
			{
				Vector3i vector3i = blockPos + cubesInBrush[i];
				if (this.HasValidNeighbor(vector3i, 0, itemInventoryData))
				{
					bool flag = itemInventoryData.world.GetBlock(cubesInBrush[i]).Equals(BlockValue.Air);
					sbyte outputBasedOnRuleset = this.GetOutputBasedOnRuleset22(vector3i, itemInventoryData);
					if (outputBasedOnRuleset <= -1 && flag)
					{
						itemInventoryData.world.SetBlockRPC(vector3i, block, outputBasedOnRuleset);
					}
					else if (outputBasedOnRuleset >= 0 && !flag)
					{
						itemInventoryData.world.SetBlockRPC(vector3i, BlockValue.Air, outputBasedOnRuleset);
					}
					else
					{
						itemInventoryData.world.SetBlockRPC(vector3i, outputBasedOnRuleset);
					}
				}
			}
		}
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x0005B3C0 File Offset: 0x000595C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetUp22DegreeRules()
	{
		this.blockMaskRuleset = new BlockMaskRules<sbyte?, char>('*');
		this.blockMaskRuleset.AddRule(new BlockRule<sbyte?, char>(new sbyte?((sbyte)(-1)), new char[]
		{
			'*', 'A', '*', '*', 'A', '*', '*', 'A', '*', '*',
			'B', '*', '*', '*', '*', '*', 'A', '*', '*', 'B',
			'*', '*', 'B', '*', '*', 'B', '*'
		}));
		this.blockMaskRuleset.AddRule(new BlockRule<sbyte?, char>(new sbyte?((sbyte)(-1)), new char[]
		{
			'*', 'A', '*', '*', 'A', '*', '*', 'A', '*', '*',
			'B', '*', '*', '*', '*', '*', 'A', '*', '*', 'B',
			'*', '*', 'B', '*', '*', 'B', '*'
		}));
		this.blockMaskRuleset.AddRule(new BlockRule<sbyte?, char>(new sbyte?((sbyte)(-1)), new char[]
		{
			'*', '*', '*', '*', '*', '*', '*', '*', '*', '*',
			'*', '*', 'H', 'A', 'H', '*', '*', '*', '*', '*',
			'*', '*', '*', '*', '*', '*', '*'
		}));
		this.blockMaskRuleset.AddRule(new BlockRule<sbyte?, char>(new sbyte?((sbyte)(-128)), new char[]
		{
			'*', '*', '*', '*', '*', '*', '*', '*', '*', '*',
			'*', '*', 'B', 'A', 'B', '*', '*', '*', '*', '*',
			'*', '*', '*', '*', '*', '*', '*'
		}));
		this.blockMaskRuleset.AddRule(new BlockRule<sbyte?, char>(new sbyte?((sbyte)(-128)), new char[]
		{
			'*', '*', '*', '*', '*', '*', '*', '*', '*', '*',
			'*', '*', 'B', 'H', 'B', '*', 'H', '*', '*', '*',
			'*', '*', '*', '*', '*', '*', '*'
		}));
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x0005B4AC File Offset: 0x000596AC
	[PublicizedFrom(EAccessModifier.Private)]
	public sbyte GetOutputBasedOnRuleset22(Vector3i _pos, ItemInventoryData _data)
	{
		char[] array = new char[27];
		int num = 0;
		for (int i = 1; i >= -1; i--)
		{
			for (int j = 1; j >= -1; j--)
			{
				for (int k = 1; k >= -1; k--)
				{
					int density = (int)_data.world.GetDensity(_pos.x + k, _pos.y + i, _pos.z + j);
					if (density >= 0)
					{
						array[num] = 'A';
					}
					else if (density > -32)
					{
						array[num] = 'H';
					}
					else
					{
						array[num] = 'B';
					}
					num++;
				}
			}
		}
		sbyte? output = this.blockMaskRuleset.GetOutput(array);
		if (output != null)
		{
			return output.Value;
		}
		sbyte density2 = _data.world.GetDensity(_pos.x, _pos.y, _pos.z);
		if (density2 >= 0)
		{
			return sbyte.MaxValue;
		}
		if (density2 > -32)
		{
			return -1;
		}
		return sbyte.MinValue;
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x0005B588 File Offset: 0x00059788
	[PublicizedFrom(EAccessModifier.Private)]
	public void SnapTerrain45(WorldRayHitInfo _hitInfo)
	{
		ItemInventoryData itemInventoryData = this.lastData;
		if (Time.time - itemInventoryData.actionData[0].lastUseTime > Constants.cBuildIntervall && _hitInfo.bHitValid && GameUtils.IsBlockOrTerrain(_hitInfo.tag))
		{
			Vector3i blockPos = _hitInfo.hit.blockPos;
			BlockValue block = itemInventoryData.world.GetBlock(blockPos);
			Vector3i[] cubesInBrush = this.brush.GetCubesInBrush();
			for (int i = 0; i < cubesInBrush.Length; i++)
			{
				Vector3i vector3i = blockPos + cubesInBrush[i];
				if (this.HasValidNeighbor(vector3i, 0, itemInventoryData))
				{
					bool flag = itemInventoryData.world.GetBlock(vector3i).Equals(BlockValue.Air);
					sbyte b = this.GetAverageDensity(vector3i, itemInventoryData);
					if (this.AirNeighborCount(vector3i, itemInventoryData) > 3 && !flag)
					{
						b = sbyte.MinValue;
					}
					if (b <= -1 && flag)
					{
						itemInventoryData.world.SetBlockRPC(vector3i, block, b);
					}
					else if (b >= 0 && !flag)
					{
						itemInventoryData.world.SetBlockRPC(vector3i, BlockValue.Air, b);
					}
					else
					{
						itemInventoryData.world.SetBlockRPC(vector3i, b);
					}
				}
			}
		}
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x00010E62 File Offset: 0x0000F062
	public bool ExecuteUseAction(ItemInventoryData _data, bool _bReleased, PlayerActionsLocal playerActions)
	{
		return false;
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x0005B6CC File Offset: 0x000598CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public int AirNeighborCount(Vector3i _pos, ItemInventoryData _data)
	{
		int num = 0;
		for (int i = _pos.x - 1; i <= _pos.x + 1; i++)
		{
			for (int j = _pos.z - 1; j <= _pos.z + 1; j++)
			{
				if ((i != 0 || j != 0) && _data.world.GetBlock(i, _pos.y, j).Equals(BlockValue.Air))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x0005B73C File Offset: 0x0005993C
	[PublicizedFrom(EAccessModifier.Protected)]
	public sbyte GetAverageLockedDensity(Vector3i _pos, ItemInventoryData _data)
	{
		Vector3i[] cubesInBrush = this.brush.GetCubesInBrush();
		int num = (int)_data.world.GetDensity(_pos.x, _pos.y, _pos.z);
		for (int i = 0; i < cubesInBrush.Length; i++)
		{
			Vector3i vector3i = _pos + cubesInBrush[i];
			num += (int)_data.world.GetDensity(vector3i);
		}
		return (sbyte)(num / (cubesInBrush.Length + 1));
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0005B7A8 File Offset: 0x000599A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public sbyte GetAverageDensity(Vector3i _pos, ItemInventoryData _data)
	{
		float num = 0f;
		float num2 = 1f;
		float num3 = num + (float)_data.world.GetDensity(_pos.x, _pos.y, _pos.z);
		num2 += 1f;
		float num4 = num3 + (float)_data.world.GetDensity(_pos.x - 1, _pos.y, _pos.z);
		num2 += 1f;
		float num5 = num4 + (float)_data.world.GetDensity(_pos.x + 1, _pos.y, _pos.z);
		num2 += 1f;
		float num6 = num5 + (float)_data.world.GetDensity(_pos.x, _pos.y, _pos.z - 1);
		num2 += 1f;
		return (sbyte)((num6 + (float)_data.world.GetDensity(_pos.x, _pos.y, _pos.z + 1)) / num2);
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x0005B888 File Offset: 0x00059A88
	[PublicizedFrom(EAccessModifier.Private)]
	public sbyte[] GetLocalDensityMap(Vector3i blockTargetPos, ItemInventoryData data, Vector3i[] localArea)
	{
		sbyte[] array = new sbyte[localArea.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = data.world.GetDensity(localArea[i].x, localArea[i].y, localArea[i].z);
		}
		return array;
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0005B8E0 File Offset: 0x00059AE0
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool HasValidNeighbor(Vector3i _pos, sbyte _denThreshold, ItemInventoryData _data)
	{
		if (_denThreshold < 0)
		{
			return _data.world.GetDensity(_pos.x - 1, _pos.y, _pos.z) <= _denThreshold || _data.world.GetDensity(_pos.x + 1, _pos.y, _pos.z) <= _denThreshold || _data.world.GetDensity(_pos.x, _pos.y - 1, _pos.z) <= _denThreshold || _data.world.GetDensity(_pos.x, _pos.y + 1, _pos.z) <= _denThreshold || _data.world.GetDensity(_pos.x, _pos.y, _pos.z - 1) <= _denThreshold || _data.world.GetDensity(_pos.x, _pos.y, _pos.z + 1) <= _denThreshold;
		}
		return _denThreshold >= 0 && (_data.world.GetDensity(_pos.x - 1, _pos.y, _pos.z) >= _denThreshold || _data.world.GetDensity(_pos.x + 1, _pos.y, _pos.z) >= _denThreshold || _data.world.GetDensity(_pos.x, _pos.y - 1, _pos.z) >= _denThreshold || _data.world.GetDensity(_pos.x, _pos.y + 1, _pos.z) >= _denThreshold || _data.world.GetDensity(_pos.x, _pos.y, _pos.z - 1) >= _denThreshold || _data.world.GetDensity(_pos.x, _pos.y, _pos.z + 1) >= _denThreshold);
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x0005BAB0 File Offset: 0x00059CB0
	public override string ToString()
	{
		return "Smooth Terrain";
	}

	// Token: 0x04000BEA RID: 3050
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTools.Brush brush;

	// Token: 0x04000BEB RID: 3051
	[PublicizedFrom(EAccessModifier.Private)]
	public NguiWdwTerrainEditor parentWindow;

	// Token: 0x04000BEC RID: 3052
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemInventoryData lastData;

	// Token: 0x04000BED RID: 3053
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isButtonDown;

	// Token: 0x04000BEE RID: 3054
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockMaskRules<sbyte?, char> blockMaskRuleset;
}
