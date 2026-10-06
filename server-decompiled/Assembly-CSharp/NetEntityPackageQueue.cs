using System;
using System.Collections.Generic;

// Token: 0x02000799 RID: 1945
public class NetEntityPackageQueue
{
	// Token: 0x06003A1F RID: 14879 RVA: 0x0017A313 File Offset: 0x00178513
	public NetEntityPackageQueue(World world)
	{
		this.world = world;
	}

	// Token: 0x06003A20 RID: 14880 RVA: 0x0017A32F File Offset: 0x0017852F
	public bool HasPackagesForEntity(int entityId)
	{
		return this.entityPackageQueues.ContainsKey(entityId);
	}

	// Token: 0x06003A21 RID: 14881 RVA: 0x0017A340 File Offset: 0x00178540
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

	// Token: 0x06003A22 RID: 14882 RVA: 0x0017A37C File Offset: 0x0017857C
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

	// Token: 0x06003A23 RID: 14883 RVA: 0x0017A3C0 File Offset: 0x001785C0
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

	// Token: 0x04002F7B RID: 12155
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly World world;

	// Token: 0x04002F7C RID: 12156
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, Queue<NetPackage>> entityPackageQueues = new Dictionary<int, Queue<NetPackage>>(64);
}
