using System;
using System.Collections.Generic;
using System.ComponentModel;
using Platform;

// Token: 0x02000B5E RID: 2910
public interface IMapChunkDatabase
{
	// Token: 0x060057D9 RID: 22489
	void Clear();

	// Token: 0x060057DA RID: 22490
	ushort[] GetMapColors(long _chunkKey);

	// Token: 0x060057DB RID: 22491 RVA: 0x0021B840 File Offset: 0x00219A40
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

	// Token: 0x060057DC RID: 22492
	void Add(int _chunkX, int _chunkZ, ushort[] _mapColors);

	// Token: 0x060057DD RID: 22493
	void Add(List<int> _chunks, List<ushort[]> _mapPieces);

	// Token: 0x060057DE RID: 22494
	bool Contains(long _chunkKey);

	// Token: 0x060057DF RID: 22495
	bool IsNetworkDataAvail();

	// Token: 0x060057E0 RID: 22496
	void ResetNetworkDataAvail();

	// Token: 0x060057E1 RID: 22497
	NetPackage GetMapChunkPackagesToSend();

	// Token: 0x060057E2 RID: 22498
	void LoadAsync(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x060057E3 RID: 22499
	void SaveAsync(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x060057E4 RID: 22500
	void SetClientMapMiddlePosition(Vector2i _pos);

	// Token: 0x060057E5 RID: 22501 RVA: 0x0021B8BC File Offset: 0x00219ABC
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

	// Token: 0x060057E6 RID: 22502 RVA: 0x0021B9A4 File Offset: 0x00219BA4
	public static int ToChunkDBKey(long _worldChunkKey)
	{
		return IMapChunkDatabase.ToChunkDBKey(WorldChunkCache.extractX(_worldChunkKey), WorldChunkCache.extractZ(_worldChunkKey));
	}

	// Token: 0x060057E7 RID: 22503 RVA: 0x0021B9B7 File Offset: 0x00219BB7
	public static int ToChunkDBKey(int _chunkX, int _chunkZ)
	{
		return ((_chunkZ & 65535) << 16) | (_chunkX & 65535);
	}

	// Token: 0x060057E8 RID: 22504 RVA: 0x0021B9CB File Offset: 0x00219BCB
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

	// Token: 0x02000B5F RID: 2911
	public class DirectoryPlayerId
	{
		// Token: 0x060057E9 RID: 22505 RVA: 0x0021BA08 File Offset: 0x00219C08
		public DirectoryPlayerId(string _dir, string _file)
		{
			this.file = _file;
			this.dir = _dir;
		}

		// Token: 0x04004406 RID: 17414
		public string file;

		// Token: 0x04004407 RID: 17415
		public string dir;
	}
}
