using System;
using System.Collections;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000F64 RID: 3940
[Preserve]
public class XUiC_InGameMenuWindow : XUiController
{
	// Token: 0x06007B02 RID: 31490 RVA: 0x002E992C File Offset: 0x002E7B2C
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

	// Token: 0x06007B03 RID: 31491 RVA: 0x002E9B36 File Offset: 0x002E7D36
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

	// Token: 0x06007B04 RID: 31492 RVA: 0x002E9B68 File Offset: 0x002E7D68
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOptions_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.continueGamePause = true;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		LocalPlayerUI.primaryUI.windowManager.Open(XUiC_OptionsMenuNew.ParentSelector.WindowGroup, false);
	}

	// Token: 0x06007B05 RID: 31493 RVA: 0x002E9BA7 File Offset: 0x002E7DA7
	[PublicizedFrom(EAccessModifier.Private)]
	public void StnSandboxSettings_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open("sandboxOptions", true);
	}

	// Token: 0x06007B06 RID: 31494 RVA: 0x002E9BE0 File Offset: 0x002E7DE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnHelp_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_PrefabEditorHelp.ID, true);
	}

	// Token: 0x06007B07 RID: 31495 RVA: 0x002E9C1C File Offset: 0x002E7E1C
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

	// Token: 0x06007B08 RID: 31496 RVA: 0x002A21E4 File Offset: 0x002A03E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void savePrefab(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
	}

	// Token: 0x06007B09 RID: 31497 RVA: 0x002E9C8F File Offset: 0x002E7E8F
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

	// Token: 0x06007B0A RID: 31498 RVA: 0x002E9CCC File Offset: 0x002E7ECC
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

	// Token: 0x06007B0B RID: 31499 RVA: 0x002E9D23 File Offset: 0x002E7F23
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator DisconnectAfterDisplayingExitingGameMessage()
	{
		yield return GameManager.Instance.ShowExitingGameUICoroutine();
		GameManager.Instance.Disconnect();
		yield break;
	}

	// Token: 0x06007B0C RID: 31500 RVA: 0x002E9D2B File Offset: 0x002E7F2B
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnExportPrefab_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_ExportPrefab.Open(this.xui);
	}

	// Token: 0x06007B0D RID: 31501 RVA: 0x002E9D54 File Offset: 0x002E7F54
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnTpPoi_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.xui.playerUI.windowManager.Open(XUiC_PoiTeleportMenu.ID, true);
	}

	// Token: 0x06007B0E RID: 31502 RVA: 0x002E9D8D File Offset: 0x002E7F8D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOpenConsole_OnPressed(XUiController _sender, int _mouseButton)
	{
		GUIWindowConsole.Open();
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06007B0F RID: 31503 RVA: 0x002E9DB0 File Offset: 0x002E7FB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBugReport_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		XUiC_BugReportWindow.Open();
	}

	// Token: 0x06007B10 RID: 31504 RVA: 0x002E9DD4 File Offset: 0x002E7FD4
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

	// Token: 0x06007B11 RID: 31505 RVA: 0x002E9FD8 File Offset: 0x002E81D8
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

	// Token: 0x06007B12 RID: 31506 RVA: 0x002EA044 File Offset: 0x002E8244
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

	// Token: 0x06007B13 RID: 31507 RVA: 0x002EA094 File Offset: 0x002E8294
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

	// Token: 0x04005B57 RID: 23383
	public static string ID = "";

	// Token: 0x04005B58 RID: 23384
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnInvite;

	// Token: 0x04005B59 RID: 23385
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOptions;

	// Token: 0x04005B5A RID: 23386
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSandboxSettings;

	// Token: 0x04005B5B RID: 23387
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnHelp;

	// Token: 0x04005B5C RID: 23388
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnSave;

	// Token: 0x04005B5D RID: 23389
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnExit;

	// Token: 0x04005B5E RID: 23390
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnExportPrefab;

	// Token: 0x04005B5F RID: 23391
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnTpPoi;

	// Token: 0x04005B60 RID: 23392
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOpenConsole;

	// Token: 0x04005B61 RID: 23393
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnBugReport;

	// Token: 0x04005B62 RID: 23394
	[PublicizedFrom(EAccessModifier.Private)]
	public bool continueGamePause;

	// Token: 0x04005B63 RID: 23395
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string ServerInfoWindowGroupName = "serverinfowindow";

	// Token: 0x04005B64 RID: 23396
	[PublicizedFrom(EAccessModifier.Private)]
	public string serverInfoWindowGroup;
}
