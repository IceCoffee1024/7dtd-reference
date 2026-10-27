using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Scripting;

// Token: 0x02000418 RID: 1048
[Preserve]
public class AIDirectorGameStagePartySpawner
{
	// Token: 0x06002036 RID: 8246 RVA: 0x000C3330 File Offset: 0x000C1530
	public AIDirectorGameStagePartySpawner(World _world, string _gameStageName)
	{
		this.world = _world;
		this.def = GameStageDefinition.GetGameStage(_gameStageName);
		this.partyMembers = new ReadOnlyCollection<EntityPlayer>(this.members);
		this.partyLevel = -1;
		this.gsScaling = 1f;
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x000C338F File Offset: 0x000C158F
	public void SetScaling(float _scaling)
	{
		this.gsScaling = Utils.FastLerp(1f, 2.5f, (_scaling - 1f) / 3f);
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x000C33B4 File Offset: 0x000C15B4
	public void ResetPartyLevel(int mod = 0)
	{
		int num = this.CalcPartyLevel();
		if (mod != 0)
		{
			num %= mod;
		}
		this.SetPartyLevel(num);
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x000C33D8 File Offset: 0x000C15D8
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

	// Token: 0x0600203A RID: 8250 RVA: 0x000C3420 File Offset: 0x000C1620
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

	// Token: 0x0600203B RID: 8251 RVA: 0x000C356C File Offset: 0x000C176C
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

	// Token: 0x0600203C RID: 8252 RVA: 0x000C35F8 File Offset: 0x000C17F8
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

	// Token: 0x0600203D RID: 8253 RVA: 0x000C3644 File Offset: 0x000C1844
	public bool IsMemberOfParty(int _entityID)
	{
		return this.memberIDs.Contains(_entityID);
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x000C3652 File Offset: 0x000C1852
	public void RemoveMember(EntityPlayer _player, bool removeID)
	{
		this.members.Remove(_player);
		if (removeID)
		{
			this.memberIDs.Remove(_player.entityId);
		}
	}

	// Token: 0x0600203F RID: 8255 RVA: 0x000C3678 File Offset: 0x000C1878
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

	// Token: 0x06002040 RID: 8256 RVA: 0x000C36C0 File Offset: 0x000C18C0
	public void ClearMembers()
	{
		this.members.Clear();
		this.memberIDs.Clear();
	}

	// Token: 0x06002041 RID: 8257 RVA: 0x000C36D8 File Offset: 0x000C18D8
	public void IncSpawnCount()
	{
		this.spawnCount++;
	}

	// Token: 0x06002042 RID: 8258 RVA: 0x000C36E8 File Offset: 0x000C18E8
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
	// (get) Token: 0x06002043 RID: 8259 RVA: 0x000C3709 File Offset: 0x000C1909
	public bool IsDone
	{
		get
		{
			return this.groupIndex > 0 && this.spawnGroup == null;
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06002044 RID: 8260 RVA: 0x000C371F File Offset: 0x000C191F
	public bool canSpawn
	{
		get
		{
			return this.spawnGroup != null && this.spawnCount < this.numToSpawn;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06002045 RID: 8261 RVA: 0x000C3739 File Offset: 0x000C1939
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
	// (get) Token: 0x06002046 RID: 8262 RVA: 0x000C3750 File Offset: 0x000C1950
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

	// Token: 0x06002047 RID: 8263 RVA: 0x000C3768 File Offset: 0x000C1968
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

	// Token: 0x06002048 RID: 8264 RVA: 0x000C3818 File Offset: 0x000C1A18
	public override string ToString()
	{
		return string.Format("{0} {1} (count {2}, numToSpawn {3}, maxAlive {4})", new object[] { this.groupIndex, this.spawnGroupName, this.spawnCount, this.numToSpawn, this.maxAlive });
	}

	// Token: 0x04001616 RID: 5654
	public ReadOnlyCollection<EntityPlayer> partyMembers;

	// Token: 0x04001617 RID: 5655
	public float gsScaling;

	// Token: 0x04001618 RID: 5656
	public int groupIndex;

	// Token: 0x04001619 RID: 5657
	public int partyLevel;

	// Token: 0x0400161A RID: 5658
	public int stageSpawnMax;

	// Token: 0x0400161B RID: 5659
	public int bonusLootEvery;

	// Token: 0x0400161C RID: 5660
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStageDefinition def;

	// Token: 0x0400161D RID: 5661
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStageDefinition.Stage stage;

	// Token: 0x0400161E RID: 5662
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStageDefinition.SpawnGroup spawnGroup;

	// Token: 0x0400161F RID: 5663
	[PublicizedFrom(EAccessModifier.Private)]
	public int spawnCount;

	// Token: 0x04001620 RID: 5664
	[PublicizedFrom(EAccessModifier.Private)]
	public int numToSpawn;

	// Token: 0x04001621 RID: 5665
	[PublicizedFrom(EAccessModifier.Private)]
	public double interval;

	// Token: 0x04001622 RID: 5666
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextStageTime;

	// Token: 0x04001623 RID: 5667
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04001624 RID: 5668
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> memberIDs = new HashSet<int>();

	// Token: 0x04001625 RID: 5669
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> members = new List<EntityPlayer>();
}
