using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x0200076B RID: 1899
public class AuthorizationManager : IAuthorizationResponses
{
	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06003883 RID: 14467 RVA: 0x00171E20 File Offset: 0x00170020
	public static AuthorizationManager Instance
	{
		get
		{
			AuthorizationManager authorizationManager;
			if ((authorizationManager = AuthorizationManager.instance) == null)
			{
				authorizationManager = (AuthorizationManager.instance = new AuthorizationManager());
			}
			return authorizationManager;
		}
	}

	// Token: 0x06003884 RID: 14468 RVA: 0x00171E38 File Offset: 0x00170038
	public void Init()
	{
		new SortedList<string, IConsoleCommand>();
		ReflectionHelpers.FindTypesImplementingBase(typeof(IAuthorizer), delegate(Type _type)
		{
			IAuthorizer authorizer2 = ReflectionHelpers.Instantiate<IAuthorizer>(_type);
			this.authorizers.Add(authorizer2, 0);
		}, false);
		foreach (IAuthorizer authorizer in this.authorizers.Keys)
		{
			authorizer.Init(this);
		}
	}

	// Token: 0x06003885 RID: 14469 RVA: 0x00171EAC File Offset: 0x001700AC
	public void Cleanup()
	{
		foreach (IAuthorizer authorizer in this.authorizers.Keys)
		{
			authorizer.Cleanup();
		}
	}

	// Token: 0x06003886 RID: 14470 RVA: 0x00171EFC File Offset: 0x001700FC
	public void ServerStart()
	{
		foreach (IAuthorizer authorizer in this.authorizers.Keys)
		{
			authorizer.ServerStart();
		}
		this.clientsInAuthorization.Clear();
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x00171F58 File Offset: 0x00170158
	public void ServerStop()
	{
		foreach (IAuthorizer authorizer in this.authorizers.Keys)
		{
			authorizer.ServerStop();
		}
		this.clientsInAuthorization.Clear();
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x00171FB4 File Offset: 0x001701B4
	public void Authorize(ClientInfo _clientInfo, string _playerName, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _platformUserAndToken, [TupleElementNames(new string[] { "userId", "token" })] ValueTuple<PlatformUserIdentifierAbs, string> _crossplatformUserAndToken, string _compatibilityVersion, ulong _discordUserId)
	{
		this.clientsInAuthorization.Add(_clientInfo);
		PlatformUserIdentifierAbs item = _platformUserAndToken.Item1;
		if (item != null)
		{
			item.DecodeTicket(_platformUserAndToken.Item2);
		}
		PlatformUserIdentifierAbs item2 = _crossplatformUserAndToken.Item1;
		if (item2 != null)
		{
			item2.DecodeTicket(_crossplatformUserAndToken.Item2);
		}
		_clientInfo.playerName = _playerName;
		_clientInfo.compatibilityVersion = _compatibilityVersion;
		_clientInfo.PlatformId = _platformUserAndToken.Item1;
		_clientInfo.CrossplatformId = _crossplatformUserAndToken.Item1;
		_clientInfo.DiscordUserId = _discordUserId;
		this.tryAuthorizer(0, _clientInfo);
	}

	// Token: 0x06003889 RID: 14473 RVA: 0x00172038 File Offset: 0x00170238
	[PublicizedFrom(EAccessModifier.Private)]
	public void tryAuthorizer(int _currentIndex, ClientInfo _clientInfo)
	{
		while (_currentIndex < this.authorizers.Count)
		{
			IAuthorizer authorizer = this.authorizers.Keys[_currentIndex];
			bool authorizerActive = authorizer.AuthorizerActive;
			bool flag = true;
			EPlatformIdentifier platformRestriction = authorizer.PlatformRestriction;
			if (platformRestriction < EPlatformIdentifier.Count)
			{
				flag = platformRestriction == _clientInfo.PlatformId.PlatformIdentifier || (_clientInfo.CrossplatformId != null && platformRestriction == _clientInfo.CrossplatformId.PlatformIdentifier);
			}
			bool flag2 = !authorizerActive || !flag;
			_currentIndex++;
			if (!flag2)
			{
				if (authorizer.StateLocalizationKey != null)
				{
					_clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageAuthState>().Setup(authorizer.StateLocalizationKey));
				}
				ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> valueTuple = authorizer.Authorize(_clientInfo);
				EAuthorizerSyncResult item = valueTuple.Item1;
				GameUtils.KickPlayerData? item2 = valueTuple.Item2;
				switch (item)
				{
				case EAuthorizerSyncResult.WaitAsync:
					return;
				case EAuthorizerSyncResult.SyncAllow:
					this.AuthorizationAccepted(authorizer, _clientInfo);
					return;
				case EAuthorizerSyncResult.SyncDeny:
					this.AuthorizationDenied(authorizer, _clientInfo, item2.Value);
					return;
				case EAuthorizerSyncResult.SyncFinalAllow:
					this.playerAllowed(_clientInfo);
					return;
				default:
					return;
				}
			}
		}
		this.playerAllowed(_clientInfo);
	}

	// Token: 0x0600388A RID: 14474 RVA: 0x0017212E File Offset: 0x0017032E
	public void AuthorizationDenied(IAuthorizer _authorizer, ClientInfo _clientInfo, GameUtils.KickPlayerData _kickPlayerData)
	{
		if (_authorizer != null)
		{
			Log.Out(string.Format("[Auth] {0} authorization failed: {1}", _authorizer.AuthorizerName, _clientInfo));
		}
		this.clientsInAuthorization.Remove(_clientInfo);
		GameUtils.KickPlayerForClientInfo(_clientInfo, _kickPlayerData);
	}

	// Token: 0x0600388B RID: 14475 RVA: 0x00172160 File Offset: 0x00170360
	public void AuthorizationAccepted(IAuthorizer _authorizer, ClientInfo _clientInfo)
	{
		Log.Out(string.Format("[Auth] {0} authorization successful: {1}", _authorizer.AuthorizerName, _clientInfo));
		int num = this.authorizers.IndexOfKey(_authorizer);
		if (!this.clientsInAuthorization.Contains(_clientInfo))
		{
			return;
		}
		this.tryAuthorizer(num + 1, _clientInfo);
	}

	// Token: 0x0600388C RID: 14476 RVA: 0x001721AC File Offset: 0x001703AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void playerAllowed(ClientInfo _clientInfo)
	{
		this.clientsInAuthorization.Remove(_clientInfo);
		if (_clientInfo.loginDone)
		{
			return;
		}
		_clientInfo.loginDone = true;
		INetConnection[] netConnection = _clientInfo.netConnection;
		for (int i = 0; i < netConnection.Length; i++)
		{
			netConnection[i].UpgradeToFullConnection();
		}
		Log.Out("Allowing player with id " + _clientInfo.InternalId.CombinedString);
		_clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackageAuthState>().Setup("authstate_authenticated"));
		try
		{
			string text = SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.ToString();
			PlatformLobbyId platformLobbyId = PlatformLobbyId.None;
			if (PlatformManager.NativePlatform.PlatformIdentifier == _clientInfo.PlatformId.PlatformIdentifier)
			{
				ILobbyHost lobbyHost = PlatformManager.NativePlatform.LobbyHost;
				if (lobbyHost != null && lobbyHost.IsInLobby)
				{
					platformLobbyId = new PlatformLobbyId(PlatformManager.NativePlatform.PlatformIdentifier, PlatformManager.NativePlatform.LobbyHost.LobbyId);
					goto IL_00EB;
				}
			}
			PlatformLobbyId platformLobbyId2;
			if (PlatformManager.ClientLobbyManager.TryGetLobbyId(_clientInfo.PlatformId.PlatformIdentifier, out platformLobbyId2))
			{
				platformLobbyId = platformLobbyId2;
			}
			IL_00EB:
			ValueTuple<PlatformUserIdentifierAbs, string> valueTuple;
			ValueTuple<PlatformUserIdentifierAbs, string> valueTuple2;
			if (GameManager.IsDedicatedServer)
			{
				valueTuple = default(ValueTuple<PlatformUserIdentifierAbs, string>);
				valueTuple2 = default(ValueTuple<PlatformUserIdentifierAbs, string>);
			}
			else
			{
				PlatformUserIdentifierAbs platformUserId = PlatformManager.NativePlatform.User.PlatformUserId;
				IAuthenticationClient authenticationClient = PlatformManager.NativePlatform.AuthenticationClient;
				valueTuple = new ValueTuple<PlatformUserIdentifierAbs, string>(platformUserId, (authenticationClient != null) ? authenticationClient.GetAuthTicket() : null);
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
				IPlatform crossplatformPlatform2 = PlatformManager.CrossplatformPlatform;
				string text2;
				if (crossplatformPlatform2 == null)
				{
					text2 = null;
				}
				else
				{
					IAuthenticationClient authenticationClient2 = crossplatformPlatform2.AuthenticationClient;
					text2 = ((authenticationClient2 != null) ? authenticationClient2.GetAuthTicket() : null);
				}
				valueTuple2 = new ValueTuple<PlatformUserIdentifierAbs, string>(platformUserIdentifierAbs, text2);
			}
			_clientInfo.SendPackage(NetPackageManager.GetPackage<NetPackagePlayerLoginAnswer>().Setup(true, text, platformLobbyId, valueTuple, valueTuple2));
		}
		catch (Exception ex)
		{
			string text3 = "Exception in playerAllowed: ";
			Exception ex2 = ex;
			Log.Error(text3 + ((ex2 != null) ? ex2.ToString() : null));
			SingletonMonoBehaviour<ConnectionManager>.Instance.DisconnectClient(_clientInfo, false, false);
		}
	}

	// Token: 0x0600388D RID: 14477 RVA: 0x00172390 File Offset: 0x00170590
	public void Disconnect(ClientInfo _cInfo)
	{
		if (!ThreadManager.IsMainThread())
		{
			ThreadManager.AddSingleTaskMainThread("Auth.Disconnect-" + _cInfo.ClientNumber.ToString(), delegate(object _parameter)
			{
				this.Disconnect((ClientInfo)_parameter);
			}, _cInfo);
			return;
		}
		this.clientsInAuthorization.Remove(_cInfo);
		for (int i = this.authorizers.Keys.Count - 1; i >= 0; i--)
		{
			IAuthorizer authorizer = this.authorizers.Keys[i];
			bool authorizerActive = authorizer.AuthorizerActive;
			bool flag = true;
			EPlatformIdentifier platformRestriction = authorizer.PlatformRestriction;
			if (platformRestriction < EPlatformIdentifier.Count)
			{
				EPlatformIdentifier eplatformIdentifier = platformRestriction;
				PlatformUserIdentifierAbs platformId = _cInfo.PlatformId;
				bool flag2;
				if (eplatformIdentifier != ((platformId != null) ? platformId.PlatformIdentifier : EPlatformIdentifier.Count))
				{
					EPlatformIdentifier eplatformIdentifier2 = platformRestriction;
					PlatformUserIdentifierAbs crossplatformId = _cInfo.CrossplatformId;
					flag2 = eplatformIdentifier2 == ((crossplatformId != null) ? crossplatformId.PlatformIdentifier : EPlatformIdentifier.Count);
				}
				else
				{
					flag2 = true;
				}
				flag = flag2;
			}
			if (authorizerActive && flag)
			{
				authorizer.Disconnect(_cInfo);
			}
		}
	}

	// Token: 0x04002E3D RID: 11837
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SortedList<IAuthorizer, int> authorizers = new SortedList<IAuthorizer, int>(new AuthorizationManager.AuthorizerComparer());

	// Token: 0x04002E3E RID: 11838
	[PublicizedFrom(EAccessModifier.Private)]
	public static AuthorizationManager instance;

	// Token: 0x04002E3F RID: 11839
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<ClientInfo> clientsInAuthorization = new HashSet<ClientInfo>();

	// Token: 0x0200076C RID: 1900
	[PublicizedFrom(EAccessModifier.Private)]
	public class AuthorizerComparer : IComparer<IAuthorizer>
	{
		// Token: 0x06003891 RID: 14481 RVA: 0x001724B4 File Offset: 0x001706B4
		public int Compare(IAuthorizer _x, IAuthorizer _y)
		{
			return _x.Order.CompareTo(_y.Order);
		}
	}
}
