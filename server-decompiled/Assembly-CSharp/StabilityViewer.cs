using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020012F7 RID: 4855
public class StabilityViewer
{
	// Token: 0x060099D4 RID: 39380 RVA: 0x003A5594 File Offset: 0x003A3794
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

	// Token: 0x1700126A RID: 4714
	// (get) Token: 0x060099D5 RID: 39381 RVA: 0x002E72F1 File Offset: 0x002E54F1
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

	// Token: 0x060099D6 RID: 39382 RVA: 0x003A56C8 File Offset: 0x003A38C8
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

	// Token: 0x060099D7 RID: 39383 RVA: 0x003A5800 File Offset: 0x003A3A00
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

	// Token: 0x060099D8 RID: 39384 RVA: 0x003A58A4 File Offset: 0x003A3AA4
	public void OnDestroy()
	{
		this.Clear();
	}

	// Token: 0x060099D9 RID: 39385 RVA: 0x003A58AC File Offset: 0x003A3AAC
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

	// Token: 0x060099DA RID: 39386 RVA: 0x003A5914 File Offset: 0x003A3B14
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

	// Token: 0x060099DB RID: 39387 RVA: 0x003A598C File Offset: 0x003A3B8C
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

	// Token: 0x0400739E RID: 29598
	public static Dictionary<Vector3i, GameObject> boxes = new Dictionary<Vector3i, GameObject>();

	// Token: 0x0400739F RID: 29599
	public static List<Vector3i> buildingChunks = new List<Vector3i>();

	// Token: 0x040073A0 RID: 29600
	public bool worldIsReady;

	// Token: 0x040073A1 RID: 29601
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i startPos;

	// Token: 0x040073A2 RID: 29602
	public static List<Material> materials = new List<Material>();

	// Token: 0x040073A3 RID: 29603
	public static GameObject displayObject = null;

	// Token: 0x040073A4 RID: 29604
	public static GameObject StabilityViewBoxes = null;

	// Token: 0x040073A5 RID: 29605
	public static Material stabilityMtrl = null;

	// Token: 0x040073A6 RID: 29606
	public static int numMaterials = 7;

	// Token: 0x040073A7 RID: 29607
	public static bool bGatheringChunks = false;

	// Token: 0x040073A8 RID: 29608
	public static int TotalIterations = 0;

	// Token: 0x040073A9 RID: 29609
	public static int GetBlocks = 0;

	// Token: 0x040073AA RID: 29610
	[PublicizedFrom(EAccessModifier.Private)]
	public Text debugText;

	// Token: 0x040073AB RID: 29611
	public int searchSizeXZ = 5;

	// Token: 0x040073AC RID: 29612
	public int searchSizeY = 2;

	// Token: 0x040073AD RID: 29613
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i currSearch;

	// Token: 0x040073AE RID: 29614
	[PublicizedFrom(EAccessModifier.Private)]
	public int asynCount = 100;

	// Token: 0x040073AF RID: 29615
	[PublicizedFrom(EAccessModifier.Private)]
	public float totalTimer;

	// Token: 0x040073B0 RID: 29616
	[PublicizedFrom(EAccessModifier.Private)]
	public float prevTotalTimer;

	// Token: 0x040073B1 RID: 29617
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateDisplayTimer;

	// Token: 0x040073B2 RID: 29618
	[PublicizedFrom(EAccessModifier.Private)]
	public float totalTime;

	// Token: 0x040073B3 RID: 29619
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject textGo;

	// Token: 0x040073B4 RID: 29620
	[PublicizedFrom(EAccessModifier.Private)]
	public Camera mainCam;

	// Token: 0x040073B5 RID: 29621
	[PublicizedFrom(EAccessModifier.Private)]
	public bool startedSearch;

	// Token: 0x040073B6 RID: 29622
	[PublicizedFrom(EAccessModifier.Private)]
	public bool flipFlop;

	// Token: 0x040073B7 RID: 29623
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CalculatedStability;

	// Token: 0x020012F8 RID: 4856
	public class BuildStabilityBlocks
	{
		// Token: 0x060099DD RID: 39389 RVA: 0x003A5E8D File Offset: 0x003A408D
		public BuildStabilityBlocks(Vector3i _startPos)
		{
			this.startPos = _startPos;
			GameManager.Instance.StartCoroutine(this.RegisterWhenDone());
		}

		// Token: 0x060099DE RID: 39390 RVA: 0x003A5EAD File Offset: 0x003A40AD
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

		// Token: 0x040073B8 RID: 29624
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i startPos;
	}
}
