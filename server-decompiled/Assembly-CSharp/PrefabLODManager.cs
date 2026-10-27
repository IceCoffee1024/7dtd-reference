using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

// Token: 0x02000962 RID: 2402
public class PrefabLODManager
{
	// Token: 0x060045D3 RID: 17875 RVA: 0x001BC2B2 File Offset: 0x001BA4B2
	public void TriggerUpdate()
	{
		this.lastTime = 0f;
		this.lastDisplayUpdate = 0f;
	}

	// Token: 0x060045D4 RID: 17876 RVA: 0x001BC2CC File Offset: 0x001BA4CC
	public void FrameUpdate()
	{
		try
		{
			if (!GameManager.IsDedicatedServer)
			{
				float time = Time.time;
				if (time - this.lastDisplayUpdate >= 1f)
				{
					this.lastDisplayUpdate = time;
					World world = GameManager.Instance.World;
					if (!world.ChunkCache.IsFixedSize)
					{
						List<EntityPlayerLocal> localPlayers = world.GetLocalPlayers();
						if (localPlayers.Count != 0)
						{
							EntityPlayerLocal entityPlayerLocal = localPlayers[0];
							if (!(entityPlayerLocal == null))
							{
								this.UpdateDisplay(entityPlayerLocal);
								if (time - this.lastTime >= 1f)
								{
									this.lastTime = time;
									DynamicPrefabDecorator dynamicPrefabDecorator = world.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
									if (dynamicPrefabDecorator != null)
									{
										for (int i = 0; i < world.Players.list.Count; i++)
										{
											this.prefabsAroundFar.Clear();
											this.prefabsAroundNear.Clear();
											EntityPlayer entityPlayer = world.Players.list[i];
											if (!(entityPlayer == null))
											{
												Vector3 position = entityPlayer.position;
												int num = ((entityPlayer.ChunkObserver != null) ? entityPlayer.ChunkObserver.viewDim : GamePrefs.GetInt(EnumGamePrefs.OptionsGfxViewDistance));
												num = (num - 1) * 16;
												if (!entityPlayer.isEntityRemote)
												{
													dynamicPrefabDecorator.GetPrefabsAround(position, (float)num, (float)PrefabLODManager.lodPoiDistance, this.prefabsAroundFar, this.prefabsAroundNear);
													this.UpdatePrefabsAround(this.prefabsAroundFar, this.prefabsAroundNear);
												}
												else if ((entityPlayer.position - entityPlayerLocal.position).sqrMagnitude <= (float)(2 * num * (2 * num)))
												{
													dynamicPrefabDecorator.GetPrefabsAround(position, (float)num, (float)PrefabLODManager.lodPoiDistance, this.prefabsAroundFar, this.prefabsAroundNear);
													entityPlayer.SetPrefabsAroundNear(this.prefabsAroundNear);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x060045D5 RID: 17877 RVA: 0x001BC4C4 File Offset: 0x001BA6C4
	public PrefabLODManager.PrefabGameObject GetInstance(int id)
	{
		PrefabLODManager.PrefabGameObject prefabGameObject;
		this.displayedPrefabs.TryGetValue(id, out prefabGameObject);
		return prefabGameObject;
	}

	// Token: 0x060045D6 RID: 17878 RVA: 0x001BC4E4 File Offset: 0x001BA6E4
	public void UpdatePrefabsAround(Dictionary<int, PrefabInstance> _prefabsAroundFar, Dictionary<int, PrefabInstance> _prefabsAroundNear)
	{
		try
		{
			if (!GameManager.IsDedicatedServer)
			{
				List<EntityPlayerLocal> localPlayers = GameManager.Instance.World.GetLocalPlayers();
				if (localPlayers.Count != 0)
				{
					localPlayers[0].SetPrefabsAroundNear(_prefabsAroundNear);
					foreach (KeyValuePair<int, PrefabInstance> keyValuePair in _prefabsAroundFar)
					{
						PrefabInstance value = keyValuePair.Value;
						if (!this.displayedPrefabs.ContainsKey(value.id))
						{
							PathAbstractions.AbstractedLocation imposterLocation = value.GetImposterLocation();
							if (imposterLocation.Type != PathAbstractions.EAbstractedLocationType.None)
							{
								string fullPath = imposterLocation.FullPath;
								PrefabLODManager.PrefabGameObject prefabGameObject = new PrefabLODManager.PrefabGameObject();
								prefabGameObject.prefabInstance = value;
								prefabGameObject.meshPath = imposterLocation.FullPath;
								this.displayedPrefabs.Add(value.id, prefabGameObject);
								if (!this.meshPathToData.ContainsKey(fullPath))
								{
									this.meshPathToData.Add(fullPath, new PrefabLODManager.MeshPrefabSet());
								}
								this.meshPathToData[fullPath].prefabIDs.Add(value.id);
							}
						}
					}
					List<int> list = new List<int>();
					foreach (KeyValuePair<int, PrefabLODManager.PrefabGameObject> keyValuePair2 in this.displayedPrefabs)
					{
						int key = keyValuePair2.Key;
						if (!_prefabsAroundFar.ContainsKey(key))
						{
							list.Add(key);
						}
					}
					if (list.Count > 0)
					{
						this.removePrefabs(list);
					}
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x060045D7 RID: 17879 RVA: 0x001BC6B8 File Offset: 0x001BA8B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void removePrefabs(List<int> toRemove)
	{
		try
		{
			foreach (int num in toRemove)
			{
				string meshPath = this.displayedPrefabs[num].meshPath;
				GameObject go = this.displayedPrefabs[num].go;
				if (go)
				{
					PrefabLODManager.MeshPrefabSet meshPrefabSet = this.meshPathToData[meshPath];
					meshPrefabSet.prefabIDs.Remove(num);
					if (meshPrefabSet.prefabIDs.Count == 0)
					{
						Mesh[] meshes = meshPrefabSet.meshInfo.meshes;
						for (int i = 0; i < meshes.Length; i++)
						{
							global::UnityEngine.Object.Destroy(meshes[i]);
						}
						this.meshPathToData.Remove(meshPath);
					}
					if (this.displayedPrefabs[num].signDatas != null)
					{
						foreach (ImposterCanvas imposterCanvas in this.displayedPrefabs[num].signDatas)
						{
							ImposterSignFactory.Destroy(imposterCanvas);
						}
					}
					this.displayedPrefabs.Remove(num);
					global::UnityEngine.Object.Destroy(go);
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x060045D8 RID: 17880 RVA: 0x001BC838 File Offset: 0x001BAA38
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildGameObjectFromMeshInfo(SimpleMeshInfo _meshInfo, PrefabLODManager.PrefabGameObject _pgo)
	{
		try
		{
			_pgo.go = SimpleMeshFile.CreateUnityObjects(_meshInfo);
			GameObject go = _pgo.go;
			PrefabInstance prefabInstance = _pgo.prefabInstance;
			if (!go)
			{
				Log.Error("Loading LOD mesh for Prefab " + _pgo.prefabInstance.name + " failed.");
			}
			else
			{
				go.name = prefabInstance.location.Name;
				Transform transform = go.transform;
				transform.SetParent(this.parentTransform, false);
				Vector3 vector = prefabInstance.boundingBoxPosition.ToVector3();
				vector += new Vector3((float)prefabInstance.boundingBoxSize.x * 0.5f, -4f, (float)prefabInstance.boundingBoxSize.z * 0.5f);
				Vector3 zero = Vector3.zero;
				Quaternion quaternion = Quaternion.identity;
				switch (prefabInstance.rotation)
				{
				case 0:
					zero = new Vector3(-0.5f, 0f, -0.5f);
					break;
				case 1:
					zero = new Vector3(0.5f, 0f, -0.5f);
					quaternion = Quaternion.Euler(0f, 270f, 0f);
					break;
				case 2:
					zero = new Vector3(0.5f, 0f, 0.5f);
					quaternion = Quaternion.Euler(0f, 180f, 0f);
					break;
				case 3:
					zero = new Vector3(-0.5f, 0f, 0.5f);
					quaternion = Quaternion.Euler(0f, 90f, 0f);
					break;
				}
				if (Utils.FastAbs(vector.x - (float)((int)vector.x)) > 0.001f)
				{
					vector.x += zero.x;
				}
				if (Utils.FastAbs(vector.z - (float)((int)vector.z)) > 0.001f)
				{
					vector.z += zero.z;
				}
				float num;
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					num = prefabInstance.prefab.distantPOIYOffset;
				}
				else
				{
					num = prefabInstance.yOffsetOfPrefab;
				}
				vector.y += num;
				transform.SetPositionAndRotation(vector - Origin.position, quaternion);
				for (int i = 0; i < transform.childCount; i++)
				{
					transform.GetChild(i).gameObject.SetActive(false);
				}
				if (OcclusionManager.Instance.cullPrefabs)
				{
					Occludee.Add(go);
				}
				this.LoadImposterSigns(_pgo, go, _meshInfo);
			}
		}
		finally
		{
		}
	}

	// Token: 0x060045D9 RID: 17881 RVA: 0x001BCAC4 File Offset: 0x001BACC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void LoadImposterSigns(PrefabLODManager.PrefabGameObject _pgo, GameObject _go, SimpleMeshInfo _meshInfo)
	{
		if (_meshInfo.signs == null || _meshInfo.signs.Count == 0)
		{
			return;
		}
		_pgo.signDatas = new List<ImposterCanvas>();
		foreach (ImposterCanvas imposterCanvas in _meshInfo.signs)
		{
			ImposterCanvas imposterCanvas2 = imposterCanvas.Clone();
			ImposterSignFactory.Create(_go, imposterCanvas2);
			if (imposterCanvas2.Canvas != null)
			{
				_pgo.signDatas.Add(imposterCanvas2);
			}
		}
	}

	// Token: 0x060045DA RID: 17882 RVA: 0x001BCB58 File Offset: 0x001BAD58
	public void meshLoadedCallback(SimpleMeshInfo meshInfo, object userCallbackData)
	{
		try
		{
			string text = (string)userCallbackData;
			PrefabLODManager.MeshPrefabSet meshPrefabSet = this.meshPathToData[text];
			if (meshPrefabSet.meshInfo != null)
			{
				Log.Error("Meshes have already been provided for path " + text);
			}
			else
			{
				meshPrefabSet.meshInfo = meshInfo;
				meshPrefabSet.isLoading = false;
				for (int i = 0; i < meshPrefabSet.prefabIDs.Count; i++)
				{
					PrefabLODManager.PrefabGameObject prefabGameObject = this.displayedPrefabs[meshPrefabSet.prefabIDs[i]];
					this.BuildGameObjectFromMeshInfo(meshInfo, prefabGameObject);
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x060045DB RID: 17883 RVA: 0x001BCBEC File Offset: 0x001BADEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateDisplay(EntityPlayerLocal _localPlayer)
	{
		try
		{
			this.stopWatch.ResetAndRestart();
			if (!this.parentTransform)
			{
				this.parentTransform = new GameObject("PrefabsLOD").transform;
				Origin.Add(this.parentTransform, 0);
				this.sharedMat = MeshDescription.GetOpaqueMaterial();
			}
			bool bTextureArray = MeshDescription.meshes[0].bTextureArray;
			foreach (KeyValuePair<string, PrefabLODManager.MeshPrefabSet> keyValuePair in this.meshPathToData)
			{
				string key = keyValuePair.Key;
				PrefabLODManager.MeshPrefabSet value = keyValuePair.Value;
				if (value.meshInfo != null)
				{
					using (List<int>.Enumerator enumerator2 = value.prefabIDs.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int num = enumerator2.Current;
							PrefabLODManager.PrefabGameObject prefabGameObject = this.displayedPrefabs[num];
							if (prefabGameObject.go == null)
							{
								this.BuildGameObjectFromMeshInfo(value.meshInfo, prefabGameObject);
							}
						}
						continue;
					}
				}
				if (!value.isLoading)
				{
					value.isLoading = true;
					string text = key;
					float num2 = 0f;
					Material material = this.sharedMat;
					bool flag = bTextureArray;
					bool flag2 = true;
					SimpleMeshFile.GameObjectMeshesReadCallback gameObjectMeshesReadCallback = new SimpleMeshFile.GameObjectMeshesReadCallback(this.meshLoadedCallback);
					SimpleMeshFile.ReadMesh(text, num2, material, flag, flag2, key, gameObjectMeshesReadCallback);
					if (this.stopWatch.ElapsedMicroseconds > 900L)
					{
						this.lastDisplayUpdate = 0f;
						return;
					}
				}
			}
			Vector3 position = _localPlayer.position;
			int num3 = (int)position.x;
			int num4 = num3;
			int num5 = (int)position.z;
			int num6 = num5;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
			for (int i = 0; i < chunkArrayCopySync.Count; i++)
			{
				Chunk chunk = chunkArrayCopySync[i];
				if (chunk.IsDisplayed && chunk.IsCollisionMeshGenerated && (chunk.GetAABB().center - position).sqrMagnitude <= 36864f)
				{
					if (chunk.worldPosIMin.x < num3)
					{
						num3 = chunk.worldPosIMin.x;
						num7 = 1;
					}
					else if (chunk.worldPosIMin.x == num3)
					{
						num7++;
					}
					if (chunk.worldPosIMax.x > num4)
					{
						num4 = chunk.worldPosIMax.x;
						num8 = 1;
					}
					else if (chunk.worldPosIMax.x == num4)
					{
						num8++;
					}
					if (chunk.worldPosIMin.z < num5)
					{
						num5 = chunk.worldPosIMin.z;
						num9 = 1;
					}
					else if (chunk.worldPosIMin.z == num5)
					{
						num9++;
					}
					if (chunk.worldPosIMax.z > num6)
					{
						num6 = chunk.worldPosIMax.z;
						num10 = 1;
					}
					else if (chunk.worldPosIMax.z == num6)
					{
						num10++;
					}
				}
			}
			int num11 = num4 - num3 + 1;
			int num12 = num6 - num5 + 1;
			if (num7 * 16 != num12)
			{
				num3 += 16;
			}
			if (num8 * 16 != num12)
			{
				num4 -= 16;
			}
			if (num9 * 16 != num11)
			{
				num5 += 16;
			}
			if (num10 * 16 != num11)
			{
				num6 -= 16;
			}
			Vector3 vector = new Vector3((float)num3, 0f, (float)num5);
			Vector3 vector2 = new Vector3((float)(num4 + 1), 256f, (float)(num6 + 1));
			Bounds bounds = new Bounds((vector2 + vector) * 0.5f, vector2 - vector);
			foreach (KeyValuePair<int, PrefabLODManager.PrefabGameObject> keyValuePair2 in this.displayedPrefabs)
			{
				PrefabLODManager.PrefabGameObject value2 = keyValuePair2.Value;
				if (value2.go)
				{
					PrefabInstance prefabInstance = value2.prefabInstance;
					Vector3 vector3 = prefabInstance.boundingBoxPosition;
					Vector3 vector4 = prefabInstance.boundingBoxPosition + prefabInstance.boundingBoxSize;
					if (!(bounds.Contains(vector3) + bounds.Contains(vector4) + bounds.Contains(new Vector3(vector4.x, vector3.y, vector3.z)) + bounds.Contains(new Vector3(vector3.x, vector3.y, vector4.z))))
					{
						if (!value2.isAllShown)
						{
							value2.isAllShown = true;
							Transform transform = value2.go.transform;
							int childCount = transform.childCount;
							for (int j = 0; j < childCount; j++)
							{
								transform.GetChild(j).gameObject.SetActive(true);
							}
						}
					}
					else
					{
						value2.isAllShown = false;
						Transform transform2 = value2.go.transform;
						int childCount2 = transform2.childCount;
						for (int k = 0; k < childCount2; k++)
						{
							Transform child = transform2.GetChild(k);
							string name = child.name;
							StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(name, ',', 1, 0, -1);
							if (separatorPositions.TotalFound == 1)
							{
								int num13 = StringParsers.ParseSInt32(name, 0, separatorPositions.Sep1 - 1, NumberStyles.Integer);
								int num14 = StringParsers.ParseSInt32(name, separatorPositions.Sep1 + 1, -1, NumberStyles.Integer);
								float num15 = 1f;
								float num16 = 1f;
								switch (prefabInstance.rotation)
								{
								case 1:
								{
									int num17 = num13;
									num13 = -num14;
									num14 = num17;
									num15 = -1f;
									break;
								}
								case 2:
									num13 = -num13;
									num14 = -num14;
									num15 = -1f;
									num16 = -1f;
									break;
								case 3:
								{
									int num18 = num13;
									num13 = num14;
									num14 = -num18;
									num16 = -1f;
									break;
								}
								}
								num13 *= 32;
								num14 *= 32;
								num13 += prefabInstance.boundingBoxPosition.x;
								num14 += prefabInstance.boundingBoxPosition.z;
								num13 += prefabInstance.boundingBoxSize.x / 2;
								num14 += prefabInstance.boundingBoxSize.z / 2;
								Vector3 vector5 = new Vector3((float)num13, (float)prefabInstance.boundingBoxPosition.y, (float)num14);
								Vector3 vector6 = new Vector3((float)num13 + num15 * 32f, (float)prefabInstance.boundingBoxPosition.y, (float)num14 + num16 * 32f);
								if (bounds.Contains(vector5) && bounds.Contains(vector6))
								{
									if (child.gameObject.activeSelf)
									{
										child.gameObject.SetActive(false);
									}
								}
								else if (!child.gameObject.activeSelf)
								{
									child.gameObject.SetActive(true);
								}
							}
						}
					}
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x060045DC RID: 17884 RVA: 0x001BD304 File Offset: 0x001BB504
	public void Cleanup()
	{
		if (this.parentTransform != null)
		{
			Origin.Remove(this.parentTransform);
			global::UnityEngine.Object.Destroy(this.parentTransform.gameObject);
			this.parentTransform = null;
		}
		this.removePrefabs(this.displayedPrefabs.Keys.ToList<int>());
	}

	// Token: 0x060045DD RID: 17885 RVA: 0x001BD357 File Offset: 0x001BB557
	public void SetPOIDistance(int _distance)
	{
		PrefabLODManager.lodPoiDistance = _distance;
		this.lastDisplayUpdate = 0f;
		this.lastTime = 0f;
	}

	// Token: 0x060045DE RID: 17886 RVA: 0x001BD378 File Offset: 0x001BB578
	public void UpdateMaterials()
	{
		try
		{
			global::UnityEngine.Object.Destroy(this.sharedMat);
			this.sharedMat = MeshDescription.GetOpaqueMaterial();
			foreach (KeyValuePair<int, PrefabLODManager.PrefabGameObject> keyValuePair in this.displayedPrefabs)
			{
				PrefabLODManager.PrefabGameObject value = keyValuePair.Value;
				if (value.go)
				{
					MeshDescription.GetOpaqueMaterial();
					Renderer[] componentsInChildren = value.go.GetComponentsInChildren<Renderer>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].material = this.sharedMat;
					}
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x04003865 RID: 14437
	public const int cPrefabYPosition = 4;

	// Token: 0x04003866 RID: 14438
	public const int cLodPoiDistance = 1000;

	// Token: 0x04003867 RID: 14439
	public static int lodPoiDistance = 1000;

	// Token: 0x04003868 RID: 14440
	[PublicizedFrom(EAccessModifier.Private)]
	public Material sharedMat;

	// Token: 0x04003869 RID: 14441
	public Dictionary<int, PrefabInstance> prefabsAroundFar = new Dictionary<int, PrefabInstance>();

	// Token: 0x0400386A RID: 14442
	public Dictionary<int, PrefabInstance> prefabsAroundNear = new Dictionary<int, PrefabInstance>();

	// Token: 0x0400386B RID: 14443
	public Dictionary<int, PrefabLODManager.PrefabGameObject> displayedPrefabs = new Dictionary<int, PrefabLODManager.PrefabGameObject>();

	// Token: 0x0400386C RID: 14444
	public Dictionary<string, PrefabLODManager.MeshPrefabSet> meshPathToData = new Dictionary<string, PrefabLODManager.MeshPrefabSet>();

	// Token: 0x0400386D RID: 14445
	public Transform parentTransform;

	// Token: 0x0400386E RID: 14446
	public float lastTime;

	// Token: 0x0400386F RID: 14447
	public float lastDisplayUpdate;

	// Token: 0x04003870 RID: 14448
	public MicroStopwatch stopWatch = new MicroStopwatch();

	// Token: 0x02000963 RID: 2403
	public class PrefabGameObject
	{
		// Token: 0x04003871 RID: 14449
		public string meshPath;

		// Token: 0x04003872 RID: 14450
		public PrefabInstance prefabInstance;

		// Token: 0x04003873 RID: 14451
		public GameObject go;

		// Token: 0x04003874 RID: 14452
		public bool isAllShown;

		// Token: 0x04003875 RID: 14453
		public List<ImposterCanvas> signDatas;
	}

	// Token: 0x02000964 RID: 2404
	public class MeshPrefabSet
	{
		// Token: 0x060045E2 RID: 17890 RVA: 0x001BD47B File Offset: 0x001BB67B
		public MeshPrefabSet()
		{
			this.prefabIDs = new List<int>();
		}

		// Token: 0x04003876 RID: 14454
		public bool isLoading;

		// Token: 0x04003877 RID: 14455
		public SimpleMeshInfo meshInfo;

		// Token: 0x04003878 RID: 14456
		public List<int> prefabIDs;
	}
}
