using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000412 RID: 1042
[Preserve]
public class AIDirectorChunkEventComponent : AIDirectorHordeComponent
{
	// Token: 0x06002010 RID: 8208 RVA: 0x000C2B85 File Offset: 0x000C0D85
	public void Clear()
	{
		this.activeChunks.Clear();
		this.checkChunks.Clear();
	}

	// Token: 0x06002011 RID: 8209 RVA: 0x000C2BA0 File Offset: 0x000C0DA0
	public override void Tick(double _dt)
	{
		base.Tick(_dt);
		float num = (float)_dt;
		this.spawnDelay -= num;
		if (this.spawnDelay <= 0f)
		{
			this.spawnDelay = 5f;
			this.CheckToSpawn();
			foreach (KeyValuePair<long, AIDirectorChunkData> keyValuePair in this.activeChunks)
			{
				if (!keyValuePair.Value.Tick(5f))
				{
					this.removeChunks.Add(keyValuePair.Key);
				}
			}
			if (this.removeChunks.Count > 0)
			{
				for (int i = 0; i < this.removeChunks.Count; i++)
				{
					this.activeChunks.Remove(this.removeChunks[i]);
				}
				this.removeChunks.Clear();
			}
		}
		this.TickActiveSpawns(num);
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x000C2C98 File Offset: 0x000C0E98
	[PublicizedFrom(EAccessModifier.Private)]
	public void TickActiveSpawns(float dt)
	{
		for (int i = this.scoutSpawnList.Count - 1; i >= 0; i--)
		{
			if (this.scoutSpawnList[i].Update(this.Director.World, dt))
			{
				AIDirector.LogAIExtra("Scout horde spawn finished (all mobs spawned)", Array.Empty<object>());
				this.scoutSpawnList[i].Cleanup();
				this.scoutSpawnList.RemoveAt(i);
			}
		}
		for (int j = this.hordeSpawnList.Count - 1; j >= 0; j--)
		{
			if (this.hordeSpawnList[j].Tick((double)dt))
			{
				AIDirector.LogAIExtra("Scout triggered horde finished (all mobs spawned)", Array.Empty<object>());
				this.hordeSpawnList.RemoveAt(j);
			}
		}
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x000C2D50 File Offset: 0x000C0F50
	public override void Read(PooledBinaryReader _stream, int _outerVersion)
	{
		if (_outerVersion >= 5)
		{
			this.activeChunks.Clear();
			int num = _stream.ReadInt32();
			int num2 = _stream.ReadInt32();
			for (int i = 0; i < num2; i++)
			{
				long num3 = _stream.ReadInt64();
				AIDirectorChunkData aidirectorChunkData = new AIDirectorChunkData();
				aidirectorChunkData.Read(_stream, num);
				this.activeChunks[num3] = aidirectorChunkData;
			}
		}
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x000C2DAC File Offset: 0x000C0FAC
	public override void Write(PooledBinaryWriter _stream)
	{
		_stream.Write(1);
		_stream.Write(this.activeChunks.Count);
		foreach (KeyValuePair<long, AIDirectorChunkData> keyValuePair in this.activeChunks)
		{
			_stream.Write(keyValuePair.Key);
			keyValuePair.Value.Write(_stream);
		}
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x000C2E2C File Offset: 0x000C102C
	public int GetActiveCount()
	{
		return this.activeChunks.Count;
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x06002016 RID: 8214 RVA: 0x000C2E39 File Offset: 0x000C1039
	public bool HasAnySpawns
	{
		get
		{
			return this.hordeSpawnList.Count != 0;
		}
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x000C2E4C File Offset: 0x000C104C
	public AIDirectorChunkData GetChunkDataFromPosition(Vector3i _position, bool _createIfNeeded)
	{
		int num = World.toChunkXZ(_position.x) / 5;
		int num2 = World.toChunkXZ(_position.z) / 5;
		long num3 = WorldChunkCache.MakeChunkKey(num, num2);
		AIDirectorChunkData aidirectorChunkData;
		if (this.activeChunks.TryGetValue(num3, out aidirectorChunkData))
		{
			return aidirectorChunkData;
		}
		if (_createIfNeeded)
		{
			aidirectorChunkData = new AIDirectorChunkData();
			this.activeChunks[num3] = aidirectorChunkData;
		}
		return aidirectorChunkData;
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x000C2EA4 File Offset: 0x000C10A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void StartCooldownOnNeighbors(Vector3i _position, bool _isLong)
	{
		int num = World.toChunkXZ(_position.x) / 5;
		int num2 = World.toChunkXZ(_position.z) / 5;
		for (int i = 0; i < AIDirectorChunkEventComponent.neighbors.Length; i += 2)
		{
			long num3 = WorldChunkCache.MakeChunkKey(num + AIDirectorChunkEventComponent.neighbors[i], num2 + AIDirectorChunkEventComponent.neighbors[i + 1]);
			AIDirectorChunkData aidirectorChunkData;
			if (!this.activeChunks.TryGetValue(num3, out aidirectorChunkData))
			{
				aidirectorChunkData = new AIDirectorChunkData();
				this.activeChunks[num3] = aidirectorChunkData;
			}
			aidirectorChunkData.StartNeighborCooldown(_isLong);
		}
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x000C2F28 File Offset: 0x000C1128
	public void NotifyEvent(AIDirectorChunkEvent _chunkEvent)
	{
		AIDirectorChunkData chunkDataFromPosition = this.GetChunkDataFromPosition(_chunkEvent.Position, true);
		if (chunkDataFromPosition.IsReady)
		{
			chunkDataFromPosition.AddEvent(_chunkEvent);
			if (!this.checkChunks.Contains(chunkDataFromPosition))
			{
				this.checkChunks.Add(chunkDataFromPosition);
			}
		}
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x000C2F6C File Offset: 0x000C116C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckToSpawn()
	{
		if (this.checkChunks.Count > 0)
		{
			AIDirectorChunkData aidirectorChunkData = this.checkChunks[0];
			this.checkChunks.RemoveAt(0);
			this.CheckToSpawn(aidirectorChunkData);
		}
	}

	// Token: 0x0600201B RID: 8219 RVA: 0x000C2FA8 File Offset: 0x000C11A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckToSpawn(AIDirectorChunkData _chunkData)
	{
		if (GameStats.GetBool(EnumGameStats.ZombieHordeMeter) && GameStats.GetBool(EnumGameStats.IsSpawnEnemies) && _chunkData.ActivityLevel >= 25f)
		{
			AIDirectorChunkEvent aidirectorChunkEvent = _chunkData.FindBestEventAndReset();
			if (aidirectorChunkEvent != null)
			{
				bool flag = this.Director.random.RandomFloat < 0.2f && !GameUtils.IsPlaytesting();
				this.StartCooldownOnNeighbors(aidirectorChunkEvent.Position, flag);
				if (flag)
				{
					_chunkData.SetLongDelay();
					this.SpawnScouts(aidirectorChunkEvent.Position.ToVector3());
					return;
				}
			}
			else
			{
				AIDirector.LogAI("Chunk event not found!", Array.Empty<object>());
			}
		}
	}

	// Token: 0x0600201C RID: 8220 RVA: 0x000C3038 File Offset: 0x000C1238
	public void SpawnScouts(Vector3 targetPos)
	{
		Vector3 vector;
		if (base.FindScoutStartPos(targetPos, out vector))
		{
			EntityPlayer closestPlayer = this.Director.World.GetClosestPlayer(targetPos, 120f, false);
			if (closestPlayer)
			{
				int num = GameStageDefinition.CalcGameStageAround(closestPlayer);
				string text = "ScoutsRadiated";
				if (num < 45)
				{
					text = "Scouts1";
				}
				else if (num < 85)
				{
					text = "Scouts2";
				}
				else if (num < 125)
				{
					text = "ScoutsFeral";
				}
				EntitySpawner entitySpawner = new EntitySpawner(text, Vector3i.zero, Vector3i.zero, 0, null);
				this.scoutSpawnList.Add(new AIScoutHordeSpawner(entitySpawner, vector, targetPos, false));
				AIDirector.LogAI("Spawning {0} at {1}, to {2}", new object[]
				{
					text,
					vector.ToCultureInvariantString(),
					targetPos.ToCultureInvariantString()
				});
				return;
			}
		}
		else
		{
			AIDirector.LogAI("Scout spawning failed", Array.Empty<object>());
		}
	}

	// Token: 0x0600201D RID: 8221 RVA: 0x000C3108 File Offset: 0x000C1308
	public AIScoutHordeSpawner.IHorde CreateHorde(Vector3 startPos)
	{
		AIDirectorChunkEventComponent.Horde horde = new AIDirectorChunkEventComponent.Horde(this, startPos);
		this.hordeSpawnList.Add(horde);
		return horde;
	}

	// Token: 0x040015E1 RID: 5601
	public const int cVersion = 1;

	// Token: 0x040015E2 RID: 5602
	public const int cChunksPerArea = 5;

	// Token: 0x040015E3 RID: 5603
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cEventDelay = 5f;

	// Token: 0x040015E4 RID: 5604
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivityLevelToSpawn = 25f;

	// Token: 0x040015E5 RID: 5605
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSpawnChance = 0.2f;

	// Token: 0x040015E6 RID: 5606
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, AIDirectorChunkData> activeChunks = new Dictionary<long, AIDirectorChunkData>();

	// Token: 0x040015E7 RID: 5607
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> removeChunks = new List<long>();

	// Token: 0x040015E8 RID: 5608
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIScoutHordeSpawner> scoutSpawnList = new List<AIScoutHordeSpawner>();

	// Token: 0x040015E9 RID: 5609
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorChunkEventComponent.Horde> hordeSpawnList = new List<AIDirectorChunkEventComponent.Horde>();

	// Token: 0x040015EA RID: 5610
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorChunkData> checkChunks = new List<AIDirectorChunkData>();

	// Token: 0x040015EB RID: 5611
	[PublicizedFrom(EAccessModifier.Private)]
	public float spawnDelay;

	// Token: 0x040015EC RID: 5612
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[] neighbors = new int[]
	{
		-1, 0, 1, 0, 0, -1, 0, 1, -1, -1,
		1, -1, -1, 1, 1, 1
	};

	// Token: 0x02000413 RID: 1043
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class Horde : AIScoutHordeSpawner.IHorde
	{
		// Token: 0x06002020 RID: 8224 RVA: 0x000C3182 File Offset: 0x000C1382
		public Horde(AIDirectorChunkEventComponent outer, Vector3 pos)
		{
			this._outer = outer;
			this._pos = pos;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000C3198 File Offset: 0x000C1398
		public void SpawnMore(int size)
		{
			int num = this._numSpawned + size;
			int num2 = num - this._numSpawned;
			this._numSpawned = num;
			if (this._horde != null)
			{
				this._horde.numToSpawn += num2;
				return;
			}
			this._horde = new AIHordeSpawner(this._outer.Director.World, "ScoutGSList", this._pos, 30f);
			this._horde.numToSpawn = num2;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x000C3211 File Offset: 0x000C1411
		public void SetSpawnPos(Vector3 pos)
		{
			if (this._horde != null)
			{
				this._horde.targetPos = pos;
			}
			this._pos = pos;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x000C322E File Offset: 0x000C142E
		public void Destroy()
		{
			if (this._horde != null)
			{
				this._horde.Cleanup();
			}
			this._horde = null;
			this._destroy = true;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x000C3251 File Offset: 0x000C1451
		public bool Tick(double dt)
		{
			if (this._destroy)
			{
				return true;
			}
			if (this._horde != null && this._horde.Tick(dt))
			{
				this._horde.Cleanup();
				this._horde = null;
			}
			return false;
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x000C3286 File Offset: 0x000C1486
		public bool canSpawnMore
		{
			get
			{
				return this._numSpawned < 25;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x000C3292 File Offset: 0x000C1492
		public bool isSpawning
		{
			get
			{
				return this._horde != null && this._horde.isSpawning;
			}
		}

		// Token: 0x040015ED RID: 5613
		[PublicizedFrom(EAccessModifier.Private)]
		public AIDirectorChunkEventComponent _outer;

		// Token: 0x040015EE RID: 5614
		[PublicizedFrom(EAccessModifier.Private)]
		public AIHordeSpawner _horde;

		// Token: 0x040015EF RID: 5615
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 _pos;

		// Token: 0x040015F0 RID: 5616
		[PublicizedFrom(EAccessModifier.Private)]
		public int _numSpawned;

		// Token: 0x040015F1 RID: 5617
		[PublicizedFrom(EAccessModifier.Private)]
		public bool _destroy;
	}
}
