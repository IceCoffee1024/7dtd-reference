using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000863 RID: 2147
[Preserve]
public class NetPackageVehiclePositions : NetPackage
{
	// Token: 0x06003EB8 RID: 16056 RVA: 0x0018B713 File Offset: 0x00189913
	public NetPackageVehiclePositions Setup([TupleElementNames(new string[] { "entityId", "position" })] List<ValueTuple<int, Vector3>> _positions)
	{
		this.positions = new List<ValueTuple<int, Vector3>>(_positions);
		return this;
	}

	// Token: 0x06003EB9 RID: 16057 RVA: 0x0018B724 File Offset: 0x00189924
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			primaryPlayer.Waypoints.SetEntityVehicleWaypointFromVehicleManager(this.positions);
		}
	}

	// Token: 0x06003EBA RID: 16058 RVA: 0x0018B754 File Offset: 0x00189954
	public override void read(PooledBinaryReader _reader)
	{
		int num = _reader.ReadInt32();
		this.positions = new List<ValueTuple<int, Vector3>>();
		for (int i = 0; i < num; i++)
		{
			this.positions.Add(new ValueTuple<int, Vector3>(_reader.ReadInt32(), StreamUtils.ReadVector3(_reader)));
		}
	}

	// Token: 0x06003EBB RID: 16059 RVA: 0x0018B79C File Offset: 0x0018999C
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

	// Token: 0x06003EBC RID: 16060 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040032B7 RID: 12983
	[TupleElementNames(new string[] { "entityId", "position" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, Vector3>> positions;
}
