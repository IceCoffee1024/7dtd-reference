using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CEF RID: 3311
public class FlatAreaManager
{
	// Token: 0x0600655D RID: 25949 RVA: 0x002793EC File Offset: 0x002775EC
	public IEnumerator DefineFlatAreas(World world)
	{
		this.areaList16x16.Clear();
		this.areaList32x32.Clear();
		Vector2i worldSize = world.ChunkCache.ChunkProvider.GetWorldSize();
		Vector2i worldSizeHalf = worldSize / 2;
		MicroStopwatch ms = new MicroStopwatch();
		ms.Start();
		Log.Out("Begin Defining Flat Areas");
		ChunkProviderGenerateWorldFromRaw chunkProviderRaw = world.ChunkCache.ChunkProvider as ChunkProviderGenerateWorldFromRaw;
		long yieldTimeStamp = ms.ElapsedMilliseconds;
		Dictionary<Vector3i, int> candidateSpots = new Dictionary<Vector3i, int>();
		int margin = (int)Mathf.Ceil(8.125f) * 16;
		for (int z = -worldSizeHalf.y + margin; z < worldSizeHalf.y - margin; z += 16)
		{
			for (int i = -worldSizeHalf.x + margin; i < worldSizeHalf.x - margin; i += 16)
			{
				float height = chunkProviderRaw.GetHeight(i + worldSizeHalf.x, z + worldSizeHalf.y);
				Vector3i vector3i = new Vector3i(i, (int)height, z);
				int num = 16;
				bool flag = true;
				int num2 = i;
				int num3 = i + 16;
				int num4 = z;
				int num5 = z + 16;
				for (int j = num4; j < num5; j++)
				{
					for (int k = num2; k < num3; k++)
					{
						if (j > num4 || num3 > num2)
						{
							float height2 = chunkProviderRaw.GetHeight(k + worldSizeHalf.x, j + worldSizeHalf.y);
							if (Mathf.Abs(height - height2) > 1f)
							{
								flag = false;
								break;
							}
						}
						if (!this.IsPositionValid(k, j))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
				if (flag)
				{
					Vector3i vector3i2 = new Vector3i(vector3i.x - 16, vector3i.y, vector3i.z);
					Vector3i vector3i3 = new Vector3i(vector3i.x, vector3i.y, vector3i.z - 16);
					Vector3i vector3i4 = new Vector3i(vector3i.x - 16, vector3i.y, vector3i.z - 16);
					if (candidateSpots.ContainsKey(vector3i2) && candidateSpots.ContainsKey(vector3i3) && candidateSpots.ContainsKey(vector3i4))
					{
						candidateSpots.Remove(vector3i2);
						candidateSpots.Remove(vector3i3);
						candidateSpots.Remove(vector3i4);
						vector3i = vector3i4;
						num = 32;
					}
					candidateSpots.Add(vector3i, num);
				}
			}
			if (ms.ElapsedMilliseconds - yieldTimeStamp > 250L)
			{
				yieldTimeStamp = ms.ElapsedMilliseconds;
				yield return null;
			}
		}
		foreach (KeyValuePair<Vector3i, int> keyValuePair in candidateSpots)
		{
			FlatArea flatArea = new FlatArea(keyValuePair.Key, keyValuePair.Value);
			if (flatArea.size == 16)
			{
				this.areaList16x16.Add(flatArea);
			}
			else
			{
				this.areaList32x32.Add(flatArea);
			}
		}
		ms.Stop();
		yield return null;
		Log.Out("Flat Areas Defined. Time Taken: {0}ms. Areas found: {1} (Small: {2} Large: {3})", new object[]
		{
			ms.ElapsedMilliseconds,
			this.areaList16x16.Count + this.areaList32x32.Count,
			this.areaList16x16.Count,
			this.areaList32x32.Count
		});
		yield break;
	}

	// Token: 0x0600655E RID: 25950 RVA: 0x00279404 File Offset: 0x00277604
	public bool IsPositionValid(int _x, int _z)
	{
		World world = GameManager.Instance.World;
		EnumDecoOccupied decoOccupiedFromMap = DecoManager.Instance.GetDecoOccupiedFromMap(_x, _z);
		return decoOccupiedFromMap == EnumDecoOccupied.Free || decoOccupiedFromMap == EnumDecoOccupied.Perimeter || decoOccupiedFromMap == EnumDecoOccupied.Deco;
	}

	// Token: 0x0600655F RID: 25951 RVA: 0x00279437 File Offset: 0x00277637
	public void Cleanup()
	{
		this.areaList16x16.Clear();
		this.areaList32x32.Clear();
	}

	// Token: 0x06006560 RID: 25952 RVA: 0x0027944F File Offset: 0x0027764F
	public List<FlatArea> GetAreasWithinRange(Vector3 worldPos, float distance, eFlatAreaSizeFilter searchMode = eFlatAreaSizeFilter.All, BiomeFilterTypes biomeFilter = BiomeFilterTypes.AnyBiome, string[] biomeNames = null, ChunkProtectionLevel maxAllowedChunkProtectionLevel = ChunkProtectionLevel.NearLandClaim)
	{
		return this.GetAreasWithinRange(worldPos, 0f, distance, searchMode, biomeFilter, biomeNames, maxAllowedChunkProtectionLevel);
	}

	// Token: 0x06006561 RID: 25953 RVA: 0x00279468 File Offset: 0x00277668
	public List<FlatArea> GetAreasWithinRange(Vector3 worldPos, float minDistance, float maxDistance, eFlatAreaSizeFilter searchMode = eFlatAreaSizeFilter.All, BiomeFilterTypes biomeFilter = BiomeFilterTypes.AnyBiome, string[] biomeNames = null, ChunkProtectionLevel maxAllowedChunkProtectionLevel = ChunkProtectionLevel.NearLandClaim)
	{
		if (minDistance > maxDistance)
		{
			float num = minDistance;
			float num2 = maxDistance;
			maxDistance = num;
			minDistance = num2;
		}
		Vector2 vector = new Vector2(worldPos.x, worldPos.z);
		List<FlatArea> list = new List<FlatArea>();
		int num3 = 0;
		if (searchMode == eFlatAreaSizeFilter.All || searchMode == eFlatAreaSizeFilter.Large)
		{
			foreach (FlatArea flatArea in this.areaList32x32)
			{
				Vector2 vector2 = new Vector2(flatArea.Center.x, flatArea.Center.z);
				float num4 = Vector2.Distance(vector, vector2);
				if (num4 >= minDistance && num4 <= maxDistance)
				{
					if (flatArea.IsValid(GameManager.Instance.World, biomeFilter, biomeNames, maxAllowedChunkProtectionLevel))
					{
						list.Add(flatArea);
					}
					else
					{
						num3++;
					}
				}
			}
		}
		if (searchMode == eFlatAreaSizeFilter.All || searchMode == eFlatAreaSizeFilter.Small)
		{
			foreach (FlatArea flatArea2 in this.areaList16x16)
			{
				Vector2 vector3 = new Vector2(flatArea2.Center.x, flatArea2.Center.z);
				float num5 = Vector2.Distance(vector, vector3);
				if (num5 >= minDistance && num5 <= maxDistance)
				{
					if (flatArea2.IsValid(GameManager.Instance.World, biomeFilter, biomeNames, maxAllowedChunkProtectionLevel))
					{
						list.Add(flatArea2);
					}
					else
					{
						num3++;
					}
				}
			}
		}
		Log.Out("Found {0} flat areas within range ({1} <> {2}) of position {3} ({4} areas invalid). Search Mode {5}", new object[]
		{
			list.Count,
			minDistance,
			maxDistance,
			worldPos,
			num3,
			searchMode.ToString()
		});
		return list;
	}

	// Token: 0x06006562 RID: 25954 RVA: 0x0027963C File Offset: 0x0027783C
	public List<FlatArea> GetAllFlatAreas()
	{
		List<FlatArea> list = new List<FlatArea>();
		list.AddRange(this.areaList16x16);
		list.AddRange(this.areaList32x32);
		return list;
	}

	// Token: 0x04004ED3 RID: 20179
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cHeightDifferenceThreshold = 1f;

	// Token: 0x04004ED4 RID: 20180
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cYieldMS = 250;

	// Token: 0x04004ED5 RID: 20181
	[PublicizedFrom(EAccessModifier.Private)]
	public List<FlatArea> areaList16x16 = new List<FlatArea>();

	// Token: 0x04004ED6 RID: 20182
	[PublicizedFrom(EAccessModifier.Private)]
	public List<FlatArea> areaList32x32 = new List<FlatArea>();
}
