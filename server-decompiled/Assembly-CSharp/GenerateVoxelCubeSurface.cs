using System;
using System.Collections.Generic;

// Token: 0x02000CA3 RID: 3235
public class GenerateVoxelCubeSurface
{
	// Token: 0x0600635D RID: 25437 RVA: 0x0026FD63 File Offset: 0x0026DF63
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

	// Token: 0x0600635E RID: 25438 RVA: 0x0026FD7A File Offset: 0x0026DF7A
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

	// Token: 0x0600635F RID: 25439 RVA: 0x0026FD91 File Offset: 0x0026DF91
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

	// Token: 0x06006360 RID: 25440 RVA: 0x0026FDA8 File Offset: 0x0026DFA8
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

	// Token: 0x06006361 RID: 25441 RVA: 0x0026FDBF File Offset: 0x0026DFBF
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

	// Token: 0x06006362 RID: 25442 RVA: 0x0026FDD6 File Offset: 0x0026DFD6
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

	// Token: 0x06006363 RID: 25443 RVA: 0x0026FDED File Offset: 0x0026DFED
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

	// Token: 0x06006364 RID: 25444 RVA: 0x0026FE04 File Offset: 0x0026E004
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

	// Token: 0x06006365 RID: 25445 RVA: 0x0026FE22 File Offset: 0x0026E022
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

	// Token: 0x06006366 RID: 25446 RVA: 0x0026FE40 File Offset: 0x0026E040
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
