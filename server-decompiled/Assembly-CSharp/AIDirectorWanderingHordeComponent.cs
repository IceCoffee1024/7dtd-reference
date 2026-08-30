using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200041E RID: 1054
[Preserve]
public class AIDirectorWanderingHordeComponent : AIDirectorHordeComponent
{
	// Token: 0x06002091 RID: 8337 RVA: 0x000C4403 File Offset: 0x000C2603
	public override void InitNewGame()
	{
		this.isPlaytest = GameUtils.IsPlaytesting();
		this.BanditNextTime = 0UL;
		this.HordeNextTime = 0UL;
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x000C4420 File Offset: 0x000C2620
	public override void Tick(double _dt)
	{
		if (this.isPlaytest)
		{
			return;
		}
		base.Tick(_dt);
		this.TickActiveSpawns((float)_dt);
		this.TickNextTime(ref this.HordeNextTime, AIWanderingHordeSpawner.SpawnType.Horde);
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x000C4448 File Offset: 0x000C2648
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickActiveSpawns(float dt)
	{
		for (int i = this.spawners.Count - 1; i >= 0; i--)
		{
			AIWanderingHordeSpawner aiwanderingHordeSpawner = this.spawners[i];
			if (aiwanderingHordeSpawner.Update(this.Director.World, dt))
			{
				AIDirector.LogAIExtra("Wandering spawner finished {0}", new object[] { aiwanderingHordeSpawner.spawnType });
				aiwanderingHordeSpawner.Cleanup();
				this.spawners.RemoveAt(i);
			}
		}
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x000C44C0 File Offset: 0x000C26C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickNextTime(ref ulong _nextTime, AIWanderingHordeSpawner.SpawnType _spawnType)
	{
		if (!GameStats.GetBool(EnumGameStats.ZombieHordeMeter) || !GameStats.GetBool(EnumGameStats.IsSpawnEnemies))
		{
			_nextTime = 0UL;
			return;
		}
		if (_nextTime == 0UL)
		{
			if (this.Director.World.worldTime > 28000UL)
			{
				this.ChooseNextTime(_spawnType);
				return;
			}
		}
		else
		{
			int num = (int)(_nextTime - this.Director.World.worldTime);
			int num2 = num / 1000;
			if (num2 < 7)
			{
				if (this.OtherHordesAreActive)
				{
					_nextTime += (ulong)((7 - num2) * 1000);
					return;
				}
				if (num <= 0)
				{
					if (this.Director.World.Players.Count > 0)
					{
						this.StartSpawning(_spawnType);
						return;
					}
					this.ChooseNextTime(_spawnType);
				}
			}
		}
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x000C456C File Offset: 0x000C276C
	public override void Read(BinaryReader _stream, int _version)
	{
		base.Read(_stream, _version);
		this.HordeNextTime = _stream.ReadUInt64();
		if (_version > 3)
		{
			this.BanditNextTime = _stream.ReadUInt64();
		}
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x000C4592 File Offset: 0x000C2792
	public override void Write(BinaryWriter _stream)
	{
		base.Write(_stream);
		_stream.Write(this.HordeNextTime);
		_stream.Write(this.BanditNextTime);
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x000C45B4 File Offset: 0x000C27B4
	public void StartSpawning(AIWanderingHordeSpawner.SpawnType _spawnType)
	{
		AIDirector.LogAI("Wandering StartSpawning {0}", new object[] { _spawnType });
		this.CleanupType(_spawnType);
		bool flag = false;
		DictionaryList<int, AIDirectorPlayerState> trackedPlayers = this.Director.GetComponent<AIDirectorPlayerManagementComponent>().trackedPlayers;
		for (int i = 0; i < trackedPlayers.list.Count; i++)
		{
			if (!trackedPlayers.list[i].Dead)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			AIDirector.LogAI("Spawn {0}, no living players, wait 4 hours", new object[] { _spawnType });
			this.SetNextTime(_spawnType, this.Director.World.worldTime + 4000UL);
			return;
		}
		List<AIDirectorPlayerState> list = new List<AIDirectorPlayerState>();
		Vector3 vector;
		Vector3 vector2;
		Vector3 vector3;
		uint num = base.FindTargets(out vector, out vector2, out vector3, list);
		if (num > 0U)
		{
			AIDirector.LogAI("Spawn {0}, find targets, wait {1} hours", new object[] { _spawnType, num });
			this.SetNextTime(_spawnType, this.Director.World.worldTime + (ulong)(1000U * num));
			return;
		}
		this.ChooseNextTime(_spawnType);
		this.spawners.Add(new AIWanderingHordeSpawner(this.Director, _spawnType, null, list, this.Director.World.worldTime + 12000UL, vector, vector2, vector3));
	}

	// Token: 0x06002098 RID: 8344 RVA: 0x000C4700 File Offset: 0x000C2900
	[PublicizedFrom(EAccessModifier.Private)]
	public void CleanupType(AIWanderingHordeSpawner.SpawnType _spawnType)
	{
		for (int i = this.spawners.Count - 1; i >= 0; i--)
		{
			AIWanderingHordeSpawner aiwanderingHordeSpawner = this.spawners[i];
			if (aiwanderingHordeSpawner.spawnType == _spawnType)
			{
				aiwanderingHordeSpawner.Cleanup();
				this.spawners.RemoveAt(i);
			}
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06002099 RID: 8345 RVA: 0x000C474D File Offset: 0x000C294D
	public bool HasAnySpawns
	{
		get
		{
			return this.spawners.Count != 0;
		}
	}

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x0600209A RID: 8346 RVA: 0x000C475D File Offset: 0x000C295D
	public bool OtherHordesAreActive
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.IsBloodMoonVisible() || this.Director.GetComponent<AIDirectorChunkEventComponent>().HasAnySpawns;
		}
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x000C4778 File Offset: 0x000C2978
	[PublicizedFrom(EAccessModifier.Private)]
	public void ChooseNextTime(AIWanderingHordeSpawner.SpawnType _spawnType)
	{
		if (_spawnType == AIWanderingHordeSpawner.SpawnType.Bandits)
		{
			this.BanditNextTime = this.Director.World.worldTime + (ulong)((long)base.Random.RandomRange(12000, 24000));
			this.BanditNextTime += 2000UL;
			return;
		}
		if (_spawnType == AIWanderingHordeSpawner.SpawnType.Horde)
		{
			this.HordeNextTime = this.Director.World.worldTime + (ulong)((long)base.Random.RandomRange(12000, 24000));
		}
	}

	// Token: 0x0600209C RID: 8348 RVA: 0x000C47FA File Offset: 0x000C29FA
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetNextTime(AIWanderingHordeSpawner.SpawnType _spawnType, ulong _time)
	{
		if (_spawnType == AIWanderingHordeSpawner.SpawnType.Bandits)
		{
			this.BanditNextTime = _time;
			return;
		}
		if (_spawnType == AIWanderingHordeSpawner.SpawnType.Horde)
		{
			this.HordeNextTime = _time;
		}
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x000C4812 File Offset: 0x000C2A12
	public void LogTimes()
	{
		AIDirector.LogAI("Next wandering - bandit {0}, horde {1}", new object[]
		{
			GameUtils.WorldTimeToString(this.BanditNextTime),
			GameUtils.WorldTimeToString(this.HordeNextTime)
		});
	}

	// Token: 0x04001642 RID: 5698
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cNextHourMin = 7;

	// Token: 0x04001643 RID: 5699
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPlaytest;

	// Token: 0x04001644 RID: 5700
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIWanderingHordeSpawner> spawners = new List<AIWanderingHordeSpawner>();

	// Token: 0x04001645 RID: 5701
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong BanditNextTime;

	// Token: 0x04001646 RID: 5702
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong HordeNextTime;
}
