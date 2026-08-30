using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

// Token: 0x02000B43 RID: 2883
public class MapChunkDatabaseByRegion : IMapChunkDatabase
{
	// Token: 0x0600577A RID: 22394 RVA: 0x00216718 File Offset: 0x00214918
	public MapChunkDatabaseByRegion(int playerId)
	{
		this.m_regions = new Dictionary<Vector2i, MapChunkDatabaseByRegion.RegionData>();
		this.m_chunksSent = new HashSet<int>();
		this.m_chunkIdsToSend = new List<int>();
		this.m_chunkDataToSend = new List<ushort[]>();
		this.m_playerId = playerId;
	}

	// Token: 0x0600577B RID: 22395 RVA: 0x0021676C File Offset: 0x0021496C
	public void Clear()
	{
		object regionsLock = this.m_regionsLock;
		lock (regionsLock)
		{
			this.m_regions.Clear();
		}
	}

	// Token: 0x0600577C RID: 22396 RVA: 0x002167B4 File Offset: 0x002149B4
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

	// Token: 0x0600577D RID: 22397 RVA: 0x0021681C File Offset: 0x00214A1C
	public void Add(int _chunkX, int _chunkZ, ushort[] _mapColors)
	{
		this.Add(_chunkX, _chunkZ, _mapColors, false);
	}

	// Token: 0x0600577E RID: 22398 RVA: 0x00216828 File Offset: 0x00214A28
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

	// Token: 0x0600577F RID: 22399 RVA: 0x00216870 File Offset: 0x00214A70
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

	// Token: 0x06005780 RID: 22400 RVA: 0x002168DC File Offset: 0x00214ADC
	public bool IsNetworkDataAvail()
	{
		return this.m_networkDataAvailable;
	}

	// Token: 0x06005781 RID: 22401 RVA: 0x002168E4 File Offset: 0x00214AE4
	public void ResetNetworkDataAvail()
	{
		this.m_networkDataAvailable = false;
	}

	// Token: 0x06005782 RID: 22402 RVA: 0x002168F0 File Offset: 0x00214AF0
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

	// Token: 0x06005783 RID: 22403 RVA: 0x00216A4C File Offset: 0x00214C4C
	public void LoadAsync(ThreadManager.TaskInfo _taskInfo)
	{
		IMapChunkDatabase.DirectoryPlayerId directoryPlayerId = (IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter;
		this.Load(Path.Join(directoryPlayerId.dir, directoryPlayerId.file));
	}

	// Token: 0x06005784 RID: 22404 RVA: 0x00216A88 File Offset: 0x00214C88
	public void SaveAsync(ThreadManager.TaskInfo _taskInfo)
	{
		IMapChunkDatabase.DirectoryPlayerId directoryPlayerId = (IMapChunkDatabase.DirectoryPlayerId)_taskInfo.parameter;
		this.Save(Path.Join(directoryPlayerId.dir, directoryPlayerId.file));
	}

	// Token: 0x06005785 RID: 22405 RVA: 0x00216AC2 File Offset: 0x00214CC2
	public void SetClientMapMiddlePosition(Vector2i _pos)
	{
		this.m_clientMapMiddlePosition = _pos;
		this.m_clientMapMiddlePositionUpdated = true;
	}

	// Token: 0x06005786 RID: 22406 RVA: 0x00216AD4 File Offset: 0x00214CD4
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

	// Token: 0x06005787 RID: 22407 RVA: 0x00216B4C File Offset: 0x00214D4C
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

	// Token: 0x06005788 RID: 22408 RVA: 0x00216D14 File Offset: 0x00214F14
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

	// Token: 0x06005789 RID: 22409 RVA: 0x00216F2C File Offset: 0x0021512C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToRegionAndOffset(long worldChunkKey, out Vector2i regionPos, out Vector2i chunkOffset)
	{
		int num = WorldChunkCache.extractX(worldChunkKey);
		int num2 = WorldChunkCache.extractZ(worldChunkKey);
		this.ToRegionAndOffset(num, num2, out regionPos, out chunkOffset);
	}

	// Token: 0x0600578A RID: 22410 RVA: 0x00216F51 File Offset: 0x00215151
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToRegionAndOffset(Vector3i chunkPos, out Vector2i regionPos, out Vector2i chunkOffset)
	{
		this.ToRegionAndOffset(chunkPos.x, chunkPos.y, out regionPos, out chunkOffset);
	}

	// Token: 0x0600578B RID: 22411 RVA: 0x00216F68 File Offset: 0x00215168
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

	// Token: 0x0600578C RID: 22412 RVA: 0x00216FA8 File Offset: 0x002151A8
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetRegionDataPath(string rootDirectory, Vector2i regionPos)
	{
		return Path.Join(rootDirectory, string.Format("{0}{1}{2}{3}{4}", new object[] { "r.", regionPos.x, ".", regionPos.y, ".7rm" }));
	}

	// Token: 0x0600578D RID: 22413 RVA: 0x0021700C File Offset: 0x0021520C
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

	// Token: 0x04004354 RID: 17236
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilePrefix = "r.";

	// Token: 0x04004355 RID: 17237
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FileElementSeparator = ".";

	// Token: 0x04004356 RID: 17238
	[PublicizedFrom(EAccessModifier.Private)]
	public const string FilePostfix = ".7rm";

	// Token: 0x04004357 RID: 17239
	[PublicizedFrom(EAccessModifier.Private)]
	public const int REGION_CHUNK_WIDTH = 32;

	// Token: 0x04004358 RID: 17240
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHUNK_TO_REGION_SHIFT = 5;

	// Token: 0x04004359 RID: 17241
	[PublicizedFrom(EAccessModifier.Private)]
	public const int REGION_CHUNK_AREA = 1024;

	// Token: 0x0400435A RID: 17242
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHUNK_DATA_LENGTH = 256;

	// Token: 0x0400435B RID: 17243
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ushort[] EMPTY_CHUNK_DATA = new ushort[256];

	// Token: 0x0400435C RID: 17244
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<Vector2i, MapChunkDatabaseByRegion.RegionData> m_regions;

	// Token: 0x0400435D RID: 17245
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object m_regionsLock = new object();

	// Token: 0x0400435E RID: 17246
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<int> m_chunksSent;

	// Token: 0x0400435F RID: 17247
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> m_chunkIdsToSend;

	// Token: 0x04004360 RID: 17248
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ushort[]> m_chunkDataToSend;

	// Token: 0x04004361 RID: 17249
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int m_playerId;

	// Token: 0x04004362 RID: 17250
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_networkDataAvailable;

	// Token: 0x04004363 RID: 17251
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i m_clientMapMiddlePosition;

	// Token: 0x04004364 RID: 17252
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_clientMapMiddlePositionUpdated;

	// Token: 0x02000B44 RID: 2884
	[PublicizedFrom(EAccessModifier.Private)]
	public class RegionData
	{
		// Token: 0x0600578F RID: 22415 RVA: 0x0021713C File Offset: 0x0021533C
		public RegionData()
		{
			this.m_regionData = new ushort[32][][];
			for (int i = 0; i < 32; i++)
			{
				this.m_regionData[i] = new ushort[32][];
			}
			this.m_dirty = true;
		}

		// Token: 0x06005790 RID: 22416 RVA: 0x0021717F File Offset: 0x0021537F
		public ushort[] GetChunkData(Vector2i offset)
		{
			return this.m_regionData[offset.y][offset.x];
		}

		// Token: 0x06005791 RID: 22417 RVA: 0x00217198 File Offset: 0x00215398
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

		// Token: 0x06005792 RID: 22418 RVA: 0x00217210 File Offset: 0x00215410
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

		// Token: 0x06005793 RID: 22419 RVA: 0x00217320 File Offset: 0x00215520
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

		// Token: 0x04004365 RID: 17253
		[PublicizedFrom(EAccessModifier.Private)]
		public const int VERSION = 1;

		// Token: 0x04004366 RID: 17254
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ushort[][][] m_regionData;

		// Token: 0x04004367 RID: 17255
		[PublicizedFrom(EAccessModifier.Private)]
		public string m_lastRegionPath;

		// Token: 0x04004368 RID: 17256
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_dirty;
	}
}
