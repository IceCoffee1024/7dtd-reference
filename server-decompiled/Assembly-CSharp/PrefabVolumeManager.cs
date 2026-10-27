using System;
using System.Globalization;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000965 RID: 2405
public class PrefabVolumeManager : ISelectionBoxCallback
{
	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x060045E3 RID: 17891 RVA: 0x001BD48E File Offset: 0x001BB68E
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

	// Token: 0x060045E4 RID: 17892 RVA: 0x001BD4A4 File Offset: 0x001BB6A4
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

	// Token: 0x060045E5 RID: 17893 RVA: 0x001BD5FC File Offset: 0x001BB7FC
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

	// Token: 0x060045E6 RID: 17894 RVA: 0x001BD6E4 File Offset: 0x001BB8E4
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

	// Token: 0x060045E7 RID: 17895 RVA: 0x001BD74C File Offset: 0x001BB94C
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

	// Token: 0x060045E8 RID: 17896 RVA: 0x001BD7FC File Offset: 0x001BB9FC
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

	// Token: 0x060045E9 RID: 17897 RVA: 0x001BD87C File Offset: 0x001BBA7C
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

	// Token: 0x060045EA RID: 17898 RVA: 0x001BD8F4 File Offset: 0x001BBAF4
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

	// Token: 0x060045EB RID: 17899 RVA: 0x001BD970 File Offset: 0x001BBB70
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

	// Token: 0x060045EC RID: 17900 RVA: 0x001BD9F4 File Offset: 0x001BBBF4
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

	// Token: 0x060045ED RID: 17901 RVA: 0x001BDA70 File Offset: 0x001BBC70
	public void Init()
	{
		SelectionBoxManager.Instance.CategoryTraderTeleport.SetCallback(this);
		SelectionBoxManager.Instance.CategoryInfoVolume.SetCallback(this);
		SelectionBoxManager.Instance.CategoryWallVolume.SetCallback(this);
	}

	// Token: 0x060045EE RID: 17902 RVA: 0x001BDAA4 File Offset: 0x001BBCA4
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

	// Token: 0x060045EF RID: 17903 RVA: 0x00020119 File Offset: 0x0001E319
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x060045F0 RID: 17904 RVA: 0x001BDAFA File Offset: 0x001BBCFA
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		this.SelectionBoxMoved(this.getBoxVolumeType(_box), _box, _moveVector);
	}

	// Token: 0x060045F1 RID: 17905 RVA: 0x001BDB0C File Offset: 0x001BBD0C
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		this.SelectionBoxSized(this.getBoxVolumeType(_box), _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
	}

	// Token: 0x060045F2 RID: 17906 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x060045F3 RID: 17907 RVA: 0x001BDB31 File Offset: 0x001BBD31
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		return this.SelectionBoxDelete(this.getBoxVolumeType(_box), _box, _checkCanDeleteOnly);
	}

	// Token: 0x060045F4 RID: 17908 RVA: 0x001BDB42 File Offset: 0x001BBD42
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x060045F5 RID: 17909 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
	}

	// Token: 0x060045F6 RID: 17910 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x060045F7 RID: 17911 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x04003879 RID: 14457
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabVolumeManager instance;
}
