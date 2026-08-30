using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000422 RID: 1058
[Preserve]
public class AIWanderingHordeSpawner
{
	// Token: 0x0600209D RID: 8349 RVA: 0x000C4A14 File Offset: 0x000C2C14
	public AIWanderingHordeSpawner(AIDirector _director, AIWanderingHordeSpawner.SpawnType _spawnType, AIWanderingHordeSpawner.HordeArrivedDelegate _arrivedEvent, List<AIDirectorPlayerState> _targets, ulong _endTime, Vector3 _startPos, Vector3 _pitStopPos, Vector3 _endPos)
	{
		this.director = _director;
		this.startPos = _startPos;
		this.pitStopPos = _pitStopPos;
		this.endPos = _endPos;
		this.endTime = _endTime;
		this.arrivedCallback = _arrivedEvent;
		this.spawnType = _spawnType;
		AIWanderingHordeSpawner.SpawnType spawnType = this.spawnType;
		string text;
		int num;
		if (spawnType != AIWanderingHordeSpawner.SpawnType.Bandits)
		{
			if (spawnType != AIWanderingHordeSpawner.SpawnType.Horde)
			{
			}
			text = "WanderingHorde";
			num = 50;
		}
		else
		{
			text = "WanderingBandits";
			num = 0;
		}
		this.spawner = new AIDirectorGameStagePartySpawner(_director.World, text);
		for (int i = 0; i < _targets.Count; i++)
		{
			this.spawner.AddMember(_targets[i].Player);
		}
		this.spawner.ResetPartyLevel(num);
		this.spawner.ClearMembers();
	}

	// Token: 0x0600209E RID: 8350 RVA: 0x000C4AE0 File Offset: 0x000C2CE0
	public bool Update(World world, float _deltaTime)
	{
		if (world.GetPlayers().Count == 0)
		{
			return true;
		}
		if (world.worldTime >= this.endTime)
		{
			if (this.arrivedCallback != null)
			{
				this.arrivedCallback();
			}
			return true;
		}
		bool flag = this.UpdateSpawn(world, _deltaTime);
		if (flag && this.commandList.Count == 0)
		{
			if (this.arrivedCallback != null)
			{
				this.arrivedCallback();
			}
			return true;
		}
		if (!flag)
		{
			AstarManager.Instance.AddLocationLine(this.startPos, this.endPos, 64);
		}
		else
		{
			Vector3 vector = Vector3.zero;
			int num = 0;
			for (int i = 0; i < this.commandList.Count; i++)
			{
				Entity enemy = this.commandList[i].Enemy;
				if (!enemy.IsDead())
				{
					vector += enemy.position;
					num++;
				}
			}
			if (num > 0)
			{
				vector *= 1f / (float)num;
				AstarManager.Instance.AddLocation(vector, 64);
			}
		}
		this.UpdateHorde(_deltaTime);
		return false;
	}

	// Token: 0x0600209F RID: 8351 RVA: 0x000C4BE0 File Offset: 0x000C2DE0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool UpdateSpawn(World _world, float _deltaTime)
	{
		if (!AIDirector.CanSpawn(1f))
		{
			return true;
		}
		if (!this.spawner.Tick((double)_deltaTime))
		{
			return true;
		}
		this.spawnDelay -= _deltaTime;
		if (this.spawnDelay >= 0f)
		{
			return false;
		}
		this.spawnDelay = 1f;
		if (!this.spawner.canSpawn)
		{
			return false;
		}
		Vector3 vector;
		if (!_world.GetMobRandomSpawnPosWithWater(this.startPos, 1, 6, 15, true, out vector))
		{
			return false;
		}
		EntityEnemy entityEnemy = (EntityEnemy)EntityFactory.CreateEntity(EntityGroups.GetRandomFromGroup(this.spawner.spawnGroupName, ref this.lastClassId, null), vector);
		entityEnemy.SetSpawnerSource(EnumSpawnerSource.Dynamic);
		_world.SpawnEntityInWorld(entityEnemy);
		entityEnemy.IsHordeZombie = true;
		entityEnemy.bIsChunkObserver = true;
		entityEnemy.IsHordeZombie = true;
		entityEnemy.bIsChunkObserver = true;
		int num = this.bonusLootSpawnCount + 1;
		this.bonusLootSpawnCount = num;
		if (num >= GameStageDefinition.LootWanderingBonusEvery)
		{
			this.bonusLootSpawnCount = 0;
			entityEnemy.lootDropProb *= GameStageDefinition.LootWanderingBonusScale;
		}
		AIWanderingHordeSpawner.ZombieCommand zombieCommand = new AIWanderingHordeSpawner.ZombieCommand();
		zombieCommand.Enemy = entityEnemy;
		zombieCommand.TargetPos = AIWanderingHordeSpawner.RandomPos(this.director, this.endPos, 6f);
		zombieCommand.Command = AIWanderingHordeSpawner.ECommand.EndPos;
		this.commandList.Add(zombieCommand);
		entityEnemy.SetInvestigatePosition(zombieCommand.TargetPos, 6000, false);
		AIDirector.LogAI("Spawned wandering horde (group {0}, zombie {1})", new object[]
		{
			this.spawner.spawnGroupName,
			entityEnemy
		});
		this.spawner.IncSpawnCount();
		return false;
	}

	// Token: 0x060020A0 RID: 8352 RVA: 0x000C4D54 File Offset: 0x000C2F54
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateHorde(float dt)
	{
		int i = 0;
		while (i < this.commandList.Count)
		{
			AIWanderingHordeSpawner.ZombieCommand zombieCommand = this.commandList[i];
			bool flag = zombieCommand.Enemy.IsDead() || zombieCommand.Enemy.GetAttackTarget() != null;
			if (!flag)
			{
				if (zombieCommand.Command == AIWanderingHordeSpawner.ECommand.PitStop || zombieCommand.Command == AIWanderingHordeSpawner.ECommand.EndPos)
				{
					if (zombieCommand.Enemy.HasInvestigatePosition)
					{
						if (zombieCommand.Enemy.InvestigatePosition != zombieCommand.TargetPos)
						{
							flag = true;
							string text = "Wandering horde zombie '";
							EntityEnemy enemy = zombieCommand.Enemy;
							AIDirector.LogAIExtra(text + ((enemy != null) ? enemy.ToString() : null) + "' removed from horde control. Was killed or investigating", Array.Empty<object>());
						}
						else
						{
							zombieCommand.Enemy.SetInvestigatePosition(zombieCommand.TargetPos, 6000, false);
						}
					}
					else if (zombieCommand.Command == AIWanderingHordeSpawner.ECommand.PitStop)
					{
						string text2 = "Wandering horde zombie '";
						EntityEnemy enemy2 = zombieCommand.Enemy;
						AIDirector.LogAIExtra(text2 + ((enemy2 != null) ? enemy2.ToString() : null) + "' reached pitstop. Wander around for awhile", Array.Empty<object>());
						zombieCommand.WanderTime = 90f + this.director.random.RandomFloat * 4f;
						zombieCommand.Command = AIWanderingHordeSpawner.ECommand.Wander;
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					zombieCommand.WanderTime -= dt;
					zombieCommand.Enemy.ResetDespawnTime();
					if (zombieCommand.WanderTime <= 0f && zombieCommand.Enemy.GetAttackTarget() == null)
					{
						string text3 = "Wandering horde zombie '";
						EntityEnemy enemy3 = zombieCommand.Enemy;
						AIDirector.LogAIExtra(text3 + ((enemy3 != null) ? enemy3.ToString() : null) + "' wandered long enough. Going to endstop", Array.Empty<object>());
						zombieCommand.Command = AIWanderingHordeSpawner.ECommand.EndPos;
						zombieCommand.TargetPos = AIWanderingHordeSpawner.RandomPos(this.director, this.endPos, 6f);
						zombieCommand.Enemy.SetInvestigatePosition(zombieCommand.TargetPos, 6000, false);
						zombieCommand.Enemy.IsHordeZombie = false;
					}
				}
			}
			if (flag)
			{
				string text4 = "Wandering horde zombie '";
				EntityEnemy enemy4 = zombieCommand.Enemy;
				AIDirector.LogAIExtra(text4 + ((enemy4 != null) ? enemy4.ToString() : null) + "' removed from control", Array.Empty<object>());
				zombieCommand.Enemy.IsHordeZombie = false;
				zombieCommand.Enemy.bIsChunkObserver = false;
				this.commandList.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x000C4FA4 File Offset: 0x000C31A4
	public void Cleanup()
	{
		for (int i = 0; i < this.commandList.Count; i++)
		{
			AIWanderingHordeSpawner.ZombieCommand zombieCommand = this.commandList[i];
			zombieCommand.Enemy.IsHordeZombie = false;
			zombieCommand.Enemy.bIsChunkObserver = false;
		}
	}

	// Token: 0x060020A2 RID: 8354 RVA: 0x000C4FEC File Offset: 0x000C31EC
	public static Vector3 RandomPos(AIDirector director, Vector3 target, float radius)
	{
		Vector2 vector = director.random.RandomOnUnitCircle * radius;
		return target + new Vector3(vector.x, 0f, vector.y);
	}

	// Token: 0x0400164D RID: 5709
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cInvestigateTime = 6000;

	// Token: 0x0400164E RID: 5710
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirector director;

	// Token: 0x0400164F RID: 5711
	[PublicizedFrom(EAccessModifier.Private)]
	public AIWanderingHordeSpawner.HordeArrivedDelegate arrivedCallback;

	// Token: 0x04001650 RID: 5712
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startPos;

	// Token: 0x04001651 RID: 5713
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 pitStopPos;

	// Token: 0x04001652 RID: 5714
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 endPos;

	// Token: 0x04001653 RID: 5715
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong endTime;

	// Token: 0x04001654 RID: 5716
	public AIWanderingHordeSpawner.SpawnType spawnType;

	// Token: 0x04001655 RID: 5717
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorGameStagePartySpawner spawner;

	// Token: 0x04001656 RID: 5718
	[PublicizedFrom(EAccessModifier.Private)]
	public float spawnDelay;

	// Token: 0x04001657 RID: 5719
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastClassId;

	// Token: 0x04001658 RID: 5720
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIWanderingHordeSpawner.ZombieCommand> commandList = new List<AIWanderingHordeSpawner.ZombieCommand>();

	// Token: 0x04001659 RID: 5721
	[PublicizedFrom(EAccessModifier.Private)]
	public int bonusLootSpawnCount;

	// Token: 0x02000423 RID: 1059
	// (Invoke) Token: 0x060020A4 RID: 8356
	public delegate void HordeArrivedDelegate();

	// Token: 0x02000424 RID: 1060
	public enum SpawnType
	{
		// Token: 0x0400165B RID: 5723
		Bandits,
		// Token: 0x0400165C RID: 5724
		Horde
	}

	// Token: 0x02000425 RID: 1061
	[PublicizedFrom(EAccessModifier.Private)]
	public enum ECommand
	{
		// Token: 0x0400165E RID: 5726
		PitStop,
		// Token: 0x0400165F RID: 5727
		Wander,
		// Token: 0x04001660 RID: 5728
		EndPos
	}

	// Token: 0x02000426 RID: 1062
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class ZombieCommand
	{
		// Token: 0x04001661 RID: 5729
		public AIWanderingHordeSpawner.ECommand Command;

		// Token: 0x04001662 RID: 5730
		public EntityEnemy Enemy;

		// Token: 0x04001663 RID: 5731
		public float WanderTime;

		// Token: 0x04001664 RID: 5732
		public Vector3 TargetPos;
	}
}
