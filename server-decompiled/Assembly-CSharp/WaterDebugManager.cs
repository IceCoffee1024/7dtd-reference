using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000CEF RID: 3311
public class WaterDebugManager
{
	// Token: 0x17000AA5 RID: 2725
	// (get) Token: 0x060064FE RID: 25854 RVA: 0x0027B20D File Offset: 0x0027940D
	// (set) Token: 0x060064FF RID: 25855 RVA: 0x0027B215 File Offset: 0x00279415
	public bool RenderingEnabled { get; set; } = true;

	// Token: 0x06006500 RID: 25856 RVA: 0x0027B220 File Offset: 0x00279420
	public void InitializeDebugRender(Chunk chunk)
	{
		WaterDebugRenderer waterDebugRenderer = WaterDebugPools.rendererPool.AllocSync(true);
		waterDebugRenderer.LoadFromChunk(chunk);
		chunk.AssignWaterDebugRenderer(new WaterDebugManager.RendererHandle(chunk, this));
		this.newRenderers.Enqueue(new WaterDebugManager.InitializedRenderer
		{
			chunkKey = chunk.Key,
			renderer = waterDebugRenderer
		});
	}

	// Token: 0x06006501 RID: 25857 RVA: 0x0027B276 File Offset: 0x00279476
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReturnRenderer(long key)
	{
		this.renderersToRemove.Enqueue(key);
	}

	// Token: 0x06006502 RID: 25858 RVA: 0x0027B284 File Offset: 0x00279484
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateRenderers()
	{
		WaterDebugManager.InitializedRenderer initializedRenderer;
		while (this.newRenderers.TryDequeue(out initializedRenderer))
		{
			WaterDebugRenderer waterDebugRenderer;
			if (this.activeRenderers.TryGetValue(initializedRenderer.chunkKey, out waterDebugRenderer))
			{
				WaterDebugPools.rendererPool.FreeSync(waterDebugRenderer);
				this.activeRenderers.Remove(initializedRenderer.chunkKey);
			}
			this.activeRenderers.Add(initializedRenderer.chunkKey, initializedRenderer.renderer);
		}
		long num;
		while (this.renderersToRemove.TryDequeue(out num))
		{
			WaterDebugRenderer waterDebugRenderer2;
			if (this.activeRenderers.TryGetValue(num, out waterDebugRenderer2))
			{
				WaterDebugPools.rendererPool.FreeSync(waterDebugRenderer2);
				this.activeRenderers.Remove(num);
			}
		}
	}

	// Token: 0x06006503 RID: 25859 RVA: 0x0027B324 File Offset: 0x00279524
	public void DebugDraw()
	{
		this.UpdateRenderers();
		if (this.RenderingEnabled)
		{
			foreach (WaterDebugRenderer waterDebugRenderer in this.activeRenderers.Values)
			{
				waterDebugRenderer.Draw();
			}
		}
	}

	// Token: 0x06006504 RID: 25860 RVA: 0x0027B388 File Offset: 0x00279588
	public void Cleanup()
	{
		WaterDebugManager.InitializedRenderer initializedRenderer;
		while (this.newRenderers.TryDequeue(out initializedRenderer))
		{
			WaterDebugPools.rendererPool.FreeSync(initializedRenderer.renderer);
		}
		foreach (WaterDebugRenderer waterDebugRenderer in this.activeRenderers.Values)
		{
			WaterDebugPools.rendererPool.FreeSync(waterDebugRenderer);
		}
		this.activeRenderers.Clear();
	}

	// Token: 0x04004ED0 RID: 20176
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<WaterDebugManager.InitializedRenderer> newRenderers = new ConcurrentQueue<WaterDebugManager.InitializedRenderer>();

	// Token: 0x04004ED1 RID: 20177
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, WaterDebugRenderer> activeRenderers = new Dictionary<long, WaterDebugRenderer>();

	// Token: 0x04004ED2 RID: 20178
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<long> renderersToRemove = new ConcurrentQueue<long>();

	// Token: 0x02000CF0 RID: 3312
	[PublicizedFrom(EAccessModifier.Private)]
	public struct InitializedRenderer
	{
		// Token: 0x04004ED3 RID: 20179
		public long chunkKey;

		// Token: 0x04004ED4 RID: 20180
		public WaterDebugRenderer renderer;
	}

	// Token: 0x02000CF1 RID: 3313
	public struct RendererHandle
	{
		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06006506 RID: 25862 RVA: 0x0027B440 File Offset: 0x00279640
		public bool IsValid
		{
			get
			{
				return this.manager != null && this.key != null;
			}
		}

		// Token: 0x06006507 RID: 25863 RVA: 0x0027B457 File Offset: 0x00279657
		public RendererHandle(Chunk _chunk, WaterDebugManager _manager)
		{
			this.manager = _manager;
			this.key = new long?(_chunk.Key);
		}

		// Token: 0x06006508 RID: 25864 RVA: 0x0027B474 File Offset: 0x00279674
		[Conditional("UNITY_EDITOR")]
		public void SetChunkOrigin(Vector3i _origin)
		{
			if (!this.IsValid)
			{
				return;
			}
			WaterDebugRenderer waterDebugRenderer;
			if (this.manager.activeRenderers.TryGetValue(this.key.Value, out waterDebugRenderer))
			{
				waterDebugRenderer.SetChunkOrigin(_origin);
			}
		}

		// Token: 0x06006509 RID: 25865 RVA: 0x0027B4B8 File Offset: 0x002796B8
		[Conditional("UNITY_EDITOR")]
		public void SetWater(int _x, int _y, int _z, float mass)
		{
			if (!this.IsValid)
			{
				return;
			}
			WaterDebugRenderer waterDebugRenderer;
			if (this.manager.activeRenderers.TryGetValue(this.key.Value, out waterDebugRenderer))
			{
				waterDebugRenderer.SetWater(_x, _y, _z, mass);
			}
		}

		// Token: 0x0600650A RID: 25866 RVA: 0x0027B4F8 File Offset: 0x002796F8
		[Conditional("UNITY_EDITOR")]
		public void Reset()
		{
			if (this.IsValid)
			{
				this.manager.ReturnRenderer(this.key.Value);
			}
			this.manager = null;
			this.key = null;
		}

		// Token: 0x04004ED5 RID: 20181
		[PublicizedFrom(EAccessModifier.Private)]
		public WaterDebugManager manager;

		// Token: 0x04004ED6 RID: 20182
		[PublicizedFrom(EAccessModifier.Private)]
		public long? key;
	}
}
