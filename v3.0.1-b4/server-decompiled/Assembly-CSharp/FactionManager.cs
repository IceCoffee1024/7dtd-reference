using System;
using System.IO;
using UnityEngine;

// Token: 0x0200051B RID: 1307
public class FactionManager
{
	// Token: 0x06002B0D RID: 11021 RVA: 0x001104F0 File Offset: 0x0010E6F0
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

	// Token: 0x06002B0E RID: 11022 RVA: 0x0011052C File Offset: 0x0010E72C
	public static void Init()
	{
		FactionManager.Instance = new FactionManager();
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x00110538 File Offset: 0x0010E738
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

	// Token: 0x06002B10 RID: 11024 RVA: 0x001105DC File Offset: 0x0010E7DC
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

	// Token: 0x06002B11 RID: 11025 RVA: 0x00110670 File Offset: 0x0010E870
	public Faction CreateFaction(string _name = "", bool _playerFaction = true, string _icon = "")
	{
		Faction faction = new Faction(_name, _playerFaction, _icon);
		this.AddFaction(faction);
		return faction;
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x00110690 File Offset: 0x0010E890
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

	// Token: 0x06002B13 RID: 11027 RVA: 0x001106D7 File Offset: 0x0010E8D7
	public void RemoveFaction(byte _id)
	{
		this.Factions[(int)_id] = null;
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x001106E2 File Offset: 0x0010E8E2
	public Faction GetFaction(byte _id)
	{
		return this.Factions[(int)_id];
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x001106EC File Offset: 0x0010E8EC
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

	// Token: 0x06002B16 RID: 11030 RVA: 0x0011072C File Offset: 0x0010E92C
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

	// Token: 0x06002B17 RID: 11031 RVA: 0x001107A1 File Offset: 0x0010E9A1
	public void SetRelationship(byte _myFaction, byte _targetFaction, sbyte _modification)
	{
		if (this.Factions[(int)_myFaction] != null)
		{
			this.Factions[(int)_myFaction].ModifyRelationship(_targetFaction, (float)_modification);
		}
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x001107A1 File Offset: 0x0010E9A1
	public void ModifyRelationship(byte _myFaction, byte _targetFaction, sbyte _modification)
	{
		if (this.Factions[(int)_myFaction] != null)
		{
			this.Factions[(int)_myFaction].ModifyRelationship(_targetFaction, (float)_modification);
		}
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x001107C0 File Offset: 0x0010E9C0
	public void Write(BinaryWriter _bw)
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

	// Token: 0x06002B1A RID: 11034 RVA: 0x00110814 File Offset: 0x0010EA14
	public void Read(BinaryReader _br)
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

	// Token: 0x06002B1B RID: 11035 RVA: 0x0011085C File Offset: 0x0010EA5C
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

	// Token: 0x06002B1C RID: 11036 RVA: 0x001108D8 File Offset: 0x0010EAD8
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

	// Token: 0x06002B1D RID: 11037 RVA: 0x00110964 File Offset: 0x0010EB64
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

	// Token: 0x040020DD RID: 8413
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SAVE_TIME_SEC = 60f;

	// Token: 0x040020DE RID: 8414
	public static FactionManager Instance;

	// Token: 0x040020DF RID: 8415
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte Version = 1;

	// Token: 0x040020E0 RID: 8416
	[PublicizedFrom(EAccessModifier.Private)]
	public Faction[] Factions = new Faction[255];

	// Token: 0x040020E1 RID: 8417
	[PublicizedFrom(EAccessModifier.Private)]
	public float saveTime = 60f;

	// Token: 0x040020E2 RID: 8418
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo dataSaveThreadInfo;

	// Token: 0x040020E3 RID: 8419
	[PublicizedFrom(EAccessModifier.Private)]
	public float rel;

	// Token: 0x0200051C RID: 1308
	public enum Relationship
	{
		// Token: 0x040020E5 RID: 8421
		Hate,
		// Token: 0x040020E6 RID: 8422
		Dislike = 200,
		// Token: 0x040020E7 RID: 8423
		Neutral = 400,
		// Token: 0x040020E8 RID: 8424
		Like = 600,
		// Token: 0x040020E9 RID: 8425
		Love = 800,
		// Token: 0x040020EA RID: 8426
		Leader = 1001
	}
}
