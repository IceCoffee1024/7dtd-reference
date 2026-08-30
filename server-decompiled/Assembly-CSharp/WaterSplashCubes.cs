using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class WaterSplashCubes
{
	// Token: 0x06000C6E RID: 3182 RVA: 0x0004DB48 File Offset: 0x0004BD48
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

	// Token: 0x06000C6F RID: 3183 RVA: 0x0004DB9A File Offset: 0x0004BD9A
	public static long MakeKey(int x, int z)
	{
		return (((long)z & 16777215L) << 24) | ((long)x & 16777215L);
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x0004DBB2 File Offset: 0x0004BDB2
	public static object GetSyncRoot()
	{
		return ((ICollection)WaterSplashCubes.splashes.dict).SyncRoot;
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0004DBC4 File Offset: 0x0004BDC4
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

	// Token: 0x06000C72 RID: 3186 RVA: 0x0004DDC4 File Offset: 0x0004BFC4
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

	// Token: 0x06000C73 RID: 3187 RVA: 0x0004E068 File Offset: 0x0004C268
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

	// Token: 0x06000C74 RID: 3188 RVA: 0x0004E238 File Offset: 0x0004C438
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

	// Token: 0x06000C75 RID: 3189 RVA: 0x0004E28C File Offset: 0x0004C48C
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

	// Token: 0x06000C76 RID: 3190 RVA: 0x0004E2F8 File Offset: 0x0004C4F8
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

	// Token: 0x04000B2C RID: 2860
	public static WaterSplashCubes instance = null;

	// Token: 0x04000B2D RID: 2861
	[PublicizedFrom(EAccessModifier.Private)]
	public static DictionaryList<long, DictionaryList<int, GameObject>> splashes = new DictionaryList<long, DictionaryList<int, GameObject>>();

	// Token: 0x04000B2E RID: 2862
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameObject root;

	// Token: 0x04000B2F RID: 2863
	public static List<WaterSplashCubes.ParticlePlacement> addList;

	// Token: 0x04000B30 RID: 2864
	public static List<Vector3i> removeList;

	// Token: 0x04000B31 RID: 2865
	[PublicizedFrom(EAccessModifier.Private)]
	public static int checkListNum = 0;

	// Token: 0x04000B32 RID: 2866
	[PublicizedFrom(EAccessModifier.Private)]
	public static global::UnityEngine.Object[] waterFallSplashCubeEffect = null;

	// Token: 0x04000B33 RID: 2867
	public static float particleLimiter = 1f;

	// Token: 0x04000B34 RID: 2868
	[PublicizedFrom(EAccessModifier.Private)]
	public static int particleCount = 0;

	// Token: 0x04000B35 RID: 2869
	[PublicizedFrom(EAccessModifier.Private)]
	public static float cleanUpTimer = 0f;

	// Token: 0x04000B36 RID: 2870
	[PublicizedFrom(EAccessModifier.Private)]
	public static int currentCleanupIndex = 15;

	// Token: 0x0200019B RID: 411
	public enum SplashType
	{
		// Token: 0x04000B38 RID: 2872
		Splash,
		// Token: 0x04000B39 RID: 2873
		Slope,
		// Token: 0x04000B3A RID: 2874
		Area,
		// Token: 0x04000B3B RID: 2875
		Mist
	}

	// Token: 0x0200019C RID: 412
	public class ParticlePlacement
	{
		// Token: 0x06000C78 RID: 3192 RVA: 0x0004E3D3 File Offset: 0x0004C5D3
		public ParticlePlacement(Vector3i _pos, BlockFace _dir, WaterSplashCubes.SplashType _type)
		{
			this.pos = _pos;
			this.dir = _dir;
			this.type = _type;
		}

		// Token: 0x04000B3C RID: 2876
		public Vector3i pos;

		// Token: 0x04000B3D RID: 2877
		public BlockFace dir;

		// Token: 0x04000B3E RID: 2878
		public WaterSplashCubes.SplashType type;
	}
}
