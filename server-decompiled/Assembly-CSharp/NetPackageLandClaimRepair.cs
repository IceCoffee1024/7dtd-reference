using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007B5 RID: 1973
[Preserve]
public class NetPackageLandClaimRepair : NetPackage
{
	// Token: 0x06003AAD RID: 15021 RVA: 0x0017D0E7 File Offset: 0x0017B2E7
	public NetPackageLandClaimRepair Setup(Vector3i _blockPosition, bool _beginRepair)
	{
		this.blockPosition = _blockPosition;
		this.beginRepair = _beginRepair;
		return this;
	}

	// Token: 0x06003AAE RID: 15022 RVA: 0x0017D0F8 File Offset: 0x0017B2F8
	public override void read(PooledBinaryReader _br)
	{
		long num = _br.ReadInt64();
		long num2 = _br.ReadInt64();
		long num3 = _br.ReadInt64();
		this.blockPosition = new Vector3i((float)num, (float)num2, (float)num3);
		this.beginRepair = _br.ReadBoolean();
	}

	// Token: 0x06003AAF RID: 15023 RVA: 0x0017D138 File Offset: 0x0017B338
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((long)this.blockPosition.x);
		_bw.Write((long)this.blockPosition.y);
		_bw.Write((long)this.blockPosition.z);
		_bw.Write(this.beginRepair);
	}

	// Token: 0x06003AB0 RID: 15024 RVA: 0x0017D190 File Offset: 0x0017B390
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

	// Token: 0x04002FE3 RID: 12259
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPosition;

	// Token: 0x04002FE4 RID: 12260
	[PublicizedFrom(EAccessModifier.Private)]
	public bool beginRepair;
}
