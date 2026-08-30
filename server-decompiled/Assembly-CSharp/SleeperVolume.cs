using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000BB1 RID: 2993
public class SleeperVolume
{
	// Token: 0x17000978 RID: 2424
	// (get) Token: 0x06005A70 RID: 23152 RVA: 0x0022B448 File Offset: 0x00229648
	public bool IsTrigger
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.TriggeredByIndices.Count > 0;
		}
	}

	// Token: 0x17000979 RID: 2425
	// (get) Token: 0x06005A71 RID: 23153 RVA: 0x0022B458 File Offset: 0x00229658
	public bool IsTriggerAndNoRespawn
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return (this.flags & 7) == 3 && this.respawnMap.Count == 0;
		}
	}

	// Token: 0x06005A72 RID: 23154 RVA: 0x0022B475 File Offset: 0x00229675
	public static void WorldInit()
	{
		if (GameUtils.IsPlaytesting())
		{
			SleeperVolume.sleeperRandom = GameRandomManager.Instance.CreateGameRandom((int)Stopwatch.GetTimestamp());
			return;
		}
		SleeperVolume.sleeperRandom = GameRandomManager.Instance.CreateGameRandom();
	}

	// Token: 0x06005A73 RID: 23155 RVA: 0x0022B4A4 File Offset: 0x002296A4
	public static SleeperVolume Create(PrefabSleeperVolume psv, Vector3i _boxMin, Vector3i _boxMax)
	{
		SleeperVolume sleeperVolume = new SleeperVolume();
		sleeperVolume.SetMinMax(_boxMin, _boxMax);
		sleeperVolume.groupId = psv.groupId;
		sleeperVolume.isQuestExclude = psv.isQuestExclude;
		sleeperVolume.isPriority = psv.isPriority;
		sleeperVolume.spawnCountMin = psv.spawnCountMin;
		sleeperVolume.spawnCountMax = psv.spawnCountMax;
		sleeperVolume.flags = psv.flags;
		sleeperVolume.TriggeredByIndices = new List<byte>(psv.triggeredByIndices);
		sleeperVolume.groupName = GameStageGroup.CleanName(psv.groupName);
		sleeperVolume.SetScript(psv.minScript);
		sleeperVolume.AddToPrefabInstance();
		return sleeperVolume;
	}

	// Token: 0x06005A74 RID: 23156 RVA: 0x0022B53C File Offset: 0x0022973C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMinMax(Vector3i _boxMin, Vector3i _boxMax)
	{
		this.BoxMin = _boxMin;
		this.BoxMax = _boxMax;
		this.Center = (this.BoxMin + this.BoxMax).ToVector3() * 0.5f;
	}

	// Token: 0x06005A75 RID: 23157 RVA: 0x0022B580 File Offset: 0x00229780
	public bool Intersects(Bounds bounds)
	{
		return BoundsUtils.Intersects(bounds, this.BoxMin, this.BoxMax);
	}

	// Token: 0x1700097A RID: 2426
	// (get) Token: 0x06005A76 RID: 23158 RVA: 0x0022B59E File Offset: 0x0022979E
	public PrefabInstance PrefabInstance
	{
		get
		{
			return this.prefabInstance;
		}
	}

	// Token: 0x06005A77 RID: 23159 RVA: 0x0022B5A8 File Offset: 0x002297A8
	public bool AddToPrefabInstance()
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.World.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		this.prefabInstance = dynamicPrefabDecorator.FindVolumeOwner(PrefabVolumeAbs.EVolumeType.Sleeper, this.BoxMin, this.BoxMax);
		if (this.prefabInstance != null)
		{
			this.prefabInstance.AddSleeperVolume(this);
			return true;
		}
		return false;
	}

	// Token: 0x06005A78 RID: 23160 RVA: 0x0022B5FF File Offset: 0x002297FF
	public void AddSpawnPoint(int _x, int _y, int _z, BlockSleeper _block, BlockValue _blockValue)
	{
		if (this.spawnPointList.Count < 255)
		{
			this.spawnPointList.Add(new SleeperVolume.SpawnPoint(new Vector3i(_x, _y, _z), _block.GetSleeperRotation(_blockValue), _blockValue.type));
		}
	}

	// Token: 0x06005A79 RID: 23161 RVA: 0x0022B63B File Offset: 0x0022983B
	public void SetScript(string _script)
	{
		if (string.IsNullOrEmpty(_script))
		{
			this.minScript = null;
			return;
		}
		this.minScript = new MinScript();
		this.minScript.SetText(_script);
	}

	// Token: 0x06005A7A RID: 23162 RVA: 0x0022B664 File Offset: 0x00229864
	public void Tick(World _world)
	{
		if (this.isSpawning)
		{
			if (this.minScript != null && this.minScript.IsRunning())
			{
				foreach (KeyValuePair<int, SleeperVolume.RespawnData> keyValuePair in this.respawnMap)
				{
					if (!this.pendingSpawnMap.Contains(keyValuePair.Key) && !_world.GetEntity(keyValuePair.Key))
					{
						this.respawnMap.Clear();
						this.respawnList = null;
						this.groupCountList.Clear();
						this.numSpawned = 0;
						this.minScript.Restart();
						break;
					}
				}
				this.minScript.Tick(this);
			}
			if (SleeperVolume.TickSpawnCount < 2)
			{
				this.UpdateSpawn(_world);
			}
		}
		if (!this.isSpawning)
		{
			if (this.isSpawned)
			{
				if (this.respawnMap.Count == 0)
				{
					this.isSpawned = false;
				}
				foreach (KeyValuePair<int, SleeperVolume.RespawnData> keyValuePair2 in this.respawnMap)
				{
					if (!this.pendingSpawnMap.Contains(keyValuePair2.Key) && !_world.GetEntity(keyValuePair2.Key))
					{
						this.isSpawned = false;
						break;
					}
				}
			}
			if (this.playerTouchedToUpdate != null)
			{
				this.UpdatePlayerTouched(_world, this.playerTouchedToUpdate);
				this.playerTouchedToUpdate = null;
				return;
			}
			int num = this.ticksUntilDespawn - 1;
			this.ticksUntilDespawn = num;
			if (num == 0)
			{
				this.Despawn(_world);
			}
		}
	}

	// Token: 0x06005A7B RID: 23163 RVA: 0x0022B81C File Offset: 0x00229A1C
	public int GetPlayerTouchedToUpdateId()
	{
		int num = -1;
		if (this.playerTouchedToUpdate != null)
		{
			num = this.playerTouchedToUpdate.entityId;
		}
		return num;
	}

	// Token: 0x06005A7C RID: 23164 RVA: 0x0022B848 File Offset: 0x00229A48
	public int GetPlayerTouchedTriggerId()
	{
		int num = -1;
		if (this.playerTouchedTrigger != null)
		{
			num = this.playerTouchedTrigger.entityId;
		}
		return num;
	}

	// Token: 0x06005A7D RID: 23165 RVA: 0x0022B872 File Offset: 0x00229A72
	public void DespawnAndReset(World _world)
	{
		this.Despawn(_world);
		this.Reset();
	}

	// Token: 0x06005A7E RID: 23166 RVA: 0x0022B884 File Offset: 0x00229A84
	[PublicizedFrom(EAccessModifier.Private)]
	public void Despawn(World _world)
	{
		this.triggerState = SleeperVolume.ETriggerType.Passive;
		this.playerTouchedTrigger = null;
		this.CompletePendingSpawns();
		int num = 0;
		foreach (KeyValuePair<int, SleeperVolume.RespawnData> keyValuePair in this.respawnMap)
		{
			EntityAlive entityAlive = _world.GetEntity(keyValuePair.Key) as EntityAlive;
			if (entityAlive && entityAlive.IsSleeping)
			{
				entityAlive.IsDespawned = true;
				entityAlive.MarkToUnload();
				num++;
			}
		}
	}

	// Token: 0x06005A7F RID: 23167 RVA: 0x0022B91C File Offset: 0x00229B1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Reset()
	{
		this.playerTouchedToUpdate = null;
		this.playerTouchedTrigger = null;
		this.respawnTime = ulong.MaxValue;
		this.isSpawning = false;
		this.isSpawned = false;
		this.wasCleared = false;
		this.groupCountList = null;
		this.numSpawned = 0;
		this.respawnMap.Clear();
		this.respawnList = null;
		this.CancelPendingSpawns();
		if (this.minScript != null)
		{
			this.minScript.Reset();
		}
	}

	// Token: 0x06005A80 RID: 23168 RVA: 0x0022B990 File Offset: 0x00229B90
	[PublicizedFrom(EAccessModifier.Private)]
	public void CancelPendingSpawns()
	{
		if (this.pendingSpawnOps.Count > 0)
		{
			EntityAsyncManager.EntityCreateHandle entityCreateHandle;
			while (this.pendingSpawnOps.TryDequeue(out entityCreateHandle))
			{
				entityCreateHandle.WaitForComplete();
				global::UnityEngine.Object.Destroy(entityCreateHandle.Entity.RootTransform);
			}
		}
		this.pendingSpawnMap.Clear();
	}

	// Token: 0x06005A81 RID: 23169 RVA: 0x0022B9E0 File Offset: 0x00229BE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void CompletePendingSpawns()
	{
		if (this.pendingSpawnOps.Count > 0)
		{
			EntityAsyncManager.EntityCreateHandle entityCreateHandle;
			while (this.pendingSpawnOps.TryDequeue(out entityCreateHandle))
			{
				entityCreateHandle.WaitForComplete();
			}
		}
		this.pendingSpawnMap.Clear();
	}

	// Token: 0x06005A82 RID: 23170 RVA: 0x0022BA20 File Offset: 0x00229C20
	public int GetAliveCount()
	{
		int num = 0;
		if (this.groupCountList != null)
		{
			for (int i = 0; i < this.groupCountList.Count; i++)
			{
				num += this.groupCountList[i].count;
			}
		}
		return num - this.numSpawned + this.respawnMap.Count;
	}

	// Token: 0x06005A83 RID: 23171 RVA: 0x0022BA78 File Offset: 0x00229C78
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdatePlayerTouched(World _world, EntityPlayer _playerTouched)
	{
		if (this.isSpawned || (_world.worldTime < this.respawnTime && this.wasCleared))
		{
			return;
		}
		if (_world.worldTime >= this.respawnTime)
		{
			this.Reset();
		}
		this.CancelPendingSpawns();
		this.isSpawning = true;
		this.isSpawned = true;
		float num = 1f;
		if (this.prefabInstance != null)
		{
			num = ((this.prefabInstance.LastQuestClass == null) ? 1f : this.prefabInstance.LastQuestClass.SpawnMultiplier);
			byte difficultyTier = this.prefabInstance.prefab.DifficultyTier;
			num *= (((int)difficultyTier < SleeperVolume.difficultyTierScale.Length) ? SleeperVolume.difficultyTierScale[(int)difficultyTier] : SleeperVolume.difficultyTierScale[SleeperVolume.difficultyTierScale.Length - 1]);
			if (this.prefabInstance.LastRefreshType.Test_AnySet(QuestEventManager.banditTag))
			{
				num = 0.2f;
			}
		}
		if (this.spawnPointList.Count > 0)
		{
			int num2 = 0;
			this.gameStage = Mathf.Max(0, this.GetGameStageAround(_playerTouched) + num2);
			if (this.respawnMap.Count > 0)
			{
				this.respawnList = new List<int>(this.respawnMap.Count);
				foreach (KeyValuePair<int, SleeperVolume.RespawnData> keyValuePair in this.respawnMap)
				{
					this.respawnList.Add(keyValuePair.Key);
				}
			}
			this.ResetSpawnsAvailable();
			if (this.groupCountList != null)
			{
				this.groupCountList.Clear();
			}
			if (this.spawnCountMin < 0 || this.spawnCountMax < 0)
			{
				this.spawnCountMin = 5;
				this.spawnCountMax = 6;
			}
			this.AddSpawnCount(this.groupName, (float)this.spawnCountMin * num, (float)this.spawnCountMax * num);
			this.spawnDelay = 0;
		}
		if (this.minScript != null)
		{
			this.minScript.Run(this, _playerTouched, num);
		}
	}

	// Token: 0x06005A84 RID: 23172 RVA: 0x0022BC64 File Offset: 0x00229E64
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetSpawnsAvailable()
	{
		bool flag = false;
		if (this.prefabInstance != null && this.prefabInstance.LastRefreshType.Test_AnySet(QuestEventManager.infestedTag))
		{
			flag = true;
		}
		this.spawnsAvailable = new List<int>(this.spawnPointList.Count);
		for (int i = 0; i < this.spawnPointList.Count; i++)
		{
			if (flag || this.spawnPointList[i].GetBlock().spawnMode != BlockSleeper.eMode.Infested)
			{
				this.spawnsAvailable.Add(i);
			}
		}
	}

	// Token: 0x06005A85 RID: 23173 RVA: 0x0022BCEC File Offset: 0x00229EEC
	public void AddSpawnCount(string _groupName, float _min, float _max)
	{
		if (_max == 0f)
		{
			return;
		}
		SleeperVolume.GroupCount groupCount;
		groupCount.groupName = _groupName;
		float num = SleeperVolume.sleeperRandom.RandomRange(_min, _max);
		int num2 = (int)num;
		if (SleeperVolume.sleeperRandom.RandomFloat < num - (float)num2)
		{
			num2++;
		}
		if (_min > 0f && num2 == 0)
		{
			num2 = 1;
		}
		groupCount.count = num2;
		if (num2 > 0)
		{
			if (this.groupCountList == null)
			{
				this.groupCountList = new List<SleeperVolume.GroupCount>();
			}
			this.groupCountList.Add(groupCount);
		}
	}

	// Token: 0x06005A86 RID: 23174 RVA: 0x0022BD68 File Offset: 0x00229F68
	public void CheckTouching(World _world, EntityPlayer _player)
	{
		if (this.IsTriggerAndNoRespawn || _player.IsSpectator)
		{
			return;
		}
		Vector3 position = _player.position;
		position.y += 0.8f;
		SleeperVolume.ETriggerType etriggerType = (SleeperVolume.ETriggerType)(this.flags & 7);
		if (this.hasPassives)
		{
			if (position.x >= (float)this.BoxMin.x - -0.3f && position.x < (float)this.BoxMax.x + -0.3f && position.y >= (float)this.BoxMin.y && position.y < (float)this.BoxMax.y && position.z >= (float)this.BoxMin.z - -0.3f && position.z < (float)this.BoxMax.z + -0.3f && etriggerType != SleeperVolume.ETriggerType.Passive)
			{
				this.TouchGroup(_world, _player, true);
			}
		}
		else if ((etriggerType == SleeperVolume.ETriggerType.Attack || etriggerType == SleeperVolume.ETriggerType.Trigger) && this.triggerState != etriggerType && position.x >= (float)this.BoxMin.x - -0.1f && position.x < (float)this.BoxMax.x + -0.1f && position.y >= (float)this.BoxMin.y && position.y < (float)this.BoxMax.y && position.z >= (float)this.BoxMin.z - -0.1f && position.z < (float)this.BoxMax.z + -0.1f)
		{
			this.TouchGroup(_world, _player, true);
		}
		if (this.playerTouchedToUpdate == null && this.CheckTrigger(_world, position))
		{
			this.TouchGroup(_world, _player, false);
		}
	}

	// Token: 0x06005A87 RID: 23175 RVA: 0x0022BF3C File Offset: 0x0022A13C
	[PublicizedFrom(EAccessModifier.Private)]
	public void TouchGroup(World _world, EntityPlayer _player, bool setActive)
	{
		SleeperVolume.ETriggerType etriggerType = (SleeperVolume.ETriggerType)(this.flags & 7);
		if (this.groupId == 0 || this.prefabInstance == null)
		{
			this.Touch(_world, _player, setActive, etriggerType);
			return;
		}
		List<SleeperVolume> sleeperVolumes = this.prefabInstance.sleeperVolumes;
		for (int i = 0; i < sleeperVolumes.Count; i++)
		{
			SleeperVolume sleeperVolume = sleeperVolumes[i];
			if (sleeperVolume.groupId == this.groupId && !sleeperVolume.IsTriggerAndNoRespawn)
			{
				sleeperVolume.Touch(_world, _player, setActive, etriggerType);
			}
		}
	}

	// Token: 0x06005A88 RID: 23176 RVA: 0x0022BFB4 File Offset: 0x0022A1B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Touch(World _world, EntityPlayer _player, bool setActive, SleeperVolume.ETriggerType trigger)
	{
		if (setActive)
		{
			bool flag = (trigger == SleeperVolume.ETriggerType.Attack || trigger == SleeperVolume.ETriggerType.Trigger) && _player;
			foreach (KeyValuePair<int, SleeperVolume.RespawnData> keyValuePair in this.respawnMap)
			{
				int key = keyValuePair.Key;
				EntityAlive entityAlive = (EntityAlive)_world.GetEntity(key);
				if (entityAlive)
				{
					if (flag && _player.Stealth.CanSleeperAttackDetect(entityAlive))
					{
						entityAlive.ConditionalTriggerSleeperWakeUp();
						entityAlive.SetAttackTarget(_player, 400);
					}
					else if (trigger == SleeperVolume.ETriggerType.Wander)
					{
						entityAlive.ConditionalTriggerSleeperWakeUp();
					}
					else if (--SleeperVolume.wanderingCountdown <= 0)
					{
						SleeperVolume.wanderingCountdown = 10;
						entityAlive.ConditionalTriggerSleeperWakeUp();
					}
					else
					{
						entityAlive.SetSleeperActive();
					}
				}
			}
			this.hasPassives = false;
			this.triggerState = trigger;
			return;
		}
		this.playerTouchedToUpdate = _player;
		this.ticksUntilDespawn = 900;
		if (this.hasPassives)
		{
			this.ticksUntilDespawn = 200;
		}
		if (this.wasCleared && _world.worldTime < this.respawnTime)
		{
			this.respawnTime = Math.Max(this.respawnTime, _world.worldTime + 1000UL);
		}
	}

	// Token: 0x06005A89 RID: 23177 RVA: 0x0022C108 File Offset: 0x0022A308
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckTrigger(World _world, Vector3 playerPos)
	{
		if (this.isSpawned)
		{
			Vector3i vector3i = this.BoxMin - SleeperVolume.unpadding;
			Vector3i vector3i2 = this.BoxMax + SleeperVolume.unpadding;
			return playerPos.x >= (float)vector3i.x && playerPos.x < (float)vector3i2.x && playerPos.y >= (float)vector3i.y && playerPos.y < (float)vector3i2.y && playerPos.z >= (float)vector3i.z && playerPos.z < (float)vector3i2.z;
		}
		Vector3i vector3i3 = this.BoxMin - SleeperVolume.triggerPaddingMin;
		Vector3i vector3i4 = this.BoxMax + SleeperVolume.triggerPaddingMax;
		if (playerPos.x < (float)vector3i3.x || playerPos.x >= (float)vector3i4.x || playerPos.y < (float)vector3i3.y || playerPos.y >= (float)vector3i4.y || playerPos.z < (float)vector3i3.z || playerPos.z >= (float)vector3i4.z)
		{
			return false;
		}
		if (!this.wasCleared)
		{
			if (this.prefabInstance != null)
			{
				_world.UncullPOI(this.prefabInstance);
			}
			return true;
		}
		if (GameUtils.CheckForAnyPlayerHome(GameManager.Instance.World, this.BoxMin, this.BoxMax) != GameUtils.EPlayerHomeType.None)
		{
			this.respawnTime = Math.Max(this.respawnTime, _world.worldTime + 24000UL);
			return false;
		}
		return true;
	}

	// Token: 0x06005A8A RID: 23178 RVA: 0x0022C27C File Offset: 0x0022A47C
	public void CheckNoise(World _world, Vector3 pos)
	{
		if (this.hasPassives && pos.x >= (float)this.BoxMin.x - 0.9f && pos.x < (float)this.BoxMax.x + 0.9f && pos.y >= (float)this.BoxMin.y - 0.9f && pos.y < (float)this.BoxMax.y + 0.9f && pos.z >= (float)this.BoxMin.z - 0.9f && pos.z < (float)this.BoxMax.z + 0.9f && (this.flags & 7) != 1)
		{
			this.TouchGroup(_world, null, true);
		}
	}

	// Token: 0x06005A8B RID: 23179 RVA: 0x0022C347 File Offset: 0x0022A547
	public void OnTriggered(EntityPlayer _player, World _world, int _triggerIndex)
	{
		this.triggerState = (SleeperVolume.ETriggerType)(this.flags & 7);
		this.playerTouchedTrigger = _player;
		this.UpdatePlayerTouched(_world, _player);
	}

	// Token: 0x06005A8C RID: 23180 RVA: 0x0022C368 File Offset: 0x0022A568
	public void EntityDied(EntityAlive entity)
	{
		if (!this.respawnMap.Remove(entity.entityId))
		{
			return;
		}
		if (this.respawnList != null)
		{
			this.respawnList.Remove(entity.entityId);
		}
		int num = this.numSpawned;
		int count = this.respawnMap.Count;
		if (!this.isSpawning)
		{
			this.ClearedUpdate(entity.world);
		}
	}

	// Token: 0x06005A8D RID: 23181 RVA: 0x0022C3CC File Offset: 0x0022A5CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearedUpdate(World _world)
	{
		if (this.wasCleared || this.respawnMap.Count > 0)
		{
			return;
		}
		int @int = GamePrefs.GetInt(EnumGamePrefs.LootRespawnDays);
		if (@int > 0)
		{
			this.respawnTime = _world.worldTime + (ulong)(@int * 24000);
		}
		else
		{
			this.respawnTime = (ulong)(-1);
		}
		this.wasCleared = true;
	}

	// Token: 0x06005A8E RID: 23182 RVA: 0x0022C422 File Offset: 0x0022A622
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetGameStageAround(EntityPlayer player)
	{
		return GameStageDefinition.CalcGameStageAround(player);
	}

	// Token: 0x06005A8F RID: 23183 RVA: 0x0022C42C File Offset: 0x0022A62C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool SpawnPointIsHidden(World _world, int _index)
	{
		SleeperVolume.SpawnPoint spawnPoint = this.spawnPointList[_index];
		Vector3 vector = spawnPoint.pos.ToVector3();
		vector.x += 0.5f;
		vector.z += 0.5f;
		int num = 0;
		if (spawnPoint.GetBlock().pose == 5)
		{
			num = 1;
		}
		float[] array = SleeperVolume.isHiddenOffsets[num];
		for (int i = 0; i < _world.Players.list.Count; i++)
		{
			EntityPlayer entityPlayer = _world.Players.list[i];
			Vector3 headPosition = entityPlayer.getHeadPosition();
			Vector3 vector2 = vector - headPosition;
			float num2 = vector2.sqrMagnitude;
			if (num2 <= 3600f)
			{
				int modelLayer = entityPlayer.GetModelLayer();
				entityPlayer.SetModelLayer(2, false, null);
				Ray ray = new Ray(headPosition, Vector3.one);
				Vector3 vector3 = Vector3.Cross(vector2.normalized, Vector3.up);
				for (int j = 0; j < array.Length; j += 2)
				{
					Vector3 vector4 = vector + vector3 * array[j];
					vector4.y += array[j + 1];
					Vector3 vector5 = vector4 - headPosition;
					num2 = vector5.magnitude;
					ray.direction = vector5;
					if (!Voxel.Raycast(_world, ray, num2, 71, 0f))
					{
						entityPlayer.SetModelLayer(modelLayer, false, null);
						return false;
					}
				}
				entityPlayer.SetModelLayer(modelLayer, false, null);
			}
		}
		return true;
	}

	// Token: 0x06005A90 RID: 23184 RVA: 0x0022C5A4 File Offset: 0x0022A7A4
	[PublicizedFrom(EAccessModifier.Private)]
	public int FindFathestSpawnFromPlayers(World _world)
	{
		int num = -1;
		float num2 = float.MinValue;
		for (int i = 0; i < this.spawnsAvailable.Count; i++)
		{
			int num3 = this.spawnsAvailable[i];
			Vector3i pos = this.spawnPointList[num3].pos;
			if (_world.CanSleeperSpawnAtPos(pos, this.minScript == null))
			{
				Vector3 vector = pos.ToVector3();
				vector.x += 0.5f;
				vector.z += 0.5f;
				float num4 = float.MaxValue;
				for (int j = 0; j < _world.Players.list.Count; j++)
				{
					Vector3 position = _world.Players.list[j].position;
					float sqrMagnitude = (vector - position).sqrMagnitude;
					if (sqrMagnitude < num4)
					{
						num4 = sqrMagnitude;
					}
				}
				if (num4 > num2)
				{
					num2 = num4;
					num = i;
				}
			}
		}
		if (num < 0)
		{
			return -1;
		}
		int num5 = this.spawnsAvailable[num];
		this.spawnsAvailable.RemoveAt(num);
		return num5;
	}

	// Token: 0x06005A91 RID: 23185 RVA: 0x0022C6C0 File Offset: 0x0022A8C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveSpawnAvailable(int index)
	{
		for (int i = 0; i < this.spawnsAvailable.Count; i++)
		{
			if (this.spawnsAvailable[i] == index)
			{
				this.spawnsAvailable.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06005A92 RID: 23186 RVA: 0x0022C700 File Offset: 0x0022A900
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateSpawn(World _world)
	{
		int num = this.spawnDelay - 1;
		this.spawnDelay = num;
		if (num > 0)
		{
			return;
		}
		this.spawnDelay = 2;
		bool flag = AIDirector.CanSpawn(2.1f);
		int @int = GameStats.GetInt(EnumGameStats.EnemyCount);
		bool flag2 = false;
		if (this.minScript != null && this.minScript.IsRunning())
		{
			flag2 = true;
		}
		if (this.spawnsAvailable != null)
		{
			string text = Time.time.ToCultureInvariantString();
			if (this.respawnList != null && this.respawnList.Count > 0)
			{
				int num2 = this.respawnList[this.respawnList.Count - 1];
				this.respawnList.RemoveAt(this.respawnList.Count - 1);
				Entity entity = _world.GetEntity(num2);
				if (this.pendingSpawnMap.Contains(num2))
				{
					flag2 = true;
				}
				else if (entity)
				{
					this.hasPassives = true;
					flag2 = true;
					Log.Out("{0} SleeperVolume {1}: Still alive '{2}'", new object[] { text, this.BoxMin, entity.name });
				}
				else
				{
					int num3 = this.respawnMap[num2].spawnPointIndex;
					if (num3 >= 0)
					{
						this.RemoveSpawnAvailable(num3);
					}
					else
					{
						num3 = this.FindSpawnIndex(_world);
					}
					if (num3 >= 0)
					{
						SleeperVolume.SpawnPoint spawnPoint = this.spawnPointList[num3];
						if (!this.CheckSpawnPos(_world, spawnPoint.pos))
						{
							this.respawnList.Add(num2);
							this.spawnsAvailable.Add(num3);
							return;
						}
						string className = this.respawnMap[num2].className;
						int num4 = EntityClass.FromString(className);
						EntityClass entityClass;
						EntityClass.list.TryGetValue(num4, out entityClass);
						if (entityClass == null || (entityClass.bIsEnemyEntity && !EntityFactory.EnemySpawnMode))
						{
							this.respawnMap.Remove(num2);
						}
						else
						{
							Log.Out("{0} SleeperVolume {1}: Restoring {2} ({3}) '{4}', count {5}", new object[]
							{
								text,
								this.BoxMin,
								spawnPoint.pos,
								World.toChunkXZ(spawnPoint.pos),
								className,
								@int
							});
							BlockSleeper block = spawnPoint.GetBlock();
							if (this.Spawn(_world, num4, num3, block))
							{
								this.respawnMap.Remove(num2);
							}
						}
						flag2 = true;
					}
				}
			}
			else if (flag)
			{
				GameStageDefinition gameStageDefinition = null;
				if (this.groupCountList != null)
				{
					int num5 = 0;
					for (int i = 0; i < this.groupCountList.Count; i++)
					{
						num5 += this.groupCountList[i].count;
						if (num5 > this.numSpawned)
						{
							GameStageGroup gameStageGroup = GameStageGroup.TryGet(this.groupCountList[i].groupName);
							if (gameStageGroup == null)
							{
								PrefabInstance prefabInstance = this.prefabInstance;
								string text2 = ((prefabInstance != null) ? prefabInstance.name : null) ?? "null";
								Log.Error("{0} SleeperVolume {1} {2}: Spawning group '{3}' missing", new object[]
								{
									text,
									text2,
									this.BoxMin,
									this.groupCountList[i].groupName
								});
								gameStageGroup = GameStageGroup.TryGet("GroupGenericZombie");
							}
							gameStageDefinition = gameStageGroup.spawner;
							break;
						}
					}
				}
				if (gameStageDefinition != null)
				{
					GameStageDefinition.Stage stage = gameStageDefinition.GetStage(this.gameStage);
					if (stage != null)
					{
						int num6 = this.FindSpawnIndex(_world);
						if (num6 >= 0)
						{
							SleeperVolume.SpawnPoint spawnPoint2 = this.spawnPointList[num6];
							if (!this.CheckSpawnPos(_world, spawnPoint2.pos))
							{
								this.spawnsAvailable.Add(num6);
								return;
							}
							BlockSleeper block2 = spawnPoint2.GetBlock();
							if (block2 == null)
							{
								Log.Error("{0} BlockSleeper {1} null, type {2}", new object[] { text, spawnPoint2.pos, spawnPoint2.blockType });
							}
							else
							{
								string spawnGroup = block2.spawnGroup;
								if (string.IsNullOrEmpty(spawnGroup))
								{
									spawnGroup = stage.GetSpawnGroup(0).groupName;
								}
								int randomFromGroup = EntityGroups.GetRandomFromGroup(spawnGroup, ref this.lastClassId, SleeperVolume.sleeperRandom);
								EntityClass entityClass2;
								EntityClass.list.TryGetValue(randomFromGroup, out entityClass2);
								if (entityClass2 == null || (entityClass2.bIsEnemyEntity && !EntityFactory.EnemySpawnMode))
								{
									this.numSpawned++;
								}
								else
								{
									Log.Out("{0} SleeperVolume {1}: Spawning {2} ({3}), group '{4}', class {5}, count {6}", new object[]
									{
										text,
										this.BoxMin,
										spawnPoint2.pos,
										World.toChunkXZ(spawnPoint2.pos),
										spawnGroup,
										(entityClass2 != null) ? entityClass2.entityClassName : "?",
										@int
									});
									if (this.Spawn(_world, randomFromGroup, num6, block2))
									{
										this.numSpawned++;
									}
								}
								flag2 = true;
							}
						}
					}
				}
			}
		}
		if (!flag2)
		{
			this.isSpawning = false;
			this.respawnList = null;
			if (this.numSpawned == 0)
			{
				if (this.respawnMap.Count == 0)
				{
					this.wasCleared = true;
				}
				Log.Out("{0} SleeperVolume {1}: None spawned, canSpawn {2}, respawnCnt {3}", new object[]
				{
					Time.time.ToCultureInvariantString(),
					this.BoxMin,
					flag,
					this.respawnMap.Count
				});
				return;
			}
			this.ClearedUpdate(_world);
		}
	}

	// Token: 0x06005A93 RID: 23187 RVA: 0x0022CC58 File Offset: 0x0022AE58
	[PublicizedFrom(EAccessModifier.Private)]
	public int FindSpawnIndex(World _world)
	{
		if (this.spawnsAvailable.Count == 0)
		{
			this.ResetSpawnsAvailable();
		}
		int num = SleeperVolume.sleeperRandom.RandomRange(0, this.spawnsAvailable.Count);
		for (int i = this.spawnsAvailable.Count; i > 0; i--)
		{
			int num2 = this.spawnsAvailable[num];
			Vector3i pos = this.spawnPointList[num2].pos;
			if (_world.CanSleeperSpawnAtPos(pos, true) && this.SpawnPointIsHidden(_world, num2))
			{
				this.spawnsAvailable.RemoveAt(num);
				return num2;
			}
			if (++num >= this.spawnsAvailable.Count)
			{
				num = 0;
			}
		}
		return this.FindFathestSpawnFromPlayers(_world);
	}

	// Token: 0x06005A94 RID: 23188 RVA: 0x0022CD08 File Offset: 0x0022AF08
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckSpawnPos(World _world, Vector3i pos)
	{
		if (GameManager.bRecordNextSession || GameManager.bPlayRecordedSession)
		{
			return true;
		}
		Chunk chunk = (Chunk)_world.GetChunkFromWorldPos(pos);
		return chunk != null && !chunk.IsInternalBlocksCulled && !chunk.NeedsCopying && !chunk.NeedsRegeneration;
	}

	// Token: 0x06005A95 RID: 23189 RVA: 0x0022CD50 File Offset: 0x0022AF50
	[PublicizedFrom(EAccessModifier.Private)]
	public bool Spawn(World _world, int entityClassId, int spawnIndex, BlockSleeper block)
	{
		SleeperVolume.<>c__DisplayClass102_0 CS$<>8__locals1 = new SleeperVolume.<>c__DisplayClass102_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1._world = _world;
		CS$<>8__locals1.spawnIndex = spawnIndex;
		CS$<>8__locals1.block = block;
		SleeperVolume.SpawnPoint spawnPoint = this.spawnPointList[CS$<>8__locals1.spawnIndex];
		CS$<>8__locals1.spawnPos = spawnPoint.pos.ToVector3();
		SleeperVolume.<>c__DisplayClass102_0 CS$<>8__locals2 = CS$<>8__locals1;
		CS$<>8__locals2.spawnPos.x = CS$<>8__locals2.spawnPos.x + 0.502f;
		SleeperVolume.<>c__DisplayClass102_0 CS$<>8__locals3 = CS$<>8__locals1;
		CS$<>8__locals3.spawnPos.z = CS$<>8__locals3.spawnPos.z + 0.501f;
		EntityClass entityClass;
		if (!EntityClass.list.TryGetValue(entityClassId, out entityClass))
		{
			Log.Warning("Spawn class {0} is missing", new object[] { entityClassId });
			entityClassId = EntityClass.FromString("zombieArlene");
		}
		else if (CS$<>8__locals1.block != null && CS$<>8__locals1.block.ExcludesWalkType(EntityAlive.GetSpawnWalkType(entityClass)))
		{
			Log.Warning("Spawn {0} can't walk on block {1} with walkType {2}", new object[]
			{
				entityClass.entityClassName,
				CS$<>8__locals1.block,
				EntityAlive.GetSpawnWalkType(entityClass)
			});
			return false;
		}
		EntityCreationData entityCreationData = EntityFactory.SetupEntityCreationData(entityClassId, CS$<>8__locals1.spawnPos, new Vector3(0f, spawnPoint.rot, 0f));
		EntityAsyncManager.EntityCreateHandle entityCreateHandle = CS$<>8__locals1._world.entityAsyncManager.StartCreateEntity(entityCreationData, delegate(EntityAsyncManager.EntityCreateHandle handle)
		{
			CS$<>8__locals1.<>4__this.AddEnemyToWorld(CS$<>8__locals1._world, handle.Entity as EntityAlive, CS$<>8__locals1.spawnIndex, CS$<>8__locals1.spawnPos, CS$<>8__locals1.block.look, CS$<>8__locals1.block.pose);
		});
		this.pendingSpawnOps.Enqueue(entityCreateHandle);
		this.pendingSpawnMap.Add(entityCreationData.id);
		SleeperVolume.RespawnData respawnData;
		respawnData.className = entityClass.entityClassName;
		respawnData.spawnPointIndex = CS$<>8__locals1.spawnIndex;
		this.respawnMap.Add(entityCreationData.id, respawnData);
		SleeperVolume.TickSpawnCount++;
		return true;
	}

	// Token: 0x06005A96 RID: 23190 RVA: 0x0022CEEC File Offset: 0x0022B0EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddEnemyToWorld(World _world, EntityAlive zombie, int spawnIndex, Vector3 spawnPos, Vector3 lookDir, int pose)
	{
		if (!zombie)
		{
			Log.Error("Spawn is null");
			return;
		}
		zombie.SetSpawnerSource(EnumSpawnerSource.Dynamic);
		zombie.IsSleeperPassive = true;
		zombie.SleeperSpawnPosition = spawnPos;
		zombie.SleeperSpawnLookDir = lookDir;
		zombie.SetSleeper();
		zombie.TriggerSleeperPose(pose, false);
		_world.SpawnEntityInWorld(zombie);
		this.hasPassives = true;
		this.SpawnParticle("sleeperSpawn", zombie);
		if (this.playerTouchedTrigger)
		{
			GameManager.Instance.StartCoroutine(this.WakeAttackLater(zombie, this.playerTouchedTrigger));
		}
	}

	// Token: 0x06005A97 RID: 23191 RVA: 0x0022CF78 File Offset: 0x0022B178
	[PublicizedFrom(EAccessModifier.Private)]
	public void SpawnParticle(string _particleName, EntityAlive _zombie)
	{
		World world = _zombie.world;
		Vector3 position = _zombie.position;
		position.y += 0.5f;
		Vector3i vector3i = World.worldToBlockPos(position);
		vector3i.y++;
		if (!world.GetBlock(vector3i).isair)
		{
			vector3i.y--;
			float lightBrightness = world.GetLightBrightness(vector3i);
			ParticleEffect particleEffect = new ParticleEffect(_particleName, position, lightBrightness, Color.white, null, null, false, 1f, "");
			world.GetGameManager().SpawnParticleEffectServer(particleEffect, _zombie.entityId, false, false);
		}
	}

	// Token: 0x06005A98 RID: 23192 RVA: 0x0022D00E File Offset: 0x0022B20E
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator WakeAttackLater(EntityAlive _ea, EntityPlayer _playerTouched)
	{
		yield return new WaitForSeconds(1f);
		if (_ea && _playerTouched)
		{
			_ea.ConditionalTriggerSleeperWakeUp();
			_ea.SetAttackTarget(_playerTouched, 400);
		}
		yield break;
	}

	// Token: 0x06005A99 RID: 23193 RVA: 0x0022D024 File Offset: 0x0022B224
	public List<SleeperVolume.SpawnPoint> GetSpawnPoints()
	{
		return this.spawnPointList;
	}

	// Token: 0x06005A9A RID: 23194 RVA: 0x0022D02C File Offset: 0x0022B22C
	public static SleeperVolume Read(BinaryReader _br)
	{
		SleeperVolume sleeperVolume = new SleeperVolume();
		int num = (int)_br.ReadByte();
		string text = _br.ReadString();
		text = GameStageGroup.CleanName(text);
		if (num >= 13)
		{
			if (num >= 16)
			{
				sleeperVolume.groupId = _br.ReadInt16();
			}
			sleeperVolume.spawnCountMin = _br.ReadInt16();
			sleeperVolume.spawnCountMax = _br.ReadInt16();
		}
		sleeperVolume.groupName = text;
		sleeperVolume.SetMinMax(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
		sleeperVolume.respawnTime = _br.ReadUInt64();
		if (num <= 13)
		{
			_br.ReadUInt64();
		}
		sleeperVolume.numSpawned = _br.ReadInt32();
		if (num > 7)
		{
			_br.ReadInt32();
		}
		sleeperVolume.gameStage = _br.ReadInt32();
		if (num > 3)
		{
			if (num > 4)
			{
				if (num < 11)
				{
					_br.ReadString();
				}
			}
			else
			{
				_br.ReadInt32();
			}
		}
		if (num >= 10)
		{
			_br.ReadString();
			_br.ReadInt32();
		}
		if (num > 5)
		{
			sleeperVolume.ticksUntilDespawn = _br.ReadInt32();
		}
		if (num >= 14)
		{
			ushort num2 = _br.ReadUInt16();
			sleeperVolume.isQuestExclude = (num2 & 1) > 0;
			sleeperVolume.isPriority = (num2 & 2) > 0;
			sleeperVolume.isSpawning = (num2 & 4) > 0;
			sleeperVolume.wasCleared = (num2 & 8) > 0;
			if (num >= 18)
			{
				sleeperVolume.flags = _br.ReadInt32();
			}
		}
		else
		{
			sleeperVolume.isSpawning = _br.ReadBoolean();
			sleeperVolume.wasCleared = _br.ReadBoolean();
			if (num >= 12)
			{
				sleeperVolume.isQuestExclude = _br.ReadBoolean();
			}
		}
		int num3 = (int)_br.ReadByte();
		if (num3 > 0)
		{
			for (int i = 0; i < num3; i++)
			{
				sleeperVolume.spawnPointList.Add(SleeperVolume.SpawnPoint.Read(_br, num));
			}
		}
		if (num > 1)
		{
			num3 = (int)_br.ReadByte();
			if (num3 > 0)
			{
				sleeperVolume.spawnsAvailable = new List<int>(num3);
				for (int j = 0; j < num3; j++)
				{
					sleeperVolume.spawnsAvailable.Add((int)_br.ReadByte());
				}
			}
		}
		num3 = (int)_br.ReadByte();
		if (num3 > 0)
		{
			for (int k = 0; k < num3; k++)
			{
				_br.ReadInt32();
			}
			sleeperVolume.hasPassives = true;
		}
		if (num >= 8)
		{
			num3 = (int)_br.ReadByte();
			if (num3 > 0)
			{
				for (int l = 0; l < num3; l++)
				{
					int num4 = _br.ReadInt32();
					SleeperVolume.RespawnData respawnData;
					respawnData.className = _br.ReadString();
					respawnData.spawnPointIndex = ((num >= 17) ? ((int)_br.ReadByte()) : (-1));
					sleeperVolume.respawnMap.Add(num4, respawnData);
				}
			}
		}
		num3 = (int)_br.ReadByte();
		if (num3 > 0)
		{
			sleeperVolume.groupCountList = new List<SleeperVolume.GroupCount>(num3);
			for (int m = 0; m < num3; m++)
			{
				SleeperVolume.GroupCount groupCount;
				groupCount.groupName = text;
				if (num >= 21)
				{
					groupCount.groupName = _br.ReadString();
				}
				groupCount.count = _br.ReadInt32();
				sleeperVolume.groupCountList.Add(groupCount);
			}
		}
		if (num >= 19)
		{
			num3 = (int)_br.ReadByte();
			sleeperVolume.TriggeredByIndices.Clear();
			if (num3 > 0)
			{
				for (int n = 0; n < num3; n++)
				{
					sleeperVolume.TriggeredByIndices.Add(_br.ReadByte());
				}
			}
		}
		if ((sleeperVolume.flags & 16) > 0)
		{
			sleeperVolume.minScript = MinScript.Read(_br);
		}
		return sleeperVolume;
	}

	// Token: 0x06005A9B RID: 23195 RVA: 0x0022D350 File Offset: 0x0022B550
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(21);
		_bw.Write(this.groupName ?? string.Empty);
		_bw.Write(this.groupId);
		_bw.Write(this.spawnCountMin);
		_bw.Write(this.spawnCountMax);
		_bw.Write(this.BoxMin.x);
		_bw.Write(this.BoxMin.y);
		_bw.Write(this.BoxMin.z);
		_bw.Write(this.BoxMax.x);
		_bw.Write(this.BoxMax.y);
		_bw.Write(this.BoxMax.z);
		_bw.Write(this.respawnTime);
		_bw.Write(this.numSpawned);
		_bw.Write(0);
		_bw.Write(this.gameStage);
		_bw.Write(string.Empty);
		_bw.Write(0);
		_bw.Write(this.ticksUntilDespawn);
		ushort num = 0;
		if (this.isQuestExclude)
		{
			num |= 1;
		}
		if (this.isPriority)
		{
			num |= 2;
		}
		if (this.isSpawning)
		{
			num |= 4;
		}
		if (this.wasCleared)
		{
			num |= 8;
		}
		_bw.Write(num);
		this.flags &= -17;
		if (this.minScript != null && this.minScript.HasData())
		{
			this.flags |= 16;
		}
		_bw.Write(this.flags);
		int count = this.spawnPointList.Count;
		_bw.Write((byte)count);
		for (int i = 0; i < count; i++)
		{
			this.spawnPointList[i].Write(_bw);
		}
		int num2 = ((this.spawnsAvailable != null) ? this.spawnsAvailable.Count : 0);
		_bw.Write((byte)num2);
		for (int j = 0; j < num2; j++)
		{
			_bw.Write((byte)this.spawnsAvailable[j]);
		}
		_bw.Write(0);
		_bw.Write((byte)((this.respawnMap != null) ? this.respawnMap.Count : 0));
		if (this.respawnMap != null)
		{
			foreach (KeyValuePair<int, SleeperVolume.RespawnData> keyValuePair in this.respawnMap)
			{
				_bw.Write(keyValuePair.Key);
				_bw.Write(keyValuePair.Value.className);
				_bw.Write((byte)keyValuePair.Value.spawnPointIndex);
			}
		}
		int num3 = ((this.groupCountList != null) ? this.groupCountList.Count : 0);
		if (num3 > 255)
		{
			num3 = 255;
			Log.Error("{0}, groupCountList > 255", new object[] { this });
		}
		_bw.Write((byte)num3);
		if (this.groupCountList != null)
		{
			for (int k = 0; k < this.groupCountList.Count; k++)
			{
				_bw.Write(this.groupCountList[k].groupName);
				_bw.Write(this.groupCountList[k].count);
			}
		}
		_bw.Write((byte)this.TriggeredByIndices.Count);
		for (int l = 0; l < this.TriggeredByIndices.Count; l++)
		{
			_bw.Write(this.TriggeredByIndices[l]);
		}
		if ((this.flags & 16) > 0)
		{
			this.minScript.Write(_bw);
		}
	}

	// Token: 0x06005A9C RID: 23196 RVA: 0x0022D6D0 File Offset: 0x0022B8D0
	public override string ToString()
	{
		string text = ((this.groupCountList != null && this.groupCountList.Count > 0) ? this.groupCountList[0].groupName : "");
		return string.Format("{0} {1} G{2} Trig{3} RespawnC{4}", new object[]
		{
			this.BoxMin,
			text,
			this.groupId,
			this.IsTrigger ? 1 : 0,
			this.respawnMap.Count
		});
	}

	// Token: 0x06005A9D RID: 23197 RVA: 0x0022D763 File Offset: 0x0022B963
	[Conditional("DEBUG_SLEEPERLOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogSleeper(string format, params object[] args)
	{
		format = string.Format("{0} {1} SleeperVolume {2}", GameManager.frameTime.ToCultureInvariantString(), GameManager.frameCount, format);
		Log.Warning(format, args);
	}

	// Token: 0x06005A9E RID: 23198 RVA: 0x0022D790 File Offset: 0x0022B990
	public void Draw(float _duration)
	{
		Vector3 vector = this.BoxMin.ToVector3() - Origin.position;
		Vector3 vector2 = this.BoxMax.ToVector3();
		vector2 -= Origin.position;
		Color color = this.GetColor();
		Utils.DrawBoxLines(vector, vector2, color, _duration);
	}

	// Token: 0x06005A9F RID: 23199 RVA: 0x0022D7DC File Offset: 0x0022B9DC
	public void DrawDebugLines(float _duration)
	{
		string text = string.Format("SleeperVolume{0},{1}", this.BoxMin, this.BoxMax);
		Color color = this.GetColor();
		Vector3 vector = this.BoxMin.ToVector3();
		Vector3 vector2 = this.BoxMax.ToVector3();
		vector += DebugLines.InsideOffsetV;
		vector2 -= DebugLines.InsideOffsetV;
		DebugLines.Create(text, GameManager.Instance.World.GetPrimaryPlayer().RootTransform, color, color, 0.05f, 0.05f, _duration).AddCube(vector, vector2);
	}

	// Token: 0x06005AA0 RID: 23200 RVA: 0x0022D870 File Offset: 0x0022BA70
	[PublicizedFrom(EAccessModifier.Private)]
	public Color GetColor()
	{
		Color color = (this.isQuestExclude ? Color.red : Color.green);
		if (this.respawnMap.Count > 0)
		{
			color.b = 1f;
		}
		if (this.IsTrigger)
		{
			color = new Color(0.25f, 0.25f, 0.25f);
		}
		if (this.wasCleared)
		{
			color.r *= 0.4f;
			color.g *= 0.4f;
			color.b *= 0.4f;
			color.a = 0.16f;
		}
		return color;
	}

	// Token: 0x06005AA1 RID: 23201 RVA: 0x0022D910 File Offset: 0x0022BB10
	public string GetDescription()
	{
		long num = (long)(this.respawnTime - GameManager.Instance.World.worldTime);
		if (num < 0L)
		{
			num = 0L;
		}
		int num2 = -1;
		int num3 = 0;
		if (this.groupCountList != null)
		{
			num2 = this.groupCountList.Count;
			for (int i = 0; i > this.groupCountList.Count; i++)
			{
				num3 += this.groupCountList[i].count;
			}
		}
		return string.Format("{0}, grpId {1}, {2} ({3}), cntList {4}/{5}, respawnCnt {6}, spawned {7}, clear{8}, plHome {9}, respawnIn {10}, {11}", new object[]
		{
			this.BoxMin,
			this.groupId,
			(SleeperVolume.ETriggerType)(this.flags & 7),
			this.triggerState,
			num2,
			num3,
			this.respawnMap.Count,
			this.numSpawned,
			this.wasCleared,
			GameUtils.CheckForAnyPlayerHome(GameManager.Instance.World, this.BoxMin, this.BoxMax),
			this.DurationToString(num),
			(this.prefabInstance != null) ? (this.prefabInstance.name + ", volumes " + this.prefabInstance.sleeperVolumes.Count.ToString()) : "?"
		});
	}

	// Token: 0x06005AA2 RID: 23202 RVA: 0x0022DA7C File Offset: 0x0022BC7C
	[PublicizedFrom(EAccessModifier.Private)]
	public string DurationToString(long duration)
	{
		string text = "";
		int num = (int)((double)duration / 1000.0 / 24.0);
		if (num > 0)
		{
			text += num.ToString("0:");
		}
		int num2 = (int)((double)duration / 1000.0) % 24;
		if (num > 0 || num2 > 0)
		{
			text += num2.ToString("00:");
		}
		int num3 = (int)((double)duration / 1000.0 * 60.0) % 60;
		if (num > 0 || num2 > 0 || num3 > 0)
		{
			text += num3.ToString("00:");
		}
		return text + ((int)((double)duration / 1000.0 * 60.0 * 60.0) % 60).ToString("00");
	}

	// Token: 0x0400461E RID: 17950
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cVersion = 21;

	// Token: 0x0400461F RID: 17951
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSpawnDelay = 2;

	// Token: 0x04004620 RID: 17952
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSpawnVisibleCheckDist = 60f;

	// Token: 0x04004621 RID: 17953
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDespawnDelay = 900;

	// Token: 0x04004622 RID: 17954
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDespawnPassiveDelay = 200;

	// Token: 0x04004623 RID: 17955
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cBedrollClearTime = 24000;

	// Token: 0x04004624 RID: 17956
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPlayerInsideDelayTime = 1000;

	// Token: 0x04004625 RID: 17957
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cPlayerYOffset = 0.8f;

	// Token: 0x04004626 RID: 17958
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAttackPaddingXZ = -0.1f;

	// Token: 0x04004627 RID: 17959
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cPassivePaddingXZ = -0.3f;

	// Token: 0x04004628 RID: 17960
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cPassiveNoisePadding = 0.9f;

	// Token: 0x04004629 RID: 17961
	public static Vector3i chunkPadding = new Vector3i(12, 1, 12);

	// Token: 0x0400462A RID: 17962
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3i triggerPaddingMin = new Vector3i(8f, 0.7f, 8f);

	// Token: 0x0400462B RID: 17963
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3i triggerPaddingMax = new Vector3i(8f, 0.7f, 8f);

	// Token: 0x0400462C RID: 17964
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3i unpadding = new Vector3i(14, 16, 14);

	// Token: 0x0400462D RID: 17965
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance prefabInstance;

	// Token: 0x0400462E RID: 17966
	[PublicizedFrom(EAccessModifier.Private)]
	public short groupId;

	// Token: 0x0400462F RID: 17967
	[PublicizedFrom(EAccessModifier.Private)]
	public short spawnCountMin;

	// Token: 0x04004630 RID: 17968
	[PublicizedFrom(EAccessModifier.Private)]
	public short spawnCountMax;

	// Token: 0x04004631 RID: 17969
	public const int cTriggerFlagsMask = 7;

	// Token: 0x04004632 RID: 17970
	public const int cFlagsHasScript = 16;

	// Token: 0x04004633 RID: 17971
	[PublicizedFrom(EAccessModifier.Private)]
	public int flags;

	// Token: 0x04004634 RID: 17972
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SleeperVolume.SpawnPoint> spawnPointList = new List<SleeperVolume.SpawnPoint>();

	// Token: 0x04004635 RID: 17973
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> spawnsAvailable;

	// Token: 0x04004636 RID: 17974
	[PublicizedFrom(EAccessModifier.Private)]
	public string groupName;

	// Token: 0x04004637 RID: 17975
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SleeperVolume.GroupCount> groupCountList;

	// Token: 0x04004638 RID: 17976
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, SleeperVolume.RespawnData> respawnMap = new Dictionary<int, SleeperVolume.RespawnData>();

	// Token: 0x04004639 RID: 17977
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> respawnList;

	// Token: 0x0400463A RID: 17978
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> pendingSpawnMap = new HashSet<int>();

	// Token: 0x0400463B RID: 17979
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<EntityAsyncManager.EntityCreateHandle> pendingSpawnOps = new Queue<EntityAsyncManager.EntityCreateHandle>();

	// Token: 0x0400463C RID: 17980
	[PublicizedFrom(EAccessModifier.Private)]
	public int gameStage;

	// Token: 0x0400463D RID: 17981
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastClassId;

	// Token: 0x0400463E RID: 17982
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong respawnTime = ulong.MaxValue;

	// Token: 0x0400463F RID: 17983
	[PublicizedFrom(EAccessModifier.Private)]
	public const uint cRespawnNever = 4294967295U;

	// Token: 0x04004640 RID: 17984
	[PublicizedFrom(EAccessModifier.Private)]
	public int numSpawned;

	// Token: 0x04004641 RID: 17985
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSpawned;

	// Token: 0x04004642 RID: 17986
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSpawning;

	// Token: 0x04004643 RID: 17987
	[PublicizedFrom(EAccessModifier.Private)]
	public int spawnDelay;

	// Token: 0x04004644 RID: 17988
	[PublicizedFrom(EAccessModifier.Private)]
	public int ticksUntilDespawn;

	// Token: 0x04004645 RID: 17989
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer playerTouchedToUpdate;

	// Token: 0x04004646 RID: 17990
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer playerTouchedTrigger;

	// Token: 0x04004647 RID: 17991
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasPassives;

	// Token: 0x04004648 RID: 17992
	[PublicizedFrom(EAccessModifier.Private)]
	public SleeperVolume.ETriggerType triggerState = SleeperVolume.ETriggerType.Passive;

	// Token: 0x04004649 RID: 17993
	public bool wasCleared;

	// Token: 0x0400464A RID: 17994
	public bool isQuestExclude;

	// Token: 0x0400464B RID: 17995
	public bool isPriority;

	// Token: 0x0400464C RID: 17996
	public Vector3i BoxMin;

	// Token: 0x0400464D RID: 17997
	public Vector3i BoxMax;

	// Token: 0x0400464E RID: 17998
	public Vector3 Center;

	// Token: 0x0400464F RID: 17999
	public List<byte> TriggeredByIndices = new List<byte>();

	// Token: 0x04004650 RID: 18000
	[PublicizedFrom(EAccessModifier.Private)]
	public MinScript minScript;

	// Token: 0x04004651 RID: 18001
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWanderingCountdown = 10;

	// Token: 0x04004652 RID: 18002
	[PublicizedFrom(EAccessModifier.Private)]
	public static int wanderingCountdown = 5;

	// Token: 0x04004653 RID: 18003
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameRandom sleeperRandom;

	// Token: 0x04004654 RID: 18004
	public static int TickSpawnCount;

	// Token: 0x04004655 RID: 18005
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSpawnPerTickMax = 2;

	// Token: 0x04004656 RID: 18006
	[PublicizedFrom(EAccessModifier.Private)]
	public static float[] difficultyTierScale = new float[] { 1f, 1f, 1f, 0.9f, 0.9f, 0.9f, 0.9f };

	// Token: 0x04004657 RID: 18007
	[PublicizedFrom(EAccessModifier.Private)]
	public static float[][] isHiddenOffsets = new float[][]
	{
		new float[]
		{
			-0.7f, 0.3f, 0f, 0.3f, 0.7f, 0.3f, -0.7f, 0.8f, 0f, 0.8f,
			0.7f, 0.8f
		},
		new float[]
		{
			-0.4f, 0.5f, 0f, 0.5f, 0.4f, 0.5f, -0.4f, 1.5f, 0f, 1.5f,
			0.4f, 1.5f
		}
	};

	// Token: 0x04004658 RID: 18008
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagsQuestExclude = 1;

	// Token: 0x04004659 RID: 18009
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagsPriority = 2;

	// Token: 0x0400465A RID: 18010
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagsSpawning = 4;

	// Token: 0x0400465B RID: 18011
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cFlagsCleared = 8;

	// Token: 0x02000BB2 RID: 2994
	public enum ETriggerType
	{
		// Token: 0x0400465D RID: 18013
		Active,
		// Token: 0x0400465E RID: 18014
		Passive,
		// Token: 0x0400465F RID: 18015
		Attack,
		// Token: 0x04004660 RID: 18016
		Trigger,
		// Token: 0x04004661 RID: 18017
		Wander
	}

	// Token: 0x02000BB3 RID: 2995
	[PublicizedFrom(EAccessModifier.Private)]
	public struct GroupCount
	{
		// Token: 0x04004662 RID: 18018
		public string groupName;

		// Token: 0x04004663 RID: 18019
		public int count;
	}

	// Token: 0x02000BB4 RID: 2996
	[PublicizedFrom(EAccessModifier.Private)]
	public struct RespawnData
	{
		// Token: 0x04004664 RID: 18020
		public string className;

		// Token: 0x04004665 RID: 18021
		public int spawnPointIndex;
	}

	// Token: 0x02000BB5 RID: 2997
	public struct SpawnPoint
	{
		// Token: 0x06005AA5 RID: 23205 RVA: 0x0022DC6B File Offset: 0x0022BE6B
		public SpawnPoint(Vector3i _pos, float _rot, int _blockType)
		{
			this.pos = _pos;
			this.rot = _rot;
			this.blockType = _blockType;
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x0022DC84 File Offset: 0x0022BE84
		public BlockSleeper GetBlock()
		{
			BlockSleeper blockSleeper = Block.list[this.blockType] as BlockSleeper;
			if (blockSleeper == null)
			{
				blockSleeper = (BlockSleeper)Block.GetBlockByName("sleeperSit", false);
			}
			return blockSleeper;
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x0022DCB8 File Offset: 0x0022BEB8
		public static SleeperVolume.SpawnPoint Read(BinaryReader _br, int _version)
		{
			Vector3i vector3i = new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
			if (_version >= 7 && _version < 20)
			{
				_br.ReadSingle();
				_br.ReadSingle();
				_br.ReadSingle();
			}
			float num = _br.ReadSingle();
			if (_version < 20)
			{
				_br.ReadByte();
			}
			int num2 = 0;
			if (_version > 14)
			{
				string text = _br.ReadString();
				Block blockByName = Block.GetBlockByName(text, false);
				if (blockByName != null)
				{
					num2 = blockByName.blockID;
				}
				else
				{
					Log.Warning("SpawnPoint Read missing block {0}", new object[] { text });
				}
			}
			else if (_version >= 9)
			{
				num2 = (int)_br.ReadUInt16();
			}
			return new SleeperVolume.SpawnPoint(vector3i, num, num2);
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x0022DD60 File Offset: 0x0022BF60
		public void Write(BinaryWriter _bw)
		{
			_bw.Write(this.pos.x);
			_bw.Write(this.pos.y);
			_bw.Write(this.pos.z);
			_bw.Write(this.rot);
			_bw.Write(this.GetBlock().GetBlockName());
		}

		// Token: 0x04004666 RID: 18022
		public readonly Vector3i pos;

		// Token: 0x04004667 RID: 18023
		public readonly float rot;

		// Token: 0x04004668 RID: 18024
		public readonly int blockType;
	}
}
