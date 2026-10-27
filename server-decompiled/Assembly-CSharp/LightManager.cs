using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

// Token: 0x0200127C RID: 4732
public class LightManager
{
	// Token: 0x06009623 RID: 38435 RVA: 0x0038BCCC File Offset: 0x00389ECC
	public static void Init()
	{
		for (int i = 0; i < 512; i++)
		{
			LightManager.regLightBins[i] = new Dictionary<Vector3i, LightManager.RegLightGroup>();
		}
	}

	// Token: 0x06009624 RID: 38436 RVA: 0x0038BCF8 File Offset: 0x00389EF8
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

	// Token: 0x06009625 RID: 38437 RVA: 0x0038BD48 File Offset: 0x00389F48
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

	// Token: 0x06009626 RID: 38438 RVA: 0x0038BD9C File Offset: 0x00389F9C
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

	// Token: 0x06009627 RID: 38439 RVA: 0x0038BDEA File Offset: 0x00389FEA
	public static void UnRegisterLight(Vector3 _worldPosition, float _lightRange)
	{
		if (_lightRange <= 0f)
		{
			return;
		}
		LightManager.RegisterLight(_worldPosition - Vector3.one * _lightRange, null, _lightRange, _worldPosition, LightManager.SearchDir.All, false);
	}

	// Token: 0x06009628 RID: 38440 RVA: 0x0038BE10 File Offset: 0x0038A010
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

	// Token: 0x06009629 RID: 38441 RVA: 0x0038BE98 File Offset: 0x0038A098
	public static void ShowSearchPattern(bool _bShow = true)
	{
		LightManager.ShowSearchPatternOn = _bShow;
		foreach (KeyValuePair<Vector3i, GameObject> keyValuePair in LightManager.debugPoints)
		{
			keyValuePair.Value.SetActive(LightManager.ShowSearchPatternOn);
		}
	}

	// Token: 0x0600962A RID: 38442 RVA: 0x0038BEFC File Offset: 0x0038A0FC
	public static void ShowLightLevel(bool _bShow = true)
	{
		LightManager.ShowLightLevelOn = _bShow;
	}

	// Token: 0x0600962B RID: 38443 RVA: 0x000027FC File Offset: 0x000009FC
	public static void UpdateUI()
	{
	}

	// Token: 0x0600962C RID: 38444 RVA: 0x0038BF04 File Offset: 0x0038A104
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

	// Token: 0x0600962D RID: 38445 RVA: 0x0038BFAC File Offset: 0x0038A1AC
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

	// Token: 0x0600962E RID: 38446 RVA: 0x0038C230 File Offset: 0x0038A430
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

	// Token: 0x0600962F RID: 38447 RVA: 0x0038C2B8 File Offset: 0x0038A4B8
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

	// Token: 0x06009630 RID: 38448 RVA: 0x0038C340 File Offset: 0x0038A540
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

	// Token: 0x06009631 RID: 38449 RVA: 0x0038C488 File Offset: 0x0038A688
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

	// Token: 0x06009632 RID: 38450 RVA: 0x0038C4D0 File Offset: 0x0038A6D0
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

	// Token: 0x06009633 RID: 38451 RVA: 0x0038C61D File Offset: 0x0038A81D
	public static void SetPlayerLightLevel(float _lightLevel)
	{
		LightManager.PlayerLightLevel = _lightLevel;
	}

	// Token: 0x06009634 RID: 38452 RVA: 0x0038C628 File Offset: 0x0038A828
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

	// Token: 0x06009635 RID: 38453 RVA: 0x0038C680 File Offset: 0x0038A880
	public static float GetWorldLightLevelInRange(Vector3 pos, float distanceMax)
	{
		float lightLevel = LightManager.GetLightLevel(pos);
		if (lightLevel >= 1f)
		{
			return 1f;
		}
		return lightLevel;
	}

	// Token: 0x06009636 RID: 38454 RVA: 0x0038C6A4 File Offset: 0x0038A8A4
	public static void LightChanged(Vector3 lightPos)
	{
		List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
		for (int i = localPlayers.Count - 1; i >= 0; i--)
		{
			localPlayers[i].renderManager.reflectionManager.LightChanged(lightPos);
		}
	}

	// Token: 0x06009637 RID: 38455 RVA: 0x0038C6EB File Offset: 0x0038A8EB
	public static void Dispose()
	{
		if (LightManager.myServer != null)
		{
			LightManager.myServer.Dispose();
			LightManager.myServer = null;
		}
	}

	// Token: 0x06009638 RID: 38456 RVA: 0x0038C704 File Offset: 0x0038A904
	public static void AttachLocalPlayer(EntityPlayerLocal localPlayer, World world)
	{
		if (LightManager.myServer != null)
		{
			LightManager.myServer.AttachLocalPlayer(localPlayer);
		}
	}

	// Token: 0x06009639 RID: 38457 RVA: 0x0038C718 File Offset: 0x0038A918
	public static void EntityAddedToWorld(Entity entity, World world)
	{
		if (LightManager.myServer != null)
		{
			LightManager.myServer.EntityAddedToWorld(entity, world);
		}
	}

	// Token: 0x0600963A RID: 38458 RVA: 0x0038C72D File Offset: 0x0038A92D
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

	// Token: 0x0600963B RID: 38459 RVA: 0x0038C74C File Offset: 0x0038A94C
	public static void CreateServer()
	{
		if (LightManager.myServer == null && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			LightManager.myServer = new LightManager.Server();
		}
	}

	// Token: 0x040070B1 RID: 28849
	[PublicizedFrom(EAccessModifier.Private)]
	public const int raycastMask = 65536;

	// Token: 0x040070B2 RID: 28850
	[PublicizedFrom(EAccessModifier.Private)]
	public const int enclosureRaycastMask = 8454417;

	// Token: 0x040070B3 RID: 28851
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRegGridSize = 8;

	// Token: 0x040070B4 RID: 28852
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRegPosMask = -8;

	// Token: 0x040070B5 RID: 28853
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRegBins = 512;

	// Token: 0x040070B6 RID: 28854
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cRegBinShift = 3;

	// Token: 0x040070B7 RID: 28855
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<Vector3i, LightManager.RegLightGroup>[] regLightBins = new Dictionary<Vector3i, LightManager.RegLightGroup>[512];

	// Token: 0x040070B8 RID: 28856
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryList<int, List<Light>> movingLights = new DictionaryList<int, List<Light>>();

	// Token: 0x040070B9 RID: 28857
	[PublicizedFrom(EAccessModifier.Private)]
	public static LightManager.Server myServer = null;

	// Token: 0x040070BA RID: 28858
	[PublicizedFrom(EAccessModifier.Private)]
	public static float PlayerLightLevel = 0f;

	// Token: 0x040070BB RID: 28859
	public static bool ShowSearchPatternOn = false;

	// Token: 0x040070BC RID: 28860
	public static bool ShowLightLevelOn = false;

	// Token: 0x040070BD RID: 28861
	[PublicizedFrom(EAccessModifier.Private)]
	public static Canvas debugUI = null;

	// Token: 0x040070BE RID: 28862
	[PublicizedFrom(EAccessModifier.Private)]
	public static Text debugText;

	// Token: 0x040070BF RID: 28863
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<Vector3i, GameObject> debugPoints = new Dictionary<Vector3i, GameObject>();

	// Token: 0x040070C0 RID: 28864
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject LightLevelDebugPoints = new GameObject("LightLevelDebugPoints");

	// Token: 0x040070C1 RID: 28865
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Vector3> lightsEffectingRemovals = new List<Vector3>();

	// Token: 0x0200127D RID: 4733
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SearchDir
	{
		// Token: 0x040070C3 RID: 28867
		All,
		// Token: 0x040070C4 RID: 28868
		Forward,
		// Token: 0x040070C5 RID: 28869
		Right
	}

	// Token: 0x0200127E RID: 4734
	[PublicizedFrom(EAccessModifier.Private)]
	public struct RegLightGroup
	{
		// Token: 0x040070C6 RID: 28870
		public Dictionary<Vector3, Light> lights;
	}

	// Token: 0x0200127F RID: 4735
	[Preserve]
	public class NetPackageLight : NetPackage
	{
		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x0600963E RID: 38462 RVA: 0x0002F260 File Offset: 0x0002D460
		public override NetPackageDirection PackageDirection
		{
			get
			{
				return NetPackageDirection.ToClient;
			}
		}

		// Token: 0x0600963F RID: 38463 RVA: 0x0038C7D7 File Offset: 0x0038A9D7
		public LightManager.NetPackageLight Setup(int _entityId, float _lightLevel = 0f)
		{
			this.entityId = _entityId;
			this.lightLevel = _lightLevel;
			return this;
		}

		// Token: 0x06009640 RID: 38464 RVA: 0x0038C7E8 File Offset: 0x0038A9E8
		public override void read(PooledBinaryReader _reader)
		{
			this.entityId = _reader.ReadInt32();
			this.lightLevel = _reader.ReadSingle();
		}

		// Token: 0x06009641 RID: 38465 RVA: 0x0038C802 File Offset: 0x0038AA02
		public override void write(PooledBinaryWriter _writer)
		{
			base.write(_writer);
			_writer.Write(this.entityId);
			_writer.Write(this.lightLevel);
		}

		// Token: 0x06009642 RID: 38466 RVA: 0x0038C824 File Offset: 0x0038AA24
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

		// Token: 0x040070C7 RID: 28871
		public int entityId;

		// Token: 0x040070C8 RID: 28872
		public float lightLevel;
	}

	// Token: 0x02001280 RID: 4736
	public class Server : IDisposable
	{
		// Token: 0x06009645 RID: 38469 RVA: 0x0038C8FC File Offset: 0x0038AAFC
		public void SendLightLevel(int entityId, float lightLevel)
		{
			LightManager.Client client;
			if (this.m_players.TryGetValue(entityId, out client))
			{
				client.SetPlayerLightLevel(lightLevel);
			}
		}

		// Token: 0x06009646 RID: 38470 RVA: 0x0038C920 File Offset: 0x0038AB20
		public void AttachLocalPlayer(EntityPlayerLocal localPlayer)
		{
			this.m_localPlayer = localPlayer;
		}

		// Token: 0x06009647 RID: 38471 RVA: 0x0038C92C File Offset: 0x0038AB2C
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

		// Token: 0x06009648 RID: 38472 RVA: 0x0038C998 File Offset: 0x0038AB98
		public void EntityRemovedFromWorld(Entity entity, World world)
		{
			LightManager.Client client;
			if (this.m_players.TryGetValue(entity.entityId, out client))
			{
				this.m_players.Remove(entity.entityId);
				client.Dispose();
			}
		}

		// Token: 0x06009649 RID: 38473 RVA: 0x0038C9D4 File Offset: 0x0038ABD4
		public void Dispose()
		{
			foreach (KeyValuePair<int, LightManager.Client> keyValuePair in this.m_players)
			{
				keyValuePair.Value.Dispose();
			}
			this.m_players = null;
			this.m_localPlayer = null;
		}

		// Token: 0x040070C9 RID: 28873
		[PublicizedFrom(EAccessModifier.Private)]
		public EntityPlayerLocal m_localPlayer;

		// Token: 0x040070CA RID: 28874
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<int, LightManager.Client> m_players = new Dictionary<int, LightManager.Client>();
	}

	// Token: 0x02001281 RID: 4737
	public class Client : IDisposable
	{
		// Token: 0x0600964A RID: 38474 RVA: 0x0038CA3C File Offset: 0x0038AC3C
		public Client(int _entityId)
		{
			this.entityId = _entityId;
		}

		// Token: 0x0600964B RID: 38475 RVA: 0x000027FC File Offset: 0x000009FC
		public void Dispose()
		{
		}

		// Token: 0x0600964C RID: 38476 RVA: 0x0038CA4C File Offset: 0x0038AC4C
		public void SetPlayerLightLevel(float _lightLevel)
		{
			LightManager.NetPackageLight netPackageLight = NetPackageManager.GetPackage<LightManager.NetPackageLight>().Setup(this.entityId, _lightLevel);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageLight, false, this.entityId, -1, -1, null, 192, false);
		}

		// Token: 0x040070CB RID: 28875
		public int entityId;
	}
}
