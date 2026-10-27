using System;
using System.Collections.Generic;

// Token: 0x02000B04 RID: 2820
public class EntitySpawnerClassForDay
{
	// Token: 0x0600538C RID: 21388 RVA: 0x00201AC0 File Offset: 0x001FFCC0
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

	// Token: 0x0600538D RID: 21389 RVA: 0x00201B14 File Offset: 0x001FFD14
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

	// Token: 0x0600538E RID: 21390 RVA: 0x00201BF2 File Offset: 0x001FFDF2
	public int Count()
	{
		return this.days.Count;
	}

	// Token: 0x04004156 RID: 16726
	public bool bDynamicSpawner;

	// Token: 0x04004157 RID: 16727
	public bool bWrapDays;

	// Token: 0x04004158 RID: 16728
	public bool bClampDays;

	// Token: 0x04004159 RID: 16729
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntitySpawnerClass> days = new List<EntitySpawnerClass>();
}
