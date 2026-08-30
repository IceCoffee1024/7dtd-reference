using System;
using UnityEngine;

// Token: 0x02000CE0 RID: 3296
public class WaterDebugRenderer : IMemoryPoolableObject
{
	// Token: 0x06006503 RID: 25859 RVA: 0x00277C2C File Offset: 0x00275E2C
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

	// Token: 0x06006504 RID: 25860 RVA: 0x00277C8C File Offset: 0x00275E8C
	public void SetWater(int _x, int _y, int _z, float mass)
	{
		int num = _y / 16;
		int num2 = _y % 16;
		this.GetOrCreateLayer(num).SetWater(_x, num2, _z, mass);
	}

	// Token: 0x06006505 RID: 25861 RVA: 0x00277CB4 File Offset: 0x00275EB4
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

	// Token: 0x06006506 RID: 25862 RVA: 0x00277D24 File Offset: 0x00275F24
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

	// Token: 0x06006507 RID: 25863 RVA: 0x00277DAC File Offset: 0x00275FAC
	public void Draw()
	{
		for (int i = 0; i < this.numActiveLayers; i++)
		{
			int num = this.activeLayers[i];
			this.layers[num].Draw();
		}
	}

	// Token: 0x06006508 RID: 25864 RVA: 0x00277DE0 File Offset: 0x00275FE0
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

	// Token: 0x06006509 RID: 25865 RVA: 0x00277E34 File Offset: 0x00276034
	public void Cleanup()
	{
		this.Clear();
	}

	// Token: 0x0600650A RID: 25866 RVA: 0x00277E34 File Offset: 0x00276034
	public void Reset()
	{
		this.Clear();
	}

	// Token: 0x04004E94 RID: 20116
	[PublicizedFrom(EAccessModifier.Private)]
	public const int numLayers = 16;

	// Token: 0x04004E95 RID: 20117
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 chunkOrigin = Vector3.zero;

	// Token: 0x04004E96 RID: 20118
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterDebugRendererLayer[] layers = new WaterDebugRendererLayer[16];

	// Token: 0x04004E97 RID: 20119
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] activeLayers = new int[16];

	// Token: 0x04004E98 RID: 20120
	[PublicizedFrom(EAccessModifier.Private)]
	public int numActiveLayers;
}
