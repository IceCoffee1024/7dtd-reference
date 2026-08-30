using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000483 RID: 1155
public class EntityAsyncManager
{
	// Token: 0x060023D7 RID: 9175 RVA: 0x000D910B File Offset: 0x000D730B
	public EntityAsyncManager(NetEntityPackageQueue netEntityPackageQueue)
	{
		this.netEntityPackageQueue = netEntityPackageQueue;
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000D9134 File Offset: 0x000D7334
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

	// Token: 0x060023D9 RID: 9177 RVA: 0x000D9190 File Offset: 0x000D7390
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

	// Token: 0x060023DA RID: 9178 RVA: 0x000D91D8 File Offset: 0x000D73D8
	public bool IsEntityPending(int _entityId)
	{
		EntityAsyncManager.EntityCreateHandle entityCreateHandle;
		return this.requestIdMap.TryGetValue(_entityId, out entityCreateHandle) && !entityCreateHandle.IsCompleted;
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x000D9200 File Offset: 0x000D7400
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

	// Token: 0x060023DC RID: 9180 RVA: 0x000D924C File Offset: 0x000D744C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCreateEntityRequestFinalized(int id)
	{
		this.requestIdMap.Remove(id);
		this.netEntityPackageQueue.ProcessPackagesForEntity(id);
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x000D9268 File Offset: 0x000D7468
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

	// Token: 0x060023DE RID: 9182 RVA: 0x000D92C5 File Offset: 0x000D74C5
	public void Cleanup()
	{
		this.CompletePendingCreateTasks();
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x000D92CD File Offset: 0x000D74CD
	[Conditional("DEBUG_ENTITY_ASYNC")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogDebug(string message)
	{
		Log.Out("[EntityAsync] " + message);
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x000D92CD File Offset: 0x000D74CD
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogInfo(string message)
	{
		Log.Out("[EntityAsync] " + message);
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000D92DF File Offset: 0x000D74DF
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogWarning(string message)
	{
		Log.Warning("[EntityAsync] " + message);
	}

	// Token: 0x04001974 RID: 6516
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<EntityAsyncManager.EntityCreateHandle> requestQueue = new Queue<EntityAsyncManager.EntityCreateHandle>(64);

	// Token: 0x04001975 RID: 6517
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, EntityAsyncManager.EntityCreateHandle> requestIdMap = new Dictionary<int, EntityAsyncManager.EntityCreateHandle>(64);

	// Token: 0x04001976 RID: 6518
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly NetEntityPackageQueue netEntityPackageQueue;

	// Token: 0x02000484 RID: 1156
	public class EntityCreateHandle
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x000D92F1 File Offset: 0x000D74F1
		// (set) Token: 0x060023E3 RID: 9187 RVA: 0x000D92F9 File Offset: 0x000D74F9
		public bool IsCompleted
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x000D9302 File Offset: 0x000D7502
		public int Id
		{
			get
			{
				return this.op.EntityId;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x000D930F File Offset: 0x000D750F
		public string DebugInfo
		{
			get
			{
				return this.op.DebugEntityInfo;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000D931C File Offset: 0x000D751C
		public Entity Entity
		{
			get
			{
				return this.op.entity;
			}
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x000D9329 File Offset: 0x000D7529
		public EntityCreateHandle(EntityAsyncManager manager, EntityFactory.CreateEntityOperation op, Action<EntityAsyncManager.EntityCreateHandle> onComplete)
		{
			this.manager = manager;
			this.op = op;
			this.onComplete = onComplete;
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000D9348 File Offset: 0x000D7548
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

		// Token: 0x060023E9 RID: 9193 RVA: 0x000D93C0 File Offset: 0x000D75C0
		public Entity WaitForComplete()
		{
			this.op.WaitForLoadingComplete();
			if (!this.TryComplete())
			{
				return null;
			}
			return this.op.entity;
		}

		// Token: 0x04001977 RID: 6519
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EntityAsyncManager manager;

		// Token: 0x04001978 RID: 6520
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EntityFactory.CreateEntityOperation op;

		// Token: 0x04001979 RID: 6521
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<EntityAsyncManager.EntityCreateHandle> onComplete;
	}
}
