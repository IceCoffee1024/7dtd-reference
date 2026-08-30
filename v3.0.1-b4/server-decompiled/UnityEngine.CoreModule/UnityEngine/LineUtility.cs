using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020001C1 RID: 449
	[NativeHeader("Runtime/Export/Graphics/LineUtility.bindings.h")]
	public sealed class LineUtility
	{
		// Token: 0x06001038 RID: 4152 RVA: 0x00015E0C File Offset: 0x0001400C
		public static void Simplify(List<Vector3> points, float tolerance, List<int> pointsToKeep)
		{
			bool flag = points == null;
			if (flag)
			{
				throw new ArgumentNullException("points");
			}
			bool flag2 = pointsToKeep == null;
			if (flag2)
			{
				throw new ArgumentNullException("pointsToKeep");
			}
			LineUtility.GeneratePointsToKeep3D(points, tolerance, pointsToKeep);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x00015E4C File Offset: 0x0001404C
		public static void Simplify(List<Vector3> points, float tolerance, List<Vector3> simplifiedPoints)
		{
			bool flag = points == null;
			if (flag)
			{
				throw new ArgumentNullException("points");
			}
			bool flag2 = simplifiedPoints == null;
			if (flag2)
			{
				throw new ArgumentNullException("simplifiedPoints");
			}
			LineUtility.GenerateSimplifiedPoints3D(points, tolerance, simplifiedPoints);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00015E8C File Offset: 0x0001408C
		public static void Simplify(List<Vector2> points, float tolerance, List<int> pointsToKeep)
		{
			bool flag = points == null;
			if (flag)
			{
				throw new ArgumentNullException("points");
			}
			bool flag2 = pointsToKeep == null;
			if (flag2)
			{
				throw new ArgumentNullException("pointsToKeep");
			}
			LineUtility.GeneratePointsToKeep2D(points, tolerance, pointsToKeep);
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00015ECC File Offset: 0x000140CC
		public static void Simplify(List<Vector2> points, float tolerance, List<Vector2> simplifiedPoints)
		{
			bool flag = points == null;
			if (flag)
			{
				throw new ArgumentNullException("points");
			}
			bool flag2 = simplifiedPoints == null;
			if (flag2)
			{
				throw new ArgumentNullException("simplifiedPoints");
			}
			LineUtility.GenerateSimplifiedPoints2D(points, tolerance, simplifiedPoints);
		}

		// Token: 0x0600103C RID: 4156
		[FreeFunction("LineUtility_Bindings::GeneratePointsToKeep3D", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GeneratePointsToKeep3D(object pointsList, float tolerance, object pointsToKeepList);

		// Token: 0x0600103D RID: 4157
		[FreeFunction("LineUtility_Bindings::GeneratePointsToKeep2D", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GeneratePointsToKeep2D(object pointsList, float tolerance, object pointsToKeepList);

		// Token: 0x0600103E RID: 4158
		[FreeFunction("LineUtility_Bindings::GenerateSimplifiedPoints3D", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GenerateSimplifiedPoints3D(object pointsList, float tolerance, object simplifiedPoints);

		// Token: 0x0600103F RID: 4159
		[FreeFunction("LineUtility_Bindings::GenerateSimplifiedPoints2D", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void GenerateSimplifiedPoints2D(object pointsList, float tolerance, object simplifiedPoints);
	}
}
