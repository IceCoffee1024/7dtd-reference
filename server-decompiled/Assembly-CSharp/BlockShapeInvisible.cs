using System;
using System.Security.Cryptography;
using UnityEngine.Scripting;

// Token: 0x020001AC RID: 428
[Preserve]
public class BlockShapeInvisible : BlockShape
{
	// Token: 0x06000CD0 RID: 3280 RVA: 0x00050CD0 File Offset: 0x0004EED0
	public BlockShapeInvisible()
	{
		this.IsSolidCube = false;
		this.IsSolidSpace = false;
		this.LightOpacity = 0;
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x00050CED File Offset: 0x0004EEED
	public override void Init(Block _block)
	{
		base.Init(_block);
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return false;
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 0;
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x000027FC File Offset: 0x000009FC
	public override void CalculateCollisionHash(IncrementalHash calcHash)
	{
	}
}
