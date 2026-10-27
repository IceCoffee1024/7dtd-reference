using System;
using Platform.EOS;
using Platform.Shared;

namespace Platform.XBL
{
	// Token: 0x02001C59 RID: 7257
	public class IdProviderGameCore
	{
		// Token: 0x17001B55 RID: 6997
		// (get) Token: 0x0600D746 RID: 55110 RVA: 0x004D407B File Offset: 0x004D227B
		// (set) Token: 0x0600D747 RID: 55111 RVA: 0x004D4083 File Offset: 0x004D2283
		public UserIdentifierXbl Id
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D748 RID: 55112 RVA: 0x004D408C File Offset: 0x004D228C
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

		// Token: 0x0600D749 RID: 55113 RVA: 0x004D40C8 File Offset: 0x004D22C8
		[PublicizedFrom(EAccessModifier.Private)]
		public void NativeLogin(IPlatform _platform)
		{
			this.Id = new UserIdentifierXbl(this.m_nativeUser.LocalID.Value.ToString());
			Log.Out("[XBL] Initializing user id with Local ID");
			XblXuidMapper.SetXuid(this.Id, this.m_nativeUser.Xuid);
		}

		// Token: 0x0600D74A RID: 55114 RVA: 0x004D4118 File Offset: 0x004D2318
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

		// Token: 0x0600D74B RID: 55115 RVA: 0x004D419C File Offset: 0x004D239C
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

		// Token: 0x0400A393 RID: 41875
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly User m_nativeUser;
	}
}
