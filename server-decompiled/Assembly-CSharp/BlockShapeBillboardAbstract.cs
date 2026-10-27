using System;
using System.Security.Cryptography;
using UnityEngine.Scripting;

// Token: 0x020001A0 RID: 416
[Preserve]
public class BlockShapeBillboardAbstract : BlockShape
{
	// Token: 0x06000C99 RID: 3225 RVA: 0x0004E5D3 File Offset: 0x0004C7D3
	public BlockShapeBillboardAbstract()
	{
		this.IsSolidCube = false;
		this.IsSolidSpace = false;
		this.LightOpacity = 0;
		this.IsOmitTerrainSnappingUp = true;
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsRenderDecoration()
	{
		return true;
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return false;
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 0;
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x000027FC File Offset: 0x000009FC
	public override void CalculateCollisionHash(IncrementalHash calcHash)
	{
	}

	// Token: 0x04000B55 RID: 2901
	public float yPosSubtract;
}
