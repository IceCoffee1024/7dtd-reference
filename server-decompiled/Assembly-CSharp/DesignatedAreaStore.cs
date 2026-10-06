using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000928 RID: 2344
public class DesignatedAreaStore<T> where T : class, IDesignatedArea
{
	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x06004399 RID: 17305 RVA: 0x001A81DC File Offset: 0x001A63DC
	public List<T> Areas
	{
		get
		{
			return this.areas;
		}
	}

	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x0600439A RID: 17306 RVA: 0x001A81E4 File Offset: 0x001A63E4
	public int Count
	{
		get
		{
			return this.areas.Count;
		}
	}

	// Token: 0x0600439B RID: 17307 RVA: 0x001A81F1 File Offset: 0x001A63F1
	public void Clear()
	{
		this.areas.Clear();
		this.maxSizeX = int.MinValue;
	}

	// Token: 0x0600439C RID: 17308 RVA: 0x001A820C File Offset: 0x001A640C
	public void Add(T _area)
	{
		this.maxSizeX = Utils.FastMax(this.maxSizeX, _area.AreaBounds.size.x);
		this.areas.Add(_area);
		this.areas.Sort(DesignatedAreaStore<T>.sortByMinX);
	}

	// Token: 0x0600439D RID: 17309 RVA: 0x001A8264 File Offset: 0x001A6464
	public bool OverlapsBounds(Vector3i _min, Vector3i _max)
	{
		for (int i = 0; i < this.areas.Count; i++)
		{
			BoundsInt areaBounds = this.areas[i].AreaBounds;
			if (_max.x >= areaBounds.xMin && _min.x < areaBounds.xMax && _max.z >= areaBounds.zMin && _min.z < areaBounds.zMax)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600439E RID: 17310 RVA: 0x001A82E0 File Offset: 0x001A64E0
	public T GetAt(int _x, int _z, int _padding = 0)
	{
		T t = default(T);
		int num = _x - this.maxSizeX - _padding;
		int i = 0;
		int num2 = this.areas.Count;
		while (i < num2)
		{
			int num3 = (i + num2) / 2;
			if (this.areas[num3].AreaBounds.xMin < num)
			{
				i = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		for (int j = i; j < this.areas.Count; j++)
		{
			T t2 = this.areas[j];
			if (t2.AreaBounds.xMin > _x + _padding)
			{
				break;
			}
			if (_x <= t2.AreaBounds.xMax + _padding && _z >= t2.AreaBounds.zMin - _padding && _z <= t2.AreaBounds.zMax + _padding)
			{
				t = t2;
				break;
			}
		}
		return t;
	}

	// Token: 0x0600439F RID: 17311 RVA: 0x001A83E8 File Offset: 0x001A65E8
	public T GetOuterAt(int _x, int _z, int _depth = 0)
	{
		T t = default(T);
		int num = _x - this.maxSizeX;
		int i = 0;
		int num2 = this.areas.Count;
		while (i < num2)
		{
			int num3 = (i + num2) / 2;
			if (this.areas[num3].AreaBounds.xMin < num)
			{
				i = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		for (int j = i; j < this.areas.Count; j++)
		{
			T t2 = this.areas[j];
			if (t2.AreaBounds.xMin > _x)
			{
				break;
			}
			if (_x <= t2.AreaBounds.xMax && _z >= t2.AreaBounds.zMin && _z <= t2.AreaBounds.zMax && (_x <= t2.AreaBounds.xMin + _depth || _x >= t2.AreaBounds.xMax - _depth || _z <= t2.AreaBounds.zMin + _depth || _z >= t2.AreaBounds.zMax - _depth))
			{
				t = t2;
				break;
			}
		}
		return t;
	}

	// Token: 0x060043A0 RID: 17312 RVA: 0x001A8554 File Offset: 0x001A6754
	public bool ContainsPoint(int _x, int _z, int _padding = 0)
	{
		return this.GetAt(_x, _z, _padding) != null;
	}

	// Token: 0x040036D0 RID: 14032
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<T> areas = new List<T>();

	// Token: 0x040036D1 RID: 14033
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxSizeX = int.MinValue;

	// Token: 0x040036D2 RID: 14034
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Comparison<T> sortByMinX = (T a, T b) => a.AreaBounds.xMin - b.AreaBounds.xMin;
}
