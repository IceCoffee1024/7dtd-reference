using System;
using System.Collections.Generic;

// Token: 0x02000CC3 RID: 3267
public class GenerateVoxelCubeSurface
{
	// Token: 0x060063DC RID: 25564 RVA: 0x00276063 File Offset: 0x00274263
	public static IEnumerable<Vector3i> GenerateCubeSurfacePositions(Vector3i origin, int radius)
	{
		if (radius <= 0)
		{
			yield return origin;
			yield break;
		}
		foreach (Vector3i vector3i in GenerateVoxelCubeSurface.GenerateCubeSurfaceTop(origin, radius))
		{
			yield return vector3i;
		}
		IEnumerator<Vector3i> enumerator = null;
		foreach (Vector3i vector3i2 in GenerateVoxelCubeSurface.GenerateCubeSurfaceBottom(origin, radius))
		{
			yield return vector3i2;
		}
		enumerator = null;
		foreach (Vector3i vector3i3 in GenerateVoxelCubeSurface.GenerateCubeSurfaceLeft(origin, radius))
		{
			yield return vector3i3;
		}
		enumerator = null;
		foreach (Vector3i vector3i4 in GenerateVoxelCubeSurface.GenerateCubeSurfaceRight(origin, radius))
		{
			yield return vector3i4;
		}
		enumerator = null;
		foreach (Vector3i vector3i5 in GenerateVoxelCubeSurface.GenerateCubeSurfaceFront(origin, radius))
		{
			yield return vector3i5;
		}
		enumerator = null;
		foreach (Vector3i vector3i6 in GenerateVoxelCubeSurface.GenerateCubeSurfaceBack(origin, radius))
		{
			yield return vector3i6;
		}
		enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060063DD RID: 25565 RVA: 0x0027607A File Offset: 0x0027427A
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateCubeSurfaceTop(Vector3i origin, int radius)
	{
		origin.x -= radius;
		origin.y += radius;
		origin.z -= radius;
		int num = radius * 2 + 1;
		foreach (Vector3i vector3i in GenerateVoxelCubeSurface.GenerateXZ(origin, num, num))
		{
			yield return vector3i;
		}
		IEnumerator<Vector3i> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060063DE RID: 25566 RVA: 0x00276091 File Offset: 0x00274291
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateCubeSurfaceBottom(Vector3i origin, int radius)
	{
		origin.x -= radius;
		origin.y -= radius;
		origin.z -= radius;
		int num = radius * 2 + 1;
		foreach (Vector3i vector3i in GenerateVoxelCubeSurface.GenerateXZ(origin, num, num))
		{
			yield return vector3i;
		}
		IEnumerator<Vector3i> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060063DF RID: 25567 RVA: 0x002760A8 File Offset: 0x002742A8
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateCubeSurfaceRight(Vector3i origin, int radius)
	{
		origin.x += radius;
		origin.y -= radius - 1;
		origin.z -= radius;
		int num = radius * 2 + 1;
		foreach (Vector3i vector3i in GenerateVoxelCubeSurface.GenerateYZ(origin, num - 2, num))
		{
			yield return vector3i;
		}
		IEnumerator<Vector3i> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060063E0 RID: 25568 RVA: 0x002760BF File Offset: 0x002742BF
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateCubeSurfaceLeft(Vector3i origin, int radius)
	{
		origin.x -= radius;
		origin.y -= radius - 1;
		origin.z -= radius;
		int num = radius * 2 + 1;
		foreach (Vector3i vector3i in GenerateVoxelCubeSurface.GenerateYZ(origin, num - 2, num))
		{
			yield return vector3i;
		}
		IEnumerator<Vector3i> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060063E1 RID: 25569 RVA: 0x002760D6 File Offset: 0x002742D6
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateCubeSurfaceFront(Vector3i origin, int radius)
	{
		origin.x -= radius - 1;
		origin.y -= radius - 1;
		origin.z += radius;
		int num = radius * 2 - 1;
		foreach (Vector3i vector3i in GenerateVoxelCubeSurface.GenerateXY(origin, num, num))
		{
			yield return vector3i;
		}
		IEnumerator<Vector3i> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060063E2 RID: 25570 RVA: 0x002760ED File Offset: 0x002742ED
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateCubeSurfaceBack(Vector3i origin, int radius)
	{
		origin.x -= radius - 1;
		origin.y -= radius - 1;
		origin.z -= radius;
		int num = radius * 2 - 1;
		foreach (Vector3i vector3i in GenerateVoxelCubeSurface.GenerateXY(origin, num, num))
		{
			yield return vector3i;
		}
		IEnumerator<Vector3i> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x060063E3 RID: 25571 RVA: 0x00276104 File Offset: 0x00274304
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateXZ(Vector3i min, int xLength, int zLength)
	{
		if (min.y < 0 || min.y > 255)
		{
			yield break;
		}
		int xEnd = min.x + xLength;
		int zEnd = min.z + zLength;
		int num;
		for (int x = min.x; x < xEnd; x = num + 1)
		{
			for (int z = min.z; z < zEnd; z = num + 1)
			{
				yield return new Vector3i(x, min.y, z);
				num = z;
			}
			num = x;
		}
		yield break;
	}

	// Token: 0x060063E4 RID: 25572 RVA: 0x00276122 File Offset: 0x00274322
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateYZ(Vector3i min, int yLength, int zLength)
	{
		int yEnd = min.y + yLength;
		int zEnd = min.z + zLength;
		int num;
		for (int y = min.y; y < yEnd; y = num + 1)
		{
			if (y >= 0 && y < 256)
			{
				for (int z = min.z; z < zEnd; z = num + 1)
				{
					yield return new Vector3i(min.x, y, z);
					num = z;
				}
			}
			num = y;
		}
		yield break;
	}

	// Token: 0x060063E5 RID: 25573 RVA: 0x00276140 File Offset: 0x00274340
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerable<Vector3i> GenerateXY(Vector3i min, int xLength, int yLength)
	{
		int xEnd = min.x + xLength;
		int yEnd = min.y + yLength;
		int num;
		for (int y = min.y; y < yEnd; y = num + 1)
		{
			if (y >= 0 && y < 256)
			{
				for (int x = min.x; x < xEnd; x = num + 1)
				{
					yield return new Vector3i(x, y, min.z);
					num = x;
				}
			}
			num = y;
		}
		yield break;
	}
}
