using System;

namespace Webserver.LiveData
{
	// Token: 0x02001B33 RID: 6963
	public class Hostiles : EntityFilterList<EntityEnemy>
	{
		// Token: 0x0600D14C RID: 53580 RVA: 0x004BFB04 File Offset: 0x004BDD04
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

		// Token: 0x04009EB5 RID: 40629
		public static readonly Hostiles Instance = new Hostiles();
	}
}
