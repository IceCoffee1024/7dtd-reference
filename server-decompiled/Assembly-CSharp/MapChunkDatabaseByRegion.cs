using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

// Token: 0x02000B72 RID: 2930
public class MapChunkDatabaseByRegion : IMapChunkDatabase
{
	// Token: 0x06005841 RID: 22593 RVA: 0x0021DC1C File Offset: 0x0021BE1C
	public MapChunkDatabaseByRegion(int playerId)
	{
		this.m_regions = new Dictionary<Vector2i, MapChunkDatabaseByRegion.RegionData>();
		this.m_chunksSent = new HashSet<int>();
		this.m_chunkIdsToSend = new List<int>();
		this.m_chunkDataToSend = new List<ushort[]>();
		this.m_playerId = playerId;
	}

	// Token: 0x06005842 RID: 22594 RVA: 0x0021DC70 File Offset: 0x0021BE70
	public void Clear()
	{
		object regionsLock = this.m_regionsLock;
		lock (regionsLock)
		{
			this.m_regions.Clear();
		}
	}

	// Token: 0x06005843 RID: 22595 RVA: 0x0021DCB8 File Offset: 0x0021BEB8
	public ushort[] GetMapColors(long _chunkKey)
	{
		Vector2i vector2i;
		Vector2i vector2i2;
		this.ToRegionAndOffset(_chunkKey, out vector2i, out vector2i2);
		object regionsLock = this.m_regionsLock;
		MapChunkDatabaseByRegion.RegionData regionData;
		lock (regionsLock)
		{
			if (!this.m_regions.TryGetValue(vector2i, out regionData))
			{
				return null;
			}
		}
		return regionData.GetChunkData(vector2i2);
	}

	// Token: 0x06005844 RID: 22596 RVA: 0x0021DD20 File Offset: 0x0021BF20
	public void Add(int _chunkX, int _chunkZ, ushort[] _mapColors)
	{
		this.Add(_chunkX, _chunkZ, _mapColors, false);
	}

	// Token: 0x06005845 RID: 22597 RVA: 0x0021DD2C File Offset: 0x0021BF2C
	public void Add(List<int> _chunks, List<ushort[]> _mapPieces)
	{
		for (int i = 0; i < _chunks.Count; i++)
		{
			int num;
			int num2;
			IMapChunkDatabase.FromChunkDBKey(_chunks[i], out num, out num2);
			this.Add(num, num2, _mapPieces[i], true);
		}
		this.m_networkDataAvailable = true;
	}

	// Token: 0x06005846 RID: 22598 RVA: 0x0021DD74 File Offset: 0x0021BF74
	public bool Contains(long _chunkKey)
	{
		Vector2i vector2i;
		Vector2i vector2i2;
		this.ToRegionAndOffset(_chunkKey, out vector2i, out vector2i2);
		object regionsLock = this.m_regionsLock;
		MapChunkDatabaseByRegion.RegionData regionData;
		lock (regionsLock)
		{
			if (!this.m_regions.TryGetValue(vector2i, out regionData))
			{
				return false;
			}
		}
		return regionData.GetChunkData(vector2i2) != null;
	}

	// Token: 0x06005847 RID: 22599 RVA: 0x0021DDE0 File Offset: 0x0021BFE0
	public bool IsNetworkDataAvail()
	{
		return this.m_networkDataAvailable;
	}

	// Token: 0x06005848 RID: 22600 RVA: 0x0021DDE8 File Offset: 0x0021BFE8
	public void ResetNetworkDataAvail()
	{
		this.m_networkDataAvailable = false;
	}

	// Token: 0x06005849 RID: 22601 RVA: 0x0021DDF4 File Offset: 0x0021BFF4
	public NetPackage GetMapChunkPackagesToSend()
	{
		if (!this.m_clientMapMiddlePositionUpdated)
		{
			return null;
		}
		this.m_clientMapMiddlePositionUpdated = false;
		this.m_chunkIdsToSend.Clear();
		this.m_chunkDataToSend.Clear();
		int num = World.toChunkXZ(this.m_clientMapMiddlePosition.x);
		int num2 = World.toChunkXZ(this.m_clientMapMiddlePosition.y);
		int num3 = 8;
		object regionsLock = this.m_regionsLock;
		lock (regionsLock)
		{
			for (int i = -num3; i <= num3; i++)
			{
				for (int j = -num3; j <= num3; j++)
				{
					long num4 = WorldChunkCache.MakeChunkKey(num + i, num2 + j);
					int num5 = IMapChunkDatabase.ToChunkDBKey(num4);
					if (!this.m_chunksSent.Contains(num5))
					{
						Vector2i vector2i;
						Vector2i vector2i2;
						this.ToRegionAndOffset(num4, out vector2i, out vector2i2);
						MapChunkDatabaseByRegion.RegionData regionData;
						if (this.m_regions.TryGetValue(vector2i, out regionData))
						{
							ushort[] chunkData = regionData.GetChunkData(vector2i2);
							if (chunkData != null)
							{
								this.m_chunksSent.Add(num5);
								this.m_chunkIdsToSend.Add(num5);
								this.m_chunkDataToSend.Add(chunkData);
							}
						}
					}
				}
			}
		}
		if (this.m_chunkIdsToSend.Count == 0)
		{
			return null;
		}
		return NetPackageManager.GetPackage<NetPackageMapChunks>().Setup(this.m_playerId, this.m_chunkIdsToSend, this.m_chunkDataToSend);
	}

	// Token: 0x0600584A RID: 22602 RVA: 0x0021DF50 File Offset: 0x0021C150
	public void LoadAsync(ThreadManager.TaskInfo _taskInfo)
	{
		IMapChunkDatabase.DirectoryPlayerId directoryPlayerId = (IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter;
		this.Load(Path.Join(directoryPlayerId.dir, directoryPlayerId.file));
	}

	// Token: 0x0600584B RID: 22603 RVA: 0x0021DF8C File Offset: 0x0021C18C
	public void SaveAsync(ThreadManager.TaskInfo _taskInfo)
	{
		IMapChunkDatabase.DirectoryPlayerId directoryPlayerId = (IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter;
		this.Save(Path.Join(directoryPlayerId.dir, directoryPlayerId.file));
	}

	// Token: 0x0600584C RID: 22604 RVA: 0x0021DFC6 File Offset: 0x0021C1C6
	public void SetClientMapMiddlePosition(Vector2i _pos)
	{
		this.m_clientMapMiddlePosition = _pos;
		this.m_clientMapMiddlePositionUpdated = true;
	}

	// Token: 0x0600584D RID: 22605 RVA: 0x0021DFD8 File Offset: 0x0021C1D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Add(int chunkX, int chunkZ, ushort[] mapColors, bool skipCopy)
	{
		Vector2i vector2i;
		Vector2i vector2i2;
		this.ToRegionAndOffset(chunkX, chunkZ, out vector2i, out vector2i2);
		object regionsLock = this.m_regionsLock;
		MapChunkDatabaseByRegion.RegionData regionData;
		lock (regionsLock)
		{
			if (!this.m_regions.TryGetValue(vector2i, out regionData))
			{
				regionData = new MapChunkDatabaseByRegion.RegionData();
				this.m_regions.Add(vector2i, regionData);
			}
		}
		regionData.SetChunkData(vector2i2, mapColors, skipCopy);
	}

	// Token: 0x0600584E RID: 22606 RVA: 0x0021E050 File Offset: 0x0021C250
	[PublicizedFrom(EAccessModifier.Private)]
	public void Load(string rootDirectory)
	{
		if (!SdDirectory.Exists(rootDirectory))
		{
			return;
		}
		object regionsLock = this.m_regionsLock;
		lock (regionsLock)
		{
			HashSet<Vector2i> hashSet = new HashSet<Vector2i>(this.m_regions.Keys);
			foreach (SdFileInfo sdFileInfo in new SdDirectoryInfo(rootDirectory).EnumerateFiles())
			{
				Vector2i vector2i;
				if (this.TryGetRegionPosFromFileName(sdFileInfo.Name, out vector2i))
				{
					try
					{
						hashSet.Add(vector2i);
						MapChunkDatabaseByRegion.RegionData regionData;
						if (!this.m_regions.TryGetValue(vector2i, out regionData))
						{
							regionData = new MapChunkDatabaseByRegion.RegionData();
							this.m_regions.Add(vector2i, regionData);
						}
						regionData.Load(sdFileInfo.FullName);
						hashSet.Remove(vector2i);
					}
					catch (Exception ex)
					{
						Log.Warning(string.Format("[{0}] Failed to load region ({1}, {2}) from '{3}': {4}", new object[] { "MapChunkDatabaseByRegion", vector2i.x, vector2i.y, sdFileInfo.FullName, ex }));
					}
				}
			}
			foreach (Vector2i vector2i2 in hashSet)
			{
				this.m_regions.Remove(vector2i2);
			}
		}
	}

	// Token: 0x0600584F RID: 22607 RVA: 0x0021E218 File Offset: 0x0021C418
	[PublicizedFrom(EAccessModifier.Private)]
	public void Save(string rootDirectory)
	{
		SdDirectory.CreateDirectory(rootDirectory);
		Dictionary<Vector2i, string> dictionary = new Dictionary<Vector2i, string>();
		foreach (SdFileInfo sdFileInfo in new SdDirectoryInfo(rootDirectory).EnumerateFiles())
		{
			Vector2i vector2i;
			if (this.TryGetRegionPosFromFileName(sdFileInfo.Name, out vector2i))
			{
				dictionary[vector2i] = sdFileInfo.FullName;
			}
		}
		object regionsLock = this.m_regionsLock;
		lock (regionsLock)
		{
			foreach (KeyValuePair<Vector2i, MapChunkDatabaseByRegion.RegionData> keyValuePair in this.m_regions)
			{
				Vector2i vector2i2;
				MapChunkDatabaseByRegion.RegionData regionData;
				keyValuePair.Deconstruct(out vector2i2, out regionData);
				Vector2i vector2i3 = vector2i2;
				MapChunkDatabaseByRegion.RegionData regionData2 = regionData;
				string regionDataPath = MapChunkDatabaseByRegion.GetRegionDataPath(rootDirectory, vector2i3);
				try
				{
					dictionary.Remove(vector2i3);
					regionData2.Save(regionDataPath);
				}
				catch (Exception ex)
				{
					Log.Warning(string.Format("[{0}] Failed to save region ({1}, {2}) to '{3}': {4}", new object[] { "MapChunkDatabaseByRegion", vector2i3.x, vector2i3.y, regionDataPath, ex }));
				}
			}
		}
		foreach (KeyValuePair<Vector2i, string> keyValuePair2 in dictionary)
		{
			Vector2i vector2i2;
			string text;
			keyValuePair2.Deconstruct(out vector2i2, out text);
			Vector2i vector2i4 = vector2i2;
			string text2 = text;
			try
			{
				SdFile.Delete(text2);
			}
			catch (Exception ex2)
			{
				Log.Warning(string.Format("[{0}] Failed to delete region ({1}, {2}) to '{3}': {4}", new object[] { "MapChunkDatabaseByRegion", vector2i4.x, vector2i4.y, text2, ex2 }));
			}
		}
	}

	// Token: 0x06005850 RID: 22608 RVA: 0x0021E430 File Offset: 0x0021C630
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToRegionAndOffset(long worldChunkKey, out Vector2i regionPos, out Vector2i chunkOffset)
	{
		int num = WorldChunkCache.extractX(worldChunkKey);
		int num2 = WorldChunkCache.extractZ(worldChunkKey);
		this.ToRegionAndOffset(num, num2, out regionPos, out chunkOffset);
	}

	// Token: 0x06005851 RID: 22609 RVA: 0x0021E455 File Offset: 0x0021C655
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToRegionAndOffset(Vector3i chunkPos, out Vector2i regionPos, out Vector2i chunkOffset)
	{
		this.ToRegionAndOffset(chunkPos.x, chunkPos.y, out regionPos, out chunkOffset);
	}

	// Token: 0x06005852 RID: 22610 RVA: 0x0021E46C File Offset: 0x0021C66C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToRegionAndOffset(int chunkX, int chunkZ, out Vector2i regionPos, out Vector2i chunkOffset)
	{
		int num = chunkX >> 5;
		int num2 = chunkZ >> 5;
		regionPos = new Vector2i(num, num2);
		int num3 = chunkX - num * 32;
		int num4 = chunkZ - num2 * 32;
		chunkOffset = new Vector2i(num3, num4);
	}

	// Token: 0x06005853 RID: 22611 RVA: 0x0021E4AC File Offset: 0x0021C6AC
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetRegionDataPath(string rootDirectory, Vector2i regionPos)
	{
		return Path.Join(rootDirectory, string.Format("{0}{1}{2}{3}{4}", new object[] { "r.", regionPos.x, ".", regionPos.y, ".7rm" }));
	}

	// Token: 0x06005854 RID: 22612 RVA: 0x0021E510 File Offset: 0x0021C710
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetRegionPosFromFileName(StringSpan name, out Vector2i regionPos)
	{
		if (name.IndexOf("r.") != 0)
		{
			regionPos = default(Vector2i);
			return false;
		}
		name = name.Slice("r.".Length);
		int num = name.Length - ".7rm".Length;
		if (num < 0 || name.LastIndexOf(".7rm") != num)
		{
			regionPos = default(Vector2i);
			return false;
		}
		name = name.Slice(0, num);
		int num2 = 0;
		bool flag = false;
		int num3 = 0;
		bool flag2 = false;
		foreach (StringSpan stringSpan in name.GetSplitEnumerator(".", StringSplitOptions.None))
		{
			if (flag2)
			{
				regionPos = default(Vector2i);
				return false;
			}
			if (flag)
			{
				if (!int.TryParse(stringSpan.AsSpan(), out num3))
				{
					regionPos = default(Vector2i);
					return false;
				}
				flag2 = true;
			}
			else
			{
				if (!int.TryParse(stringSpan.AsSpan(), out num2))
				{
					regionPos = default(Vector2i);
					return false;
				}
				flag = true;
			}
		}
		if (!flag || !flag2)
		{
			regionPos = default(Vector2i);
			return false;
		}
		regionPos = new Vector2i(num2, num3);
		return true;
	}

	// Token: 0x04004452 RID: 17490
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilePrefix = "r.";

	// Token: 0x04004453 RID: 17491
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FileElementSeparator = ".";

	// Token: 0x04004454 RID: 17492
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilePostfix = ".7rm";

	// Token: 0x04004455 RID: 17493
	[PublicizedFrom(EAccessModifier.Private)]
	public const int REGION_CHUNK_WIDTH = 32;

	// Token: 0x04004456 RID: 17494
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHUNK_TO_REGION_SHIFT = 5;

	// Token: 0x04004457 RID: 17495
	[PublicizedFrom(EAccessModifier.Private)]
	public const int REGION_CHUNK_AREA = 1024;

	// Token: 0x04004458 RID: 17496
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHUNK_DATA_LENGTH = 256;

	// Token: 0x04004459 RID: 17497
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ushort[] EMPTY_CHUNK_DATA = new ushort[256];

	// Token: 0x0400445A RID: 17498
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Vector2i, MapChunkDatabaseByRegion.RegionData> m_regions;

	// Token: 0x0400445B RID: 17499
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_regionsLock = new object();

	// Token: 0x0400445C RID: 17500
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<int> m_chunksSent;

	// Token: 0x0400445D RID: 17501
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> m_chunkIdsToSend;

	// Token: 0x0400445E RID: 17502
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ushort[]> m_chunkDataToSend;

	// Token: 0x0400445F RID: 17503
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_playerId;

	// Token: 0x04004460 RID: 17504
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_networkDataAvailable;

	// Token: 0x04004461 RID: 17505
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i m_clientMapMiddlePosition;

	// Token: 0x04004462 RID: 17506
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_clientMapMiddlePositionUpdated;

	// Token: 0x02000B73 RID: 2931
	[PublicizedFrom(EAccessModifier.Private)]
	public class RegionData
	{
		// Token: 0x06005856 RID: 22614 RVA: 0x0021E640 File Offset: 0x0021C840
		public RegionData()
		{
			this.m_regionData = new ushort[32][][];
			for (int i = 0; i < 32; i++)
			{
				this.m_regionData[i] = new ushort[32][];
			}
			this.m_dirty = true;
		}

		// Token: 0x06005857 RID: 22615 RVA: 0x0021E683 File Offset: 0x0021C883
		public ushort[] GetChunkData(Vector2i offset)
		{
			return this.m_regionData[offset.y][offset.x];
		}

		// Token: 0x06005858 RID: 22616 RVA: 0x0021E69C File Offset: 0x0021C89C
		public void SetChunkData(Vector2i offset, ushort[] mapColors, bool skipCopy)
		{
			if (skipCopy)
			{
				this.m_regionData[offset.y][offset.x] = mapColors;
				this.m_dirty = true;
				return;
			}
			ushort[] array = this.m_regionData[offset.y][offset.x];
			if (array == null)
			{
				array = new ushort[256];
				this.m_regionData[offset.y][offset.x] = array;
			}
			Array.Copy(mapColors, array, 256);
			this.m_dirty = true;
		}

		// Token: 0x06005859 RID: 22617 RVA: 0x0021E714 File Offset: 0x0021C914
		public void Load(string regionFilePath)
		{
			using (Stream stream = SdFile.Open(regionFilePath, FileMode.Open, FileAccess.Read))
			{
				using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(true))
					{
						pooledBinaryReader.SetBaseStream(stream);
						pooledBinaryReader.ReadInt32();
						pooledBinaryReader.SetBaseStream(gzipStream);
						ushort[] array = null;
						for (int i = 0; i < 32; i++)
						{
							for (int j = 0; j < 32; j++)
							{
								if (array == null)
								{
									array = new ushort[256];
								}
								for (int k = 0; k < 256; k++)
								{
									array[k] = pooledBinaryReader.ReadUInt16();
								}
								if (MapChunkDatabaseByRegion.EMPTY_CHUNK_DATA.SequenceEqual<ushort>(array))
								{
									this.m_regionData[i][j] = null;
								}
								else
								{
									this.m_regionData[i][j] = array;
									array = null;
								}
							}
						}
						this.m_lastRegionPath = regionFilePath;
						this.m_dirty = false;
					}
				}
			}
		}

		// Token: 0x0600585A RID: 22618 RVA: 0x0021E824 File Offset: 0x0021CA24
		public void Save(string regionFilePath)
		{
			if (!this.m_dirty && regionFilePath == this.m_lastRegionPath)
			{
				return;
			}
			using (Stream stream = SdFile.Open(regionFilePath, FileMode.Create, FileAccess.Write))
			{
				using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Compress))
				{
					using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(true))
					{
						pooledBinaryWriter.SetBaseStream(stream);
						pooledBinaryWriter.Write(1);
						pooledBinaryWriter.SetBaseStream(gzipStream);
						for (int i = 0; i < 32; i++)
						{
							for (int j = 0; j < 32; j++)
							{
								ushort[] array = this.m_regionData[i][j] ?? MapChunkDatabaseByRegion.EMPTY_CHUNK_DATA;
								for (int k = 0; k < 256; k++)
								{
									pooledBinaryWriter.Write(array[k]);
								}
							}
						}
						this.m_lastRegionPath = regionFilePath;
						this.m_dirty = false;
					}
				}
			}
		}

		// Token: 0x04004463 RID: 17507
		[PublicizedFrom(EAccessModifier.Private)]
		public const int VERSION = 1;

		// Token: 0x04004464 RID: 17508
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ushort[][][] m_regionData;

		// Token: 0x04004465 RID: 17509
		[PublicizedFrom(EAccessModifier.Private)]
		public string m_lastRegionPath;

		// Token: 0x04004466 RID: 17510
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_dirty;
	}
}
