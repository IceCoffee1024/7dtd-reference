using System;
using System.Collections.Generic;

namespace Webserver.LiveData
{
	// Token: 0x02001B32 RID: 6962
	public abstract class EntityFilterList<T> where T : Entity
	{
		// Token: 0x0600D148 RID: 53576 RVA: 0x004BFA1C File Offset: 0x004BDC1C
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

		// Token: 0x0600D149 RID: 53577 RVA: 0x004BFA94 File Offset: 0x004BDC94
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

		// Token: 0x0600D14A RID: 53578
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract T predicate(Entity _e);

		// Token: 0x0600D14B RID: 53579 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityFilterList()
		{
		}
	}
}
