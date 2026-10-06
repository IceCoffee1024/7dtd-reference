using System;
using UnityEngine.Scripting;

// Token: 0x02000863 RID: 2147
[Preserve]
public class NetPackageVehicleDataSync : NetPackage
{
	// Token: 0x06003E4A RID: 15946 RVA: 0x0018B128 File Offset: 0x00189328
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

	// Token: 0x06003E4B RID: 15947 RVA: 0x0018B18C File Offset: 0x0018938C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageVehicleDataSync()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.entityData);
	}

	// Token: 0x06003E4C RID: 15948 RVA: 0x0018B1C4 File Offset: 0x001893C4
	public override void read(PooledBinaryReader _br)
	{
		this.senderId = _br.ReadInt32();
		this.vehicleId = _br.ReadInt32();
		this.syncFlags = _br.ReadUInt16();
		int num = (int)_br.ReadUInt16();
		StreamUtils.StreamCopy(_br.BaseStream, this.entityData, num, null, true);
	}

	// Token: 0x06003E4D RID: 15949 RVA: 0x0018B210 File Offset: 0x00189410
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.senderId);
		_bw.Write(this.vehicleId);
		_bw.Write(this.syncFlags);
		_bw.Write((ushort)this.entityData.Length);
		this.entityData.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003E4E RID: 15950 RVA: 0x0018B26C File Offset: 0x0018946C
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

	// Token: 0x04003295 RID: 12949
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderId;

	// Token: 0x04003296 RID: 12950
	[PublicizedFrom(EAccessModifier.Private)]
	public int vehicleId;

	// Token: 0x04003297 RID: 12951
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort syncFlags;

	// Token: 0x04003298 RID: 12952
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream entityData = MemoryPools.poolMemoryStream.AllocSync(true);
}
