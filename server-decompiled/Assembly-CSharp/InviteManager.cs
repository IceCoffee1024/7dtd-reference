using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02001212 RID: 4626
public class InviteManager
{
	// Token: 0x17001164 RID: 4452
	// (get) Token: 0x06009387 RID: 37767 RVA: 0x0037D46C File Offset: 0x0037B66C
	public static InviteManager Instance
	{
		get
		{
			if (InviteManager._instance == null)
			{
				InviteManager._instance = new InviteManager();
				PlatformManager.NativePlatform.User.UserLoggedIn += delegate(IPlatform _)
				{
					InviteManager._instance.listeners = PlatformManager.MultiPlatform.InviteListeners;
					if (LaunchPrefs.SessionInvite.Value.Length > 3)
					{
						string text = LaunchPrefs.SessionInvite.Value.Substring(0, 3);
						foreach (IJoinSessionGameInviteListener joinSessionGameInviteListener in InviteManager._instance.listeners)
						{
							if (text == joinSessionGameInviteListener.GetListenerIdentifier())
							{
								InviteManager._instance.pendingListener = joinSessionGameInviteListener;
								InviteManager._instance.pendingInvite = LaunchPrefs.SessionInvite.Value.Substring(3);
								break;
							}
						}
						Log.Error("[InviteManager] Invite string not formatted correctly. The identifier \"" + text + "\" did not match an existing listener");
					}
				};
			}
			return InviteManager._instance;
		}
	}

	// Token: 0x06009388 RID: 37768 RVA: 0x0037D4C0 File Offset: 0x0037B6C0
	public void Update()
	{
		if (this.listeners == null || this.listeners.Count == 0)
		{
			return;
		}
		if (!XUiC_MainMenu.openedOnce)
		{
			return;
		}
		if (this.CheckForInvites())
		{
			this.StopJoinCoroutine();
		}
		if (string.IsNullOrEmpty(this.pendingInvite) || this.pendingListener == null)
		{
			if (this.joinInviteCoroutine != null)
			{
				this.StopJoinCoroutine();
			}
			return;
		}
		if (this.joinInviteCoroutine != null)
		{
			return;
		}
		if (XUiC_WorldGenerationWindow.WorldBuilderIsGenerating)
		{
			XUiC_WorldGenerationWindow.CancelGeneration();
			return;
		}
		if (this.profileReady)
		{
			this.joinInviteCoroutine = ThreadManager.StartCoroutine(this.StartJoinIntentCoroutine());
			return;
		}
		if (this.creatingProfile)
		{
			return;
		}
		if (ProfileSDF.CurrentProfileName().Length == 0)
		{
			this.creatingProfile = true;
			XUiC_PlayerProfile.Open(LocalPlayerUI.primaryUI.xui, delegate
			{
				this.profileReady = true;
				this.creatingProfile = false;
			});
			return;
		}
		this.profileReady = true;
	}

	// Token: 0x06009389 RID: 37769 RVA: 0x0037D58C File Offset: 0x0037B78C
	public bool HasPendingInvite()
	{
		this.CheckForInvites();
		return !string.IsNullOrEmpty(this.pendingInvite);
	}

	// Token: 0x0600938A RID: 37770 RVA: 0x0037D5A3 File Offset: 0x0037B7A3
	public bool IsConnectingToInvite()
	{
		return this.connectingToSession;
	}

	// Token: 0x0600938B RID: 37771 RVA: 0x0037D5AB File Offset: 0x0037B7AB
	public IEnumerable<string> GetCommandLineArguments()
	{
		this.CheckForInvites();
		if (string.IsNullOrEmpty(this.commandLineInvite) && !string.IsNullOrEmpty(this.pendingInvite))
		{
			this.commandLineInvite = this.pendingListener.GetListenerIdentifier() + this.pendingInvite;
		}
		if (string.IsNullOrEmpty(this.commandLineInvite))
		{
			yield break;
		}
		yield return LaunchPrefs.SessionInvite.ToCommandLine(this.commandLineInvite);
		yield break;
	}

	// Token: 0x0600938C RID: 37772 RVA: 0x0037D5BC File Offset: 0x0037B7BC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CheckForInvites()
	{
		if (this.listeners == null || this.listeners.Count == 0)
		{
			return false;
		}
		foreach (IJoinSessionGameInviteListener joinSessionGameInviteListener in this.listeners)
		{
			ValueTuple<string, string> valueTuple = joinSessionGameInviteListener.TakePendingInvite();
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			if (!string.IsNullOrEmpty(item))
			{
				this.pendingListener = joinSessionGameInviteListener;
				this.pendingInvite = item;
				this.pendingPassword = item2;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600938D RID: 37773 RVA: 0x0037D654 File Offset: 0x0037B854
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator StartJoinIntentCoroutine()
	{
		InviteManager.<>c__DisplayClass17_0 CS$<>8__locals1 = new InviteManager.<>c__DisplayClass17_0();
		CS$<>8__locals1.<>4__this = this;
		while (!GameManager.Instance.IsSafeToDisconnect())
		{
			yield return null;
		}
		CS$<>8__locals1.warningAccepted = true;
		yield return CS$<>8__locals1.<StartJoinIntentCoroutine>g__RequestPlayerWarning|0();
		if (!CS$<>8__locals1.warningAccepted)
		{
			this.StopJoinCoroutine();
			yield break;
		}
		CS$<>8__locals1.hasPrivileges = true;
		yield return CS$<>8__locals1.<StartJoinIntentCoroutine>g__CheckMultiplayerPrivileges|1();
		if (!CS$<>8__locals1.hasPrivileges)
		{
			this.StopJoinCoroutine();
			yield break;
		}
		CS$<>8__locals1.gameDisconnected = true;
		yield return CS$<>8__locals1.<StartJoinIntentCoroutine>g__ShowDisconnectDialog|2();
		if (!CS$<>8__locals1.gameDisconnected)
		{
			this.StopJoinCoroutine();
			yield break;
		}
		if (PlatformApplicationManager.IsRestartRequired)
		{
			while (!GameManager.Instance.IsSafeToDisconnect())
			{
				yield return null;
			}
			this.commandLineInvite = this.pendingListener.GetListenerIdentifier() + this.pendingInvite;
			GameManager.Instance.Disconnect();
			this.StopJoinCoroutine();
			yield break;
		}
		yield return CS$<>8__locals1.<StartJoinIntentCoroutine>g__ConnectToSession|3();
		yield break;
	}

	// Token: 0x0600938E RID: 37774 RVA: 0x0037D663 File Offset: 0x0037B863
	[PublicizedFrom(EAccessModifier.Private)]
	public void StopJoinCoroutine()
	{
		if (this.joinInviteCoroutine == null)
		{
			return;
		}
		this.pendingInvite = null;
		this.pendingListener = null;
		ThreadManager.StopCoroutine(this.joinInviteCoroutine);
		this.joinInviteCoroutine = null;
		this.connectingToSession = false;
	}

	// Token: 0x0600938F RID: 37775 RVA: 0x0037D695 File Offset: 0x0037B895
	public static IEnumerator HandleSessionIdInvite(string _sessionId, string _password, Action<bool> _onFinished)
	{
		InviteManager.<>c__DisplayClass19_0 CS$<>8__locals1 = new InviteManager.<>c__DisplayClass19_0();
		if (string.IsNullOrEmpty(_sessionId))
		{
			if (_onFinished != null)
			{
				_onFinished(false);
			}
			yield break;
		}
		CS$<>8__locals1.serverInfo = null;
		yield return CS$<>8__locals1.<HandleSessionIdInvite>g__RequestSessionDetails|0(_sessionId);
		if (CS$<>8__locals1.serverInfo == null)
		{
			Log.Error("[InviteManager] Failed to find server details for session " + _sessionId + ".");
			if (_onFinished != null)
			{
				_onFinished(false);
			}
			yield break;
		}
		if (CS$<>8__locals1.serverInfo.AllowsCrossplay)
		{
			InviteManager.<>c__DisplayClass19_1 CS$<>8__locals2 = new InviteManager.<>c__DisplayClass19_1();
			CS$<>8__locals2.crossplayResult = null;
			XUiC_MultiplayerPrivilegeNotification.ResolvePrivilegesWithDialog(EUserPerms.Crossplay, delegate(bool result)
			{
				CS$<>8__locals2.crossplayResult = new bool?(result);
			}, 0f, true, delegate
			{
				CS$<>8__locals2.crossplayResult = new bool?(false);
			});
			while (CS$<>8__locals2.crossplayResult == null)
			{
				yield return null;
			}
			yield return null;
			while (XUiC_ProgressWindow.IsWindowOpen())
			{
				yield return null;
			}
			if (!CS$<>8__locals2.crossplayResult.Value)
			{
				Log.Error("[InviteManager] Could not join game. The server allows crossplay but crossplay is not allowed by the local user.");
				if (_onFinished != null)
				{
					_onFinished(false);
				}
				yield break;
			}
			CS$<>8__locals2 = null;
		}
		else if (!CS$<>8__locals1.serverInfo.PlayGroup.IsCurrent())
		{
			Log.Error(string.Format("[InviteManager] Could not join game. The server does not have crossplay enabled and is in a different play group: {0}", CS$<>8__locals1.serverInfo.PlayGroup));
			if (_onFinished != null)
			{
				_onFinished(false);
			}
			string text = Localization.Get("xuiConnectionDenied", false, null);
			string text2 = string.Format(Localization.Get("auth_unsupportedplatform", false, null), Localization.Get("platformName" + PlatformManager.NativePlatform.PlatformIdentifier.ToStringCached<EPlatformIdentifier>(), false, null));
			XUiC_MessageBoxWindowGroup.ShowOk(LocalPlayerUI.primaryUI.xui, text, text2, "", null, true, true, false);
			yield break;
		}
		if (!string.IsNullOrEmpty(_password))
		{
			ServerInfoCache.Instance.SavePassword(CS$<>8__locals1.serverInfo, _password);
		}
		Log.Out("[InviteManager] Got server details, trying to connect");
		if (_onFinished != null)
		{
			_onFinished(true);
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.Connect(CS$<>8__locals1.serverInfo);
		yield break;
	}

	// Token: 0x06009390 RID: 37776 RVA: 0x0037D6B2 File Offset: 0x0037B8B2
	public static IEnumerator HandleIpPortInvite(string _ip, int _port, string _password, Action<bool> _onFinished)
	{
		if (string.IsNullOrEmpty(_ip))
		{
			if (_onFinished != null)
			{
				_onFinished(false);
			}
			yield break;
		}
		if (_port < 1 || _port > 65530)
		{
			if (_onFinished != null)
			{
				_onFinished(false);
			}
			yield break;
		}
		GameServerInfo gameServerInfo = new GameServerInfo();
		gameServerInfo.SetValue(GameInfoString.IP, _ip);
		gameServerInfo.SetValue(GameInfoInt.Port, _port);
		PermissionsManager.IsCrossplayAllowed();
		if (!string.IsNullOrEmpty(_password))
		{
			ServerInfoCache.Instance.SavePassword(gameServerInfo, _password);
		}
		Log.Out("[InviteManager] Got server IP/port, trying to connect");
		if (_onFinished != null)
		{
			_onFinished(true);
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.Connect(gameServerInfo);
		yield break;
	}

	// Token: 0x04006E78 RID: 28280
	[PublicizedFrom(EAccessModifier.Private)]
	public static InviteManager _instance;

	// Token: 0x04006E79 RID: 28281
	[PublicizedFrom(EAccessModifier.Private)]
	public IList<IJoinSessionGameInviteListener> listeners;

	// Token: 0x04006E7A RID: 28282
	[PublicizedFrom(EAccessModifier.Private)]
	public IJoinSessionGameInviteListener pendingListener;

	// Token: 0x04006E7B RID: 28283
	[PublicizedFrom(EAccessModifier.Private)]
	public string pendingInvite;

	// Token: 0x04006E7C RID: 28284
	[PublicizedFrom(EAccessModifier.Private)]
	public string pendingPassword;

	// Token: 0x04006E7D RID: 28285
	[PublicizedFrom(EAccessModifier.Private)]
	public string commandLineInvite;

	// Token: 0x04006E7E RID: 28286
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine joinInviteCoroutine;

	// Token: 0x04006E7F RID: 28287
	[PublicizedFrom(EAccessModifier.Private)]
	public bool connectingToSession;

	// Token: 0x04006E80 RID: 28288
	[PublicizedFrom(EAccessModifier.Private)]
	public bool profileReady;

	// Token: 0x04006E81 RID: 28289
	[PublicizedFrom(EAccessModifier.Private)]
	public bool creatingProfile;
}
