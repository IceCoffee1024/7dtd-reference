using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200082A RID: 2090
[Preserve]
public class NetPackagePlayerInventoryForAI : NetPackage
{
	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06003D1D RID: 15645 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D1E RID: 15646 RVA: 0x001858E4 File Offset: 0x00183AE4
	public NetPackagePlayerInventoryForAI Setup(EntityAlive entity, AIDirectorPlayerInventory inventory)
	{
		this.m_entityId = entity.entityId;
		this.m_inventory = inventory;
		return this;
	}

	// Token: 0x06003D1F RID: 15647 RVA: 0x001858FA File Offset: 0x00183AFA
	public override void read(PooledBinaryReader _reader)
	{
		this.m_entityId = _reader.ReadInt32();
		this.m_inventory.bag = NetPackagePlayerInventoryForAI.ReadInventorySet(_reader);
		this.m_inventory.belt = NetPackagePlayerInventoryForAI.ReadInventorySet(_reader);
	}

	// Token: 0x06003D20 RID: 15648 RVA: 0x0018592A File Offset: 0x00183B2A
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_entityId);
		NetPackagePlayerInventoryForAI.WriteInventorySet(_writer, this.m_inventory.bag);
		NetPackagePlayerInventoryForAI.WriteInventorySet(_writer, this.m_inventory.belt);
	}

	// Token: 0x06003D21 RID: 15649 RVA: 0x00185964 File Offset: 0x00183B64
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null || _world.aiDirector == null)
		{
			return;
		}
		AIDirectorPlayerInventory aidirectorPlayerInventory;
		aidirectorPlayerInventory.bag = this.m_inventory.bag;
		aidirectorPlayerInventory.belt = this.m_inventory.belt;
		_world.aiDirector.UpdatePlayerInventory(this.m_entityId, aidirectorPlayerInventory);
	}

	// Token: 0x06003D22 RID: 15650 RVA: 0x001859B4 File Offset: 0x00183BB4
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<AIDirectorPlayerInventory.ItemId> ReadInventorySet(PooledBinaryReader stream)
	{
		List<AIDirectorPlayerInventory.ItemId> list = null;
		int num = (int)stream.ReadInt16();
		for (int i = 0; i < num; i++)
		{
			if (list == null)
			{
				list = new List<AIDirectorPlayerInventory.ItemId>();
			}
			list.Add(AIDirectorPlayerInventory.ItemId.Read(stream));
		}
		return list;
	}

	// Token: 0x06003D23 RID: 15651 RVA: 0x001859EC File Offset: 0x00183BEC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void WriteInventorySet(PooledBinaryWriter stream, List<AIDirectorPlayerInventory.ItemId> items)
	{
		int num = ((items != null) ? items.Count : 0);
		stream.Write((short)num);
		if (num > 0)
		{
			for (int i = 0; i < items.Count; i++)
			{
				items[i].Write(stream);
			}
		}
	}

	// Token: 0x040031AC RID: 12716
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_entityId;

	// Token: 0x040031AD RID: 12717
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerInventory m_inventory;
}
