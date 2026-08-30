using System;

namespace Webserver.LiveData
{
	// Token: 0x02001ADE RID: 6878
	public class Animals : EntityFilterList<EntityAnimal>
	{
		// Token: 0x0600CE64 RID: 52836 RVA: 0x004B5124 File Offset: 0x004B3324
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

		// Token: 0x04009CBD RID: 40125
		public static readonly Animals Instance = new Animals();
	}
}
