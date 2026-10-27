using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007D6 RID: 2006
[Preserve]
public class NetPackageDropItemsContainer : NetPackage
{
	// Token: 0x06003B66 RID: 15206 RVA: 0x0017F825 File Offset: 0x0017DA25
	public NetPackageDropItemsContainer Setup(int _droppedByID, string _containerEntity, Vector3 _worldPos, ItemStack[] _items)
	{
		this.droppedByID = _droppedByID;
		this.worldPos = _worldPos;
		this.items = _items;
		this.containerEntity = _containerEntity;
		return this;
	}

	// Token: 0x06003B67 RID: 15207 RVA: 0x0017F845 File Offset: 0x0017DA45
	public override void read(PooledBinaryReader _br)
	{
		this.droppedByID = _br.ReadInt32();
		this.containerEntity = _br.ReadString();
		this.worldPos = StreamUtils.ReadVector3(_br);
		this.items = GameUtils.ReadItemStack(_br);
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x0017F878 File Offset: 0x0017DA78
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

	// Token: 0x06003B69 RID: 15209 RVA: 0x0017F8E0 File Offset: 0x0017DAE0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().DropContentInLootContainerServer(this.droppedByID, this.containerEntity, this.worldPos, this.items, false, null);
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x06003B6A RID: 15210 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x0400305D RID: 12381
	[PublicizedFrom(EAccessModifier.Private)]
	public int droppedByID;

	// Token: 0x0400305E RID: 12382
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 worldPos;

	// Token: 0x0400305F RID: 12383
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerEntity = "";

	// Token: 0x04003060 RID: 12384
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;
}
