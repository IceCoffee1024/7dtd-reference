using System;
using UnityEngine.Scripting;

// Token: 0x020007A7 RID: 1959
[Preserve]
public class NetPackageBlockTrigger : NetPackage
{
	// Token: 0x06003A33 RID: 14899 RVA: 0x00179DA3 File Offset: 0x00177FA3
	public NetPackageBlockTrigger Setup(Vector3i _blockPos, BlockValue _blockValue)
	{
		this.blockPos = _blockPos;
		this.blockValue = _blockValue;
		return this;
	}

	// Token: 0x06003A34 RID: 14900 RVA: 0x00179DB4 File Offset: 0x00177FB4
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockValue = new BlockValue(_br.ReadUInt32());
	}

	// Token: 0x06003A35 RID: 14901 RVA: 0x00179DD3 File Offset: 0x00177FD3
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write(this.blockValue.rawData);
	}

	// Token: 0x06003A36 RID: 14902 RVA: 0x00179DFC File Offset: 0x00177FFC
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

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x06003A37 RID: 14903 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003A38 RID: 14904 RVA: 0x00104CB5 File Offset: 0x00102EB5
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x04002F5D RID: 12125
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04002F5E RID: 12126
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;
}
