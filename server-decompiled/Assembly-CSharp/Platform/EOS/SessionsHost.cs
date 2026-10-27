using System;
using System.Collections.Generic;
using System.Text;
using Epic.OnlineServices;
using Epic.OnlineServices.Sessions;
using UnityEngine;

namespace Platform.EOS
{
	// Token: 0x02001D76 RID: 7542
	public class SessionsHost : IMasterServerAnnouncer
	{
		// Token: 0x0600DF30 RID: 57136 RVA: 0x004F7864 File Offset: 0x004F5A64
		public static string GetMatchmakingGroupTag(EMatchmakingGroup _matchmakingGroup)
		{
			if (_matchmakingGroup == EMatchmakingGroup.CertQA)
			{
				return "CertQA";
			}
			return "<WeDontCare>";
		}

		// Token: 0x0600DF31 RID: 57137 RVA: 0x004F7875 File Offset: 0x004F5A75
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x0600DF32 RID: 57138 RVA: 0x004F789C File Offset: 0x004F5A9C
		public void Update()
		{
			if (!this.GameServerInitialized)
			{
				if (this.updatesSessionModification != null)
				{
					object lockObject = AntiCheatCommon.LockObject;
					lock (lockObject)
					{
						this.updatesSessionModification.Release();
					}
					this.updatesSessionModification = null;
				}
				return;
			}
			if (this.commitBackendCountdown.HasPassed())
			{
				this.commitBackendCountdown.Reset();
				this.commitBackendCountdown.SetTimeout(30f);
				if (this.updatesSessionModification != null)
				{
					this.commitSessionToBackend(false, this.updatesSessionModification);
					this.updatesSessionModification = null;
				}
			}
		}

		// Token: 0x0600DF33 RID: 57139 RVA: 0x004F7948 File Offset: 0x004F5B48
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.sessionsInterface = ((Api)this.owner.Api).PlatformInterface.GetSessionsInterface();
			}
		}

		// Token: 0x17001C73 RID: 7283
		// (get) Token: 0x0600DF34 RID: 57140 RVA: 0x004F79A4 File Offset: 0x004F5BA4
		public bool GameServerInitialized
		{
			get
			{
				return this.sessionId != null;
			}
		}

		// Token: 0x0600DF35 RID: 57141 RVA: 0x0004E500 File Offset: 0x0004C700
		public string GetServerPorts()
		{
			return string.Empty;
		}

		// Token: 0x0600DF36 RID: 57142 RVA: 0x004F79B0 File Offset: 0x004F5BB0
		[PublicizedFrom(EAccessModifier.Private)]
		public string GetBucketId()
		{
			if (!GameManager.IsDedicatedServer)
			{
				return SessionsHost.GetMatchmakingGroupTag(PlatformManager.MultiPlatform.User.GetMatchmakingGroup());
			}
			string @string = GamePrefs.GetString(EnumGamePrefs.ServerMatchmakingGroup);
			if (!string.IsNullOrEmpty(@string))
			{
				Log.Out("[EOS] using GamePref matchmaking group: " + @string);
				return @string;
			}
			return "<WeDontCare>";
		}

		// Token: 0x0600DF37 RID: 57143 RVA: 0x004F7A04 File Offset: 0x004F5C04
		public void AdvertiseServer(Action _onServerRegistered)
		{
			Log.Out("[EOS] Registering server");
			EosHelpers.AssertMainThread("SeHo.Adv");
			IUserClient user = this.owner.User;
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			if ((platformUserIdentifierAbs = ((user != null) ? user.PlatformUserId : null)) == null)
			{
				IUserClient userServer = this.owner.UserServer;
				platformUserIdentifierAbs = ((userServer != null) ? userServer.PlatformUserId : null);
			}
			UserIdentifierEos userIdentifierEos = (UserIdentifierEos)platformUserIdentifierAbs;
			if (this.sessionsInterface == null)
			{
				if (_onServerRegistered != null)
				{
					_onServerRegistered();
				}
				return;
			}
			GameServerInfo localServerInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo;
			localServerInfo.SetValue(GameInfoString.CombinedPrimaryId, (userIdentifierEos != null) ? userIdentifierEos.CombinedString : null);
			GameServerInfo gameServerInfo = localServerInfo;
			GameInfoString gameInfoString = GameInfoString.CombinedNativeId;
			IUserClient user2 = PlatformManager.NativePlatform.User;
			string text;
			if (user2 == null)
			{
				text = null;
			}
			else
			{
				PlatformUserIdentifierAbs platformUserId = user2.PlatformUserId;
				text = ((platformUserId != null) ? platformUserId.CombinedString : null);
			}
			gameServerInfo.SetValue(gameInfoString, text);
			string bucketId = this.GetBucketId();
			CreateSessionModificationOptions createSessionModificationOptions = new CreateSessionModificationOptions
			{
				SessionName = "GameHost",
				BucketId = bucketId,
				MaxPlayers = (uint)localServerInfo.GetValue(GameInfoInt.MaxPlayers),
				LocalUserId = ((userIdentifierEos != null) ? userIdentifierEos.ProductUserId : null),
				PresenceEnabled = false,
				SanctionsEnabled = this.owner.AntiCheatServer.ServerEacEnabled(),
				AllowedPlatformIds = EPlayGroupExtensions.GetCurrentlyAllowedPlatformIds()
			};
			object obj = AntiCheatCommon.LockObject;
			SessionModification sessionModification;
			Result result;
			lock (obj)
			{
				result = this.sessionsInterface.CreateSessionModification(ref createSessionModificationOptions, out sessionModification);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] Failed creating session modification: " + result.ToStringCached<Result>());
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					if (sessionModification != null)
					{
						sessionModification.Release();
					}
				}
				if (_onServerRegistered != null)
				{
					_onServerRegistered();
				}
				return;
			}
			SessionModificationSetPermissionLevelOptions sessionModificationSetPermissionLevelOptions = default(SessionModificationSetPermissionLevelOptions);
			int value = localServerInfo.GetValue(GameInfoInt.ServerVisibility);
			OnlineSessionPermissionLevel onlineSessionPermissionLevel;
			if (value != 1)
			{
				if (value == 2)
				{
					onlineSessionPermissionLevel = OnlineSessionPermissionLevel.PublicAdvertised;
				}
				else
				{
					onlineSessionPermissionLevel = OnlineSessionPermissionLevel.JoinViaPresence;
				}
			}
			else
			{
				onlineSessionPermissionLevel = OnlineSessionPermissionLevel.JoinViaPresence;
			}
			sessionModificationSetPermissionLevelOptions.PermissionLevel = onlineSessionPermissionLevel;
			SessionModificationSetPermissionLevelOptions sessionModificationSetPermissionLevelOptions2 = sessionModificationSetPermissionLevelOptions;
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				result = sessionModification.SetPermissionLevel(ref sessionModificationSetPermissionLevelOptions2);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] Failed setting permission level: " + result.ToStringCached<Result>());
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					sessionModification.Release();
				}
				if (_onServerRegistered != null)
				{
					_onServerRegistered();
				}
				return;
			}
			SessionModificationSetJoinInProgressAllowedOptions sessionModificationSetJoinInProgressAllowedOptions = new SessionModificationSetJoinInProgressAllowedOptions
			{
				AllowJoinInProgress = true
			};
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				result = sessionModification.SetJoinInProgressAllowed(ref sessionModificationSetJoinInProgressAllowedOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] Failed setting join in progress: " + result.ToStringCached<Result>());
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					sessionModification.Release();
				}
				if (_onServerRegistered != null)
				{
					_onServerRegistered();
				}
				return;
			}
			SessionModificationSetInvitesAllowedOptions sessionModificationSetInvitesAllowedOptions = new SessionModificationSetInvitesAllowedOptions
			{
				InvitesAllowed = false
			};
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				result = sessionModification.SetInvitesAllowed(ref sessionModificationSetInvitesAllowedOptions);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] Failed setting invites allowed: " + result.ToStringCached<Result>());
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					sessionModification.Release();
				}
				if (_onServerRegistered != null)
				{
					_onServerRegistered();
				}
				return;
			}
			if (!this.setBaseAttributes(sessionModification, localServerInfo))
			{
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					sessionModification.Release();
				}
				if (_onServerRegistered != null)
				{
					_onServerRegistered();
				}
				return;
			}
			this.onServerRegistered = _onServerRegistered;
			this.commitSessionToBackend(true, sessionModification);
		}

		// Token: 0x0600DF38 RID: 57144 RVA: 0x004F7E44 File Offset: 0x004F6044
		[PublicizedFrom(EAccessModifier.Private)]
		public void sessionRegisteredCallback(ref UpdateSessionCallbackInfo _callbackData)
		{
			if (this.onServerRegistered == null)
			{
				return;
			}
			if (_callbackData.ResultCode != Result.Success)
			{
				Log.Error("[EOS] Failed registering session on backend: " + _callbackData.ResultCode.ToStringCached<Result>());
				Log.Warning(string.Format("[EOS] Attribute count: {0}", this.registeredAttributes.Count));
				Action action = this.onServerRegistered;
				if (action != null)
				{
					action();
				}
				this.onServerRegistered = null;
				return;
			}
			this.sessionId = _callbackData.SessionId;
			Log.Out(string.Format("[EOS] Server registered, session: {0}, {1} attributes", this.sessionId, this.registeredAttributes.Count));
			GameServerInfo localServerInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo;
			localServerInfo.OnChangedString += this.updateSessionString;
			localServerInfo.OnChangedInt += this.updateSessionInt;
			localServerInfo.OnChangedBool += this.updateSessionBool;
			localServerInfo.SetValue(GameInfoString.IP, this.getPublicIpFromHostedSession());
			localServerInfo.SetValue(GameInfoString.UniqueId, this.sessionId);
			Action action2 = this.onServerRegistered;
			if (action2 != null)
			{
				action2();
			}
			this.onServerRegistered = null;
		}

		// Token: 0x0600DF39 RID: 57145 RVA: 0x004F7F5C File Offset: 0x004F615C
		[PublicizedFrom(EAccessModifier.Private)]
		public string getPublicIpFromHostedSession()
		{
			CopyActiveSessionHandleOptions copyActiveSessionHandleOptions = new CopyActiveSessionHandleOptions
			{
				SessionName = "GameHost"
			};
			object obj = AntiCheatCommon.LockObject;
			ActiveSession activeSession;
			Result result;
			lock (obj)
			{
				result = this.sessionsInterface.CopyActiveSessionHandle(ref copyActiveSessionHandleOptions, out activeSession);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] Failed getting active session: " + result.ToStringCached<Result>());
				return null;
			}
			ActiveSessionCopyInfoOptions activeSessionCopyInfoOptions = default(ActiveSessionCopyInfoOptions);
			obj = AntiCheatCommon.LockObject;
			ActiveSessionInfo? activeSessionInfo;
			lock (obj)
			{
				result = activeSession.CopyInfo(ref activeSessionCopyInfoOptions, out activeSessionInfo);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] Failed getting active session info: " + result.ToStringCached<Result>());
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					activeSession.Release();
				}
				return null;
			}
			string text = activeSessionInfo.Value.SessionDetails.Value.HostAddress;
			Log.Out("[EOS] Session address: " + Utils.MaskIp(text));
			obj = AntiCheatCommon.LockObject;
			lock (obj)
			{
				activeSession.Release();
			}
			return text;
		}

		// Token: 0x0600DF3A RID: 57146 RVA: 0x004F80E4 File Offset: 0x004F62E4
		public void StopServer()
		{
			EosHelpers.AssertMainThread("SeHo.Stop");
			this.onServerRegistered = null;
			if (!this.GameServerInitialized)
			{
				return;
			}
			Log.Out("[EOS] Unregistering server");
			if (SingletonMonoBehaviour<ConnectionManager>.Instance != null && SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo != null)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedString -= this.updateSessionString;
				SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedInt -= this.updateSessionInt;
				SingletonMonoBehaviour<ConnectionManager>.Instance.LocalServerInfo.OnChangedBool -= this.updateSessionBool;
			}
			this.registeredAttributes.Clear();
			DestroySessionOptions destroySessionOptions = new DestroySessionOptions
			{
				SessionName = "GameHost"
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.sessionsInterface.DestroySession(ref destroySessionOptions, null, delegate(ref DestroySessionCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode == Result.Success)
					{
						Log.Out("[EOS] Server unregistered");
						this.sessionId = null;
						return;
					}
					Log.Error("[EOS] Failed unregistering session on backend: " + _callbackData.ResultCode.ToStringCached<Result>());
				});
			}
		}

		// Token: 0x0600DF3B RID: 57147 RVA: 0x004F81EC File Offset: 0x004F63EC
		public void RegisterUser(ClientInfo _cInfo)
		{
			EosHelpers.AssertMainThread("SeHo.Reg");
			RegisterPlayersOptions registerPlayersOptions = new RegisterPlayersOptions
			{
				SessionName = "GameHost",
				PlayersToRegister = new ProductUserId[] { ((UserIdentifierEos)_cInfo.CrossplatformId).ProductUserId }
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.sessionsInterface.RegisterPlayers(ref registerPlayersOptions, null, delegate(ref RegisterPlayersCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode != Result.Success)
					{
						Log.Error("[EOS] Failed registering player in session: " + _callbackData.ResultCode.ToStringCached<Result>());
						return;
					}
					if (_callbackData.SanctionedPlayers != null)
					{
						ProductUserId[] sanctionedPlayers = _callbackData.SanctionedPlayers;
						for (int i = 0; i < sanctionedPlayers.Length; i++)
						{
							if (sanctionedPlayers[i] == ((UserIdentifierEos)_cInfo.CrossplatformId).ProductUserId)
							{
								Log.Out("Player " + _cInfo.playerName + " has a sanction and cannot join the session, kicking player");
								GameUtils.KickPlayerForClientInfo(_cInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.CrossPlatformAuthenticationFailed, 9, default(DateTime), "Sanction"));
							}
						}
					}
				});
			}
		}

		// Token: 0x0600DF3C RID: 57148 RVA: 0x004F8298 File Offset: 0x004F6498
		public void UnregisterUser(ClientInfo _cInfo)
		{
			if (((_cInfo != null) ? _cInfo.CrossplatformId : null) == null)
			{
				return;
			}
			EosHelpers.AssertMainThread("SeHo.Free");
			UnregisterPlayersOptions unregisterPlayersOptions = new UnregisterPlayersOptions
			{
				SessionName = "GameHost",
				PlayersToUnregister = new ProductUserId[] { ((UserIdentifierEos)_cInfo.CrossplatformId).ProductUserId }
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.sessionsInterface.UnregisterPlayers(ref unregisterPlayersOptions, null, delegate(ref UnregisterPlayersCallbackInfo _callbackData)
				{
					if (_callbackData.ResultCode != Result.Success)
					{
						Log.Error("[EOS] Failed unregistering player in session: " + _callbackData.ResultCode.ToStringCached<Result>());
						return;
					}
				});
			}
		}

		// Token: 0x0600DF3D RID: 57149 RVA: 0x004F8354 File Offset: 0x004F6554
		[PublicizedFrom(EAccessModifier.Private)]
		public SessionModification getSessionModificationHandle()
		{
			UpdateSessionModificationOptions updateSessionModificationOptions = new UpdateSessionModificationOptions
			{
				SessionName = "GameHost"
			};
			object obj = AntiCheatCommon.LockObject;
			SessionModification sessionModification;
			Result result;
			lock (obj)
			{
				result = this.sessionsInterface.UpdateSessionModification(ref updateSessionModificationOptions, out sessionModification);
			}
			if (result != Result.Success)
			{
				Log.Error("[EOS] Failed getting session modification: " + result.ToStringCached<Result>());
				obj = AntiCheatCommon.LockObject;
				lock (obj)
				{
					sessionModification.Release();
				}
				sessionModification = null;
			}
			return sessionModification;
		}

		// Token: 0x0600DF3E RID: 57150 RVA: 0x004F840C File Offset: 0x004F660C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool addAttribute(SessionModification _sessionModificationHandle, string _key, string _value)
		{
			if (_value == null)
			{
				_value = "";
			}
			_value = _value + "~$#$~" + _value.ToLowerInvariant();
			return this.addAttributeInternal(_sessionModificationHandle, _key, new AttributeDataValue
			{
				AsUtf8 = _value
			}, _value);
		}

		// Token: 0x0600DF3F RID: 57151 RVA: 0x004F8458 File Offset: 0x004F6658
		[PublicizedFrom(EAccessModifier.Private)]
		public bool addAttribute(SessionModification _sessionModificationHandle, string _key, int _value)
		{
			return this.addAttributeInternal(_sessionModificationHandle, _key, new AttributeDataValue
			{
				AsInt64 = new long?((long)_value)
			}, _value.ToString());
		}

		// Token: 0x0600DF40 RID: 57152 RVA: 0x004F848C File Offset: 0x004F668C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool addAttribute(SessionModification _sessionModificationHandle, string _key, bool _value)
		{
			return this.addAttributeInternal(_sessionModificationHandle, _key, new AttributeDataValue
			{
				AsBool = new bool?(_value)
			}, _value.ToString());
		}

		// Token: 0x0600DF41 RID: 57153 RVA: 0x004F84C0 File Offset: 0x004F66C0
		[PublicizedFrom(EAccessModifier.Private)]
		public bool addBoolsAttribute(SessionModification _sessionModificationHandle, string _values)
		{
			return this.addAttributeInternal(_sessionModificationHandle, "-BoolValues-", new AttributeDataValue
			{
				AsUtf8 = _values
			}, _values);
		}

		// Token: 0x0600DF42 RID: 57154 RVA: 0x004F84F0 File Offset: 0x004F66F0
		[PublicizedFrom(EAccessModifier.Private)]
		public bool addAttributeInternal(SessionModification _sessionModificationHandle, string _key, AttributeDataValue _value, string _valueString)
		{
			SessionModificationAddAttributeOptions sessionModificationAddAttributeOptions = new SessionModificationAddAttributeOptions
			{
				AdvertisementType = SessionAttributeAdvertisementType.Advertise,
				SessionAttribute = new AttributeData?(new AttributeData
				{
					Key = _key,
					Value = _value
				})
			};
			object lockObject = AntiCheatCommon.LockObject;
			Result result;
			lock (lockObject)
			{
				result = _sessionModificationHandle.AddAttribute(ref sessionModificationAddAttributeOptions);
			}
			if (result == Result.Success)
			{
				this.registeredAttributes.Add(_key);
				return true;
			}
			Log.Error(string.Format("[EOS] Failed setting {0}th attribute '{1}' to '{2}': {3}", new object[]
			{
				this.registeredAttributes.Count + 1,
				_key,
				_valueString,
				result.ToStringCached<Result>()
			}));
			return false;
		}

		// Token: 0x0600DF43 RID: 57155 RVA: 0x004F85C4 File Offset: 0x004F67C4
		[PublicizedFrom(EAccessModifier.Private)]
		public bool setBaseAttributes(SessionModification _sessionModificationHandle, GameServerInfo _gameServerInfo)
		{
			foreach (GameInfoInt gameInfoInt in GameServerInfo.IntInfosInGameTags)
			{
				if (!this.addAttribute(_sessionModificationHandle, gameInfoInt.ToStringCached<GameInfoInt>(), _gameServerInfo.GetValue(gameInfoInt)))
				{
					return false;
				}
			}
			if (!this.addBoolsAttribute(_sessionModificationHandle, this.getBoolsString(_gameServerInfo)))
			{
				return false;
			}
			foreach (GameInfoString gameInfoString in GameServerInfo.SearchableStringInfos)
			{
				if (!this.addAttribute(_sessionModificationHandle, gameInfoString.ToStringCached<GameInfoString>(), _gameServerInfo.GetValue(gameInfoString)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600DF44 RID: 57156 RVA: 0x004F8647 File Offset: 0x004F6847
		[PublicizedFrom(EAccessModifier.Private)]
		public SessionModification getUpdateSessionModification()
		{
			if (this.updatesSessionModification == null)
			{
				this.updatesSessionModification = this.getSessionModificationHandle();
			}
			return this.updatesSessionModification;
		}

		// Token: 0x0600DF45 RID: 57157 RVA: 0x004F866C File Offset: 0x004F686C
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateSessionString(GameServerInfo _gameServerInfo, GameInfoString _gameInfoKey)
		{
			if (!this.GameServerInitialized)
			{
				return;
			}
			if (!GameServerInfo.IsSearchable(_gameInfoKey))
			{
				return;
			}
			if (!this.commitBackendCountdown.IsRunning)
			{
				this.commitBackendCountdown.ResetAndRestart();
			}
			if (_gameInfoKey.ToStringCached<GameInfoString>().EndsWith("ID", StringComparison.OrdinalIgnoreCase))
			{
				this.commitBackendCountdown.SetTimeout(5f);
			}
			this.addAttribute(this.getUpdateSessionModification(), _gameInfoKey.ToStringCached<GameInfoString>(), _gameServerInfo.GetValue(_gameInfoKey));
		}

		// Token: 0x0600DF46 RID: 57158 RVA: 0x004F86E0 File Offset: 0x004F68E0
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateSessionInt(GameServerInfo _gameServerInfo, GameInfoInt _gameInfoKey)
		{
			if (!this.GameServerInitialized)
			{
				return;
			}
			if (!GameServerInfo.IsSearchable(_gameInfoKey))
			{
				return;
			}
			if (!this.commitBackendCountdown.IsRunning)
			{
				this.commitBackendCountdown.ResetAndRestart();
			}
			this.addAttribute(this.getUpdateSessionModification(), _gameInfoKey.ToStringCached<GameInfoInt>(), _gameServerInfo.GetValue(_gameInfoKey));
		}

		// Token: 0x0600DF47 RID: 57159 RVA: 0x004F8731 File Offset: 0x004F6931
		[PublicizedFrom(EAccessModifier.Private)]
		public void updateSessionBool(GameServerInfo _gameServerInfo, GameInfoBool _gameInfoKey)
		{
			if (!this.GameServerInitialized)
			{
				return;
			}
			if (!GameServerInfo.IsSearchable(_gameInfoKey))
			{
				return;
			}
			if (!this.commitBackendCountdown.IsRunning)
			{
				this.commitBackendCountdown.ResetAndRestart();
			}
			this.addBoolsAttribute(this.getUpdateSessionModification(), this.getBoolsString(_gameServerInfo));
		}

		// Token: 0x0600DF48 RID: 57160 RVA: 0x004F8774 File Offset: 0x004F6974
		[PublicizedFrom(EAccessModifier.Private)]
		public string getBoolsString(GameServerInfo _gameServerInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(',');
			foreach (GameInfoBool gameInfoBool in GameServerInfo.BoolInfosInGameTags)
			{
				stringBuilder.Append(gameInfoBool.ToStringCached<GameInfoBool>());
				stringBuilder.Append('=');
				stringBuilder.Append(_gameServerInfo.GetValue(gameInfoBool) ? '1' : '0');
				stringBuilder.Append(',');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600DF49 RID: 57161 RVA: 0x004F87E4 File Offset: 0x004F69E4
		[PublicizedFrom(EAccessModifier.Private)]
		public void commitSessionToBackend(bool _initialRegistration, SessionModification _sessionModification)
		{
			UpdateSessionOptions updateSessionOptions = new UpdateSessionOptions
			{
				SessionModificationHandle = _sessionModification
			};
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.sessionsInterface.UpdateSession(ref updateSessionOptions, new SessionsHost.SessionModificationCallbackArgs(_sessionModification, _initialRegistration, _initialRegistration ? new OnUpdateSessionCallback(this.sessionRegisteredCallback) : new OnUpdateSessionCallback(this.sessionUpdatedCallback)), new OnUpdateSessionCallback(this.commitSessionCallbackWrapper));
			}
		}

		// Token: 0x0600DF4A RID: 57162 RVA: 0x004F886C File Offset: 0x004F6A6C
		[PublicizedFrom(EAccessModifier.Private)]
		public void commitSessionCallbackWrapper(ref UpdateSessionCallbackInfo _callbackData)
		{
			SessionsHost.SessionModificationCallbackArgs sessionModificationCallbackArgs = (SessionsHost.SessionModificationCallbackArgs)_callbackData.ClientData;
			if (_callbackData.ResultCode == Result.OperationWillRetry)
			{
				Log.Warning("[EOS] Failed updating session on backend, will retry");
				return;
			}
			sessionModificationCallbackArgs.SessionModification.Release();
			sessionModificationCallbackArgs.SessionModification = null;
			if (sessionModificationCallbackArgs.IsInitialRegistration || this.GameServerInitialized)
			{
				sessionModificationCallbackArgs.Callback(ref _callbackData);
			}
		}

		// Token: 0x0600DF4B RID: 57163 RVA: 0x004F88C8 File Offset: 0x004F6AC8
		[PublicizedFrom(EAccessModifier.Private)]
		public void sessionUpdatedCallback(ref UpdateSessionCallbackInfo _callbackData)
		{
			if (_callbackData.ResultCode != Result.Success)
			{
				Log.Error("[EOS] Failed updating session on backend: " + _callbackData.ResultCode.ToStringCached<Result>() + ". From: " + StackTraceUtility.ExtractStackTrace());
				Log.Warning(string.Format("[EOS] Attribute count: {0}", this.registeredAttributes.Count));
				return;
			}
		}

		// Token: 0x0400A888 RID: 43144
		[PublicizedFrom(EAccessModifier.Private)]
		public const float sessionUpdateIntervalSecsDefault = 30f;

		// Token: 0x0400A889 RID: 43145
		[PublicizedFrom(EAccessModifier.Private)]
		public const float sessionUpdateIntervalSecsImportant = 5f;

		// Token: 0x0400A88A RID: 43146
		[PublicizedFrom(EAccessModifier.Private)]
		public const string sessionName = "GameHost";

		// Token: 0x0400A88B RID: 43147
		public const string DefaultMatchmakingGroupTag = "<WeDontCare>";

		// Token: 0x0400A88C RID: 43148
		public const string EmptyStringAttributeValue = "##EMPTY##";

		// Token: 0x0400A88D RID: 43149
		public const string LowerCaseAttributeSeparator = "~$#$~";

		// Token: 0x0400A88E RID: 43150
		public const string BoolsAttributeName = "-BoolValues-";

		// Token: 0x0400A88F RID: 43151
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A890 RID: 43152
		[PublicizedFrom(EAccessModifier.Private)]
		public SessionsInterface sessionsInterface;

		// Token: 0x0400A891 RID: 43153
		[PublicizedFrom(EAccessModifier.Private)]
		public string sessionId;

		// Token: 0x0400A892 RID: 43154
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CountdownTimer commitBackendCountdown = new CountdownTimer(30f, false);

		// Token: 0x0400A893 RID: 43155
		[PublicizedFrom(EAccessModifier.Private)]
		public SessionModification updatesSessionModification;

		// Token: 0x0400A894 RID: 43156
		[PublicizedFrom(EAccessModifier.Private)]
		public Action onServerRegistered;

		// Token: 0x0400A895 RID: 43157
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<string> registeredAttributes = new HashSet<string>();

		// Token: 0x02001D77 RID: 7543
		[PublicizedFrom(EAccessModifier.Private)]
		public class SessionModificationCallbackArgs
		{
			// Token: 0x0600DF4E RID: 57166 RVA: 0x004F897B File Offset: 0x004F6B7B
			public SessionModificationCallbackArgs(SessionModification _sessionModification, bool _isInitialRegistration, OnUpdateSessionCallback _callback)
			{
				this.SessionModification = _sessionModification;
				this.IsInitialRegistration = _isInitialRegistration;
				this.Callback = _callback;
			}

			// Token: 0x0400A896 RID: 43158
			public SessionModification SessionModification;

			// Token: 0x0400A897 RID: 43159
			public readonly bool IsInitialRegistration;

			// Token: 0x0400A898 RID: 43160
			public readonly OnUpdateSessionCallback Callback;
		}
	}
}
