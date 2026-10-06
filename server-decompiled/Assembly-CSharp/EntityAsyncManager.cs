using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000489 RID: 1161
public class EntityAsyncManager
{
	// Token: 0x060023EB RID: 9195 RVA: 0x000D9CFF File Offset: 0x000D7EFF
	public EntityAsyncManager(NetEntityPackageQueue netEntityPackageQueue)
	{
		this.netEntityPackageQueue = netEntityPackageQueue;
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x000D9D28 File Offset: 0x000D7F28
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

	// Token: 0x060023ED RID: 9197 RVA: 0x000D9D84 File Offset: 0x000D7F84
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

	// Token: 0x060023EE RID: 9198 RVA: 0x000D9DCC File Offset: 0x000D7FCC
	public bool IsEntityPending(int _entityId)
	{
		EntityAsyncManager.EntityCreateHandle entityCreateHandle;
		return this.requestIdMap.TryGetValue(_entityId, out entityCreateHandle) && !entityCreateHandle.IsCompleted;
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000D9DF4 File Offset: 0x000D7FF4
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

	// Token: 0x060023F0 RID: 9200 RVA: 0x000D9E40 File Offset: 0x000D8040
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCreateEntityRequestFinalized(int id)
	{
		this.requestIdMap.Remove(id);
		this.netEntityPackageQueue.ProcessPackagesForEntity(id);
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x000D9E5C File Offset: 0x000D805C
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

	// Token: 0x060023F2 RID: 9202 RVA: 0x000D9EB9 File Offset: 0x000D80B9
	public void Cleanup()
	{
		this.CompletePendingCreateTasks();
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000D9EC1 File Offset: 0x000D80C1
	[Conditional("DEBUG_ENTITY_ASYNC")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogDebug(string message)
	{
		Log.Out("[EntityAsync] " + message);
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000D9EC1 File Offset: 0x000D80C1
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogInfo(string message)
	{
		Log.Out("[EntityAsync] " + message);
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000D9ED3 File Offset: 0x000D80D3
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogWarning(string message)
	{
		Log.Warning("[EntityAsync] " + message);
	}

	// Token: 0x04001982 RID: 6530
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<EntityAsyncManager.EntityCreateHandle> requestQueue = new Queue<EntityAsyncManager.EntityCreateHandle>(64);

	// Token: 0x04001983 RID: 6531
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, EntityAsyncManager.EntityCreateHandle> requestIdMap = new Dictionary<int, EntityAsyncManager.EntityCreateHandle>(64);

	// Token: 0x04001984 RID: 6532
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly NetEntityPackageQueue netEntityPackageQueue;

	// Token: 0x0200048A RID: 1162
	public class EntityCreateHandle
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000D9EE5 File Offset: 0x000D80E5
		// (set) Token: 0x060023F7 RID: 9207 RVA: 0x000D9EED File Offset: 0x000D80ED
		public bool IsCompleted
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000D9EF6 File Offset: 0x000D80F6
		public int Id
		{
			get
			{
				return this.op.EntityId;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060023F9 RID: 9209 RVA: 0x000D9F03 File Offset: 0x000D8103
		public string DebugInfo
		{
			get
			{
				return this.op.DebugEntityInfo;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x000D9F10 File Offset: 0x000D8110
		public Entity Entity
		{
			get
			{
				return this.op.entity;
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x000D9F1D File Offset: 0x000D811D
		public EntityCreateHandle(EntityAsyncManager manager, EntityFactory.CreateEntityOperation op, Action<EntityAsyncManager.EntityCreateHandle> onComplete)
		{
			this.manager = manager;
			this.op = op;
			this.onComplete = onComplete;
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x000D9F3C File Offset: 0x000D813C
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

		// Token: 0x060023FD RID: 9213 RVA: 0x000D9FB4 File Offset: 0x000D81B4
		public Entity WaitForComplete()
		{
			this.op.WaitForLoadingComplete();
			if (!this.TryComplete())
			{
				return null;
			}
			return this.op.entity;
		}

		// Token: 0x04001985 RID: 6533
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EntityAsyncManager manager;

		// Token: 0x04001986 RID: 6534
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EntityFactory.CreateEntityOperation op;

		// Token: 0x04001987 RID: 6535
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<EntityAsyncManager.EntityCreateHandle> onComplete;
	}
}
