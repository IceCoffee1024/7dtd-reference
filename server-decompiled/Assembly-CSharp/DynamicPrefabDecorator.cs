using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000912 RID: 2322
public class DynamicPrefabDecorator : IDynamicDecorator, ISelectionBoxCallback
{
	// Token: 0x1400004D RID: 77
	// (add) Token: 0x0600433B RID: 17211 RVA: 0x001A2C1C File Offset: 0x001A0E1C
	// (remove) Token: 0x0600433C RID: 17212 RVA: 0x001A2C54 File Offset: 0x001A0E54
	public event Action<PrefabInstance> OnPrefabLoaded;

	// Token: 0x1400004E RID: 78
	// (add) Token: 0x0600433D RID: 17213 RVA: 0x001A2C8C File Offset: 0x001A0E8C
	// (remove) Token: 0x0600433E RID: 17214 RVA: 0x001A2CC4 File Offset: 0x001A0EC4
	public event Action<PrefabInstance> OnPrefabChanged;

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x0600433F RID: 17215 RVA: 0x001A2CFC File Offset: 0x001A0EFC
	// (remove) Token: 0x06004340 RID: 17216 RVA: 0x001A2D34 File Offset: 0x001A0F34
	public event Action<PrefabInstance> OnPrefabRemoved;

	// Token: 0x06004341 RID: 17217 RVA: 0x001A2D6C File Offset: 0x001A0F6C
	public DynamicPrefabDecorator(PrefabCache _prefabCache)
	{
		this.prefabCache = _prefabCache;
	}

	// Token: 0x06004342 RID: 17218 RVA: 0x001A2E29 File Offset: 0x001A1029
	public IEnumerator Load(string _path, bool _skipBlockData = false)
	{
		if (!SdFile.Exists(_path + "/prefabs.xml"))
		{
			yield break;
		}
		MicroStopwatch msw = new MicroStopwatch(true);
		XmlFile xmlFile;
		try
		{
			this.id = 0;
			xmlFile = new XmlFile(_path, "prefabs", false, false);
		}
		catch (Exception ex)
		{
			Log.Error("Loading prefabs xml file for level '" + Path.GetFileName(_path) + "': " + ex.Message);
			Log.Exception(ex);
			yield break;
		}
		int i = 0;
		int totalPrefabs = xmlFile.XmlDoc.Root.Elements("decoration").Count<XElement>();
		LocalPlayerUI ui = LocalPlayerUI.primaryUI;
		bool progressWindowOpen = ui && ui.windowManager.IsWindowOpen(XUiC_ProgressWindow.ID);
		ConnectionManager instance = SingletonMonoBehaviour<ConnectionManager>.Instance;
		bool loadTraders = instance != null && instance.IsServer;
		foreach (XElement xelement in xmlFile.XmlDoc.Root.Elements("decoration"))
		{
			try
			{
				int num = i;
				i = num + 1;
				if (xelement.HasAttribute("name"))
				{
					string attribute = xelement.GetAttribute("name");
					Vector3i vector3i = Vector3i.Parse(xelement.GetAttribute("position"));
					bool flag;
					StringParsers.TryParseBool(xelement.GetAttribute("y_is_groundlevel"), out flag);
					byte b = 0;
					if (xelement.HasAttribute("rotation"))
					{
						b = byte.Parse(xelement.GetAttribute("rotation"));
					}
					Prefab prefabRotated = this.prefabCache.GetPrefabRotated(attribute, (int)b, true, true, false, _skipBlockData);
					if (prefabRotated == null)
					{
						Log.Warning("Could not load prefab '" + attribute + "'. Skipping it");
						continue;
					}
					if (flag)
					{
						vector3i.y += prefabRotated.yOffset;
					}
					if (prefabRotated.bTraderArea && loadTraders)
					{
						this.AddTrader(new TraderArea(vector3i, prefabRotated.size, prefabRotated.TraderAreaProtect, prefabRotated.TeleportVolumeList));
					}
					num = this.id;
					this.id = num + 1;
					PrefabInstance prefabInstance = new PrefabInstance(num, prefabRotated.location, vector3i, b, prefabRotated, 0);
					this.AddWorldPrefab(prefabInstance, prefabInstance.prefab.HasQuestTag());
				}
			}
			catch (Exception ex2)
			{
				Log.Error("Loading prefabs xml file for level '" + Path.GetFileName(_path) + "': " + ex2.Message);
				Log.Exception(ex2);
			}
			if (msw.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
			{
				if (progressWindowOpen)
				{
					XUiC_ProgressWindow.SetText(ui, string.Format(Localization.Get("uiLoadCreatingWorldPrefabs", false, null), Math.Min(100.0, 105.0 * (double)i / (double)totalPrefabs).ToString("0")), true);
				}
				yield return null;
				msw.ResetAndRestart();
			}
		}
		IEnumerator<XElement> enumerator = null;
		if (progressWindowOpen)
		{
			XUiC_ProgressWindow.SetText(ui, string.Format(Localization.Get("uiLoadCreatingWorldPrefabs", false, null), "100"), true);
			yield return null;
		}
		this.SortPrefabs();
		XUiC_ProgressWindow.SetText(ui, Localization.Get("uiLoadCreatingWorld", false, null), true);
		yield return null;
		yield break;
		yield break;
	}

	// Token: 0x06004343 RID: 17219 RVA: 0x001A2E48 File Offset: 0x001A1048
	[PublicizedFrom(EAccessModifier.Private)]
	public void SortPrefabs()
	{
		object obj = this.listsLock;
		lock (obj)
		{
			this.allPrefabsSorted.Clear();
			this.allPrefabsSorted.AddRange(this.allPrefabs);
			this.allPrefabsSorted.Sort((PrefabInstance a, PrefabInstance b) => a.boundingBoxPosition.x.CompareTo(b.boundingBoxPosition.x));
			this.isSortNeeded = false;
		}
	}

	// Token: 0x06004344 RID: 17220 RVA: 0x001A2ED0 File Offset: 0x001A10D0
	public int GetNextId()
	{
		int num = this.id;
		this.id = num + 1;
		return num;
	}

	// Token: 0x06004345 RID: 17221 RVA: 0x001A2EF0 File Offset: 0x001A10F0
	public bool Save(string _path)
	{
		bool flag;
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.CreateXmlDeclaration();
			XmlElement xmlElement = xmlDocument.AddXmlElement("prefabs");
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabs[i];
				if (prefabInstance != null)
				{
					string text = "";
					Vector3i boundingBoxPosition = prefabInstance.boundingBoxPosition;
					if (prefabInstance.prefab != null && prefabInstance.prefab.location.Type != PathAbstractions.EAbstractedLocationType.None)
					{
						text = prefabInstance.prefab.PrefabName;
						boundingBoxPosition.y -= prefabInstance.prefab.yOffset;
					}
					else if (prefabInstance.location.Type != PathAbstractions.EAbstractedLocationType.None)
					{
						text = prefabInstance.location.Name;
					}
					xmlElement.AddXmlElement("decoration").SetAttrib("type", "model").SetAttrib("name", text)
						.SetAttrib("position", boundingBoxPosition.ToStringNoBlanks())
						.SetAttrib("rotation", prefabInstance.rotation.ToString())
						.SetAttrib("y_is_groundlevel", "true");
				}
			}
			xmlDocument.SdSave(_path + "/prefabs.xml");
			flag = true;
		}
		catch (Exception ex)
		{
			Log.Error(ex.ToString());
			Log.Error(ex.StackTrace);
			flag = false;
		}
		return flag;
	}

	// Token: 0x06004346 RID: 17222 RVA: 0x001A3058 File Offset: 0x001A1258
	public void Cleanup()
	{
		this.prefabMeshExisting.Clear();
	}

	// Token: 0x06004347 RID: 17223 RVA: 0x001A3068 File Offset: 0x001A1268
	public void GetAllPrefabs(List<PrefabInstance> _prefabs)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			_prefabs.AddRange(this.allPrefabs);
		}
	}

	// Token: 0x06004348 RID: 17224 RVA: 0x001A30B0 File Offset: 0x001A12B0
	public void GetPOIPrefabs(List<PrefabInstance> _prefabs)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			_prefabs.AddRange(this.poiPrefabs);
		}
	}

	// Token: 0x06004349 RID: 17225 RVA: 0x001A30F8 File Offset: 0x001A12F8
	public void GetWorldPrefabs(List<PrefabInstance> _prefabs)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			_prefabs.AddRange(this.worldPrefabs);
		}
	}

	// Token: 0x0600434A RID: 17226 RVA: 0x001A3140 File Offset: 0x001A1340
	public void AddWorldPrefab(PrefabInstance _pi, bool _isPOI = false)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			this.allPrefabs.Add(_pi);
			this.worldPrefabs.Add(_pi);
			if (_isPOI)
			{
				this.poiPrefabs.Add(_pi);
			}
			this.isSortNeeded = true;
		}
	}

	// Token: 0x0600434B RID: 17227 RVA: 0x001A31A8 File Offset: 0x001A13A8
	public void RemoveWorldPrefab(PrefabInstance _pi)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			if (!this.worldPrefabs.Remove(_pi))
			{
				Log.Warning(string.Format("{0} is not a world prefab", _pi));
			}
			this.allPrefabs.Remove(_pi);
			this.poiPrefabs.Remove(_pi);
			this.allPrefabsSorted.Remove(_pi);
		}
	}

	// Token: 0x0600434C RID: 17228 RVA: 0x001A3228 File Offset: 0x001A1428
	public void AddEventPrefab(PrefabInstance _pi)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			this.allPrefabs.Add(_pi);
			this.isSortNeeded = true;
		}
	}

	// Token: 0x0600434D RID: 17229 RVA: 0x001A3278 File Offset: 0x001A1478
	public void RemoveEventPrefab(PrefabInstance _pi)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			this.allPrefabs.Remove(_pi);
			this.allPrefabsSorted.Remove(_pi);
		}
	}

	// Token: 0x0600434E RID: 17230 RVA: 0x001A32CC File Offset: 0x001A14CC
	public void ClearTraders()
	{
		this.traderAreas.Clear();
	}

	// Token: 0x0600434F RID: 17231 RVA: 0x001A32D9 File Offset: 0x001A14D9
	public void AddTrader(TraderArea _ta)
	{
		this.ProtectSizeXMax = Utils.FastMax(this.ProtectSizeXMax, _ta.ProtectSize.x);
		this.traderAreas.Add(_ta);
		this.traderAreas.Sort(this.traderComparer);
	}

	// Token: 0x06004350 RID: 17232 RVA: 0x001A3314 File Offset: 0x001A1514
	public List<TraderArea> GetTraderAreas()
	{
		return this.traderAreas;
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x001A331C File Offset: 0x001A151C
	public bool IsWithinTraderArea(Vector3i _minPos, Vector3i _maxPos)
	{
		for (int i = 0; i < this.traderAreas.Count; i++)
		{
			if (this.traderAreas[i].Overlaps(_minPos, _maxPos))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x001A3358 File Offset: 0x001A1558
	[PublicizedFrom(EAccessModifier.Private)]
	public int TraderBinarySearch(int x)
	{
		int num = x - this.ProtectSizeXMax;
		int i = 0;
		int num2 = this.traderAreas.Count;
		while (i < num2)
		{
			int num3 = (i + num2) / 2;
			if (this.traderAreas[num3].ProtectPosition.x < num)
			{
				i = num3 + 1;
			}
			else
			{
				num2 = num3;
			}
		}
		return i;
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x001A33AC File Offset: 0x001A15AC
	public TraderArea GetTraderAtPosition(Vector3i _pos, int _padding)
	{
		TraderArea traderArea = null;
		int num = -_padding;
		int i = this.TraderBinarySearch(_pos.x - _padding);
		int count = this.traderAreas.Count;
		while (i < count)
		{
			TraderArea traderArea2 = this.traderAreas[i];
			int num2 = _pos.x - traderArea2.ProtectPosition.x;
			if (num2 < num)
			{
				break;
			}
			if (num2 < traderArea2.ProtectSize.x + _padding)
			{
				int num3 = _pos.z - traderArea2.ProtectPosition.z;
				if (num3 >= num && num3 < traderArea2.ProtectSize.z + _padding)
				{
					traderArea = traderArea2;
					break;
				}
			}
			i++;
		}
		return traderArea;
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x001A3450 File Offset: 0x001A1650
	public void CopyAllPrefabsIntoWorld(World _world, bool _bOverwriteExistingBlocks = false)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				if (this.allPrefabs[i].standaloneBlockSize == 0)
				{
					this.allPrefabs[i].CopyIntoWorld(_world, true, _bOverwriteExistingBlocks, FastTags<TagGroup.Global>.none);
				}
				else
				{
					Log.Warning("Prefab with standaloneBlockSize={0} not supported", new object[] { this.allPrefabs[i].standaloneBlockSize });
				}
			}
		}
	}

	// Token: 0x06004355 RID: 17237 RVA: 0x001A34F8 File Offset: 0x001A16F8
	public void CleanAllPrefabsFromWorld(World _world)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				this.allPrefabs[i].CleanFromWorld(_world, true);
			}
		}
	}

	// Token: 0x06004356 RID: 17238 RVA: 0x001A355C File Offset: 0x001A175C
	public void ClearAllPrefabs()
	{
		object obj = this.listsLock;
		lock (obj)
		{
			foreach (PrefabInstance prefabInstance in this.allPrefabs)
			{
				this.CallPrefabRemovedEvent(prefabInstance);
			}
			this.allPrefabs.Clear();
			this.poiPrefabs.Clear();
			this.worldPrefabs.Clear();
			this.allPrefabsSorted.Clear();
		}
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x001A3604 File Offset: 0x001A1804
	[PublicizedFrom(EAccessModifier.Private)]
	public void CallPrefabRemovedEvent(PrefabInstance _prefabInstance)
	{
		if (this.OnPrefabRemoved != null)
		{
			this.OnPrefabRemoved(_prefabInstance);
		}
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x001A361C File Offset: 0x001A181C
	public void CallPrefabChangedEvent(PrefabInstance _prefabInstance)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			this.isSortNeeded = true;
		}
		if (this.OnPrefabChanged != null)
		{
			this.OnPrefabChanged(_prefabInstance);
		}
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x001A3674 File Offset: 0x001A1874
	public void CreateBoundingBoxes()
	{
		object obj = this.listsLock;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				this.allPrefabs[i].CreateBoundingBox(false);
			}
		}
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x001A36D8 File Offset: 0x001A18D8
	public PrefabInstance GetPrefab(int _id)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				if (this.allPrefabs[i].id == _id)
				{
					return this.allPrefabs[i];
				}
			}
		}
		return null;
	}

	// Token: 0x0600435B RID: 17243 RVA: 0x001A3750 File Offset: 0x001A1950
	public bool IsActivePrefab(int _id)
	{
		PrefabInstance prefab = this.GetPrefab(_id);
		return prefab != null && prefab == this.ActivePrefab;
	}

	// Token: 0x0600435C RID: 17244 RVA: 0x001A3774 File Offset: 0x001A1974
	public PrefabInstance CreateNewPrefabAndActivate(PathAbstractions.AbstractedLocation _location, Vector3i _position, Prefab _bad, bool _bSetActive = true)
	{
		if (_bad == null)
		{
			_bad = new Prefab(new Vector3i(3, 3, 3));
		}
		PrefabInstance prefabInstance = new PrefabInstance(this.GetNextId(), _location, _position, 0, _bad, 0);
		this.AddWorldPrefab(prefabInstance, false);
		prefabInstance.CreateBoundingBox(true);
		if (_bSetActive)
		{
			SelectionBox selectionBox;
			SelectionBoxManager.Instance.CategoryDynamicPrefab.TryGetBox(prefabInstance.name, out selectionBox);
			SelectionBoxManager.Instance.SetActive(selectionBox, true);
		}
		if (this.OnPrefabLoaded != null)
		{
			this.OnPrefabLoaded(prefabInstance);
		}
		return prefabInstance;
	}

	// Token: 0x0600435D RID: 17245 RVA: 0x001A37F0 File Offset: 0x001A19F0
	public PrefabInstance RemoveActivePrefab(World _world)
	{
		if (this.ActivePrefab == null)
		{
			return null;
		}
		PrefabInstance activePrefab = this.ActivePrefab;
		this.RemovePrefabAndSelection(_world, activePrefab, true);
		this.ActivePrefab = null;
		return activePrefab;
	}

	// Token: 0x0600435E RID: 17246 RVA: 0x001A3820 File Offset: 0x001A1A20
	public void RemovePrefabAndSelection(World _world, PrefabInstance _prefab, bool _bCleanFromWorld)
	{
		if (_bCleanFromWorld)
		{
			_prefab.CleanFromWorld(_world, true);
		}
		this.RemoveWorldPrefab(_prefab);
		SelectionBoxManager.Instance.CategoryDynamicPrefab.RemoveBox(_prefab.name);
		foreach (PrefabVolumeListAbs prefabVolumeListAbs in _prefab.prefab.AllVolumeLists)
		{
			prefabVolumeListAbs.RemoveVolumes(_prefab);
		}
		SelectionBoxManager.Instance.CategorySleeperVolume.RemoveBox(_prefab.name);
		this.CallPrefabRemovedEvent(_prefab);
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x001A38BC File Offset: 0x001A1ABC
	public virtual void DecorateChunk(World _world, Chunk _chunk)
	{
		this.DecorateChunk(_world, _chunk, false);
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x001A38C8 File Offset: 0x001A1AC8
	public void DecorateChunk(World _world, Chunk _chunk, bool _bForceOverwriteBlocks = false)
	{
		List<PrefabInstance> value = this.decorateChunkPIs.Value;
		object obj = this.listsLock;
		lock (obj)
		{
			int blockWorldPosX = _chunk.GetBlockWorldPosX(0);
			int blockWorldPosZ = _chunk.GetBlockWorldPosZ(0);
			this.GetPrefabsAtXZ(blockWorldPosX, blockWorldPosX + 15, blockWorldPosZ, blockWorldPosZ + 15, value);
			value.Sort(new Comparison<PrefabInstance>(this.prefabInstanceSizeComparison));
		}
		for (int i = 0; i < value.Count; i++)
		{
			PrefabInstance prefabInstance = value[i];
			if (prefabInstance.Overlaps(_chunk))
			{
				prefabInstance.CopyIntoChunk(_world, _chunk, _bForceOverwriteBlocks, default(FastTags<TagGroup.Global>));
			}
		}
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x001A3984 File Offset: 0x001A1B84
	[PublicizedFrom(EAccessModifier.Private)]
	public int prefabInstanceSizeComparison(PrefabInstance _a, PrefabInstance _b)
	{
		int num = _a.boundingBoxSize.x * _a.boundingBoxSize.z;
		return (_b.boundingBoxSize.x * _b.boundingBoxSize.z).CompareTo(num);
	}

	// Token: 0x06004362 RID: 17250 RVA: 0x001A39CC File Offset: 0x001A1BCC
	public bool IsEntityInPrefab(int _entityId)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				if (this.allPrefabs[i].Contains(_entityId))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004363 RID: 17251 RVA: 0x001A3A38 File Offset: 0x001A1C38
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		if (!_bActivated)
		{
			this.ActivePrefab = null;
			return true;
		}
		PrefabInstance prefabInstance = _box.UserData as PrefabInstance;
		if (prefabInstance != null)
		{
			this.ActivePrefab = prefabInstance;
			this.ActivePrefab.UpdateImposterView();
		}
		else
		{
			Log.Error("Selected prefab SelectionBox has no PrefabInstance assigned");
			StringParsers.SeparatorPositions separatorPositions = StringParsers.GetSeparatorPositions(_box.name, '.', 1, 0, -1);
			int num;
			if (separatorPositions.TotalFound >= 1 && StringParsers.TryParseSInt32(_box.name, out num, separatorPositions.Sep1 + 1, separatorPositions.Sep2 - 1, NumberStyles.Integer))
			{
				this.ActivePrefab = this.GetPrefab(num);
			}
		}
		return true;
	}

	// Token: 0x06004364 RID: 17252 RVA: 0x001A3AC6 File Offset: 0x001A1CC6
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		if (this.ActivePrefab != null)
		{
			this.ActivePrefab.MoveBoundingBox(new Vector3i(_moveVector));
			this.ActivePrefab.UpdateImposterView();
		}
	}

	// Token: 0x06004365 RID: 17253 RVA: 0x001A3AEC File Offset: 0x001A1CEC
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		if (GameManager.Instance.IsEditMode() && !PrefabEditModeManager.Instance.IsActive())
		{
			return;
		}
		if (this.ActivePrefab != null)
		{
			this.ActivePrefab.ResizeBoundingBox(new Vector3i(_dEast + _dWest, _dTop + _dBottom, _dNorth + _dSouth));
			this.ActivePrefab.MoveBoundingBox(new Vector3i(-_dWest, -_dBottom, -_dSouth));
		}
	}

	// Token: 0x06004366 RID: 17254 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x06004367 RID: 17255 RVA: 0x001A3B4F File Offset: 0x001A1D4F
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		PrefabInstance prefabInstance = _box.UserData as PrefabInstance;
		if (prefabInstance != null)
		{
			prefabInstance.DestroyImposterView();
		}
		return false;
	}

	// Token: 0x06004368 RID: 17256 RVA: 0x001A3B68 File Offset: 0x001A1D68
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		if (_criteria == EnumSelectionBoxAvailabilities.CanResize)
		{
			return PrefabEditModeManager.Instance.IsActive();
		}
		return _criteria == EnumSelectionBoxAvailabilities.CanShowProperties;
	}

	// Token: 0x06004369 RID: 17257 RVA: 0x001A3B80 File Offset: 0x001A1D80
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
		XUiC_EditorPanelSelector childByType = _windowManager.playerUI.xui.FindWindowGroupByName(XUiC_EditorPanelSelector.ID).GetChildByType<XUiC_EditorPanelSelector>();
		if (childByType == null)
		{
			return;
		}
		childByType.SetSelected("prefabList");
		_windowManager.SwitchVisible(XUiC_InGameMenuWindow.ID, true);
	}

	// Token: 0x0600436A RID: 17258 RVA: 0x001A3BC3 File Offset: 0x001A1DC3
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
		this.ActivePrefab.RotateAroundY();
		this.ActivePrefab.UpdateImposterView();
	}

	// Token: 0x0600436B RID: 17259 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x0600436C RID: 17260 RVA: 0x001A3BDC File Offset: 0x001A1DDC
	[PublicizedFrom(EAccessModifier.Private)]
	public int PrefabBinarySearch(int x)
	{
		object obj = this.listsLock;
		int num4;
		lock (obj)
		{
			if (this.isSortNeeded)
			{
				this.SortPrefabs();
			}
			int num = x - 200;
			int i = 0;
			int num2 = this.allPrefabsSorted.Count;
			while (i < num2)
			{
				int num3 = (i + num2) / 2;
				if (this.allPrefabsSorted[num3].boundingBoxPosition.x < num)
				{
					i = num3 + 1;
				}
				else
				{
					num2 = num3;
				}
			}
			num4 = i;
		}
		return num4;
	}

	// Token: 0x0600436D RID: 17261 RVA: 0x001A3C74 File Offset: 0x001A1E74
	public PrefabInstance GetPrefabAtPosition(Vector3 _position, FastTags<TagGroup.Poi>? _excludeTags = null, FastTags<TagGroup.Poi>? _requiredTags = null)
	{
		FastTags<TagGroup.Poi> fastTags = _excludeTags.GetValueOrDefault();
		if (_excludeTags == null)
		{
			fastTags = DynamicPrefabDecorator.streetTileTag;
			_excludeTags = new FastTags<TagGroup.Poi>?(fastTags);
		}
		fastTags = _requiredTags.GetValueOrDefault();
		if (_requiredTags == null)
		{
			fastTags = FastTags<TagGroup.Poi>.none;
			_requiredTags = new FastTags<TagGroup.Poi>?(fastTags);
		}
		bool flag = !_excludeTags.Value.IsEmpty;
		bool flag2 = !_requiredTags.Value.IsEmpty;
		object obj = this.listsLock;
		PrefabInstance prefabInstance3;
		lock (obj)
		{
			PrefabInstance prefabInstance = null;
			Vector3i vector3i = Vector3i.Floor(_position);
			int i = this.PrefabBinarySearch(vector3i.x);
			int count = this.allPrefabsSorted.Count;
			while (i < count)
			{
				PrefabInstance prefabInstance2 = this.allPrefabsSorted[i];
				int num = vector3i.x - prefabInstance2.boundingBoxPosition.x;
				if (num < 0)
				{
					break;
				}
				if (num < prefabInstance2.boundingBoxSize.x)
				{
					int num2 = vector3i.z - prefabInstance2.boundingBoxPosition.z;
					if (num2 >= 0 && num2 < prefabInstance2.boundingBoxSize.z)
					{
						int num3 = vector3i.y - prefabInstance2.boundingBoxPosition.y;
						if (num3 >= 0 && num3 < prefabInstance2.boundingBoxSize.y && (!flag || !prefabInstance2.prefab.Tags.Test_AnySet(_excludeTags.Value)) && (!flag2 || prefabInstance2.prefab.Tags.Test_AllSet(_requiredTags.Value)))
						{
							prefabInstance = prefabInstance2;
							for (i++; i < count; i++)
							{
								prefabInstance2 = this.allPrefabsSorted[i];
								num = vector3i.x - prefabInstance2.boundingBoxPosition.x;
								if (num < 0)
								{
									break;
								}
								if (num < prefabInstance2.boundingBoxSize.x)
								{
									num2 = vector3i.z - prefabInstance2.boundingBoxPosition.z;
									if (num2 >= 0 && num2 < prefabInstance2.boundingBoxSize.z)
									{
										num3 = vector3i.y - prefabInstance2.boundingBoxPosition.y;
										if (num3 >= 0 && num3 < prefabInstance2.boundingBoxSize.y && (!flag || !prefabInstance2.prefab.Tags.Test_AnySet(_excludeTags.Value)) && (!flag2 || prefabInstance2.prefab.Tags.Test_AllSet(_requiredTags.Value)))
										{
											if (prefabInstance.boundingBoxPosition.x != prefabInstance2.boundingBoxPosition.x || prefabInstance.boundingBoxSize.x >= prefabInstance2.boundingBoxPosition.x)
											{
												prefabInstance = prefabInstance2;
												break;
											}
											break;
										}
									}
								}
							}
							break;
						}
					}
				}
				i++;
			}
			prefabInstance3 = prefabInstance;
		}
		return prefabInstance3;
	}

	// Token: 0x0600436E RID: 17262 RVA: 0x001A3F84 File Offset: 0x001A2184
	public void GetPrefabsAtXZ(int _xMin, int _xMax, int _zMin, int _zMax, List<PrefabInstance> _list)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			_list.Clear();
			int num = this.PrefabBinarySearch(_xMin);
			int count = this.allPrefabsSorted.Count;
			for (int i = num; i < count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabsSorted[i];
				if (prefabInstance.boundingBoxPosition.x > _xMax)
				{
					break;
				}
				if (prefabInstance.boundingBoxPosition.x + prefabInstance.boundingBoxSize.x > _xMin && prefabInstance.boundingBoxPosition.z <= _zMax && prefabInstance.boundingBoxPosition.z + prefabInstance.boundingBoxSize.z > _zMin)
				{
					_list.Add(prefabInstance);
				}
			}
		}
	}

	// Token: 0x0600436F RID: 17263 RVA: 0x001A4054 File Offset: 0x001A2254
	public bool HasPrefabsAtXZ(int _xMin, int _xMax, int _zMin, int _zMax)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			for (int i = this.PrefabBinarySearch(_xMin); i < this.allPrefabsSorted.Count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabsSorted[i];
				if (prefabInstance.boundingBoxPosition.x > _xMax)
				{
					break;
				}
				if (prefabInstance.boundingBoxPosition.x + prefabInstance.boundingBoxSize.x > _xMin && prefabInstance.boundingBoxPosition.z <= _zMax && prefabInstance.boundingBoxPosition.z + prefabInstance.boundingBoxSize.z > _zMin)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004370 RID: 17264 RVA: 0x001A4114 File Offset: 0x001A2314
	public virtual void GetPrefabsAround(Vector3 _position, float _nearDistance, float _farDistance, Dictionary<int, PrefabInstance> _prefabsFar, Dictionary<int, PrefabInstance> _prefabsNear)
	{
		object obj = this.listsLock;
		lock (obj)
		{
			Vector2 vector = new Vector2(_position.x, _position.z);
			float num = _farDistance * _farDistance;
			for (int i = this.PrefabBinarySearch((int)(vector.x - _farDistance)); i < this.allPrefabsSorted.Count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabsSorted[i];
				if (_position.x - (float)prefabInstance.boundingBoxPosition.x < -_farDistance)
				{
					break;
				}
				float num2 = _position.x - ((float)prefabInstance.boundingBoxPosition.x + (float)prefabInstance.boundingBoxSize.x * 0.5f);
				float num3 = _position.z - ((float)prefabInstance.boundingBoxPosition.z + (float)prefabInstance.boundingBoxSize.z * 0.5f);
				if (num2 * num2 + num3 * num3 <= num)
				{
					Vector2 vector2;
					vector2.x = (float)prefabInstance.boundingBoxPosition.x;
					vector2.y = (float)prefabInstance.boundingBoxPosition.z;
					Vector2 vector3;
					vector3.x = vector2.x + (float)prefabInstance.boundingBoxSize.x;
					vector3.y = vector2.y;
					Vector2 vector4;
					vector4.x = vector2.x;
					vector4.y = vector2.y + (float)prefabInstance.boundingBoxSize.z;
					Vector2 vector5;
					vector5.x = vector3.x;
					vector5.y = vector4.y;
					if (!DynamicMeshManager.IsOutsideDistantTerrain(vector2.x, vector3.x, vector2.y, vector4.y))
					{
						Vector2 vector6 = vector2 - vector;
						if (Utils.FastMax(Utils.FastAbs(vector6.x), Utils.FastAbs(vector6.y)) < _nearDistance)
						{
							Vector2 vector7 = vector3 - vector;
							if (Utils.FastMax(Utils.FastAbs(vector7.x), Utils.FastAbs(vector7.y)) < _nearDistance)
							{
								Vector2 vector8 = vector4 - vector;
								if (Utils.FastMax(Utils.FastAbs(vector8.x), Utils.FastAbs(vector8.y)) < _nearDistance)
								{
									Vector2 vector9 = vector5 - vector;
									if (Utils.FastMax(Utils.FastAbs(vector9.x), Utils.FastAbs(vector9.y)) < _nearDistance)
									{
										_prefabsNear.Add(prefabInstance.id, prefabInstance);
										goto IL_02D7;
									}
								}
							}
						}
						string text = ((prefabInstance.prefab.distantPOIOverride == null) ? prefabInstance.prefab.PrefabName : prefabInstance.prefab.distantPOIOverride);
						bool flag2;
						if (!this.prefabMeshExisting.TryGetValue(text, out flag2))
						{
							flag2 = PathAbstractions.PrefabImpostersSearchPaths.GetLocation(text, null, null).Type != PathAbstractions.EAbstractedLocationType.None;
							this.prefabMeshExisting[text] = flag2;
						}
						if (flag2)
						{
							_prefabsFar.Add(prefabInstance.id, prefabInstance);
						}
					}
				}
				IL_02D7:;
			}
		}
	}

	// Token: 0x06004371 RID: 17265 RVA: 0x001A4438 File Offset: 0x001A2638
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool ValidPrefabForQuest(EntityTrader trader, PrefabInstance prefab, FastTags<TagGroup.Global> questTag, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1, BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome, string biomeFilter = "")
	{
		if (!prefab.prefab.SleeperVolumeList.AnyUsedEntry || !prefab.prefab.GetQuestTag(questTag))
		{
			return false;
		}
		Vector2 vector = new Vector2((float)prefab.boundingBoxPosition.x, (float)prefab.boundingBoxPosition.z);
		if (usedPOILocations != null && usedPOILocations.Contains(vector))
		{
			return false;
		}
		ulong num;
		if (QuestEventManager.Current.CheckForPOILockouts(entityIDforQuests, vector, out num) != QuestEventManager.POILockoutReasonTypes.None)
		{
			return false;
		}
		new Vector2((float)prefab.boundingBoxPosition.x + (float)prefab.boundingBoxSize.x / 2f, (float)prefab.boundingBoxPosition.z + (float)prefab.boundingBoxSize.z / 2f);
		if (biomeFilterType != BiomeFilterTypes.AnyBiome)
		{
			string[] array = null;
			BiomeDefinition biomeAt = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int)vector.x, (int)vector.y);
			if (biomeFilterType == BiomeFilterTypes.OnlyBiome)
			{
				if (biomeAt.m_sBiomeName != biomeFilter)
				{
					return false;
				}
			}
			else if (biomeFilterType == BiomeFilterTypes.ExcludeBiome)
			{
				if (array == null)
				{
					array = biomeFilter.Split(',', StringSplitOptions.None);
				}
				bool flag = false;
				for (int i = 0; i < array.Length; i++)
				{
					if (biomeAt.m_sBiomeName == array[i])
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					return false;
				}
			}
			else if (biomeFilterType == BiomeFilterTypes.SameBiome && trader != null)
			{
				BiomeDefinition biomeAt2 = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int)trader.position.x, (int)trader.position.z);
				if (biomeAt != biomeAt2)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06004372 RID: 17266 RVA: 0x001A45CC File Offset: 0x001A27CC
	public static PrefabInstance GetRandomPOINearTrader(EntityTrader trader, FastTags<TagGroup.Global> questTag, byte difficulty, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1, BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome, string biomeFilter = "")
	{
		QuestEventManager questEventManager = QuestEventManager.Current;
		GameRandom gameRandom = GameManager.Instance.World.GetGameRandom();
		int num = trader.PreferredDistanceIndex;
		for (int i = 0; i < 3; i++)
		{
			num %= 3;
			List<PrefabInstance> prefabsForTrader = questEventManager.GetPrefabsForTrader(trader.traderArea, (int)difficulty, num, gameRandom);
			if (prefabsForTrader != null)
			{
				for (int j = 0; j < prefabsForTrader.Count; j++)
				{
					PrefabInstance prefabInstance = prefabsForTrader[j];
					if (DynamicPrefabDecorator.ValidPrefabForQuest(trader, prefabInstance, questTag, usedPOILocations, entityIDforQuests, biomeFilterType, biomeFilter))
					{
						return prefabInstance;
					}
				}
			}
			num++;
		}
		return null;
	}

	// Token: 0x06004373 RID: 17267 RVA: 0x001A4658 File Offset: 0x001A2858
	public static PrefabInstance GetRandomPOINearWorldPos(Vector2 worldPos, int minSearchDistance, int maxSearchDistance, FastTags<TagGroup.Global> questTag, byte difficulty, List<Vector2> usedPOILocations = null, int entityIDforQuests = -1, BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome, string biomeFilter = "")
	{
		List<PrefabInstance> prefabsByDifficultyTier = QuestEventManager.Current.GetPrefabsByDifficultyTier((int)difficulty);
		if (prefabsByDifficultyTier == null)
		{
			return null;
		}
		string[] array = null;
		BiomeDefinition biomeAt = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int)worldPos.x, (int)worldPos.y);
		World world = GameManager.Instance.World;
		for (int i = 0; i < 50; i++)
		{
			int num = world.GetGameRandom().RandomRange(prefabsByDifficultyTier.Count);
			PrefabInstance prefabInstance = prefabsByDifficultyTier[num];
			if (prefabInstance.prefab.SleeperVolumeList.AnyUsedEntry && prefabInstance.prefab.GetQuestTag(questTag) && prefabInstance.prefab.DifficultyTier == difficulty)
			{
				Vector2 vector = new Vector2((float)prefabInstance.boundingBoxPosition.x, (float)prefabInstance.boundingBoxPosition.z);
				ulong num2;
				if ((usedPOILocations == null || !usedPOILocations.Contains(vector)) && QuestEventManager.Current.CheckForPOILockouts(entityIDforQuests, vector, out num2) == QuestEventManager.POILockoutReasonTypes.None)
				{
					Vector2 vector2 = new Vector2((float)prefabInstance.boundingBoxPosition.x + (float)prefabInstance.boundingBoxSize.x / 2f, (float)prefabInstance.boundingBoxPosition.z + (float)prefabInstance.boundingBoxSize.z / 2f);
					if (biomeFilterType != BiomeFilterTypes.AnyBiome)
					{
						BiomeDefinition biomeAt2 = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider().GetBiomeAt((int)vector.x, (int)vector.y);
						if (biomeFilterType == BiomeFilterTypes.OnlyBiome)
						{
							if (biomeAt2.m_sBiomeName != biomeFilter)
							{
								goto IL_01FC;
							}
						}
						else if (biomeFilterType == BiomeFilterTypes.ExcludeBiome)
						{
							if (array == null)
							{
								array = biomeFilter.Split(',', StringSplitOptions.None);
							}
							bool flag = false;
							for (int j = 0; j < array.Length; j++)
							{
								if (biomeAt2.m_sBiomeName == array[j])
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								goto IL_01FC;
							}
						}
						else if (biomeFilterType == BiomeFilterTypes.SameBiome && biomeAt2 != biomeAt)
						{
							goto IL_01FC;
						}
					}
					float sqrMagnitude = (worldPos - vector2).sqrMagnitude;
					if (sqrMagnitude < (float)maxSearchDistance && sqrMagnitude > (float)minSearchDistance)
					{
						return prefabInstance;
					}
				}
			}
			IL_01FC:;
		}
		return null;
	}

	// Token: 0x06004374 RID: 17268 RVA: 0x001A4874 File Offset: 0x001A2A74
	public virtual PrefabInstance GetClosestPOIToWorldPos(FastTags<TagGroup.Global> questTag, Vector2 worldPos, List<Vector2> excludeList = null, int maxSearchDistanceSquared = -1, bool ignoreCurrentPOI = false, BiomeFilterTypes biomeFilterType = BiomeFilterTypes.SameBiome, string biomeFilter = "", string questKey = "")
	{
		object obj = this.listsLock;
		PrefabInstance prefabInstance3;
		lock (obj)
		{
			List<Tuple<PrefabInstance, Vector2>> list = new List<Tuple<PrefabInstance, Vector2>>();
			string[] array = null;
			Vector3 vector = new Vector3(worldPos.x, 0f, worldPos.y);
			IBiomeProvider biomeProvider = GameManager.Instance.World.ChunkCache.ChunkProvider.GetBiomeProvider();
			BiomeDefinition biomeDefinition = ((biomeProvider != null) ? biomeProvider.GetBiomeAt((int)worldPos.x, (int)worldPos.y) : null);
			for (int i = 0; i < this.poiPrefabs.Count; i++)
			{
				PrefabInstance prefabInstance = this.poiPrefabs[i];
				if (!prefabInstance.prefab.PrefabName.Contains("rwg_tile") && (prefabInstance.prefab.GetQuestTag(questTag) || questTag.IsEmpty))
				{
					if (ignoreCurrentPOI)
					{
						vector.y = (float)prefabInstance.boundingBoxPosition.y;
						if (prefabInstance.Overlaps(vector, 0f))
						{
							goto IL_01FC;
						}
					}
					Vector2 vector2 = new Vector2((float)prefabInstance.boundingBoxPosition.x + (float)prefabInstance.boundingBoxSize.x / 2f, (float)prefabInstance.boundingBoxPosition.z + (float)prefabInstance.boundingBoxSize.z / 2f);
					if (excludeList == null || !excludeList.Contains(new Vector2((float)prefabInstance.boundingBoxPosition.x, (float)prefabInstance.boundingBoxPosition.z)))
					{
						if (biomeFilterType != BiomeFilterTypes.AnyBiome)
						{
							BiomeDefinition biomeDefinition2 = ((biomeProvider != null) ? biomeProvider.GetBiomeAt((int)vector2.x, (int)vector2.y) : null);
							if (biomeFilterType == BiomeFilterTypes.OnlyBiome)
							{
								if (biomeDefinition2.m_sBiomeName != biomeFilter)
								{
									goto IL_01FC;
								}
							}
							else if (biomeFilterType == BiomeFilterTypes.ExcludeBiome)
							{
								if (array == null)
								{
									array = biomeFilter.Split(',', StringSplitOptions.None);
								}
								bool flag2 = false;
								for (int j = 0; j < array.Length; j++)
								{
									if (biomeDefinition2.m_sBiomeName == array[j])
									{
										flag2 = true;
										break;
									}
								}
								if (flag2)
								{
									goto IL_01FC;
								}
							}
							else if (biomeFilterType == BiomeFilterTypes.SameBiome && biomeDefinition2 != biomeDefinition)
							{
								goto IL_01FC;
							}
						}
						list.Add(new Tuple<PrefabInstance, Vector2>(prefabInstance, vector2));
					}
				}
				IL_01FC:;
			}
			float num = ((maxSearchDistanceSquared < 0) ? float.MaxValue : ((float)maxSearchDistanceSquared));
			PrefabInstance prefabInstance2;
			if (string.Compare(questKey, "traderquest", true) == 0)
			{
				prefabInstance2 = this.chooseBestTrader(list, worldPos, num);
			}
			else
			{
				prefabInstance2 = this.chooseClosestPrefab(list, worldPos, num);
			}
			prefabInstance3 = prefabInstance2;
		}
		return prefabInstance3;
	}

	// Token: 0x06004375 RID: 17269 RVA: 0x001A4AFC File Offset: 0x001A2CFC
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance chooseClosestPrefab(List<Tuple<PrefabInstance, Vector2>> prefabCandidates, Vector2 worldPos, float maxSearchDistanceSquared)
	{
		PrefabInstance prefabInstance = null;
		foreach (Tuple<PrefabInstance, Vector2> tuple in prefabCandidates)
		{
			float sqrMagnitude = (worldPos - tuple.Item2).sqrMagnitude;
			if (sqrMagnitude < maxSearchDistanceSquared)
			{
				maxSearchDistanceSquared = sqrMagnitude;
				prefabInstance = tuple.Item1;
			}
		}
		return prefabInstance;
	}

	// Token: 0x06004376 RID: 17270 RVA: 0x001A4B6C File Offset: 0x001A2D6C
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance chooseBestTrader(List<Tuple<PrefabInstance, Vector2>> prefabCandidates, Vector2 worldPos, float maxSearchDistanceSquared)
	{
		PrefabInstance prefabInstance = null;
		int num = 0;
		foreach (Tuple<PrefabInstance, Vector2> tuple in prefabCandidates)
		{
			if ((worldPos - tuple.Item2).sqrMagnitude <= maxSearchDistanceSquared)
			{
				TraderArea traderAtPosition = this.GetTraderAtPosition(new Vector3i(tuple.Item2.x, 0f, tuple.Item2.y), 0);
				if (traderAtPosition != null)
				{
					int traderPoiCount = QuestEventManager.Current.GetTraderPoiCount(traderAtPosition, 1, 0);
					if (num < traderPoiCount)
					{
						num = traderPoiCount;
						prefabInstance = tuple.Item1;
					}
				}
			}
		}
		return prefabInstance;
	}

	// Token: 0x06004377 RID: 17271 RVA: 0x001A4C1C File Offset: 0x001A2E1C
	public virtual PrefabInstance GetPrefabFromWorldPos(int x, int z)
	{
		object obj = this.listsLock;
		PrefabInstance prefabInstance;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				if (this.allPrefabs[i].boundingBoxPosition.x == x && this.allPrefabs[i].boundingBoxPosition.z == z && !this.allPrefabs[i].prefab.PrefabName.Contains("rwg_tile") && !this.allPrefabs[i].prefab.PrefabName.Contains("part_"))
				{
					return this.allPrefabs[i];
				}
			}
			prefabInstance = null;
		}
		return prefabInstance;
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x001A4CFC File Offset: 0x001A2EFC
	public virtual PrefabInstance GetPrefabFromWorldPosInside(int _x, int _z)
	{
		object obj = this.listsLock;
		PrefabInstance prefabInstance2;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabs[i];
				int x = prefabInstance.boundingBoxPosition.x;
				int z = prefabInstance.boundingBoxPosition.z;
				if (x <= _x && z <= _z && x + prefabInstance.boundingBoxSize.x >= _x && z + prefabInstance.boundingBoxSize.z >= _z)
				{
					return this.allPrefabs[i];
				}
			}
			prefabInstance2 = null;
		}
		return prefabInstance2;
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x001A4DB4 File Offset: 0x001A2FB4
	public virtual PrefabInstance GetPrefabFromWorldPosInsideWithOffset(int _x, int _z, int _offset)
	{
		object obj = this.listsLock;
		PrefabInstance prefabInstance2;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabs[i];
				int num = prefabInstance.boundingBoxPosition.x - _offset;
				int num2 = prefabInstance.boundingBoxPosition.z - _offset;
				int num3 = prefabInstance.boundingBoxPosition.x + prefabInstance.boundingBoxSize.x + _offset;
				int num4 = prefabInstance.boundingBoxPosition.z + prefabInstance.boundingBoxSize.z + _offset;
				if (num <= _x && num2 <= _z && num3 >= _x && num4 >= _z)
				{
					return this.allPrefabs[i];
				}
			}
			prefabInstance2 = null;
		}
		return prefabInstance2;
	}

	// Token: 0x0600437A RID: 17274 RVA: 0x001A4E90 File Offset: 0x001A3090
	public virtual PrefabInstance GetPrefabFromWorldPosInside(int _x, int _y, int _z)
	{
		object obj = this.listsLock;
		PrefabInstance prefabInstance2;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabs[i];
				int x = prefabInstance.boundingBoxPosition.x;
				int y = prefabInstance.boundingBoxPosition.y;
				int z = prefabInstance.boundingBoxPosition.z;
				if (x <= _x && y <= _y && z <= _z && x + prefabInstance.boundingBoxSize.x >= _x && y + prefabInstance.boundingBoxSize.y >= _y && z + prefabInstance.boundingBoxSize.z >= _z)
				{
					return this.allPrefabs[i];
				}
			}
			prefabInstance2 = null;
		}
		return prefabInstance2;
	}

	// Token: 0x0600437B RID: 17275 RVA: 0x001A4F74 File Offset: 0x001A3174
	public virtual List<PrefabInstance> GetPrefabsFromWorldPosInside(Vector3 _pos, FastTags<TagGroup.Global> _questTags)
	{
		_pos += this.boundsPad;
		List<PrefabInstance> list = new List<PrefabInstance>();
		Bounds bounds = default(Bounds);
		object obj = this.listsLock;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabs[i];
				if (prefabInstance.prefab.GetQuestTag(_questTags))
				{
					bounds.SetMinMax(prefabInstance.boundingBoxPosition, prefabInstance.boundingBoxPosition + prefabInstance.boundingBoxSize - this.boundsPad);
					if (bounds.Contains(_pos))
					{
						list.AddRange(this.GetPrefabsIntersecting(prefabInstance));
					}
				}
			}
		}
		list = list.OrderByDescending<PrefabInstance, int>((PrefabInstance pi) => pi.boundingBoxSize.x * pi.boundingBoxSize.z).ToList<PrefabInstance>();
		return list;
	}

	// Token: 0x0600437C RID: 17276 RVA: 0x001A5080 File Offset: 0x001A3280
	public virtual List<PrefabInstance> GetPrefabsIntersecting(PrefabInstance parentPI)
	{
		List<PrefabInstance> list = new List<PrefabInstance>();
		list.Add(parentPI);
		Bounds bounds = default(Bounds);
		bounds.SetMinMax(parentPI.boundingBoxPosition, parentPI.boundingBoxPosition + parentPI.boundingBoxSize - this.boundsPad);
		float num = bounds.size.x * bounds.size.z;
		Bounds bounds2 = default(Bounds);
		object obj = this.listsLock;
		lock (obj)
		{
			for (int i = 0; i < this.allPrefabs.Count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabs[i];
				if (parentPI != prefabInstance)
				{
					bounds2.SetMinMax(prefabInstance.boundingBoxPosition, prefabInstance.boundingBoxPosition + prefabInstance.boundingBoxSize - this.boundsPad);
					if (bounds.Intersects(bounds2) && bounds2.size.x * bounds2.size.z < num && !list.Contains(prefabInstance))
					{
						list.Add(prefabInstance);
					}
				}
			}
		}
		list = list.OrderByDescending<PrefabInstance, int>((PrefabInstance pi) => pi.boundingBoxSize.x * pi.boundingBoxSize.z).ToList<PrefabInstance>();
		return list;
	}

	// Token: 0x0600437D RID: 17277 RVA: 0x001A51F8 File Offset: 0x001A33F8
	public PrefabInstance FindVolumeOwner(PrefabVolumeAbs.EVolumeType _volumeType, Vector3i _worldMin, Vector3i _worldMax)
	{
		object obj = this.listsLock;
		PrefabInstance prefabInstance2;
		lock (obj)
		{
			for (int i = this.PrefabBinarySearch(_worldMin.x); i < this.allPrefabsSorted.Count; i++)
			{
				PrefabInstance prefabInstance = this.allPrefabsSorted[i];
				if (prefabInstance.boundingBoxPosition.x > _worldMax.x)
				{
					break;
				}
				if (prefabInstance.boundingBoxPosition.x + prefabInstance.boundingBoxSize.x > _worldMin.x && prefabInstance.boundingBoxPosition.z <= _worldMax.z && prefabInstance.boundingBoxPosition.z + prefabInstance.boundingBoxSize.z > _worldMin.z && prefabInstance.boundingBoxPosition.y <= _worldMax.y && prefabInstance.boundingBoxPosition.y + prefabInstance.boundingBoxSize.y > _worldMin.y)
				{
					PrefabVolumeListAbs prefabVolumeListAbs = prefabInstance.prefab.AllVolumeListsByType[_volumeType];
					for (int j = 0; j < prefabVolumeListAbs.Count; j++)
					{
						PrefabVolumeAbs prefabVolumeAbs = prefabVolumeListAbs.Get(j);
						Vector3i vector3i = prefabVolumeAbs.startPos + prefabInstance.boundingBoxPosition;
						Vector3i vector3i2 = vector3i + prefabVolumeAbs.size;
						if (vector3i.Equals(_worldMin) && vector3i2.Equals(_worldMax))
						{
							return prefabInstance;
						}
					}
				}
			}
			prefabInstance2 = null;
		}
		return prefabInstance2;
	}

	// Token: 0x0600437E RID: 17278 RVA: 0x001A5390 File Offset: 0x001A3590
	public IEnumerator CopyWorldPrefabHeightsIntoHeightMap(int _heightMapWidth, int _heightMapHeight, IBackedArray<ushort> _heightData, int _heightMapScale = 1, ushort[] _topTextures = null)
	{
		MicroStopwatch yieldMs = new MicroStopwatch(true);
		if (this.blockValueTerrainFiller.isair)
		{
			this.blockValueTerrainFiller = Block.GetBlockValue(Constants.cTerrainFillerBlockName, false);
			this.blockValueTerrainFiller2 = Block.GetBlockValue(Constants.cTerrainFiller2BlockName, false);
		}
		int num;
		for (int i = 0; i < this.worldPrefabs.Count; i = num + 1)
		{
			PrefabInstance prefabInstance = this.worldPrefabs[i];
			if (prefabInstance.prefab != null)
			{
				this.copyPrefabsIntoHeightMap(prefabInstance, _heightMapWidth, _heightMapHeight, _heightData, _heightMapScale, _topTextures);
				if (yieldMs.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
				{
					yield return null;
					yieldMs.ResetAndRestart();
				}
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x0600437F RID: 17279 RVA: 0x001A53C4 File Offset: 0x001A35C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void copyPrefabsIntoHeightMap(PrefabInstance _pi, int _heightMapWidth, int _heightMapHeight, IBackedArray<ushort> _heightData, int _heightMapScale, ushort[] _topTextures = null)
	{
		using (IBackedArrayView<ushort> backedArrayView = BackedArrays.CreateSingleView<ushort>(_heightData, BackedArrayHandleMode.ReadWrite, 0))
		{
			int rotation = (int)_pi.rotation;
			Prefab prefab = _pi.prefab;
			int yOffset = prefab.yOffset;
			Vector3i size = prefab.size;
			int x = _pi.boundingBoxPosition.x;
			int y = _pi.boundingBoxPosition.y;
			int z = _pi.boundingBoxPosition.z;
			IChunkProvider chunkProvider = GameManager.Instance.World.ChunkCache.ChunkProvider;
			bool flag = chunkProvider != null && chunkProvider.WorldInfo.RandomGeneratedWorld;
			if (_pi.boundingBoxPosition.x < -_heightMapWidth / 2 || _pi.boundingBoxPosition.x + size.x > _heightMapWidth / 2 || _pi.boundingBoxPosition.z < -_heightMapHeight / 2 || _pi.boundingBoxPosition.z + size.z > _heightMapHeight / 2)
			{
				Log.Warning(string.Format("Prefab {0} outside of the world bounds (position {1})", _pi.name, _pi.boundingBoxPosition));
			}
			for (int i = (size.z + _heightMapScale - 1) % _heightMapScale; i < size.z; i += _heightMapScale)
			{
				int num = i + z;
				int num2 = (num / _heightMapScale + _heightMapHeight / 2) * _heightMapWidth;
				int num3 = (num + _heightMapHeight / 2) * _heightMapScale * _heightMapWidth;
				for (int j = (size.x + _heightMapScale - 1) % _heightMapScale; j < size.x; j += _heightMapScale)
				{
					int num4 = j + x;
					int num5 = (num4 / _heightMapScale + _heightMapWidth / 2 + num2) % _heightData.Length;
					int num6 = (num4 + _heightMapWidth / 2) * _heightMapScale + num3;
					for (int k = 0; k < size.y; k++)
					{
						BlockValue blockNoDamage = prefab.GetBlockNoDamage(rotation, j, k, i);
						WaterValue water = prefab.GetWater(j, k, i);
						Block block = blockNoDamage.Block;
						if (blockNoDamage.isair || block == null || !block.shape.IsTerrain() || water.HasMass())
						{
							if (k > -yOffset)
							{
								break;
							}
						}
						else
						{
							sbyte density = prefab.GetDensity(rotation, j, k, i);
							float num7 = (float)(y + k);
							num7 += (float)(-(float)density) / 128f - 1f;
							if (num7 > 0f)
							{
								ushort num8 = (ushort)(num7 / 0.0038910506f);
								if (blockNoDamage.type != this.blockValueTerrainFiller2.type || num8 <= backedArrayView[num5])
								{
									if (num5 >= 0 && num5 < _heightData.Length && (flag || num8 > backedArrayView[num5]))
									{
										backedArrayView[num5] = num8;
									}
									if (block != null && _topTextures != null && !blockNoDamage.isair && blockNoDamage.type != this.blockValueTerrainFiller.type && blockNoDamage.type != this.blockValueTerrainFiller2.type)
									{
										int sideTextureId = block.GetSideTextureId(blockNoDamage, BlockFace.Top, 0);
										if (num6 >= 0 && num6 < _topTextures.Length)
										{
											_topTextures[num6] = (ushort)sideTextureId;
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

	// Token: 0x06004380 RID: 17280 RVA: 0x001A56DC File Offset: 0x001A38DC
	public void CalculateStats(out int basePrefabCount, out int rotatedPrefabsCount, out int activePrefabCount, out int basePrefabBytes, out int rotatedPrefabBytes, out int activePrefabBytes)
	{
		ChunkCluster chunkCache = GameManager.Instance.World.ChunkCache;
		this.prefabCache.CalculateStats(out basePrefabCount, out rotatedPrefabsCount, out basePrefabBytes, out rotatedPrefabBytes);
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			activePrefabCount = -1;
			activePrefabBytes = -1;
			return;
		}
		List<EntityPlayer> list = GameManager.Instance.World.Players.list;
		HashSet<Prefab> hashSet = new HashSet<Prefab>();
		List<PrefabInstance> list2 = new List<PrefabInstance>();
		foreach (EntityPlayer entityPlayer in list)
		{
			foreach (long num in entityPlayer.ChunkObserver.chunksAround.list)
			{
				IChunk chunkSync = chunkCache.GetChunkSync(num);
				if (chunkSync != null)
				{
					Vector3i worldPos = chunkSync.GetWorldPos();
					this.GetPrefabsAtXZ(worldPos.x, worldPos.x + 15, worldPos.z, worldPos.z + 15, list2);
					foreach (PrefabInstance prefabInstance in list2)
					{
						hashSet.Add(prefabInstance.prefab);
					}
				}
			}
		}
		activePrefabCount = hashSet.Count;
		activePrefabBytes = 0;
		foreach (Prefab prefab in hashSet)
		{
			activePrefabBytes += prefab.EstimateOwnedBytes();
		}
	}

	// Token: 0x04003626 RID: 13862
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cPrefabMaxRadius = 200;

	// Token: 0x04003627 RID: 13863
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabCache prefabCache;

	// Token: 0x04003628 RID: 13864
	[PublicizedFrom(EAccessModifier.Private)]
	public object listsLock = new object();

	// Token: 0x04003629 RID: 13865
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance> allPrefabs = new List<PrefabInstance>();

	// Token: 0x0400362A RID: 13866
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance> poiPrefabs = new List<PrefabInstance>();

	// Token: 0x0400362B RID: 13867
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance> worldPrefabs = new List<PrefabInstance>();

	// Token: 0x0400362C RID: 13868
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance> allPrefabsSorted = new List<PrefabInstance>();

	// Token: 0x0400362D RID: 13869
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isSortNeeded = true;

	// Token: 0x0400362E RID: 13870
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TraderArea> traderAreas = new List<TraderArea>();

	// Token: 0x0400362F RID: 13871
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x04003630 RID: 13872
	public PrefabInstance ActivePrefab;

	// Token: 0x04003631 RID: 13873
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, bool> prefabMeshExisting = new Dictionary<string, bool>();

	// Token: 0x04003635 RID: 13877
	public static readonly FastTags<TagGroup.Poi> streetTileTag = FastTags<TagGroup.Poi>.Parse("streettile");

	// Token: 0x04003636 RID: 13878
	public int ProtectSizeXMax;

	// Token: 0x04003637 RID: 13879
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicPrefabDecorator.TraderComparer traderComparer = new DynamicPrefabDecorator.TraderComparer();

	// Token: 0x04003638 RID: 13880
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadLocal<List<PrefabInstance>> decorateChunkPIs = new ThreadLocal<List<PrefabInstance>>(() => new List<PrefabInstance>());

	// Token: 0x04003639 RID: 13881
	public static int PrefabPreviewLimit = 0;

	// Token: 0x0400363A RID: 13882
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Vector3 boundsPad = new Vector3(0.001f, 0.001f, 0.001f);

	// Token: 0x0400363B RID: 13883
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValueTerrainFiller;

	// Token: 0x0400363C RID: 13884
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValueTerrainFiller2;

	// Token: 0x02000913 RID: 2323
	public class TraderComparer : IComparer<TraderArea>
	{
		// Token: 0x06004382 RID: 17282 RVA: 0x001A58B7 File Offset: 0x001A3AB7
		public int Compare(TraderArea _ta1, TraderArea _ta2)
		{
			return _ta1.ProtectPosition.x - _ta2.ProtectPosition.x;
		}
	}
}
