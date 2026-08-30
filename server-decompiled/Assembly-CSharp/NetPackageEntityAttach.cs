using System;
using UnityEngine.Scripting;

// Token: 0x02000861 RID: 2145
[Preserve]
public class NetPackageEntityAttach : NetPackage
{
	// Token: 0x06003EB2 RID: 16050 RVA: 0x0018B563 File Offset: 0x00189763
	public NetPackageEntityAttach Setup(NetPackageEntityAttach.AttachType _attachType, int _riderId, int _vehicleId, int _slot)
	{
		this.attachType = _attachType;
		this.riderId = _riderId;
		this.vehicleId = _vehicleId;
		this.slot = _slot;
		return this;
	}

	// Token: 0x06003EB3 RID: 16051 RVA: 0x0018B583 File Offset: 0x00189783
	public override void read(PooledBinaryReader _br)
	{
		this.attachType = (NetPackageEntityAttach.AttachType)_br.ReadByte();
		this.riderId = _br.ReadInt32();
		this.vehicleId = _br.ReadInt32();
		this.slot = (int)_br.ReadInt16();
	}

	// Token: 0x06003EB4 RID: 16052 RVA: 0x0018B5B5 File Offset: 0x001897B5
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.attachType);
		_bw.Write(this.riderId);
		_bw.Write(this.vehicleId);
		_bw.Write((short)this.slot);
	}

	// Token: 0x06003EB5 RID: 16053 RVA: 0x0018B5F0 File Offset: 0x001897F0
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

	// Token: 0x06003EB6 RID: 16054 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040032AE RID: 12974
	[PublicizedFrom(EAccessModifier.Private)]
	public int riderId;

	// Token: 0x040032AF RID: 12975
	[PublicizedFrom(EAccessModifier.Private)]
	public int vehicleId;

	// Token: 0x040032B0 RID: 12976
	[PublicizedFrom(EAccessModifier.Private)]
	public int slot;

	// Token: 0x040032B1 RID: 12977
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityAttach.AttachType attachType;

	// Token: 0x02000862 RID: 2146
	public enum AttachType : byte
	{
		// Token: 0x040032B3 RID: 12979
		AttachServer,
		// Token: 0x040032B4 RID: 12980
		AttachClient,
		// Token: 0x040032B5 RID: 12981
		DetachServer,
		// Token: 0x040032B6 RID: 12982
		DetachClient
	}
}
