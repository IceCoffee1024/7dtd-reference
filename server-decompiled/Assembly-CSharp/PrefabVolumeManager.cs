using System;
using System.Globalization;
using PrefabVolumes;
using UnityEngine;

// Token: 0x02000955 RID: 2389
public class PrefabVolumeManager : ISelectionBoxCallback
{
	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x0600461F RID: 17951 RVA: 0x001BBF02 File Offset: 0x001BA102
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

	// Token: 0x06004620 RID: 17952 RVA: 0x001BBF18 File Offset: 0x001BA118
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

	// Token: 0x06004621 RID: 17953 RVA: 0x001BC070 File Offset: 0x001BA270
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

	// Token: 0x06004622 RID: 17954 RVA: 0x001BC158 File Offset: 0x001BA358
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

	// Token: 0x06004623 RID: 17955 RVA: 0x001BC1C0 File Offset: 0x001BA3C0
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

	// Token: 0x06004624 RID: 17956 RVA: 0x001BC270 File Offset: 0x001BA470
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

	// Token: 0x06004625 RID: 17957 RVA: 0x001BC2F0 File Offset: 0x001BA4F0
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

	// Token: 0x06004626 RID: 17958 RVA: 0x001BC368 File Offset: 0x001BA568
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

	// Token: 0x06004627 RID: 17959 RVA: 0x001BC3E4 File Offset: 0x001BA5E4
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

	// Token: 0x06004628 RID: 17960 RVA: 0x001BC468 File Offset: 0x001BA668
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

	// Token: 0x06004629 RID: 17961 RVA: 0x001BC4E4 File Offset: 0x001BA6E4
	public void Init()
	{
		SelectionBoxManager.Instance.CategoryTraderTeleport.SetCallback(this);
		SelectionBoxManager.Instance.CategoryInfoVolume.SetCallback(this);
		SelectionBoxManager.Instance.CategoryWallVolume.SetCallback(this);
	}

	// Token: 0x0600462A RID: 17962 RVA: 0x001BC518 File Offset: 0x001BA718
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

	// Token: 0x0600462B RID: 17963 RVA: 0x00020021 File Offset: 0x0001E221
	public bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated)
	{
		return true;
	}

	// Token: 0x0600462C RID: 17964 RVA: 0x001BC56E File Offset: 0x001BA76E
	public void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector)
	{
		this.SelectionBoxMoved(this.getBoxVolumeType(_box), _box, _moveVector);
	}

	// Token: 0x0600462D RID: 17965 RVA: 0x001BC580 File Offset: 0x001BA780
	public void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest)
	{
		this.SelectionBoxSized(this.getBoxVolumeType(_box), _box, _dTop, _dBottom, _dNorth, _dSouth, _dEast, _dWest);
	}

	// Token: 0x0600462E RID: 17966 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxMirrored(Vector3i _axis)
	{
	}

	// Token: 0x0600462F RID: 17967 RVA: 0x001BC5A5 File Offset: 0x001BA7A5
	public bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly)
	{
		return this.SelectionBoxDelete(this.getBoxVolumeType(_box), _box, _checkCanDeleteOnly);
	}

	// Token: 0x06004630 RID: 17968 RVA: 0x001BC5B6 File Offset: 0x001BA7B6
	public bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria)
	{
		return _criteria == EnumSelectionBoxAvailabilities.CanResize;
	}

	// Token: 0x06004631 RID: 17969 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager)
	{
	}

	// Token: 0x06004632 RID: 17970 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxRotated(SelectionBox _box)
	{
	}

	// Token: 0x06004633 RID: 17971 RVA: 0x000027FC File Offset: 0x000009FC
	public void OnSelectionBoxUserDataChanged(SelectionBox _box)
	{
	}

	// Token: 0x0400385D RID: 14429
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefabVolumeManager instance;
}
