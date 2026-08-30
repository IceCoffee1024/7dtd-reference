using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003FE RID: 1022
[Preserve]
public class AIDirector
{
	// Token: 0x06001F8F RID: 8079 RVA: 0x000BF1C5 File Offset: 0x000BD3C5
	public AIDirector(World _world)
	{
		this.World = _world;
		this.CreateComponents();
		this.Init();
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x000BF1F6 File Offset: 0x000BD3F6
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		this.random = GameRandomManager.Instance.CreateGameRandom();
		this.ComponentsInitNewGame();
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x000BF210 File Offset: 0x000BD410
	[PublicizedFrom(EAccessModifier.Private)]
	public void CreateComponents()
	{
		this.CreateComponent<AIDirectorMarkerManagementComponent>();
		this.CreateComponent<AIDirectorPlayerManagementComponent>();
		this.CreateComponent<AIDirectorWanderingHordeComponent>();
		this.CreateComponent<AIDirectorAirDropComponent>();
		this.CreateComponent<AIDirectorChunkEventComponent>();
		this.CreateComponent<AIDirectorBloodMoonComponent>();
		this.playerManagementComponent = this.GetComponent<AIDirectorPlayerManagementComponent>();
		this.chunkEventComponent = this.GetComponent<AIDirectorChunkEventComponent>();
		this.bloodMoonComponent = this.GetComponent<AIDirectorBloodMoonComponent>();
	}

	// Token: 0x06001F92 RID: 8082 RVA: 0x000BF26C File Offset: 0x000BD46C
	[PublicizedFrom(EAccessModifier.Private)]
	public T CreateComponent<T>() where T : AIDirectorComponent, new()
	{
		string fullName = typeof(T).FullName;
		if (this.components.dict.ContainsKey(fullName))
		{
			throw new Exception("Multiple instances of the same component type are not allowed!");
		}
		T t = new T();
		t.Director = this;
		this.components.Add(fullName, t);
		return t;
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x000BF2CC File Offset: 0x000BD4CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsInitNewGame()
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].InitNewGame();
		}
	}

	// Token: 0x06001F94 RID: 8084 RVA: 0x000BF30C File Offset: 0x000BD50C
	public T GetComponent<T>() where T : AIDirectorComponent
	{
		string fullName = typeof(T).FullName;
		AIDirectorComponent aidirectorComponent;
		if (this.components.dict.TryGetValue(fullName, out aidirectorComponent))
		{
			return aidirectorComponent as T;
		}
		return default(T);
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x06001F95 RID: 8085 RVA: 0x000BF353 File Offset: 0x000BD553
	public AIDirectorBloodMoonComponent BloodMoonComponent
	{
		get
		{
			return this.bloodMoonComponent;
		}
	}

	// Token: 0x06001F96 RID: 8086 RVA: 0x000BF35C File Offset: 0x000BD55C
	public void Load(BinaryReader stream)
	{
		int num = stream.ReadInt32();
		this.ComponentsLoad(stream, num);
		if (this.World.worldTime == 0UL)
		{
			this.Init();
		}
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x000BF38B File Offset: 0x000BD58B
	public void Save(BinaryWriter stream)
	{
		stream.Write(10);
		this.ComponentsSave(stream);
	}

	// Token: 0x06001F98 RID: 8088 RVA: 0x000BF39C File Offset: 0x000BD59C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsLoad(BinaryReader reader, int version)
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].Read(reader, version);
		}
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x000BF3DC File Offset: 0x000BD5DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsSave(BinaryWriter writer)
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].Write(writer);
		}
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x000BF41B File Offset: 0x000BD61B
	public void Tick(double dt)
	{
		this.ComponentsTick(dt);
		this.DebugTick();
	}

	// Token: 0x06001F9B RID: 8091 RVA: 0x000BF42C File Offset: 0x000BD62C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsTick(double _dt)
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].Tick(_dt);
		}
	}

	// Token: 0x06001F9C RID: 8092 RVA: 0x000BF46B File Offset: 0x000BD66B
	public static bool CanSpawn(float _priority = 1f)
	{
		return (float)GameStats.GetInt(EnumGameStats.EnemyCount) < (float)GamePrefs.GetInt(EnumGamePrefs.MaxSpawnedZombies) * _priority;
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x000BF484 File Offset: 0x000BD684
	public static ulong GetActivityWorldTimeDelay()
	{
		float num = (float)GameStats.GetInt(EnumGameStats.TimeOfDayIncPerSec) / 6f;
		num = Utils.FastClamp(num, 0.2f, 5f);
		return (ulong)(1000f * num);
	}

	// Token: 0x06001F9E RID: 8094 RVA: 0x000BF4BC File Offset: 0x000BD6BC
	public void NotifyActivity(EnumAIDirectorChunkEvent type, Vector3i position, float value, float _duration = 720f)
	{
		if (value > 0f && GameStats.GetBool(EnumGameStats.ZombieHordeMeter) && GameStats.GetBool(EnumGameStats.IsSpawnEnemies) && AIDirector.HeatMapSensitivityModifier > 0f && !this.BloodMoonComponent.BloodMoonActive && !TwitchManager.BossHordeActive)
		{
			AIDirectorChunkEvent aidirectorChunkEvent = new AIDirectorChunkEvent(type, position, value * AIDirector.HeatMapSensitivityModifier, _duration);
			this.chunkEventComponent.NotifyEvent(aidirectorChunkEvent);
		}
	}

	// Token: 0x06001F9F RID: 8095 RVA: 0x000BF520 File Offset: 0x000BD720
	public void NotifyNoise(Entity instigator, Vector3 position, string clipName, float volumeScale)
	{
		AIDirectorData.Noise noise;
		if (!AIDirectorData.FindNoise(clipName, out noise))
		{
			return;
		}
		if (instigator is EntityEnemy)
		{
			return;
		}
		AIDirectorPlayerState aidirectorPlayerState = null;
		if (instigator)
		{
			if (instigator.IsIgnoredByAI())
			{
				return;
			}
			this.playerManagementComponent.trackedPlayers.dict.TryGetValue(instigator.entityId, out aidirectorPlayerState);
		}
		EntityItem entityItem = instigator as EntityItem;
		if (entityItem != null && ItemClass.GetForId(entityItem.itemStack.itemValue.type).ThrowableDecoy.Value)
		{
			return;
		}
		if (aidirectorPlayerState != null)
		{
			if (aidirectorPlayerState.Player.IsCrouching)
			{
				volumeScale *= noise.muffledWhenCrouched;
			}
			float num = noise.volume * volumeScale;
			if (aidirectorPlayerState.Player.Stealth.NotifyNoise(num, noise.duration))
			{
				instigator.world.CheckSleeperVolumeNoise(position);
			}
		}
		if (noise.heatMapStrength > 0f)
		{
			this.NotifyActivity(EnumAIDirectorChunkEvent.Sound, World.worldToBlockPos(position), noise.heatMapStrength * volumeScale, 240f);
		}
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x000027FC File Offset: 0x000009FC
	public void NotifyIntentToAttack(EntityAlive zombie, EntityAlive player)
	{
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x000BF610 File Offset: 0x000BD810
	public void UpdatePlayerInventory(EntityPlayerLocal player)
	{
		this.playerManagementComponent.UpdatePlayerInventory(player);
	}

	// Token: 0x06001FA2 RID: 8098 RVA: 0x000BF61E File Offset: 0x000BD81E
	public void UpdatePlayerInventory(int entityId, AIDirectorPlayerInventory inventory)
	{
		this.playerManagementComponent.UpdatePlayerInventory(entityId, inventory);
	}

	// Token: 0x06001FA3 RID: 8099 RVA: 0x000BF630 File Offset: 0x000BD830
	public void OnSoundPlayedAtPosition(int _entityThatCausedSound, Vector3 _position, string clipName, float volumeScale)
	{
		Entity entity = null;
		if (_entityThatCausedSound != -1)
		{
			entity = this.World.GetEntity(_entityThatCausedSound);
		}
		this.NotifyNoise(entity, _position, clipName, volumeScale);
	}

	// Token: 0x06001FA4 RID: 8100 RVA: 0x000BF65C File Offset: 0x000BD85C
	public void AddEntity(Entity entity)
	{
		EntityPlayer entityPlayer;
		if (entityPlayer = entity as EntityPlayer)
		{
			this.AddPlayer(entityPlayer);
		}
	}

	// Token: 0x06001FA5 RID: 8101 RVA: 0x000BF680 File Offset: 0x000BD880
	public void RemoveEntity(Entity entity)
	{
		EntityPlayer entityPlayer;
		if (entityPlayer = entity as EntityPlayer)
		{
			this.RemovePlayer(entityPlayer);
		}
	}

	// Token: 0x06001FA6 RID: 8102 RVA: 0x000BF6A3 File Offset: 0x000BD8A3
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddPlayer(EntityPlayer player)
	{
		this.playerManagementComponent.AddPlayer(player);
		this.BloodMoonComponent.AddPlayer(player);
	}

	// Token: 0x06001FA7 RID: 8103 RVA: 0x000BF6BD File Offset: 0x000BD8BD
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemovePlayer(EntityPlayer player)
	{
		this.playerManagementComponent.RemovePlayer(player);
		this.BloodMoonComponent.RemovePlayer(player);
	}

	// Token: 0x06001FA8 RID: 8104 RVA: 0x000BF6D7 File Offset: 0x000BD8D7
	public static void LogAI(string _format, params object[] _args)
	{
		_format = string.Format("AIDirector: {0}", _format);
		Log.Out(_format, _args);
	}

	// Token: 0x06001FA9 RID: 8105 RVA: 0x000BF6ED File Offset: 0x000BD8ED
	public static void LogAIExtra(string _format, params object[] _args)
	{
		if (AIDirectorConstants.DebugOutput)
		{
			AIDirector.LogAI(_format, _args);
		}
	}

	// Token: 0x06001FAA RID: 8106 RVA: 0x000BF6FD File Offset: 0x000BD8FD
	public void DebugFrameLateUpdate()
	{
		if (AIDirector.debugSendLatencyToPlayerIds.Count > 0)
		{
			this.DebugSendLatency();
		}
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x000BF712 File Offset: 0x000BD912
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugTick()
	{
		if (AIDirector.debugSendNameInfoToPlayerIds.Count > 0)
		{
			this.DebugSendNameInfo();
		}
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x000BF728 File Offset: 0x000BD928
	public static void DebugToggleSendNameInfo(int playerId)
	{
		if (AIDirector.debugSendNameInfoToPlayerIds.Remove(playerId))
		{
			Log.Out("DebugToggleSendNames {0} off", new object[] { playerId });
			NetPackageDebug netPackageDebug = NetPackageManager.GetPackage<NetPackageDebug>().Setup(NetPackageDebug.Type.AINameInfoClientOff, -1, null);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageDebug, false, playerId, -1, -1, null, 192, false);
			return;
		}
		Log.Out("DebugToggleSendNames {0} on", new object[] { playerId });
		AIDirector.debugSendNameInfoToPlayerIds.Add(playerId);
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x000BF7AC File Offset: 0x000BD9AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugSendNameInfo()
	{
		int num = this.debugNameInfoTicks - 1;
		this.debugNameInfoTicks = num;
		if (num > 0)
		{
			return;
		}
		this.debugNameInfoTicks = 5;
		World world = GameManager.Instance.World;
		for (int i = 0; i < AIDirector.debugSendNameInfoToPlayerIds.Count; i++)
		{
			int num2 = AIDirector.debugSendNameInfoToPlayerIds[i];
			EntityPlayer entityPlayer;
			world.Players.dict.TryGetValue(num2, out entityPlayer);
			if (entityPlayer)
			{
				ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(num2);
				if (clientInfo != null)
				{
					Bounds bounds = new Bounds(entityPlayer.position, new Vector3(50f, 50f, 50f));
					world.GetEntitiesInBounds(typeof(EntityAlive), bounds, this.debugEntities);
					for (int j = this.debugEntities.Count - 1; j >= 0; j--)
					{
						EntityAlive entityAlive = (EntityAlive)this.debugEntities[j];
						if (entityAlive.aiManager != null)
						{
							string text = entityAlive.aiManager.MakeDebugName(entityPlayer);
							NetPackageDebug netPackageDebug = NetPackageManager.GetPackage<NetPackageDebug>().Setup(NetPackageDebug.Type.AINameInfo, entityAlive.entityId, Encoding.UTF8.GetBytes(text));
							clientInfo.SendPackage(netPackageDebug);
						}
					}
					this.debugEntities.Clear();
				}
			}
		}
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x000BF8F8 File Offset: 0x000BDAF8
	public static void DebugReceiveNameInfo(int entityId, byte[] _data)
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		EntityAlive entityAlive = world.GetEntity(entityId) as EntityAlive;
		if (entityAlive)
		{
			entityAlive.SetupDebugNameHUD(true);
			string @string = Encoding.UTF8.GetString(_data);
			entityAlive.DebugNameInfo = @string;
		}
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x000BF943 File Offset: 0x000BDB43
	public static void DebugToggleFreezePos()
	{
		AIDirector.debugFreezePos = !AIDirector.debugFreezePos;
		Log.Out("DebugToggleFreezePos {0}", new object[] { AIDirector.debugFreezePos });
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x000BF970 File Offset: 0x000BDB70
	public static void DebugToggleSendLatency(int playerId)
	{
		if (!AIDirector.debugSendLatencyToPlayerIds.Remove(playerId))
		{
			Log.Out("DebugToggleSendLatency {0} on", new object[] { playerId });
			AIDirector.debugSendLatencyToPlayerIds.Add(playerId);
			return;
		}
		Log.Out("DebugToggleSendLatency {0} off", new object[] { playerId });
		if (GameManager.Instance.World.GetPrimaryPlayerId() != playerId)
		{
			NetPackageDebug netPackageDebug = NetPackageManager.GetPackage<NetPackageDebug>().Setup(NetPackageDebug.Type.AILatencyClientOff, -1, null);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageDebug, false, playerId, -1, -1, null, 192, false);
			return;
		}
		AIDirector.DebugLatencyOff();
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x000BFA0C File Offset: 0x000BDC0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugSendLatency()
	{
		World world = GameManager.Instance.World;
		for (int i = 0; i < AIDirector.debugSendLatencyToPlayerIds.Count; i++)
		{
			int num = AIDirector.debugSendLatencyToPlayerIds[i];
			EntityPlayer entityPlayer;
			world.Players.dict.TryGetValue(num, out entityPlayer);
			if (entityPlayer)
			{
				ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(num);
				Bounds bounds = new Bounds(entityPlayer.position, new Vector3(50f, 50f, 50f));
				world.GetEntitiesInBounds(typeof(EntityAlive), bounds, this.debugEntities);
				for (int j = this.debugEntities.Count - 1; j >= 0; j--)
				{
					EntityAlive entityAlive = (EntityAlive)this.debugEntities[j];
					if (entityAlive.aiManager != null)
					{
						using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
						{
							pooledBinaryWriter.SetBaseStream(AIDirector.latencyStream);
							AIDirector.latencyStream.Position = 0L;
							pooledBinaryWriter.Write(entityAlive.position.x);
							pooledBinaryWriter.Write(entityAlive.position.y);
							pooledBinaryWriter.Write(entityAlive.position.z);
							Vector3 vector = entityAlive.GetVelocityPerSecond();
							Vector3 vector2 = entityAlive.motion * 20f;
							if (vector.sqrMagnitude < vector2.sqrMagnitude)
							{
								vector = vector2;
							}
							pooledBinaryWriter.Write(vector.x);
							pooledBinaryWriter.Write(vector.y);
							pooledBinaryWriter.Write(vector.z);
							Quaternion rotation = entityAlive.transform.rotation;
							pooledBinaryWriter.Write(rotation.x);
							pooledBinaryWriter.Write(rotation.y);
							pooledBinaryWriter.Write(rotation.z);
							pooledBinaryWriter.Write(rotation.w);
							byte[] array = AIDirector.latencyStream.ToArray();
							if (clientInfo != null)
							{
								NetPackageDebug netPackageDebug = NetPackageManager.GetPackage<NetPackageDebug>().Setup(NetPackageDebug.Type.AILatency, entityAlive.entityId, array);
								clientInfo.SendPackage(netPackageDebug);
							}
							else
							{
								AIDirector.DebugReceiveLatency(entityAlive.entityId, array);
							}
						}
					}
				}
				this.debugEntities.Clear();
			}
		}
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x000BFC6C File Offset: 0x000BDE6C
	public static void DebugReceiveLatency(int entityId, byte[] _data)
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		EntityAlive entityAlive = world.GetEntity(entityId) as EntityAlive;
		if (entityAlive)
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				MemoryStream memoryStream = new MemoryStream(_data);
				pooledBinaryReader.SetBaseStream(memoryStream);
				Vector3 vector;
				vector.x = pooledBinaryReader.ReadSingle();
				vector.y = pooledBinaryReader.ReadSingle();
				vector.z = pooledBinaryReader.ReadSingle();
				Vector3 vector2;
				vector2.x = pooledBinaryReader.ReadSingle();
				vector2.y = pooledBinaryReader.ReadSingle();
				vector2.z = pooledBinaryReader.ReadSingle();
				Quaternion quaternion;
				quaternion.x = pooledBinaryReader.ReadSingle();
				quaternion.y = pooledBinaryReader.ReadSingle();
				quaternion.z = pooledBinaryReader.ReadSingle();
				quaternion.w = pooledBinaryReader.ReadSingle();
				Transform transform = entityAlive.transform;
				Transform parent = transform.parent;
				Transform transform2 = parent.Find("DebugLatency");
				if (!transform2)
				{
					GameObject gameObject = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/Debug/DebugLatency"), parent);
					gameObject.name = "DebugLatency";
					transform2 = gameObject.transform;
				}
				Vector3 vector3 = vector - Origin.position;
				transform2.position = vector3;
				transform2.rotation = quaternion;
				LineRenderer component = transform2.GetComponent<LineRenderer>();
				component.SetPosition(0, Quaternion.Inverse(quaternion) * (transform.position - vector3));
				float num = (float)world.GetPrimaryPlayer().pingToServer * 0.001f;
				if (num < 0f)
				{
					num = 0f;
				}
				num *= 2f;
				if (vector2.y < 0f)
				{
					vector2.y = 0f;
				}
				component.SetPosition(2, Quaternion.Inverse(quaternion) * (vector2 * num));
			}
		}
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x000BFE60 File Offset: 0x000BE060
	public static void DebugLatencyOff()
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		for (int i = 0; i < world.Entities.list.Count; i++)
		{
			EntityAlive entityAlive = world.Entities.list[i] as EntityAlive;
			if (entityAlive)
			{
				Transform transform = entityAlive.transform.parent.Find("DebugLatency");
				if (transform)
				{
					global::UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
		}
	}

	// Token: 0x04001559 RID: 5465
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivityDuration = 720f;

	// Token: 0x0400155A RID: 5466
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivityNoiseDuration = 240f;

	// Token: 0x0400155B RID: 5467
	public readonly World World;

	// Token: 0x0400155C RID: 5468
	public GameRandom random;

	// Token: 0x0400155D RID: 5469
	public static float HeatMapSensitivityModifier = 1f;

	// Token: 0x0400155E RID: 5470
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryList<string, AIDirectorComponent> components = new DictionaryList<string, AIDirectorComponent>();

	// Token: 0x0400155F RID: 5471
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerManagementComponent playerManagementComponent;

	// Token: 0x04001560 RID: 5472
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorChunkEventComponent chunkEventComponent;

	// Token: 0x04001561 RID: 5473
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorBloodMoonComponent bloodMoonComponent;

	// Token: 0x04001562 RID: 5474
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> debugEntities = new List<Entity>();

	// Token: 0x04001563 RID: 5475
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDebugSendNameInfoTickRate = 5;

	// Token: 0x04001564 RID: 5476
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> debugSendNameInfoToPlayerIds = new List<int>();

	// Token: 0x04001565 RID: 5477
	[PublicizedFrom(EAccessModifier.Private)]
	public int debugNameInfoTicks;

	// Token: 0x04001566 RID: 5478
	public static bool debugFreezePos;

	// Token: 0x04001567 RID: 5479
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cLatencyName = "DebugLatency";

	// Token: 0x04001568 RID: 5480
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryStream latencyStream = new MemoryStream();

	// Token: 0x04001569 RID: 5481
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> debugSendLatencyToPlayerIds = new List<int>();

	// Token: 0x020003FF RID: 1023
	public enum HordeEvent
	{
		// Token: 0x0400156B RID: 5483
		None,
		// Token: 0x0400156C RID: 5484
		Warn1,
		// Token: 0x0400156D RID: 5485
		Warn2,
		// Token: 0x0400156E RID: 5486
		Spawn
	}
}
