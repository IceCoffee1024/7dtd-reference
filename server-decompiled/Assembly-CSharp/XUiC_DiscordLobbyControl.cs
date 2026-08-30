using System;
using Discord.Sdk;
using UnityEngine.Scripting;

// Token: 0x02000348 RID: 840
[Preserve]
public class XUiC_DiscordLobbyControl : XUiController
{
	// Token: 0x06001841 RID: 6209 RVA: 0x0008A61C File Offset: 0x0008881C
	public override void Init()
	{
		base.Init();
		DiscordManager.Instance.LobbyStateChanged += this.onLobbyStateChanged;
		DiscordManager.Instance.CallStatusChanged += this.onCallStatusChanged;
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnJoinVoice") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += this.btnJoinPressed;
		}
		XUiC_SimpleButton xuiC_SimpleButton2 = base.GetChildById("btnLeaveVoice") as XUiC_SimpleButton;
		if (xuiC_SimpleButton2 != null)
		{
			xuiC_SimpleButton2.OnPressed += this.btnLeavePressed;
		}
		XUiController childById = base.GetChildById("btnJoinVoice");
		XUiV_Button xuiV_Button = ((childById != null) ? childById.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button != null)
		{
			xuiV_Button.Controller.OnPress += this.btnJoinPressed;
		}
		XUiController childById2 = base.GetChildById("btnLeaveVoice");
		XUiV_Button xuiV_Button2 = ((childById2 != null) ? childById2.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button2 != null)
		{
			xuiV_Button2.Controller.OnPress += this.btnLeavePressed;
		}
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void onLobbyStateChanged(DiscordManager.LobbyInfo _lobby, bool _isReady, bool _isJoined)
	{
		this.IsDirty = true;
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void onCallStatusChanged(DiscordManager.CallInfo _call, Call.Status _callStatus)
	{
		this.IsDirty = true;
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x0008A71E File Offset: 0x0008891E
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnJoinPressed(XUiController _sender, int _mouseButton)
	{
		DiscordManager.Instance.JoinLobbyVoice(this.lobbyType);
	}

	// Token: 0x06001845 RID: 6213 RVA: 0x0008A730 File Offset: 0x00088930
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnLeavePressed(XUiController _sender, int _mouseButton)
	{
		DiscordManager.Instance.LeaveLobbyVoice(true);
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x0008A73D File Offset: 0x0008893D
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x0008A75C File Offset: 0x0008895C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "lobby_ready")
		{
			_value = this.lobby.IsReady.ToString();
			return true;
		}
		if (_bindingName == "lobby_joined")
		{
			_value = this.lobby.IsJoined.ToString();
			return true;
		}
		if (_bindingName == "voice_status")
		{
			_value = this.lobby.VoiceCall.Status.ToString();
			return true;
		}
		if (_bindingName == "in_other_voice")
		{
			DiscordManager.LobbyInfo activeVoiceLobby = DiscordManager.Instance.ActiveVoiceLobby;
			_value = (activeVoiceLobby != null && activeVoiceLobby != this.lobby).ToString();
			return true;
		}
		if (!(_bindingName == "any_lobby_unstable_state"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = DiscordManager.Instance.AnyLobbyInUnstableVoiceState.ToString();
		return true;
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x0008A846 File Offset: 0x00088A46
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "lobby_type")
		{
			this.lobbyType = EnumUtils.Parse<DiscordManager.ELobbyType>(_value, true);
			this.lobby = DiscordManager.Instance.GetLobby(this.lobbyType);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x04000FA2 RID: 4002
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.ELobbyType lobbyType;

	// Token: 0x04000FA3 RID: 4003
	[PublicizedFrom(EAccessModifier.Private)]
	public DiscordManager.LobbyInfo lobby;
}
