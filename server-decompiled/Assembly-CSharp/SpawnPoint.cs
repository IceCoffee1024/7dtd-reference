using System;
using UnityEngine;

// Token: 0x02000BB9 RID: 3001
public class SpawnPoint
{
	// Token: 0x06005ABE RID: 23230 RVA: 0x0022E0BF File Offset: 0x0022C2BF
	public SpawnPoint()
	{
		this.spawnPosition = SpawnPosition.Undef;
		this.team = 0;
		this.activeInGameMode = 0;
	}

	// Token: 0x06005ABF RID: 23231 RVA: 0x0022E0E0 File Offset: 0x0022C2E0
	public SpawnPoint(Vector3i _blockPos)
	{
		this.spawnPosition = new SpawnPosition(_blockPos, 0f);
		this.team = 0;
		this.activeInGameMode = -1;
	}

	// Token: 0x06005AC0 RID: 23232 RVA: 0x0022E107 File Offset: 0x0022C307
	public SpawnPoint(Vector3 _position, float _heading)
	{
		this.spawnPosition = new SpawnPosition(_position, _heading);
		this.team = 0;
		this.activeInGameMode = -1;
	}

	// Token: 0x06005AC1 RID: 23233 RVA: 0x0022E12A File Offset: 0x0022C32A
	public void Read(IBinaryReaderOrWriter _readerOrWriter, uint _version)
	{
		this.spawnPosition.Read(_readerOrWriter, _version);
		this.team = _readerOrWriter.ReadWrite(0);
		this.activeInGameMode = _readerOrWriter.ReadWrite(0);
	}

	// Token: 0x06005AC2 RID: 23234 RVA: 0x0022E153 File Offset: 0x0022C353
	public void Read(PooledBinaryReader _br, uint _version)
	{
		this.spawnPosition.Read(_br, _version);
		this.team = _br.ReadInt32();
		this.activeInGameMode = _br.ReadInt32();
	}

	// Token: 0x06005AC3 RID: 23235 RVA: 0x0022E17A File Offset: 0x0022C37A
	public void Write(PooledBinaryWriter _bw)
	{
		this.spawnPosition.Write(_bw);
		_bw.Write(this.team);
		_bw.Write(this.activeInGameMode);
	}

	// Token: 0x06005AC4 RID: 23236 RVA: 0x0022E1A0 File Offset: 0x0022C3A0
	public override int GetHashCode()
	{
		return this.spawnPosition.ToBlockPos().GetHashCode();
	}

	// Token: 0x04004676 RID: 18038
	public SpawnPosition spawnPosition;

	// Token: 0x04004677 RID: 18039
	public int team;

	// Token: 0x04004678 RID: 18040
	public int activeInGameMode;
}
