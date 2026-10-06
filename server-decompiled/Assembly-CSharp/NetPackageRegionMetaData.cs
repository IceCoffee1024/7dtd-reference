using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000393 RID: 915
[Preserve]
public class NetPackageRegionMetaData : DynamicMeshServerData
{
	// Token: 0x06001B1F RID: 6943 RVA: 0x000A12FF File Offset: 0x0009F4FF
	public NetPackageRegionMetaData()
	{
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x000A1312 File Offset: 0x0009F512
	public NetPackageRegionMetaData(DynamicMeshRegion region)
	{
		this.X = region.WorldPosition.x;
		this.Z = region.WorldPosition.z;
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x000A1348 File Offset: 0x0009F548
	public override bool Prechecks()
	{
		if (DynamicMeshManager.DoLog)
		{
			DynamicMeshManager.LogMsg(string.Concat(new string[]
			{
				"Sending region data: ",
				this.X.ToString(),
				",",
				this.Z.ToString(),
				"  Items: ",
				this.ChunksWithData.Count.ToString()
			}));
		}
		return true;
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x000A13B8 File Offset: 0x0009F5B8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!DynamicMeshManager.CONTENT_ENABLED)
		{
			return;
		}
		if (DynamicMeshManager.Instance == null)
		{
			return;
		}
		DynamicMeshRegion region = DynamicMeshManager.Instance.GetRegion(this.X, this.Z);
		if (DynamicMeshManager.DoLog)
		{
			string text = "Recieved Region meta data ";
			Vector3i worldPosition = region.WorldPosition;
			DynamicMeshManager.LogMsg(text + worldPosition.ToString() + " items: " + this.ChunksWithData.Count.ToString());
		}
		foreach (Vector2i vector2i in this.ChunksWithData)
		{
			DynamicMeshManager.Instance.AddChunk(DynamicMeshUnity.GetItemKey(vector2i.x, vector2i.y), false, false, null);
		}
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x000A1498 File Offset: 0x0009F698
	public override void read(PooledBinaryReader reader)
	{
		this.X = reader.ReadInt32();
		this.Z = reader.ReadInt32();
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.ChunksWithData.Add(new Vector2i(reader.ReadInt32(), reader.ReadInt32()));
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06001B24 RID: 6948 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x000A14EC File Offset: 0x0009F6EC
	public override void write(PooledBinaryWriter writer)
	{
		base.write(writer);
		writer.Write(this.X);
		writer.Write(this.Z);
		writer.Write(this.ChunksWithData.Count);
		for (int i = 0; i < this.ChunksWithData.Count; i++)
		{
			writer.Write(this.ChunksWithData[i].x);
			writer.Write(this.ChunksWithData[i].y);
		}
	}

	// Token: 0x04001176 RID: 4470
	public List<Vector2i> ChunksWithData = new List<Vector2i>();
}
