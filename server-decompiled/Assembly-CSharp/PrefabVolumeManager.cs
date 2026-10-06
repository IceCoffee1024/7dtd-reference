using System;
using System.Globalization;
using PrefabVolumes;
using UnityEngine;

// Token: 0x0200095B RID: 2395
public class PrefabVolumeManager : ISelectionBoxCallback
{
	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x060045C0 RID: 17856 RVA: 0x001BC40A File Offset: 0x001BA60A
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

	// Token: 0x060045C1 RID: 17857 RVA: 0x001BC420 File Offset: 0x001BA620
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

	// Token: 0x060045C2 RID: 17858 RVA: 0x001BC578 File Offset: 0x001BA778
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

	// Token: 0x060045C3 RID: 17859 RVA: 0x001BC660 File Offset: 0x001BA860
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

	// Token: 0x060045C4 RID: 17860 RVA: 0x001BC6C8 File Offset: 0x001BA8C8
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

	// Token: 0x060045C5 RID: 17861 RVA: 0x001BC778 File Offset: 0x001BA978
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

	// Token: 0x060045C6 RID: 17862 RVA: 0x001BC7F8 File Offset: 0x001BA9F8
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

	// Token: 0x060045C7 RID: 17863 RVA: 0x001BC870 File Offset: 0x001BAA70
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

	// Token: 0x060045C8 RID: 17864 RVA: 0x001BC8EC File Offset: 0x001BAAEC
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

	// Token: 0x060045C9 RID: 17865 RVA: 0x001BC970 File Offset: 0x001BAB70
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

	// Token: 0x060045CA RID: 17866 RVA: 0x001BC9EC File Offset: 0x001BABEC
	public void Init()
	{
		SelectionBoxManager.Instance.CategoryTraderTeleport.SetCallback(this);
		SelectionBoxManager.Instance.CategoryInfoVolume.SetCallback(this);
		SelectionBoxManager.Instance.CategoryWallVolume.SetCallback(this);
	}

	// Token: 0x060045CB RID: 17867 RVA: 0x001BCA20 File Offset: 0x001BAC20
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

	// Token: 0x060045CC RID: 17868 RVA: 0x00020119 File Offset: 0x0001E319
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x060045CD RID: 17869 RVA: 0x001BCA76 File Offset: 0x001BAC76
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		this.SelectionBoxMoved(this.getBoxVolumeType(_box), _box, _moveVector);
	}

	// Token: 0x060045CE RID: 17870 RVA: 0x001BCA88 File Offset: 0x001BAC88
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		this.SelectionBoxSized(this.getBoxVolumeType(_box), _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
	}

	// Token: 0x060045CF RID: 17871 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x060045D0 RID: 17872 RVA: 0x001BCAAD File Offset: 0x001BACAD
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		return this.SelectionBoxDelete(this.getBoxVolumeType(_box), _box, _checkCanDeleteOnly);
	}

	// Token: 0x060045D1 RID: 17873 RVA: 0x001BCABE File Offset: 0x001BACBE
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x060045D2 RID: 17874 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
	}

	// Token: 0x060045D3 RID: 17875 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x060045D4 RID: 17876 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x04003859 RID: 14425
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabVolumeManager instance;
}
