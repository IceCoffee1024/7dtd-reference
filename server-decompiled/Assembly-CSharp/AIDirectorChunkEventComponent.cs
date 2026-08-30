using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200040A RID: 1034
[Preserve]
public class AIDirectorChunkEventComponent : AIDirectorHordeComponent
{
	// Token: 0x06001FFF RID: 8191 RVA: 0x000C1F89 File Offset: 0x000C0189
	public void Clear()
	{
		this.activeChunks.Clear();
		this.checkChunks.Clear();
	}

	// Token: 0x06002000 RID: 8192 RVA: 0x000C1FA4 File Offset: 0x000C01A4
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

	// Token: 0x06002001 RID: 8193 RVA: 0x000C209C File Offset: 0x000C029C
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

	// Token: 0x06002002 RID: 8194 RVA: 0x000C2154 File Offset: 0x000C0354
	public override void Read(BinaryReader _stream, int _outerVersion)
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

	// Token: 0x06002003 RID: 8195 RVA: 0x000C21B0 File Offset: 0x000C03B0
	public override void Write(BinaryWriter _stream)
	{
		_stream.Write(1);
		_stream.Write(this.activeChunks.Count);
		foreach (KeyValuePair<long, AIDirectorChunkData> keyValuePair in this.activeChunks)
		{
			_stream.Write(keyValuePair.Key);
			keyValuePair.Value.Write(_stream);
		}
	}

	// Token: 0x06002004 RID: 8196 RVA: 0x000C2230 File Offset: 0x000C0430
	public int GetActiveCount()
	{
		return this.activeChunks.Count;
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06002005 RID: 8197 RVA: 0x000C223D File Offset: 0x000C043D
	public bool HasAnySpawns
	{
		get
		{
			return this.hordeSpawnList.Count != 0;
		}
	}

	// Token: 0x06002006 RID: 8198 RVA: 0x000C2250 File Offset: 0x000C0450
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

	// Token: 0x06002007 RID: 8199 RVA: 0x000C22A8 File Offset: 0x000C04A8
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

	// Token: 0x06002008 RID: 8200 RVA: 0x000C232C File Offset: 0x000C052C
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

	// Token: 0x06002009 RID: 8201 RVA: 0x000C2370 File Offset: 0x000C0570
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

	// Token: 0x0600200A RID: 8202 RVA: 0x000C23AC File Offset: 0x000C05AC
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

	// Token: 0x0600200B RID: 8203 RVA: 0x000C243C File Offset: 0x000C063C
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

	// Token: 0x0600200C RID: 8204 RVA: 0x000C250C File Offset: 0x000C070C
	public AIScoutHordeSpawner.IHorde CreateHorde(Vector3 startPos)
	{
		AIDirectorChunkEventComponent.Horde horde = new AIDirectorChunkEventComponent.Horde(this, startPos);
		this.hordeSpawnList.Add(horde);
		return horde;
	}

	// Token: 0x040015C5 RID: 5573
	public const int cVersion = 1;

	// Token: 0x040015C6 RID: 5574
	public const int cChunksPerArea = 5;

	// Token: 0x040015C7 RID: 5575
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cEventDelay = 5f;

	// Token: 0x040015C8 RID: 5576
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivityLevelToSpawn = 25f;

	// Token: 0x040015C9 RID: 5577
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSpawnChance = 0.2f;

	// Token: 0x040015CA RID: 5578
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, AIDirectorChunkData> activeChunks = new Dictionary<long, AIDirectorChunkData>();

	// Token: 0x040015CB RID: 5579
	[PublicizedFrom(EAccessModifier.Private)]
	public List<long> removeChunks = new List<long>();

	// Token: 0x040015CC RID: 5580
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIScoutHordeSpawner> scoutSpawnList = new List<AIScoutHordeSpawner>();

	// Token: 0x040015CD RID: 5581
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorChunkEventComponent.Horde> hordeSpawnList = new List<AIDirectorChunkEventComponent.Horde>();

	// Token: 0x040015CE RID: 5582
	[PublicizedFrom(EAccessModifier.Private)]
	public List<AIDirectorChunkData> checkChunks = new List<AIDirectorChunkData>();

	// Token: 0x040015CF RID: 5583
	[PublicizedFrom(EAccessModifier.Private)]
	public float spawnDelay;

	// Token: 0x040015D0 RID: 5584
	[PublicizedFrom(EAccessModifier.Private)]
	public static int[] neighbors = new int[]
	{
		-1, 0, 1, 0, 0, -1, 0, 1, -1, -1,
		1, -1, -1, 1, 1, 1
	};

	// Token: 0x0200040B RID: 1035
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public class Horde : AIScoutHordeSpawner.IHorde
	{
		// Token: 0x0600200F RID: 8207 RVA: 0x000C2586 File Offset: 0x000C0786
		public Horde(AIDirectorChunkEventComponent outer, Vector3 pos)
		{
			this._outer = outer;
			this._pos = pos;
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x000C259C File Offset: 0x000C079C
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

		// Token: 0x06002011 RID: 8209 RVA: 0x000C2615 File Offset: 0x000C0815
		public void SetSpawnPos(Vector3 pos)
		{
			if (this._horde != null)
			{
				this._horde.targetPos = pos;
			}
			this._pos = pos;
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x000C2632 File Offset: 0x000C0832
		public void Destroy()
		{
			if (this._horde != null)
			{
				this._horde.Cleanup();
			}
			this._horde = null;
			this._destroy = true;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x000C2655 File Offset: 0x000C0855
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

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x000C268A File Offset: 0x000C088A
		public bool canSpawnMore
		{
			get
			{
				return this._numSpawned < 25;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06002015 RID: 8213 RVA: 0x000C2696 File Offset: 0x000C0896
		public bool isSpawning
		{
			get
			{
				return this._horde != null && this._horde.isSpawning;
			}
		}

		// Token: 0x040015D1 RID: 5585
		[PublicizedFrom(EAccessModifier.Private)]
		public AIDirectorChunkEventComponent _outer;

		// Token: 0x040015D2 RID: 5586
		[PublicizedFrom(EAccessModifier.Private)]
		public AIHordeSpawner _horde;

		// Token: 0x040015D3 RID: 5587
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3 _pos;

		// Token: 0x040015D4 RID: 5588
		[PublicizedFrom(EAccessModifier.Private)]
		public int _numSpawned;

		// Token: 0x040015D5 RID: 5589
		[PublicizedFrom(EAccessModifier.Private)]
		public bool _destroy;
	}
}
