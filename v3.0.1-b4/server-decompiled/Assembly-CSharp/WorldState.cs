using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x02000BFA RID: 3066
public class WorldState
{
	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x06005D9C RID: 23964 RVA: 0x00246BFE File Offset: 0x00244DFE
	// (set) Token: 0x06005D9D RID: 23965 RVA: 0x00246C06 File Offset: 0x00244E06
	public string Guid
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06005D9E RID: 23966 RVA: 0x00246C0F File Offset: 0x00244E0F
	public WorldState()
	{
		this.providerId = EnumChunkProviderId.Disc;
		this.saveDataLimit = -1L;
		this.playerSpawnPoints = new SpawnPointList();
		this.GenerateNewGuid();
	}

	// Token: 0x06005D9F RID: 23967 RVA: 0x00246C44 File Offset: 0x00244E44
	[PublicizedFrom(EAccessModifier.Private)]
	public bool SaveLoad(string _filename, bool _load, bool _warnOnDifferentVersion, bool _infOnDiferentVersion)
	{
		bool flag2;
		lock (this)
		{
			Stream stream = null;
			try
			{
				if (_load)
				{
					try
					{
						stream = SdFile.OpenRead(_filename);
						goto IL_0054;
					}
					catch (Exception ex)
					{
						Log.Error(string.Format("Opening saved game: {0}", ex));
						goto IL_0054;
					}
				}
				try
				{
					stream = new BufferedStream(SdFile.Open(_filename, FileMode.Create, FileAccess.Write, FileShare.Read));
				}
				catch (Exception ex2)
				{
					Log.Error(string.Format("Opening buffer to save game: {0}", ex2));
				}
				IL_0054:
				flag2 = stream != null && this.SaveLoad(stream, _load, _warnOnDifferentVersion, _infOnDiferentVersion);
			}
			catch (Exception ex3)
			{
				Log.Error(string.Format("Exception reading world header at pos {0}: {1}", (stream != null) ? stream.Position : 0L, ex3));
				flag2 = false;
			}
			finally
			{
				if (stream != null)
				{
					stream.Dispose();
				}
			}
		}
		return flag2;
	}

	// Token: 0x06005DA0 RID: 23968 RVA: 0x00246D3C File Offset: 0x00244F3C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool SaveLoad(Stream _stream, bool _load, bool _warnOnDifferentVersion, bool _infOnDiferentVersion)
	{
		bool flag3;
		lock (this)
		{
			PooledBinaryWriter pooledBinaryWriter = null;
			PooledBinaryReader pooledBinaryReader = null;
			try
			{
				IBinaryReaderOrWriter binaryReaderOrWriter;
				if (_load)
				{
					this.chunkSizeX = (this.chunkSizeY = (this.chunkSizeZ = (this.chunkCount = 0)));
					pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false);
					pooledBinaryReader.SetBaseStream(_stream);
					binaryReaderOrWriter = pooledBinaryReader;
					int num = (int)binaryReaderOrWriter.ReadWrite(' ');
					char c = binaryReaderOrWriter.ReadWrite(' ');
					char c2 = binaryReaderOrWriter.ReadWrite(' ');
					byte b = binaryReaderOrWriter.ReadWrite(1);
					if (num != 116 || c != 't' || c2 != 'w' || b != 0)
					{
						Log.Error("Invalid magic bytes in world header");
						return false;
					}
				}
				else
				{
					pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false);
					pooledBinaryWriter.SetBaseStream(_stream);
					binaryReaderOrWriter = pooledBinaryWriter;
					binaryReaderOrWriter.ReadWrite('t');
					binaryReaderOrWriter.ReadWrite('t');
					binaryReaderOrWriter.ReadWrite('w');
					binaryReaderOrWriter.ReadWrite(0);
				}
				this.version = binaryReaderOrWriter.ReadWrite((uint)WorldState.CurrentSaveVersion);
				if (_load)
				{
					if ((ulong)this.version > (ulong)((long)WorldState.CurrentSaveVersion))
					{
						return true;
					}
					if (this.version > 11U)
					{
						this.gameVersionString = binaryReaderOrWriter.ReadWrite("");
						if (this.gameVersionString != Constants.cVersionInformation.LongString)
						{
							if (_warnOnDifferentVersion)
							{
								Log.Warning("Loaded world file from different version: '{0}'", new object[] { this.gameVersionString });
							}
							else if (_infOnDiferentVersion)
							{
								Log.Out("Loaded world file from different version: '{0}'", new object[] { this.gameVersionString });
							}
						}
					}
				}
				else
				{
					binaryReaderOrWriter.ReadWrite(Constants.cVersionInformation.LongString);
				}
				if (_load)
				{
					if (this.version > 14U)
					{
						VersionInformation.EGameReleaseType egameReleaseType = (VersionInformation.EGameReleaseType)binaryReaderOrWriter.ReadWrite(1);
						int num2 = binaryReaderOrWriter.ReadWrite(3);
						int num3 = binaryReaderOrWriter.ReadWrite(1);
						int num4 = binaryReaderOrWriter.ReadWrite(4);
						this.gameVersion = new VersionInformation(egameReleaseType, num2, num3, num4);
					}
					else
					{
						VersionInformation.TryParseLegacyString(this.gameVersionString, out this.gameVersion);
					}
				}
				else
				{
					binaryReaderOrWriter.ReadWrite(1);
					binaryReaderOrWriter.ReadWrite(3);
					binaryReaderOrWriter.ReadWrite(1);
					binaryReaderOrWriter.ReadWrite(4);
				}
				binaryReaderOrWriter.ReadWrite(0U);
				if (this.version > 6U)
				{
					this.activeGameMode = binaryReaderOrWriter.ReadWrite(this.activeGameMode);
				}
				binaryReaderOrWriter.ReadWrite(0U);
				this.waterLevel = binaryReaderOrWriter.ReadWrite(this.waterLevel);
				this.chunkSizeX = binaryReaderOrWriter.ReadWrite(this.chunkSizeX);
				this.chunkSizeZ = binaryReaderOrWriter.ReadWrite(this.chunkSizeY);
				this.chunkSizeY = binaryReaderOrWriter.ReadWrite(this.chunkSizeZ);
				this.chunkCount = binaryReaderOrWriter.ReadWrite(this.chunkCount);
				this.providerId = (EnumChunkProviderId)binaryReaderOrWriter.ReadWrite((int)this.providerId);
				this.seed = binaryReaderOrWriter.ReadWrite(this.seed);
				this.worldTime = binaryReaderOrWriter.ReadWrite(this.worldTime);
				if (this.version > 8U)
				{
					this.timeInTicks = binaryReaderOrWriter.ReadWrite(this.timeInTicks);
				}
				if (_load)
				{
					if (this.version == 10U)
					{
						binaryReaderOrWriter.ReadWrite(0UL);
					}
					if (this.version > 1U && this.version < 7U)
					{
						binaryReaderOrWriter.ReadWrite(false);
					}
					if (this.version > 4U && this.version < 7U)
					{
						binaryReaderOrWriter.ReadWrite(false);
						binaryReaderOrWriter.ReadWrite(false);
					}
					if (this.version > 5U)
					{
						this.playerSpawnPoints.Read(binaryReaderOrWriter);
					}
					else if (this.version > 2U)
					{
						this.playerSpawnPoints.Clear();
						int num5 = binaryReaderOrWriter.ReadWrite(0);
						for (int i = 0; i < num5; i++)
						{
							this.playerSpawnPoints.Add(new SpawnPoint(new Vector3i(binaryReaderOrWriter.ReadWrite(0), binaryReaderOrWriter.ReadWrite(0), binaryReaderOrWriter.ReadWrite(0))));
						}
					}
				}
				else
				{
					binaryReaderOrWriter.ReadWrite(0);
					binaryReaderOrWriter.ReadWrite(0);
				}
				if (this.version > 3U)
				{
					this.nextEntityID = binaryReaderOrWriter.ReadWrite(this.nextEntityID);
				}
				if (_load)
				{
					this.nextEntityID = Utils.FastMax(this.nextEntityID, 171);
				}
				if (this.version >= 21U)
				{
					this.saveDataLimit = binaryReaderOrWriter.ReadWrite(this.saveDataLimit);
				}
				else
				{
					this.saveDataLimit = -1L;
				}
				if (this.version > 7U)
				{
					int num6 = binaryReaderOrWriter.ReadWrite((int)((this.dynamicSpawnerState != null) ? this.dynamicSpawnerState.Length : 0L));
					if (_load)
					{
						if (num6 > 0)
						{
							this.dynamicSpawnerState = new MemoryStream(num6);
							this.dynamicSpawnerState.SetLength((long)num6);
							binaryReaderOrWriter.ReadWrite(this.dynamicSpawnerState.GetBuffer(), 0, num6);
							this.dynamicSpawnerState.Position = 0L;
						}
					}
					else if (this.dynamicSpawnerState != null)
					{
						this.dynamicSpawnerState.Position = 0L;
						StreamUtils.StreamCopy(this.dynamicSpawnerState, binaryReaderOrWriter.BaseStream, null, true);
					}
				}
				if (this.version > 10U)
				{
					int num7 = binaryReaderOrWriter.ReadWrite((int)((this.aiDirectorState != null) ? this.aiDirectorState.Length : 0L));
					if (_load)
					{
						if (num7 > 0)
						{
							this.aiDirectorState = new MemoryStream(num7);
							this.aiDirectorState.SetLength((long)num7);
							binaryReaderOrWriter.ReadWrite(this.aiDirectorState.GetBuffer(), 0, num7);
							this.aiDirectorState.Position = 0L;
						}
					}
					else if (this.aiDirectorState != null)
					{
						this.aiDirectorState.Position = 0L;
						StreamUtils.StreamCopy(this.aiDirectorState, binaryReaderOrWriter.BaseStream, null, true);
					}
				}
				if (this.version > 12U)
				{
					if (this.version >= 23U)
					{
						this.sleeperVolumesSaveVersion = binaryReaderOrWriter.ReadWrite(1);
					}
					int num8 = binaryReaderOrWriter.ReadWrite((int)((this.sleeperVolumeState != null) ? this.sleeperVolumeState.Length : 0L));
					if (_load)
					{
						if (num8 > 0)
						{
							this.sleeperVolumeState = new MemoryStream(num8);
							this.sleeperVolumeState.SetLength((long)num8);
							binaryReaderOrWriter.ReadWrite(this.sleeperVolumeState.GetBuffer(), 0, num8);
							this.sleeperVolumeState.Position = 0L;
						}
					}
					else if (this.sleeperVolumeState != null)
					{
						this.sleeperVolumeState.Position = 0L;
						StreamUtils.StreamCopy(this.sleeperVolumeState, binaryReaderOrWriter.BaseStream, null, true);
					}
				}
				if (this.version >= 19U)
				{
					if (this.version >= 23U)
					{
						this.triggerVolumesSaveVersion = binaryReaderOrWriter.ReadWrite(1);
					}
					int num9 = binaryReaderOrWriter.ReadWrite((int)((this.triggerVolumeState != null) ? this.triggerVolumeState.Length : 0L));
					if (_load)
					{
						if (num9 > 0)
						{
							this.triggerVolumeState = new MemoryStream(num9);
							this.triggerVolumeState.SetLength((long)num9);
							binaryReaderOrWriter.ReadWrite(this.triggerVolumeState.GetBuffer(), 0, num9);
							this.triggerVolumeState.Position = 0L;
						}
					}
					else if (this.triggerVolumeState != null)
					{
						this.triggerVolumeState.Position = 0L;
						StreamUtils.StreamCopy(this.triggerVolumeState, binaryReaderOrWriter.BaseStream, null, true);
					}
				}
				if (this.version >= 20U)
				{
					if (this.version >= 23U)
					{
						this.wallVolumesSaveVersion = binaryReaderOrWriter.ReadWrite(1);
					}
					int num10 = binaryReaderOrWriter.ReadWrite((int)((this.wallVolumeState != null) ? this.wallVolumeState.Length : 0L));
					if (_load)
					{
						if (num10 > 0)
						{
							this.wallVolumeState = new MemoryStream(num10);
							this.wallVolumeState.SetLength((long)num10);
							binaryReaderOrWriter.ReadWrite(this.wallVolumeState.GetBuffer(), 0, num10);
							this.wallVolumeState.Position = 0L;
						}
					}
					else if (this.wallVolumeState != null)
					{
						this.wallVolumeState.Position = 0L;
						StreamUtils.StreamCopy(this.wallVolumeState, binaryReaderOrWriter.BaseStream, null, true);
					}
				}
				bool flag2 = false;
				if (this.version > 11U)
				{
					long position = binaryReaderOrWriter.BaseStream.Position;
					int num11 = 0;
					if (this.version > 15U)
					{
						num11 = binaryReaderOrWriter.ReadWrite(0);
					}
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.version >= 22U)
					{
						if (_load)
						{
							int num12 = num11 - 4;
							if (num12 > 0)
							{
								flag2 = true;
								WeatherManager.Load(binaryReaderOrWriter, num12);
							}
						}
						else
						{
							WeatherManager.Save(binaryReaderOrWriter);
						}
					}
					if (this.version > 15U)
					{
						if (_load)
						{
							if (binaryReaderOrWriter.BaseStream.Position != position + (long)num11)
							{
								if (flag2)
								{
									Log.Out("Failed reading weather data from world header");
								}
								binaryReaderOrWriter.BaseStream.Position = position + (long)num11;
							}
						}
						else
						{
							num11 = (int)(binaryReaderOrWriter.BaseStream.Position - position);
							binaryReaderOrWriter.BaseStream.Position = position;
							binaryReaderOrWriter.ReadWrite(num11);
							binaryReaderOrWriter.BaseStream.Seek(0L, SeekOrigin.End);
						}
					}
				}
				if (this.version > 13U && (flag2 || this.version > 15U))
				{
					this.Guid = binaryReaderOrWriter.ReadWrite(this.Guid);
				}
				if (_load && string.IsNullOrEmpty(this.Guid))
				{
					this.GenerateNewGuid();
				}
				flag3 = true;
			}
			catch (Exception ex)
			{
				Log.Error("Exception reading world header at pos {0}:", new object[] { _stream.Position });
				Log.Exception(ex);
				flag3 = false;
			}
			finally
			{
				if (pooledBinaryReader != null)
				{
					MemoryPools.poolBinaryReader.FreeSync(pooledBinaryReader);
				}
				if (pooledBinaryWriter != null)
				{
					MemoryPools.poolBinaryWriter.FreeSync(pooledBinaryWriter);
				}
			}
		}
		return flag3;
	}

	// Token: 0x06005DA1 RID: 23969 RVA: 0x002476AC File Offset: 0x002458AC
	public bool Load(string _filename, bool _warnOnDifferentVersion = true, bool _infOnDiferentVersion = false, bool _makeExtraBackupOnSuccess = false)
	{
		WorldState.<>c__DisplayClass32_0 CS$<>8__locals1;
		CS$<>8__locals1._makeExtraBackupOnSuccess = _makeExtraBackupOnSuccess;
		CS$<>8__locals1._filename = _filename;
		if (this.SaveLoad(CS$<>8__locals1._filename, true, _warnOnDifferentVersion, _infOnDiferentVersion))
		{
			WorldState.<Load>g__DoExtraBackup|32_0(CS$<>8__locals1._filename, ref CS$<>8__locals1);
			return true;
		}
		Log.Warning("Failed loading world header file: " + CS$<>8__locals1._filename);
		SdFile.Copy(CS$<>8__locals1._filename, CS$<>8__locals1._filename + ".loadFailed", true);
		string text = CS$<>8__locals1._filename + ".bak";
		if (SdFile.Exists(text))
		{
			Log.Out("Trying backup header: " + text);
			if (this.SaveLoad(text, true, _warnOnDifferentVersion, _infOnDiferentVersion))
			{
				WorldState.<Load>g__DoExtraBackup|32_0(text, ref CS$<>8__locals1);
				return true;
			}
			SdFile.Copy(text, text + ".loadFailed", true);
			Log.Error("Failed loading backup header file!");
		}
		else
		{
			Log.Out("No backup header!");
		}
		string text2 = CS$<>8__locals1._filename + ".ext.bak";
		if (SdFile.Exists(text2))
		{
			Log.Out("Trying extra backup header (from last successful load): " + text2);
			if (this.SaveLoad(text2, true, _warnOnDifferentVersion, _infOnDiferentVersion))
			{
				return true;
			}
			SdFile.Copy(text2, text2 + ".loadFailed", true);
			Log.Error("Failed loading extra backup header file!");
		}
		else
		{
			Log.Out("No extra backup header!");
		}
		return false;
	}

	// Token: 0x06005DA2 RID: 23970 RVA: 0x002477E2 File Offset: 0x002459E2
	public bool Save(string _filename)
	{
		if (SdFile.Exists(_filename) && GameIO.FileSize(_filename) > 0L)
		{
			SdFile.Copy(_filename, _filename + ".bak", true);
		}
		return this.SaveLoad(_filename, false, false, false);
	}

	// Token: 0x06005DA3 RID: 23971 RVA: 0x00247812 File Offset: 0x00245A12
	public bool Save(Stream _stream)
	{
		return this.SaveLoad(_stream, false, false, false);
	}

	// Token: 0x06005DA4 RID: 23972 RVA: 0x00247820 File Offset: 0x00245A20
	public void SetFrom(World _world, EnumChunkProviderId _chunkProviderId)
	{
		this.waterLevel = WorldConstants.WaterLevel;
		this.chunkSizeX = 16;
		this.chunkSizeY = 16;
		this.chunkSizeZ = 256;
		this.chunkCount = 0;
		this.providerId = _chunkProviderId;
		this.seed = _world.Seed;
		this.worldTime = _world.worldTime;
		this.timeInTicks = GameTimer.Instance.ticks;
		this.sleeperVolumeState = new MemoryStream();
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.sleeperVolumeState);
			_world.WriteSleeperVolumes(pooledBinaryWriter);
		}
		this.triggerVolumeState = new MemoryStream();
		using (PooledBinaryWriter pooledBinaryWriter2 = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter2.SetBaseStream(this.triggerVolumeState);
			_world.WriteTriggerVolumes(pooledBinaryWriter2);
		}
		this.wallVolumeState = new MemoryStream();
		using (PooledBinaryWriter pooledBinaryWriter3 = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter3.SetBaseStream(this.wallVolumeState);
			_world.WriteWallVolumes(pooledBinaryWriter3);
		}
		this.nextEntityID = EntityFactory.nextEntityID;
		this.activeGameMode = _world.GetGameMode();
		this.dynamicSpawnerState = new MemoryStream();
		if (_world.GetDynamiceSpawnManager() != null)
		{
			using (PooledBinaryWriter pooledBinaryWriter4 = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter4.SetBaseStream(this.dynamicSpawnerState);
				_world.GetDynamiceSpawnManager().Write(pooledBinaryWriter4);
			}
		}
		this.aiDirectorState = new MemoryStream();
		if (_world.aiDirector != null)
		{
			using (PooledBinaryWriter pooledBinaryWriter5 = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter5.SetBaseStream(this.aiDirectorState);
				_world.aiDirector.Save(pooledBinaryWriter5);
				return;
			}
		}
		using (PooledBinaryWriter pooledBinaryWriter6 = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter6.SetBaseStream(this.aiDirectorState);
			new AIDirector(_world).Save(pooledBinaryWriter6);
		}
	}

	// Token: 0x06005DA5 RID: 23973 RVA: 0x00247A50 File Offset: 0x00245C50
	public void ResetDynamicData()
	{
		this.worldTime = 0UL;
		this.timeInTicks = 0UL;
	}

	// Token: 0x06005DA6 RID: 23974 RVA: 0x00247A62 File Offset: 0x00245C62
	public void GenerateNewGuid()
	{
		this.Guid = Utils.GenerateGuid();
	}

	// Token: 0x06005DA8 RID: 23976 RVA: 0x00247A78 File Offset: 0x00245C78
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <Load>g__DoExtraBackup|32_0(string sourceFilename, ref WorldState.<>c__DisplayClass32_0 A_1)
	{
		if (!A_1._makeExtraBackupOnSuccess)
		{
			return;
		}
		string text = A_1._filename + ".ext.bak";
		try
		{
			SdFile.Copy(sourceFilename, text, true);
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("Failed to make extra backup (due to successfully loading) by copying '{0}' to '{1}': {2}", sourceFilename, text, ex));
		}
	}

	// Token: 0x04004860 RID: 18528
	[PublicizedFrom(EAccessModifier.Private)]
	public static int CurrentSaveVersion = 23;

	// Token: 0x04004861 RID: 18529
	[PublicizedFrom(EAccessModifier.Private)]
	public uint version;

	// Token: 0x04004862 RID: 18530
	public string gameVersionString = "";

	// Token: 0x04004863 RID: 18531
	public VersionInformation gameVersion;

	// Token: 0x04004864 RID: 18532
	[PublicizedFrom(EAccessModifier.Private)]
	public float waterLevel;

	// Token: 0x04004865 RID: 18533
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkSizeX;

	// Token: 0x04004866 RID: 18534
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkSizeY;

	// Token: 0x04004867 RID: 18535
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkSizeZ;

	// Token: 0x04004868 RID: 18536
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkCount;

	// Token: 0x04004869 RID: 18537
	public MemoryStream dynamicSpawnerState;

	// Token: 0x0400486A RID: 18538
	public MemoryStream aiDirectorState;

	// Token: 0x0400486B RID: 18539
	public int activeGameMode;

	// Token: 0x0400486C RID: 18540
	public EnumChunkProviderId providerId;

	// Token: 0x0400486D RID: 18541
	public int seed;

	// Token: 0x0400486E RID: 18542
	public ulong worldTime;

	// Token: 0x0400486F RID: 18543
	public ulong timeInTicks;

	// Token: 0x04004870 RID: 18544
	public int nextEntityID;

	// Token: 0x04004871 RID: 18545
	public long saveDataLimit;

	// Token: 0x04004872 RID: 18546
	[PublicizedFrom(EAccessModifier.Private)]
	public SpawnPointList playerSpawnPoints;

	// Token: 0x04004873 RID: 18547
	public int sleeperVolumesSaveVersion;

	// Token: 0x04004874 RID: 18548
	public MemoryStream sleeperVolumeState;

	// Token: 0x04004875 RID: 18549
	public int triggerVolumesSaveVersion;

	// Token: 0x04004876 RID: 18550
	public MemoryStream triggerVolumeState;

	// Token: 0x04004877 RID: 18551
	public int wallVolumesSaveVersion;

	// Token: 0x04004878 RID: 18552
	public MemoryStream wallVolumeState;
}
