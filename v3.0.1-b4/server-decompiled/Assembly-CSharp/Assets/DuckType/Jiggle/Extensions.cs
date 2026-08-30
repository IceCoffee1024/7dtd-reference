using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DuckType.Jiggle
{
	// Token: 0x02001D35 RID: 7477
	public static class Extensions
	{
		// Token: 0x0600DCA3 RID: 56483 RVA: 0x004F25E0 File Offset: 0x004F07E0
		public static Quaternion Append(this Quaternion source, Quaternion quaternion)
		{
			return quaternion * source;
		}

		// Token: 0x0600DCA4 RID: 56484 RVA: 0x004F25E9 File Offset: 0x004F07E9
		public static Quaternion FromToRotation(this Quaternion source, Quaternion target)
		{
			return Quaternion.Inverse(source) * target;
		}

		// Token: 0x0600DCA5 RID: 56485 RVA: 0x004F25F7 File Offset: 0x004F07F7
		public static Quaternion Scale(this Quaternion source, float scale)
		{
			return Quaternion.SlerpUnclamped(Quaternion.identity, source, scale);
		}

		// Token: 0x0600DCA6 RID: 56486 RVA: 0x004F2605 File Offset: 0x004F0805
		public static Quaternion Inverse(this Quaternion source)
		{
			return Quaternion.Inverse(source);
		}

		// Token: 0x0600DCA7 RID: 56487 RVA: 0x004F2610 File Offset: 0x004F0810
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

		// Token: 0x0600DCA8 RID: 56488 RVA: 0x004F2688 File Offset: 0x004F0888
		public static bool HasLength(this Vector3 source)
		{
			return source.x != 0f || source.y != 0f || source.z != 0f;
		}

		// Token: 0x0600DCA9 RID: 56489 RVA: 0x004F26B6 File Offset: 0x004F08B6
		public static float Clamp01(this float source)
		{
			return Mathf.Max(Mathf.Min(source, 1f), 0f);
		}
	}
}
