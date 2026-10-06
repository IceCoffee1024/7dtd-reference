using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007F4 RID: 2036
[Preserve]
public class NetPackageEntityWaypointList : NetPackage
{
	// Token: 0x06003C08 RID: 15368 RVA: 0x001811C7 File Offset: 0x0017F3C7
	public NetPackageEntityWaypointList Setup(eWayPointListType _listType, [TupleElementNames(new string[] { "entityId", "position" })] List<ValueTuple<int, Vector3>> _positions)
	{
		this.listType = (short)_listType;
		this.positions = new List<ValueTuple<int, Vector3>>(_positions);
		return this;
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x001811E0 File Offset: 0x0017F3E0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			if (this.listType == 0)
			{
				primaryPlayer.Waypoints.SetEntityVehicleWaypointFromVehicleManager(this.positions);
				return;
			}
			primaryPlayer.Waypoints.SetDroneWaypointsFromDroneManager(this.positions);
		}
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x00181228 File Offset: 0x0017F428
	public override void read(PooledBinaryReader _reader)
	{
		this.listType = _reader.ReadInt16();
		int num = _reader.ReadInt32();
		this.positions = new List<ValueTuple<int, Vector3>>();
		for (int i = 0; i < num; i++)
		{
			this.positions.Add(new ValueTuple<int, Vector3>(_reader.ReadInt32(), StreamUtils.ReadVector3(_reader)));
		}
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x0018127C File Offset: 0x0017F47C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.listType);
		_writer.Write(this.positions.Count);
		for (int i = 0; i < this.positions.Count; i++)
		{
			_writer.Write(this.positions[i].Item1);
			StreamUtils.Write(_writer, this.positions[i].Item2);
		}
	}

	// Token: 0x040030C3 RID: 12483
	[PublicizedFrom(EAccessModifier.Private)]
	public short listType;

	// Token: 0x040030C4 RID: 12484
	[TupleElementNames(new string[] { "entityId", "position" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, Vector3>> positions;
}
