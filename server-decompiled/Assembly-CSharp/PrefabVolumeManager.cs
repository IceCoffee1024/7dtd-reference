using System;
using System.Globalization;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000944 RID: 2372
public class PrefabVolumeManager : ISelectionBoxCallback
{
	// Token: 0x17000754 RID: 1876
	// (get) Token: 0x06004599 RID: 17817 RVA: 0x001B970E File Offset: 0x001B790E
	public static PrefabVolumeManager Instance
	{
		get
		{
			PrefabVolumeManager prefabVolumeManager;
			if ((prefabVolumeManager = PrefabVolumeManager.instance) == null)
			{
				prefabVolumeManager = (PrefabVolumeManager.instance = new PrefabVolumeManager());
			}
			return prefabVolumeManager;
		}
	}

	// Token: 0x0600459A RID: 17818 RVA: 0x001B9724 File Offset: 0x001B7924
	public void AddVolumeServer(PrefabVolumeAbs.EVolumeType _volumeType, Vector3i _startPos, Vector3i _size)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEditorAddVolumeFromClient>().Setup(_volumeType, _startPos, _size), false);
			return;
		}
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.World.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator == null)
		{
			return;
		}
		PrefabInstance prefabFromWorldPosInside = dynamicPrefabDecorator.GetPrefabFromWorldPosInside(_startPos.x + _size.x / 2, _startPos.z + _size.z / 2);
		if (prefabFromWorldPosInside == null)
		{
			return;
		}
		PrefabVolumeListAbs prefabVolumeListAbs;
		if (!prefabFromWorldPosInside.prefab.AllVolumeListsByType.TryGetValue(_volumeType, out prefabVolumeListAbs))
		{
			throw new ArgumentException("VolumeList for Type " + _volumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " not found", "_volumeType");
		}
		if (!prefabVolumeListAbs.CanCreateVolume(prefabFromWorldPosInside.name, prefabFromWorldPosInside.boundingBoxPosition, _startPos - prefabFromWorldPosInside.boundingBoxPosition, _size))
		{
			return;
		}
		ValueTuple<int, PrefabVolumeAbs, SelectionBox> valueTuple = prefabVolumeListAbs.AddNewVolume(prefabFromWorldPosInside.name, prefabFromWorldPosInside.boundingBoxPosition, _startPos - prefabFromWorldPosInside.boundingBoxPosition, _size);
		int item = valueTuple.Item1;
		PrefabVolumeAbs item2 = valueTuple.Item2;
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorUpdateVolume>().Setup(NetPackageEditorUpdateVolume.EChangeType.Added, prefabFromWorldPosInside.id, item, item2), false, -1, -1, -1, null, 192, false);
		if (PrefabEditModeManager.Instance.IsActive() && PrefabEditModeManager.Instance.PrefabInstanceId == prefabFromWorldPosInside.id)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x0600459B RID: 17819 RVA: 0x001B987C File Offset: 0x001B7A7C
	public void CloneVolumeServer(PrefabVolumeAbs.EVolumeType _volumeType, int _prefabInstanceId, int _existingIndex, Vector3i _offset)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEditorAddVolumeFromClient>().Setup(_volumeType, _prefabInstanceId, _existingIndex, _offset), false);
			return;
		}
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.World.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator();
		if (dynamicPrefabDecorator == null)
		{
			return;
		}
		PrefabInstance prefab = dynamicPrefabDecorator.GetPrefab(_prefabInstanceId);
		if (prefab == null)
		{
			return;
		}
		PrefabVolumeListAbs prefabVolumeListAbs;
		if (!prefab.prefab.AllVolumeListsByType.TryGetValue(_volumeType, out prefabVolumeListAbs))
		{
			throw new ArgumentException("VolumeList for Type " + _volumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " not found", "_volumeType");
		}
		ValueTuple<int, PrefabVolumeAbs, SelectionBox> valueTuple = prefabVolumeListAbs.CloneVolume(prefab.name, prefab.boundingBoxPosition, _existingIndex, _offset);
		int item = valueTuple.Item1;
		PrefabVolumeAbs item2 = valueTuple.Item2;
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorUpdateVolume>().Setup(NetPackageEditorUpdateVolume.EChangeType.Added, prefab.id, item, item2), false, -1, -1, -1, null, 192, false);
	}

	// Token: 0x0600459C RID: 17820 RVA: 0x001B9964 File Offset: 0x001B7B64
	public void UpdatePropertiesServer(int _prefabInstanceId, int _volumeId, PrefabVolumeAbs _volumeSettings)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.AddUpdatePropertiesClient(_prefabInstanceId, _volumeId, _volumeSettings);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEditorUpdateVolume>().Setup(NetPackageEditorUpdateVolume.EChangeType.Changed, _prefabInstanceId, _volumeId, _volumeSettings), false, -1, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEditorUpdateVolume>().Setup(NetPackageEditorUpdateVolume.EChangeType.Changed, _prefabInstanceId, _volumeId, _volumeSettings), false);
	}

	// Token: 0x0600459D RID: 17821 RVA: 0x001B99CC File Offset: 0x001B7BCC
	public void AddUpdatePropertiesClient(int _prefabInstanceId, int _volumeId, PrefabVolumeAbs _volumeSettings)
	{
		PrefabInstance prefabInstance = PrefabInstanceClientManager.Instance.GetPrefabInstance(_prefabInstanceId);
		if (prefabInstance == null)
		{
			Log.Error("Prefab not found: " + _prefabInstanceId.ToString());
			return;
		}
		PrefabVolumeListAbs prefabVolumeListAbs;
		if (!prefabInstance.prefab.AllVolumeListsByType.TryGetValue(_volumeSettings.VolumeType, out prefabVolumeListAbs))
		{
			throw new ArgumentException("VolumeList for Type " + _volumeSettings.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " not found", "_volumeSettings");
		}
		prefabVolumeListAbs.SetVolume(prefabInstance, _volumeId, _volumeSettings);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && PrefabEditModeManager.Instance.IsActive() && PrefabEditModeManager.Instance.PrefabInstanceId == prefabInstance.id)
		{
			PrefabEditModeManager.Instance.NeedsSaving = true;
		}
	}

	// Token: 0x0600459E RID: 17822 RVA: 0x001B9A7C File Offset: 0x001B7C7C
	public static bool GetPrefabIdAndVolumeId(string _name, out int _volumeId, out PrefabInstance _prefabInstance)
	{
		_volumeId = -1;
		_prefabInstance = null;
		int num = _name.LastIndexOf('.');
		if (num < 0)
		{
			return false;
		}
		int num2 = _name.IndexOf('_', num + 2);
		if (num2 < 0 || num2 >= _name.Length - 1)
		{
			return false;
		}
		int num3;
		if (!StringParsers.TryParseSInt32(_name, out num3, num + 1, num2 - 1, NumberStyles.Integer))
		{
			return false;
		}
		if (!StringParsers.TryParseSInt32(_name, out _volumeId, num2 + 1, -1, NumberStyles.Integer))
		{
			_volumeId = -1;
			return false;
		}
		_prefabInstance = PrefabInstanceClientManager.Instance.GetPrefabInstance(num3);
		if (_prefabInstance == null)
		{
			_volumeId = -1;
		}
		return _prefabInstance != null;
	}

	// Token: 0x0600459F RID: 17823 RVA: 0x001B9AFC File Offset: 0x001B7CFC
	public static bool TryGetSelectedVolume<TVolume>(SelectionCategory _selectionCategory, out SelectionBox _box, out TVolume _volume, out PrefabInstance _prefabInstance, out int _volumeIndex) where TVolume : PrefabVolumeAbs
	{
		_box = null;
		_volume = default(TVolume);
		_prefabInstance = null;
		_volumeIndex = -1;
		SelectionBox selection = SelectionBoxManager.Instance.Selection;
		if (selection == null || selection.Category != _selectionCategory)
		{
			return false;
		}
		TVolume tvolume = selection.UserData as TVolume;
		if (tvolume == null)
		{
			return false;
		}
		if (!PrefabVolumeManager.GetPrefabIdAndVolumeId(selection.name, out _volumeIndex, out _prefabInstance))
		{
			return false;
		}
		_box = selection;
		_volume = tvolume;
		return true;
	}

	// Token: 0x060045A0 RID: 17824 RVA: 0x001B9B74 File Offset: 0x001B7D74
	public void SelectionBoxMoved(PrefabVolumeAbs.EVolumeType _volumeType, SelectionBox _box, Vector3 _moveVector)
	{
		int num;
		PrefabInstance prefabInstance;
		if (!PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance))
		{
			return;
		}
		PrefabVolumeListAbs prefabVolumeListAbs;
		if (!prefabInstance.prefab.AllVolumeListsByType.TryGetValue(_volumeType, out prefabVolumeListAbs))
		{
			throw new ArgumentException("VolumeList for Type " + _volumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " not found", "_volumeType");
		}
		PrefabVolumeAbs prefabVolumeAbs = prefabVolumeListAbs.Get(num).Clone();
		prefabVolumeAbs.Move(new Vector3i(_moveVector));
		this.UpdatePropertiesServer(prefabInstance.id, num, prefabVolumeAbs);
	}

	// Token: 0x060045A1 RID: 17825 RVA: 0x001B9BF0 File Offset: 0x001B7DF0
	public void SelectionBoxSized(PrefabVolumeAbs.EVolumeType _volumeType, SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		int num;
		PrefabInstance prefabInstance;
		if (!PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance))
		{
			return;
		}
		PrefabVolumeListAbs prefabVolumeListAbs;
		if (!prefabInstance.prefab.AllVolumeListsByType.TryGetValue(_volumeType, out prefabVolumeListAbs))
		{
			throw new ArgumentException("VolumeList for Type " + _volumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " not found", "_volumeType");
		}
		PrefabVolumeAbs prefabVolumeAbs = prefabVolumeListAbs.Get(num).Clone();
		prefabVolumeAbs.Resize(_dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest, false, false, false);
		this.UpdatePropertiesServer(prefabInstance.id, num, prefabVolumeAbs);
	}

	// Token: 0x060045A2 RID: 17826 RVA: 0x001B9C74 File Offset: 0x001B7E74
	public bool SelectionBoxDelete(PrefabVolumeAbs.EVolumeType _volumeType, SelectionBox _box, bool _checkCanDeleteOnly)
	{
		int num;
		PrefabInstance prefabInstance;
		if (!PrefabVolumeManager.GetPrefabIdAndVolumeId(_box.name, out num, out prefabInstance))
		{
			return false;
		}
		if (_checkCanDeleteOnly)
		{
			return true;
		}
		PrefabVolumeListAbs prefabVolumeListAbs;
		if (!prefabInstance.prefab.AllVolumeListsByType.TryGetValue(_volumeType, out prefabVolumeListAbs))
		{
			throw new ArgumentException("VolumeList for Type " + _volumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " not found", "_volumeType");
		}
		PrefabVolumeAbs prefabVolumeAbs = prefabVolumeListAbs.Get(num).Clone();
		prefabVolumeAbs.MarkUnused();
		this.UpdatePropertiesServer(prefabInstance.id, num, prefabVolumeAbs);
		return true;
	}

	// Token: 0x060045A3 RID: 17827 RVA: 0x001B9CF0 File Offset: 0x001B7EF0
	public void Init()
	{
		SelectionBoxManager.Instance.CategoryTraderTeleport.SetCallback(this);
		SelectionBoxManager.Instance.CategoryInfoVolume.SetCallback(this);
		SelectionBoxManager.Instance.CategoryWallVolume.SetCallback(this);
	}

	// Token: 0x060045A4 RID: 17828 RVA: 0x001B9D24 File Offset: 0x001B7F24
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabVolumeAbs.EVolumeType getBoxVolumeType(SelectionBox _box)
	{
		SelectionCategory category = _box.Category;
		if (category == SelectionBoxManager.Instance.CategoryTraderTeleport)
		{
			return PrefabVolumeAbs.EVolumeType.Teleport;
		}
		if (category == SelectionBoxManager.Instance.CategoryInfoVolume)
		{
			return PrefabVolumeAbs.EVolumeType.Info;
		}
		if (category == SelectionBoxManager.Instance.CategoryWallVolume)
		{
			return PrefabVolumeAbs.EVolumeType.Wall;
		}
		throw new ArgumentException("Invalid box category " + category.name);
	}

	// Token: 0x060045A5 RID: 17829 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x060045A6 RID: 17830 RVA: 0x001B9D7A File Offset: 0x001B7F7A
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		this.SelectionBoxMoved(this.getBoxVolumeType(_box), _box, _moveVector);
	}

	// Token: 0x060045A7 RID: 17831 RVA: 0x001B9D8C File Offset: 0x001B7F8C
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		this.SelectionBoxSized(this.getBoxVolumeType(_box), _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
	}

	// Token: 0x060045A8 RID: 17832 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x060045A9 RID: 17833 RVA: 0x001B9DB1 File Offset: 0x001B7FB1
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		return this.SelectionBoxDelete(this.getBoxVolumeType(_box), _box, _checkCanDeleteOnly);
	}

	// Token: 0x060045AA RID: 17834 RVA: 0x001B9DC2 File Offset: 0x001B7FC2
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x060045AB RID: 17835 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
	}

	// Token: 0x060045AC RID: 17836 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x060045AD RID: 17837 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x040037CC RID: 14284
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabVolumeManager instance;
}
