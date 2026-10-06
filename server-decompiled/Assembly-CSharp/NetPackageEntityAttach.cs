using System;
using UnityEngine.Scripting;

// Token: 0x02000864 RID: 2148
[Preserve]
public class NetPackageEntityAttach : NetPackage
{
	// Token: 0x06003E50 RID: 15952 RVA: 0x0018B3ED File Offset: 0x001895ED
	public NetPackageEntityAttach Setup(NetPackageEntityAttach.AttachType _attachType, int _riderId, int _vehicleId, int _slot)
	{
		this.attachType = _attachType;
		this.riderId = _riderId;
		this.vehicleId = _vehicleId;
		this.slot = _slot;
		return this;
	}

	// Token: 0x06003E51 RID: 15953 RVA: 0x0018B40D File Offset: 0x0018960D
	public override void read(PooledBinaryReader _br)
	{
		this.attachType = (NetPackageEntityAttach.AttachType)_br.ReadByte();
		this.riderId = _br.ReadInt32();
		this.vehicleId = _br.ReadInt32();
		this.slot = (int)_br.ReadInt16();
	}

	// Token: 0x06003E52 RID: 15954 RVA: 0x0018B43F File Offset: 0x0018963F
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.attachType);
		_bw.Write(this.riderId);
		_bw.Write(this.vehicleId);
		_bw.Write((short)this.slot);
	}

	// Token: 0x06003E53 RID: 15955 RVA: 0x0018B47C File Offset: 0x0018967C
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

	// Token: 0x04003299 RID: 12953
	[PublicizedFrom(EAccessModifier.Private)]
	public int riderId;

	// Token: 0x0400329A RID: 12954
	[PublicizedFrom(EAccessModifier.Private)]
	public int vehicleId;

	// Token: 0x0400329B RID: 12955
	[PublicizedFrom(EAccessModifier.Private)]
	public int slot;

	// Token: 0x0400329C RID: 12956
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityAttach.AttachType attachType;

	// Token: 0x02000865 RID: 2149
	public enum AttachType : byte
	{
		// Token: 0x0400329E RID: 12958
		AttachServer,
		// Token: 0x0400329F RID: 12959
		AttachClient,
		// Token: 0x040032A0 RID: 12960
		DetachServer,
		// Token: 0x040032A1 RID: 12961
		DetachClient
	}
}
