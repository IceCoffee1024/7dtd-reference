using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007C9 RID: 1993
[Preserve]
public class NetPackageDropItemsContainer : NetPackage
{
	// Token: 0x06003B21 RID: 15137 RVA: 0x0017E14C File Offset: 0x0017C34C
	public NetPackageDropItemsContainer Setup(int _droppedByID, string _containerEntity, Vector3 _worldPos, ItemStack[] _items)
	{
		this.droppedByID = _droppedByID;
		this.worldPos = _worldPos;
		this.items = _items;
		this.containerEntity = _containerEntity;
		return this;
	}

	// Token: 0x06003B22 RID: 15138 RVA: 0x0017E16C File Offset: 0x0017C36C
	public override void read(PooledBinaryReader _br)
	{
		this.droppedByID = _br.ReadInt32();
		this.containerEntity = _br.ReadString();
		this.worldPos = StreamUtils.ReadVector3(_br);
		this.items = GameUtils.ReadItemStack(_br);
	}

	// Token: 0x06003B23 RID: 15139 RVA: 0x0017E1A0 File Offset: 0x0017C3A0
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.droppedByID);
		_bw.Write(this.containerEntity);
		StreamUtils.Write(_bw, this.worldPos);
		_bw.Write((ushort)this.items.Length);
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i].Write(_bw);
		}
	}

	// Token: 0x06003B24 RID: 15140 RVA: 0x0017E208 File Offset: 0x0017C408
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().DropContentInLootContainerServer(this.droppedByID, this.containerEntity, this.worldPos, this.items, false, null);
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x06003B25 RID: 15141 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003B26 RID: 15142 RVA: 0x0017E246 File Offset: 0x0017C446
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x04003043 RID: 12355
	[PublicizedFrom(EAccessModifier.Private)]
	public int droppedByID;

	// Token: 0x04003044 RID: 12356
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 worldPos;

	// Token: 0x04003045 RID: 12357
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerEntity = "";

	// Token: 0x04003046 RID: 12358
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;
}
