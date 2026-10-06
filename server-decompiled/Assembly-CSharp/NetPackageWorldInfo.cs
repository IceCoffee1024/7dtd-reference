using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x0200087C RID: 2172
[Preserve]
public class NetPackageWorldInfo : NetPackage
{
	// Token: 0x06003ED9 RID: 16089 RVA: 0x0018DAB5 File Offset: 0x0018BCB5
	public NetPackageWorldInfo Setup(string _gameMode, string _levelName, string _gameName, string _guid, PersistentPlayerList _playerList, ulong _ticks, bool _fixedSizeCC, bool _firstTimeJoin)
	{
		this.gameMode = _gameMode;
		this.levelName = _levelName;
		this.gameName = _gameName;
		this.ppList = _playerList;
		this.ticks = _ticks;
		this.guid = _guid;
		this.fixedSizeCC = _fixedSizeCC;
		this.firstTimeJoin = _firstTimeJoin;
		return this;
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x06003EDA RID: 16090 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003EDB RID: 16091 RVA: 0x0018DAF8 File Offset: 0x0018BCF8
	public override void read(PooledBinaryReader _reader)
	{
		this.gameMode = _reader.ReadString();
		this.levelName = _reader.ReadString();
		this.gameName = _reader.ReadString();
		this.guid = _reader.ReadString();
		this.ppList = (_reader.ReadBoolean() ? PersistentPlayerList.Read(_reader) : new PersistentPlayerList());
		this.ticks = _reader.ReadUInt64();
		this.fixedSizeCC = _reader.ReadBoolean();
		this.firstTimeJoin = _reader.ReadBoolean();
		int num = _reader.ReadInt32();
		this.worldFileHashes = new Dictionary<string, uint>();
		for (int i = 0; i < num; i++)
		{
			string text = _reader.ReadString();
			uint num2 = _reader.ReadUInt32();
			this.worldFileHashes.Add(text, num2);
		}
		NetPackageWorldInfo.worldDataSize = _reader.ReadInt64();
	}

	// Token: 0x06003EDC RID: 16092 RVA: 0x0018DBB8 File Offset: 0x0018BDB8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.gameMode);
		_writer.Write(this.levelName);
		_writer.Write(this.gameName);
		_writer.Write(this.guid);
		_writer.Write(this.ppList != null);
		PersistentPlayerList persistentPlayerList = this.ppList;
		if (persistentPlayerList != null)
		{
			persistentPlayerList.Write(_writer);
		}
		_writer.Write(this.ticks);
		_writer.Write(this.fixedSizeCC);
		_writer.Write(this.firstTimeJoin);
		_writer.Write(NetPackageWorldInfo.worldHashesData);
		_writer.Write(NetPackageWorldInfo.worldDataSize);
	}

	// Token: 0x06003EDD RID: 16093 RVA: 0x0018DC58 File Offset: 0x0018BE58
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.WorldInfo(this.gameMode, this.levelName, this.gameName, this.guid, this.ppList, this.ticks, this.fixedSizeCC, this.firstTimeJoin, this.worldFileHashes, NetPackageWorldInfo.worldDataSize);
	}

	// Token: 0x06003EDE RID: 16094 RVA: 0x0018DCA8 File Offset: 0x0018BEA8
	public static void PrepareWorldHashes()
	{
		NetPackageWorldInfo.worldHashesData = null;
		ChunkProviderGenerateWorldFromRaw chunkProviderGenerateWorldFromRaw = GameManager.Instance.World.ChunkCache.ChunkProvider as ChunkProviderGenerateWorldFromRaw;
		Dictionary<string, uint> dictionary = ((chunkProviderGenerateWorldFromRaw != null) ? chunkProviderGenerateWorldFromRaw.worldFileCrcs : null);
		NetPackageWorldInfo.worldDataSize = ((chunkProviderGenerateWorldFromRaw != null) ? chunkProviderGenerateWorldFromRaw.worldFileTotalSize : 0L);
		List<string> list = null;
		if (dictionary != null)
		{
			list = GameUtils.GetWorldFilesToTransmitToClient(dictionary.Keys);
		}
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				if (dictionary != null)
				{
					binaryWriter.Write(list.Count);
					using (List<string>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text = enumerator.Current;
							binaryWriter.Write(text);
							binaryWriter.Write(dictionary[text]);
						}
						goto IL_00B5;
					}
				}
				binaryWriter.Write(0);
				IL_00B5:
				NetPackageWorldInfo.worldHashesData = memoryStream.ToArray();
			}
		}
	}

	// Token: 0x04003319 RID: 13081
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameMode;

	// Token: 0x0400331A RID: 13082
	[PublicizedFrom(EAccessModifier.Private)]
	public string levelName;

	// Token: 0x0400331B RID: 13083
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameName;

	// Token: 0x0400331C RID: 13084
	[PublicizedFrom(EAccessModifier.Private)]
	public string guid;

	// Token: 0x0400331D RID: 13085
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerList ppList;

	// Token: 0x0400331E RID: 13086
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong ticks;

	// Token: 0x0400331F RID: 13087
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fixedSizeCC;

	// Token: 0x04003320 RID: 13088
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstTimeJoin;

	// Token: 0x04003321 RID: 13089
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, uint> worldFileHashes;

	// Token: 0x04003322 RID: 13090
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] worldHashesData;

	// Token: 0x04003323 RID: 13091
	[PublicizedFrom(EAccessModifier.Private)]
	public static long worldDataSize;
}
