using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BD7 RID: 3031
public class SpawnPointList : List<SpawnPoint>
{
	// Token: 0x06005B04 RID: 23300 RVA: 0x00232A28 File Offset: 0x00230C28
	public SpawnPoint Find(Vector3i _blockPos)
	{
		for (int i = 0; i < base.Count; i++)
		{
			SpawnPoint spawnPoint = base[i];
			if (spawnPoint.spawnPosition.ToBlockPos().Equals(_blockPos))
			{
				return spawnPoint;
			}
		}
		return null;
	}

	// Token: 0x06005B05 RID: 23301 RVA: 0x00232A68 File Offset: 0x00230C68
	public virtual SpawnPosition GetRandomSpawnPosition(World _world, Vector3? _refPosition = null, int _minDistance = 0, int _maxDistance = 0)
	{
		if (base.Count > 0)
		{
			GameRandom gameRandom = _world.GetGameRandom();
			if (_refPosition == null)
			{
				return base[gameRandom.RandomRange(base.Count)].spawnPosition;
			}
			Vector3 value = _refPosition.Value;
			for (int i = 0; i < 100; i++)
			{
				int num = gameRandom.RandomRange(base.Count);
				SpawnPosition spawnPosition = base[num].spawnPosition;
				float magnitude = (spawnPosition.position - value).magnitude;
				if (magnitude >= (float)_minDistance && magnitude <= (float)_maxDistance)
				{
					return spawnPosition;
				}
			}
			float num2 = float.MaxValue;
			int num3 = -1;
			for (int j = 0; j < base.Count; j++)
			{
				float sqrMagnitude = (base[j].spawnPosition.position - value).sqrMagnitude;
				if (sqrMagnitude < num2)
				{
					num2 = sqrMagnitude;
					num3 = j;
				}
			}
			if (num3 != -1)
			{
				return base[num3].spawnPosition;
			}
		}
		return SpawnPosition.Undef;
	}

	// Token: 0x06005B06 RID: 23302 RVA: 0x00232B68 File Offset: 0x00230D68
	public void Read(IBinaryReaderOrWriter _readerOrWriter)
	{
		if (_readerOrWriter == null)
		{
			return;
		}
		base.Clear();
		uint num = (uint)_readerOrWriter.ReadWrite(0);
		int num2 = _readerOrWriter.ReadWrite(0);
		for (int i = 0; i < num2; i++)
		{
			SpawnPoint spawnPoint = new SpawnPoint();
			spawnPoint.Read(_readerOrWriter, num);
			base.Add(spawnPoint);
		}
	}

	// Token: 0x06005B07 RID: 23303 RVA: 0x00232BB0 File Offset: 0x00230DB0
	public void Read(PooledBinaryReader _br)
	{
		if (_br == null)
		{
			return;
		}
		base.Clear();
		uint num = (uint)_br.ReadByte();
		int num2 = _br.ReadInt32();
		for (int i = 0; i < num2; i++)
		{
			SpawnPoint spawnPoint = new SpawnPoint();
			spawnPoint.Read(_br, num);
			base.Add(spawnPoint);
		}
	}

	// Token: 0x06005B08 RID: 23304 RVA: 0x00232BF8 File Offset: 0x00230DF8
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write((byte)SpawnPointList.CurrentSaveVersion);
		_bw.Write(base.Count);
		for (int i = 0; i < base.Count; i++)
		{
			base[i].Write(_bw);
		}
	}

	// Token: 0x040046DD RID: 18141
	[PublicizedFrom(EAccessModifier.Private)]
	public static uint CurrentSaveVersion = 2U;
}
