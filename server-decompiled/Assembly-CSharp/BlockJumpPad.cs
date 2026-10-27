using System;
using UnityEngine.Scripting;

// Token: 0x02000126 RID: 294
[Preserve]
public class BlockJumpPad : Block
{
	// Token: 0x060007BF RID: 1983 RVA: 0x00036D95 File Offset: 0x00034F95
	public override void OnEntityWalking(WorldBase _world, int _x, int _y, int _z, BlockValue _blockValue, Entity entity)
	{
		entity.motion.y = 3f;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BlockFace getInventoryFace()
	{
		return BlockFace.Top;
	}
}
