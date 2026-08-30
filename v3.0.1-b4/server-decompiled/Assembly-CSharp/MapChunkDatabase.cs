using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000B42 RID: 2882
public class MapChunkDatabase : DatabaseWithFixedDS<int, ushort[]>, IMapChunkDatabase
{
	// Token: 0x06005769 RID: 22377 RVA: 0x002163F4 File Offset: 0x002145F4
	public MapChunkDatabase(int _playerId)
		: base(7364973, 4, GamePrefs.GetInt(EnumGamePrefs.MaxUncoveredMapChunksPerPlayer), 512, 0, 131072)
	{
		this.playerId = _playerId;
	}

	// Token: 0x0600576A RID: 22378 RVA: 0x00216460 File Offset: 0x00214660
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

	// Token: 0x0600576B RID: 22379 RVA: 0x0021649B File Offset: 0x0021469B
	public override void Clear()
	{
		base.Clear();
		this.chunksSent.Clear();
		this.bNetworkDataAvail = false;
	}

	// Token: 0x0600576C RID: 22380 RVA: 0x002164B5 File Offset: 0x002146B5
	public ushort[] GetMapColors(long _chunkKey)
	{
		return base.GetDS(IMapChunkDatabase.ToChunkDBKey(_chunkKey));
	}

	// Token: 0x0600576D RID: 22381 RVA: 0x002164C4 File Offset: 0x002146C4
	public void Add(List<int> _chunks, List<ushort[]> _mapPieces)
	{
		for (int i = 0; i < _chunks.Count; i++)
		{
			base.SetDS(_chunks[i], _mapPieces[i]);
		}
		this.bNetworkDataAvail = true;
	}

	// Token: 0x0600576E RID: 22382 RVA: 0x002164FD File Offset: 0x002146FD
	public bool Contains(long _chunkKey)
	{
		return base.ContainsDS(IMapChunkDatabase.ToChunkDBKey(_chunkKey));
	}

	// Token: 0x0600576F RID: 22383 RVA: 0x0021650B File Offset: 0x0021470B
	public bool IsNetworkDataAvail()
	{
		return this.bNetworkDataAvail;
	}

	// Token: 0x06005770 RID: 22384 RVA: 0x00216513 File Offset: 0x00214713
	public void ResetNetworkDataAvail()
	{
		this.bNetworkDataAvail = false;
	}

	// Token: 0x06005771 RID: 22385 RVA: 0x0021651C File Offset: 0x0021471C
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

	// Token: 0x06005772 RID: 22386 RVA: 0x00216617 File Offset: 0x00214817
	public void LoadAsync(ThreadManager.TaskInfo _taskInfo)
	{
		base.Load(((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).dir, ((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).file + ".map");
	}

	// Token: 0x06005773 RID: 22387 RVA: 0x00216649 File Offset: 0x00214849
	public void SaveAsync(ThreadManager.TaskInfo _taskInfo)
	{
		base.Save(((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).dir, ((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).file + ".map");
	}

	// Token: 0x06005774 RID: 22388 RVA: 0x0021667B File Offset: 0x0021487B
	public void SetClientMapMiddlePosition(Vector2i _pos)
	{
		if (!this.clientMapMiddlePosition.Equals(_pos))
		{
			this.clientMapMiddlePosition = _pos;
			this.bClientMapMiddlePositionUpdated = true;
		}
	}

	// Token: 0x06005775 RID: 22389 RVA: 0x001FA34F File Offset: 0x001F854F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int readKey(BinaryReader _br)
	{
		return _br.ReadInt32();
	}

	// Token: 0x06005776 RID: 22390 RVA: 0x001FA346 File Offset: 0x001F8546
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void writeKey(BinaryWriter _bw, int _key)
	{
		_bw.Write(_key);
	}

	// Token: 0x06005777 RID: 22391 RVA: 0x0021669C File Offset: 0x0021489C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyFromRead(byte[] _dataRead, ushort[] _data)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_data[i] = (ushort)((int)_dataRead[i * 2] | ((int)_dataRead[i * 2 + 1] << 8));
		}
	}

	// Token: 0x06005778 RID: 22392 RVA: 0x002166CC File Offset: 0x002148CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyToWrite(ushort[] _data, byte[] _dataWrite)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_dataWrite[i * 2] = (byte)(_data[i] & 255);
			_dataWrite[i * 2 + 1] = (byte)((_data[i] >> 8) & 255);
		}
	}

	// Token: 0x06005779 RID: 22393 RVA: 0x00216709 File Offset: 0x00214909
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ushort[] allocateDataStorage()
	{
		return new ushort[256];
	}

	// Token: 0x0400434B RID: 17227
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxMapChunks = 131072;

	// Token: 0x0400434C RID: 17228
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EXT = "map";

	// Token: 0x0400434D RID: 17229
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> chunksSent = new HashSet<int>();

	// Token: 0x0400434E RID: 17230
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x0400434F RID: 17231
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i clientMapMiddlePosition = new Vector2i(int.MaxValue, int.MaxValue);

	// Token: 0x04004350 RID: 17232
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bClientMapMiddlePositionUpdated;

	// Token: 0x04004351 RID: 17233
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bNetworkDataAvail;

	// Token: 0x04004352 RID: 17234
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> toSendList = new List<int>();

	// Token: 0x04004353 RID: 17235
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ushort[]> mapPiecesList = new List<ushort[]>();
}
