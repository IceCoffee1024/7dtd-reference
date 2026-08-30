using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x0200081E RID: 2078
[Preserve]
public class NetPackagePlayerInventoryForAI : NetPackage
{
	// Token: 0x1700065C RID: 1628
	// (get) Token: 0x06003CFB RID: 15611 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x00183974 File Offset: 0x00181B74
	public NetPackagePlayerInventoryForAI Setup(EntityAlive entity, AIDirectorPlayerInventory inventory)
	{
		this.m_entityId = entity.entityId;
		this.m_inventory = inventory;
		return this;
	}

	// Token: 0x06003CFD RID: 15613 RVA: 0x0018398C File Offset: 0x00181B8C
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

	// Token: 0x06003CFE RID: 15614 RVA: 0x001839E0 File Offset: 0x00181BE0
	public override void read(PooledBinaryReader _reader)
	{
		this.m_entityId = _reader.ReadInt32();
		this.m_inventory.bag = NetPackagePlayerInventoryForAI.ReadInventorySet(_reader);
		this.m_inventory.belt = NetPackagePlayerInventoryForAI.ReadInventorySet(_reader);
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x00183A10 File Offset: 0x00181C10
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_entityId);
		NetPackagePlayerInventoryForAI.WriteInventorySet(_writer, this.m_inventory.bag);
		NetPackagePlayerInventoryForAI.WriteInventorySet(_writer, this.m_inventory.belt);
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x00183A48 File Offset: 0x00181C48
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

	// Token: 0x06003D01 RID: 15617 RVA: 0x00183A98 File Offset: 0x00181C98
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

	// Token: 0x06003D02 RID: 15618 RVA: 0x00183AD0 File Offset: 0x00181CD0
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

	// Token: 0x04003146 RID: 12614
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_entityId;

	// Token: 0x04003147 RID: 12615
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerInventory m_inventory;
}
