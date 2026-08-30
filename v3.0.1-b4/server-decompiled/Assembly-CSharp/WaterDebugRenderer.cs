using System;
using UnityEngine;

// Token: 0x02000CCE RID: 3278
public class WaterDebugRenderer : IMemoryPoolableObject
{
	// Token: 0x06006477 RID: 25719 RVA: 0x00274D30 File Offset: 0x00272F30
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

	// Token: 0x06006478 RID: 25720 RVA: 0x00274D90 File Offset: 0x00272F90
	public void SetWater(int _x, int _y, int _z, float mass)
	{
		int num = _y / 16;
		int num2 = _y % 16;
		this.GetOrCreateLayer(num).SetWater(_x, num2, _z, mass);
	}

	// Token: 0x06006479 RID: 25721 RVA: 0x00274DB8 File Offset: 0x00272FB8
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

	// Token: 0x0600647A RID: 25722 RVA: 0x00274E28 File Offset: 0x00273028
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

	// Token: 0x0600647B RID: 25723 RVA: 0x00274EB0 File Offset: 0x002730B0
	public void Draw()
	{
		for (int i = 0; i < this.numActiveLayers; i++)
		{
			int num = this.activeLayers[i];
			this.layers[num].Draw();
		}
	}

	// Token: 0x0600647C RID: 25724 RVA: 0x00274EE4 File Offset: 0x002730E4
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

	// Token: 0x0600647D RID: 25725 RVA: 0x00274F38 File Offset: 0x00273138
	public void Cleanup()
	{
		this.Clear();
	}

	// Token: 0x0600647E RID: 25726 RVA: 0x00274F38 File Offset: 0x00273138
	public void Reset()
	{
		this.Clear();
	}

	// Token: 0x04004DF6 RID: 19958
	[PublicizedFrom(EAccessModifier.Private)]
	public const int numLayers = 16;

	// Token: 0x04004DF7 RID: 19959
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 chunkOrigin = Vector3.zero;

	// Token: 0x04004DF8 RID: 19960
	[PublicizedFrom(EAccessModifier.Private)]
	public WaterDebugRendererLayer[] layers = new WaterDebugRendererLayer[16];

	// Token: 0x04004DF9 RID: 19961
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] activeLayers = new int[16];

	// Token: 0x04004DFA RID: 19962
	[PublicizedFrom(EAccessModifier.Private)]
	public int numActiveLayers;
}
