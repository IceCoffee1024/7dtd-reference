using System;
using UnityEngine.Scripting;

// Token: 0x02000161 RID: 353
[Preserve]
public class BlockTrunkTip : BlockDamage
{
	// Token: 0x060009B4 RID: 2484 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool RotateVerticesOnCollisionCheck(BlockValue _blockValue)
	{
		return false;
	}
}
