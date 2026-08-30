using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200038C RID: 908
[Preserve]
public class NetPackageRegionMetaData : DynamicMeshServerData
{
	// Token: 0x06001B07 RID: 6919 RVA: 0x000A093F File Offset: 0x0009EB3F
	public NetPackageRegionMetaData()
	{
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x000A0952 File Offset: 0x0009EB52
	public NetPackageRegionMetaData(DynamicMeshRegion region)
	{
		this.X = region.WorldPosition.x;
		this.Z = region.WorldPosition.z;
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x000A0988 File Offset: 0x0009EB88
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
				this.ChunksWithData.Count.ToString(),
				"   length: ",
				this.GetLength().ToString()
			}));
		}
		return true;
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x000A0A10 File Offset: 0x0009EC10
	public override int GetLength()
	{
		return 12 + this.ChunksWithData.Count * 8;
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x000A0A24 File Offset: 0x0009EC24
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

	// Token: 0x06001B0C RID: 6924 RVA: 0x000A0B04 File Offset: 0x0009ED04
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

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001B0D RID: 6925 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001B0E RID: 6926 RVA: 0x000A0B58 File Offset: 0x0009ED58
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

	// Token: 0x04001168 RID: 4456
	public List<Vector2i> ChunksWithData = new List<Vector2i>();
}
