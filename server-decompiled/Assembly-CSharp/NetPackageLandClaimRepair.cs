using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007A0 RID: 1952
[Preserve]
public class NetPackageLandClaimRepair : NetPackage
{
	// Token: 0x06003A01 RID: 14849 RVA: 0x00179831 File Offset: 0x00177A31
	public NetPackageLandClaimRepair Setup(Vector3i _blockPosition, bool _beginRepair)
	{
		this.blockPosition = _blockPosition;
		this.beginRepair = _beginRepair;
		return this;
	}

	// Token: 0x06003A02 RID: 14850 RVA: 0x00179844 File Offset: 0x00177A44
	public override void read(PooledBinaryReader _br)
	{
		long num = _br.ReadInt64();
		long num2 = _br.ReadInt64();
		long num3 = _br.ReadInt64();
		this.blockPosition = new Vector3i((float)num, (float)num2, (float)num3);
		this.beginRepair = _br.ReadBoolean();
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x00179884 File Offset: 0x00177A84
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((long)this.blockPosition.x);
		_bw.Write((long)this.blockPosition.y);
		_bw.Write((long)this.blockPosition.z);
		_bw.Write(this.beginRepair);
	}

	// Token: 0x06003A04 RID: 14852 RVA: 0x001798DC File Offset: 0x00177ADC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		TEFeatureAreaRepair selfOrFeature = _world.GetTileEntity(this.blockPosition).GetSelfOrFeature<TEFeatureAreaRepair>();
		if (selfOrFeature != null)
		{
			if (this.beginRepair)
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					selfOrFeature.RepairAll(_world, this.blockPosition, base.Sender.entityId);
					return;
				}
			}
			else if (selfOrFeature.Parent.Owner.Equals(PlatformManager.InternalLocalUserIdentifier))
			{
				selfOrFeature.IsRepairing = false;
			}
		}
	}

	// Token: 0x06003A05 RID: 14853 RVA: 0x0004A832 File Offset: 0x00048A32
	public override int GetLength()
	{
		return 25;
	}

	// Token: 0x04002F54 RID: 12116
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPosition;

	// Token: 0x04002F55 RID: 12117
	[PublicizedFrom(EAccessModifier.Private)]
	public bool beginRepair;
}
