using System;

// Token: 0x020003A9 RID: 937
public class DynamicMeshSyncRequest
{
	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06001BCC RID: 7116 RVA: 0x000A513C File Offset: 0x000A333C
	public int SecondsAlive
	{
		get
		{
			return (int)(DateTime.Now - this.Created).TotalSeconds;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06001BCD RID: 7117 RVA: 0x000A5164 File Offset: 0x000A3364
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

	// Token: 0x06001BCE RID: 7118 RVA: 0x000A519E File Offset: 0x000A339E
	public static DynamicMeshSyncRequest Create(DynamicMeshItem item, bool isDelete)
	{
		return new DynamicMeshSyncRequest
		{
			Item = item,
			IsDelete = isDelete
		};
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x000A51B3 File Offset: 0x000A33B3
	public static DynamicMeshSyncRequest Create(DynamicMeshItem item, bool isDelete, int clientId)
	{
		return new DynamicMeshSyncRequest
		{
			Item = item,
			IsDelete = isDelete,
			ClientId = clientId
		};
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x000A51CF File Offset: 0x000A33CF
	public bool TryGetData()
	{
		if (DynamicMeshThread.ChunkDataQueue.CollectBytes(this.Item.Key, out this.Data, out this.Length))
		{
			this.HasData = true;
			return true;
		}
		return false;
	}

	// Token: 0x040011EB RID: 4587
	public DynamicMeshItem Item;

	// Token: 0x040011EC RID: 4588
	public bool IsDelete;

	// Token: 0x040011ED RID: 4589
	public bool SyncComplete;

	// Token: 0x040011EE RID: 4590
	public byte[] Data;

	// Token: 0x040011EF RID: 4591
	public bool HasData;

	// Token: 0x040011F0 RID: 4592
	public int Length;

	// Token: 0x040011F1 RID: 4593
	public int ClientId = -1;

	// Token: 0x040011F2 RID: 4594
	public DateTime? Initiated;

	// Token: 0x040011F3 RID: 4595
	public DateTime Created;
}
