using System;
using System.Collections.Generic;

// Token: 0x02000AD5 RID: 2773
public class EntitySpawnerClassForDay
{
	// Token: 0x060052C6 RID: 21190 RVA: 0x001FA66C File Offset: 0x001F886C
	public void AddForDay(int _day, EntitySpawnerClass _class)
	{
		if (_day != 0 && this.days.Count == 0)
		{
			this.days.Add(_class);
		}
		while (this.days.Count <= _day)
		{
			this.days.Add(null);
		}
		this.days[_day] = _class;
	}

	// Token: 0x060052C7 RID: 21191 RVA: 0x001FA6C0 File Offset: 0x001F88C0
	public EntitySpawnerClass Day(int _day)
	{
		if (this.days.Count == 0)
		{
			return null;
		}
		if (this.bWrapDays && _day > 0 && _day >= this.days.Count)
		{
			if (this.days.Count > 1)
			{
				_day %= this.days.Count - 1;
				if (_day == 0)
				{
					_day = this.days.Count - 1;
				}
			}
			else
			{
				_day = 1;
			}
			if (_day == 0)
			{
				_day++;
			}
		}
		else if (this.bClampDays && _day >= this.days.Count && this.days.Count > 0)
		{
			_day = this.days.Count - 1;
		}
		if (_day >= this.days.Count || this.days[_day] == null)
		{
			return this.days[0];
		}
		return this.days[_day];
	}

	// Token: 0x060052C8 RID: 21192 RVA: 0x001FA79E File Offset: 0x001F899E
	public int Count()
	{
		return this.days.Count;
	}

	// Token: 0x0400405E RID: 16478
	public bool bDynamicSpawner;

	// Token: 0x0400405F RID: 16479
	public bool bWrapDays;

	// Token: 0x04004060 RID: 16480
	public bool bClampDays;

	// Token: 0x04004061 RID: 16481
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntitySpawnerClass> days = new List<EntitySpawnerClass>();
}
