using System;
using UnityEngine.Scripting;

// Token: 0x02000827 RID: 2087
[Preserve]
public class NetPackagePlayerId : NetPackage
{
	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x06003D4C RID: 15692 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003D4D RID: 15693 RVA: 0x00185996 File Offset: 0x00183B96
	public NetPackagePlayerId Setup(int _id, int _teamNumber, PlayerDataFile _playerDataFile, int _chunkViewDim)
	{
		this.id = _id;
		this.teamNumber = _teamNumber;
		this.playerDataFile = _playerDataFile;
		this.chunkViewDim = _chunkViewDim;
		return this;
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x001859B6 File Offset: 0x00183BB6
	public override void read(PooledBinaryReader _reader)
	{
		this.id = _reader.ReadInt32();
		this.teamNumber = (int)_reader.ReadInt16();
		this.playerDataFile = new PlayerDataFile();
		this.playerDataFile.ReadNetwork(_reader);
		this.chunkViewDim = _reader.ReadInt32();
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x001859F3 File Offset: 0x00183BF3
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.id);
		_writer.Write((short)this.teamNumber);
		this.playerDataFile.WriteNetwork(_writer);
		_writer.Write(this.chunkViewDim);
	}

	// Token: 0x06003D50 RID: 15696 RVA: 0x00185A2D File Offset: 0x00183C2D
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.PlayerId(this.id, this.teamNumber, this.playerDataFile, this.chunkViewDim);
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x0017CB1E File Offset: 0x0017AD1E
	public override int GetLength()
	{
		return 40;
	}

	// Token: 0x040031BD RID: 12733
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040031BE RID: 12734
	[PublicizedFrom(EAccessModifier.Private)]
	public int teamNumber;

	// Token: 0x040031BF RID: 12735
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerDataFile playerDataFile;

	// Token: 0x040031C0 RID: 12736
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkViewDim;
}
