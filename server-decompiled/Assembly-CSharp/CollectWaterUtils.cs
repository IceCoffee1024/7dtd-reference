using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CCB RID: 3275
public static class CollectWaterUtils
{
	// Token: 0x060063F5 RID: 25589 RVA: 0x0027719C File Offset: 0x0027539C
	public static int CollectWater(ChunkCluster cc, int requiredMass, Vector3i origin, int maxRadius, List<CollectWaterUtils.WaterPoint> points)
	{
		int num = requiredMass;
		for (int i = 0; i <= maxRadius; i++)
		{
			int num2 = 0;
			int num3 = 0;
			foreach (Vector3i vector3i in CollectWaterUtils.GenerateCollectionPositions(origin, i))
			{
				WaterValue water = cc.GetWater(vector3i);
				if (water.HasMass())
				{
					int mass = water.GetMass();
					points.Add(new CollectWaterUtils.WaterPoint(vector3i, mass));
					num2 += mass;
					num3++;
				}
			}
			if (num2 > num)
			{
				int j = num2 - num;
				int num4 = (num2 - num) / num3;
				num4 = Mathf.Max(num4, 1);
				int num5 = points.Count - num3;
				while (j > 0)
				{
					CollectWaterUtils.WaterPoint waterPoint = points[num5];
					if (waterPoint.massToTake > 0)
					{
						int num6 = Mathf.Min(num4, waterPoint.massToTake);
						waterPoint.massToTake -= num6;
						j -= num6;
						num2 -= num6;
						points[num5] = waterPoint;
					}
					num5++;
					if (num5 == points.Count)
					{
						num5 = points.Count - num3;
					}
				}
			}
			num -= num2;
			if (num <= 0)
			{
				break;
			}
		}
		return requiredMass - num;
	}

	// Token: 0x060063F6 RID: 25590 RVA: 0x002772D8 File Offset: 0x002754D8
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateCollectionPositions(Vector3i center, int radius)
	{
		if (radius <= 0)
		{
			yield return center;
			yield break;
		}
		int num;
		for (int x = -radius; x <= radius; x = num + 1)
		{
			int remainingYZ = radius - Mathf.Abs(x);
			for (int y = -remainingYZ; y <= remainingYZ; y = num + 1)
			{
				int zAbs = remainingYZ - Mathf.Abs(y);
				if (zAbs == 0)
				{
					yield return new Vector3i(center.x + x, center.y + y, center.z);
				}
				else
				{
					yield return new Vector3i(center.x + x, center.y + y, center.z + zAbs);
					yield return new Vector3i(center.x + x, center.y + y, center.z - zAbs);
				}
				num = y;
			}
			num = x;
		}
		yield break;
	}

	// Token: 0x02000CCC RID: 3276
	public struct WaterPoint
	{
		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x060063F7 RID: 25591 RVA: 0x002772EF File Offset: 0x002754EF
		public int finalMass
		{
			get
			{
				return this.mass - this.massToTake;
			}
		}

		// Token: 0x060063F8 RID: 25592 RVA: 0x002772FE File Offset: 0x002754FE
		public WaterPoint(Vector3i _pos, int _mass)
		{
			this.worldPos = _pos;
			this.mass = _mass;
			this.massToTake = _mass;
		}

		// Token: 0x04004DFF RID: 19967
		public Vector3i worldPos;

		// Token: 0x04004E00 RID: 19968
		public int mass;

		// Token: 0x04004E01 RID: 19969
		public int massToTake;
	}
}
