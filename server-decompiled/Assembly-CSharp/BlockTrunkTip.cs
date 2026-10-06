using System;
using UnityEngine.Scripting;

// Token: 0x02000164 RID: 356
[Preserve]
public class BlockTrunkTip : BlockDamage
{
	// Token: 0x060009C4 RID: 2500 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool RotateVerticesOnCollisionCheck(BlockValue _blockValue)
	{
		return false;
	}
}
