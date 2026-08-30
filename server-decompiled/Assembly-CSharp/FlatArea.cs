using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CDC RID: 3292
public class FlatArea
{
	// Token: 0x17000A80 RID: 2688
	// (get) Token: 0x060064C4 RID: 25796 RVA: 0x002761DE File Offset: 0x002743DE
	public int Height
	{
		get
		{
			return this.position.y;
		}
	}

	// Token: 0x17000A81 RID: 2689
	// (get) Token: 0x060064C5 RID: 25797 RVA: 0x002761EB File Offset: 0x002743EB
	public int MinX
	{
		get
		{
			return this.position.x;
		}
	}

	// Token: 0x17000A82 RID: 2690
	// (get) Token: 0x060064C6 RID: 25798 RVA: 0x002761F8 File Offset: 0x002743F8
	public int MaxX
	{
		get
		{
			return this.position.x + this.size - 1;
		}
	}

	// Token: 0x17000A83 RID: 2691
	// (get) Token: 0x060064C7 RID: 25799 RVA: 0x0027620E File Offset: 0x0027440E
	public int MinZ
	{
		get
		{
			return this.position.z;
		}
	}

	// Token: 0x17000A84 RID: 2692
	// (get) Token: 0x060064C8 RID: 25800 RVA: 0x0027621B File Offset: 0x0027441B
	public int MaxZ
	{
		get
		{
			return this.position.z + this.size - 1;
		}
	}

	// Token: 0x17000A85 RID: 2693
	// (get) Token: 0x060064C9 RID: 25801 RVA: 0x00276231 File Offset: 0x00274431
	public Vector3 Center
	{
		get
		{
			return this.position + new Vector3((float)(this.size / 2), 0f, (float)(this.size / 2));
		}
	}

	// Token: 0x060064CA RID: 25802 RVA: 0x0027625F File Offset: 0x0027445F
	public FlatArea(Vector3i _position, int _size)
	{
		this.position = _position;
		this.size = _size;
	}

	// Token: 0x060064CB RID: 25803 RVA: 0x00276278 File Offset: 0x00274478
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

	// Token: 0x060064CC RID: 25804 RVA: 0x002763E2 File Offset: 0x002745E2
	public bool IsInArea(int _x, int _z)
	{
		return _x >= this.MinX && _x <= this.MaxX && _z >= this.MinZ && _z <= this.MaxZ;
	}

	// Token: 0x060064CD RID: 25805 RVA: 0x00276410 File Offset: 0x00274610
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

	// Token: 0x060064CE RID: 25806 RVA: 0x00276460 File Offset: 0x00274660
	public Vector3 GetRandomPosition(float margin = 0f)
	{
		return new Vector3(global::UnityEngine.Random.Range((float)this.MinX + margin, (float)this.MaxX - margin + 1f), (float)this.Height, global::UnityEngine.Random.Range((float)this.MinZ + margin, (float)this.MaxZ - margin + 1f));
	}

	// Token: 0x060064CF RID: 25807 RVA: 0x002764B4 File Offset: 0x002746B4
	public override bool Equals(object obj)
	{
		FlatArea flatArea = obj as FlatArea;
		return flatArea != null && this.position == flatArea.position && this.size == flatArea.size;
	}

	// Token: 0x060064D0 RID: 25808 RVA: 0x000111D7 File Offset: 0x0000F3D7
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x04004E32 RID: 20018
	public Vector3i position;

	// Token: 0x04004E33 RID: 20019
	public int size;

	// Token: 0x04004E34 RID: 20020
	public ChunkProtectionLevel maxChunkProtectionLevel;
}
