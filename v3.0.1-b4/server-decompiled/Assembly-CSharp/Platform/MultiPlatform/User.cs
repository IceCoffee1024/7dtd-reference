using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform.MultiPlatform
{
	// Token: 0x02001C99 RID: 7321
	public class User : IUserClient
	{
		// Token: 0x0600D8F3 RID: 55539 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x17001AD9 RID: 6873
		// (get) Token: 0x0600D8F4 RID: 55540 RVA: 0x004DCCE8 File Offset: 0x004DAEE8
		public EUserStatus UserStatus
		{
			get
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				if (((crossplatformPlatform != null) ? crossplatformPlatform.User : null) == null)
				{
					return PlatformManager.NativePlatform.User.UserStatus;
				}
				return PlatformManager.CrossplatformPlatform.User.UserStatus;
			}
		}

		// Token: 0x14000139 RID: 313
		// (add) Token: 0x0600D8F5 RID: 55541 RVA: 0x004DCD1C File Offset: 0x004DAF1C
		// (remove) Token: 0x0600D8F6 RID: 55542 RVA: 0x004DCD84 File Offset: 0x004DAF84
		public event Action<IPlatform> UserLoggedIn
		{
			add
			{
				lock (this)
				{
					PlatformManager.NativePlatform.User.UserLoggedIn += value;
					IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
					if (((crossplatformPlatform != null) ? crossplatformPlatform.User : null) != null)
					{
						PlatformManager.CrossplatformPlatform.User.UserLoggedIn += value;
					}
				}
			}
			remove
			{
				lock (this)
				{
					PlatformManager.NativePlatform.User.UserLoggedIn -= value;
					IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
					if (((crossplatformPlatform != null) ? crossplatformPlatform.User : null) != null)
					{
						PlatformManager.CrossplatformPlatform.User.UserLoggedIn -= value;
					}
				}
			}
		}

		// Token: 0x1400013A RID: 314
		// (add) Token: 0x0600D8F7 RID: 55543 RVA: 0x004DCDEC File Offset: 0x004DAFEC
		// (remove) Token: 0x0600D8F8 RID: 55544 RVA: 0x004DCE28 File Offset: 0x004DB028
		public event UserBlocksChangedCallback UserBlocksChanged
		{
			add
			{
				PlatformManager.NativePlatform.User.UserBlocksChanged += value;
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				IUserClient userClient = ((crossplatformPlatform != null) ? crossplatformPlatform.User : null);
				if (userClient != null)
				{
					userClient.UserBlocksChanged += value;
				}
			}
			remove
			{
				PlatformManager.NativePlatform.User.UserBlocksChanged -= value;
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				IUserClient userClient = ((crossplatformPlatform != null) ? crossplatformPlatform.User : null);
				if (userClient != null)
				{
					userClient.UserBlocksChanged -= value;
				}
			}
		}

		// Token: 0x17001ADA RID: 6874
		// (get) Token: 0x0600D8F9 RID: 55545 RVA: 0x004C2473 File Offset: 0x004C0673
		public PlatformUserIdentifierAbs PlatformUserId
		{
			get
			{
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
				return platformUserIdentifierAbs ?? PlatformManager.NativePlatform.User.PlatformUserId;
			}
		}

		// Token: 0x0600D8FA RID: 55546 RVA: 0x004DCE64 File Offset: 0x004DB064
		public void Login(LoginUserCallback _delegate)
		{
			PlatformManager.NativePlatform.User.Login(delegate(IPlatform _nativePlatform, EApiStatusReason _nativeReason, string _statusReasonAdditionalText)
			{
				if (_nativePlatform.Api.ClientApiStatus != EApiStatus.Ok || _nativePlatform.User.UserStatus != EUserStatus.LoggedIn)
				{
					_delegate(_nativePlatform, _nativeReason, _statusReasonAdditionalText);
					return;
				}
				if (_nativeReason != EApiStatusReason.Ok)
				{
					_delegate(_nativePlatform, _nativeReason, _statusReasonAdditionalText);
					return;
				}
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				if (((crossplatformPlatform != null) ? crossplatformPlatform.User : null) == null)
				{
					_delegate(_nativePlatform, _nativeReason, _statusReasonAdditionalText);
					return;
				}
				PlatformManager.CrossplatformPlatform.User.Login(_delegate);
			});
		}

		// Token: 0x0600D8FB RID: 55547 RVA: 0x004DCE9C File Offset: 0x004DB09C
		public void PlayOffline(LoginUserCallback _delegate)
		{
			PlatformManager.NativePlatform.User.PlayOffline(delegate(IPlatform _nativePlatform, EApiStatusReason _nativeReason, string _statusReasonAdditionalText)
			{
				if (_nativePlatform.Api.ClientApiStatus != EApiStatus.Ok || _nativePlatform.User.UserStatus != EUserStatus.OfflineMode)
				{
					_delegate(_nativePlatform, _nativeReason, _statusReasonAdditionalText);
					return;
				}
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				if (((crossplatformPlatform != null) ? crossplatformPlatform.User : null) == null)
				{
					_delegate(_nativePlatform, _nativeReason, _statusReasonAdditionalText);
					return;
				}
				PlatformManager.CrossplatformPlatform.User.PlayOffline(_delegate);
			});
		}

		// Token: 0x0600D8FC RID: 55548 RVA: 0x004DCED1 File Offset: 0x004DB0D1
		public void StartAdvertisePlaying(GameServerInfo _serverInfo)
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform != null)
			{
				IUserClient user = crossplatformPlatform.User;
				if (user != null)
				{
					user.StartAdvertisePlaying(_serverInfo);
				}
			}
			PlatformManager.NativePlatform.User.StartAdvertisePlaying(_serverInfo);
		}

		// Token: 0x0600D8FD RID: 55549 RVA: 0x004DCEFF File Offset: 0x004DB0FF
		public void StopAdvertisePlaying()
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			if (crossplatformPlatform != null)
			{
				IUserClient user = crossplatformPlatform.User;
				if (user != null)
				{
					user.StopAdvertisePlaying();
				}
			}
			PlatformManager.NativePlatform.User.StopAdvertisePlaying();
		}

		// Token: 0x0600D8FE RID: 55550 RVA: 0x000876FD File Offset: 0x000858FD
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D8FF RID: 55551 RVA: 0x000876FD File Offset: 0x000858FD
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600D900 RID: 55552 RVA: 0x004DCF2C File Offset: 0x004DB12C
		public bool IsFriend(PlatformUserIdentifierAbs _playerId)
		{
			if (_playerId == null)
			{
				return false;
			}
			IPlatform platform = PlatformManager.InstanceForPlatformIdentifier(_playerId.PlatformIdentifier);
			if (platform == null)
			{
				return false;
			}
			IUserClient user = platform.User;
			return user != null && user.IsFriend(_playerId);
		}

		// Token: 0x0600D901 RID: 55553 RVA: 0x004DCF64 File Offset: 0x004DB164
		public bool CanShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			if (!PlatformManager.NativePlatform.User.CanShowProfile(_playerId))
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				bool? flag;
				if (crossplatformPlatform == null)
				{
					flag = null;
				}
				else
				{
					IUserClient user = crossplatformPlatform.User;
					flag = ((user != null) ? new bool?(user.CanShowProfile(_playerId)) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
			return true;
		}

		// Token: 0x0600D902 RID: 55554 RVA: 0x004DCFC0 File Offset: 0x004DB1C0
		public void ShowProfile(PlatformUserIdentifierAbs _playerId)
		{
			if (PlatformManager.NativePlatform.User.CanShowProfile(_playerId))
			{
				PlatformManager.NativePlatform.User.ShowProfile(_playerId);
				return;
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			bool? flag;
			if (crossplatformPlatform == null)
			{
				flag = null;
			}
			else
			{
				IUserClient user = crossplatformPlatform.User;
				flag = ((user != null) ? new bool?(user.CanShowProfile(_playerId)) : null);
			}
			bool? flag2 = flag;
			if (flag2.GetValueOrDefault())
			{
				PlatformManager.CrossplatformPlatform.User.ShowProfile(_playerId);
			}
		}

		// Token: 0x17001ADB RID: 6875
		// (get) Token: 0x0600D903 RID: 55555 RVA: 0x004DD040 File Offset: 0x004DB240
		public EUserPerms Permissions
		{
			get
			{
				if (GameManager.IsDedicatedServer)
				{
					return EUserPerms.All;
				}
				EUserPerms permissions = PlatformManager.NativePlatform.User.Permissions;
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				EUserPerms? euserPerms;
				if (crossplatformPlatform == null)
				{
					euserPerms = null;
				}
				else
				{
					IUserClient user = crossplatformPlatform.User;
					euserPerms = ((user != null) ? new EUserPerms?(user.Permissions) : null);
				}
				return permissions & (euserPerms ?? EUserPerms.All);
			}
		}

		// Token: 0x0600D904 RID: 55556 RVA: 0x004DD0B0 File Offset: 0x004DB2B0
		public string GetPermissionDenyReason(EUserPerms _perms)
		{
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			string text;
			if (crossplatformPlatform == null)
			{
				text = null;
			}
			else
			{
				IUserClient user = crossplatformPlatform.User;
				text = ((user != null) ? user.GetPermissionDenyReason(_perms) : null);
			}
			string text2 = text;
			if (!string.IsNullOrEmpty(text2))
			{
				return text2;
			}
			string permissionDenyReason = PlatformManager.NativePlatform.User.GetPermissionDenyReason(_perms);
			if (!string.IsNullOrEmpty(permissionDenyReason))
			{
				return permissionDenyReason;
			}
			return null;
		}

		// Token: 0x0600D905 RID: 55557 RVA: 0x004DD102 File Offset: 0x004DB302
		public IEnumerator ResolvePermissions(EUserPerms _perms, bool _canPrompt, CoroutineCancellationToken _cancellationToken = null)
		{
			if (_canPrompt && this.UserStatus != EUserStatus.LoggedIn)
			{
				Log.Out("[MultiPlatform] ResolvePermissions: Attempting Login as we're allowed to prompt.");
				bool loginAttemptDone = false;
				this.Login(delegate(IPlatform platform, EApiStatusReason reason, string text)
				{
					CoroutineCancellationToken cancellationToken2 = _cancellationToken;
					if (cancellationToken2 != null && cancellationToken2.IsCancelled())
					{
						return;
					}
					loginAttemptDone = true;
					EUserStatus userStatus = this.UserStatus;
					((userStatus == EUserStatus.LoggedIn) ? new Action<string>(Log.Out) : new Action<string>(Log.Warning))(string.Format("[MultiPlatform] {0}: Login Attempt Completed. Status: {1}, Platform: {2}, Reason: {3}, Additional Reason: '{4}'.", new object[] { "ResolvePermissions", userStatus, platform, reason, text }));
				});
				while (!loginAttemptDone)
				{
					yield return null;
					CoroutineCancellationToken cancellationToken = _cancellationToken;
					if (cancellationToken != null && cancellationToken.IsCancelled())
					{
						yield break;
					}
				}
			}
			yield return PlatformManager.NativePlatform.User.ResolvePermissions(_perms, _canPrompt, _cancellationToken);
			_perms &= PlatformManager.NativePlatform.User.Permissions;
			if (_perms == (EUserPerms)0)
			{
				yield break;
			}
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			object obj;
			if (crossplatformPlatform == null)
			{
				obj = null;
			}
			else
			{
				IUserClient user = crossplatformPlatform.User;
				obj = ((user != null) ? user.ResolvePermissions(_perms, _canPrompt, _cancellationToken) : null);
			}
			yield return obj;
			yield break;
		}

		// Token: 0x0600D906 RID: 55558 RVA: 0x004DD126 File Offset: 0x004DB326
		public IEnumerator ResolveUserBlocks(IReadOnlyList<IPlatformUserBlockedResults> _results)
		{
			if (GameManager.IsDedicatedServer)
			{
				yield break;
			}
			if (!this.Permissions.HasCommunication())
			{
				PlatformUserIdentifierAbs platformUserId = this.PlatformUserId;
				foreach (IPlatformUserBlockedResults platformUserBlockedResults in _results)
				{
					if (!object.Equals(platformUserId, platformUserBlockedResults.User.PrimaryId))
					{
						platformUserBlockedResults.Block(EBlockType.TextChat);
						platformUserBlockedResults.Block(EBlockType.VoiceChat);
					}
				}
			}
			yield return PlatformManager.NativePlatform.User.ResolveUserBlocks(_results);
			IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
			object obj;
			if (crossplatformPlatform == null)
			{
				obj = null;
			}
			else
			{
				IUserClient user = crossplatformPlatform.User;
				obj = ((user != null) ? user.ResolveUserBlocks(_results) : null);
			}
			yield return obj;
			yield break;
		}

		// Token: 0x0600D907 RID: 55559 RVA: 0x004DD13C File Offset: 0x004DB33C
		public EMatchmakingGroup GetMatchmakingGroup()
		{
			IUserClient user = PlatformManager.NativePlatform.User;
			if (user == null)
			{
				return EMatchmakingGroup.Dev;
			}
			return user.GetMatchmakingGroup();
		}

		// Token: 0x0600D908 RID: 55560 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}
	}
}
