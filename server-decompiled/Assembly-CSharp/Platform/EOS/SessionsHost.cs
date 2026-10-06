using System;
using System.Collections.Generic;
using System.Text;
using Epic.OnlineServices;
using Epic.OnlineServices.Sessions;
using UnityEngine;

namespace Platform.EOS
{
	// Token: 0x02001D45 RID: 7493
	public class SessionsHost : IMasterServerAnnouncer
	{
		// Token: 0x0600DE1C RID: 56860 RVA: 0x004F3CC4 File Offset: 0x004F1EC4
		public static string GetMatchmakingGroupTag(EMatchmakingGroup _matchmakingGroup)
		{
			if (_matchmakingGroup == EMatchmakingGroup.CertQA)
			{
				return "CertQA";
			}
			return "<WeDontCare>";
		}

		// Token: 0x0600DE1D RID: 56861 RVA: 0x004F3CD5 File Offset: 0x004F1ED5
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += this.apiInitialized;
		}

		// Token: 0x0600DE1E RID: 56862 RVA: 0x004F3CFC File Offset: 0x004F1EFC
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

		// Token: 0x0600DE1F RID: 56863 RVA: 0x004F3DA8 File Offset: 0x004F1FA8
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiInitialized()
		{
			object lockObject = AntiCheatCommon.LockObject;
			lock (lockObject)
			{
				this.sessionsInterface = ((Api)this.owner.Api).PlatformInterface.GetSessionsInterface();
			}
		}

		// Token: 0x17001C2B RID: 7211
		// (get) Token: 0x0600DE20 RID: 56864 RVA: 0x004F3E04 File Offset: 0x004F2004
		public bool GameServerInitialized
		{
			get
			{
				return this.sessionId != null;
			}
		}

		// Token: 0x0600DE21 RID: 56865 RVA: 0x0004E540 File Offset: 0x0004C740
		public string GetServerPorts()
		{
			return string.Empty;
		}

		// Token: 0x0600DE22 RID: 56866 RVA: 0x004F3E10 File Offset: 0x004F2010
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

		// Token: 0x0600DE23 RID: 56867 RVA: 0x004F3E64 File Offset: 0x004F2064
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

		// Token: 0x0600DE24 RID: 56868 RVA: 0x004F42A4 File Offset: 0x004F24A4
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

		// Token: 0x0600DE25 RID: 56869 RVA: 0x004F43BC File Offset: 0x004F25BC
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

		// Token: 0x0600DE26 RID: 56870 RVA: 0x004F4544 File Offset: 0x004F2744
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

		// Token: 0x0600DE27 RID: 56871 RVA: 0x004F464C File Offset: 0x004F284C
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

		// Token: 0x0600DE28 RID: 56872 RVA: 0x004F46F8 File Offset: 0x004F28F8
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

		// Token: 0x0600DE29 RID: 56873 RVA: 0x004F47B4 File Offset: 0x004F29B4
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

		// Token: 0x0600DE2A RID: 56874 RVA: 0x004F486C File Offset: 0x004F2A6C
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

		// Token: 0x0600DE2B RID: 56875 RVA: 0x004F48B8 File Offset: 0x004F2AB8
		[PublicizedFrom(EAccessModifier.Private)]
		public bool addAttribute(SessionModification _sessionModificationHandle, string _key, int _value)
		{
			return this.addAttributeInternal(_sessionModificationHandle, _key, new AttributeDataValue
			{
				AsInt64 = new long?((long)_value)
			}, _value.ToString());
		}

		// Token: 0x0600DE2C RID: 56876 RVA: 0x004F48EC File Offset: 0x004F2AEC
		[PublicizedFrom(EAccessModifier.Private)]
		public bool addAttribute(SessionModification _sessionModificationHandle, string _key, bool _value)
		{
			return this.addAttributeInternal(_sessionModificationHandle, _key, new AttributeDataValue
			{
				AsBool = new bool?(_value)
			}, _value.ToString());
		}

		// Token: 0x0600DE2D RID: 56877 RVA: 0x004F4920 File Offset: 0x004F2B20
		[PublicizedFrom(EAccessModifier.Private)]
		public bool addBoolsAttribute(SessionModification _sessionModificationHandle, string _values)
		{
			return this.addAttributeInternal(_sessionModificationHandle, "-BoolValues-", new AttributeDataValue
			{
				AsUtf8 = _values
			}, _values);
		}

		// Token: 0x0600DE2E RID: 56878 RVA: 0x004F4950 File Offset: 0x004F2B50
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

		// Token: 0x0600DE2F RID: 56879 RVA: 0x004F4A24 File Offset: 0x004F2C24
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

		// Token: 0x0600DE30 RID: 56880 RVA: 0x004F4AA7 File Offset: 0x004F2CA7
		[PublicizedFrom(EAccessModifier.Private)]
		public SessionModification getUpdateSessionModification()
		{
			if (this.updatesSessionModification == null)
			{
				this.updatesSessionModification = this.getSessionModificationHandle();
			}
			return this.updatesSessionModification;
		}

		// Token: 0x0600DE31 RID: 56881 RVA: 0x004F4ACC File Offset: 0x004F2CCC
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

		// Token: 0x0600DE32 RID: 56882 RVA: 0x004F4B40 File Offset: 0x004F2D40
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

		// Token: 0x0600DE33 RID: 56883 RVA: 0x004F4B91 File Offset: 0x004F2D91
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

		// Token: 0x0600DE34 RID: 56884 RVA: 0x004F4BD4 File Offset: 0x004F2DD4
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

		// Token: 0x0600DE35 RID: 56885 RVA: 0x004F4C44 File Offset: 0x004F2E44
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

		// Token: 0x0600DE36 RID: 56886 RVA: 0x004F4CCC File Offset: 0x004F2ECC
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

		// Token: 0x0600DE37 RID: 56887 RVA: 0x004F4D28 File Offset: 0x004F2F28
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

		// Token: 0x0400A79F RID: 42911
		[PublicizedFrom(EAccessModifier.Private)]
		public const float sessionUpdateIntervalSecsDefault = 30f;

		// Token: 0x0400A7A0 RID: 42912
		[PublicizedFrom(EAccessModifier.Private)]
		public const float sessionUpdateIntervalSecsImportant = 5f;

		// Token: 0x0400A7A1 RID: 42913
		[PublicizedFrom(EAccessModifier.Private)]
		public const string sessionName = "GameHost";

		// Token: 0x0400A7A2 RID: 42914
		public const string DefaultMatchmakingGroupTag = "<WeDontCare>";

		// Token: 0x0400A7A3 RID: 42915
		public const string EmptyStringAttributeValue = "##EMPTY##";

		// Token: 0x0400A7A4 RID: 42916
		public const string LowerCaseAttributeSeparator = "~$#$~";

		// Token: 0x0400A7A5 RID: 42917
		public const string BoolsAttributeName = "-BoolValues-";

		// Token: 0x0400A7A6 RID: 42918
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A7A7 RID: 42919
		[PublicizedFrom(EAccessModifier.Private)]
		public SessionsInterface sessionsInterface;

		// Token: 0x0400A7A8 RID: 42920
		[PublicizedFrom(EAccessModifier.Private)]
		public string sessionId;

		// Token: 0x0400A7A9 RID: 42921
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CountdownTimer commitBackendCountdown = new CountdownTimer(30f, false);

		// Token: 0x0400A7AA RID: 42922
		[PublicizedFrom(EAccessModifier.Private)]
		public SessionModification updatesSessionModification;

		// Token: 0x0400A7AB RID: 42923
		[PublicizedFrom(EAccessModifier.Private)]
		public Action onServerRegistered;

		// Token: 0x0400A7AC RID: 42924
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<string> registeredAttributes = new HashSet<string>();

		// Token: 0x02001D46 RID: 7494
		[PublicizedFrom(EAccessModifier.Private)]
		public class SessionModificationCallbackArgs
		{
			// Token: 0x0600DE3A RID: 56890 RVA: 0x004F4DDB File Offset: 0x004F2FDB
			public SessionModificationCallbackArgs(SessionModification _sessionModification, bool _isInitialRegistration, OnUpdateSessionCallback _callback)
			{
				this.SessionModification = _sessionModification;
				this.IsInitialRegistration = _isInitialRegistration;
				this.Callback = _callback;
			}

			// Token: 0x0400A7AD RID: 42925
			public SessionModification SessionModification;

			// Token: 0x0400A7AE RID: 42926
			public readonly bool IsInitialRegistration;

			// Token: 0x0400A7AF RID: 42927
			public readonly OnUpdateSessionCallback Callback;
		}
	}
}
