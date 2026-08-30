using System;
using System.Collections.Generic;

namespace Webserver.LiveData
{
	// Token: 0x02001ADF RID: 6879
	public abstract class EntityFilterList<T> where T : Entity
	{
		// Token: 0x0600CE67 RID: 52839 RVA: 0x004B515C File Offset: 0x004B335C
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

		// Token: 0x0600CE68 RID: 52840 RVA: 0x004B51D4 File Offset: 0x004B33D4
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

		// Token: 0x0600CE69 RID: 52841
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract T predicate(Entity _e);

		// Token: 0x0600CE6A RID: 52842 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityFilterList()
		{
		}
	}
}
