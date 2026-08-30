using System;
using System.Security.Cryptography;
using UnityEngine.Scripting;

// Token: 0x0200019D RID: 413
[Preserve]
public class BlockShapeBillboardAbstract : BlockShape
{
	// Token: 0x06000C97 RID: 3223 RVA: 0x0004E1F7 File Offset: 0x0004C3F7
	public BlockShapeBillboardAbstract()
	{
		this.IsSolidCube = false;
		this.IsSolidSpace = false;
		this.LightOpacity = 0;
		this.IsOmitTerrainSnappingUp = true;
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsRenderDecoration()
	{
		return true;
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return false;
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 0;
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x000027FC File Offset: 0x000009FC
	public override void CalculateCollisionHash(IncrementalHash calcHash)
	{
	}

	// Token: 0x04000B4F RID: 2895
	public float yPosSubtract;
}
