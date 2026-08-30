using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000FFC RID: 4092
[Preserve]
public class XUiC_ReportPlayer : XUiController
{
	// Token: 0x17000EF3 RID: 3827
	// (get) Token: 0x06008142 RID: 33090 RVA: 0x00312A04 File Offset: 0x00310C04
	// (set) Token: 0x06008143 RID: 33091 RVA: 0x00312A15 File Offset: 0x00310C15
	[XuiXmlBinding("reportedplayer")]
	public string ReportedPlayerName
	{
		get
		{
			return this.reportedPlayerName ?? "";
		}
		set
		{
			this.reportedPlayerName = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000EF4 RID: 3828
	// (get) Token: 0x06008144 RID: 33092 RVA: 0x00312A25 File Offset: 0x00310C25
	// (set) Token: 0x06008145 RID: 33093 RVA: 0x00312A2D File Offset: 0x00310C2D
	public int ReportedEntityId
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.reportedEntityId;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.reportedEntityId = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000EF5 RID: 3829
	// (get) Token: 0x06008146 RID: 33094 RVA: 0x00312A3D File Offset: 0x00310C3D
	[XuiXmlBinding("playeronserver")]
	public bool PlayerOnServer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.ReportedEntityId != -1;
		}
	}

	// Token: 0x17000EF6 RID: 3830
	// (get) Token: 0x06008147 RID: 33095 RVA: 0x00312A59 File Offset: 0x00310C59
	// (set) Token: 0x06008148 RID: 33096 RVA: 0x00312A6A File Offset: 0x00310C6A
	[XuiXmlBinding("inputWarning")]
	public string InputErrorMessage
	{
		get
		{
			return this.inputErrorMessage ?? "";
		}
		set
		{
			this.inputErrorMessage = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x06008149 RID: 33097 RVA: 0x00312A7C File Offset: 0x00310C7C
	public override void Init()
	{
		base.Init();
		XUiC_ReportPlayer.id = base.WindowGroup.Id;
		IPlayerReporting playerReporting = PlatformManager.MultiPlatform.PlayerReporting;
		IList<IPlayerReporting.PlayerReportCategory> list = ((playerReporting != null) ? playerReporting.ReportCategories() : null);
		if (list != null)
		{
			foreach (IPlayerReporting.PlayerReportCategory playerReportCategory in list)
			{
				this.cbxCategory.Elements.Add(playerReportCategory);
			}
		}
	}

	// Token: 0x0600814A RID: 33098 RVA: 0x0028AD37 File Offset: 0x00288F37
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600814B RID: 33099 RVA: 0x00088DF5 File Offset: 0x00086FF5
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600814C RID: 33100 RVA: 0x00312B00 File Offset: 0x00310D00
	[XuiBindEvent("OnPress", "btnKickPlayer")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnKick_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.reportedPlayerData != null)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			ClientInfo clientInfo;
			if (ConsoleHelper.ParseParamPartialNameOrId(this.reportedPlayerData.PlayerName.Text, out platformUserIdentifierAbs, out clientInfo, true) == 1)
			{
				DateTime maxValue = DateTime.MaxValue;
				string text = "";
				if (clientInfo != null)
				{
					ClientInfo clientInfo2 = clientInfo;
					GameUtils.EKickReason ekickReason = GameUtils.EKickReason.ManualKick;
					int num = 0;
					string text2 = (string.IsNullOrEmpty(text) ? "" : text);
					GameUtils.KickPlayerForClientInfo(clientInfo2, new GameUtils.KickPlayerData(ekickReason, num, maxValue, text2));
				}
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("[xui] Kick Succeeded");
		}
		else
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("[xui] Failed to find player to kick");
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600814D RID: 33101 RVA: 0x00312BA0 File Offset: 0x00310DA0
	[XuiBindEvent("OnPress", "btnSend")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSend_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.reportedPlayerData == null)
		{
			this.reportSentMessageBox(false);
			return;
		}
		string text = this.txtMessage.Text;
		if (!string.IsNullOrEmpty(this.initialText))
		{
			text = text + "\n" + string.Format(Localization.Get("xuiReportAutomatedMessage", false, null), this.initialText, this.initialCategory);
		}
		IPlayerReporting playerReporting = PlatformManager.MultiPlatform.PlayerReporting;
		if (playerReporting != null)
		{
			playerReporting.ReportPlayer(this.reportedPlayerData.PrimaryId, this.cbxCategory.Value, text, new Action<bool>(this.reportSentMessageBox));
		}
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600814E RID: 33102 RVA: 0x00312C58 File Offset: 0x00310E58
	[PublicizedFrom(EAccessModifier.Private)]
	public void reportSentMessageBox(bool _success)
	{
		string text = (_success ? Localization.Get("xuiReportPlayerSuccess", false, null) : Localization.Get("xuiReportPlayerFail", false, null));
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiReportPlayerHeader", false, null), text, "", null, false, XUiC_ReportPlayer.OpenDialogsModal, false);
	}

	// Token: 0x0600814F RID: 33103 RVA: 0x00312CA8 File Offset: 0x00310EA8
	public override void OnOpen()
	{
		base.OnOpen();
		this.txtMessage.Text = this.initialText;
		this.InputErrorMessage = null;
		XUiC_ComboBoxList<IPlayerReporting.PlayerReportCategory> xuiC_ComboBoxList = this.cbxCategory;
		List<IPlayerReporting.PlayerReportCategory> elements = this.cbxCategory.Elements;
		IPlayerReporting playerReporting = PlatformManager.MultiPlatform.PlayerReporting;
		xuiC_ComboBoxList.SelectedIndex = elements.IndexOf((playerReporting != null) ? playerReporting.GetPlayerReportCategoryMapping(this.initialCategory) : null);
		if (this.cbxCategory.SelectedIndex == -1)
		{
			this.cbxCategory.SelectedIndex = 0;
		}
		if (this.reportedPlayerData != null)
		{
			this.ReportedPlayerName = GeneratedTextManager.GetDisplayTextImmediately(this.reportedPlayerData.PlayerName, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
			PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
			int? num;
			if (persistentPlayers == null)
			{
				num = null;
			}
			else
			{
				PersistentPlayerData playerData = persistentPlayers.GetPlayerData(this.reportedPlayerData.PrimaryId);
				num = ((playerData != null) ? new int?(playerData.EntityId) : null);
			}
			this.ReportedEntityId = num ?? (-1);
			return;
		}
		Log.Out("Sign does not have an author, cannot report player.");
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x06008150 RID: 33104 RVA: 0x00312DC8 File Offset: 0x00310FC8
	public override void OnClose()
	{
		base.OnClose();
		this.reportedPlayerData = null;
	}

	// Token: 0x06008151 RID: 33105 RVA: 0x00312DD7 File Offset: 0x00310FD7
	[XuiBindEvent("OnInputErrorHandler", "txtMessage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateErrorMessage(XUiController _sender, string _errorMessage)
	{
		this.InputErrorMessage = _errorMessage;
	}

	// Token: 0x06008152 RID: 33106 RVA: 0x00312DE0 File Offset: 0x00310FE0
	public static void Open(PlayerData _reportedPlayerData, EnumReportCategory _initialCategory = EnumReportCategory.None, string _initialText = "")
	{
		if (LocalPlayerUI.primaryUI.windowManager.IsWindowOpen(XUiC_ReportPlayer.id))
		{
			return;
		}
		XUiC_ReportPlayer childByType = LocalPlayerUI.primaryUI.xui.GetChildByType<XUiC_ReportPlayer>();
		childByType.reportedPlayerData = _reportedPlayerData;
		childByType.initialCategory = _initialCategory;
		childByType.initialText = _initialText;
		LocalPlayerUI.primaryUI.windowManager.Open(childByType.windowGroup, XUiC_ReportPlayer.OpenDialogsModal);
	}

	// Token: 0x17000EF7 RID: 3831
	// (get) Token: 0x06008153 RID: 33107 RVA: 0x002A4321 File Offset: 0x002A2521
	public static bool OpenDialogsModal
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.gameStateManager.IsGameStarted();
		}
	}

	// Token: 0x04005FA9 RID: 24489
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x04005FAA RID: 24490
	[PublicizedFrom(EAccessModifier.Private)]
	public string initialText = "";

	// Token: 0x04005FAB RID: 24491
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumReportCategory initialCategory;

	// Token: 0x04005FAC RID: 24492
	[XuiBindComponent("cbxCategory", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPlayerReporting.PlayerReportCategory> cbxCategory;

	// Token: 0x04005FAD RID: 24493
	[XuiBindComponent("txtMessage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtMessage;

	// Token: 0x04005FAE RID: 24494
	[XuiBindComponent("btnKick", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnKickPlayer;

	// Token: 0x04005FAF RID: 24495
	[XuiBindComponent("btnSend", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSend;

	// Token: 0x04005FB0 RID: 24496
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04005FB1 RID: 24497
	[PublicizedFrom(EAccessModifier.Private)]
	public string reportedPlayerName;

	// Token: 0x04005FB2 RID: 24498
	[PublicizedFrom(EAccessModifier.Private)]
	public int reportedEntityId;

	// Token: 0x04005FB3 RID: 24499
	[PublicizedFrom(EAccessModifier.Private)]
	public string inputErrorMessage;

	// Token: 0x04005FB4 RID: 24500
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerData reportedPlayerData;
}
