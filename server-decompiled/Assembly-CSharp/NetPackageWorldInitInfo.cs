using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200087D RID: 2173
[Preserve]
public class NetPackageWorldInitInfo : NetPackage
{
	// Token: 0x170006B7 RID: 1719
	// (get) Token: 0x06003EE0 RID: 16096 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x0018DDB4 File Offset: 0x0018BFB4
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

	// Token: 0x06003EE2 RID: 16098 RVA: 0x0018DE3C File Offset: 0x0018C03C
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

	// Token: 0x06003EE3 RID: 16099 RVA: 0x0018DED8 File Offset: 0x0018C0D8
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

	// Token: 0x06003EE4 RID: 16100 RVA: 0x0018DFA8 File Offset: 0x0018C1A8
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

	// Token: 0x04003324 RID: 13092
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance.Serializable> eventPrefabs;

	// Token: 0x04003325 RID: 13093
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, WallVolume>> wallVolumes;

	// Token: 0x04003326 RID: 13094
	[PublicizedFrom(EAccessModifier.Private)]
	public int dataLength;
}
