using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x0200048B RID: 1163
public class EntityAsyncManager
{
	// Token: 0x060023F1 RID: 9201 RVA: 0x000DA083 File Offset: 0x000D8283
	public EntityAsyncManager(NetEntityPackageQueue netEntityPackageQueue)
	{
		this.netEntityPackageQueue = netEntityPackageQueue;
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x000DA0AC File Offset: 0x000D82AC
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

	// Token: 0x060023F3 RID: 9203 RVA: 0x000DA108 File Offset: 0x000D8308
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

	// Token: 0x060023F4 RID: 9204 RVA: 0x000DA150 File Offset: 0x000D8350
	public bool IsEntityPending(int _entityId)
	{
		EntityAsyncManager.EntityCreateHandle entityCreateHandle;
		return this.requestIdMap.TryGetValue(_entityId, out entityCreateHandle) && !entityCreateHandle.IsCompleted;
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000DA178 File Offset: 0x000D8378
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

	// Token: 0x060023F6 RID: 9206 RVA: 0x000DA1C4 File Offset: 0x000D83C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCreateEntityRequestFinalized(int id)
	{
		this.requestIdMap.Remove(id);
		this.netEntityPackageQueue.ProcessPackagesForEntity(id);
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000DA1E0 File Offset: 0x000D83E0
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

	// Token: 0x060023F8 RID: 9208 RVA: 0x000DA23D File Offset: 0x000D843D
	public void Cleanup()
	{
		this.CompletePendingCreateTasks();
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000DA245 File Offset: 0x000D8445
	[Conditional("DEBUG_ENTITY_ASYNC")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogDebug(string message)
	{
		Log.Out("[EntityAsync] " + message);
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x000DA245 File Offset: 0x000D8445
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogInfo(string message)
	{
		Log.Out("[EntityAsync] " + message);
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000DA257 File Offset: 0x000D8457
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogWarning(string message)
	{
		Log.Warning("[EntityAsync] " + message);
	}

	// Token: 0x04001992 RID: 6546
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<EntityAsyncManager.EntityCreateHandle> requestQueue = new Queue<EntityAsyncManager.EntityCreateHandle>(64);

	// Token: 0x04001993 RID: 6547
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, EntityAsyncManager.EntityCreateHandle> requestIdMap = new Dictionary<int, EntityAsyncManager.EntityCreateHandle>(64);

	// Token: 0x04001994 RID: 6548
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly NetEntityPackageQueue netEntityPackageQueue;

	// Token: 0x0200048C RID: 1164
	public class EntityCreateHandle
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x060023FC RID: 9212 RVA: 0x000DA269 File Offset: 0x000D8469
		// (set) Token: 0x060023FD RID: 9213 RVA: 0x000DA271 File Offset: 0x000D8471
		public bool IsCompleted
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x000DA27A File Offset: 0x000D847A
		public int Id
		{
			get
			{
				return this.op.EntityId;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060023FF RID: 9215 RVA: 0x000DA287 File Offset: 0x000D8487
		public string DebugInfo
		{
			get
			{
				return this.op.DebugEntityInfo;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x000DA294 File Offset: 0x000D8494
		public Entity Entity
		{
			get
			{
				return this.op.entity;
			}
		}

		// Token: 0x06002401 RID: 9217 RVA: 0x000DA2A1 File Offset: 0x000D84A1
		public EntityCreateHandle(EntityAsyncManager manager, EntityFactory.CreateEntityOperation op, Action<EntityAsyncManager.EntityCreateHandle> onComplete)
		{
			this.manager = manager;
			this.op = op;
			this.onComplete = onComplete;
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000DA2C0 File Offset: 0x000D84C0
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

		// Token: 0x06002403 RID: 9219 RVA: 0x000DA338 File Offset: 0x000D8538
		public Entity WaitForComplete()
		{
			this.op.WaitForLoadingComplete();
			if (!this.TryComplete())
			{
				return null;
			}
			return this.op.entity;
		}

		// Token: 0x04001995 RID: 6549
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EntityAsyncManager manager;

		// Token: 0x04001996 RID: 6550
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly EntityFactory.CreateEntityOperation op;

		// Token: 0x04001997 RID: 6551
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<EntityAsyncManager.EntityCreateHandle> onComplete;
	}
}
