using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D8F RID: 7567
	public static class Extensions
	{
		// Token: 0x0600DFBD RID: 57277 RVA: 0x004FDC10 File Offset: 0x004FBE10
		public static Quaternion Append(this Quaternion source, Quaternion quaternion)
		{
			return quaternion * source;
		}

		// Token: 0x0600DFBE RID: 57278 RVA: 0x004FDC19 File Offset: 0x004FBE19
		public static Quaternion FromToRotation(this Quaternion source, Quaternion target)
		{
			return Quaternion.Inverse(source) * target;
		}

		// Token: 0x0600DFBF RID: 57279 RVA: 0x004FDC27 File Offset: 0x004FBE27
		public static Quaternion Scale(this Quaternion source, float scale)
		{
			return Quaternion.SlerpUnclamped(Quaternion.identity, source, scale);
		}

		// Token: 0x0600DFC0 RID: 57280 RVA: 0x004FDC35 File Offset: 0x004FBE35
		public static Quaternion Inverse(this Quaternion source)
		{
			return Quaternion.Inverse(source);
		}

		// Token: 0x0600DFC1 RID: 57281 RVA: 0x004FDC40 File Offset: 0x004FBE40
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

		// Token: 0x0600DFC2 RID: 57282 RVA: 0x004FDCB8 File Offset: 0x004FBEB8
		public static bool HasLength(this Vector3 source)
		{
			return source.x != 0f || source.y != 0f || source.z != 0f;
		}

		// Token: 0x0600DFC3 RID: 57283 RVA: 0x004FDCE6 File Offset: 0x004FBEE6
		public static float Clamp01(this float source)
		{
			return Mathf.Max(Mathf.Min(source, 1f), 0f);
		}
	}
}
