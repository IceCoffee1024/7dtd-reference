using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000CE1 RID: 3297
public class WaterDebugManager
{
	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x0600650C RID: 25868 RVA: 0x00277E69 File Offset: 0x00276069
	// (set) Token: 0x0600650D RID: 25869 RVA: 0x00277E71 File Offset: 0x00276071
	public bool RenderingEnabled { get; set; } = true;

	// Token: 0x0600650E RID: 25870 RVA: 0x00277E7C File Offset: 0x0027607C
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

	// Token: 0x0600650F RID: 25871 RVA: 0x00277ED2 File Offset: 0x002760D2
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReturnRenderer(long key)
	{
		this.renderersToRemove.Enqueue(key);
	}

	// Token: 0x06006510 RID: 25872 RVA: 0x00277EE0 File Offset: 0x002760E0
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

	// Token: 0x06006511 RID: 25873 RVA: 0x00277F80 File Offset: 0x00276180
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

	// Token: 0x06006512 RID: 25874 RVA: 0x00277FE4 File Offset: 0x002761E4
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

	// Token: 0x04004E9A RID: 20122
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<WaterDebugManager.InitializedRenderer> newRenderers = new ConcurrentQueue<WaterDebugManager.InitializedRenderer>();

	// Token: 0x04004E9B RID: 20123
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, WaterDebugRenderer> activeRenderers = new Dictionary<long, WaterDebugRenderer>();

	// Token: 0x04004E9C RID: 20124
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<long> renderersToRemove = new ConcurrentQueue<long>();

	// Token: 0x02000CE2 RID: 3298
	[PublicizedFrom(EAccessModifier.Private)]
	public struct InitializedRenderer
	{
		// Token: 0x04004E9D RID: 20125
		public long chunkKey;

		// Token: 0x04004E9E RID: 20126
		public WaterDebugRenderer renderer;
	}

	// Token: 0x02000CE3 RID: 3299
	public struct RendererHandle
	{
		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06006514 RID: 25876 RVA: 0x0027809C File Offset: 0x0027629C
		public bool IsValid
		{
			get
			{
				return this.manager != null && this.key != null;
			}
		}

		// Token: 0x06006515 RID: 25877 RVA: 0x002780B3 File Offset: 0x002762B3
		public RendererHandle(Chunk _chunk, WaterDebugManager _manager)
		{
			this.manager = _manager;
			this.key = new long?(_chunk.Key);
		}

		// Token: 0x06006516 RID: 25878 RVA: 0x002780D0 File Offset: 0x002762D0
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

		// Token: 0x06006517 RID: 25879 RVA: 0x00278114 File Offset: 0x00276314
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

		// Token: 0x06006518 RID: 25880 RVA: 0x00278154 File Offset: 0x00276354
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

		// Token: 0x04004E9F RID: 20127
		[PublicizedFrom(EAccessModifier.Private)]
		public WaterDebugManager manager;

		// Token: 0x04004EA0 RID: 20128
		[PublicizedFrom(EAccessModifier.Private)]
		public long? key;
	}
}
