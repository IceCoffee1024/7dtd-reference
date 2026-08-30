using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200087A RID: 2170
[Preserve]
public class NetPackageWorldInitInfo : NetPackage
{
	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06003F4E RID: 16206 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003F4F RID: 16207 RVA: 0x0018DFA0 File Offset: 0x0018C1A0
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

	// Token: 0x06003F50 RID: 16208 RVA: 0x0018E028 File Offset: 0x0018C228
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

	// Token: 0x06003F51 RID: 16209 RVA: 0x0018E0C4 File Offset: 0x0018C2C4
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

	// Token: 0x06003F52 RID: 16210 RVA: 0x0018E194 File Offset: 0x0018C394
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

	// Token: 0x06003F53 RID: 16211 RVA: 0x0018E234 File Offset: 0x0018C434
	public override int GetLength()
	{
		return this.dataLength;
	}

	// Token: 0x04003339 RID: 13113
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance.Serializable> eventPrefabs;

	// Token: 0x0400333A RID: 13114
	[PublicizedFrom(EAccessModifier.Private)]
	public List<ValueTuple<int, WallVolume>> wallVolumes;

	// Token: 0x0400333B RID: 13115
	[PublicizedFrom(EAccessModifier.Private)]
	public int dataLength;
}
