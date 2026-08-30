using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000404 RID: 1028
[Preserve]
public class AIDirectorBloodMoonParty
{
	// Token: 0x06001FDA RID: 8154 RVA: 0x000C109C File Offset: 0x000BF29C
	public AIDirectorBloodMoonParty(EntityPlayer _initialPlayer, AIDirectorBloodMoonComponent _controller, int _bloodMoonCountUNUSED)
	{
		this.spawnWorld = _initialPlayer.world;
		this.spawnBasePos = _initialPlayer.position;
		this.controller = _controller;
		this.partySpawner = new AIDirectorGameStagePartySpawner(_controller.Director.World, "BloodMoonHorde");
		this.partySpawner.AddMember(_initialPlayer);
		_initialPlayer.bloodMoonParty = this;
		this.spawnBaseDir = _controller.Random.RandomRange(0, 360);
		this.groupIndex = -1;
	}

	// Token: 0x06001FDB RID: 8155 RVA: 0x000C1128 File Offset: 0x000BF328
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcBestDir(Vector3 basePos)
	{
		int[] array = new int[16];
		int num = 0;
		for (int i = 0; i < 16; i++)
		{
			float num2 = (float)i * 22.5f;
			this.spawnDirectionV = Quaternion.AngleAxis(num2, Vector3.up) * Vector3.forward * 40f;
			int num3 = 0;
			for (int j = 0; j < 9; j++)
			{
				Vector3 vector;
				if (this.spawnWorld.GetRandomSpawnPositionMinMaxToPosition(basePos + this.spawnDirectionV, 0, 10, 30, false, out vector, -1, true, 30, false, EnumLandClaimOwner.None, false))
				{
					num3++;
				}
			}
			if (num3 > 0)
			{
				num3 = (num3 + 2) / 3;
				if (Utils.FastAbs(Mathf.DeltaAngle(num2, (float)this.spawnBaseDir)) <= 60f)
				{
					num3 *= 3;
				}
			}
			array[i] = num3;
			num = Utils.FastMax(num, num3);
		}
		int num4 = 0;
		for (int k = 0; k < 16; k++)
		{
			if (array[k] == num)
			{
				num4++;
			}
		}
		int num5 = 0;
		int num6 = this.controller.Random.RandomRange(0, num4);
		for (int l = 0; l < 16; l++)
		{
			if (array[l] >= num && --num6 < 0)
			{
				num5 = l;
				break;
			}
		}
		this.spawnDirectionV = Quaternion.AngleAxis((float)num5 * 22.5f, Vector3.up) * Vector3.forward * 40f;
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x000C128C File Offset: 0x000BF48C
	public bool Tick(World _world, double _dt, bool _canSpawn)
	{
		if (this.partySpawner.partyLevel < 0)
		{
			this.InitParty();
		}
		for (int i = this.zombies.Count - 1; i >= 0; i--)
		{
			AIDirectorBloodMoonParty.ManagedZombie managedZombie = this.zombies[i];
			managedZombie.updateDelay -= (float)_dt;
			if (managedZombie.updateDelay <= 0f)
			{
				managedZombie.updateDelay = 1.8f;
				if (!this.SeekTarget(managedZombie))
				{
					this.zombies.RemoveAt(i);
				}
			}
		}
		this.partySpawner.Tick(_dt);
		bool flag = false;
		if (_canSpawn)
		{
			if (!this.partySpawner.canSpawn || this.partySpawner.partyMembers.Count == 0)
			{
				return true;
			}
			if (AIDirector.CanSpawn(1.9f))
			{
				int num = this.partySpawner.groupIndex;
				if (num != this.groupIndex)
				{
					this.groupIndex = num;
					this.spawnBaseDir += 120;
					this.CalcBestDir(this.spawnBasePos);
				}
				flag = true;
				int count = this.partySpawner.partyMembers.Count;
				int num2 = Utils.FastMin(this.partySpawner.maxAlive, this.enemyActiveMax);
				if (this.zombies.Count < num2)
				{
					for (int j = Utils.FastMin(count, 3); j > 0; j--)
					{
						if (this.nextPlayer >= count)
						{
							this.nextPlayer = 0;
						}
						EntityPlayer entityPlayer = this.partySpawner.partyMembers[this.nextPlayer];
						bool flag2 = false;
						if (this.IsPlayerATarget(entityPlayer))
						{
							flag2 = this.SpawnZombie(_world, entityPlayer, entityPlayer.position, this.spawnDirectionV);
						}
						this.nextPlayer++;
						if (flag2)
						{
							break;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x000C143D File Offset: 0x000BF63D
	public void PlayerLoggedOut(EntityPlayer _player)
	{
		this.partySpawner.RemoveMember(_player, false);
		if (this.nextPlayer >= this.partySpawner.partyMembers.Count)
		{
			this.nextPlayer = 0;
		}
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x000C146C File Offset: 0x000BF66C
	public void KillPartyZombies()
	{
		int count = this.zombies.Count;
		if (count > 0)
		{
			this.partySpawner.DecSpawnCount(count);
			for (int i = 0; i < count; i++)
			{
				EntityEnemy zombie = this.zombies[i].zombie;
				if (zombie && !zombie.IsDead() && !zombie.IsDespawned && zombie.gameObject)
				{
					zombie.Kill(DamageResponse.New(true));
				}
			}
			this.zombies.Clear();
		}
	}

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06001FDF RID: 8159 RVA: 0x000C14EF File Offset: 0x000BF6EF
	public bool IsEmpty
	{
		get
		{
			return this.partySpawner.partyMembers.Count <= 0;
		}
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x000C1507 File Offset: 0x000BF707
	public bool IsMemberOfParty(int _entityID)
	{
		return this.partySpawner.IsMemberOfParty(_entityID);
	}

	// Token: 0x06001FE1 RID: 8161 RVA: 0x000C1518 File Offset: 0x000BF718
	public bool TryAddPlayer(EntityPlayer _player)
	{
		for (int i = 0; i < this.partySpawner.partyMembers.Count; i++)
		{
			if ((this.partySpawner.partyMembers[i].GetPosition() - _player.GetPosition()).sqrMagnitude <= 6400f)
			{
				this.AddPlayer(_player);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001FE2 RID: 8162 RVA: 0x000C157A File Offset: 0x000BF77A
	public void AddPlayer(EntityPlayer _player)
	{
		this.partySpawner.AddMember(_player);
		_player.bloodMoonParty = this;
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x000C1590 File Offset: 0x000BF790
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitParty()
	{
		int num = this.partySpawner.CalcPartyLevel();
		int num2 = AIDirectorBloodMoonComponent.BloodMoonEnemyCount * this.partySpawner.partyMembers.Count;
		this.enemyActiveMax = Utils.FastMin(30, num2);
		float num3 = Utils.FastMax(1f, (float)num2 / (float)this.enemyActiveMax);
		num3 = Utils.FastLerp(1f, num3, (float)num / 60f);
		this.partySpawner.SetScaling(num3);
		this.partySpawner.SetPartyLevel(num);
		this.bonusLootSpawnCount = this.partySpawner.bonusLootEvery / 2;
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x000C1624 File Offset: 0x000BF824
	[PublicizedFrom(EAccessModifier.Private)]
	public bool SpawnZombie(World _world, EntityPlayer _target, Vector3 _focusPos, Vector3 _radiusV)
	{
		Vector3 vector;
		if (!this.CalcSpawnPos(_world, _focusPos, _radiusV, out vector))
		{
			return false;
		}
		bool flag = true;
		int num = EntityGroups.GetRandomFromGroup(this.partySpawner.spawnGroupName, ref this.lastClassId, null);
		if (_target.AttachedToEntity && this.controller.Random.RandomFloat < 0.5f)
		{
			flag = false;
			num = EntityClass.FromString("animalZombieVultureRadiated");
		}
		EntityEnemy entityEnemy = (EntityEnemy)EntityFactory.CreateEntity(num, vector);
		entityEnemy.SetSpawnerSource(EnumSpawnerSource.Dynamic);
		_world.SpawnEntityInWorld(entityEnemy);
		entityEnemy.IsHordeZombie = true;
		entityEnemy.IsBloodMoon = true;
		entityEnemy.bIsChunkObserver = true;
		entityEnemy.timeStayAfterDeath /= 3;
		if (flag)
		{
			int num2 = this.bonusLootSpawnCount + 1;
			this.bonusLootSpawnCount = num2;
			if (num2 >= this.partySpawner.bonusLootEvery)
			{
				this.bonusLootSpawnCount = 0;
				entityEnemy.lootDropProb *= GameStageDefinition.LootBonusScale;
			}
		}
		AIDirectorBloodMoonParty.ManagedZombie managedZombie = new AIDirectorBloodMoonParty.ManagedZombie(entityEnemy, _target);
		this.zombies.Add(managedZombie);
		this.SeekTarget(managedZombie);
		this.partySpawner.IncSpawnCount();
		AstarManager.Instance.AddLocation(vector, 40);
		ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(_world.worldTime);
		int item = valueTuple.Item1;
		int item2 = valueTuple.Item2;
		int item3 = valueTuple.Item3;
		Log.Out("BloodMoonParty: SpawnZombie grp {0}, cnt {1}, {2}, loot {3}, at player {4}, day/time {5} {6:D2}:{7:D2}", new object[]
		{
			this.partySpawner.ToString(),
			this.zombies.Count,
			entityEnemy.EntityName,
			entityEnemy.lootDropProb,
			_target.entityId,
			item,
			item2,
			item3
		});
		return true;
	}

	// Token: 0x06001FE5 RID: 8165 RVA: 0x000C17D8 File Offset: 0x000BF9D8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CalcSpawnPos(World _world, Vector3 _focusPos, Vector3 _radiusV, out Vector3 spawnPos)
	{
		_radiusV = Quaternion.AngleAxis((this.controller.Random.RandomFloat - 0.5f) * 90f, Vector3.up) * _radiusV;
		return _world.GetMobRandomSpawnPosWithWater(_focusPos + _radiusV, 0, 10, 30, false, out spawnPos);
	}

	// Token: 0x06001FE6 RID: 8166 RVA: 0x000C1830 File Offset: 0x000BFA30
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer FindPartyTarget(Vector3 fromPos)
	{
		float num = float.MaxValue;
		EntityPlayer entityPlayer = null;
		for (int i = this.partySpawner.partyMembers.Count - 1; i >= 0; i--)
		{
			EntityPlayer entityPlayer2 = this.partySpawner.partyMembers[i];
			if (this.IsPlayerATarget(entityPlayer2))
			{
				float sqrMagnitude = (fromPos - entityPlayer2.position).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					entityPlayer = entityPlayer2;
				}
			}
		}
		return entityPlayer;
	}

	// Token: 0x06001FE7 RID: 8167 RVA: 0x000C18A0 File Offset: 0x000BFAA0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool SeekTarget(AIDirectorBloodMoonParty.ManagedZombie mz)
	{
		EntityAlive zombie = mz.zombie;
		if (!zombie || zombie.IsDead() || zombie.IsDespawned || !zombie.gameObject)
		{
			return false;
		}
		EntityPlayer entityPlayer = zombie.GetAttackTarget() as EntityPlayer;
		if (entityPlayer)
		{
			mz.player = entityPlayer;
		}
		if (!mz.player || !this.IsPlayerATarget(mz.player))
		{
			mz.player = this.FindPartyTarget(zombie.position);
		}
		if (mz.player)
		{
			Vector3 vector = zombie.position - mz.player.position;
			float sqrMagnitude = vector.sqrMagnitude;
			vector.y = 0f;
			Vector3 vector2;
			if (vector.sqrMagnitude >= 22500f && this.CalcSpawnPos(zombie.world, mz.player.position, this.spawnDirectionV, out vector2) && !zombie.world.IsPlayerAliveAndNear(zombie.position, 70f))
			{
				if (this.controller.Random.RandomFloat < 0.5f)
				{
					this.partySpawner.DecSpawnCount(1);
					zombie.lootDropProb = 0f;
					zombie.Kill(DamageResponse.New(true));
					return false;
				}
				zombie.SetPosition(vector2, true);
				zombie.moveHelper.Stop();
				Log.Warning("SeekTarget {0}, far, move {1}", new object[]
				{
					zombie.GetDebugName(),
					vector2
				});
			}
			if (sqrMagnitude <= 10000f || entityPlayer != mz.player)
			{
				zombie.SetAttackTarget(mz.player, 1200);
			}
			else
			{
				if (entityPlayer)
				{
					zombie.SetAttackTarget(null, 0);
				}
				zombie.SetInvestigatePosition(mz.player.position, 1200, true);
			}
			return true;
		}
		if (!zombie.world.IsPlayerAliveAndNear(zombie.position, 60f))
		{
			zombie.Kill(DamageResponse.New(true));
			return false;
		}
		return true;
	}

	// Token: 0x06001FE8 RID: 8168 RVA: 0x000C1A94 File Offset: 0x000BFC94
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsPlayerATarget(EntityPlayer player)
	{
		return !player.IsDead() && player.IsSpawned() && player.entityId != -1 && !player.IsIgnoredByAI() && player.Progression.Level > 1 && !player.IsBloodMoonDead;
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06001FE9 RID: 8169 RVA: 0x000C1AD4 File Offset: 0x000BFCD4
	public bool BloodmoonZombiesRemain
	{
		get
		{
			return this.zombies.Count > 0;
		}
	}

	// Token: 0x04001594 RID: 5524
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cPartyJoinDistance = 80f;

	// Token: 0x04001595 RID: 5525
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cPartyJoinDistanceSq = 6400f;

	// Token: 0x04001596 RID: 5526
	public const float cSightDist = 100f;

	// Token: 0x04001597 RID: 5527
	public const float cSightDistSq = 10000f;

	// Token: 0x04001598 RID: 5528
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTeleportDist = 150f;

	// Token: 0x04001599 RID: 5529
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cTeleportDistSq = 22500f;

	// Token: 0x0400159A RID: 5530
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSpawnPreferredArc = 120;

	// Token: 0x0400159B RID: 5531
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSpawnAngle = 90f;

	// Token: 0x0400159C RID: 5532
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSpawnDistance = 40f;

	// Token: 0x0400159D RID: 5533
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSpawnMinRandDistance = 0;

	// Token: 0x0400159E RID: 5534
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSpawnMaxRandDistance = 10;

	// Token: 0x0400159F RID: 5535
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSpawnMinPlayerDistance = 30;

	// Token: 0x040015A0 RID: 5536
	public AIDirectorGameStagePartySpawner partySpawner;

	// Token: 0x040015A1 RID: 5537
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastClassId;

	// Token: 0x040015A2 RID: 5538
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorBloodMoonParty.ManagedZombie> zombies = new List<AIDirectorBloodMoonParty.ManagedZombie>();

	// Token: 0x040015A3 RID: 5539
	[PublicizedFrom(EAccessModifier.Private)]
	public World spawnWorld;

	// Token: 0x040015A4 RID: 5540
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 spawnBasePos;

	// Token: 0x040015A5 RID: 5541
	[PublicizedFrom(EAccessModifier.Private)]
	public int spawnBaseDir;

	// Token: 0x040015A6 RID: 5542
	[PublicizedFrom(EAccessModifier.Private)]
	public int enemyActiveMax;

	// Token: 0x040015A7 RID: 5543
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorBloodMoonComponent controller;

	// Token: 0x040015A8 RID: 5544
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 spawnDirectionV;

	// Token: 0x040015A9 RID: 5545
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextPlayer;

	// Token: 0x040015AA RID: 5546
	[PublicizedFrom(EAccessModifier.Private)]
	public int groupIndex;

	// Token: 0x040015AB RID: 5547
	[PublicizedFrom(EAccessModifier.Private)]
	public int bonusLootSpawnCount;

	// Token: 0x02000405 RID: 1029
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class ManagedZombie
	{
		// Token: 0x06001FEA RID: 8170 RVA: 0x000C1AE4 File Offset: 0x000BFCE4
		public ManagedZombie(EntityEnemy _zombie, EntityPlayer _player)
		{
			this.zombie = _zombie;
			this.player = _player;
		}

		// Token: 0x040015AC RID: 5548
		public EntityPlayer player;

		// Token: 0x040015AD RID: 5549
		public EntityEnemy zombie;

		// Token: 0x040015AE RID: 5550
		public float updateDelay;
	}
}
