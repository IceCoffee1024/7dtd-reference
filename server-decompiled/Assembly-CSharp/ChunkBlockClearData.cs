using System;
using System.Collections.Generic;

// Token: 0x02000AFA RID: 2810
public class ChunkBlockClearData : ChunkCustomData
{
	// Token: 0x060054AE RID: 21678 RVA: 0x002064D4 File Offset: 0x002046D4
	public ChunkBlockClearData()
	{
	}

	// Token: 0x060054AF RID: 21679 RVA: 0x002064E7 File Offset: 0x002046E7
	public ChunkBlockClearData(string _key, ulong _expiresInWorldTime, bool _isSavedToNetwork, World _world)
		: base(_key, _expiresInWorldTime, _isSavedToNetwork)
	{
		this.World = _world;
	}

	// Token: 0x060054B0 RID: 21680 RVA: 0x00206508 File Offset: 0x00204708
	public override void OnRemove(Chunk chunk)
	{
		for (int i = this.BlockList.Count - 1; i >= 0; i--)
		{
			Vector3i vector3i = this.BlockList[i];
			chunk.SetBlock(this.World, vector3i.x, vector3i.y, vector3i.z, BlockValue.Air, true, true, false, false, -1);
		}
	}

	// Token: 0x04004191 RID: 16785
	public List<Vector3i> BlockList = new List<Vector3i>();

	// Token: 0x04004192 RID: 16786
	public World World;
}
