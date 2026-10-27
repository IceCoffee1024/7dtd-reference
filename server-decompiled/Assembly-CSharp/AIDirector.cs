using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000406 RID: 1030
[Preserve]
public class AIDirector
{
	// Token: 0x06001FA0 RID: 8096 RVA: 0x000BFD91 File Offset: 0x000BDF91
	public AIDirector(World _world)
	{
		this.World = _world;
		this.CreateComponents();
		this.Init();
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x000BFDC2 File Offset: 0x000BDFC2
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		this.random = GameRandomManager.Instance.CreateGameRandom();
		this.ComponentsInitNewGame();
	}

	// Token: 0x06001FA2 RID: 8098 RVA: 0x000BFDDC File Offset: 0x000BDFDC
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

	// Token: 0x06001FA3 RID: 8099 RVA: 0x000BFE38 File Offset: 0x000BE038
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

	// Token: 0x06001FA4 RID: 8100 RVA: 0x000BFE98 File Offset: 0x000BE098
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsInitNewGame()
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].InitNewGame();
		}
	}

	// Token: 0x06001FA5 RID: 8101 RVA: 0x000BFED8 File Offset: 0x000BE0D8
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

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x000BFF1F File Offset: 0x000BE11F
	public AIDirectorBloodMoonComponent BloodMoonComponent
	{
		get
		{
			return this.bloodMoonComponent;
		}
	}

	// Token: 0x06001FA7 RID: 8103 RVA: 0x000BFF28 File Offset: 0x000BE128
	public void Load(PooledBinaryReader stream)
	{
		int num = stream.ReadInt32();
		this.ComponentsLoad(stream, num);
		if (this.World.worldTime == 0UL)
		{
			this.Init();
		}
	}

	// Token: 0x06001FA8 RID: 8104 RVA: 0x000BFF57 File Offset: 0x000BE157
	public void Save(PooledBinaryWriter stream)
	{
		stream.Write(10);
		this.ComponentsSave(stream);
	}

	// Token: 0x06001FA9 RID: 8105 RVA: 0x000BFF68 File Offset: 0x000BE168
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsLoad(PooledBinaryReader reader, int version)
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].Read(reader, version);
		}
	}

	// Token: 0x06001FAA RID: 8106 RVA: 0x000BFFA8 File Offset: 0x000BE1A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsSave(PooledBinaryWriter writer)
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].Write(writer);
		}
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x000BFFE7 File Offset: 0x000BE1E7
	public void Tick(double dt)
	{
		this.ComponentsTick(dt);
		this.DebugTick();
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x000BFFF8 File Offset: 0x000BE1F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsTick(double _dt)
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].Tick(_dt);
		}
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x000C0037 File Offset: 0x000BE237
	public static bool CanSpawn(float _priority = 1f)
	{
		return (float)GameStats.GetInt(EnumGameStats.EnemyCount) < (float)GamePrefs.GetInt(EnumGamePrefs.MaxSpawnedZombies) * _priority;
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x000C0050 File Offset: 0x000BE250
	public static ulong GetActivityWorldTimeDelay()
	{
		float num = (float)GameStats.GetInt(EnumGameStats.TimeOfDayIncPerSec) / 6f;
		num = Utils.FastClamp(num, 0.2f, 5f);
		return (ulong)(1000f * num);
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x000C0088 File Offset: 0x000BE288
	public void NotifyActivity(EnumAIDirectorChunkEvent type, Vector3i position, float value, float _duration = 720f)
	{
		if (value > 0f && GameStats.GetBool(EnumGameStats.ZombieHordeMeter) && GameStats.GetBool(EnumGameStats.IsSpawnEnemies) && AIDirector.HeatMapSensitivityModifier > 0f && !this.BloodMoonComponent.BloodMoonActive && !TwitchManager.BossHordeActive)
		{
			AIDirectorChunkEvent aidirectorChunkEvent = new AIDirectorChunkEvent(type, position, value * AIDirector.HeatMapSensitivityModifier, _duration);
			this.chunkEventComponent.NotifyEvent(aidirectorChunkEvent);
		}
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x000C00EC File Offset: 0x000BE2EC
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

	// Token: 0x06001FB1 RID: 8113 RVA: 0x000027FC File Offset: 0x000009FC
	public void NotifyIntentToAttack(EntityAlive zombie, EntityAlive player)
	{
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x000C01DC File Offset: 0x000BE3DC
	public void UpdatePlayerInventory(EntityPlayerLocal player)
	{
		this.playerManagementComponent.UpdatePlayerInventory(player);
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x000C01EA File Offset: 0x000BE3EA
	public void UpdatePlayerInventory(int entityId, AIDirectorPlayerInventory inventory)
	{
		this.playerManagementComponent.UpdatePlayerInventory(entityId, inventory);
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x000C01FC File Offset: 0x000BE3FC
	public void OnSoundPlayedAtPosition(int _entityThatCausedSound, Vector3 _position, string clipName, float volumeScale)
	{
		Entity entity = null;
		if (_entityThatCausedSound != -1)
		{
			entity = this.World.GetEntity(_entityThatCausedSound);
		}
		this.NotifyNoise(entity, _position, clipName, volumeScale);
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000C0228 File Offset: 0x000BE428
	public void AddEntity(Entity entity)
	{
		EntityPlayer entityPlayer;
		if (entityPlayer = entity as EntityPlayer)
		{
			this.AddPlayer(entityPlayer);
		}
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000C024C File Offset: 0x000BE44C
	public void RemoveEntity(Entity entity)
	{
		EntityPlayer entityPlayer;
		if (entityPlayer = entity as EntityPlayer)
		{
			this.RemovePlayer(entityPlayer);
		}
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000C026F File Offset: 0x000BE46F
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddPlayer(EntityPlayer player)
	{
		this.playerManagementComponent.AddPlayer(player);
		this.BloodMoonComponent.AddPlayer(player);
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000C0289 File Offset: 0x000BE489
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemovePlayer(EntityPlayer player)
	{
		this.playerManagementComponent.RemovePlayer(player);
		this.BloodMoonComponent.RemovePlayer(player);
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000C02A3 File Offset: 0x000BE4A3
	public static void LogAI(string _format, params object[] _args)
	{
		_format = string.Format("AIDirector: {0}", _format);
		Log.Out(_format, _args);
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x000C02B9 File Offset: 0x000BE4B9
	public static void LogAIExtra(string _format, params object[] _args)
	{
		if (AIDirectorConstants.DebugOutput)
		{
			AIDirector.LogAI(_format, _args);
		}
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x000C02C9 File Offset: 0x000BE4C9
	public void DebugFrameLateUpdate()
	{
		if (AIDirector.debugSendLatencyToPlayerIds.Count > 0)
		{
			this.DebugSendLatency();
		}
	}

	// Token: 0x06001FBC RID: 8124 RVA: 0x000C02DE File Offset: 0x000BE4DE
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugTick()
	{
		if (AIDirector.debugSendNameInfoToPlayerIds.Count > 0)
		{
			this.DebugSendNameInfo();
		}
	}

	// Token: 0x06001FBD RID: 8125 RVA: 0x000C02F4 File Offset: 0x000BE4F4
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

	// Token: 0x06001FBE RID: 8126 RVA: 0x000C0378 File Offset: 0x000BE578
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

	// Token: 0x06001FBF RID: 8127 RVA: 0x000C04C4 File Offset: 0x000BE6C4
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

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000C050F File Offset: 0x000BE70F
	public static void DebugToggleFreezePos()
	{
		AIDirector.debugFreezePos = !AIDirector.debugFreezePos;
		Log.Out("DebugToggleFreezePos {0}", new object[] { AIDirector.debugFreezePos });
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000C053C File Offset: 0x000BE73C
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

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000C05D8 File Offset: 0x000BE7D8
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

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000C0838 File Offset: 0x000BEA38
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

	// Token: 0x06001FC4 RID: 8132 RVA: 0x000C0A2C File Offset: 0x000BEC2C
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

	// Token: 0x04001575 RID: 5493
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivityDuration = 720f;

	// Token: 0x04001576 RID: 5494
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivityNoiseDuration = 240f;

	// Token: 0x04001577 RID: 5495
	public readonly World World;

	// Token: 0x04001578 RID: 5496
	public GameRandom random;

	// Token: 0x04001579 RID: 5497
	public static float HeatMapSensitivityModifier = 1f;

	// Token: 0x0400157A RID: 5498
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryList<string, AIDirectorComponent> components = new DictionaryList<string, AIDirectorComponent>();

	// Token: 0x0400157B RID: 5499
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerManagementComponent playerManagementComponent;

	// Token: 0x0400157C RID: 5500
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorChunkEventComponent chunkEventComponent;

	// Token: 0x0400157D RID: 5501
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorBloodMoonComponent bloodMoonComponent;

	// Token: 0x0400157E RID: 5502
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> debugEntities = new List<Entity>();

	// Token: 0x0400157F RID: 5503
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDebugSendNameInfoTickRate = 5;

	// Token: 0x04001580 RID: 5504
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> debugSendNameInfoToPlayerIds = new List<int>();

	// Token: 0x04001581 RID: 5505
	[PublicizedFrom(EAccessModifier.Private)]
	public int debugNameInfoTicks;

	// Token: 0x04001582 RID: 5506
	public static bool debugFreezePos;

	// Token: 0x04001583 RID: 5507
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cLatencyName = "DebugLatency";

	// Token: 0x04001584 RID: 5508
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryStream latencyStream = new MemoryStream();

	// Token: 0x04001585 RID: 5509
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> debugSendLatencyToPlayerIds = new List<int>();

	// Token: 0x02000407 RID: 1031
	public enum HordeEvent
	{
		// Token: 0x04001587 RID: 5511
		None,
		// Token: 0x04001588 RID: 5512
		Warn1,
		// Token: 0x04001589 RID: 5513
		Warn2,
		// Token: 0x0400158A RID: 5514
		Spawn
	}
}
