using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BA8 RID: 2984
public class SpawnPointList : List<SpawnPoint>
{
	// Token: 0x06005A3E RID: 23102 RVA: 0x0022B468 File Offset: 0x00229668
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

	// Token: 0x06005A3F RID: 23103 RVA: 0x0022B4A8 File Offset: 0x002296A8
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

	// Token: 0x06005A40 RID: 23104 RVA: 0x0022B5A8 File Offset: 0x002297A8
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

	// Token: 0x06005A41 RID: 23105 RVA: 0x0022B5F0 File Offset: 0x002297F0
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

	// Token: 0x06005A42 RID: 23106 RVA: 0x0022B638 File Offset: 0x00229838
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write((byte)SpawnPointList.CurrentSaveVersion);
		_bw.Write(base.Count);
		for (int i = 0; i < base.Count; i++)
		{
			base[i].Write(_bw);
		}
	}

	// Token: 0x040045DD RID: 17885
	[PublicizedFrom(EAccessModifier.Private)]
	public static uint CurrentSaveVersion = 2U;
}
