using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000B54 RID: 2900
public class MapChunkDatabase : DatabaseWithFixedDS<int, ushort[]>, IMapChunkDatabase
{
	// Token: 0x060057F1 RID: 22513 RVA: 0x00219110 File Offset: 0x00217310
	public MapChunkDatabase(int _playerId)
		: base(7364973, 4, GamePrefs.GetInt(EnumGamePrefs.MaxUncoveredMapChunksPerPlayer), 512, 0, 131072)
	{
		this.playerId = _playerId;
	}

	// Token: 0x060057F2 RID: 22514 RVA: 0x0021917C File Offset: 0x0021737C
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

	// Token: 0x060057F3 RID: 22515 RVA: 0x002191B7 File Offset: 0x002173B7
	public override void Clear()
	{
		base.Clear();
		this.chunksSent.Clear();
		this.bNetworkDataAvail = false;
	}

	// Token: 0x060057F4 RID: 22516 RVA: 0x002191D1 File Offset: 0x002173D1
	public ushort[] GetMapColors(long _chunkKey)
	{
		return base.GetDS(IMapChunkDatabase.ToChunkDBKey(_chunkKey));
	}

	// Token: 0x060057F5 RID: 22517 RVA: 0x002191E0 File Offset: 0x002173E0
	public void Add(List<int> _chunks, List<ushort[]> _mapPieces)
	{
		for (int i = 0; i < _chunks.Count; i++)
		{
			base.SetDS(_chunks[i], _mapPieces[i]);
		}
		this.bNetworkDataAvail = true;
	}

	// Token: 0x060057F6 RID: 22518 RVA: 0x00219219 File Offset: 0x00217419
	public bool Contains(long _chunkKey)
	{
		return base.ContainsDS(IMapChunkDatabase.ToChunkDBKey(_chunkKey));
	}

	// Token: 0x060057F7 RID: 22519 RVA: 0x00219227 File Offset: 0x00217427
	public bool IsNetworkDataAvail()
	{
		return this.bNetworkDataAvail;
	}

	// Token: 0x060057F8 RID: 22520 RVA: 0x0021922F File Offset: 0x0021742F
	public void ResetNetworkDataAvail()
	{
		this.bNetworkDataAvail = false;
	}

	// Token: 0x060057F9 RID: 22521 RVA: 0x00219238 File Offset: 0x00217438
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

	// Token: 0x060057FA RID: 22522 RVA: 0x00219333 File Offset: 0x00217533
	public void LoadAsync(ThreadManager.TaskInfo _taskInfo)
	{
		base.Load(((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).dir, ((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).file + ".map");
	}

	// Token: 0x060057FB RID: 22523 RVA: 0x00219365 File Offset: 0x00217565
	public void SaveAsync(ThreadManager.TaskInfo _taskInfo)
	{
		base.Save(((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).dir, ((IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter).file + ".map");
	}

	// Token: 0x060057FC RID: 22524 RVA: 0x00219397 File Offset: 0x00217597
	public void SetClientMapMiddlePosition(Vector2i _pos)
	{
		if (!this.clientMapMiddlePosition.Equals(_pos))
		{
			this.clientMapMiddlePosition = _pos;
			this.bClientMapMiddlePositionUpdated = true;
		}
	}

	// Token: 0x060057FD RID: 22525 RVA: 0x001FCFBF File Offset: 0x001FB1BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override int readKey(BinaryReader _br)
	{
		return _br.ReadInt32();
	}

	// Token: 0x060057FE RID: 22526 RVA: 0x001FCFB6 File Offset: 0x001FB1B6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void writeKey(BinaryWriter _bw, int _key)
	{
		_bw.Write(_key);
	}

	// Token: 0x060057FF RID: 22527 RVA: 0x002193B8 File Offset: 0x002175B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyFromRead(byte[] _dataRead, ushort[] _data)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_data[i] = (ushort)((int)_dataRead[i * 2] | ((int)_dataRead[i * 2 + 1] << 8));
		}
	}

	// Token: 0x06005800 RID: 22528 RVA: 0x002193E8 File Offset: 0x002175E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyToWrite(ushort[] _data, byte[] _dataWrite)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_dataWrite[i * 2] = (byte)(_data[i] & 255);
			_dataWrite[i * 2 + 1] = (byte)((_data[i] >> 8) & 255);
		}
	}

	// Token: 0x06005801 RID: 22529 RVA: 0x00219425 File Offset: 0x00217625
	[PublicizedFrom(EAccessModifier.Protected)]
	public override ushort[] allocateDataStorage()
	{
		return new ushort[256];
	}

	// Token: 0x040043E6 RID: 17382
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxMapChunks = 131072;

	// Token: 0x040043E7 RID: 17383
	[PublicizedFrom(EAccessModifier.Private)]
	public const string EXT = "map";

	// Token: 0x040043E8 RID: 17384
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> chunksSent = new HashSet<int>();

	// Token: 0x040043E9 RID: 17385
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x040043EA RID: 17386
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i clientMapMiddlePosition = new Vector2i(int.MaxValue, int.MaxValue);

	// Token: 0x040043EB RID: 17387
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bClientMapMiddlePositionUpdated;

	// Token: 0x040043EC RID: 17388
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bNetworkDataAvail;

	// Token: 0x040043ED RID: 17389
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> toSendList = new List<int>();

	// Token: 0x040043EE RID: 17390
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ushort[]> mapPiecesList = new List<ushort[]>();
}
