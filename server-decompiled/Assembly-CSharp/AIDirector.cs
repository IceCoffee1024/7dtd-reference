using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Twitch;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000402 RID: 1026
[Preserve]
public class AIDirector
{
	// Token: 0x06001FAC RID: 8108 RVA: 0x000BF931 File Offset: 0x000BDB31
	public AIDirector(World _world)
	{
		this.World = _world;
		this.CreateComponents();
		this.Init();
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x000BF962 File Offset: 0x000BDB62
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		this.random = GameRandomManager.Instance.CreateGameRandom();
		this.ComponentsInitNewGame();
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x000BF97C File Offset: 0x000BDB7C
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

	// Token: 0x06001FAF RID: 8111 RVA: 0x000BF9D8 File Offset: 0x000BDBD8
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

	// Token: 0x06001FB0 RID: 8112 RVA: 0x000BFA38 File Offset: 0x000BDC38
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsInitNewGame()
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].InitNewGame();
		}
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x000BFA78 File Offset: 0x000BDC78
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

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x000BFABF File Offset: 0x000BDCBF
	public AIDirectorBloodMoonComponent BloodMoonComponent
	{
		get
		{
			return this.bloodMoonComponent;
		}
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x000BFAC8 File Offset: 0x000BDCC8
	public void Load(BinaryReader stream)
	{
		int num = stream.ReadInt32();
		this.ComponentsLoad(stream, num);
		if (this.World.worldTime == 0UL)
		{
			this.Init();
		}
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x000BFAF7 File Offset: 0x000BDCF7
	public void Save(BinaryWriter stream)
	{
		stream.Write(10);
		this.ComponentsSave(stream);
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000BFB08 File Offset: 0x000BDD08
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsLoad(BinaryReader reader, int version)
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].Read(reader, version);
		}
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000BFB48 File Offset: 0x000BDD48
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsSave(BinaryWriter writer)
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].Write(writer);
		}
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000BFB87 File Offset: 0x000BDD87
	public void Tick(double dt)
	{
		this.ComponentsTick(dt);
		this.DebugTick();
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000BFB98 File Offset: 0x000BDD98
	[PublicizedFrom(EAccessModifier.Private)]
	public void ComponentsTick(double _dt)
	{
		for (int i = 0; i < this.components.list.Count; i++)
		{
			this.components.list[i].Tick(_dt);
		}
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000BFBD7 File Offset: 0x000BDDD7
	public static bool CanSpawn(float _priority = 1f)
	{
		return (float)GameStats.GetInt(EnumGameStats.EnemyCount) < (float)GamePrefs.GetInt(EnumGamePrefs.MaxSpawnedZombies) * _priority;
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x000BFBF0 File Offset: 0x000BDDF0
	public static ulong GetActivityWorldTimeDelay()
	{
		float num = (float)GameStats.GetInt(EnumGameStats.TimeOfDayIncPerSec) / 6f;
		num = Utils.FastClamp(num, 0.2f, 5f);
		return (ulong)(1000f * num);
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x000BFC28 File Offset: 0x000BDE28
	public void NotifyActivity(EnumAIDirectorChunkEvent type, Vector3i position, float value, float _duration = 720f)
	{
		if (value > 0f && GameStats.GetBool(EnumGameStats.ZombieHordeMeter) && GameStats.GetBool(EnumGameStats.IsSpawnEnemies) && AIDirector.HeatMapSensitivityModifier > 0f && !this.BloodMoonComponent.BloodMoonActive && !TwitchManager.BossHordeActive)
		{
			AIDirectorChunkEvent aidirectorChunkEvent = new AIDirectorChunkEvent(type, position, value * AIDirector.HeatMapSensitivityModifier, _duration);
			this.chunkEventComponent.NotifyEvent(aidirectorChunkEvent);
		}
	}

	// Token: 0x06001FBC RID: 8124 RVA: 0x000BFC8C File Offset: 0x000BDE8C
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

	// Token: 0x06001FBD RID: 8125 RVA: 0x000027FC File Offset: 0x000009FC
	public void NotifyIntentToAttack(EntityAlive zombie, EntityAlive player)
	{
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x000BFD7C File Offset: 0x000BDF7C
	public void UpdatePlayerInventory(EntityPlayerLocal player)
	{
		this.playerManagementComponent.UpdatePlayerInventory(player);
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x000BFD8A File Offset: 0x000BDF8A
	public void UpdatePlayerInventory(int entityId, AIDirectorPlayerInventory inventory)
	{
		this.playerManagementComponent.UpdatePlayerInventory(entityId, inventory);
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000BFD9C File Offset: 0x000BDF9C
	public void OnSoundPlayedAtPosition(int _entityThatCausedSound, Vector3 _position, string clipName, float volumeScale)
	{
		Entity entity = null;
		if (_entityThatCausedSound != -1)
		{
			entity = this.World.GetEntity(_entityThatCausedSound);
		}
		this.NotifyNoise(entity, _position, clipName, volumeScale);
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000BFDC8 File Offset: 0x000BDFC8
	public void AddEntity(Entity entity)
	{
		EntityPlayer entityPlayer;
		if (entityPlayer = entity as EntityPlayer)
		{
			this.AddPlayer(entityPlayer);
		}
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000BFDEC File Offset: 0x000BDFEC
	public void RemoveEntity(Entity entity)
	{
		EntityPlayer entityPlayer;
		if (entityPlayer = entity as EntityPlayer)
		{
			this.RemovePlayer(entityPlayer);
		}
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000BFE0F File Offset: 0x000BE00F
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddPlayer(EntityPlayer player)
	{
		this.playerManagementComponent.AddPlayer(player);
		this.BloodMoonComponent.AddPlayer(player);
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x000BFE29 File Offset: 0x000BE029
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemovePlayer(EntityPlayer player)
	{
		this.playerManagementComponent.RemovePlayer(player);
		this.BloodMoonComponent.RemovePlayer(player);
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x000BFE43 File Offset: 0x000BE043
	public static void LogAI(string _format, params object[] _args)
	{
		_format = string.Format("AIDirector: {0}", _format);
		Log.Out(_format, _args);
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000BFE59 File Offset: 0x000BE059
	public static void LogAIExtra(string _format, params object[] _args)
	{
		if (AIDirectorConstants.DebugOutput)
		{
			AIDirector.LogAI(_format, _args);
		}
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000BFE69 File Offset: 0x000BE069
	public void DebugFrameLateUpdate()
	{
		if (AIDirector.debugSendLatencyToPlayerIds.Count > 0)
		{
			this.DebugSendLatency();
		}
	}

	// Token: 0x06001FC8 RID: 8136 RVA: 0x000BFE7E File Offset: 0x000BE07E
	[PublicizedFrom(EAccessModifier.Private)]
	public void DebugTick()
	{
		if (AIDirector.debugSendNameInfoToPlayerIds.Count > 0)
		{
			this.DebugSendNameInfo();
		}
	}

	// Token: 0x06001FC9 RID: 8137 RVA: 0x000BFE94 File Offset: 0x000BE094
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

	// Token: 0x06001FCA RID: 8138 RVA: 0x000BFF18 File Offset: 0x000BE118
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

	// Token: 0x06001FCB RID: 8139 RVA: 0x000C0064 File Offset: 0x000BE264
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

	// Token: 0x06001FCC RID: 8140 RVA: 0x000C00AF File Offset: 0x000BE2AF
	public static void DebugToggleFreezePos()
	{
		AIDirector.debugFreezePos = !AIDirector.debugFreezePos;
		Log.Out("DebugToggleFreezePos {0}", new object[] { AIDirector.debugFreezePos });
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x000C00DC File Offset: 0x000BE2DC
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

	// Token: 0x06001FCE RID: 8142 RVA: 0x000C0178 File Offset: 0x000BE378
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

	// Token: 0x06001FCF RID: 8143 RVA: 0x000C03D8 File Offset: 0x000BE5D8
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

	// Token: 0x06001FD0 RID: 8144 RVA: 0x000C05CC File Offset: 0x000BE7CC
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

	// Token: 0x0400156E RID: 5486
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivityDuration = 720f;

	// Token: 0x0400156F RID: 5487
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cActivityNoiseDuration = 240f;

	// Token: 0x04001570 RID: 5488
	public readonly World World;

	// Token: 0x04001571 RID: 5489
	public GameRandom random;

	// Token: 0x04001572 RID: 5490
	public static float HeatMapSensitivityModifier = 1f;

	// Token: 0x04001573 RID: 5491
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionaryList<string, AIDirectorComponent> components = new DictionaryList<string, AIDirectorComponent>();

	// Token: 0x04001574 RID: 5492
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorPlayerManagementComponent playerManagementComponent;

	// Token: 0x04001575 RID: 5493
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorChunkEventComponent chunkEventComponent;

	// Token: 0x04001576 RID: 5494
	[PublicizedFrom(EAccessModifier.Private)]
	public AIDirectorBloodMoonComponent bloodMoonComponent;

	// Token: 0x04001577 RID: 5495
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> debugEntities = new List<Entity>();

	// Token: 0x04001578 RID: 5496
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cDebugSendNameInfoTickRate = 5;

	// Token: 0x04001579 RID: 5497
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> debugSendNameInfoToPlayerIds = new List<int>();

	// Token: 0x0400157A RID: 5498
	[PublicizedFrom(EAccessModifier.Private)]
	public int debugNameInfoTicks;

	// Token: 0x0400157B RID: 5499
	public static bool debugFreezePos;

	// Token: 0x0400157C RID: 5500
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cLatencyName = "DebugLatency";

	// Token: 0x0400157D RID: 5501
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryStream latencyStream = new MemoryStream();

	// Token: 0x0400157E RID: 5502
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> debugSendLatencyToPlayerIds = new List<int>();

	// Token: 0x02000403 RID: 1027
	public enum HordeEvent
	{
		// Token: 0x04001580 RID: 5504
		None,
		// Token: 0x04001581 RID: 5505
		Warn1,
		// Token: 0x04001582 RID: 5506
		Warn2,
		// Token: 0x04001583 RID: 5507
		Spawn
	}
}
