using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;

namespace Platform.EOS
{
	// Token: 0x02001D88 RID: 7560
	public abstract class UserBase : IUserClient
	{
		// Token: 0x17001C7C RID: 7292
		// (get) Token: 0x0600DF92 RID: 57234 RVA: 0x004F9E50 File Offset: 0x004F8050
		public ConnectInterface ConnectInterface
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return ((Api)this.Owner.Api).ConnectInterface;
			}
		}

		// Token: 0x0600DF93 RID: 57235 RVA: 0x004F9E67 File Offset: 0x004F8067
		public virtual void Init(IPlatform _owner)
		{
			this.Owner = _owner;
			this.Owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x17001C7D RID: 7293
		// (get) Token: 0x0600DF94 RID: 57236 RVA: 0x004F9E8C File Offset: 0x004F808C
		// (set) Token: 0x0600DF95 RID: 57237 RVA: 0x004F9E94 File Offset: 0x004F8094
		public EUserStatus UserStatus
		{
			get; [PublicizedFrom(EAccessModifier.Protected)]
			set;
		} = EUserStatus.NotAttempted;

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x0600DF96 RID: 57238 RVA: 0x004F9EA0 File Offset: 0x004F80A0
		// (remove) Token: 0x0600DF97 RID: 57239 RVA: 0x004F9F00 File Offset: 0x004F8100
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
		// (add) Token: 0x0600DF98 RID: 57240 RVA: 0x000027FC File Offset: 0x000009FC
		// (remove) Token: 0x0600DF99 RID: 57241 RVA: 0x000027FC File Offset: 0x000009FC
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x17001C7E RID: 7294
		// (get) Token: 0x0600DF9A RID: 57242 RVA: 0x004F9F4C File Offset: 0x004F814C
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
				return this.PlatformUserIdEos;
			}
		}

		// Token: 0x0600DF9B RID: 57243 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Login(LoginUserCallback _delegate)
		{
		}

		// Token: 0x0600DF9C RID: 57244 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void PlayOffline(LoginUserCallback _delegate)
		{
		}

		// Token: 0x0600DF9D RID: 57245 RVA: 0x000027FC File Offset: 0x000009FC
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
		}

		// Token: 0x0600DF9E RID: 57246 RVA: 0x000027FC File Offset: 0x000009FC
		public void StopAdvertisePlaying()
		{
		}

		// Token: 0x0600DF9F RID: 57247 RVA: 0x000882A0 File Offset: 0x000864A0
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DFA0 RID: 57248 RVA: 0x000882A0 File Offset: 0x000864A0
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DFA1 RID: 57249 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			return false;
		}

		// Token: 0x17001C7F RID: 7295
		// (get) Token: 0x0600DFA2 RID: 57250
		public abstract EUserPerms Permissions { get; }

		// Token: 0x0600DFA3 RID: 57251 RVA: 0x000200DA File Offset: 0x0001E2DA
		public virtual string GetPermissionDenyReason(EUserPerms _perms)
		{
			return null;
		}

		// Token: 0x0600DFA4 RID: 57252 RVA: 0x004F9F54 File Offset: 0x004F8154
		public virtual IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			yield break;
		}

		// Token: 0x0600DFA5 RID: 57253 RVA: 0x004E946C File Offset: 0x004E766C
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			return Enumerable.Empty<object>().GetEnumerator();
		}

		// Token: 0x0600DFA6 RID: 57254 RVA: 0x004F9F5C File Offset: 0x004F815C
		public virtual void Destroy()
		{
			EosHelpers.AssertMainThread("Usr.Destroy");
			this.removeNotifications();
		}

		// Token: 0x0600DFA7 RID: 57255 RVA: 0x004F9F6E File Offset: 0x004F816E
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			EosHelpers.AssertMainThread("Usr.Init");
			this.addNotifications();
		}

		// Token: 0x0600DFA8 RID: 57256 RVA: 0x004F9F80 File Offset: 0x004F8180
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

		// Token: 0x0600DFA9 RID: 57257 RVA: 0x004F9FFC File Offset: 0x004F81FC
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

		// Token: 0x0600DFAA RID: 57258 RVA: 0x004FA070 File Offset: 0x004F8270
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnAuthExpiration(ref AuthExpirationCallbackInfo _data)
		{
			Log.Out("[EOS] Refreshing Login");
			this.startLogin(null, true);
		}

		// Token: 0x0600DFAB RID: 57259
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void startLogin(LoginUserCallback _delegate, bool _refreshing = false);

		// Token: 0x0600DFAC RID: 57260 RVA: 0x004FA084 File Offset: 0x004F8284
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

		// Token: 0x0600DFAD RID: 57261
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void connectCreateUser(ContinuanceToken _continuanceToken, LoginUserCallback _callback);

		// Token: 0x0600DFAE RID: 57262 RVA: 0x004FA18C File Offset: 0x004F838C
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void eosLoggedIn(ProductUserId _puid, LoginUserCallback _callback)
		{
			this.PlatformUserIdEos = new UserIdentifierEos(_puid);
			if (EosHelpers.UserAccountState != EUserAccountState.NewUser)
			{
				EosHelpers.UserAccountState = EUserAccountState.ReturningUser;
			}
		}

		// Token: 0x0600DFAF RID: 57263 RVA: 0x004FA1A8 File Offset: 0x004F83A8
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

		// Token: 0x0600DFB0 RID: 57264 RVA: 0x004FA1DA File Offset: 0x004F83DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserBase()
		{
		}

		// Token: 0x0400A8CC RID: 43212
		[PublicizedFrom(EAccessModifier.Protected)]
		public IPlatform Owner;

		// Token: 0x0400A8CD RID: 43213
		[PublicizedFrom(EAccessModifier.Private)]
		public ulong notifyAuthExpirationHandle;

		// Token: 0x0400A8CF RID: 43215
		[PublicizedFrom(EAccessModifier.Protected)]
		public Action<IPlatform> userLoggedIn;

		// Token: 0x0400A8D0 RID: 43216
		[PublicizedFrom(EAccessModifier.Protected)]
		public UserIdentifierEos PlatformUserIdEos;
	}
}
