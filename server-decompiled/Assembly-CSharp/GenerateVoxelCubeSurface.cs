using System;
using System.Collections.Generic;

// Token: 0x02000CCE RID: 3278
public class GenerateVoxelCubeSurface
{
	// Token: 0x06006401 RID: 25601 RVA: 0x0027758B File Offset: 0x0027578B
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

	// Token: 0x06006402 RID: 25602 RVA: 0x002775A2 File Offset: 0x002757A2
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

	// Token: 0x06006403 RID: 25603 RVA: 0x002775B9 File Offset: 0x002757B9
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

	// Token: 0x06006404 RID: 25604 RVA: 0x002775D0 File Offset: 0x002757D0
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

	// Token: 0x06006405 RID: 25605 RVA: 0x002775E7 File Offset: 0x002757E7
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

	// Token: 0x06006406 RID: 25606 RVA: 0x002775FE File Offset: 0x002757FE
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

	// Token: 0x06006407 RID: 25607 RVA: 0x00277615 File Offset: 0x00275815
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

	// Token: 0x06006408 RID: 25608 RVA: 0x0027762C File Offset: 0x0027582C
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

	// Token: 0x06006409 RID: 25609 RVA: 0x0027764A File Offset: 0x0027584A
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

	// Token: 0x0600640A RID: 25610 RVA: 0x00277668 File Offset: 0x00275868
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
