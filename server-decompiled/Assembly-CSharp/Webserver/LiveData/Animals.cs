using System;

namespace Webserver.LiveData
{
	// Token: 0x02001B31 RID: 6961
	public class Animals : EntityFilterList<EntityAnimal>
	{
		// Token: 0x0600D145 RID: 53573 RVA: 0x004BF9E4 File Offset: 0x004BDBE4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override EntityAnimal predicate(Entity _e)
		{
			EntityAnimal entityAnimal = _e as EntityAnimal;
			if (entityAnimal != null && entityAnimal.IsAlive())
			{
				return entityAnimal;
			}
			return null;
		}

		// Token: 0x04009EB4 RID: 40628
		public static readonly Animals Instance = new Animals();
	}
}
