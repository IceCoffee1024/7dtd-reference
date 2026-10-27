using System;
using System.Collections.Generic;

// Token: 0x02000B29 RID: 2857
public class ChunkBlockClearData : ChunkCustomData
{
	// Token: 0x06005574 RID: 21876 RVA: 0x0020D9DC File Offset: 0x0020BBDC
	public ChunkBlockClearData()
	{
	}

	// Token: 0x06005575 RID: 21877 RVA: 0x0020D9EF File Offset: 0x0020BBEF
	public ChunkBlockClearData(string _key, ulong _expiresInWorldTime, bool _isSavedToNetwork, World _world)
		: base(_key, _expiresInWorldTime, _isSavedToNetwork)
	{
		this.World = _world;
	}

	// Token: 0x06005576 RID: 21878 RVA: 0x0020DA10 File Offset: 0x0020BC10
	public override void OnRemove(Chunk chunk)
	{
		for (int i = this.BlockList.Count - 1; i >= 0; i--)
		{
			Vector3i vector3i = this.BlockList[i];
			chunk.SetBlock(this.World, vector3i.x, vector3i.y, vector3i.z, BlockValue.Air, true, true, false, false, -1);
		}
	}

	// Token: 0x0400428E RID: 17038
	public List<Vector3i> BlockList = new List<Vector3i>();

	// Token: 0x0400428F RID: 17039
	public World World;
}
