using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003A1 RID: 929
public class DynamicMeshClientConnection
{
	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x000A4784 File Offset: 0x000A2984
	public bool TriggerSend
	{
		get
		{
			return (DateTime.Now - this.LastSend).TotalSeconds > 1.0;
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06001BB5 RID: 7093 RVA: 0x000A47B4 File Offset: 0x000A29B4
	public bool HasMessage
	{
		get
		{
			return this.ItemsToSend.Count > 0;
		}
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x000A47C4 File Offset: 0x000A29C4
	public DynamicMeshClientConnection(int entityId)
	{
		this.EntityId = entityId;
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x000A4854 File Offset: 0x000A2A54
	public void AddToQueue(DynamicMeshSyncRequest package)
	{
		ValueTuple<int, int> valueTuple = new ValueTuple<int, int>(DynamicMeshUnity.RoundRegion(package.Item.WorldPosition.x), DynamicMeshUnity.RoundRegion(package.Item.WorldPosition.z));
		ConcurrentQueue<DynamicMeshSyncRequest> orAdd = this.ItemsToSend.GetOrAdd(valueTuple, new ConcurrentQueue<DynamicMeshSyncRequest>());
		orAdd.Enqueue(package);
		this.SendMessage = DynamicMeshServer.AutoSend || this.SendMessage || orAdd.Count == 1;
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x000A48CC File Offset: 0x000A2ACC
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

	// Token: 0x06001BB9 RID: 7097 RVA: 0x000A4918 File Offset: 0x000A2B18
	public void UpdateItemsToSend(NetPackageDynamicClientArrive package)
	{
		DynamicMeshClientConnection.UpdateItemsToSend(this, package);
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x000A4924 File Offset: 0x000A2B24
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

	// Token: 0x06001BBB RID: 7099 RVA: 0x000A4C04 File Offset: 0x000A2E04
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

	// Token: 0x040011DB RID: 4571
	public int EntityId;

	// Token: 0x040011DC RID: 4572
	public ConcurrentDictionary<ValueTuple<int, int>, ConcurrentQueue<DynamicMeshSyncRequest>> ItemsToSend = new ConcurrentDictionary<ValueTuple<int, int>, ConcurrentQueue<DynamicMeshSyncRequest>>();

	// Token: 0x040011DD RID: 4573
	public ConcurrentQueue<long> RequestedChunks = new ConcurrentQueue<long>();

	// Token: 0x040011DE RID: 4574
	public List<long> FinalChunks = new List<long>();

	// Token: 0x040011DF RID: 4575
	public long CurrentRequestedChunk = long.MaxValue;

	// Token: 0x040011E0 RID: 4576
	public DateTime RequestTime = DateTime.Now.AddDays(-1.0);

	// Token: 0x040011E1 RID: 4577
	public DateTime LastSend = DateTime.Now.AddDays(-1.0);

	// Token: 0x040011E2 RID: 4578
	public bool SendMessage;

	// Token: 0x040011E3 RID: 4579
	public ValueTuple<int, int> LastKey = ValueTuple.Create<int, int>(0, 0);
}
