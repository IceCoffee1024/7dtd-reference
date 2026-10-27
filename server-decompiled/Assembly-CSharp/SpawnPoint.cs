using System;
using UnityEngine;

// Token: 0x02000BD6 RID: 3030
public class SpawnPoint
{
	// Token: 0x06005AFC RID: 23292 RVA: 0x00232917 File Offset: 0x00230B17
	public SpawnPoint()
	{
		this.spawnPosition = SpawnPosition.Undef;
		this.team = 0;
		this.activeInGameMode = 0;
	}

	// Token: 0x06005AFD RID: 23293 RVA: 0x00232938 File Offset: 0x00230B38
	public SpawnPoint(Vector3i _blockPos)
	{
		this.spawnPosition = new SpawnPosition(_blockPos, 0f);
		this.team = 0;
		this.activeInGameMode = -1;
	}

	// Token: 0x06005AFE RID: 23294 RVA: 0x0023295F File Offset: 0x00230B5F
	public SpawnPoint(Vector3 _position, float _heading)
	{
		this.spawnPosition = new SpawnPosition(_position, _heading);
		this.team = 0;
		this.activeInGameMode = -1;
	}

	// Token: 0x06005AFF RID: 23295 RVA: 0x00232982 File Offset: 0x00230B82
	public void Read(IBinaryReaderOrWriter _readerOrWriter, uint _version)
	{
		this.spawnPosition.Read(_readerOrWriter, _version);
		this.team = _readerOrWriter.ReadWrite(0);
		this.activeInGameMode = _readerOrWriter.ReadWrite(0);
	}

	// Token: 0x06005B00 RID: 23296 RVA: 0x002329AB File Offset: 0x00230BAB
	public void Read(PooledBinaryReader _br, uint _version)
	{
		this.spawnPosition.Read(_br, _version);
		this.team = _br.ReadInt32();
		this.activeInGameMode = _br.ReadInt32();
	}

	// Token: 0x06005B01 RID: 23297 RVA: 0x002329D2 File Offset: 0x00230BD2
	public void Write(PooledBinaryWriter _bw)
	{
		this.spawnPosition.Write(_bw);
		_bw.Write(this.team);
		_bw.Write(this.activeInGameMode);
	}

	// Token: 0x06005B02 RID: 23298 RVA: 0x002329F8 File Offset: 0x00230BF8
	public override int GetHashCode()
	{
		return this.spawnPosition.ToBlockPos().GetHashCode();
	}

	// Token: 0x040046DA RID: 18138
	public SpawnPosition spawnPosition;

	// Token: 0x040046DB RID: 18139
	public int team;

	// Token: 0x040046DC RID: 18140
	public int activeInGameMode;
}
