using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D07 RID: 3335
public class FlatArea
{
	// Token: 0x17000AAF RID: 2735
	// (get) Token: 0x06006567 RID: 25959 RVA: 0x0027D9A6 File Offset: 0x0027BBA6
	public int Height
	{
		get
		{
			return this.position.y;
		}
	}

	// Token: 0x17000AB0 RID: 2736
	// (get) Token: 0x06006568 RID: 25960 RVA: 0x0027D9B3 File Offset: 0x0027BBB3
	public int MinX
	{
		get
		{
			return this.position.x;
		}
	}

	// Token: 0x17000AB1 RID: 2737
	// (get) Token: 0x06006569 RID: 25961 RVA: 0x0027D9C0 File Offset: 0x0027BBC0
	public int MaxX
	{
		get
		{
			return this.position.x + this.size - 1;
		}
	}

	// Token: 0x17000AB2 RID: 2738
	// (get) Token: 0x0600656A RID: 25962 RVA: 0x0027D9D6 File Offset: 0x0027BBD6
	public int MinZ
	{
		get
		{
			return this.position.z;
		}
	}

	// Token: 0x17000AB3 RID: 2739
	// (get) Token: 0x0600656B RID: 25963 RVA: 0x0027D9E3 File Offset: 0x0027BBE3
	public int MaxZ
	{
		get
		{
			return this.position.z + this.size - 1;
		}
	}

	// Token: 0x17000AB4 RID: 2740
	// (get) Token: 0x0600656C RID: 25964 RVA: 0x0027D9F9 File Offset: 0x0027BBF9
	public Vector3 Center
	{
		get
		{
			return this.position + new Vector3((float)(this.size / 2), 0f, (float)(this.size / 2));
		}
	}

	// Token: 0x0600656D RID: 25965 RVA: 0x0027DA27 File Offset: 0x0027BC27
	public FlatArea(Vector3i _position, int _size)
	{
		this.position = _position;
		this.size = _size;
	}

	// Token: 0x0600656E RID: 25966 RVA: 0x0027DA40 File Offset: 0x0027BC40
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

	// Token: 0x0600656F RID: 25967 RVA: 0x0027DBAA File Offset: 0x0027BDAA
	public bool IsInArea(int _x, int _z)
	{
		return _x >= this.MinX && _x <= this.MaxX && _z >= this.MinZ && _z <= this.MaxZ;
	}

	// Token: 0x06006570 RID: 25968 RVA: 0x0027DBD8 File Offset: 0x0027BDD8
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

	// Token: 0x06006571 RID: 25969 RVA: 0x0027DC28 File Offset: 0x0027BE28
	public Vector3 GetRandomPosition(float margin = 0f)
	{
		return new Vector3(global::UnityEngine.Random.Range((float)this.MinX + margin, (float)this.MaxX - margin + 1f), (float)this.Height, global::UnityEngine.Random.Range((float)this.MinZ + margin, (float)this.MaxZ - margin + 1f));
	}

	// Token: 0x06006572 RID: 25970 RVA: 0x0027DC7C File Offset: 0x0027BE7C
	public override bool Equals(object obj)
	{
		FlatArea flatArea = obj as FlatArea;
		return flatArea != null && this.position == flatArea.position && this.size == flatArea.size;
	}

	// Token: 0x06006573 RID: 25971 RVA: 0x000111D7 File Offset: 0x0000F3D7
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x04004F2B RID: 20267
	public Vector3i position;

	// Token: 0x04004F2C RID: 20268
	public int size;

	// Token: 0x04004F2D RID: 20269
	public ChunkProtectionLevel maxChunkProtectionLevel;
}
