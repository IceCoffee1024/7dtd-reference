using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000CCF RID: 3279
public class WaterDebugManager
{
	// Token: 0x17000A7C RID: 2684
	// (get) Token: 0x06006480 RID: 25728 RVA: 0x00274F6D File Offset: 0x0027316D
	// (set) Token: 0x06006481 RID: 25729 RVA: 0x00274F75 File Offset: 0x00273175
	public bool RenderingEnabled { get; set; } = true;

	// Token: 0x06006482 RID: 25730 RVA: 0x00274F80 File Offset: 0x00273180
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

	// Token: 0x06006483 RID: 25731 RVA: 0x00274FD6 File Offset: 0x002731D6
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReturnRenderer(long key)
	{
		this.renderersToRemove.Enqueue(key);
	}

	// Token: 0x06006484 RID: 25732 RVA: 0x00274FE4 File Offset: 0x002731E4
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

	// Token: 0x06006485 RID: 25733 RVA: 0x00275084 File Offset: 0x00273284
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

	// Token: 0x06006486 RID: 25734 RVA: 0x002750E8 File Offset: 0x002732E8
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

	// Token: 0x04004DFC RID: 19964
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<WaterDebugManager.InitializedRenderer> newRenderers = new ConcurrentQueue<WaterDebugManager.InitializedRenderer>();

	// Token: 0x04004DFD RID: 19965
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, WaterDebugRenderer> activeRenderers = new Dictionary<long, WaterDebugRenderer>();

	// Token: 0x04004DFE RID: 19966
	[PublicizedFrom(EAccessModifier.Private)]
	public ConcurrentQueue<long> renderersToRemove = new ConcurrentQueue<long>();

	// Token: 0x02000CD0 RID: 3280
	[PublicizedFrom(EAccessModifier.Private)]
	public struct InitializedRenderer
	{
		// Token: 0x04004DFF RID: 19967
		public long chunkKey;

		// Token: 0x04004E00 RID: 19968
		public WaterDebugRenderer renderer;
	}

	// Token: 0x02000CD1 RID: 3281
	public struct RendererHandle
	{
		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06006488 RID: 25736 RVA: 0x002751A0 File Offset: 0x002733A0
		public bool IsValid
		{
			get
			{
				return this.manager != null && this.key != null;
			}
		}

		// Token: 0x06006489 RID: 25737 RVA: 0x002751B7 File Offset: 0x002733B7
		public RendererHandle(Chunk _chunk, WaterDebugManager _manager)
		{
			this.manager = _manager;
			this.key = new long?(_chunk.Key);
		}

		// Token: 0x0600648A RID: 25738 RVA: 0x002751D4 File Offset: 0x002733D4
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

		// Token: 0x0600648B RID: 25739 RVA: 0x00275218 File Offset: 0x00273418
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

		// Token: 0x0600648C RID: 25740 RVA: 0x00275258 File Offset: 0x00273458
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

		// Token: 0x04004E01 RID: 19969
		[PublicizedFrom(EAccessModifier.Private)]
		public WaterDebugManager manager;

		// Token: 0x04004E02 RID: 19970
		[PublicizedFrom(EAccessModifier.Private)]
		public long? key;
	}
}
