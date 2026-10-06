using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007CC RID: 1996
[Preserve]
public class NetPackageDropItemsContainer : NetPackage
{
	// Token: 0x06003B43 RID: 15171 RVA: 0x0017E720 File Offset: 0x0017C920
	public NetPackageDropItemsContainer Setup(int _droppedByID, string _containerEntity, Vector3 _worldPos, ItemStack[] _items)
	{
		this.droppedByID = _droppedByID;
		this.worldPos = _worldPos;
		this.items = _items;
		this.containerEntity = _containerEntity;
		return this;
	}

	// Token: 0x06003B44 RID: 15172 RVA: 0x0017E740 File Offset: 0x0017C940
	public override void read(PooledBinaryReader _br)
	{
		this.droppedByID = _br.ReadInt32();
		this.containerEntity = _br.ReadString();
		this.worldPos = StreamUtils.ReadVector3(_br);
		this.items = GameUtils.ReadItemStack(_br);
	}

	// Token: 0x06003B45 RID: 15173 RVA: 0x0017E774 File Offset: 0x0017C974
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

	// Token: 0x06003B46 RID: 15174 RVA: 0x0017E7DC File Offset: 0x0017C9DC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().DropContentInLootContainerServer(this.droppedByID, this.containerEntity, this.worldPos, this.items, false, null);
	}

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x06003B47 RID: 15175 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x0400303D RID: 12349
	[PublicizedFrom(EAccessModifier.Private)]
	public int droppedByID;

	// Token: 0x0400303E RID: 12350
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 worldPos;

	// Token: 0x0400303F RID: 12351
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerEntity = "";

	// Token: 0x04003040 RID: 12352
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;
}
