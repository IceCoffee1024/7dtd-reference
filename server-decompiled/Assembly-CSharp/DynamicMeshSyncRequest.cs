using System;

// Token: 0x020003AB RID: 939
public class DynamicMeshSyncRequest
{
	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x000A534C File Offset: 0x000A354C
	public int SecondsAlive
	{
		get
		{
			return (int)(DateTime.Now - this.Created).TotalSeconds;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06001BD1 RID: 7121 RVA: 0x000A5374 File Offset: 0x000A3574
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

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000A53AE File Offset: 0x000A35AE
	public static DynamicMeshSyncRequest Create(DynamicMeshItem item, bool isDelete)
	{
		return new DynamicMeshSyncRequest
		{
			Item = item,
			IsDelete = isDelete
		};
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x000A53C3 File Offset: 0x000A35C3
	public static DynamicMeshSyncRequest Create(DynamicMeshItem item, bool isDelete, int clientId)
	{
		return new DynamicMeshSyncRequest
		{
			Item = item,
			IsDelete = isDelete,
			ClientId = clientId
		};
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x000A53DF File Offset: 0x000A35DF
	public bool TryGetData()
	{
		if (DynamicMeshThread.ChunkDataQueue.CollectBytes(this.Item.Key, out this.Data, out this.Length))
		{
			this.HasData = true;
			return true;
		}
		return false;
	}

	// Token: 0x040011FB RID: 4603
	public DynamicMeshItem Item;

	// Token: 0x040011FC RID: 4604
	public bool IsDelete;

	// Token: 0x040011FD RID: 4605
	public bool SyncComplete;

	// Token: 0x040011FE RID: 4606
	public byte[] Data;

	// Token: 0x040011FF RID: 4607
	public bool HasData;

	// Token: 0x04001200 RID: 4608
	public int Length;

	// Token: 0x04001201 RID: 4609
	public int ClientId = -1;

	// Token: 0x04001202 RID: 4610
	public DateTime? Initiated;

	// Token: 0x04001203 RID: 4611
	public DateTime Created;
}
