using System;
using UnityEngine.Scripting;

// Token: 0x020007B4 RID: 1972
[Preserve]
public class NetPackageBlockTrigger : NetPackage
{
	// Token: 0x06003AC1 RID: 15041 RVA: 0x0017C6B2 File Offset: 0x0017A8B2
	public NetPackageBlockTrigger Setup(Vector3i _blockPos, BlockValue _blockValue)
	{
		this.blockPos = _blockPos;
		this.blockValue = _blockValue;
		return this;
	}

	// Token: 0x06003AC2 RID: 15042 RVA: 0x0017C6C3 File Offset: 0x0017A8C3
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockValue = new BlockValue(_br.ReadUInt32());
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x0017C6E2 File Offset: 0x0017A8E2
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write(this.blockValue.rawData);
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x0017C708 File Offset: 0x0017A908
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (base.Sender.bAttachedToEntity)
		{
			EntityPlayer entityPlayer = _world.GetEntity(base.Sender.entityId) as EntityPlayer;
			this.blockValue.Block.HandleTrigger(entityPlayer, _world, this.blockPos, this.blockValue);
		}
	}

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06003AC5 RID: 15045 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x04002FD0 RID: 12240
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04002FD1 RID: 12241
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;
}
