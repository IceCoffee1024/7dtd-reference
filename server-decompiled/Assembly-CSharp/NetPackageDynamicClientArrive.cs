using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000390 RID: 912
[Preserve]
public class NetPackageDynamicClientArrive : NetPackage
{
	// Token: 0x06001B13 RID: 6931 RVA: 0x000A0C50 File Offset: 0x0009EE50
	public void BuildData()
	{
		foreach (DynamicMeshItem dynamicMeshItem in DynamicMeshManager.Instance.ItemsDictionary.Values)
		{
			this.Items.Add(this.FromPool(dynamicMeshItem));
		}
		Log.Out("Client package items: " + this.Items.Count.ToString());
	}

	// Token: 0x06001B14 RID: 6932 RVA: 0x000A0CD4 File Offset: 0x0009EED4
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionItemData FromPool(DynamicMeshItem i)
	{
		return new RegionItemData(i.WorldPosition.x, i.WorldPosition.z, i.UpdateTime);
	}

	// Token: 0x06001B15 RID: 6933 RVA: 0x000A0CF8 File Offset: 0x0009EEF8
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

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001B16 RID: 6934 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x000A0D50 File Offset: 0x0009EF50
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

	// Token: 0x06001B18 RID: 6936 RVA: 0x000A0DD0 File Offset: 0x0009EFD0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (DynamicMeshManager.CONTENT_ENABLED)
		{
			DynamicMeshServer.ClientMessageRecieved(this);
		}
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x000A0DDF File Offset: 0x0009EFDF
	public override int GetLength()
	{
		return 4 + 12 * this.Items.Count;
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06001B1A RID: 6938 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int Channel
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06001B1B RID: 6939 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04001173 RID: 4467
	public List<RegionItemData> Items = new List<RegionItemData>();
}
