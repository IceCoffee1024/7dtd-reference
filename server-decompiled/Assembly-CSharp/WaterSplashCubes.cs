using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000198 RID: 408
public class WaterSplashCubes
{
	// Token: 0x06000C63 RID: 3171 RVA: 0x0004D730 File Offset: 0x0004B930
	public WaterSplashCubes()
	{
		if (WaterSplashCubes.instance == null)
		{
			WaterSplashCubes.instance = this;
		}
		WaterSplashCubes.addList = new List<WaterSplashCubes.ParticlePlacement>();
		WaterSplashCubes.removeList = new List<Vector3i>();
		WaterSplashCubes.root = GameObject.Find("WaterSplashes");
		WaterSplashCubes.particleLimiter = GamePrefs.GetFloat(EnumGamePrefs.OptionsGfxWaterPtlLimiter);
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x0004D782 File Offset: 0x0004B982
	public static long MakeKey(int x, int z)
	{
		return (((long)z & 16777215L) << 24) | ((long)x & 16777215L);
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x0004D79A File Offset: 0x0004B99A
	public static object GetSyncRoot()
	{
		return ((ICollection)WaterSplashCubes.splashes.dict).SyncRoot;
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x0004D7AC File Offset: 0x0004B9AC
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject AddParticleEffect(Vector3i pos, BlockFace face, WaterSplashCubes.SplashType type)
	{
		if (WaterSplashCubes.waterFallSplashCubeEffect == null)
		{
			WaterSplashCubes.waterFallSplashCubeEffect = new global::UnityEngine.Object[EnumUtils.Names<WaterSplashCubes.SplashType>().Count];
			WaterSplashCubes.waterFallSplashCubeEffect[0] = Resources.Load("prefabs/WaterFallSplashCube");
			WaterSplashCubes.waterFallSplashCubeEffect[1] = Resources.Load("prefabs/WaterFallSlopeParticles");
			WaterSplashCubes.waterFallSplashCubeEffect[2] = Resources.Load("prefabs/WaterFallAreaParticles");
			WaterSplashCubes.waterFallSplashCubeEffect[3] = Resources.Load("prefabs/WaterFallSplashCube");
		}
		GameObject gameObject = (GameObject)global::UnityEngine.Object.Instantiate(WaterSplashCubes.waterFallSplashCubeEffect[(int)type]);
		WaterSplashCubes.particleCount++;
		switch (face)
		{
		case BlockFace.Top:
			gameObject.transform.Rotate(new Vector3(90f, 0f, 0f));
			break;
		case BlockFace.Bottom:
			gameObject.transform.Rotate(new Vector3(-90f, 0f, 0f));
			break;
		case BlockFace.North:
			gameObject.transform.Rotate(new Vector3(0f, 0f, 0f));
			break;
		case BlockFace.West:
			gameObject.transform.Rotate(new Vector3(0f, -90f, 0f));
			break;
		case BlockFace.South:
			gameObject.transform.Rotate(new Vector3(0f, 180f, 0f));
			break;
		case BlockFace.East:
			gameObject.transform.Rotate(new Vector3(0f, 90f, 0f));
			break;
		}
		gameObject.transform.position = new Vector3((float)pos.x + 0.5f, (float)pos.y + 0.5f, (float)pos.z + 0.5f) - Origin.position;
		gameObject.transform.parent = WaterSplashCubes.root.transform;
		if (WaterSplashCubes.particleLimiter < 1f && WaterSplashCubes.particleCount % Mathf.CeilToInt((1f - WaterSplashCubes.particleLimiter) * 6f) != 0)
		{
			gameObject.SetActive(false);
		}
		return gameObject;
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x0004D9AC File Offset: 0x0004BBAC
	public static void Update()
	{
		if (WaterSplashCubes.removeList == null)
		{
			return;
		}
		if (WaterSplashCubes.addList == null)
		{
			return;
		}
		List<Vector3i> list = WaterSplashCubes.removeList;
		lock (list)
		{
			for (int i = 0; i < WaterSplashCubes.removeList.Count; i++)
			{
				Vector3i vector3i = WaterSplashCubes.removeList[i];
				int x = vector3i.x;
				int y = vector3i.y;
				int z = vector3i.z;
				long num = WaterSplashCubes.MakeKey(x, z);
				object obj = WaterSplashCubes.GetSyncRoot();
				lock (obj)
				{
					if (WaterSplashCubes.splashes.dict.ContainsKey(num))
					{
						DictionaryList<int, GameObject> dictionaryList = WaterSplashCubes.splashes.dict[num];
						if (dictionaryList.dict.ContainsKey(y))
						{
							global::UnityEngine.Object.DestroyImmediate(dictionaryList.dict[y]);
							dictionaryList.Remove(y);
						}
					}
				}
			}
			WaterSplashCubes.removeList.Clear();
		}
		List<WaterSplashCubes.ParticlePlacement> list2 = WaterSplashCubes.addList;
		lock (list2)
		{
			for (int j = 0; j < WaterSplashCubes.addList.Count; j++)
			{
				WaterSplashCubes.ParticlePlacement particlePlacement = WaterSplashCubes.addList[j];
				int x2 = particlePlacement.pos.x;
				int y2 = particlePlacement.pos.y;
				int z2 = particlePlacement.pos.z;
				long num2 = WaterSplashCubes.MakeKey(x2, z2);
				object obj = WaterSplashCubes.GetSyncRoot();
				lock (obj)
				{
					if (!WaterSplashCubes.splashes.dict.ContainsKey(num2))
					{
						DictionaryList<int, GameObject> dictionaryList2 = new DictionaryList<int, GameObject>();
						dictionaryList2.Add(y2, WaterSplashCubes.AddParticleEffect(particlePlacement.pos, particlePlacement.dir, particlePlacement.type));
						WaterSplashCubes.splashes.Add(num2, dictionaryList2);
					}
					else
					{
						DictionaryList<int, GameObject> dictionaryList3 = WaterSplashCubes.splashes.dict[num2];
						if (!dictionaryList3.dict.ContainsKey(y2))
						{
							dictionaryList3.Add(y2, WaterSplashCubes.AddParticleEffect(particlePlacement.pos, particlePlacement.dir, particlePlacement.type));
						}
					}
				}
			}
			WaterSplashCubes.addList.Clear();
		}
		WaterSplashCubes.CleanUp();
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x0004DC50 File Offset: 0x0004BE50
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanUp()
	{
		if (Time.time > WaterSplashCubes.cleanUpTimer + 0.15f)
		{
			WaterSplashCubes.cleanUpTimer = Time.time;
			if (WaterSplashCubes.root == null)
			{
				return;
			}
			if (WaterSplashCubes.root.transform == null)
			{
				return;
			}
			if (GameManager.Instance == null)
			{
				return;
			}
			World world = GameManager.Instance.World;
			if (world == null)
			{
				return;
			}
			bool flag = false;
			if (WaterSplashCubes.currentCleanupIndex >= WaterSplashCubes.root.transform.childCount)
			{
				WaterSplashCubes.currentCleanupIndex = WaterSplashCubes.root.transform.childCount;
				flag = true;
			}
			int num = 0;
			while (num < WaterSplashCubes.root.transform.childCount && num < WaterSplashCubes.currentCleanupIndex)
			{
				Transform child = WaterSplashCubes.root.transform.GetChild(num);
				if (child != null)
				{
					Vector3i vector3i = new Vector3i((int)(child.position.x - 0.5f), (int)(child.position.y - 0.5f), (int)(child.position.z - 0.5f));
					if (!world.IsChunkAreaLoaded(vector3i.x, vector3i.y, vector3i.z))
					{
						WaterSplashCubes.RemoveSplashAt(vector3i.x, vector3i.y, vector3i.z);
					}
					else if (!world.IsWater(vector3i) || !world.IsAir((int)(child.position.x - 0.5f), (int)(child.position.y - 0.5f) + 1, (int)(child.position.z - 0.5f)))
					{
						WaterSplashCubes.RemoveSplashAt(vector3i.x, vector3i.y, vector3i.z);
					}
				}
				num++;
			}
			WaterSplashCubes.currentCleanupIndex += 15;
			if (flag)
			{
				WaterSplashCubes.currentCleanupIndex = 0;
			}
		}
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x0004DE20 File Offset: 0x0004C020
	public static void RemoveSplashAt(int _x, int _y, int _z)
	{
		if (WaterSplashCubes.removeList == null)
		{
			return;
		}
		List<Vector3i> list = WaterSplashCubes.removeList;
		lock (list)
		{
			WaterSplashCubes.removeList.Add(new Vector3i(_x, _y, _z));
		}
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x0004DE74 File Offset: 0x0004C074
	public static void AddSplashAt(int _x, int _y, int _z, BlockFace _dir, WaterSplashCubes.SplashType _type)
	{
		if (WaterSplashCubes.particleLimiter <= 0f)
		{
			return;
		}
		if (WaterSplashCubes.addList == null)
		{
			return;
		}
		List<WaterSplashCubes.ParticlePlacement> list = WaterSplashCubes.addList;
		lock (list)
		{
			WaterSplashCubes.ParticlePlacement particlePlacement = new WaterSplashCubes.ParticlePlacement(new Vector3i(_x, _y, _z), _dir, _type);
			WaterSplashCubes.addList.Add(particlePlacement);
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x0004DEE0 File Offset: 0x0004C0E0
	public static void Clear()
	{
		object syncRoot = WaterSplashCubes.GetSyncRoot();
		lock (syncRoot)
		{
			for (int i = 0; i < WaterSplashCubes.splashes.Count; i++)
			{
				DictionaryList<int, GameObject> dictionaryList = WaterSplashCubes.splashes.list[i];
				for (int j = 0; j < dictionaryList.list.Count; j++)
				{
					global::UnityEngine.Object.DestroyImmediate(dictionaryList.list[j]);
				}
				dictionaryList.Clear();
			}
			WaterSplashCubes.splashes.Clear();
		}
	}

	// Token: 0x04000B1F RID: 2847
	public static WaterSplashCubes instance = null;

	// Token: 0x04000B20 RID: 2848
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryList<long, DictionaryList<int, GameObject>> splashes = new DictionaryList<long, DictionaryList<int, GameObject>>();

	// Token: 0x04000B21 RID: 2849
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject root;

	// Token: 0x04000B22 RID: 2850
	public static List<WaterSplashCubes.ParticlePlacement> addList;

	// Token: 0x04000B23 RID: 2851
	public static List<Vector3i> removeList;

	// Token: 0x04000B24 RID: 2852
	[PublicizedFrom(EAccessModifier.Private)]
	public static int checkListNum = 0;

	// Token: 0x04000B25 RID: 2853
	[PublicizedFrom(EAccessModifier.Private)]
	public static global::UnityEngine.Object[] waterFallSplashCubeEffect = null;

	// Token: 0x04000B26 RID: 2854
	public static float particleLimiter = 1f;

	// Token: 0x04000B27 RID: 2855
	[PublicizedFrom(EAccessModifier.Private)]
	public static int particleCount = 0;

	// Token: 0x04000B28 RID: 2856
	[PublicizedFrom(EAccessModifier.Private)]
	public static float cleanUpTimer = 0f;

	// Token: 0x04000B29 RID: 2857
	[PublicizedFrom(EAccessModifier.Private)]
	public static int currentCleanupIndex = 15;

	// Token: 0x02000199 RID: 409
	public enum SplashType
	{
		// Token: 0x04000B2B RID: 2859
		Splash,
		// Token: 0x04000B2C RID: 2860
		Slope,
		// Token: 0x04000B2D RID: 2861
		Area,
		// Token: 0x04000B2E RID: 2862
		Mist
	}

	// Token: 0x0200019A RID: 410
	public class ParticlePlacement
	{
		// Token: 0x06000C6D RID: 3181 RVA: 0x0004DFBB File Offset: 0x0004C1BB
		public ParticlePlacement(Vector3i _pos, BlockFace _dir, WaterSplashCubes.SplashType _type)
		{
			this.pos = _pos;
			this.dir = _dir;
			this.type = _type;
		}

		// Token: 0x04000B2F RID: 2863
		public Vector3i pos;

		// Token: 0x04000B30 RID: 2864
		public BlockFace dir;

		// Token: 0x04000B31 RID: 2865
		public WaterSplashCubes.SplashType type;
	}
}
