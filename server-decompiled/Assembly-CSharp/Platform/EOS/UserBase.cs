using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001CFD RID: 7421
	public abstract class UserBase : IUserClient
	{
		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x0600DB62 RID: 56162 RVA: 0x004EABF4 File Offset: 0x004E8DF4
		public ConnectInterface ConnectInterface
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return ((Api)this.Owner.Api).ConnectInterface;
			}
		}

		// Token: 0x0600DB63 RID: 56163 RVA: 0x004EAC0B File Offset: 0x004E8E0B
		public virtual void Init(IPlatform _owner)
		{
			this.Owner = _owner;
			this.Owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x0600DB64 RID: 56164 RVA: 0x004EAC30 File Offset: 0x004E8E30
		// (set) Token: 0x0600DB65 RID: 56165 RVA: 0x004EAC38 File Offset: 0x004E8E38
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x0600DB66 RID: 56166 RVA: 0x004EAC44 File Offset: 0x004E8E44
		// (remove) Token: 0x0600DB67 RID: 56167 RVA: 0x004EACA4 File Offset: 0x004E8EA4
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
		// (add) Token: 0x0600DB68 RID: 56168 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600DB69 RID: 56169 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x0600DB6A RID: 56170 RVA: 0x004EACF0 File Offset: 0x004E8EF0
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.PlatformUserIdEos;
			}
		}

		// Token: 0x0600DB6B RID: 56171 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Login(LoginUserCallback _delegate)
		{
		}

		// Token: 0x0600DB6C RID: 56172 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void PlayOffline(LoginUserCallback _delegate)
		{
		}

		// Token: 0x0600DB6D RID: 56173 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600DB6E RID: 56174 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600DB6F RID: 56175 RVA: 0x000876FD File Offset: 0x000858FD
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DB70 RID: 56176 RVA: 0x000876FD File Offset: 0x000858FD
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DB71 RID: 56177 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			return false;
		}

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x0600DB72 RID: 56178
		public abstract EUserPerms Permissions { get; }

		// Token: 0x0600DB73 RID: 56179 RVA: 0x0001FF1E File Offset: 0x0001E11E
		public virtual string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600DB74 RID: 56180 RVA: 0x004EACF8 File Offset: 0x004E8EF8
		public virtual IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			yield break;
		}

		// Token: 0x0600DB75 RID: 56181 RVA: 0x004DA868 File Offset: 0x004D8A68
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DB76 RID: 56182 RVA: 0x004EAD00 File Offset: 0x004E8F00
		public virtual void Destroy()
		{
			EosHelpers.AssertMainThread("Usr.Destroy");
			this.removeNotifications();
		}

		// Token: 0x0600DB77 RID: 56183 RVA: 0x004EAD12 File Offset: 0x004E8F12
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			EosHelpers.AssertMainThread("Usr.Init");
			this.addNotifications();
		}

		// Token: 0x0600DB78 RID: 56184 RVA: 0x004EAD24 File Offset: 0x004E8F24
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

		// Token: 0x0600DB79 RID: 56185 RVA: 0x004EADA0 File Offset: 0x004E8FA0
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

		// Token: 0x0600DB7A RID: 56186 RVA: 0x004EAE14 File Offset: 0x004E9014
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnAuthExpiration(ref AuthExpirationCallbackInfo _data)
		{
			Log.Out("[EOS] Refreshing Login");
			this.startLogin(null, true);
		}

		// Token: 0x0600DB7B RID: 56187
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void startLogin(LoginUserCallback _delegate, bool _refreshing = false);

		// Token: 0x0600DB7C RID: 56188 RVA: 0x004EAE28 File Offset: 0x004E9028
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

		// Token: 0x0600DB7D RID: 56189
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void connectCreateUser(ContinuanceToken _continuanceToken, LoginUserCallback _callback);

		// Token: 0x0600DB7E RID: 56190 RVA: 0x004EAF30 File Offset: 0x004E9130
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void eosLoggedIn(ProductUserId _puid, LoginUserCallback _callback)
		{
			this.PlatformUserIdEos = new UserIdentifierEos(_puid);
			if (EosHelpers.UserAccountState != EUserAccountState.NewUser)
			{
				EosHelpers.UserAccountState = EUserAccountState.ReturningUser;
			}
		}

		// Token: 0x0600DB7F RID: 56191 RVA: 0x004EAF4C File Offset: 0x004E914C
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

		// Token: 0x0600DB80 RID: 56192 RVA: 0x004EAF7E File Offset: 0x004E917E
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserBase()
		{
		}

		// Token: 0x0400A5D7 RID: 42455
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform Owner;

		// Token: 0x0400A5D8 RID: 42456
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong notifyAuthExpirationHandle;

		// Token: 0x0400A5DA RID: 42458
		[PublicizedFrom(EAccessModifier.Protected)]
		public Action<IPlatform> userLoggedIn;

		// Token: 0x0400A5DB RID: 42459
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserIdentifierEos PlatformUserIdEos;
	}
}
