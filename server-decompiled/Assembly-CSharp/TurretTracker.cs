using System;
using System.Collections.Generic;
using System.IO;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000C83 RID: 3203
[Preserve]
public class TurretTracker
{
	// Token: 0x17000A1B RID: 2587
	// (get) Token: 0x0600625A RID: 25178 RVA: 0x0026C45E File Offset: 0x0026A65E
	public static TurretTracker Instance
	{
		get
		{
			return TurretTracker.instance;
		}
	}

	// Token: 0x0600625B RID: 25179 RVA: 0x0026C465 File Offset: 0x0026A665
	public static void Init()
	{
		TurretTracker.instance = new TurretTracker();
		TurretTracker.instance.Load();
	}

	// Token: 0x0600625C RID: 25180 RVA: 0x0026C47C File Offset: 0x0026A67C
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

	// Token: 0x0600625D RID: 25181 RVA: 0x0026C4F0 File Offset: 0x0026A6F0
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

	// Token: 0x0600625E RID: 25182 RVA: 0x0026C54C File Offset: 0x0026A74C
	public void TriggerSave()
	{
		this.saveTime = Mathf.Min(this.saveTime, 10f);
	}

	// Token: 0x0600625F RID: 25183 RVA: 0x0026C564 File Offset: 0x0026A764
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

	// Token: 0x06006260 RID: 25184 RVA: 0x0026C5F9 File Offset: 0x0026A7F9
	public static void Cleanup()
	{
		if (TurretTracker.instance != null)
		{
			TurretTracker.instance.SaveAndClear();
		}
	}

	// Token: 0x06006261 RID: 25185 RVA: 0x0026C60C File Offset: 0x0026A80C
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

	// Token: 0x06006262 RID: 25186 RVA: 0x0026C63C File Offset: 0x0026A83C
	[PublicizedFrom(EAccessModifier.Private)]
	public void WaitOnSave()
	{
		if (this.saveThread != null)
		{
			this.saveThread.WaitForEnd(30);
			this.saveThread = null;
		}
	}

	// Token: 0x06006263 RID: 25187 RVA: 0x0026C65C File Offset: 0x0026A85C
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

	// Token: 0x06006264 RID: 25188 RVA: 0x0026C794 File Offset: 0x0026A994
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

	// Token: 0x06006265 RID: 25189 RVA: 0x0026C880 File Offset: 0x0026AA80
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

	// Token: 0x06006266 RID: 25190 RVA: 0x0026C914 File Offset: 0x0026AB14
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

	// Token: 0x06006267 RID: 25191 RVA: 0x0026C9B4 File Offset: 0x0026ABB4
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

	// Token: 0x06006268 RID: 25192 RVA: 0x0026CA20 File Offset: 0x0026AC20
	public List<int> GetTurretsList()
	{
		List<int> list = new List<int>();
		this.GetTurrets(list);
		return list;
	}

	// Token: 0x06006269 RID: 25193 RVA: 0x0026CA3C File Offset: 0x0026AC3C
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

	// Token: 0x0600626A RID: 25194 RVA: 0x0026CA9E File Offset: 0x0026AC9E
	public static int GetServerTurretCount()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return TurretTracker.Instance.turretsActive.Count + TurretTracker.Instance.turretsUnloaded.Count;
		}
		return TurretTracker.serverTurretCount;
	}

	// Token: 0x0600626B RID: 25195 RVA: 0x0026CAD1 File Offset: 0x0026ACD1
	public static void SetServerTurretCount(int count)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		TurretTracker.serverTurretCount = count;
	}

	// Token: 0x0600626C RID: 25196 RVA: 0x0026CAE6 File Offset: 0x0026ACE6
	public static bool CanAddMoreTurrets()
	{
		return !(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() || TurretTracker.GetServerTurretCount() < 500;
	}

	// Token: 0x04004C8F RID: 19599
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 1;

	// Token: 0x04004C90 RID: 19600
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSaveTime = 120f;

	// Token: 0x04004C91 RID: 19601
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cChangeSaveDelay = 10f;

	// Token: 0x04004C92 RID: 19602
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxTurrets = 500;

	// Token: 0x04004C93 RID: 19603
	[PublicizedFrom(EAccessModifier.Private)]
	public static int serverTurretCount;

	// Token: 0x04004C94 RID: 19604
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityTurret> turretsActive = new List<EntityTurret>();

	// Token: 0x04004C95 RID: 19605
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<int> turretsUnloaded = new List<int>();

	// Token: 0x04004C96 RID: 19606
	[PublicizedFrom(EAccessModifier.Private)]
	public float saveTime = 120f;

	// Token: 0x04004C97 RID: 19607
	[PublicizedFrom(EAccessModifier.Private)]
	public static TurretTracker instance;

	// Token: 0x04004C98 RID: 19608
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo saveThread;

	// Token: 0x04004C99 RID: 19609
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cNameKey = "turrets";

	// Token: 0x04004C9A RID: 19610
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cThreadKey = "turretDataSave";
}
