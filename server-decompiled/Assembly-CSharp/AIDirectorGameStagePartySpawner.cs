using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Scripting;

// Token: 0x02000416 RID: 1046
[Preserve]
public class AIDirectorGameStagePartySpawner
{
	// Token: 0x06002031 RID: 8241 RVA: 0x000C2F84 File Offset: 0x000C1184
	public AIDirectorGameStagePartySpawner(World _world, string _gameStageName)
	{
		this.world = _world;
		this.def = GameStageDefinition.GetGameStage(_gameStageName);
		this.partyMembers = new ReadOnlyCollection<EntityPlayer>(this.members);
		this.partyLevel = -1;
		this.gsScaling = 1f;
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x000C2FE3 File Offset: 0x000C11E3
	public void SetScaling(float _scaling)
	{
		this.gsScaling = Utils.FastLerp(1f, 2.5f, (_scaling - 1f) / 3f);
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x000C3008 File Offset: 0x000C1208
	public void ResetPartyLevel(int mod = 0)
	{
		int num = this.CalcPartyLevel();
		if (mod != 0)
		{
			num %= mod;
		}
		this.SetPartyLevel(num);
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x000C302C File Offset: 0x000C122C
	public int CalcPartyLevel()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.members.Count; i++)
		{
			EntityPlayer entityPlayer = this.members[i];
			list.Add(entityPlayer.gameStage);
		}
		return GameStageDefinition.CalcPartyLevel(list);
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x000C3074 File Offset: 0x000C1274
	public void SetPartyLevel(int _partyLevel)
	{
		this.partyLevel = _partyLevel;
		this.partyLevel = (int)((float)this.partyLevel * this.gsScaling);
		this.stageSpawnMax = 0;
		this.groupIndex = 0;
		this.spawnCount = 0;
		if (this.def != null)
		{
			this.stage = this.def.GetStage(_partyLevel);
			if (this.stage != null)
			{
				this.stageSpawnMax = this.CalcStageSpawnMax();
				this.SetupGroup();
			}
		}
		this.bonusLootEvery = Utils.FastMax(this.stageSpawnMax / GameStageDefinition.LootBonusMaxCount, GameStageDefinition.LootBonusEvery);
		Log.Out("Party of {0}, GS {1} ({2}), scaling {3}, enemy max {4}, bonus every {5}", new object[]
		{
			this.members.Count,
			this.partyLevel,
			_partyLevel,
			this.gsScaling,
			this.stageSpawnMax,
			this.bonusLootEvery
		});
		Log.Out("Party members: ");
		for (int i = 0; i < this.members.Count; i++)
		{
			EntityPlayer entityPlayer = this.members[i];
			Log.Out("Player id {0}, gameStage {1}", new object[] { entityPlayer.entityId, entityPlayer.gameStage });
		}
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x000C31C0 File Offset: 0x000C13C0
	public bool Tick(double _deltaTime)
	{
		if (this.spawnGroup != null)
		{
			bool flag = false;
			if (this.nextStageTime > 0UL && this.world.worldTime >= this.nextStageTime)
			{
				flag = true;
			}
			else if (this.spawnCount >= this.numToSpawn)
			{
				this.interval -= _deltaTime;
				flag = this.interval <= 0.0;
			}
			if (flag)
			{
				this.groupIndex++;
				this.SetupGroup();
			}
		}
		return this.spawnGroup != null;
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x000C324C File Offset: 0x000C144C
	public void AddMember(EntityPlayer _player)
	{
		if (!this.memberIDs.Contains(_player.entityId))
		{
			this.memberIDs.Add(_player.entityId);
		}
		if (!this.members.Contains(_player))
		{
			this.members.Add(_player);
		}
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x000C3298 File Offset: 0x000C1498
	public bool IsMemberOfParty(int _entityID)
	{
		return this.memberIDs.Contains(_entityID);
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x000C32A6 File Offset: 0x000C14A6
	public void RemoveMember(EntityPlayer _player, bool removeID)
	{
		this.members.Remove(_player);
		if (removeID)
		{
			this.memberIDs.Remove(_player.entityId);
		}
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x000C32CC File Offset: 0x000C14CC
	[PublicizedFrom(EAccessModifier.Private)]
	public int CalcStageSpawnMax()
	{
		int num = 0;
		int count = this.stage.Count;
		for (int i = 0; i < count; i++)
		{
			this.spawnGroup = this.stage.GetSpawnGroup(i);
			num += (int)this.spawnGroup.spawnCount;
		}
		return num;
	}

	// Token: 0x0600203B RID: 8251 RVA: 0x000C3314 File Offset: 0x000C1514
	public void ClearMembers()
	{
		this.members.Clear();
		this.memberIDs.Clear();
	}

	// Token: 0x0600203C RID: 8252 RVA: 0x000C332C File Offset: 0x000C152C
	public void IncSpawnCount()
	{
		this.spawnCount++;
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x000C333C File Offset: 0x000C153C
	public void DecSpawnCount(int dec)
	{
		if (dec > this.spawnCount)
		{
			this.spawnCount = 0;
			return;
		}
		this.spawnCount -= dec;
	}

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x0600203E RID: 8254 RVA: 0x000C335D File Offset: 0x000C155D
	public bool IsDone
	{
		get
		{
			return this.groupIndex > 0 && this.spawnGroup == null;
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x0600203F RID: 8255 RVA: 0x000C3373 File Offset: 0x000C1573
	public bool canSpawn
	{
		get
		{
			return this.spawnGroup != null && this.spawnCount < this.numToSpawn;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06002040 RID: 8256 RVA: 0x000C338D File Offset: 0x000C158D
	public int maxAlive
	{
		get
		{
			if (this.spawnGroup == null)
			{
				return 0;
			}
			return (int)this.spawnGroup.maxAlive;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06002041 RID: 8257 RVA: 0x000C33A4 File Offset: 0x000C15A4
	public string spawnGroupName
	{
		get
		{
			if (this.spawnGroup == null)
			{
				return null;
			}
			return this.spawnGroup.groupName;
		}
	}

	// Token: 0x06002042 RID: 8258 RVA: 0x000C33BC File Offset: 0x000C15BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupGroup()
	{
		this.spawnGroup = this.stage.GetSpawnGroup(this.groupIndex);
		if (this.spawnGroup != null)
		{
			this.interval = (double)this.spawnGroup.interval;
			this.nextStageTime = ((this.spawnGroup.duration > 0) ? (this.world.worldTime + (ulong)(this.spawnGroup.duration * 1000)) : 0UL);
			this.numToSpawn = EntitySpawner.ModifySpawnCountByGameDifficulty((int)this.spawnGroup.spawnCount);
			this.spawnCount = 0;
			return;
		}
		Log.Out("AIDirectorGameStagePartySpawner: groups done ({0})", new object[] { this.groupIndex });
	}

	// Token: 0x06002043 RID: 8259 RVA: 0x000C346C File Offset: 0x000C166C
	public override string ToString()
	{
		return string.Format("{0} {1} (count {2}, numToSpawn {3}, maxAlive {4})", new object[] { this.groupIndex, this.spawnGroupName, this.spawnCount, this.numToSpawn, this.maxAlive });
	}

	// Token: 0x04001606 RID: 5638
	public ReadOnlyCollection<EntityPlayer> partyMembers;

	// Token: 0x04001607 RID: 5639
	public float gsScaling;

	// Token: 0x04001608 RID: 5640
	public int groupIndex;

	// Token: 0x04001609 RID: 5641
	public int partyLevel;

	// Token: 0x0400160A RID: 5642
	public int stageSpawnMax;

	// Token: 0x0400160B RID: 5643
	public int bonusLootEvery;

	// Token: 0x0400160C RID: 5644
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStageDefinition def;

	// Token: 0x0400160D RID: 5645
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStageDefinition.Stage stage;

	// Token: 0x0400160E RID: 5646
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStageDefinition.SpawnGroup spawnGroup;

	// Token: 0x0400160F RID: 5647
	[PublicizedFrom(EAccessModifier.Private)]
	public int spawnCount;

	// Token: 0x04001610 RID: 5648
	[PublicizedFrom(EAccessModifier.Private)]
	public int numToSpawn;

	// Token: 0x04001611 RID: 5649
	[PublicizedFrom(EAccessModifier.Private)]
	public double interval;

	// Token: 0x04001612 RID: 5650
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextStageTime;

	// Token: 0x04001613 RID: 5651
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04001614 RID: 5652
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> memberIDs = new HashSet<int>();

	// Token: 0x04001615 RID: 5653
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> members = new List<EntityPlayer>();
}
