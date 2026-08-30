using System;
using System.Collections;

// Token: 0x02000B2B RID: 2859
public class ChunkProviderDummy : ChunkProviderAbstract
{
	// Token: 0x06005696 RID: 22166 RVA: 0x0021330B File Offset: 0x0021150B
	public override IEnumerator Init(World _worldData)
	{
		MultiBlockManager.Instance.Initialize(null);
		yield return null;
		yield break;
	}

	// Token: 0x06005697 RID: 22167 RVA: 0x00213313 File Offset: 0x00211513
	public override void Cleanup()
	{
		base.Cleanup();
		MultiBlockManager.Instance.Cleanup();
	}

	// Token: 0x06005698 RID: 22168 RVA: 0x00046EDA File Offset: 0x000450DA
	public override EnumChunkProviderId GetProviderId()
	{
		return EnumChunkProviderId.NetworkClient;
	}

	// Token: 0x06005699 RID: 22169 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void setChunkPrerequisits(string sSeed)
	{
	}

	// Token: 0x0600569A RID: 22170 RVA: 0x00213325 File Offset: 0x00211525
	public override void UnloadChunk(Chunk _c)
	{
		MemoryPools.PoolChunks.FreeSync(_c);
	}

	// Token: 0x0600569B RID: 22171 RVA: 0x00213332 File Offset: 0x00211532
	public override SpawnPointList GetSpawnPointList()
	{
		return this.dummySpawnPointList;
	}

	// Token: 0x0600569C RID: 22172 RVA: 0x0021333A File Offset: 0x0021153A
	public override void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
		this.dummySpawnPointList = _spawnPointList;
	}

	// Token: 0x04004315 RID: 17173
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPointList dummySpawnPointList = new SpawnPointList();
}
