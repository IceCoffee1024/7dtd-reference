using System;
using UnityEngine.Scripting;

// Token: 0x0200081C RID: 2076
[Preserve]
public class NetPackagePlayerId : NetPackage
{
	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x06003CED RID: 15597 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003CEE RID: 15598 RVA: 0x001835BE File Offset: 0x001817BE
	public NetPackagePlayerId Setup(int _id, int _teamNumber, PlayerDataFile _playerDataFile, int _chunkViewDim)
	{
		this.id = _id;
		this.teamNumber = _teamNumber;
		this.playerDataFile = _playerDataFile;
		this.chunkViewDim = _chunkViewDim;
		return this;
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x001835DE File Offset: 0x001817DE
	public override void read(PooledBinaryReader _reader)
	{
		this.id = _reader.ReadInt32();
		this.teamNumber = (int)_reader.ReadInt16();
		this.playerDataFile = new PlayerDataFile();
		this.playerDataFile.ReadNetwork(_reader);
		this.chunkViewDim = _reader.ReadInt32();
	}

	// Token: 0x06003CF0 RID: 15600 RVA: 0x0018361B File Offset: 0x0018181B
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.id);
		_writer.Write((short)this.teamNumber);
		this.playerDataFile.WriteNetwork(_writer);
		_writer.Write(this.chunkViewDim);
	}

	// Token: 0x06003CF1 RID: 15601 RVA: 0x00183655 File Offset: 0x00181855
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PlayerId(this.id, this.teamNumber, this.playerDataFile, this.chunkViewDim);
	}

	// Token: 0x06003CF2 RID: 15602 RVA: 0x0017A89E File Offset: 0x00178A9E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x0400313E RID: 12606
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x0400313F RID: 12607
	[PublicizedFrom(EAccessModifier.Private)]
	public int teamNumber;

	// Token: 0x04003140 RID: 12608
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerDataFile playerDataFile;

	// Token: 0x04003141 RID: 12609
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkViewDim;
}
