using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000399 RID: 921
public static class DynamicMeshServer
{
	// Token: 0x06001B84 RID: 7044 RVA: 0x000A36E4 File Offset: 0x000A18E4
	public static void OnClientConnect(ClientInfo info)
	{
		DynamicMeshServer.GetData(info.entityId);
		if (DynamicMeshManager.DoLog)
		{
			DynamicMeshManager.LogMsg("User " + info.entityId.ToString() + " joined dynamic meshes: " + DynamicMeshServer.ClientData.Count.ToString());
		}
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x000A3738 File Offset: 0x000A1938
	public static void OnClientDisconnect(ClientInfo info)
	{
		int entityId = info.entityId;
		Log.Out("Client disconnected from dy mesh: Id: " + entityId.ToString() + " Total: " + DynamicMeshServer.ClientData.Count.ToString());
		try
		{
			DynamicMeshServer.ClientData.Remove(entityId);
		}
		catch (Exception ex)
		{
			Log.Error("Client removal error: " + ex.Message);
		}
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x000A37B0 File Offset: 0x000A19B0
	public static void CleanUp()
	{
		DynamicMeshServer.ClientData.Clear();
		DynamicMeshServer.DelayedClientChecks.Clear();
		DynamicMeshItem dynamicMeshItem;
		while (DynamicMeshServer.SyncReleaseQueue.TryDequeue(out dynamicMeshItem))
		{
		}
		DynamicMeshSyncRequest dynamicMeshSyncRequest;
		while (DynamicMeshServer.SyncRequests.TryDequeue(out dynamicMeshSyncRequest))
		{
		}
		DynamicMeshServer.ActiveSyncs.Clear();
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x000A37F8 File Offset: 0x000A19F8
	public static void SendToAllClients(DynamicMeshItem item, bool isDelete)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount() == 0)
		{
			return;
		}
		DynamicMeshSyncRequest dynamicMeshSyncRequest = DynamicMeshSyncRequest.Create(item, isDelete);
		DynamicMeshServer.SyncRequests.Enqueue(dynamicMeshSyncRequest);
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x000A3825 File Offset: 0x000A1A25
	public static void SyncRelease(DynamicMeshItem item)
	{
		if (item == null)
		{
			return;
		}
		DynamicMeshServer.SyncReleaseQueue.Enqueue(item);
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x000A3838 File Offset: 0x000A1A38
	public static void Update()
	{
		for (;;)
		{
			DynamicMeshItem item;
			if (!DynamicMeshServer.SyncReleaseQueue.TryDequeue(out item))
			{
				break;
			}
			DynamicMeshSyncRequest dynamicMeshSyncRequest = DynamicMeshServer.ActiveSyncs.FirstOrDefault<DynamicMeshSyncRequest>((DynamicMeshSyncRequest d) => d.Item.WorldPosition.x == item.WorldPosition.x && d.Item.WorldPosition.z == item.WorldPosition.z);
			if (dynamicMeshSyncRequest == null)
			{
				Log.Warning("Active sync could not be found to be cleared: " + item.ToDebugLocation());
			}
			else
			{
				dynamicMeshSyncRequest.SyncComplete = true;
			}
		}
		DynamicMeshSyncRequest dynamicMeshSyncRequest2;
		if (DynamicMeshServer.ActiveSyncs.Count < DynamicMeshServer.MaxActiveSyncs && DynamicMeshServer.SyncRequests.TryDequeue(out dynamicMeshSyncRequest2))
		{
			dynamicMeshSyncRequest2.Initiated = new DateTime?(DateTime.Now);
			DynamicMeshServer.ActiveSyncs.Add(dynamicMeshSyncRequest2);
		}
		for (int i = DynamicMeshServer.ActiveSyncs.Count - 1; i >= 0; i--)
		{
			DynamicMeshSyncRequest dynamicMeshSyncRequest3 = DynamicMeshServer.ActiveSyncs[i];
			if (dynamicMeshSyncRequest3.SyncComplete || SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount() == 0)
			{
				DynamicMeshThread.ChunkDataQueue.ManuallyReleaseBytes(dynamicMeshSyncRequest3.Data);
				if (DynamicMeshManager.DoLogNet)
				{
					int num = (int)(DateTime.Now - dynamicMeshSyncRequest3.Initiated.Value).TotalMilliseconds;
					Log.Out(string.Concat(new string[]
					{
						"Package for ",
						(dynamicMeshSyncRequest3.ClientId == -1) ? "all" : dynamicMeshSyncRequest3.ClientId.ToString(),
						" took ",
						num.ToString(),
						"ms for ",
						dynamicMeshSyncRequest3.Length.ToString(),
						" bytes"
					}));
				}
				DynamicMeshServer.ActiveSyncs.RemoveAt(i);
			}
			else if (!dynamicMeshSyncRequest3.HasData)
			{
				if (dynamicMeshSyncRequest3.IsDelete || dynamicMeshSyncRequest3.TryGetData())
				{
					NetPackageDynamicMesh package = NetPackageManager.GetPackage<NetPackageDynamicMesh>();
					package.Setup(dynamicMeshSyncRequest3.Item, dynamicMeshSyncRequest3.Data);
					package.PresumedLength = dynamicMeshSyncRequest3.Length;
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, false, dynamicMeshSyncRequest3.ClientId, -1, -1, null, 192, false);
				}
			}
			else if (dynamicMeshSyncRequest3.SecondsAttempted > 20)
			{
				Log.Warning("Sync waited more than 20 seconds. Removing...");
				DynamicMeshServer.ActiveSyncs.Remove(dynamicMeshSyncRequest3);
				break;
			}
		}
		using (Dictionary<int, DynamicMeshClientConnection>.ValueCollection.Enumerator enumerator = DynamicMeshServer.ClientData.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DynamicMeshClientConnection con = enumerator.Current;
				if ((con.SendMessage || con.TriggerSend) && con.ItemsToSend.Count > 0)
				{
					con.SendMessage = false;
					ConcurrentQueue<DynamicMeshSyncRequest> concurrentQueue = null;
					if (!con.ItemsToSend.ContainsKey(con.LastKey))
					{
						EntityPlayer entityPlayer = GameManager.Instance.World.GetPlayers().FirstOrDefault<EntityPlayer>((EntityPlayer d) => d.entityId == con.EntityId);
						Vector3 playerPos = ((entityPlayer == null) ? Vector3.zero : entityPlayer.GetPosition());
						ValueTuple<int, int> valueTuple = (from d in con.ItemsToSend.Keys.ToList<ValueTuple<int, int>>()
							orderby Math.Abs(Mathf.Sqrt(Mathf.Pow(playerPos.x - (float)d.Item1, 2f) + Mathf.Pow(playerPos.z - (float)d.Item2, 2f)))
							select d).FirstOrDefault<ValueTuple<int, int>>();
						con.LastKey = valueTuple;
						if (DynamicMeshManager.DoLog || DynamicMeshManager.DoLogNet)
						{
							float num2 = Math.Abs(Mathf.Sqrt(Mathf.Pow(playerPos.x - (float)valueTuple.Item1, 2f) + Mathf.Pow(playerPos.z - (float)valueTuple.Item2, 2f)));
							int count = con.ItemsToSend[valueTuple].Count;
							string[] array = new string[6];
							array[0] = "Switching key to ";
							int num3 = 1;
							ValueTuple<int, int> valueTuple2 = valueTuple;
							array[num3] = valueTuple2.ToString();
							array[2] = " Dist: ";
							array[3] = num2.ToString();
							array[4] = "   Items: ";
							array[5] = count.ToString();
							Log.Out(string.Concat(array));
						}
					}
					DynamicMeshSyncRequest dynamicMeshSyncRequest4;
					if (!con.ItemsToSend.TryGetValue(con.LastKey, out concurrentQueue))
					{
						if (!DynamicMeshManager.DoLog || DynamicMeshManager.DoLogNet)
						{
							Log.Out("Could not find last key to skipping");
						}
					}
					else if (concurrentQueue.Count == 0)
					{
						con.ItemsToSend.TryRemove(con.LastKey, out concurrentQueue);
					}
					else if (concurrentQueue.TryDequeue(out dynamicMeshSyncRequest4))
					{
						DynamicMeshServer.SyncRequests.Enqueue(dynamicMeshSyncRequest4);
						con.LastSend = DateTime.Now;
					}
				}
			}
		}
		if (DynamicMeshServer.debugTime < Time.time)
		{
			if (DynamicMeshManager.DoLog || DynamicMeshManager.DoLogNet)
			{
				DynamicMeshManager.LogMsg(string.Concat(new string[]
				{
					"Dyn Mesh Server update. ",
					DynamicMeshServer.ClientData.Count.ToString(),
					"   Prefabs: ",
					DynamicMeshManager.Instance.PrefabCheck.ToString(),
					" buff: ",
					DynamicMeshManager.Instance.BufferRegionLoadRequests.Count.ToString(),
					"  chunkData: ",
					DynamicMeshManager.Instance.ChunkMeshData.Count.ToString(),
					"   Primary: ",
					DynamicMeshThread.PrimaryQueue.Count.ToString(),
					"   Secondary: ",
					DynamicMeshThread.SecondaryQueue.Count.ToString()
				}));
			}
			DynamicMeshServer.debugTime = Time.time + 10f;
		}
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x000A3E0C File Offset: 0x000A200C
	public static void ChunkRequested(long key, bool isFinal, ClientInfo client)
	{
		DynamicMeshClientConnection data = DynamicMeshServer.GetData(client.entityId);
		data.RequestedChunks.Enqueue(key);
		if (isFinal)
		{
			data.FinalChunks.Add(key);
		}
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x000A3E40 File Offset: 0x000A2040
	public static DynamicMeshClientConnection GetData(int entityId)
	{
		DynamicMeshClientConnection dynamicMeshClientConnection;
		if (!DynamicMeshServer.ClientData.TryGetValue(entityId, out dynamicMeshClientConnection))
		{
			dynamicMeshClientConnection = new DynamicMeshClientConnection(entityId);
			DynamicMeshServer.ClientData.Add(entityId, dynamicMeshClientConnection);
		}
		return dynamicMeshClientConnection;
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x000A3E70 File Offset: 0x000A2070
	public static DynamicMeshClientConnection GetData(NetPackage package)
	{
		return DynamicMeshServer.GetData(package.Sender.entityId);
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x000A3E82 File Offset: 0x000A2082
	public static void ClientMessageRecieved(NetPackageDynamicClientArrive package)
	{
		DynamicMeshServer.GetData(package).UpdateItemsToSend(package);
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x000A3E90 File Offset: 0x000A2090
	public static void ProcessDelayedPackages()
	{
		if (DynamicMeshManager.DoLog)
		{
			DynamicMeshManager.LogMsg("==Processing delayed packages: " + DynamicMeshServer.DelayedClientChecks.Count.ToString());
		}
		foreach (NetPackageDynamicClientArrive netPackageDynamicClientArrive in DynamicMeshServer.DelayedClientChecks)
		{
			DynamicMeshServer.GetData(netPackageDynamicClientArrive).UpdateItemsToSend(netPackageDynamicClientArrive);
		}
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x000A3F10 File Offset: 0x000A2110
	public static void ClientReadyForNextMesh(NetPackageDynamicMesh package)
	{
		DynamicMeshServer.GetData(package).SendMessage = true;
	}

	// Token: 0x040011B8 RID: 4536
	public static Dictionary<int, DynamicMeshClientConnection> ClientData = new Dictionary<int, DynamicMeshClientConnection>();

	// Token: 0x040011B9 RID: 4537
	public static List<NetPackageDynamicClientArrive> DelayedClientChecks = new List<NetPackageDynamicClientArrive>();

	// Token: 0x040011BA RID: 4538
	public static bool AutoSend = false;

	// Token: 0x040011BB RID: 4539
	public static DynamicMeshServerType ConnectionType = DynamicMeshServerType.Mesh;

	// Token: 0x040011BC RID: 4540
	[PublicizedFrom(EAccessModifier.Private)]
	public static float debugTime = 0f;

	// Token: 0x040011BD RID: 4541
	public static bool ResendPackages = true;

	// Token: 0x040011BE RID: 4542
	public static bool ShowSender = true;

	// Token: 0x040011BF RID: 4543
	public static ConcurrentQueue<DynamicMeshSyncRequest> SyncRequests = new ConcurrentQueue<DynamicMeshSyncRequest>();

	// Token: 0x040011C0 RID: 4544
	public static List<DynamicMeshSyncRequest> ActiveSyncs = new List<DynamicMeshSyncRequest>();

	// Token: 0x040011C1 RID: 4545
	[PublicizedFrom(EAccessModifier.Private)]
	public static ConcurrentQueue<DynamicMeshItem> SyncReleaseQueue = new ConcurrentQueue<DynamicMeshItem>();

	// Token: 0x040011C2 RID: 4546
	public static int MaxActiveSyncs = 10;
}
