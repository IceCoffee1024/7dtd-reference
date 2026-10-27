using System;
using System.Collections;
using System.Collections.Generic;
using Platform;
using UnityEngine;

// Token: 0x02001258 RID: 4696
public class InviteManager
{
	// Token: 0x170011EE RID: 4590
	// (get) Token: 0x06009570 RID: 38256 RVA: 0x0038938C File Offset: 0x0038758C
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

	// Token: 0x06009571 RID: 38257 RVA: 0x003893E0 File Offset: 0x003875E0
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

	// Token: 0x06009572 RID: 38258 RVA: 0x003894AC File Offset: 0x003876AC
	public bool HasPendingInvite()
	{
		this.CheckForInvites();
		return !string.IsNullOrEmpty(this.pendingInvite);
	}

	// Token: 0x06009573 RID: 38259 RVA: 0x003894C3 File Offset: 0x003876C3
	public bool IsConnectingToInvite()
	{
		return this.connectingToSession;
	}

	// Token: 0x06009574 RID: 38260 RVA: 0x003894CB File Offset: 0x003876CB
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

	// Token: 0x06009575 RID: 38261 RVA: 0x003894DC File Offset: 0x003876DC
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

	// Token: 0x06009576 RID: 38262 RVA: 0x00389574 File Offset: 0x00387774
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

	// Token: 0x06009577 RID: 38263 RVA: 0x00389583 File Offset: 0x00387783
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

	// Token: 0x06009578 RID: 38264 RVA: 0x003895B5 File Offset: 0x003877B5
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

	// Token: 0x06009579 RID: 38265 RVA: 0x003895D2 File Offset: 0x003877D2
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

	// Token: 0x04006FFC RID: 28668
	[PublicizedFrom(EAccessModifier.Private)]
	public static InviteManager _instance;

	// Token: 0x04006FFD RID: 28669
	[PublicizedFrom(EAccessModifier.Private)]
	public IList<IJoinSessionGameInviteListener> listeners;

	// Token: 0x04006FFE RID: 28670
	[PublicizedFrom(EAccessModifier.Private)]
	public IJoinSessionGameInviteListener pendingListener;

	// Token: 0x04006FFF RID: 28671
	[PublicizedFrom(EAccessModifier.Private)]
	public string pendingInvite;

	// Token: 0x04007000 RID: 28672
	[PublicizedFrom(EAccessModifier.Private)]
	public string pendingPassword;

	// Token: 0x04007001 RID: 28673
	[PublicizedFrom(EAccessModifier.Private)]
	public string commandLineInvite;

	// Token: 0x04007002 RID: 28674
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine joinInviteCoroutine;

	// Token: 0x04007003 RID: 28675
	[PublicizedFrom(EAccessModifier.Private)]
	public bool connectingToSession;

	// Token: 0x04007004 RID: 28676
	[PublicizedFrom(EAccessModifier.Private)]
	public bool profileReady;

	// Token: 0x04007005 RID: 28677
	[PublicizedFrom(EAccessModifier.Private)]
	public bool creatingProfile;
}
