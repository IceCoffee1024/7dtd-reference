using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E37 RID: 3639
[Preserve]
public class XUiC_WorldEditor : XUiC_EditingToolsDialogBase
{
	// Token: 0x17000CA3 RID: 3235
	// (get) Token: 0x06007000 RID: 28672 RVA: 0x002B1BF4 File Offset: 0x002AFDF4
	[XuiXmlBinding("startable")]
	public bool Startable
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			XUiC_WorldList xuiC_WorldList = this.worldList;
			return xuiC_WorldList != null && xuiC_WorldList.SelectedEntryIndex >= 0;
		}
	}

	// Token: 0x06007001 RID: 28673 RVA: 0x002B1C0D File Offset: 0x002AFE0D
	public override void Init()
	{
		base.Init();
		XUiC_WorldEditor.ID = base.WindowGroup.Id;
	}

	// Token: 0x06007002 RID: 28674 RVA: 0x002B1C28 File Offset: 0x002AFE28
	public override void OnOpen()
	{
		base.OnOpen();
		this.worldList.SetUserDataStorageTypeFilter(new UserDataStorageType[1]);
		this.worldList.RebuildList(false);
		if (string.IsNullOrEmpty(GamePrefs.GetString(EnumGamePrefs.GameWorld)) || !this.worldList.SelectByName(GamePrefs.GetString(EnumGamePrefs.GameWorld)))
		{
			this.worldList.SelectedEntryIndex = 0;
		}
	}

	// Token: 0x06007003 RID: 28675 RVA: 0x002B1C88 File Offset: 0x002AFE88
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false) && this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
		{
			this.BtnStart_OnPressed(this, 0);
		}
	}

	// Token: 0x06007004 RID: 28676 RVA: 0x0008A069 File Offset: 0x00088269
	[XuiBindEvent("SelectionChanged", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_SelectionChanged(XUiC_List<XUiC_WorldList.WorldListEntry> _list, XUiC_WorldList.WorldListEntry _previousEntry, XUiC_WorldList.WorldListEntry _newEntry)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007005 RID: 28677 RVA: 0x002B1CEF File Offset: 0x002AFEEF
	[XuiBindEvent("ListEntryDoubleClicked", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_OnEntryDoubleClicked(XUiC_List<XUiC_WorldList.WorldListEntry> _list, XUiC_WorldList.WorldListEntry _entry)
	{
		if (this.worldList.SelectedEntryIndex >= 0)
		{
			this.BtnStart_OnPressed(this, -1);
		}
	}

	// Token: 0x06007006 RID: 28678 RVA: 0x002B1D07 File Offset: 0x002AFF07
	[XuiBindEvent("OnPress", "btnDataManagement")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDataManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x06007007 RID: 28679 RVA: 0x002B1D1B File Offset: 0x002AFF1B
	[XuiBindEvent("OnPress", "btnCreate")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCreateManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_WorldEditorCreateWorld.Open(this);
	}

	// Token: 0x06007008 RID: 28680 RVA: 0x002B1D23 File Offset: 0x002AFF23
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDataManagementWindowClosed()
	{
		this.worldList.RebuildList(false);
	}

	// Token: 0x06007009 RID: 28681 RVA: 0x002B1D34 File Offset: 0x002AFF34
	[XuiBindEvent("OnPress", "btnStart")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnStart_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.btnStart.ViewComponent.Enabled)
		{
			return;
		}
		new GameModeEditWorld().ResetGamePrefs();
		PathAbstractions.AbstractedLocation selectedWorld = this.worldList.SelectedEntryData.Location;
		GamePrefs.Set(EnumGamePrefs.GameWorld, selectedWorld.Name);
		if (XUiC_WorldEditor.canSaveWorldIn(selectedWorld))
		{
			this.StartEditor();
			return;
		}
		string text = selectedWorld.Name + "_UserCopy";
		PathAbstractions.AbstractedLocation newLocation = XUiC_WorldEditor.LocationForNewWorld(text);
		int num = 0;
		while (newLocation.Exists())
		{
			string name = selectedWorld.Name;
			string text2 = "_UserCopy";
			int num2;
			num = (num2 = num + 1);
			text = name + text2 + num2.ToString();
			newLocation = XUiC_WorldEditor.LocationForNewWorld(text);
		}
		GamePrefs.Set(EnumGamePrefs.GameWorld, text);
		XUiC_MessageBoxWindowGroup.ShowOkCancel(this.xui, Localization.Get("xuiCreateWorldCanNotEditWorldInGameFolderTitle", false, null), string.Format(Localization.Get("xuiCreateWorldCanNotEditWorldInGameFolderText", false, null), GameIO.GetOsStylePath(newLocation.FullPath)), "", delegate
		{
			Log.Out(string.Concat(new string[] { "Will copy the world from '", selectedWorld.FullPath, "' to '", newLocation.FullPath, "' for editing." }));
			GameIO.CopyDirectory(selectedWorld.FullPath, newLocation.FullPath);
			this.StartEditor();
		}, delegate
		{
			this.xui.playerUI.windowManager.Open(this.windowGroup, true);
		}, false, true, false);
	}

	// Token: 0x0600700A RID: 28682 RVA: 0x002B1E6C File Offset: 0x002B006C
	public void StartEditor()
	{
		GamePrefs.Set(EnumGamePrefs.GameMode, GameModeEditWorld.TypeName);
		GamePrefs.Set(EnumGamePrefs.GameName, "WorldEditor");
		GamePrefs.Set(EnumGamePrefs.GameSaveStorageType, 0);
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		NetworkConnectionError networkConnectionError = SingletonMonoBehaviour<ConnectionManager>.Instance.StartServers(GamePrefs.GetString(EnumGamePrefs.ServerPassword), false);
		if (networkConnectionError != NetworkConnectionError.NoError)
		{
			XUiC_MessageBoxWindowGroup.ShowNetworkError(this.xui, networkConnectionError, false);
		}
	}

	// Token: 0x0600700B RID: 28683 RVA: 0x002B1EDC File Offset: 0x002B00DC
	public static PathAbstractions.AbstractedLocation LocationForNewWorld(string _name)
	{
		PathAbstractions.EAbstractedLocationType eabstractedLocationType = PathAbstractions.EAbstractedLocationType.UserDataPath;
		return PathAbstractions.WorldsSearchPaths.BuildLocation(eabstractedLocationType, null, _name, null, new UserDataStorageType?(UserDataStorageType.DeviceLocal)).Value;
	}

	// Token: 0x0600700C RID: 28684 RVA: 0x001A6B9C File Offset: 0x001A4D9C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canSaveWorldIn(PathAbstractions.AbstractedLocation _location)
	{
		return _location.Type != PathAbstractions.EAbstractedLocationType.GameData;
	}

	// Token: 0x04005468 RID: 21608
	public static string ID = "";

	// Token: 0x04005469 RID: 21609
	[PublicizedFrom(EAccessModifier.Private)]
	public const UserDataStorageType AllowedUserDataStorageType = UserDataStorageType.DeviceLocal;

	// Token: 0x0400546A RID: 21610
	[XuiBindComponent("btnDataManagement", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDataManagement;

	// Token: 0x0400546B RID: 21611
	[XuiBindComponent("btnStart", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStart;

	// Token: 0x0400546C RID: 21612
	[XuiBindComponent("btnCreate", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCreate;

	// Token: 0x0400546D RID: 21613
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldList worldList;
}
