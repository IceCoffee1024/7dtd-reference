using System;

// Token: 0x02000AEF RID: 2799
public abstract class SpawnManagerAbstract
{
	// Token: 0x06005374 RID: 21364 RVA: 0x001FDE0E File Offset: 0x001FC00E
	public SpawnManagerAbstract(World _world)
	{
		this.world = _world;
	}

	// Token: 0x06005375 RID: 21365
	public abstract void Update(string _spawnerName, bool _bSpawnEnemyEntities, object _userData);

	// Token: 0x04004138 RID: 16696
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;
}
