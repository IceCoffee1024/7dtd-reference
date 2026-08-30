using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000829 RID: 2089
[Preserve]
public class NetPackagePlayerInventoryForAI : NetPackage
{
	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x06003D5A RID: 15706 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D5B RID: 15707 RVA: 0x00185D4C File Offset: 0x00183F4C
	public NetPackagePlayerInventoryForAI Setup(EntityAlive entity, AIDirectorPlayerInventory inventory)
	{
		this.m_entityId = entity.entityId;
		this.m_inventory = inventory;
		return this;
	}

	// Token: 0x06003D5C RID: 15708 RVA: 0x00185D64 File Offset: 0x00183F64
	public override int GetLength()
	{
		int num = 8;
		if (this.m_inventory.bag != null)
		{
			num += 4 * this.m_inventory.bag.Count;
		}
		if (this.m_inventory.belt != null)
		{
			num += 4 * this.m_inventory.belt.Count;
		}
		return num;
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x00185DB8 File Offset: 0x00183FB8
	public override void read(PooledBinaryReader _reader)
	{
		this.m_entityId = _reader.ReadInt32();
		this.m_inventory.bag = NetPackagePlayerInventoryForAI.ReadInventorySet(_reader);
		this.m_inventory.belt = NetPackagePlayerInventoryForAI.ReadInventorySet(_reader);
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x00185DE8 File Offset: 0x00183FE8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_entityId);
		NetPackagePlayerInventoryForAI.WriteInventorySet(_writer, this.m_inventory.bag);
		NetPackagePlayerInventoryForAI.WriteInventorySet(_writer, this.m_inventory.belt);
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x00185E20 File Offset: 0x00184020
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

	// Token: 0x06003D60 RID: 15712 RVA: 0x00185E70 File Offset: 0x00184070
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<AIDirectorPlayerInventory.ItemId> ReadInventorySet(BinaryReader stream)
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

	// Token: 0x06003D61 RID: 15713 RVA: 0x00185EA8 File Offset: 0x001840A8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void WriteInventorySet(BinaryWriter stream, List<AIDirectorPlayerInventory.ItemId> items)
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

	// Token: 0x040031C5 RID: 12741
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_entityId;

	// Token: 0x040031C6 RID: 12742
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerInventory m_inventory;
}
