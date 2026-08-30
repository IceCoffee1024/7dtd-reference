using System;
using System.Collections.Generic;
using PrefabVolumes;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F4C RID: 3916
[Preserve]
public class XUiC_LevelTools3Window : XUiController
{
	// Token: 0x06007B45 RID: 31557 RVA: 0x002E9F70 File Offset: 0x002E8170
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

	// Token: 0x06007B46 RID: 31558 RVA: 0x002EA0F0 File Offset: 0x002E82F0
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

	// Token: 0x06007B47 RID: 31559 RVA: 0x002EA194 File Offset: 0x002E8394
	[PublicizedFrom(EAccessModifier.Private)]
	public bool volumeTypeByName(string _name, out XUiC_LevelTools3Window.VolumeTypeDefinition _result)
	{
		_result = this.volumeTypeDefinitions.Find((XUiC_LevelTools3Window.VolumeTypeDefinition _vtd) => _vtd.SelectionCategory.name == _name);
		return _result != null;
	}

	// Token: 0x06007B48 RID: 31560 RVA: 0x002EA1CC File Offset: 0x002E83CC
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

	// Token: 0x06007B49 RID: 31561 RVA: 0x002EA214 File Offset: 0x002E8414
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

	// Token: 0x06007B4A RID: 31562 RVA: 0x002EA25C File Offset: 0x002E845C
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

	// Token: 0x06007B4B RID: 31563 RVA: 0x002EA2AC File Offset: 0x002E84AC
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

	// Token: 0x06007B4C RID: 31564 RVA: 0x002EA308 File Offset: 0x002E8508
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

	// Token: 0x06007B4D RID: 31565 RVA: 0x002EA34A File Offset: 0x002E854A
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleVolumesConfirmDeleteOnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		GamePrefs.Set(EnumGamePrefs.OptionsPoiVolumesSkipDeleteConfirmation, !_newValue);
	}

	// Token: 0x06007B4E RID: 31566 RVA: 0x002EA35C File Offset: 0x002E855C
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

	// Token: 0x06007B4F RID: 31567 RVA: 0x002EA3C4 File Offset: 0x002E85C4
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

	// Token: 0x06007B50 RID: 31568 RVA: 0x002EA3FC File Offset: 0x002E85FC
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

	// Token: 0x04005B16 RID: 23318
	public static string ID = "";

	// Token: 0x04005B17 RID: 23319
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<XUiC_LevelTools3Window.VolumeTypeDefinition> volumeTypeDefinitions = new List<XUiC_LevelTools3Window.VolumeTypeDefinition>
	{
		new XUiC_LevelTools3Window.VolumeTypeDefinition("SleeperVolume", PrefabVolumeAbs.EVolumeType.Sleeper),
		new XUiC_LevelTools3Window.VolumeTypeDefinition("TriggerVolume", PrefabVolumeAbs.EVolumeType.Trigger),
		new XUiC_LevelTools3Window.VolumeTypeDefinition("InfoVolume", PrefabVolumeAbs.EVolumeType.Info),
		new XUiC_LevelTools3Window.VolumeTypeDefinition("TraderTeleport", PrefabVolumeAbs.EVolumeType.Teleport),
		new XUiC_LevelTools3Window.VolumeTypeDefinition("WallVolume", PrefabVolumeAbs.EVolumeType.Wall)
	};

	// Token: 0x04005B18 RID: 23320
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList volumeTypeSelector;

	// Token: 0x04005B19 RID: 23321
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnVolumesCreate;

	// Token: 0x04005B1A RID: 23322
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnVolumesCreateFromSelection;

	// Token: 0x04005B1B RID: 23323
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleVolumesShow;

	// Token: 0x04005B1C RID: 23324
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnVolumesDupe;

	// Token: 0x04005B1D RID: 23325
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleVolumesConfirmDelete;

	// Token: 0x02000F4D RID: 3917
	[PublicizedFrom(EAccessModifier.Private)]
	public class VolumeTypeDefinition
	{
		// Token: 0x06007B53 RID: 31571 RVA: 0x002EA56F File Offset: 0x002E876F
		public VolumeTypeDefinition(string _selectionCategoryName, PrefabVolumeAbs.EVolumeType _volumeType)
		{
			this.SelectionCategory = SelectionBoxManager.Instance.GetCategory(_selectionCategoryName);
			this.VolumeType = _volumeType;
		}

		// Token: 0x04005B1E RID: 23326
		public readonly SelectionCategory SelectionCategory;

		// Token: 0x04005B1F RID: 23327
		public readonly PrefabVolumeAbs.EVolumeType VolumeType;
	}
}
