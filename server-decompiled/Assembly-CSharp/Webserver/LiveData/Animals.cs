using System;

namespace Webserver.LiveData
{
	// Token: 0x02001B05 RID: 6917
	public class Animals : EntityFilterList<EntityAnimal>
	{
		// Token: 0x0600CFED RID: 53229 RVA: 0x004BB518 File Offset: 0x004B9718
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

		// Token: 0x04009DEE RID: 40430
		public static readonly Animals Instance = new Animals();
	}
}
