using System;

namespace Webserver.LiveData
{
	// Token: 0x02001AE0 RID: 6880
	public class Hostiles : EntityFilterList<EntityEnemy>
	{
		// Token: 0x0600CE6B RID: 52843 RVA: 0x004B5244 File Offset: 0x004B3444
		[PublicizedFrom(EAccessModifier.Protected)]
		public override EntityEnemy predicate(Entity _e)
		{
			EntityEnemy entityEnemy = _e as EntityEnemy;
			if (entityEnemy != null && entityEnemy.IsAlive())
			{
				return entityEnemy;
			}
			return null;
		}

		// Token: 0x04009CBE RID: 40126
		public static readonly Hostiles Instance = new Hostiles();
	}
}
