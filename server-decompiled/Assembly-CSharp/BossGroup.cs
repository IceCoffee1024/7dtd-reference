using System;
using System.Collections.Generic;
using Audio;
using GameEvent.SequenceActions;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public class BossGroup
{
	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06002BBA RID: 11194 RVA: 0x00113664 File Offset: 0x00111864
	public string GetBossNavClass
	{
		get
		{
			BossGroup.BossGroupTypes currentGroupType = this.CurrentGroupType;
			if (currentGroupType == BossGroup.BossGroupTypes.ImmortalBoss)
			{
				return "twitch_vote_boss_shield";
			}
			if (currentGroupType != BossGroup.BossGroupTypes.Specialized)
			{
				return "twitch_vote_boss";
			}
			return "";
		}
	}

	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06002BBB RID: 11195 RVA: 0x00113694 File Offset: 0x00111894
	public string GetMinionNavClass
	{
		get
		{
			BossGroup.BossGroupTypes currentGroupType = this.CurrentGroupType;
			if (currentGroupType == BossGroup.BossGroupTypes.ImmortalMinions)
			{
				return "twitch_vote_minion_shield";
			}
			if (currentGroupType != BossGroup.BossGroupTypes.Specialized)
			{
				return "twitch_vote_minion";
			}
			return "";
		}
	}

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06002BBC RID: 11196 RVA: 0x001136C3 File Offset: 0x001118C3
	public int MinionCount
	{
		get
		{
			if (this.MinionEntityIDs != null)
			{
				return this.MinionEntityIDs.Count;
			}
			return 0;
		}
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x001136DC File Offset: 0x001118DC
	public BossGroup(EntityPlayer target, EntityAlive boss, List<EntityAlive> minions, BossGroup.BossGroupTypes bossGroupType, string bossIcon)
	{
		this.CurrentGroupType = bossGroupType;
		this.TargetPlayer = target;
		this.BossEntity = boss;
		this.MinionEntities = minions;
		this.BossName = Localization.Get(EntityClass.list[this.BossEntity.entityClass].entityClassName, false, null);
		this.BossEntityID = boss.entityId;
		this.MinionEntityIDs = new List<int>();
		for (int i = 0; i < minions.Count; i++)
		{
			this.MinionEntityIDs.Add(minions[i].entityId);
		}
		this.BossIcon = bossIcon;
		this.BossGroupID = ++BossGroup.nextID;
		this.serverBounds.size = this.LeavingSize;
	}

	// Token: 0x06002BBE RID: 11198 RVA: 0x00113818 File Offset: 0x00111A18
	public BossGroup(int bossGroupID, BossGroup.BossGroupTypes bossGroupType, int bossEntityID, List<int> minionIDs, string bossIcon)
	{
		this.CurrentGroupType = bossGroupType;
		this.BossEntityID = bossEntityID;
		this.MinionEntityIDs = minionIDs;
		this.BossEntity = null;
		this.MinionEntities = null;
		this.BossIcon = bossIcon;
		this.BossGroupID = bossGroupID;
	}

	// Token: 0x06002BBF RID: 11199 RVA: 0x001138D8 File Offset: 0x00111AD8
	public void Update(EntityPlayerLocal player)
	{
		float num = -1f;
		EntityAlive entityAlive = null;
		if (this.BossEntity == null)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				this.BossEntity = GameManager.Instance.World.GetEntity(this.BossEntityID) as EntityAlive;
				if (this.BossEntity != null)
				{
					if (this.BossName == "")
					{
						this.BossName = Localization.Get(EntityClass.list[this.BossEntity.entityClass].entityClassName, false, null);
					}
					if (this.BossEntity.IsAlive())
					{
						entityAlive = this.BossEntity;
					}
				}
			}
		}
		else if (this.BossEntity.IsAlive())
		{
			entityAlive = this.BossEntity;
		}
		if (entityAlive != null)
		{
			num = entityAlive.GetDistance(player);
		}
		if (this.MinionEntities != null)
		{
			for (int i = 0; i < this.MinionEntities.Count; i++)
			{
				if (this.MinionEntities[i] != null && this.MinionEntities[i].IsAlive())
				{
					float distance = this.MinionEntities[i].GetDistance(player);
					if (num == -1f || distance < num)
					{
						entityAlive = this.MinionEntities[i];
						num = distance;
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < this.MinionEntityIDs.Count; j++)
			{
				EntityAlive entityAlive2 = GameManager.Instance.World.GetEntity(this.MinionEntityIDs[j]) as EntityAlive;
				if (entityAlive2 != null && entityAlive2.IsAlive())
				{
					float distance2 = entityAlive2.GetDistance(player);
					if (num == -1f || distance2 < num)
					{
						entityAlive = entityAlive2;
						num = distance2;
					}
				}
			}
		}
		if (entityAlive == null)
		{
			this.ReadyForRemove = true;
			return;
		}
		this.ReadyForRemove = false;
		this.bounds.center = entityAlive.position;
		this.bounds.size = (this.IsCurrent ? this.LeavingSize : this.EnteringSize);
	}

	// Token: 0x06002BC0 RID: 11200 RVA: 0x00113ADB File Offset: 0x00111CDB
	public bool IsPlayerWithinRange(EntityPlayer player)
	{
		return this.bounds.Contains(player.position);
	}

	// Token: 0x06002BC1 RID: 11201 RVA: 0x00113AEE File Offset: 0x00111CEE
	public bool IsPlayerWithinServerRange(EntityPlayer player)
	{
		return this.serverBounds.Contains(player.position);
	}

	// Token: 0x06002BC2 RID: 11202 RVA: 0x00113B04 File Offset: 0x00111D04
	public void RemoveMinion(int entityID)
	{
		if (this.MinionEntityIDs != null)
		{
			this.MinionEntityIDs.Remove(entityID);
		}
		if (this.MinionEntities != null)
		{
			for (int i = this.MinionEntities.Count - 1; i >= 0; i--)
			{
				if (this.MinionEntities[i] != null && this.MinionEntities[i].entityId == entityID)
				{
					this.MinionEntities.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x06002BC3 RID: 11203 RVA: 0x00113B7C File Offset: 0x00111D7C
	public void AddNavObjects()
	{
		if (this.MinionEntities == null)
		{
			this.MinionEntities = new List<EntityAlive>();
			for (int i = 0; i < this.MinionEntityIDs.Count; i++)
			{
				this.MinionEntities.Add(GameManager.Instance.World.GetEntity(this.MinionEntityIDs[i]) as EntityAlive);
			}
		}
		if (this.BossEntity != null)
		{
			this.BossEntity.Buffs.AddBuff("twitch_give_navobject", -1, true, false, -1f);
		}
		if (this.MinionEntities != null)
		{
			for (int j = 0; j < this.MinionEntities.Count; j++)
			{
				if (this.MinionEntities[j] != null)
				{
					this.MinionEntities[j].Buffs.AddBuff("twitch_give_navobject", -1, true, false, -1f);
				}
			}
		}
	}

	// Token: 0x06002BC4 RID: 11204 RVA: 0x00113C5F File Offset: 0x00111E5F
	public void RequestStatRefresh()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageBossEvent>().Setup(NetPackageBossEvent.BossEventTypes.RequestStats, this.BossGroupID), false);
		}
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x00113C8C File Offset: 0x00111E8C
	public void RefreshStats(int playerID)
	{
		if (this.MinionEntities == null)
		{
			this.MinionEntities = new List<EntityAlive>();
			for (int i = 0; i < this.MinionEntityIDs.Count; i++)
			{
				this.MinionEntities.Add(GameManager.Instance.World.GetEntity(this.MinionEntityIDs[i]) as EntityAlive);
			}
		}
		if (this.BossEntity != null)
		{
			this.BossEntity.bPlayerStatsChanged = true;
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityStatChanged>().Setup(this.BossEntity, playerID, NetPackageEntityStatChanged.EnumStat.Health), false, -1, -1, -1, null, 192, false);
		}
		if (this.MinionEntities != null)
		{
			for (int j = 0; j < this.MinionEntities.Count; j++)
			{
				if (this.MinionEntities[j] != null)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityStatChanged>().Setup(this.MinionEntities[j], playerID, NetPackageEntityStatChanged.EnumStat.Health), false, -1, -1, -1, null, 192, false);
				}
			}
		}
	}

	// Token: 0x06002BC6 RID: 11206 RVA: 0x00113DA0 File Offset: 0x00111FA0
	public void RemoveNavObjects()
	{
		if (this.BossEntity != null)
		{
			this.BossEntity.RemoveNavObject("twitch_vote_boss");
			this.BossEntity.RemoveNavObject("twitch_vote_boss_shield");
		}
		if (this.MinionEntities != null)
		{
			for (int i = 0; i < this.MinionEntities.Count; i++)
			{
				if (this.MinionEntities[i] != null)
				{
					this.MinionEntities[i].RemoveNavObject("twitch_vote_minion");
					this.MinionEntities[i].RemoveNavObject("twitch_vote_minion_shield");
				}
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				this.MinionEntities = null;
			}
		}
	}

	// Token: 0x06002BC7 RID: 11207 RVA: 0x00113E4C File Offset: 0x0011204C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupTeleportList()
	{
		this.ClosestEnemy = this.GetClosestEntity(this.TargetPlayer);
		if (this.ClosestEnemy == null)
		{
			return;
		}
		this.serverBounds.center = this.ClosestEnemy.position;
		for (int i = 0; i < this.MinionEntities.Count; i++)
		{
			if (!(this.MinionEntities[i] == null) && this.MinionEntities[i].IsAlive() && !(this.MinionEntities[i] == this.ClosestEnemy))
			{
				EntityAlive entityAlive = this.MinionEntities[i];
				if (Vector3.Distance(this.ClosestEnemy.position, this.MinionEntities[i].position) > BossGroup.autoPullDistance)
				{
					if (!this.TeleportList.Contains(this.MinionEntities[i]))
					{
						this.TeleportList.Add(this.MinionEntities[i]);
					}
				}
				else if (this.TeleportList.Contains(this.MinionEntities[i]))
				{
					this.TeleportList.Remove(this.MinionEntities[i]);
				}
			}
		}
		if (this.ClosestEnemy == this.BossEntity || this.BossEntity == null || !this.BossEntity.IsAlive())
		{
			return;
		}
		if (Vector3.Distance(this.ClosestEnemy.position, this.BossEntity.position) > BossGroup.autoPullDistance)
		{
			if (!this.TeleportList.Contains(this.BossEntity))
			{
				this.TeleportList.Add(this.BossEntity);
				return;
			}
		}
		else if (this.TeleportList.Contains(this.BossEntity))
		{
			this.TeleportList.Remove(this.BossEntity);
		}
	}

	// Token: 0x06002BC8 RID: 11208 RVA: 0x00114028 File Offset: 0x00112228
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive GetClosestEntity(EntityPlayer player)
	{
		EntityAlive entityAlive = null;
		float num = -1f;
		for (int i = this.MinionEntities.Count - 1; i >= 0; i--)
		{
			if (this.MinionEntities[i] != null && this.MinionEntities[i].IsAlive())
			{
				float num2 = Vector3.Distance(this.TargetPlayer.position, this.MinionEntities[i].position);
				if (num > num2 || num == -1f)
				{
					num = num2;
					entityAlive = this.MinionEntities[i];
				}
			}
		}
		if (this.BossEntity != null && this.BossEntity.IsAlive())
		{
			float num2 = Vector3.Distance(this.TargetPlayer.position, this.BossEntity.position);
			if (num > num2 || num == -1f)
			{
				entityAlive = this.BossEntity;
			}
		}
		return entityAlive;
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x0011410C File Offset: 0x0011230C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleTeleportList()
	{
		for (int i = this.TeleportList.Count - 1; i >= 0; i--)
		{
			EntityAlive entityAlive = this.TeleportList[i];
			Vector3 zero = Vector3.zero;
			if (ActionBaseSpawn.FindValidPosition(out zero, this.ClosestEnemy.position, 3f, 6f, true, 0f, false, 0f))
			{
				if (this.pullSound != "")
				{
					Manager.BroadcastPlayByLocalPlayer(entityAlive.position, this.pullSound);
				}
				entityAlive.SetPosition(zero, true);
				entityAlive.SetAttackTarget(this.TargetPlayer, 12000);
				this.TeleportList.RemoveAt(i);
				if (this.pullSound != "")
				{
					Manager.BroadcastPlayByLocalPlayer(zero, this.pullSound);
				}
			}
		}
	}

	// Token: 0x06002BCA RID: 11210 RVA: 0x001141DA File Offset: 0x001123DA
	public void HandleAutoPull()
	{
		if (this.TeleportList.Count > 0)
		{
			this.HandleTeleportList();
		}
	}

	// Token: 0x06002BCB RID: 11211 RVA: 0x001141F0 File Offset: 0x001123F0
	public void HandleLiveHandling()
	{
		this.liveTime += Time.deltaTime;
		this.attackTime -= Time.deltaTime;
		if (this.liveTime > 5f && !this.IsPlayerWithinServerRange(this.TargetPlayer))
		{
			this.RemoveNavObjects();
			this.DespawnAll();
		}
		if (this.attackTime <= 0f)
		{
			this.HandleAttackTrigger();
			this.attackTime = 5f;
		}
	}

	// Token: 0x06002BCC RID: 11212 RVA: 0x00114268 File Offset: 0x00112468
	public bool ServerUpdate()
	{
		bool flag = false;
		this.SetupTeleportList();
		for (int i = this.MinionEntities.Count - 1; i >= 0; i--)
		{
			if (this.MinionEntities[i] == null || !this.MinionEntities[i].IsAlive())
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageBossEvent>().Setup(NetPackageBossEvent.BossEventTypes.RemoveMinion, this.BossGroupID, this.MinionEntities[i].entityId), false, -1, -1, -1, null, 192, false);
				this.MinionEntityIDs.Remove(this.MinionEntities[i].entityId);
				this.MinionEntities.RemoveAt(i);
			}
			else
			{
				flag = true;
			}
		}
		if (this.BossEntity != null && this.BossEntity.IsAlive())
		{
			flag = true;
		}
		if (!flag)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageBossEvent>().Setup(NetPackageBossEvent.BossEventTypes.RemoveGroup, this.BossGroupID), false, -1, -1, -1, null, 192, false);
		}
		return !flag;
	}

	// Token: 0x06002BCD RID: 11213 RVA: 0x00114380 File Offset: 0x00112580
	public void DespawnAll()
	{
		for (int i = this.MinionEntities.Count - 1; i >= 0; i--)
		{
			if (this.MinionEntities[i] != null && this.MinionEntities[i].IsAlive())
			{
				this.MinionEntities[i].ForceDespawn();
			}
		}
		if (this.BossEntity != null && this.BossEntity.IsAlive())
		{
			this.BossEntity.ForceDespawn();
		}
	}

	// Token: 0x06002BCE RID: 11214 RVA: 0x00114404 File Offset: 0x00112604
	public void HandleAttackTrigger()
	{
		for (int i = this.MinionEntities.Count - 1; i >= 0; i--)
		{
			if (this.MinionEntities[i] != null && this.MinionEntities[i].IsAlive() && this.MinionEntities[i].GetAttackTarget() == null)
			{
				this.MinionEntities[i].SetAttackTarget(this.TargetPlayer, 60000);
			}
		}
		if (this.BossEntity != null && this.BossEntity.IsAlive() && this.BossEntity.GetAttackTarget() == null)
		{
			this.BossEntity.SetAttackTarget(this.TargetPlayer, 60000);
		}
	}

	// Token: 0x0400213D RID: 8509
	public int BossGroupID = -1;

	// Token: 0x0400213E RID: 8510
	public int BossEntityID = -1;

	// Token: 0x0400213F RID: 8511
	public EntityAlive BossEntity;

	// Token: 0x04002140 RID: 8512
	public List<int> MinionEntityIDs;

	// Token: 0x04002141 RID: 8513
	public List<EntityAlive> MinionEntities;

	// Token: 0x04002142 RID: 8514
	public EntityPlayer TargetPlayer;

	// Token: 0x04002143 RID: 8515
	public BossGroup.BossGroupTypes CurrentGroupType;

	// Token: 0x04002144 RID: 8516
	public string BossIcon = "";

	// Token: 0x04002145 RID: 8517
	public string BossName = "";

	// Token: 0x04002146 RID: 8518
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds serverBounds;

	// Token: 0x04002147 RID: 8519
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds bounds;

	// Token: 0x04002148 RID: 8520
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 EnteringSize = new Vector3(32f, 32f, 32f);

	// Token: 0x04002149 RID: 8521
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 LeavingSize = new Vector3(200f, 200f, 200f);

	// Token: 0x0400214A RID: 8522
	[PublicizedFrom(EAccessModifier.Private)]
	public static float autoPullDistance = 32f;

	// Token: 0x0400214B RID: 8523
	[PublicizedFrom(EAccessModifier.Private)]
	public static int nextID = -1;

	// Token: 0x0400214C RID: 8524
	public bool IsCurrent;

	// Token: 0x0400214D RID: 8525
	public bool ReadyForRemove;

	// Token: 0x0400214E RID: 8526
	public string pullSound = "twitch_pull";

	// Token: 0x0400214F RID: 8527
	[PublicizedFrom(EAccessModifier.Private)]
	public float liveTime;

	// Token: 0x04002150 RID: 8528
	[PublicizedFrom(EAccessModifier.Private)]
	public float attackTime = 5f;

	// Token: 0x04002151 RID: 8529
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive ClosestEnemy;

	// Token: 0x04002152 RID: 8530
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityAlive> TeleportList = new List<EntityAlive>();

	// Token: 0x02000530 RID: 1328
	public enum BossGroupTypes
	{
		// Token: 0x04002154 RID: 8532
		Standard,
		// Token: 0x04002155 RID: 8533
		ImmortalBoss,
		// Token: 0x04002156 RID: 8534
		ImmortalMinions,
		// Token: 0x04002157 RID: 8535
		Specialized
	}
}
