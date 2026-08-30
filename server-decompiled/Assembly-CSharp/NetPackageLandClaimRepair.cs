using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020007AA RID: 1962
[Preserve]
public class NetPackageLandClaimRepair : NetPackage
{
	// Token: 0x06003A59 RID: 14937 RVA: 0x0017BAB1 File Offset: 0x00179CB1
	public NetPackageLandClaimRepair Setup(Vector3i _blockPosition, bool _beginRepair)
	{
		this.blockPosition = _blockPosition;
		this.beginRepair = _beginRepair;
		return this;
	}

	// Token: 0x06003A5A RID: 14938 RVA: 0x0017BAC4 File Offset: 0x00179CC4
	public override void read(PooledBinaryReader _br)
	{
		long num = _br.ReadInt64();
		long num2 = _br.ReadInt64();
		long num3 = _br.ReadInt64();
		this.blockPosition = new Vector3i((float)num, (float)num2, (float)num3);
		this.beginRepair = _br.ReadBoolean();
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x0017BB04 File Offset: 0x00179D04
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((long)this.blockPosition.x);
		_bw.Write((long)this.blockPosition.y);
		_bw.Write((long)this.blockPosition.z);
		_bw.Write(this.beginRepair);
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x0017BB5C File Offset: 0x00179D5C
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

	// Token: 0x06003A5D RID: 14941 RVA: 0x0004AC4A File Offset: 0x00048E4A
	public override int GetLength()
	{
		return 25;
	}

	// Token: 0x04002FCD RID: 12237
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPosition;

	// Token: 0x04002FCE RID: 12238
	[PublicizedFrom(EAccessModifier.Private)]
	public bool beginRepair;
}
