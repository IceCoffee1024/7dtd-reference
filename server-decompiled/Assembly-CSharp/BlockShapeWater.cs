using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020001BB RID: 443
[Preserve]
public class BlockShapeWater : BlockShapeCube
{
	// Token: 0x06000D42 RID: 3394 RVA: 0x00056F92 File Offset: 0x00055192
	public BlockShapeWater()
	{
		this.IsSolidCube = false;
		this.IsSolidSpace = false;
		this.LightOpacity = 0;
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x00056FAF File Offset: 0x000551AF
	public override void renderFace(Vector3[] _vertices, LightingAround _lightingAround, long _textureFull, VoxelMesh[] _meshes, Vector2 UVdata, BlockShape.MeshPurpose _purpose = BlockShape.MeshPurpose.World)
	{
		_meshes[1].AddBasicQuad(_vertices, Color.white, UVdata, true, false);
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 0;
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return false;
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0003D372 File Offset: 0x0003B572
	public override float GetStepHeight(BlockValue _blockValue, BlockFace crossingFace)
	{
		return 0f;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMovementBlocked(BlockValue _blockValue, BlockFace crossingFace)
	{
		return false;
	}
}
