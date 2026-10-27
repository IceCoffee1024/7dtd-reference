using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007FE RID: 2046
[Preserve]
public class NetPackageEntityWaypointList : NetPackage
{
	// Token: 0x06003C2B RID: 15403 RVA: 0x001822CB File Offset: 0x001804CB
	public NetPackageEntityWaypointList Setup(eWayPointListType _listType, [TupleElementNames(new string[] { "entityId", "position" })] List<ValueTuple<int, Vector3>> _positions)
	{
		this.listType = (short)_listType;
		this.positions = new List<ValueTuple<int, Vector3>>(_positions);
		return this;
	}

	// Token: 0x06003C2C RID: 15404 RVA: 0x001822E4 File Offset: 0x001804E4
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

	// Token: 0x06003C2D RID: 15405 RVA: 0x0018232C File Offset: 0x0018052C
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

	// Token: 0x06003C2E RID: 15406 RVA: 0x00182380 File Offset: 0x00180580
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

	// Token: 0x040030E3 RID: 12515
	[PublicizedFrom(EAccessModifier.Private)]
	public short listType;

	// Token: 0x040030E4 RID: 12516
	[TupleElementNames(new string[] { "entityId", "position" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, Vector3>> positions;
}
