using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PrefabVolumes;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x02001379 RID: 4985
public static class POIMarkerToolManager
{
	// Token: 0x170012CE RID: 4814
	// (get) Token: 0x06009CD9 RID: 40153 RVA: 0x003B42D0 File Offset: 0x003B24D0
	public static PrefabManagerData PrefabManagerData
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (POIMarkerToolManager.prefabManagerData != null)
			{
				return POIMarkerToolManager.prefabManagerData;
			}
			POIMarkerToolManager.prefabManagerData = new PrefabManagerData();
			if (POIMarkerToolManager.prefabManagerData.AllPrefabDatas.Count != 0)
			{
				return POIMarkerToolManager.prefabManagerData;
			}
			POIMarkerToolManager.prefabManagerData.LoadPrefabs();
			POIMarkerToolManager.prefabManagerData.ShufflePrefabData(GameRandomManager.Instance.BaseSeed);
			return POIMarkerToolManager.prefabManagerData;
		}
	}

	// Token: 0x170012CF RID: 4815
	// (get) Token: 0x06009CDA RID: 40154 RVA: 0x003B432E File Offset: 0x003B252E
	public static Dictionary<string, PrefabData> AllPrefabData
	{
		get
		{
			if (POIMarkerToolManager.allPrefabData.Count == 0)
			{
				ThreadManager.RunCoroutineSync(POIMarkerToolManager.loadPrefabDatas());
			}
			return POIMarkerToolManager.allPrefabData;
		}
	}

	// Token: 0x06009CDB RID: 40155 RVA: 0x003B434B File Offset: 0x003B254B
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator loadPrefabDatas()
	{
		if (POIMarkerToolManager.allPrefabData.Count != 0)
		{
			yield break;
		}
		MicroStopwatch ms = new MicroStopwatch(true);
		List<PathAbstractions.AbstractedLocation> prefabs = PathAbstractions.PrefabsSearchPaths.GetAvailablePathsList(null, true, null);
		foreach (PathAbstractions.AbstractedLocation abstractedLocation in prefabs)
		{
			PrefabData prefabData = PrefabData.LoadPrefabData(abstractedLocation);
			POIMarkerToolManager.allPrefabData[abstractedLocation.Name] = prefabData;
			if (ms.ElapsedMilliseconds > 500L)
			{
				yield return null;
				ms.ResetAndRestart();
			}
		}
		List<PathAbstractions.AbstractedLocation>.Enumerator enumerator = default(List<PathAbstractions.AbstractedLocation>.Enumerator);
		Log.Out(string.Format("LoadPrefabDatas {0} of {1} in {2} s", POIMarkerToolManager.allPrefabData.Count, prefabs.Count, (float)ms.ElapsedMilliseconds * 0.001f));
		yield break;
		yield break;
	}

	// Token: 0x06009CDC RID: 40156 RVA: 0x003B4353 File Offset: 0x003B2553
	public static void CleanUp()
	{
		POIMarkerToolManager.allPrefabData.Clear();
		PrefabManagerData prefabManagerData = POIMarkerToolManager.prefabManagerData;
		if (prefabManagerData != null)
		{
			prefabManagerData.Cleanup();
		}
		POIMarkerToolManager.prefabManagerData = null;
		POIMarkerToolManager.registeredPoiMarkers.Clear();
		POIMarkerToolManager.lastSelectedBoxInGroup.Clear();
	}

	// Token: 0x06009CDD RID: 40157 RVA: 0x003B438C File Offset: 0x003B258C
	public static void RegisterPoiMarker(SelectionBox _selBox)
	{
		if (_selBox == null)
		{
			return;
		}
		Marker marker = _selBox.UserData as Marker;
		if (marker == null)
		{
			return;
		}
		if (!POIMarkerToolManager.registeredPoiMarkers.Contains(_selBox))
		{
			POIMarkerToolManager.registeredPoiMarkers.Add(_selBox);
		}
		if (marker.MarkerType == Marker.MarkerTypes.PartSpawn && marker.PartDirty)
		{
			if (!string.IsNullOrEmpty(marker.PartToSpawn))
			{
				if (POIMarkerToolManager.getCurrentPreviewName(_selBox) != marker.PartToSpawn)
				{
					ThreadManager.StartCoroutine(POIMarkerToolManager.createPartSpawnPreview(marker, _selBox, _selBox.IsActive || POIMarkerToolManager.isFirstMarkerInGroup(marker)));
				}
			}
			else
			{
				POIMarkerToolManager.destroyPreview(_selBox);
			}
		}
		if (_selBox.IsActive)
		{
			POIMarkerToolManager.SelectionChanged(_selBox);
		}
	}

	// Token: 0x06009CDE RID: 40158 RVA: 0x003B4430 File Offset: 0x003B2630
	public static void UnRegisterPoiMarker(SelectionBox _selBox)
	{
		POIMarkerToolManager.registeredPoiMarkers.Remove(_selBox);
	}

	// Token: 0x06009CDF RID: 40159 RVA: 0x003B4440 File Offset: 0x003B2640
	public static void SelectionChanged(SelectionBox _selBox)
	{
		if (_selBox)
		{
			Marker marker = _selBox.UserData as Marker;
			if (marker != null)
			{
				POIMarkerToolManager.lastSelectedBoxInGroup[marker.GroupName] = _selBox.name;
			}
		}
		POIMarkerToolManager.UpdateAllColors();
	}

	// Token: 0x06009CE0 RID: 40160 RVA: 0x003B4480 File Offset: 0x003B2680
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isFirstMarkerInGroup(Marker _currentMarker)
	{
		string groupName = _currentMarker.GroupName;
		foreach (SelectionBox selectionBox in POIMarkerToolManager.registeredPoiMarkers)
		{
			Marker marker = selectionBox.UserData as Marker;
			if (marker != null && !(marker.GroupName != groupName))
			{
				return marker == _currentMarker;
			}
		}
		return false;
	}

	// Token: 0x06009CE1 RID: 40161 RVA: 0x003B44F8 File Offset: 0x003B26F8
	public static void UpdateAllColors()
	{
		SelectionBox selection = SelectionBoxManager.Instance.Selection;
		Marker marker = ((selection != null) ? selection.UserData : null) as Marker;
		int num = ((marker != null) ? marker.GroupId : (-1));
		foreach (SelectionBox selectionBox in POIMarkerToolManager.registeredPoiMarkers)
		{
			Marker marker2 = selectionBox.UserData as Marker;
			if (marker2 != null)
			{
				bool flag = marker2 == marker;
				bool flag2 = marker2.GroupId == num;
				if (flag)
				{
					if (marker2.MarkerType == Marker.MarkerTypes.PartSpawn)
					{
						selectionBox.SetAllFacesColor(new Color(0f, 0f, 0f, 0f), true);
					}
					else
					{
						selectionBox.SetAllFacesColor(marker2.GroupColor + new Color(0.5f, 0.5f, 0.5f, 0f), true);
					}
				}
				else if (flag2)
				{
					selectionBox.SetAllFacesColor(marker2.GroupColor + new Color(0.2f, 0.2f, 0.2f, 0f), true);
				}
				else
				{
					selectionBox.SetAllFacesColor(marker2.GroupColor, true);
				}
				string text;
				if (flag2)
				{
					POIMarkerToolManager.makePreviewVisible(selectionBox, flag);
				}
				else if (POIMarkerToolManager.lastSelectedBoxInGroup.TryGetValue(marker2.GroupName, out text))
				{
					POIMarkerToolManager.makePreviewVisible(selectionBox, selectionBox.name == text);
				}
				else
				{
					POIMarkerToolManager.makePreviewVisible(selectionBox, POIMarkerToolManager.isFirstMarkerInGroup(marker2));
				}
			}
		}
	}

	// Token: 0x06009CE2 RID: 40162 RVA: 0x003B4688 File Offset: 0x003B2888
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator createPartSpawnPreview(Marker _currentMarker, SelectionBox _selBox, bool _makeVisible)
	{
		POIMarkerToolManager.destroyPreview(_selBox);
		Prefab prefab = new Prefab();
		prefab.Load(_currentMarker.PartToSpawn, true, false, true, false);
		prefab.Init(0, 0);
		prefab.RotateY(false, (prefab.rotationToFaceNorth + (int)_currentMarker.Rotations) % 4);
		yield return prefab.ToTransform(true, true, true, false, POIMarkerToolManager.createPreviewTransform(_selBox), prefab.PrefabName, new Vector3(-((float)prefab.size.x / 2f), 0.1f, -((float)prefab.size.z / 2f)), 0);
		POIMarkerToolManager.makePreviewVisible(_selBox, _makeVisible);
		yield break;
	}

	// Token: 0x06009CE3 RID: 40163 RVA: 0x003B46A8 File Offset: 0x003B28A8
	public static void DisplayPrefabPreviewForMarker(SelectionBox _selBox)
	{
		Marker marker = _selBox.UserData as Marker;
		if (marker == null)
		{
			return;
		}
		string text = "ghosttown,countrytown";
		if (PrefabEditModeManager.Instance.VoxelPrefab != null)
		{
			text = PrefabEditModeManager.Instance.VoxelPrefab.PrefabName;
			text = text.Replace("rwg_tile_", "");
			text = text.Split('_', StringSplitOptions.None)[0];
		}
		bool flag = !Marker.MarkerSizes.Contains(new Vector3i(marker.size.x, 0, marker.size.z));
		Prefab previewPrefabWithAnyTags = POIMarkerToolManager.PrefabManagerData.GetPreviewPrefabWithAnyTags(FastTags<TagGroup.Poi>.Parse(text), -1, new Vector2i(marker.size.x, marker.size.z), flag);
		if (previewPrefabWithAnyTags == null)
		{
			return;
		}
		previewPrefabWithAnyTags.RotateY(true, (previewPrefabWithAnyTags.rotationToFaceNorth + (int)marker.Rotations) % 4);
		POIMarkerToolManager.destroyPreview(_selBox);
		GameManager.Instance.StartCoroutine(previewPrefabWithAnyTags.ToTransform(true, true, true, false, POIMarkerToolManager.createPreviewTransform(_selBox), previewPrefabWithAnyTags.PrefabName, new Vector3(-((float)previewPrefabWithAnyTags.size.x / 2f), (float)previewPrefabWithAnyTags.yOffset + 0.15f, -((float)previewPrefabWithAnyTags.size.z / 2f)), 0));
	}

	// Token: 0x06009CE4 RID: 40164 RVA: 0x003B47D8 File Offset: 0x003B29D8
	[PublicizedFrom(EAccessModifier.Private)]
	public static string getCurrentPreviewName(SelectionBox _selBox)
	{
		Transform previewTransform = POIMarkerToolManager.getPreviewTransform(_selBox);
		if (previewTransform == null)
		{
			return null;
		}
		if (previewTransform.childCount == 0)
		{
			return null;
		}
		return previewTransform.GetChild(0).name;
	}

	// Token: 0x06009CE5 RID: 40165 RVA: 0x003B4810 File Offset: 0x003B2A10
	[PublicizedFrom(EAccessModifier.Private)]
	public static void makePreviewVisible(SelectionBox _selBox, bool _visible)
	{
		Transform previewTransform = POIMarkerToolManager.getPreviewTransform(_selBox);
		if (previewTransform != null)
		{
			previewTransform.gameObject.SetActive(_visible);
		}
	}

	// Token: 0x06009CE6 RID: 40166 RVA: 0x003B4839 File Offset: 0x003B2A39
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform createPreviewTransform(SelectionBox _selBox)
	{
		Transform transform = new GameObject("PrefabPreview").transform;
		transform.parent = _selBox.transform;
		transform.localPosition = Vector3.zero;
		return transform;
	}

	// Token: 0x06009CE7 RID: 40167 RVA: 0x003B4864 File Offset: 0x003B2A64
	[PublicizedFrom(EAccessModifier.Private)]
	public static void destroyPreview(SelectionBox _selBox)
	{
		Transform previewTransform = POIMarkerToolManager.getPreviewTransform(_selBox);
		if (previewTransform != null)
		{
			global::UnityEngine.Object.DestroyImmediate(previewTransform.gameObject);
		}
	}

	// Token: 0x06009CE8 RID: 40168 RVA: 0x003B488C File Offset: 0x003B2A8C
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform getPreviewTransform(SelectionBox _selBox)
	{
		return _selBox.transform.Find("PrefabPreview");
	}

	// Token: 0x06009CE9 RID: 40169 RVA: 0x003B489E File Offset: 0x003B2A9E
	public static ReadOnlyCollection<SelectionBox> GetRegisteredBoxes()
	{
		return POIMarkerToolManager.registeredPoiMarkersReadOnly;
	}

	// Token: 0x04007672 RID: 30322
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PrefabPreviewTransformName = "PrefabPreview";

	// Token: 0x04007673 RID: 30323
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabManagerData prefabManagerData;

	// Token: 0x04007674 RID: 30324
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, PrefabData> allPrefabData = new CaseInsensitiveStringDictionary<PrefabData>();

	// Token: 0x04007675 RID: 30325
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SelectionBox> registeredPoiMarkers = new List<SelectionBox>();

	// Token: 0x04007676 RID: 30326
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ReadOnlyCollection<SelectionBox> registeredPoiMarkersReadOnly = POIMarkerToolManager.registeredPoiMarkers.AsReadOnly();

	// Token: 0x04007677 RID: 30327
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, string> lastSelectedBoxInGroup = new Dictionary<string, string>();
}
