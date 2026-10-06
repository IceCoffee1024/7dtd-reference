using System;
using UnityEngine;

// Token: 0x02000CEE RID: 3310
public class WaterDebugRenderer : IMemoryPoolableObject
{
	// Token: 0x060064F5 RID: 25845 RVA: 0x0027AFD0 File Offset: 0x002791D0
	public void SetChunkOrigin(Vector3 _origin)
	{
		this.chunkOrigin = _origin;
		for (int i = 0; i < this.numActiveLayers; i++)
		{
			int num = this.activeLayers[i];
			Vector3 vector = this.chunkOrigin + Vector3.up * (float)num * 16f;
			this.layers[num].SetLayerOrigin(vector);
		}
	}

	// Token: 0x060064F6 RID: 25846 RVA: 0x0027B030 File Offset: 0x00279230
	public void SetWater(int _x, int _y, int _z, float mass)
	{
		int num = _y / 16;
		int num2 = _y % 16;
		this.GetOrCreateLayer(num).SetWater(_x, num2, _z, mass);
	}

	// Token: 0x060064F7 RID: 25847 RVA: 0x0027B058 File Offset: 0x00279258
	public void LoadFromChunk(Chunk chunk)
	{
		this.SetChunkOrigin(chunk.GetWorldPos());
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 256; k++)
				{
					float num = (float)chunk.GetWater(i, k, j).GetMass();
					if (num > 195f)
					{
						this.SetWater(i, k, j, num);
					}
				}
			}
		}
	}

	// Token: 0x060064F8 RID: 25848 RVA: 0x0027B0C8 File Offset: 0x002792C8
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterDebugRendererLayer GetOrCreateLayer(int layerIndex)
	{
		WaterDebugRendererLayer waterDebugRendererLayer = this.layers[layerIndex];
		if (waterDebugRendererLayer == null)
		{
			waterDebugRendererLayer = WaterDebugPools.layerPool.AllocSync(false);
			Vector3 vector = this.chunkOrigin + Vector3.up * (float)layerIndex * 16f;
			waterDebugRendererLayer.SetLayerOrigin(vector);
			this.layers[layerIndex] = waterDebugRendererLayer;
			this.activeLayers[this.numActiveLayers] = layerIndex;
			this.numActiveLayers++;
			Array.Sort<int>(this.activeLayers, 0, this.numActiveLayers);
		}
		return waterDebugRendererLayer;
	}

	// Token: 0x060064F9 RID: 25849 RVA: 0x0027B150 File Offset: 0x00279350
	public void Draw()
	{
		for (int i = 0; i < this.numActiveLayers; i++)
		{
			int num = this.activeLayers[i];
			this.layers[num].Draw();
		}
	}

	// Token: 0x060064FA RID: 25850 RVA: 0x0027B184 File Offset: 0x00279384
	public void Clear()
	{
		for (int i = 0; i < this.numActiveLayers; i++)
		{
			int num = this.activeLayers[i];
			WaterDebugRendererLayer waterDebugRendererLayer = this.layers[num];
			WaterDebugPools.layerPool.FreeSync(waterDebugRendererLayer);
			this.layers[num] = null;
			this.activeLayers[i] = 0;
		}
		this.numActiveLayers = 0;
	}

	// Token: 0x060064FB RID: 25851 RVA: 0x0027B1D8 File Offset: 0x002793D8
	public void Cleanup()
	{
		this.Clear();
	}

	// Token: 0x060064FC RID: 25852 RVA: 0x0027B1D8 File Offset: 0x002793D8
	public void Reset()
	{
		this.Clear();
	}

	// Token: 0x04004ECA RID: 20170
	[PublicizedFrom(EAccessModifier.Private)]
	public const int numLayers = 16;

	// Token: 0x04004ECB RID: 20171
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 chunkOrigin = Vector3.zero;

	// Token: 0x04004ECC RID: 20172
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterDebugRendererLayer[] layers = new WaterDebugRendererLayer[16];

	// Token: 0x04004ECD RID: 20173
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] activeLayers = new int[16];

	// Token: 0x04004ECE RID: 20174
	[PublicizedFrom(EAccessModifier.Private)]
	public int numActiveLayers;
}
