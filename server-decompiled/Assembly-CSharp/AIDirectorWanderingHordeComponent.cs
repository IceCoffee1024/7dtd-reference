using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000422 RID: 1058
[Preserve]
public class AIDirectorWanderingHordeComponent : AIDirectorHordeComponent
{
	// Token: 0x06002085 RID: 8325 RVA: 0x000C4863 File Offset: 0x000C2A63
	public override void InitNewGame()
	{
		this.isPlaytest = GameUtils.IsPlaytesting();
		this.BanditNextTime = 0UL;
		this.HordeNextTime = 0UL;
	}

	// Token: 0x06002086 RID: 8326 RVA: 0x000C4880 File Offset: 0x000C2A80
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

	// Token: 0x06002087 RID: 8327 RVA: 0x000C48A8 File Offset: 0x000C2AA8
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

	// Token: 0x06002088 RID: 8328 RVA: 0x000C4920 File Offset: 0x000C2B20
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

	// Token: 0x06002089 RID: 8329 RVA: 0x000C49CC File Offset: 0x000C2BCC
	public override void Read(PooledBinaryReader _stream, int _version)
	{
		base.Read(_stream, _version);
		this.HordeNextTime = _stream.ReadUInt64();
		if (_version > 3)
		{
			this.BanditNextTime = _stream.ReadUInt64();
		}
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x000C49F2 File Offset: 0x000C2BF2
	public override void Write(PooledBinaryWriter _stream)
	{
		base.Write(_stream);
		_stream.Write(this.HordeNextTime);
		_stream.Write(this.BanditNextTime);
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x000C4A14 File Offset: 0x000C2C14
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

	// Token: 0x0600208C RID: 8332 RVA: 0x000C4B60 File Offset: 0x000C2D60
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
	// (get) Token: 0x0600208D RID: 8333 RVA: 0x000C4BAD File Offset: 0x000C2DAD
	public bool HasAnySpawns
	{
		get
		{
			return this.spawners.Count != 0;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x0600208E RID: 8334 RVA: 0x000C4BBD File Offset: 0x000C2DBD
	public bool OtherHordesAreActive
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SkyManager.IsBloodMoonVisible() || this.Director.GetComponent<AIDirectorChunkEventComponent>().HasAnySpawns;
		}
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x000C4BD8 File Offset: 0x000C2DD8
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

	// Token: 0x06002090 RID: 8336 RVA: 0x000C4C5A File Offset: 0x000C2E5A
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

	// Token: 0x06002091 RID: 8337 RVA: 0x000C4C72 File Offset: 0x000C2E72
	public void LogTimes()
	{
		AIDirector.LogAI("Next wandering - bandit {0}, horde {1}", new object[]
		{
			GameUtils.WorldTimeToString(this.BanditNextTime),
			GameUtils.WorldTimeToString(this.HordeNextTime)
		});
	}

	// Token: 0x04001649 RID: 5705
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cNextHourMin = 7;

	// Token: 0x0400164A RID: 5706
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPlaytest;

	// Token: 0x0400164B RID: 5707
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIWanderingHordeSpawner> spawners = new List<AIWanderingHordeSpawner>();

	// Token: 0x0400164C RID: 5708
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong BanditNextTime;

	// Token: 0x0400164D RID: 5709
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong HordeNextTime;
}
