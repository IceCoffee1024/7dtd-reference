using System;
using System.Collections.Generic;
using System.IO;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C91 RID: 3217
[Preserve]
public class TurretTracker
{
	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x0600624D RID: 25165 RVA: 0x0026F8A2 File Offset: 0x0026DAA2
	public static TurretTracker Instance
	{
		get
		{
			return TurretTracker.instance;
		}
	}

	// Token: 0x0600624E RID: 25166 RVA: 0x0026F8A9 File Offset: 0x0026DAA9
	public static void Init()
	{
		TurretTracker.instance = new TurretTracker();
		TurretTracker.instance.Load();
	}

	// Token: 0x0600624F RID: 25167 RVA: 0x0026F8C0 File Offset: 0x0026DAC0
	public void AddTrackedTurret(EntityTurret _turret)
	{
		if (!_turret)
		{
			Log.Error("{0} AddTrackedTurret null", new object[] { base.GetType() });
			return;
		}
		if (this.turretsUnloaded.Contains(_turret.entityId))
		{
			this.turretsUnloaded.Remove(_turret.entityId);
		}
		if (!this.turretsActive.Contains(_turret))
		{
			this.turretsActive.Add(_turret);
			this.TriggerSave();
		}
	}

	// Token: 0x06006250 RID: 25168 RVA: 0x0026F934 File Offset: 0x0026DB34
	public void RemoveTrackedTurret(EntityTurret _turret, EnumRemoveEntityReason _reason)
	{
		this.turretsActive.Remove(_turret);
		if (_reason == EnumRemoveEntityReason.Unloaded)
		{
			this.turretsUnloaded.Add(_turret.entityId);
		}
		this.TriggerSave();
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageVehicleCount>().Setup(), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06006251 RID: 25169 RVA: 0x0026F990 File Offset: 0x0026DB90
	public void TriggerSave()
	{
		this.saveTime = Mathf.Min(this.saveTime, 10f);
	}

	// Token: 0x06006252 RID: 25170 RVA: 0x0026F9A8 File Offset: 0x0026DBA8
	public void Update()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		World world = GameManager.Instance.World;
		if (world == null || world.Players == null || world.Players.Count == 0)
		{
			return;
		}
		if (!GameManager.Instance.gameStateManager.IsGameStarted())
		{
			return;
		}
		this.saveTime -= Time.deltaTime;
		if (this.saveTime <= 0f && (this.saveThread == null || this.saveThread.HasTerminated()))
		{
			this.saveTime = 120f;
			this.Save();
		}
	}

	// Token: 0x06006253 RID: 25171 RVA: 0x0026FA3D File Offset: 0x0026DC3D
	public static void Cleanup()
	{
		if (TurretTracker.instance != null)
		{
			TurretTracker.instance.SaveAndClear();
		}
	}

	// Token: 0x06006254 RID: 25172 RVA: 0x0026FA50 File Offset: 0x0026DC50
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveAndClear()
	{
		this.WaitOnSave();
		this.Save();
		this.WaitOnSave();
		this.turretsActive.Clear();
		this.turretsUnloaded.Clear();
		TurretTracker.instance = null;
	}

	// Token: 0x06006255 RID: 25173 RVA: 0x0026FA80 File Offset: 0x0026DC80
	[PublicizedFrom(EAccessModifier.Private)]
	public void WaitOnSave()
	{
		if (this.saveThread != null)
		{
			this.saveThread.WaitForEnd(30);
			this.saveThread = null;
		}
	}

	// Token: 0x06006256 RID: 25174 RVA: 0x0026FAA0 File Offset: 0x0026DCA0
	public void Load()
	{
		string text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "turrets.dat");
		if (SdFile.Exists(text))
		{
			try
			{
				using (Stream stream = SdFile.OpenRead(text))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream);
						this.read(pooledBinaryReader);
					}
				}
			}
			catch (Exception)
			{
				text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "turrets.dat.bak");
				if (SdFile.Exists(text))
				{
					using (Stream stream2 = SdFile.OpenRead(text))
					{
						using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader2.SetBaseStream(stream2);
							this.read(pooledBinaryReader2);
						}
					}
				}
			}
			Log.Out("{0} {1}, loaded {2}", new object[]
			{
				base.GetType(),
				text,
				this.turretsUnloaded.Count
			});
		}
	}

	// Token: 0x06006257 RID: 25175 RVA: 0x0026FBD8 File Offset: 0x0026DDD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Save()
	{
		if (this.saveThread == null || !ThreadManager.ActiveThreads.ContainsKey("turretDataSave"))
		{
			Log.Out("{0} saving {1} ({2} + {3})", new object[]
			{
				base.GetType(),
				this.turretsActive.Count + this.turretsUnloaded.Count,
				this.turretsActive.Count,
				this.turretsUnloaded.Count
			});
			PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				this.write(pooledBinaryWriter);
			}
			this.saveThread = ThreadManager.StartThread("turretDataSave", null, new ThreadManager.ThreadFunctionLoopDelegate(this.SaveThread), null, pooledExpandableMemoryStream, null, false, true);
		}
	}

	// Token: 0x06006258 RID: 25176 RVA: 0x0026FCC4 File Offset: 0x0026DEC4
	[PublicizedFrom(EAccessModifier.Private)]
	public int SaveThread(ThreadManager.ThreadInfo _threadInfo)
	{
		PooledExpandableMemoryStream pooledExpandableMemoryStream = (PooledExpandableMemoryStream)_threadInfo.parameter;
		string text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "turrets.dat");
		if (SdFile.Exists(text))
		{
			SdFile.Copy(text, string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "turrets.dat.bak"), true);
		}
		pooledExpandableMemoryStream.Position = 0L;
		StreamUtils.WriteStreamToFile(pooledExpandableMemoryStream, text);
		Log.Out("{0} saved {1} bytes", new object[]
		{
			base.GetType(),
			pooledExpandableMemoryStream.Length
		});
		MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		return -1;
	}

	// Token: 0x06006259 RID: 25177 RVA: 0x0026FD58 File Offset: 0x0026DF58
	[PublicizedFrom(EAccessModifier.Private)]
	public void read(PooledBinaryReader _br)
	{
		if (_br.ReadChar() != 'v' || _br.ReadChar() != 'd' || _br.ReadChar() != 'a' || _br.ReadChar() != '\0')
		{
			Log.Error("{0} file bad signature", new object[] { base.GetType() });
			return;
		}
		if (_br.ReadByte() != 1)
		{
			Log.Error("{0} file bad version", new object[] { base.GetType() });
			return;
		}
		this.turretsUnloaded.Clear();
		int num = _br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.turretsUnloaded.Add(_br.ReadInt32());
		}
	}

	// Token: 0x0600625A RID: 25178 RVA: 0x0026FDF8 File Offset: 0x0026DFF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void write(PooledBinaryWriter _bw)
	{
		_bw.Write('v');
		_bw.Write('d');
		_bw.Write('a');
		_bw.Write(0);
		_bw.Write(1);
		List<int> list = new List<int>();
		this.GetTurrets(list);
		_bw.Write(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			_bw.Write(list[i]);
		}
	}

	// Token: 0x0600625B RID: 25179 RVA: 0x0026FE64 File Offset: 0x0026E064
	public List<int> GetTurretsList()
	{
		List<int> list = new List<int>();
		this.GetTurrets(list);
		return list;
	}

	// Token: 0x0600625C RID: 25180 RVA: 0x0026FE80 File Offset: 0x0026E080
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetTurrets(List<int> _list)
	{
		for (int i = 0; i < this.turretsActive.Count; i++)
		{
			_list.Add(this.turretsActive[i].entityId);
		}
		for (int j = 0; j < this.turretsUnloaded.Count; j++)
		{
			_list.Add(this.turretsUnloaded[j]);
		}
	}

	// Token: 0x0600625D RID: 25181 RVA: 0x0026FEE2 File Offset: 0x0026E0E2
	public static int GetServerTurretCount()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return TurretTracker.Instance.turretsActive.Count + TurretTracker.Instance.turretsUnloaded.Count;
		}
		return TurretTracker.serverTurretCount;
	}

	// Token: 0x0600625E RID: 25182 RVA: 0x0026FF15 File Offset: 0x0026E115
	public static void SetServerTurretCount(int count)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		TurretTracker.serverTurretCount = count;
	}

	// Token: 0x0600625F RID: 25183 RVA: 0x0026FF2A File Offset: 0x0026E12A
	public static bool CanAddMoreTurrets()
	{
		return !(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() || TurretTracker.GetServerTurretCount() < 500;
	}

	// Token: 0x04004CC5 RID: 19653
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 1;

	// Token: 0x04004CC6 RID: 19654
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSaveTime = 120f;

	// Token: 0x04004CC7 RID: 19655
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cChangeSaveDelay = 10f;

	// Token: 0x04004CC8 RID: 19656
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxTurrets = 500;

	// Token: 0x04004CC9 RID: 19657
	[PublicizedFrom(EAccessModifier.Private)]
	public static int serverTurretCount;

	// Token: 0x04004CCA RID: 19658
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityTurret> turretsActive = new List<EntityTurret>();

	// Token: 0x04004CCB RID: 19659
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> turretsUnloaded = new List<int>();

	// Token: 0x04004CCC RID: 19660
	[PublicizedFrom(EAccessModifier.Private)]
	public float saveTime = 120f;

	// Token: 0x04004CCD RID: 19661
	[PublicizedFrom(EAccessModifier.Private)]
	public static TurretTracker instance;

	// Token: 0x04004CCE RID: 19662
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo saveThread;

	// Token: 0x04004CCF RID: 19663
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cNameKey = "turrets";

	// Token: 0x04004CD0 RID: 19664
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cThreadKey = "turretDataSave";
}
