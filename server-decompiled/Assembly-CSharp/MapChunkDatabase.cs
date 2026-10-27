using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000B71 RID: 2929
public class MapChunkDatabase : DatabaseWithFixedDS<int, ushort[]>, IMapChunkDatabase
{
	// Token: 0x06005830 RID: 22576 RVA: 0x0021D8F8 File Offset: 0x0021BAF8
	public MapChunkDatabase(int _playerId)
		: base(7364973, 4, GamePrefs.GetInt(EnumGamePrefs.MaxUncoveredMapChunksPerPlayer), 512, 0, 131072)
	{
		this.playerId = _playerId;
	}

	// Token: 0x06005831 RID: 22577 RVA: 0x0021D964 File Offset: 0x0021BB64
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

	// Token: 0x06005832 RID: 22578 RVA: 0x0021D99F File Offset: 0x0021BB9F
	public override void Clear()
	{
		base.Clear();
		this.chunksSent.Clear();
		this.bNetworkDataAvail = false;
	}

	// Token: 0x06005833 RID: 22579 RVA: 0x0021D9B9 File Offset: 0x0021BBB9
	public ushort[] GetMapColors(long _chunkKey)
	{
		return base.GetDS(IMapChunkDatabase.ToChunkDBKey(_chunkKey));
	}

	// Token: 0x06005834 RID: 22580 RVA: 0x0021D9C8 File Offset: 0x0021BBC8
	public void Add(List<int> _chunks, List<ushort[]> _mapPieces)
	{
		for (int i = 0; i < _chunks.Count; i++)
		{
			base.SetDS(_chunks[i], _mapPieces[i]);
		}
		this.bNetworkDataAvail = true;
	}

	// Token: 0x06005835 RID: 22581 RVA: 0x0021DA01 File Offset: 0x0021BC01
	public bool Contains(long _chunkKey)
	{
		return base.ContainsDS(IMapChunkDatabase.ToChunkDBKey(_chunkKey));
	}

	// Token: 0x06005836 RID: 22582 RVA: 0x0021DA0F File Offset: 0x0021BC0F
	public bool IsNetworkDataAvail()
	{
		return this.bNetworkDataAvail;
	}

	// Token: 0x06005837 RID: 22583 RVA: 0x0021DA17 File Offset: 0x0021BC17
	public void ResetNetworkDataAvail()
	{
		this.bNetworkDataAvail = false;
	}

	// Token: 0x06005838 RID: 22584 RVA: 0x0021DA20 File Offset: 0x0021BC20
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

	// Token: 0x06005839 RID: 22585 RVA: 0x0021DB1B File Offset: 0x0021BD1B
	public void LoadAsync(ThreadManager.TaskInfo _taskInfo)
	{
		base.Load(((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).dir, ((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).file + ".map");
	}

	// Token: 0x0600583A RID: 22586 RVA: 0x0021DB4D File Offset: 0x0021BD4D
	public void SaveAsync(ThreadManager.TaskInfo _taskInfo)
	{
		base.Save(((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).dir, ((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).file + ".map");
	}

	// Token: 0x0600583B RID: 22587 RVA: 0x0021DB7F File Offset: 0x0021BD7F
	public void SetClientMapMiddlePosition(Vector2i _pos)
	{
		if (!this.clientMapMiddlePosition.Equals(_pos))
		{
			this.clientMapMiddlePosition = _pos;
			this.bClientMapMiddlePositionUpdated = true;
		}
	}

	// Token: 0x0600583C RID: 22588 RVA: 0x002017A3 File Offset: 0x001FF9A3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int readKey(BinaryReader _br)
	{
		return _br.ReadInt32();
	}

	// Token: 0x0600583D RID: 22589 RVA: 0x0020179A File Offset: 0x001FF99A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void writeKey(BinaryWriter _bw, int _key)
	{
		_bw.Write(_key);
	}

	// Token: 0x0600583E RID: 22590 RVA: 0x0021DBA0 File Offset: 0x0021BDA0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyFromRead(byte[] _dataRead, ushort[] _data)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_data[i] = (ushort)((int)_dataRead[i * 2] | ((int)_dataRead[i * 2 + 1] << 8));
		}
	}

	// Token: 0x0600583F RID: 22591 RVA: 0x0021DBD0 File Offset: 0x0021BDD0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyToWrite(ushort[] _data, byte[] _dataWrite)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_dataWrite[i * 2] = (byte)(_data[i] & 255);
			_dataWrite[i * 2 + 1] = (byte)((_data[i] >> 8) & 255);
		}
	}

	// Token: 0x06005840 RID: 22592 RVA: 0x0021DC0D File Offset: 0x0021BE0D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ushort[] allocateDataStorage()
	{
		return new ushort[256];
	}

	// Token: 0x04004449 RID: 17481
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxMapChunks = 131072;

	// Token: 0x0400444A RID: 17482
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EXT = "map";

	// Token: 0x0400444B RID: 17483
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> chunksSent = new HashSet<int>();

	// Token: 0x0400444C RID: 17484
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x0400444D RID: 17485
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i clientMapMiddlePosition = new Vector2i(int.MaxValue, int.MaxValue);

	// Token: 0x0400444E RID: 17486
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bClientMapMiddlePositionUpdated;

	// Token: 0x0400444F RID: 17487
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bNetworkDataAvail;

	// Token: 0x04004450 RID: 17488
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> toSendList = new List<int>();

	// Token: 0x04004451 RID: 17489
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ushort[]> mapPiecesList = new List<ushort[]>();
}
