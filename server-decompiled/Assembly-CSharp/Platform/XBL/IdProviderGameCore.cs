using System;
using Platform.EOS;
using Platform.Shared;

namespace Platform.XBL
{
	// Token: 0x02001C28 RID: 7208
	public class IdProviderGameCore
	{
		// Token: 0x17001B0D RID: 6925
		// (get) Token: 0x0600D632 RID: 54834 RVA: 0x004D0553 File Offset: 0x004CE753
		// (set) Token: 0x0600D633 RID: 54835 RVA: 0x004D055B File Offset: 0x004CE75B
		public UserIdentifierXbl Id
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D634 RID: 54836 RVA: 0x004D0564 File Offset: 0x004CE764
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

		// Token: 0x0600D635 RID: 54837 RVA: 0x004D05A0 File Offset: 0x004CE7A0
		[PublicizedFrom(EAccessModifier.Private)]
		public void NativeLogin(IPlatform _platform)
		{
			this.Id = new UserIdentifierXbl(this.m_nativeUser.LocalID.Value.ToString());
			Log.Out("[XBL] Initializing user id with Local ID");
			XblXuidMapper.SetXuid(this.Id, this.m_nativeUser.Xuid);
		}

		// Token: 0x0600D636 RID: 54838 RVA: 0x004D05F0 File Offset: 0x004CE7F0
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

		// Token: 0x0600D637 RID: 54839 RVA: 0x004D0674 File Offset: 0x004CE874
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

		// Token: 0x0400A2AA RID: 41642
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly User m_nativeUser;
	}
}
