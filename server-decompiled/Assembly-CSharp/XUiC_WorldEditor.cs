using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E5B RID: 3675
[Preserve]
public class XUiC_WorldEditor : XUiC_EditingToolsDialogBase
{
	// Token: 0x17000CD7 RID: 3287
	// (get) Token: 0x06007098 RID: 28824 RVA: 0x002B84C8 File Offset: 0x002B66C8
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

	// Token: 0x06007099 RID: 28825 RVA: 0x002B84E1 File Offset: 0x002B66E1
	public override void Init()
	{
		base.Init();
		XUiC_WorldEditor.ID = base.WindowGroup.Id;
	}

	// Token: 0x0600709A RID: 28826 RVA: 0x002B84FC File Offset: 0x002B66FC
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

	// Token: 0x0600709B RID: 28827 RVA: 0x002B855C File Offset: 0x002B675C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false) && this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
		{
			this.BtnStart_OnPressed(this, 0);
		}
	}

	// Token: 0x0600709C RID: 28828 RVA: 0x0008A995 File Offset: 0x00088B95
	[XuiBindEvent("SelectionChanged", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_SelectionChanged(XUiC_List<XUiC_WorldList.WorldListEntry> _list, XUiC_WorldList.WorldListEntry _previousEntry, XUiC_WorldList.WorldListEntry _newEntry)
	{
		this.IsDirty = true;
	}

	// Token: 0x0600709D RID: 28829 RVA: 0x002B85C3 File Offset: 0x002B67C3
	[XuiBindEvent("ListEntryDoubleClicked", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_OnEntryDoubleClicked(XUiC_List<XUiC_WorldList.WorldListEntry> _list, XUiC_WorldList.WorldListEntry _entry)
	{
		if (this.worldList.SelectedEntryIndex >= 0)
		{
			this.BtnStart_OnPressed(this, -1);
		}
	}

	// Token: 0x0600709E RID: 28830 RVA: 0x002B85DB File Offset: 0x002B67DB
	[XuiBindEvent("OnPress", "btnDataManagement")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDataManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x0600709F RID: 28831 RVA: 0x002B85EF File Offset: 0x002B67EF
	[XuiBindEvent("OnPress", "btnCreate")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCreateManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_WorldEditorCreateWorld.Open(this);
	}

	// Token: 0x060070A0 RID: 28832 RVA: 0x002B85F7 File Offset: 0x002B67F7
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDataManagementWindowClosed()
	{
		this.worldList.RebuildList(false);
	}

	// Token: 0x060070A1 RID: 28833 RVA: 0x002B8608 File Offset: 0x002B6808
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

	// Token: 0x060070A2 RID: 28834 RVA: 0x002B8740 File Offset: 0x002B6940
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

	// Token: 0x060070A3 RID: 28835 RVA: 0x002B87B0 File Offset: 0x002B69B0
	public static PathAbstractions.AbstractedLocation LocationForNewWorld(string _name)
	{
		PathAbstractions.EAbstractedLocationType eabstractedLocationType = PathAbstractions.EAbstractedLocationType.UserDataPath;
		return PathAbstractions.WorldsSearchPaths.BuildLocation(eabstractedLocationType, null, _name, null, new UserDataStorageType?(UserDataStorageType.DeviceLocal)).Value;
	}

	// Token: 0x060070A4 RID: 28836 RVA: 0x001A9274 File Offset: 0x001A7474
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canSaveWorldIn(PathAbstractions.AbstractedLocation _location)
	{
		return _location.Type != PathAbstractions.EAbstractedLocationType.GameData;
	}

	// Token: 0x04005543 RID: 21827
	public static string ID = "";

	// Token: 0x04005544 RID: 21828
	[PublicizedFrom(EAccessModifier.Private)]
	public const UserDataStorageType AllowedUserDataStorageType = UserDataStorageType.DeviceLocal;

	// Token: 0x04005545 RID: 21829
	[XuiBindComponent("btnDataManagement", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDataManagement;

	// Token: 0x04005546 RID: 21830
	[XuiBindComponent("btnStart", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStart;

	// Token: 0x04005547 RID: 21831
	[XuiBindComponent("btnCreate", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCreate;

	// Token: 0x04005548 RID: 21832
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldList worldList;
}
