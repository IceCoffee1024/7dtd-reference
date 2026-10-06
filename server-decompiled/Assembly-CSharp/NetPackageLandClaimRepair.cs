using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007AD RID: 1965
[Preserve]
public class NetPackageLandClaimRepair : NetPackage
{
	// Token: 0x06003A96 RID: 14998 RVA: 0x0017C14F File Offset: 0x0017A34F
	public NetPackageLandClaimRepair Setup(Vector3i _blockPosition, bool _beginRepair)
	{
		this.blockPosition = _blockPosition;
		this.beginRepair = _beginRepair;
		return this;
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x0017C160 File Offset: 0x0017A360
	public override void read(PooledBinaryReader _br)
	{
		long num = _br.ReadInt64();
		long num2 = _br.ReadInt64();
		long num3 = _br.ReadInt64();
		this.blockPosition = new Vector3i((float)num, (float)num2, (float)num3);
		this.beginRepair = _br.ReadBoolean();
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x0017C1A0 File Offset: 0x0017A3A0
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((long)this.blockPosition.x);
		_bw.Write((long)this.blockPosition.y);
		_bw.Write((long)this.blockPosition.z);
		_bw.Write(this.beginRepair);
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x0017C1F8 File Offset: 0x0017A3F8
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

	// Token: 0x04002FC7 RID: 12231
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPosition;

	// Token: 0x04002FC8 RID: 12232
	[PublicizedFrom(EAccessModifier.Private)]
	public bool beginRepair;
}
