using System;
using System.Collections.Generic;

// Token: 0x02000796 RID: 1942
public class NetEntityPackageQueue
{
	// Token: 0x060039DC RID: 14812 RVA: 0x00179B5B File Offset: 0x00177D5B
	public NetEntityPackageQueue(World world)
	{
		this.world = world;
	}

	// Token: 0x060039DD RID: 14813 RVA: 0x00179B77 File Offset: 0x00177D77
	public bool HasPackagesForEntity(int entityId)
	{
		return this.entityPackageQueues.ContainsKey(entityId);
	}

	// Token: 0x060039DE RID: 14814 RVA: 0x00179B88 File Offset: 0x00177D88
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

	// Token: 0x060039DF RID: 14815 RVA: 0x00179BC4 File Offset: 0x00177DC4
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

	// Token: 0x060039E0 RID: 14816 RVA: 0x00179C08 File Offset: 0x00177E08
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

	// Token: 0x04002F81 RID: 12161
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly World world;

	// Token: 0x04002F82 RID: 12162
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, Queue<NetPackage>> entityPackageQueues = new Dictionary<int, Queue<NetPackage>>(64);
}
