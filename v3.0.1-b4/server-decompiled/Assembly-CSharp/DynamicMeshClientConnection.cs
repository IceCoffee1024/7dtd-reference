using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200039D RID: 925
public class DynamicMeshClientConnection
{
	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06001B97 RID: 7063 RVA: 0x000A4044 File Offset: 0x000A2244
	public bool TriggerSend
	{
		get
		{
			return (DateTime.Now - this.LastSend).TotalSeconds > 1.0;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06001B98 RID: 7064 RVA: 0x000A4074 File Offset: 0x000A2274
	public bool HasMessage
	{
		get
		{
			return this.ItemsToSend.Count > 0;
		}
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x000A4084 File Offset: 0x000A2284
	public DynamicMeshClientConnection(int entityId)
	{
		this.EntityId = entityId;
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x000A4114 File Offset: 0x000A2314
	public void AddToQueue(DynamicMeshSyncRequest package)
	{
		ValueTuple<int, int> valueTuple = new ValueTuple<int, int>(DynamicMeshUnity.RoundRegion(package.Item.WorldPosition.x), DynamicMeshUnity.RoundRegion(package.Item.WorldPosition.z));
		ConcurrentQueue<DynamicMeshSyncRequest> orAdd = this.ItemsToSend.GetOrAdd(valueTuple, new ConcurrentQueue<DynamicMeshSyncRequest>());
		orAdd.Enqueue(package);
		this.SendMessage = DynamicMeshServer.AutoSend || this.SendMessage || orAdd.Count == 1;
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x000A418C File Offset: 0x000A238C
	public bool RequestChunk()
	{
		if (this.CurrentRequestedChunk != 9223372036854775807L)
		{
			return false;
		}
		if (!this.RequestedChunks.TryDequeue(out this.CurrentRequestedChunk))
		{
			return false;
		}
		this.RequestTime = DateTime.Now;
		DynamicMeshThread.RequestChunk(this.CurrentRequestedChunk);
		return true;
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x000A41D8 File Offset: 0x000A23D8
	public void UpdateItemsToSend(NetPackageDynamicClientArrive package)
	{
		DynamicMeshClientConnection.UpdateItemsToSend(this, package);
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x000A41E4 File Offset: 0x000A23E4
	public static void UpdateItemsToSend(DynamicMeshClientConnection data, NetPackageDynamicClientArrive package)
	{
		if (DynamicMeshManager.Instance == null || DynamicMeshManager.Instance.ItemsDictionary == null)
		{
			DynamicMeshManager.LogMsg(package.Sender.playerName + " connected before the world was ready. Can not sync dymesh data. They must reconnect to start the sync");
			data.SendMessage = true;
			return;
		}
		data.SendMessage = false;
		data.ItemsToSend.Clear();
		DynamicMeshManager.LogMsg(string.Concat(new string[]
		{
			"Update items to send for ",
			package.Sender.playerName,
			" id: ",
			package.Sender.entityId.ToString(),
			"  recieved: ",
			package.Items.Count.ToString()
		}));
		EntityPlayer entityPlayer = GameManager.Instance.World.GetPlayers().FirstOrDefault<EntityPlayer>((EntityPlayer d) => d.entityId == package.Sender.entityId);
		Vector3 playerPos = ((entityPlayer == null) ? Vector3.zero : entityPlayer.GetPosition());
		playerPos.y = 0f;
		List<DynamicMeshRegion> list = DynamicMeshRegion.Regions.Values.OrderBy<DynamicMeshRegion, float>((DynamicMeshRegion d) => Math.Abs(Vector3.Distance(playerPos, d.WorldPosition.ToVector3()))).ToList<DynamicMeshRegion>();
		int num = 0;
		List<DynamicMeshItem> list2 = new List<DynamicMeshItem>(DynamicMeshManager.Instance.ItemsDictionary.Count);
		NetPackageRegionMetaData package2 = NetPackageManager.GetPackage<NetPackageRegionMetaData>();
		foreach (DynamicMeshRegion dynamicMeshRegion in list)
		{
			num += DynamicMeshClientConnection.ProcessItem(data, dynamicMeshRegion, dynamicMeshRegion.LoadedItems, package, package2);
			num += DynamicMeshClientConnection.ProcessItem(data, dynamicMeshRegion, dynamicMeshRegion.UnloadedItems, package, package2);
		}
		package2.ChunksWithData.AddRange(DynamicMeshThread.PrimaryQueue.Values.Select<DynamicMeshItem, Vector2i>((DynamicMeshItem d) => new Vector2i(d.WorldPosition.x, d.WorldPosition.z)));
		package2.ChunksWithData.AddRange(DynamicMeshThread.SecondaryQueue.Values.Select<DynamicMeshItem, Vector2i>((DynamicMeshItem d) => new Vector2i(d.WorldPosition.x, d.WorldPosition.z)));
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package2, false, data.EntityId, -1, -1, null, 192, false);
		DynamicMeshManager.LogMsg(string.Concat(new string[]
		{
			"Items to send: ",
			list2.Count.ToString(),
			"   Added: ",
			num.ToString(),
			"   chunks: ",
			package2.ChunksWithData.Count.ToString()
		}));
		data.SendMessage = data.ItemsToSend.Count > 0;
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x000A44C4 File Offset: 0x000A26C4
	[PublicizedFrom(EAccessModifier.Private)]
	public static int ProcessItem(DynamicMeshClientConnection conn, DynamicMeshRegion r, List<DynamicMeshItem> items, NetPackageDynamicClientArrive package, NetPackageRegionMetaData allChunkData)
	{
		int num = 0;
		using (List<DynamicMeshItem>.Enumerator enumerator = items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DynamicMeshItem i = enumerator.Current;
				if (i != null)
				{
					if (i.FileExists())
					{
						allChunkData.ChunksWithData.Add(new Vector2i(i.WorldPosition.x, i.WorldPosition.z));
					}
					if (!package.Items.Any<RegionItemData>((RegionItemData d) => d.X == i.WorldPosition.x && d.Z == i.WorldPosition.z && d.UpdateTime == i.UpdateTime))
					{
						DynamicMeshSyncRequest dynamicMeshSyncRequest = DynamicMeshSyncRequest.Create(i, false, conn.EntityId);
						conn.AddToQueue(dynamicMeshSyncRequest);
						num++;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x040011C6 RID: 4550
	public int EntityId;

	// Token: 0x040011C7 RID: 4551
	public ConcurrentDictionary<ValueTuple<int, int>, ConcurrentQueue<DynamicMeshSyncRequest>> ItemsToSend = new ConcurrentDictionary<ValueTuple<int, int>, ConcurrentQueue<DynamicMeshSyncRequest>>();

	// Token: 0x040011C8 RID: 4552
	public ConcurrentQueue<long> RequestedChunks = new ConcurrentQueue<long>();

	// Token: 0x040011C9 RID: 4553
	public List<long> FinalChunks = new List<long>();

	// Token: 0x040011CA RID: 4554
	public long CurrentRequestedChunk = long.MaxValue;

	// Token: 0x040011CB RID: 4555
	public DateTime RequestTime = DateTime.Now.AddDays(-1.0);

	// Token: 0x040011CC RID: 4556
	public DateTime LastSend = DateTime.Now.AddDays(-1.0);

	// Token: 0x040011CD RID: 4557
	public bool SendMessage;

	// Token: 0x040011CE RID: 4558
	public ValueTuple<int, int> LastKey = ValueTuple.Create<int, int>(0, 0);
}
