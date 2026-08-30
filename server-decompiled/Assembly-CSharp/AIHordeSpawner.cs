using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200041C RID: 1052
[Preserve]
public class AIHordeSpawner
{
	// Token: 0x06002087 RID: 8327 RVA: 0x000C40D2 File Offset: 0x000C22D2
	public AIHordeSpawner(World _world, string _spawnerDefinition, Vector3 _targetPos, float _playerSearchBounds)
	{
		this.world = _world;
		this.spawner = new AIDirectorGameStagePartySpawner(_world, _spawnerDefinition);
		this.playerSearchBounds = _playerSearchBounds;
		this.targetPos = _targetPos;
	}

	// Token: 0x06002088 RID: 8328 RVA: 0x000C4108 File Offset: 0x000C2308
	public bool Tick(double _dt)
	{
		if (this.world.GetPlayers().Count == 0 || !AIDirector.CanSpawn(1f))
		{
			return true;
		}
		if (!this.isInited)
		{
			List<Entity> entitiesInBounds = this.world.GetEntitiesInBounds(typeof(EntityPlayer), BoundsUtils.BoundsForMinMax(this.targetPos.x - this.playerSearchBounds, this.targetPos.y - this.playerSearchBounds, this.targetPos.z - this.playerSearchBounds, this.targetPos.x + this.playerSearchBounds, this.targetPos.y + this.playerSearchBounds, this.targetPos.z + this.playerSearchBounds), new List<Entity>());
			for (int i = 0; i < entitiesInBounds.Count; i++)
			{
				EntityPlayer entityPlayer = (EntityPlayer)entitiesInBounds[i];
				if (!entityPlayer.IsIgnoredByAI())
				{
					this.spawner.AddMember(entityPlayer);
				}
			}
			if (this.spawner.partyMembers.Count == 0)
			{
				return false;
			}
			this.isInited = true;
			this.spawner.ResetPartyLevel(0);
			this.spawner.ClearMembers();
		}
		if (!this.spawner.Tick(_dt))
		{
			return true;
		}
		if (!this.spawner.canSpawn || this.numSpawned >= this.numToSpawn)
		{
			return false;
		}
		Vector3 vector;
		if (this.world.IsDaytime())
		{
			if (!this.world.GetMobRandomSpawnPosWithWater(this.targetPos, 45, 55, 45, true, out vector))
			{
				return false;
			}
		}
		else if (!this.world.GetMobRandomSpawnPosWithWater(this.targetPos, 55, 70, 55, true, out vector))
		{
			return false;
		}
		EntityEnemy entityEnemy = (EntityEnemy)EntityFactory.CreateEntity(EntityGroups.GetRandomFromGroup(this.spawner.spawnGroupName, ref this.lastClassId, null), vector);
		Log.Out("Screamer spawned {0} from {1}", new object[]
		{
			entityEnemy.EntityName,
			this.spawner.spawnGroupName
		});
		entityEnemy.SetSpawnerSource(EnumSpawnerSource.Dynamic);
		this.world.SpawnEntityInWorld(entityEnemy);
		entityEnemy.IsHordeZombie = true;
		entityEnemy.bIsChunkObserver = true;
		entityEnemy.SetInvestigatePosition(AIWanderingHordeSpawner.RandomPos(this.world.aiDirector, this.targetPos, 3f), 2400, true);
		this.hordeList.Add(entityEnemy);
		this.spawner.IncSpawnCount();
		this.numSpawned++;
		return false;
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06002089 RID: 8329 RVA: 0x000C4361 File Offset: 0x000C2561
	public bool isSpawning
	{
		get
		{
			return this.spawner.canSpawn;
		}
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x000C4370 File Offset: 0x000C2570
	public void Cleanup()
	{
		for (int i = 0; i < this.hordeList.Count; i++)
		{
			EntityEnemy entityEnemy = this.hordeList[i];
			entityEnemy.IsHordeZombie = false;
			entityEnemy.bIsChunkObserver = false;
		}
		this.hordeList.Clear();
	}

	// Token: 0x04001633 RID: 5683
	public Vector3 targetPos;

	// Token: 0x04001634 RID: 5684
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorGameStagePartySpawner spawner;

	// Token: 0x04001635 RID: 5685
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastClassId;

	// Token: 0x04001636 RID: 5686
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityEnemy> hordeList = new List<EntityEnemy>();

	// Token: 0x04001637 RID: 5687
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04001638 RID: 5688
	[PublicizedFrom(EAccessModifier.Private)]
	public float playerSearchBounds;

	// Token: 0x04001639 RID: 5689
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInited;

	// Token: 0x0400163A RID: 5690
	public int numToSpawn;

	// Token: 0x0400163B RID: 5691
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSpawned;
}
