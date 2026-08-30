using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform.MultiPlatform
{
	// Token: 0x02001CC0 RID: 7360
	public class User : IUserClient
	{
		// Token: 0x0600DA8B RID: 55947 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x17001B4B RID: 6987
		// (get) Token: 0x0600DA8C RID: 55948 RVA: 0x004E32E4 File Offset: 0x004E14E4
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

		// Token: 0x1400013A RID: 314
		// (add) Token: 0x0600DA8D RID: 55949 RVA: 0x004E3318 File Offset: 0x004E1518
		// (remove) Token: 0x0600DA8E RID: 55950 RVA: 0x004E3380 File Offset: 0x004E1580
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

		// Token: 0x1400013B RID: 315
		// (add) Token: 0x0600DA8F RID: 55951 RVA: 0x004E33E8 File Offset: 0x004E15E8
		// (remove) Token: 0x0600DA90 RID: 55952 RVA: 0x004E3424 File Offset: 0x004E1624
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

		// Token: 0x17001B4C RID: 6988
		// (get) Token: 0x0600DA91 RID: 55953 RVA: 0x004C8867 File Offset: 0x004C6A67
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

		// Token: 0x0600DA92 RID: 55954 RVA: 0x004E3460 File Offset: 0x004E1660
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

		// Token: 0x0600DA93 RID: 55955 RVA: 0x004E3498 File Offset: 0x004E1698
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

		// Token: 0x0600DA94 RID: 55956 RVA: 0x004E34CD File Offset: 0x004E16CD
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

		// Token: 0x0600DA95 RID: 55957 RVA: 0x004E34FB File Offset: 0x004E16FB
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

		// Token: 0x0600DA96 RID: 55958 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DA97 RID: 55959 RVA: 0x00087DB8 File Offset: 0x00085FB8
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DA98 RID: 55960 RVA: 0x004E3528 File Offset: 0x004E1728
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

		// Token: 0x0600DA99 RID: 55961 RVA: 0x004E3560 File Offset: 0x004E1760
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

		// Token: 0x0600DA9A RID: 55962 RVA: 0x004E35BC File Offset: 0x004E17BC
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

		// Token: 0x17001B4D RID: 6989
		// (get) Token: 0x0600DA9B RID: 55963 RVA: 0x004E363C File Offset: 0x004E183C
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

		// Token: 0x0600DA9C RID: 55964 RVA: 0x004E36AC File Offset: 0x004E18AC
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

		// Token: 0x0600DA9D RID: 55965 RVA: 0x004E36FE File Offset: 0x004E18FE
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

		// Token: 0x0600DA9E RID: 55966 RVA: 0x004E3722 File Offset: 0x004E1922
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

		// Token: 0x0600DA9F RID: 55967 RVA: 0x004E3738 File Offset: 0x004E1938
		public EMatchmakingGroup GetMatchmakingGroup()
		{
			IUserClient user = PlatformManager.NativePlatform.User;
			if (user == null)
			{
				return EMatchmakingGroup.Dev;
			}
			return user.GetMatchmakingGroup();
		}

		// Token: 0x0600DAA0 RID: 55968 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}
	}
}
