using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001024 RID: 4132
[Preserve]
public class XUiC_ReportPlayer : XUiController
{
	// Token: 0x17000F53 RID: 3923
	// (get) Token: 0x06008243 RID: 33347 RVA: 0x0031A264 File Offset: 0x00318464
	// (set) Token: 0x06008244 RID: 33348 RVA: 0x0031A275 File Offset: 0x00318475
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

	// Token: 0x17000F54 RID: 3924
	// (get) Token: 0x06008245 RID: 33349 RVA: 0x0031A285 File Offset: 0x00318485
	// (set) Token: 0x06008246 RID: 33350 RVA: 0x0031A28D File Offset: 0x0031848D
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

	// Token: 0x17000F55 RID: 3925
	// (get) Token: 0x06008247 RID: 33351 RVA: 0x0031A29D File Offset: 0x0031849D
	[XuiXmlBinding("playeronserver")]
	public bool PlayerOnServer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.ReportedEntityId != -1;
		}
	}

	// Token: 0x17000F56 RID: 3926
	// (get) Token: 0x06008248 RID: 33352 RVA: 0x0031A2B9 File Offset: 0x003184B9
	// (set) Token: 0x06008249 RID: 33353 RVA: 0x0031A2CA File Offset: 0x003184CA
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

	// Token: 0x0600824A RID: 33354 RVA: 0x0031A2DC File Offset: 0x003184DC
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

	// Token: 0x0600824B RID: 33355 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600824C RID: 33356 RVA: 0x00089715 File Offset: 0x00087915
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600824D RID: 33357 RVA: 0x0031A360 File Offset: 0x00318560
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

	// Token: 0x0600824E RID: 33358 RVA: 0x0031A400 File Offset: 0x00318600
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

	// Token: 0x0600824F RID: 33359 RVA: 0x0031A4B8 File Offset: 0x003186B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void reportSentMessageBox(bool _success)
	{
		string text = (_success ? Localization.Get("xuiReportPlayerSuccess", false, null) : Localization.Get("xuiReportPlayerFail", false, null));
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiReportPlayerHeader", false, null), text, "", null, false, XUiC_ReportPlayer.OpenDialogsModal, false);
	}

	// Token: 0x06008250 RID: 33360 RVA: 0x0031A508 File Offset: 0x00318708
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

	// Token: 0x06008251 RID: 33361 RVA: 0x0031A628 File Offset: 0x00318828
	public override void OnClose()
	{
		base.OnClose();
		this.reportedPlayerData = null;
	}

	// Token: 0x06008252 RID: 33362 RVA: 0x0031A637 File Offset: 0x00318837
	[XuiBindEvent("OnInputErrorHandler", "txtMessage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateErrorMessage(XUiController _sender, string _errorMessage)
	{
		this.InputErrorMessage = _errorMessage;
	}

	// Token: 0x06008253 RID: 33363 RVA: 0x0031A640 File Offset: 0x00318840
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

	// Token: 0x17000F57 RID: 3927
	// (get) Token: 0x06008254 RID: 33364 RVA: 0x002AAB45 File Offset: 0x002A8D45
	public static bool OpenDialogsModal
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.gameStateManager.IsGameStarted();
		}
	}

	// Token: 0x04006099 RID: 24729
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x0400609A RID: 24730
	[PublicizedFrom(EAccessModifier.Private)]
	public string initialText = "";

	// Token: 0x0400609B RID: 24731
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumReportCategory initialCategory;

	// Token: 0x0400609C RID: 24732
	[XuiBindComponent("cbxCategory", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPlayerReporting.PlayerReportCategory> cbxCategory;

	// Token: 0x0400609D RID: 24733
	[XuiBindComponent("txtMessage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtMessage;

	// Token: 0x0400609E RID: 24734
	[XuiBindComponent("btnKick", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnKickPlayer;

	// Token: 0x0400609F RID: 24735
	[XuiBindComponent("btnSend", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSend;

	// Token: 0x040060A0 RID: 24736
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040060A1 RID: 24737
	[PublicizedFrom(EAccessModifier.Private)]
	public string reportedPlayerName;

	// Token: 0x040060A2 RID: 24738
	[PublicizedFrom(EAccessModifier.Private)]
	public int reportedEntityId;

	// Token: 0x040060A3 RID: 24739
	[PublicizedFrom(EAccessModifier.Private)]
	public string inputErrorMessage;

	// Token: 0x040060A4 RID: 24740
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerData reportedPlayerData;
}
