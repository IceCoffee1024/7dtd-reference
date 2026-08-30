using System;
using UnityEngine.Scripting;

// Token: 0x02000860 RID: 2144
[Preserve]
public class NetPackageVehicleDataSync : NetPackage
{
	// Token: 0x06003EAB RID: 16043 RVA: 0x0018B28C File Offset: 0x0018948C
	public NetPackageVehicleDataSync Setup(EntityVehicle _ev, int _senderId, ushort _syncFlags)
	{
		this.senderId = _senderId;
		this.vehicleId = _ev.entityId;
		this.syncFlags = _syncFlags;
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.entityData);
			_ev.WriteSyncData(pooledBinaryWriter, _syncFlags);
		}
		return this;
	}

	// Token: 0x06003EAC RID: 16044 RVA: 0x0018B2F0 File Offset: 0x001894F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageVehicleDataSync()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.entityData);
	}

	// Token: 0x06003EAD RID: 16045 RVA: 0x0018B328 File Offset: 0x00189528
	public override void read(PooledBinaryReader _br)
	{
		this.senderId = _br.ReadInt32();
		this.vehicleId = _br.ReadInt32();
		this.syncFlags = _br.ReadUInt16();
		int num = (int)_br.ReadUInt16();
		StreamUtils.StreamCopy(_br.BaseStream, this.entityData, num, null, true);
	}

	// Token: 0x06003EAE RID: 16046 RVA: 0x0018B374 File Offset: 0x00189574
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.senderId);
		_bw.Write(this.vehicleId);
		_bw.Write(this.syncFlags);
		_bw.Write((ushort)this.entityData.Length);
		this.entityData.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003EAF RID: 16047 RVA: 0x0018B3D0 File Offset: 0x001895D0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.senderId, false))
		{
			return;
		}
		EntityVehicle entityVehicle = GameManager.Instance.World.GetEntity(this.vehicleId) as EntityVehicle;
		if (entityVehicle == null)
		{
			return;
		}
		if (this.entityData.Length > 0L)
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = this.entityData;
			lock (pooledExpandableMemoryStream)
			{
				this.entityData.Position = 0L;
				try
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(this.entityData);
						entityVehicle.ReadSyncData(pooledBinaryReader, this.syncFlags, this.senderId);
					}
				}
				catch (Exception ex)
				{
					Log.Exception(ex);
					string text = "Error syncing data for entity ";
					EntityVehicle entityVehicle2 = entityVehicle;
					Log.Error(text + ((entityVehicle2 != null) ? entityVehicle2.ToString() : null) + "; Sender id = " + this.senderId.ToString());
				}
			}
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			ushort syncFlagsReplicated = entityVehicle.GetSyncFlagsReplicated(this.syncFlags);
			if (syncFlagsReplicated != 0)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageVehicleDataSync>().Setup(entityVehicle, this.senderId, syncFlagsReplicated), false, -1, this.senderId, -1, null, 192, false);
			}
		}
	}

	// Token: 0x06003EB0 RID: 16048 RVA: 0x0018B538 File Offset: 0x00189738
	public override int GetLength()
	{
		return (int)(12L + this.entityData.Length);
	}

	// Token: 0x040032AA RID: 12970
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderId;

	// Token: 0x040032AB RID: 12971
	[PublicizedFrom(EAccessModifier.Private)]
	public int vehicleId;

	// Token: 0x040032AC RID: 12972
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort syncFlags;

	// Token: 0x040032AD RID: 12973
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream entityData = MemoryPools.poolMemoryStream.AllocSync(true);
}
