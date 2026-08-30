using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F65 RID: 3941
[Preserve]
public class XUiC_LevelTools3Window : XUiController
{
	// Token: 0x06007C37 RID: 31799 RVA: 0x002EE744 File Offset: 0x002EC944
	public override void Init()
	{
		base.Init();
		XUiC_LevelTools3Window.ID = base.WindowGroup.Id;
		XUiController childById = base.GetChildById("volumeTypeSelector");
		this.volumeTypeSelector = ((childById != null) ? childById.GetChildByType<XUiC_CategoryList>() : null);
		if (this.volumeTypeSelector != null)
		{
			this.volumeTypeSelector.SetCategoryToFirst();
		}
		XUiController childById2 = base.GetChildById("btnVolumesCreate");
		this.btnVolumesCreate = ((childById2 != null) ? childById2.GetChildByType<XUiC_SimpleButton>() : null);
		if (this.btnVolumesCreate != null)
		{
			this.btnVolumesCreate.OnPressed += this.BtnVolumesCreateOnOnPressed;
		}
		XUiController childById3 = base.GetChildById("btnVolumesCreateFromSelection");
		this.btnVolumesCreateFromSelection = ((childById3 != null) ? childById3.GetChildByType<XUiC_SimpleButton>() : null);
		if (this.btnVolumesCreateFromSelection != null)
		{
			this.btnVolumesCreateFromSelection.OnPressed += this.BtnVolumesCreateFromSelectionOnOnPressed;
		}
		XUiController childById4 = base.GetChildById("toggleVolumesShow");
		this.toggleVolumesShow = ((childById4 != null) ? childById4.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleVolumesShow != null)
		{
			this.toggleVolumesShow.OnValueChanged += this.ToggleVolumesShowOnOnValueChanged;
		}
		XUiController childById5 = base.GetChildById("btnVolumesDupe");
		this.btnVolumesDupe = ((childById5 != null) ? childById5.GetChildByType<XUiC_SimpleButton>() : null);
		if (this.btnVolumesDupe != null)
		{
			this.btnVolumesDupe.OnPressed += this.BtnVolumesDupeOnOnPressed;
		}
		XUiController childById6 = base.GetChildById("toggleVolumesConfirmDelete");
		this.toggleVolumesConfirmDelete = ((childById6 != null) ? childById6.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleVolumesConfirmDelete != null)
		{
			this.toggleVolumesConfirmDelete.OnValueChanged += this.ToggleVolumesConfirmDeleteOnValueChanged;
		}
	}

	// Token: 0x06007C38 RID: 31800 RVA: 0x002EE8C4 File Offset: 0x002ECAC4
	public override void Update(float _dt)
	{
		base.Update(_dt);
		XUiC_CategoryEntry currentCategory = this.volumeTypeSelector.CurrentCategory;
		string text = ((currentCategory != null) ? currentCategory.CategoryName : null) ?? "";
		SelectionCategory category = SelectionBoxManager.Instance.GetCategory(text);
		XUiC_SimpleButton xuiC_SimpleButton = this.btnVolumesCreateFromSelection;
		BlockToolSelection instance = BlockToolSelection.Instance;
		xuiC_SimpleButton.Enabled = instance != null && instance.SelectionActive;
		this.toggleVolumesShow.Value = category != null && category.IsVisible();
		PrefabVolumeAbs prefabVolumeAbs;
		int num;
		int num2;
		this.btnVolumesDupe.Enabled = this.cloneableVolumeSelection(out prefabVolumeAbs, out num, out num2);
		this.toggleVolumesConfirmDelete.Value = !GamePrefs.GetBool(EnumGamePrefs.OptionsPoiVolumesSkipDeleteConfirmation);
	}

	// Token: 0x06007C39 RID: 31801 RVA: 0x002EE968 File Offset: 0x002ECB68
	[PublicizedFrom(EAccessModifier.Private)]
	public bool volumeTypeByName(string _name, out XUiC_LevelTools3Window.VolumeTypeDefinition _result)
	{
		_result = this.volumeTypeDefinitions.Find((XUiC_LevelTools3Window.VolumeTypeDefinition _vtd) => _vtd.SelectionCategory.name == _name);
		return _result != null;
	}

	// Token: 0x06007C3A RID: 31802 RVA: 0x002EE9A0 File Offset: 0x002ECBA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnVolumesCreateOnOnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_CategoryEntry currentCategory = this.volumeTypeSelector.CurrentCategory;
		string text = ((currentCategory != null) ? currentCategory.CategoryName : null) ?? "";
		XUiC_LevelTools3Window.VolumeTypeDefinition volumeTypeDefinition;
		if (!this.volumeTypeByName(text, out volumeTypeDefinition))
		{
			return;
		}
		XUiC_LevelTools3Window.addVolume(volumeTypeDefinition.VolumeType);
	}

	// Token: 0x06007C3B RID: 31803 RVA: 0x002EE9E8 File Offset: 0x002ECBE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnVolumesCreateFromSelectionOnOnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_CategoryEntry currentCategory = this.volumeTypeSelector.CurrentCategory;
		string text = ((currentCategory != null) ? currentCategory.CategoryName : null) ?? "";
		XUiC_LevelTools3Window.VolumeTypeDefinition volumeTypeDefinition;
		if (!this.volumeTypeByName(text, out volumeTypeDefinition))
		{
			return;
		}
		XUiC_LevelTools3Window.addVolumeFromSelection(volumeTypeDefinition.VolumeType);
	}

	// Token: 0x06007C3C RID: 31804 RVA: 0x002EEA30 File Offset: 0x002ECC30
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleVolumesShowOnOnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		XUiC_CategoryEntry currentCategory = this.volumeTypeSelector.CurrentCategory;
		string text = ((currentCategory != null) ? currentCategory.CategoryName : null) ?? "";
		XUiC_LevelTools3Window.VolumeTypeDefinition volumeTypeDefinition;
		if (!this.volumeTypeByName(text, out volumeTypeDefinition))
		{
			return;
		}
		SelectionCategory selectionCategory = volumeTypeDefinition.SelectionCategory;
		selectionCategory.SetVisible(!selectionCategory.IsVisible());
	}

	// Token: 0x06007C3D RID: 31805 RVA: 0x002EEA80 File Offset: 0x002ECC80
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cloneableVolumeSelection(out PrefabVolumeAbs _existingVolume, out int _prefabInstanceId, out int _volumeId)
	{
		_existingVolume = null;
		_prefabInstanceId = 0;
		_volumeId = 0;
		SelectionBox selection = SelectionBoxManager.Instance.Selection;
		if (selection == null)
		{
			return false;
		}
		_existingVolume = selection.UserData as PrefabVolumeAbs;
		if (_existingVolume == null)
		{
			return false;
		}
		PrefabInstance prefabInstance;
		if (!PrefabVolumeManager.GetPrefabIdAndVolumeId(selection.name, out _volumeId, out prefabInstance))
		{
			return false;
		}
		_prefabInstanceId = prefabInstance.id;
		return true;
	}

	// Token: 0x06007C3E RID: 31806 RVA: 0x002EEADC File Offset: 0x002ECCDC
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnVolumesDupeOnOnPressed(XUiController _sender, int _mouseButton)
	{
		PrefabVolumeAbs prefabVolumeAbs;
		int num;
		int num2;
		if (!this.cloneableVolumeSelection(out prefabVolumeAbs, out num, out num2))
		{
			return;
		}
		PrefabVolumeManager.Instance.CloneVolumeServer(prefabVolumeAbs.VolumeType, num, num2, new Vector3i(0, prefabVolumeAbs.size.y + 1, 0));
	}

	// Token: 0x06007C3F RID: 31807 RVA: 0x002EEB1E File Offset: 0x002ECD1E
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleVolumesConfirmDeleteOnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		GamePrefs.Set(EnumGamePrefs.OptionsPoiVolumesSkipDeleteConfirmation, !_newValue);
	}

	// Token: 0x06007C40 RID: 31808 RVA: 0x002EEB30 File Offset: 0x002ECD30
	[PublicizedFrom(EAccessModifier.Private)]
	public static void addVolume(PrefabVolumeAbs.EVolumeType _volumeType)
	{
		Vector3 raycastHitPoint = XUiC_LevelTools3Window.getRaycastHitPoint(100f, 0f);
		if (raycastHitPoint.Equals(Vector3.zero))
		{
			return;
		}
		Vector3i vector3i = new Vector3i(5, 4, 5);
		Vector3i vector3i2 = World.worldToBlockPos(raycastHitPoint) - new Vector3i(vector3i.x / 2, 0, vector3i.z / 2);
		PrefabVolumeManager.Instance.AddVolumeServer(_volumeType, vector3i2, vector3i);
	}

	// Token: 0x06007C41 RID: 31809 RVA: 0x002EEB98 File Offset: 0x002ECD98
	[PublicizedFrom(EAccessModifier.Private)]
	public static void addVolumeFromSelection(PrefabVolumeAbs.EVolumeType _volumeType)
	{
		BlockToolSelection instance = BlockToolSelection.Instance;
		if (instance == null || !instance.SelectionActive)
		{
			return;
		}
		PrefabVolumeManager.Instance.AddVolumeServer(_volumeType, instance.SelectionMin, instance.SelectionSize);
	}

	// Token: 0x06007C42 RID: 31810 RVA: 0x002EEBD0 File Offset: 0x002ECDD0
	public static Vector3 getRaycastHitPoint(float _maxDistance = 100f, float _offsetUp = 0f)
	{
		Camera finalCamera = GameManager.Instance.World.GetPrimaryPlayer().finalCamera;
		Ray ray = finalCamera.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
		ray.origin += Origin.position;
		Transform transform = finalCamera.transform;
		ray.origin += transform.forward * 0.1f;
		ray.origin += transform.up * _offsetUp;
		if (Voxel.Raycast(GameManager.Instance.World, ray, _maxDistance, -555266061, 4095, 0f))
		{
			return Voxel.voxelRayHitInfo.hit.pos - ray.direction * 0.05f;
		}
		return Vector3.zero;
	}

	// Token: 0x04005BDD RID: 23517
	public static string ID = "";

	// Token: 0x04005BDE RID: 23518
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_LevelTools3Window.VolumeTypeDefinition> volumeTypeDefinitions = new List<XUiC_LevelTools3Window.VolumeTypeDefinition>
	{
		new XUiC_LevelTools3Window.VolumeTypeDefinition("SleeperVolume", PrefabVolumeAbs.EVolumeType.Sleeper),
		new XUiC_LevelTools3Window.VolumeTypeDefinition("TriggerVolume", PrefabVolumeAbs.EVolumeType.Trigger),
		new XUiC_LevelTools3Window.VolumeTypeDefinition("InfoVolume", PrefabVolumeAbs.EVolumeType.Info),
		new XUiC_LevelTools3Window.VolumeTypeDefinition("TraderTeleport", PrefabVolumeAbs.EVolumeType.Teleport),
		new XUiC_LevelTools3Window.VolumeTypeDefinition("WallVolume", PrefabVolumeAbs.EVolumeType.Wall)
	};

	// Token: 0x04005BDF RID: 23519
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList volumeTypeSelector;

	// Token: 0x04005BE0 RID: 23520
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnVolumesCreate;

	// Token: 0x04005BE1 RID: 23521
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnVolumesCreateFromSelection;

	// Token: 0x04005BE2 RID: 23522
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleVolumesShow;

	// Token: 0x04005BE3 RID: 23523
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnVolumesDupe;

	// Token: 0x04005BE4 RID: 23524
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleVolumesConfirmDelete;

	// Token: 0x02000F66 RID: 3942
	[PublicizedFrom(EAccessModifier.Private)]
	public class VolumeTypeDefinition
	{
		// Token: 0x06007C45 RID: 31813 RVA: 0x002EED43 File Offset: 0x002ECF43
		public VolumeTypeDefinition(string _selectionCategoryName, PrefabVolumeAbs.EVolumeType _volumeType)
		{
			this.SelectionCategory = SelectionBoxManager.Instance.GetCategory(_selectionCategoryName);
			this.VolumeType = _volumeType;
		}

		// Token: 0x04005BE5 RID: 23525
		public readonly SelectionCategory SelectionCategory;

		// Token: 0x04005BE6 RID: 23526
		public readonly PrefabVolumeAbs.EVolumeType VolumeType;
	}
}
