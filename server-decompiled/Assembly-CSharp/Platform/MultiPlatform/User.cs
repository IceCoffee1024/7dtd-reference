using System;
using System.Collections;
using System.Collections.Generic;

namespace Platform.MultiPlatform
{
	// Token: 0x02001D23 RID: 7459
	public class User : IUserClient
	{
		// Token: 0x0600DD19 RID: 56601 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x17001C39 RID: 7225
		// (get) Token: 0x0600DD1A RID: 56602 RVA: 0x004EB9CC File Offset: 0x004E9BCC
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
		// (add) Token: 0x0600DD1B RID: 56603 RVA: 0x004EBA00 File Offset: 0x004E9C00
		// (remove) Token: 0x0600DD1C RID: 56604 RVA: 0x004EBA68 File Offset: 0x004E9C68
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
		// (add) Token: 0x0600DD1D RID: 56605 RVA: 0x004EBAD0 File Offset: 0x004E9CD0
		// (remove) Token: 0x0600DD1E RID: 56606 RVA: 0x004EBB0C File Offset: 0x004E9D0C
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

		// Token: 0x17001C3A RID: 7226
		// (get) Token: 0x0600DD1F RID: 56607 RVA: 0x004D0E77 File Offset: 0x004CF077
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

		// Token: 0x0600DD20 RID: 56608 RVA: 0x004EBB48 File Offset: 0x004E9D48
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

		// Token: 0x0600DD21 RID: 56609 RVA: 0x004EBB80 File Offset: 0x004E9D80
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

		// Token: 0x0600DD22 RID: 56610 RVA: 0x004EBBB5 File Offset: 0x004E9DB5
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

		// Token: 0x0600DD23 RID: 56611 RVA: 0x004EBBE3 File Offset: 0x004E9DE3
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

		// Token: 0x0600DD24 RID: 56612 RVA: 0x000882A0 File Offset: 0x000864A0
		public void GetLoginTicket(Action<bool, byte[], string> _callback)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DD25 RID: 56613 RVA: 0x000882A0 File Offset: 0x000864A0
		public string GetFriendName(PlatformUserIdentifierAbs _playerId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600DD26 RID: 56614 RVA: 0x004EBC10 File Offset: 0x004E9E10
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

		// Token: 0x0600DD27 RID: 56615 RVA: 0x004EBC48 File Offset: 0x004E9E48
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

		// Token: 0x0600DD28 RID: 56616 RVA: 0x004EBCA4 File Offset: 0x004E9EA4
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

		// Token: 0x17001C3B RID: 7227
		// (get) Token: 0x0600DD29 RID: 56617 RVA: 0x004EBD24 File Offset: 0x004E9F24
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

		// Token: 0x0600DD2A RID: 56618 RVA: 0x004EBD94 File Offset: 0x004E9F94
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

		// Token: 0x0600DD2B RID: 56619 RVA: 0x004EBDE6 File Offset: 0x004E9FE6
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

		// Token: 0x0600DD2C RID: 56620 RVA: 0x004EBE0A File Offset: 0x004EA00A
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

		// Token: 0x0600DD2D RID: 56621 RVA: 0x004EBE20 File Offset: 0x004EA020
		public EMatchmakingGroup GetMatchmakingGroup()
		{
			IUserClient user = PlatformManager.NativePlatform.User;
			if (user == null)
			{
				return EMatchmakingGroup.Dev;
			}
			return user.GetMatchmakingGroup();
		}

		// Token: 0x0600DD2E RID: 56622 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}
	}
}
