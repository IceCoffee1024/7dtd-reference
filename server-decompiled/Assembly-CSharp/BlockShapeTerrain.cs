using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001BA RID: 442
[Preserve]
public class BlockShapeTerrain : BlockShapeCube
{
	// Token: 0x06000D3B RID: 3387 RVA: 0x00056B68 File Offset: 0x00054D68
	public BlockShapeTerrain()
	{
		this.IsOmitTerrainSnappingUp = true;
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x00056C30 File Offset: 0x00054E30
	public override void renderFace(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, BlockFace _face, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
		float num = _drawPos.y - _vertices[0].y + 1f + 0.0001f;
		if (num > -0.01f)
		{
			num = 0.0001f;
		}
		UVRectTiling uvrectTiling = MeshDescription.meshes[4].textureAtlas.uvMapping[500 + (int)_blockValue.decaltex];
		Utils.MoveInBlockFaceDirection(_vertices, _face, num);
		_meshes[4].AddQuadNoCollision(_vertices[0], _vertices[1], _vertices[2], _vertices[3], Color.white, uvrectTiling.uv);
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x00056CCE File Offset: 0x00054ECE
	public override bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return _blockValue.hasdecal && _blockValue.decalface == _face;
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 0;
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsTerrain()
	{
		return true;
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x00056CE8 File Offset: 0x00054EE8
	public override void renderFull(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
		byte sun = _lightingAround[LightingAround.Pos.Middle].sun;
		byte block = _lightingAround[LightingAround.Pos.Middle].block;
		VoxelMeshTerrain voxelMeshTerrain = (VoxelMeshTerrain)_meshes[5];
		Block block2 = _blockValue.Block;
		byte meshIndex = block2.MeshIndex;
		voxelMeshTerrain.AddBlockSideTri(this.v[0] + _drawPos, this.v[2] + _drawPos, this.v[1] + _drawPos, (int)meshIndex, _blockValue, VoxelMesh.COLOR_BOTTOM, BlockFace.Bottom, sun, block);
		voxelMeshTerrain.AddBlockSideTri(this.v[0] + _drawPos, this.v[1] + _drawPos, this.v[4] + _drawPos, (int)meshIndex, _blockValue, VoxelMesh.COLOR_BOTTOM, BlockFace.Bottom, sun, block);
		voxelMeshTerrain.AddBlockSideTri(this.v[2] + _drawPos, this.v[5] + _drawPos, this.v[1] + _drawPos, (int)meshIndex, _blockValue, VoxelMesh.COLOR_TOP, BlockFace.Top, sun, block);
		voxelMeshTerrain.AddBlockSideTri(this.v[1] + _drawPos, this.v[5] + _drawPos, this.v[4] + _drawPos, (int)meshIndex, _blockValue, VoxelMesh.COLOR_TOP, BlockFace.Top, sun, block);
		voxelMeshTerrain.AddBlockSideTri(this.v[3] + _drawPos, this.v[5] + _drawPos, this.v[2] + _drawPos, (int)meshIndex, _blockValue, VoxelMesh.COLOR_BOTTOM, BlockFace.Bottom, sun, block);
		voxelMeshTerrain.AddBlockSideTri(this.v[0] + _drawPos, this.v[3] + _drawPos, this.v[2] + _drawPos, (int)meshIndex, _blockValue, VoxelMesh.COLOR_BOTTOM, BlockFace.Bottom, sun, block);
		voxelMeshTerrain.AddBlockSideTri(this.v[0] + _drawPos, this.v[4] + _drawPos, this.v[3] + _drawPos, (int)meshIndex, _blockValue, VoxelMesh.COLOR_TOP, BlockFace.Top, sun, block);
		voxelMeshTerrain.AddBlockSideTri(this.v[4] + _drawPos, this.v[5] + _drawPos, this.v[3] + _drawPos, (int)meshIndex, _blockValue, VoxelMesh.COLOR_TOP, BlockFace.Top, sun, block);
		int sideTextureId = block2.GetSideTextureId(_blockValue, BlockFace.Top, 0);
		int sideTextureId2 = block2.GetSideTextureId(_blockValue, BlockFace.South, 0);
		int num = voxelMeshTerrain.FindOrCreateSubMesh((sideTextureId << 16) | sideTextureId2, -1, -1);
		for (int i = 0; i < voxelMeshTerrain.Indices.Count; i += 3)
		{
			voxelMeshTerrain.AddIndices(voxelMeshTerrain.Indices[i], voxelMeshTerrain.Indices[i + 1], voxelMeshTerrain.Indices[i + 2], num);
		}
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x00056FCD File Offset: 0x000551CD
	public override Quaternion GetPreviewRotation()
	{
		return Quaternion.AngleAxis(55f, Vector3.up) * Quaternion.AngleAxis(10f, Vector3.forward);
	}

	// Token: 0x04000BAF RID: 2991
	[PublicizedFrom(EAccessModifier.Private)]
	public new readonly Vector3[] v = new Vector3[]
	{
		new Vector3(0.5f, 0f, 0.5f),
		new Vector3(0.5f, 0.5f, 0f),
		new Vector3(1f, 0.5f, 0.5f),
		new Vector3(0.5f, 0.5f, 1f),
		new Vector3(0f, 0.5f, 0.5f),
		new Vector3(0.5f, 1f, 0.5f)
	};
}
