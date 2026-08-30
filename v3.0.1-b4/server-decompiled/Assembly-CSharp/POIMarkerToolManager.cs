using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PrefabVolumes;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x0200133F RID: 4927
public static class POIMarkerToolManager
{
	// Token: 0x17001250 RID: 4688
	// (get) Token: 0x06009B32 RID: 39730 RVA: 0x003AA4AC File Offset: 0x003A86AC
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

	// Token: 0x17001251 RID: 4689
	// (get) Token: 0x06009B33 RID: 39731 RVA: 0x003AA50A File Offset: 0x003A870A
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

	// Token: 0x06009B34 RID: 39732 RVA: 0x003AA527 File Offset: 0x003A8727
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

	// Token: 0x06009B35 RID: 39733 RVA: 0x003AA52F File Offset: 0x003A872F
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

	// Token: 0x06009B36 RID: 39734 RVA: 0x003AA568 File Offset: 0x003A8768
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

	// Token: 0x06009B37 RID: 39735 RVA: 0x003AA60C File Offset: 0x003A880C
	public static void UnRegisterPoiMarker(SelectionBox _selBox)
	{
		POIMarkerToolManager.registeredPoiMarkers.Remove(_selBox);
	}

	// Token: 0x06009B38 RID: 39736 RVA: 0x003AA61C File Offset: 0x003A881C
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

	// Token: 0x06009B39 RID: 39737 RVA: 0x003AA65C File Offset: 0x003A885C
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

	// Token: 0x06009B3A RID: 39738 RVA: 0x003AA6D4 File Offset: 0x003A88D4
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

	// Token: 0x06009B3B RID: 39739 RVA: 0x003AA864 File Offset: 0x003A8A64
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

	// Token: 0x06009B3C RID: 39740 RVA: 0x003AA884 File Offset: 0x003A8A84
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

	// Token: 0x06009B3D RID: 39741 RVA: 0x003AA9B4 File Offset: 0x003A8BB4
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

	// Token: 0x06009B3E RID: 39742 RVA: 0x003AA9EC File Offset: 0x003A8BEC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void makePreviewVisible(SelectionBox _selBox, bool _visible)
	{
		Transform previewTransform = POIMarkerToolManager.getPreviewTransform(_selBox);
		if (previewTransform != null)
		{
			previewTransform.gameObject.SetActive(_visible);
		}
	}

	// Token: 0x06009B3F RID: 39743 RVA: 0x003AAA15 File Offset: 0x003A8C15
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform createPreviewTransform(SelectionBox _selBox)
	{
		Transform transform = new GameObject("PrefabPreview").transform;
		transform.parent = _selBox.transform;
		transform.localPosition = Vector3.zero;
		return transform;
	}

	// Token: 0x06009B40 RID: 39744 RVA: 0x003AAA40 File Offset: 0x003A8C40
	[PublicizedFrom(EAccessModifier.Private)]
	public static void destroyPreview(SelectionBox _selBox)
	{
		Transform previewTransform = POIMarkerToolManager.getPreviewTransform(_selBox);
		if (previewTransform != null)
		{
			global::UnityEngine.Object.DestroyImmediate(previewTransform.gameObject);
		}
	}

	// Token: 0x06009B41 RID: 39745 RVA: 0x003AAA68 File Offset: 0x003A8C68
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform getPreviewTransform(SelectionBox _selBox)
	{
		return _selBox.transform.Find("PrefabPreview");
	}

	// Token: 0x06009B42 RID: 39746 RVA: 0x003AAA7A File Offset: 0x003A8C7A
	public static ReadOnlyCollection<SelectionBox> GetRegisteredBoxes()
	{
		return POIMarkerToolManager.registeredPoiMarkersReadOnly;
	}

	// Token: 0x0400751F RID: 29983
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PrefabPreviewTransformName = "PrefabPreview";

	// Token: 0x04007520 RID: 29984
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabManagerData prefabManagerData;

	// Token: 0x04007521 RID: 29985
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, PrefabData> allPrefabData = new CaseInsensitiveStringDictionary<PrefabData>();

	// Token: 0x04007522 RID: 29986
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SelectionBox> registeredPoiMarkers = new List<SelectionBox>();

	// Token: 0x04007523 RID: 29987
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ReadOnlyCollection<SelectionBox> registeredPoiMarkersReadOnly = POIMarkerToolManager.registeredPoiMarkers.AsReadOnly();

	// Token: 0x04007524 RID: 29988
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, string> lastSelectedBoxInGroup = new Dictionary<string, string>();
}
