using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000932 RID: 2354
public class DesignatedAreaStore<T> where T : class, IDesignatedArea
{
	// Token: 0x17000760 RID: 1888
	// (get) Token: 0x060043BC RID: 17340 RVA: 0x001A9260 File Offset: 0x001A7460
	public List<T> Areas
	{
		get
		{
			return this.areas;
		}
	}

	// Token: 0x17000761 RID: 1889
	// (get) Token: 0x060043BD RID: 17341 RVA: 0x001A9268 File Offset: 0x001A7468
	public int Count
	{
		get
		{
			return this.areas.Count;
		}
	}

	// Token: 0x060043BE RID: 17342 RVA: 0x001A9275 File Offset: 0x001A7475
	public void Clear()
	{
		this.areas.Clear();
		this.maxSizeX = int.MinValue;
	}

	// Token: 0x060043BF RID: 17343 RVA: 0x001A9290 File Offset: 0x001A7490
	public void Add(T _area)
	{
		this.maxSizeX = Utils.FastMax(this.maxSizeX, _area.AreaBounds.size.x);
		this.areas.Add(_area);
		this.areas.Sort(DesignatedAreaStore<T>.sortByMinX);
	}

	// Token: 0x060043C0 RID: 17344 RVA: 0x001A92E8 File Offset: 0x001A74E8
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

	// Token: 0x060043C1 RID: 17345 RVA: 0x001A9364 File Offset: 0x001A7564
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

	// Token: 0x060043C2 RID: 17346 RVA: 0x001A946C File Offset: 0x001A766C
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

	// Token: 0x060043C3 RID: 17347 RVA: 0x001A95D8 File Offset: 0x001A77D8
	public bool ContainsPoint(int _x, int _z, int _padding = 0)
	{
		return this.GetAt(_x, _z, _padding) != null;
	}

	// Token: 0x040036F0 RID: 14064
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<T> areas = new List<T>();

	// Token: 0x040036F1 RID: 14065
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxSizeX = int.MinValue;

	// Token: 0x040036F2 RID: 14066
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Comparison<T> sortByMinX = (T a, T b) => a.AreaBounds.xMin - b.AreaBounds.xMin;
}
