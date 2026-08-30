using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000487 RID: 1159
public class EntityAsyncManager
{
	// Token: 0x060023F8 RID: 9208 RVA: 0x000D9BE3 File Offset: 0x000D7DE3
	public EntityAsyncManager(NetEntityPackageQueue netEntityPackageQueue)
	{
		this.netEntityPackageQueue = netEntityPackageQueue;
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000D9C0C File Offset: 0x000D7E0C
	public EntityAsyncManager.EntityCreateHandle StartCreateEntity(EntityCreationData _ecd, Action<EntityAsyncManager.EntityCreateHandle> _onComplete = null)
	{
		EntityFactory.CreateEntityOperation createEntityOperation = EntityFactory.CreateEntityAsync(_ecd);
		EntityAsyncManager.EntityCreateHandle entityCreateHandle = new EntityAsyncManager.EntityCreateHandle(this, createEntityOperation, _onComplete);
		if (!this.requestIdMap.TryAdd(entityCreateHandle.Id, entityCreateHandle))
		{
			throw new Exception(string.Format("Request already exists for entity id {0}. EntityCreationData: {1}", entityCreateHandle.Id, _ecd));
		}
		this.requestQueue.Enqueue(entityCreateHandle);
		return entityCreateHandle;
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x000D9C68 File Offset: 0x000D7E68
	public void Update()
	{
		EntityAsyncManager.EntityCreateHandle entityCreateHandle;
		while (this.requestQueue.TryPeek(out entityCreateHandle))
		{
			if (entityCreateHandle.IsCompleted)
			{
				this.requestQueue.Dequeue();
			}
			else
			{
				if (!entityCreateHandle.TryComplete())
				{
					break;
				}
				this.requestQueue.Dequeue();
			}
		}
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000D9CB0 File Offset: 0x000D7EB0
	public bool IsEntityPending(int _entityId)
	{
		EntityAsyncManager.EntityCreateHandle entityCreateHandle;
		return this.requestIdMap.TryGetValue(_entityId, out entityCreateHandle) && !entityCreateHandle.IsCompleted;
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000D9CD8 File Offset: 0x000D7ED8
	public void EnsureEntity(int _entityId)
	{
		EntityAsyncManager.EntityCreateHandle entityCreateHandle;
		if (!this.requestIdMap.TryGetValue(_entityId, out entityCreateHandle))
		{
			return;
		}
		EntityAsyncManager.LogWarning("force spawning pending entity " + entityCreateHandle.DebugInfo);
		entityCreateHandle.WaitForComplete();
		this.requestIdMap.Remove(entityCreateHandle.Id);
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000D9D24 File Offset: 0x000D7F24
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCreateEntityRequestFinalized(int id)
	{
		this.requestIdMap.Remove(id);
		this.netEntityPackageQueue.ProcessPackagesForEntity(id);
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000D9D40 File Offset: 0x000D7F40
	public void CompletePendingCreateTasks()
	{
		if (this.requestQueue.Count == 0)
		{
			return;
		}
		EntityAsyncManager.LogInfo(string.Format("completing {0} pending create tasks", this.requestQueue.Count));
		EntityAsyncManager.EntityCreateHandle entityCreateHandle;
		while (this.requestQueue.TryDequeue(out entityCreateHandle))
		{
			entityCreateHandle.WaitForComplete();
		}
		this.requestIdMap.Clear();
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000D9D9D File Offset: 0x000D7F9D
	public void Cleanup()
	{
		this.CompletePendingCreateTasks();
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000D9DA5 File Offset: 0x000D7FA5
	[Conditional("DEBUG_ENTITY_ASYNC")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogDebug(string message)
	{
		Log.Out("[EntityAsync] " + message);
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000D9DA5 File Offset: 0x000D7FA5
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogInfo(string message)
	{
		Log.Out("[EntityAsync] " + message);
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x000D9DB7 File Offset: 0x000D7FB7
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogWarning(string message)
	{
		Log.Warning("[EntityAsync] " + message);
	}

	// Token: 0x0400198B RID: 6539
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<EntityAsyncManager.EntityCreateHandle> requestQueue = new Queue<EntityAsyncManager.EntityCreateHandle>(64);

	// Token: 0x0400198C RID: 6540
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, EntityAsyncManager.EntityCreateHandle> requestIdMap = new Dictionary<int, EntityAsyncManager.EntityCreateHandle>(64);

	// Token: 0x0400198D RID: 6541
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly NetEntityPackageQueue netEntityPackageQueue;

	// Token: 0x02000488 RID: 1160
	public class EntityCreateHandle
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06002403 RID: 9219 RVA: 0x000D9DC9 File Offset: 0x000D7FC9
		// (set) Token: 0x06002404 RID: 9220 RVA: 0x000D9DD1 File Offset: 0x000D7FD1
		public bool IsCompleted
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06002405 RID: 9221 RVA: 0x000D9DDA File Offset: 0x000D7FDA
		public int Id
		{
			get
			{
				return this.op.EntityId;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06002406 RID: 9222 RVA: 0x000D9DE7 File Offset: 0x000D7FE7
		public string DebugInfo
		{
			get
			{
				return this.op.DebugEntityInfo;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06002407 RID: 9223 RVA: 0x000D9DF4 File Offset: 0x000D7FF4
		public Entity Entity
		{
			get
			{
				return this.op.entity;
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000D9E01 File Offset: 0x000D8001
		public EntityCreateHandle(EntityAsyncManager manager, EntityFactory.CreateEntityOperation op, Action<EntityAsyncManager.EntityCreateHandle> onComplete)
		{
			this.manager = manager;
			this.op = op;
			this.onComplete = onComplete;
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000D9E20 File Offset: 0x000D8020
		public bool TryComplete()
		{
			if (this.IsCompleted)
			{
				return true;
			}
			if (!this.op.IsLoadingComplete)
			{
				return false;
			}
			try
			{
				this.IsCompleted = true;
				this.op.CompleteEntity();
				Action<EntityAsyncManager.EntityCreateHandle> action = this.onComplete;
				if (action != null)
				{
					action(this);
				}
				this.onComplete = null;
			}
			finally
			{
				this.manager.OnCreateEntityRequestFinalized(this.Id);
			}
			return true;
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000D9E98 File Offset: 0x000D8098
		public Entity WaitForComplete()
		{
			this.op.WaitForLoadingComplete();
			if (!this.TryComplete())
			{
				return null;
			}
			return this.op.entity;
		}

		// Token: 0x0400198E RID: 6542
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EntityAsyncManager manager;

		// Token: 0x0400198F RID: 6543
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EntityFactory.CreateEntityOperation op;

		// Token: 0x04001990 RID: 6544
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<EntityAsyncManager.EntityCreateHandle> onComplete;
	}
}
