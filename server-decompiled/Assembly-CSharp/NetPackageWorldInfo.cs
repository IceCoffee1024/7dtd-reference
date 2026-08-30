using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000879 RID: 2169
[Preserve]
public class NetPackageWorldInfo : NetPackage
{
	// Token: 0x06003F46 RID: 16198 RVA: 0x0018DC91 File Offset: 0x0018BE91
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

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06003F47 RID: 16199 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003F48 RID: 16200 RVA: 0x0018DCD4 File Offset: 0x0018BED4
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

	// Token: 0x06003F49 RID: 16201 RVA: 0x0018DD94 File Offset: 0x0018BF94
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

	// Token: 0x06003F4A RID: 16202 RVA: 0x0018DE34 File Offset: 0x0018C034
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.WorldInfo(this.gameMode, this.levelName, this.gameName, this.guid, this.ppList, this.ticks, this.fixedSizeCC, this.firstTimeJoin, this.worldFileHashes, NetPackageWorldInfo.worldDataSize);
	}

	// Token: 0x06003F4B RID: 16203 RVA: 0x0018DE82 File Offset: 0x0018C082
	public override int GetLength()
	{
		return 48 + NetPackageWorldInfo.worldHashesData.Length + 4 + 8;
	}

	// Token: 0x06003F4C RID: 16204 RVA: 0x0018DE94 File Offset: 0x0018C094
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

	// Token: 0x0400332E RID: 13102
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameMode;

	// Token: 0x0400332F RID: 13103
	[PublicizedFrom(EAccessModifier.Private)]
	public string levelName;

	// Token: 0x04003330 RID: 13104
	[PublicizedFrom(EAccessModifier.Private)]
	public string gameName;

	// Token: 0x04003331 RID: 13105
	[PublicizedFrom(EAccessModifier.Private)]
	public string guid;

	// Token: 0x04003332 RID: 13106
	[PublicizedFrom(EAccessModifier.Private)]
	public PersistentPlayerList ppList;

	// Token: 0x04003333 RID: 13107
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong ticks;

	// Token: 0x04003334 RID: 13108
	[PublicizedFrom(EAccessModifier.Private)]
	public bool fixedSizeCC;

	// Token: 0x04003335 RID: 13109
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstTimeJoin;

	// Token: 0x04003336 RID: 13110
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, uint> worldFileHashes;

	// Token: 0x04003337 RID: 13111
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] worldHashesData;

	// Token: 0x04003338 RID: 13112
	[PublicizedFrom(EAccessModifier.Private)]
	public static long worldDataSize;
}
