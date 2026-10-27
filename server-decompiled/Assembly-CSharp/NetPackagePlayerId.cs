using System;
using UnityEngine.Scripting;

// Token: 0x02000831 RID: 2097
[Preserve]
public class NetPackagePlayerId : NetPackage
{
	// Token: 0x1700068A RID: 1674
	// (get) Token: 0x06003D30 RID: 15664 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D31 RID: 15665 RVA: 0x001866EA File Offset: 0x001848EA
	public NetPackagePlayerId Setup(int _id, int _teamNumber, PlayerDataFile _playerDataFile, int _chunkViewDim)
	{
		this.id = _id;
		this.teamNumber = _teamNumber;
		this.playerDataFile = _playerDataFile;
		this.chunkViewDim = _chunkViewDim;
		return this;
	}

	// Token: 0x06003D32 RID: 15666 RVA: 0x0018670A File Offset: 0x0018490A
	public override void read(PooledBinaryReader _reader)
	{
		this.id = _reader.ReadInt32();
		this.teamNumber = (int)_reader.ReadInt16();
		this.playerDataFile = new PlayerDataFile();
		this.playerDataFile.ReadNetwork(_reader, StreamModeRead.FromServer);
		this.chunkViewDim = _reader.ReadInt32();
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x00186748 File Offset: 0x00184948
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.id);
		_writer.Write((short)this.teamNumber);
		this.playerDataFile.WriteNetwork(_writer, StreamModeWrite.ToClient);
		_writer.Write(this.chunkViewDim);
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x00186783 File Offset: 0x00184983
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PlayerId(this.id, this.teamNumber, this.playerDataFile, this.chunkViewDim);
	}

	// Token: 0x040031C3 RID: 12739
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040031C4 RID: 12740
	[PublicizedFrom(EAccessModifier.Private)]
	public int teamNumber;

	// Token: 0x040031C5 RID: 12741
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerDataFile playerDataFile;

	// Token: 0x040031C6 RID: 12742
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkViewDim;
}
