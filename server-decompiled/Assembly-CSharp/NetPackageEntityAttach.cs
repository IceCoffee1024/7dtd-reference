using System;
using UnityEngine.Scripting;

// Token: 0x0200086E RID: 2158
[Preserve]
public class NetPackageEntityAttach : NetPackage
{
	// Token: 0x06003E73 RID: 15987 RVA: 0x0018C495 File Offset: 0x0018A695
	public NetPackageEntityAttach Setup(NetPackageEntityAttach.AttachType _attachType, int _riderId, int _vehicleId, int _slot)
	{
		this.attachType = _attachType;
		this.riderId = _riderId;
		this.vehicleId = _vehicleId;
		this.slot = _slot;
		return this;
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x0018C4B5 File Offset: 0x0018A6B5
	public override void read(PooledBinaryReader _br)
	{
		this.attachType = (NetPackageEntityAttach.AttachType)_br.ReadByte();
		this.riderId = _br.ReadInt32();
		this.vehicleId = _br.ReadInt32();
		this.slot = (int)_br.ReadInt16();
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x0018C4E7 File Offset: 0x0018A6E7
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.attachType);
		_bw.Write(this.riderId);
		_bw.Write(this.vehicleId);
		_bw.Write((short)this.slot);
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x0018C524 File Offset: 0x0018A724
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		Entity entity = GameManager.Instance.World.GetEntity(this.riderId);
		if (entity == null)
		{
			return;
		}
		Entity entity2 = GameManager.Instance.World.GetEntity(this.vehicleId);
		switch (this.attachType)
		{
		case NetPackageEntityAttach.AttachType.AttachServer:
		{
			if (entity2 == null)
			{
				return;
			}
			int num = entity2.FindAttachSlot(entity);
			if (num < 0)
			{
				num = entity.AttachToEntity(entity2, this.slot);
			}
			if (num >= 0)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityAttach>().Setup(NetPackageEntityAttach.AttachType.AttachClient, this.riderId, this.vehicleId, num), false, -1, -1, -1, null, 192, false);
				return;
			}
			break;
		}
		case NetPackageEntityAttach.AttachType.AttachClient:
			if (entity2 == null)
			{
				return;
			}
			entity.AttachToEntity(entity2, this.slot);
			return;
		case NetPackageEntityAttach.AttachType.DetachServer:
			entity.Detach();
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityAttach>().Setup(NetPackageEntityAttach.AttachType.DetachClient, this.riderId, -1, -1), false, -1, this.riderId, -1, null, 192, false);
			return;
		case NetPackageEntityAttach.AttachType.DetachClient:
			entity.Detach();
			break;
		default:
			return;
		}
	}

	// Token: 0x040032B9 RID: 12985
	[PublicizedFrom(EAccessModifier.Private)]
	public int riderId;

	// Token: 0x040032BA RID: 12986
	[PublicizedFrom(EAccessModifier.Private)]
	public int vehicleId;

	// Token: 0x040032BB RID: 12987
	[PublicizedFrom(EAccessModifier.Private)]
	public int slot;

	// Token: 0x040032BC RID: 12988
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityAttach.AttachType attachType;

	// Token: 0x0200086F RID: 2159
	public enum AttachType : byte
	{
		// Token: 0x040032BE RID: 12990
		AttachServer,
		// Token: 0x040032BF RID: 12991
		AttachClient,
		// Token: 0x040032C0 RID: 12992
		DetachServer,
		// Token: 0x040032C1 RID: 12993
		DetachClient
	}
}
