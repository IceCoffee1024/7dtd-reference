using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FED RID: 4077
[Preserve]
public class XUiC_PrefabList : XUiController
{
	// Token: 0x0600801F RID: 32799 RVA: 0x0030B468 File Offset: 0x00309668
	public override void Init()
	{
		base.Init();
		XUiC_PrefabList.ID = base.WindowGroup.Id;
		this.prefabPreview = (XUiV_Texture)base.GetChildById("prefabPreview").ViewComponent;
		this.noPreviewLabel = (XUiV_Label)base.GetChildById("noPreview").ViewComponent;
		this.noPreviewLabel.IsVisible = false;
		this.groupList = base.GetChildById("groups") as XUiC_PrefabGroupList;
		this.fileList = base.GetChildById("files") as XUiC_PrefabFileList;
		this.btnLoad = base.GetChildById("btnLoad") as XUiC_SimpleButton;
		this.btnProperties = base.GetChildById("btnProperties") as XUiC_SimpleButton;
		this.btnSave = base.GetChildById("btnSave") as XUiC_SimpleButton;
		this.btnLoadIntoPrefab = base.GetChildById("btnLoadIntoPrefab") as XUiC_SimpleButton;
		this.btnApplyLoadedPrefab = base.GetChildById("btnApplyLoadedPrefab") as XUiC_SimpleButton;
		this.groupList.SelectionChanged += this.GroupListSelectionChanged;
		this.fileList.SelectionChanged += this.FileList_SelectionChanged;
		this.fileList.ListEntryDoubleClicked += this.FileList_OnEntryDoubleClicked;
		this.fileList.PageNumberChanged += this.FileListOnPageNumberChanged;
		this.btnLoad.OnPressed += this.BtnLoad_OnPressed;
		this.btnProperties.OnPressed += this.BtnPropertiesOnOnPressed;
		this.btnSave.OnPressed += this.BtnSave_OnPressed;
		this.btnLoadIntoPrefab.OnPressed += this.BtnLoadIntoPrefabOnOnPressed;
		this.btnApplyLoadedPrefab.OnPressed += this.BtnApplyLoadedPrefabOnOnPressed;
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnCleanOtherPrefabs") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += this.BtnCleanOtherPrefabsOnOnPressed;
		}
		XUiC_SimpleButton xuiC_SimpleButton2 = base.GetChildById("btnLoadIntoClipboard") as XUiC_SimpleButton;
		if (xuiC_SimpleButton2 != null)
		{
			xuiC_SimpleButton2.OnPressed += this.BtnLoadIntoClipboardOnOnPressed;
		}
		XUiC_SimpleButton xuiC_SimpleButton3 = base.GetChildById("btnNew") as XUiC_SimpleButton;
		if (xuiC_SimpleButton3 != null)
		{
			xuiC_SimpleButton3.OnPressed += this.BtnNewOnOnPressed;
		}
		this.btnWorldPlacePrefab = base.GetChildById("btnWorldPlacePrefab") as XUiC_SimpleButton;
		if (this.btnWorldPlacePrefab != null)
		{
			this.btnWorldPlacePrefab.OnPressed += this.BtnWorldPlacePrefabOnPressed;
		}
		this.btnWorldReplacePrefab = base.GetChildById("btnWorldReplacePrefab") as XUiC_SimpleButton;
		if (this.btnWorldReplacePrefab != null)
		{
			this.btnWorldReplacePrefab.OnPressed += this.BtnWorldReplacePrefabOnPressed;
		}
		this.btnWorldDeletePrefab = base.GetChildById("btnWorldDeletePrefab") as XUiC_SimpleButton;
		if (this.btnWorldDeletePrefab != null)
		{
			this.btnWorldDeletePrefab.OnPressed += this.BtnWorldDeletePrefabOnPressed;
		}
		this.btnWorldApplyPrefabChanges = base.GetChildById("btnWorldApplyPrefabChanges") as XUiC_SimpleButton;
		if (this.btnWorldApplyPrefabChanges != null)
		{
			this.btnWorldApplyPrefabChanges.OnPressed += this.BtnWorldApplyPrefabChangesOnPressed;
		}
		this.btnWorldRevertPrefabChanges = base.GetChildById("btnWorldRevertPrefabChanges") as XUiC_SimpleButton;
		if (this.btnWorldRevertPrefabChanges != null)
		{
			this.btnWorldRevertPrefabChanges.OnPressed += this.BtnWorldRevertPrefabChangesOnPressed;
		}
		this.btnLoad.Enabled = false;
		this.btnProperties.Enabled = false;
		this.groupList.SelectedEntryIndex = 0;
	}

	// Token: 0x06008020 RID: 32800 RVA: 0x0030B7D8 File Offset: 0x003099D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void GroupListSelectionChanged(XUiC_List<XUiC_PrefabGroupList.PrefabGroupEntry> _list, XUiC_PrefabGroupList.PrefabGroupEntry _previousEntry, XUiC_PrefabGroupList.PrefabGroupEntry _newEntry)
	{
		string text = null;
		if (_newEntry != null)
		{
			text = _newEntry.filterString;
		}
		this.fileList.SetGroupFilter(text);
		if (this.fileList.EntryCount > 0)
		{
			this.fileList.SelectedEntryIndex = 0;
		}
	}

	// Token: 0x06008021 RID: 32801 RVA: 0x0030B817 File Offset: 0x00309A17
	[PublicizedFrom(EAccessModifier.Private)]
	public void FileListOnPageNumberChanged(XUiC_List<XUiC_PrefabFileList.PrefabFileEntry> _list, int _pageNumber)
	{
		this.fileList.SelectedEntryIndex = this.fileList.Page * this.fileList.PageLength;
	}

	// Token: 0x06008022 RID: 32802 RVA: 0x0030B83B File Offset: 0x00309A3B
	[PublicizedFrom(EAccessModifier.Private)]
	public void FileList_SelectionChanged(XUiC_List<XUiC_PrefabFileList.PrefabFileEntry> _list, XUiC_PrefabFileList.PrefabFileEntry _previousEntry, XUiC_PrefabFileList.PrefabFileEntry _newEntry)
	{
		this.btnLoad.Enabled = _newEntry != null;
		this.btnProperties.Enabled = _newEntry != null;
		this.updatePreview(_newEntry);
	}

	// Token: 0x06008023 RID: 32803 RVA: 0x0030B864 File Offset: 0x00309A64
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePreview(XUiC_PrefabFileList.PrefabFileEntry _newEntry)
	{
		if (this.prefabPreview.Texture != null)
		{
			global::UnityEngine.Object @object = (Texture2D)this.prefabPreview.Texture;
			this.prefabPreview.Texture = null;
			global::UnityEngine.Object.Destroy(@object);
		}
		if (_newEntry != null)
		{
			string text = _newEntry.location.FullPathNoExtension + ".jpg";
			if (SdFile.Exists(text))
			{
				Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGB24, false);
				byte[] array = SdFile.ReadAllBytes(text);
				texture2D.LoadImage(array);
				this.prefabPreview.Texture = texture2D;
				this.noPreviewLabel.IsVisible = false;
				this.prefabPreview.IsVisible = true;
				return;
			}
		}
		this.noPreviewLabel.IsVisible = true;
		this.prefabPreview.IsVisible = false;
	}

	// Token: 0x06008024 RID: 32804 RVA: 0x0030B91C File Offset: 0x00309B1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void FileList_OnEntryDoubleClicked(XUiC_List<XUiC_PrefabFileList.PrefabFileEntry> _list, XUiC_PrefabFileList.PrefabFileEntry _entry)
	{
		if (PrefabEditModeManager.Instance.IsActive())
		{
			this.BtnLoad_OnPressed(this, -1);
		}
	}

	// Token: 0x06008025 RID: 32805 RVA: 0x0030B932 File Offset: 0x00309B32
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLoad_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.fileList.SelectedEntryData != null)
		{
			XUiC_SaveDirtyPrefab.Show(this.xui, new Action<XUiC_SaveDirtyPrefab.ESelectedAction>(this.loadPrefab), XUiC_SaveDirtyPrefab.EMode.AskSaveIfDirty);
		}
	}

	// Token: 0x06008026 RID: 32806 RVA: 0x0030B95C File Offset: 0x00309B5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void loadPrefab(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
		if (_action == XUiC_SaveDirtyPrefab.ESelectedAction.Cancel)
		{
			return;
		}
		PathAbstractions.AbstractedLocation location = this.fileList.SelectedEntryData.location;
		bool flag = XUiC_LevelToolsHelpers.IsShowImposter();
		if (flag && PrefabEditModeManager.Instance.HasPrefabImposter(location))
		{
			PrefabEditModeManager.Instance.LoadImposterPrefab(location);
			return;
		}
		if (flag)
		{
			GameManager.ShowTooltip(GameManager.Instance.World.GetLocalPlayers()[0], string.Format(Localization.Get("xuiPrefabsPrefabHasNoImposter", false, null), location.Name), false, false, 0f);
			XUiC_LevelToolsHelpers.SetShowImposter();
		}
		PrefabEditModeManager.Instance.LoadVoxelPrefab(location, false, false);
	}

	// Token: 0x06008027 RID: 32807 RVA: 0x0030BA0C File Offset: 0x00309C0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPropertiesOnOnPressed(XUiController _sender, int _mouseButton)
	{
		PathAbstractions.AbstractedLocation location = this.fileList.SelectedEntryData.location;
		PathAbstractions.AbstractedLocation loadedPrefab = PrefabEditModeManager.Instance.LoadedPrefab;
		if (location == loadedPrefab)
		{
			XUiC_PrefabPropertiesEditor.Show(this.xui, XUiC_PrefabPropertiesEditor.EPropertiesFrom.LoadedPrefab, PathAbstractions.AbstractedLocation.None);
			return;
		}
		XUiC_PrefabPropertiesEditor.Show(this.xui, XUiC_PrefabPropertiesEditor.EPropertiesFrom.FileBrowserSelection, location);
	}

	// Token: 0x06008028 RID: 32808 RVA: 0x0030BA5D File Offset: 0x00309C5D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_SaveDirtyPrefab.Show(this.xui, new Action<XUiC_SaveDirtyPrefab.ESelectedAction>(this.savePrefab), XUiC_SaveDirtyPrefab.EMode.ForceSave);
	}

	// Token: 0x06008029 RID: 32809 RVA: 0x0030BA77 File Offset: 0x00309C77
	[PublicizedFrom(EAccessModifier.Private)]
	public void savePrefab(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
	}

	// Token: 0x0600802A RID: 32810 RVA: 0x0030BA94 File Offset: 0x00309C94
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLoadIntoPrefabOnOnPressed(XUiController _sender, int _mouseButton)
	{
		PathAbstractions.AbstractedLocation location = this.fileList.SelectedEntryData.location;
		BlockToolSelection blockToolSelection = GameManager.Instance.GetActiveBlockTool() as BlockToolSelection;
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		Prefab prefab = new Prefab();
		prefab.Load(location, true, true, true, false);
		dynamicPrefabDecorator.CreateNewPrefabAndActivate(location, blockToolSelection.SelectionStart, prefab, true);
		this.xui.playerUI.windowManager.Close(XUiC_InGameMenuWindow.ID);
	}

	// Token: 0x0600802B RID: 32811 RVA: 0x0030BB08 File Offset: 0x00309D08
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnApplyLoadedPrefabOnOnPressed(XUiController _sender, int _mouseButton)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		PrefabInstance prefabInstance = ((((dynamicPrefabDecorator != null) ? dynamicPrefabDecorator.ActivePrefab : null) != null) ? dynamicPrefabDecorator.ActivePrefab : null);
		prefabInstance.CleanFromWorld(GameManager.Instance.World, true);
		prefabInstance.CopyIntoWorld(GameManager.Instance.World, true, false, FastTags<TagGroup.Global>.none);
		GameManager.Instance.World.m_ChunkManager.RemoveAllChunksOnAllClients();
	}

	// Token: 0x0600802C RID: 32812 RVA: 0x000882A0 File Offset: 0x000864A0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCleanOtherPrefabsOnOnPressed(XUiController _sender, int _mouseButton)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600802D RID: 32813 RVA: 0x0030BB78 File Offset: 0x00309D78
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnLoadIntoClipboardOnOnPressed(XUiController _sender, int _mouseButton)
	{
		PathAbstractions.AbstractedLocation location = this.fileList.SelectedEntryData.location;
		BlockToolSelection blockToolSelection = GameManager.Instance.GetActiveBlockTool() as BlockToolSelection;
		Prefab prefab = new Prefab();
		prefab.Load(location, true, true, true, false);
		blockToolSelection.LoadPrefabIntoClipboard(prefab);
		this.xui.playerUI.windowManager.Close(XUiC_InGameMenuWindow.ID);
	}

	// Token: 0x0600802E RID: 32814 RVA: 0x0030BBD7 File Offset: 0x00309DD7
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnNewOnOnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_SaveDirtyPrefab.Show(this.xui, new Action<XUiC_SaveDirtyPrefab.ESelectedAction>(this.newPrefab), XUiC_SaveDirtyPrefab.EMode.AskSaveIfDirty);
	}

	// Token: 0x0600802F RID: 32815 RVA: 0x0030BBF1 File Offset: 0x00309DF1
	[PublicizedFrom(EAccessModifier.Private)]
	public void newPrefab(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		if (_action == XUiC_SaveDirtyPrefab.ESelectedAction.Cancel)
		{
			this.xui.playerUI.windowManager.Open(XUiC_InGameMenuWindow.ID, true);
			return;
		}
		this.xui.playerUI.windowManager.Open(XUiC_CreatePoi.ID, true);
	}

	// Token: 0x06008030 RID: 32816 RVA: 0x0030BC30 File Offset: 0x00309E30
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnWorldPlacePrefabOnPressed(XUiController _sender, int _mouseButton)
	{
		GameUtils.DirEightWay closestDirection = GameUtils.GetClosestDirection(this.xui.playerUI.entityPlayer.rotation.y, true);
		BlockToolSelection blockToolSelection = GameManager.Instance.GetActiveBlockTool() as BlockToolSelection;
		if (blockToolSelection == null)
		{
			return;
		}
		if (GameManager.Instance.GetDynamicPrefabDecorator() == null)
		{
			return;
		}
		PathAbstractions.AbstractedLocation location = this.fileList.SelectedEntryData.location;
		Prefab prefab = new Prefab();
		prefab.Load(location, true, true, false, false);
		int num;
		switch (closestDirection)
		{
		case GameUtils.DirEightWay.N:
			num = 2;
			goto IL_00A3;
		case GameUtils.DirEightWay.E:
			num = 3;
			goto IL_00A3;
		case GameUtils.DirEightWay.S:
			num = 0;
			goto IL_00A3;
		case GameUtils.DirEightWay.W:
			num = 1;
			goto IL_00A3;
		}
		throw new ArgumentOutOfRangeException();
		IL_00A3:
		int num2 = MathUtils.Mod(num - prefab.rotationToFaceNorth, 4);
		Vector3i vector3i;
		switch (closestDirection)
		{
		case GameUtils.DirEightWay.N:
			vector3i = new Vector3i(-prefab.size.x / 2, 0, 0);
			goto IL_015A;
		case GameUtils.DirEightWay.E:
			vector3i = new Vector3i(0, 0, -prefab.size.z / 2);
			goto IL_015A;
		case GameUtils.DirEightWay.S:
			vector3i = new Vector3i(-prefab.size.x / 2, 0, 1 - prefab.size.z);
			goto IL_015A;
		case GameUtils.DirEightWay.W:
			vector3i = new Vector3i(1 - prefab.size.x, 0, -prefab.size.z / 2);
			goto IL_015A;
		}
		throw new ArgumentOutOfRangeException();
		IL_015A:
		Vector3i vector3i2 = vector3i;
		vector3i2.y = prefab.yOffset;
		PrefabInstance prefabInstance = GameManager.Instance.GetDynamicPrefabDecorator().CreateNewPrefabAndActivate(prefab.location, blockToolSelection.SelectionStart + vector3i2, prefab, true);
		while (num2-- > 0)
		{
			prefabInstance.RotateAroundY();
		}
		prefabInstance.UpdateImposterView();
		this.xui.playerUI.windowManager.Close(XUiC_InGameMenuWindow.ID);
	}

	// Token: 0x06008031 RID: 32817 RVA: 0x0030BE04 File Offset: 0x0030A004
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnWorldReplacePrefabOnPressed(XUiController _sender, int _mouseButton)
	{
		if (GameManager.Instance.GetDynamicPrefabDecorator() != null)
		{
			PrefabInstance prefabInstance = GameManager.Instance.GetDynamicPrefabDecorator().RemoveActivePrefab(GameManager.Instance.World);
			PathAbstractions.AbstractedLocation location = this.fileList.SelectedEntryData.location;
			Prefab prefab = new Prefab();
			prefab.Load(location, true, true, false, false);
			Vector3i vector3i = prefabInstance.boundingBoxPosition + new Vector3i(0f, (float)prefab.yOffset - prefabInstance.yOffsetOfPrefab, 0f);
			PrefabInstance prefabInstance2 = GameManager.Instance.GetDynamicPrefabDecorator().CreateNewPrefabAndActivate(prefab.location, vector3i, prefab, true);
			for (;;)
			{
				PrefabInstance prefabInstance3 = prefabInstance;
				byte rotation = prefabInstance3.rotation;
				prefabInstance3.rotation = rotation - 1;
				if (rotation <= 0)
				{
					break;
				}
				prefabInstance2.RotateAroundY();
			}
			prefabInstance2.UpdateImposterView();
		}
	}

	// Token: 0x06008032 RID: 32818 RVA: 0x0030BEC8 File Offset: 0x0030A0C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnWorldDeletePrefabOnPressed(XUiController _sender, int _mouseButton)
	{
		GameManager.Instance.GetDynamicPrefabDecorator().RemoveActivePrefab(GameManager.Instance.World);
	}

	// Token: 0x06008033 RID: 32819 RVA: 0x0030BEE4 File Offset: 0x0030A0E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnWorldApplyPrefabChangesOnPressed(XUiController _sender, int _mouseButton)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		PrefabInstance prefabInstance = ((dynamicPrefabDecorator != null) ? dynamicPrefabDecorator.ActivePrefab : null);
		if (prefabInstance == null)
		{
			return;
		}
		prefabInstance.CleanFromWorld(GameManager.Instance.World, true);
		prefabInstance.CopyIntoWorld(GameManager.Instance.World, true, false, FastTags<TagGroup.Global>.none);
		prefabInstance.UpdateImposterView();
	}

	// Token: 0x06008034 RID: 32820 RVA: 0x0030BF3C File Offset: 0x0030A13C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnWorldRevertPrefabChangesOnPressed(XUiController _sender, int _mouseButton)
	{
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		PrefabInstance prefabInstance = ((dynamicPrefabDecorator != null) ? dynamicPrefabDecorator.ActivePrefab : null);
		if (prefabInstance == null)
		{
			return;
		}
		prefabInstance.UpdateBoundingBoxPosAndScale(GameManager.Instance.GetDynamicPrefabDecorator().ActivePrefab.lastCopiedPrefabPosition, prefabInstance.prefab.size, true);
		prefabInstance.rotation = prefabInstance.lastCopiedRotation;
		prefabInstance.UpdateImposterView();
	}

	// Token: 0x06008035 RID: 32821 RVA: 0x0030BF9C File Offset: 0x0030A19C
	public override void OnOpen()
	{
		base.OnOpen();
		PathAbstractions.AbstractedLocation abstractedLocation = PathAbstractions.AbstractedLocation.None;
		if (this.fileList.SelectedEntryData != null)
		{
			abstractedLocation = this.fileList.SelectedEntryData.location;
		}
		if (this.groupList.SelectedEntryData != null)
		{
			string name = this.groupList.SelectedEntryData.name;
			this.groupList.RebuildList(false);
			if (!this.groupList.SelectByName(name))
			{
				this.groupList.SelectedEntryIndex = 0;
			}
		}
		this.fileList.RebuildList(false);
		if (abstractedLocation.Type != PathAbstractions.EAbstractedLocationType.None)
		{
			this.fileList.SelectByLocation(abstractedLocation);
		}
		base.RefreshBindings();
	}

	// Token: 0x06008036 RID: 32822 RVA: 0x0030C040 File Offset: 0x0030A240
	public override void Update(float _dt)
	{
		base.Update(_dt);
		bool flag = PrefabEditModeManager.Instance.VoxelPrefab != null;
		this.btnSave.Enabled = flag;
		BlockToolSelection blockToolSelection = GameManager.Instance.GetActiveBlockTool() as BlockToolSelection;
		DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
		PrefabInstance prefabInstance = ((dynamicPrefabDecorator != null) ? dynamicPrefabDecorator.ActivePrefab : null);
		this.btnLoadIntoPrefab.Enabled = this.fileList.SelectedEntryData != null && blockToolSelection != null && blockToolSelection.SelectionActive && dynamicPrefabDecorator != null;
		this.btnApplyLoadedPrefab.Enabled = prefabInstance != null && !prefabInstance.IsBBInSyncWithPrefab();
		this.btnWorldPlacePrefab.Enabled = blockToolSelection != null && blockToolSelection.SelectionActive && this.fileList.SelectedEntryData != null;
		this.btnWorldReplacePrefab.Enabled = prefabInstance != null && this.fileList.SelectedEntryData != null;
		this.btnWorldDeletePrefab.Enabled = prefabInstance != null;
		this.btnWorldApplyPrefabChanges.Enabled = prefabInstance != null && !prefabInstance.IsBBInSyncWithPrefab();
		this.btnWorldRevertPrefabChanges.Enabled = prefabInstance != null && !prefabInstance.IsBBInSyncWithPrefab() && prefabInstance.bPrefabCopiedIntoWorld;
	}

	// Token: 0x04005F14 RID: 24340
	public static string ID = "";

	// Token: 0x04005F15 RID: 24341
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabGroupList groupList;

	// Token: 0x04005F16 RID: 24342
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_PrefabFileList fileList;

	// Token: 0x04005F17 RID: 24343
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture prefabPreview;

	// Token: 0x04005F18 RID: 24344
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label noPreviewLabel;

	// Token: 0x04005F19 RID: 24345
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnLoad;

	// Token: 0x04005F1A RID: 24346
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnProperties;

	// Token: 0x04005F1B RID: 24347
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSave;

	// Token: 0x04005F1C RID: 24348
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnLoadIntoPrefab;

	// Token: 0x04005F1D RID: 24349
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnApplyLoadedPrefab;

	// Token: 0x04005F1E RID: 24350
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnWorldPlacePrefab;

	// Token: 0x04005F1F RID: 24351
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnWorldReplacePrefab;

	// Token: 0x04005F20 RID: 24352
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnWorldDeletePrefab;

	// Token: 0x04005F21 RID: 24353
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnWorldApplyPrefabChanges;

	// Token: 0x04005F22 RID: 24354
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnWorldRevertPrefabChanges;
}
