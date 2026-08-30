using System;
using System.Collections.Generic;

namespace Webserver.LiveData
{
	// Token: 0x02001B06 RID: 6918
	public abstract class EntityFilterList<T> where T : Entity
	{
		// Token: 0x0600CFF0 RID: 53232 RVA: 0x004BB550 File Offset: 0x004B9750
		public void Get(List<T> _list)
		{
			_list.Clear();
			try
			{
				List<Entity> list = GameManager.Instance.World.Entities.list;
				for (int i = 0; i < list.Count; i++)
				{
					Entity entity = list[i];
					T t = this.predicate(entity);
					if (t != null)
					{
						_list.Add(t);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		// Token: 0x0600CFF1 RID: 53233 RVA: 0x004BB5C8 File Offset: 0x004B97C8
		public int GetCount()
		{
			int num = 0;
			try
			{
				List<Entity> list = GameManager.Instance.World.Entities.list;
				for (int i = 0; i < list.Count; i++)
				{
					Entity entity = list[i];
					if (this.predicate(entity) != null)
					{
						num++;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
			return num;
		}

		// Token: 0x0600CFF2 RID: 53234
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract T predicate(Entity _e);

		// Token: 0x0600CFF3 RID: 53235 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityFilterList()
		{
		}
	}
}
