using System;
using System.Collections;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F55 RID: 3925
[Preserve]
public class XUiC_InGameMenuWindow : XUiController
{
	// Token: 0x06007ABF RID: 31423 RVA: 0x002E8030 File Offset: 0x002E6230
	public override void Init()
	{
		base.Init();
		XUiC_InGameMenuWindow.ID = base.WindowGroup.Id;
		this.btnInvite = base.GetChildById("btnInvite").GetChildByType<XUiC_SimpleButton>();
		this.btnInvite.OnPressed += this.BtnInvite_OnPressed;
		this.btnOptions = base.GetChildById("btnOptions").GetChildByType<XUiC_SimpleButton>();
		this.btnOptions.OnPressed += this.BtnOptions_OnPressed;
		this.btnSandboxSettings = base.GetChildById("btnSandboxSettings").GetChildByType<XUiC_SimpleButton>();
		this.btnSandboxSettings.OnPressed += this.StnSandboxSettings_OnPressed;
		this.btnHelp = base.GetChildById("btnHelp").GetChildByType<XUiC_SimpleButton>();
		this.btnHelp.OnPressed += this.BtnHelp_OnPressed;
		this.btnSave = base.GetChildById("btnSave").GetChildByType<XUiC_SimpleButton>();
		this.btnSave.OnPressed += this.BtnSave_OnPressed;
		this.btnExit = base.GetChildById("btnExit").GetChildByType<XUiC_SimpleButton>();
		this.btnExit.OnPressed += this.BtnExit_OnPressed;
		this.btnExportPrefab = base.GetChildById("btnExportPrefab").GetChildByType<XUiC_SimpleButton>();
		this.btnExportPrefab.OnPressed += this.BtnExportPrefab_OnPressed;
		this.btnTpPoi = base.GetChildById("btnTpPoi").GetChildByType<XUiC_SimpleButton>();
		this.btnTpPoi.OnPressed += this.BtnTpPoi_OnPressed;
		this.btnOpenConsole = base.GetChildById("btnOpenConsole").GetChildByType<XUiC_SimpleButton>();
		this.btnOpenConsole.OnPressed += this.BtnOpenConsole_OnPressed;
		this.btnBugReport = base.GetChildById("btnBugReport").GetChildByType<XUiC_SimpleButton>();
		this.btnBugReport.OnPressed += this.BtnBugReport_OnPressed;
		XUiController xuiController = this.xui.FindWindowGroupByName(XUiC_InGameMenuWindow.ServerInfoWindowGroupName);
		if (xuiController != null)
		{
			this.serverInfoWindowGroup = xuiController.WindowGroup.Id;
		}
	}

	// Token: 0x06007AC0 RID: 31424 RVA: 0x002E823A File Offset: 0x002E643A
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnInvite_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		IMultiplayerInvitationDialog multiplayerInvitationDialog = PlatformManager.NativePlatform.MultiplayerInvitationDialog;
		if (multiplayerInvitationDialog == null)
		{
			return;
		}
		multiplayerInvitationDialog.ShowInviteDialog();
	}

	// Token: 0x06007AC1 RID: 31425 RVA: 0x002E826C File Offset: 0x002E646C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOptions_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.continueGamePause = true;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		LocalPlayerUI.primaryUI.windowManager.Open(XUiC_OptionsMenuNew.ParentSelector.WindowGroup, false);
	}

	// Token: 0x06007AC2 RID: 31426 RVA: 0x002E82AB File Offset: 0x002E64AB
	[PublicizedFrom(EAccessModifier.Private)]
	public void StnSandboxSettings_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open("sandboxOptions", true);
	}

	// Token: 0x06007AC3 RID: 31427 RVA: 0x002E82E4 File Offset: 0x002E64E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnHelp_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_PrefabEditorHelp.ID, true);
	}

	// Token: 0x06007AC4 RID: 31428 RVA: 0x002E8320 File Offset: 0x002E6520
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (PrefabEditModeManager.Instance.IsActive())
		{
			XUiC_SaveDirtyPrefab.Show(this.xui, new Action<XUiC_SaveDirtyPrefab.ESelectedAction>(this.savePrefab), XUiC_SaveDirtyPrefab.EMode.ForceSave);
			return;
		}
		GameManager.Instance.SaveLocalPlayerData();
		GameManager.Instance.SaveWorld();
		GameManager.ShowTooltip(GameManager.Instance.World.GetLocalPlayers()[0], Localization.Get("xuiWorldEditorSaved", false, null), false, false, 0f);
	}

	// Token: 0x06007AC5 RID: 31429 RVA: 0x002A0CBC File Offset: 0x0029EEBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void savePrefab(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
	}

	// Token: 0x06007AC6 RID: 31430 RVA: 0x002E8393 File Offset: 0x002E6593
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnExit_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (PrefabEditModeManager.Instance.IsActive() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			XUiC_SaveDirtyPrefab.Show(this.xui, new Action<XUiC_SaveDirtyPrefab.ESelectedAction>(this.exitGame), XUiC_SaveDirtyPrefab.EMode.AskSaveIfDirty);
			return;
		}
		this.exitGame(XUiC_SaveDirtyPrefab.ESelectedAction.DontSave);
	}

	// Token: 0x06007AC7 RID: 31431 RVA: 0x002E83D0 File Offset: 0x002E65D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void exitGame(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		if (_action == XUiC_SaveDirtyPrefab.ESelectedAction.Cancel)
		{
			this.xui.playerUI.windowManager.Open(this.windowGroup, true);
			return;
		}
		GameManager.Instance.SetActiveBlockTool(null);
		if (PlatformApplicationManager.IsRestartRequired)
		{
			ThreadManager.StartCoroutine(this.DisconnectAfterDisplayingExitingGameMessage());
			return;
		}
		GameManager.Instance.Disconnect();
	}

	// Token: 0x06007AC8 RID: 31432 RVA: 0x002E8427 File Offset: 0x002E6627
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator DisconnectAfterDisplayingExitingGameMessage()
	{
		yield return GameManager.Instance.ShowExitingGameUICoroutine();
		GameManager.Instance.Disconnect();
		yield break;
	}

	// Token: 0x06007AC9 RID: 31433 RVA: 0x002E842F File Offset: 0x002E662F
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnExportPrefab_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_ExportPrefab.Open(this.xui);
	}

	// Token: 0x06007ACA RID: 31434 RVA: 0x002E8458 File Offset: 0x002E6658
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnTpPoi_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_PoiTeleportMenu.ID, true);
	}

	// Token: 0x06007ACB RID: 31435 RVA: 0x002E8491 File Offset: 0x002E6691
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOpenConsole_OnPressed(XUiController _sender, int _mouseButton)
	{
		GUIWindowConsole.Open();
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007ACC RID: 31436 RVA: 0x002E84B4 File Offset: 0x002E66B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBugReport_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_BugReportWindow.Open();
	}

	// Token: 0x06007ACD RID: 31437 RVA: 0x002E84D8 File Offset: 0x002E66D8
	public override void OnOpen()
	{
		base.OnOpen();
		if (PlatformManager.NativePlatform.MultiplayerInvitationDialog != null)
		{
			int num = SingletonMonoBehaviour<ConnectionManager>.Instance.ClientCount() + (GameManager.IsDedicatedServer ? 0 : 1);
			int @int = GamePrefs.GetInt(EnumGamePrefs.ServerMaxPlayerCount);
			this.btnInvite.Enabled = PlatformManager.NativePlatform.MultiplayerInvitationDialog.CanShow && (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || num < @int);
			this.btnInvite.ViewComponent.IsVisible = !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || SingletonMonoBehaviour<ConnectionManager>.Instance.HasRunningServers;
		}
		else
		{
			this.btnInvite.ViewComponent.IsVisible = false;
		}
		this.btnSave.ViewComponent.IsVisible = GameManager.Instance.IsEditMode() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
		this.btnHelp.ViewComponent.IsVisible = GameManager.Instance.IsEditMode();
		this.btnExportPrefab.ViewComponent.IsVisible = GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled);
		this.btnTpPoi.ViewComponent.IsVisible = GameManager.Instance.GetDynamicPrefabDecorator() != null && (GameManager.Instance.IsEditMode() || GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled));
		this.continueGamePause = false;
		GameManager.Instance.Pause(true);
		XUiC_FocusedBlockHealth.SetData(this.xui.playerUI, null, 0f);
		this.xui.playerUI.windowManager.Close("toolbelt");
		XUi.InGameMenuOpen = true;
		if (GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled))
		{
			this.xui.playerUI.windowManager.Open(XUiC_EditorPanelSelector.ID, false);
		}
		if (this.serverInfoWindowGroup != null)
		{
			this.xui.playerUI.windowManager.Open(this.serverInfoWindowGroup, false);
		}
		if (this.btnInvite.ViewComponent.IsVisible)
		{
			this.btnInvite.SelectCursorElement(true, false);
		}
		else
		{
			this.btnOptions.SelectCursorElement(true, false);
		}
		base.RefreshBindings();
	}

	// Token: 0x06007ACE RID: 31438 RVA: 0x002E86DC File Offset: 0x002E68DC
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close(XUiC_EditorPanelSelector.ID);
		if (!this.continueGamePause)
		{
			GameManager.Instance.Pause(false);
			XUi.InGameMenuOpen = false;
		}
		if (this.serverInfoWindowGroup != null)
		{
			this.xui.playerUI.windowManager.Close(this.serverInfoWindowGroup);
		}
	}

	// Token: 0x06007ACF RID: 31439 RVA: 0x002E8748 File Offset: 0x002E6948
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (PrefabEditModeManager.Instance.IsActive())
		{
			bool flag = PrefabEditModeManager.Instance.VoxelPrefab != null;
			this.btnSave.Enabled = flag;
		}
		this.btnExportPrefab.Enabled = BlockToolSelection.Instance.SelectionActive;
	}

	// Token: 0x06007AD0 RID: 31440 RVA: 0x002E8798 File Offset: 0x002E6998
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "creativeenabled")
		{
			_value = AchievementUtils.IsCreativeModeActive().ToString();
			return true;
		}
		if (_bindingName == "bug_reporting")
		{
			_value = BacktraceUtils.BugReportFeature.ToString();
			return true;
		}
		if (!(_bindingName == "console_button"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = GamePrefs.GetBool(EnumGamePrefs.OptionsShowConsoleButton).ToString();
		return true;
	}

	// Token: 0x04005B24 RID: 23332
	public static string ID = "";

	// Token: 0x04005B25 RID: 23333
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnInvite;

	// Token: 0x04005B26 RID: 23334
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOptions;

	// Token: 0x04005B27 RID: 23335
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSandboxSettings;

	// Token: 0x04005B28 RID: 23336
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnHelp;

	// Token: 0x04005B29 RID: 23337
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSave;

	// Token: 0x04005B2A RID: 23338
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnExit;

	// Token: 0x04005B2B RID: 23339
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnExportPrefab;

	// Token: 0x04005B2C RID: 23340
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnTpPoi;

	// Token: 0x04005B2D RID: 23341
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOpenConsole;

	// Token: 0x04005B2E RID: 23342
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnBugReport;

	// Token: 0x04005B2F RID: 23343
	[PublicizedFrom(EAccessModifier.Private)]
	public bool continueGamePause;

	// Token: 0x04005B30 RID: 23344
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string ServerInfoWindowGroupName = "serverinfowindow";

	// Token: 0x04005B31 RID: 23345
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverInfoWindowGroup;
}
