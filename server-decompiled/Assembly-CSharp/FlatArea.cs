using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CFC RID: 3324
public class FlatArea
{
	// Token: 0x17000AA9 RID: 2729
	// (get) Token: 0x06006542 RID: 25922 RVA: 0x0027C47E File Offset: 0x0027A67E
	public int Height
	{
		get
		{
			return this.position.y;
		}
	}

	// Token: 0x17000AAA RID: 2730
	// (get) Token: 0x06006543 RID: 25923 RVA: 0x0027C48B File Offset: 0x0027A68B
	public int MinX
	{
		get
		{
			return this.position.x;
		}
	}

	// Token: 0x17000AAB RID: 2731
	// (get) Token: 0x06006544 RID: 25924 RVA: 0x0027C498 File Offset: 0x0027A698
	public int MaxX
	{
		get
		{
			return this.position.x + this.size - 1;
		}
	}

	// Token: 0x17000AAC RID: 2732
	// (get) Token: 0x06006545 RID: 25925 RVA: 0x0027C4AE File Offset: 0x0027A6AE
	public int MinZ
	{
		get
		{
			return this.position.z;
		}
	}

	// Token: 0x17000AAD RID: 2733
	// (get) Token: 0x06006546 RID: 25926 RVA: 0x0027C4BB File Offset: 0x0027A6BB
	public int MaxZ
	{
		get
		{
			return this.position.z + this.size - 1;
		}
	}

	// Token: 0x17000AAE RID: 2734
	// (get) Token: 0x06006547 RID: 25927 RVA: 0x0027C4D1 File Offset: 0x0027A6D1
	public Vector3 Center
	{
		get
		{
			return this.position + new Vector3((float)(this.size / 2), 0f, (float)(this.size / 2));
		}
	}

	// Token: 0x06006548 RID: 25928 RVA: 0x0027C4FF File Offset: 0x0027A6FF
	public FlatArea(Vector3i _position, int _size)
	{
		this.position = _position;
		this.size = _size;
	}

	// Token: 0x06006549 RID: 25929 RVA: 0x0027C518 File Offset: 0x0027A718
	public bool IsValid(World world, BiomeFilterTypes biomeFilter = BiomeFilterTypes.AnyBiome, string[] biomeNames = null, ChunkProtectionLevel maxAllowedChunkProtectionLevel = ChunkProtectionLevel.NearLandClaim)
	{
		this.maxChunkProtectionLevel = ChunkProtectionLevel.None;
		int num = this.MinX >> 4 << 4;
		int num2 = this.MaxX >> 4 << 4;
		int num3 = this.MinZ >> 4 << 4;
		int num4 = this.MaxZ >> 4 << 4;
		for (int i = num; i <= num2; i += 16)
		{
			for (int j = num3; j <= num4; j += 16)
			{
				ChunkProtectionLevel chunkProtectionLevel = world.ChunkCache.ChunkProvider.GetChunkProtectionLevel(new Vector3i(i, this.position.y, j));
				if (chunkProtectionLevel > this.maxChunkProtectionLevel)
				{
					this.maxChunkProtectionLevel = chunkProtectionLevel;
				}
			}
		}
		if (this.maxChunkProtectionLevel > maxAllowedChunkProtectionLevel)
		{
			return false;
		}
		if (biomeFilter != BiomeFilterTypes.AnyBiome)
		{
			BiomeDefinition biomeInWorld = GameManager.Instance.World.GetBiomeInWorld((int)this.Center.x, (int)this.Center.z);
			if (biomeInWorld == null)
			{
				return false;
			}
			if (biomeFilter == BiomeFilterTypes.OnlyBiome)
			{
				if (biomeInWorld.m_sBiomeName != biomeNames[0])
				{
					return false;
				}
			}
			else if (biomeFilter == BiomeFilterTypes.ExcludeBiome)
			{
				bool flag = false;
				for (int k = 0; k < biomeNames.Length; k++)
				{
					if (biomeInWorld.m_sBiomeName == biomeNames[k])
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					return false;
				}
			}
			else if (biomeFilter == BiomeFilterTypes.SameBiome && biomeInWorld.m_sBiomeName != biomeNames[0])
			{
				return false;
			}
		}
		return !world.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator().HasPrefabsAtXZ(this.MinX, this.MaxX, this.MinZ, this.MaxZ);
	}

	// Token: 0x0600654A RID: 25930 RVA: 0x0027C682 File Offset: 0x0027A882
	public bool IsInArea(int _x, int _z)
	{
		return _x >= this.MinX && _x <= this.MaxX && _z >= this.MinZ && _z <= this.MaxZ;
	}

	// Token: 0x0600654B RID: 25931 RVA: 0x0027C6B0 File Offset: 0x0027A8B0
	public List<Vector2i> GetPositions()
	{
		List<Vector2i> list = new List<Vector2i>();
		for (int i = this.MinX; i <= this.MaxX; i++)
		{
			for (int j = this.MinZ; j <= this.MaxZ; j++)
			{
				list.Add(new Vector2i(i, j));
			}
		}
		return list;
	}

	// Token: 0x0600654C RID: 25932 RVA: 0x0027C700 File Offset: 0x0027A900
	public Vector3 GetRandomPosition(float margin = 0f)
	{
		return new Vector3(global::UnityEngine.Random.Range((float)this.MinX + margin, (float)this.MaxX - margin + 1f), (float)this.Height, global::UnityEngine.Random.Range((float)this.MinZ + margin, (float)this.MaxZ - margin + 1f));
	}

	// Token: 0x0600654D RID: 25933 RVA: 0x0027C754 File Offset: 0x0027A954
	public override bool Equals(object obj)
	{
		FlatArea flatArea = obj as FlatArea;
		return flatArea != null && this.position == flatArea.position && this.size == flatArea.size;
	}

	// Token: 0x0600654E RID: 25934 RVA: 0x000111D7 File Offset: 0x0000F3D7
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x04004F06 RID: 20230
	public Vector3i position;

	// Token: 0x04004F07 RID: 20231
	public int size;

	// Token: 0x04004F08 RID: 20232
	public ChunkProtectionLevel maxChunkProtectionLevel;
}
