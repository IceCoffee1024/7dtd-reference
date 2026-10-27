using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200080E RID: 2062
[Preserve]
public class NetPackageItemDrop : NetPackage
{
	// Token: 0x17000671 RID: 1649
	// (get) Token: 0x06003C7F RID: 15487 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C80 RID: 15488 RVA: 0x00183A44 File Offset: 0x00181C44
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

	// Token: 0x06003C81 RID: 15489 RVA: 0x00183A94 File Offset: 0x00181C94
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

	// Token: 0x06003C82 RID: 15490 RVA: 0x00183B10 File Offset: 0x00181D10
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

	// Token: 0x06003C83 RID: 15491 RVA: 0x00183B88 File Offset: 0x00181D88
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ItemDropServer(this.itemStack, this.dropPos, this.randomPosAdd, this.initialMotion, this.entityId, this.lifetime, this.bDropPosIsRelativeToHead, this.clientInstanceId);
	}

	// Token: 0x04003134 RID: 12596
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack;

	// Token: 0x04003135 RID: 12597
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 dropPos;

	// Token: 0x04003136 RID: 12598
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 initialMotion;

	// Token: 0x04003137 RID: 12599
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 randomPosAdd;

	// Token: 0x04003138 RID: 12600
	[PublicizedFrom(EAccessModifier.Private)]
	public float lifetime;

	// Token: 0x04003139 RID: 12601
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400313A RID: 12602
	[PublicizedFrom(EAccessModifier.Private)]
	public int clientInstanceId;

	// Token: 0x0400313B RID: 12603
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropPosIsRelativeToHead;
}
