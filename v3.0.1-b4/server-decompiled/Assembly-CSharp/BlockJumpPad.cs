using System;
using UnityEngine.Scripting;

// Token: 0x02000123 RID: 291
[Preserve]
public class BlockJumpPad : Block
{
	// Token: 0x060007AF RID: 1967 RVA: 0x000369F1 File Offset: 0x00034BF1
	public override void OnEntityWalking(WorldBase _world, int _x, int _y, int _z, BlockValue _blockValue, Entity entity)
	{
		entity.motion.y = 3f;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BlockFace getInventoryFace()
	{
		return BlockFace.Top;
	}
}
