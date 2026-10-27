using System;
using UnityEngine.Scripting;

// Token: 0x020007BC RID: 1980
[Preserve]
public class NetPackageBlockTrigger : NetPackage
{
	// Token: 0x06003AD8 RID: 15064 RVA: 0x0017D64A File Offset: 0x0017B84A
	public NetPackageBlockTrigger Setup(Vector3i _blockPos, BlockValue _blockValue)
	{
		this.blockPos = _blockPos;
		this.blockValue = _blockValue;
		return this;
	}

	// Token: 0x06003AD9 RID: 15065 RVA: 0x0017D65B File Offset: 0x0017B85B
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockValue = new BlockValue(_br.ReadUInt32());
	}

	// Token: 0x06003ADA RID: 15066 RVA: 0x0017D67A File Offset: 0x0017B87A
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write(this.blockValue.rawData);
	}

	// Token: 0x06003ADB RID: 15067 RVA: 0x0017D6A0 File Offset: 0x0017B8A0
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

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06003ADC RID: 15068 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x04002FEC RID: 12268
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04002FED RID: 12269
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;
}
