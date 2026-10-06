using System;
using System.Collections.Generic;

// Token: 0x02000B06 RID: 2822
[PublicizedFrom(EAccessModifier.Internal)]
public class ChannelCalculator
{
	// Token: 0x060053A2 RID: 21410 RVA: 0x002018D3 File Offset: 0x001FFAD3
	public ChannelCalculator(WorldBase _world)
	{
		this.world = _world;
	}

	// Token: 0x1700092C RID: 2348
	// (get) Token: 0x060053A3 RID: 21411 RVA: 0x002018E2 File Offset: 0x001FFAE2
	public static HashSet<Vector3i> list
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (ChannelCalculator.List == null)
			{
				ChannelCalculator.List = new HashSet<Vector3i>();
			}
			return ChannelCalculator.List;
		}
	}

	// Token: 0x1700092D RID: 2349
	// (get) Token: 0x060053A4 RID: 21412 RVA: 0x002018FA File Offset: 0x001FFAFA
	public static List<Vector3i> list2
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (ChannelCalculator.List2 == null)
			{
				ChannelCalculator.List2 = new List<Vector3i>();
			}
			return ChannelCalculator.List2;
		}
	}

	// Token: 0x060053A5 RID: 21413 RVA: 0x00201914 File Offset: 0x001FFB14
	public void BlockRemovedAt(Vector3i _pos, HashSet<Vector3i> _stab0Positions)
	{
		BlockValue block = this.world.GetBlock(_pos);
		Block block2 = block.Block;
		if ((!block.isair && block2.blockMaterial.IsLiquid) || block2.StabilityIgnore)
		{
			return;
		}
		ChannelCalculator.list.Clear();
		ChannelCalculator.list2.Clear();
		this.CalcChangedPositionsFromRemove(_pos, ChannelCalculator.list2, _stab0Positions, null);
		IChunk chunk = null;
		for (int i = 0; i < ChannelCalculator.list2.Count; i++)
		{
			Vector3i vector3i = ChannelCalculator.list2[i];
			if (this.world.GetChunkFromWorldPos(vector3i, ref chunk))
			{
				int num = World.toBlockXZ(vector3i.x);
				int num2 = World.toBlockY(vector3i.y);
				int num3 = World.toBlockXZ(vector3i.z);
				int stability = (int)chunk.GetStability(num, num2, num3);
				if (stability > 1)
				{
					this.ChangeStability(vector3i, stability, null, _stab0Positions, chunk);
				}
			}
		}
	}

	// Token: 0x060053A6 RID: 21414 RVA: 0x002019F8 File Offset: 0x001FFBF8
	public void BlockPlacedAt(Vector3i _pos, bool _isForceFullStab)
	{
		int num = 15;
		if (!_isForceFullStab)
		{
			_pos.y--;
			num = (int)this.world.GetStability(_pos);
			_pos.y++;
		}
		if (num == 15)
		{
			List<Vector3i> list = new List<Vector3i>();
			for (;;)
			{
				BlockValue block;
				BlockValue blockValue = (block = this.world.GetBlock(_pos));
				Block block2;
				if (block.isair || (block2 = blockValue.Block).blockMaterial.IsLiquid || block2.StabilityIgnore)
				{
					goto IL_00A9;
				}
				if (!block2.StabilitySupport)
				{
					break;
				}
				this.world.SetStability(_pos, 15);
				list.Add(_pos);
				_pos.y++;
			}
			this.world.SetStability(_pos, 1);
			IL_00A9:
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.ChangeStability(list[i], 15, null, null, null);
			}
			return;
		}
		bool flag;
		int maxStabilityAround = this.getMaxStabilityAround(_pos, out flag);
		int num2 = (flag ? maxStabilityAround : (maxStabilityAround - 1));
		BlockValue block3 = this.world.GetBlock(_pos);
		Block block4 = block3.Block;
		if (!block3.isair && !block4.blockMaterial.IsLiquid && !block4.StabilityIgnore)
		{
			if (num2 > 1 && !block4.StabilitySupport)
			{
				num2 = 1;
			}
			this.world.SetStability(_pos, (byte)((num2 < 0) ? 0 : num2));
			this.ChangeStability(_pos, num2, null, null, null);
		}
	}

	// Token: 0x060053A7 RID: 21415 RVA: 0x00201B60 File Offset: 0x001FFD60
	[PublicizedFrom(EAccessModifier.Private)]
	public int getMaxStabilityAround(Vector3i _pos, out bool _bFromDownwards)
	{
		_bFromDownwards = false;
		int num = 0;
		int num2 = 0;
		Vector3i[] allDirections = Vector3i.AllDirections;
		for (int i = 0; i < allDirections.Length; i++)
		{
			Vector3i vector3i = _pos + allDirections[i];
			int stability = (int)this.world.GetStability(vector3i);
			if (allDirections[i].y == -1)
			{
				num2 = stability;
			}
			if (stability > num && this.world.GetBlock(vector3i).Block.StabilitySupport)
			{
				num = stability;
			}
		}
		_bFromDownwards = num == num2;
		return num;
	}

	// Token: 0x060053A8 RID: 21416 RVA: 0x00201BE4 File Offset: 0x001FFDE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcChangedPositionsFromRemove(Vector3i _pos, List<Vector3i> _neighbors, HashSet<Vector3i> _stab0Positions, IChunk chunk = null)
	{
		int stability = (int)this.world.GetStability(_pos);
		this.world.SetStability(_pos, 0);
		_stab0Positions.Add(_pos);
		foreach (Vector3i vector3i in Vector3i.AllDirections)
		{
			Vector3i vector3i2 = _pos + vector3i;
			if (this.world.GetChunkFromWorldPos(vector3i2, ref chunk))
			{
				Vector3i vector3i3 = World.toBlock(vector3i2);
				BlockValue blockNoDamage = chunk.GetBlockNoDamage(vector3i3.x, vector3i3.y, vector3i3.z);
				if (!blockNoDamage.isair)
				{
					Block block = blockNoDamage.Block;
					if (!block.blockMaterial.IsLiquid && !block.StabilityIgnore)
					{
						int stability2 = (int)chunk.GetStability(vector3i3.x, vector3i3.y, vector3i3.z);
						if (stability2 == 1 && !block.StabilitySupport)
						{
							if (!this.IsBlockSupportedByNeighbor(vector3i2))
							{
								this.world.SetStability(vector3i2, 0);
								_stab0Positions.Add(vector3i2);
							}
						}
						else if (stability2 == stability - 1 || (vector3i.y == 1 && stability2 == stability))
						{
							this.CalcChangedPositionsFromRemove(vector3i2, _neighbors, _stab0Positions, chunk);
						}
						else if (stability2 >= stability)
						{
							_neighbors.Add(vector3i2);
						}
					}
				}
			}
		}
	}

	// Token: 0x060053A9 RID: 21417 RVA: 0x00201D28 File Offset: 0x001FFF28
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsBlockSupportedByNeighbor(Vector3i _blockPos)
	{
		IChunk chunk = null;
		foreach (Vector3i vector3i in Vector3i.AllDirections)
		{
			Vector3i vector3i2 = _blockPos + vector3i;
			if (this.world.GetChunkFromWorldPos(vector3i2, ref chunk))
			{
				Vector3i vector3i3 = World.toBlock(vector3i2);
				BlockValue blockNoDamage = chunk.GetBlockNoDamage(vector3i3.x, vector3i3.y, vector3i3.z);
				if (!blockNoDamage.isair)
				{
					Block block = blockNoDamage.Block;
					if (!block.blockMaterial.IsLiquid && !block.StabilityIgnore && chunk.GetStability(vector3i3.x, vector3i3.y, vector3i3.z) > 1)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060053AA RID: 21418 RVA: 0x00201DE4 File Offset: 0x001FFFE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ChangeStability(Vector3i _pos, int _stab, List<Vector3i> _changedPositions, HashSet<Vector3i> _stab0Positions = null, IChunk chunk = null)
	{
		foreach (Vector3i vector3i in Vector3i.AllDirections)
		{
			Vector3i vector3i2 = _pos + vector3i;
			if (this.world.GetChunkFromWorldPos(vector3i2, ref chunk))
			{
				Vector3i vector3i3 = World.toBlock(vector3i2);
				BlockValue blockNoDamage = chunk.GetBlockNoDamage(vector3i3.x, vector3i3.y, vector3i3.z);
				if (!blockNoDamage.isair)
				{
					Block block = blockNoDamage.Block;
					if (!block.blockMaterial.IsLiquid && !block.StabilityIgnore)
					{
						int num = _stab - 1;
						if ((int)chunk.GetStability(vector3i3.x, vector3i3.y, vector3i3.z) < num)
						{
							if (!block.StabilitySupport && num > 1)
							{
								num = 1;
							}
							if (_stab0Positions != null)
							{
								if (num == 0)
								{
									_stab0Positions.Add(vector3i2);
								}
								else
								{
									_stab0Positions.Remove(vector3i2);
								}
							}
							if (_changedPositions != null)
							{
								_changedPositions.Add(vector3i2);
							}
							chunk.SetStability(vector3i3.x, vector3i3.y, vector3i3.z, (byte)num);
							this.ChangeStability(vector3i2, num, _changedPositions, _stab0Positions, chunk);
						}
					}
				}
			}
		}
	}

	// Token: 0x04004187 RID: 16775
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly WorldBase world;

	// Token: 0x04004188 RID: 16776
	[ThreadStatic]
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<Vector3i> List;

	// Token: 0x04004189 RID: 16777
	[ThreadStatic]
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Vector3i> List2;
}
