using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PrefabVolumes;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x0200138B RID: 5003
public static class POIMarkerToolManager
{
	// Token: 0x170012DA RID: 4826
	// (get) Token: 0x06009D31 RID: 40241 RVA: 0x003B66D4 File Offset: 0x003B48D4
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

	// Token: 0x170012DB RID: 4827
	// (get) Token: 0x06009D32 RID: 40242 RVA: 0x003B6732 File Offset: 0x003B4932
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

	// Token: 0x06009D33 RID: 40243 RVA: 0x003B674F File Offset: 0x003B494F
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

	// Token: 0x06009D34 RID: 40244 RVA: 0x003B6757 File Offset: 0x003B4957
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

	// Token: 0x06009D35 RID: 40245 RVA: 0x003B6790 File Offset: 0x003B4990
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

	// Token: 0x06009D36 RID: 40246 RVA: 0x003B6834 File Offset: 0x003B4A34
	public static void UnRegisterPoiMarker(SelectionBox _selBox)
	{
		POIMarkerToolManager.registeredPoiMarkers.Remove(_selBox);
	}

	// Token: 0x06009D37 RID: 40247 RVA: 0x003B6844 File Offset: 0x003B4A44
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

	// Token: 0x06009D38 RID: 40248 RVA: 0x003B6884 File Offset: 0x003B4A84
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

	// Token: 0x06009D39 RID: 40249 RVA: 0x003B68FC File Offset: 0x003B4AFC
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

	// Token: 0x06009D3A RID: 40250 RVA: 0x003B6A8C File Offset: 0x003B4C8C
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

	// Token: 0x06009D3B RID: 40251 RVA: 0x003B6AAC File Offset: 0x003B4CAC
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

	// Token: 0x06009D3C RID: 40252 RVA: 0x003B6BDC File Offset: 0x003B4DDC
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

	// Token: 0x06009D3D RID: 40253 RVA: 0x003B6C14 File Offset: 0x003B4E14
	[PublicizedFrom(EAccessModifier.Private)]
	public static void makePreviewVisible(SelectionBox _selBox, bool _visible)
	{
		Transform previewTransform = POIMarkerToolManager.getPreviewTransform(_selBox);
		if (previewTransform != null)
		{
			previewTransform.gameObject.SetActive(_visible);
		}
	}

	// Token: 0x06009D3E RID: 40254 RVA: 0x003B6C3D File Offset: 0x003B4E3D
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform createPreviewTransform(SelectionBox _selBox)
	{
		Transform transform = new GameObject("PrefabPreview").transform;
		transform.parent = _selBox.transform;
		transform.localPosition = Vector3.zero;
		return transform;
	}

	// Token: 0x06009D3F RID: 40255 RVA: 0x003B6C68 File Offset: 0x003B4E68
	[PublicizedFrom(EAccessModifier.Private)]
	public static void destroyPreview(SelectionBox _selBox)
	{
		Transform previewTransform = POIMarkerToolManager.getPreviewTransform(_selBox);
		if (previewTransform != null)
		{
			global::UnityEngine.Object.DestroyImmediate(previewTransform.gameObject);
		}
	}

	// Token: 0x06009D40 RID: 40256 RVA: 0x003B6C90 File Offset: 0x003B4E90
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform getPreviewTransform(SelectionBox _selBox)
	{
		return _selBox.transform.Find("PrefabPreview");
	}

	// Token: 0x06009D41 RID: 40257 RVA: 0x003B6CA2 File Offset: 0x003B4EA2
	public static ReadOnlyCollection<SelectionBox> GetRegisteredBoxes()
	{
		return POIMarkerToolManager.registeredPoiMarkersReadOnly;
	}

	// Token: 0x040076B3 RID: 30387
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PrefabPreviewTransformName = "PrefabPreview";

	// Token: 0x040076B4 RID: 30388
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabManagerData prefabManagerData;

	// Token: 0x040076B5 RID: 30389
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, PrefabData> allPrefabData = new CaseInsensitiveStringDictionary<PrefabData>();

	// Token: 0x040076B6 RID: 30390
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SelectionBox> registeredPoiMarkers = new List<SelectionBox>();

	// Token: 0x040076B7 RID: 30391
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ReadOnlyCollection<SelectionBox> registeredPoiMarkersReadOnly = POIMarkerToolManager.registeredPoiMarkers.AsReadOnly();

	// Token: 0x040076B8 RID: 30392
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, string> lastSelectedBoxInGroup = new Dictionary<string, string>();
}
