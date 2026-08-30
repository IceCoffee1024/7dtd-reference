using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001015 RID: 4117
[Preserve]
public class XUiC_ReportPlayer : XUiController
{
	// Token: 0x17000F2C RID: 3884
	// (get) Token: 0x06008234 RID: 33332 RVA: 0x00317234 File Offset: 0x00315434
	// (set) Token: 0x06008235 RID: 33333 RVA: 0x00317245 File Offset: 0x00315445
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

	// Token: 0x17000F2D RID: 3885
	// (get) Token: 0x06008236 RID: 33334 RVA: 0x00317255 File Offset: 0x00315455
	// (set) Token: 0x06008237 RID: 33335 RVA: 0x0031725D File Offset: 0x0031545D
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

	// Token: 0x17000F2E RID: 3886
	// (get) Token: 0x06008238 RID: 33336 RVA: 0x0031726D File Offset: 0x0031546D
	[XuiXmlBinding("playeronserver")]
	public bool PlayerOnServer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.ReportedEntityId != -1;
		}
	}

	// Token: 0x17000F2F RID: 3887
	// (get) Token: 0x06008239 RID: 33337 RVA: 0x00317289 File Offset: 0x00315489
	// (set) Token: 0x0600823A RID: 33338 RVA: 0x0031729A File Offset: 0x0031549A
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

	// Token: 0x0600823B RID: 33339 RVA: 0x003172AC File Offset: 0x003154AC
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

	// Token: 0x0600823C RID: 33340 RVA: 0x0028DC1B File Offset: 0x0028BE1B
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600823D RID: 33341 RVA: 0x00089495 File Offset: 0x00087695
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600823E RID: 33342 RVA: 0x00317330 File Offset: 0x00315530
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

	// Token: 0x0600823F RID: 33343 RVA: 0x003173D0 File Offset: 0x003155D0
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

	// Token: 0x06008240 RID: 33344 RVA: 0x00317488 File Offset: 0x00315688
	[PublicizedFrom(EAccessModifier.Private)]
	public void reportSentMessageBox(bool _success)
	{
		string text = (_success ? Localization.Get("xuiReportPlayerSuccess", false, null) : Localization.Get("xuiReportPlayerFail", false, null));
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiReportPlayerHeader", false, null), text, "", null, false, XUiC_ReportPlayer.OpenDialogsModal, false);
	}

	// Token: 0x06008241 RID: 33345 RVA: 0x003174D8 File Offset: 0x003156D8
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

	// Token: 0x06008242 RID: 33346 RVA: 0x003175F8 File Offset: 0x003157F8
	public override void OnClose()
	{
		base.OnClose();
		this.reportedPlayerData = null;
	}

	// Token: 0x06008243 RID: 33347 RVA: 0x00317607 File Offset: 0x00315807
	[XuiBindEvent("OnInputErrorHandler", "txtMessage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateErrorMessage(XUiController _sender, string _errorMessage)
	{
		this.InputErrorMessage = _errorMessage;
	}

	// Token: 0x06008244 RID: 33348 RVA: 0x00317610 File Offset: 0x00315810
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

	// Token: 0x17000F30 RID: 3888
	// (get) Token: 0x06008245 RID: 33349 RVA: 0x002A77B5 File Offset: 0x002A59B5
	public static bool OpenDialogsModal
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.gameStateManager.IsGameStarted();
		}
	}

	// Token: 0x04006070 RID: 24688
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x04006071 RID: 24689
	[PublicizedFrom(EAccessModifier.Private)]
	public string initialText = "";

	// Token: 0x04006072 RID: 24690
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumReportCategory initialCategory;

	// Token: 0x04006073 RID: 24691
	[XuiBindComponent("cbxCategory", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPlayerReporting.PlayerReportCategory> cbxCategory;

	// Token: 0x04006074 RID: 24692
	[XuiBindComponent("txtMessage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtMessage;

	// Token: 0x04006075 RID: 24693
	[XuiBindComponent("btnKick", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnKickPlayer;

	// Token: 0x04006076 RID: 24694
	[XuiBindComponent("btnSend", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSend;

	// Token: 0x04006077 RID: 24695
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x04006078 RID: 24696
	[PublicizedFrom(EAccessModifier.Private)]
	public string reportedPlayerName;

	// Token: 0x04006079 RID: 24697
	[PublicizedFrom(EAccessModifier.Private)]
	public int reportedEntityId;

	// Token: 0x0400607A RID: 24698
	[PublicizedFrom(EAccessModifier.Private)]
	public string inputErrorMessage;

	// Token: 0x0400607B RID: 24699
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerData reportedPlayerData;
}
