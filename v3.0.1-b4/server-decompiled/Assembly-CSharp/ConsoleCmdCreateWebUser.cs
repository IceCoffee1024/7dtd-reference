using System;
using System.Collections.Generic;
using System.Text;
using Platform;
using UnityEngine.Scripting;
using Webserver;

// Token: 0x020001FC RID: 508
[Preserve]
public class ConsoleCmdCreateWebUser : ConsoleCmdAbstract
{
	// Token: 0x06000F97 RID: 3991 RVA: 0x000650C4 File Offset: 0x000632C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "createwebuser" };
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x000650D4 File Offset: 0x000632D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Create a web dashboard user account";
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x000650DC File Offset: 0x000632DC
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

	// Token: 0x06000F9C RID: 3996 RVA: 0x00065208 File Offset: 0x00063408
	[PublicizedFrom(EAccessModifier.Private)]
	public void openUserRegistrationPage(string _url)
	{
		GUIWindowConsole.Close();
		XUiC_MessageBoxWindowGroup.ShowUrlConfirmationDialog(LocalPlayerUI.GetUIForPrimaryPlayer().xui, _url, true, new Func<string, bool>(Utils.OpenSystemBrowser), null, Localization.Get("xuiOpenUserCreationConfirmationText", false, null), "", null, false);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Requested browser for user creation at " + _url);
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x00065260 File Offset: 0x00063460
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

	// Token: 0x06000F9E RID: 3998 RVA: 0x00065304 File Offset: 0x00063504
	[PublicizedFrom(EAccessModifier.Private)]
	public string createToken(string _playerName, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossPlatformUserId)
	{
		return UserRegistrationTokens.CreateToken(_playerName, _platformUserId, _crossPlatformUserId);
	}

	// Token: 0x04000C97 RID: 3223
	[PublicizedFrom(EAccessModifier.Private)]
	public const string registrationPagePath = "app/createuser";
}
