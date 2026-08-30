using System;
using System.Collections.Generic;
using System.ComponentModel;
using Platform;

// Token: 0x02000B4C RID: 2892
public interface IMapChunkDatabase
{
	// Token: 0x060057BF RID: 22463
	void Clear();

	// Token: 0x060057C0 RID: 22464
	ushort[] GetMapColors(long _chunkKey);

	// Token: 0x060057C1 RID: 22465 RVA: 0x002185B4 File Offset: 0x002167B4
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

	// Token: 0x060057C2 RID: 22466
	void Add(int _chunkX, int _chunkZ, ushort[] _mapColors);

	// Token: 0x060057C3 RID: 22467
	void Add(List<int> _chunks, List<ushort[]> _mapPieces);

	// Token: 0x060057C4 RID: 22468
	bool Contains(long _chunkKey);

	// Token: 0x060057C5 RID: 22469
	bool IsNetworkDataAvail();

	// Token: 0x060057C6 RID: 22470
	void ResetNetworkDataAvail();

	// Token: 0x060057C7 RID: 22471
	NetPackage GetMapChunkPackagesToSend();

	// Token: 0x060057C8 RID: 22472
	void LoadAsync(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x060057C9 RID: 22473
	void SaveAsync(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x060057CA RID: 22474
	void SetClientMapMiddlePosition(Vector2i _pos);

	// Token: 0x060057CB RID: 22475 RVA: 0x00218630 File Offset: 0x00216830
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

	// Token: 0x060057CC RID: 22476 RVA: 0x00218718 File Offset: 0x00216918
	public static int ToChunkDBKey(long _worldChunkKey)
	{
		return IMapChunkDatabase.ToChunkDBKey(WorldChunkCache.extractX(_worldChunkKey), WorldChunkCache.extractZ(_worldChunkKey));
	}

	// Token: 0x060057CD RID: 22477 RVA: 0x0021872B File Offset: 0x0021692B
	public static int ToChunkDBKey(int _chunkX, int _chunkZ)
	{
		return ((_chunkZ & 65535) << 16) | (_chunkX & 65535);
	}

	// Token: 0x060057CE RID: 22478 RVA: 0x0021873F File Offset: 0x0021693F
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

	// Token: 0x02000B4D RID: 2893
	public class DirectoryPlayerId
	{
		// Token: 0x060057CF RID: 22479 RVA: 0x0021877C File Offset: 0x0021697C
		public DirectoryPlayerId(string _dir, string _file)
		{
			this.file = _file;
			this.dir = _dir;
		}

		// Token: 0x040043C8 RID: 17352
		public string file;

		// Token: 0x040043C9 RID: 17353
		public string dir;
	}
}
