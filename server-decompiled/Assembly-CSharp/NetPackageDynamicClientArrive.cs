using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000397 RID: 919
[Preserve]
public class NetPackageDynamicClientArrive : NetPackage
{
	// Token: 0x06001B2A RID: 6954 RVA: 0x000A15E4 File Offset: 0x0009F7E4
	public void BuildData()
	{
		foreach (DynamicMeshItem dynamicMeshItem in DynamicMeshManager.Instance.ItemsDictionary.Values)
		{
			this.Items.Add(this.FromPool(dynamicMeshItem));
		}
		Log.Out("Client package items: " + this.Items.Count.ToString());
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x000A1668 File Offset: 0x0009F868
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionItemData FromPool(DynamicMeshItem i)
	{
		return new RegionItemData(i.WorldPosition.x, i.WorldPosition.z, i.UpdateTime);
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x000A168C File Offset: 0x0009F88C
	public override void read(PooledBinaryReader reader)
	{
		int num = reader.ReadInt32();
		this.Items = new List<RegionItemData>(num);
		for (int i = 0; i < num; i++)
		{
			int num2 = reader.ReadInt32();
			int num3 = reader.ReadInt32();
			int num4 = reader.ReadInt32();
			this.Items.Add(new RegionItemData(num2, num3, num4));
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06001B2D RID: 6957 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000A16E4 File Offset: 0x0009F8E4
	public override void write(PooledBinaryWriter writer)
	{
		base.write(writer);
		writer.Write(this.Items.Count);
		for (int i = 0; i < this.Items.Count; i++)
		{
			writer.Write(this.Items[i].X);
			writer.Write(this.Items[i].Z);
			writer.Write(this.Items[i].UpdateTime);
		}
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x000A1764 File Offset: 0x0009F964
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (DynamicMeshManager.CONTENT_ENABLED)
		{
			DynamicMeshServer.ClientMessageRecieved(this);
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06001B30 RID: 6960 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06001B31 RID: 6961 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04001181 RID: 4481
	public List<RegionItemData> Items = new List<RegionItemData>();
}
