using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007BE RID: 1982
[Preserve]
public class NetPackageDropItemsContainer : NetPackage
{
	// Token: 0x06003AC2 RID: 15042 RVA: 0x0017BD74 File Offset: 0x00179F74
	public NetPackageDropItemsContainer Setup(int _droppedByID, string _containerEntity, Vector3 _worldPos, ItemStack[] _items)
	{
		this.droppedByID = _droppedByID;
		this.worldPos = _worldPos;
		this.items = _items;
		this.containerEntity = _containerEntity;
		return this;
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x0017BD94 File Offset: 0x00179F94
	public override void read(PooledBinaryReader _br)
	{
		this.droppedByID = _br.ReadInt32();
		this.containerEntity = _br.ReadString();
		this.worldPos = StreamUtils.ReadVector3(_br);
		this.items = GameUtils.ReadItemStack(_br);
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x0017BDC8 File Offset: 0x00179FC8
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

	// Token: 0x06003AC5 RID: 15045 RVA: 0x0017BE30 File Offset: 0x0017A030
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().DropContentInLootContainerServer(this.droppedByID, this.containerEntity, this.worldPos, this.items, false, null);
	}

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x06003AC6 RID: 15046 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003AC7 RID: 15047 RVA: 0x0017BE6E File Offset: 0x0017A06E
	public override int GetLength()
	{
		return 16;
	}

	// Token: 0x04002FC4 RID: 12228
	[PublicizedFrom(EAccessModifier.Private)]
	public int droppedByID;

	// Token: 0x04002FC5 RID: 12229
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 worldPos;

	// Token: 0x04002FC6 RID: 12230
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerEntity = "";

	// Token: 0x04002FC7 RID: 12231
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] items;
}
