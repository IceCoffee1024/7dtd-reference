using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D5D RID: 7517
	public static class Extensions
	{
		// Token: 0x0600DE44 RID: 56900 RVA: 0x004F9054 File Offset: 0x004F7254
		public static Quaternion Append(this Quaternion source, Quaternion quaternion)
		{
			return quaternion * source;
		}

		// Token: 0x0600DE45 RID: 56901 RVA: 0x004F905D File Offset: 0x004F725D
		public static Quaternion FromToRotation(this Quaternion source, Quaternion target)
		{
			return Quaternion.Inverse(source) * target;
		}

		// Token: 0x0600DE46 RID: 56902 RVA: 0x004F906B File Offset: 0x004F726B
		public static Quaternion Scale(this Quaternion source, float scale)
		{
			return Quaternion.SlerpUnclamped(Quaternion.identity, source, scale);
		}

		// Token: 0x0600DE47 RID: 56903 RVA: 0x004F9079 File Offset: 0x004F7279
		public static Quaternion Inverse(this Quaternion source)
		{
			return Quaternion.Inverse(source);
		}

		// Token: 0x0600DE48 RID: 56904 RVA: 0x004F9084 File Offset: 0x004F7284
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

		// Token: 0x0600DE49 RID: 56905 RVA: 0x004F90FC File Offset: 0x004F72FC
		public static bool HasLength(this Vector3 source)
		{
			return source.x != 0f || source.y != 0f || source.z != 0f;
		}

		// Token: 0x0600DE4A RID: 56906 RVA: 0x004F912A File Offset: 0x004F732A
		public static float Clamp01(this float source)
		{
			return Mathf.Max(Mathf.Min(source, 1f), 0f);
		}
	}
}
