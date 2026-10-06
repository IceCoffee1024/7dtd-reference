using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000796 RID: 1942
public class NetEntityDistribution
{
	// Token: 0x06003A02 RID: 14850 RVA: 0x00178F38 File Offset: 0x00177138
	public NetEntityDistribution(World _world, int _v)
	{
		this.trackedEntitySet = new HashSetList<NetEntityDistributionEntry>();
		this.trackedEntityHashTable = new IntHashMap();
		this.world = _world;
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x0017910C File Offset: 0x0017730C
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

	// Token: 0x06003A04 RID: 14852 RVA: 0x00179524 File Offset: 0x00177724
	public void SendPacketToTrackedPlayers(int _entityId, int _excludePlayer, NetPackage _package, bool _inRangeOnly = false)
	{
		NetEntityDistributionEntry netEntityDistributionEntry = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(_entityId);
		if (netEntityDistributionEntry != null)
		{
			netEntityDistributionEntry.SendToPlayers(_package, _excludePlayer, _inRangeOnly, 192);
		}
	}

	// Token: 0x06003A05 RID: 14853 RVA: 0x00179558 File Offset: 0x00177758
	public void SendPacketToTrackedPlayersAndTrackedEntity(int _entityId, int _excludePlayer, NetPackage _package, bool _inRangeOnly = false)
	{
		NetEntityDistributionEntry netEntityDistributionEntry = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(_entityId);
		if (netEntityDistributionEntry != null)
		{
			netEntityDistributionEntry.sendPacketToTrackedPlayersAndTrackedEntity(_package, _excludePlayer, _inRangeOnly);
		}
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x00179584 File Offset: 0x00177784
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

	// Token: 0x06003A07 RID: 14855 RVA: 0x0017963A File Offset: 0x0017783A
	public void Add(Entity _e, int _d, int _t)
	{
		this.Add(_e, _d, _t, false);
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x00179648 File Offset: 0x00177848
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

	// Token: 0x06003A09 RID: 14857 RVA: 0x001796A8 File Offset: 0x001778A8
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

	// Token: 0x06003A0A RID: 14858 RVA: 0x00179730 File Offset: 0x00177930
	public void SyncEntity(Entity _e, Vector3 _pos, Vector3 _rot)
	{
		NetEntityDistributionEntry netEntityDistributionEntry = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(_e.entityId);
		if (netEntityDistributionEntry != null)
		{
			netEntityDistributionEntry.encodedPos = NetEntityDistributionEntry.EncodePos(_pos);
			netEntityDistributionEntry.encodedRot = NetEntityDistributionEntry.EncodePos(_rot);
		}
	}

	// Token: 0x06003A0B RID: 14859 RVA: 0x00179770 File Offset: 0x00177970
	public void SendFullUpdateNextTick(Entity _e)
	{
		NetEntityDistributionEntry netEntityDistributionEntry = (NetEntityDistributionEntry)this.trackedEntityHashTable.lookup(_e.entityId);
		if (netEntityDistributionEntry != null)
		{
			netEntityDistributionEntry.SendFullUpdateNextTick();
		}
	}

	// Token: 0x06003A0C RID: 14860 RVA: 0x0017979D File Offset: 0x0017799D
	public void Cleanup()
	{
		this.trackedEntitySet.Clear();
		this.trackedEntityHashTable.clearMap();
	}

	// Token: 0x06003A0D RID: 14861 RVA: 0x001797B5 File Offset: 0x001779B5
	public NetEntityDistributionEntry FindEntry(Entity entity)
	{
		return this.trackedEntityHashTable.lookup(entity.entityId) as NetEntityDistributionEntry;
	}

	// Token: 0x04002F59 RID: 12121
	public const float cHighPriorityRange = 5f;

	// Token: 0x04002F5A RID: 12122
	public const float cLowPriorityRange = 18f;

	// Token: 0x04002F5B RID: 12123
	public const float cLowestPriorityRange = 25f;

	// Token: 0x04002F5C RID: 12124
	public const int MobsUpdateTicks = 3;

	// Token: 0x04002F5D RID: 12125
	public const int lowPriorityTick = 6;

	// Token: 0x04002F5E RID: 12126
	public const int lowestPriorityTick = 10;

	// Token: 0x04002F5F RID: 12127
	public static float priorityViewAngleLimit = 60f;

	// Token: 0x04002F60 RID: 12128
	[PublicizedFrom(EAccessModifier.Private)]
	public const float priorityViewAngleMinDistance = 128f;

	// Token: 0x04002F61 RID: 12129
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetList<NetEntityDistributionEntry> trackedEntitySet;

	// Token: 0x04002F62 RID: 12130
	[PublicizedFrom(EAccessModifier.Private)]
	public IntHashMap trackedEntityHashTable;

	// Token: 0x04002F63 RID: 12131
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04002F64 RID: 12132
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

	// Token: 0x04002F65 RID: 12133
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityPlayer> playerList = new List<EntityPlayer>();

	// Token: 0x04002F66 RID: 12134
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EntityEnemy> enemyList = new List<EntityEnemy>();

	// Token: 0x02000797 RID: 1943
	[PublicizedFrom(EAccessModifier.Private)]
	public struct SEnts
	{
		// Token: 0x06003A0F RID: 14863 RVA: 0x001797D9 File Offset: 0x001779D9
		public SEnts(Type _eType, int _distance, int _update, bool _motion)
		{
			this.eType = _eType;
			this.distance = _distance;
			this.update = _update;
			this.motion = _motion;
		}

		// Token: 0x04002F67 RID: 12135
		public Type eType;

		// Token: 0x04002F68 RID: 12136
		public int distance;

		// Token: 0x04002F69 RID: 12137
		public int update;

		// Token: 0x04002F6A RID: 12138
		public bool motion;
	}
}
