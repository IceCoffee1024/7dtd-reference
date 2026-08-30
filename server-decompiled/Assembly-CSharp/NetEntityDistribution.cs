using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000793 RID: 1939
public class NetEntityDistribution
{
	// Token: 0x060039BF RID: 14783 RVA: 0x00178780 File Offset: 0x00176980
	public NetEntityDistribution(World _world, int _v)
	{
		this.trackedEntitySet = new HashSetList<NetEntityDistributionEntry>();
		this.trackedEntityHashTable = new IntHashMap();
		this.world = _world;
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x00178954 File Offset: 0x00176B54
	public void OnUpdateEntities()
	{
		this.playerList.Clear();
		this.enemyList.Clear();
		for (int i = 0; i < this.trackedEntitySet.list.Count; i++)
		{
			NetEntityDistributionEntry netEntityDistributionEntry = this.trackedEntitySet.list[i];
			EntityEnemy entityEnemy = netEntityDistributionEntry.trackedEntity as EntityEnemy;
			if (entityEnemy != null)
			{
				this.enemyList.Add(entityEnemy);
			}
			else
			{
				EntityPlayer entityPlayer = netEntityDistributionEntry.trackedEntity as EntityPlayer;
				if (entityPlayer != null)
				{
					this.playerList.Add(entityPlayer);
				}
			}
		}
		foreach (EntityEnemy entityEnemy2 in this.enemyList)
		{
			NetEntityDistributionEntry netEntityDistributionEntry2 = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(entityEnemy2.entityId);
			bool flag = entityEnemy2.IsAirBorne();
			netEntityDistributionEntry2.priorityLevel = 1;
			if (GameManager.enableNetworkdPrioritization)
			{
				float num = float.MaxValue;
				bool flag2 = false;
				Vector3 position = entityEnemy2.transform.position;
				position.y = 0f;
				for (int j = 0; j < this.world.Players.Count; j++)
				{
					Transform transform = this.world.Players.list[j].transform;
					Vector3 position2 = transform.position;
					position2.y = 0f;
					Vector3 vector = position - position2;
					float num2 = vector.x * vector.x + vector.z + vector.z;
					if (!flag2 && num2 < 16384f && Vector3.Angle(transform.forward, vector.normalized) < NetEntityDistribution.priorityViewAngleLimit)
					{
						flag2 = true;
					}
					if (num2 < num)
					{
						num = num2;
					}
				}
				if (num < 25f)
				{
					netEntityDistributionEntry2.priorityLevel = 0;
				}
				else if (!flag2 && !flag)
				{
					if (num > 625f)
					{
						netEntityDistributionEntry2.priorityLevel = 3;
					}
					else if (num > 324f)
					{
						netEntityDistributionEntry2.priorityLevel = 2;
					}
				}
			}
		}
		if (this.playerList.Count > 1)
		{
			foreach (EntityPlayer entityPlayer2 in this.playerList)
			{
				NetEntityDistributionEntry netEntityDistributionEntry3 = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(entityPlayer2.entityId);
				netEntityDistributionEntry3.priorityLevel = 1;
				if (GameManager.enableNetworkdPrioritization)
				{
					Vector3 position3 = entityPlayer2.transform.position;
					foreach (EntityPlayer entityPlayer3 in this.playerList)
					{
						if (!(entityPlayer3 == entityPlayer2))
						{
							Vector3 vector2 = position3 - entityPlayer3.transform.position;
							if (vector2.x * vector2.x + vector2.z * vector2.z < 25f)
							{
								netEntityDistributionEntry3.priorityLevel = 0;
								break;
							}
						}
					}
				}
			}
		}
		for (int k = 0; k < this.trackedEntitySet.list.Count; k++)
		{
			this.trackedEntitySet.list[k].updatePlayerList(this.world.Players.list);
		}
		for (int l = 0; l < this.playerList.Count; l++)
		{
			EntityPlayer entityPlayer4 = this.playerList[l];
			for (int m = 0; m < this.trackedEntitySet.list.Count; m++)
			{
				NetEntityDistributionEntry netEntityDistributionEntry4 = this.trackedEntitySet.list[m];
				if (netEntityDistributionEntry4.trackedEntity != entityPlayer4)
				{
					netEntityDistributionEntry4.updatePlayerEntity(entityPlayer4);
				}
			}
		}
	}

	// Token: 0x060039C1 RID: 14785 RVA: 0x00178D6C File Offset: 0x00176F6C
	public void SendPacketToTrackedPlayers(int _entityId, int _excludePlayer, NetPackage _package, bool _inRangeOnly = false)
	{
		NetEntityDistributionEntry netEntityDistributionEntry = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(_entityId);
		if (netEntityDistributionEntry != null)
		{
			netEntityDistributionEntry.SendToPlayers(_package, _excludePlayer, _inRangeOnly, 192);
		}
	}

	// Token: 0x060039C2 RID: 14786 RVA: 0x00178DA0 File Offset: 0x00176FA0
	public void SendPacketToTrackedPlayersAndTrackedEntity(int _entityId, int _excludePlayer, NetPackage _package, bool _inRangeOnly = false)
	{
		NetEntityDistributionEntry netEntityDistributionEntry = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(_entityId);
		if (netEntityDistributionEntry != null)
		{
			netEntityDistributionEntry.sendPacketToTrackedPlayersAndTrackedEntity(_package, _excludePlayer, _inRangeOnly);
		}
	}

	// Token: 0x060039C3 RID: 14787 RVA: 0x00178DCC File Offset: 0x00176FCC
	public void Add(Entity _e)
	{
		for (int i = 0; i < this.config.Count; i++)
		{
			NetEntityDistribution.SEnts sents = this.config[i];
			if (sents.eType.IsAssignableFrom(_e.GetType()))
			{
				this.Add(_e, sents.distance, sents.update, sents.motion);
			}
			if (_e is EntityPlayer)
			{
				EntityPlayer entityPlayer = (EntityPlayer)_e;
				for (int j = 0; j < this.trackedEntitySet.list.Count; j++)
				{
					NetEntityDistributionEntry netEntityDistributionEntry = this.trackedEntitySet.list[j];
					if (netEntityDistributionEntry.trackedEntity != entityPlayer)
					{
						netEntityDistributionEntry.updatePlayerEntity(entityPlayer);
					}
				}
			}
		}
	}

	// Token: 0x060039C4 RID: 14788 RVA: 0x00178E82 File Offset: 0x00177082
	public void Add(Entity _e, int _d, int _t)
	{
		this.Add(_e, _d, _t, false);
	}

	// Token: 0x060039C5 RID: 14789 RVA: 0x00178E90 File Offset: 0x00177090
	public void Add(Entity _e, int _distance, int _t, bool _upd)
	{
		if (!this.trackedEntityHashTable.containsItem(_e.entityId))
		{
			NetEntityDistributionEntry netEntityDistributionEntry = new NetEntityDistributionEntry(_e, _distance, _t, _upd);
			this.trackedEntitySet.Add(netEntityDistributionEntry);
			this.trackedEntityHashTable.addKey(_e.entityId, netEntityDistributionEntry);
			netEntityDistributionEntry.updatePlayerEntities(this.world.Players.list);
		}
	}

	// Token: 0x060039C6 RID: 14790 RVA: 0x00178EF0 File Offset: 0x001770F0
	public void Remove(Entity _e, EnumRemoveEntityReason _reason)
	{
		if (_e is EntityPlayer)
		{
			EntityPlayer entityPlayer = (EntityPlayer)_e;
			for (int i = 0; i < this.trackedEntitySet.list.Count; i++)
			{
				this.trackedEntitySet.list[i].Remove(entityPlayer);
			}
		}
		NetEntityDistributionEntry netEntityDistributionEntry = (NetEntityDistributionEntry)this.trackedEntityHashTable.removeObject(_e.entityId);
		if (netEntityDistributionEntry != null)
		{
			this.trackedEntitySet.Remove(netEntityDistributionEntry);
			if (_reason == EnumRemoveEntityReason.Unloaded)
			{
				netEntityDistributionEntry.SendUnloadEntityToPlayers();
				return;
			}
			netEntityDistributionEntry.SendDestroyEntityToPlayers();
		}
	}

	// Token: 0x060039C7 RID: 14791 RVA: 0x00178F78 File Offset: 0x00177178
	public void SyncEntity(Entity _e, Vector3 _pos, Vector3 _rot)
	{
		NetEntityDistributionEntry netEntityDistributionEntry = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(_e.entityId);
		if (netEntityDistributionEntry != null)
		{
			netEntityDistributionEntry.encodedPos = NetEntityDistributionEntry.EncodePos(_pos);
			netEntityDistributionEntry.encodedRot = NetEntityDistributionEntry.EncodePos(_rot);
		}
	}

	// Token: 0x060039C8 RID: 14792 RVA: 0x00178FB8 File Offset: 0x001771B8
	public void SendFullUpdateNextTick(Entity _e)
	{
		NetEntityDistributionEntry netEntityDistributionEntry = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(_e.entityId);
		if (netEntityDistributionEntry != null)
		{
			netEntityDistributionEntry.SendFullUpdateNextTick();
		}
	}

	// Token: 0x060039C9 RID: 14793 RVA: 0x00178FE5 File Offset: 0x001771E5
	public void Cleanup()
	{
		this.trackedEntitySet.Clear();
		this.trackedEntityHashTable.clearMap();
	}

	// Token: 0x060039CA RID: 14794 RVA: 0x00178FFD File Offset: 0x001771FD
	public NetEntityDistributionEntry FindEntry(Entity entity)
	{
		return this.trackedEntityHashTable.lookup(entity.entityId) as NetEntityDistributionEntry;
	}

	// Token: 0x04002F5F RID: 12127
	public const float cHighPriorityRange = 5f;

	// Token: 0x04002F60 RID: 12128
	public const float cLowPriorityRange = 18f;

	// Token: 0x04002F61 RID: 12129
	public const float cLowestPriorityRange = 25f;

	// Token: 0x04002F62 RID: 12130
	public const int MobsUpdateTicks = 3;

	// Token: 0x04002F63 RID: 12131
	public const int lowPriorityTick = 6;

	// Token: 0x04002F64 RID: 12132
	public const int lowestPriorityTick = 10;

	// Token: 0x04002F65 RID: 12133
	public static float priorityViewAngleLimit = 60f;

	// Token: 0x04002F66 RID: 12134
	[PublicizedFrom(EAccessModifier.Private)]
	public const float priorityViewAngleMinDistance = 128f;

	// Token: 0x04002F67 RID: 12135
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetList<NetEntityDistributionEntry> trackedEntitySet;

	// Token: 0x04002F68 RID: 12136
	[PublicizedFrom(EAccessModifier.Private)]
	public IntHashMap trackedEntityHashTable;

	// Token: 0x04002F69 RID: 12137
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04002F6A RID: 12138
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NetEntityDistribution.SEnts> config = new List<NetEntityDistribution.SEnts>
	{
		new NetEntityDistribution.SEnts(typeof(EntityPlayer), int.MaxValue, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityVehicle), int.MaxValue, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityEnemy), 80, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityNPC), 80, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityItem), 64, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityFallingBlock), 120, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityFallingBlocks), 120, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityFallingTree), 120, 1, false),
		new NetEntityDistribution.SEnts(typeof(EntityAnimalStag), 80, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityAnimalRabbit), 64, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityCar), 100, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntitySupplyCrate), 1200, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntitySupplyPlane), 1200, 3, true),
		new NetEntityDistribution.SEnts(typeof(EntityTurret), 60, 3, false),
		new NetEntityDistribution.SEnts(typeof(EntityHomerunGoal), 80, 3, false)
	};

	// Token: 0x04002F6B RID: 12139
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> playerList = new List<EntityPlayer>();

	// Token: 0x04002F6C RID: 12140
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityEnemy> enemyList = new List<EntityEnemy>();

	// Token: 0x02000794 RID: 1940
	[PublicizedFrom(EAccessModifier.Private)]
	public struct SEnts
	{
		// Token: 0x060039CC RID: 14796 RVA: 0x00179021 File Offset: 0x00177221
		public SEnts(Type _eType, int _distance, int _update, bool _motion)
		{
			this.eType = _eType;
			this.distance = _distance;
			this.update = _update;
			this.motion = _motion;
		}

		// Token: 0x04002F6D RID: 12141
		public Type eType;

		// Token: 0x04002F6E RID: 12142
		public int distance;

		// Token: 0x04002F6F RID: 12143
		public int update;

		// Token: 0x04002F70 RID: 12144
		public bool motion;
	}
}
