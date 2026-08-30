using System;
using UnityEngine.Scripting;

// Token: 0x0200085F RID: 2143
[Preserve]
public class NetPackageVehicleCount : NetPackage
{
	// Token: 0x06003EA5 RID: 16037 RVA: 0x0018B1EF File Offset: 0x001893EF
	public NetPackageVehicleCount Setup()
	{
		this.vehicleCount = VehicleManager.GetServerVehicleCount();
		this.turretCount = TurretTracker.GetServerTurretCount();
		this.droneCount = DroneManager.GetServerDroneCount();
		return this;
	}

	// Token: 0x06003EA6 RID: 16038 RVA: 0x0018B213 File Offset: 0x00189413
	public override void read(PooledBinaryReader _reader)
	{
		this.vehicleCount = _reader.ReadInt32();
		this.turretCount = _reader.ReadInt32();
		this.droneCount = _reader.ReadInt32();
	}

	// Token: 0x06003EA7 RID: 16039 RVA: 0x0018B239 File Offset: 0x00189439
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.vehicleCount);
		_writer.Write(this.turretCount);
		_writer.Write(this.droneCount);
	}

	// Token: 0x06003EA8 RID: 16040 RVA: 0x0018B266 File Offset: 0x00189466
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		VehicleManager.SetServerVehicleCount(this.vehicleCount);
		TurretTracker.SetServerTurretCount(this.turretCount);
		DroneManager.SetServerDroneCount(this.droneCount);
	}

	// Token: 0x06003EA9 RID: 16041 RVA: 0x00180278 File Offset: 0x0017E478
	public override int GetLength()
	{
		return 12;
	}

	// Token: 0x040032A7 RID: 12967
	public int vehicleCount;

	// Token: 0x040032A8 RID: 12968
	public int turretCount;

	// Token: 0x040032A9 RID: 12969
	public int droneCount;
}
