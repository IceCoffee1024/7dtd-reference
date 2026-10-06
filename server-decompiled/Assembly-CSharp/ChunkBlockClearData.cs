using System;
using System.Collections.Generic;

// Token: 0x02000B1E RID: 2846
public class ChunkBlockClearData : ChunkCustomData
{
	// Token: 0x0600554F RID: 21839 RVA: 0x0020C460 File Offset: 0x0020A660
	public ChunkBlockClearData()
	{
	}

	// Token: 0x06005550 RID: 21840 RVA: 0x0020C473 File Offset: 0x0020A673
	public ChunkBlockClearData(string _key, ulong _expiresInWorldTime, bool _isSavedToNetwork, World _world)
		: base(_key, _expiresInWorldTime, _isSavedToNetwork)
	{
		this.World = _world;
	}

	// Token: 0x06005551 RID: 21841 RVA: 0x0020C494 File Offset: 0x0020A694
	public override void OnRemove(Chunk chunk)
	{
		for (int i = this.BlockList.Count - 1; i >= 0; i--)
		{
			Vector3i vector3i = this.BlockList[i];
			chunk.SetBlock(this.World, vector3i.x, vector3i.y, vector3i.z, BlockValue.Air, true, true, false, false, -1);
		}
	}

	// Token: 0x04004269 RID: 17001
	public List<Vector3i> BlockList = new List<Vector3i>();

	// Token: 0x0400426A RID: 17002
	public World World;
}
