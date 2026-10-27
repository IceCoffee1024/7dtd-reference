using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000870 RID: 2160
[Preserve]
public class NetPackageVehiclePositions : NetPackage
{
	// Token: 0x06003E78 RID: 15992 RVA: 0x0018C647 File Offset: 0x0018A847
	public NetPackageVehiclePositions Setup([TupleElementNames(new string[] { "entityId", "position" })] List<ValueTuple<int, Vector3>> _positions)
	{
		this.positions = new List<ValueTuple<int, Vector3>>(_positions);
		return this;
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x0018C658 File Offset: 0x0018A858
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			primaryPlayer.Waypoints.SetEntityVehicleWaypointFromVehicleManager(this.positions);
		}
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x0018C688 File Offset: 0x0018A888
	public override void read(PooledBinaryReader _reader)
	{
		int num = _reader.ReadInt32();
		this.positions = new List<ValueTuple<int, Vector3>>();
		for (int i = 0; i < num; i++)
		{
			this.positions.Add(new ValueTuple<int, Vector3>(_reader.ReadInt32(), StreamUtils.ReadVector3(_reader)));
		}
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x0018C6D0 File Offset: 0x0018A8D0
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

	// Token: 0x040032C2 RID: 12994
	[TupleElementNames(new string[] { "entityId", "position" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, Vector3>> positions;
}
