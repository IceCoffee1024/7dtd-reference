using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007F1 RID: 2033
[Preserve]
public class NetPackageEntityWaypointList : NetPackage
{
	// Token: 0x06003C04 RID: 15364 RVA: 0x00180C8F File Offset: 0x0017EE8F
	public NetPackageEntityWaypointList Setup(eWayPointListType _listType, [TupleElementNames(new string[] { "entityId", "position" })] List<ValueTuple<int, Vector3>> _positions)
	{
		this.listType = (short)_listType;
		this.positions = new List<ValueTuple<int, Vector3>>(_positions);
		return this;
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x00180CA8 File Offset: 0x0017EEA8
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

	// Token: 0x06003C06 RID: 15366 RVA: 0x00180CF0 File Offset: 0x0017EEF0
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

	// Token: 0x06003C07 RID: 15367 RVA: 0x00180D44 File Offset: 0x0017EF44
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

	// Token: 0x06003C08 RID: 15368 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040030CA RID: 12490
	[PublicizedFrom(EAccessModifier.Private)]
	public short listType;

	// Token: 0x040030CB RID: 12491
	[TupleElementNames(new string[] { "entityId", "position" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, Vector3>> positions;
}
