using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000423 RID: 1059
[Preserve]
public class AIScoutHordeSpawner
{
	// Token: 0x06002097 RID: 8343 RVA: 0x000C4C3F File Offset: 0x000C2E3F
	public AIScoutHordeSpawner(EntitySpawner _spawner, Vector3 _startPos, Vector3 _endPos, bool _isBloodMoon)
	{
		this.spawner = _spawner;
		this.startPos = _startPos;
		this.endPos = _endPos;
		this.isBloodMoon = _isBloodMoon;
	}

	// Token: 0x06002098 RID: 8344 RVA: 0x000C4C7A File Offset: 0x000C2E7A
	public bool Update(World world, float dt)
	{
		if (world.GetPlayers().Count == 0)
		{
			return true;
		}
		if (this.SpawnUpdate(world) && this.hordeList.Count == 0)
		{
			return true;
		}
		this.UpdateHorde(world, dt);
		return false;
	}

	// Token: 0x06002099 RID: 8345 RVA: 0x000C4CAC File Offset: 0x000C2EAC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool SpawnUpdate(World world)
	{
		if (!AIDirector.CanSpawn(1f) || this.spawner.CurrentWave > 0)
		{
			return true;
		}
		this.spawner.SpawnManually(world, GameUtils.WorldTimeToDays(world.worldTime), true, delegate(EntitySpawner _es, out EntityPlayer _outPlayerToAttack)
		{
			_outPlayerToAttack = null;
			return true;
		}, delegate(EntitySpawner _es, EntityPlayer _inPlayerToAttack, out EntityPlayer _outPlayerToAttack, out Vector3 _pos)
		{
			_outPlayerToAttack = null;
			return world.GetMobRandomSpawnPosWithWater(this.startPos, 0, 8, 10, true, out _pos);
		}, null, this.spawnedList);
		for (int i = 0; i < this.spawnedList.Count; i++)
		{
			EntityEnemy entityEnemy = this.spawnedList[i] as EntityEnemy;
			if (entityEnemy != null)
			{
				entityEnemy.IsHordeZombie = true;
				entityEnemy.IsScoutZombie = true;
				entityEnemy.IsBloodMoon = this.isBloodMoon;
				entityEnemy.bIsChunkObserver = true;
				AIScoutHordeSpawner.ZombieCommand zombieCommand = new AIScoutHordeSpawner.ZombieCommand();
				zombieCommand.Zombie = entityEnemy;
				zombieCommand.TargetPos = AIScoutHordeSpawner.CalcRandomPos(world.aiDirector, this.endPos, 6f);
				zombieCommand.Wandering = false;
				zombieCommand.AttackDelay = 2f;
				entityEnemy.SetInvestigatePosition(zombieCommand.TargetPos, 6000, true);
				this.hordeList.Add(zombieCommand);
				string text = "scout horde spawned '";
				EntityEnemy entityEnemy2 = entityEnemy;
				AIDirector.LogAI(text + ((entityEnemy2 != null) ? entityEnemy2.ToString() : null) + "'. Moving to point of interest", Array.Empty<object>());
			}
		}
		this.spawnedList.Clear();
		return this.spawner.CurrentWave > 0;
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x000C4E38 File Offset: 0x000C3038
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateHorde(World world, float deltaTime)
	{
		int i = 0;
		while (i < this.hordeList.Count)
		{
			bool flag = false;
			AIScoutHordeSpawner.ZombieCommand zombieCommand = this.hordeList[i];
			EntityEnemy zombie = zombieCommand.Zombie;
			if (zombie.IsDead())
			{
				flag = true;
			}
			else
			{
				EntityAlive attackTarget = zombie.GetAttackTarget();
				bool flag2 = attackTarget is EntityPlayer;
				if (zombieCommand.Horde != null)
				{
					if (attackTarget && !attackTarget.IsDead() && flag2)
					{
						zombieCommand.Horde.SetSpawnPos(attackTarget.GetPosition());
					}
					else
					{
						zombieCommand.Horde.SetSpawnPos(zombie.GetPosition());
					}
				}
				if (zombieCommand.Attacking)
				{
					if (!zombieCommand.Zombie.HasInvestigatePosition && (attackTarget == null || attackTarget.IsDead() || !flag2))
					{
						zombieCommand.Wandering = true;
						zombieCommand.WorldExpiryTime = world.worldTime + 2000UL;
					}
					else
					{
						zombieCommand.AttackDelay -= deltaTime;
						if (zombieCommand.AttackDelay <= 0f && zombie.bodyDamage.CurrentStun == EnumEntityStunType.None)
						{
							if (zombie.HasInvestigatePosition || (flag2 && !attackTarget.IsDead()))
							{
								Vector3 vector = (attackTarget ? attackTarget.GetPosition() : zombieCommand.Zombie.InvestigatePosition);
								if (this.spawnHordeNear(world, zombieCommand, vector))
								{
									zombieCommand.AttackDelay = 18f;
								}
								else
								{
									flag = true;
								}
							}
							else
							{
								zombieCommand.Wandering = true;
								zombieCommand.WorldExpiryTime = world.worldTime + 2000UL;
							}
						}
					}
				}
				else if (attackTarget)
				{
					if (flag2)
					{
						zombieCommand.Attacking = true;
					}
				}
				else if (zombieCommand.Wandering)
				{
					if (world.worldTime >= zombieCommand.WorldExpiryTime)
					{
						flag = true;
					}
				}
				else if (zombieCommand.Zombie.HasInvestigatePosition)
				{
					if (zombieCommand.Zombie.InvestigatePosition == zombieCommand.TargetPos)
					{
						zombieCommand.Zombie.SetInvestigatePosition(zombieCommand.TargetPos, 6000, true);
					}
				}
				else
				{
					zombieCommand.Wandering = true;
					zombieCommand.WorldExpiryTime = world.worldTime + 2000UL;
				}
			}
			if (flag)
			{
				if (zombieCommand.Horde != null)
				{
					zombieCommand.Horde.Destroy();
				}
				string text = "scout horde '";
				EntityEnemy zombie2 = zombieCommand.Zombie;
				AIDirector.LogAIExtra(text + ((zombie2 != null) ? zombie2.ToString() : null) + "' removed from control", Array.Empty<object>());
				zombieCommand.Zombie.IsHordeZombie = false;
				zombieCommand.Zombie.bIsChunkObserver = false;
				this.hordeList.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x000C50C4 File Offset: 0x000C32C4
	public void Cleanup()
	{
		for (int i = 0; i < this.hordeList.Count; i++)
		{
			AIScoutHordeSpawner.ZombieCommand zombieCommand = this.hordeList[i];
			zombieCommand.Zombie.IsHordeZombie = false;
			zombieCommand.Zombie.bIsChunkObserver = false;
		}
		this.hordeList.Clear();
	}

	// Token: 0x0600209C RID: 8348 RVA: 0x000C5118 File Offset: 0x000C3318
	[PublicizedFrom(EAccessModifier.Private)]
	public bool spawnHordeNear(World world, AIScoutHordeSpawner.ZombieCommand command, Vector3 target)
	{
		AIDirector.LogAI("Scout spawned a zombie horde", Array.Empty<object>());
		if (command.Horde == null)
		{
			AIDirectorChunkEventComponent component = world.GetAIDirector().GetComponent<AIDirectorChunkEventComponent>();
			command.Horde = component.CreateHorde(target);
		}
		if (command.Horde.canSpawnMore)
		{
			int num = 5;
			if (world.aiDirector.random.RandomFloat < 0.12f)
			{
				num--;
				if (this.spawner.CurrentWave > 0)
				{
					Vector3 vector = this.endPos;
					this.spawner.ResetSpawner();
					this.spawner.numberToSpawnThisWave = 1;
					this.endPos = target;
					this.SpawnUpdate(world);
					this.endPos = vector;
				}
				else
				{
					this.spawner.numberToSpawnThisWave++;
				}
			}
			command.Horde.SpawnMore(num);
			command.Zombie.PlayOneShot(command.Zombie.GetSoundAlert(), false, false, false, null, 1f);
		}
		command.Horde.SetSpawnPos(target);
		return command.Horde.canSpawnMore || command.Horde.isSpawning;
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x000C522C File Offset: 0x000C342C
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 CalcRandomPos(AIDirector director, Vector3 target, float radius)
	{
		Vector2 vector = director.random.RandomOnUnitCircle * radius;
		return target + new Vector3(vector.x, 0f, vector.y);
	}

	// Token: 0x04001648 RID: 5704
	[PublicizedFrom(EAccessModifier.Private)]
	public EntitySpawner spawner;

	// Token: 0x04001649 RID: 5705
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startPos;

	// Token: 0x0400164A RID: 5706
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 endPos;

	// Token: 0x0400164B RID: 5707
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBloodMoon;

	// Token: 0x0400164C RID: 5708
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> spawnedList = new List<Entity>();

	// Token: 0x0400164D RID: 5709
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIScoutHordeSpawner.ZombieCommand> hordeList = new List<AIScoutHordeSpawner.ZombieCommand>();

	// Token: 0x02000424 RID: 1060
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class ZombieCommand
	{
		// Token: 0x0400164E RID: 5710
		public EntityEnemy Zombie;

		// Token: 0x0400164F RID: 5711
		public ulong WorldExpiryTime;

		// Token: 0x04001650 RID: 5712
		public Vector3 TargetPos;

		// Token: 0x04001651 RID: 5713
		public bool Wandering;

		// Token: 0x04001652 RID: 5714
		public bool Attacking;

		// Token: 0x04001653 RID: 5715
		public float AttackDelay;

		// Token: 0x04001654 RID: 5716
		public AIScoutHordeSpawner.IHorde Horde;
	}

	// Token: 0x02000425 RID: 1061
	public interface IHorde
	{
		// Token: 0x0600209F RID: 8351
		void SpawnMore(int size);

		// Token: 0x060020A0 RID: 8352
		void SetSpawnPos(Vector3 pos);

		// Token: 0x060020A1 RID: 8353
		void Destroy();

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060020A2 RID: 8354
		bool canSpawnMore { get; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060020A3 RID: 8355
		bool isSpawning { get; }
	}
}
