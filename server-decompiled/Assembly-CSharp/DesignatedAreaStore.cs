using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000923 RID: 2339
public class DesignatedAreaStore<T> where T : class, IDesignatedArea
{
	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x06004407 RID: 17415 RVA: 0x001A80D8 File Offset: 0x001A62D8
	public List<T> Areas
	{
		get
		{
			return this.areas;
		}
	}

	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x06004408 RID: 17416 RVA: 0x001A80E0 File Offset: 0x001A62E0
	public int Count
	{
		get
		{
			return this.areas.Count;
		}
	}

	// Token: 0x06004409 RID: 17417 RVA: 0x001A80ED File Offset: 0x001A62ED
	public void Clear()
	{
		this.areas.Clear();
		this.maxSizeX = int.MinValue;
	}

	// Token: 0x0600440A RID: 17418 RVA: 0x001A8108 File Offset: 0x001A6308
	public void Add(T _area)
	{
		this.maxSizeX = Utils.FastMax(this.maxSizeX, _area.AreaBounds.size.x);
		this.areas.Add(_area);
		this.areas.Sort(DesignatedAreaStore<T>.sortByMinX);
	}

	// Token: 0x0600440B RID: 17419 RVA: 0x001A8160 File Offset: 0x001A6360
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

	// Token: 0x0600440C RID: 17420 RVA: 0x001A81DC File Offset: 0x001A63DC
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

	// Token: 0x0600440D RID: 17421 RVA: 0x001A82E4 File Offset: 0x001A64E4
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

	// Token: 0x0600440E RID: 17422 RVA: 0x001A8450 File Offset: 0x001A6650
	public bool ContainsPoint(int _x, int _z, int _padding = 0)
	{
		return this.GetAt(_x, _z, _padding) != null;
	}

	// Token: 0x040036DC RID: 14044
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<T> areas = new List<T>();

	// Token: 0x040036DD RID: 14045
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxSizeX = int.MinValue;

	// Token: 0x040036DE RID: 14046
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Comparison<T> sortByMinX = (T a, T b) => a.AreaBounds.xMin - b.AreaBounds.xMin;
}
