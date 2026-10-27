using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000886 RID: 2182
[Preserve]
public class NetPackageWorldInfo : NetPackage
{
	// Token: 0x06003EFC RID: 16124 RVA: 0x0018EB5D File Offset: 0x0018CD5D
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

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x06003EFD RID: 16125 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003EFE RID: 16126 RVA: 0x0018EBA0 File Offset: 0x0018CDA0
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

	// Token: 0x06003EFF RID: 16127 RVA: 0x0018EC60 File Offset: 0x0018CE60
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

	// Token: 0x06003F00 RID: 16128 RVA: 0x0018ED00 File Offset: 0x0018CF00
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.WorldInfo(this.gameMode, this.levelName, this.gameName, this.guid, this.ppList, this.ticks, this.fixedSizeCC, this.firstTimeJoin, this.worldFileHashes, NetPackageWorldInfo.worldDataSize);
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x0018ED50 File Offset: 0x0018CF50
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

	// Token: 0x04003339 RID: 13113
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameMode;

	// Token: 0x0400333A RID: 13114
	[PublicizedFrom(EAccessModifier.Private)]
	public string levelName;

	// Token: 0x0400333B RID: 13115
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameName;

	// Token: 0x0400333C RID: 13116
	[PublicizedFrom(EAccessModifier.Private)]
	public string guid;

	// Token: 0x0400333D RID: 13117
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerList ppList;

	// Token: 0x0400333E RID: 13118
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong ticks;

	// Token: 0x0400333F RID: 13119
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fixedSizeCC;

	// Token: 0x04003340 RID: 13120
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstTimeJoin;

	// Token: 0x04003341 RID: 13121
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, uint> worldFileHashes;

	// Token: 0x04003342 RID: 13122
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] worldHashesData;

	// Token: 0x04003343 RID: 13123
	[PublicizedFrom(EAccessModifier.Private)]
	public static long worldDataSize;
}
