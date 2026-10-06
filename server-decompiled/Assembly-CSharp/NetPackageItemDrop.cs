using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000804 RID: 2052
[Preserve]
public class NetPackageItemDrop : NetPackage
{
	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06003C5C RID: 15452 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x00182940 File Offset: 0x00180B40
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

	// Token: 0x06003C5E RID: 15454 RVA: 0x00182990 File Offset: 0x00180B90
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

	// Token: 0x06003C5F RID: 15455 RVA: 0x00182A0C File Offset: 0x00180C0C
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

	// Token: 0x06003C60 RID: 15456 RVA: 0x00182A84 File Offset: 0x00180C84
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		_world.GetGameManager().ItemDropServer(this.itemStack, this.dropPos, this.randomPosAdd, this.initialMotion, this.entityId, this.lifetime, this.bDropPosIsRelativeToHead, this.clientInstanceId);
	}

	// Token: 0x04003114 RID: 12564
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack;

	// Token: 0x04003115 RID: 12565
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 dropPos;

	// Token: 0x04003116 RID: 12566
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 initialMotion;

	// Token: 0x04003117 RID: 12567
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 randomPosAdd;

	// Token: 0x04003118 RID: 12568
	[PublicizedFrom(EAccessModifier.Private)]
	public float lifetime;

	// Token: 0x04003119 RID: 12569
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x0400311A RID: 12570
	[PublicizedFrom(EAccessModifier.Private)]
	public int clientInstanceId;

	// Token: 0x0400311B RID: 12571
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bDropPosIsRelativeToHead;
}
