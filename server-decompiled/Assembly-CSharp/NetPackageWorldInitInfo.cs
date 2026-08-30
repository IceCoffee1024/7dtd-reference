using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200086E RID: 2158
[Preserve]
public class NetPackageWorldInitInfo : NetPackage
{
	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06003EE9 RID: 16105 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003EEA RID: 16106 RVA: 0x0018BED0 File Offset: 0x0018A0D0
	public NetPackageWorldInitInfo Setup(List<PrefabInstance.Serializable> _eventPrefabs, List<ValueTuple<int, WallVolume>> _wallVolumes)
	{
		this.eventPrefabs = _eventPrefabs;
		foreach (PrefabInstance.Serializable serializable in this.eventPrefabs)
		{
			this.dataLength += serializable.GetLength();
		}
		this.wallVolumes = _wallVolumes;
		this.dataLength += _wallVolumes.Count * 29;
		return this;
	}

	// Token: 0x06003EEB RID: 16107 RVA: 0x0018BF58 File Offset: 0x0018A158
	public override void read(PooledBinaryReader _br)
	{
		if (this.eventPrefabs == null)
		{
			this.eventPrefabs = new List<PrefabInstance.Serializable>();
		}
		this.eventPrefabs.Clear();
		int num = _br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.eventPrefabs.Add(new PrefabInstance.Serializable(_br));
		}
		if (this.wallVolumes == null)
		{
			this.wallVolumes = new List<ValueTuple<int, WallVolume>>();
		}
		this.eventPrefabs.Clear();
		int num2 = _br.ReadInt32();
		for (int j = 0; j < num2; j++)
		{
			this.wallVolumes.Add(new ValueTuple<int, WallVolume>(_br.ReadInt32(), WallVolume.Read(_br)));
		}
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x0018BFF4 File Offset: 0x0018A1F4
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.eventPrefabs.Count);
		foreach (PrefabInstance.Serializable serializable in this.eventPrefabs)
		{
			serializable.Write(_bw);
		}
		_bw.Write(this.wallVolumes.Count);
		foreach (ValueTuple<int, WallVolume> valueTuple in this.wallVolumes)
		{
			int item = valueTuple.Item1;
			WallVolume item2 = valueTuple.Item2;
			_bw.Write(item);
			item2.Write(_bw);
		}
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x0018C0C4 File Offset: 0x0018A2C4
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			Log.Error("[NetPackageWorldInitInfo] world is null");
			return;
		}
		EventPrefabsClient eventPrefabsClient = _world.m_EventPrefabsClient;
		if (eventPrefabsClient != null)
		{
			using (List<PrefabInstance.Serializable>.Enumerator enumerator = this.eventPrefabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PrefabInstance.Serializable serializable = enumerator.Current;
					eventPrefabsClient.TryAdd(serializable.id, serializable.prefabName, serializable.rotation, serializable.position);
				}
				goto IL_006F;
			}
		}
		Log.Error("[NetPackageWorldInitInfo] EventPrefabsClient not available");
		IL_006F:
		_world.SetWallVolumesForClient(this.wallVolumes);
		_callbacks.worldInitInfoReceived = true;
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x0018C164 File Offset: 0x0018A364
	public override int GetLength()
	{
		return this.dataLength;
	}

	// Token: 0x040032B8 RID: 12984
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance.Serializable> eventPrefabs;

	// Token: 0x040032B9 RID: 12985
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, WallVolume>> wallVolumes;

	// Token: 0x040032BA RID: 12986
	[PublicizedFrom(EAccessModifier.Private)]
	public int dataLength;
}
