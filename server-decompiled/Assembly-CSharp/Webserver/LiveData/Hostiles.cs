using System;

namespace Webserver.LiveData
{
	// Token: 0x02001B64 RID: 7012
	public class Hostiles : EntityFilterList<EntityEnemy>
	{
		// Token: 0x0600D260 RID: 53856 RVA: 0x004C362C File Offset: 0x004C182C
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

		// Token: 0x04009F9E RID: 40862
		public static readonly Hostiles Instance = new Hostiles();
	}
}
