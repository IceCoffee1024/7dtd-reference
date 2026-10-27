using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000425 RID: 1061
[Preserve]
public class AIScoutHordeSpawner
{
	// Token: 0x0600209C RID: 8348 RVA: 0x000C4FEB File Offset: 0x000C31EB
	public AIScoutHordeSpawner(EntitySpawner _spawner, Vector3 _startPos, Vector3 _endPos, bool _isBloodMoon)
	{
		this.spawner = _spawner;
		this.startPos = _startPos;
		this.endPos = _endPos;
		this.isBloodMoon = _isBloodMoon;
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x000C5026 File Offset: 0x000C3226
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

	// Token: 0x0600209E RID: 8350 RVA: 0x000C5058 File Offset: 0x000C3258
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

	// Token: 0x0600209F RID: 8351 RVA: 0x000C51E4 File Offset: 0x000C33E4
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

	// Token: 0x060020A0 RID: 8352 RVA: 0x000C5470 File Offset: 0x000C3670
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

	// Token: 0x060020A1 RID: 8353 RVA: 0x000C54C4 File Offset: 0x000C36C4
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

	// Token: 0x060020A2 RID: 8354 RVA: 0x000C55D8 File Offset: 0x000C37D8
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 CalcRandomPos(AIDirector director, Vector3 target, float radius)
	{
		Vector2 vector = director.random.RandomOnUnitCircle * radius;
		return target + new Vector3(vector.x, 0f, vector.y);
	}

	// Token: 0x04001658 RID: 5720
	[PublicizedFrom(EAccessModifier.Private)]
	public EntitySpawner spawner;

	// Token: 0x04001659 RID: 5721
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 startPos;

	// Token: 0x0400165A RID: 5722
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 endPos;

	// Token: 0x0400165B RID: 5723
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBloodMoon;

	// Token: 0x0400165C RID: 5724
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> spawnedList = new List<Entity>();

	// Token: 0x0400165D RID: 5725
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIScoutHordeSpawner.ZombieCommand> hordeList = new List<AIScoutHordeSpawner.ZombieCommand>();

	// Token: 0x02000426 RID: 1062
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class ZombieCommand
	{
		// Token: 0x0400165E RID: 5726
		public EntityEnemy Zombie;

		// Token: 0x0400165F RID: 5727
		public ulong WorldExpiryTime;

		// Token: 0x04001660 RID: 5728
		public Vector3 TargetPos;

		// Token: 0x04001661 RID: 5729
		public bool Wandering;

		// Token: 0x04001662 RID: 5730
		public bool Attacking;

		// Token: 0x04001663 RID: 5731
		public float AttackDelay;

		// Token: 0x04001664 RID: 5732
		public AIScoutHordeSpawner.IHorde Horde;
	}

	// Token: 0x02000427 RID: 1063
	public interface IHorde
	{
		// Token: 0x060020A4 RID: 8356
		void SpawnMore(int size);

		// Token: 0x060020A5 RID: 8357
		void SetSpawnPos(Vector3 pos);

		// Token: 0x060020A6 RID: 8358
		void Destroy();

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060020A7 RID: 8359
		bool canSpawnMore { get; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060020A8 RID: 8360
		bool isSpawning { get; }
	}
}
