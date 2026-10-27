using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02001034 RID: 4148
[Preserve]
public class XUiC_ReportPlayer : XUiController
{
	// Token: 0x17000F5E RID: 3934
	// (get) Token: 0x06008295 RID: 33429 RVA: 0x0031BF68 File Offset: 0x0031A168
	// (set) Token: 0x06008296 RID: 33430 RVA: 0x0031BF79 File Offset: 0x0031A179
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

	// Token: 0x17000F5F RID: 3935
	// (get) Token: 0x06008297 RID: 33431 RVA: 0x0031BF89 File Offset: 0x0031A189
	// (set) Token: 0x06008298 RID: 33432 RVA: 0x0031BF91 File Offset: 0x0031A191
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

	// Token: 0x17000F60 RID: 3936
	// (get) Token: 0x06008299 RID: 33433 RVA: 0x0031BFA1 File Offset: 0x0031A1A1
	[XuiXmlBinding("playeronserver")]
	public bool PlayerOnServer
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && this.ReportedEntityId != -1;
		}
	}

	// Token: 0x17000F61 RID: 3937
	// (get) Token: 0x0600829A RID: 33434 RVA: 0x0031BFBD File Offset: 0x0031A1BD
	// (set) Token: 0x0600829B RID: 33435 RVA: 0x0031BFCE File Offset: 0x0031A1CE
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

	// Token: 0x0600829C RID: 33436 RVA: 0x0031BFE0 File Offset: 0x0031A1E0
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

	// Token: 0x0600829D RID: 33437 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600829E RID: 33438 RVA: 0x00089949 File Offset: 0x00087B49
	[XuiBindEvent("OnPress", "btnCancel")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
	}

	// Token: 0x0600829F RID: 33439 RVA: 0x0031C064 File Offset: 0x0031A264
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

	// Token: 0x060082A0 RID: 33440 RVA: 0x0031C104 File Offset: 0x0031A304
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

	// Token: 0x060082A1 RID: 33441 RVA: 0x0031C1BC File Offset: 0x0031A3BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void reportSentMessageBox(bool _success)
	{
		string text = (_success ? Localization.Get("xuiReportPlayerSuccess", false, null) : Localization.Get("xuiReportPlayerFail", false, null));
		XUiC_MessageBoxWindowGroup.ShowOk(this.xui, Localization.Get("xuiReportPlayerHeader", false, null), text, "", null, false, XUiC_ReportPlayer.OpenDialogsModal, false);
	}

	// Token: 0x060082A2 RID: 33442 RVA: 0x0031C20C File Offset: 0x0031A40C
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

	// Token: 0x060082A3 RID: 33443 RVA: 0x0031C32C File Offset: 0x0031A52C
	public override void OnClose()
	{
		base.OnClose();
		this.reportedPlayerData = null;
	}

	// Token: 0x060082A4 RID: 33444 RVA: 0x0031C33B File Offset: 0x0031A53B
	[XuiBindEvent("OnInputErrorHandler", "txtMessage")]
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateErrorMessage(XUiController _sender, string _errorMessage)
	{
		this.InputErrorMessage = _errorMessage;
	}

	// Token: 0x060082A5 RID: 33445 RVA: 0x0031C344 File Offset: 0x0031A544
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

	// Token: 0x17000F62 RID: 3938
	// (get) Token: 0x060082A6 RID: 33446 RVA: 0x002AC06D File Offset: 0x002AA26D
	public static bool OpenDialogsModal
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.gameStateManager.IsGameStarted();
		}
	}

	// Token: 0x040060D7 RID: 24791
	[PublicizedFrom(EAccessModifier.Private)]
	public static string id;

	// Token: 0x040060D8 RID: 24792
	[PublicizedFrom(EAccessModifier.Private)]
	public string initialText = "";

	// Token: 0x040060D9 RID: 24793
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumReportCategory initialCategory;

	// Token: 0x040060DA RID: 24794
	[XuiBindComponent("cbxCategory", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_ComboBoxList<IPlayerReporting.PlayerReportCategory> cbxCategory;

	// Token: 0x040060DB RID: 24795
	[XuiBindComponent("txtMessage", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_TextInput txtMessage;

	// Token: 0x040060DC RID: 24796
	[XuiBindComponent("btnKick", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnKickPlayer;

	// Token: 0x040060DD RID: 24797
	[XuiBindComponent("btnSend", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnSend;

	// Token: 0x040060DE RID: 24798
	[XuiBindComponent("btnCancel", true)]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiC_Button btnCancel;

	// Token: 0x040060DF RID: 24799
	[PublicizedFrom(EAccessModifier.Private)]
	public string reportedPlayerName;

	// Token: 0x040060E0 RID: 24800
	[PublicizedFrom(EAccessModifier.Private)]
	public int reportedEntityId;

	// Token: 0x040060E1 RID: 24801
	[PublicizedFrom(EAccessModifier.Private)]
	public string inputErrorMessage;

	// Token: 0x040060E2 RID: 24802
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerData reportedPlayerData;
}
