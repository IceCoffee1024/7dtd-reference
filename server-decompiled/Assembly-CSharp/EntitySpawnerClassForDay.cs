using System;
using System.Collections.Generic;

// Token: 0x02000AF9 RID: 2809
public class EntitySpawnerClassForDay
{
	// Token: 0x06005367 RID: 21351 RVA: 0x0020053C File Offset: 0x001FE73C
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

	// Token: 0x06005368 RID: 21352 RVA: 0x00200590 File Offset: 0x001FE790
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

	// Token: 0x06005369 RID: 21353 RVA: 0x0020066E File Offset: 0x001FE86E
	public int Count()
	{
		return this.days.Count;
	}

	// Token: 0x04004131 RID: 16689
	public bool bDynamicSpawner;

	// Token: 0x04004132 RID: 16690
	public bool bWrapDays;

	// Token: 0x04004133 RID: 16691
	public bool bClampDays;

	// Token: 0x04004134 RID: 16692
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntitySpawnerClass> days = new List<EntitySpawnerClass>();
}
