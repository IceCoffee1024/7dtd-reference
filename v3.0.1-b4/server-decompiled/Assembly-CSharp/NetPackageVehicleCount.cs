using System;
using UnityEngine.Scripting;

// Token: 0x02000853 RID: 2131
[Preserve]
public class NetPackageVehicleCount : NetPackage
{
	// Token: 0x06003E40 RID: 15936 RVA: 0x0018911F File Offset: 0x0018731F
	public NetPackageVehicleCount Setup()
	{
		this.vehicleCount = VehicleManager.GetServerVehicleCount();
		this.turretCount = TurretTracker.GetServerTurretCount();
		this.droneCount = DroneManager.GetServerDroneCount();
		return this;
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x00189143 File Offset: 0x00187343
	public override void read(PooledBinaryReader _reader)
	{
		this.vehicleCount = _reader.ReadInt32();
		this.turretCount = _reader.ReadInt32();
		this.droneCount = _reader.ReadInt32();
	}

	// Token: 0x06003E42 RID: 15938 RVA: 0x00189169 File Offset: 0x00187369
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.vehicleCount);
		_writer.Write(this.turretCount);
		_writer.Write(this.droneCount);
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x00189196 File Offset: 0x00187396
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		VehicleManager.SetServerVehicleCount(this.vehicleCount);
		TurretTracker.SetServerTurretCount(this.turretCount);
		DroneManager.SetServerDroneCount(this.droneCount);
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x0017DEA0 File Offset: 0x0017C0A0
	public override int GetLength()
	{
		return 12;
	}

	// Token: 0x04003226 RID: 12838
	public int vehicleCount;

	// Token: 0x04003227 RID: 12839
	public int turretCount;

	// Token: 0x04003228 RID: 12840
	public int droneCount;
}
