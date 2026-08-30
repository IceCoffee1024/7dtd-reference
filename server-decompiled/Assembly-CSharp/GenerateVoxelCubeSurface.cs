using System;
using System.Collections.Generic;

// Token: 0x02000CB5 RID: 3253
public class GenerateVoxelCubeSurface
{
	// Token: 0x060063E9 RID: 25577 RVA: 0x00272C5F File Offset: 0x00270E5F
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

	// Token: 0x060063EA RID: 25578 RVA: 0x00272C76 File Offset: 0x00270E76
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

	// Token: 0x060063EB RID: 25579 RVA: 0x00272C8D File Offset: 0x00270E8D
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

	// Token: 0x060063EC RID: 25580 RVA: 0x00272CA4 File Offset: 0x00270EA4
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

	// Token: 0x060063ED RID: 25581 RVA: 0x00272CBB File Offset: 0x00270EBB
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

	// Token: 0x060063EE RID: 25582 RVA: 0x00272CD2 File Offset: 0x00270ED2
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

	// Token: 0x060063EF RID: 25583 RVA: 0x00272CE9 File Offset: 0x00270EE9
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

	// Token: 0x060063F0 RID: 25584 RVA: 0x00272D00 File Offset: 0x00270F00
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

	// Token: 0x060063F1 RID: 25585 RVA: 0x00272D1E File Offset: 0x00270F1E
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

	// Token: 0x060063F2 RID: 25586 RVA: 0x00272D3C File Offset: 0x00270F3C
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
