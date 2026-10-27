using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000887 RID: 2183
[Preserve]
public class NetPackageWorldInitInfo : NetPackage
{
	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x06003F03 RID: 16131 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003F04 RID: 16132 RVA: 0x0018EE5C File Offset: 0x0018D05C
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

	// Token: 0x06003F05 RID: 16133 RVA: 0x0018EEE4 File Offset: 0x0018D0E4
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

	// Token: 0x06003F06 RID: 16134 RVA: 0x0018EF80 File Offset: 0x0018D180
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

	// Token: 0x06003F07 RID: 16135 RVA: 0x0018F050 File Offset: 0x0018D250
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

	// Token: 0x04003344 RID: 13124
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance.Serializable> eventPrefabs;

	// Token: 0x04003345 RID: 13125
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, WallVolume>> wallVolumes;

	// Token: 0x04003346 RID: 13126
	[PublicizedFrom(EAccessModifier.Private)]
	public int dataLength;
}
