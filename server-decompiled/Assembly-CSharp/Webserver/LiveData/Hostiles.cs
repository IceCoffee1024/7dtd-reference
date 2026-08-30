using System;

namespace Webserver.LiveData
{
	// Token: 0x02001B07 RID: 6919
	public class Hostiles : EntityFilterList<EntityEnemy>
	{
		// Token: 0x0600CFF4 RID: 53236 RVA: 0x004BB638 File Offset: 0x004B9838
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

		// Token: 0x04009DEF RID: 40431
		public static readonly Hostiles Instance = new Hostiles();
	}
}
