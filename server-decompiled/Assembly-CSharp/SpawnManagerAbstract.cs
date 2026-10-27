using System;

// Token: 0x02000B0C RID: 2828
public abstract class SpawnManagerAbstract
{
	// Token: 0x060053B2 RID: 21426 RVA: 0x002025F2 File Offset: 0x002007F2
	public SpawnManagerAbstract(World _world)
	{
		this.world = _world;
	}

	// Token: 0x060053B3 RID: 21427
	public abstract void Update(string _spawnerName, bool _bSpawnEnemyEntities, object _userData);

	// Token: 0x0400419A RID: 16794
	[PublicizedFrom(EAccessModifier.Protected)]
	public World world;
}
