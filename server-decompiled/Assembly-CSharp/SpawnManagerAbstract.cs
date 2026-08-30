using System;

// Token: 0x02000ADD RID: 2781
public abstract class SpawnManagerAbstract
{
	// Token: 0x060052EC RID: 21228 RVA: 0x001FB19E File Offset: 0x001F939E
	public SpawnManagerAbstract(World _world)
	{
		this.world = _world;
	}

	// Token: 0x060052ED RID: 21229
	public abstract void Update(string _spawnerName, bool _bSpawnEnemyEntities, object _userData);

	// Token: 0x040040A2 RID: 16546
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;
}
