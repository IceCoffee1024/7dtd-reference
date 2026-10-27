using System;

namespace Webserver.LiveData
{
	// Token: 0x02001B62 RID: 7010
	public class Animals : EntityFilterList<EntityAnimal>
	{
		// Token: 0x0600D259 RID: 53849 RVA: 0x004C350C File Offset: 0x004C170C
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

		// Token: 0x04009F9D RID: 40861
		public static readonly Animals Instance = new Animals();
	}
}
