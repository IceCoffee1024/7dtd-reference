using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003E3 RID: 995
[Preserve]
public class DroneManager
{
	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06001E00 RID: 7680 RVA: 0x000B67AC File Offset: 0x000B49AC
	public static DroneManager Instance
	{
		get
		{
			return DroneManager.instance;
		}
	}

	// Token: 0x06001E02 RID: 7682 RVA: 0x000B67F2 File Offset: 0x000B49F2
	public static void Init()
	{
		DroneManager.instance = new DroneManager();
		DroneManager.instance.Load();
	}

	// Token: 0x06001E03 RID: 7683 RVA: 0x000B6808 File Offset: 0x000B4A08
	public void AddTrackedDrone(EntityDrone _drone)
	{
		if (!_drone)
		{
			Log.Error("{0} AddTrackedDrone null", new object[] { base.GetType() });
			return;
		}
		_drone.OnWakeUp();
		if (!this.dronesActive.Contains(_drone))
		{
			this.dronesActive.Add(_drone);
			this.TriggerSave();
		}
	}

	// Token: 0x06001E04 RID: 7684 RVA: 0x000B6860 File Offset: 0x000B4A60
	public void RemoveTrackedDrone(EntityDrone _drone, EnumRemoveEntityReason _reason)
	{
		this.dronesActive.Remove(_drone);
		if (_reason == EnumRemoveEntityReason.Unloaded)
		{
			EntityAlive owner = _drone.Owner;
			if (owner)
			{
				OwnedEntityData ownedEntity = owner.GetOwnedEntity(_drone.entityId);
				if (ownedEntity != null)
				{
					ownedEntity.SetLastKnownPosition(_drone.position);
				}
			}
			this.dronesUnloaded.Add(new EntityCreationData(_drone, StreamModeWrite.ToClient));
		}
		this.TriggerSave();
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageVehicleCount>().Setup(), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x06001E05 RID: 7685 RVA: 0x000B68E8 File Offset: 0x000B4AE8
	public void TriggerSave()
	{
		this.saveTime = Mathf.Min(this.saveTime, 10f);
	}

	// Token: 0x06001E06 RID: 7686 RVA: 0x000B6900 File Offset: 0x000B4B00
	public void Update()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		World world = GameManager.Instance.World;
		if (world == null || world.Players == null || world.Players.Count == 0)
		{
			return;
		}
		if (!GameManager.Instance.gameStateManager.IsGameStarted())
		{
			return;
		}
		int num = 0;
		for (int i = this.dronesUnloaded.Count - 1; i >= 0; i--)
		{
			EntityCreationData entityCreationData = this.dronesUnloaded[i];
			EntityDrone entityDrone = world.GetEntity(entityCreationData.id) as EntityDrone;
			if (entityDrone)
			{
				Log.Warning("{0} already loaded #{1}, id {2}, {3}, {4}", new object[]
				{
					base.GetType(),
					i,
					entityCreationData.id,
					entityDrone,
					entityDrone.position.ToCultureInvariantString()
				});
				this.dronesUnloaded.RemoveAt(i);
			}
			else if (world.IsChunkAreaCollidersLoaded(entityCreationData.pos))
			{
				if (!this.isValidDronePos(entityCreationData.pos))
				{
					bool flag = false;
					IDictionary<PlatformUserIdentifierAbs, PersistentPlayerData> players = GameManager.Instance.GetPersistentPlayerList().Players;
					if (players != null)
					{
						foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in players)
						{
							EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(keyValuePair.Value.EntityId) as EntityPlayer;
							if (entityPlayer)
							{
								List<OwnedEntityData> ownedEntities = entityPlayer.ownedEntities;
								for (int j = 0; j < ownedEntities.Count; j++)
								{
									if (entityCreationData.id == ownedEntities[j].Id)
									{
										Log.Warning("recovering {0} owned entity for {1}", new object[] { entityCreationData.id, entityPlayer.entityId });
										entityCreationData.pos = entityPlayer.getHeadPosition();
										entityCreationData.belongsPlayerId = entityPlayer.entityId;
										flag = true;
										break;
									}
								}
							}
						}
					}
					if (!flag)
					{
						entityCreationData.pos = Vector3.zero;
						if (entityCreationData.belongsPlayerId == -1)
						{
							this.dronesWithoutOwner.Add(entityCreationData);
							this.dronesUnloaded.RemoveAt(i);
							goto IL_027F;
						}
						goto IL_027F;
					}
				}
				entityDrone = this.CreateDroneEntity(entityCreationData, world);
				if (entityDrone)
				{
					num++;
				}
				else
				{
					Log.Error("DroneManager load failed #{0}, id {1}, {2}", new object[]
					{
						i,
						entityCreationData.id,
						EntityClass.GetEntityClassName(entityCreationData.entityClass)
					});
				}
				this.dronesUnloaded.RemoveAt(i);
			}
			IL_027F:;
		}
		for (int k = 0; k < this.dronesActive.Count; k++)
		{
			EntityDrone entityDrone2 = this.dronesActive[k];
			if (entityDrone2.Owner && (entityDrone2.position - entityDrone2.Owner.getChestPosition()).sqrMagnitude > 1024f && entityDrone2.GetState() != EntityDrone.State.Shutdown && entityDrone2.OrderState != EntityDrone.Orders.Stay)
			{
				entityDrone2.TeleportOutOfRange();
			}
		}
		this.saveTime -= Time.deltaTime;
		if (this.saveTime <= 0f && (this.saveThread == null || this.saveThread.HasTerminated()))
		{
			this.saveTime = 120f;
			this.Save();
		}
	}

	// Token: 0x06001E07 RID: 7687 RVA: 0x000B6C6C File Offset: 0x000B4E6C
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityDrone CreateDroneEntity(EntityCreationData data, World world)
	{
		EntityDrone entityDrone = EntityFactory.CreateEntity(data) as EntityDrone;
		if (entityDrone)
		{
			this.dronesActive.Add(entityDrone);
			world.SpawnEntityInWorld(entityDrone);
			entityDrone.SyncOwnerData();
		}
		return entityDrone;
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x000B6CA8 File Offset: 0x000B4EA8
	public EntityDrone LoadDrone(int _entityId, World world)
	{
		EntityDrone entityDrone = null;
		foreach (EntityCreationData entityCreationData in this.dronesUnloaded)
		{
			if (entityCreationData.id == _entityId)
			{
				entityDrone = this.CreateDroneEntity(entityCreationData, world);
				break;
			}
		}
		return entityDrone;
	}

	// Token: 0x06001E09 RID: 7689 RVA: 0x000B6D0C File Offset: 0x000B4F0C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isValidDronePos(Vector3 pos)
	{
		return !float.IsNaN(pos.x) && !float.IsNaN(pos.y) && !float.IsNaN(pos.z);
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x000B6D38 File Offset: 0x000B4F38
	public void RemoveAllDronesFromMap()
	{
		GameManager gameManager = GameManager.Instance;
		PersistentPlayerList persistentPlayerList = gameManager.GetPersistentPlayerList();
		World world = gameManager.World;
		foreach (KeyValuePair<PlatformUserIdentifierAbs, PersistentPlayerData> keyValuePair in persistentPlayerList.Players)
		{
			EntityPlayer entityPlayer = world.GetEntity(keyValuePair.Value.EntityId) as EntityPlayer;
			if (entityPlayer)
			{
				List<OwnedEntityData> ownedEntities = entityPlayer.ownedEntities;
				int num;
				int i;
				Predicate<EntityDrone> <>9__0;
				for (i = ownedEntities.Count - 1; i >= 0; i = num)
				{
					List<EntityDrone> list = this.dronesActive;
					Predicate<EntityDrone> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (EntityDrone v) => v.entityId == ownedEntities[i].Id);
					}
					EntityDrone entityDrone = list.Find(predicate);
					if (entityDrone && entityPlayer.HasOwnedEntity(entityDrone.entityId))
					{
						GameManager.Instance.World.RemoveEntityFromMap(entityDrone, EnumRemoveEntityReason.Unloaded);
					}
					num = i - 1;
				}
			}
		}
		this.UpdateWaypointsForAllPlayers();
	}

	// Token: 0x06001E0B RID: 7691 RVA: 0x000B6E68 File Offset: 0x000B5068
	public void ClearAllDronesForPlayer(EntityPlayer player)
	{
		this.ClearAllDronesForPlayer(player.entityId);
	}

	// Token: 0x06001E0C RID: 7692 RVA: 0x000B6E76 File Offset: 0x000B5076
	public void ClearAllDronesForPlayer(int entityId)
	{
		this.ClearUnloadedDrones(entityId);
		this.ClearActiveDrones(entityId);
		this.TriggerSave();
		this.UpdateWaypointsForPlayer(entityId);
	}

	// Token: 0x06001E0D RID: 7693 RVA: 0x000B6E93 File Offset: 0x000B5093
	public void ClearUnloadedDrones(EntityPlayer player)
	{
		this.ClearUnloadedDrones(player.entityId);
	}

	// Token: 0x06001E0E RID: 7694 RVA: 0x000B6EA4 File Offset: 0x000B50A4
	public void ClearUnloadedDrones(int entityId)
	{
		for (int i = this.dronesUnloaded.Count - 1; i >= 0; i--)
		{
			if (this.dronesUnloaded[i].belongsPlayerId == entityId)
			{
				this.dronesUnloaded.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001E0F RID: 7695 RVA: 0x000B6EEC File Offset: 0x000B50EC
	public void ClearActiveDrones(int entityId)
	{
		for (int i = this.dronesActive.Count - 1; i >= 0; i--)
		{
			EntityDrone entityDrone = this.dronesActive[i];
			if (entityDrone.belongsPlayerId == entityId)
			{
				this.dronesActive.RemoveAt(i);
				GameManager.Instance.World.RemoveEntity(entityDrone.entityId, EnumRemoveEntityReason.Killed);
			}
		}
	}

	// Token: 0x06001E10 RID: 7696 RVA: 0x000B6F4C File Offset: 0x000B514C
	public string LogActiveDrones()
	{
		string text = string.Empty;
		for (int i = 0; i < this.dronesActive.Count; i++)
		{
			EntityDrone entityDrone = this.dronesActive[i];
			string[] array = new string[8];
			array[0] = text;
			array[1] = "owner: ";
			int num = 2;
			PlatformUserIdentifierAbs ownerID = entityDrone.OwnerID;
			array[num] = ((ownerID != null) ? ownerID.ToString() : null);
			array[3] = " pid: ";
			array[4] = entityDrone.belongsPlayerId.ToString();
			array[5] = " id: ";
			array[6] = entityDrone.EntityId.ToString();
			array[7] = Environment.NewLine;
			text = string.Concat(array);
		}
		return text;
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x000B6FEC File Offset: 0x000B51EC
	public string LogUnloadedDrones()
	{
		string text = string.Empty;
		for (int i = 0; i < this.dronesUnloaded.Count; i++)
		{
			EntityCreationData entityCreationData = this.dronesUnloaded[i];
			text = string.Concat(new string[]
			{
				text,
				"pid: ",
				entityCreationData.belongsPlayerId.ToString(),
				" id: ",
				entityCreationData.id.ToString(),
				Environment.NewLine
			});
		}
		return text;
	}

	// Token: 0x06001E12 RID: 7698 RVA: 0x000B7068 File Offset: 0x000B5268
	public bool AssignUnloadedDrone(EntityPlayer player, int entityId)
	{
		for (int i = 0; i < this.dronesUnloaded.Count; i++)
		{
			EntityCreationData entityCreationData = this.dronesUnloaded[i];
			if (entityCreationData.id == entityId)
			{
				entityCreationData.pos = player.getHeadPosition();
				entityCreationData.belongsPlayerId = player.entityId;
				Log.Warning(entityCreationData.belongsPlayerId.ToString());
				this.debugDronePlayerAssignment.Add(entityCreationData.belongsPlayerId);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x000B70E0 File Offset: 0x000B52E0
	public EntityDrone GetActiveDronesWithId(int entityId)
	{
		for (int i = this.dronesActive.Count - 1; i >= 0; i--)
		{
			EntityDrone entityDrone = this.dronesActive[i];
			if (entityDrone.entityId == entityId)
			{
				return entityDrone;
			}
		}
		return null;
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x000B7120 File Offset: 0x000B5320
	public void RemoveActiveDrone(int entityId)
	{
		EntityDrone drone = this.GetActiveDronesWithId(entityId);
		if (drone)
		{
			World world = GameManager.Instance.World;
			EntityPlayer entityPlayer = world.Players.list.Find((EntityPlayer p) => p == drone.Owner);
			if (entityPlayer)
			{
				entityPlayer.RemoveOwnedEntity(drone);
			}
			drone.belongsPlayerId = -1;
			drone.Owner = null;
			drone.OwnerID = null;
			this.dronesActive.Remove(drone);
			world.RemoveEntity(entityId, EnumRemoveEntityReason.Killed);
		}
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x000B71C8 File Offset: 0x000B53C8
	public List<EntityCreationData> GetAllDronesECD()
	{
		List<EntityCreationData> list = new List<EntityCreationData>();
		for (int i = 0; i < this.dronesActive.Count; i++)
		{
			list.Add(new EntityCreationData(this.dronesActive[i], StreamModeWrite.Persistency));
		}
		for (int j = 0; j < this.dronesUnloaded.Count; j++)
		{
			list.Add(this.dronesUnloaded[j]);
		}
		return list;
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x000B7234 File Offset: 0x000B5434
	public void UpdateWaypointsForAllPlayers()
	{
		foreach (EntityPlayer entityPlayer in GameManager.Instance.World.GetPlayers())
		{
			this.UpdateWaypointsForPlayer(entityPlayer.entityId);
		}
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x000B7298 File Offset: 0x000B5498
	public void UpdateWaypointsForPlayer(int entityId)
	{
		EntityPlayer entityPlayer = (EntityPlayer)GameManager.Instance.World.GetEntity(entityId);
		if (entityPlayer == null)
		{
			return;
		}
		NavObjectManager.Instance.UnRegisterNavObjectByClass(EntityClass.junkDroneClass);
		List<EntityCreationData> allDronesECD = this.GetAllDronesECD();
		List<OwnedEntityData> ownedEntities = entityPlayer.GetOwnedEntities(EntityClass.junkDroneClass);
		List<ValueTuple<int, Vector3>> list = new List<ValueTuple<int, Vector3>>();
		foreach (OwnedEntityData ownedEntityData in ownedEntities)
		{
			foreach (EntityCreationData entityCreationData in allDronesECD)
			{
				if (entityCreationData.id == ownedEntityData.Id)
				{
					list.Add(new ValueTuple<int, Vector3>(ownedEntityData.Id, entityCreationData.pos));
				}
			}
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null && entityId == primaryPlayer.entityId)
		{
			primaryPlayer.Waypoints.SetDroneWaypointsFromDroneManager(list);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityWaypointList>().Setup(eWayPointListType.Drone, list), false, entityId, -1, -1, null, 192, false);
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x000B73E4 File Offset: 0x000B55E4
	public void SpawnFollowingDronesForPLayer(int entityId, World world)
	{
		if ((EntityPlayer)GameManager.Instance.World.GetEntity(entityId) == null)
		{
			return;
		}
		foreach (EntityCreationData entityCreationData in this.GetAllDronesECD())
		{
			if (entityCreationData.belongsPlayerId == entityId && this.dronesUnloaded.Contains(entityCreationData) && entityCreationData.orderState == 0)
			{
				this.CreateDroneEntity(entityCreationData, world).TeleportIfFollowing();
			}
		}
		foreach (EntityDrone entityDrone in this.dronesActive)
		{
			if (entityDrone.belongsPlayerId == entityId && entityDrone.OrderState == EntityDrone.Orders.Follow)
			{
				entityDrone.TeleportIfFollowing();
			}
		}
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x000B74CC File Offset: 0x000B56CC
	public static void Cleanup()
	{
		if (DroneManager.instance != null)
		{
			DroneManager.instance.SaveAndClear();
		}
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x000B74DF File Offset: 0x000B56DF
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveAndClear()
	{
		this.WaitOnSave();
		this.Save();
		this.WaitOnSave();
		this.dronesActive.Clear();
		this.dronesUnloaded.Clear();
		DroneManager.instance = null;
	}

	// Token: 0x06001E1B RID: 7707 RVA: 0x000B750F File Offset: 0x000B570F
	[PublicizedFrom(EAccessModifier.Private)]
	public void WaitOnSave()
	{
		if (this.saveThread != null)
		{
			this.saveThread.WaitForEnd(30);
			this.saveThread = null;
		}
	}

	// Token: 0x06001E1C RID: 7708 RVA: 0x000B7530 File Offset: 0x000B5730
	public void Load()
	{
		string text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "drones.dat");
		if (SdFile.Exists(text))
		{
			try
			{
				using (Stream stream = SdFile.OpenRead(text))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream);
						this.read(pooledBinaryReader);
					}
				}
			}
			catch (Exception)
			{
				text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "drones.dat.bak");
				if (SdFile.Exists(text))
				{
					using (Stream stream2 = SdFile.OpenRead(text))
					{
						using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader2.SetBaseStream(stream2);
							this.read(pooledBinaryReader2);
						}
					}
				}
			}
			Log.Out("{0} {1}, loaded {2}", new object[]
			{
				base.GetType(),
				text,
				this.dronesUnloaded.Count
			});
		}
	}

	// Token: 0x06001E1D RID: 7709 RVA: 0x000B7668 File Offset: 0x000B5868
	[PublicizedFrom(EAccessModifier.Private)]
	public void Save()
	{
		if (this.saveThread == null || !ThreadManager.ActiveThreads.ContainsKey("droneDataSave"))
		{
			Log.Out("{0} saving {1} ({2} + {3})", new object[]
			{
				base.GetType(),
				this.dronesActive.Count + this.dronesUnloaded.Count,
				this.dronesActive.Count,
				this.dronesUnloaded.Count
			});
			PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				this.write(pooledBinaryWriter);
			}
			this.saveThread = ThreadManager.StartThread("droneDataSave", null, new ThreadManager.ThreadFunctionLoopDelegate(this.SaveThread), null, pooledExpandableMemoryStream, null, false, true);
		}
	}

	// Token: 0x06001E1E RID: 7710 RVA: 0x000B7754 File Offset: 0x000B5954
	[PublicizedFrom(EAccessModifier.Private)]
	public int SaveThread(ThreadManager.ThreadInfo _threadInfo)
	{
		PooledExpandableMemoryStream pooledExpandableMemoryStream = (PooledExpandableMemoryStream)_threadInfo.parameter;
		string text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "drones.dat");
		if (SdFile.Exists(text))
		{
			SdFile.Copy(text, string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "drones.dat.bak"), true);
		}
		pooledExpandableMemoryStream.Position = 0L;
		StreamUtils.WriteStreamToFile(pooledExpandableMemoryStream, text);
		Log.Out("{0} saved {1} bytes", new object[]
		{
			base.GetType(),
			pooledExpandableMemoryStream.Length
		});
		MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		return -1;
	}

	// Token: 0x06001E1F RID: 7711 RVA: 0x000B77E8 File Offset: 0x000B59E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void read(PooledBinaryReader _br)
	{
		if (_br.ReadChar() != 'v' || _br.ReadChar() != 'd' || _br.ReadChar() != 'a' || _br.ReadChar() != '\0')
		{
			Log.Error("{0} file bad signature", new object[] { base.GetType() });
			return;
		}
		if (_br.ReadByte() != 1)
		{
			Log.Error("{0} file bad version", new object[] { base.GetType() });
			return;
		}
		this.dronesUnloaded.Clear();
		int num = _br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			EntityCreationData entityCreationData = new EntityCreationData();
			entityCreationData.read(_br, StreamModeRead.Persistency);
			this.dronesUnloaded.Add(entityCreationData);
		}
	}

	// Token: 0x06001E20 RID: 7712 RVA: 0x000B7890 File Offset: 0x000B5A90
	[PublicizedFrom(EAccessModifier.Private)]
	public void write(PooledBinaryWriter _bw)
	{
		_bw.Write('v');
		_bw.Write('d');
		_bw.Write('a');
		_bw.Write(0);
		_bw.Write(1);
		List<EntityCreationData> list = new List<EntityCreationData>();
		this.GetDrones(list);
		_bw.Write(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			EntityCreationData entityCreationData = list[i];
			if (!this.isValidDronePos(entityCreationData.pos))
			{
				EntityPlayer entityPlayer = GameManager.Instance.World.GetEntity(entityCreationData.belongsPlayerId) as EntityPlayer;
				if (entityPlayer)
				{
					Log.Warning("corrupted data using the player position");
					entityCreationData.pos = entityPlayer.getHeadPosition();
				}
				else
				{
					Log.Warning("corrupted data clearing the drone position");
					entityCreationData.pos = Vector3.zero;
				}
			}
			entityCreationData.write(_bw, StreamModeWrite.Persistency);
		}
	}

	// Token: 0x06001E21 RID: 7713 RVA: 0x000B795C File Offset: 0x000B5B5C
	public List<EntityCreationData> GetDronesList()
	{
		List<EntityCreationData> list = new List<EntityCreationData>();
		this.GetDrones(list);
		return list;
	}

	// Token: 0x06001E22 RID: 7714 RVA: 0x000B7978 File Offset: 0x000B5B78
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetDrones(List<EntityCreationData> _list)
	{
		for (int i = 0; i < this.dronesActive.Count; i++)
		{
			_list.Add(new EntityCreationData(this.dronesActive[i], StreamModeWrite.ToClient));
		}
		for (int j = 0; j < this.dronesUnloaded.Count; j++)
		{
			_list.Add(this.dronesUnloaded[j]);
		}
		for (int k = 0; k < this.dronesWithoutOwner.Count; k++)
		{
			_list.Add(this.dronesWithoutOwner[k]);
		}
	}

	// Token: 0x06001E23 RID: 7715 RVA: 0x000B7A04 File Offset: 0x000B5C04
	[return: TupleElementNames(new string[] { "entityId", "position" })]
	public List<ValueTuple<int, Vector3>> GetDronePositionsList()
	{
		List<ValueTuple<int, Vector3>> list = new List<ValueTuple<int, Vector3>>();
		for (int i = 0; i < this.dronesActive.Count; i++)
		{
			list.Add(new ValueTuple<int, Vector3>(this.dronesActive[i].entityId, this.dronesActive[i].position));
		}
		for (int j = 0; j < this.dronesUnloaded.Count; j++)
		{
			list.Add(new ValueTuple<int, Vector3>(this.dronesUnloaded[j].id, this.dronesUnloaded[j].pos));
		}
		for (int k = 0; k < this.dronesWithoutOwner.Count; k++)
		{
			list.Add(new ValueTuple<int, Vector3>(this.dronesWithoutOwner[k].id, this.dronesWithoutOwner[k].pos));
		}
		return list;
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x000B7AE1 File Offset: 0x000B5CE1
	public static int GetServerDroneCount()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return DroneManager.Instance.dronesActive.Count + DroneManager.Instance.dronesUnloaded.Count;
		}
		return DroneManager.serverDroneCount;
	}

	// Token: 0x06001E25 RID: 7717 RVA: 0x000B7B14 File Offset: 0x000B5D14
	public static void SetServerDroneCount(int count)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		DroneManager.serverDroneCount = count;
	}

	// Token: 0x06001E26 RID: 7718 RVA: 0x000B7B29 File Offset: 0x000B5D29
	public static bool CanAddMoreDrones()
	{
		return !(DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() || DroneManager.GetServerDroneCount() < 500;
	}

	// Token: 0x06001E27 RID: 7719 RVA: 0x000B7B44 File Offset: 0x000B5D44
	[Conditional("DEBUG_DRONEMAN")]
	public static void VMLog(string _format = "", params object[] _args)
	{
		int frameCount = GameManager.frameCount;
		_format = string.Format("{0} {1} {2}", frameCount, "DroneManager", _format);
		Log.Out(_format, _args);
	}

	// Token: 0x04001406 RID: 5126
	public static bool Debug_LocalControl;

	// Token: 0x04001407 RID: 5127
	public static bool DebugLogEnabled;

	// Token: 0x04001408 RID: 5128
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cVersion = 1;

	// Token: 0x04001409 RID: 5129
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSaveTime = 120f;

	// Token: 0x0400140A RID: 5130
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cChangeSaveDelay = 10f;

	// Token: 0x0400140B RID: 5131
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMaxDrones = 500;

	// Token: 0x0400140C RID: 5132
	public const int cMaxActiveDronePlayerRange = 32;

	// Token: 0x0400140D RID: 5133
	[PublicizedFrom(EAccessModifier.Private)]
	public static int serverDroneCount;

	// Token: 0x0400140E RID: 5134
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityDrone> dronesActive = new List<EntityDrone>();

	// Token: 0x0400140F RID: 5135
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityCreationData> dronesUnloaded = new List<EntityCreationData>();

	// Token: 0x04001410 RID: 5136
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<EntityCreationData> dronesWithoutOwner = new List<EntityCreationData>();

	// Token: 0x04001411 RID: 5137
	[PublicizedFrom(EAccessModifier.Private)]
	public float saveTime = 120f;

	// Token: 0x04001412 RID: 5138
	[PublicizedFrom(EAccessModifier.Private)]
	public static DroneManager instance;

	// Token: 0x04001413 RID: 5139
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo saveThread;

	// Token: 0x04001414 RID: 5140
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> debugDronePlayerAssignment = new List<int>();

	// Token: 0x04001415 RID: 5141
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cNameKey = "drones";

	// Token: 0x04001416 RID: 5142
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cThreadKey = "droneDataSave";

	// Token: 0x04001417 RID: 5143
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cStringName = "DroneManager";
}
