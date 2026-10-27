using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000834 RID: 2100
[Preserve]
public class NetPackagePlayerInventoryForAI : NetPackage
{
	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06003D40 RID: 15680 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D41 RID: 15681 RVA: 0x001869E8 File Offset: 0x00184BE8
	public NetPackagePlayerInventoryForAI Setup(EntityAlive entity, AIDirectorPlayerInventory inventory)
	{
		this.m_entityId = entity.entityId;
		this.m_inventory = inventory;
		return this;
	}

	// Token: 0x06003D42 RID: 15682 RVA: 0x001869FE File Offset: 0x00184BFE
	public override void read(PooledBinaryReader _reader)
	{
		this.m_entityId = _reader.ReadInt32();
		this.m_inventory.bag = NetPackagePlayerInventoryForAI.ReadInventorySet(_reader);
		this.m_inventory.belt = NetPackagePlayerInventoryForAI.ReadInventorySet(_reader);
	}

	// Token: 0x06003D43 RID: 15683 RVA: 0x00186A2E File Offset: 0x00184C2E
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.m_entityId);
		NetPackagePlayerInventoryForAI.WriteInventorySet(_writer, this.m_inventory.bag);
		NetPackagePlayerInventoryForAI.WriteInventorySet(_writer, this.m_inventory.belt);
	}

	// Token: 0x06003D44 RID: 15684 RVA: 0x00186A68 File Offset: 0x00184C68
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

	// Token: 0x06003D45 RID: 15685 RVA: 0x00186AB8 File Offset: 0x00184CB8
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

	// Token: 0x06003D46 RID: 15686 RVA: 0x00186AF0 File Offset: 0x00184CF0
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

	// Token: 0x040031CC RID: 12748
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_entityId;

	// Token: 0x040031CD RID: 12749
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerInventory m_inventory;
}
