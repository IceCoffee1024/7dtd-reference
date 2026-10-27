using System;
using System.Collections;

// Token: 0x02000B48 RID: 2888
public class ChunkProviderDummy : ChunkProviderAbstract
{
	// Token: 0x060056D5 RID: 22229 RVA: 0x00217AF3 File Offset: 0x00215CF3
	public override IEnumerator Init(World _worldData)
	{
		MultiBlockManager.Instance.Initialize(null);
		yield return null;
		yield break;
	}

	// Token: 0x060056D6 RID: 22230 RVA: 0x00217AFB File Offset: 0x00215CFB
	public override void Cleanup()
	{
		base.Cleanup();
		MultiBlockManager.Instance.Cleanup();
	}

	// Token: 0x060056D7 RID: 22231 RVA: 0x00047076 File Offset: 0x00045276
	public override EnumChunkProviderId GetProviderId()
	{
		return EnumChunkProviderId.NetworkClient;
	}

	// Token: 0x060056D8 RID: 22232 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void setChunkPrerequisits(string sSeed)
	{
	}

	// Token: 0x060056D9 RID: 22233 RVA: 0x00217B0D File Offset: 0x00215D0D
	public override void UnloadChunk(Chunk _c)
	{
		MemoryPools.PoolChunks.FreeSync(_c);
	}

	// Token: 0x060056DA RID: 22234 RVA: 0x00217B1A File Offset: 0x00215D1A
	public override SpawnPointList GetSpawnPointList()
	{
		return this.dummySpawnPointList;
	}

	// Token: 0x060056DB RID: 22235 RVA: 0x00217B22 File Offset: 0x00215D22
	public override void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
		this.dummySpawnPointList = _spawnPointList;
	}

	// Token: 0x04004378 RID: 17272
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPointList dummySpawnPointList = new SpawnPointList();
}
