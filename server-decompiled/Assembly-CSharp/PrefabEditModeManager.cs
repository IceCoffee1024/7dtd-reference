using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000949 RID: 2377
public class PrefabEditModeManager
{
	// Token: 0x14000051 RID: 81
	// (add) Token: 0x060044F7 RID: 17655 RVA: 0x001B4534 File Offset: 0x001B2734
	// (remove) Token: 0x060044F8 RID: 17656 RVA: 0x001B456C File Offset: 0x001B276C
	public event Action<PrefabInstance> OnPrefabChanged;

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x060044F9 RID: 17657 RVA: 0x001B45A1 File Offset: 0x001B27A1
	public int PrefabInstanceId
	{
		get
		{
			return this.prefabInstanceId;
		}
	}

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x060044FA RID: 17658 RVA: 0x001B45AC File Offset: 0x001B27AC
	public SelectionBox boxShowFacing
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			SelectionBox selectionBox;
			if (SelectionBoxManager.Instance.CategoryPrefabFacingVolume.TryGetBox("single", out selectionBox))
			{
				return selectionBox;
			}
			selectionBox = SelectionBoxManager.Instance.CategoryPrefabFacingVolume.AddBox("single", Vector3i.zero, Vector3i.one, true, true);
			return selectionBox;
		}
	}

	// Token: 0x060044FB RID: 17659 RVA: 0x001B45F5 File Offset: 0x001B27F5
	public PrefabEditModeManager()
	{
		PrefabEditModeManager.Instance = this;
	}

	// Token: 0x060044FC RID: 17660 RVA: 0x001B4620 File Offset: 0x001B2820
	public void Init()
	{
		this.ReloadAllXmls();
		this.InitXmlWatcher();
		if (this.IsActive())
		{
			SelectionBoxManager.Instance.CategoryDynamicPrefab.SetVisible(false);
		}
		this.NeedsSaving = false;
		GameManager.Instance.World.ChunkCache.OnBlockChangedDelegates += this.blockChangeDelegate;
	}

	// Token: 0x060044FD RID: 17661 RVA: 0x001B4678 File Offset: 0x001B2878
	public void Update()
	{
		if (!this.IsActive())
		{
			return;
		}
		this.updateCompositionGrid();
	}

	// Token: 0x060044FE RID: 17662 RVA: 0x001B4689 File Offset: 0x001B2889
	public bool IsActive()
	{
		return GameManager.Instance.IsEditMode() && GamePrefs.GetString(EnumGamePrefs.GameWorld) == "Empty";
	}

	// Token: 0x060044FF RID: 17663 RVA: 0x001B46AC File Offset: 0x001B28AC
	public void LoadRecentlyUsedOrCreateNew()
	{
		if (this.VoxelPrefab != null)
		{
			return;
		}
		string @string = GamePrefs.GetString(EnumGamePrefs.LastLoadedPrefab);
		PathAbstractions.AbstractedLocation abstractedLocation = PathAbstractions.AbstractedLocation.None;
		if (!string.IsNullOrEmpty(@string))
		{
			abstractedLocation = PathAbstractions.PrefabsSearchPaths.GetLocation(@string, null, null);
		}
		if (abstractedLocation.Exists())
		{
			ThreadManager.StartCoroutine(this.loadLastUsedPrefabLater());
			return;
		}
		this.NewVoxelPrefab();
	}

	// Token: 0x06004500 RID: 17664 RVA: 0x001B4718 File Offset: 0x001B2918
	public void LoadRecentlyUsed()
	{
		string @string = GamePrefs.GetString(EnumGamePrefs.LastLoadedPrefab);
		if (!string.IsNullOrEmpty(@string) && (this.VoxelPrefab == null || @string != this.VoxelPrefab.PrefabName))
		{
			ThreadManager.StartCoroutine(this.loadLastUsedPrefabLater());
		}
	}

	// Token: 0x06004501 RID: 17665 RVA: 0x001B475F File Offset: 0x001B295F
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator loadLastUsedPrefabLater()
	{
		yield return new WaitForSeconds(1f);
		ChunkCluster cc = GameManager.Instance.World.ChunkCache;
		List<Chunk> chunkArrayCopySync = cc.GetChunkArrayCopySync();
		foreach (Chunk c in chunkArrayCopySync)
		{
			if (!cc.IsOnBorder(c))
			{
				if (!c.IsEmpty())
				{
					while (c.NeedsRegeneration || c.NeedsCopying)
					{
						yield return new WaitForSeconds(1f);
					}
					c = null;
				}
			}
		}
		List<Chunk>.Enumerator enumerator = default(List<Chunk>.Enumerator);
		PathAbstractions.AbstractedLocation location = PathAbstractions.PrefabsSearchPaths.GetLocation(GamePrefs.GetString(EnumGamePrefs.LastLoadedPrefab), null, null);
		this.LoadVoxelPrefab(location, false, false);
		yield break;
		yield break;
	}

	// Token: 0x06004502 RID: 17666 RVA: 0x001B4770 File Offset: 0x001B2970
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitXmlWatcher()
	{
		string gameDir = GameIO.GetGameDir("Data/Prefabs");
		Log.Out("Watching prefabs folder for XML changes: " + gameDir);
		this.xmlWatcher = new FileSystemWatcher(gameDir, "*.xml");
		this.xmlWatcher.IncludeSubdirectories = true;
		this.xmlWatcher.Changed += this.OnXmlFileChanged;
		this.xmlWatcher.Created += this.OnXmlFileChanged;
		this.xmlWatcher.Deleted += this.OnXmlFileChanged;
		this.xmlWatcher.EnableRaisingEvents = true;
	}

	// Token: 0x06004503 RID: 17667 RVA: 0x001B4808 File Offset: 0x001B2A08
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnXmlFileChanged(object _sender, FileSystemEventArgs _e)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(_e.Name);
		PathAbstractions.AbstractedLocation abstractedLocation = new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.GameData, fileNameWithoutExtension, Path.ChangeExtension(_e.FullPath, ".tts"), null, false, null);
		if (_e.ChangeType == WatcherChangeTypes.Deleted)
		{
			Log.Out(string.Format("Prefab XML {0}: {1}", _e.ChangeType, _e.Name));
			Dictionary<PathAbstractions.AbstractedLocation, Prefab> dictionary = this.loadedPrefabHeaders;
			lock (dictionary)
			{
				this.loadedPrefabHeaders.Remove(abstractedLocation);
				return;
			}
		}
		if (!abstractedLocation.FileNameNoExtension.EndsWith("_signs"))
		{
			this.LoadXml(abstractedLocation);
			if (this.VoxelPrefab != null && this.VoxelPrefab.location == abstractedLocation)
			{
				Log.Out(string.Format("Prefab XML {0}: {1}, applying changes to loaded prefab", _e.ChangeType, _e.Name));
				this.VoxelPrefab.LoadXMLData(this.VoxelPrefab.location);
				return;
			}
			if (this.VoxelPrefab != null)
			{
				Log.Out(string.Format("Prefab XML {0}: {1}, change not related to loaded prefab (Loaded: {2}, FP {3}; Changed: {4}, FP {5})", new object[]
				{
					_e.ChangeType,
					_e.Name,
					this.VoxelPrefab.location,
					this.VoxelPrefab.location.FullPath,
					abstractedLocation,
					abstractedLocation.FullPath
				}));
			}
		}
	}

	// Token: 0x06004504 RID: 17668 RVA: 0x001B4980 File Offset: 0x001B2B80
	public void ReloadAllXmls()
	{
		Dictionary<PathAbstractions.AbstractedLocation, Prefab> dictionary = this.loadedPrefabHeaders;
		lock (dictionary)
		{
			this.loadedPrefabHeaders.Clear();
			foreach (PathAbstractions.AbstractedLocation abstractedLocation in PathAbstractions.PrefabsSearchPaths.GetAvailablePathsList(null, false, null))
			{
				this.LoadXml(abstractedLocation);
			}
		}
	}

	// Token: 0x06004505 RID: 17669 RVA: 0x001B4A18 File Offset: 0x001B2C18
	public void LoadXml(PathAbstractions.AbstractedLocation _location)
	{
		Prefab prefab = new Prefab();
		prefab.LoadXMLData(_location);
		Dictionary<PathAbstractions.AbstractedLocation, Prefab> dictionary = this.loadedPrefabHeaders;
		lock (dictionary)
		{
			this.loadedPrefabHeaders[_location] = prefab;
		}
	}

	// Token: 0x06004506 RID: 17670 RVA: 0x001B4A70 File Offset: 0x001B2C70
	public void Cleanup()
	{
		if (this.xmlWatcher != null)
		{
			this.xmlWatcher.EnableRaisingEvents = false;
			this.xmlWatcher = null;
		}
		Dictionary<PathAbstractions.AbstractedLocation, Prefab> dictionary = this.loadedPrefabHeaders;
		lock (dictionary)
		{
			this.loadedPrefabHeaders.Clear();
		}
		if (this.groundGrid)
		{
			global::UnityEngine.Object.Destroy(this.groundGrid);
			this.groundGrid = null;
		}
		if (this.prefabInstanceId != -1)
		{
			DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
			dynamicPrefabDecorator.RemovePrefabAndSelection(GameManager.Instance.World, dynamicPrefabDecorator.GetPrefab(this.prefabInstanceId), false);
			this.prefabInstanceId = -1;
		}
		GameManager.Instance.World.ChunkCache.OnBlockChangedDelegates -= this.blockChangeDelegate;
		this.ClearImposterPrefab();
		this.ClearVoxelPrefab();
		this.OnPrefabChanged = null;
		this.HighlightBlocks(null);
		GameManager.Instance.TriggerSendOfLocalPlayerDataFile(0f);
	}

	// Token: 0x06004507 RID: 17671 RVA: 0x001B4B70 File Offset: 0x001B2D70
	[PublicizedFrom(EAccessModifier.Private)]
	public void blockChangeDelegate(Vector3i pos, BlockValue bvOld, sbyte oldDens, TextureFullArray oldTex, BlockValue bvNew)
	{
		this.NeedsSaving = true;
	}

	// Token: 0x06004508 RID: 17672 RVA: 0x001B4B7C File Offset: 0x001B2D7C
	public void FindPrefabs(string _group, List<PathAbstractions.AbstractedLocation> _result)
	{
		Dictionary<PathAbstractions.AbstractedLocation, Prefab> dictionary = this.loadedPrefabHeaders;
		lock (dictionary)
		{
			foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> keyValuePair in this.loadedPrefabHeaders)
			{
				if (_group == null)
				{
					_result.Add(keyValuePair.Key);
				}
				else if (_group.Length == 0)
				{
					if (keyValuePair.Value.editorGroups == null || keyValuePair.Value.editorGroups.Count == 0)
					{
						_result.Add(keyValuePair.Key);
					}
				}
				else if (keyValuePair.Value.editorGroups != null)
				{
					for (int i = 0; i < keyValuePair.Value.editorGroups.Count; i++)
					{
						if (string.Compare(keyValuePair.Value.editorGroups[i], _group, StringComparison.OrdinalIgnoreCase) == 0)
						{
							_result.Add(keyValuePair.Key);
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x06004509 RID: 17673 RVA: 0x001B4C9C File Offset: 0x001B2E9C
	public void GetAllTags(List<string> _result, Prefab _considerLoadedPrefab = null)
	{
		Dictionary<PathAbstractions.AbstractedLocation, Prefab> dictionary = this.loadedPrefabHeaders;
		lock (dictionary)
		{
			foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> keyValuePair in this.loadedPrefabHeaders)
			{
				if (!keyValuePair.Value.Tags.IsEmpty)
				{
					foreach (string text in keyValuePair.Value.Tags.GetTagNames())
					{
						if (!_result.ContainsCaseInsensitive(text))
						{
							_result.Add(text);
						}
					}
				}
			}
		}
		if (_considerLoadedPrefab != null && !_considerLoadedPrefab.Tags.IsEmpty)
		{
			foreach (string text2 in _considerLoadedPrefab.Tags.GetTagNames())
			{
				if (!_result.ContainsCaseInsensitive(text2))
				{
					_result.Add(text2);
				}
			}
		}
	}

	// Token: 0x0600450A RID: 17674 RVA: 0x001B4DF4 File Offset: 0x001B2FF4
	public void GetAllThemeTags(List<string> _result, Prefab _considerLoadedPrefab = null)
	{
		Dictionary<PathAbstractions.AbstractedLocation, Prefab> dictionary = this.loadedPrefabHeaders;
		lock (dictionary)
		{
			foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> keyValuePair in this.loadedPrefabHeaders)
			{
				if (!keyValuePair.Value.ThemeTags.IsEmpty)
				{
					foreach (string text in keyValuePair.Value.ThemeTags.GetTagNames())
					{
						if (!_result.ContainsCaseInsensitive(text))
						{
							_result.Add(text);
						}
					}
				}
			}
		}
		if (_considerLoadedPrefab != null && !_considerLoadedPrefab.ThemeTags.IsEmpty)
		{
			foreach (string text2 in _considerLoadedPrefab.ThemeTags.GetTagNames())
			{
				if (!_result.ContainsCaseInsensitive(text2))
				{
					_result.Add(text2);
				}
			}
		}
	}

	// Token: 0x0600450B RID: 17675 RVA: 0x001B4F4C File Offset: 0x001B314C
	public void GetAllGroups(List<string> _result, Prefab _considerLoadedPrefab = null)
	{
		Dictionary<PathAbstractions.AbstractedLocation, Prefab> dictionary = this.loadedPrefabHeaders;
		lock (dictionary)
		{
			foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> keyValuePair in this.loadedPrefabHeaders)
			{
				if (keyValuePair.Value.editorGroups != null)
				{
					foreach (string text in keyValuePair.Value.editorGroups)
					{
						if (!_result.ContainsCaseInsensitive(text))
						{
							_result.Add(text);
						}
					}
				}
			}
		}
		if (((_considerLoadedPrefab != null) ? _considerLoadedPrefab.editorGroups : null) != null)
		{
			foreach (string text2 in _considerLoadedPrefab.editorGroups)
			{
				if (!_result.ContainsCaseInsensitive(text2))
				{
					_result.Add(text2);
				}
			}
		}
	}

	// Token: 0x0600450C RID: 17676 RVA: 0x001B5080 File Offset: 0x001B3280
	public void GetAllZones(List<string> _result, Prefab _considerLoadedPrefab = null)
	{
		Dictionary<PathAbstractions.AbstractedLocation, Prefab> dictionary = this.loadedPrefabHeaders;
		lock (dictionary)
		{
			foreach (KeyValuePair<PathAbstractions.AbstractedLocation, Prefab> keyValuePair in this.loadedPrefabHeaders)
			{
				string[] allowedZones = keyValuePair.Value.GetAllowedZones();
				if (allowedZones != null)
				{
					foreach (string text in allowedZones)
					{
						if (!_result.ContainsCaseInsensitive(text))
						{
							_result.Add(text);
						}
					}
				}
			}
		}
		if (_considerLoadedPrefab != null)
		{
			string[] allowedZones2 = _considerLoadedPrefab.GetAllowedZones();
			if (allowedZones2 != null)
			{
				foreach (string text2 in allowedZones2)
				{
					if (!_result.ContainsCaseInsensitive(text2))
					{
						_result.Add(text2);
					}
				}
			}
		}
	}

	// Token: 0x0600450D RID: 17677 RVA: 0x001B5174 File Offset: 0x001B3374
	public void GetAllQuestTags(List<string> _result, Prefab _considerLoadedPrefab = null)
	{
		string[] array;
		if (_considerLoadedPrefab != null)
		{
			array = _considerLoadedPrefab.GetQuestTags().ToString().Split(',', StringSplitOptions.None);
			Array.Sort<string>(array);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (!_result.ContainsCaseInsensitive(text))
				{
					_result.Add(text);
				}
			}
		}
		array = QuestEventManager.allQuestTags.ToString().Split(',', StringSplitOptions.None);
		Array.Sort<string>(array);
		for (int j = 0; j < array.Length; j++)
		{
			string text2 = array[j].Trim();
			if (!_result.ContainsCaseInsensitive(text2))
			{
				_result.Add(text2);
			}
		}
	}

	// Token: 0x0600450E RID: 17678 RVA: 0x001B521C File Offset: 0x001B341C
	public bool HasPrefabImposter(PathAbstractions.AbstractedLocation _location)
	{
		return SdFile.Exists(_location.FullPathNoExtension + ".mesh");
	}

	// Token: 0x0600450F RID: 17679 RVA: 0x001B5234 File Offset: 0x001B3434
	public void ClearImposterPrefab()
	{
		this.ClearEditorImposterSigns();
		global::UnityEngine.Object.Destroy(this.ImposterPrefab);
		this.ImposterPrefab = null;
	}

	// Token: 0x06004510 RID: 17680 RVA: 0x001B5250 File Offset: 0x001B3450
	public bool LoadImposterPrefab(PathAbstractions.AbstractedLocation _location)
	{
		this.ClearImposterPrefab();
		this.ClearVoxelPrefab();
		string text = _location.FullPathNoExtension + ".mesh";
		if (!SdFile.Exists(text))
		{
			return false;
		}
		this.LoadedPrefab = _location;
		bool bTextureArray = MeshDescription.meshes[0].bTextureArray;
		SimpleMeshInfo simpleMeshInfo;
		this.ImposterPrefab = SimpleMeshFile.ReadGameObject(text, out simpleMeshInfo, 0f, null, bTextureArray, false, null, null);
		this.ImposterPrefab.transform.name = _location.Name;
		this.ImposterPrefab.transform.position = new Vector3(0f, -3f, 0f);
		this.LoadEditorImposterSigns(simpleMeshInfo);
		return true;
	}

	// Token: 0x06004511 RID: 17681 RVA: 0x001B52F4 File Offset: 0x001B34F4
	public void LoadEditorImposterSigns(SimpleMeshInfo _meshInfo)
	{
		if (((_meshInfo != null) ? _meshInfo.signs : null) == null || _meshInfo.signs.Count == 0)
		{
			return;
		}
		if (this.ImposterSignData == null)
		{
			this.ImposterSignData = new List<ImposterCanvas>();
		}
		this.ImposterSignData.Clear();
		foreach (ImposterCanvas imposterCanvas in _meshInfo.signs)
		{
			ImposterSignFactory.Create(this.ImposterPrefab, imposterCanvas);
			if (imposterCanvas.Canvas != null)
			{
				this.ImposterSignData.Add(imposterCanvas);
			}
		}
	}

	// Token: 0x06004512 RID: 17682 RVA: 0x001B53A4 File Offset: 0x001B35A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearEditorImposterSigns()
	{
		if (this.ImposterSignData != null)
		{
			foreach (ImposterCanvas imposterCanvas in this.ImposterSignData)
			{
				ImposterSignFactory.Destroy(imposterCanvas);
			}
			this.ImposterSignData = null;
		}
	}

	// Token: 0x06004513 RID: 17683 RVA: 0x001B5404 File Offset: 0x001B3604
	public bool IsShowingImposterPrefab()
	{
		return this.ImposterPrefab != null;
	}

	// Token: 0x06004514 RID: 17684 RVA: 0x001B5412 File Offset: 0x001B3612
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeAllChunks()
	{
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunks();
		GameManager.Instance.World.ChunkCache.Clear();
		WaterSimulationNative.Instance.Clear();
	}

	// Token: 0x06004515 RID: 17685 RVA: 0x001B5448 File Offset: 0x001B3648
	public void ClearVoxelPrefab()
	{
		SelectionBoxManager.Instance.Deactivate();
		SelectionBoxManager.Instance.CategoryDynamicPrefab.Clear();
		SelectionBoxManager.Instance.CategoryTraderTeleport.Clear();
		SelectionBoxManager.Instance.CategorySleeperVolume.Clear();
		SelectionBoxManager.Instance.CategoryInfoVolume.Clear();
		SelectionBoxManager.Instance.CategoryWallVolume.Clear();
		SelectionBoxManager.Instance.CategoryTriggerVolume.Clear();
		SelectionBoxManager.Instance.CategoryPOIMarker.Clear();
		SelectionBoxManager.Instance.CategoryPrefabFacingVolume.Clear();
		SleeperVolumeToolManager.CleanUp();
		POIMarkerToolManager.CleanUp();
		this.prefabInstanceId = -1;
		this.LoadedPrefab = PathAbstractions.AbstractedLocation.None;
		this.VoxelPrefab = null;
		this.removeAllChunks();
		DecoManager.Instance.OnWorldUnloaded();
		ThreadManager.RunCoroutineSync(DecoManager.Instance.OnWorldLoaded(1024, 1024, GameManager.Instance.World, null));
		this.bShowFacing = false;
		this.HighlightQuestLoot = this.HighlightQuestLoot;
		this.HighlightBlockTriggers = this.HighlightBlockTriggers;
		this.showCompositionGrid = false;
	}

	// Token: 0x06004516 RID: 17686 RVA: 0x001B5554 File Offset: 0x001B3754
	public bool NewVoxelPrefab()
	{
		this.ClearImposterPrefab();
		this.ClearVoxelPrefab();
		this.VoxelPrefab = new Prefab();
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		if (this.prefabInstanceId != -1)
		{
			dynamicPrefabDecorator.RemovePrefabAndSelection(GameManager.Instance.World, dynamicPrefabDecorator.GetPrefab(this.prefabInstanceId), false);
		}
		dynamicPrefabDecorator.ClearAllPrefabs();
		this.prefabInstanceId = dynamicPrefabDecorator.CreateNewPrefabAndActivate(Prefab.LocationForNewPrefab("New Prefab", null), Vector3i.zero, this.VoxelPrefab, true).id;
		SelectionBoxManager.Instance.CategoryDynamicPrefab.SetVisible(false);
		this.curGridYPos = this.VoxelPrefab.yOffset;
		if (this.groundGrid)
		{
			this.groundGrid.transform.position = new Vector3(0f, (float)(1 - this.curGridYPos) - 0.01f, 0f);
		}
		this.ToggleGroundGrid(true);
		for (int i = -10; i <= 10; i++)
		{
			for (int j = -10; j <= 10; j++)
			{
				Chunk chunk = MemoryPools.PoolChunks.AllocSync(true);
				chunk.X = i;
				chunk.Z = j;
				chunk.ResetBiomeIntensity(BiomeIntensity.Default);
				chunk.NeedsRegeneration = true;
				chunk.NeedsLightCalculation = false;
				chunk.NeedsDecoration = false;
				chunk.ResetLights(byte.MaxValue);
				GameManager.Instance.World.ChunkCache.AddChunkSync(chunk, false);
				WaterSimulationNative.Instance.InitializeChunk(chunk);
			}
		}
		this.NeedsSaving = false;
		GamePrefs.Set(EnumGamePrefs.LastLoadedPrefab, string.Empty);
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
		WaterSimulationNative.Instance.SetPaused(true);
		return true;
	}

	// Token: 0x06004517 RID: 17687 RVA: 0x001B56FC File Offset: 0x001B38FC
	public bool LoadVoxelPrefab(PathAbstractions.AbstractedLocation _location, bool _bBulk = false, bool _bIgnoreExcludeImposterCheck = false)
	{
		this.ClearImposterPrefab();
		this.ClearVoxelPrefab();
		this.highlightBlocks(0);
		if (_location.Type == PathAbstractions.EAbstractedLocationType.None)
		{
			Log.Out("No prefab found to load!");
			return false;
		}
		this.VoxelPrefab = new Prefab();
		if (!this.VoxelPrefab.Load(_location, true, true, true, false))
		{
			Log.Out(string.Format("Error loading prefab {0}", _location));
			this.VoxelPrefab = null;
			return false;
		}
		if (!_bIgnoreExcludeImposterCheck && _bBulk && this.VoxelPrefab.bExcludeDistantPOIMesh)
		{
			this.VoxelPrefab = null;
			return false;
		}
		int num = this.VoxelPrefab.size.x * this.VoxelPrefab.size.y * this.VoxelPrefab.size.z;
		if (!_bIgnoreExcludeImposterCheck && _bBulk && ((this.VoxelPrefab.size.y <= 6 && num < 1500) || (this.VoxelPrefab.size.y > 6 && num < 100)))
		{
			this.VoxelPrefab = null;
			return false;
		}
		this.LoadedPrefab = _location;
		this.curGridYPos = this.VoxelPrefab.yOffset;
		if (this.groundGrid)
		{
			this.groundGrid.transform.position = new Vector3(0f, (float)(1 - this.curGridYPos) - 0.01f, 0f);
		}
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		this.removeAllChunks();
		int num2 = -1 * this.VoxelPrefab.size.x / 2;
		int num3 = -1 * this.VoxelPrefab.size.z / 2;
		int num4 = num2 + this.VoxelPrefab.size.x;
		int num5 = num3 + this.VoxelPrefab.size.z;
		chunkCache.ChunkMinPos = new Vector2i((num2 - 1) / 16 - 1, (num3 - 1) / 16 - 1);
		chunkCache.ChunkMinPos -= new Vector2i(2, 2);
		chunkCache.ChunkMaxPos = new Vector2i(num4 / 16 + 1, num5 / 16 + 1);
		chunkCache.ChunkMaxPos += new Vector2i(2, 2);
		List<Chunk> list = new List<Chunk>();
		for (int i = chunkCache.ChunkMinPos.x; i <= chunkCache.ChunkMaxPos.x; i++)
		{
			for (int j = chunkCache.ChunkMinPos.y; j <= chunkCache.ChunkMaxPos.y; j++)
			{
				Chunk chunk = MemoryPools.PoolChunks.AllocSync(true);
				chunk.X = i;
				chunk.Z = j;
				chunk.SetFullSunlight();
				chunk.NeedsLightCalculation = false;
				chunk.NeedsDecoration = false;
				chunk.NeedsRegeneration = false;
				chunkCache.AddChunkSync(chunk, true);
				list.Add(chunk);
			}
		}
		Vector3i vector3i = new Vector3i(num2, 1, num3);
		this.VoxelPrefab.CopyIntoLocal(chunkCache, vector3i, true, false, FastTags<TagGroup.Global>.none);
		for (int k = 0; k < list.Count; k++)
		{
			Chunk chunk2 = list[k];
			chunk2.NeedsLightCalculation = false;
			chunk2.NeedsRegeneration = true;
			WaterSimulationNative.Instance.InitializeChunk(chunk2);
		}
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		if (this.prefabInstanceId != -1)
		{
			dynamicPrefabDecorator.RemovePrefabAndSelection(GameManager.Instance.World, dynamicPrefabDecorator.GetPrefab(this.prefabInstanceId), false);
		}
		dynamicPrefabDecorator.ClearAllPrefabs();
		this.prefabInstanceId = dynamicPrefabDecorator.CreateNewPrefabAndActivate(this.VoxelPrefab.location, vector3i, this.VoxelPrefab, false).id;
		this.NeedsSaving = false;
		GamePrefs.Set(EnumGamePrefs.LastLoadedPrefab, _location.Name);
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
		this.HighlightQuestLoot = this.HighlightQuestLoot;
		this.HighlightBlockTriggers = this.HighlightBlockTriggers;
		WaterSimulationNative.Instance.SetPaused(true);
		this.highlightBlocks(this.highlightBlockId);
		return true;
	}

	// Token: 0x06004518 RID: 17688 RVA: 0x001B5AE4 File Offset: 0x001B3CE4
	public bool SaveVoxelPrefab()
	{
		if (this.VoxelPrefab == null)
		{
			return false;
		}
		bool ignorePaintTextures = Chunk.IgnorePaintTextures;
		Chunk.IgnorePaintTextures = false;
		this.updatePrefabBounds();
		Chunk.IgnorePaintTextures = ignorePaintTextures;
		EnumInsideOutside[] array = this.VoxelPrefab.UpdateInsideOutside(this.minPos, this.maxPos);
		this.VoxelPrefab.RecalcInsideDevices(array);
		bool flag = this.VoxelPrefab.Save(this.VoxelPrefab.location, true);
		if (flag)
		{
			this.LoadedPrefab = this.VoxelPrefab.location;
			this.LoadXml(this.VoxelPrefab.location);
			GamePrefs.Set(EnumGamePrefs.LastLoadedPrefab, this.VoxelPrefab.PrefabName);
		}
		this.NeedsSaving = false;
		return flag;
	}

	// Token: 0x06004519 RID: 17689 RVA: 0x001B5B90 File Offset: 0x001B3D90
	public void UpdateMinMax()
	{
		if (this.VoxelPrefab == null)
		{
			return;
		}
		Vector3i zero = new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);
		Vector3i zero2 = new Vector3i(int.MinValue, int.MinValue, int.MinValue);
		foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync())
		{
			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					for (int k = 0; k < 256; k++)
					{
						int blockId = chunk.GetBlockId(i, k, j);
						WaterValue water = chunk.GetWater(i, k, j);
						if (blockId != 0 || water.HasMass() || chunk.GetDensity(i, k, j) < 0)
						{
							Vector3i vector3i = chunk.ToWorldPos(new Vector3i(i, k, j));
							if (zero.x > vector3i.x)
							{
								zero.x = vector3i.x;
							}
							if (zero.y > vector3i.y)
							{
								zero.y = vector3i.y;
							}
							if (zero.z > vector3i.z)
							{
								zero.z = vector3i.z;
							}
							if (zero2.x < vector3i.x)
							{
								zero2.x = vector3i.x;
							}
							if (zero2.y < vector3i.y)
							{
								zero2.y = vector3i.y;
							}
							if (zero2.z < vector3i.z)
							{
								zero2.z = vector3i.z;
							}
						}
					}
				}
			}
		}
		if (zero.x == 2147483647)
		{
			zero = Vector3i.zero;
			zero2 = Vector3i.zero;
		}
		this.minPos = zero;
		this.maxPos = zero2;
	}

	// Token: 0x0600451A RID: 17690 RVA: 0x001B5D9C File Offset: 0x001B3F9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePrefabBounds()
	{
		if (this.VoxelPrefab == null)
		{
			return;
		}
		this.VoxelPrefab.yOffset = this.curGridYPos;
		this.UpdateMinMax();
		this.VoxelPrefab.CopyFromWorldWithEntities(GameManager.Instance.World, this.minPos, this.maxPos, new List<int>());
		if (this.prefabInstanceId != -1)
		{
			GameManager.Instance.GetDynamicPrefabDecorator().GetPrefab(this.prefabInstanceId).UpdateBoundingBoxPosAndScale(this.minPos, this.VoxelPrefab.size, true);
		}
	}

	// Token: 0x0600451B RID: 17691 RVA: 0x001B5E28 File Offset: 0x001B4028
	public void SetGroundLevel(int _yOffset)
	{
		this.curGridYPos = _yOffset;
		if (this.groundGrid)
		{
			this.groundGrid.transform.position = new Vector3(0f, (float)(1 - this.curGridYPos) - 0.01f, 0f);
		}
		if (this.VoxelPrefab != null)
		{
			this.VoxelPrefab.yOffset = this.curGridYPos;
			if (this.prefabInstanceId != -1 && this.OnPrefabChanged != null)
			{
				PrefabInstance prefab = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefab(this.prefabInstanceId);
				this.OnPrefabChanged(prefab);
			}
			this.NeedsSaving = true;
		}
	}

	// Token: 0x0600451C RID: 17692 RVA: 0x001B5ECA File Offset: 0x001B40CA
	public void MoveGroundGridUpOrDown(int _deltaY)
	{
		this.SetGroundLevel(Utils.FastClamp(this.curGridYPos - _deltaY, -200, 0));
	}

	// Token: 0x0600451D RID: 17693 RVA: 0x001B5EE8 File Offset: 0x001B40E8
	public void ToggleGroundGrid(bool _bForceOn = false)
	{
		if (this.groundGrid == null)
		{
			this.groundGrid = global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GroundGrid/GroundGrid"));
			this.groundGrid.transform.position = new Vector3(0f, (float)(1 - this.curGridYPos) + 0.01f, 0f);
			for (int i = 0; i < this.groundGrid.transform.childCount; i++)
			{
				this.groundGrid.transform.GetChild(i).gameObject.tag = "B_Mesh";
			}
			return;
		}
		this.groundGrid.SetActive(_bForceOn || !this.groundGrid.activeSelf);
	}

	// Token: 0x0600451E RID: 17694 RVA: 0x001B5FA1 File Offset: 0x001B41A1
	public bool IsGroundGrid()
	{
		return this.groundGrid != null && this.groundGrid.activeSelf;
	}

	// Token: 0x0600451F RID: 17695 RVA: 0x001B5FC0 File Offset: 0x001B41C0
	public void ToggleCompositionGrid()
	{
		this.showCompositionGrid = !this.showCompositionGrid;
		if (!this.showCompositionGrid)
		{
			return;
		}
		SelectionCategory categoryDynamicPrefab = SelectionBoxManager.Instance.CategoryDynamicPrefab;
		bool flag = categoryDynamicPrefab.IsVisible();
		this.UpdatePrefabBounds();
		this.compositionGridMin = this.minPos;
		this.compositionGridMax = this.maxPos;
		this.compositionGridGroundLevel = this.curGridYPos;
		if (!flag)
		{
			categoryDynamicPrefab.SetVisible(false);
		}
	}

	// Token: 0x06004520 RID: 17696 RVA: 0x001B6029 File Offset: 0x001B4229
	public bool IsCompositionGrid()
	{
		return this.showCompositionGrid && this.IsActive();
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x001B603C File Offset: 0x001B423C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateCompositionGrid()
	{
		if (!this.showCompositionGrid)
		{
			return;
		}
		Color color = new Color32(0, byte.MaxValue, 40, byte.MaxValue);
		Color color2 = new Color32(0, byte.MaxValue, 200, byte.MaxValue);
		Color color3 = new Color32(0, byte.MaxValue, 200, byte.MaxValue);
		Vector3i vector3i = this.compositionGridMin;
		Vector3i vector3i2 = this.compositionGridMax + Vector3i.one;
		Vector3i vector3i3 = vector3i2 - vector3i;
		if (vector3i3 == Vector3i.one)
		{
			return;
		}
		World world = GameManager.Instance.World;
		EntityPlayerLocal entityPlayerLocal = ((world != null) ? world.GetPrimaryPlayer() : null);
		if (entityPlayerLocal == null)
		{
			return;
		}
		float num = (float)(1 - this.compositionGridGroundLevel) + 0.05f;
		float num2 = (float)vector3i3.x / 1.618034f;
		int num3 = Mathf.RoundToInt(num2);
		int num4 = Mathf.RoundToInt(((float)vector3i3.x - num2) / 2f);
		float num5 = (float)vector3i3.z / 1.618034f;
		int num6 = Mathf.RoundToInt(num5);
		int num7 = Mathf.RoundToInt(((float)vector3i3.z - num5) / 2f);
		float num8 = (float)vector3i3.x / 2f;
		float num9 = (float)vector3i3.z / 2f;
		Vector3 vector = new Vector3((float)(vector3i.x + num3), num, (float)vector3i.z);
		Vector3 vector2 = new Vector3((float)(vector3i.x + num3), num, (float)vector3i2.z);
		DebugLines.Create("GoldenRatio_X1", entityPlayerLocal.RootTransform, vector, vector2, color, color, 0.1f, 0.1f, 0.1f);
		vector = new Vector3((float)(vector3i2.x - num3), num, (float)vector3i.z);
		vector2 = new Vector3((float)(vector3i2.x - num3), num, (float)vector3i2.z);
		DebugLines.Create("GoldenRatio_X2", entityPlayerLocal.RootTransform, vector, vector2, color, color, 0.1f, 0.1f, 0.1f);
		vector = new Vector3((float)vector3i.x, num, (float)(vector3i.z + num6));
		vector2 = new Vector3((float)vector3i2.x, num, (float)(vector3i.z + num6));
		DebugLines.Create("GoldenRatio_Z1", entityPlayerLocal.RootTransform, vector, vector2, color, color, 0.1f, 0.1f, 0.1f);
		vector = new Vector3((float)vector3i.x, num, (float)(vector3i2.z - num6));
		vector2 = new Vector3((float)vector3i2.x, num, (float)(vector3i2.z - num6));
		DebugLines.Create("GoldenRatio_Z2", entityPlayerLocal.RootTransform, vector, vector2, color, color, 0.1f, 0.1f, 0.1f);
		vector = new Vector3((float)vector3i.x + num8, num, (float)vector3i.z);
		vector2 = new Vector3((float)vector3i.x + num8, num, (float)vector3i2.z);
		DebugLines.Create("GoldenRatio_InnerX", entityPlayerLocal.RootTransform, vector, vector2, color2, color2, 0.03f, 0.03f, 0.1f);
		vector = new Vector3((float)vector3i.x, num, (float)vector3i.z + num9);
		vector2 = new Vector3((float)vector3i2.x, num, (float)vector3i.z + num9);
		DebugLines.Create("GoldenRatio_InnerZ", entityPlayerLocal.RootTransform, vector, vector2, color2, color2, 0.03f, 0.03f, 0.1f);
		vector = new Vector3((float)(vector3i.x + num4), num, (float)vector3i.z);
		vector2 = new Vector3((float)(vector3i.x + num4), num, (float)vector3i2.z);
		DebugLines.Create("GoldenRatio_OuterX1", entityPlayerLocal.RootTransform, vector, vector2, color3, color3, 0.03f, 0.03f, 0.1f);
		vector = new Vector3((float)(vector3i2.x - num4), num, (float)vector3i.z);
		vector2 = new Vector3((float)(vector3i2.x - num4), num, (float)vector3i2.z);
		DebugLines.Create("GoldenRatio_OuterX2", entityPlayerLocal.RootTransform, vector, vector2, color3, color3, 0.03f, 0.03f, 0.1f);
		vector = new Vector3((float)vector3i.x, num, (float)(vector3i.z + num7));
		vector2 = new Vector3((float)vector3i2.x, num, (float)(vector3i.z + num7));
		DebugLines.Create("GoldenRatio_OuterZ1", entityPlayerLocal.RootTransform, vector, vector2, color3, color3, 0.03f, 0.03f, 0.1f);
		vector = new Vector3((float)vector3i.x, num, (float)(vector3i2.z - num7));
		vector2 = new Vector3((float)vector3i2.x, num, (float)(vector3i2.z - num7));
		DebugLines.Create("GoldenRatio_OuterZ2", entityPlayerLocal.RootTransform, vector, vector2, color3, color3, 0.03f, 0.03f, 0.1f);
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x001B6528 File Offset: 0x001B4728
	public void UpdatePrefabBounds()
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		if (this.prefabInstanceId == -1)
		{
			this.VoxelPrefab = new Prefab();
			this.prefabInstanceId = dynamicPrefabDecorator.CreateNewPrefabAndActivate(this.VoxelPrefab.location, Vector3i.zero, this.VoxelPrefab, true).id;
		}
		if (this.prefabInstanceId != -1)
		{
			SelectionBoxManager.Instance.CategoryDynamicPrefab.SetVisible(true);
		}
		this.updatePrefabBounds();
	}

	// Token: 0x06004523 RID: 17699 RVA: 0x001B659B File Offset: 0x001B479B
	public bool IsPrefabFacing()
	{
		return this.bShowFacing;
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x001B65A4 File Offset: 0x001B47A4
	public void TogglePrefabFacing(bool _bShow)
	{
		if (this.VoxelPrefab == null)
		{
			return;
		}
		this.bShowFacing = _bShow;
		this.updateFacing();
		this.boxShowFacing.SetPositionAndSize(new Vector3i(0, 2, -this.VoxelPrefab.size.z / 2 - 3), Vector3i.one);
		SelectionBoxManager.Instance.SetActive(this.boxShowFacing, this.bShowFacing);
		SelectionBoxManager.Instance.CategoryPrefabFacingVolume.SetVisible(this.bShowFacing);
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x001B661E File Offset: 0x001B481E
	public void RotatePrefabFacing()
	{
		if (this.VoxelPrefab == null)
		{
			return;
		}
		this.VoxelPrefab.rotationToFaceNorth++;
		this.VoxelPrefab.rotationToFaceNorth &= 3;
		this.updateFacing();
		this.NeedsSaving = true;
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x001B665C File Offset: 0x001B485C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateFacing()
	{
		if (this.VoxelPrefab == null)
		{
			return;
		}
		float num = 0f;
		switch (this.VoxelPrefab.rotationToFaceNorth)
		{
		case 1:
			num = 90f;
			break;
		case 2:
			num = 180f;
			break;
		case 3:
			num = 270f;
			break;
		}
		this.boxShowFacing.FacingDirection = num;
	}

	// Token: 0x06004527 RID: 17703 RVA: 0x001B66BC File Offset: 0x001B48BC
	public void MovePrefabUpOrDown(int _deltaY)
	{
		this.updatePrefabBounds();
		Vector3i vector3i = this.minPos + _deltaY * Vector3i.up;
		if (vector3i.y < 1 || this.maxPos.y + _deltaY > 250)
		{
			return;
		}
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		List<Chunk> chunkArrayCopySync = chunkCache.GetChunkArrayCopySync();
		foreach (Chunk chunk in chunkArrayCopySync)
		{
			chunk.RemoveAllTileEntities();
			if (!chunk.IsEmpty())
			{
				for (int i = 0; i < 16; i++)
				{
					for (int j = 0; j < 16; j++)
					{
						for (int k = 0; k < 254; k++)
						{
							chunk.SetWater(i, k, j, WaterValue.Empty);
							BlockValue block = chunk.GetBlock(i, k, j);
							if (!block.isair && !block.ischild)
							{
								chunkCache.SetBlock(chunk.ToWorldPos(new Vector3i(i, k, j)), BlockValue.Air, true, false);
								chunk.SetDensity(i, k, j, MarchingCubes.DensityAir);
								chunk.SetTextureFull(i, k, j, 0L, 0);
							}
						}
					}
				}
			}
		}
		this.VoxelPrefab.CopyIntoLocal(chunkCache, vector3i, true, false, FastTags<TagGroup.Global>.none);
		foreach (Chunk chunk2 in chunkArrayCopySync)
		{
			chunk2.NeedsRegeneration = true;
		}
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
		this.UpdateMinMax();
		if (this.prefabInstanceId != -1)
		{
			GameManager.Instance.GetDynamicPrefabDecorator().GetPrefab(this.prefabInstanceId).UpdateBoundingBoxPosAndScale(this.minPos, this.VoxelPrefab.size, false);
		}
	}

	// Token: 0x06004528 RID: 17704 RVA: 0x001B68D0 File Offset: 0x001B4AD0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool checkLayerEmpty(int _y, List<Chunk> chunks)
	{
		bool bAllEmpty = true;
		ChunkBlockLayer.LoopBlocksDelegate <>9__0;
		for (int i = 0; i < chunks.Count; i++)
		{
			Chunk chunk = chunks[i];
			ChunkBlockLayer.LoopBlocksDelegate loopBlocksDelegate;
			if ((loopBlocksDelegate = <>9__0) == null)
			{
				loopBlocksDelegate = (<>9__0 = delegate(int x, int y, int z, BlockValue bv)
				{
					if (y == _y)
					{
						bAllEmpty = false;
					}
				});
			}
			chunk.LoopOverAllBlocks(loopBlocksDelegate, false, false);
		}
		return bAllEmpty;
	}

	// Token: 0x06004529 RID: 17705 RVA: 0x001B6938 File Offset: 0x001B4B38
	public void StripTextures()
	{
		HashSet<Chunk> changedChunks = new HashSet<Chunk>();
		List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
		bool bUseSelection = BlockToolSelection.Instance.SelectionActive;
		Vector3i selStart = BlockToolSelection.Instance.SelectionMin;
		Vector3i selEnd = selStart + BlockToolSelection.Instance.SelectionSize - Vector3i.one;
		for (int i = 0; i < chunkArrayCopySync.Count; i++)
		{
			Chunk chunk = chunkArrayCopySync[i];
			chunk.LoopOverAllBlocks(delegate(int x, int y, int z, BlockValue bv)
			{
				if (bUseSelection)
				{
					Vector3i vector3i = chunk.ToWorldPos(new Vector3i(x, y, z));
					if (vector3i.x < selStart.x || vector3i.x > selEnd.x || vector3i.y < selStart.y || vector3i.y > selEnd.y || vector3i.z < selStart.z || vector3i.z > selEnd.z)
					{
						return;
					}
				}
				for (int j = 0; j < 1; j++)
				{
					if (chunk.GetTextureFull(x, y, z, j) != 0L)
					{
						chunk.SetTextureFull(x, y, z, 0L, j);
						changedChunks.Add(chunk);
					}
				}
			}, false, false);
		}
		foreach (Chunk chunk2 in changedChunks)
		{
			chunk2.NeedsRegeneration = true;
		}
		if (changedChunks.Count > 0)
		{
			this.NeedsSaving = true;
		}
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
	}

	// Token: 0x0600452A RID: 17706 RVA: 0x001B6A64 File Offset: 0x001B4C64
	public void StripInternalTextures()
	{
		HashSet<Chunk> changedChunks = new HashSet<Chunk>();
		World world = GameManager.Instance.World;
		List<Chunk> chunkArrayCopySync = world.ChunkCache.GetChunkArrayCopySync();
		bool bUseSelection = BlockToolSelection.Instance.SelectionActive;
		Vector3i selStart = BlockToolSelection.Instance.SelectionMin;
		Vector3i selEnd = selStart + BlockToolSelection.Instance.SelectionSize - Vector3i.one;
		for (int i = 0; i < chunkArrayCopySync.Count; i++)
		{
			Chunk chunk = chunkArrayCopySync[i];
			chunk.LoopOverAllBlocks(delegate(int x, int y, int z, BlockValue bv)
			{
				if (bUseSelection)
				{
					Vector3i vector3i = chunk.ToWorldPos(new Vector3i(x, y, z));
					if (vector3i.x < selStart.x || vector3i.x > selEnd.x || vector3i.y < selStart.y || vector3i.y > selEnd.y || vector3i.z < selStart.z || vector3i.z > selEnd.z)
					{
						return;
					}
				}
				if (chunk.GetTextureFull(x, y, z, 0) == 0L)
				{
					return;
				}
				BlockShapeNew blockShapeNew = bv.Block.shape as BlockShapeNew;
				if (blockShapeNew == null)
				{
					return;
				}
				for (int j = 0; j < 6; j++)
				{
					BlockFace blockFace = (BlockFace)j;
					Vector3i vector3i2 = new Vector3i(BlockFaceFlags.OffsetForFace(blockFace));
					Vector3i vector3i3 = chunk.ToWorldPos(new Vector3i(x, y, z)) + vector3i2;
					BlockValue block = world.GetBlock(vector3i3);
					if (!block.isair)
					{
						BlockShapeNew blockShapeNew2 = block.Block.shape as BlockShapeNew;
						if (blockShapeNew2 != null)
						{
							BlockFace blockFace2 = BlockFaceFlags.OppositeFace(blockFace);
							BlockShapeNew.EnumFaceOcclusionInfo faceInfo = blockShapeNew.GetFaceInfo(bv, blockFace);
							BlockShapeNew.EnumFaceOcclusionInfo faceInfo2 = blockShapeNew2.GetFaceInfo(block, blockFace2);
							if ((faceInfo == BlockShapeNew.EnumFaceOcclusionInfo.Full && faceInfo2 == BlockShapeNew.EnumFaceOcclusionInfo.Full) || (faceInfo == BlockShapeNew.EnumFaceOcclusionInfo.Part && faceInfo2 == BlockShapeNew.EnumFaceOcclusionInfo.Full) || (faceInfo == BlockShapeNew.EnumFaceOcclusionInfo.Part && faceInfo2 == BlockShapeNew.EnumFaceOcclusionInfo.Part && blockShapeNew == blockShapeNew2 && bv.rotation == block.rotation))
							{
								blockFace = blockShapeNew.GetRotatedBlockFace(bv, blockFace);
								for (int k = 0; k < 1; k++)
								{
									world.ChunkCache.SetBlockFaceTexture(chunk.ToWorldPos(new Vector3i(x, y, z)), blockFace, 0, k);
								}
								changedChunks.Add(chunk);
							}
						}
					}
				}
			}, false, false);
		}
		foreach (Chunk chunk2 in changedChunks)
		{
			chunk2.NeedsRegeneration = true;
		}
		if (changedChunks.Count > 0)
		{
			this.NeedsSaving = true;
		}
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
	}

	// Token: 0x0600452B RID: 17707 RVA: 0x001B6B9C File Offset: 0x001B4D9C
	public void GetLootAndFetchLootContainerCount(out int _loot, out int _fetchLoot, out int _restorePower)
	{
		int tempLoot = 0;
		int tempFetchLoot = 0;
		int tempRestorePower = 0;
		List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
		ChunkBlockLayer.LoopBlocksDelegate <>9__0;
		for (int i = 0; i < chunkArrayCopySync.Count; i++)
		{
			Chunk chunk = chunkArrayCopySync[i];
			ChunkBlockLayer.LoopBlocksDelegate loopBlocksDelegate;
			if ((loopBlocksDelegate = <>9__0) == null)
			{
				loopBlocksDelegate = (<>9__0 = delegate(int _, int _, int _, BlockValue bv)
				{
					Block block = bv.Block;
					if (block == null)
					{
						return;
					}
					bool flag = block.IndexName != null;
					if (flag && block.IndexName == Constants.cQuestLootFetchContainerIndexName)
					{
						int num = tempFetchLoot;
						tempFetchLoot = num + 1;
						return;
					}
					if (flag && block.IndexName == Constants.cQuestRestorePowerIndexName)
					{
						int num = tempRestorePower;
						tempRestorePower = num + 1;
						return;
					}
					BlockCompositeTileEntity blockCompositeTileEntity = block as BlockCompositeTileEntity;
					if (blockCompositeTileEntity != null && blockCompositeTileEntity.CompositeData.HasFeature<TEFeatureStorage>())
					{
						int num = tempLoot;
						tempLoot = num + 1;
					}
				});
			}
			chunk.LoopOverAllBlocks(loopBlocksDelegate, false, false);
		}
		_loot = tempLoot;
		_fetchLoot = tempFetchLoot;
		_restorePower = tempRestorePower;
	}

	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x0600452C RID: 17708 RVA: 0x001B6C2F File Offset: 0x001B4E2F
	// (set) Token: 0x0600452D RID: 17709 RVA: 0x001B6C37 File Offset: 0x001B4E37
	public bool HighlightingBlocks
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600452E RID: 17710 RVA: 0x001B6C40 File Offset: 0x001B4E40
	public void HighlightBlocks(Block _blockClass)
	{
		this.highlightBlockId = ((_blockClass != null) ? _blockClass.blockID : 0);
		this.HighlightingBlocks = this.highlightBlockId > 0;
		this.highlightBlocks(this.highlightBlockId);
	}

	// Token: 0x0600452F RID: 17711 RVA: 0x001B6C6F File Offset: 0x001B4E6F
	public void ToggleHighlightBlocks()
	{
		this.HighlightingBlocks = !this.HighlightingBlocks;
		this.highlightBlocks(this.HighlightingBlocks ? this.highlightBlockId : 0);
	}

	// Token: 0x06004530 RID: 17712 RVA: 0x001B6C98 File Offset: 0x001B4E98
	[PublicizedFrom(EAccessModifier.Private)]
	public void highlightBlocks(int _blockId)
	{
		BlockHighlighter.Cleanup();
		if (!this.HighlightingBlocks || _blockId <= 0)
		{
			return;
		}
		List<Chunk> chunkArrayCopySync = GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync();
		for (int i = 0; i < chunkArrayCopySync.Count; i++)
		{
			Chunk chunk = chunkArrayCopySync[i];
			chunk.LoopOverAllBlocks(delegate(int _x, int _y, int _z, BlockValue _bv)
			{
				if (_bv.type == _blockId)
				{
					BlockHighlighter.AddBlock(chunk.worldPosIMin + new Vector3i(_x, _y, _z));
				}
			}, false, false);
		}
	}

	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x06004531 RID: 17713 RVA: 0x001B6D22 File Offset: 0x001B4F22
	// (set) Token: 0x06004532 RID: 17714 RVA: 0x001B6D2C File Offset: 0x001B4F2C
	public bool HighlightQuestLoot
	{
		get
		{
			return this.bShowQuestLoot;
		}
		set
		{
			this.bShowQuestLoot = value;
			NavObjectManager.Instance.UnRegisterNavObjectByClass("editor_quest_loot_container");
			if (value)
			{
				foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync())
				{
					List<Vector3i> list;
					if (chunk.IndexedBlocks.TryGetValue(Constants.cQuestLootFetchContainerIndexName, out list))
					{
						Vector3i worldPos = chunk.GetWorldPos();
						foreach (Vector3i vector3i in list)
						{
							NavObjectManager.Instance.RegisterNavObject("editor_quest_loot_container", (worldPos + vector3i).ToVector3Center(), "", false, -1, null);
						}
					}
				}
			}
		}
	}

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x06004533 RID: 17715 RVA: 0x001B6E24 File Offset: 0x001B5024
	// (set) Token: 0x06004534 RID: 17716 RVA: 0x001B6E2C File Offset: 0x001B502C
	public bool HighlightBlockTriggers
	{
		get
		{
			return this.bShowBlockTriggers;
		}
		set
		{
			this.bShowBlockTriggers = value;
			NavObjectManager.Instance.UnRegisterNavObjectByClass("editor_block_trigger");
			if (value)
			{
				foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArrayCopySync())
				{
					List<BlockTrigger> list = chunk.GetBlockTriggers().list;
					Vector3i worldPos = chunk.GetWorldPos();
					for (int i = 0; i < list.Count; i++)
					{
						NavObject navObject = NavObjectManager.Instance.RegisterNavObject("editor_block_trigger", (worldPos + list[i].LocalChunkPos).ToVector3Center(), "", false, -1, null);
						navObject.name = list[i].TriggerDisplay();
						navObject.OverrideColor = ((list[i].TriggeredByIndices.Count > 0) ? Color.blue : Color.red);
					}
				}
			}
		}
	}

	// Token: 0x040037C6 RID: 14278
	public const int cGroundGridYDefault = 1;

	// Token: 0x040037C7 RID: 14279
	[PublicizedFrom(EAccessModifier.Private)]
	public const string SingleFacingBoxName = "single";

	// Token: 0x040037C8 RID: 14280
	public static PrefabEditModeManager Instance;

	// Token: 0x040037CA RID: 14282
	public PathAbstractions.AbstractedLocation LoadedPrefab;

	// Token: 0x040037CB RID: 14283
	public Prefab VoxelPrefab;

	// Token: 0x040037CC RID: 14284
	public GameObject ImposterPrefab;

	// Token: 0x040037CD RID: 14285
	public List<ImposterCanvas> ImposterSignData;

	// Token: 0x040037CE RID: 14286
	public bool NeedsSaving;

	// Token: 0x040037CF RID: 14287
	public Vector3i minPos;

	// Token: 0x040037D0 RID: 14288
	public Vector3i maxPos;

	// Token: 0x040037D1 RID: 14289
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<PathAbstractions.AbstractedLocation, Prefab> loadedPrefabHeaders = new Dictionary<PathAbstractions.AbstractedLocation, Prefab>();

	// Token: 0x040037D2 RID: 14290
	[PublicizedFrom(EAccessModifier.Private)]
	public int curGridYPos;

	// Token: 0x040037D3 RID: 14291
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject groundGrid;

	// Token: 0x040037D4 RID: 14292
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceId = -1;

	// Token: 0x040037D5 RID: 14293
	[PublicizedFrom(EAccessModifier.Private)]
	public FileSystemWatcher xmlWatcher;

	// Token: 0x040037D6 RID: 14294
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bShowFacing;

	// Token: 0x040037D7 RID: 14295
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bShowQuestLoot;

	// Token: 0x040037D8 RID: 14296
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bShowBlockTriggers;

	// Token: 0x040037D9 RID: 14297
	public List<byte> TriggerLayers = new List<byte>();

	// Token: 0x040037DA RID: 14298
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showCompositionGrid;

	// Token: 0x040037DB RID: 14299
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i compositionGridMin;

	// Token: 0x040037DC RID: 14300
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i compositionGridMax;

	// Token: 0x040037DD RID: 14301
	[PublicizedFrom(EAccessModifier.Private)]
	public int compositionGridGroundLevel;

	// Token: 0x040037DF RID: 14303
	[PublicizedFrom(EAccessModifier.Private)]
	public int highlightBlockId;
}
