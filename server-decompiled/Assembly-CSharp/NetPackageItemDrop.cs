using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007FA RID: 2042
[Preserve]
public class NetPackageItemDrop : NetPackage
{
	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x06003C22 RID: 15394 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x0018050C File Offset: 0x0017E70C
	public NetPackageItemDrop Setup(ItemStack _itemStack, Vector3 _dropPos, Vector3 _initialMotion, Vector3 _randomPosAdd, float _lifetime, int _entityId, bool _bDropPosIsRelativeToHead, int _clientInstanceId)
	{
		this.itemStack = _itemStack.Clone();
		this.dropPos = _dropPos;
		this.initialMotion = _initialMotion;
		this.randomPosAdd = _randomPosAdd;
		this.lifetime = _lifetime;
		this.entityId = _entityId;
		this.clientInstanceId = _clientInstanceId;
		this.bDropPosIsRelativeToHead = _bDropPosIsRelativeToHead;
		return this;
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x0018055C File Offset: 0x0017E75C
	public override void read(PooledBinaryReader _br)
	{
		this.itemStack = new ItemStack();
		this.itemStack.Read(_br);
		this.dropPos = StreamUtils.ReadVector3(_br);
		this.initialMotion = StreamUtils.ReadVector3(_br);
		this.randomPosAdd = StreamUtils.ReadVector3(_br);
		this.lifetime = _br.ReadSingle();
		this.entityId = _br.ReadInt32();
		this.clientInstanceId = _br.ReadInt32();
		this.bDropPosIsRelativeToHead = _br.ReadBoolean();
	}

	// Token: 0x06003C25 RID: 15397 RVA: 0x001805D8 File Offset: 0x0017E7D8
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.itemStack.Write(_bw);
		StreamUtils.Write(_bw, this.dropPos);
		StreamUtils.Write(_bw, this.initialMotion);
		StreamUtils.Write(_bw, this.randomPosAdd);
		_bw.Write(this.lifetime);
		_bw.Write(this.entityId);
		_bw.Write(this.clientInstanceId);
		_bw.Write(this.bDropPosIsRelativeToHead);
	}

	// Token: 0x06003C26 RID: 15398 RVA: 0x00180650 File Offset: 0x0017E850
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ItemDropServer(this.itemStack, this.dropPos, this.randomPosAdd, this.initialMotion, this.entityId, this.lifetime, this.bDropPosIsRelativeToHead, this.clientInstanceId);
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x0018069C File Offset: 0x0017E89C
	public override int GetLength()
	{
		return 52;
	}

	// Token: 0x040030A7 RID: 12455
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack;

	// Token: 0x040030A8 RID: 12456
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 dropPos;

	// Token: 0x040030A9 RID: 12457
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 initialMotion;

	// Token: 0x040030AA RID: 12458
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 randomPosAdd;

	// Token: 0x040030AB RID: 12459
	[PublicizedFrom(EAccessModifier.Private)]
	public float lifetime;

	// Token: 0x040030AC RID: 12460
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040030AD RID: 12461
	[PublicizedFrom(EAccessModifier.Private)]
	public int clientInstanceId;

	// Token: 0x040030AE RID: 12462
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropPosIsRelativeToHead;
}
