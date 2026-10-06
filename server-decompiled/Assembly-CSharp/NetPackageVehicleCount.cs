using System;
using UnityEngine.Scripting;

// Token: 0x02000862 RID: 2146
[Preserve]
public class NetPackageVehicleCount : NetPackage
{
	// Token: 0x06003E45 RID: 15941 RVA: 0x0018B08B File Offset: 0x0018928B
	public NetPackageVehicleCount Setup()
	{
		this.vehicleCount = VehicleManager.GetServerVehicleCount();
		this.turretCount = TurretTracker.GetServerTurretCount();
		this.droneCount = DroneManager.GetServerDroneCount();
		return this;
	}

	// Token: 0x06003E46 RID: 15942 RVA: 0x0018B0AF File Offset: 0x001892AF
	public override void read(PooledBinaryReader _reader)
	{
		this.vehicleCount = _reader.ReadInt32();
		this.turretCount = _reader.ReadInt32();
		this.droneCount = _reader.ReadInt32();
	}

	// Token: 0x06003E47 RID: 15943 RVA: 0x0018B0D5 File Offset: 0x001892D5
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.vehicleCount);
		_writer.Write(this.turretCount);
		_writer.Write(this.droneCount);
	}

	// Token: 0x06003E48 RID: 15944 RVA: 0x0018B102 File Offset: 0x00189302
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		VehicleManager.SetServerVehicleCount(this.vehicleCount);
		TurretTracker.SetServerTurretCount(this.turretCount);
		DroneManager.SetServerDroneCount(this.droneCount);
	}

	// Token: 0x04003292 RID: 12946
	public int vehicleCount;

	// Token: 0x04003293 RID: 12947
	public int turretCount;

	// Token: 0x04003294 RID: 12948
	public int droneCount;
}
