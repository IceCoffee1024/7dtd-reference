using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D57 RID: 7511
	public abstract class UserBase : IUserClient
	{
		// Token: 0x17001C34 RID: 7220
		// (get) Token: 0x0600DE7E RID: 56958 RVA: 0x004F62B0 File Offset: 0x004F44B0
		public ConnectInterface ConnectInterface
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return ((Api)this.Owner.Api).ConnectInterface;
			}
		}

		// Token: 0x0600DE7F RID: 56959 RVA: 0x004F62C7 File Offset: 0x004F44C7
		public virtual void Init(IPlatform _owner)
		{
			this.Owner = _owner;
			this.Owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x17001C35 RID: 7221
		// (get) Token: 0x0600DE80 RID: 56960 RVA: 0x004F62EC File Offset: 0x004F44EC
		// (set) Token: 0x0600DE81 RID: 56961 RVA: 0x004F62F4 File Offset: 0x004F44F4
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x0600DE82 RID: 56962 RVA: 0x004F6300 File Offset: 0x004F4500
		// (remove) Token: 0x0600DE83 RID: 56963 RVA: 0x004F6360 File Offset: 0x004F4560
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

		// Token: 0x14000142 RID: 322
		// (add) Token: 0x0600DE84 RID: 56964 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600DE85 RID: 56965 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17001C36 RID: 7222
		// (get) Token: 0x0600DE86 RID: 56966 RVA: 0x004F63AC File Offset: 0x004F45AC
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.PlatformUserIdEos;
			}
		}

		// Token: 0x0600DE87 RID: 56967 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Login(LoginUserCallback _delegate)
		{
		}

		// Token: 0x0600DE88 RID: 56968 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void PlayOffline(LoginUserCallback _delegate)
		{
		}

		// Token: 0x0600DE89 RID: 56969 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600DE8A RID: 56970 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600DE8B RID: 56971 RVA: 0x0008806C File Offset: 0x0008626C
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DE8C RID: 56972 RVA: 0x0008806C File Offset: 0x0008626C
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DE8D RID: 56973 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			return false;
		}

		// Token: 0x17001C37 RID: 7223
		// (get) Token: 0x0600DE8E RID: 56974
		public abstract EUserPerms Permissions { get; }

		// Token: 0x0600DE8F RID: 56975 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600DE90 RID: 56976 RVA: 0x004F63B4 File Offset: 0x004F45B4
		public virtual IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			yield break;
		}

		// Token: 0x0600DE91 RID: 56977 RVA: 0x004E58CC File Offset: 0x004E3ACC
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DE92 RID: 56978 RVA: 0x004F63BC File Offset: 0x004F45BC
		public virtual void Destroy()
		{
			EosHelpers.AssertMainThread("Usr.Destroy");
			this.removeNotifications();
		}

		// Token: 0x0600DE93 RID: 56979 RVA: 0x004F63CE File Offset: 0x004F45CE
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			EosHelpers.AssertMainThread("Usr.Init");
			this.addNotifications();
		}

		// Token: 0x0600DE94 RID: 56980 RVA: 0x004F63E0 File Offset: 0x004F45E0
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

		// Token: 0x0600DE95 RID: 56981 RVA: 0x004F645C File Offset: 0x004F465C
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

		// Token: 0x0600DE96 RID: 56982 RVA: 0x004F64D0 File Offset: 0x004F46D0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnAuthExpiration(ref AuthExpirationCallbackInfo _data)
		{
			Log.Out("[EOS] Refreshing Login");
			this.startLogin(null, true);
		}

		// Token: 0x0600DE97 RID: 56983
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void startLogin(LoginUserCallback _delegate, bool _refreshing = false);

		// Token: 0x0600DE98 RID: 56984 RVA: 0x004F64E4 File Offset: 0x004F46E4
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

		// Token: 0x0600DE99 RID: 56985
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void connectCreateUser(ContinuanceToken _continuanceToken, LoginUserCallback _callback);

		// Token: 0x0600DE9A RID: 56986 RVA: 0x004F65EC File Offset: 0x004F47EC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void eosLoggedIn(ProductUserId _puid, LoginUserCallback _callback)
		{
			this.PlatformUserIdEos = new UserIdentifierEos(_puid);
			if (EosHelpers.UserAccountState != EUserAccountState.NewUser)
			{
				EosHelpers.UserAccountState = EUserAccountState.ReturningUser;
			}
		}

		// Token: 0x0600DE9B RID: 56987 RVA: 0x004F6608 File Offset: 0x004F4808
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

		// Token: 0x0600DE9C RID: 56988 RVA: 0x004F663A File Offset: 0x004F483A
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserBase()
		{
		}

		// Token: 0x0400A7E3 RID: 42979
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform Owner;

		// Token: 0x0400A7E4 RID: 42980
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong notifyAuthExpirationHandle;

		// Token: 0x0400A7E6 RID: 42982
		[PublicizedFrom(EAccessModifier.Protected)]
		public Action<IPlatform> userLoggedIn;

		// Token: 0x0400A7E7 RID: 42983
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserIdentifierEos PlatformUserIdEos;
	}
}
