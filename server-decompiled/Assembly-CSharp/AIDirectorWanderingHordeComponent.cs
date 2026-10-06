using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000420 RID: 1056
[Preserve]
public class AIDirectorWanderingHordeComponent : AIDirectorHordeComponent
{
	// Token: 0x06002080 RID: 8320 RVA: 0x000C44B7 File Offset: 0x000C26B7
	public override void InitNewGame()
	{
		this.isPlaytest = GameUtils.IsPlaytesting();
		this.BanditNextTime = 0UL;
		this.HordeNextTime = 0UL;
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x000C44D4 File Offset: 0x000C26D4
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

	// Token: 0x06002082 RID: 8322 RVA: 0x000C44FC File Offset: 0x000C26FC
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

	// Token: 0x06002083 RID: 8323 RVA: 0x000C4574 File Offset: 0x000C2774
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

	// Token: 0x06002084 RID: 8324 RVA: 0x000C4620 File Offset: 0x000C2820
	public override void Read(PooledBinaryReader _stream, int _version)
	{
		base.Read(_stream, _version);
		this.HordeNextTime = _stream.ReadUInt64();
		if (_version > 3)
		{
			this.BanditNextTime = _stream.ReadUInt64();
		}
	}

	// Token: 0x06002085 RID: 8325 RVA: 0x000C4646 File Offset: 0x000C2846
	public override void Write(PooledBinaryWriter _stream)
	{
		base.Write(_stream);
		_stream.Write(this.HordeNextTime);
		_stream.Write(this.BanditNextTime);
	}

	// Token: 0x06002086 RID: 8326 RVA: 0x000C4668 File Offset: 0x000C2868
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

	// Token: 0x06002087 RID: 8327 RVA: 0x000C47B4 File Offset: 0x000C29B4
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

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06002088 RID: 8328 RVA: 0x000C4801 File Offset: 0x000C2A01
	public bool HasAnySpawns
	{
		get
		{
			return this.spawners.Count != 0;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06002089 RID: 8329 RVA: 0x000C4811 File Offset: 0x000C2A11
	public bool OtherHordesAreActive
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.IsBloodMoonVisible() || this.Director.GetComponent<AIDirectorChunkEventComponent>().HasAnySpawns;
		}
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x000C482C File Offset: 0x000C2A2C
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

	// Token: 0x0600208B RID: 8331 RVA: 0x000C48AE File Offset: 0x000C2AAE
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

	// Token: 0x0600208C RID: 8332 RVA: 0x000C48C6 File Offset: 0x000C2AC6
	public void LogTimes()
	{
		AIDirector.LogAI("Next wandering - bandit {0}, horde {1}", new object[]
		{
			GameUtils.WorldTimeToString(this.BanditNextTime),
			GameUtils.WorldTimeToString(this.HordeNextTime)
		});
	}

	// Token: 0x04001639 RID: 5689
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cNextHourMin = 7;

	// Token: 0x0400163A RID: 5690
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPlaytest;

	// Token: 0x0400163B RID: 5691
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIWanderingHordeSpawner> spawners = new List<AIWanderingHordeSpawner>();

	// Token: 0x0400163C RID: 5692
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong BanditNextTime;

	// Token: 0x0400163D RID: 5693
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong HordeNextTime;
}
