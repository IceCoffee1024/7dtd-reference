using System;
using System.Collections.Generic;

// Token: 0x020007A1 RID: 1953
public class NetEntityPackageQueue
{
	// Token: 0x06003A36 RID: 14902 RVA: 0x0017B2AB File Offset: 0x001794AB
	public NetEntityPackageQueue(World world)
	{
		this.world = world;
	}

	// Token: 0x06003A37 RID: 14903 RVA: 0x0017B2C7 File Offset: 0x001794C7
	public bool HasPackagesForEntity(int entityId)
	{
		return this.entityPackageQueues.ContainsKey(entityId);
	}

	// Token: 0x06003A38 RID: 14904 RVA: 0x0017B2D8 File Offset: 0x001794D8
	public void EnqueueNetPackageForEntity(int entityId, NetPackage netPackage)
	{
		Queue<NetPackage> queue;
		if (!this.entityPackageQueues.TryGetValue(entityId, out queue))
		{
			queue = new Queue<NetPackage>(10);
			this.entityPackageQueues.Add(entityId, queue);
		}
		queue.Enqueue(netPackage);
	}

	// Token: 0x06003A39 RID: 14905 RVA: 0x0017B314 File Offset: 0x00179514
	public void ProcessPackagesForEntity(int entityId)
	{
		Queue<NetPackage> queue;
		if (!this.entityPackageQueues.Remove(entityId, out queue))
		{
			return;
		}
		NetPackage netPackage;
		while (queue.TryDequeue(out netPackage))
		{
			netPackage.ProcessPackage(this.world, GameManager.Instance);
			NetPackageManager.FreePackage(netPackage);
		}
	}

	// Token: 0x06003A3A RID: 14906 RVA: 0x0017B358 File Offset: 0x00179558
	public void Cleanup()
	{
		foreach (Queue<NetPackage> queue in this.entityPackageQueues.Values)
		{
			NetPackage netPackage;
			while (queue.TryDequeue(out netPackage))
			{
				NetPackageManager.FreePackage(netPackage);
			}
		}
		this.entityPackageQueues.Clear();
	}

	// Token: 0x04002F97 RID: 12183
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly World world;

	// Token: 0x04002F98 RID: 12184
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, Queue<NetPackage>> entityPackageQueues = new Dictionary<int, Queue<NetPackage>>(64);
}
