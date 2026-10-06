using System;
using System.Collections;

// Token: 0x02000B3D RID: 2877
public class ChunkProviderDummy : ChunkProviderAbstract
{
	// Token: 0x060056B0 RID: 22192 RVA: 0x00216597 File Offset: 0x00214797
	public override IEnumerator Init(World _worldData)
	{
		MultiBlockManager.Instance.Initialize(null);
		yield return null;
		yield break;
	}

	// Token: 0x060056B1 RID: 22193 RVA: 0x0021659F File Offset: 0x0021479F
	public override void Cleanup()
	{
		base.Cleanup();
		MultiBlockManager.Instance.Cleanup();
	}

	// Token: 0x060056B2 RID: 22194 RVA: 0x000470B6 File Offset: 0x000452B6
	public override EnumChunkProviderId GetProviderId()
	{
		return EnumChunkProviderId.NetworkClient;
	}

	// Token: 0x060056B3 RID: 22195 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void setChunkPrerequisits(string sSeed)
	{
	}

	// Token: 0x060056B4 RID: 22196 RVA: 0x002165B1 File Offset: 0x002147B1
	public override void UnloadChunk(Chunk _c)
	{
		MemoryPools.PoolChunks.FreeSync(_c);
	}

	// Token: 0x060056B5 RID: 22197 RVA: 0x002165BE File Offset: 0x002147BE
	public override SpawnPointList GetSpawnPointList()
	{
		return this.dummySpawnPointList;
	}

	// Token: 0x060056B6 RID: 22198 RVA: 0x002165C6 File Offset: 0x002147C6
	public override void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
		this.dummySpawnPointList = _spawnPointList;
	}

	// Token: 0x04004353 RID: 17235
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPointList dummySpawnPointList = new SpawnPointList();
}
