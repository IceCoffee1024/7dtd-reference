using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000394 RID: 916
[Preserve]
public class NetPackageDynamicClientArrive : NetPackage
{
	// Token: 0x06001B30 RID: 6960 RVA: 0x000A1390 File Offset: 0x0009F590
	public void BuildData()
	{
		foreach (DynamicMeshItem dynamicMeshItem in DynamicMeshManager.Instance.ItemsDictionary.Values)
		{
			this.Items.Add(this.FromPool(dynamicMeshItem));
		}
		Log.Out("Client package items: " + this.Items.Count.ToString());
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x000A1414 File Offset: 0x0009F614
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionItemData FromPool(DynamicMeshItem i)
	{
		return new RegionItemData(i.WorldPosition.x, i.WorldPosition.z, i.UpdateTime);
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x000A1438 File Offset: 0x0009F638
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

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001B33 RID: 6963 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x000A1490 File Offset: 0x0009F690
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

	// Token: 0x06001B35 RID: 6965 RVA: 0x000A1510 File Offset: 0x0009F710
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (DynamicMeshManager.CONTENT_ENABLED)
		{
			DynamicMeshServer.ClientMessageRecieved(this);
		}
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x000A151F File Offset: 0x0009F71F
	public override int GetLength()
	{
		return 4 + 12 * this.Items.Count;
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001B37 RID: 6967 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06001B38 RID: 6968 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04001188 RID: 4488
	public List<RegionItemData> Items = new List<RegionItemData>();
}
