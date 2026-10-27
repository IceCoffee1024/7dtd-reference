using System;
using System.Collections.Generic;
using System.ComponentModel;
using Platform;

// Token: 0x02000B69 RID: 2921
public interface IMapChunkDatabase
{
	// Token: 0x060057FE RID: 22526
	void Clear();

	// Token: 0x060057FF RID: 22527
	ushort[] GetMapColors(long _chunkKey);

	// Token: 0x06005800 RID: 22528 RVA: 0x0021CD9C File Offset: 0x0021AF9C
	void Add(Vector3i _chunkPos, World _world)
	{
		int num = (_world.IsEditor() ? 7 : 4);
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				Chunk chunk = (Chunk)_world.GetChunkSync(_chunkPos.x + i, _chunkPos.z + j);
				if (chunk != null && !chunk.NeedsDecoration)
				{
					this.Add(_chunkPos.x + i, _chunkPos.z + j, chunk.GetMapColors());
				}
			}
		}
	}

	// Token: 0x06005801 RID: 22529
	void Add(int _chunkX, int _chunkZ, ushort[] _mapColors);

	// Token: 0x06005802 RID: 22530
	void Add(List<int> _chunks, List<ushort[]> _mapPieces);

	// Token: 0x06005803 RID: 22531
	bool Contains(long _chunkKey);

	// Token: 0x06005804 RID: 22532
	bool IsNetworkDataAvail();

	// Token: 0x06005805 RID: 22533
	void ResetNetworkDataAvail();

	// Token: 0x06005806 RID: 22534
	NetPackage GetMapChunkPackagesToSend();

	// Token: 0x06005807 RID: 22535
	void LoadAsync(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x06005808 RID: 22536
	void SaveAsync(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x06005809 RID: 22537
	void SetClientMapMiddlePosition(Vector2i _pos);

	// Token: 0x0600580A RID: 22538 RVA: 0x0021CE18 File Offset: 0x0021B018
	public static bool TryCreateOrLoad(int _entityId, out IMapChunkDatabase _mapDatabase, Func<IMapChunkDatabase.DirectoryPlayerId> _parameterSupplier)
	{
		if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
		{
			bool flag;
			if (_entityId != -1 && GameManager.Instance)
			{
				World world = GameManager.Instance.World;
				if (world == null)
				{
					flag = false;
				}
				else
				{
					EntityPlayerLocal primaryPlayer = world.GetPrimaryPlayer();
					int? num = ((primaryPlayer != null) ? new int?(primaryPlayer.entityId) : null);
					flag = (num.GetValueOrDefault() == _entityId) & (num != null);
				}
			}
			else
			{
				flag = false;
			}
			if (!flag)
			{
				_mapDatabase = null;
				return false;
			}
		}
		MapChunkDatabaseType value = LaunchPrefs.MapChunkDatabase.Value;
		IMapChunkDatabase mapChunkDatabase;
		if (value != MapChunkDatabaseType.Fixed)
		{
			if (value != MapChunkDatabaseType.Region)
			{
				throw new InvalidEnumArgumentException(string.Format("Unknown {0}: {1}", "MapChunkDatabaseType", LaunchPrefs.MapChunkDatabase.Value));
			}
			mapChunkDatabase = new MapChunkDatabaseByRegion(_entityId);
		}
		else
		{
			mapChunkDatabase = new MapChunkDatabase(_entityId);
		}
		IMapChunkDatabase mapChunkDatabase2 = mapChunkDatabase;
		IMapChunkDatabase.DirectoryPlayerId directoryPlayerId = _parameterSupplier();
		ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(mapChunkDatabase2.LoadAsync), directoryPlayerId, null, true);
		_mapDatabase = mapChunkDatabase2;
		return true;
	}

	// Token: 0x0600580B RID: 22539 RVA: 0x0021CF00 File Offset: 0x0021B100
	public static int ToChunkDBKey(long _worldChunkKey)
	{
		return IMapChunkDatabase.ToChunkDBKey(WorldChunkCache.extractX(_worldChunkKey), WorldChunkCache.extractZ(_worldChunkKey));
	}

	// Token: 0x0600580C RID: 22540 RVA: 0x0021CF13 File Offset: 0x0021B113
	public static int ToChunkDBKey(int _chunkX, int _chunkZ)
	{
		return ((_chunkZ & 65535) << 16) | (_chunkX & 65535);
	}

	// Token: 0x0600580D RID: 22541 RVA: 0x0021CF27 File Offset: 0x0021B127
	public static void FromChunkDBKey(int _chunkDBKey, out int _chunkX, out int _chunkZ)
	{
		_chunkX = _chunkDBKey & 65535;
		if (_chunkX > 32767)
		{
			_chunkX |= -65536;
		}
		_chunkZ = (_chunkDBKey >> 16) & 65535;
		if (_chunkZ > 32767)
		{
			_chunkZ |= -65536;
		}
	}

	// Token: 0x02000B6A RID: 2922
	public class DirectoryPlayerId
	{
		// Token: 0x0600580E RID: 22542 RVA: 0x0021CF64 File Offset: 0x0021B164
		public DirectoryPlayerId(string _dir, string _file)
		{
			this.file = _file;
			this.dir = _dir;
		}

		// Token: 0x0400442B RID: 17451
		public string file;

		// Token: 0x0400442C RID: 17452
		public string dir;
	}
}
