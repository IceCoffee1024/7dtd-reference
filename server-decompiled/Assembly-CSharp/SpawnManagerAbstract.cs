using System;

// Token: 0x02000B01 RID: 2817
public abstract class SpawnManagerAbstract
{
	// Token: 0x0600538D RID: 21389 RVA: 0x0020106E File Offset: 0x001FF26E
	public SpawnManagerAbstract(World _world)
	{
		this.world = _world;
	}

	// Token: 0x0600538E RID: 21390
	public abstract void Update(string _spawnerName, bool _bSpawnEnemyEntities, object _userData);

	// Token: 0x04004175 RID: 16757
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;
}
