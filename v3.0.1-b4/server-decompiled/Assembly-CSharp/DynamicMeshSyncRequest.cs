using System;

// Token: 0x020003A2 RID: 930
public class DynamicMeshSyncRequest
{
	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x000A47BC File Offset: 0x000A29BC
	public int SecondsAlive
	{
		get
		{
			return (int)(DateTime.Now - this.Created).TotalSeconds;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x000A47E4 File Offset: 0x000A29E4
	public int SecondsAttempted
	{
		get
		{
			if (this.Initiated != null)
			{
				return (int)(DateTime.Now - this.Initiated.Value).TotalSeconds;
			}
			return 0;
		}
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x000A481E File Offset: 0x000A2A1E
	public static DynamicMeshSyncRequest Create(DynamicMeshItem item, bool isDelete)
	{
		return new DynamicMeshSyncRequest
		{
			Item = item,
			IsDelete = isDelete
		};
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x000A4833 File Offset: 0x000A2A33
	public static DynamicMeshSyncRequest Create(DynamicMeshItem item, bool isDelete, int clientId)
	{
		return new DynamicMeshSyncRequest
		{
			Item = item,
			IsDelete = isDelete,
			ClientId = clientId
		};
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x000A484F File Offset: 0x000A2A4F
	public bool TryGetData()
	{
		if (DynamicMeshThread.ChunkDataQueue.CollectBytes(this.Item.Key, out this.Data, out this.Length))
		{
			this.HasData = true;
			return true;
		}
		return false;
	}

	// Token: 0x040011DD RID: 4573
	public DynamicMeshItem Item;

	// Token: 0x040011DE RID: 4574
	public bool IsDelete;

	// Token: 0x040011DF RID: 4575
	public bool SyncComplete;

	// Token: 0x040011E0 RID: 4576
	public byte[] Data;

	// Token: 0x040011E1 RID: 4577
	public bool HasData;

	// Token: 0x040011E2 RID: 4578
	public int Length;

	// Token: 0x040011E3 RID: 4579
	public int ClientId = -1;

	// Token: 0x040011E4 RID: 4580
	public DateTime? Initiated;

	// Token: 0x040011E5 RID: 4581
	public DateTime Created;
}
