using System;
using System.Collections.Generic;

namespace Webserver.LiveData
{
	// Token: 0x02001B63 RID: 7011
	public abstract class EntityFilterList<T> where T : Entity
	{
		// Token: 0x0600D25C RID: 53852 RVA: 0x004C3544 File Offset: 0x004C1744
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

		// Token: 0x0600D25D RID: 53853 RVA: 0x004C35BC File Offset: 0x004C17BC
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

		// Token: 0x0600D25E RID: 53854
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract T predicate(Entity _e);

		// Token: 0x0600D25F RID: 53855 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityFilterList()
		{
		}
	}
}
