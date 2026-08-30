using System;
using UnityEngine.Scripting;

// Token: 0x02000855 RID: 2133
[Preserve]
public class NetPackageEntityAttach : NetPackage
{
	// Token: 0x06003E4D RID: 15949 RVA: 0x00189493 File Offset: 0x00187693
	public NetPackageEntityAttach Setup(NetPackageEntityAttach.AttachType _attachType, int _riderId, int _vehicleId, int _slot)
	{
		this.attachType = _attachType;
		this.riderId = _riderId;
		this.vehicleId = _vehicleId;
		this.slot = _slot;
		return this;
	}

	// Token: 0x06003E4E RID: 15950 RVA: 0x001894B3 File Offset: 0x001876B3
	public override void read(PooledBinaryReader _br)
	{
		this.attachType = (NetPackageEntityAttach.AttachType)_br.ReadByte();
		this.riderId = _br.ReadInt32();
		this.vehicleId = _br.ReadInt32();
		this.slot = (int)_br.ReadInt16();
	}

	// Token: 0x06003E4F RID: 15951 RVA: 0x001894E5 File Offset: 0x001876E5
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write((byte)this.attachType);
		_bw.Write(this.riderId);
		_bw.Write(this.vehicleId);
		_bw.Write((short)this.slot);
	}

	// Token: 0x06003E50 RID: 15952 RVA: 0x00189520 File Offset: 0x00187720
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

	// Token: 0x06003E51 RID: 15953 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x0400322D RID: 12845
	[PublicizedFrom(EAccessModifier.Private)]
	public int riderId;

	// Token: 0x0400322E RID: 12846
	[PublicizedFrom(EAccessModifier.Private)]
	public int vehicleId;

	// Token: 0x0400322F RID: 12847
	[PublicizedFrom(EAccessModifier.Private)]
	public int slot;

	// Token: 0x04003230 RID: 12848
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageEntityAttach.AttachType attachType;

	// Token: 0x02000856 RID: 2134
	public enum AttachType : byte
	{
		// Token: 0x04003232 RID: 12850
		AttachServer,
		// Token: 0x04003233 RID: 12851
		AttachClient,
		// Token: 0x04003234 RID: 12852
		DetachServer,
		// Token: 0x04003235 RID: 12853
		DetachClient
	}
}
