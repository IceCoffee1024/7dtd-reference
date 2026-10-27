using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000C4B RID: 3147
public class DistantTerrain
{
	// Token: 0x06005F36 RID: 24374 RVA: 0x0025B4B0 File Offset: 0x002596B0
	public void Init()
	{
		DistantTerrain.cShiftMidResChunks = DistantTerrain.cShiftHiResChunks + new Vector3(0f, -0.2f, 0f);
		DistantTerrain.cShiftLowResChunks = DistantTerrain.cShiftMidResChunks + new Vector3(0f, -0.2f, 0f);
		DistantTerrain.Instance = this;
		this.goDistantTerrain = new GameObject("DistantChunks");
		Origin.Add(this.goDistantTerrain.transform, 0);
		float[][] array = new float[2][];
		float[][] array2 = new float[2][];
		int[][] array3 = new int[2][];
		int[][] array4 = new int[2][];
		int[][] array5 = new int[2][];
		Vector3[][] array6 = new Vector3[2][];
		this.TerExQuadTree = new DistantTerrainQuadTree(20000, 20000, 16, 16);
		array[0] = new float[] { 1024f };
		array2[0] = new float[] { 256f };
		array3[0] = new int[] { 33 };
		array4[0] = new int[] { 9 };
		array5[0] = new int[] { 2 };
		array6[0] = new Vector3[] { Vector3.zero };
		array[1] = new float[] { 192f, 512f, 2560f };
		array2[1] = new float[] { 16f, 64f, 256f };
		array3[1] = new int[] { 17, 17, 17 };
		array4[1] = new int[] { 3, 3, 5 };
		array6[1] = new Vector3[]
		{
			DistantTerrain.cShiftHiResChunks,
			DistantTerrain.cShiftMidResChunks,
			DistantTerrain.cShiftLowResChunks
		};
		int num = Utils.FastMin(12, GameUtils.GetViewDistance()) * 2 * 16;
		num = num / 128 + ((num % 128 != 0) ? 1 : 0);
		int num2 = (num * 128 + 128) / 2;
		int num3 = (int)((float)((int)(2000f / array2[1][2])) * array2[1][2]);
		array[1][0] = (float)num2;
		array[1][2] = (float)num3;
		array5[1] = new int[] { 0, 1, 2 };
		float[] array7 = new float[] { 1f, 1f, 1f, 1f };
		this.CurMapId = 1;
		this.ChunkBackGroundMapId = 1;
		this.MainChunkMap = new DistantChunkMap[2];
		this.MainChunkMap[0] = new DistantChunkMap(new Vector2(20000f, 20000f), array[0], array2[0], array3[0], array4[0], array5[0], 28, null, null, this.goDistantTerrain, array6[0]);
		this.MainChunkMap[1] = new DistantChunkMap(new Vector2(20000f, 20000f), array[1], array2[1], array3[1], array4[1], array5[1], 28, null, null, this.goDistantTerrain, array6[1]);
		this.IsOnMapSyncProcess = false;
		this.IsTerrainReady = false;
		this.CurrentPlayerPosVec = new Vector2(float.MaxValue, float.MaxValue);
		this.CurrentPlayerExtendedPosVec = new Vector2(float.MaxValue, float.MaxValue);
		this.MainChunkMap[this.CurMapId].TerrainOrigin = Vector2.zero;
		this.TerrainOriginIntCoor = new Vector2i(0, 0);
		this.NbResLevel = this.MainChunkMap[this.CurMapId].NbResLevel;
		this.MaxNbResLevel = 0;
		for (int i = 0; i < this.MainChunkMap.Length; i++)
		{
			if (this.MaxNbResLevel < this.MainChunkMap[i].NbResLevel)
			{
				this.MaxNbResLevel = this.MainChunkMap[i].NbResLevel;
			}
		}
		this.DistantChunkStack = new OptimizedList<DistantChunk>[this.NbResLevel];
		this.MetaThreadContList = new List<ThreadInfoParam>(this.NbResLevel * 3);
		this.MetaCoroutineContList = new List<ThreadInfoParam>(this.NbResLevel * 3);
		this.PlPosCache = new UtilList<DistantTerrain.PlayerPosHelper>(100000, null);
		this.ChunkActivationCacheDic = new Dictionary<int, DistantTerrain.ChunkStateHelper>();
		this.ChunkToActivateList = new OptimizedList<DistantTerrain.ChunkStateHelper>();
		this.WaterPlaneGameObjStack = new Stack<GameObject>[this.NbResLevel];
		for (int j = 0; j < this.NbResLevel; j++)
		{
			this.WaterPlaneGameObjStack[j] = new Stack<GameObject>();
		}
		this.BaseMesh = new DistantChunkBasicMesh[this.MainChunkMap[this.CurMapId].NbResLevel];
		for (int k = 0; k < this.MainChunkMap[this.CurMapId].NbResLevel; k++)
		{
			this.BaseMesh[k] = this.MainChunkMap[this.CurMapId].ChunkMapInfoArray[k].BaseMesh;
		}
		this.ChunkDataList = new OptimizedList<DistantChunk>[this.MainChunkMap[this.CurMapId].NbResLevel];
		this.ChunkDataProxyArray = new DistantChunk[this.MainChunkMap[this.CurMapId].NbResLevel];
		for (int l = 0; l < this.MainChunkMap[this.CurMapId].NbResLevel; l++)
		{
			this.ChunkDataList[l] = new OptimizedList<DistantChunk>();
			this.ChunkDataProxyArray[l] = new DistantChunk(this.MainChunkMap[this.CurMapId], l, Vector2i.zero, Vector2.zero, array7, this.WaterPlaneGameObjStack);
		}
		for (int m = 0; m < this.MaxNbResLevel; m++)
		{
			this.DistantChunkStack[m] = new OptimizedList<DistantChunk>();
		}
		this.ChunkDataListBackGround = new List<OptimizedList<DistantChunk>[]>();
		this.ChunkDataListBackGround.Add(new OptimizedList<DistantChunk>[this.MainChunkMap[this.ChunkBackGroundMapId].NbResLevel]);
		for (int n = 0; n < this.MainChunkMap[this.ChunkBackGroundMapId].NbResLevel; n++)
		{
			this.ChunkDataListBackGround.Last<OptimizedList<DistantChunk>[]>()[n] = new OptimizedList<DistantChunk>();
		}
		this.ThInfoParamPool = new ThreadInfoParamPool(this.MainChunkMap[this.CurMapId].NbResLevel * 4, 2, 0);
		this.ThProcessingPool = new ThreadProcessingPool(4, 0);
		this.ThContainerPool = new ThreadContainerPool(2000, 0);
		this.EdgeResFactor = new float[] { 1f, 1f, 1f, 1f };
	}

	// Token: 0x06005F37 RID: 24375 RVA: 0x0025BA88 File Offset: 0x00259C88
	public void Cleanup()
	{
		this.MainChunkMap[this.CurMapId].wcd = null;
		this.clearThreadProcess();
		if (!GameManager.IsSplatMapAvailable())
		{
			Renderer[] componentsInChildren = this.goDistantTerrain.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Material[] sharedMaterials = componentsInChildren[i].sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					global::UnityEngine.Object.Destroy(sharedMaterials[j]);
				}
			}
		}
		MeshFilter[] componentsInChildren2 = this.goDistantTerrain.GetComponentsInChildren<MeshFilter>(true);
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			global::UnityEngine.Object.Destroy(componentsInChildren2[k].sharedMesh);
			global::UnityEngine.Object.Destroy(componentsInChildren2[k].mesh);
		}
		global::UnityEngine.Object.Destroy(this.goDistantTerrain);
		this.PlPosCache.Clear();
		this.ChunkActivationCacheDic.Clear();
		this.ChunkToActivateList.Clear();
		DistantTerrain.Instance = null;
	}

	// Token: 0x06005F38 RID: 24376 RVA: 0x0025BB61 File Offset: 0x00259D61
	public void Configure(DelegateGetTerrainHeight _delegate, WorldCreationData _wcd, float _size = 0f)
	{
		DistantChunkMap.TGHeightFunc = _delegate;
		this.MainChunkMap[this.CurMapId].wcd = _wcd;
		this.PlPosCache.Clear();
		this.ChunkActivationCacheDic.Clear();
		this.ChunkToActivateList.Clear();
	}

	// Token: 0x06005F39 RID: 24377 RVA: 0x0025BB9D File Offset: 0x00259D9D
	[PublicizedFrom(EAccessModifier.Private)]
	public void setTerrainGOState(bool IsTerrainActive)
	{
		this.MainChunkMap[this.CurMapId].ParentGameObject.SetActive(IsTerrainActive);
	}

	// Token: 0x06005F3A RID: 24378 RVA: 0x0025BBB8 File Offset: 0x00259DB8
	public void UpdateTerrain(Vector3 InCenterPosXZ)
	{
		if (this.PlPosCache.Count > this.PlPosCache.Capacity - 2)
		{
			this.RedrawTerrain(InCenterPosXZ, this.ChunkBackGroundMapId, this.ChunkDataListBackGround[0]);
		}
		if (this.PlPosCache.Count == 0 || this.PlPosCache[this.PlPosCache.Count - 1].PlayerPos.GetHashCode() != InCenterPosXZ.GetHashCode())
		{
			this.PlPosCache.Add(new DistantTerrain.PlayerPosHelper(InCenterPosXZ));
		}
		bool flag = this.IsSpwaningProcessOngoing();
		if (flag)
		{
			this.threadManagementUpdate();
			return;
		}
		Vector3 vector = this.PlPosCache.Dequeue().PlayerPos;
		int num = 0;
		if (!this.IsOnMapSyncProcess)
		{
			do
			{
				this.ManagePlayerPos(vector);
				this.UpdateCurrentPosOnMap();
				if (this.PlPosCache.Count > 0)
				{
					vector = this.PlPosCache.Dequeue().PlayerPos;
				}
			}
			while (this.MetaThreadContList.Count == 0 && this.PlPosCache.Count > 0);
			if (this.ProcessCacheCnt > 10)
			{
				this.ProcessChunkActivationCache(InCenterPosXZ);
				this.ProcessCacheCnt = 0;
			}
			this.ProcessCacheCnt++;
			this.threadManagementUpdate();
			return;
		}
		if (!flag)
		{
			this.ManagePlayerPos(vector);
			this.UpdateCurrentPosOnMapAsync(this.ChunkBackGroundMapId, 0);
			ThreadInfoParam objectBig = this.ThInfoParamPool.GetObjectBig(this.MainChunkMap[this.CurMapId], 0, 0);
			objectBig.CntThreadContList = 0;
			objectBig.LengthThreadContList = 0;
			objectBig.IsAsynchronous = true;
			for (int i = 0; i < this.ChunkDataListBackGround[0].Length; i++)
			{
				int num2 = 0;
				while (num2 < this.ChunkDataListBackGround[0][i].Count && objectBig.LengthThreadContList < 50)
				{
					if (!this.ChunkDataListBackGround[0][i].array[num2].IsMeshUpdated && !this.ChunkDataListBackGround[0][i].array[num2].IsOnActivationProcess)
					{
						this.ChunkDataListBackGround[0][i].array[num2].IsOnActivationProcess = true;
						this.ChunkDataListBackGround[0][i].array[num2].IsFreeToUse = false;
						objectBig.ThreadContListA[objectBig.LengthThreadContList] = this.ThContainerPool.GetObject(this, this.ChunkDataListBackGround[0][i].array[num2], this.BaseMesh[this.ChunkDataListBackGround[0][i].array[num2].ChunkMapInfo.ChunkDataListResLevel], this.ChunkDataListBackGround[0][i].array[num2].WasReset);
						objectBig.LengthThreadContList++;
					}
					if (!this.ChunkDataListBackGround[0][i].array[num2].IsMeshUpdated)
					{
						num++;
					}
					num2++;
				}
			}
			if (objectBig.LengthThreadContList - objectBig.CntThreadContList != 0)
			{
				this.MetaThreadContList.Add(objectBig);
			}
			else if (objectBig != null)
			{
				this.ThInfoParamPool.ReturnObject(objectBig, this.ThContainerPool);
			}
		}
		if (num == 0 && !flag)
		{
			this.IsOnMapSyncProcess = false;
			this.ResetChunkDataList(this.ChunkDataListBackGround[0]);
			this.IsTerrainReady = true;
			return;
		}
		this.threadManagementUpdate();
	}

	// Token: 0x06005F3B RID: 24379 RVA: 0x0025BF10 File Offset: 0x0025A110
	[PublicizedFrom(EAccessModifier.Private)]
	public void ManagePlayerPos(Vector3 InCenterPosXZ)
	{
		float num = ((this.MainChunkMap[this.ChunkBackGroundMapId].NbResLevel > 1) ? this.MainChunkMap[this.ChunkBackGroundMapId].ChunkMapInfoArray[1].ChunkWidth : this.MainChunkMap[this.ChunkBackGroundMapId].ChunkMapInfoArray[0].ChunkWidth);
		Mathf.Abs(InCenterPosXZ.x - this.CurrentPlayerPosVec.x);
		float num2 = InCenterPosXZ.x - this.CurrentPlayerPosVec.x;
		float num3 = InCenterPosXZ.z - this.CurrentPlayerPosVec.y;
		if (Mathf.Sqrt(num2 * num2 + num3 * num3) >= num)
		{
			this.RedrawTerrain(InCenterPosXZ, this.ChunkBackGroundMapId, this.ChunkDataListBackGround[0]);
		}
		this.setPlayerCurrentPosition(InCenterPosXZ.x, InCenterPosXZ.z);
	}

	// Token: 0x06005F3C RID: 24380 RVA: 0x0025BFE3 File Offset: 0x0025A1E3
	public void RedrawTerrain()
	{
		this.RedrawTerrain(this.CurrentPlayerPosVec, this.ChunkBackGroundMapId, this.ChunkDataListBackGround[0]);
	}

	// Token: 0x06005F3D RID: 24381 RVA: 0x0025C008 File Offset: 0x0025A208
	public void RedrawTerrain(Vector3 PlayerPos, int _CurMapId, OptimizedList<DistantChunk>[] CurChunkDataList)
	{
		int nbResLevel = this.MainChunkMap[_CurMapId].NbResLevel;
		this.PlPosCache.Clear();
		this.ChunkActivationCacheDic.Clear();
		this.ChunkToActivateList.Clear();
		this.CurrentPlayerPosVec.Set(PlayerPos.x, PlayerPos.z);
		this.CurrentPlayerExtendedPosVec.Set(PlayerPos.x, PlayerPos.z);
		float chunkWidth = this.MainChunkMap[_CurMapId].ChunkMapInfoArray[this.NbResLevel - 1].ChunkWidth;
		int num = (int)(this.CurrentPlayerPosVec.x / chunkWidth);
		int num2 = (int)(this.CurrentPlayerPosVec.y / chunkWidth);
		if (PlayerPos.x < 0f)
		{
			num--;
		}
		if (PlayerPos.y < 0f)
		{
			num2--;
		}
		num = 0;
		num2 = 0;
		this.TerrainOriginIntCoor.Set(num, num2);
		this.MainChunkMap[_CurMapId].TerrainOrigin.Set((float)num * chunkWidth, (float)num2 * chunkWidth);
		this.IsTerrainReady = false;
		this.IsOnMapSyncProcess = true;
		this.clearThreadProcess();
		for (int i = 0; i < nbResLevel; i++)
		{
			if (CurChunkDataList[i].Count != 0)
			{
				for (int j = CurChunkDataList[i].Count - 1; j >= 0; j--)
				{
					CurChunkDataList[i].array[j].IsFreeToUse = true;
					this.putDistantChunkOnStack(i, j, CurChunkDataList);
				}
			}
			if (this.ChunkDataList[i].Count != 0)
			{
				for (int j = this.ChunkDataList[i].Count - 1; j >= 0; j--)
				{
					this.ChunkDataList[i].array[j].IsFreeToUse = true;
					this.putDistantChunkOnStack(i, j, this.ChunkDataList);
				}
			}
			int num3 = ((i + 1 < this.NbResLevel) ? (i + 1) : (this.NbResLevel - 1));
			float num4 = Mathf.Floor(this.CurrentPlayerPosVec.x / this.MainChunkMap[_CurMapId].ChunkMapInfoArray[num3].ChunkWidth + 1E-05f) * this.MainChunkMap[_CurMapId].ChunkMapInfoArray[num3].ChunkWidth;
			float num5 = Mathf.Floor(this.CurrentPlayerPosVec.y / this.MainChunkMap[_CurMapId].ChunkMapInfoArray[num3].ChunkWidth + 1E-05f) * this.MainChunkMap[_CurMapId].ChunkMapInfoArray[num3].ChunkWidth;
			this.MainChunkMap[_CurMapId].ChunkMapInfoArray[i].ShiftVec.Set(num4, num5);
			DistantChunkPosData distantChunkPosData = this.MainChunkMap[_CurMapId].ComputeChunkPos((int)this.CurrentPlayerPosVec.x, (int)this.CurrentPlayerPosVec.y, i);
			for (int j = 0; j < distantChunkPosData.ChunkPos.Length; j++)
			{
				this.GetDistantChunkFromStack(this.MainChunkMap[_CurMapId], i, distantChunkPosData.ChunkIntPos[j], this.MainChunkMap[_CurMapId].TerrainOrigin, distantChunkPosData.EdgeResFactor[j], this.WaterPlaneGameObjStack, CurChunkDataList);
			}
		}
	}

	// Token: 0x06005F3E RID: 24382 RVA: 0x0025C2E0 File Offset: 0x0025A4E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void clearThreadProcess()
	{
		if (this.ThProcessing != null)
		{
			if (!this.ThProcessing.IsThreadFinished())
			{
				this.ThProcessing.CancelThread();
				this.ThProcessing.TaskInfo.WaitForEnd();
			}
			this.ThProcessingPool.ReturnObject(this.ThProcessing);
			this.ThProcessing = null;
		}
		for (int i = this.MetaThreadContList.Count - 1; i >= 0; i--)
		{
			while (this.MetaThreadContList[i].CntThreadContList < this.MetaThreadContList[i].LengthThreadContList)
			{
				this.MetaThreadContList[i].ThreadContListA[this.MetaThreadContList[i].CntThreadContList].DChunk.IsFreeToUse = true;
				this.ThContainerPool.ReturnObject(this.MetaThreadContList[i].ThreadContListA[this.MetaThreadContList[i].CntThreadContList], true);
				this.MetaThreadContList[i].CntThreadContList++;
			}
			this.ThInfoParamPool.ReturnObject(this.MetaThreadContList[i], this.ThContainerPool);
			this.MetaThreadContList.RemoveAt(i);
		}
		this.MetaThreadContList.Clear();
		for (int j = this.MetaCoroutineContList.Count - 1; j >= 0; j--)
		{
			while (this.MetaCoroutineContList[j].CntThreadContList < this.MetaCoroutineContList[j].LengthThreadContList)
			{
				this.MetaCoroutineContList[j].ThreadContListA[this.MetaCoroutineContList[j].CntThreadContList].DChunk.IsFreeToUse = true;
				this.ThContainerPool.ReturnObject(this.MetaCoroutineContList[j].ThreadContListA[this.MetaCoroutineContList[j].CntThreadContList], true);
				this.MetaCoroutineContList[j].CntThreadContList++;
			}
			this.ThInfoParamPool.ReturnObject(this.MetaCoroutineContList[j], this.ThContainerPool);
			this.MetaCoroutineContList.RemoveAt(j);
		}
		this.MetaCoroutineContList.Clear();
		if (this.YieldCoroutine != null)
		{
			ThreadManager.StopCoroutine(this.YieldCoroutine);
		}
		this.DebugYieldResLevel = -1;
	}

	// Token: 0x06005F3F RID: 24383 RVA: 0x0025C528 File Offset: 0x0025A728
	public void SetTerrainVisible(bool IsTerrainVisible)
	{
		if (Camera.main != null)
		{
			Transform transform = Camera.main.transform;
			Transform transform2;
			if ((transform2 = transform.Find("NearCamera")) != null)
			{
				if (IsTerrainVisible)
				{
					transform2.GetComponent<Camera>().cullingMask = transform2.GetComponent<Camera>().cullingMask | 268435456;
				}
				else
				{
					transform2.GetComponent<Camera>().cullingMask = transform2.GetComponent<Camera>().cullingMask & -268435457;
				}
			}
			transform2 = transform;
			if (transform != null)
			{
				if (IsTerrainVisible)
				{
					transform2.GetComponent<Camera>().cullingMask = transform2.GetComponent<Camera>().cullingMask | 268435456;
				}
				else
				{
					transform2.GetComponent<Camera>().cullingMask = transform2.GetComponent<Camera>().cullingMask & -268435457;
				}
			}
		}
		this.setTerrainGOState(IsTerrainVisible);
		this.setAllCollidersOnOff(IsTerrainVisible);
	}

	// Token: 0x06005F40 RID: 24384 RVA: 0x0025C5F6 File Offset: 0x0025A7F6
	[PublicizedFrom(EAccessModifier.Private)]
	public void setPlayerCurrentPosition(float _posX, float _posY)
	{
		this.CurrentPlayerPosVec.Set(_posX, _posY);
		this.CurrentPlayerExtendedPosVec.Set(_posX, _posY);
	}

	// Token: 0x06005F41 RID: 24385 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void setAllCollidersOnOff(bool _bColliderEnabled)
	{
	}

	// Token: 0x06005F42 RID: 24386 RVA: 0x0025C614 File Offset: 0x0025A814
	[PublicizedFrom(EAccessModifier.Private)]
	public void setResLevelIdCollidersOnOff(int ResLevelId, bool IsColliderEnabled)
	{
		this.MainChunkMap[this.CurMapId].ChunkMapInfoArray[ResLevelId].IsColliderEnabled = IsColliderEnabled;
		for (int i = 0; i < this.ChunkDataList[ResLevelId].Count; i++)
		{
			this.ChunkDataList[ResLevelId].array[i].CellObj.GetComponent<Collider>().enabled = IsColliderEnabled;
		}
	}

	// Token: 0x06005F43 RID: 24387 RVA: 0x0025C674 File Offset: 0x0025A874
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetDistantChunkFromStack(DistantChunkMap _BaseChunkMap, int _ResLevel, Vector2i _CellIdVector, Vector2 _TerrainOriginCoor, float[] _EdgeResFactor, Stack<GameObject>[] _WaterPlaneGameObjStack)
	{
		DistantChunk distantChunk;
		if (this.DistantChunkStack[_ResLevel].Count > 0)
		{
			distantChunk = this.DistantChunkStack[_ResLevel].array[0];
			this.DistantChunkStack[_ResLevel].RemoveAt(0);
			distantChunk.ResetDistantChunkSameResLevel(_BaseChunkMap, _CellIdVector, _TerrainOriginCoor, _EdgeResFactor);
		}
		else
		{
			distantChunk = new DistantChunk(_BaseChunkMap, _ResLevel, _CellIdVector, _TerrainOriginCoor, _EdgeResFactor, _WaterPlaneGameObjStack);
		}
		this.ChunkDataList[_ResLevel].Add(distantChunk);
	}

	// Token: 0x06005F44 RID: 24388 RVA: 0x0025C6DC File Offset: 0x0025A8DC
	[PublicizedFrom(EAccessModifier.Private)]
	public DistantChunk GetDistantChunkFromStack(DistantChunkMap _BaseChunkMap, int _ResLevel, Vector2i _CellIdVector, Vector2 _TerrainOriginCoor, float[] _EdgeResFactor, Stack<GameObject>[] _WaterPlaneGameObjStack, OptimizedList<DistantChunk>[] CurChunkDataList)
	{
		DistantChunkMapInfo distantChunkMapInfo = _BaseChunkMap.ChunkMapInfoArray[_ResLevel];
		DistantChunk distantChunk;
		if (this.DistantChunkStack[distantChunkMapInfo.ChunkDataListResLevel].Count > 0)
		{
			if (this.DistantChunkStack[distantChunkMapInfo.ChunkDataListResLevel].array[0].IsFreeToUse && !this.DistantChunkStack[distantChunkMapInfo.ChunkDataListResLevel].array[0].IsOnActivationProcess)
			{
				distantChunk = this.DistantChunkStack[distantChunkMapInfo.ChunkDataListResLevel].array[0];
				this.DistantChunkStack[distantChunkMapInfo.ChunkDataListResLevel].RemoveAt(0);
				distantChunk.ResetDistantChunkSameResLevel(_BaseChunkMap, _CellIdVector, _TerrainOriginCoor, _EdgeResFactor);
			}
			else
			{
				distantChunk = new DistantChunk(_BaseChunkMap, _ResLevel, _CellIdVector, _TerrainOriginCoor, _EdgeResFactor, _WaterPlaneGameObjStack);
				this.SetDefaultNewGameObj(distantChunk);
			}
		}
		else
		{
			distantChunk = new DistantChunk(_BaseChunkMap, _ResLevel, _CellIdVector, _TerrainOriginCoor, _EdgeResFactor, _WaterPlaneGameObjStack);
			this.SetDefaultNewGameObj(distantChunk);
		}
		CurChunkDataList[_ResLevel].Add(distantChunk);
		return distantChunk;
	}

	// Token: 0x06005F45 RID: 24389 RVA: 0x0025C7B0 File Offset: 0x0025A9B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDefaultNewGameObj(DistantChunk CurChunk)
	{
		int chunkDataListResLevel = CurChunk.ChunkMapInfo.ChunkDataListResLevel;
		CurChunk.CellObj = new GameObject("DC", new Type[]
		{
			typeof(MeshRenderer),
			typeof(MeshFilter)
		});
		CurChunk.CellObj.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
		CurChunk.CellObj.GetComponent<Renderer>().receiveShadows = false;
		CurChunk.CellObj.transform.parent = this.MainChunkMap[this.CurMapId].ParentGameObject.transform;
		MeshFilter component = CurChunk.CellObj.GetComponent<MeshFilter>();
		if (component.mesh != null)
		{
			global::UnityEngine.Object.Destroy(component.mesh);
		}
		component.mesh = new Mesh();
		Mesh mesh = component.mesh;
		mesh.name = "DC";
		if (OcclusionManager.Instance.cullDistantTerrain)
		{
			Occludee.Add(CurChunk.CellObj);
		}
		mesh.subMeshCount = 1;
	}

	// Token: 0x06005F46 RID: 24390 RVA: 0x0025C8A0 File Offset: 0x0025AAA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void putDistantChunkOnStack(DistantChunk _DistantChunk, bool _AlsoRemoveInChunkDataList)
	{
		if (_DistantChunk == null)
		{
			return;
		}
		if (_DistantChunk.CellObj != null)
		{
			_DistantChunk.CellObj.SetActive(false);
			_DistantChunk.ActivateWaterPlane(0f, false);
			_DistantChunk.IsOnActivationProcess = false;
			_DistantChunk.IsMeshUpdated = false;
			this.DistantChunkStack[_DistantChunk.ResLevel].Add(_DistantChunk);
			if (_AlsoRemoveInChunkDataList)
			{
				this.ChunkDataList[_DistantChunk.ResLevel].Remove(_DistantChunk);
			}
			return;
		}
	}

	// Token: 0x06005F47 RID: 24391 RVA: 0x0025C914 File Offset: 0x0025AB14
	[PublicizedFrom(EAccessModifier.Private)]
	public void putDistantChunkOnStack(int _ResLevel, int _DistantChunkListId)
	{
		if (this.ChunkDataList[_ResLevel].array[_DistantChunkListId] == null)
		{
			return;
		}
		this.ChunkDataList[_ResLevel].array[_DistantChunkListId].CellObj.SetActive(false);
		this.ChunkDataList[_ResLevel].array[_DistantChunkListId].ActivateWaterPlane(0f, false);
		this.ChunkDataList[_ResLevel].array[_DistantChunkListId].IsOnActivationProcess = false;
		this.ChunkDataList[_ResLevel].array[_DistantChunkListId].IsMeshUpdated = false;
		this.DistantChunkStack[_ResLevel].Add(this.ChunkDataList[_ResLevel].array[_DistantChunkListId]);
		this.ChunkDataList[_ResLevel].RemoveAt(_DistantChunkListId);
	}

	// Token: 0x06005F48 RID: 24392 RVA: 0x0025C9BC File Offset: 0x0025ABBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void putDistantChunkOnStack(int _ResLevel, int _DistantChunkListId, OptimizedList<DistantChunk>[] CurChunkDataList)
	{
		if (CurChunkDataList[_ResLevel].array[_DistantChunkListId] == null)
		{
			return;
		}
		if (CurChunkDataList[_ResLevel].array[_DistantChunkListId].CellObj != null)
		{
			CurChunkDataList[_ResLevel].array[_DistantChunkListId].CellObj.SetActive(false);
		}
		CurChunkDataList[_ResLevel].array[_DistantChunkListId].IsMeshUpdated = false;
		CurChunkDataList[_ResLevel].array[_DistantChunkListId].ActivateWaterPlane(0f, false);
		CurChunkDataList[_ResLevel].array[_DistantChunkListId].IsOnActivationProcess = false;
		this.DistantChunkStack[CurChunkDataList[_ResLevel].array[_DistantChunkListId].ChunkMapInfo.ChunkDataListResLevel].Add(CurChunkDataList[_ResLevel].array[_DistantChunkListId]);
		CurChunkDataList[_ResLevel].RemoveAt(_DistantChunkListId);
	}

	// Token: 0x06005F49 RID: 24393 RVA: 0x0025CA6C File Offset: 0x0025AC6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void putDistantChunkOnStack(DistantChunk _DistantChunk, bool _AlsoRemoveInChunkDataList, OptimizedList<DistantChunk>[] CurChunkDataList)
	{
		if (_DistantChunk == null)
		{
			return;
		}
		if (_DistantChunk.CellObj != null)
		{
			_DistantChunk.CellObj.SetActive(false);
		}
		_DistantChunk.ActivateWaterPlane(0f, false);
		_DistantChunk.IsOnActivationProcess = false;
		_DistantChunk.IsMeshUpdated = false;
		this.DistantChunkStack[_DistantChunk.ChunkMapInfo.ChunkDataListResLevel].Add(_DistantChunk);
		if (_AlsoRemoveInChunkDataList)
		{
			CurChunkDataList[_DistantChunk.ResLevel].Remove(_DistantChunk);
		}
	}

	// Token: 0x06005F4A RID: 24394 RVA: 0x0025CADC File Offset: 0x0025ACDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetChunkDataList(int _MapId)
	{
		for (int i = 0; i < this.ChunkDataList.Length; i++)
		{
			for (int j = 0; j < this.ChunkDataList[i].Count; j++)
			{
				this.putDistantChunkOnStack(i, j);
			}
		}
		this.ChunkDataList = new OptimizedList<DistantChunk>[this.MainChunkMap[_MapId].NbResLevel];
		for (int k = 0; k < this.MainChunkMap[_MapId].NbResLevel; k++)
		{
			this.ChunkDataList[k] = new OptimizedList<DistantChunk>();
		}
	}

	// Token: 0x06005F4B RID: 24395 RVA: 0x0025CB5C File Offset: 0x0025AD5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ResetChunkDataList(OptimizedList<DistantChunk>[] ChunkDataListBackground)
	{
		for (int i = 0; i < this.ChunkDataList.Length; i++)
		{
			for (int j = 0; j < this.ChunkDataList[i].Count; j++)
			{
				this.putDistantChunkOnStack(i, j);
			}
		}
		this.ChunkDataList = new OptimizedList<DistantChunk>[ChunkDataListBackground.Length];
		for (int k = 0; k < ChunkDataListBackground.Length; k++)
		{
			this.ChunkDataList[k] = ChunkDataListBackground[k];
			ChunkDataListBackground[k] = new OptimizedList<DistantChunk>();
		}
	}

	// Token: 0x06005F4C RID: 24396 RVA: 0x0025CBCC File Offset: 0x0025ADCC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateCurrentPosOnMap()
	{
		Vector2i vector2i = default(Vector2i);
		Vector2i vector2i2 = default(Vector2i);
		Vector2 vector = default(Vector2);
		vector = this.CurrentPlayerExtendedPosVec - this.MainChunkMap[this.CurMapId].TerrainOrigin;
		for (int i = 0; i < this.NbResLevel; i++)
		{
			int num = this.MainChunkMap[this.CurMapId].FindOutsideFromChunk(vector, i);
			if (num >= 0)
			{
				this.MetaThreadContList.Add(this.ThInfoParamPool.GetObject(this.MainChunkMap[this.CurMapId], i, num));
				this.MetaThreadContList.Last<ThreadInfoParam>().IsAsynchronous = false;
				DistantChunkMapInfo distantChunkMapInfo = this.MainChunkMap[this.CurMapId].ChunkMapInfoArray[i];
				int num2 = i;
				DistantChunkMapInfo distantChunkMapInfo2;
				int num3;
				if (i < this.NbResLevel - 1)
				{
					distantChunkMapInfo2 = this.MainChunkMap[this.CurMapId].ChunkMapInfoArray[i + 1];
					num3 = i + 1;
				}
				else
				{
					distantChunkMapInfo2 = this.MainChunkMap[this.CurMapId].ChunkMapInfoArray[i];
					num3 = i;
				}
				vector2i.Set(Convert.ToInt32(distantChunkMapInfo.ShiftVec.x / distantChunkMapInfo2.ChunkWidth), Convert.ToInt32(distantChunkMapInfo.ShiftVec.y / distantChunkMapInfo2.ChunkWidth));
				vector2i2.Set(Convert.ToInt32(distantChunkMapInfo.ShiftVec.x / distantChunkMapInfo.ChunkWidth), Convert.ToInt32(distantChunkMapInfo.ShiftVec.y / distantChunkMapInfo.ChunkWidth));
				if (i < this.NbResLevel - 1)
				{
					this.MetaThreadContList.Last<ThreadInfoParam>().CntForwardChunkToDeleteId = 0;
					this.MetaThreadContList.Last<ThreadInfoParam>().LengthForwardChunkToDeleteId = 0;
					for (int j = 0; j < distantChunkMapInfo.ChunkToDelete[num].Length; j++)
					{
						int num4 = this.findDistantChunkInList(distantChunkMapInfo.ChunkToDelete[num][j] + vector2i, i + 1);
						if (num4 >= 0)
						{
							this.MetaThreadContList.Last<ThreadInfoParam>().ForwardChunkToDeleteIdA[j] = this.ChunkDataList[i + 1].array[num4];
							this.MetaThreadContList.Last<ThreadInfoParam>().LengthForwardChunkToDeleteId++;
							this.ChunkDataList[i + 1].array[num4].IsOnActivationProcess = true;
							this.ChunkDataList[i + 1].array[num4].IsOnSeamCorrectionProcess = false;
						}
					}
				}
				this.MetaThreadContList.Last<ThreadInfoParam>().CntBackwardChunkToDeleteId = 0;
				this.MetaThreadContList.Last<ThreadInfoParam>().LengthBackwardChunkToDeleteId = 0;
				for (int j = 0; j < distantChunkMapInfo.ChunkToConvDel[num].Length; j++)
				{
					int num4 = this.findDistantChunkInList(distantChunkMapInfo.ChunkToConvDel[num][j] + vector2i2, i);
					if (num4 >= 0)
					{
						this.MetaThreadContList.Last<ThreadInfoParam>().BackwardChunkToDeleteIdA[j] = this.ChunkDataList[i].array[num4];
						this.MetaThreadContList.Last<ThreadInfoParam>().LengthBackwardChunkToDeleteId++;
						this.ChunkDataList[i].array[num4].IsOnActivationProcess = true;
						this.ChunkDataList[i].array[num4].IsOnSeamCorrectionProcess = false;
					}
				}
				this.MetaThreadContList.Last<ThreadInfoParam>().LengthThreadContList = 0;
				for (int j = 0; j < distantChunkMapInfo.ChunkToAdd[num].Length; j++)
				{
					this.EdgeResFactor[0] = distantChunkMapInfo.ChunkToAddEdgeFactor[num][j].x;
					this.EdgeResFactor[1] = distantChunkMapInfo.ChunkToAddEdgeFactor[num][j].y;
					this.EdgeResFactor[2] = distantChunkMapInfo.ChunkToAddEdgeFactor[num][j].z;
					this.EdgeResFactor[3] = distantChunkMapInfo.ChunkToAddEdgeFactor[num][j].w;
					this.GetDistantChunkFromStack(this.MainChunkMap[this.CurMapId], num2, distantChunkMapInfo.ChunkToAdd[num][j] + vector2i2, this.MainChunkMap[this.CurMapId].TerrainOrigin, this.EdgeResFactor, this.WaterPlaneGameObjStack);
					this.MetaThreadContList.Last<ThreadInfoParam>().ThreadContListA[this.MetaThreadContList.Last<ThreadInfoParam>().LengthThreadContList] = this.ThContainerPool.GetObject(this, this.ChunkDataList[i].Last(), this.BaseMesh[i], this.ChunkDataList[i].Last().WasReset);
					this.MetaThreadContList.Last<ThreadInfoParam>().LengthThreadContList++;
					this.ChunkDataList[i].Last().IsOnActivationProcess = true;
				}
				this.MetaThreadContList.Last<ThreadInfoParam>().CntThreadContList = 0;
				if (i < this.NbResLevel - 1)
				{
					for (int j = 0; j < distantChunkMapInfo.ChunkToConvAdd[num].Length; j++)
					{
						this.EdgeResFactor[0] = distantChunkMapInfo.ChunkToConvAddEdgeFactor[num][j].x;
						this.EdgeResFactor[1] = distantChunkMapInfo.ChunkToConvAddEdgeFactor[num][j].y;
						this.EdgeResFactor[2] = distantChunkMapInfo.ChunkToConvAddEdgeFactor[num][j].z;
						this.EdgeResFactor[3] = distantChunkMapInfo.ChunkToConvAddEdgeFactor[num][j].w;
						this.GetDistantChunkFromStack(this.MainChunkMap[this.CurMapId], num3, distantChunkMapInfo.ChunkToConvAdd[num][j] + vector2i, this.MainChunkMap[this.CurMapId].TerrainOrigin, this.EdgeResFactor, this.WaterPlaneGameObjStack);
						this.MetaThreadContList.Last<ThreadInfoParam>().ThreadContListA[this.MetaThreadContList.Last<ThreadInfoParam>().LengthThreadContList] = this.ThContainerPool.GetObject(this, this.ChunkDataList[i + 1].Last(), this.BaseMesh[i + 1], this.ChunkDataList[i + 1].Last().WasReset);
						this.MetaThreadContList.Last<ThreadInfoParam>().LengthThreadContList++;
						this.ChunkDataList[i + 1].Last().IsOnActivationProcess = true;
					}
				}
				if (i < this.NbResLevel - 1)
				{
					for (int j = 0; j < distantChunkMapInfo.ChunkEdgeToOwnResLevel[num].Length; j++)
					{
						int num5 = 0;
						for (int k = 0; k < distantChunkMapInfo.ChunkEdgeToOwnResLevel[num][j].Length; k++)
						{
							int num4 = this.findDistantChunkInList(distantChunkMapInfo.ChunkEdgeToOwnResLevel[num][j][k] + vector2i2, i);
							int num6 = distantChunkMapInfo.ChunkEdgeToOwnRLEdgeId[num][j][k];
							if (num4 > 0)
							{
								this.MetaThreadContList.Last<ThreadInfoParam>().ForwardChunkSeamToAdjust[j][num5] = this.ChunkDataList[i].array[num4];
								this.MetaThreadContList.Last<ThreadInfoParam>().ForwardEdgeId[j][num5] = num6;
								num5++;
								this.ChunkDataList[i].array[num4].IsOnSeamCorrectionProcess = true;
							}
						}
						this.MetaThreadContList.Last<ThreadInfoParam>().SDLengthForwardChunkSeamToAdjust[j] = num5;
					}
					this.MetaThreadContList.Last<ThreadInfoParam>().FDLengthForwardChunkSeamToAdjust = distantChunkMapInfo.ChunkEdgeToOwnResLevel[num].Length;
				}
				if (i < this.NbResLevel - 1)
				{
					for (int j = 0; j < distantChunkMapInfo.ChunkEdgeToNextResLevel[num].Length; j++)
					{
						int k = 0;
						int num5 = 0;
						while (k < distantChunkMapInfo.ChunkEdgeToNextResLevel[num][j].Length)
						{
							int num4 = this.findDistantChunkInList(distantChunkMapInfo.ChunkEdgeToNextResLevel[num][j][k] + vector2i2, i);
							int num6 = distantChunkMapInfo.ChunkEdgeToNextRLEdgeId[num][j][k];
							if (num4 > 0)
							{
								this.MetaThreadContList.Last<ThreadInfoParam>().BackwardChunkSeamToAdjust[j][num5] = this.ChunkDataList[i].array[num4];
								this.MetaThreadContList.Last<ThreadInfoParam>().BackwardEdgeId[j][num5] = num6;
								num5++;
								this.ChunkDataList[i].array[num4].IsOnSeamCorrectionProcess = true;
							}
							k++;
						}
						this.MetaThreadContList.Last<ThreadInfoParam>().SDLengthBackwardChunkSeamToAdjust[j] = num5;
					}
					this.MetaThreadContList.Last<ThreadInfoParam>().FDLengthBackwardChunkSeamToAdjust = distantChunkMapInfo.ChunkEdgeToNextResLevel[num].Length;
				}
				this.MainChunkMap[this.CurMapId].UpdatePlayerPos(i, num);
			}
		}
	}

	// Token: 0x06005F4D RID: 24397 RVA: 0x0025D3FC File Offset: 0x0025B5FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void threadManagementUpdate()
	{
		int num = 0;
		for (int i = 0; i < this.MetaThreadContList.Count; i++)
		{
			if (this.MetaThreadContList[i].IsThreadDone)
			{
				this.MetaCoroutineContList.Add(this.MetaThreadContList[i]);
				this.IdToRemove[num] = i;
				num++;
			}
		}
		for (int j = num - 1; j >= 0; j--)
		{
			this.MetaThreadContList.RemoveAt(this.IdToRemove[j]);
		}
		if (this.MetaCoroutineContList.Count > 0 && this.MetaCoroutineContList[0].IsCoroutineDone)
		{
			this.ThInfoParamPool.ReturnObject(this.MetaCoroutineContList[0], this.ThContainerPool);
			this.MetaCoroutineContList.RemoveAt(0);
		}
		if (this.MetaCoroutineContList.Count > 0 && this.DebugYieldResLevel < 0 && !this.MetaCoroutineContList[0].IsCoroutineDone)
		{
			this.YieldCoroutine = this.UpdateMetaListCoroutine(this.MetaCoroutineContList[0]);
			ThreadManager.StartCoroutine(this.YieldCoroutine);
		}
		if (this.ThProcessing != null && this.ThProcessing.IsThreadFinished())
		{
			this.ThProcessingPool.ReturnObject(this.ThProcessing);
			this.ThProcessing = null;
		}
		if (this.ThProcessing == null && this.MetaThreadContList.Count != 0)
		{
			this.ThProcessing = this.ThProcessingPool.GetObject(this.MetaThreadContList);
		}
	}

	// Token: 0x06005F4E RID: 24398 RVA: 0x0025D56A File Offset: 0x0025B76A
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsSpwaningProcessOngoing()
	{
		return this.MetaThreadContList.Count != 0 || this.MetaCoroutineContList.Count != 0 || this.ThProcessing != null;
	}

	// Token: 0x06005F4F RID: 24399 RVA: 0x0025D594 File Offset: 0x0025B794
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateCurrentPosOnMapAsync(int _mapId, int _chunkDataListBGId)
	{
		Vector2i vector2i = default(Vector2i);
		Vector2i vector2i2 = default(Vector2i);
		Vector2 vector = default(Vector2);
		DistantChunkMap distantChunkMap = this.MainChunkMap[_mapId];
		vector = this.CurrentPlayerPosVec - distantChunkMap.TerrainOrigin;
		for (int i = 0; i < distantChunkMap.NbResLevel; i++)
		{
			int num = distantChunkMap.FindOutsideFromChunk(vector, i);
			if (num >= 0)
			{
				DistantChunkMapInfo distantChunkMapInfo = distantChunkMap.ChunkMapInfoArray[i];
				int num2 = i;
				DistantChunkMapInfo distantChunkMapInfo2;
				int num3;
				if (i < distantChunkMap.NbResLevel - 1)
				{
					distantChunkMapInfo2 = distantChunkMap.ChunkMapInfoArray[i + 1];
					num3 = i + 1;
				}
				else
				{
					distantChunkMapInfo2 = distantChunkMap.ChunkMapInfoArray[i];
					num3 = i;
				}
				vector2i.Set(Convert.ToInt32(distantChunkMapInfo.ShiftVec.x / distantChunkMapInfo2.ChunkWidth), Convert.ToInt32(distantChunkMapInfo.ShiftVec.y / distantChunkMapInfo2.ChunkWidth));
				vector2i2.Set(Convert.ToInt32(distantChunkMapInfo.ShiftVec.x / distantChunkMapInfo.ChunkWidth), Convert.ToInt32(distantChunkMapInfo.ShiftVec.y / distantChunkMapInfo.ChunkWidth));
				if (i < distantChunkMap.NbResLevel - 1)
				{
					for (int j = 0; j < distantChunkMapInfo.ChunkToDelete[num].Length; j++)
					{
						int num4 = this.findDistantChunkInList(distantChunkMapInfo.ChunkToDelete[num][j] + vector2i, i + 1, this.ChunkDataListBackGround[_chunkDataListBGId]);
						if (num4 > 0)
						{
							this.putDistantChunkOnStack(i + 1, num4, this.ChunkDataListBackGround[_chunkDataListBGId]);
						}
					}
				}
				for (int k = 0; k < distantChunkMapInfo.ChunkToConvDel[num].Length; k++)
				{
					int num4 = this.findDistantChunkInList(distantChunkMapInfo.ChunkToConvDel[num][k] + vector2i2, i, this.ChunkDataListBackGround[_chunkDataListBGId]);
					if (num4 > 0)
					{
						this.putDistantChunkOnStack(i, num4, this.ChunkDataListBackGround[_chunkDataListBGId]);
					}
				}
				for (int l = 0; l < distantChunkMapInfo.ChunkToAdd[num].Length; l++)
				{
					this.EdgeResFactor[0] = distantChunkMapInfo.ChunkToAddEdgeFactor[num][l].x;
					this.EdgeResFactor[1] = distantChunkMapInfo.ChunkToAddEdgeFactor[num][l].y;
					this.EdgeResFactor[2] = distantChunkMapInfo.ChunkToAddEdgeFactor[num][l].z;
					this.EdgeResFactor[3] = distantChunkMapInfo.ChunkToAddEdgeFactor[num][l].w;
					this.GetDistantChunkFromStack(distantChunkMap, num2, distantChunkMapInfo.ChunkToAdd[num][l] + vector2i2, distantChunkMap.TerrainOrigin, this.EdgeResFactor, this.WaterPlaneGameObjStack, this.ChunkDataListBackGround[_chunkDataListBGId]);
				}
				if (i < distantChunkMap.NbResLevel - 1)
				{
					for (int m = 0; m < distantChunkMapInfo.ChunkToConvAdd[num].Length; m++)
					{
						this.EdgeResFactor[0] = distantChunkMapInfo.ChunkToConvAddEdgeFactor[num][m].x;
						this.EdgeResFactor[1] = distantChunkMapInfo.ChunkToConvAddEdgeFactor[num][m].y;
						this.EdgeResFactor[2] = distantChunkMapInfo.ChunkToConvAddEdgeFactor[num][m].z;
						this.EdgeResFactor[3] = distantChunkMapInfo.ChunkToConvAddEdgeFactor[num][m].w;
						this.GetDistantChunkFromStack(distantChunkMap, num3, distantChunkMapInfo.ChunkToConvAdd[num][m] + vector2i, distantChunkMap.TerrainOrigin, this.EdgeResFactor, this.WaterPlaneGameObjStack, this.ChunkDataListBackGround[_chunkDataListBGId]);
					}
				}
				distantChunkMap.UpdatePlayerPos(i, num);
			}
		}
	}

	// Token: 0x06005F50 RID: 24400 RVA: 0x0025D948 File Offset: 0x0025BB48
	public void ThreadExtraWork(DistantChunk DChunk, DistantChunkBasicMesh BMesh, bool WasReset)
	{
		if (!DChunk.IsOnActivationProcess)
		{
			return;
		}
		DChunk.ActivateObject(WasReset);
	}

	// Token: 0x06005F51 RID: 24401 RVA: 0x0025D95A File Offset: 0x0025BB5A
	public void MainExtraWork(DistantChunk DChunk, DistantChunkBasicMesh BMesh)
	{
		if (!DChunk.IsOnActivationProcess)
		{
			return;
		}
		DChunk.ActivateUnityMeshGameObject(BMesh);
	}

	// Token: 0x06005F52 RID: 24402 RVA: 0x0025D96C File Offset: 0x0025BB6C
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator UpdateMetaListCoroutine(ThreadInfoParam threadInfoParam)
	{
		this.DebugYieldResLevel = threadInfoParam.ResLevel;
		DistantChunkMapInfo distantChunkMapInfo = this.MainChunkMap[this.CurMapId].ChunkMapInfoArray[threadInfoParam.ResLevel];
		int NbChunkAddBackward = this.MainChunkMap[this.CurMapId].ChunkMapInfoArray[threadInfoParam.ResLevel].ChunkToConvAdd[threadInfoParam.OutId].Length;
		int NbChunkIONLC = distantChunkMapInfo.NbCurChunkInOneNextLevelChunk;
		int CorrectedNbChunkToBeUpdated = 15 + NbChunkIONLC;
		CorrectedNbChunkToBeUpdated -= CorrectedNbChunkToBeUpdated % NbChunkIONLC;
		while (threadInfoParam.LengthThreadContList - threadInfoParam.CntThreadContList > 0 && threadInfoParam != null)
		{
			int num = threadInfoParam.LengthThreadContList - threadInfoParam.CntThreadContList;
			int num2 = ((num > CorrectedNbChunkToBeUpdated) ? CorrectedNbChunkToBeUpdated : num);
			int num3 = 0;
			for (int i = 0; i < num2; i++)
			{
				if (threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList].DChunk != null && threadInfoParam.CntThreadContList < threadInfoParam.LengthThreadContList)
				{
					threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList].MainExtraWork();
					if (threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList].DChunk.IsOnActivationProcess)
					{
						threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList].DChunk.IsMeshUpdated = true;
						threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList].DChunk.IsOnActivationProcess = false;
					}
					else
					{
						threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList].DChunk.IsMeshUpdated = false;
						if (threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList].DChunk.CellObj != null)
						{
							threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList].DChunk.CellObj.SetActive(false);
						}
					}
					threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList].DChunk.IsFreeToUse = true;
					this.ThContainerPool.ReturnObject(threadInfoParam.ThreadContListA[threadInfoParam.CntThreadContList], !threadInfoParam.IsAsynchronous);
					threadInfoParam.CntThreadContList++;
					num3++;
				}
			}
			int num4 = threadInfoParam.LengthThreadContList - threadInfoParam.CntThreadContList;
			if (!threadInfoParam.IsAsynchronous)
			{
				if (num4 >= NbChunkAddBackward)
				{
					int num5 = num2 / NbChunkIONLC;
					num5 = ((num5 > threadInfoParam.LengthForwardChunkToDeleteId - threadInfoParam.CntForwardChunkToDeleteId) ? (threadInfoParam.LengthForwardChunkToDeleteId - threadInfoParam.CntForwardChunkToDeleteId) : num5);
					for (int j = 0; j < num5; j++)
					{
						this.putDistantChunkOnStack(threadInfoParam.ForwardChunkToDeleteIdA[threadInfoParam.CntForwardChunkToDeleteId], true);
						threadInfoParam.CntForwardChunkToDeleteId++;
					}
				}
				else if (num4 + num3 <= NbChunkAddBackward)
				{
					int num6 = Mathf.Min(num2 * NbChunkIONLC, threadInfoParam.LengthBackwardChunkToDeleteId - threadInfoParam.CntBackwardChunkToDeleteId);
					for (int k = 0; k < num6; k++)
					{
						this.putDistantChunkOnStack(threadInfoParam.BackwardChunkToDeleteIdA[threadInfoParam.CntBackwardChunkToDeleteId], true);
						threadInfoParam.CntBackwardChunkToDeleteId++;
					}
				}
				else
				{
					int num7 = (num4 + num2 - NbChunkAddBackward) / NbChunkIONLC;
					for (int l = 0; l < num7; l++)
					{
						this.putDistantChunkOnStack(threadInfoParam.ForwardChunkToDeleteIdA[threadInfoParam.CntForwardChunkToDeleteId], true);
						threadInfoParam.CntForwardChunkToDeleteId++;
					}
					num7 = (num2 - (num4 + num2 - NbChunkAddBackward)) * NbChunkIONLC;
					if (num7 > threadInfoParam.LengthBackwardChunkToDeleteId - threadInfoParam.CntBackwardChunkToDeleteId)
					{
						num7 = threadInfoParam.LengthBackwardChunkToDeleteId - threadInfoParam.CntBackwardChunkToDeleteId;
					}
					for (int m = 0; m < num7; m++)
					{
						this.putDistantChunkOnStack(threadInfoParam.BackwardChunkToDeleteIdA[threadInfoParam.CntBackwardChunkToDeleteId], true);
						threadInfoParam.CntBackwardChunkToDeleteId++;
					}
				}
			}
			if (!threadInfoParam.IsAsynchronous && threadInfoParam.FDLengthForwardChunkSeamToAdjust - threadInfoParam.CntForwardChunkSeamToAdjust > 0 && threadInfoParam.ResLevel < this.NbResLevel - 1)
			{
				int num8;
				if (num4 >= NbChunkAddBackward)
				{
					num8 = CorrectedNbChunkToBeUpdated / NbChunkIONLC;
				}
				else if (num4 + num2 <= NbChunkAddBackward)
				{
					num8 = 0;
				}
				else
				{
					num8 = (num4 + num2 - NbChunkAddBackward) / NbChunkIONLC;
				}
				for (int n = 0; n < num8; n++)
				{
					if (threadInfoParam.CntForwardChunkSeamToAdjust < threadInfoParam.FDLengthForwardChunkSeamToAdjust)
					{
						for (int num9 = 0; num9 < threadInfoParam.SDLengthForwardChunkSeamToAdjust[threadInfoParam.CntForwardChunkSeamToAdjust]; num9++)
						{
							if (threadInfoParam.ForwardChunkSeamToAdjust[threadInfoParam.CntForwardChunkSeamToAdjust][num9] != null)
							{
								if (threadInfoParam.ForwardEdgeId[threadInfoParam.CntForwardChunkSeamToAdjust][num9] > 3)
								{
									threadInfoParam.ForwardChunkSeamToAdjust[threadInfoParam.CntForwardChunkSeamToAdjust][num9].ResetEdgeToOwnResLevel(threadInfoParam.ForwardEdgeId[threadInfoParam.CntForwardChunkSeamToAdjust][num9] / 10);
									threadInfoParam.ForwardChunkSeamToAdjust[threadInfoParam.CntForwardChunkSeamToAdjust][num9].ResetEdgeToOwnResLevel(threadInfoParam.ForwardEdgeId[threadInfoParam.CntForwardChunkSeamToAdjust][num9] % 10);
								}
								else
								{
									threadInfoParam.ForwardChunkSeamToAdjust[threadInfoParam.CntForwardChunkSeamToAdjust][num9].ResetEdgeToOwnResLevel(threadInfoParam.ForwardEdgeId[threadInfoParam.CntForwardChunkSeamToAdjust][num9]);
								}
								threadInfoParam.ForwardChunkSeamToAdjust[threadInfoParam.CntForwardChunkSeamToAdjust][num9].IsOnSeamCorrectionProcess = false;
							}
						}
						threadInfoParam.CntForwardChunkSeamToAdjust++;
					}
				}
			}
			if (!threadInfoParam.IsAsynchronous && threadInfoParam.FDLengthBackwardChunkSeamToAdjust > 0 && threadInfoParam.ResLevel < this.NbResLevel - 1)
			{
				int num10;
				if (num4 >= NbChunkAddBackward)
				{
					num10 = 0;
				}
				else if (num4 + num2 <= NbChunkAddBackward)
				{
					num10 = num2;
				}
				else
				{
					num10 = NbChunkAddBackward - num4;
				}
				num10 = ((num10 > threadInfoParam.FDLengthBackwardChunkSeamToAdjust - threadInfoParam.CntBackwardChunkSeamToAdjust) ? (threadInfoParam.FDLengthBackwardChunkSeamToAdjust - threadInfoParam.CntBackwardChunkSeamToAdjust) : num10);
				for (int num11 = 0; num11 < num10; num11++)
				{
					if (threadInfoParam.CntBackwardChunkSeamToAdjust < threadInfoParam.FDLengthBackwardChunkSeamToAdjust)
					{
						for (int num12 = 0; num12 < threadInfoParam.SDLengthBackwardChunkSeamToAdjust[num11]; num12++)
						{
							if (threadInfoParam.BackwardChunkSeamToAdjust[threadInfoParam.CntBackwardChunkSeamToAdjust][num12] != null)
							{
								if (threadInfoParam.BackwardEdgeId[threadInfoParam.CntBackwardChunkSeamToAdjust][num12] > 3)
								{
									threadInfoParam.BackwardChunkSeamToAdjust[threadInfoParam.CntBackwardChunkSeamToAdjust][num12].ResetEdgeToNextResLevel(threadInfoParam.BackwardEdgeId[threadInfoParam.CntBackwardChunkSeamToAdjust][num12] / 10);
									threadInfoParam.BackwardChunkSeamToAdjust[threadInfoParam.CntBackwardChunkSeamToAdjust][num12].ResetEdgeToNextResLevel(threadInfoParam.BackwardEdgeId[threadInfoParam.CntBackwardChunkSeamToAdjust][num12] % 10);
								}
								else
								{
									threadInfoParam.BackwardChunkSeamToAdjust[threadInfoParam.CntBackwardChunkSeamToAdjust][num12].ResetEdgeToNextResLevel(threadInfoParam.BackwardEdgeId[threadInfoParam.CntBackwardChunkSeamToAdjust][num12]);
								}
								threadInfoParam.BackwardChunkSeamToAdjust[threadInfoParam.CntBackwardChunkSeamToAdjust][num12].IsOnSeamCorrectionProcess = false;
							}
						}
						threadInfoParam.CntBackwardChunkSeamToAdjust++;
					}
				}
			}
			yield return null;
		}
		this.DebugYieldResLevel = -1;
		threadInfoParam.IsCoroutineDone = true;
		yield break;
	}

	// Token: 0x06005F53 RID: 24403 RVA: 0x0025D984 File Offset: 0x0025BB84
	[PublicizedFrom(EAccessModifier.Private)]
	public int findDistantChunkInList(long _CellKey, int _ResLevel)
	{
		int num = 0;
		while (num < this.ChunkDataList[_ResLevel].Count && _CellKey != this.ChunkDataList[_ResLevel].array[num].CellKey)
		{
			num++;
		}
		if (num < this.ChunkDataList[_ResLevel].Count)
		{
			return num;
		}
		return -1;
	}

	// Token: 0x06005F54 RID: 24404 RVA: 0x0025D9D4 File Offset: 0x0025BBD4
	[PublicizedFrom(EAccessModifier.Private)]
	public int findDistantChunkInList(Vector2i _IntPosVec, int _ResLevel)
	{
		long cellKeyIdFromIdVector = DistantChunk.GetCellKeyIdFromIdVector(_IntPosVec);
		int num = 0;
		while (num < this.ChunkDataList[_ResLevel].Count && cellKeyIdFromIdVector != this.ChunkDataList[_ResLevel].array[num].CellKey)
		{
			num++;
		}
		if (num < this.ChunkDataList[_ResLevel].Count)
		{
			return num;
		}
		return -1;
	}

	// Token: 0x06005F55 RID: 24405 RVA: 0x0025DA2C File Offset: 0x0025BC2C
	[PublicizedFrom(EAccessModifier.Private)]
	public int findDistantChunkInList(Vector2i _IntPosVec, int _ResLevel, OptimizedList<DistantChunk>[] CurChunkDataList)
	{
		long cellKeyIdFromIdVector = DistantChunk.GetCellKeyIdFromIdVector(_IntPosVec);
		int num = 0;
		while (num < CurChunkDataList[_ResLevel].Count && cellKeyIdFromIdVector != CurChunkDataList[_ResLevel].array[num].CellKey)
		{
			num++;
		}
		if (num < CurChunkDataList[_ResLevel].Count)
		{
			return num;
		}
		return -1;
	}

	// Token: 0x06005F56 RID: 24406 RVA: 0x0025DA74 File Offset: 0x0025BC74
	public bool ActivateChunk(int _chunkX, int _chunkZ, bool IsActive)
	{
		int num = _chunkX - this.TerrainOriginIntCoor.x;
		int num2 = _chunkZ - this.TerrainOriginIntCoor.y;
		Vector2i vector2i = new Vector2i(num, num2);
		Vector2i vector2i2 = new Vector2i(_chunkX, _chunkZ);
		DistantChunk distantChunk;
		if (this.IsOnMapSyncProcess)
		{
			int num3;
			if ((num3 = this.findDistantChunkInList(vector2i, 0, this.ChunkDataListBackGround[0])) < 0)
			{
				this.UpdateChunkActivationCache(vector2i2, IsActive, false);
				return false;
			}
			distantChunk = this.ChunkDataListBackGround[0][0].array[num3];
		}
		else
		{
			int num3;
			if ((num3 = this.findDistantChunkInList(vector2i, 0)) < 0)
			{
				this.UpdateChunkActivationCache(vector2i2, IsActive, false);
				return false;
			}
			distantChunk = this.ChunkDataList[0].array[num3];
		}
		this.UpdateChunkActivationCache(vector2i2, IsActive, true);
		distantChunk.IsChunkActivated = IsActive;
		if (distantChunk.CellObj != null)
		{
			distantChunk.CellObj.SetActive(IsActive);
			return true;
		}
		return true;
	}

	// Token: 0x06005F57 RID: 24407 RVA: 0x0025DB50 File Offset: 0x0025BD50
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateChunkActivationCache(Vector2i LocPosId, bool IsActive, bool IsChunkFound)
	{
		int hashCode = LocPosId.GetHashCode();
		DistantTerrain.ChunkStateHelper chunkStateHelper;
		if (this.ChunkActivationCacheDic.TryGetValue(hashCode, out chunkStateHelper))
		{
			chunkStateHelper.IsActive = IsActive;
			return;
		}
		this.ChunkActivationCacheDic.Add(hashCode, new DistantTerrain.ChunkStateHelper(LocPosId.x, LocPosId.y, IsActive));
	}

	// Token: 0x06005F58 RID: 24408 RVA: 0x0025DBA4 File Offset: 0x0025BDA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ProcessChunkActivationCache(Vector3 PlayerPos)
	{
		DistantChunkMapInfo distantChunkMapInfo = this.MainChunkMap[this.CurMapId].ChunkMapInfoArray[0];
		int num = -distantChunkMapInfo.LLIntArea.x;
		new Vector2(distantChunkMapInfo.ShiftVec.x + (float)(distantChunkMapInfo.LLIntArea.x + this.TerrainOriginIntCoor.x) * distantChunkMapInfo.ChunkWidth, distantChunkMapInfo.ShiftVec.y + (float)(distantChunkMapInfo.LLIntArea.y + this.TerrainOriginIntCoor.y) * distantChunkMapInfo.ChunkWidth) - Vector2.one * (float)distantChunkMapInfo.LLIntArea.x * 2f * distantChunkMapInfo.ChunkWidth;
		int num2 = (int)distantChunkMapInfo.ChunkWidth / 16;
		Vector2 vector = this.MainChunkMap[this.CurMapId].TerrainOrigin * (float)num2;
		Vector2i zero = Vector2i.zero;
		int num3 = ((int)distantChunkMapInfo.ShiftVec.x >> 4) + distantChunkMapInfo.LLIntArea.x * num2 + (int)vector.x;
		int num4 = ((int)distantChunkMapInfo.ShiftVec.y >> 4) + distantChunkMapInfo.LLIntArea.y * num2 + (int)vector.y;
		int num5 = num * 2 * num2;
		int num6 = ((int)PlayerPos.x >> 4) - num - 1;
		int num7 = ((int)PlayerPos.z >> 4) - num - 1;
		this.ChunkToActivateList.Clear();
		OptimizedList<int> optimizedList = new OptimizedList<int>();
		foreach (KeyValuePair<int, DistantTerrain.ChunkStateHelper> keyValuePair in this.ChunkActivationCacheDic)
		{
			int num8 = keyValuePair.Value.PosX - num3;
			int num9 = keyValuePair.Value.PosZ - num4;
			int num10 = keyValuePair.Value.PosX - num6;
			int num11 = keyValuePair.Value.PosZ - num7;
			bool flag = false;
			if (num10 < 0 || num10 > num5 + 2 || (num11 < 0 && num11 > num5 + 2))
			{
				optimizedList.Add(keyValuePair.Key);
				flag = true;
			}
			if (!flag && num8 >= 0 && num8 < num5 && num9 >= 0 && num9 < num5)
			{
				this.ChunkToActivateList.Add(keyValuePair.Value);
			}
		}
		for (int i = 0; i < optimizedList.Count; i++)
		{
			this.ChunkActivationCacheDic.Remove(optimizedList.array[i]);
		}
		for (int j = 0; j < this.ChunkToActivateList.Count; j++)
		{
			DistantTerrain.ChunkStateHelper chunkStateHelper = this.ChunkToActivateList.array[j];
			this.ActivateChunk(chunkStateHelper.PosX, chunkStateHelper.PosZ, chunkStateHelper.IsActive);
		}
		if (!this.IsSpwaningProcessOngoing() && this.IsPlayerPosCacheEmpty())
		{
			this.ChunkActivationCacheDic.Clear();
		}
	}

	// Token: 0x06005F59 RID: 24409 RVA: 0x0025DE80 File Offset: 0x0025C080
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsPlayerPosCacheEmpty()
	{
		return this.PlPosCache.Count == 0;
	}

	// Token: 0x04004AB6 RID: 19126
	[PublicizedFrom(EAccessModifier.Private)]
	public const int NbChunkToBeUpdated = 15;

	// Token: 0x04004AB7 RID: 19127
	[PublicizedFrom(EAccessModifier.Private)]
	public const int DT_ViewDistance = 2000;

	// Token: 0x04004AB8 RID: 19128
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxNbDChunkOnAsyncUpdate = 50;

	// Token: 0x04004AB9 RID: 19129
	public OptimizedList<DistantChunk>[] ChunkDataList;

	// Token: 0x04004ABA RID: 19130
	public List<OptimizedList<DistantChunk>[]> ChunkDataListBackGround;

	// Token: 0x04004ABB RID: 19131
	[PublicizedFrom(EAccessModifier.Private)]
	public DistantChunk[] ChunkDataProxyArray;

	// Token: 0x04004ABC RID: 19132
	[PublicizedFrom(EAccessModifier.Private)]
	public UtilList<DistantTerrain.PlayerPosHelper> PlPosCache;

	// Token: 0x04004ABD RID: 19133
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<int, DistantTerrain.ChunkStateHelper> ChunkActivationCacheDic;

	// Token: 0x04004ABE RID: 19134
	[PublicizedFrom(EAccessModifier.Private)]
	public OptimizedList<DistantTerrain.ChunkStateHelper> ChunkToActivateList;

	// Token: 0x04004ABF RID: 19135
	[PublicizedFrom(EAccessModifier.Private)]
	public int ProcessCacheCnt;

	// Token: 0x04004AC0 RID: 19136
	public List<ThreadInfoParam> MetaThreadContList;

	// Token: 0x04004AC1 RID: 19137
	public List<ThreadInfoParam> MetaCoroutineContList;

	// Token: 0x04004AC2 RID: 19138
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadProcessing ThProcessing;

	// Token: 0x04004AC3 RID: 19139
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsOnMapSyncProcess;

	// Token: 0x04004AC4 RID: 19140
	public bool IsTerrainReady;

	// Token: 0x04004AC5 RID: 19141
	[PublicizedFrom(EAccessModifier.Private)]
	public DistantChunkBasicMesh[] BaseMesh;

	// Token: 0x04004AC6 RID: 19142
	[PublicizedFrom(EAccessModifier.Private)]
	public DistantChunkMap[] MainChunkMap;

	// Token: 0x04004AC7 RID: 19143
	[PublicizedFrom(EAccessModifier.Private)]
	public int CurMapId;

	// Token: 0x04004AC8 RID: 19144
	[PublicizedFrom(EAccessModifier.Private)]
	public int ChunkBackGroundMapId;

	// Token: 0x04004AC9 RID: 19145
	[PublicizedFrom(EAccessModifier.Private)]
	public int NbResLevel;

	// Token: 0x04004ACA RID: 19146
	[PublicizedFrom(EAccessModifier.Private)]
	public int MaxNbResLevel;

	// Token: 0x04004ACB RID: 19147
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWorldSizeX = 20000;

	// Token: 0x04004ACC RID: 19148
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cWorldSizeZ = 20000;

	// Token: 0x04004ACD RID: 19149
	public Vector2 CurrentPlayerPosVec;

	// Token: 0x04004ACE RID: 19150
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 CurrentPlayerExtendedPosVec;

	// Token: 0x04004ACF RID: 19151
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i TerrainOriginIntCoor;

	// Token: 0x04004AD0 RID: 19152
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] EdgeResFactor;

	// Token: 0x04004AD1 RID: 19153
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator YieldCoroutine;

	// Token: 0x04004AD2 RID: 19154
	[PublicizedFrom(EAccessModifier.Private)]
	public OptimizedList<DistantChunk>[] DistantChunkStack;

	// Token: 0x04004AD3 RID: 19155
	[PublicizedFrom(EAccessModifier.Private)]
	public Stack<GameObject>[] WaterPlaneGameObjStack;

	// Token: 0x04004AD4 RID: 19156
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadInfoParamPool ThInfoParamPool;

	// Token: 0x04004AD5 RID: 19157
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadProcessingPool ThProcessingPool;

	// Token: 0x04004AD6 RID: 19158
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadContainerPool ThContainerPool;

	// Token: 0x04004AD7 RID: 19159
	public int DebugYieldResLevel = -1;

	// Token: 0x04004AD8 RID: 19160
	[PublicizedFrom(EAccessModifier.Private)]
	public DistantTerrainQuadTree TerExQuadTree;

	// Token: 0x04004AD9 RID: 19161
	public static Vector3 cShiftHiResChunks;

	// Token: 0x04004ADA RID: 19162
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 cShiftMidResChunks;

	// Token: 0x04004ADB RID: 19163
	[PublicizedFrom(EAccessModifier.Private)]
	public static Vector3 cShiftLowResChunks;

	// Token: 0x04004ADC RID: 19164
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goDistantTerrain;

	// Token: 0x04004ADD RID: 19165
	public static DistantTerrain Instance;

	// Token: 0x04004ADE RID: 19166
	[PublicizedFrom(EAccessModifier.Private)]
	public int[] IdToRemove = new int[20];

	// Token: 0x02000C4C RID: 3148
	public class PlayerPosHelper
	{
		// Token: 0x06005F5B RID: 24411 RVA: 0x0025DEAC File Offset: 0x0025C0AC
		public PlayerPosHelper(Vector3 _PlayerPos)
		{
			this.PlayerPos = _PlayerPos;
		}

		// Token: 0x04004ADF RID: 19167
		public Vector3 PlayerPos;
	}

	// Token: 0x02000C4D RID: 3149
	public class ChunkStateHelper
	{
		// Token: 0x06005F5C RID: 24412 RVA: 0x0025DEBB File Offset: 0x0025C0BB
		public ChunkStateHelper(int _PosX, int _PosZ, bool _IsActive)
		{
			this.PosX = _PosX;
			this.PosZ = _PosZ;
			this.IsActive = _IsActive;
		}

		// Token: 0x04004AE0 RID: 19168
		public bool IsActive;

		// Token: 0x04004AE1 RID: 19169
		public int PosX;

		// Token: 0x04004AE2 RID: 19170
		public int PosZ;
	}
}
