using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007E6 RID: 2022
[Preserve]
public class NetPackageEntityWaypointList : NetPackage
{
	// Token: 0x06003BA5 RID: 15269 RVA: 0x0017E8B7 File Offset: 0x0017CAB7
	public NetPackageEntityWaypointList Setup(eWayPointListType _listType, [TupleElementNames(new string[] { "entityId", "position" })] List<ValueTuple<int, Vector3>> _positions)
	{
		this.listType = (short)_listType;
		this.positions = new List<ValueTuple<int, Vector3>>(_positions);
		return this;
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x0017E8D0 File Offset: 0x0017CAD0
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

	// Token: 0x06003BA7 RID: 15271 RVA: 0x0017E918 File Offset: 0x0017CB18
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

	// Token: 0x06003BA8 RID: 15272 RVA: 0x0017E96C File Offset: 0x0017CB6C
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

	// Token: 0x06003BA9 RID: 15273 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400304B RID: 12363
	[PublicizedFrom(EAccessModifier.Private)]
	public short listType;

	// Token: 0x0400304C RID: 12364
	[TupleElementNames(new string[] { "entityId", "position" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, Vector3>> positions;
}
