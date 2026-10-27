using System;
using UnityEngine.Scripting;

// Token: 0x0200086C RID: 2156
[Preserve]
public class NetPackageVehicleCount : NetPackage
{
	// Token: 0x06003E68 RID: 15976 RVA: 0x0018C133 File Offset: 0x0018A333
	public NetPackageVehicleCount Setup()
	{
		this.vehicleCount = VehicleManager.GetServerVehicleCount();
		this.turretCount = TurretTracker.GetServerTurretCount();
		this.droneCount = DroneManager.GetServerDroneCount();
		return this;
	}

	// Token: 0x06003E69 RID: 15977 RVA: 0x0018C157 File Offset: 0x0018A357
	public override void read(PooledBinaryReader _reader)
	{
		this.vehicleCount = _reader.ReadInt32();
		this.turretCount = _reader.ReadInt32();
		this.droneCount = _reader.ReadInt32();
	}

	// Token: 0x06003E6A RID: 15978 RVA: 0x0018C17D File Offset: 0x0018A37D
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.vehicleCount);
		_writer.Write(this.turretCount);
		_writer.Write(this.droneCount);
	}

	// Token: 0x06003E6B RID: 15979 RVA: 0x0018C1AA File Offset: 0x0018A3AA
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		VehicleManager.SetServerVehicleCount(this.vehicleCount);
		TurretTracker.SetServerTurretCount(this.turretCount);
		DroneManager.SetServerDroneCount(this.droneCount);
	}

	// Token: 0x040032B2 RID: 12978
	public int vehicleCount;

	// Token: 0x040032B3 RID: 12979
	public int turretCount;

	// Token: 0x040032B4 RID: 12980
	public int droneCount;
}
