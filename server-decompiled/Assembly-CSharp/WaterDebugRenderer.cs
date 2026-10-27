using System;
using UnityEngine;

// Token: 0x02000CF9 RID: 3321
public class WaterDebugRenderer : IMemoryPoolableObject
{
	// Token: 0x0600651A RID: 25882 RVA: 0x0027C4F8 File Offset: 0x0027A6F8
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

	// Token: 0x0600651B RID: 25883 RVA: 0x0027C558 File Offset: 0x0027A758
	public void SetWater(int _x, int _y, int _z, float mass)
	{
		int num = _y / 16;
		int num2 = _y % 16;
		this.GetOrCreateLayer(num).SetWater(_x, num2, _z, mass);
	}

	// Token: 0x0600651C RID: 25884 RVA: 0x0027C580 File Offset: 0x0027A780
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

	// Token: 0x0600651D RID: 25885 RVA: 0x0027C5F0 File Offset: 0x0027A7F0
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

	// Token: 0x0600651E RID: 25886 RVA: 0x0027C678 File Offset: 0x0027A878
	public void Draw()
	{
		for (int i = 0; i < this.numActiveLayers; i++)
		{
			int num = this.activeLayers[i];
			this.layers[num].Draw();
		}
	}

	// Token: 0x0600651F RID: 25887 RVA: 0x0027C6AC File Offset: 0x0027A8AC
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

	// Token: 0x06006520 RID: 25888 RVA: 0x0027C700 File Offset: 0x0027A900
	public void Cleanup()
	{
		this.Clear();
	}

	// Token: 0x06006521 RID: 25889 RVA: 0x0027C700 File Offset: 0x0027A900
	public void Reset()
	{
		this.Clear();
	}

	// Token: 0x04004EEF RID: 20207
	[PublicizedFrom(EAccessModifier.Private)]
	public const int numLayers = 16;

	// Token: 0x04004EF0 RID: 20208
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 chunkOrigin = Vector3.zero;

	// Token: 0x04004EF1 RID: 20209
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterDebugRendererLayer[] layers = new WaterDebugRendererLayer[16];

	// Token: 0x04004EF2 RID: 20210
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] activeLayers = new int[16];

	// Token: 0x04004EF3 RID: 20211
	[PublicizedFrom(EAccessModifier.Private)]
	public int numActiveLayers;
}
