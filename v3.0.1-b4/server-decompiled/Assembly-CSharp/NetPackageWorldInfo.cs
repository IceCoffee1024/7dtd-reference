using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x0200086D RID: 2157
[Preserve]
public class NetPackageWorldInfo : NetPackage
{
	// Token: 0x06003EE1 RID: 16097 RVA: 0x0018BBC1 File Offset: 0x00189DC1
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

	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06003EE2 RID: 16098 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003EE3 RID: 16099 RVA: 0x0018BC04 File Offset: 0x00189E04
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

	// Token: 0x06003EE4 RID: 16100 RVA: 0x0018BCC4 File Offset: 0x00189EC4
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

	// Token: 0x06003EE5 RID: 16101 RVA: 0x0018BD64 File Offset: 0x00189F64
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.WorldInfo(this.gameMode, this.levelName, this.gameName, this.guid, this.ppList, this.ticks, this.fixedSizeCC, this.firstTimeJoin, this.worldFileHashes, NetPackageWorldInfo.worldDataSize);
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x0018BDB2 File Offset: 0x00189FB2
	public override int GetLength()
	{
		return 48 + NetPackageWorldInfo.worldHashesData.Length + 4 + 8;
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x0018BDC4 File Offset: 0x00189FC4
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

	// Token: 0x040032AD RID: 12973
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameMode;

	// Token: 0x040032AE RID: 12974
	[PublicizedFrom(EAccessModifier.Private)]
	public string levelName;

	// Token: 0x040032AF RID: 12975
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameName;

	// Token: 0x040032B0 RID: 12976
	[PublicizedFrom(EAccessModifier.Private)]
	public string guid;

	// Token: 0x040032B1 RID: 12977
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerList ppList;

	// Token: 0x040032B2 RID: 12978
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong ticks;

	// Token: 0x040032B3 RID: 12979
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fixedSizeCC;

	// Token: 0x040032B4 RID: 12980
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstTimeJoin;

	// Token: 0x040032B5 RID: 12981
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, uint> worldFileHashes;

	// Token: 0x040032B6 RID: 12982
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] worldHashesData;

	// Token: 0x040032B7 RID: 12983
	[PublicizedFrom(EAccessModifier.Private)]
	public static long worldDataSize;
}
