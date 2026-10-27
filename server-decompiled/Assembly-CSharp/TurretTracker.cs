using System;
using System.Collections.Generic;
using System.IO;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C9C RID: 3228
[Preserve]
public class TurretTracker
{
	// Token: 0x17000A37 RID: 2615
	// (get) Token: 0x06006272 RID: 25202 RVA: 0x00270DCA File Offset: 0x0026EFCA
	public static TurretTracker Instance
	{
		get
		{
			return TurretTracker.instance;
		}
	}

	// Token: 0x06006273 RID: 25203 RVA: 0x00270DD1 File Offset: 0x0026EFD1
	public static void Init()
	{
		TurretTracker.instance = new TurretTracker();
		TurretTracker.instance.Load();
	}

	// Token: 0x06006274 RID: 25204 RVA: 0x00270DE8 File Offset: 0x0026EFE8
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

	// Token: 0x06006275 RID: 25205 RVA: 0x00270E5C File Offset: 0x0026F05C
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

	// Token: 0x06006276 RID: 25206 RVA: 0x00270EB8 File Offset: 0x0026F0B8
	public void TriggerSave()
	{
		this.saveTime = Mathf.Min(this.saveTime, 10f);
	}

	// Token: 0x06006277 RID: 25207 RVA: 0x00270ED0 File Offset: 0x0026F0D0
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

	// Token: 0x06006278 RID: 25208 RVA: 0x00270F65 File Offset: 0x0026F165
	public static void Cleanup()
	{
		if (TurretTracker.instance != null)
		{
			TurretTracker.instance.SaveAndClear();
		}
	}

	// Token: 0x06006279 RID: 25209 RVA: 0x00270F78 File Offset: 0x0026F178
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

	// Token: 0x0600627A RID: 25210 RVA: 0x00270FA8 File Offset: 0x0026F1A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void WaitOnSave()
	{
		if (this.saveThread != null)
		{
			this.saveThread.WaitForEnd(30);
			this.saveThread = null;
		}
	}

	// Token: 0x0600627B RID: 25211 RVA: 0x00270FC8 File Offset: 0x0026F1C8
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

	// Token: 0x0600627C RID: 25212 RVA: 0x00271100 File Offset: 0x0026F300
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

	// Token: 0x0600627D RID: 25213 RVA: 0x002711EC File Offset: 0x0026F3EC
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

	// Token: 0x0600627E RID: 25214 RVA: 0x00271280 File Offset: 0x0026F480
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

	// Token: 0x0600627F RID: 25215 RVA: 0x00271320 File Offset: 0x0026F520
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

	// Token: 0x06006280 RID: 25216 RVA: 0x0027138C File Offset: 0x0026F58C
	public List<int> GetTurretsList()
	{
		List<int> list = new List<int>();
		this.GetTurrets(list);
		return list;
	}

	// Token: 0x06006281 RID: 25217 RVA: 0x002713A8 File Offset: 0x0026F5A8
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

	// Token: 0x06006282 RID: 25218 RVA: 0x0027140A File Offset: 0x0026F60A
	public static int GetServerTurretCount()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return TurretTracker.Instance.turretsActive.Count + TurretTracker.Instance.turretsUnloaded.Count;
		}
		return TurretTracker.serverTurretCount;
	}

	// Token: 0x06006283 RID: 25219 RVA: 0x0027143D File Offset: 0x0026F63D
	public static void SetServerTurretCount(int count)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		TurretTracker.serverTurretCount = count;
	}

	// Token: 0x06006284 RID: 25220 RVA: 0x00271452 File Offset: 0x0026F652
	public static bool CanAddMoreTurrets()
	{
		return !(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() || TurretTracker.GetServerTurretCount() < 500;
	}

	// Token: 0x04004CEA RID: 19690
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 1;

	// Token: 0x04004CEB RID: 19691
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSaveTime = 120f;

	// Token: 0x04004CEC RID: 19692
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cChangeSaveDelay = 10f;

	// Token: 0x04004CED RID: 19693
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxTurrets = 500;

	// Token: 0x04004CEE RID: 19694
	[PublicizedFrom(EAccessModifier.Private)]
	public static int serverTurretCount;

	// Token: 0x04004CEF RID: 19695
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityTurret> turretsActive = new List<EntityTurret>();

	// Token: 0x04004CF0 RID: 19696
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> turretsUnloaded = new List<int>();

	// Token: 0x04004CF1 RID: 19697
	[PublicizedFrom(EAccessModifier.Private)]
	public float saveTime = 120f;

	// Token: 0x04004CF2 RID: 19698
	[PublicizedFrom(EAccessModifier.Private)]
	public static TurretTracker instance;

	// Token: 0x04004CF3 RID: 19699
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo saveThread;

	// Token: 0x04004CF4 RID: 19700
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cNameKey = "turrets";

	// Token: 0x04004CF5 RID: 19701
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cThreadKey = "turretDataSave";
}
