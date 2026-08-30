using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Scripting;

// Token: 0x02000410 RID: 1040
[Preserve]
public class AIDirectorGameStagePartySpawner
{
	// Token: 0x06002025 RID: 8229 RVA: 0x000C2734 File Offset: 0x000C0934
	public AIDirectorGameStagePartySpawner(World _world, string _gameStageName)
	{
		this.world = _world;
		this.def = GameStageDefinition.GetGameStage(_gameStageName);
		this.partyMembers = new ReadOnlyCollection<EntityPlayer>(this.members);
		this.partyLevel = -1;
		this.gsScaling = 1f;
	}

	// Token: 0x06002026 RID: 8230 RVA: 0x000C2793 File Offset: 0x000C0993
	public void SetScaling(float _scaling)
	{
		this.gsScaling = Utils.FastLerp(1f, 2.5f, (_scaling - 1f) / 3f);
	}

	// Token: 0x06002027 RID: 8231 RVA: 0x000C27B8 File Offset: 0x000C09B8
	public void ResetPartyLevel(int mod = 0)
	{
		int num = this.CalcPartyLevel();
		if (mod != 0)
		{
			num %= mod;
		}
		this.SetPartyLevel(num);
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x000C27DC File Offset: 0x000C09DC
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

	// Token: 0x06002029 RID: 8233 RVA: 0x000C2824 File Offset: 0x000C0A24
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

	// Token: 0x0600202A RID: 8234 RVA: 0x000C2970 File Offset: 0x000C0B70
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

	// Token: 0x0600202B RID: 8235 RVA: 0x000C29FC File Offset: 0x000C0BFC
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

	// Token: 0x0600202C RID: 8236 RVA: 0x000C2A48 File Offset: 0x000C0C48
	public bool IsMemberOfParty(int _entityID)
	{
		return this.memberIDs.Contains(_entityID);
	}

	// Token: 0x0600202D RID: 8237 RVA: 0x000C2A56 File Offset: 0x000C0C56
	public void RemoveMember(EntityPlayer _player, bool removeID)
	{
		this.members.Remove(_player);
		if (removeID)
		{
			this.memberIDs.Remove(_player.entityId);
		}
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x000C2A7C File Offset: 0x000C0C7C
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

	// Token: 0x0600202F RID: 8239 RVA: 0x000C2AC4 File Offset: 0x000C0CC4
	public void ClearMembers()
	{
		this.members.Clear();
		this.memberIDs.Clear();
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x000C2ADC File Offset: 0x000C0CDC
	public void IncSpawnCount()
	{
		this.spawnCount++;
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x000C2AEC File Offset: 0x000C0CEC
	public void DecSpawnCount(int dec)
	{
		if (dec > this.spawnCount)
		{
			this.spawnCount = 0;
			return;
		}
		this.spawnCount -= dec;
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06002032 RID: 8242 RVA: 0x000C2B0D File Offset: 0x000C0D0D
	public bool IsDone
	{
		get
		{
			return this.groupIndex > 0 && this.spawnGroup == null;
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06002033 RID: 8243 RVA: 0x000C2B23 File Offset: 0x000C0D23
	public bool canSpawn
	{
		get
		{
			return this.spawnGroup != null && this.spawnCount < this.numToSpawn;
		}
	}

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06002034 RID: 8244 RVA: 0x000C2B3D File Offset: 0x000C0D3D
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

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06002035 RID: 8245 RVA: 0x000C2B54 File Offset: 0x000C0D54
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

	// Token: 0x06002036 RID: 8246 RVA: 0x000C2B6C File Offset: 0x000C0D6C
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

	// Token: 0x06002037 RID: 8247 RVA: 0x000C2C1C File Offset: 0x000C0E1C
	public override string ToString()
	{
		return string.Format("{0} {1} (count {2}, numToSpawn {3}, maxAlive {4})", new object[] { this.groupIndex, this.spawnGroupName, this.spawnCount, this.numToSpawn, this.maxAlive });
	}

	// Token: 0x040015FA RID: 5626
	public ReadOnlyCollection<EntityPlayer> partyMembers;

	// Token: 0x040015FB RID: 5627
	public float gsScaling;

	// Token: 0x040015FC RID: 5628
	public int groupIndex;

	// Token: 0x040015FD RID: 5629
	public int partyLevel;

	// Token: 0x040015FE RID: 5630
	public int stageSpawnMax;

	// Token: 0x040015FF RID: 5631
	public int bonusLootEvery;

	// Token: 0x04001600 RID: 5632
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStageDefinition def;

	// Token: 0x04001601 RID: 5633
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStageDefinition.Stage stage;

	// Token: 0x04001602 RID: 5634
	[PublicizedFrom(EAccessModifier.Private)]
	public GameStageDefinition.SpawnGroup spawnGroup;

	// Token: 0x04001603 RID: 5635
	[PublicizedFrom(EAccessModifier.Private)]
	public int spawnCount;

	// Token: 0x04001604 RID: 5636
	[PublicizedFrom(EAccessModifier.Private)]
	public int numToSpawn;

	// Token: 0x04001605 RID: 5637
	[PublicizedFrom(EAccessModifier.Private)]
	public double interval;

	// Token: 0x04001606 RID: 5638
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong nextStageTime;

	// Token: 0x04001607 RID: 5639
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04001608 RID: 5640
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<int> memberIDs = new HashSet<int>();

	// Token: 0x04001609 RID: 5641
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> members = new List<EntityPlayer>();
}
