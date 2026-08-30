using System;
using Platform.EOS;
using Platform.Shared;

namespace Platform.XBL
{
	// Token: 0x02001BCF RID: 7119
	public class IdProviderGameCore
	{
		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x0600D32C RID: 54060 RVA: 0x004C5677 File Offset: 0x004C3877
		// (set) Token: 0x0600D32D RID: 54061 RVA: 0x004C567F File Offset: 0x004C387F
		public UserIdentifierXbl Id
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D32E RID: 54062 RVA: 0x004C5688 File Offset: 0x004C3888
		public IdProviderGameCore(User _nativeUser, IUserClient _crossplatformUser)
		{
			this.m_nativeUser = _nativeUser;
			if (_crossplatformUser != null)
			{
				_crossplatformUser.UserLoggedIn += this.CrossplatformLogin;
				return;
			}
			this.m_nativeUser.UserLoggedIn += this.NativeLogin;
		}

		// Token: 0x0600D32F RID: 54063 RVA: 0x004C56C4 File Offset: 0x004C38C4
		[PublicizedFrom(EAccessModifier.Private)]
		public void NativeLogin(IPlatform _platform)
		{
			this.Id = new UserIdentifierXbl(this.m_nativeUser.LocalID.Value.ToString());
			Log.Out("[XBL] Initializing user id with Local ID");
			XblXuidMapper.SetXuid(this.Id, this.m_nativeUser.Xuid);
		}

		// Token: 0x0600D330 RID: 54064 RVA: 0x004C5714 File Offset: 0x004C3914
		[PublicizedFrom(EAccessModifier.Private)]
		public void CrossplatformLogin(IPlatform _platform)
		{
			PlatformUserIdentifierAbs nativePlatformUserId = ((User)_platform.User).NativePlatformUserId;
			if (nativePlatformUserId != null)
			{
				UserIdentifierXbl userIdentifierXbl = nativePlatformUserId as UserIdentifierXbl;
				if (userIdentifierXbl == null)
				{
					Log.Error(string.Format("[XBL] Got different native platform id from EOS: {0}", nativePlatformUserId.PlatformIdentifier));
					return;
				}
				this.Id = userIdentifierXbl;
				Log.Out("[XBL] Initializing user id with PXUID " + userIdentifierXbl.CombinedString);
				PlatformIdCache.SetCachedId(this.Id);
				XblXuidMapper.SetXuid(this.Id, this.m_nativeUser.Xuid);
			}
		}

		// Token: 0x0600D331 RID: 54065 RVA: 0x004C5798 File Offset: 0x004C3998
		public bool LoadOfflineId()
		{
			UserIdentifierXbl userIdentifierXbl;
			if (PlatformIdCache.TryGetCachedId<UserIdentifierXbl>(out userIdentifierXbl))
			{
				this.Id = userIdentifierXbl;
				Log.Out("[XBL] Retrieved offline user id: " + userIdentifierXbl.CombinedString);
				return true;
			}
			return false;
		}

		// Token: 0x0400A0A4 RID: 41124
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly User m_nativeUser;
	}
}
