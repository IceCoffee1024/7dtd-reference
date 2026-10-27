using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using UnityEngine;

namespace Platform.EOS
{
	// Token: 0x02001D7A RID: 7546
	public class User : UserBase
	{
		// Token: 0x17001C74 RID: 7284
		// (get) Token: 0x0600DF55 RID: 57173 RVA: 0x004F8A82 File Offset: 0x004F6C82
		// (set) Token: 0x0600DF54 RID: 57172 RVA: 0x004F8A79 File Offset: 0x004F6C79
		public PlatformUserIdentifierAbs NativePlatformUserId
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600DF56 RID: 57174 RVA: 0x004F8A8C File Offset: 0x004F6C8C
		public override void Init(IPlatform _owner)
		{
			base.Init(_owner);
			EPlatformIdentifier platformIdentifier = PlatformManager.NativePlatform.PlatformIdentifier;
			ExternalCredentialType externalCredentialType;
			switch (platformIdentifier)
			{
			case EPlatformIdentifier.Steam:
				externalCredentialType = ExternalCredentialType.SteamAppTicket;
				break;
			case EPlatformIdentifier.XBL:
				externalCredentialType = ExternalCredentialType.XblXstsToken;
				break;
			case EPlatformIdentifier.PSN:
				externalCredentialType = ExternalCredentialType.PsnIdToken;
				break;
			default:
				throw new Exception("[EOS] Can not run EOS with the " + platformIdentifier.ToStringCached<EPlatformIdentifier>() + " platform");
			}
			this.externalCredentialType = externalCredentialType;
			this.nativeApplicationStateController = PlatformManager.NativePlatform.ApplicationState;
			if (this.nativeApplicationStateController != null)
			{
				this.nativeApplicationStateController.OnApplicationStateChanged += this.OnApplicationStateChanged;
			}
		}

		// Token: 0x0600DF57 RID: 57175 RVA: 0x004F8B1E File Offset: 0x004F6D1E
		public override void Destroy()
		{
			base.Destroy();
			if (this.nativeApplicationStateController != null)
			{
				this.nativeApplicationStateController.OnApplicationStateChanged -= this.OnApplicationStateChanged;
				this.nativeApplicationStateController = null;
			}
		}

		// Token: 0x0600DF58 RID: 57176 RVA: 0x004F8B4C File Offset: 0x004F6D4C
		public override void Login(LoginUserCallback _delegate)
		{
			if (base.UserStatus == EUserStatus.LoggedIn)
			{
				Log.Out("[EOS] Login already done.");
				this.eosLoginDone(_delegate);
				return;
			}
			Log.Out("[EOS] Login");
			EosHelpers.TestEosConnection(delegate(bool _success)
			{
				if (!_success)
				{
					this.UserStatus = EUserStatus.OfflineMode;
					_delegate(this.Owner, EApiStatusReason.Other, "No connection to EOS backend");
					return;
				}
				if (PlatformManager.NativePlatform.User.UserStatus == EUserStatus.LoggedIn)
				{
					this.startLogin(_delegate, false);
					return;
				}
				this.UserStatus = EUserStatus.OfflineMode;
				_delegate(this.Owner, EApiStatusReason.Other, "User offline");
			});
		}

		// Token: 0x0600DF59 RID: 57177 RVA: 0x004F8BA8 File Offset: 0x004F6DA8
		public override void PlayOffline(LoginUserCallback _delegate)
		{
			base.UserStatus = EUserStatus.NotAttempted;
			Dictionary<PlatformUserIdentifierAbs, UserIdentifierEos> dictionary = this.loadUserMappings();
			if (dictionary == null)
			{
				_delegate(this.Owner, EApiStatusReason.NoOnlineStart, null);
				return;
			}
			PlatformUserIdentifierAbs platformUserId = PlatformManager.NativePlatform.User.PlatformUserId;
			if (platformUserId == null)
			{
				Log.Warning("[EOS] No native platform user logged in, can not proceed in offline mode");
				_delegate(this.Owner, EApiStatusReason.Other, "Not logged in to native platform");
				return;
			}
			UserIdentifierEos userIdentifierEos;
			if (!dictionary.TryGetValue(platformUserId, out userIdentifierEos))
			{
				Log.Warning("[EOS] No mapping for the logged in user: " + platformUserId.CombinedString);
				_delegate(this.Owner, EApiStatusReason.NoOnlineStart, null);
				return;
			}
			this.PlatformUserIdEos = userIdentifierEos;
			base.UserStatus = EUserStatus.OfflineMode;
			Action<IPlatform> userLoggedIn = this.userLoggedIn;
			if (userLoggedIn != null)
			{
				userLoggedIn(this.Owner);
			}
			_delegate(this.Owner, EApiStatusReason.NotLoggedOn, null);
		}

		// Token: 0x0600DF5A RID: 57178 RVA: 0x004F8C67 File Offset: 0x004F6E67
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void startLogin(LoginUserCallback _delegate, bool _refreshing = false)
		{
			this.fetchTicket(_delegate, _refreshing);
		}

		// Token: 0x0600DF5B RID: 57179 RVA: 0x004F8C74 File Offset: 0x004F6E74
		[PublicizedFrom(EAccessModifier.Private)]
		public void fetchTicket(LoginUserCallback _delegate, bool _refreshing = false)
		{
			PlatformManager.NativePlatform.User.GetLoginTicket(delegate(bool _success, byte[] _byteTicket, string _stringTicket)
			{
				if (_success)
				{
					this.connectLogin(_byteTicket, _stringTicket, this.externalCredentialType, _delegate, _refreshing, null);
					return;
				}
				Log.Error("[EOS] Failed fetching login ticket from native platform");
				this.UserStatus = EUserStatus.TemporaryError;
				LoginUserCallback @delegate = _delegate;
				if (@delegate == null)
				{
					return;
				}
				@delegate(this.Owner, EApiStatusReason.NoLoginTicket, null);
			});
		}

		// Token: 0x0600DF5C RID: 57180 RVA: 0x004F8CB8 File Offset: 0x004F6EB8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void connectCreateUser(ContinuanceToken _continuanceToken, LoginUserCallback _callback)
		{
			EosHelpers.AssertMainThread("Usr.Create");
			Log.Out("[EOS] Creating account");
			EosHelpers.UserAccountState = EUserAccountState.NewUser;
			CreateUserOptions createUserOptions = new CreateUserOptions
			{
				ContinuanceToken = _continuanceToken
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				base.ConnectInterface.CreateUser(ref createUserOptions, null, delegate(ref CreateUserCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode == Result.Success)
					{
						string text = "[EOS] CreateUser succeeded, PUID: ";
						ProductUserId localUserId = _callbackData.LocalUserId;
						Log.Out(text + ((localUserId != null) ? localUserId.ToString() : null));
						this.syncExternalAccountInfo(_callbackData.LocalUserId, _callback);
						return;
					}
					if (Common.IsOperationComplete(_callbackData.ResultCode))
					{
						Log.Warning("[EOS] CreateUser failed: " + _callbackData.ResultCode.ToString());
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
						Log.Error("[EOS] CreateUser error: " + _callbackData.ResultCode.ToString());
						this.UserStatus = EUserStatus.PermanentError;
						LoginUserCallback callback2 = _callback;
						if (callback2 == null)
						{
							return;
						}
						callback2(this.Owner, EApiStatusReason.Unknown, _callbackData.ResultCode.ToStringCached<Result>());
						return;
					}
				});
			}
		}

		// Token: 0x0600DF5D RID: 57181 RVA: 0x004F8D50 File Offset: 0x004F6F50
		[PublicizedFrom(EAccessModifier.Private)]
		public void syncExternalAccountInfo(ProductUserId _puid, LoginUserCallback _callback)
		{
			if (PlatformManager.NativePlatform.PlatformIdentifier == EPlatformIdentifier.XBL)
			{
				Log.Out("[EOS] EnsureAccountInfo required for this platform, starting additional login");
				OnLoginCallback <>9__1;
				PlatformManager.NativePlatform.User.GetLoginTicket(delegate(bool _, byte[] _byteTicket, string _stringTicket)
				{
					Utf8String utf8String = ((_byteTicket != null) ? Common.ToString(new ArraySegment<byte>(_byteTicket)) : new Utf8String(_stringTicket));
					LoginOptions loginOptions = new LoginOptions
					{
						Credentials = new Credentials?(new Credentials
						{
							Token = utf8String,
							Type = this.externalCredentialType
						}),
						UserLoginInfo = null
					};
					object lockObject = AntiCheatCommon.LockObject;
					lock (lockObject)
					{
						ConnectInterface connectInterface = this.ConnectInterface;
						object obj = null;
						OnLoginCallback onLoginCallback;
						if ((onLoginCallback = <>9__1) == null)
						{
							onLoginCallback = (<>9__1 = delegate(ref LoginCallbackInfo _callbackData)
							{
								if (_callbackData.ResultCode == Result.Success)
								{
									string text = "[EOS] ensure account info succeeded, PUID: ";
									ProductUserId localUserId = _callbackData.LocalUserId;
									Log.Out(text + ((localUserId != null) ? localUserId.ToString() : null));
									this.eosLoggedIn(_callbackData.LocalUserId, _callback);
									return;
								}
								if (Common.IsOperationComplete(_callbackData.ResultCode))
								{
									Log.Warning("[EOS] ensure account info failed: " + _callbackData.ResultCode.ToString());
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
									Log.Error("[EOS] ensure account info error: " + _callbackData.ResultCode.ToString());
									this.UserStatus = EUserStatus.PermanentError;
									LoginUserCallback callback2 = _callback;
									if (callback2 == null)
									{
										return;
									}
									callback2(this.Owner, EApiStatusReason.Unknown, _callbackData.ResultCode.ToStringCached<Result>());
									return;
								}
							});
						}
						connectInterface.Login(ref loginOptions, obj, onLoginCallback);
					}
				});
				return;
			}
			this.eosLoggedIn(_puid, _callback);
		}

		// Token: 0x0600DF5E RID: 57182 RVA: 0x004F8DB1 File Offset: 0x004F6FB1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void eosLoggedIn(ProductUserId _puid, LoginUserCallback _callback)
		{
			base.eosLoggedIn(_puid, _callback);
			this.getNativePlatformUserIdentifier(_callback);
		}

		// Token: 0x0600DF5F RID: 57183 RVA: 0x004F8DC4 File Offset: 0x004F6FC4
		[PublicizedFrom(EAccessModifier.Private)]
		public void getNativePlatformUserIdentifier(LoginUserCallback _callback)
		{
			Log.Out("[EOS] Getting native user for " + this.PlatformUserIdEos.ReadablePlatformUserIdentifier);
			IdToken idToken = new IdToken
			{
				JsonWebToken = this.Owner.AuthenticationClient.GetAuthTicket(),
				ProductUserId = this.PlatformUserIdEos.ProductUserId
			};
			VerifyIdTokenOptions verifyIdTokenOptions = new VerifyIdTokenOptions
			{
				IdToken = new IdToken?(idToken)
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				base.ConnectInterface.VerifyIdToken(ref verifyIdTokenOptions, null, delegate(ref VerifyIdTokenCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode != Result.Success)
					{
						Log.Error("[EOS] VerifyIdToken failed: " + _callbackData.ResultCode.ToStringCached<Result>());
						this.UserStatus = EUserStatus.TemporaryError;
						_callback(this.Owner, EApiStatusReason.Unknown, _callbackData.ResultCode.ToStringCached<Result>());
						return;
					}
					if (!_callbackData.IsAccountInfoPresent)
					{
						Log.Error("[EOS] VerifyIdToken failed: No account info");
						this.UserStatus = EUserStatus.TemporaryError;
						_callback(this.Owner, EApiStatusReason.Unknown, "NoAccountInfo");
						return;
					}
					string text = _callbackData.AccountId;
					ExternalAccountType accountIdType = _callbackData.AccountIdType;
					ProductUserId productUserId = _callbackData.ProductUserId;
					EPlatformIdentifier eplatformIdentifier;
					if (!EosHelpers.AccountTypeMappings.TryGetValue(accountIdType, out eplatformIdentifier))
					{
						Log.Error("[EOS] VerifyIdToken failed: Unsupported account type: " + accountIdType.ToString());
						this.UserStatus = EUserStatus.TemporaryError;
						_callback(this.Owner, EApiStatusReason.Unknown, "UnsupportedAccountType");
						return;
					}
					string text2 = productUserId.ToString();
					if (text2 != this.PlatformUserIdEos.ProductUserIdString)
					{
						Log.Error("[EOS] VerifyIdToken failed: PUID mismatch: " + text2);
						this.UserStatus = EUserStatus.TemporaryError;
						_callback(this.Owner, EApiStatusReason.Unknown, "PUID mismatch");
						return;
					}
					PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromPlatformAndId(eplatformIdentifier.ToStringCached<EPlatformIdentifier>(), text, true);
					if (platformUserIdentifierAbs == null)
					{
						Log.Error("[EOS] VerifyIdToken failed: Could not create user identifier from platform/accountid: " + eplatformIdentifier.ToStringCached<EPlatformIdentifier>() + "/" + text);
						this.UserStatus = EUserStatus.TemporaryError;
						_callback(this.Owner, EApiStatusReason.Unknown, "NoUserId");
						return;
					}
					this.NativePlatformUserId = platformUserIdentifierAbs;
					this.eosLoginDone(_callback);
				});
			}
		}

		// Token: 0x0600DF60 RID: 57184 RVA: 0x004F8E9C File Offset: 0x004F709C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void eosLoginDone(LoginUserCallback _callback)
		{
			base.eosLoginDone(_callback);
			this.saveUserMapping();
		}

		// Token: 0x17001C75 RID: 7285
		// (get) Token: 0x0600DF61 RID: 57185 RVA: 0x004F8EAB File Offset: 0x004F70AB
		public override EUserPerms Permissions
		{
			get
			{
				if (this.playerHasSanctions)
				{
					return EUserPerms.Multiplayer | EUserPerms.Communication | EUserPerms.Crossplay;
				}
				return EUserPerms.All;
			}
		}

		// Token: 0x0600DF62 RID: 57186 RVA: 0x004F8EBC File Offset: 0x004F70BC
		public override string GetPermissionDenyReason(EUserPerms _perms)
		{
			EUserPerms euserPerms = ~this.Permissions & _perms;
			if (euserPerms.HasFlag(EUserPerms.HostMultiplayer))
			{
				return this.reasonForPermissions;
			}
			return null;
		}

		// Token: 0x0600DF63 RID: 57187 RVA: 0x004F8EEE File Offset: 0x004F70EE
		public override IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			Log.Out(string.Format("[EOS] {0}({1}: [{2}], {3}: {4})", new object[] { "ResolvePermissions", "_perms", _perms, "_canPrompt", _canPrompt }));
			if (base.UserStatus == EUserStatus.LoggedIn)
			{
				if (((Api)this.Owner.Api).SanctionsInterface == null || ((Api)this.Owner.Api).eosSanctionsCheck == null)
				{
					Log.Out(string.Format("[EOS] ResolvePermissions not possible: eosSanctionsCheck: {0}, SanctionsInterface: {1}", ((Api)this.Owner.Api).eosSanctionsCheck != null, ((Api)this.Owner.Api).SanctionsInterface != null));
					this.playerHasSanctions = true;
					yield break;
				}
				if (_perms.HasHostMultiplayer())
				{
					User.<>c__DisplayClass23_0 CS$<>8__locals1 = new User.<>c__DisplayClass23_0();
					CS$<>8__locals1.connectionTestComplete = false;
					CS$<>8__locals1.connectionTestSuccess = false;
					EosHelpers.TestEosConnection(delegate(bool _isConnected)
					{
						CS$<>8__locals1.connectionTestComplete = true;
						CS$<>8__locals1.connectionTestSuccess = _isConnected;
					});
					while (!CS$<>8__locals1.connectionTestComplete)
					{
						yield return null;
						if (_cancellationToken != null && _cancellationToken.IsCancelled())
						{
							yield break;
						}
					}
					if (!CS$<>8__locals1.connectionTestSuccess)
					{
						Log.Out("[EOS] Could not check sanctions as the connection test failed");
						this.playerHasSanctions = true;
						this.reasonForPermissions = Localization.Get("permissionsSanction_error", false, null);
						yield break;
					}
					yield return ((Api)this.Owner.Api).eosSanctionsCheck.CheckSanctionsEnumerator((this.Owner.Api as Api).SanctionsInterface, this.PlatformUserIdEos.ProductUserId, this.PlatformUserIdEos.ProductUserId, delegate(SanctionsCheckResult _checkResult)
					{
						if (!_checkResult.Success)
						{
							this.playerHasSanctions = true;
							this.reasonForPermissions = Localization.Get("permissionsSanction_error", false, null);
							return;
						}
						Log.Out(string.Format("[EOS] CheckSanctionsEnumerator: hasSanctions {0}", _checkResult.HasActiveSanctions));
						if (_checkResult.HasActiveSanctions)
						{
							this.playerHasSanctions = true;
							this.reasonForPermissions = _checkResult.ReasonForSanction;
							return;
						}
						this.playerHasSanctions = false;
						this.reasonForPermissions = null;
					}, _cancellationToken);
					CS$<>8__locals1 = null;
				}
			}
			yield break;
		}

		// Token: 0x0600DF64 RID: 57188 RVA: 0x004F8F14 File Offset: 0x004F7114
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnApplicationStateChanged(ApplicationState _newState)
		{
			bool flag = _newState == ApplicationState.Suspended;
			if (this.wasSuspended == flag)
			{
				return;
			}
			this.wasSuspended = flag;
			if (flag)
			{
				this.OnSuspend();
				return;
			}
			this.resumeCount++;
			this.OnResume();
		}

		// Token: 0x0600DF65 RID: 57189 RVA: 0x004F8F55 File Offset: 0x004F7155
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnSuspend()
		{
			this.shouldRefreshLoginOnResume = this.shouldRefreshLoginOnResume || base.UserStatus == EUserStatus.LoggedIn;
			Log.Out(string.Format("[EOS] User.OnSuspend() shouldRefreshLoginOnResume: {0}", this.shouldRefreshLoginOnResume));
		}

		// Token: 0x0600DF66 RID: 57190 RVA: 0x004F8F8B File Offset: 0x004F718B
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnResume()
		{
			Log.Out(string.Format("[EOS] User.OnResume() shouldRefreshLoginOnResume: {0}", this.shouldRefreshLoginOnResume));
			if (this.shouldRefreshLoginOnResume)
			{
				ThreadManager.StartCoroutine(this.OnResumeRefreshLoginCoroutine());
			}
		}

		// Token: 0x0600DF67 RID: 57191 RVA: 0x004F8FBB File Offset: 0x004F71BB
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator OnResumeRefreshLoginCoroutine()
		{
			User.<>c__DisplayClass30_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.initialResumeCount = this.resumeCount;
			yield return this.refreshLoginCoroutine();
			if (this.<OnResumeRefreshLoginCoroutine>g__ShouldExitCoroutine|30_0(true, ref CS$<>8__locals1))
			{
				yield break;
			}
			if (this.nativeApplicationStateController != null)
			{
				Log.Out("[EOS] Waiting for network to be ready...");
				for (;;)
				{
					yield return new WaitForSecondsRealtime(0.25f);
					if (this.<OnResumeRefreshLoginCoroutine>g__ShouldExitCoroutine|30_0(false, ref CS$<>8__locals1))
					{
						break;
					}
					if (this.nativeApplicationStateController.NetworkConnectionState)
					{
						goto Block_4;
					}
				}
				yield break;
				Block_4:
				Log.Out("[EOS] Network is ready. Trying to refresh login...");
				yield return this.refreshLoginCoroutine();
				if (this.<OnResumeRefreshLoginCoroutine>g__ShouldExitCoroutine|30_0(true, ref CS$<>8__locals1))
				{
					yield break;
				}
			}
			User.<>c__DisplayClass30_1 CS$<>8__locals2 = new User.<>c__DisplayClass30_1();
			Log.Out("[EOS] Waiting for EOS to be reachable...");
			CS$<>8__locals2.eosReachable = false;
			bool eosReachableFirstCheck = true;
			float waitTime = 2f;
			while (!CS$<>8__locals2.eosReachable)
			{
				User.<>c__DisplayClass30_2 CS$<>8__locals3 = new User.<>c__DisplayClass30_2();
				CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals2;
				if (eosReachableFirstCheck)
				{
					eosReachableFirstCheck = false;
				}
				else
				{
					Log.Out(string.Format("[EOS] No connection to EOS. Will retry in {0} s", waitTime));
					yield return new WaitForSecondsRealtime(waitTime);
					waitTime = Math.Min(waitTime * 2f, 60f);
				}
				if (this.<OnResumeRefreshLoginCoroutine>g__ShouldExitCoroutine|30_0(false, ref CS$<>8__locals1))
				{
					yield break;
				}
				Log.Out("[EOS] Testing connecting to EOS...");
				CS$<>8__locals3.eosTestComplete = false;
				EosHelpers.TestEosConnection(delegate(bool _success)
				{
					CS$<>8__locals3.CS$<>8__locals1.eosReachable = _success;
					CS$<>8__locals3.eosTestComplete = true;
				});
				while (!CS$<>8__locals3.eosTestComplete)
				{
					yield return new WaitForSecondsRealtime(0.25f);
					if (this.<OnResumeRefreshLoginCoroutine>g__ShouldExitCoroutine|30_0(false, ref CS$<>8__locals1))
					{
						yield break;
					}
				}
				CS$<>8__locals3 = null;
			}
			Log.Out("[EOS] EOS is reachable so we can try refresh the login now.");
			CS$<>8__locals2 = null;
			yield return this.refreshLoginCoroutine();
			if (base.UserStatus == EUserStatus.LoggedIn)
			{
				Log.Warning("[EOS] Refresh login on resume has failed. User will have to trigger a login through other means.");
			}
			yield break;
		}

		// Token: 0x0600DF68 RID: 57192 RVA: 0x004F8FCA File Offset: 0x004F71CA
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator refreshLoginCoroutine()
		{
			bool done = false;
			Log.Out("[EOS] Refreshing Login");
			this.fetchTicket(delegate(IPlatform _, EApiStatusReason _, string _)
			{
				done = true;
			}, true);
			while (!done)
			{
				yield return new WaitForSecondsRealtime(0.25f);
			}
			yield break;
		}

		// Token: 0x0600DF69 RID: 57193 RVA: 0x004F8FDC File Offset: 0x004F71DC
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<PlatformUserIdentifierAbs, UserIdentifierEos> loadUserMappings()
		{
			if (!SdPlayerPrefs.HasKey("EosMappings"))
			{
				Log.Warning("[EOS] No platform -> EOS mappings found");
				return null;
			}
			Dictionary<PlatformUserIdentifierAbs, UserIdentifierEos> dictionary = new Dictionary<PlatformUserIdentifierAbs, UserIdentifierEos>();
			string[] array = SdPlayerPrefs.GetString("EosMappings").Split(';', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length != 0)
				{
					string[] array2 = array[i].Split('=', StringSplitOptions.None);
					if (array2.Length != 2)
					{
						Log.Warning("[EOS] Malformed user mapping entry: '" + array[i] + "'");
					}
					else
					{
						PlatformUserIdentifierAbs platformUserIdentifierAbs = PlatformUserIdentifierAbs.FromCombinedString(array2[0], true);
						if (platformUserIdentifierAbs == null)
						{
							Log.Warning("[EOS] Malformed user identifier entry: '" + array2[0] + "'");
						}
						else
						{
							PlatformUserIdentifierAbs platformUserIdentifierAbs2 = PlatformUserIdentifierAbs.FromCombinedString(array2[1], true);
							if (platformUserIdentifierAbs2 == null)
							{
								Log.Warning("[EOS] Malformed user identifier EOS mapping entry: '" + array2[1] + "'");
							}
							else if (platformUserIdentifierAbs2.PlatformIdentifier != EPlatformIdentifier.EOS)
							{
								Log.Warning("[EOS] Stored user identifier EOS mapping not an EOS identifier: '" + array2[1] + "'");
							}
							else
							{
								if (dictionary.ContainsKey(platformUserIdentifierAbs))
								{
									Log.Warning("[EOS] User identifier found multiple times: " + array2[0]);
								}
								dictionary[platformUserIdentifierAbs] = (UserIdentifierEos)platformUserIdentifierAbs2;
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600DF6A RID: 57194 RVA: 0x004F9108 File Offset: 0x004F7308
		[PublicizedFrom(EAccessModifier.Private)]
		public void saveUserMapping()
		{
			Dictionary<PlatformUserIdentifierAbs, UserIdentifierEos> dictionary = this.loadUserMappings() ?? new Dictionary<PlatformUserIdentifierAbs, UserIdentifierEos>();
			dictionary[PlatformManager.NativePlatform.User.PlatformUserId] = this.PlatformUserIdEos;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<PlatformUserIdentifierAbs, UserIdentifierEos> keyValuePair in dictionary)
			{
				stringBuilder.Append(keyValuePair.Key.CombinedString + "=" + keyValuePair.Value.CombinedString + ";");
			}
			SdPlayerPrefs.SetString("EosMappings", stringBuilder.ToString());
			SdPlayerPrefs.Save();
		}

		// Token: 0x0600DF6D RID: 57197 RVA: 0x004F9244 File Offset: 0x004F7444
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Private)]
		public bool <OnResumeRefreshLoginCoroutine>g__ShouldExitCoroutine|30_0(bool _attemptedLogin, ref User.<>c__DisplayClass30_0 A_2)
		{
			if (A_2.initialResumeCount != this.resumeCount)
			{
				Log.Out("[EOS] Another resume is in progress. Exiting.");
				return true;
			}
			if (!this.shouldRefreshLoginOnResume)
			{
				Log.Out("[EOS] Refresh login on resume is no longer needed. Exiting.");
				return true;
			}
			if (base.UserStatus == EUserStatus.LoggedIn)
			{
				this.shouldRefreshLoginOnResume = false;
				if (!_attemptedLogin)
				{
					Log.Out("[EOS] User logged in through other means. Exiting.");
				}
				else
				{
					Log.Out("[EOS] User successfully logged in on resume.");
				}
				return true;
			}
			return false;
		}

		// Token: 0x0400A89C RID: 43164
		[PublicizedFrom(EAccessModifier.Private)]
		public const string EosMappingsPrefName = "EosMappings";

		// Token: 0x0400A89D RID: 43165
		[PublicizedFrom(EAccessModifier.Private)]
		public IApplicationStateController nativeApplicationStateController;

		// Token: 0x0400A89E RID: 43166
		[PublicizedFrom(EAccessModifier.Private)]
		public ExternalCredentialType externalCredentialType;

		// Token: 0x0400A8A0 RID: 43168
		[PublicizedFrom(EAccessModifier.Private)]
		public bool playerHasSanctions;

		// Token: 0x0400A8A1 RID: 43169
		[PublicizedFrom(EAccessModifier.Private)]
		public string reasonForPermissions;

		// Token: 0x0400A8A2 RID: 43170
		[PublicizedFrom(EAccessModifier.Private)]
		public bool wasSuspended;

		// Token: 0x0400A8A3 RID: 43171
		[PublicizedFrom(EAccessModifier.Private)]
		public int resumeCount;

		// Token: 0x0400A8A4 RID: 43172
		[PublicizedFrom(EAccessModifier.Private)]
		public bool shouldRefreshLoginOnResume;
	}
}
