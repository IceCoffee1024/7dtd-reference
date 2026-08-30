using System;
using System.Security.Cryptography;
using UnityEngine.Scripting;

// Token: 0x020001A9 RID: 425
[Preserve]
public class BlockShapeInvisible : BlockShape
{
	// Token: 0x06000CCE RID: 3278 RVA: 0x000508F4 File Offset: 0x0004EAF4
	public BlockShapeInvisible()
	{
		this.IsSolidCube = false;
		this.IsSolidSpace = false;
		this.LightOpacity = 0;
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x00050911 File Offset: 0x0004EB11
	public override void Init(Block _block)
	{
		base.Init(_block);
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return false;
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 0;
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x000027FC File Offset: 0x000009FC
	public override void CalculateCollisionHash(IncrementalHash calcHash)
	{
	}
}
