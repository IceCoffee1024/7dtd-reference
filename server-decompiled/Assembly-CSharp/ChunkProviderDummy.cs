using System;
using System.Collections;

// Token: 0x02000B19 RID: 2841
public class ChunkProviderDummy : ChunkProviderAbstract
{
	// Token: 0x0600560E RID: 22030 RVA: 0x002105EF File Offset: 0x0020E7EF
	public override IEnumerator Init(World _worldData)
	{
		MultiBlockManager.Instance.Initialize(null);
		yield return null;
		yield break;
	}

	// Token: 0x0600560F RID: 22031 RVA: 0x002105F7 File Offset: 0x0020E7F7
	public override void Cleanup()
	{
		base.Cleanup();
		MultiBlockManager.Instance.Cleanup();
	}

	// Token: 0x06005610 RID: 22032 RVA: 0x00046C7A File Offset: 0x00044E7A
	public override EnumChunkProviderId GetProviderId()
	{
		return EnumChunkProviderId.NetworkClient;
	}

	// Token: 0x06005611 RID: 22033 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void setChunkPrerequisits(string sSeed)
	{
	}

	// Token: 0x06005612 RID: 22034 RVA: 0x00210609 File Offset: 0x0020E809
	public override void UnloadChunk(Chunk _c)
	{
		MemoryPools.PoolChunks.FreeSync(_c);
	}

	// Token: 0x06005613 RID: 22035 RVA: 0x00210616 File Offset: 0x0020E816
	public override SpawnPointList GetSpawnPointList()
	{
		return this.dummySpawnPointList;
	}

	// Token: 0x06005614 RID: 22036 RVA: 0x0021061E File Offset: 0x0020E81E
	public override void SetSpawnPointList(SpawnPointList _spawnPointList)
	{
		this.dummySpawnPointList = _spawnPointList;
	}

	// Token: 0x0400427A RID: 17018
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPointList dummySpawnPointList = new SpawnPointList();
}
