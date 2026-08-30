using System;
using System.Collections.Generic;
using System.Text;
using Platform;
using UnityEngine.Scripting;
using Webserver;

// Token: 0x020001FE RID: 510
[Preserve]
public class ConsoleCmdCreateWebUser : ConsoleCmdAbstract
{
	// Token: 0x06000FA2 RID: 4002 RVA: 0x0006560C File Offset: 0x0006380C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "createwebuser" };
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x0006561C File Offset: 0x0006381C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Create a web dashboard user account";
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x00032147 File Offset: 0x00030347
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x00061652 File Offset: 0x0005F852
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x00065624 File Offset: 0x00063824
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_senderInfo.NetworkConnection != null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be executed from the in-game console.");
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (_senderInfo.IsLocalGame)
			{
				string @string = GamePrefs.GetString(EnumGamePrefs.PlayerName);
				PlatformUserIdentifierAbs platformUserId = PlatformManager.NativePlatform.User.PlatformUserId;
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				PlatformUserIdentifierAbs platformUserIdentifierAbs;
				if (crossplatformPlatform == null)
				{
					platformUserIdentifierAbs = null;
				}
				else
				{
					IUserClient user = crossplatformPlatform.User;
					platformUserIdentifierAbs = ((user != null) ? user.PlatformUserId : null);
				}
				string text = this.createToken(@string, platformUserId, platformUserIdentifierAbs);
				string text2 = this.createRegistrationPageUrl(text, true);
				this.openUserRegistrationPage(text2);
				return;
			}
			string text3 = this.createToken(_senderInfo.RemoteClientInfo.playerName, _senderInfo.RemoteClientInfo.PlatformId, _senderInfo.RemoteClientInfo.CrossplatformId);
			string text4 = this.createRegistrationPageUrl(text3, false);
			_senderInfo.RemoteClientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageConsoleCmdClient>().Setup(this.GetCommands()[0] + " " + Convert.ToBase64String(Encoding.UTF8.GetBytes(text4)), true));
			return;
		}
		else
		{
			if (_params.Count < 1)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Missing URL in server reply");
				return;
			}
			string string2 = Encoding.UTF8.GetString(Convert.FromBase64String(_params[0]));
			this.openUserRegistrationPage(string2);
			return;
		}
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x00065750 File Offset: 0x00063950
	[PublicizedFrom(EAccessModifier.Private)]
	public void openUserRegistrationPage(string _url)
	{
		GUIWindowConsole.Close();
		XUiC_MessageBoxWindowGroup.ShowUrlConfirmationDialog(LocalPlayerUI.GetUIForPrimaryPlayer().xui, _url, true, new Func<string, bool>(Utils.OpenSystemBrowser), null, Localization.Get("xuiOpenUserCreationConfirmationText", false, null), "", null, false);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Requested browser for user creation at " + _url);
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x000657A8 File Offset: 0x000639A8
	[PublicizedFrom(EAccessModifier.Private)]
	public string createRegistrationPageUrl(string _token, bool _isLocalOnListenServer = false)
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.WebDashboardPort);
		string text;
		if (_isLocalOnListenServer)
		{
			text = string.Format("http://localhost:{0}/", @int);
		}
		else
		{
			string @string = GamePrefs.GetString(EnumGamePrefs.WebDashboardUrl);
			if (!string.IsNullOrEmpty(@string))
			{
				string text2 = @string;
				string text3 = @string;
				text = text2 + ((text3[text3.Length - 1] == '/') ? "" : "/");
			}
			else
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Server does not specify an explicit WebDashboardUrl, using game server's public IP");
				text = string.Format("http://{0}:{1}/", SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.GetValue(GameInfoString.IP), @int);
			}
		}
		return text + "app/createuser?token=" + _token;
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x0006584C File Offset: 0x00063A4C
	[PublicizedFrom(EAccessModifier.Private)]
	public string createToken(string _playerName, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossPlatformUserId)
	{
		return UserRegistrationTokens.CreateToken(_playerName, _platformUserId, _crossPlatformUserId);
	}

	// Token: 0x04000CA4 RID: 3236
	[PublicizedFrom(EAccessModifier.Private)]
	public const string registrationPagePath = "app/createuser";
}
