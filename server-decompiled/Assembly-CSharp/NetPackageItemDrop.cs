using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000805 RID: 2053
[Preserve]
public class NetPackageItemDrop : NetPackage
{
	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x06003C81 RID: 15489 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x001828E4 File Offset: 0x00180AE4
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

	// Token: 0x06003C83 RID: 15491 RVA: 0x00182934 File Offset: 0x00180B34
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

	// Token: 0x06003C84 RID: 15492 RVA: 0x001829B0 File Offset: 0x00180BB0
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

	// Token: 0x06003C85 RID: 15493 RVA: 0x00182A28 File Offset: 0x00180C28
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ItemDropServer(this.itemStack, this.dropPos, this.randomPosAdd, this.initialMotion, this.entityId, this.lifetime, this.bDropPosIsRelativeToHead, this.clientInstanceId);
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x00182A74 File Offset: 0x00180C74
	public override int GetLength()
	{
		return 52;
	}

	// Token: 0x04003126 RID: 12582
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack;

	// Token: 0x04003127 RID: 12583
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 dropPos;

	// Token: 0x04003128 RID: 12584
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 initialMotion;

	// Token: 0x04003129 RID: 12585
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 randomPosAdd;

	// Token: 0x0400312A RID: 12586
	[PublicizedFrom(EAccessModifier.Private)]
	public float lifetime;

	// Token: 0x0400312B RID: 12587
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400312C RID: 12588
	[PublicizedFrom(EAccessModifier.Private)]
	public int clientInstanceId;

	// Token: 0x0400312D RID: 12589
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropPosIsRelativeToHead;
}
