using System;
using System.Collections.Generic;
using Platform;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x02000E6E RID: 3694
public class PrefabPreviewManager
{
	// Token: 0x17000CE9 RID: 3305
	// (get) Token: 0x0600710B RID: 28939 RVA: 0x002BB081 File Offset: 0x002B9281
	public static WorldBuilder WorldBuilder
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_WorldGenerationWindow instance = XUiC_WorldGenerationWindow.Instance;
			if (instance == null)
			{
				return null;
			}
			return instance.WorldBuilder;
		}
	}

	// Token: 0x0600710C RID: 28940 RVA: 0x002BB093 File Offset: 0x002B9293
	public PrefabPreviewManager(XUiC_WorldGenerationPreview _previewWindow)
	{
		this.previewWindow = _previewWindow;
	}

	// Token: 0x0600710D RID: 28941 RVA: 0x002BB0B8 File Offset: 0x002B92B8
	public void InitPrefabs()
	{
		if (this.Initialized)
		{
			return;
		}
		this.prefabsAround.Clear();
		PrefabPreviewManager.WorldBuilder.PrefabManager.GetPrefabsAround(this.previewWindow.GetCameraPosition(), 100000f, this.prefabsAround);
		this.UpdatePrefabsAround(this.prefabsAround);
		this.Initialized = true;
	}

	// Token: 0x0600710E RID: 28942 RVA: 0x002BB111 File Offset: 0x002B9311
	public void RemovePrefabs()
	{
		this.ClearDisplayedPrefabs();
		this.displayedPrefabs.Clear();
		this.prefabsAround.Clear();
		this.Initialized = false;
	}

	// Token: 0x0600710F RID: 28943 RVA: 0x002BB136 File Offset: 0x002B9336
	public void Update()
	{
		if (Time.time - this.lastDisplayUpdate < 2f)
		{
			return;
		}
		this.lastDisplayUpdate = Time.time;
		this.ForceUpdate();
	}

	// Token: 0x06007110 RID: 28944 RVA: 0x002BB160 File Offset: 0x002B9360
	public void ForceUpdate()
	{
		this.UpdateDisplay();
		if (Time.time - this.lastTime < 2f)
		{
			return;
		}
		this.lastTime = Time.time;
		if (PrefabPreviewManager.WorldBuilder == null)
		{
			return;
		}
		if (XUiC_WorldGenerationWindow.Instance == null)
		{
			return;
		}
		if (PrefabPreviewManager.WorldBuilder.PrefabManager.UsedPrefabsWorld == null)
		{
			return;
		}
		this.InitPrefabs();
	}

	// Token: 0x06007111 RID: 28945 RVA: 0x002BB1BC File Offset: 0x002B93BC
	public void ClearDisplayedPrefabs()
	{
		if (this.displayedPrefabs == null || this.displayedPrefabs.Count == 0)
		{
			return;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, PrefabPreviewManager.PrefabGameObject> keyValuePair in this.displayedPrefabs)
		{
			list.Add(keyValuePair.Key);
		}
		foreach (int num in list)
		{
			if (!(this.displayedPrefabs[num].Go == null))
			{
				MeshFilter[] componentsInChildren = this.displayedPrefabs[num].Go.GetComponentsInChildren<MeshFilter>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					global::UnityEngine.Object.Destroy(componentsInChildren[i].mesh);
				}
				Renderer[] componentsInChildren2 = this.displayedPrefabs[num].Go.GetComponentsInChildren<Renderer>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					global::UnityEngine.Object.Destroy(componentsInChildren2[j].material);
				}
				global::UnityEngine.Object.Destroy(this.displayedPrefabs[num].Go);
				this.displayedPrefabs.Remove(num);
			}
		}
	}

	// Token: 0x06007112 RID: 28946 RVA: 0x002BB328 File Offset: 0x002B9528
	public void UpdatePrefabsAround(Dictionary<int, PrefabDataInstance> _prefabsAround)
	{
		foreach (KeyValuePair<int, PrefabDataInstance> keyValuePair in _prefabsAround)
		{
			PrefabDataInstance value = keyValuePair.Value;
			if (!this.displayedPrefabs.ContainsKey(value.id))
			{
				string name = value.location.Name;
				if (PathAbstractions.PrefabImpostersSearchPaths.GetLocation(name, null, null).Type != PathAbstractions.EAbstractedLocationType.None)
				{
					PrefabPreviewManager.PrefabGameObject prefabGameObject = new PrefabPreviewManager.PrefabGameObject
					{
						PrefabInstance = value
					};
					this.displayedPrefabs.Add(value.id, prefabGameObject);
				}
			}
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, PrefabPreviewManager.PrefabGameObject> keyValuePair2 in this.displayedPrefabs)
		{
			if (!_prefabsAround.ContainsKey(keyValuePair2.Key))
			{
				list.Add(keyValuePair2.Key);
			}
		}
		foreach (int num in list)
		{
			if (!(this.displayedPrefabs[num].Go == null))
			{
				MeshFilter[] componentsInChildren = this.displayedPrefabs[num].Go.GetComponentsInChildren<MeshFilter>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					global::UnityEngine.Object.Destroy(componentsInChildren[i].mesh);
				}
				Renderer[] componentsInChildren2 = this.displayedPrefabs[num].Go.GetComponentsInChildren<Renderer>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					global::UnityEngine.Object.Destroy(componentsInChildren2[j].material);
				}
				global::UnityEngine.Object.Destroy(this.displayedPrefabs[num].Go);
				this.displayedPrefabs.Remove(num);
			}
		}
	}

	// Token: 0x06007113 RID: 28947 RVA: 0x002BB538 File Offset: 0x002B9738
	public void UpdateDisplay()
	{
		if (XUiC_WorldGenerationWindow.Instance.PreviewQualityLevel == XUiC_WorldGenerationWindow.PreviewQuality.NoPreview)
		{
			return;
		}
		MicroStopwatch microStopwatch = new MicroStopwatch();
		if (this.parentTransform == null)
		{
			this.parentTransform = new GameObject("PrefabsLOD").transform;
			this.parentTransform.gameObject.layer = 11;
		}
		foreach (KeyValuePair<int, PrefabPreviewManager.PrefabGameObject> keyValuePair in this.displayedPrefabs)
		{
			PrefabPreviewManager.PrefabGameObject value = keyValuePair.Value;
			PrefabDataInstance prefabInstance = value.PrefabInstance;
			Vector3 vector = prefabInstance.boundingBoxPosition.ToVector3();
			Vector3 vector2 = prefabInstance.boundingBoxPosition.ToVector3();
			Vector3 vector3 = prefabInstance.boundingBoxSize.ToVector3();
			if (prefabInstance.rotation % 2 == 0)
			{
				vector += new Vector3((float)prefabInstance.boundingBoxSize.x * 0.5f, 0f, (float)prefabInstance.boundingBoxSize.z * 0.5f);
				vector2 += new Vector3((float)prefabInstance.boundingBoxSize.x * 0.5f, 0f, (float)prefabInstance.boundingBoxSize.z * 0.5f);
			}
			else
			{
				vector += new Vector3((float)prefabInstance.boundingBoxSize.z * 0.5f, 0f, (float)prefabInstance.boundingBoxSize.x * 0.5f);
				vector2 += new Vector3((float)prefabInstance.boundingBoxSize.z * 0.5f, 0f, (float)prefabInstance.boundingBoxSize.x * 0.5f);
				vector3 = new Vector3(vector3.z, vector3.y, vector3.x);
			}
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
			float num = 0f;
			Utils.DrawBounds(new Bounds(vector2 + new Vector3(0f, (float)prefabInstance.boundingBoxSize.y * 0.5f + 0.1f + num, 0f) - Origin.position, vector3), Color.green, 2f);
			if (!value.Go)
			{
				XUiC_WorldGenerationWindow.PreviewQuality previewQuality = XUiC_WorldGenerationWindow.Instance.PreviewQualityLevel;
				if (((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent() || SystemInfo.systemMemorySize < 1300) && previewQuality > XUiC_WorldGenerationWindow.PreviewQuality.Default)
				{
					previewQuality = XUiC_WorldGenerationWindow.PreviewQuality.Default;
				}
				GameObject gameObject;
				if (previewQuality == XUiC_WorldGenerationWindow.PreviewQuality.Highest)
				{
					string name = prefabInstance.location.Name;
					gameObject = SimpleMeshFile.ReadGameObject(PathAbstractions.PrefabImpostersSearchPaths.GetLocation(name, null, null), 0f, null, true, false, null, null);
					if (gameObject == null)
					{
						continue;
					}
					Renderer[] array = gameObject.GetComponentsInChildren<Renderer>();
					for (int i = 0; i < array.Length; i++)
					{
						array[i].gameObject.layer = 11;
					}
				}
				else if (previewQuality >= XUiC_WorldGenerationWindow.PreviewQuality.Low && previewQuality <= XUiC_WorldGenerationWindow.PreviewQuality.High)
				{
					gameObject = new GameObject();
					if (!prefabInstance.prefab.Name.Contains("rwg_tile") && !prefabInstance.prefab.Name.Contains("part_driveway"))
					{
						Transform transform = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
						transform.SetParent(gameObject.transform);
						transform.localPosition = new Vector3(0f, (float)(prefabInstance.boundingBoxSize.y / 2), 0f);
						transform.localScale = prefabInstance.boundingBoxSize.ToVector3();
						if ((int)(prefabInstance.previewColor.r + prefabInstance.previewColor.g + prefabInstance.previewColor.b) != 765)
						{
							transform.GetComponent<Renderer>().material.color = prefabInstance.previewColor;
						}
						Renderer[] array = gameObject.GetComponentsInChildren<Renderer>();
						for (int i = 0; i < array.Length; i++)
						{
							array[i].gameObject.layer = 11;
						}
					}
				}
				else
				{
					gameObject = new GameObject();
				}
				value.Go = gameObject;
				gameObject.layer = 11;
				Transform transform2 = gameObject.transform;
				transform2.name = prefabInstance.location.Name;
				transform2.SetParent(this.parentTransform, false);
				Vector3 vector4 = vector + new Vector3(0f, 0.01f + num - 4f, 0f) - Origin.position;
				transform2.SetPositionAndRotation(vector4, quaternion);
				GameObject gameObject2 = new GameObject(prefabInstance.prefab.Name);
				Transform transform3 = gameObject2.transform;
				transform3.SetParent(transform2);
				transform3.rotation = Quaternion.Euler(90f, gameObject.transform.rotation.eulerAngles.y, 0f);
				transform3.localPosition = new Vector3(0f, (float)(prefabInstance.boundingBoxSize.y + prefabInstance.prefab.yOffset) + 0.25f, 0f);
				gameObject2.layer = 11;
				Vector2i vector2i = new Vector2i(((int)vector.x + PrefabPreviewManager.WorldBuilder.WorldSize / 2) / 150, ((int)vector.z + PrefabPreviewManager.WorldBuilder.WorldSize / 2) / 150);
				TextMesh textMesh = gameObject2.AddMissingComponent<TextMesh>();
				textMesh.alignment = TextAlignment.Center;
				textMesh.anchor = TextAnchor.MiddleCenter;
				textMesh.fontSize = (prefabInstance.prefab.Name.Contains("trader") ? 100 : 20);
				textMesh.color = (prefabInstance.prefab.Name.Contains("trader") ? Color.red : Color.green);
				textMesh.text = string.Concat(new string[]
				{
					prefabInstance.prefab.Name,
					Environment.NewLine,
					string.Format("pos {0}{1}", prefabInstance.boundingBoxPosition, Environment.NewLine),
					string.Format("yoffset {0}{1}", prefabInstance.prefab.yOffset, Environment.NewLine),
					string.Format("rots to north {0}, total left {1}{2}", prefabInstance.prefab.rotationToFaceNorth, prefabInstance.rotation, Environment.NewLine),
					string.Format("tile pos {0}{1}", vector2i, Environment.NewLine),
					string.Format("score {0}", prefabInstance.prefab.DensityScore)
				});
				if (microStopwatch.ElapsedMilliseconds > 50L)
				{
					this.lastDisplayUpdate = 0f;
					return;
				}
			}
		}
		foreach (KeyValuePair<int, PrefabPreviewManager.PrefabGameObject> keyValuePair2 in this.displayedPrefabs)
		{
			if (!(keyValuePair2.Value.Go == null))
			{
				Transform transform4 = keyValuePair2.Value.Go.transform;
				for (int j = 0; j < transform4.childCount; j++)
				{
					transform4.GetChild(j).gameObject.SetActive(true);
				}
			}
		}
	}

	// Token: 0x06007114 RID: 28948 RVA: 0x002BBDB8 File Offset: 0x002B9FB8
	public void ClearOldPreview()
	{
		this.RemovePrefabs();
	}

	// Token: 0x06007115 RID: 28949 RVA: 0x002BBDC0 File Offset: 0x002B9FC0
	public void Cleanup()
	{
		this.RemovePrefabs();
		if (!this.parentTransform)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(this.parentTransform.gameObject);
		this.parentTransform = null;
	}

	// Token: 0x04005595 RID: 21909
	[PublicizedFrom(EAccessModifier.Private)]
	public const float PrefabYPosition = 4f;

	// Token: 0x04005596 RID: 21910
	[PublicizedFrom(EAccessModifier.Private)]
	public const float DisplayUpdateDelay = 2f;

	// Token: 0x04005597 RID: 21911
	[PublicizedFrom(EAccessModifier.Private)]
	public const float PrefabListUpdateDelay = 2f;

	// Token: 0x04005598 RID: 21912
	[PublicizedFrom(EAccessModifier.Private)]
	public const int LodPoiDistance = 100000;

	// Token: 0x04005599 RID: 21913
	public bool ReadyToDisplay;

	// Token: 0x0400559A RID: 21914
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldGenerationPreview previewWindow;

	// Token: 0x0400559B RID: 21915
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, PrefabDataInstance> prefabsAround = new Dictionary<int, PrefabDataInstance>();

	// Token: 0x0400559C RID: 21916
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<int, PrefabPreviewManager.PrefabGameObject> displayedPrefabs = new Dictionary<int, PrefabPreviewManager.PrefabGameObject>();

	// Token: 0x0400559D RID: 21917
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform parentTransform;

	// Token: 0x0400559E RID: 21918
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTime;

	// Token: 0x0400559F RID: 21919
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastDisplayUpdate;

	// Token: 0x040055A0 RID: 21920
	public bool Initialized;

	// Token: 0x02000E6F RID: 3695
	[PublicizedFrom(EAccessModifier.Private)]
	public class PrefabGameObject
	{
		// Token: 0x040055A1 RID: 21921
		public PrefabDataInstance PrefabInstance;

		// Token: 0x040055A2 RID: 21922
		public GameObject Go;
	}
}
