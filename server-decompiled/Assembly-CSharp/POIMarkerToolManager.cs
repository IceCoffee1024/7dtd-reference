using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PrefabVolumes;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x02001359 RID: 4953
public static class POIMarkerToolManager
{
	// Token: 0x17001289 RID: 4745
	// (get) Token: 0x06009C2D RID: 39981 RVA: 0x003AF754 File Offset: 0x003AD954
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

	// Token: 0x1700128A RID: 4746
	// (get) Token: 0x06009C2E RID: 39982 RVA: 0x003AF7B2 File Offset: 0x003AD9B2
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

	// Token: 0x06009C2F RID: 39983 RVA: 0x003AF7CF File Offset: 0x003AD9CF
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

	// Token: 0x06009C30 RID: 39984 RVA: 0x003AF7D7 File Offset: 0x003AD9D7
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

	// Token: 0x06009C31 RID: 39985 RVA: 0x003AF810 File Offset: 0x003ADA10
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

	// Token: 0x06009C32 RID: 39986 RVA: 0x003AF8B4 File Offset: 0x003ADAB4
	public static void UnRegisterPoiMarker(SelectionBox _selBox)
	{
		POIMarkerToolManager.registeredPoiMarkers.Remove(_selBox);
	}

	// Token: 0x06009C33 RID: 39987 RVA: 0x003AF8C4 File Offset: 0x003ADAC4
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

	// Token: 0x06009C34 RID: 39988 RVA: 0x003AF904 File Offset: 0x003ADB04
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

	// Token: 0x06009C35 RID: 39989 RVA: 0x003AF97C File Offset: 0x003ADB7C
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

	// Token: 0x06009C36 RID: 39990 RVA: 0x003AFB0C File Offset: 0x003ADD0C
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

	// Token: 0x06009C37 RID: 39991 RVA: 0x003AFB2C File Offset: 0x003ADD2C
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

	// Token: 0x06009C38 RID: 39992 RVA: 0x003AFC5C File Offset: 0x003ADE5C
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

	// Token: 0x06009C39 RID: 39993 RVA: 0x003AFC94 File Offset: 0x003ADE94
	[PublicizedFrom(EAccessModifier.Private)]
	public static void makePreviewVisible(SelectionBox _selBox, bool _visible)
	{
		Transform previewTransform = POIMarkerToolManager.getPreviewTransform(_selBox);
		if (previewTransform != null)
		{
			previewTransform.gameObject.SetActive(_visible);
		}
	}

	// Token: 0x06009C3A RID: 39994 RVA: 0x003AFCBD File Offset: 0x003ADEBD
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform createPreviewTransform(SelectionBox _selBox)
	{
		Transform transform = new GameObject("PrefabPreview").transform;
		transform.parent = _selBox.transform;
		transform.localPosition = Vector3.zero;
		return transform;
	}

	// Token: 0x06009C3B RID: 39995 RVA: 0x003AFCE8 File Offset: 0x003ADEE8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void destroyPreview(SelectionBox _selBox)
	{
		Transform previewTransform = POIMarkerToolManager.getPreviewTransform(_selBox);
		if (previewTransform != null)
		{
			global::UnityEngine.Object.DestroyImmediate(previewTransform.gameObject);
		}
	}

	// Token: 0x06009C3C RID: 39996 RVA: 0x003AFD10 File Offset: 0x003ADF10
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform getPreviewTransform(SelectionBox _selBox)
	{
		return _selBox.transform.Find("PrefabPreview");
	}

	// Token: 0x06009C3D RID: 39997 RVA: 0x003AFD22 File Offset: 0x003ADF22
	public static ReadOnlyCollection<SelectionBox> GetRegisteredBoxes()
	{
		return POIMarkerToolManager.registeredPoiMarkersReadOnly;
	}

	// Token: 0x040075F7 RID: 30199
	[PublicizedFrom(EAccessModifier.Private)]
	public const string PrefabPreviewTransformName = "PrefabPreview";

	// Token: 0x040075F8 RID: 30200
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabManagerData prefabManagerData;

	// Token: 0x040075F9 RID: 30201
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, PrefabData> allPrefabData = new CaseInsensitiveStringDictionary<PrefabData>();

	// Token: 0x040075FA RID: 30202
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<SelectionBox> registeredPoiMarkers = new List<SelectionBox>();

	// Token: 0x040075FB RID: 30203
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ReadOnlyCollection<SelectionBox> registeredPoiMarkersReadOnly = POIMarkerToolManager.registeredPoiMarkers.AsReadOnly();

	// Token: 0x040075FC RID: 30204
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, string> lastSelectedBoxInGroup = new Dictionary<string, string>();
}
