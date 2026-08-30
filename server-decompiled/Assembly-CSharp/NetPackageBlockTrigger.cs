using System;
using UnityEngine.Scripting;

// Token: 0x020007B1 RID: 1969
[Preserve]
public class NetPackageBlockTrigger : NetPackage
{
	// Token: 0x06003A8B RID: 14987 RVA: 0x0017C023 File Offset: 0x0017A223
	public NetPackageBlockTrigger Setup(Vector3i _blockPos, BlockValue _blockValue)
	{
		this.blockPos = _blockPos;
		this.blockValue = _blockValue;
		return this;
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x0017C034 File Offset: 0x0017A234
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockValue = new BlockValue(_br.ReadUInt32());
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x0017C053 File Offset: 0x0017A253
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write(this.blockValue.rawData);
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x0017C07C File Offset: 0x0017A27C
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

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x06003A8F RID: 14991 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x00105CC1 File Offset: 0x00103EC1
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x04002FD6 RID: 12246
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04002FD7 RID: 12247
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;
}
