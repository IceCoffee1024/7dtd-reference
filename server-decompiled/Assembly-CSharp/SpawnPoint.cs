using System;
using UnityEngine;

// Token: 0x02000BA7 RID: 2983
public class SpawnPoint
{
	// Token: 0x06005A36 RID: 23094 RVA: 0x0022B357 File Offset: 0x00229557
	public SpawnPoint()
	{
		this.spawnPosition = SpawnPosition.Undef;
		this.team = 0;
		this.activeInGameMode = 0;
	}

	// Token: 0x06005A37 RID: 23095 RVA: 0x0022B378 File Offset: 0x00229578
	public SpawnPoint(Vector3i _blockPos)
	{
		this.spawnPosition = new SpawnPosition(_blockPos, 0f);
		this.team = 0;
		this.activeInGameMode = -1;
	}

	// Token: 0x06005A38 RID: 23096 RVA: 0x0022B39F File Offset: 0x0022959F
	public SpawnPoint(Vector3 _position, float _heading)
	{
		this.spawnPosition = new SpawnPosition(_position, _heading);
		this.team = 0;
		this.activeInGameMode = -1;
	}

	// Token: 0x06005A39 RID: 23097 RVA: 0x0022B3C2 File Offset: 0x002295C2
	public void Read(IBinaryReaderOrWriter _readerOrWriter, uint _version)
	{
		this.spawnPosition.Read(_readerOrWriter, _version);
		this.team = _readerOrWriter.ReadWrite(0);
		this.activeInGameMode = _readerOrWriter.ReadWrite(0);
	}

	// Token: 0x06005A3A RID: 23098 RVA: 0x0022B3EB File Offset: 0x002295EB
	public void Read(PooledBinaryReader _br, uint _version)
	{
		this.spawnPosition.Read(_br, _version);
		this.team = _br.ReadInt32();
		this.activeInGameMode = _br.ReadInt32();
	}

	// Token: 0x06005A3B RID: 23099 RVA: 0x0022B412 File Offset: 0x00229612
	public void Write(PooledBinaryWriter _bw)
	{
		this.spawnPosition.Write(_bw);
		_bw.Write(this.team);
		_bw.Write(this.activeInGameMode);
	}

	// Token: 0x06005A3C RID: 23100 RVA: 0x0022B438 File Offset: 0x00229638
	public override int GetHashCode()
	{
		return this.spawnPosition.ToBlockPos().GetHashCode();
	}

	// Token: 0x040045DA RID: 17882
	public SpawnPosition spawnPosition;

	// Token: 0x040045DB RID: 17883
	public int team;

	// Token: 0x040045DC RID: 17884
	public int activeInGameMode;
}
