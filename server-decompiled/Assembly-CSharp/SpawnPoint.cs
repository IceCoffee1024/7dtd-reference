using System;
using UnityEngine;

// Token: 0x02000BCB RID: 3019
public class SpawnPoint
{
	// Token: 0x06005AD7 RID: 23255 RVA: 0x002313BB File Offset: 0x0022F5BB
	public SpawnPoint()
	{
		this.spawnPosition = SpawnPosition.Undef;
		this.team = 0;
		this.activeInGameMode = 0;
	}

	// Token: 0x06005AD8 RID: 23256 RVA: 0x002313DC File Offset: 0x0022F5DC
	public SpawnPoint(Vector3i _blockPos)
	{
		this.spawnPosition = new SpawnPosition(_blockPos, 0f);
		this.team = 0;
		this.activeInGameMode = -1;
	}

	// Token: 0x06005AD9 RID: 23257 RVA: 0x00231403 File Offset: 0x0022F603
	public SpawnPoint(Vector3 _position, float _heading)
	{
		this.spawnPosition = new SpawnPosition(_position, _heading);
		this.team = 0;
		this.activeInGameMode = -1;
	}

	// Token: 0x06005ADA RID: 23258 RVA: 0x00231426 File Offset: 0x0022F626
	public void Read(IBinaryReaderOrWriter _readerOrWriter, uint _version)
	{
		this.spawnPosition.Read(_readerOrWriter, _version);
		this.team = _readerOrWriter.ReadWrite(0);
		this.activeInGameMode = _readerOrWriter.ReadWrite(0);
	}

	// Token: 0x06005ADB RID: 23259 RVA: 0x0023144F File Offset: 0x0022F64F
	public void Read(PooledBinaryReader _br, uint _version)
	{
		this.spawnPosition.Read(_br, _version);
		this.team = _br.ReadInt32();
		this.activeInGameMode = _br.ReadInt32();
	}

	// Token: 0x06005ADC RID: 23260 RVA: 0x00231476 File Offset: 0x0022F676
	public void Write(PooledBinaryWriter _bw)
	{
		this.spawnPosition.Write(_bw);
		_bw.Write(this.team);
		_bw.Write(this.activeInGameMode);
	}

	// Token: 0x06005ADD RID: 23261 RVA: 0x0023149C File Offset: 0x0022F69C
	public override int GetHashCode()
	{
		return this.spawnPosition.ToBlockPos().GetHashCode();
	}

	// Token: 0x040046B5 RID: 18101
	public SpawnPosition spawnPosition;

	// Token: 0x040046B6 RID: 18102
	public int team;

	// Token: 0x040046B7 RID: 18103
	public int activeInGameMode;
}
