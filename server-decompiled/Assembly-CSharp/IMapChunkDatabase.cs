using System;
using System.Collections.Generic;
using System.ComponentModel;
using Platform;

// Token: 0x02000B3A RID: 2874
public interface IMapChunkDatabase
{
	// Token: 0x06005737 RID: 22327
	void Clear();

	// Token: 0x06005738 RID: 22328
	ushort[] GetMapColors(long _chunkKey);

	// Token: 0x06005739 RID: 22329 RVA: 0x00215898 File Offset: 0x00213A98
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

	// Token: 0x0600573A RID: 22330
	void Add(int _chunkX, int _chunkZ, ushort[] _mapColors);

	// Token: 0x0600573B RID: 22331
	void Add(List<int> _chunks, List<ushort[]> _mapPieces);

	// Token: 0x0600573C RID: 22332
	bool Contains(long _chunkKey);

	// Token: 0x0600573D RID: 22333
	bool IsNetworkDataAvail();

	// Token: 0x0600573E RID: 22334
	void ResetNetworkDataAvail();

	// Token: 0x0600573F RID: 22335
	NetPackage GetMapChunkPackagesToSend();

	// Token: 0x06005740 RID: 22336
	void LoadAsync(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x06005741 RID: 22337
	void SaveAsync(ThreadManager.TaskInfo _taskInfo);

	// Token: 0x06005742 RID: 22338
	void SetClientMapMiddlePosition(Vector2i _pos);

	// Token: 0x06005743 RID: 22339 RVA: 0x00215914 File Offset: 0x00213B14
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

	// Token: 0x06005744 RID: 22340 RVA: 0x002159FC File Offset: 0x00213BFC
	public static int ToChunkDBKey(long _worldChunkKey)
	{
		return IMapChunkDatabase.ToChunkDBKey(WorldChunkCache.extractX(_worldChunkKey), WorldChunkCache.extractZ(_worldChunkKey));
	}

	// Token: 0x06005745 RID: 22341 RVA: 0x00215A0F File Offset: 0x00213C0F
	public static int ToChunkDBKey(int _chunkX, int _chunkZ)
	{
		return ((_chunkZ & 65535) << 16) | (_chunkX & 65535);
	}

	// Token: 0x06005746 RID: 22342 RVA: 0x00215A23 File Offset: 0x00213C23
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

	// Token: 0x02000B3B RID: 2875
	public class DirectoryPlayerId
	{
		// Token: 0x06005747 RID: 22343 RVA: 0x00215A60 File Offset: 0x00213C60
		public DirectoryPlayerId(string _dir, string _file)
		{
			this.file = _file;
			this.dir = _dir;
		}

		// Token: 0x0400432D RID: 17197
		public string file;

		// Token: 0x0400432E RID: 17198
		public string dir;
	}
}
