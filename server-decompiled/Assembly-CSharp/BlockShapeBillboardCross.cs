using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001A2 RID: 418
[Preserve]
public class BlockShapeBillboardCross : BlockShapeBillboardAbstract
{
	// Token: 0x06000CA0 RID: 3232 RVA: 0x0004EB2C File Offset: 0x0004CD2C
	public BlockShapeBillboardCross()
	{
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x0004EB4C File Offset: 0x0004CD4C
	public BlockShapeBillboardCross(float _scaleAdd)
	{
		this.s = _scaleAdd;
		this.h = 1f + _scaleAdd;
		this.yPosSubtract += _scaleAdd;
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x0004EB98 File Offset: 0x0004CD98
	public override void renderFull(Vector3i _worldPos, BlockValue _blockValue, Vector3 _drawPos, Vector3[] _vertices, LightingAround _lightingAround, TextureFullArray _textureFullArray, VoxelMesh[] _meshes, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
		byte sun = _lightingAround[LightingAround.Pos.Middle].sun;
		byte block = _lightingAround[LightingAround.Pos.Middle].block;
		float num = _drawPos.y;
		float num2 = _drawPos.y;
		float num3 = _drawPos.y;
		float num4 = _drawPos.y;
		if (_vertices != null)
		{
			float num5 = _drawPos.y - _vertices[0].y;
			num = _vertices[0].y + (_vertices[3].y - _vertices[0].y) * 0.5f - this.yPosSubtract + num5;
			num2 = _vertices[3].y + (_vertices[7].y - _vertices[3].y) * 0.5f - this.yPosSubtract + num5;
			num3 = _vertices[0].y + (_vertices[4].y - _vertices[0].y) * 0.5f - this.yPosSubtract + num5;
			num4 = _vertices[4].y + (_vertices[7].y - _vertices[4].y) * 0.5f - this.yPosSubtract + num5;
		}
		this.v[0] = new Vector3(_drawPos.x - this.s, num, _drawPos.z + 0.5f);
		this.v[1] = new Vector3(_drawPos.x + 1f + this.s, num2, _drawPos.z + 0.5f);
		this.v[2] = new Vector3(_drawPos.x + 0.5f, num3, _drawPos.z - this.s);
		this.v[3] = new Vector3(_drawPos.x + 0.5f, num4, _drawPos.z + 1f + this.s);
		this.v[4] = new Vector3(_drawPos.x - this.s, num + this.h, _drawPos.z + 0.5f);
		this.v[5] = new Vector3(_drawPos.x + 1f + this.s, num2 + this.h, _drawPos.z + 0.5f);
		this.v[6] = new Vector3(_drawPos.x + 0.5f, num3 + this.h, _drawPos.z - this.s);
		this.v[7] = new Vector3(_drawPos.x + 0.5f, num4 + this.h, _drawPos.z + 1f + this.s);
		Block block2 = _blockValue.Block;
		VoxelMesh voxelMesh = _meshes[(int)block2.MeshIndex];
		voxelMesh.AddBlockSide(this.v[0], this.v[1], this.v[5], this.v[4], _blockValue, VoxelMesh.COLOR_TOP, BlockFace.Top, sun, block, (int)block2.MeshIndex);
		voxelMesh.AddBlockSide(this.v[1], this.v[0], this.v[4], this.v[5], _blockValue, VoxelMesh.COLOR_TOP, BlockFace.Top, sun, block, (int)block2.MeshIndex);
		voxelMesh.AddBlockSide(this.v[3], this.v[2], this.v[6], this.v[7], _blockValue, VoxelMesh.COLOR_TOP, BlockFace.Top, sun, block, (int)block2.MeshIndex);
		voxelMesh.AddBlockSide(this.v[2], this.v[3], this.v[7], this.v[6], _blockValue, VoxelMesh.COLOR_TOP, BlockFace.Top, sun, block, (int)block2.MeshIndex);
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMovementBlocked(BlockValue _blockValue, BlockFace crossingFace)
	{
		return false;
	}

	// Token: 0x04000B57 RID: 2903
	[PublicizedFrom(EAccessModifier.Private)]
	public float h = 1f;

	// Token: 0x04000B58 RID: 2904
	[PublicizedFrom(EAccessModifier.Private)]
	public float s;

	// Token: 0x04000B59 RID: 2905
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] v = new Vector3[8];
}
