using System;
using UnityEngine.Scripting;

// Token: 0x02000827 RID: 2087
[Preserve]
public class NetPackagePlayerId : NetPackage
{
	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06003D0D RID: 15629 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D0E RID: 15630 RVA: 0x001855E6 File Offset: 0x001837E6
	public NetPackagePlayerId Setup(int _id, int _teamNumber, PlayerDataFile _playerDataFile, int _chunkViewDim)
	{
		this.id = _id;
		this.teamNumber = _teamNumber;
		this.playerDataFile = _playerDataFile;
		this.chunkViewDim = _chunkViewDim;
		return this;
	}

	// Token: 0x06003D0F RID: 15631 RVA: 0x00185606 File Offset: 0x00183806
	public override void read(PooledBinaryReader _reader)
	{
		this.id = _reader.ReadInt32();
		this.teamNumber = (int)_reader.ReadInt16();
		this.playerDataFile = new PlayerDataFile();
		this.playerDataFile.ReadNetwork(_reader, StreamModeRead.FromServer);
		this.chunkViewDim = _reader.ReadInt32();
	}

	// Token: 0x06003D10 RID: 15632 RVA: 0x00185644 File Offset: 0x00183844
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.id);
		_writer.Write((short)this.teamNumber);
		this.playerDataFile.WriteNetwork(_writer, StreamModeWrite.ToClient);
		_writer.Write(this.chunkViewDim);
	}

	// Token: 0x06003D11 RID: 15633 RVA: 0x0018567F File Offset: 0x0018387F
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PlayerId(this.id, this.teamNumber, this.playerDataFile, this.chunkViewDim);
	}

	// Token: 0x040031A3 RID: 12707
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040031A4 RID: 12708
	[PublicizedFrom(EAccessModifier.Private)]
	public int teamNumber;

	// Token: 0x040031A5 RID: 12709
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerDataFile playerDataFile;

	// Token: 0x040031A6 RID: 12710
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkViewDim;
}
