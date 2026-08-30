using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Platform;

// Token: 0x0200075F RID: 1887
public class AuthorizationManager : IAuthorizationResponses
{
	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x060037EA RID: 14314 RVA: 0x0016F2A0 File Offset: 0x0016D4A0
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

	// Token: 0x060037EB RID: 14315 RVA: 0x0016F2B8 File Offset: 0x0016D4B8
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

	// Token: 0x060037EC RID: 14316 RVA: 0x0016F32C File Offset: 0x0016D52C
	public void Cleanup()
	{
		foreach (IAuthorizer authorizer in this.authorizers.Keys)
		{
			authorizer.Cleanup();
		}
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x0016F37C File Offset: 0x0016D57C
	public void ServerStart()
	{
		foreach (IAuthorizer authorizer in this.authorizers.Keys)
		{
			authorizer.ServerStart();
		}
		this.clientsInAuthorization.Clear();
	}

	// Token: 0x060037EE RID: 14318 RVA: 0x0016F3D8 File Offset: 0x0016D5D8
	public void ServerStop()
	{
		foreach (IAuthorizer authorizer in this.authorizers.Keys)
		{
			authorizer.ServerStop();
		}
		this.clientsInAuthorization.Clear();
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x0016F434 File Offset: 0x0016D634
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

	// Token: 0x060037F0 RID: 14320 RVA: 0x0016F4B8 File Offset: 0x0016D6B8
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

	// Token: 0x060037F1 RID: 14321 RVA: 0x0016F5AE File Offset: 0x0016D7AE
	public void AuthorizationDenied(IAuthorizer _authorizer, ClientInfo _clientInfo, GameUtils.KickPlayerData _kickPlayerData)
	{
		if (_authorizer != null)
		{
			Log.Out(string.Format("[Auth] {0} authorization failed: {1}", _authorizer.AuthorizerName, _clientInfo));
		}
		this.clientsInAuthorization.Remove(_clientInfo);
		GameUtils.KickPlayerForClientInfo(_clientInfo, _kickPlayerData);
	}

	// Token: 0x060037F2 RID: 14322 RVA: 0x0016F5E0 File Offset: 0x0016D7E0
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

	// Token: 0x060037F3 RID: 14323 RVA: 0x0016F62C File Offset: 0x0016D82C
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

	// Token: 0x060037F4 RID: 14324 RVA: 0x0016F810 File Offset: 0x0016DA10
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

	// Token: 0x04002DCC RID: 11724
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SortedList<IAuthorizer, int> authorizers = new SortedList<IAuthorizer, int>(new AuthorizationManager.AuthorizerComparer());

	// Token: 0x04002DCD RID: 11725
	[PublicizedFrom(EAccessModifier.Private)]
	public static AuthorizationManager instance;

	// Token: 0x04002DCE RID: 11726
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<ClientInfo> clientsInAuthorization = new HashSet<ClientInfo>();

	// Token: 0x02000760 RID: 1888
	[PublicizedFrom(EAccessModifier.Private)]
	public class AuthorizerComparer : IComparer<IAuthorizer>
	{
		// Token: 0x060037F8 RID: 14328 RVA: 0x0016F934 File Offset: 0x0016DB34
		public int Compare(IAuthorizer _x, IAuthorizer _y)
		{
			return _x.Order.CompareTo(_y.Order);
		}
	}
}
