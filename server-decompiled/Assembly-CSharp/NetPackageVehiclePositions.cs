using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000866 RID: 2150
[Preserve]
public class NetPackageVehiclePositions : NetPackage
{
	// Token: 0x06003E55 RID: 15957 RVA: 0x0018B59F File Offset: 0x0018979F
	public NetPackageVehiclePositions Setup([TupleElementNames(new string[] { "entityId", "position" })] List<ValueTuple<int, Vector3>> _positions)
	{
		this.positions = new List<ValueTuple<int, Vector3>>(_positions);
		return this;
	}

	// Token: 0x06003E56 RID: 15958 RVA: 0x0018B5B0 File Offset: 0x001897B0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			primaryPlayer.Waypoints.SetEntityVehicleWaypointFromVehicleManager(this.positions);
		}
	}

	// Token: 0x06003E57 RID: 15959 RVA: 0x0018B5E0 File Offset: 0x001897E0
	public override void read(PooledBinaryReader _reader)
	{
		int num = _reader.ReadInt32();
		this.positions = new List<ValueTuple<int, Vector3>>();
		for (int i = 0; i < num; i++)
		{
			this.positions.Add(new ValueTuple<int, Vector3>(_reader.ReadInt32(), StreamUtils.ReadVector3(_reader)));
		}
	}

	// Token: 0x06003E58 RID: 15960 RVA: 0x0018B628 File Offset: 0x00189828
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.positions.Count);
		for (int i = 0; i < this.positions.Count; i++)
		{
			_writer.Write(this.positions[i].Item1);
			StreamUtils.Write(_writer, this.positions[i].Item2);
		}
	}

	// Token: 0x040032A2 RID: 12962
	[TupleElementNames(new string[] { "entityId", "position" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, Vector3>> positions;
}
