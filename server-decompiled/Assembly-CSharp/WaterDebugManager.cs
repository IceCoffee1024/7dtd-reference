using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000CFA RID: 3322
public class WaterDebugManager
{
	// Token: 0x17000AAB RID: 2731
	// (get) Token: 0x06006523 RID: 25891 RVA: 0x0027C735 File Offset: 0x0027A935
	// (set) Token: 0x06006524 RID: 25892 RVA: 0x0027C73D File Offset: 0x0027A93D
	public bool RenderingEnabled { get; set; } = true;

	// Token: 0x06006525 RID: 25893 RVA: 0x0027C748 File Offset: 0x0027A948
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

	// Token: 0x06006526 RID: 25894 RVA: 0x0027C79E File Offset: 0x0027A99E
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReturnRenderer(long key)
	{
		this.renderersToRemove.Enqueue(key);
	}

	// Token: 0x06006527 RID: 25895 RVA: 0x0027C7AC File Offset: 0x0027A9AC
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

	// Token: 0x06006528 RID: 25896 RVA: 0x0027C84C File Offset: 0x0027AA4C
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

	// Token: 0x06006529 RID: 25897 RVA: 0x0027C8B0 File Offset: 0x0027AAB0
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

	// Token: 0x04004EF5 RID: 20213
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<WaterDebugManager.InitializedRenderer> newRenderers = new ConcurrentQueue<WaterDebugManager.InitializedRenderer>();

	// Token: 0x04004EF6 RID: 20214
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, WaterDebugRenderer> activeRenderers = new Dictionary<long, WaterDebugRenderer>();

	// Token: 0x04004EF7 RID: 20215
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<long> renderersToRemove = new ConcurrentQueue<long>();

	// Token: 0x02000CFB RID: 3323
	[PublicizedFrom(EAccessModifier.Private)]
	public struct InitializedRenderer
	{
		// Token: 0x04004EF8 RID: 20216
		public long chunkKey;

		// Token: 0x04004EF9 RID: 20217
		public WaterDebugRenderer renderer;
	}

	// Token: 0x02000CFC RID: 3324
	public struct RendererHandle
	{
		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x0600652B RID: 25899 RVA: 0x0027C968 File Offset: 0x0027AB68
		public bool IsValid
		{
			get
			{
				return this.manager != null && this.key != null;
			}
		}

		// Token: 0x0600652C RID: 25900 RVA: 0x0027C97F File Offset: 0x0027AB7F
		public RendererHandle(Chunk _chunk, WaterDebugManager _manager)
		{
			this.manager = _manager;
			this.key = new long?(_chunk.Key);
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x0027C99C File Offset: 0x0027AB9C
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

		// Token: 0x0600652E RID: 25902 RVA: 0x0027C9E0 File Offset: 0x0027ABE0
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

		// Token: 0x0600652F RID: 25903 RVA: 0x0027CA20 File Offset: 0x0027AC20
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

		// Token: 0x04004EFA RID: 20218
		[PublicizedFrom(EAccessModifier.Private)]
		public WaterDebugManager manager;

		// Token: 0x04004EFB RID: 20219
		[PublicizedFrom(EAccessModifier.Private)]
		public long? key;
	}
}
