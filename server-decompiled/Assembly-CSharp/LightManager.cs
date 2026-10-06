using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

// Token: 0x0200126A RID: 4714
public class LightManager
{
	// Token: 0x060095CC RID: 38348 RVA: 0x003898D0 File Offset: 0x00387AD0
	public static void Init()
	{
		for (int i = 0; i < 512; i++)
		{
			LightManager.regLightBins[i] = new Dictionary<Vector3i, LightManager.RegLightGroup>();
		}
	}

	// Token: 0x060095CD RID: 38349 RVA: 0x003898FC File Offset: 0x00387AFC
	public static void RegisterMovingLight(Entity owner, Light _light)
	{
		int num = ((owner == null) ? (-1) : owner.entityId);
		List<Light> list;
		if (!LightManager.movingLights.dict.TryGetValue(num, out list))
		{
			list = new List<Light>();
			LightManager.movingLights.Add(num, list);
		}
		list.Add(_light);
	}

	// Token: 0x060095CE RID: 38350 RVA: 0x0038994C File Offset: 0x00387B4C
	public static void UnRegisterMovingLight(Entity owner, Light _light)
	{
		int num = ((owner == null) ? (-1) : owner.entityId);
		List<Light> list;
		if (LightManager.movingLights.dict.TryGetValue(num, out list))
		{
			list.Remove(_light);
			if (list.Count < 1)
			{
				LightManager.movingLights.Remove(num);
			}
		}
	}

	// Token: 0x060095CF RID: 38351 RVA: 0x003899A0 File Offset: 0x00387BA0
	public static float RegisterLight(Light _light)
	{
		float range = _light.range;
		if (range > 0f)
		{
			Vector3 vector = _light.transform.position + Origin.position;
			LightManager.RegisterLight(vector - Vector3.one * range, _light, range, vector, LightManager.SearchDir.All, true);
		}
		return range;
	}

	// Token: 0x060095D0 RID: 38352 RVA: 0x003899EE File Offset: 0x00387BEE
	public static void UnRegisterLight(Vector3 _worldPosition, float _lightRange)
	{
		if (_lightRange <= 0f)
		{
			return;
		}
		LightManager.RegisterLight(_worldPosition - Vector3.one * _lightRange, null, _lightRange, _worldPosition, LightManager.SearchDir.All, false);
	}

	// Token: 0x060095D1 RID: 38353 RVA: 0x00389A14 File Offset: 0x00387C14
	public static void Clear()
	{
		for (int i = 0; i < 512; i++)
		{
			LightManager.regLightBins[i] = new Dictionary<Vector3i, LightManager.RegLightGroup>();
		}
		LightManager.movingLights.Clear();
		foreach (KeyValuePair<Vector3i, GameObject> keyValuePair in LightManager.debugPoints)
		{
			global::UnityEngine.Object.Destroy(keyValuePair.Value);
		}
		LightManager.debugPoints.Clear();
	}

	// Token: 0x060095D2 RID: 38354 RVA: 0x00389A9C File Offset: 0x00387C9C
	public static void ShowSearchPattern(bool _bShow = true)
	{
		LightManager.ShowSearchPatternOn = _bShow;
		foreach (KeyValuePair<Vector3i, GameObject> keyValuePair in LightManager.debugPoints)
		{
			keyValuePair.Value.SetActive(LightManager.ShowSearchPatternOn);
		}
	}

	// Token: 0x060095D3 RID: 38355 RVA: 0x00389B00 File Offset: 0x00387D00
	public static void ShowLightLevel(bool _bShow = true)
	{
		LightManager.ShowLightLevelOn = _bShow;
	}

	// Token: 0x060095D4 RID: 38356 RVA: 0x000027FC File Offset: 0x000009FC
	public static void UpdateUI()
	{
	}

	// Token: 0x060095D5 RID: 38357 RVA: 0x00389B08 File Offset: 0x00387D08
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetLightLevelFromMovingLights(int _excludeEntityId, Vector3 _worldPosition)
	{
		float num = 0f;
		foreach (KeyValuePair<int, List<Light>> keyValuePair in LightManager.movingLights.dict)
		{
			if (keyValuePair.Key != _excludeEntityId)
			{
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					num += LightManager.GetLightLevel(keyValuePair.Value[i], _worldPosition, true);
					if (num >= 1f)
					{
						return 1f;
					}
				}
			}
		}
		return Utils.FastClamp01(num);
	}

	// Token: 0x060095D6 RID: 38358 RVA: 0x00389BB0 File Offset: 0x00387DB0
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetLightLevel(Light _light, Vector3 _worldPosition, bool _bAllowPointToSpot = false)
	{
		if (_light == null || !_light.isActiveAndEnabled)
		{
			return 0f;
		}
		Vector3 vector = _light.transform.position + Origin.position;
		Vector3 vector2 = _worldPosition - vector;
		float range = _light.range;
		float magnitude = vector2.magnitude;
		if (magnitude >= range)
		{
			return 0f;
		}
		Vector3 vector3 = vector;
		vector3.x = Mathf.Floor(vector3.x) + 0.02f;
		vector3.y = Mathf.Floor(vector3.y) + 0.02f;
		vector3.z = Mathf.Floor(vector3.z) + 0.02f;
		if (_worldPosition.x > vector3.x)
		{
			vector3.x += 0.96f;
		}
		if (_worldPosition.y > vector3.y)
		{
			vector3.y += 0.96f;
		}
		if (_worldPosition.z > vector3.z)
		{
			vector3.z += 0.96f;
		}
		Vector3 vector4 = _worldPosition - vector3;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(vector3 - Origin.position, vector4), out raycastHit, range + 0.25f, 65536) && raycastHit.distance < vector4.magnitude - 0.25f)
		{
			return 0f;
		}
		LightType lightType = _light.type;
		float num = _light.spotAngle;
		if (_bAllowPointToSpot && _light.cookie)
		{
			float cookieSize = _light.cookieSize;
			float num2 = Mathf.Sqrt(Mathf.Pow(cookieSize * 0.5f, 2f) + Mathf.Pow(range, 2f));
			num = Mathf.Acos(cookieSize * 0.5f / num2) * 57.295776f;
			lightType = LightType.Spot;
		}
		Color color = _light.color;
		if (lightType != LightType.Spot)
		{
			return (1f - magnitude / range) * Utils.FastClamp01(_light.intensity) * SkyManager.GetLuma(color) * color.a;
		}
		float num3 = Vector3.Dot(_light.transform.forward, vector2.normalized);
		float num4 = 1f - num / 180f;
		num4 = Utils.FastClamp01(num4 * 1.1f);
		if (num3 < num4)
		{
			return 0f;
		}
		float num5 = Utils.FastClamp01(num3);
		if (num4 < 1f)
		{
			num5 = (num3 - num4) / (1f - num4);
		}
		num5 = Mathf.Pow(num5, 1.25f);
		return (1f - magnitude / range) * Utils.FastClamp01(_light.intensity) * SkyManager.GetLuma(color) * num5;
	}

	// Token: 0x060095D7 RID: 38359 RVA: 0x00389E34 File Offset: 0x00388034
	[PublicizedFrom(EAccessModifier.Private)]
	public static float BlockLight(Vector3 _worldPosition)
	{
		float num = 1f;
		Vector3i vector3i = World.worldToBlockPos(_worldPosition);
		IChunk chunkFromWorldPos = GameManager.Instance.World.GetChunkFromWorldPos(vector3i);
		if (chunkFromWorldPos != null && vector3i.y >= 0 && vector3i.y < 255)
		{
			num = (float)Utils.FastMax(chunkFromWorldPos.GetLight(vector3i.x, vector3i.y, vector3i.z, Chunk.LIGHT_TYPE.SUN), chunkFromWorldPos.GetLight(vector3i.x, vector3i.y + 1, vector3i.z, Chunk.LIGHT_TYPE.SUN));
			num /= 15f;
		}
		return num;
	}

	// Token: 0x060095D8 RID: 38360 RVA: 0x00389EBC File Offset: 0x003880BC
	[PublicizedFrom(EAccessModifier.Private)]
	public static float CalcShadeLight(Vector3 _worldPosition)
	{
		bool flag = false;
		Vector3 vector;
		if (SkyManager.GetSunIntensity() < 0.05f)
		{
			vector = SkyManager.GetMoonDirection();
			if (vector.y > -0.087f)
			{
				return 0f;
			}
			flag = true;
		}
		else
		{
			vector = SkyManager.GetSunLightDirection();
		}
		float num = (float)(flag ? (-1) : 1);
		Vector3 vector2 = _worldPosition - Origin.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(vector2, -vector), out raycastHit, float.PositiveInfinity, 8454417) && raycastHit.distance < float.PositiveInfinity)
		{
			num = 0f;
		}
		return num;
	}

	// Token: 0x060095D9 RID: 38361 RVA: 0x00389F44 File Offset: 0x00388144
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetLightLevel(Vector3 _worldPosition)
	{
		float num = 0f;
		Dictionary<Vector3, Light> lightsEffecting = LightManager.GetLightsEffecting(_worldPosition);
		if (lightsEffecting != null && lightsEffecting.Count > 0)
		{
			LightManager.lightsEffectingRemovals.Clear();
			foreach (KeyValuePair<Vector3, Light> keyValuePair in lightsEffecting)
			{
				Light value = keyValuePair.Value;
				if (value == null)
				{
					LightManager.lightsEffectingRemovals.Add(keyValuePair.Key);
				}
				else
				{
					num += LightManager.GetLightLevel(value, _worldPosition, false);
					if (num >= 1f)
					{
						break;
					}
				}
			}
			for (int i = LightManager.lightsEffectingRemovals.Count - 1; i >= 0; i--)
			{
				lightsEffecting.Remove(LightManager.lightsEffectingRemovals[i]);
			}
		}
		if (num >= 1f)
		{
			return 1f;
		}
		float num2 = Mathf.Pow(WorldEnvironment.AmbientTotal, 0.6f);
		num += num2 * 0.5f;
		num += LightManager.BlockLight(_worldPosition) * num2 * 0.5f;
		if (num >= 1f)
		{
			return 1f;
		}
		float num3 = LightManager.CalcShadeLight(_worldPosition);
		if (num3 >= 0f)
		{
			num += num3;
		}
		else
		{
			num += SkyManager.GetMoonBrightness();
		}
		return Utils.FastClamp01(num);
	}

	// Token: 0x060095DA RID: 38362 RVA: 0x0038A08C File Offset: 0x0038828C
	public static Dictionary<Vector3, Light> GetLightsEffecting(Vector3 _worldPosition)
	{
		Dictionary<Vector3i, LightManager.RegLightGroup> dictionary = LightManager.regLightBins[(Utils.Fastfloor(_worldPosition.x) >> 3) & 511];
		Vector3i vector3i = World.worldToBlockPos(_worldPosition);
		vector3i &= -8;
		LightManager.RegLightGroup regLightGroup;
		if (!dictionary.TryGetValue(vector3i, out regLightGroup))
		{
			return null;
		}
		return regLightGroup.lights;
	}

	// Token: 0x060095DB RID: 38363 RVA: 0x0038A0D4 File Offset: 0x003882D4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void RegisterLight(Vector3 _searchPosition, Light _light, float _lightRange, Vector3 _lightPosition, LightManager.SearchDir _, bool _bRegister)
	{
		int num = Mathf.CeilToInt(_lightRange * 0.89f);
		if (num <= 0)
		{
			return;
		}
		int num2 = num * 2;
		Vector3i vector3i = World.worldToBlockPos(_searchPosition);
		Vector3i vector3i2 = vector3i + new Vector3i(num2, num2, num2);
		Vector3i vector3i3 = vector3i & -8;
		Vector3i vector3i4;
		vector3i4.y = vector3i3.y;
		while (vector3i4.y <= vector3i2.y)
		{
			vector3i4.z = vector3i3.z;
			while (vector3i4.z <= vector3i2.z)
			{
				vector3i4.x = vector3i3.x;
				while (vector3i4.x <= vector3i2.x)
				{
					Dictionary<Vector3i, LightManager.RegLightGroup> dictionary = LightManager.regLightBins[(vector3i4.x >> 3) & 511];
					LightManager.RegLightGroup regLightGroup;
					if (!dictionary.TryGetValue(vector3i4, out regLightGroup))
					{
						regLightGroup.lights = new Dictionary<Vector3, Light>();
						dictionary.Add(vector3i4, regLightGroup);
					}
					if (_bRegister)
					{
						regLightGroup.lights[_lightPosition] = _light;
					}
					else
					{
						regLightGroup.lights.Remove(_lightPosition);
						if (regLightGroup.lights.Count == 0)
						{
							dictionary.Remove(vector3i4);
							regLightGroup.lights = null;
						}
					}
					vector3i4.x += 8;
				}
				vector3i4.z += 8;
			}
			vector3i4.y += 8;
		}
	}

	// Token: 0x060095DC RID: 38364 RVA: 0x0038A221 File Offset: 0x00388421
	public static void SetPlayerLightLevel(float _lightLevel)
	{
		LightManager.PlayerLightLevel = _lightLevel;
	}

	// Token: 0x060095DD RID: 38365 RVA: 0x0038A22C File Offset: 0x0038842C
	public static float GetStealthLightLevel(EntityAlive _entity, out float selfLight)
	{
		if (LightManager.myServer != null)
		{
			Vector3 position = _entity.position;
			position.y += 1.68f;
			float num = LightManager.GetLightLevel(position) + LightManager.GetLightLevelFromMovingLights(_entity.entityId, position);
			selfLight = _entity.GetLightLevel();
			return Utils.FastClamp01(num);
		}
		selfLight = 0f;
		return 0f;
	}

	// Token: 0x060095DE RID: 38366 RVA: 0x0038A284 File Offset: 0x00388484
	public static float GetWorldLightLevelInRange(Vector3 pos, float distanceMax)
	{
		float lightLevel = LightManager.GetLightLevel(pos);
		if (lightLevel >= 1f)
		{
			return 1f;
		}
		return lightLevel;
	}

	// Token: 0x060095DF RID: 38367 RVA: 0x0038A2A8 File Offset: 0x003884A8
	public static void LightChanged(Vector3 lightPos)
	{
		List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
		for (int i = localPlayers.Count - 1; i >= 0; i--)
		{
			localPlayers[i].renderManager.reflectionManager.LightChanged(lightPos);
		}
	}

	// Token: 0x060095E0 RID: 38368 RVA: 0x0038A2EF File Offset: 0x003884EF
	public static void Dispose()
	{
		if (LightManager.myServer != null)
		{
			LightManager.myServer.Dispose();
			LightManager.myServer = null;
		}
	}

	// Token: 0x060095E1 RID: 38369 RVA: 0x0038A308 File Offset: 0x00388508
	public static void AttachLocalPlayer(EntityPlayerLocal localPlayer, World world)
	{
		if (LightManager.myServer != null)
		{
			LightManager.myServer.AttachLocalPlayer(localPlayer);
		}
	}

	// Token: 0x060095E2 RID: 38370 RVA: 0x0038A31C File Offset: 0x0038851C
	public static void EntityAddedToWorld(Entity entity, World world)
	{
		if (LightManager.myServer != null)
		{
			LightManager.myServer.EntityAddedToWorld(entity, world);
		}
	}

	// Token: 0x060095E3 RID: 38371 RVA: 0x0038A331 File Offset: 0x00388531
	public static void EntityRemovedFromWorld(Entity entity, World world)
	{
		if (entity == null)
		{
			return;
		}
		if (LightManager.myServer != null)
		{
			LightManager.myServer.EntityRemovedFromWorld(entity, world);
		}
	}

	// Token: 0x060095E4 RID: 38372 RVA: 0x0038A350 File Offset: 0x00388550
	public static void CreateServer()
	{
		if (LightManager.myServer == null && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			LightManager.myServer = new LightManager.Server();
		}
	}

	// Token: 0x04007070 RID: 28784
	[PublicizedFrom(EAccessModifier.Private)]
	public const int raycastMask = 65536;

	// Token: 0x04007071 RID: 28785
	[PublicizedFrom(EAccessModifier.Private)]
	public const int enclosureRaycastMask = 8454417;

	// Token: 0x04007072 RID: 28786
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRegGridSize = 8;

	// Token: 0x04007073 RID: 28787
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRegPosMask = -8;

	// Token: 0x04007074 RID: 28788
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRegBins = 512;

	// Token: 0x04007075 RID: 28789
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRegBinShift = 3;

	// Token: 0x04007076 RID: 28790
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<Vector3i, LightManager.RegLightGroup>[] regLightBins = new Dictionary<Vector3i, LightManager.RegLightGroup>[512];

	// Token: 0x04007077 RID: 28791
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryList<int, List<Light>> movingLights = new DictionaryList<int, List<Light>>();

	// Token: 0x04007078 RID: 28792
	[PublicizedFrom(EAccessModifier.Private)]
	public static LightManager.Server myServer = null;

	// Token: 0x04007079 RID: 28793
	[PublicizedFrom(EAccessModifier.Private)]
	public static float PlayerLightLevel = 0f;

	// Token: 0x0400707A RID: 28794
	public static bool ShowSearchPatternOn = false;

	// Token: 0x0400707B RID: 28795
	public static bool ShowLightLevelOn = false;

	// Token: 0x0400707C RID: 28796
	[PublicizedFrom(EAccessModifier.Private)]
	public static Canvas debugUI = null;

	// Token: 0x0400707D RID: 28797
	[PublicizedFrom(EAccessModifier.Private)]
	public static Text debugText;

	// Token: 0x0400707E RID: 28798
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<Vector3i, GameObject> debugPoints = new Dictionary<Vector3i, GameObject>();

	// Token: 0x0400707F RID: 28799
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject LightLevelDebugPoints = new GameObject("LightLevelDebugPoints");

	// Token: 0x04007080 RID: 28800
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Vector3> lightsEffectingRemovals = new List<Vector3>();

	// Token: 0x0200126B RID: 4715
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SearchDir
	{
		// Token: 0x04007082 RID: 28802
		All,
		// Token: 0x04007083 RID: 28803
		Forward,
		// Token: 0x04007084 RID: 28804
		Right
	}

	// Token: 0x0200126C RID: 4716
	[PublicizedFrom(EAccessModifier.Private)]
	public struct RegLightGroup
	{
		// Token: 0x04007085 RID: 28805
		public Dictionary<Vector3, Light> lights;
	}

	// Token: 0x0200126D RID: 4717
	[Preserve]
	public class NetPackageLight : NetPackage
	{
		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x060095E7 RID: 38375 RVA: 0x0002F260 File Offset: 0x0002D460
		public override NetPackageDirection PackageDirection
		{
			get
			{
				return NetPackageDirection.ToClient;
			}
		}

		// Token: 0x060095E8 RID: 38376 RVA: 0x0038A3DB File Offset: 0x003885DB
		public LightManager.NetPackageLight Setup(int _entityId, float _lightLevel = 0f)
		{
			this.entityId = _entityId;
			this.lightLevel = _lightLevel;
			return this;
		}

		// Token: 0x060095E9 RID: 38377 RVA: 0x0038A3EC File Offset: 0x003885EC
		public override void read(PooledBinaryReader _reader)
		{
			this.entityId = _reader.ReadInt32();
			this.lightLevel = _reader.ReadSingle();
		}

		// Token: 0x060095EA RID: 38378 RVA: 0x0038A406 File Offset: 0x00388606
		public override void write(PooledBinaryWriter _writer)
		{
			base.write(_writer);
			_writer.Write(this.entityId);
			_writer.Write(this.lightLevel);
		}

		// Token: 0x060095EB RID: 38379 RVA: 0x0038A428 File Offset: 0x00388628
		public override void ProcessPackage(World _world, GameManager _callbacks)
		{
			if (GameManager.Instance == null || GameManager.Instance.World == null)
			{
				return;
			}
			if (LightManager.myServer == null)
			{
				LightManager.SetPlayerLightLevel(this.lightLevel);
				return;
			}
			Entity entity = GameManager.Instance.World.GetEntity(this.entityId);
			if (entity == null)
			{
				return;
			}
			float num = LightManager.GetLightLevel(entity.position + Vector3.up * 1.68f) + LightManager.GetLightLevelFromMovingLights(this.entityId, entity.position + Vector3.up * 1.68f);
			num += ((EntityAlive)entity).GetLightLevel();
			LightManager.myServer.SendLightLevel(this.entityId, num);
		}

		// Token: 0x04007086 RID: 28806
		public int entityId;

		// Token: 0x04007087 RID: 28807
		public float lightLevel;
	}

	// Token: 0x0200126E RID: 4718
	public class Server : IDisposable
	{
		// Token: 0x060095EE RID: 38382 RVA: 0x0038A500 File Offset: 0x00388700
		public void SendLightLevel(int entityId, float lightLevel)
		{
			LightManager.Client client;
			if (this.m_players.TryGetValue(entityId, out client))
			{
				client.SetPlayerLightLevel(lightLevel);
			}
		}

		// Token: 0x060095EF RID: 38383 RVA: 0x0038A524 File Offset: 0x00388724
		public void AttachLocalPlayer(EntityPlayerLocal localPlayer)
		{
			this.m_localPlayer = localPlayer;
		}

		// Token: 0x060095F0 RID: 38384 RVA: 0x0038A530 File Offset: 0x00388730
		public void EntityAddedToWorld(Entity entity, World world)
		{
			if (entity is EntityPlayer && (this.m_localPlayer == null || entity.entityId != this.m_localPlayer.entityId))
			{
				LightManager.Client client;
				if (this.m_players.TryGetValue(entity.entityId, out client))
				{
					return;
				}
				client = new LightManager.Client(entity.entityId);
				this.m_players[entity.entityId] = client;
			}
		}

		// Token: 0x060095F1 RID: 38385 RVA: 0x0038A59C File Offset: 0x0038879C
		public void EntityRemovedFromWorld(Entity entity, World world)
		{
			LightManager.Client client;
			if (this.m_players.TryGetValue(entity.entityId, out client))
			{
				this.m_players.Remove(entity.entityId);
				client.Dispose();
			}
		}

		// Token: 0x060095F2 RID: 38386 RVA: 0x0038A5D8 File Offset: 0x003887D8
		public void Dispose()
		{
			foreach (KeyValuePair<int, LightManager.Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Dispose();
			}
			this.m_players = null;
			this.m_localPlayer = null;
		}

		// Token: 0x04007088 RID: 28808
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal m_localPlayer;

		// Token: 0x04007089 RID: 28809
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, LightManager.Client> m_players = new Dictionary<int, LightManager.Client>();
	}

	// Token: 0x0200126F RID: 4719
	public class Client : IDisposable
	{
		// Token: 0x060095F3 RID: 38387 RVA: 0x0038A640 File Offset: 0x00388840
		public Client(int _entityId)
		{
			this.entityId = _entityId;
		}

		// Token: 0x060095F4 RID: 38388 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x060095F5 RID: 38389 RVA: 0x0038A650 File Offset: 0x00388850
		public void SetPlayerLightLevel(float _lightLevel)
		{
			LightManager.NetPackageLight netPackageLight = NetPackageManager.GetPackage<LightManager.NetPackageLight>().Setup(this.entityId, _lightLevel);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageLight, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x0400708A RID: 28810
		public int entityId;
	}
}
