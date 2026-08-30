using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D25 RID: 7461
	public abstract class UserBase : IUserClient
	{
		// Token: 0x17001B8E RID: 7054
		// (get) Token: 0x0600DD03 RID: 56579 RVA: 0x004F1668 File Offset: 0x004EF868
		public ConnectInterface ConnectInterface
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return ((Api)this.Owner.Api).ConnectInterface;
			}
		}

		// Token: 0x0600DD04 RID: 56580 RVA: 0x004F167F File Offset: 0x004EF87F
		public virtual void Init(IPlatform _owner)
		{
			this.Owner = _owner;
			this.Owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x17001B8F RID: 7055
		// (get) Token: 0x0600DD05 RID: 56581 RVA: 0x004F16A4 File Offset: 0x004EF8A4
		// (set) Token: 0x0600DD06 RID: 56582 RVA: 0x004F16AC File Offset: 0x004EF8AC
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x14000142 RID: 322
		// (add) Token: 0x0600DD07 RID: 56583 RVA: 0x004F16B8 File Offset: 0x004EF8B8
		// (remove) Token: 0x0600DD08 RID: 56584 RVA: 0x004F1718 File Offset: 0x004EF918
		public event Action<IPlatform> UserLoggedIn
		{
			add
			{
				lock (this)
				{
					this.userLoggedIn = (Action<IPlatform>)Delegate.Combine(this.userLoggedIn, value);
					if (this.UserStatus == EUserStatus.LoggedIn)
					{
						value(this.Owner);
					}
				}
			}
			remove
			{
				lock (this)
				{
					this.userLoggedIn = (Action<IPlatform>)Delegate.Remove(this.userLoggedIn, value);
				}
			}
		}

		// Token: 0x14000143 RID: 323
		// (add) Token: 0x0600DD09 RID: 56585 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600DD0A RID: 56586 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x0600DD0B RID: 56587 RVA: 0x004F1764 File Offset: 0x004EF964
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.PlatformUserIdEos;
			}
		}

		// Token: 0x0600DD0C RID: 56588 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Login(LoginUserCallback _delegate)
		{
		}

		// Token: 0x0600DD0D RID: 56589 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void PlayOffline(LoginUserCallback _delegate)
		{
		}

		// Token: 0x0600DD0E RID: 56590 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600DD0F RID: 56591 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600DD10 RID: 56592 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DD11 RID: 56593 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DD12 RID: 56594 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			return false;
		}

		// Token: 0x17001B91 RID: 7057
		// (get) Token: 0x0600DD13 RID: 56595
		public abstract EUserPerms Permissions { get; }

		// Token: 0x0600DD14 RID: 56596 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public virtual string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600DD15 RID: 56597 RVA: 0x004F176C File Offset: 0x004EF96C
		public virtual IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			yield break;
		}

		// Token: 0x0600DD16 RID: 56598 RVA: 0x004E0D84 File Offset: 0x004DEF84
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DD17 RID: 56599 RVA: 0x004F1774 File Offset: 0x004EF974
		public virtual void Destroy()
		{
			EosHelpers.AssertMainThread("Usr.Destroy");
			this.removeNotifications();
		}

		// Token: 0x0600DD18 RID: 56600 RVA: 0x004F1786 File Offset: 0x004EF986
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			EosHelpers.AssertMainThread("Usr.Init");
			this.addNotifications();
		}

		// Token: 0x0600DD19 RID: 56601 RVA: 0x004F1798 File Offset: 0x004EF998
		[PublicizedFrom(EAccessModifier.Private)]
		public void addNotifications()
		{
			if (this.ConnectInterface == null)
			{
				return;
			}
			EosHelpers.AssertMainThread("Usr.AddNtfs");
			AddNotifyAuthExpirationOptions addNotifyAuthExpirationOptions = default(AddNotifyAuthExpirationOptions);
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.notifyAuthExpirationHandle = this.ConnectInterface.AddNotifyAuthExpiration(ref addNotifyAuthExpirationOptions, null, new OnAuthExpirationCallback(this.OnAuthExpiration));
			}
		}

		// Token: 0x0600DD1A RID: 56602 RVA: 0x004F1814 File Offset: 0x004EFA14
		[PublicizedFrom(EAccessModifier.Private)]
		public void removeNotifications()
		{
			if (this.ConnectInterface == null)
			{
				return;
			}
			EosHelpers.AssertMainThread("Usr.RemNtfs");
			if (this.notifyAuthExpirationHandle != 0UL)
			{
				object lockObject = AntiCheatCommon.LockObject;
				lock (lockObject)
				{
					this.ConnectInterface.RemoveNotifyAuthExpiration(this.notifyAuthExpirationHandle);
				}
				this.notifyAuthExpirationHandle = 0UL;
			}
		}

		// Token: 0x0600DD1B RID: 56603 RVA: 0x004F1888 File Offset: 0x004EFA88
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnAuthExpiration(ref AuthExpirationCallbackInfo _data)
		{
			Log.Out("[EOS] Refreshing Login");
			this.startLogin(null, true);
		}

		// Token: 0x0600DD1C RID: 56604
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void startLogin(LoginUserCallback _delegate, bool _refreshing = false);

		// Token: 0x0600DD1D RID: 56605 RVA: 0x004F189C File Offset: 0x004EFA9C
		[PublicizedFrom(EAccessModifier.Protected)]
		public void connectLogin(byte[] _byteTicket, string _stringTicket, ExternalCredentialType _externalType, LoginUserCallback _callback, bool _refreshing, string _displayName = null)
		{
			EosHelpers.AssertMainThread("Usr.Log");
			Utf8String utf8String = ((_byteTicket != null) ? Common.ToString(new ArraySegment<byte>(_byteTicket)) : ((_stringTicket != null) ? new Utf8String(_stringTicket) : null));
			LoginOptions loginOptions = new LoginOptions
			{
				Credentials = new Credentials?(new Credentials
				{
					Token = utf8String,
					Type = _externalType
				}),
				UserLoginInfo = null
			};
			if (_displayName != null)
			{
				loginOptions.UserLoginInfo = new UserLoginInfo?(new UserLoginInfo
				{
					DisplayName = _displayName
				});
			}
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.ConnectInterface.Login(ref loginOptions, null, delegate(ref LoginCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode == Result.Success)
					{
						if (!_refreshing)
						{
							string text = "[EOS] Login succeeded, PUID: ";
							ProductUserId localUserId = _callbackData.LocalUserId;
							Log.Out(text + ((localUserId != null) ? localUserId.ToString() : null));
							this.eosLoggedIn(_callbackData.LocalUserId, _callback);
							return;
						}
						Log.Out("[EOS] Login refreshed");
						return;
					}
					else if (Common.IsOperationComplete(_callbackData.ResultCode))
					{
						if (_callbackData.ResultCode == Result.InvalidUser)
						{
							if (!_refreshing)
							{
								this.connectCreateUser(_callbackData.ContinuanceToken, _callback);
								return;
							}
							Log.Error("[EOS] Login refresh failed, invalid user");
							return;
						}
						else
						{
							Log.Warning(string.Format("[EOS] Login {0}failed: {1}", _refreshing ? "refresh " : "", _callbackData.ResultCode));
							Result resultCode = _callbackData.ResultCode;
							if (resultCode != Result.UnrecognizedResponse)
							{
								if (resultCode != Result.ConnectExternalServiceUnavailable)
								{
									if (resultCode != Result.UnexpectedError)
									{
										this.UserStatus = EUserStatus.TemporaryError;
										LoginUserCallback callback = _callback;
										if (callback == null)
										{
											return;
										}
										callback(this.Owner, EApiStatusReason.Unknown, _callbackData.ResultCode.ToStringCached<Result>());
										return;
									}
									else
									{
										this.UserStatus = EUserStatus.OfflineMode;
										LoginUserCallback callback2 = _callback;
										if (callback2 == null)
										{
											return;
										}
										callback2(this.Owner, EApiStatusReason.Unknown, _callbackData.ResultCode.ToStringCached<Result>());
										return;
									}
								}
								else
								{
									this.UserStatus = EUserStatus.OfflineMode;
									LoginUserCallback callback3 = _callback;
									if (callback3 == null)
									{
										return;
									}
									callback3(this.Owner, EApiStatusReason.ExternalAuthUnavailable, PlatformManager.NativePlatform.PlatformDisplayName);
									return;
								}
							}
							else
							{
								this.UserStatus = EUserStatus.OfflineMode;
								LoginUserCallback callback4 = _callback;
								if (callback4 == null)
								{
									return;
								}
								callback4(this.Owner, EApiStatusReason.Unknown, _callbackData.ResultCode.ToStringCached<Result>());
								return;
							}
						}
					}
					else
					{
						Log.Error("[EOS] Login " + (_refreshing ? "refresh " : "") + "error: " + _callbackData.ResultCode.ToString());
						this.UserStatus = EUserStatus.PermanentError;
						LoginUserCallback callback5 = _callback;
						if (callback5 == null)
						{
							return;
						}
						callback5(this.Owner, EApiStatusReason.Unknown, _callbackData.ResultCode.ToStringCached<Result>());
						return;
					}
				});
			}
		}

		// Token: 0x0600DD1E RID: 56606
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void connectCreateUser(ContinuanceToken _continuanceToken, LoginUserCallback _callback);

		// Token: 0x0600DD1F RID: 56607 RVA: 0x004F19A4 File Offset: 0x004EFBA4
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void eosLoggedIn(ProductUserId _puid, LoginUserCallback _callback)
		{
			this.PlatformUserIdEos = new UserIdentifierEos(_puid);
			if (EosHelpers.UserAccountState != EUserAccountState.NewUser)
			{
				EosHelpers.UserAccountState = EUserAccountState.ReturningUser;
			}
		}

		// Token: 0x0600DD20 RID: 56608 RVA: 0x004F19C0 File Offset: 0x004EFBC0
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void eosLoginDone(LoginUserCallback _callback)
		{
			this.UserStatus = EUserStatus.LoggedIn;
			Action<IPlatform> action = this.userLoggedIn;
			if (action != null)
			{
				action(this.Owner);
			}
			if (_callback != null)
			{
				_callback(this.Owner, EApiStatusReason.Ok, null);
			}
		}

		// Token: 0x0600DD21 RID: 56609 RVA: 0x004F19F2 File Offset: 0x004EFBF2
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserBase()
		{
		}

		// Token: 0x0400A70C RID: 42764
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform Owner;

		// Token: 0x0400A70D RID: 42765
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong notifyAuthExpirationHandle;

		// Token: 0x0400A70F RID: 42767
		[PublicizedFrom(EAccessModifier.Protected)]
		public Action<IPlatform> userLoggedIn;

		// Token: 0x0400A710 RID: 42768
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserIdentifierEos PlatformUserIdEos;
	}
}
