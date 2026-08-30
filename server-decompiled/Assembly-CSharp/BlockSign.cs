using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200014A RID: 330
[Preserve]
public class BlockSign : Block
{
	// Token: 0x06000922 RID: 2338 RVA: 0x0003FD24 File Offset: 0x0003DF24
	public override void Init()
	{
		base.Init();
		if (base.Properties.Values.ContainsKey("UpwardsCount"))
		{
			this.upwardsCount = int.Parse(base.Properties.Values["UpwardsCount"]);
		}
		else
		{
			this.upwardsCount = 1;
		}
		this.IsTerrainDecoration = true;
		this.CanDecorateOnSlopes = false;
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x0003FD88 File Offset: 0x0003DF88
	public override void OnBlockAdded(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		if ((_blockValue.meta & 1) == 0)
		{
			this.shape.OnBlockAdded(world, _chunk, _blockPos, _blockValue);
			_blockValue.meta |= 1;
			for (int i = _blockPos.y + 1; i < _blockPos.y + this.upwardsCount + 1; i++)
			{
				_chunk.SetBlockRaw(World.toBlockXZ(_blockPos.x), i, World.toBlockXZ(_blockPos.z), _blockValue);
			}
		}
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0003FE04 File Offset: 0x0003E004
	public override void OnBlockRemoved(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (_chunk == null)
		{
			_chunk = (Chunk)_world.GetChunkFromWorldPos(_blockPos);
		}
		if (_chunk == null)
		{
			return;
		}
		if ((_blockValue.meta & 1) == 0)
		{
			this.shape.OnBlockRemoved(_world, _chunk, _blockPos, _blockValue);
			for (int i = _blockPos.y + 1; i < _blockPos.y + this.upwardsCount + 1; i++)
			{
				_chunk.SetBlockRaw(World.toBlockXZ(_blockPos.x), i, World.toBlockXZ(_blockPos.z), BlockValue.Air);
			}
			return;
		}
		if (_world.IsRemote())
		{
			return;
		}
		int num = _blockPos.y - 1;
		while (num >= _blockPos.y - this.upwardsCount && _blockPos.y >= 1)
		{
			BlockValue block = _world.GetBlock(_blockPos.x, num, _blockPos.z);
			if (block.type != _blockValue.type)
			{
				break;
			}
			if ((block.meta & 1) == 0)
			{
				_world.SetBlockRPC(new Vector3i(_blockPos.x, num, _blockPos.z), BlockValue.Air);
				return;
			}
			num--;
		}
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x0003FF0C File Offset: 0x0003E10C
	public override void RenderDecorations(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, INeighborBlockCache _nBlocks)
	{
		if ((_blockValue.meta & 1) == 0)
		{
			this.shape.renderDecorations(_worldPos, _blockValue, _drawPos, _vertices, _lightingAround, _textureFullArray, _meshes, _nBlocks);
		}
	}

	// Token: 0x040009A3 RID: 2467
	[PublicizedFrom(EAccessModifier.Private)]
	public int upwardsCount;
}
