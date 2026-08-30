using System;

// Token: 0x020003A6 RID: 934
public class DynamicMeshSyncRequest
{
	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06001BD3 RID: 7123 RVA: 0x000A4EFC File Offset: 0x000A30FC
	public int SecondsAlive
	{
		get
		{
			return (int)(DateTime.Now - this.Created).TotalSeconds;
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x000A4F24 File Offset: 0x000A3124
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

	// Token: 0x06001BD5 RID: 7125 RVA: 0x000A4F5E File Offset: 0x000A315E
	public static DynamicMeshSyncRequest Create(DynamicMeshItem item, bool isDelete)
	{
		return new DynamicMeshSyncRequest
		{
			Item = item,
			IsDelete = isDelete
		};
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x000A4F73 File Offset: 0x000A3173
	public static DynamicMeshSyncRequest Create(DynamicMeshItem item, bool isDelete, int clientId)
	{
		return new DynamicMeshSyncRequest
		{
			Item = item,
			IsDelete = isDelete,
			ClientId = clientId
		};
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x000A4F8F File Offset: 0x000A318F
	public bool TryGetData()
	{
		if (DynamicMeshThread.ChunkDataQueue.CollectBytes(this.Item.Key, out this.Data, out this.Length))
		{
			this.HasData = true;
			return true;
		}
		return false;
	}

	// Token: 0x040011F2 RID: 4594
	public DynamicMeshItem Item;

	// Token: 0x040011F3 RID: 4595
	public bool IsDelete;

	// Token: 0x040011F4 RID: 4596
	public bool SyncComplete;

	// Token: 0x040011F5 RID: 4597
	public byte[] Data;

	// Token: 0x040011F6 RID: 4598
	public bool HasData;

	// Token: 0x040011F7 RID: 4599
	public int Length;

	// Token: 0x040011F8 RID: 4600
	public int ClientId = -1;

	// Token: 0x040011F9 RID: 4601
	public DateTime? Initiated;

	// Token: 0x040011FA RID: 4602
	public DateTime Created;
}
