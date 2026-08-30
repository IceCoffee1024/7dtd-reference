using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000E4C RID: 3660
[Preserve]
public class XUiC_WorldEditor : XUiC_EditingToolsDialogBase
{
	// Token: 0x17000CBC RID: 3260
	// (get) Token: 0x060070A0 RID: 28832 RVA: 0x002B5118 File Offset: 0x002B3318
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

	// Token: 0x060070A1 RID: 28833 RVA: 0x002B5131 File Offset: 0x002B3331
	public override void Init()
	{
		base.Init();
		XUiC_WorldEditor.ID = base.WindowGroup.Id;
	}

	// Token: 0x060070A2 RID: 28834 RVA: 0x002B514C File Offset: 0x002B334C
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

	// Token: 0x060070A3 RID: 28835 RVA: 0x002B51AC File Offset: 0x002B33AC
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
		if (XUiUtils.HotkeysAllowedFor(this.viewComponent ?? this.children[0].ViewComponent, false) && this.xui.playerUI.playerInput.GUIActions.Apply.WasReleased)
		{
			this.BtnStart_OnPressed(this, 0);
		}
	}

	// Token: 0x060070A4 RID: 28836 RVA: 0x0008A715 File Offset: 0x00088915
	[XuiBindEvent("SelectionChanged", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_SelectionChanged(XUiC_List<XUiC_WorldList.WorldListEntry> _list, XUiC_WorldList.WorldListEntry _previousEntry, XUiC_WorldList.WorldListEntry _newEntry)
	{
		this.IsDirty = true;
	}

	// Token: 0x060070A5 RID: 28837 RVA: 0x002B5213 File Offset: 0x002B3413
	[XuiBindEvent("ListEntryDoubleClicked", "worldList")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void WorldList_OnEntryDoubleClicked(XUiC_List<XUiC_WorldList.WorldListEntry> _list, XUiC_WorldList.WorldListEntry _entry)
	{
		if (this.worldList.SelectedEntryIndex >= 0)
		{
			this.BtnStart_OnPressed(this, -1);
		}
	}

	// Token: 0x060070A6 RID: 28838 RVA: 0x002B522B File Offset: 0x002B342B
	[XuiBindEvent("OnPress", "btnDataManagement")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDataManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_DataManagement.OpenDataManagementWindow(this, new Action(this.OnDataManagementWindowClosed));
	}

	// Token: 0x060070A7 RID: 28839 RVA: 0x002B523F File Offset: 0x002B343F
	[XuiBindEvent("OnPress", "btnCreate")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCreateManagement_OnPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_WorldEditorCreateWorld.Open(this);
	}

	// Token: 0x060070A8 RID: 28840 RVA: 0x002B5247 File Offset: 0x002B3447
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDataManagementWindowClosed()
	{
		this.worldList.RebuildList(false);
	}

	// Token: 0x060070A9 RID: 28841 RVA: 0x002B5258 File Offset: 0x002B3458
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

	// Token: 0x060070AA RID: 28842 RVA: 0x002B5390 File Offset: 0x002B3590
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

	// Token: 0x060070AB RID: 28843 RVA: 0x002B5400 File Offset: 0x002B3600
	public static PathAbstractions.AbstractedLocation LocationForNewWorld(string _name)
	{
		PathAbstractions.EAbstractedLocationType eabstractedLocationType = PathAbstractions.EAbstractedLocationType.UserDataPath;
		return PathAbstractions.WorldsSearchPaths.BuildLocation(eabstractedLocationType, null, _name, null, new UserDataStorageType?(UserDataStorageType.DeviceLocal)).Value;
	}

	// Token: 0x060070AC RID: 28844 RVA: 0x001A9170 File Offset: 0x001A7370
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canSaveWorldIn(PathAbstractions.AbstractedLocation _location)
	{
		return _location.Type != PathAbstractions.EAbstractedLocationType.GameData;
	}

	// Token: 0x0400550C RID: 21772
	public static string ID = "";

	// Token: 0x0400550D RID: 21773
	[PublicizedFrom(EAccessModifier.Private)]
	public const UserDataStorageType AllowedUserDataStorageType = UserDataStorageType.DeviceLocal;

	// Token: 0x0400550E RID: 21774
	[XuiBindComponent("btnDataManagement", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnDataManagement;

	// Token: 0x0400550F RID: 21775
	[XuiBindComponent("btnStart", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnStart;

	// Token: 0x04005510 RID: 21776
	[XuiBindComponent("btnCreate", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCreate;

	// Token: 0x04005511 RID: 21777
	[XuiBindComponent(true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_WorldList worldList;
}
