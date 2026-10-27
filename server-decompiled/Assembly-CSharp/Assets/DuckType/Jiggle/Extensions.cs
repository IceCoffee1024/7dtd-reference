using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001DC0 RID: 7616
	public static class Extensions
	{
		// Token: 0x0600E0D1 RID: 57553 RVA: 0x005017A8 File Offset: 0x004FF9A8
		public static Quaternion Append(this Quaternion source, Quaternion quaternion)
		{
			return quaternion * source;
		}

		// Token: 0x0600E0D2 RID: 57554 RVA: 0x005017B1 File Offset: 0x004FF9B1
		public static Quaternion FromToRotation(this Quaternion source, Quaternion target)
		{
			return Quaternion.Inverse(source) * target;
		}

		// Token: 0x0600E0D3 RID: 57555 RVA: 0x005017BF File Offset: 0x004FF9BF
		public static Quaternion Scale(this Quaternion source, float scale)
		{
			return Quaternion.SlerpUnclamped(Quaternion.identity, source, scale);
		}

		// Token: 0x0600E0D4 RID: 57556 RVA: 0x005017CD File Offset: 0x004FF9CD
		public static Quaternion Inverse(this Quaternion source)
		{
			return Quaternion.Inverse(source);
		}

		// Token: 0x0600E0D5 RID: 57557 RVA: 0x005017D8 File Offset: 0x004FF9D8
		public static List<Vector3> GetOrthogonalVectors(this Vector3 source, int numVectors)
		{
			Vector3 normalized = source.normalized;
			Vector3 vector = ((Mathf.Abs(source.normalized.y) != 1f) ? Vector3.Cross(source, Vector3.up) : Vector3.Cross(source, Vector3.right));
			float num = 360f / (float)numVectors;
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < numVectors; i++)
			{
				list.Add(Quaternion.AngleAxis(num * (float)i, source) * vector);
			}
			return list;
		}

		// Token: 0x0600E0D6 RID: 57558 RVA: 0x00501850 File Offset: 0x004FFA50
		public static bool HasLength(this Vector3 source)
		{
			return source.x != 0f || source.y != 0f || source.z != 0f;
		}

		// Token: 0x0600E0D7 RID: 57559 RVA: 0x0050187E File Offset: 0x004FFA7E
		public static float Clamp01(this float source)
		{
			return Mathf.Max(Mathf.Min(source, 1f), 0f);
		}
	}
}
