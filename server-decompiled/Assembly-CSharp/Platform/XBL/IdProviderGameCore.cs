using System;
using Platform.EOS;
using Platform.Shared;

namespace Platform.XBL
{
	// Token: 0x02001BF6 RID: 7158
	public class IdProviderGameCore
	{
		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x0600D4BA RID: 54458 RVA: 0x004CBA6B File Offset: 0x004C9C6B
		// (set) Token: 0x0600D4BB RID: 54459 RVA: 0x004CBA73 File Offset: 0x004C9C73
		public UserIdentifierXbl Id
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D4BC RID: 54460 RVA: 0x004CBA7C File Offset: 0x004C9C7C
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

		// Token: 0x0600D4BD RID: 54461 RVA: 0x004CBAB8 File Offset: 0x004C9CB8
		[PublicizedFrom(EAccessModifier.Private)]
		public void NativeLogin(IPlatform _platform)
		{
			this.Id = new UserIdentifierXbl(this.m_nativeUser.LocalID.Value.ToString());
			Log.Out("[XBL] Initializing user id with Local ID");
			XblXuidMapper.SetXuid(this.Id, this.m_nativeUser.Xuid);
		}

		// Token: 0x0600D4BE RID: 54462 RVA: 0x004CBB08 File Offset: 0x004C9D08
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

		// Token: 0x0600D4BF RID: 54463 RVA: 0x004CBB8C File Offset: 0x004C9D8C
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

		// Token: 0x0400A1D5 RID: 41429
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly User m_nativeUser;
	}
}
