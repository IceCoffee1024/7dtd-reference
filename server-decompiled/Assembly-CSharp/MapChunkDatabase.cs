using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000B66 RID: 2918
public class MapChunkDatabase : DatabaseWithFixedDS<int, ushort[]>, IMapChunkDatabase
{
	// Token: 0x0600580B RID: 22539 RVA: 0x0021C39C File Offset: 0x0021A59C
	public MapChunkDatabase(int _playerId)
		: base(7364973, 4, GamePrefs.GetInt(EnumGamePrefs.MaxUncoveredMapChunksPerPlayer), 512, 0, 131072)
	{
		this.playerId = _playerId;
	}

	// Token: 0x0600580C RID: 22540 RVA: 0x0021C408 File Offset: 0x0021A608
	public void Add(int _chunkX, int _chunkZ, ushort[] _mapColors)
	{
		int num = IMapChunkDatabase.ToChunkDBKey(_chunkX, _chunkZ);
		ushort[] array = base.GetDS(num);
		if (array == null)
		{
			array = new ushort[_mapColors.Length];
		}
		Array.Copy(_mapColors, array, _mapColors.Length);
		base.SetDS(num, array);
	}

	// Token: 0x0600580D RID: 22541 RVA: 0x0021C443 File Offset: 0x0021A643
	public override void Clear()
	{
		base.Clear();
		this.chunksSent.Clear();
		this.bNetworkDataAvail = false;
	}

	// Token: 0x0600580E RID: 22542 RVA: 0x0021C45D File Offset: 0x0021A65D
	public ushort[] GetMapColors(long _chunkKey)
	{
		return base.GetDS(IMapChunkDatabase.ToChunkDBKey(_chunkKey));
	}

	// Token: 0x0600580F RID: 22543 RVA: 0x0021C46C File Offset: 0x0021A66C
	public void Add(List<int> _chunks, List<ushort[]> _mapPieces)
	{
		for (int i = 0; i < _chunks.Count; i++)
		{
			base.SetDS(_chunks[i], _mapPieces[i]);
		}
		this.bNetworkDataAvail = true;
	}

	// Token: 0x06005810 RID: 22544 RVA: 0x0021C4A5 File Offset: 0x0021A6A5
	public bool Contains(long _chunkKey)
	{
		return base.ContainsDS(IMapChunkDatabase.ToChunkDBKey(_chunkKey));
	}

	// Token: 0x06005811 RID: 22545 RVA: 0x0021C4B3 File Offset: 0x0021A6B3
	public bool IsNetworkDataAvail()
	{
		return this.bNetworkDataAvail;
	}

	// Token: 0x06005812 RID: 22546 RVA: 0x0021C4BB File Offset: 0x0021A6BB
	public void ResetNetworkDataAvail()
	{
		this.bNetworkDataAvail = false;
	}

	// Token: 0x06005813 RID: 22547 RVA: 0x0021C4C4 File Offset: 0x0021A6C4
	public NetPackage GetMapChunkPackagesToSend()
	{
		if (!this.bClientMapMiddlePositionUpdated)
		{
			return null;
		}
		this.bClientMapMiddlePositionUpdated = false;
		this.toSendList.Clear();
		this.mapPiecesList.Clear();
		int num = World.toChunkXZ(this.clientMapMiddlePosition.x);
		int num2 = World.toChunkXZ(this.clientMapMiddlePosition.y);
		int num3 = 8;
		for (int i = -num3; i <= num3; i++)
		{
			for (int j = -num3; j <= num3; j++)
			{
				int num4 = IMapChunkDatabase.ToChunkDBKey(WorldChunkCache.MakeChunkKey(num + i, num2 + j));
				if (!this.chunksSent.Contains(num4) && base.ContainsDS(num4))
				{
					this.toSendList.Add(num4);
					this.chunksSent.Add(num4);
					this.mapPiecesList.Add(base.GetDS(num4));
				}
			}
		}
		if (this.toSendList.Count == 0)
		{
			return null;
		}
		return NetPackageManager.GetPackage<NetPackageMapChunks>().Setup(this.playerId, this.toSendList, this.mapPiecesList);
	}

	// Token: 0x06005814 RID: 22548 RVA: 0x0021C5BF File Offset: 0x0021A7BF
	public void LoadAsync(ThreadManager.TaskInfo _taskInfo)
	{
		base.Load(((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).dir, ((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).file + ".map");
	}

	// Token: 0x06005815 RID: 22549 RVA: 0x0021C5F1 File Offset: 0x0021A7F1
	public void SaveAsync(ThreadManager.TaskInfo _taskInfo)
	{
		base.Save(((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).dir, ((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).file + ".map");
	}

	// Token: 0x06005816 RID: 22550 RVA: 0x0021C623 File Offset: 0x0021A823
	public void SetClientMapMiddlePosition(Vector2i _pos)
	{
		if (!this.clientMapMiddlePosition.Equals(_pos))
		{
			this.clientMapMiddlePosition = _pos;
			this.bClientMapMiddlePositionUpdated = true;
		}
	}

	// Token: 0x06005817 RID: 22551 RVA: 0x0020021F File Offset: 0x001FE41F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int readKey(BinaryReader _br)
	{
		return _br.ReadInt32();
	}

	// Token: 0x06005818 RID: 22552 RVA: 0x00200216 File Offset: 0x001FE416
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void writeKey(BinaryWriter _bw, int _key)
	{
		_bw.Write(_key);
	}

	// Token: 0x06005819 RID: 22553 RVA: 0x0021C644 File Offset: 0x0021A844
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyFromRead(byte[] _dataRead, ushort[] _data)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_data[i] = (ushort)((int)_dataRead[i * 2] | ((int)_dataRead[i * 2 + 1] << 8));
		}
	}

	// Token: 0x0600581A RID: 22554 RVA: 0x0021C674 File Offset: 0x0021A874
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyToWrite(ushort[] _data, byte[] _dataWrite)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_dataWrite[i * 2] = (byte)(_data[i] & 255);
			_dataWrite[i * 2 + 1] = (byte)((_data[i] >> 8) & 255);
		}
	}

	// Token: 0x0600581B RID: 22555 RVA: 0x0021C6B1 File Offset: 0x0021A8B1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ushort[] allocateDataStorage()
	{
		return new ushort[256];
	}

	// Token: 0x04004424 RID: 17444
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxMapChunks = 131072;

	// Token: 0x04004425 RID: 17445
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EXT = "map";

	// Token: 0x04004426 RID: 17446
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> chunksSent = new HashSet<int>();

	// Token: 0x04004427 RID: 17447
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x04004428 RID: 17448
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i clientMapMiddlePosition = new Vector2i(int.MaxValue, int.MaxValue);

	// Token: 0x04004429 RID: 17449
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bClientMapMiddlePositionUpdated;

	// Token: 0x0400442A RID: 17450
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bNetworkDataAvail;

	// Token: 0x0400442B RID: 17451
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> toSendList = new List<int>();

	// Token: 0x0400442C RID: 17452
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ushort[]> mapPiecesList = new List<ushort[]>();
}
