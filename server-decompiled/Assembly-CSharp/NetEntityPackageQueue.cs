using System;
using System.Collections.Generic;

// Token: 0x0200078C RID: 1932
public class NetEntityPackageQueue
{
	// Token: 0x06003984 RID: 14724 RVA: 0x001778DB File Offset: 0x00175ADB
	public NetEntityPackageQueue(World world)
	{
		this.world = world;
	}

	// Token: 0x06003985 RID: 14725 RVA: 0x001778F7 File Offset: 0x00175AF7
	public bool HasPackagesForEntity(int entityId)
	{
		return this.entityPackageQueues.ContainsKey(entityId);
	}

	// Token: 0x06003986 RID: 14726 RVA: 0x00177908 File Offset: 0x00175B08
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

	// Token: 0x06003987 RID: 14727 RVA: 0x00177944 File Offset: 0x00175B44
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

	// Token: 0x06003988 RID: 14728 RVA: 0x00177988 File Offset: 0x00175B88
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

	// Token: 0x04002F08 RID: 12040
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly World world;

	// Token: 0x04002F09 RID: 12041
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, Queue<NetPackage>> entityPackageQueues = new Dictionary<int, Queue<NetPackage>>(64);
}
