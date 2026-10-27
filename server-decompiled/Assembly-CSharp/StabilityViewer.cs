using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001309 RID: 4873
public class StabilityViewer
{
	// Token: 0x06009A2C RID: 39468 RVA: 0x003A7998 File Offset: 0x003A5B98
	public StabilityViewer()
	{
		StabilityViewer.displayObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		StabilityViewer.displayObject.transform.localScale = Vector3.one * 1.01f;
		global::UnityEngine.Object.Destroy(StabilityViewer.displayObject.GetComponent<BoxCollider>());
		for (int i = 0; i < StabilityViewer.numMaterials; i++)
		{
			StabilityViewer.materials.Add(Resources.Load<Material>("Materials/Stability" + i.ToString()));
		}
		StabilityViewer.stabilityMtrl = Resources.Load<Material>("Materials/Stability");
		StabilityViewer.StabilityViewBoxes = new GameObject();
		StabilityViewer.StabilityViewBoxes.name = "StabilityViewBoxes";
		if (this.debugText == null)
		{
			GameObject gameObject = Resources.Load<GameObject>("Prefabs/StabilityCanvas");
			if (gameObject != null)
			{
				this.textGo = global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
				if (this.textGo != null)
				{
					this.debugText = this.textGo.GetComponentInChildren<Text>();
					this.debugText.text = "Recalculating Stability.  Please Wait...";
				}
			}
		}
		this.startedSearch = false;
		this.flipFlop = false;
		StabilityViewer.bGatheringChunks = false;
		this.CalculatedStability = false;
	}

	// Token: 0x17001276 RID: 4726
	// (get) Token: 0x06009A2D RID: 39469 RVA: 0x002E8BED File Offset: 0x002E6DED
	public ChunkCluster ChunkCluster0
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (GameManager.Instance == null)
			{
				return null;
			}
			if (GameManager.Instance.World != null)
			{
				return GameManager.Instance.World.ChunkCache;
			}
			return null;
		}
	}

	// Token: 0x06009A2E RID: 39470 RVA: 0x003A7ACC File Offset: 0x003A5CCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RecalcStability()
	{
		if (this.ChunkCluster0 != null)
		{
			ReaderWriterLockSlim syncRoot = this.ChunkCluster0.GetSyncRoot();
			syncRoot.EnterWriteLock();
			StabilityInitializer stabilityInitializer = new StabilityInitializer(GameManager.Instance.World);
			MicroStopwatch microStopwatch = new MicroStopwatch();
			foreach (Chunk chunk in this.ChunkCluster0.GetChunkArray())
			{
				chunk.ResetStability();
			}
			syncRoot.ExitWriteLock();
			foreach (Chunk chunk2 in this.ChunkCluster0.GetChunkArray())
			{
				stabilityInitializer.DistributeStability(chunk2);
				chunk2.NeedsRegeneration = true;
			}
			Log.Out(string.Concat(new string[]
			{
				"#",
				this.ChunkCluster0.GetChunkArray().Count.ToString(),
				" chunks needed ",
				microStopwatch.ElapsedMilliseconds.ToString(),
				"ms"
			}));
		}
	}

	// Token: 0x06009A2F RID: 39471 RVA: 0x003A7C04 File Offset: 0x003A5E04
	public static GameObject GetBlock(float stability)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(StabilityViewer.displayObject);
		if (stability <= 0f)
		{
			gameObject.GetComponent<MeshRenderer>().material = StabilityViewer.materials[StabilityViewer.numMaterials - 1];
			return gameObject;
		}
		float num = 1f / (float)StabilityViewer.numMaterials;
		bool flag = false;
		for (int i = 0; i < StabilityViewer.numMaterials; i++)
		{
			if (stability >= 1f - num * (float)i)
			{
				gameObject.GetComponent<MeshRenderer>().material = StabilityViewer.materials[i];
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			gameObject.GetComponent<MeshRenderer>().material = StabilityViewer.materials[StabilityViewer.numMaterials - 1];
		}
		return gameObject;
	}

	// Token: 0x06009A30 RID: 39472 RVA: 0x003A7CA8 File Offset: 0x003A5EA8
	public void OnDestroy()
	{
		this.Clear();
	}

	// Token: 0x06009A31 RID: 39473 RVA: 0x003A7CB0 File Offset: 0x003A5EB0
	public void Clear()
	{
		StabilityViewer.TotalIterations = 0;
		StabilityViewer.buildingChunks.Clear();
		StabilityViewer.boxes.Clear();
		global::UnityEngine.Object.Destroy(StabilityViewer.StabilityViewBoxes);
		StabilityViewer.StabilityViewBoxes = null;
		global::UnityEngine.Object.Destroy(this.textGo);
		this.textGo = null;
		this.debugText = null;
		global::UnityEngine.Object.Destroy(StabilityViewer.displayObject);
		StabilityViewer.displayObject = null;
		this.CalculatedStability = false;
	}

	// Token: 0x06009A32 RID: 39474 RVA: 0x003A7D18 File Offset: 0x003A5F18
	public void StartSearch(int _asynCount = 100)
	{
		this.startedSearch = true;
		this.asynCount = _asynCount;
		StabilityViewer.bGatheringChunks = true;
		this.currSearch.x = -this.searchSizeXZ;
		this.currSearch.y = -this.searchSizeY;
		this.currSearch.z = -this.searchSizeXZ;
		this.totalTimer = Time.realtimeSinceStartup;
		this.prevTotalTimer = Time.realtimeSinceStartup;
		this.updateDisplayTimer = 0f;
	}

	// Token: 0x06009A33 RID: 39475 RVA: 0x003A7D90 File Offset: 0x003A5F90
	public void Update()
	{
		if (Time.time > this.updateDisplayTimer + 3f)
		{
			this.updateDisplayTimer = Time.time;
			this.prevTotalTimer = this.totalTime;
			if (!this.CalculatedStability)
			{
				this.RecalcStability();
				this.CalculatedStability = true;
			}
		}
		if (!StabilityViewer.bGatheringChunks)
		{
			return;
		}
		this.totalTime = Time.realtimeSinceStartup - this.totalTimer;
		if (this.debugText != null)
		{
			this.debugText.text = string.Concat(new string[]
			{
				"Chunks Finished: ",
				StabilityViewer.boxes.Count.ToString(),
				" Time( ",
				this.prevTotalTimer.ToCultureInvariantString(),
				" : ",
				this.totalTime.ToCultureInvariantString(),
				" ) GetBlock(): ",
				StabilityViewer.GetBlocks.ToString()
			});
		}
		if (StabilityViewer.buildingChunks.Count > this.asynCount)
		{
			return;
		}
		if (!this.worldIsReady && GameManager.Instance.World != null && Camera.main != null)
		{
			this.mainCam = Camera.main;
			this.worldIsReady = true;
		}
		if (GameManager.Instance.World == null)
		{
			this.worldIsReady = false;
		}
		if (!this.worldIsReady)
		{
			return;
		}
		Vector3i vector3i;
		vector3i.x = Mathf.FloorToInt(this.mainCam.transform.position.x);
		vector3i.y = Mathf.FloorToInt(this.mainCam.transform.position.y);
		vector3i.z = Mathf.FloorToInt(this.mainCam.transform.position.z);
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		Chunk chunk = (Chunk)chunkCache.GetChunkFromWorldPos(vector3i);
		if (chunk == null)
		{
			return;
		}
		this.startPos = chunk.GetWorldPos();
		this.startPos.y = vector3i.y - vector3i.y % 16;
		Dictionary<Vector3i, GameObject> dictionary = StabilityViewer.boxes;
		lock (dictionary)
		{
			List<Vector3i> list = StabilityViewer.buildingChunks;
			lock (list)
			{
				if (!StabilityViewer.boxes.ContainsKey(this.startPos) && !StabilityViewer.buildingChunks.Contains(this.startPos))
				{
					StabilityViewer.buildingChunks.Add(this.startPos);
					new StabilityViewer.BuildStabilityBlocks(this.startPos);
				}
			}
		}
		Vector3i zero = Vector3i.zero;
		dictionary = StabilityViewer.boxes;
		lock (dictionary)
		{
			List<Vector3i> list = StabilityViewer.buildingChunks;
			lock (list)
			{
				zero.x = this.currSearch.x * 16;
				zero.y = this.currSearch.y * 16;
				zero.z = this.currSearch.z * 16;
				Vector3i vector3i2 = this.startPos + zero;
				if (!StabilityViewer.boxes.ContainsKey(vector3i2) && !StabilityViewer.buildingChunks.Contains(vector3i2))
				{
					Chunk chunk2 = (Chunk)chunkCache.GetChunkFromWorldPos(vector3i2);
					if (chunk2 == null)
					{
						StabilityViewer.boxes.Add(vector3i2, null);
					}
					else if (chunk2.GetAvailable())
					{
						if (chunk2.IsEmpty())
						{
							StabilityViewer.boxes.Add(vector3i2, null);
						}
						else
						{
							StabilityViewer.buildingChunks.Add(vector3i2);
							new StabilityViewer.BuildStabilityBlocks(vector3i2);
						}
					}
					else
					{
						StabilityViewer.buildingChunks.Add(vector3i2);
						new StabilityViewer.BuildStabilityBlocks(vector3i2);
					}
				}
			}
		}
		this.currSearch.x = this.currSearch.x + 1;
		if (this.currSearch.x >= this.searchSizeXZ)
		{
			this.currSearch.x = -this.searchSizeXZ;
			this.currSearch.z = this.currSearch.z + 1;
		}
		if (this.currSearch.z >= this.searchSizeXZ)
		{
			this.currSearch.z = -this.searchSizeXZ;
			this.currSearch.y = this.currSearch.y + 1;
		}
		if (this.currSearch.y >= this.searchSizeY)
		{
			this.currSearch.y = -this.searchSizeY;
			StabilityViewer.bGatheringChunks = false;
			Log.Out("Stability DONE");
		}
	}

	// Token: 0x040073DF RID: 29663
	public static Dictionary<Vector3i, GameObject> boxes = new Dictionary<Vector3i, GameObject>();

	// Token: 0x040073E0 RID: 29664
	public static List<Vector3i> buildingChunks = new List<Vector3i>();

	// Token: 0x040073E1 RID: 29665
	public bool worldIsReady;

	// Token: 0x040073E2 RID: 29666
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i startPos;

	// Token: 0x040073E3 RID: 29667
	public static List<Material> materials = new List<Material>();

	// Token: 0x040073E4 RID: 29668
	public static GameObject displayObject = null;

	// Token: 0x040073E5 RID: 29669
	public static GameObject StabilityViewBoxes = null;

	// Token: 0x040073E6 RID: 29670
	public static Material stabilityMtrl = null;

	// Token: 0x040073E7 RID: 29671
	public static int numMaterials = 7;

	// Token: 0x040073E8 RID: 29672
	public static bool bGatheringChunks = false;

	// Token: 0x040073E9 RID: 29673
	public static int TotalIterations = 0;

	// Token: 0x040073EA RID: 29674
	public static int GetBlocks = 0;

	// Token: 0x040073EB RID: 29675
	[PublicizedFrom(EAccessModifier.Private)]
	public Text debugText;

	// Token: 0x040073EC RID: 29676
	public int searchSizeXZ = 5;

	// Token: 0x040073ED RID: 29677
	public int searchSizeY = 2;

	// Token: 0x040073EE RID: 29678
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i currSearch;

	// Token: 0x040073EF RID: 29679
	[PublicizedFrom(EAccessModifier.Private)]
	public int asynCount = 100;

	// Token: 0x040073F0 RID: 29680
	[PublicizedFrom(EAccessModifier.Private)]
	public float totalTimer;

	// Token: 0x040073F1 RID: 29681
	[PublicizedFrom(EAccessModifier.Private)]
	public float prevTotalTimer;

	// Token: 0x040073F2 RID: 29682
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateDisplayTimer;

	// Token: 0x040073F3 RID: 29683
	[PublicizedFrom(EAccessModifier.Private)]
	public float totalTime;

	// Token: 0x040073F4 RID: 29684
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject textGo;

	// Token: 0x040073F5 RID: 29685
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera mainCam;

	// Token: 0x040073F6 RID: 29686
	[PublicizedFrom(EAccessModifier.Private)]
	public bool startedSearch;

	// Token: 0x040073F7 RID: 29687
	[PublicizedFrom(EAccessModifier.Private)]
	public bool flipFlop;

	// Token: 0x040073F8 RID: 29688
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CalculatedStability;

	// Token: 0x0200130A RID: 4874
	public class BuildStabilityBlocks
	{
		// Token: 0x06009A35 RID: 39477 RVA: 0x003A8291 File Offset: 0x003A6491
		public BuildStabilityBlocks(Vector3i _startPos)
		{
			this.startPos = _startPos;
			GameManager.Instance.StartCoroutine(this.RegisterWhenDone());
		}

		// Token: 0x06009A36 RID: 39478 RVA: 0x003A82B1 File Offset: 0x003A64B1
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator RegisterWhenDone()
		{
			List<Vector3i> positions = null;
			List<float> stabilityValues = null;
			yield return new WaitForSeconds(0.01f);
			Chunk chunk = (Chunk)GameManager.Instance.World.ChunkCache.GetChunkFromWorldPos(this.startPos);
			List<Vector3i> list;
			if (chunk == null)
			{
				Dictionary<Vector3i, GameObject> dictionary = StabilityViewer.boxes;
				lock (dictionary)
				{
					list = StabilityViewer.buildingChunks;
					lock (list)
					{
						StabilityViewer.buildingChunks.Remove(this.startPos);
						StabilityViewer.boxes[this.startPos] = null;
						yield break;
					}
				}
			}
			if (chunk.GetAvailable())
			{
				Dictionary<Vector3i, GameObject> dictionary;
				if (chunk.IsEmpty())
				{
					dictionary = StabilityViewer.boxes;
					lock (dictionary)
					{
						list = StabilityViewer.buildingChunks;
						lock (list)
						{
							StabilityViewer.buildingChunks.Remove(this.startPos);
							StabilityViewer.boxes[this.startPos] = null;
							goto IL_0236;
						}
					}
				}
				positions = new List<Vector3i>();
				stabilityValues = new List<float>();
				for (int i = 0; i < 16; i++)
				{
					for (int j = 0; j < 16; j++)
					{
						for (int k = 0; k < 16; k++)
						{
							Vector3i vector3i;
							vector3i.x = k;
							vector3i.y = j;
							vector3i.z = i;
							BlockValue block = GameManager.Instance.World.GetBlock(this.startPos + vector3i);
							if (!block.Block.shape.IsTerrain() && !block.isair)
							{
								float blockStability = StabilityCalculator.GetBlockStability(this.startPos + vector3i);
								positions.Add(this.startPos + vector3i);
								stabilityValues.Add(blockStability);
							}
						}
					}
				}
				IL_0236:
				GameObject gameObject = null;
				if (positions != null)
				{
					gameObject = new GameObject();
					gameObject.AddComponent<MeshRenderer>().material = StabilityViewer.stabilityMtrl;
					MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
					meshFilter.mesh = new Mesh();
					Vector3[] array = new Vector3[8 * positions.Count];
					Color[] array2 = new Color[8 * positions.Count];
					int[] array3 = new int[36 * positions.Count];
					for (int l = 0; l < positions.Count; l++)
					{
						for (int m = 0; m < 8; m++)
						{
							array2[l * 8 + m] = Color.white * stabilityValues[l];
						}
						array[l * 8].x = (float)positions[l].x - 0.01f;
						array[l * 8].y = (float)positions[l].y - 0.01f;
						array[l * 8].z = (float)positions[l].z - 0.01f;
						array[l * 8 + 1].x = (float)positions[l].x - 0.01f;
						array[l * 8 + 1].y = (float)positions[l].y - 0.01f;
						array[l * 8 + 1].z = (float)positions[l].z + 1.01f;
						array[l * 8 + 2].x = (float)positions[l].x + 1.01f;
						array[l * 8 + 2].y = (float)positions[l].y - 0.01f;
						array[l * 8 + 2].z = (float)positions[l].z + 1.01f;
						array[l * 8 + 3].x = (float)positions[l].x + 1.01f;
						array[l * 8 + 3].y = (float)positions[l].y - 0.01f;
						array[l * 8 + 3].z = (float)positions[l].z - 0.01f;
						array[l * 8 + 4].x = (float)positions[l].x - 0.01f;
						array[l * 8 + 4].y = (float)positions[l].y + 1.01f;
						array[l * 8 + 4].z = (float)positions[l].z - 0.01f;
						array[l * 8 + 5].x = (float)positions[l].x - 0.01f;
						array[l * 8 + 5].y = (float)positions[l].y + 1.01f;
						array[l * 8 + 5].z = (float)positions[l].z + 1.01f;
						array[l * 8 + 6].x = (float)positions[l].x + 1.01f;
						array[l * 8 + 6].y = (float)positions[l].y + 1.01f;
						array[l * 8 + 6].z = (float)positions[l].z + 1.01f;
						array[l * 8 + 7].x = (float)positions[l].x + 1.01f;
						array[l * 8 + 7].y = (float)positions[l].y + 1.01f;
						array[l * 8 + 7].z = (float)positions[l].z - 0.01f;
						int num = 0;
						array3[l * 36 + num] = l * 8;
						num++;
						array3[l * 36 + num] = 3 + l * 8;
						num++;
						array3[l * 36 + num] = 2 + l * 8;
						num++;
						array3[l * 36 + num] = 2 + l * 8;
						num++;
						array3[l * 36 + num] = 1 + l * 8;
						num++;
						array3[l * 36 + num] = l * 8;
						num++;
						array3[l * 36 + num] = 4 + l * 8;
						num++;
						array3[l * 36 + num] = 5 + l * 8;
						num++;
						array3[l * 36 + num] = 6 + l * 8;
						num++;
						array3[l * 36 + num] = 6 + l * 8;
						num++;
						array3[l * 36 + num] = 7 + l * 8;
						num++;
						array3[l * 36 + num] = 4 + l * 8;
						num++;
						array3[l * 36 + num] = 4 + l * 8;
						num++;
						array3[l * 36 + num] = 7 + l * 8;
						num++;
						array3[l * 36 + num] = 3 + l * 8;
						num++;
						array3[l * 36 + num] = 3 + l * 8;
						num++;
						array3[l * 36 + num] = l * 8;
						num++;
						array3[l * 36 + num] = 4 + l * 8;
						num++;
						array3[l * 36 + num] = 6 + l * 8;
						num++;
						array3[l * 36 + num] = 5 + l * 8;
						num++;
						array3[l * 36 + num] = 1 + l * 8;
						num++;
						array3[l * 36 + num] = 1 + l * 8;
						num++;
						array3[l * 36 + num] = 2 + l * 8;
						num++;
						array3[l * 36 + num] = 6 + l * 8;
						num++;
						array3[l * 36 + num] = 7 + l * 8;
						num++;
						array3[l * 36 + num] = 6 + l * 8;
						num++;
						array3[l * 36 + num] = 2 + l * 8;
						num++;
						array3[l * 36 + num] = 2 + l * 8;
						num++;
						array3[l * 36 + num] = 3 + l * 8;
						num++;
						array3[l * 36 + num] = 7 + l * 8;
						num++;
						array3[l * 36 + num] = 5 + l * 8;
						num++;
						array3[l * 36 + num] = 4 + l * 8;
						num++;
						array3[l * 36 + num] = l * 8;
						num++;
						array3[l * 36 + num] = l * 8;
						num++;
						array3[l * 36 + num] = 1 + l * 8;
						num++;
						array3[l * 36 + num] = 5 + l * 8;
						num++;
					}
					meshFilter.mesh.Clear(false);
					meshFilter.mesh.vertices = array;
					meshFilter.mesh.SetIndices(array3, MeshTopology.Triangles, 0);
					meshFilter.mesh.colors = array2;
					if (StabilityViewer.StabilityViewBoxes != null)
					{
						gameObject.transform.parent = StabilityViewer.StabilityViewBoxes.transform;
					}
					else
					{
						global::UnityEngine.Object.Destroy(gameObject);
					}
				}
				dictionary = StabilityViewer.boxes;
				lock (dictionary)
				{
					list = StabilityViewer.buildingChunks;
					lock (list)
					{
						StabilityViewer.boxes[this.startPos] = gameObject;
						StabilityViewer.buildingChunks.Remove(this.startPos);
						yield break;
					}
				}
			}
			list = StabilityViewer.buildingChunks;
			lock (list)
			{
				StabilityViewer.buildingChunks.Remove(this.startPos);
				yield break;
			}
			yield break;
		}

		// Token: 0x040073F9 RID: 29689
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i startPos;
	}
}
