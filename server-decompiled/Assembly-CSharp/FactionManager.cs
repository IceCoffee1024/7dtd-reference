using System;
using System.IO;
using UnityEngine;

// Token: 0x02000526 RID: 1318
public class FactionManager
{
	// Token: 0x06002B2D RID: 11053 RVA: 0x00112338 File Offset: 0x00110538
	public void PrintData()
	{
		for (int i = 0; i < this.Factions.Length; i++)
		{
			if (this.Factions[i] != null)
			{
				Log.Out(this.Factions[i].ToString());
			}
		}
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x00112374 File Offset: 0x00110574
	public static void Init()
	{
		FactionManager.Instance = new FactionManager();
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x00112380 File Offset: 0x00110580
	public void Update()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || GameManager.Instance.World == null || GameManager.Instance.World.Players == null || GameManager.Instance.World.Players.Count == 0 || GameManager.Instance.gameStateManager.IsGameStarted())
		{
			return;
		}
		this.saveTime -= Time.deltaTime;
		if (this.saveTime <= 0f && (this.dataSaveThreadInfo == null || this.dataSaveThreadInfo.HasTerminated()))
		{
			this.saveTime = 60f;
			this.Save();
		}
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x00112424 File Offset: 0x00110624
	public FactionManager.Relationship GetRelationshipTier(EntityAlive checkingEntity, EntityAlive targetEntity)
	{
		if (checkingEntity == null || targetEntity == null)
		{
			return FactionManager.Relationship.Neutral;
		}
		this.rel = this.GetRelationshipValue(checkingEntity, targetEntity);
		if (this.rel < 200f)
		{
			return FactionManager.Relationship.Hate;
		}
		if (this.rel < 400f)
		{
			return FactionManager.Relationship.Dislike;
		}
		if (this.rel < 600f)
		{
			return FactionManager.Relationship.Neutral;
		}
		if (this.rel < 800f)
		{
			return FactionManager.Relationship.Like;
		}
		if (this.rel < 1001f)
		{
			return FactionManager.Relationship.Love;
		}
		return FactionManager.Relationship.Leader;
	}

	// Token: 0x06002B31 RID: 11057 RVA: 0x001124B8 File Offset: 0x001106B8
	public Faction CreateFaction(string _name = "", bool _playerFaction = true, string _icon = "")
	{
		Faction faction = new Faction(_name, _playerFaction, _icon);
		this.AddFaction(faction);
		return faction;
	}

	// Token: 0x06002B32 RID: 11058 RVA: 0x001124D8 File Offset: 0x001106D8
	public void AddFaction(Faction _faction)
	{
		for (int i = (_faction.IsPlayerFaction ? 8 : 0); i < this.Factions.Length; i++)
		{
			if (this.Factions[i] == null)
			{
				this.Factions[i] = _faction;
				_faction.ID = (byte)i;
				return;
			}
		}
	}

	// Token: 0x06002B33 RID: 11059 RVA: 0x0011251F File Offset: 0x0011071F
	public void RemoveFaction(byte _id)
	{
		this.Factions[(int)_id] = null;
	}

	// Token: 0x06002B34 RID: 11060 RVA: 0x0011252A File Offset: 0x0011072A
	public Faction GetFaction(byte _id)
	{
		return this.Factions[(int)_id];
	}

	// Token: 0x06002B35 RID: 11061 RVA: 0x00112534 File Offset: 0x00110734
	public Faction GetFactionByName(string _name)
	{
		for (int i = 0; i < this.Factions.Length; i++)
		{
			if (this.Factions[i].Name == _name)
			{
				return this.Factions[i];
			}
		}
		return null;
	}

	// Token: 0x06002B36 RID: 11062 RVA: 0x00112574 File Offset: 0x00110774
	public float GetRelationshipValue(EntityAlive checkingEntity, EntityAlive targetEntity)
	{
		if (checkingEntity == null || targetEntity == null)
		{
			return 400f;
		}
		if (checkingEntity.factionId == targetEntity.factionId)
		{
			return 800f;
		}
		if (this.Factions[(int)checkingEntity.factionId] != null && this.Factions[(int)targetEntity.factionId] != null)
		{
			return this.Factions[(int)checkingEntity.factionId].GetRelationship(targetEntity.factionId);
		}
		return 400f;
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x001125E9 File Offset: 0x001107E9
	public void SetRelationship(byte _myFaction, byte _targetFaction, sbyte _modification)
	{
		if (this.Factions[(int)_myFaction] != null)
		{
			this.Factions[(int)_myFaction].ModifyRelationship(_targetFaction, (float)_modification);
		}
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x001125E9 File Offset: 0x001107E9
	public void ModifyRelationship(byte _myFaction, byte _targetFaction, sbyte _modification)
	{
		if (this.Factions[(int)_myFaction] != null)
		{
			this.Factions[(int)_myFaction].ModifyRelationship(_targetFaction, (float)_modification);
		}
	}

	// Token: 0x06002B39 RID: 11065 RVA: 0x00112608 File Offset: 0x00110808
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(FactionManager.Version);
		for (int i = 0; i < this.Factions.Length; i++)
		{
			_bw.Write(this.Factions[i] != null);
			if (this.Factions[i] != null)
			{
				this.Factions[i].Write(_bw);
			}
		}
	}

	// Token: 0x06002B3A RID: 11066 RVA: 0x0011265C File Offset: 0x0011085C
	public void Read(PooledBinaryReader _br)
	{
		_br.ReadByte();
		for (int i = 0; i < 255; i++)
		{
			if (_br.ReadBoolean())
			{
				this.Factions[i] = new Faction();
				this.Factions[i].Read(_br);
			}
		}
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x001126A4 File Offset: 0x001108A4
	[PublicizedFrom(EAccessModifier.Private)]
	public int saveFactionDataThreaded(ThreadManager.ThreadInfo _threadInfo)
	{
		PooledExpandableMemoryStream pooledExpandableMemoryStream = (PooledExpandableMemoryStream)_threadInfo.parameter;
		string text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "factions.dat");
		if (!SdDirectory.Exists(GameIO.GetSaveGameDir()))
		{
			return -1;
		}
		if (SdFile.Exists(text))
		{
			SdFile.Copy(text, string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "factions.dat.bak"), true);
		}
		pooledExpandableMemoryStream.Position = 0L;
		StreamUtils.WriteStreamToFile(pooledExpandableMemoryStream, text);
		MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		return -1;
	}

	// Token: 0x06002B3C RID: 11068 RVA: 0x00112720 File Offset: 0x00110920
	public void Save()
	{
		if (this.dataSaveThreadInfo == null || !ThreadManager.ActiveThreads.ContainsKey("factionDataSave"))
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				this.Write(pooledBinaryWriter);
			}
			this.dataSaveThreadInfo = ThreadManager.StartThread("factionDataSave", null, new ThreadManager.ThreadFunctionLoopDelegate(this.saveFactionDataThreaded), null, pooledExpandableMemoryStream, null, false, true);
		}
	}

	// Token: 0x06002B3D RID: 11069 RVA: 0x001127AC File Offset: 0x001109AC
	public void Load()
	{
		string text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "factions.dat");
		if (SdDirectory.Exists(GameIO.GetSaveGameDir()) && SdFile.Exists(text))
		{
			try
			{
				using (Stream stream = SdFile.OpenRead(text))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream);
						this.Read(pooledBinaryReader);
					}
				}
			}
			catch (Exception)
			{
				text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "factions.dat.bak");
				if (SdFile.Exists(text))
				{
					using (Stream stream2 = SdFile.OpenRead(text))
					{
						using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader2.SetBaseStream(stream2);
							this.Read(pooledBinaryReader2);
						}
					}
				}
			}
		}
	}

	// Token: 0x04002114 RID: 8468
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SAVE_TIME_SEC = 60f;

	// Token: 0x04002115 RID: 8469
	public static FactionManager Instance;

	// Token: 0x04002116 RID: 8470
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte Version = 1;

	// Token: 0x04002117 RID: 8471
	[PublicizedFrom(EAccessModifier.Private)]
	public Faction[] Factions = new Faction[255];

	// Token: 0x04002118 RID: 8472
	[PublicizedFrom(EAccessModifier.Private)]
	public float saveTime = 60f;

	// Token: 0x04002119 RID: 8473
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo dataSaveThreadInfo;

	// Token: 0x0400211A RID: 8474
	[PublicizedFrom(EAccessModifier.Private)]
	public float rel;

	// Token: 0x02000527 RID: 1319
	public enum Relationship
	{
		// Token: 0x0400211C RID: 8476
		Hate,
		// Token: 0x0400211D RID: 8477
		Dislike = 200,
		// Token: 0x0400211E RID: 8478
		Neutral = 400,
		// Token: 0x0400211F RID: 8479
		Like = 600,
		// Token: 0x04002120 RID: 8480
		Love = 800,
		// Token: 0x04002121 RID: 8481
		Leader = 1001
	}
}
