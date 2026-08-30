using System;
using UnityEngine.Scripting;

// Token: 0x02000854 RID: 2132
[Preserve]
public class NetPackageVehicleDataSync : NetPackage
{
	// Token: 0x06003E46 RID: 15942 RVA: 0x001891BC File Offset: 0x001873BC
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

	// Token: 0x06003E47 RID: 15943 RVA: 0x00189220 File Offset: 0x00187420
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageVehicleDataSync()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.entityData);
	}

	// Token: 0x06003E48 RID: 15944 RVA: 0x00189258 File Offset: 0x00187458
	public override void read(PooledBinaryReader _br)
	{
		this.senderId = _br.ReadInt32();
		this.vehicleId = _br.ReadInt32();
		this.syncFlags = _br.ReadUInt16();
		int num = (int)_br.ReadUInt16();
		StreamUtils.StreamCopy(_br.BaseStream, this.entityData, num, null, true);
	}

	// Token: 0x06003E49 RID: 15945 RVA: 0x001892A4 File Offset: 0x001874A4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.senderId);
		_bw.Write(this.vehicleId);
		_bw.Write(this.syncFlags);
		_bw.Write((ushort)this.entityData.Length);
		this.entityData.WriteTo(_bw.BaseStream);
	}

	// Token: 0x06003E4A RID: 15946 RVA: 0x00189300 File Offset: 0x00187500
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

	// Token: 0x06003E4B RID: 15947 RVA: 0x00189468 File Offset: 0x00187668
	public override int GetLength()
	{
		return (int)(12L + this.entityData.Length);
	}

	// Token: 0x04003229 RID: 12841
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderId;

	// Token: 0x0400322A RID: 12842
	[PublicizedFrom(EAccessModifier.Private)]
	public int vehicleId;

	// Token: 0x0400322B RID: 12843
	[PublicizedFrom(EAccessModifier.Private)]
	public ushort syncFlags;

	// Token: 0x0400322C RID: 12844
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream entityData = MemoryPools.poolMemoryStream.AllocSync(true);
}
